using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicWebApplication.Repository
{
    public interface IRepository<T> : IDisposable
    {
        IQueryable<T> GetAll();
        Task<T> GetById(int id);
        void Insert(T dataObject);
        void Update(T dataObject);
        void Delete(int id);
        void Save();
    }
}
