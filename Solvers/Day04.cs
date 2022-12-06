namespace Solvers
{

    public class Day04 : ISolver
    {
        private string _inputFile;

        public Day04(string inputFile)
        {
            _inputFile = inputFile;
        }

        public int Answer1()
        {
            int totalContainedRanges = 0;
            foreach (string line in File.ReadLines(_inputFile))
            {
                Range firstRange = new Range(line.Split(',')[0]);
                Range secondRange = new Range(line.Split(',')[1]);

                if (firstRange.Contains(secondRange) || secondRange.Contains(firstRange))
                {
                    Console.WriteLine($"Found one: {line}");
                    totalContainedRanges++;
                }
            }
            return totalContainedRanges;
        }

        public int Answer2()
        {
            int totalOverlappingRanges = 0;
            foreach (string line in File.ReadLines(_inputFile))
            {
                Range firstRange = new Range(line.Split(',')[0]);
                Range secondRange = new Range(line.Split(',')[1]);

                if (firstRange.Overlaps(secondRange) || secondRange.Overlaps(firstRange))
                {
                    Console.WriteLine($"Found one: {line}");
                    totalOverlappingRanges++;
                }
            }
            return totalOverlappingRanges;
        }


    }

    public class Range
    {
        private int _start;
        private int _end;

        public int Start { get { return _start; } }
        public int End { get { return _end; } }

        public Range(string range)
        {
            _start = int.Parse(range.Split('-')[0]);
            _end = int.Parse(range.Split('-')[1]);
        }

        public bool Contains(Range range)
        {
            return (_start <= range.Start && _end >= range.End);
        }
        public bool Overlaps(Range range)
        {
            if ((_start <= range.Start && range.Start <= _end) ||
                (_start <= range.End && range.End <= _end)) 
            {
                return true;
            }
            return false;
        }
    }
}