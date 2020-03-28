using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.CosmosDb
{
    public static class Pagination
    {
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> collection, int page = 1, int resultsPerPage = 10)
        {
            var isEmpty = collection.Any() == false;
            if (isEmpty)
            {
                return new List<T>();
            }

            return collection.Limit(page, resultsPerPage).ToList();
        }

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
