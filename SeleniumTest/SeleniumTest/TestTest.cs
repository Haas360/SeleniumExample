using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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
            var listOfUrls = new List<string>();
            var resultsList = new List<SeoResult>();
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
            using (IWebDriver driver = new FirefoxDriver())
            {
                listOfUrls.ForEach(urlToCheck =>
                {
//                    var task = (HttpWebRequest)WebRequest.Create(urlToCheck);
//                    var taskresponse = (HttpWebResponse)task.GetResponse();
//                    if (taskresponse.StatusCode == HttpStatusCode.NotFound)
//                    {
//                        resultsList.Add(new SeoResult { Url = urlToCheck, IsOk = false, Message = "Page return 404" });
//                    }

                    driver.Navigate().GoToUrl(new Uri(urlToCheck));
                    try
                    {
                        var head = driver.FindElement(By.TagName("head"));
                        var metaTags = head.FindElements(By.TagName("meta"));
                        var desc = metaTags.FirstOrDefault(x => x.GetAttribute("name") == "description")?.GetAttribute("content");
                        var robotsMetaTags = metaTags.Where(x => x.GetAttribute("name") == "robots").ToList();
                        var areRobotsDisabled = robotsMetaTags.Any()
                                && robotsMetaTags
                                    .Select(x => x.GetAttribute("content").ToLower())
                                    .Any(x => x.Contains("noindex") && x.Contains("nofollow"));
                        var title = head.FindElement(By.TagName("title")).Text;
                        var body = driver.FindElement(By.TagName("body"));
                        var imgOnPages = body.FindElements(By.TagName("img"));
                        var imagesDictionary = imgOnPages.Select(x => new KeyValuePair<bool, string>(!string.IsNullOrEmpty(x.GetAttribute("alt")), x.GetAttribute("src") + "   Alt: " + x.GetAttribute("alt"))).ToList();

                        var result = new SeoResult
                        {
                            AreRobotsDisabled = areRobotsDisabled,
                            DescriptionSuccess = new KeyValuePair<bool, string>(!string.IsNullOrEmpty(desc), desc),
                            TitleSuccess = new KeyValuePair<bool, string>(!string.IsNullOrEmpty(title), title),
                            Url = urlToCheck,
                            ImgTags = imagesDictionary
                        };
                        result.IsOk = result.DescriptionSuccess.Key
                                      && result.TitleSuccess.Key;
                        result.IsOk = !result.ImgTags.Any()
                            ? result.IsOk
                            : result.IsOk && result.ImgTags.Select(x => x.Key).All(x => x);
                                   
                        resultsList.Add(result);
                    }
                    catch (Exception ex)
                    {

                        resultsList.Add(new SeoResult { Url = urlToCheck, IsOk = false, Message = ex.Message });
                    }
                });
                var resultHtml = ResultFormater.CreateHtmlResult(resultsList);
                //to separate
                var templatePath = @"C:\Repos\SeleniumExample\SeleniumTest\SeleniumTest\ResultTemplate.html";
                var sb = new StringBuilder();
                foreach (string line in File.ReadAllLines(templatePath))
                {
                    if (line.Contains("***testResultPlaceholder****"))
                        sb.AppendLine(line.Replace("***testResultPlaceholder****", resultHtml));
                    else
                        sb.AppendLine(line);
                }
                using (var file = new StreamWriter(File.Create(@"C:\Repos\SeleniumExample\SeleniumTest\SeleniumTest\TestResults\TestReport" + DateTime.UtcNow.Millisecond + ".html")))
                {
                    file.Write(sb.ToString());
                }
                var pp = 0;


            }

        }
    }
}
