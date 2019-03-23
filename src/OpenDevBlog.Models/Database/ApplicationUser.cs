namespace OpenDevBlog.Models.Database
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public bool IsAnonymous { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<Article> Articles { get; set; } = new HashSet<Article>();

        public ICollection<Article> ReviewedArticles { get; set; } = new HashSet<Article>();
    }
}
