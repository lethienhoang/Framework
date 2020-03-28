using Framework.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.PostgreSQL
{
    public static class Pagination
    {
        public static PagedResult<T> Paginate<T>(this IEnumerable<T> collection, PagedQueryBase query) 
            => collection.Paginate(query.Page, query.Results);

        public static PagedResult<T> Paginate<T>(this IEnumerable<T> collection, int page = 1, int resultsPerPage = 10)
        {
            if (page <= 0)
            {
                page = 1;
            }
            if (resultsPerPage <= 0)
            {
                resultsPerPage = 10;
            }
            var isEmpty = collection.Any() == false;
            if (isEmpty)
            {
                return PagedResult<T>.Empty;
            }
            var totalResults = collection.Count();
            var totalPages = (int)Math.Ceiling((decimal)totalResults / resultsPerPage);
            var data = collection.Limit(page, resultsPerPage).ToList();

            return PagedResult<T>.Create(data, page, resultsPerPage, totalPages, totalResults);
        }

        public static IEnumerable<T> Limit<T>(this IEnumerable<T> collection, PagedQueryBase query)
            => collection.Limit(query.Page, query.Results);

        public static IEnumerable<T> Limit<T>(this IEnumerable<T> collection,
            int page = 1, int resultsPerPage = 10)
        {
            if (page <= 0)
            {
                page = 1;
            }
            if (resultsPerPage <= 0)
            {
                resultsPerPage = 10;
            }
            var skip = (page - 1) * resultsPerPage;
            var data = collection.Skip(skip)
                .Take(resultsPerPage);

            return data;
        }
    }
}
