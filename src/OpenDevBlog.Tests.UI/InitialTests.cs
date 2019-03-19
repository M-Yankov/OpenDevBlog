namespace OpenDevBlog.Tests.UI
{
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Xunit;

    /// <summary>
    /// This class uses in memory server (not accessible over HTTP).
    /// </summary>
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

            this.server = new TestServer(builder);
            this.client = this.server.CreateClient();
        }

        [Fact]
        public async Task HomePageTest()
        {
            HttpResponseMessage response = await this.client.GetAsync("/");
            response.EnsureSuccessStatusCode();
        }
    }
}
