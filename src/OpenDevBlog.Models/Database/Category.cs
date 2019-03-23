namespace OpenDevBlog.Models.Database
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Category
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<CategoryArticle> Articles { get; set; } = new HashSet<CategoryArticle>();
    }
}
