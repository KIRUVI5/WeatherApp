using weatherApp.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace weatherApp.Repository.Intrface
{
    public interface IRepositoryBase<T> where T : class
    {
        public Task<List<T>> GetAll();
        Task<T> Get(int id);
        Task<T> Get(long id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(int id);
    }
}
