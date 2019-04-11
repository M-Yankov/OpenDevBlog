namespace OpenDevBlog.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using OpenDevBlog.Services;
    using OpenDevBlog.Web.Models.Articles;

    public class ArticlesController : Controller
    {
        private readonly ArticlesService articlesService;

        public ArticlesController(ArticlesService articlesService) => this.articlesService = articlesService;

        [HttpGet]
        public IActionResult Create() => this.View();

        [HttpPost]
        public async Task<IActionResult> Create(ArticleCreateModel article)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(article);
            }

            await this.articlesService.Create(article.Title,
                 article.Content,
                 article.Names,
                 article.Email);

            return this.Redirect("Success");
        }

        [HttpGet]
        public IActionResult Success() => this.View();
    }
}