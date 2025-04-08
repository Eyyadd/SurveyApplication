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
        Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken);
        Task<T?> GetById(int id, CancellationToken cancellationToken);
        Task<T?> GetByIdWithoutTracking(int id, CancellationToken cancellationToken);

        Task<int> Add(T entity, CancellationToken cancellationToken);
        Task<int> Update(int id,T entity, CancellationToken cancellationToken);
        Task<int> Delete(T entity, CancellationToken cancellationToken);

        Task<bool> IsExist(int id, CancellationToken cancellationToken);


        Task<int> SaveChanges(CancellationToken cancellation);
    }
}
