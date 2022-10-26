using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Product
{
    public class SpecialProduct : BaseEntity
    {
        [Display(Name = "محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long ProductId { get; set; }

        [Display(Name = "تاریخ شروع ویژه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateTime StartDate { get; set; }

        [Display(Name = "تاریخ پایان ویژه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateTime EndDate { get; set; }


        #region relations
        public Product Product { get; set; }

        #endregion
    }
}
