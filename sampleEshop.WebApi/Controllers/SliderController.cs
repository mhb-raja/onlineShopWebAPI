using Core.DTOs.Slider;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sampleEshop.WebApi.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sampleEshop.WebApi.Controllers
{
    public class SliderController : SiteBaseController
    {
        #region constructor

        private readonly ISliderService sliderService;
        public SliderController(ISliderService sliderService)
        {
            this.sliderService = sliderService;
        }

        #endregion

        #region all active sliders

        [HttpGet("GetActiveSliders")]
        public async Task<IActionResult> GetActiveSliders()
        {
            //var sliders = await sliderService.GetActiveSliders();
            //return JsonResponseStatus.Success(sliders);
            return JsonResponseStatus.Success(await sliderService.GetActiveSliders());
        }
        #endregion

        #region filter sliders

        [HttpGet("filter-sliders")]
        public async Task<IActionResult> GetSliders([FromQuery] SliderDatasource filter)
        {
            var sliders = await sliderService.FilterSliders(filter);
            return JsonResponseStatus.Success(sliders);
            //return JsonResponseStatus.Success();
        }

        #endregion

        #region add
        [HttpPost("add-slider")]
        public async Task<IActionResult> AddSlider([FromBody] SliderDTO slider)
        {
            if (ModelState.IsValid)
            {
                await sliderService.AddSlider(slider);
                return JsonResponseStatus.Success();
            }
            return JsonResponseStatus.Error();
        }
        #endregion

        #region get slider for edit

        //[PermissionChecker("ProductManager")]
        [HttpGet("get-slider-for-edit/{id}")]
        public async Task<IActionResult> GetSliderForEdit(long id)
        {
            var slider = await sliderService.GetSliderForEdit(id);
            if (slider == null)
                return JsonResponseStatus.NotFound();
            else
                return JsonResponseStatus.Success(slider);
        }
        #endregion


        #region edit slider

        [HttpPost("edit-slider")]
        public async Task<IActionResult> EditSlider([FromBody] SliderDTO slider)
        {
            if (ModelState.IsValid)
            {
                await sliderService.EditSlider(slider);
                return JsonResponseStatus.Success();
            }
            return JsonResponseStatus.Error();
        }
        #endregion

        #region remove slider
        [HttpGet("remove-slider/{id}")]
        public async Task<IActionResult> RemoveSlider(long id)
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    var userOpenOrder = await orderService.GetUserOpenOrder(User.GetUserId());
            //    var detail = userOpenOrder.OrderDetails.SingleOrDefault(s => s.Id == detailId);
            //    if (detail != null)
            //    {
            //        await orderService.DeleteOrderDetail(detail);
            //        return JsonResponseStatus.Success(await orderService.GetUserShoppingCart(User.GetUserId()));
            //    }
            //}
            return JsonResponseStatus.Error("not implemented");

        }

        #endregion
    }
}
