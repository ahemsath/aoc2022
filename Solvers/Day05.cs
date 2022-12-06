using System.Text.RegularExpressions;

namespace Solvers
{

    public class Day05 : ISolver
    {
        private string _inputFile;

        private LinkedList<char>[] crates = new LinkedList<char>[9];

        public Day05(string inputFile)
        {
            _inputFile = inputFile;
        }

        public string Answer1()
        {
            for (int i = 0; i < crates.Length; i++)
            {
                crates[i] = new LinkedList<char>();
            }

            Regex re = new Regex(@"move\ (\d+)\ from\ (\d+)\ to\ (\d+)");

            foreach (string line in File.ReadLines(_inputFile))
            {
                if (line.Length == 0) continue;
                if (line.StartsWith(" 1")) continue;

                if (!line.StartsWith("move"))
                {
                    // Add line to crates
                    for (int i = 0; i < crates.Length; i++)
                    {
                        // column in line based on crate stack index: (i * 4) + 1
                        int column = (i * 4) + 1;

                        if (! " []".Contains(line[column]))
                        {
                            crates[i].AddFirst(line[column]);
                        }
                    }
                }

                Match m = re.Match(line);
                if (m.Success)
                {
                    Console.WriteLine(line);
                    int cratesToMove = int.Parse(m.Groups[1].Value);
                    int fromStack = int.Parse(m.Groups[2].Value);
                    int toStack = int.Parse(m.Groups[3].Value);

                    for (int i = 0; i < cratesToMove; i++)
                    {
                        char crate = crates[fromStack - 1].Last.Value;
                        crates[fromStack - 1].RemoveLast();
                        crates[toStack - 1].AddLast(crate);
                        Console.WriteLine($"Moved {crate} from {fromStack} to {toStack}");
                    }
                }
            }

            string answer = string.Empty;
            for (int i = 0; i < crates.Length; i++)
            {
                answer+= crates[i].Last.Value;
            }

            return answer;
        }

        public string Answer2()
        {
            return string.Empty;
        }


    }

}