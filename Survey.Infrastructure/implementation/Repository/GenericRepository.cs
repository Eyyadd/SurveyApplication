using Microsoft.EntityFrameworkCore;
using Survey.Domain.Entities;
using Survey.Domain.Interfaces.IRepository;
using Survey.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Survey.Infrastructure.implementation.Repository
{
    public class GenericRepository<T>(SurveyDbContext dbContext) : IGenericRepository<T> where T : BaseEntity
    {
        private readonly SurveyDbContext _dbContext = dbContext;
        public async Task<int> Add(T entity,CancellationToken cancellationToken)
        {
            _dbContext.Set<T>().Add(entity);
            await SaveChanges(cancellationToken);

            return entity.Id;
        }

        public async Task<int> Delete(T entity, CancellationToken cancellationToken)
        {
            _dbContext.Remove(entity);
            return await SaveChanges(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<T> GetById(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().FirstAsync(x=>x.Id == id, cancellationToken);
        }

        public async Task<bool> IsExist(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().AnyAsync(x=>x.Id == id, cancellationToken);
        }

        public async Task<int> Update(int id,T entity, CancellationToken cancellationToken)
        {
            entity.Id = id;
            _dbContext.Update(entity);
            return await SaveChanges(cancellationToken);
        }

        public async Task<int> SaveChanges(CancellationToken cancellation)
        {
            return await _dbContext.SaveChangesAsync(cancellation);
        }
    }
}
