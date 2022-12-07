using System.Collections;

namespace Solvers
{

    public class Day03 : ISolver
    {
        private string _inputFile;

        public Day03(string inputFile)
        {
            _inputFile = inputFile;
        }

        public string Answer1()
        {
            int totalPriority = 0;
            foreach (string line in File.ReadLines(_inputFile))
            {
                Hashtable charsInFirstHalf = new Hashtable(); 
                string firstHalf = line.Substring(0, line.Length / 2);
                string secondHalf = line.Substring(line.Length / 2, line.Length / 2);

                if (line != firstHalf + secondHalf)
                {
                    throw new Exception("oops!");
                }

                Console.WriteLine($"firstHalf={firstHalf}, secondHalf={secondHalf}");

                foreach(char c in firstHalf)
                {
                    charsInFirstHalf[c] = c;
                }
                foreach(char c in secondHalf)
                {
                    if (charsInFirstHalf.ContainsKey(c))
                    {
                        int priorityForChar = GetPriorityForChar(c);

                        Console.WriteLine($"Found common item {c} with priority {priorityForChar}");
                        totalPriority += priorityForChar;
                        break;
                    }
                }

            }
            return totalPriority.ToString();
        }

        public string Answer2()
        {
            int totalPriority = 0;
            int lineNum = 1;
            string line1 = string.Empty;
            Hashtable commonChars = new Hashtable();
            foreach (string line in File.ReadLines(_inputFile))
            {
                if (lineNum % 3 == 1)
                {
                    line1 = line;
                }
                else if (lineNum % 3 == 2)
                {
                    foreach(char c in line)
                    {
                        if (line1.Contains(c))
                        {
                            commonChars[c] = c;
                        }
                    }
                }
                else // must be the third line
                {
                    foreach(char c in line)
                    {
                        if (commonChars.ContainsKey(c))
                        {
                            // found it
                            totalPriority += GetPriorityForChar(c);
                            commonChars.Clear();
                            break;
                        }
                    }
                }
                lineNum++;
            }
            return totalPriority.ToString();
        }

        private int GetPriorityForChar(char c)
        {
            int charAsInt = (int)c;
            if (charAsInt >= 97)
            {
                // lower case letters
                return charAsInt - 96;
            }
            // otherwise we're in capital latters that start at 65
            return charAsInt - 64 + 26;
        }


    }
}