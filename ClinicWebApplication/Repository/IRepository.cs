using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicWebApplication.Repository
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        Task<T> GetById(int id);
        Task Insert(T dataObject);
        Task Update(T dataObject);
        Task Delete(int id);
    }
}
