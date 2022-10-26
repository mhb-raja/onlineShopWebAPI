using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Product
{
    public class CategoryMini
    {

    }
    public class CategoryDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string UrlTitle { get; set; }
        public long? ParentId { get; set; }        
    }

    public class CategoryTreeItem : CategoryDTO
    {
        public List<CategoryTreeItem> children { get; set; }
    }

    public class CategoryForEditDTO : CategoryDTO
    {
        public string ParentTitle { get; set; }
    }

    public enum ChangeCategoryResult
    {
        Success,
        NotFound,
        NotFoundParent,
        AlreadyExistTitle,
        Error
    }

    public class CategoryResult
    {
        public ChangeCategoryResult Result { get; set; }
        public CategoryDTO Category { get; set; }
    }
}
