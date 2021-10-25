using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicWebApplication.Repository
{
    public class ClinicRepository<T> : IRepository<T> 
        where T : class, IEntity
    {
        private readonly ClinicContext _context;

        public ClinicRepository(ClinicContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
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
        public async Task Delete(int id)
        {
            T dataObject = _context.Set<T>().FirstOrDefault(x => x.Id == id);
            _context.Set<T>().Remove(dataObject);
            await _context.SaveChangesAsync();
        }
    }
}
