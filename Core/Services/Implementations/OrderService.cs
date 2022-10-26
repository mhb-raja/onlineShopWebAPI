using Core.DTOs.Order;
using Core.Services.Interfaces;
using DataLayer.Entities.Order;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Implementations
{
    public class OrderService : IOrderService
    {
        #region constructor

        private readonly IGenericRepository<Order> orderRepository;
        private readonly IGenericRepository<OrderDetail> orderDetailRepository;
        private readonly IUserService userService;
        private readonly IProductService productService;


        public OrderService(IGenericRepository<Order> orderRepository, IGenericRepository<OrderDetail> orderDetailRepository,
            IUserService userService, IProductService productService)
        {
            this.orderRepository = orderRepository;
            this.orderDetailRepository = orderDetailRepository;
            this.userService = userService;
            this.productService = productService;
        }


        #endregion

        #region order

        public async Task<Order> CreateUserOrder(long userId)
        {
            var order = new Order
            {
                UserId = userId
            };
            await orderRepository.AddEntity(order);
            await orderRepository.SaveChanges();

            return order;
        }

        public async Task<Order> GetUserOpenOrder(long userId)
        {
            var order = await orderRepository.GetEntitiesQuery()
                .Include(s => s.OrderDetails).ThenInclude(s => s.Product)
                .SingleOrDefaultAsync(s => s.UserId == userId && !s.IsPaid && !s.IsDelete);
            if (order == null)
                order = await CreateUserOrder(userId);

            return order;
        }

        #endregion


        #region order detail

        public async Task AddProductToOrder(long userId, long productId, int count)
        {
            var user = await userService.GetUserById(userId);
            var product = await productService.GetProductForUserOrder(productId);
            if (user != null && product != null)
            {
                var order = await GetUserOpenOrder(userId);

                if (count < 1) count = 1;
                var details = await GetOrderDetails(order.Id);
                var existDetail = details.SingleOrDefault(s => s.ProductId == productId && !s.IsDelete);
                if (existDetail != null)
                {
                    existDetail.Count += count;
                    orderDetailRepository.UpdateEntity(existDetail);
                }
                else
                {
                    var detail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = productId,
                        Count = count,
                        Price = product.Price
                    };
                    await orderDetailRepository.AddEntity(detail);
                }
                await orderDetailRepository.SaveChanges();
            }
        }

        public async Task<List<OrderDetail>> GetOrderDetails(long orderId)
        {
            return await orderDetailRepository.GetEntitiesQuery().Where(s => s.OrderId == orderId).ToListAsync();
        }

        public async Task<List<ShoppingCartItemDTO>> GetUserShoppingCart(long userId)
        {
            var openOrder = await GetUserOpenOrder(userId);
            if (openOrder == null) return null;
            return openOrder.OrderDetails.Where(s => !s.IsDelete)
                .Select(f => new ShoppingCartItemDTO
                {
                    Id = f.Id,
                    ProductId = f.ProductId,
                    Count = f.Count,
                    Price = f.Price,
                    Title = f.Product.Name,
                    ImageName = f.Product.ImageName
                }).ToList();
        }

        public async Task DeleteOrderDetail(OrderDetail detail)
        {
            orderDetailRepository.RemoveEntity(detail);
            await orderDetailRepository.SaveChanges();
        }


        #endregion

        #region Dispose
        public void Dispose()
        {
            this.orderDetailRepository.Dispose();
            this.orderRepository.Dispose();
        }
        #endregion
    }
}
