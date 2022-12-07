using System.Text.RegularExpressions;

namespace Solvers
{

    public class Day06 : ISolver
    {
        private string _inputFile;

        private LinkedList<char>[] crates = new LinkedList<char>[9];

        public Day06(string inputFile)
        {
            _inputFile = inputFile;
        }

        public string Answer1()
        {
            // input is a single line of chars
            string input = File.ReadAllText(_inputFile);
            int windowStart = 0;
            int windowEnd = 1;
            while (windowEnd < input.Length)
            {
                if (windowEnd - windowStart == 4)
                {
                    return windowEnd.ToString();
                }
                for (int i = windowEnd - 1; i >= windowStart; i--)
                {
                    if (input[i] == input[windowEnd])
                    {
                        // found a repeat, set window start to the first char after the index of the repeated char
                        windowStart = i + 1;
                        //windowEnd++;
                        break;
                    }
                }
                windowEnd++;
            }
            return string.Empty;
        }

        public string Answer2()
        {
            // input is a single line of chars
            string input = File.ReadAllText(_inputFile);
            int windowStart = 0;
            int windowEnd = 1;
            while (windowEnd < input.Length)
            {
                if (windowEnd - windowStart == 14)
                {
                    return windowEnd.ToString();
                }
                for (int i = windowEnd - 1; i >= windowStart; i--)
                {
                    if (input[i] == input[windowEnd])
                    {
                        // found a repeat, set window start to the first char after the index of the repeated char
                        windowStart = i + 1;
                        //windowEnd++;
                        break;
                    }
                }
                windowEnd++;
            }
            return string.Empty;
        }
    }

}