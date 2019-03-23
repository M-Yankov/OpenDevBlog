namespace OpenDevBlog.Data
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using OpenDevBlog.Models.Database;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<CategoryArticle> CategoryArticles { get; set; }

        public DbSet<Category> Categories { get; set; }

        public async Task MigrateAsync() =>
            await this.Database.MigrateAsync();

        public async Task EnsureCreatedAsync() =>
            await this.Database.EnsureCreatedAsync();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Article>()
                .HasOne(x => x.Author)
                .WithMany(x => x.Articles);

            builder.Entity<Article>()
                .HasOne(x => x.Reviewer)
                .WithMany(x => x.ReviewedArticles);

            builder.Entity<CategoryArticle>()
                .HasKey(x => new { x.ArticleId, x.CategoryId });
        }
    }
}
