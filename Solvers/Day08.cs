namespace Solvers
{

    public class Day08 : ISolver
    {
        private string _inputFile;

        private int[,] _treeMap;

        public Day08(string inputFile)
        {
            _inputFile = inputFile;
            // Get dimensions of input and initialize map structure
            _treeMap = GetTreeMap(_inputFile);
        }

        public string Answer1()
        {
            // Looking for the number of trees visible from the outside of the grid
            // The trees on the outside of the grid are visible by definition so they count automatically

            int numRows = _treeMap.GetLength(0);
            int numCols = _treeMap.GetLength(1);
            int visibleTrees = numRows * 2 + (numCols - 2) * 2;
            Console.WriteLine("inital visible trees = " + visibleTrees);

            // start at row 1 and end at the next-to-last-row; we can ignore the first (0th) and last rows because those trees are, by definition, visible
            for (int row = 1; row < numRows - 1; row++)
            {
                // same for the columns, ignore the first and last
                for (int column = 1; column < numCols - 1; column++)
                {
                    if (TreeIsVisible(row,column))
                    { 
                        Console.WriteLine($"Tree at position {row},{column} is visible");
                        visibleTrees++;
                    }
                }
            }
            return visibleTrees.ToString();
        }

        public string Answer2()
        {
            return string.Empty;
        }

        private bool TreeIsVisible(int row, int column)
        {
            int numRows = _treeMap.GetLength(0);
            int numCols = _treeMap.GetLength(1);

            int height = _treeMap[row, column];
            // need to check map in all four directions, up, down, left, and right,
            // to make sure there are no cells in that direction with value greater than 'height'


            // brute force at first
            bool visibleUp = true;
            bool visibleDown = true;
            bool visibleLeft = true;
            bool visibleRight = true;

            // check up
            for (int i = row-1; i >= 0; i--)
            {
                if (_treeMap[i, column] >= height)
                {
                    visibleUp = false;
                    break;
                }

            }

            // check down
            for (int i = row+1; i < numRows; i++)
            {
                if (_treeMap[i, column] >= height)
                {
                    visibleDown = false;
                    break;
                }

            }

            // check left
            for (int i = column-1; i >= 0; i--)
            {
                if (_treeMap[row, i] >= height)
                {
                    visibleLeft = false;
                    break;
                }

            }

            // check right
            for (int i = column+1; i < numCols; i++)
            {
                if (_treeMap[row, i] >= height)
                {
                    visibleRight = false;
                    break;
                }

            }

            return visibleUp || visibleDown || visibleLeft || visibleRight;
        }

        private int[,] GetTreeMap(string inputFile)
        {
            string[] lines = File.ReadAllLines(inputFile);

            int height = lines.Length;
            int width = lines[0].Length;

            int [,] treeMap = new int[height, width];
            int row = 0;
            foreach(string line in lines)
            {
                for (int column = 0; column<line.Length; column++)
                {
                    treeMap[row, column] = line[column] - '0'; // trick for converting char to int from SO: https://stackoverflow.com/questions/239103/convert-char-to-int-in-c-sharp
                }
                row++;
            }

            return treeMap;
        }
    }

}