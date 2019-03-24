namespace OpenDevBlog.Models.Database
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using OpenDevBlog.Models.Database.Base;

    public class Category : BaseDeletableModel<int>
    {
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<CategoryArticle> Articles { get; set; } = new HashSet<CategoryArticle>();
    }
}
