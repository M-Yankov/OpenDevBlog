namespace OpenDevBlog.Data
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using OpenDevBlog.Models.Database;

    public interface IApplicationDbContext
    {
        DbSet<ApplicationUser> Users { get; set; }

        DbSet<Article> Articles { get; set; }

        DbSet<CategoryArticle> CategoryArticles { get; set; }

        DbSet<Category> Categories { get; set; }

        Task MigrateAsync();

        Task EnsureCreatedAsync();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
