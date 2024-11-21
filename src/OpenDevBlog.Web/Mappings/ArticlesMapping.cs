namespace OpenDevBlog.Web.Mappings
{
    using Models.Articles;
    using OpenDevBlog.Services.Models;
     
    public static class ArticlesMapping
    {
        public static ArticleViewModel ToViewModel(this ArticleModel article) =>
            new ArticleViewModel()
            {
                CreatedBy = article.CreatedBy,
                CreatedOn = article.CreatedOn,
                Id = article.Id,
                Title = article.Title,
                Summary = article.Content
            };
    }
}
