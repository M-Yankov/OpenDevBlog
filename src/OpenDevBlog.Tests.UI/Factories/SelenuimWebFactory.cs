using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace OpenDevBlog.Tests.UI.Factories
{
    public class SelenuimWebFactory<TClass> : WebApplicationFactory<TClass> where TClass : class
    {
        protected override IWebHostBuilder CreateWebHostBuilder() =>
             Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<TestStartUp>();

        protected override void ConfigureWebHost(IWebHostBuilder builder) =>
            builder.UseEnvironment(EnvironmentName.Development);
    }
}
