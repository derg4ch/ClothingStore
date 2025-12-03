using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.DAL.Specefication
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T>(
            IQueryable<T> inputQuery,
            Specification<T> spec) where T : class
        {
            var query = inputQuery;

            // Where
            query = query.Where(spec.Criteria);

            // Include
            query = spec.Includes.Aggregate(query,
                (current, include) => current.Include(include));

            // OrderBy
            if (spec.OrderBy != null)
                query = spec.OrderBy(query);

            if (spec.OrderByDescending != null)
                query = spec.OrderByDescending(query);

            // Pagination
            if (spec.Skip.HasValue)
                query = query.Skip(spec.Skip.Value);

            if (spec.Take.HasValue)
                query = query.Take(spec.Take.Value);

            return query;
        }
    }
}
