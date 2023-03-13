using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicWebApplication.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using ClinicWebApplication.Interfaces;
using ClinicWebApplication.BusinessLayer.Specification;

namespace ClinicWebApplication.BusinessLayer.Repository
{
    public class ClinicRepository<T> : IRepository<T> 
        where T : class, IEntity
    {
        private readonly ClinicContext _context;

        public ClinicRepository(ClinicContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public Task<T> GetById(int id)
        {
            return _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Insert(T dataObject)
        {
            _context.Set<T>().Add(dataObject);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T dataObject)
        {
            _context.Entry(dataObject).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task Delete(T dataObject)
        {
            _context.Set<T>().Remove(dataObject);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<T>> FindWithSpecification(ISpecification<T> specification = null)
        {
            return await SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
        }
    }
}
