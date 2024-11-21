namespace OpenDevBlog.Web.Models.Articles
{
    using System.ComponentModel.DataAnnotations;

    public class ArticleCreateModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Names { get; set; }
    }
}
