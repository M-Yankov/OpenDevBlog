namespace OpenDevBlog.Models.Database.Base
{
    using System;

    public abstract class BaseDeletableModel<TKey> :
        BaseModel<TKey>, IAuditInfo, IDeletableModel where TKey : struct 
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
