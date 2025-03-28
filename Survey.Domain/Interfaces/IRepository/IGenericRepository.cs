using Survey.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Domain.Interfaces.IRepository
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);

        Task<int> Add(T entity);
        Task<int> Update(int id,T entity);
        Task<int> Delete(T entity);

        Task<bool> IsExist(int id);
    }
}
