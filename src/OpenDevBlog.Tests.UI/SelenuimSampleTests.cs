using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OpenDevBlog.Tests.UI
{
    using System.Net.Http;
    using Factories;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium;
    using System.IO;

    public class SelenuimSampleTests : IClassFixture<SelenuimWebFactory<TestStartUp>>
    {
        private readonly SelenuimWebFactory<TestStartUp> applicationFactory;

        public SelenuimSampleTests(SelenuimWebFactory<TestStartUp> applicationFactory)
        {
            this.applicationFactory = applicationFactory;
            HttpClient client = this.applicationFactory.CreateClient();

            string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string currentDirectory = Path.GetDirectoryName(assemblyLocation);

            using (ChromeDriver driver = new ChromeDriver(currentDirectory))
            {
                // NOT WORKING, BEACUSE THE SERVER IS NOT ACCESSIBLE OVER HTTP :x
                driver.Navigate().GoToUrl(client.BaseAddress);
                IWebElement webElement = driver.FindElementByCssSelector("a.navbar-brand");
                string expected = typeof(Web.Startup).Assembly.GetName().Name;

                Assert.Equal(expected, webElement.Text);
            }
        }

        [Fact]
        public void TestWithSelenium()
        {
            Assert.True(true);
        }
    }
}
