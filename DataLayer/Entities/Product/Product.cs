using DataLayer.Entities.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Product
{
    public class Product : BaseEntity
    {
        #region Properties

        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکترهای {0} نمیتواند بیشتر از {1} باشد")]
        public string Name { get; set; }

        [Display(Name = "عنوان در url")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکترهای {0} نمیتواند بیشتر از {1} باشد")]
        public string UrlCodeFa { get; set; }


        [ForeignKey("Category")]
        public long CategoryId { get; set; }

        [Display(Name = "قیمت")]
        public int Price { get; set; }


        [Display(Name = "توضیحات کوتاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکترهای {0} نمیتواند بیشتر از {1} باشد")]
        public string ShortDescription { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(2000, ErrorMessage = "تعداد کاراکترهای {0} نمیتواند بیشتر از {1} باشد")]
        public string Description { get; set; }

        [Display(Name = "نام تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکترهای {0} نمیتواند بیشتر از {1} باشد")]
        public string ImageName { get; set; }

        [Display(Name = "موجود/به اتمام رسیده")]
        public bool IsAvailable { get; set; }

        //[Display(Name = "ویژه")]
        //public bool IsSpecial { get; set; }


        


        #endregion

        #region Relations
        public ICollection<ProductGallery> ProductGalleries { get; set; }
        public ICollection<ProductVisit> Visits { get; set; }
        public Category Category { get; set; }        
        public ICollection<ProductComment> ProductComments { get; set; }
        public ICollection<SpecialProduct> SpecialProducts { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        #endregion
    }
}
