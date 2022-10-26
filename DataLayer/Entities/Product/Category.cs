using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Product
{
    public class Category : BaseEntity
    {
        #region Properties

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکترهای {0} نمیتواند بیشتر از {1} باشد")]
        public string Title { get; set; }


        [Display(Name = "متن لینک")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکترهای {0} نمیتواند بیشتر از {1} باشد")]
        public string UrlTitle { get; set; }

        public long? ParentId { get; set; }
        #endregion

        #region Relations

        [ForeignKey("ParentId")]
        public Category Parent { get; set; }

        public ICollection<Category> Children { get; set; }

        public ICollection<Product> Products { get; set; }

        #endregion
    }
}
