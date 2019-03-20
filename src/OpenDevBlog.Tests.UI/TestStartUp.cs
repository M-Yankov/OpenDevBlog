namespace OpenDevBlog.Tests.UI
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using OpenDevBlog.Data;
    using Web;

    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration)
            : base(configuration)
        {
        }

        public override void UseDatabase(IServiceCollection services)
        {
            IServiceProvider internalServiceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

            services.AddDbContext<ApplicationDbContext>(dataBase =>
            {
                dataBase.UseInMemoryDatabase("MemoryTestDb");
                dataBase.UseInternalServiceProvider(internalServiceProvider);
            });
        }

        public override void MigrateDataBase(IApplicationDbContext databaseContext)
        {
            databaseContext.EnsureCreated();
        }
    }
}
