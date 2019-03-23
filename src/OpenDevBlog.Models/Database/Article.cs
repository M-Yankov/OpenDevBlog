namespace OpenDevBlog.Models.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Enums;

    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(100_000)]
        public string Content { get; set; }

        [Required]
        public DateTime UpdatedOn { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }

        public string ReviewerId { get; set; }

        public ApplicationUser Reviewer { get; set; }

        public DateTime? ReviewDate { get; set; }

        public ArticleStatus Status { get; set; }

        public ICollection<CategoryArticle> Categories { get; set; } = new HashSet<CategoryArticle>();
    }
}
