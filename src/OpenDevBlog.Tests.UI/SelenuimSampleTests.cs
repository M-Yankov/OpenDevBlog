namespace OpenDevBlog.Tests.UI
{
    using System;
    using System.IO;
    using System.Threading;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Internal;
    using Microsoft.Extensions.DependencyInjection;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;

    using Xunit;

    public class SelenuimSampleTests : IDisposable
    {
        private const string TestLocalHostUrl = "http://localhost:8080";

        private readonly CancellationTokenSource tokenSource;

        public SelenuimSampleTests()
        {
            this.tokenSource = new CancellationTokenSource();

            string projectName = typeof(Web.Startup).Assembly.GetName().Name;

            string currentDirectory = Directory.GetCurrentDirectory();
            string webProjectDirectory = Path.GetFullPath(Path.Combine(currentDirectory, $@"..\..\..\..\{projectName}"));

            IWebHost webHost = WebHost.CreateDefaultBuilder(new string[0])
                .UseSetting(WebHostDefaults.ApplicationKey, projectName)
                .UseContentRoot(webProjectDirectory) // This will make appsettings.json to work.
                .ConfigureServices(services =>
                {
                    services.AddSingleton(typeof(IStartup), serviceProvider =>
                    {
                        IHostingEnvironment hostingEnvironment = serviceProvider.GetRequiredService<IHostingEnvironment>();
                        StartupMethods startupMethods = StartupLoader.LoadMethods(
                            serviceProvider,
                            typeof(TestStartup),
                            hostingEnvironment.EnvironmentName);

                        return new ConventionBasedStartup(startupMethods);
                    });
                })
                .UseEnvironment(EnvironmentName.Development)
                .UseUrls(TestLocalHostUrl)
                //// .UseStartup<Web.TestStartUp>() // It's not working
                .Build();

            webHost.RunAsync(this.tokenSource.Token);
        }

        public void Dispose()
        {
            this.tokenSource.Cancel();
        }

        [Fact]
        public void TestWithSelenium()
        {
            string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string currentDirectory = Path.GetDirectoryName(assemblyLocation);

            using (ChromeDriver driver = new ChromeDriver(currentDirectory))
            {
                driver.Navigate().GoToUrl(TestLocalHostUrl);
                IWebElement webElement = driver.FindElementByCssSelector("a.navbar-brand");

                string expected = typeof(Web.Startup).Assembly.GetName().Name;

                Assert.Equal(expected, webElement.Text);

                string appSettingValue = driver.FindElementById("myvalue").Text;
                const string ExpectedAppSettingsValue = "44";
                Assert.Equal(ExpectedAppSettingsValue, appSettingValue);
            }
        }
    }
}
