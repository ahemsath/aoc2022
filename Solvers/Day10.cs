namespace Solvers
{

    public class Day10 : ISolver
    {
        private string _inputFile;
        private char[,] _crt;

        public Day10(string inputFile)
        {
            _inputFile = inputFile;
            _crt = new char[40, 6];
        }

        public string Answer1()
        {
            int x = 1;

            int cycle = 1;

            int sumOfSignalStrengths = 0;

            foreach(var line in File.ReadLines(_inputFile))
            {
                var instruction = line.Substring(0, 4);

                if (instruction == "noop")
                {
                    sumOfSignalStrengths += MaybeSignalStrength(cycle, x);
                    UpdateCrt(cycle, x);
                    cycle++;
                }
                else if (instruction == "addx")
                {
                    var numToAdd = int.Parse(line.Substring(5));
                    sumOfSignalStrengths += MaybeSignalStrength(cycle, x);
                    UpdateCrt(cycle, x);
                    cycle++;
                    sumOfSignalStrengths += MaybeSignalStrength(cycle, x);
                    UpdateCrt(cycle, x);
                    cycle++;
                    x += numToAdd;
                }
            }

            return sumOfSignalStrengths.ToString();
        }

        public string Answer2()
        {
            for (var row = 0; row < _crt.GetLength(1); row++)
            {
                var outputLine = string.Empty;
                for (var col = 0; col < _crt.GetLength(0); col++)
                {
                    outputLine += _crt[col,row].ToString();
                }
                Console.WriteLine(outputLine);
            }
            return string.Empty;
        }

        private int MaybeSignalStrength(int cycle, int x)
        {
            if ((cycle - 20) % 40 == 0)
            {
                //Console.WriteLine($"Returning signal strength {cycle * x} for cycle {cycle}");
                return cycle * x;
            }
            return 0;
        }

        private void UpdateCrt(int cycle, int x)
        {
            var pixelRow = (cycle - 1) / 40;
            var pixelCol = (cycle - 1) % 40;

            if (Math.Abs(x - pixelCol) <= 1)
            {
                _crt[pixelCol, pixelRow] = '#';
            }
            else
            {
                _crt[pixelCol, pixelRow] = '.';
            }
        }

    }

}