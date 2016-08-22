using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsole
{
    public static class ToolShed
    {
        //Gets indicies of all items in a string array that match a certain position
        public static List<int> GetIndiciesOfMatchingItems(string[] words, int loc)
        {
            return Enumerable.Range(0, words.Count())
                  .Where(i => words[i] == words[loc])
                  .ToList();
        }

        public static string ArrToStr(long[] arr)
        {
            return "[" + string.Join(", ", from i in arr select i.ToString()) + "]";
        }
    }
}
