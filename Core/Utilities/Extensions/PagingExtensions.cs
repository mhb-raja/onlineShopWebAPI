using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Extensions
{
    public static class PagingExtensions
    {
        //public static IQueryable<T> Paging<T>(this IQueryable<T> queryable, BasePaging pager)
        //{
        //    return queryable.Skip(pager.SkipEntity).Take(pager.TakeEntity);
        //}

        //public static IQueryable<T> Paging<T>(this IQueryable<T> queryable, int skip, int take)
        //{
        //    return queryable.Skip(skip).Take(take);
        //}

        public static IQueryable<T> Paging<T>(this IQueryable<T> queryable, int pageIndex, int pageSize)
        {
            return queryable.Skip(pageIndex * pageSize).Take(pageSize);
        }

        //public static IQueryable<T> Paging<T>(this IQueryable<T> queryable, Paginator paginator)
        //{
        //    return queryable.Skip(paginator.PageIndex * paginator.PageSize).Take(paginator.PageSize);
        //}
    }
}
