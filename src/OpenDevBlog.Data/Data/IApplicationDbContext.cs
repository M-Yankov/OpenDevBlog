namespace OpenDevBlog.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public interface IApplicationDbContext
    {
        DbSet<IdentityUser> Users { get; set; }

        void Migrate();

        void EnsureCreated();
    }
}
