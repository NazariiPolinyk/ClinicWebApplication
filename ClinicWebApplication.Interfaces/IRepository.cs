using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicWebApplication.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task Insert(T dataObject);
        Task Update(T dataObject);
        Task Delete(T dataObject);
        Task<IEnumerable<T>> FindWithSpecification(ISpecification<T> specification = null);
    }
}
