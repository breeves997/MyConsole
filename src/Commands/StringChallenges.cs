using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsole.Commands
{

    public static class StringChallenges
    {
        static List<string> NSPair = new List<string> { "NORTH", "SOUTH" };
        static List<string> EWPair = new List<string> { "EAST", "WEST" };
        //Method which takes in a pattern (e.g. abab) and a sentence and checks to see
        //if the sentence follows the same pattern
        public static string WordPattern(string pattern, string str)
        {
            // check if different number of pattern characters than words in str
            string[] words = str.Split(' ');
            if (pattern.Length != words.Length) return "No";

            // check if number of distinct words matches number of distinct letters
            int patternDistinctCharCount = pattern.Distinct().Count();
            if (words.Distinct().Count() != patternDistinctCharCount) return "No";

            // check that number of distinct groups matches number of distinct pattern chars
            int groupingCount = pattern.Zip(words, (x, y) => new KeyValuePair<char, string>(x, y)).Distinct().Count();
            if (patternDistinctCharCount != groupingCount) return "No";

            return "Yes";
        }

        public static bool isMerge(string s, string part1, string part2)
        {
            /*Console.WriteLine("target: " + s);
            Console.WriteLine("part 1: " + part1);
            Console.WriteLine("part 2: " + part2);*/

            bool empty1 = part1.Length == 0,
                 empty2 = part2.Length == 0,
                 works1 = false,
                 works2 = false;

            if (s.Length == 0)
            {
                if (part1.Length == 0 && part2.Length == 0) return true;
                return false;
            }
            else
            {
                if (!empty1 && s[0] == part1[0]) works1 = isMerge(s.Substring(1), part1.Substring(1), part2);
                if (!empty2 && s[0] == part2[0]) works2 = isMerge(s.Substring(1), part1, part2.Substring(1));
                return works1 || works2;
            }
        }



        public static string[] dirReduc(String[] arr)
        {
            List<string> directions = arr.ToList();
            List<string> input = new List<string>();
            for (int i = 0; i < directions.Count - 1; i++)
            {
                input = directions.GetRange(i, 2);
                if (input.OrderBy(s => s).SequenceEqual(EWPair.OrderBy(t => t)))
                {
                    directions.RemoveRange(i, 2);
                    directions = dirReduc(directions.ToArray()).ToList();
                }
                if (input.OrderBy(s => s).SequenceEqual(NSPair.OrderBy(t => t)))
                {
                    directions.RemoveRange(i, 2);
                    directions = dirReduc(directions.ToArray()).ToList(); ;
                }
            }
            return directions.ToArray();
        }

        public static String[] dirReducSolution(String[] arr)
        {
            Stack<String> stack = new Stack<String>();

            foreach (String direction in arr)
            {
                String lastElement = stack.Count > 0 ? stack.Peek().ToString() : null;

                switch (direction)
                {
                    case "NORTH": if ("SOUTH".Equals(lastElement)) { stack.Pop(); } else { stack.Push(direction); } break;
                    case "SOUTH": if ("NORTH".Equals(lastElement)) { stack.Pop(); } else { stack.Push(direction); } break;
                    case "EAST": if ("WEST".Equals(lastElement)) { stack.Pop(); } else { stack.Push(direction); } break;
                    case "WEST": if ("EAST".Equals(lastElement)) { stack.Pop(); } else { stack.Push(direction); } break;
                }
            }
            String[] result = stack.ToArray();
            Array.Reverse(result);

            return result;
        }
    }
}

