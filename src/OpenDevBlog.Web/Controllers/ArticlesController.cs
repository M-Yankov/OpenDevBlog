namespace OpenDevBlog.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using OpenDevBlog.Models.Enums;
    using OpenDevBlog.Services;
    using OpenDevBlog.Web.Mappings;
    using OpenDevBlog.Web.Models.Articles;

    public class ArticlesController : Controller
    {
        private readonly ArticlesService articlesService;

        public ArticlesController(ArticlesService articlesService) =>
            this.articlesService = articlesService;

        [HttpGet]
        public async Task<IActionResult> Index() =>
           this.View((await this.articlesService.GetAll(ArticleStatus.Approved))
                .Select(x => x.ToViewModel()));

        [HttpGet]
        public IActionResult Create() => this.View();

        [HttpPost]
        public async Task<IActionResult> Create(ArticleCreateModel article)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(article);
            }

            await this.articlesService.CreateAsync(
                 article.Title,
                 article.Content,
                 article.Names,
                 article.Email);

            return this.Redirect("Success");
        }

        [HttpGet]
        public IActionResult Success() => this.View();

        [HttpGet]
        public IActionResult Details(int id) => this.View();

        [HttpGet]
        [Area("Identity")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Review() =>
            this.View((await this.articlesService.GetAll(ArticleStatus.Pending))
                .Select(x => x.ToViewModel()));
    }
}