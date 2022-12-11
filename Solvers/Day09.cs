namespace Solvers
{

    public class Day09 : ISolver
    {
        private string _inputFile;
        private List<(char, int)> _moves;

        public Day09(string inputFile)
        {
            _inputFile = inputFile;
            _moves = GetMoveList();
        }


        public string Answer1()
        {
            return GetTailVistedCount(2).ToString();
        }

        public string Answer2()
        {
            return GetTailVistedCount(10).ToString();
        }

        private int GetTailVistedCount(int ropeLength)
        {
            var visited = new HashSet<(int, int)>();

            var rope = new Rope(ropeLength);

            visited.Add(rope.Tail);

            foreach ((char direction, int count) move in _moves)
            {
                for (int i = 0; i < move.count; i++)
                {
                    // move rope
                    rope.MoveHead(move.direction);

                    // store tail pos
                    visited.Add(rope.Tail);
                }
            }
            return visited.Count;
        }


        private List<(char, int)> GetMoveList()
        {
            List<(char,int)> moves = new List<(char,int)>();
            foreach (string line in File.ReadLines(_inputFile))
            {
                char direction = line[0];
                int moveCount = int.Parse(line.Substring(2));
                moves.Add((direction,moveCount));
            }
            return moves;
        }
    }

    public class Rope
    {
        private List<(int x, int y)> _knots;
        private int _length;

        public (int x, int y) Head => _knots[0];
        public (int x, int y) Tail => _knots[_length - 1];

        public Rope(int length)
        {
            _length = length;
            _knots = new List<(int x, int y)>();
            for(int i = 0; i < _length; i++)
            {
                _knots.Add((0, 0)); // Initial position of each knot is 0,0
            }
        }

        public void MoveHead(char direction)
        {
            // move head
            _knots[0] = MoveKnot(Head,direction);

            // update rest of knots
            for (int i = 1; i < _length; i++)
            {
                _knots[i] = Follow(_knots[i - 1], _knots[i]);
            }

        }

        private (int,int) MoveKnot((int x, int y) knot, char direction)
        {
            switch (direction)
            {
                case 'U':
                    return (knot.x, knot.y + 1);
                case 'D':
                    return (knot.x, knot.y - 1);
                case 'L':
                    return (knot.x - 1, knot.y);
                case 'R':
                    return (knot.x + 1, knot.y);
                default:
                    throw new ArgumentException("Unknown direction " + direction);
            }
        }

        private (int, int) Follow((int x, int y) head, (int x, int y) tail)
        {
            int dx = head.x - tail.x;
            int dy = head.y - tail.y;

            if (Math.Abs(dx) <= 1 && Math.Abs(dy) <= 1)
            {
                // head and tail are adjacent, return original tail position
                return tail;
            }
            int x = tail.x;
            int y = tail.y;

            if (Math.Abs(dx) > 1)
            {
                x = dx > 0 ? x + 1 : x - 1;
                if (dy != 0) // not in same row, so move diagonally
                {
                    y = dy > 0 ? y + 1 : y - 1;
                }
            }
            else // Math.Abs(dy) must be > 1
            {
                y = dy > 0 ? y + 1 : y - 1;
                if (dx != 0) // not in same column, so move diagonally
                {
                    x = dx > 0 ? x + 1 : x - 1;
                }
            }

            return (x, y);
        }

        public override string ToString()
        {
            var output = string.Empty;
            foreach (var knot in _knots)
            {
                output += knot.ToString() + ", ";
            }

            return output;
        }
    }
}