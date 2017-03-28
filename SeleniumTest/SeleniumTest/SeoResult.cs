using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTest
{
    public class SeoResult
    {
        public bool IsOk { get; set; }
        public string Url { get; set; } 
        public string Message { get; set; } 
        public KeyValuePair<bool, string> TitleSuccess { get; set; }
        public KeyValuePair<bool, string> DescriptionSuccess { get; set; }
        public List<KeyValuePair<bool, string>> ImgTags { get; set; }
        public bool AreRobotsDisabled { get; set; }
    }
}
