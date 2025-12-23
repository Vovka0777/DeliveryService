using DeliveryService.DAL;
using DeliveryService.Domain.Enum;
using DeliveryService.Domain.Helpers;
using DeliveryService.Domain.Response;
using DeliveryService.Domain.ViewModels.Profile;
using DeliveryService.Service.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Service.Realizations
{
    public class ProfileService : IProfileService
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _appEnvironment;

        public ProfileService(ApplicationDbContext db, IWebHostEnvironment appEnvironment)
        {
            _db = db;
            _appEnvironment = appEnvironment;
        }

        public async Task<IBaseResponse<ProfileViewModel>> GetProfile(string userName)
        {
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(x => x.Login == userName);
                if (user == null)
                    return new BaseResponse<ProfileViewModel>() { Description = "Пользователь не найден", StatusCode = StatusCode.NotFound };

                var profile = new ProfileViewModel()
                {
                    Id = user.Id,
                    Login = user.Login,
                    Email = user.Email,
                    Phone = user.Phone,
                    Address = user.Address,
                    AvatarPath = string.IsNullOrEmpty(user.PathImage) ? "/images/user.png" : user.PathImage
                };

                return new BaseResponse<ProfileViewModel>() { Data = profile, StatusCode = StatusCode.OK };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ProfileViewModel>() { Description = $"[GetProfile] : {ex.Message}", StatusCode = StatusCode.InternalServerError };
            }
        }

        public async Task<IBaseResponse<ProfileViewModel>> UpdateProfile(ProfileViewModel model)
        {
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (user == null)
                    return new BaseResponse<ProfileViewModel>() { Description = "Пользователь не найден", StatusCode = StatusCode.NotFound };

                if (user.Login != model.Login)
                {
                    if (await _db.Users.AnyAsync(x => x.Login == model.Login))
                    {
                        return new BaseResponse<ProfileViewModel>() { Description = "Этот логин уже занят", StatusCode = StatusCode.InternalServerError };
                    }
                    user.Login = model.Login;
                }

                if (!string.IsNullOrWhiteSpace(model.NewPassword))
                {
                    if (string.IsNullOrWhiteSpace(model.CurrentPassword))
                    {
                        return new BaseResponse<ProfileViewModel>() { Description = "Введите текущий пароль для подтверждения смены", StatusCode = StatusCode.InternalServerError };
                    }

                    // Проверяем старый пароль
                    if (user.Password != HashPasswordHelper.HashPassword(model.CurrentPassword))
                    {
                        return new BaseResponse<ProfileViewModel>() { Description = "Неверный текущий пароль", StatusCode = StatusCode.InternalServerError };
                    }

                    // Ставим новый пароль
                    user.Password = HashPasswordHelper.HashPassword(model.NewPassword);
                }

                user.Email = model.Email;
                user.Phone = model.Phone;
                user.Address = model.Address;

                if (model.AvatarFile != null)
                {
                    string wwwRootPath = _appEnvironment.WebRootPath;
                    string folderName = "ImageUser";
                    string pathFolder = Path.Combine(wwwRootPath, folderName);

                    if (!Directory.Exists(pathFolder))
                    {
                        Directory.CreateDirectory(pathFolder);
                    }

                    string extension = Path.GetExtension(model.AvatarFile.FileName);
                    string fileName = $"{user.Login}_{Guid.NewGuid().ToString().Substring(0, 8)}{extension}";

                    string fullPath = Path.Combine(pathFolder, fileName);

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await model.AvatarFile.CopyToAsync(fileStream);
                    }

                    user.PathImage = $"/{folderName}/{fileName}";
                }

                _db.Users.Update(user);
                await _db.SaveChangesAsync();

                model.AvatarPath = user.PathImage;

                return new BaseResponse<ProfileViewModel>() { Data = model, StatusCode = StatusCode.OK, Description = "Профиль успешно обновлен" };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ProfileViewModel>() { Description = $"[UpdateProfile] : {ex.Message}", StatusCode = StatusCode.InternalServerError };
            }
        }
    }
}