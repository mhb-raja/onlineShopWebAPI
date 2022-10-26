using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Product
{
    public class ProductGalleryMiniDTO
    {
        public long Id { get; set; }
        public string ImageName { get; set; }
    }
    public class ProductGalleryDTO : ProductGalleryMiniDTO
    {
        public long ProductId { get; set; }
    }
}
