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
            HashSet<(int,int)> visited = new HashSet<(int,int)> ();

            (int x,int y) head = (0,0);
            (int x,int y) tail = (0,0);
            visited.Add(tail);

            foreach((char direction,int count) move in _moves)
            {
                Console.WriteLine($"Move head {move.direction} {move.count} times.");
                for (int i = 0; i < move.count; i++)
                {
                    // update head pos
                    head = MoveHead(head, move.direction);

                    // update tail pos
                    tail = FollowHead(head, tail);

                    // store tail pos
                    visited.Add(tail);
                }
            }
            return visited.Count.ToString();
        }

        public string Answer2()
        {
            return string.Empty;
        }

        private (int, int) MoveHead((int x, int y) head, char direction)
        {
            switch(direction)
            {
                case 'U':
                    return (head.x, head.y + 1);
                case 'D':
                    return (head.x, head.y - 1);
                case 'L':
                    return (head.x - 1, head.y);
                case 'R':
                    return (head.x + 1, head.y);
                default:
                    throw new ArgumentException("Unknown direction " + direction);
            }
        }

        private (int, int) FollowHead((int x, int y) head, (int x, int y) tail)
        {
            int dx = head.x - tail.x;
            int dy = head.y - tail.y;

            if (Math.Abs(dx) <=1 && Math.Abs(dy) <=1)
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
                    x = dx > 0 ? x + 1: x - 1;
                }
            }

            return (x, y);
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

}