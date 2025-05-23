using Exercices.Exo01.Entities;

namespace Exercices.Exo01.Repositories
{
    public interface IRepository<TEntity, TKey> where TEntity : class where TKey : struct
    {
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateByIdAsync(TKey id, TEntity entity);
        Task<bool> DeleteByIdAsync(TKey id);
        Task<AppUser?> GetUserByEmail(string email);
    }
    {
    }
}
