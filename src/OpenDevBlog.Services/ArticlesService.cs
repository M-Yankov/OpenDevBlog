namespace OpenDevBlog.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Models;

    using OpenDevBlog.Data.Data.Repositories;
    using OpenDevBlog.Models.Database;
    using OpenDevBlog.Models.Enums;

    public class ArticlesService
    {
        private readonly IGenericRepository<Article> articlesRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private const string AnonymousUsernamePrefix = "anonymous";

        public ArticlesService(
            IGenericRepository<Article> articlesRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.articlesRepository = articlesRepository;
            this.userManager = userManager;
        }

        public async Task CreateAsync(string title, string htmlContent, string names, string email)
        {
            string authorUsername = $"{AnonymousUsernamePrefix}-{email}";
            ApplicationUser author = await this.userManager
                .FindByNameAsync(authorUsername);

            if (author == null)
            {
                author = new ApplicationUser()
                {
                    Email = authorUsername,
                    IsAnonymous = true,
                    UserName = authorUsername,
                    Name = names
                };

                IdentityResult userCreatonReuslt = await this.userManager.CreateAsync(author);
                if (!userCreatonReuslt.Succeeded)
                {
                    IEnumerable<string> errors = userCreatonReuslt
                        .Errors
                        .Select(x => x.Description);

                    string errorMessage = string.Join(", ", errors);
                    throw new InvalidOperationException($"{errorMessage}");
                }
            }

            Article newArticle = new Article()
            {
                Title = title,
                Content = htmlContent,
                Status = ArticleStatus.Pending,
                AuthorId = author.Id
            };

            await this.articlesRepository.AddAsync(newArticle);
            await this.articlesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ArticleModel>> GetLatestArticlesAsync() => 
            await this.articlesRepository
                .GetAll()
                .Include(x => x.Author)
                .Where(x => x.Status == ArticleStatus.Approved)
                .Select(x => new ArticleModel()
                {
                    Content = x.Content,
                    CreatedBy = x.Author.Name,
                    Id = x.Id,
                    CreatedOn = x.CreatedOn,
                    Title = x.Title
                })
                .Take(20)
                .ToListAsync();
    }
}
