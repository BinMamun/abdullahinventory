using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface ICompositRepositoryBase<ICompositEntity, TKey>
        where ICompositEntity : class, ICompositEntity<TKey> 
        where TKey : IComparable
    {
        Task<ICompositEntity> GetByIdAsync(TKey id1, TKey id2);
        Task<IEnumerable<ICompositEntity>> GetAllAsync();
        Task AddAsync(List<ICompositEntity> entities);
        Task AddAsync(ICompositEntity entity);
        Task EditAsync(ICompositEntity entity);
        Task RemoveAsync(TKey id1, TKey id2);
    }
}
