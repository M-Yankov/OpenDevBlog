namespace OpenDevBlog.Tests.UI
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Internal;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using OpenDevBlog.Data;
    using OpenDevBlog.Web.Models.Articles;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;

    using Xunit;

    public class SelenuimSampleTests : IDisposable
    {
        private const string TestLocalHostUrl = "http://localhost:8080";

        private readonly CancellationTokenSource tokenSource;

        private readonly string executingDirectorty;
        private readonly IWebHost webHost;
        public SelenuimSampleTests()
        {
            this.tokenSource = new CancellationTokenSource();

            string projectName = typeof(Web.Startup).Assembly.GetName().Name;

            this.executingDirectorty = Directory.GetCurrentDirectory();
            string webProjectDirectory = Path.GetFullPath(Path.Combine(this.executingDirectorty, $@"..\..\..\..\{projectName}"));

            this.webHost = WebHost.CreateDefaultBuilder(new string[0])
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

            this.webHost.RunAsync(this.tokenSource.Token);
        }

        public void Dispose()
        {
            this.tokenSource.Cancel();
            this.webHost.Dispose();
        }

        [Fact]
        public void TestWithSelenium()
        {
            using (ChromeDriver driver = new ChromeDriver(this.executingDirectorty))
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

        [Fact]
        public void ExpectToCreateASimpleArticle()
        {
            const string expectedTest = "Selenium";
            using (ChromeDriver driver = new ChromeDriver(this.executingDirectorty))
            {
                UriBuilder uriBuilder = new UriBuilder(TestLocalHostUrl);
                uriBuilder.Path = "/Articles/Create";

                driver.Navigate().GoToUrl(uriBuilder.ToString());
                IWebElement webElement = driver.FindElementByCssSelector("#container textarea.inputarea");

                // webElement.Clear() not working here
                webElement.SendKeys(Keys.Control + "a");
                webElement.SendKeys(Keys.Backspace);

                webElement.SendKeys($"<div> {expectedTest} </div>");
                driver.FindElementById("btn-preview").Click();

                var webElement1 = driver.FindElementByTagName("body");
                bool test1 = webElement1.Enabled;
                string preview = driver.FindElementByCssSelector("#preview").Text;

                Assert.Equal(expectedTest, preview);

                using (new NoReloadVerificationContext(driver))
                {
                    driver.FindElementByCssSelector("form input[type=submit]").Click();
                }
                
                bool test = webElement1.Enabled;
                const string Email = "test@test.com";
                const string ArticleTitle = "Sample Selenium title";
                driver.FindElementByCssSelector($"[name={nameof(ArticleCreateModel.Title)}]").SendKeys(ArticleTitle);
                driver.FindElementByCssSelector($"[name={nameof(ArticleCreateModel.Email)}]").SendKeys(Email);
                driver.FindElementByCssSelector($"[name={nameof(ArticleCreateModel.Names)}]").SendKeys("Testing with selenium");

                driver.FindElementByCssSelector("form input[type=submit]").Click();

                OpenDevBlog.Models.Database.ApplicationUser user;
                using (IServiceScope scope = this.webHost.Services.CreateScope())
                {
                    user = scope.ServiceProvider
                        .GetService<IApplicationDbContext>().Users.Include(x => x.Articles).AsNoTracking()
                        .FirstOrDefaultAsync(x => x.UserName == $"anonymous-{Email}")
                        .GetAwaiter()
                        .GetResult();
                }

                const string expectedToEndsWith = "/Articles/Success";
                Assert.EndsWith(expectedToEndsWith, driver.Url);

                Assert.NotNull(user);
                Assert.EndsWith(Email, user.UserName);
                Assert.NotNull(user.Articles);
                Models.Database.Article article = user.Articles.FirstOrDefault();
                Assert.Equal(ArticleTitle, article.Title);
                Assert.Equal(Models.Enums.ArticleStatus.Pending, article.Status);
            }
        }

        [Fact]
        public void ExpectToShowArticles()
        {
            using (ChromeDriver driver = new ChromeDriver(this.executingDirectorty))
            {
                UriBuilder uriBuilder = new UriBuilder(TestLocalHostUrl);
                uriBuilder.Path = "/Articles";
                driver.Navigate().GoToUrl(uriBuilder.ToString());

                IWebElement articlesContainer = driver.FindElementByClassName("articles-list");
                ReadOnlyCollection<IWebElement> articleElements = articlesContainer.FindElements(By.ClassName("article-item"));

                IEnumerable<string> actualTitles = articleElements.Select(e =>
                         e.FindElement(By.ClassName("article-title")).Text);

                int acutualCount = articleElements.Count;

                const int TemporaryDefaultPageSize = 20;
                int expectedArticlesCount;
                IEnumerable<string> expectedAricleTitles;
                using (IServiceScope scope = this.webHost.Services.CreateScope())
                {
                    IQueryable<OpenDevBlog.Models.Database.Article> articlesQuery = scope.ServiceProvider
                        .GetService<IApplicationDbContext>().Articles.AsNoTracking()
                        .Where(x => x.Status == Models.Enums.ArticleStatus.Approved)
                        .OrderByDescending(x => x.CreatedOn)
                        .Take(TemporaryDefaultPageSize);

                    expectedArticlesCount = articlesQuery.Count();

                    expectedAricleTitles = articlesQuery.Select(x => x.Title).ToList();
                }

                Assert.Equal(expectedAricleTitles, actualTitles);
                Assert.Equal(expectedArticlesCount, acutualCount);
            }
        }

        [Fact]
        public void ExpectToAccessArticlesForReviewPage()
        {
            const string AdminEmail = "admin2@opendevblog.com";
            const string AdminPassword = "password";
            const string RoleName = "Moderator";

            using (IServiceScope scope = this.webHost.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                RoleManager<IdentityRole> roleManager = scope.ServiceProvider
                    .GetRequiredService<RoleManager<IdentityRole>>();

                roleManager.CreateAsync(new IdentityRole(RoleName))
                    .GetAwaiter()
                    .GetResult()
                    .VerifyIdentityResult();

                UserManager<Models.Database.ApplicationUser> usm = scope.ServiceProvider
                    .GetService<UserManager<Models.Database.ApplicationUser>>();

                Models.Database.ApplicationUser moderator = new Models.Database.ApplicationUser() { Email = AdminEmail, UserName = AdminEmail };

                usm.CreateAsync(moderator, AdminPassword)
                    .GetAwaiter()
                    .GetResult()
                    .VerifyIdentityResult();

                usm.AddToRoleAsync(moderator, RoleName)
                    .GetAwaiter()
                    .GetResult()
                    .VerifyIdentityResult();
            }

            int actualArticlesCount;
            int expectedArticlesCount;
            using (ChromeDriver driver = new ChromeDriver(this.executingDirectorty))
            {
                UriBuilder uriBuilder = new UriBuilder(TestLocalHostUrl);
                uriBuilder.Path = "/Identity/Account/Login";
                driver.Navigate().GoToUrl(uriBuilder.ToString());

                driver.FindElementById("Input_Email").SendKeys(AdminEmail);
                driver.FindElementById("Input_Password").SendKeys(AdminPassword);

                driver.FindElementByCssSelector("[type=submit]").Click();

                driver.Navigate().GoToUrl("/Identity/Account/Articles/Review");

                actualArticlesCount = driver.FindElementsByCssSelector("#articles-for-review tr").Count;
            }

            using (IServiceScope scope = this.webHost.Services.CreateScope())
            {
                IApplicationDbContext applicationDbContext = scope.ServiceProvider
                    .GetService<IApplicationDbContext>();

                expectedArticlesCount = applicationDbContext.Articles.AsNoTracking()
                    .Count(x => x.Status == Models.Enums.ArticleStatus.Pending);
            }

            Assert.Equal(expectedArticlesCount, actualArticlesCount);
        }
    }

    public static class IdentityResultExtensions
    {
        public static void VerifyIdentityResult(this IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(x => x.Description)));
            }
        }
    }

    public class NoReloadVerificationContext : IDisposable
    {
        private readonly IWebElement bodyElement;
        public NoReloadVerificationContext(IWebDriver webDriver)
        {
            this.bodyElement = webDriver.FindElement(By.TagName("body"));
            
        }

        public void Dispose() => MyAssert.DoesNotThrow(() => this.bodyElement.Enabled);
    }

    public class MyAssert : Assert
    {
        public static void DoesNotThrow(Func<object> func)
        {
            func();
        }

        public static void DoesNotThrow(Action action)
        {
            action();
        }
    }
}
