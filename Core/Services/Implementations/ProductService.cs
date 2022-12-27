using Core.DTOs.Product;
using Core.Security;
using Core.Services.Interfaces;
using Core.Utilities.Common;
using Core.Utilities.Extensions;
using DataLayer.Entities.Product;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Implementations
{
    public class ProductService : IProductService
    {
        #region constructor

        IGenericRepository<Product> productRepository;
        IGenericRepository<Category> categoryRepository;
        IGenericRepository<ProductGallery> productGalleryRepository;
        //IGenericRepository<ProductVisit> productVisitRepository;
        IGenericRepository<ProductComment> productCommentRepository;

        //IGenericRepository<SizeVariantGroup> sizeGroupRepository;
        //IGenericRepository<SizeVariant> sizeRepository;
        //IGenericRepository<PAttribute> attributeRepository;
        //IGenericRepository<AttributeValue> attribValueRepository;
        //IGenericRepository<CategoryAttribute> categoryAttribRepository;
        public ProductService(
                              IGenericRepository<Product> productRepository,
                              IGenericRepository<Category> categoryRepository,
                              IGenericRepository<ProductGallery> productGalleryRepository,
            //IGenericRepository<ProductVisit> productVisitRepository,
            IGenericRepository<ProductComment> productCommentRepository

            //IGenericRepository<SizeVariantGroup> productSizeGroupRepository,
            //IGenericRepository<SizeVariant> productSizeRepository,
            //IGenericRepository<PAttribute> attributeRepository,
            //IGenericRepository<AttributeValue> attribValueRepository,
            //IGenericRepository<CategoryAttribute> categoryAttribRepository
            )
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.productGalleryRepository = productGalleryRepository;
            //this.productVisitRepository = productVisitRepository;
            this.productCommentRepository = productCommentRepository;

            //this.sizeGroupRepository = productSizeGroupRepository;
            //this.sizeRepository = productSizeRepository;
            //this.attributeRepository = attributeRepository;
            //this.attribValueRepository = attribValueRepository;
            //this.categoryAttribRepository = categoryAttribRepository;
        }

        #endregion

        #region Category

        public async Task<ChangeCategoryResult> AddCategory(CategoryDTO ctg)
        {
            if (ctg.ParentId > 0 && !categoryRepository.GetEntitiesQuery().Any(x => x.Id == ctg.ParentId))
                return ChangeCategoryResult.NotFoundParent;
            if (categoryRepository.GetEntitiesQuery().Any(x => x.Title == ctg.Title || x.UrlTitle == ctg.UrlTitle))
                return ChangeCategoryResult.AlreadyExistTitle;

            Category newCtg = new Category
            {
                Title = ctg.Title.SanitizeText(),
                UrlTitle = ctg.UrlTitle.SanitizeText(),
                ParentId = (ctg.ParentId <= 0) ? (long?)null : ctg.ParentId
            };
            await categoryRepository.AddEntity(newCtg);
            await categoryRepository.SaveChanges();
            return ChangeCategoryResult.Success;
        }

        public async Task<CategoryResult> AddCategory2(CategoryDTO ctg)
        {
            if (ctg.ParentId > 0 && !categoryRepository.GetEntitiesQuery().Any(x => x.Id == ctg.ParentId))
                return new CategoryResult { Result = ChangeCategoryResult.NotFoundParent, Category = null };
            if (categoryRepository.GetEntitiesQuery().Any(x => x.Title == ctg.Title || x.UrlTitle == ctg.UrlTitle))
                return new CategoryResult { Result = ChangeCategoryResult.AlreadyExistTitle, Category = null };

            Category newCtg = new Category
            {
                Title = ctg.Title.SanitizeText(),
                UrlTitle = ctg.UrlTitle.SanitizeText(),
                ParentId = (ctg.ParentId <= 0) ? (long?)null : ctg.ParentId
            };
            await categoryRepository.AddEntity(newCtg);
            await categoryRepository.SaveChanges();
            ctg.Id = newCtg.Id;
            return new CategoryResult { Result = ChangeCategoryResult.Success, Category = ctg };
        }

        public async Task<CategoryForEditDTO> GetCategoryForEdit(long ctgId)
        {
            //var ctg = await categoryRepository.GetEntityById(ctgId);
            var ctg = await categoryRepository.GetEntitiesQuery().Include(c => c.Parent).SingleOrDefaultAsync(c => c.Id == ctgId);
            if (ctg == null)
                return null;
            return new CategoryForEditDTO
            {
                Id = ctg.Id,
                ParentId = ctg.ParentId,
                Title = ctg.Title,
                UrlTitle = ctg.UrlTitle,
                ParentTitle = ctg.Parent?.Title
            };
        }

        public async Task<ChangeCategoryResult> EditCategory(CategoryDTO ctg)
        {
            var ctgToEdit = await categoryRepository.GetEntityById(ctg.Id);

            if (ctgToEdit != null)
            {
                if (ctg.ParentId > 0 && !categoryRepository.GetEntitiesQuery().Any(x => x.Id == ctg.ParentId))
                    return ChangeCategoryResult.NotFoundParent;
                if (categoryRepository.GetEntitiesQuery()
                    .Any(x => (x.Title == ctg.Title || x.UrlTitle == ctg.UrlTitle) && x.Id != ctg.Id))
                    return ChangeCategoryResult.AlreadyExistTitle;

                ctgToEdit.Title = ctg.Title;
                ctgToEdit.UrlTitle = ctg.UrlTitle;
                ctgToEdit.ParentId = ctg.ParentId;

                categoryRepository.UpdateEntity(ctgToEdit);
                await categoryRepository.SaveChanges();
                return ChangeCategoryResult.Success;
            }
            else
                return ChangeCategoryResult.NotFound;
        }
        public async Task UpdateCategory(Category productCategory)
        {
            categoryRepository.UpdateEntity(productCategory);
            await categoryRepository.SaveChanges();
        }

        public async Task<List<CategoryDTO>> GetChildCategories(long categoryId)
        {
            long? id = (categoryId < 0) ? (long?)null : categoryId;

            return await categoryRepository.GetEntitiesQuery().Where(c => c.ParentId == id)
                .OrderBy(c => c.Title)
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Title = c.Title,
                    UrlTitle = c.UrlTitle,
                    ParentId = c.ParentId
                    //HasChildren = (categoryRepository.GetEntitiesQuery().Any(x => x.ParentId == c.Id))
                }).ToListAsync();
        }

        public async Task<List<CategoryTreeItem>> GetChildTreeBranches(long categoryid)
        {
            long? id = (categoryid < 0) ? (long?)null : categoryid;

            return await categoryRepository.GetEntitiesQuery().Where(c => c.ParentId == id && !c.IsDelete)
                .Select(c => new CategoryTreeItem
                {
                    Id = c.Id,
                    Title = c.Title,
                    UrlTitle = c.UrlTitle,
                    ParentId = c.ParentId
                }).ToListAsync();
        }

        public async Task<List<CategoryTreeItem>> GetChildren(List<CategoryTreeItem> list)
        {
            foreach (var item in list)
            {
                item.children = await GetChildTreeBranches(item.Id);
                if (item.children.Count > 0)
                    await GetChildren(item.children);
            }
            return list;
        }

        public async Task<List<CategoryTreeItem>> GetCategoryTree()
        {
            var root = await categoryRepository.GetEntitiesQuery()//.Include(x => x.ParentCategory)
                 .Where(x => x.ParentId == null && !x.IsDelete)
                 .Select(n => new CategoryTreeItem
                 {
                     Id = n.Id,
                     ParentId = n.ParentId,
                     Title = n.Title,
                     UrlTitle = n.UrlTitle
                 }).ToListAsync();
            await GetChildren(root);
            return root;

        }
        #endregion

        #region Product

        public async Task<ProductDTO> AddProduct(ProductDTO p)
        {
            Product newProduct = new Product
            {
                IsAvailable = p.IsAvailable,
                Price = p.Price,
                Name = p.ProductName,//.SanitizeText(),
                ShortDescription = p.ShortDescription,
                UrlCodeFa = p.UrlCodeFa,
                Description = p.Description,
                CategoryId = p.CategoryId
            };
            newProduct.ImageName = ImageUploaderExtension.
                SaveBase64ImageToServer(p.Base64Image, PathTools.ProductImageServerPath);

            //if (!string.IsNullOrEmpty(p.Base64Image))
            //{
            //    var imageFile = ImageUploaderExtension.Base64ToImage(p.Base64Image);
            //    var imageName = Guid.NewGuid().ToString("N") + ".jpeg";
            //    imageFile.AddImageToServer(imageName, PathTools.ProductImageServerPath);
            //    newProduct.ImageName = imageName;
            //}
            await productRepository.AddEntity(newProduct);
            await productRepository.SaveChanges();
            p.Id = newProduct.Id;
            return p;
        }

        public async Task<ProductDatasource> FilterProducts(ProductDatasource filter)
        {
            var productQuery = productRepository.GetEntitiesQuery().AsQueryable().Where(p => !p.IsDelete);
            filter.MaxPrice = productQuery.Max(p => p.Price);
            if (filter.AvailableOnly)
                productQuery = productQuery.Where(p => p.IsAvailable);

            if (filter.StartPrice > 0)
                productQuery = productQuery.Where(s => s.Price >= filter.StartPrice);

            if (filter.EndPrice == 0)
                filter.EndPrice = filter.MaxPrice;
            else if (filter.EndPrice != filter.MaxPrice)
                productQuery = productQuery.Where(s => s.Price <= filter.EndPrice);

            switch (filter.OrderBy)
            {
                case ProductOrderBy.PriceAsc:
                    productQuery = productQuery.OrderBy(s => s.Price);
                    break;
                case ProductOrderBy.PriceDesc:
                    productQuery = productQuery.OrderByDescending(s => s.Price);
                    break;
                case ProductOrderBy.CreateDateAsc:
                    productQuery = productQuery.OrderBy(s => s.CreatedAt);
                    break;
                case ProductOrderBy.CreateDateDesc:
                    productQuery = productQuery.OrderByDescending(s => s.CreatedAt);
                    break;
                case ProductOrderBy.IsSpecial:
                    productQuery = productQuery.SelectMany(p => p.SpecialProducts
                            .Where(sp => sp.EndDate > DateTime.Now && sp.StartDate <= DateTime.Now))
                            .Select(z => z.Product);


                    //productQuery.Include(p => p.SpecialProducts)
                    //.Where(s => s.SpecialProducts.Where(sp => sp.EndDate > DateTime.Now && sp.StartDate <= DateTime.Now).SingleOrDefault());

                    //if (filter.Categories != null && filter.Categories.Any())
                    //    productQuery = productQuery
                    //        .SelectMany(s => s.ProductSelectedCategories.Where(f => filter.Categories.Contains(f.ProductCategoryId))
                    //        .Select(t => t.Product));

                    //var productCategoriesList = await productSelectedCategoryRepository.GetEntitiesQuery()
                    // .Where(s => s.ProductId == productId).Select(f => f.ProductCategoryId).ToListAsync();
                    //var relatedProducts = await productRepository.GetEntitiesQuery()
                    //    .SelectMany(s => s.ProductSelectedCategories.Where(f => productCategoriesList.Contains(f.ProductCategoryId)).Select(t => t.Product))
                    //    .Where(s => s.Id != productId).OrderByDescending(s => s.CreateDate).Take(4).ToListAsync();
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(filter.Text))
                productQuery = productQuery.Where(s => s.Name.Contains(filter.Text));// || s.Description.Contains(filter.Text));

            if (filter.Categories != null && filter.Categories.Any())
                productQuery = productQuery.Where(p => filter.Categories.Contains(p.CategoryId));
                                   

            filter.TotalItems = productQuery.Count();

            var products = await productQuery.Paging(filter.PageIndex, filter.PageSize).
                Select(p => new ProductMiniDTO
                {
                    Id = p.Id,
                    ProductName = p.Name,
                    //Description = p.Description,
                    Base64Image = p.ImageName,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    //ShortDescription = p.ShortDescription,
                    UrlCodeFa = p.UrlCodeFa,
                    //CategoryId = p.CategoryId,//???????????????????? neccessary??
                    //CategoryTitle = p.Category.Title
                })
                .ToListAsync();

            filter.SetItems(products);
            return filter;
        }

        public async Task<ProductDTO> GetProductById(long productId)
        {
            try
            {
                var product = await productRepository.GetEntitiesQuery()
                .SingleOrDefaultAsync(s => !s.IsDelete && s.Id == productId);
                if (product == null) return null;
                return new ProductDTO
                {
                    Id = product.Id,
                    Base64Image = product.ImageName,
                    Description = product.Description,
                    IsAvailable = product.IsAvailable,
                    Price = product.Price,
                    ProductName = product.Name,
                    ShortDescription = product.ShortDescription,
                    UrlCodeFa = product.UrlCodeFa,
                    CategoryId = product.CategoryId,
                    //CategoryTitle = product.Category.Title
                };
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<ProductDTO> EditProduct(ProductDTO product)
        {
            try
            {
                var mainProduct = await productRepository.GetEntityById(product.Id);
                if (mainProduct != null)
                {
                    mainProduct.Name = product.ProductName;
                    mainProduct.Description = product.Description;
                    mainProduct.ShortDescription = product.ShortDescription;
                    mainProduct.IsAvailable = product.IsAvailable;
                    //mainProduct.HasSizeVariant=product.
                    mainProduct.CategoryId = product.CategoryId;
                    mainProduct.Price = product.Price;
                    mainProduct.UrlCodeFa = product.UrlCodeFa;

                    if (product.Base64Image.StartsWith("data:image/jpeg;base64"))
                        mainProduct.ImageName = ImageUploaderExtension
                            .SaveBase64ImageToServer(product.Base64Image, PathTools.ProductImageServerPath, mainProduct.ImageName);
                    //else
                    //    mainProduct.ImageName = product.Base64Image;//photo has not been changed, value is imagename
                    //if (!string.IsNullOrEmpty(product.Base64Image))
                    //{
                    //    var imageFile = ImageUploaderExtension.Base64ToImage(product.Base64Image);
                    //    var imageName = Guid.NewGuid().ToString("N") + ".jpeg";
                    //    imageFile.AddImageToServer(imageName, PathTools.ProductImageServerPath, mainProduct.ImageName);
                    //    mainProduct.ImageName = imageName;
                    //}

                    productRepository.UpdateEntity(mainProduct);
                    await productRepository.SaveChanges();
                    return product;
                }
                return null;
            }
            catch (Exception e)
            {

                throw;
            }

        }

        public async Task<List<ProductMiniDTO>> GetRelatedProducts(long productId)
        {
            var product = await productRepository.GetEntityById(productId);
            if (product == null)
                return null;
            var relatedProducts = await productRepository.GetEntitiesQuery()
                .Where(f => f.CategoryId == product.CategoryId && f.Id != productId).OrderByDescending(s => s.UpdatedAt).Take(6)
                .Select(p => new ProductMiniDTO
                {
                    Id = p.Id,
                    Base64Image = p.ImageName,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    ProductName = p.Name,
                    UrlCodeFa = p.UrlCodeFa
                }).ToListAsync();
            return relatedProducts;
        }

        public async Task<bool> ProductExists(long productId)
        {
            return await productRepository.GetEntitiesQuery().AnyAsync(p => p.Id == productId);
        }

        public async Task<Product> GetProductForUserOrder(long productId)
        {
            return await productRepository.GetEntitiesQuery()
                .SingleOrDefaultAsync(s => s.Id == productId && !s.IsDelete && s.IsAvailable);
        }

        public async Task DeleteProduct(long id)
        {
            await productRepository.RemoveEntity(id);
            await productRepository.SaveChanges();
        }


        #endregion



        #region product gallery

        public async Task<int> UnreadCommentsCount()
        {
            return await productCommentRepository.GetEntitiesQuery().CountAsync(p => !p.IsDelete && !p.SeenByAdmin);
        }
        public async Task<List<ProductGalleryMiniDTO>> GetProductActiveGalleries(long productId)
        {
            if (!await ProductExists(productId))
                return null;
            return await productGalleryRepository.GetEntitiesQuery()
                .Where(s => s.ProductId == productId && !s.IsDelete)
                .Select(s => new ProductGalleryMiniDTO
                {
                    Id = s.Id,
                    //ProductId = s.ProductId,
                    ImageName = s.ImageName,

                }).ToListAsync();
        }

        public async Task AddImageToProductGallery(ProductGalleryDTO item)
        {
            if (productRepository.GetEntitiesQuery().Any(x => x.Id == item.ProductId))
            {
                ProductGallery pg = new ProductGallery
                {
                    ProductId = item.ProductId,
                    ImageName = ImageUploaderExtension
                    .SaveBase64ImageToServer(item.ImageName, PathTools.ProductGalleryImageServerPath)
                };
                await productGalleryRepository.AddEntity(pg);
                await productGalleryRepository.SaveChanges();
            }
        }
        public async Task EditProductGallery(ProductGalleryDTO item)
        {
            var prev = await productGalleryRepository.GetEntityById(item.Id);
            if (prev != null)
            {
                prev.ProductId = item.ProductId;
                prev.ImageName = ImageUploaderExtension
                    .SaveBase64ImageToServer(item.ImageName, PathTools.ProductGalleryImageServerPath, item.ImageName);
                productGalleryRepository.UpdateEntity(prev);
                await productGalleryRepository.SaveChanges();
            }
        }

        public async Task DeleteProductGallery(long id)
        {
            await productGalleryRepository.RemoveEntity(id);
            await productGalleryRepository.SaveChanges();

        }
        #endregion


        #region product comments
        public async Task AddCommentToProduct(ProductCommentMiniDTO comment, long userId)
        {
            ProductComment pc = new ProductComment
            {
                ProductId = comment.ProductId,
                Text = comment.Text.SanitizeText(),
                UserID = userId,
                Approved = false,
                SeenByAdmin = false,
                ParentId = comment.ParentId
            };
            await productCommentRepository.AddEntity(pc);
            await productCommentRepository.SaveChanges();
        }


        public async Task<List<ProductCommentDTO>> GetActiveProductComments(long productId)
        {
            if (!await ProductExists(productId))
                return null;
            return await productCommentRepository.GetEntitiesQuery()
                .Include(z => z.User)
                .Where(c => c.ProductId == productId && c.Approved && !c.IsDelete)
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => new ProductCommentDTO
                {
                    Id = s.Id,
                    Text = s.Text,
                    UserId = s.UserID,
                    UserFullName = $"{s.User.Firstname} {s.User.Lastname}",
                    Date = s.CreatedAt,
                    ParentId = s.ParentId.Value,
                    LikeCount = s.LikeCount,
                    DislikeCount = s.DislikeCount
                }).ToListAsync();
        }


        public async Task<ProductCommentDatasource> FilterProductComments(ProductCommentDatasource filter)
        {
            var query = productCommentRepository.GetEntitiesQuery().AsQueryable().Where(p => !p.IsDelete);


            if (!string.IsNullOrEmpty(filter.Text))
                query = query.Where(s => s.Text.Contains(filter.Text));

            if (filter.ProductId != null && filter.ProductId > 0)
                query = query.Where(s => s.ProductId == filter.ProductId);

            filter.TotalItems = query.Count();

            try
            {
                var comments = await query
                .OrderByDescending(p => p.CreatedAt)
                .Paging(filter.PageIndex, filter.PageSize)
                .Include(p=>p.Product).Include(p=>p.User)//.Include(p=>p.ParentComment)
                .Select(p => new ProductCommentForAdminDTO
                {
                    Id = p.Id,
                    ProductName = p.Product.Name,
                    ProductImage = p.Product.ImageName,
                    ProductId = p.ProductId,
                    Text = p.Text,
                    Approved = p.Approved,
                    Date = p.CreatedAt,
                    SeenByAdmin = p.SeenByAdmin,
                    UserFullName = $"{p.User.Firstname} {p.User.Lastname}",
                    UserId = p.UserID,

                    DislikeCount = p.DislikeCount,
                    LikeCount = p.LikeCount,
                    //ParentId = p.ParentId.Value
                })
                .ToListAsync();

                filter.SetItems(comments);
                return filter;
            }
            catch (Exception e)
            {

                throw;
            }
            

            //return await productCommentRepository.GetEntitiesQuery()
            //    .Include(z => z.Product)
            //    .Where(p => !p.IsDelete)
            //    .GroupBy(s => s.ProductId)
            //    .OrderByDescending
            //    .Select(p => new ProductCommentForAdminDTO
            //    {

            //    })
            //    .ToListAsync();
        }
        /*
         var query = context.People
                   .GroupBy(p => p.name)
                   .Select(g => new { name = g.Key, count = g.Count() });
         */

        public async Task DeleteProductComment(long id)
        {
            await productCommentRepository.RemoveEntity(id);
            await productCommentRepository.SaveChanges();
        }
        #endregion


        #region Dispose
        public void Dispose()
        {
            this.productRepository?.Dispose();
            this.categoryRepository?.Dispose();
            this.productGalleryRepository?.Dispose();
            //this.productVisitRepository?.Dispose();
            this.productCommentRepository?.Dispose();
            //this.sizeGroupRepository?.Dispose();
            //this.sizeRepository?.Dispose();
        }

        #endregion
    }
}
