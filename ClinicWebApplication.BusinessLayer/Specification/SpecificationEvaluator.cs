using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClinicWebApplication.Interfaces;

namespace ClinicWebApplication.BusinessLayer.Specification
{
    public class SpecificationEvaluator<T> where T : class, IEntity
    {
        public static async Task<IQueryable<T>> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery;
            if (spec.Criteria != null) query = query.Where(spec.Criteria);
            if (spec.OrderBy != null) query = query.OrderBy(spec.OrderBy);
            if (spec.OrderByDescending != null) query = query.OrderByDescending(spec.OrderByDescending);
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return await Task.FromResult(query);
        }
    }
}
