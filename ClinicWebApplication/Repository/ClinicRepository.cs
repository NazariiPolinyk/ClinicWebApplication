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

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public Task<T> GetById(int id)
        {
            return _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Insert(T dataObject)
        {
            _context.Set<T>().ToList().Add(dataObject);
        }

        public void Update(T dataObject)
        {
            _context.Entry(dataObject).State = EntityState.Modified;
        }
        public void Delete(int id)
        {
            T dataObject = _context.Set<T>().FirstOrDefault(x => x.Id == id);
            _context.Set<T>().ToList().Remove(dataObject);
        }
        public void Save()
        {
            _context.SaveChangesAsync();
        }
    }
}
