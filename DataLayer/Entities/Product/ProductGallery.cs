using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Product
{
    public class ProductGallery :BaseEntity
    {
        #region Properties

        public long ProductId { get; set; }

        [Display(Name = "نام تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکترهای {0} نمیتواند بیشتر از {1} باشد")]
        public string ImageName { get; set; }

        #endregion

        #region Relations
        public Product Product { get; set; }
        #endregion
    }
}
