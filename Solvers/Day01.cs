namespace Solvers
{
    public class Day01 : ISolver
    {
        private string _inputFile;

        public Day01(string inputFile)
        {
            _inputFile = inputFile;
        }

        public int Answer1()
        {
            int currentCalories = 0;
            int maxCalories = 0;
            foreach (string line in File.ReadLines(_inputFile))
            {
                if (line.Length == 0)
                {
                    maxCalories = Math.Max(maxCalories, currentCalories);
                    currentCalories = 0;
                }
                else
                {
                    if (int.TryParse(line, out var result))
                    {
                        currentCalories += result;
                    }
                }
            }
            return maxCalories;
        }

        public int Answer2()
        {
            return -1;
        }
    }
}