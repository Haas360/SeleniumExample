using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTest
{
    public class ResultFormater
    {
        public static string CreateHtmlResult(List<SeoResult> results)
        {
           var stringBuilder = new StringBuilder();
           
            results.ForEach(result =>
            {
                stringBuilder.AppendLine($"<div class=\"result {GetClassForHead(result)}\">Page URL: {result.Url} <span class=\"expander\">Expand Result</span>");
                stringBuilder.AppendLine("<div class=\"hidden content\">");

                if (!result.IsOk && !string.IsNullOrEmpty(result.Message))
                {
                    stringBuilder.AppendLine($"<p>{result.Message}</p>");
                }
                else
                {
                    stringBuilder.AppendLine($"<span class=\"{GetClassForResult(result.TitleSuccess.Key)}\">Title is: {result.TitleSuccess.Value}</span>");
                    stringBuilder.AppendLine($"<span class=\"{GetClassForResult(result.DescriptionSuccess.Key)}\">Description is: {result.DescriptionSuccess.Value}</span>");
                    if (result.ImgTags.Any())
                    {
                        stringBuilder.AppendLine("<h3>Img tags result</h3>");
                        result.ImgTags.ForEach(imgTag =>
                        {
                            stringBuilder.AppendLine($"<span class=\"{GetClassForResult(imgTag.Key)}\">Url for this tag is: {imgTag.Value}</span>");
                        });
                    }
                }

                stringBuilder.AppendLine("</div>");
                stringBuilder.AppendLine("</div>");
            });
            return stringBuilder.ToString();
        }

        private static string GetClassForResult(bool wasSuccess)
        {
            return wasSuccess ? "green" : "red";
        }
        private static string GetClassForHead(SeoResult result)
        {
            if (!result.IsOk)
                return "red-text";
            if (result.AreRobotsDisabled)
                return "yellow-text";
            return "green-text";
        }
    }
}
