namespace OpenDevBlog.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using OpenDevBlog.Models.Database;
    using OpenDevBlog.Models.Database.Base;

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

        public override async Task<int> SaveChangesAsync
            (CancellationToken cancellationToken = default)
        {
            this.UpdateDatesBeforeSaveEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }

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

            builder.Entity<Article>()
                .HasKey(x => x.Id);

            builder.Entity<Category>()
                .HasKey(x => x.Id);
        }

        private void UpdateDatesBeforeSaveEntities()
        {
            IDictionary<EntityState, IAuditInfo> entriesForUpdate = this.ChangeTracker
               .Entries()
               .Where(x => x.Entity is IAuditInfo
                   && (x.State == EntityState.Modified || x.State == EntityState.Added))
               .ToDictionary(x => x.State, x => (IAuditInfo)x);

            foreach ((EntityState state, IAuditInfo entity) in entriesForUpdate)
            {
                if (state == EntityState.Added)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
