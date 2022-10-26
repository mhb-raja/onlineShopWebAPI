using DataLayer.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Order
{
    public class OrderDetail : BaseEntity
    {
        #region properties
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        #endregion

        #region relations
        public Product.Product Product { get; set; }
        public Order Order { get; set; }
        #endregion
    }
}
