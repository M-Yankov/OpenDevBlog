namespace OpenDevBlog.Tests.UI
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text.Encodings.Web;
    using System.Threading;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Internal;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;
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
                //.ConfigureServices(services =>
                //{
                //    services.AddSingleton(typeof(IStartup), serviceProvider =>
                //    {
                //        IHostingEnvironment hostingEnvironment = serviceProvider.GetRequiredService<IHostingEnvironment>();
                //        StartupMethods startupMethods = StartupLoader.LoadMethods(
                //            serviceProvider,
                //            typeof(TestStartup),
                //            hostingEnvironment.EnvironmentName);

                //        return new ConventionBasedStartup(startupMethods);
                //    });
                //})
                .UseEnvironment(EnvironmentName.Development)
                .UseUrls(TestLocalHostUrl)
                .UseStartup<TestStartup>() // It's not working
                .Build();

            this.webHost.StartAsync();//(this.tokenSource.Token);
        }

        public void Dispose()
        {
            this.tokenSource.Cancel();
            this.webHost.StopAsync();
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
                ReadOnlyCollection<IWebElement> articleElements = articlesContainer
                    .FindElements(By.ClassName("article-item"));

                IEnumerable<string> actualTitles = articleElements
                    .Select(e => e.FindElement(By.ClassName("article-title")).Text);

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
                driver.Navigate().GoToUrl(uriBuilder.Uri);

                driver.FindElementById("Input_Email").SendKeys(AdminEmail);
                driver.FindElementById("Input_Password").SendKeys(AdminPassword);

                driver.FindElementByCssSelector("[type=submit]").Click();

                UriBuilder reviewArticlesUrl = new UriBuilder(TestLocalHostUrl);
                reviewArticlesUrl.Path = "/Identity/Articles/Review";
                
                driver.Navigate().GoToUrl(reviewArticlesUrl.Uri);

                actualArticlesCount = driver.FindElementsByCssSelector("table tbody tr").Count;
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

        [Fact]
        public void ExpectToSaveMultilineArticleContent()
        {
            Type enumerableType = typeof(Enumerable);
            System.Reflection.MethodInfo[] methodInfos = enumerableType
                .GetMethods(
                    System.Reflection.BindingFlags.Static |
                    System.Reflection.BindingFlags.Public);

            TagBuilder container = new TagBuilder("div");
            container.AddCssClass("container");
            TagBuilder paragraph = new TagBuilder("p");
            paragraph.InnerHtml.Append($"{enumerableType.FullName} contains {methodInfos.Length} methods:");


            const string Email = "test@test.com";
            const string ArticleTitle = "Large article";
            using (ChromeDriver driver = new ChromeDriver(this.executingDirectorty))
            {
                driver.Manage().Window.Maximize();
                UriBuilder uriBuilder = new UriBuilder(TestLocalHostUrl);
                uriBuilder.Path = "/Articles/Create";
                driver.Navigate().GoToUrl(uriBuilder.Uri);

                IWebElement monacoEditor = driver.FindElementByCssSelector("#container textarea.inputarea");

                // webElement.Clear() not working here
                monacoEditor.SendKeys(Keys.Control + "a");
                monacoEditor.SendKeys(Keys.Backspace);

                monacoEditor.SendKeys(container.RenderStartTag().GetContent());
                monacoEditor.SendKeys(Keys.Enter);
                monacoEditor.SendKeys(paragraph.GetContent());
                monacoEditor.SendKeys(Keys.Escape + Keys.Enter);
                
                TagBuilder list = new TagBuilder("ul");
                monacoEditor.SendKeys(list.RenderStartTag().GetContent());
                monacoEditor.SendKeys(Keys.Enter);
                for (int i = 0; i < methodInfos.Length && i < 20; i++)
                {
                    TagBuilder listItem = new TagBuilder("li");
                    monacoEditor.SendKeys(listItem.RenderStartTag().GetContent());
                    monacoEditor.SendKeys(Keys.Enter);

                    TagBuilder span = new TagBuilder("span");
                    span.InnerHtml.Append(methodInfos[i].ReflectedType.Namespace);

                    listItem.InnerHtml.AppendHtml(span);
                    listItem.InnerHtml.Append($".{methodInfos[i].Name}");
                    IEnumerable<string> parameterTypes = methodInfos[i]
                        .GetParameters()
                        .Select(x => x.ParameterType.Name);

                    string methodParameters = string.Join(", ", parameterTypes);
                    listItem.InnerHtml.Append($"({methodParameters})");
                    monacoEditor.SendKeys(listItem.RenderBody().GetContent());
                    monacoEditor.SendKeys(Keys.Escape);
                    monacoEditor.SendKeys(Keys.Enter);
                    monacoEditor.SendKeys(Keys.Backspace);
                    monacoEditor.SendKeys(listItem.RenderEndTag().GetContent());

                    monacoEditor.SendKeys(Keys.Escape);
                    monacoEditor.SendKeys(Keys.Enter);
                    list.InnerHtml.AppendHtml(listItem);
                }

                container.InnerHtml.AppendHtml(paragraph);
                container.InnerHtml.AppendHtml(list);

                monacoEditor.SendKeys(list.RenderEndTag().GetContent());
                monacoEditor.SendKeys(Keys.Escape);
                monacoEditor.SendKeys(Keys.Enter);
                monacoEditor.SendKeys(container.RenderEndTag().GetContent());
                monacoEditor.SendKeys(Keys.Escape);
                monacoEditor.SendKeys(Keys.Enter);

                driver.FindElementByCssSelector($"[name={nameof(ArticleCreateModel.Title)}]").SendKeys(ArticleTitle);
                driver.FindElementByCssSelector($"[name={nameof(ArticleCreateModel.Email)}]").SendKeys(Email);
                driver.FindElementByCssSelector($"[name={nameof(ArticleCreateModel.Names)}]").SendKeys("Testing with selenium");

                driver.FindElementByCssSelector("form input[type=submit]").Click();
            }

            string fullHtml = container.GetContent();

            Models.Database.Article article = null;
            using (IServiceScope scope = this.webHost.Services.CreateScope())
            {
                article = scope.ServiceProvider
                    .GetService<IApplicationDbContext>().Articles.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Title == ArticleTitle)
                    .GetAwaiter()
                    .GetResult();
            }

            Assert.NotNull(article);
            string actualHtml = article.Content.Trim().Replace(Environment.NewLine, string.Empty);
            Assert.Equal(fullHtml, actualHtml);
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

    public static class IHtmlContentExtensions
    {
        public static string GetContent(this IHtmlContent htmlContent)
        {
            string html = string.Empty;
            using (StringWriter htmlWriter = new StringWriter())
            {
                htmlContent.WriteTo(htmlWriter, HtmlEncoder.Default);
                html = htmlWriter.ToString();
            }

            return html;
        }
    }
}
