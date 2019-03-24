namespace OpenDevBlog.Data.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    using OpenDevBlog.Models.Database.Base;

    public class GenericRepository<TEntity> :
        IGenericRepository<TEntity> where TEntity : class, IAuditInfo, IDeletableModel
    {
        private readonly IApplicationDbContext applicationDbContext;
        private readonly DbSet<TEntity> entities;

        public GenericRepository(IApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
            this.entities = applicationDbContext.Set<TEntity>();
        }

        public async Task<EntityEntry<TEntity>> AddAsync(TEntity entity) =>
            await this.entities.AddAsync(entity);

        public IQueryable<TEntity> GetAll() =>
            this.entities.AsNoTracking();

        public async Task<int> SaveChangesAsync() =>
            await this.applicationDbContext.SaveChangesAsync();

        public async Task<TEntity> GetAsync(params object[] keyValues) =>
            await this.entities.FindAsync(keyValues);

        public void HardDelete(TEntity entity) => 
            this.entities.Remove(entity);

        public void MarkAsDeleted(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            this.Update(entity);
        }

        public void Update(TEntity entity)
        {
            EntityEntry<TEntity> entry = this.applicationDbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Modified;
            }

            this.applicationDbContext.Attach(entity);
        }
    }
}
