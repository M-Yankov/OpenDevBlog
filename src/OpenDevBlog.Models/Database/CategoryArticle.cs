namespace OpenDevBlog.Models.Database
{
    using OpenDevBlog.Models.Database.Base;

    public class CategoryArticle : BaseDeletableModel<int>
    {
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int ArticleId { get; set; }

        public Article Article { get; set; }
    }
}
