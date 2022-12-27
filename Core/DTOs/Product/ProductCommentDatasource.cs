using Core.DTOs.TableDatasource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Product
{
    public class ProductCommentDatasource:GenericTableDatasource<ProductCommentForAdminDTO>
    {
        public long? ProductId { get; set; }
        public string Text { get; set; }
        public bool OnlyNotSeen { get; set; }

    }
}
