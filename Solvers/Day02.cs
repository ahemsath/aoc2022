using System.Linq;

namespace Solvers
{
    public class Day02 : ISolver
    {
        private string _inputFile;

        private Dictionary<char, int> _shapeScores = new Dictionary<char, int>() { { 'X', 1 }, { 'Y', 2 }, { 'Z', 3 } };

        // This data structure maps Rock-Paper-Scissors moves to scores
        // The first char is the opponent's move.  A = Rock, B = Paper, C = Scissors
        // The second char is our move.  X = Rock, Y = Paper, Z = Scissors
        // 6 = win, 3 = draw, 0 = lose
        private Dictionary<string, int> _moveScores = new Dictionary<string, int>()
        {
            { "AX", 3 },
            { "AY", 6 },
            { "AZ", 0 },
            { "BX", 0 },
            { "BY", 3 },
            { "BZ", 6 },
            { "CX", 6 },
            { "CY", 0 },
            { "CZ", 3 }
        };

        public Day02(string inputFile)
        {
            _inputFile = inputFile;
        }

        public int Answer1()
        {
            int totalScore = 0;
            foreach (string line in File.ReadLines(_inputFile))
            {
                char opponentPlay = line[0];
                char myPlay = line[2];
                int shapeScore = _shapeScores[myPlay];
                var moveScore = _moveScores[$"{opponentPlay}{myPlay}"];
                int currentScore =  shapeScore + moveScore;
                Console.WriteLine($"Opp play = {opponentPlay}, my play = {myPlay}, shape score = {shapeScore}, move score = {moveScore}");
                totalScore += currentScore;
            }
            return totalScore;
        }

        public int Answer2()
        {
            return -1;
        }
    }
}