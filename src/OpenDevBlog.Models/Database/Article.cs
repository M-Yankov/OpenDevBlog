namespace OpenDevBlog.Models.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Enums;
    using OpenDevBlog.Models.Database.Base;

    public class Article : BaseDeletableModel<int>, IAuditInfo, IDeletableModel
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(100_000)]
        public string Content { get; set; }

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
