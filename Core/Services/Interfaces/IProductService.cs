using Core.DTOs.Product;
using DataLayer.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IProductService : IDisposable
    {
        #region Product

        Task<ProductDTO> AddProduct(ProductDTO product);
        Task<ProductDatasource> FilterProducts(ProductDatasource filter);
        Task<ProductDTO> GetProductById(long productId);
        Task<ProductDTO> EditProduct(ProductDTO product);
        Task<List<ProductMiniDTO>> GetRelatedProducts(long productId);

        Task<bool> ProductExists(long productId);
        Task<Product> GetProductForUserOrder(long productId);

        Task DeleteProduct(long id);
        #endregion


        #region categories

        Task UpdateCategory(Category category);
        Task<List<CategoryTreeItem>> GetCategoryTree();
        Task<List<CategoryDTO>> GetChildCategories(long categoryId);

        Task<ChangeCategoryResult> AddCategory(CategoryDTO ctg);
        Task<CategoryForEditDTO> GetCategoryForEdit(long ctgId);
        Task<ChangeCategoryResult> EditCategory(CategoryDTO ctg);

        //Task<List<ProductCategory>> GetAllActiveProductCategories();

        #endregion

        #region product gallery
        Task<List<ProductGalleryMiniDTO>> GetProductActiveGalleries(long productId);
        Task AddImageToProductGallery(ProductGalleryDTO item);
        Task EditProductGallery(ProductGalleryDTO item);
        Task DeleteProductGallery(long id);

        #endregion

        #region product comments

        Task<int> UnreadCommentsCount();

        Task AddCommentToProduct(ProductCommentMiniDTO productComment, long userId);
        Task<List<ProductCommentDTO>> GetActiveProductComments(long productId);
        Task<ProductCommentDatasource> FilterProductComments(ProductCommentDatasource filter);
        Task DeleteProductComment(long id);
        #endregion

    }
}
