using DeliveryService.DAL;
using DeliveryService.Domain.Enum;
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
                    // Если картинки нет, ставим заглушку
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

                // 1. Обновляем текстовые данные
                user.Email = model.Email;
                user.Phone = model.Phone;
                user.Address = model.Address;

                // 2. Логика сохранения картинки (С ЗАЩИТОЙ ОТ ВЫЛЕТА)
                if (model.AvatarFile != null)
                {
                    string wwwRootPath = _appEnvironment.WebRootPath;

                    string folderName = "ImageUser";
                    string pathFolder = Path.Combine(wwwRootPath, folderName);


                    // ВАЖНО: Если папки нет, создаем её
                    if (!Directory.Exists(pathFolder))
                    {
                        Directory.CreateDirectory(pathFolder);
                    }

                    string fileName = $"{user.Login}_{Guid.NewGuid().ToString().Substring(0, 8)}{Path.GetExtension(model.AvatarFile.FileName)}";
                    string fullPath = Path.Combine(pathFolder, fileName);

                    // Сохраняем файл
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await model.AvatarFile.CopyToAsync(fileStream);
                    }

                    // Сохраняем путь для базы данных (путь относительно корня сайта)
                    user.PathImage = $"/{folderName}/{fileName}";
                }

                _db.Users.Update(user);
                await _db.SaveChangesAsync();
                model.AvatarPath = user.PathImage;

                return new BaseResponse<ProfileViewModel>() { Data = model, StatusCode = StatusCode.OK, Description = "Профиль успешно обновлен" };
            }
            catch (Exception ex)
            {
                // Логируем ошибку, чтобы сайт не падал
                return new BaseResponse<ProfileViewModel>() { Description = $"[UpdateProfile] : {ex.Message}", StatusCode = StatusCode.InternalServerError };
            }
        }
    }
}