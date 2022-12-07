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
            int pos = EndIndexOfNonRepeatingCharSequence(input, 4);
            return pos.ToString();
        }

        public string Answer2()
        {
            // input is a single line of chars
            string input = File.ReadAllText(_inputFile);
            int pos = EndIndexOfNonRepeatingCharSequence(input, 14);
            return pos.ToString();
        }

        private int EndIndexOfNonRepeatingCharSequence(string input, int sequenceLength)
        {
            // find a sequence of non-repeating chars of length 'sequenceLength' in the input string 'input'
            int windowStart = 0;
            int windowEnd = 1;
            while (windowEnd < input.Length)
            {
                if (windowEnd - windowStart == sequenceLength)
                {
                    return windowEnd;
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
            return -1;
        }
    }

}