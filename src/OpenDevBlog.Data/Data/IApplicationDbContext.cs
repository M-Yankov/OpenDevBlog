namespace OpenDevBlog.Data
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    using OpenDevBlog.Models.Database;

    public interface IApplicationDbContext
    {
        DbSet<ApplicationUser> Users { get; set; }

        DbSet<Article> Articles { get; set; }

        DbSet<CategoryArticle> CategoryArticles { get; set; }

        DbSet<Category> Categories { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class;

        Task MigrateAsync();

        Task EnsureCreatedAsync();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
