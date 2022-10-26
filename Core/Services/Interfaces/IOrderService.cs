using Core.DTOs.Order;
using DataLayer.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IOrderService : IDisposable
    {
        #region order

        Task<Order> CreateUserOrder(long userId);
        Task<Order> GetUserOpenOrder(long userId);
        #endregion

        #region order detail
        Task AddProductToOrder(long userId, long productId, int count);
        Task<List<OrderDetail>> GetOrderDetails(long orderId);
        Task<List<ShoppingCartItemDTO>> GetUserShoppingCart(long userId);
        Task DeleteOrderDetail(OrderDetail detail);
        #endregion
    }
}
