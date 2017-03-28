using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace SeleniumTest
{
    [TestFixture]
    public class TestTest
    {
        [Test]
        public void Test()
        {
            //            System.Environment.SetEnvironmentVariable("webdriver.gecko.driver", "C:\\webdriver\\geckodriver.exe");

            using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl(new Uri("http://www.mitfristed.dk"));
            }
            
            

        }
    }
}
