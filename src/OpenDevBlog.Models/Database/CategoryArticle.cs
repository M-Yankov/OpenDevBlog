namespace OpenDevBlog.Models.Database
{
    using System.ComponentModel.DataAnnotations;

    public class CategoryArticle
    {
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int ArticleId { get; set; }

        public Article Article { get; set; }
    }
}
