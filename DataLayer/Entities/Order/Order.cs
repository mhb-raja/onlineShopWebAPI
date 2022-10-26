using DataLayer.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Order
{
    public class Order : BaseEntity
    {
        #region Properties

        public long UserId { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaymentDate { get; set; }


        #endregion

        #region Relations

        public User User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

        #endregion
    }
}
