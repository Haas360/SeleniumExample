using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTest.TestHelpers
{
    public static class ListExtensions
    {
        public static List<List<T>> SplitList<T>(this List<T> listToSplit, int nSize = 3)
        {
            var resultList = new List<List<T>>();
            for (int i = 0; i < listToSplit.Count; i += nSize)
            {
                resultList.Add(listToSplit.GetRange(i, Math.Min(nSize, listToSplit.Count - i)));
            }
            return resultList;
        }
    }
}
