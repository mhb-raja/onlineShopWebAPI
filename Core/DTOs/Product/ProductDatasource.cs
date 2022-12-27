using Core.DTOs.TableDatasource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Product
{
    public class ProductDatasource : GenericTableDatasource<ProductMiniDTO>
    {
        public string Text { get; set; }
        public int StartPrice { get; set; }
        public int EndPrice { get; set; }
        public int MaxPrice { get; set; }
        public bool AvailableOnly { get; set; }
        public List<long> Categories { get; set; }
        public ProductOrderBy? OrderBy { get; set; }
    }

    public enum ProductOrderBy
    {
        PriceAsc = 1,
        PriceDesc = 2,
        CreateDateAsc = 3,
        CreateDateDesc = 4,
        IsSpecial = 5
    }
}
