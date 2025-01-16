using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class CompositRepository<ICompositEntity, TKey> : ICompositRepositoryBase<ICompositEntity, TKey>
        where ICompositEntity : class, ICompositEntity<TKey>
        where TKey : IComparable
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<ICompositEntity> _dbSet;

        public CompositRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<ICompositEntity>();
        }

        public async Task<ICompositEntity> GetByIdAsync(TKey id1, TKey id2)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.ItemId.Equals(id1) && e.WarehouseId.Equals(id2));
        }

        public async Task<IEnumerable<ICompositEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(ICompositEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddAsync(List<ICompositEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task EditAsync(ICompositEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task RemoveAsync(TKey id1, TKey id2)
        {
            var entity = await GetByIdAsync(id1, id2);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
    }
}