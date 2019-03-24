namespace OpenDevBlog.Models.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;

    using OpenDevBlog.Models.Database.Base;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableModel
    {
        public bool IsAnonymous { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<Article> Articles { get; set; } = new HashSet<Article>();

        public ICollection<Article> ReviewedArticles { get; set; } = new HashSet<Article>();

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
