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
        where T : IModel
    {
        private bool disposedValue = false;
        private readonly ClinicContext _context;
        private IQueryable<T> _dbSet;

        public ClinicRepository(ClinicContext context, IQueryable<T> dbSet)
        {
            _context = context;
            _dbSet = dbSet;
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public Task<T> GetById(int id)
        {
            return _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Insert(T dataObject)
        {
            _dbSet.ToList().Add(dataObject);
        }

        public void Update(T dataObject)
        {
            _context.Entry(dataObject).State = EntityState.Modified;
        }
        public void Delete(int id)
        {
            T dataObject = _dbSet.FirstOrDefault(x => x.Id == id);
            _dbSet.ToList().Remove(dataObject);
        }
        public void Save()
        {
            _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ClinicRepository()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
