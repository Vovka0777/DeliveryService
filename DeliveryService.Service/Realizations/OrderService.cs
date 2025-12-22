using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DeliveryService.DAL; // Убрали .Interfaces, так как контекст обычно тут
using DeliveryService.Domain.Models;
using DeliveryService.Domain.Response;
using DeliveryService.Domain.Enum; // Добавили для StatusOrder и StatusCode
using DeliveryService.Domain.ViewModels.Order;
using DeliveryService.Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DeliveryService.Service.Realizations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IBaseResponse<CartViewModel>> GetCart(string userName)
        {
            try
            {
                // ИСПРАВЛЕНО: Убрали "|| x.Name == userName", оставили только Login
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Login == userName);

                if (user == null)
                    return new BaseResponse<CartViewModel> { Description = "Пользователь не найден", StatusCode = StatusCode.UserNotFound };

                var order = await _context.Orders
                    .Include(x => x.Items)
                    .ThenInclude(x => x.Item)
                    .FirstOrDefaultAsync(x => x.UserId == user.Id && x.Status == StatusOrder.Cart);

                var model = new CartViewModel();
                if (order != null)
                {
                    model.OrderId = order.Id;
                    model.Items = order.Items.Select(x => new OrderItemViewModel
                    {
                        ItemId = x.ItemId,
                        ItemName = x.Item.Name,
                        ImagePath = x.Item.PathImg,
                        Price = x.Item.Price,
                        Quantity = x.Quantity
                    }).ToList();
                }

                return new BaseResponse<CartViewModel> { Data = model, StatusCode = StatusCode.OK };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CartViewModel> { Description = ex.Message, StatusCode = StatusCode.InternalServerError };
            }
        }

        public async Task<IBaseResponse<bool>> AddToCart(Guid itemId, string userName)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Login == userName || x.Login == userName);
                if (user == null)
                    return new BaseResponse<bool> { Description = "Пользователь не найден", StatusCode = StatusCode.UserNotFound };

                var order = await _context.Orders
                    .Include(x => x.Items)
                    .FirstOrDefaultAsync(x => x.UserId == user.Id && x.Status == StatusOrder.Cart);

                if (order == null)
                {
                    order = new Order
                    {
                        UserId = user.Id,
                        DateCreated = DateTime.Now,
                        Status = StatusOrder.Cart
                    };
                    await _context.Orders.AddAsync(order);
                    await _context.SaveChangesAsync();
                }

                var item = await _context.Items.FindAsync(itemId);
                if (item == null)
                    return new BaseResponse<bool> { Description = "Товар не найден", StatusCode = StatusCode.InternalServerError };

                var orderItem = order.Items.FirstOrDefault(x => x.ItemId == itemId);
                if (orderItem == null)
                {
                    order.Items.Add(new OrderItem
                    {
                        OrderId = order.Id,
                        ItemId = itemId,
                        Price = item.Price,
                        Quantity = 1
                    });
                }
                else
                {
                    orderItem.Quantity++;
                }

                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                return new BaseResponse<bool> { Data = true, StatusCode = StatusCode.OK };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool> { Description = ex.Message, StatusCode = StatusCode.InternalServerError };
            }
        }

        public async Task<IBaseResponse<bool>> RemoveFromCart(Guid itemId, string userName)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Login == userName || x.Login == userName);
                if (user == null) return new BaseResponse<bool> { Description = "User not found", StatusCode = StatusCode.UserNotFound };

                var order = await _context.Orders.Include(x => x.Items)
                    .FirstOrDefaultAsync(x => x.UserId == user.Id && x.Status == StatusOrder.Cart);

                if (order == null) return new BaseResponse<bool> { Data = false, StatusCode = StatusCode.OK };

                var orderItem = order.Items.FirstOrDefault(x => x.ItemId == itemId);
                if (orderItem != null)
                {
                    if (orderItem.Quantity > 1)
                        orderItem.Quantity--;
                    else
                        order.Items.Remove(orderItem);

                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();
                }

                return new BaseResponse<bool> { Data = true, StatusCode = StatusCode.OK };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool> { Description = ex.Message, StatusCode = StatusCode.InternalServerError };
            }
        }

        public async Task<IBaseResponse<bool>> ConfirmOrder(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Login == userName || x.Login == userName);
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.UserId == user.Id && x.Status == StatusOrder.Cart);

            if (order != null)
            {
                order.Status = StatusOrder.Created; // Теперь StatusOrder виден
                order.DateCreated = DateTime.Now;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                return new BaseResponse<bool> { Data = true, StatusCode = StatusCode.OK };
            }
            return new BaseResponse<bool> { Description = "Корзина пуста", StatusCode = StatusCode.InternalServerError };
        }
    }
}