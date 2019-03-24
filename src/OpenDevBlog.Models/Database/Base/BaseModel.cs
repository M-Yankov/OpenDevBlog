namespace OpenDevBlog.Models.Database.Base
{
    using System;

    public abstract class BaseModel<TKey> : IAuditInfo where TKey : struct 
    {
        public TKey Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
