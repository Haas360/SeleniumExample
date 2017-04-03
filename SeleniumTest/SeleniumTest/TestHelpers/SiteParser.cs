using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
// ReSharper disable AccessToDisposedClosure

namespace SeleniumTest.TestHelpers
{
    public class PageParser
    {
        public Task<List<SeoResult>> GetSeoInformation(List<string> listOfUrls)
        {
            return Task.Run(() =>
            {
                var resultsList = new List<SeoResult>();
                using (IWebDriver driver = new FirefoxDriver())
                {
                    listOfUrls.ForEach(urlToCheck =>
                    {
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
                            var imagesDictionary = imgOnPages
                                .Select(x => new KeyValuePair<bool, string>(!string.IsNullOrEmpty(x.GetAttribute("alt")),
                                    x.GetAttribute("src") + "   Alt: " + x.GetAttribute("alt"))).ToList();

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
                            if (areRobotsDisabled)
                                result.IsOk = true;
                            resultsList.Add(result);
                        }
                        catch (Exception ex)
                        {
                            resultsList.Add(new SeoResult { Url = urlToCheck, IsOk = false, Message = ex.Message });
                        }
                        
                    });
                    return resultsList;
                }
            });
            
        }
    }
}
