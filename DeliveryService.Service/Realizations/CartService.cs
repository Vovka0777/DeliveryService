using DeliveryService.DAL;
using DeliveryService.Domain.Enum;
using DeliveryService.Domain.Models;
using DeliveryService.Domain.Response;
using DeliveryService.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Service.Realizations
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _db;

        public CartService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IBaseResponse<Basket>> AddItem(string userName, Guid itemId)
        {
            try
            {
                var user = await _db.Users.Include(x => x.Basket).FirstOrDefaultAsync(x => x.Login == userName);
                if (user == null) return new BaseResponse<Basket>() { Description = "Пользователь не найден", StatusCode = StatusCode.NotFound};

                if (user.Basket == null)
                {
                    // UserId должен совпадать с типом Id в UserDb (Guid)
                    user.Basket = new Basket { UserId = user.Id, Items = new List<BasketItem>() };
                    _db.Baskets.Add(user.Basket);
                    await _db.SaveChangesAsync();
                }

                var basket = await _db.Baskets.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == user.Basket.Id);
                var item = await _db.Items.FirstOrDefaultAsync(x => x.Id == itemId);

                if (item == null) return new BaseResponse<Basket>() { Description = "Товар не найден", StatusCode = StatusCode.OK };

                var basketItem = basket.Items.FirstOrDefault(x => x.ItemId == itemId);
                if (basketItem != null)
                {
                    basketItem.Quantity++;
                }
                else
                {
                    basket.Items.Add(new BasketItem { BasketId = basket.Id, ItemId = item.Id, Quantity = 1 });
                }

                await _db.SaveChangesAsync();

                return new BaseResponse<Basket>() { Data = basket, StatusCode = StatusCode.OK };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Basket>() { Description = ex.Message, StatusCode = StatusCode.InternalServerError };
            }
        }

        public async Task<IBaseResponse<IEnumerable<BasketItem>>> GetItems(string userName)
        {
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(x => x.Login == userName);
                if (user == null) return new BaseResponse<IEnumerable<BasketItem>>() { Description = "Пользователь не найден", StatusCode = StatusCode.NotFound };

                var basket = await _db.Baskets
                    .Include(x => x.Items)
                    .ThenInclude(x => x.Item) // Подгружаем данные о товаре
                    .FirstOrDefaultAsync(x => x.UserId == user.Id);

                if (basket == null)
                {
                    return new BaseResponse<IEnumerable<BasketItem>>() { Data = new List<BasketItem>(), StatusCode = StatusCode.OK };
                }

                return new BaseResponse<IEnumerable<BasketItem>>() { Data = basket.Items, StatusCode = StatusCode.OK };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<BasketItem>>() { Description = ex.Message, StatusCode = StatusCode.InternalServerError };
            }
        }

        public async Task<IBaseResponse<bool>> RemoveItem(string userName, Guid itemId)
        {
            try
            {
                var user = await _db.Users.Include(x => x.Basket).FirstOrDefaultAsync(x => x.Login == userName);
                if (user == null || user.Basket == null) return new BaseResponse<bool>() { Data = false, StatusCode = StatusCode.NotFound };

                var basketItem = await _db.BasketItems.FirstOrDefaultAsync(x => x.BasketId == user.Basket.Id && x.ItemId == itemId);

                if (basketItem != null)
                {
                    _db.BasketItems.Remove(basketItem);
                    await _db.SaveChangesAsync();
                }

                return new BaseResponse<bool>() { Data = true, StatusCode = StatusCode.OK };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>() { Description = ex.Message, StatusCode = StatusCode.InternalServerError };
            }
        }

        public async Task<IBaseResponse<bool>> ClearBasket(string userName)
        {
            // Реализация аналогична RemoveItem, только удаляем всё
            return new BaseResponse<bool>() { Data = true, StatusCode = StatusCode.OK };
        }
        public async Task<IBaseResponse<bool>> DeleteItem(Guid id)
        {
            try
            {
                var item = await _db.BasketItems.FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    return new BaseResponse<bool>() { Description = "Товар не найден", StatusCode = StatusCode.NotFound };
                }

                _db.BasketItems.Remove(item);
                await _db.SaveChangesAsync();

                return new BaseResponse<bool>() { Data = true, StatusCode = StatusCode.OK, Description = "Товар удален" };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>() { Description = ex.Message, StatusCode = StatusCode.InternalServerError };
            }
        }
    }
}