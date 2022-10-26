using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Product
{
    public class ProductCommentMiniDTO
    {
        public string Text { get; set; }
        public long ProductId { get; set; }
    }

    public class ProductCommentDTO : ProductCommentMiniDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string UserFullName { get; set; }
        public string Date { get; set; }
    }
}
