using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenDevBlog.Web.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace OpenDevBlog.Tests.UI
{
    public class IntitialTests
    {
        private readonly TestServer server;
        private readonly HttpClient client;

        public IntitialTests()
        {
            string projectName = typeof(Web.Startup).Assembly.GetName().Name;
            
            string currentDirectory = Directory.GetCurrentDirectory();
            string webProjectDirectory = Path.GetFullPath(Path.Combine(currentDirectory, $@"..\..\..\..\{projectName}"));
            string appSettingsPath = Path.Combine(webProjectDirectory, "appSettings.json");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                 .AddJsonFile(appSettingsPath)
                 .Build();

            IWebHostBuilder builder = new WebHostBuilder()
                .UseConfiguration(configuration)
                .UseStartup<Web.Startup>();

            server = new TestServer(builder);
            client = server.CreateClient();
        }

        [Fact] 
        public async Task TesstMe()
        {
            var response = await client.GetAsync("/");
            response.EnsureSuccessStatusCode();
        }
    } 
    
}
