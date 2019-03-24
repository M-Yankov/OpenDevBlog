namespace OpenDevBlog.Tests.UI
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using OpenDevBlog.Data;
    using OpenDevBlog.Models.Database;
    using OpenDevBlog.Web;

    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration)
            : base(configuration)
        {
        }

        protected override void UseDatabase(IServiceCollection services)
        {
            IServiceProvider internalServiceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

            services.AddDbContext<ApplicationDbContext>(dataBase =>
            {
                dataBase.UseInMemoryDatabase("MemoryTestDb");
                dataBase.UseInternalServiceProvider(internalServiceProvider);
            });
        }

        protected override async Task MigrateDataBaseAsync(IApplicationDbContext databaseContext)
        {
            await databaseContext.EnsureCreatedAsync();

            ApplicationUser admin = new ApplicationUser();
            admin.Email = "admin@opendevblog.com";
            admin.UserName = "admin";
            admin.Name = "Admin author";

            ApplicationUser anonymouseAuthor = new ApplicationUser();
            anonymouseAuthor.Email = "sergio@mail.it";
            anonymouseAuthor.UserName = "sergio@mail.it";
            anonymouseAuthor.Name = "Sergio Augusto";
            anonymouseAuthor.IsAnonymous = true;

            Article article = new Article();
            article.Title = "Approved Article;";
            article.Content = "<div> Hello World </div>";
            article.CreatedOn = DateTime.UtcNow.AddDays(-2);
            article.ModifiedOn = article.CreatedOn;
            article.Status = Models.Enums.ArticleStatus.Approved;
            article.ReviewDate = article.CreatedOn.AddDays(1);
            article.ReviewerId = admin.Id;
            article.Reviewer = admin;
            article.Author = anonymouseAuthor;

            Article article2 = new Article();
            article2.Title = "Undefined Test not working";
            article2.Content = "<div> How to Make a ....? </div>";
            article2.CreatedOn = DateTime.UtcNow.AddDays(-2);
            article2.ModifiedOn = article2.CreatedOn;
            article2.Status = Models.Enums.ArticleStatus.Pending;
            article2.ReviewDate = article2.CreatedOn.AddDays(1);
            article2.Author = anonymouseAuthor;

            await databaseContext.Articles.AddRangeAsync(article, article2);
            await databaseContext.SaveChangesAsync();
        }
    }
}
