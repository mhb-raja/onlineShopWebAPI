using Core.DTOs.Product;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sampleEshop.WebApi.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sampleEshop.WebApi.Controllers
{
    public class AdminProductController : SiteBaseController
    {
        #region constructor

        private readonly IProductService productService;
        //private readonly ILogger<AdminProductController> _logger;
        public AdminProductController(IProductService productService)
            //,            ILogger<AdminProductController> logger)
        {
            this.productService = productService;
            //this._logger = logger;
        }
        #endregion

        #region product

        #region add product

        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct([FromBody] ProductDTO product)
        {
            if (ModelState.IsValid)
            {                
                return JsonResponseStatus.Success(await productService.AddProduct(product));
            }
            return JsonResponseStatus.Error();
        }
        #endregion

        #region edit product

        [HttpPost("edit-product")]
        public async Task<IActionResult> EditProduct([FromBody] ProductDTO prd)
        {
            if (ModelState.IsValid)
            {
                return JsonResponseStatus.Success(await productService.EditProduct(prd));
            }
            return JsonResponseStatus.Error();
        }
        #endregion

        #region get product for edit

        //[PermissionChecker("ProductManager")]
        [HttpGet("get-product-for-edit/{id}")]
        public async Task<IActionResult> GetProductForEdit(long id)
        {
            var prd = await productService.GetProductById(id);
            if (prd == null)
                return JsonResponseStatus.NotFound("آیتم مورد نظر پیدا نشد");
            else
                return JsonResponseStatus.Success(prd);
        }
        #endregion

        #region filter products

        [HttpGet("filter-products")]
        public async Task<IActionResult> GetProducts([FromQuery] ProductDatasource filter)
        {
            var res = await productService.FilterProducts(filter);
            return JsonResponseStatus.Success(res);
            //return JsonResponseStatus.Success();
        }

        #endregion

        #endregion


        #region category

        #region Get products categories

        [HttpGet("get-tree")]
        public async Task<IActionResult> GetCategoriesTree()
        {
            var res = await productService.GetCategoryTree();
            return JsonResponseStatus.Success(res);
        }
        #endregion

        #region add category
        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDTO ctg)
        {
            if (ModelState.IsValid)
            {
                var res = await productService.AddCategory(ctg);
                switch (res)
                {
                    case ChangeCategoryResult.NotFoundParent:
                        return JsonResponseStatus.Error("کتگوری پدر وجود ندارد");
                    case ChangeCategoryResult.AlreadyExistTitle:
                        return JsonResponseStatus.Error("این عنوان قبلا ثبت شده است");
                }
                return JsonResponseStatus.Success();
            }
            return JsonResponseStatus.Error();
        }
        #endregion

        #region get category for edit

        //[PermissionChecker("ProductManager")]
        [HttpGet("get-category-for-edit/{id}")]
        public async Task<IActionResult> GetCategoryForEdit(long id)
        {
            var ctg = await productService.GetCategoryForEdit(id);
            if (ctg == null)
                return JsonResponseStatus.NotFound("آیتم مورد نظر پیدا نشد");
            else
                return JsonResponseStatus.Success(ctg);
        }
        #endregion

        #region edit category

        [HttpPost("edit-category")]
        public async Task<IActionResult> EditCategory([FromBody] CategoryDTO ctg)
        {
            if (ModelState.IsValid)
            {
                var res = await productService.EditCategory(ctg);
                switch (res)
                {
                    case ChangeCategoryResult.NotFoundParent:
                        return JsonResponseStatus.Error("کتگوری پدر وجود ندارد");
                    case ChangeCategoryResult.AlreadyExistTitle:
                        return JsonResponseStatus.Error("این عنوان قبلا ثبت شده است");
                    case ChangeCategoryResult.NotFound:
                        return JsonResponseStatus.NotFound();
                }

                return JsonResponseStatus.Success();
            }
            return JsonResponseStatus.Error();
        }
        #endregion

        #endregion

        #region Gallery

        #region get product galleries

        [HttpGet("product-galleries/{id}")]
        public async Task<IActionResult> GetProductGalleries(long id)
        {
            var productGalleries = await productService.GetProductActiveGalleries(id);
            if (productGalleries == null)
                return JsonResponseStatus.NotFound();
            
            return JsonResponseStatus.Success(productGalleries);
        }

        #endregion


        #region add gallery

        [HttpPost("add-gallery")]
        public async Task<IActionResult> AddGallery([FromBody] ProductGalleryDTO gallery)
        {
            if (ModelState.IsValid)
            {
                await productService.AddImageToProductGallery(gallery);
                return JsonResponseStatus.Success();
            }
            return JsonResponseStatus.Error();
        }
        #endregion

        #region edit gallery

        [HttpPost("edit-gallery")]
        public async Task<IActionResult> Editgallery([FromBody] ProductGalleryDTO gallery)
        {
            if (ModelState.IsValid)
            {
                await productService.EditProductGallery(gallery);
                return JsonResponseStatus.Success();
            }
            return JsonResponseStatus.Error();
        }
        #endregion

        #region delete gallery

        [HttpGet("delete-gallery/{id}")]
        public async Task<IActionResult> Deletegallery(long id)
        {
            if (ModelState.IsValid)
            {
                await productService.DeleteProductGallery(id);
                return JsonResponseStatus.Success();
            }
            return JsonResponseStatus.Error();
        }
        #endregion
        #endregion
    }
}
