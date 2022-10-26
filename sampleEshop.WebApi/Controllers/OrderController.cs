using Core.Services.Interfaces;
using Core.Utilities.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sampleEshop.WebApi.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sampleEshop.WebApi.Controllers
{
    public class OrderController : SiteBaseController
    {
        #region constructor

        private readonly IOrderService orderService;
        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        #endregion

        #region add product to order
        [HttpGet("add-order")]
        public async Task<IActionResult> AddProductToOrder(long productId, int count)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.GetUserId();
                await orderService.AddProductToOrder(userId, productId, count);
                return JsonResponseStatus.Success(await orderService.GetUserShoppingCart(userId));
                //return JsonResponseStatus.Success(new
                //{
                //    message = "محصول با موفقیت به سبد خرید شما اضافه شد",
                //    cart = await orderService.GetUserShoppingCart(userId)
                //});
            }
            return JsonResponseStatus.Error("برای افزودن محصول به سبد خرید، ابتدا لاگین کنید " );
        }
        #endregion

        #region test
        [HttpGet("test")]
        public async Task<IActionResult> test()
        {
            var details = await orderService.GetUserShoppingCart(User.GetUserId());
            return JsonResponseStatus.Error("sayidi maro");
        }


        #endregion

        #region get user shopping cart
        [HttpGet("get-cart")]
        public async Task<IActionResult> GetUserShoppingCart()
        {
            if (User.Identity.IsAuthenticated)
            {
                var details = await orderService.GetUserShoppingCart(User.GetUserId());
                return JsonResponseStatus.Success(details);
            }
            return JsonResponseStatus.Error("برای مشاهده سبد خرید، ابتدا لاگین کنید ");
        }
        #endregion

        #region remove item from cart
        [HttpGet("remove-order-detail/{detailId}")]
        public async Task<IActionResult> RemoveOrderDetail(long detailId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userOpenOrder = await orderService.GetUserOpenOrder(User.GetUserId());
                var detail = userOpenOrder.OrderDetails.SingleOrDefault(s => s.Id == detailId);
                if (detail != null)
                {
                    await orderService.DeleteOrderDetail(detail);
                    return JsonResponseStatus.Success(await orderService.GetUserShoppingCart(User.GetUserId()));
                }
            }
            return JsonResponseStatus.Error();
        }

        #endregion
    }
}
