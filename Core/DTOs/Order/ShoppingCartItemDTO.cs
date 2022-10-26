using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Order
{
    public class ShoppingCartItemDTO
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string ImageName { get; set; }
        public int Count { get; set; }
    }
}
