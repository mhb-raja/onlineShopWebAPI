using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Product
{
    public class ProductMiniDTO
    {
        public long Id { get; set; }
        public string ProductName { get; set; }
        public string UrlCodeFa { get; set; }
        public int Price { get; set; }
        public string Base64Image { get; set; }
        public bool IsAvailable { get; set; }
    }
    public class ProductDTO : ProductMiniDTO
    {
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }
        public string CategoryTitle { get; set; }

        //public bool HasSizeVariant { get; set; }
        //public long? SizeGroupId { get; set; }
    }

}
