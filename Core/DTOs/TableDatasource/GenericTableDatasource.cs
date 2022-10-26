using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.TableDatasource
{
    public class GenericTableDatasource<TEntity>
    {
        public List<TEntity> Items { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int TotalItems { get; set; }

        public GenericTableDatasource()
        {
            this.PageSize = 10;
        }
        internal GenericTableDatasource<TEntity> SetItems(List<TEntity> entities)
        {
            this.Items = entities;
            return this;
        }
    }
}
