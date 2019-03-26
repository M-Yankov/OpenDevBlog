namespace OpenDevBlog.Data.Data.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore.ChangeTracking;

    using OpenDevBlog.Models.Database.Base;

    public interface IGenericRepository<TEntity> where TEntity : class, IAuditInfo, IDeletableModel
    {
        Task<EntityEntry<TEntity>> AddAsync(TEntity entity);

        IQueryable<TEntity> GetAll();

        Task<TEntity> GetAsync(params object[] keyValues);

        void HardDelete(TEntity entity);

        void MarkAsDeleted(TEntity entity);

        Task<int> SaveChangesAsync();

        void Update(TEntity entity);
    }
}