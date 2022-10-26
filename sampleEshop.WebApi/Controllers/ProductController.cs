using Core.DTOs.Product;
using Core.Services.Interfaces;
using Core.Utilities.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sampleEshop.WebApi.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace sampleEshop.WebApi.Controllers
{
    public class ProductController : SiteBaseController
    {
        #region constructor
        IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        #endregion

        #region filter products

        [HttpGet("filter-products")]
        public async Task<IActionResult> GetProducts([FromQuery] ProductDatasource filter)
        {
            var res = await productService.FilterProducts(filter);
            return JsonResponseStatus.Success(res);
        }

        #endregion

        #region Get products categories

        [HttpGet("child-categories/{id}")]
        public async Task<IActionResult> GetChildCategories(long id)
        {
            return JsonResponseStatus.Success(await productService.GetChildCategories(id));
        }
        #endregion

        #region get single product

        [HttpGet("product-detail/{id}")]
        public async Task<IActionResult> GetProduct(long id)
        {
            var product = await productService.GetProductById(id);
            var productGalleries = await productService.GetProductActiveGalleries(id);

            if (product == null)
                return JsonResponseStatus.NotFound();

            return JsonResponseStatus.Success(new { product = product, gallery = productGalleries });
        }
        #endregion

        #region get related products

        [HttpGet("related-products/{id}")]
        public async Task<IActionResult> GetRelatedProducts(long id)
        {
            var relatedProducts = await productService.GetRelatedProducts(id);
            if (relatedProducts == null)
                return JsonResponseStatus.NotFound();

            return JsonResponseStatus.Success(relatedProducts);
        }

        #endregion

        #region product comments       

        #region get list
        [HttpGet("product-comments/{id}")]
        public async Task<IActionResult> GetProductComments(long id)
        {
            var comments = await productService.GetActiveProductComments(id);
            if(comments==null)
                return JsonResponseStatus.NotFound();
            return JsonResponseStatus.Success(comments);
        }
        #endregion

        #region add
        [HttpPost("add-product-comment")]
        public async Task<IActionResult> AddProductComment([FromBody] ProductCommentMiniDTO comment)
        {
            if (!User.Identity.IsAuthenticated)
                return JsonResponseStatus.Error("لطفا ابتدا وارد سایت شوید");

            if (!await productService.ProductExists(comment.ProductId))
                return JsonResponseStatus.NotFound();

            var userId = User.GetUserId();
            await productService.AddCommentToProduct(comment, userId);
            return JsonResponseStatus.Success();
        }
        #endregion

        #endregion

        //#region latest products

        //public async Task<IActionResult> GetLatestProducts()
        //{

        //    return JsonResponseStatus.Error();
        //}

        //#endregion
    }
}
