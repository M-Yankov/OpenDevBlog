namespace OpenDevBlog.Models.Database.Base
{
    using System;

    public interface IDeletableModel
    {
        bool IsDeleted { get; set; }

        DateTime? DeletedOn { get; set; }
    }
}
