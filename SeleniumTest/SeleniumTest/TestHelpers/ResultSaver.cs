using System;
using System.IO;
using System.Linq;
using System.Text;

namespace SeleniumTest.TestHelpers
{
    public class ResultSaver
    {
        public void SaveResult(string seoResults)
        {
            var templatePath = @"C:\Repos\SeleniumExample\SeleniumTest\SeleniumTest\ResultTemplate.html";
            var sb = new StringBuilder();

            File.ReadAllLines(templatePath).ToList().ForEach(line =>
            {
                sb.AppendLine(line.Contains("***testResultPlaceholder****")
                    ? line.Replace("***testResultPlaceholder****", seoResults)
                    : line);
            });
            var dateForFile =
                $"{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}__{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}";
            using (var file = new StreamWriter(File.Create(@"C:\Repos\SeleniumExample\SeleniumTest\SeleniumTest\TestResults\TestReport" + dateForFile + ".html")))
            {
                file.Write(sb.ToString());
            }
        }
    }
}
