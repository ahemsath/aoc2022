namespace Solvers
{
    public enum Shape
    {
        Rock,
        Paper,
        Scissors
    }

    public enum Strategy
    {
        Lose,
        Draw,
        Win
    }

    public class Day02 : ISolver
    {
        private string _inputFile;

        private Dictionary<char, Shape> _opponentShapeMap = new Dictionary<char, Shape>() { { 'A', Shape.Rock }, { 'B', Shape.Paper }, { 'C', Shape.Scissors } };
        private Dictionary<char, Shape> _myShapeMap = new Dictionary<char, Shape>() { { 'X', Shape.Rock }, { 'Y', Shape.Paper }, { 'Z', Shape.Scissors } };

        private Dictionary<Shape, int> _shapeScores = new Dictionary<Shape, int>() { { Shape.Rock, 1 }, { Shape.Paper, 2 }, { Shape.Scissors, 3 } };

        // This data structure maps Rock-Paper-Scissors moves to scores
        // First shape is opponent's, second is ours
        // 6 = win, 3 = draw, 0 = lose
        private Dictionary<(Shape,Shape), int> _moveScores = new Dictionary<(Shape,Shape), int>()
        {
            { (Shape.Rock,Shape.Rock), 3 },
            { (Shape.Rock,Shape.Paper), 6 },
            { (Shape.Rock,Shape.Scissors), 0 },
            { (Shape.Paper,Shape.Rock), 0 },
            { (Shape.Paper,Shape.Paper), 3 },
            { (Shape.Paper,Shape.Scissors), 6 },
            { (Shape.Scissors,Shape.Rock), 6 },
            { (Shape.Scissors,Shape.Paper), 0 },
            { (Shape.Scissors,Shape.Scissors), 3 }
        };

        // This data structure encodes which shape I need to choose based on the opponent's shape and my strategy
        private Dictionary<(Shape, Strategy), Shape> _stratMap = new Dictionary<(Shape, Strategy), Shape>()
        {
            { (Shape.Rock,Strategy.Lose), Shape.Scissors },
            { (Shape.Rock,Strategy.Draw), Shape.Rock },
            { (Shape.Rock,Strategy.Win), Shape.Paper },
            { (Shape.Paper,Strategy.Lose), Shape.Rock },
            { (Shape.Paper,Strategy.Draw), Shape.Paper },
            { (Shape.Paper,Strategy.Win), Shape.Scissors },
            { (Shape.Scissors,Strategy.Lose), Shape.Paper },
            { (Shape.Scissors,Strategy.Draw), Shape.Scissors },
            { (Shape.Scissors,Strategy.Win), Shape.Rock }
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
                Shape opponentPlay = _opponentShapeMap[line[0]];
                Shape myPlay = _myShapeMap[line[2]];
                int shapeScore = _shapeScores[myPlay];
                var moveScore = _moveScores[(opponentPlay,myPlay)];
                int currentScore =  shapeScore + moveScore;
                //Console.WriteLine($"Opp play = {opponentPlay}, my play = {myPlay}, shape score = {shapeScore}, move score = {moveScore}");
                totalScore += currentScore;
            }
            return totalScore;
        }

        public int Answer2()
        {
            int totalScore = 0;
            foreach (string line in File.ReadLines(_inputFile))
            {
                Shape opponentPlay = _opponentShapeMap[line[0]];
                Shape myPlay;
                Strategy myStrat;

                // determine strategy
                switch(line[2])
                {
                    case 'X':
                        myStrat = Strategy.Lose;
                        break;
                    case 'Y':
                        myStrat = Strategy.Draw;
                        break;
                    case 'Z':
                        myStrat = Strategy.Win;
                        break;
                    default:
                        myStrat = Strategy.Win;
                        break;
                }

                myPlay = _stratMap[(opponentPlay,myStrat)];

                int shapeScore = _shapeScores[myPlay];
                var moveScore = _moveScores[(opponentPlay, myPlay)];
                int currentScore = shapeScore + moveScore;
                //Console.WriteLine($"Opp play = {opponentPlay}, my play = {myPlay}, shape score = {shapeScore}, move score = {moveScore}");
                totalScore += currentScore;
            }
            return totalScore;
        }
    }
}