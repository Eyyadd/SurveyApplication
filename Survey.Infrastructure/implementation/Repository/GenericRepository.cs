using Microsoft.EntityFrameworkCore;
using Survey.Domain.Entities;
using Survey.Domain.Interfaces.IRepository;
using Survey.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.implementation.Repository
{
    public class GenericRepository<T>(SurveyDbContext dbContext) : IGenericRepository<T> where T : BaseEntity
    {
        private readonly SurveyDbContext _dbContext = dbContext;
        public async Task<int> Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<int> Delete(T entity)
        {
            _dbContext.Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbContext.Set<T>().FirstAsync(x=>x.Id == id);
        }

        public async Task<bool> IsExist(int id)
        {
            return await _dbContext.Set<T>().AnyAsync(x=>x.Id == id);
        }

        public async Task<int> Update(int id,T entity)
        {
            entity.Id = id;
            _dbContext.Update(entity);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
