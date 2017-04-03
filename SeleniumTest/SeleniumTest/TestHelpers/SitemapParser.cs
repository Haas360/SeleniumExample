using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace SeleniumTest.TestHelpers
{
    public class SitemapParser
    {
        public List<string> GetListOfUrls(Uri sitemapUrl)
        {
            var listOfUrls = new List<string>();
            using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl(new Uri("https://www.nozebra.dk/sitemap.xml"));
                var xml = driver.PageSource;

                var doc = new XmlDocument();
                doc.LoadXml(xml);
                var locations = doc.GetElementsByTagName("loc");

                foreach (XmlNode location in locations)
                {
                    listOfUrls.Add(location.FirstChild.Value);
                }
            }
            return listOfUrls;
        }
    }
}
