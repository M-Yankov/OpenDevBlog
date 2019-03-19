namespace OpenDevBlog.Web.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public void Migrate()
        {
            this.Database.Migrate();
        }

        public void EnsureCreated()
        {
            this.Database.EnsureCreated();
        }
    }
}
