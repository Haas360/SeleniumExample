using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SeleniumTest.TestHelpers;

namespace SeleniumTest
{
    [TestFixture]
    public class TestTest
    {
        [Test]
        public async Task Test()
        {
            //Get all urls from sitemap
            var sitemapParser = new SitemapParser();
            var sitemapUrl = new Uri(""); //Put URL to sitemap here
            var listOfUrls = sitemapParser.GetListOfUrls(sitemapUrl);
            var resultsList = new List<SeoResult>();

            //Parse all page from sitemap, chack SEO
            var pageParser = new PageParser();
            var splitedUrlList = listOfUrls.SplitList(8);
            var splitedTasks = new List<Task<List<SeoResult>>>();
            splitedUrlList.ForEach(urls =>
            {
                splitedTasks.Add(pageParser.GetSeoInformation(urls));
            });
            await Task.WhenAll(splitedTasks);
            splitedTasks.ForEach(tasks =>
            {
                resultsList.AddRange(tasks.Result);
            });
            //Generate Report
            var resultHtml = ResultFormater.CreateHtmlResult(resultsList);
            var resultSaver = new ResultSaver();
            resultSaver.SaveResult(resultHtml);
           
            Assert.IsTrue(resultsList.All(x=>x.IsOk));
        }
    }
}
