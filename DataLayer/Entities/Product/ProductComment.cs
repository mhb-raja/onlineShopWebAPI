using DataLayer.Entities.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Product
{
    public class ProductComment:BaseEntity
    {
        #region Properties

        public long ProductId { get; set; }

        public long UserID { get; set; }

        [Display(Name = "نظر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "تعداد کاراکترهای {0} نمیتواند بیشتر از {1} باشد")]
        public string Text { get; set; }

        public bool IsAccepted { get; set; }
        #endregion

        #region Relations
        public Product Product { get; set; }
        public User User { get; set; }

        //[ForeignKey("ParentId")]
        //public ProductComment ParentComment { get; set; }
        #endregion
    }
}
