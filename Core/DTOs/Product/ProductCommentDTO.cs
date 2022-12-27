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
        public long ParentId { get; set; }
    }

    public class ProductCommentDTO : ProductCommentMiniDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string UserFullName { get; set; }
        public DateTime Date { get; set; }

        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        

        //public string AdminReply { get; set; }
        //public DateTime AdminReplyDate { get; set; }
    }

    public class ProductCommentForAdminDTO : ProductCommentDTO
    {
        public bool Approved { get; set; }
        public bool SeenByAdmin { get; set; }

        public string ProductName { get; set; }
        public string ProductImage { get; set; }
    }


}
