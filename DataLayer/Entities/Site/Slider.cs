using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Site
{
    public class Slider:BaseEntity
    {
        #region Properties

        [Display(Name = "تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //[MaxLength(150, ErrorMessage = "تعداد کاراکترهای {0} نمیتواند بیشتر از {1} باشد")]
        public string ImageName { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکترهای {0} نمیتواند بیشتر از {1} باشد")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(1000, ErrorMessage = "تعداد کاراکترهای {0} نمیتواند بیشتر از {1} باشد")]
        public string Description { get; set; }

        [Display(Name = "تاریخ شروع نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateTime ActiveFrom { get; set; }

        [Display(Name = "تاریخ خاتمه نمایش")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateTime? ActiveUntil { get; set; }

        [Display(Name = "لینک")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکترهای {0} نمیتواند بیشتر از {1} باشد")]
        public string Link { get; set; }

        #endregion
    }
}
