namespace Solvers
{
    public class Day11 : ISolver
    {
        private string _inputFile;

        public Day11(string inputFile)
        {
            _inputFile = inputFile;
        }

        public string Answer1()
        {
            var monkeyList = GetMonkeys(false);

            for (int round = 0; round < 20; round++)
            {
                foreach(var monkey in monkeyList)
                {
                    var throwList = monkey.TakeTurn();
                    foreach ((long item, int targetMonkey) in throwList)
                    {
                        monkeyList[targetMonkey].Catch(item);
                    }
                }
            }

            // calculate "monkey business"
            var inspectionCounts = monkeyList.Select(x => x.InspectionCount).ToList().OrderByDescending(x => x).ToArray();
            return (inspectionCounts[0] * inspectionCounts[1]).ToString();
        }

        public string Answer2()
        {
            var monkeyList = GetMonkeys(true);

            for (int round = 0; round < 10000; round++)
            {
                foreach (var monkey in monkeyList)
                {
                    var throwList = monkey.TakeTurn();
                    foreach ((long item, int targetMonkey) in throwList)
                    {
                        monkeyList[targetMonkey].Catch(item);
                    }
                }
            }

            // calculate "monkey business"
            var inspectionCounts = monkeyList.Select(x => x.InspectionCount).ToList().OrderByDescending(x => x).ToArray();
            return (inspectionCounts[0] * inspectionCounts[1]).ToString();
        }

        private List<Monkey> GetMonkeys(bool scary)
        {
            var monkeyList = new List<Monkey>();
            var productOfDivisors = 1;
            var lines = File.ReadAllLines(_inputFile);
            int index = 0;
            while (index + 5 < lines.Length)
            {
                // input looks like this
                //Monkey 0:
                //Starting items: 79, 98
                //Operation: new = old * 19
                //Test: divisible by 23
                //If true: throw to monkey 2
                //If false: throw to monkey 3

                // We don't actually need to read the monkey number from the input because they appear in order starting at 0, and we're adding them to a list in order
                index++; // skip "Monkey" line

                // read input items
                string startingItemsList = lines[index].Split(':')[1].Trim();
                List<long> startingItems = startingItemsList.Split(",").Select(x => long.Parse(x.Trim())).ToList();
                index++;

                // read operation
                string operation = lines[index].Split(':')[1].Split('=')[1].Trim();
                index++;

                // read test divisor
                int testDivisor = int.Parse(lines[index].Split(':')[1].Trim().Replace("divisible by ", ""));
                productOfDivisors *= testDivisor;
                index++;

                // read monkey to throw item to if test passes
                int trueMonkey = int.Parse(lines[index].Split(':')[1].Trim().Replace("throw to monkey ", ""));
                index++;

                // read monkey to throw item to if test false
                int falseMonkey = int.Parse(lines[index].Split(':')[1].Trim().Replace("throw to monkey ", ""));
                index++;

                monkeyList.Add(new Monkey(startingItems, operation, testDivisor, trueMonkey, falseMonkey));

                // skip blank line before next monkey, if any
                index++;
            }

            if (scary)
            {
                foreach (var monkey in monkeyList)
                {
                    monkey.GetScary(productOfDivisors);
                }
            }

            return monkeyList;
        }

    }

    public class Monkey
    {
        private List<long> _items;
        private IOperation _operation;
        private int _testDivisor;
        private int _trueMonkey;
        private int _falseMonkey;

        private bool _simpleWorryReduction;
        private int _worryReductionModulus;
        

        private long _inspectionCount;
        public long InspectionCount => _inspectionCount;
        public int Divisor => _testDivisor;

        public Monkey(List<long> items, string operation, int testDivisor, int trueMonkey, int falseMonkey)
        {
            _items = items;
            _operation = ParseOperation(operation);
            _testDivisor = testDivisor;
            _trueMonkey = trueMonkey;
            _falseMonkey = falseMonkey;
            _inspectionCount = 0;
            _simpleWorryReduction = true;
            _worryReductionModulus = 0;
        }

        // The output of this method is a list of items paired with the monkey they are being thrown to
        // The reason we can't throw the items from the Monkey class is that it has no reference the the list of monkeys
        public List<(long item, int monkey)> TakeTurn()
        {
            var output = new List<(long item, int monkey)>();

            foreach(var item in _items)
            {
                // inspect item
                _inspectionCount++;
                // perform operation
                var newWorryLevel = _operation.Apply(item);
                if (_simpleWorryReduction)
                {
                     newWorryLevel /= 3;
                }
                else
                {
                    // The trick to handling the explosion in worry levels caused by no longer dividing new worry levels by three, and also taking 10,000 rounds,
                    // is to perform a modulo operation on the new worry level, where the modulus is the product of all monkeys' test divisors. This works because:
                    // 1. The main thing is to keep track of which monkey each item is thrown to during each monkey's turn
                    // 2. The thing that determines 1. for each monkey is whether it is evenly divisible by its test divisor
                    // 3. The answer to 2. is remains correct for all monkeys when you only keep track of the remainder of dividing by the product of all monkeys' test divisors.

                    // I definitely did not figure this out on my own. I got a hint from Reddit that all we really care about is the divisibility test for each item. I had a hunch
                    // that modulating by the common product of all divisors was correct, and I consulted a coworker's solution to confirm.

                    // We still need to use 64-bit (long) integers to avoid overflow, even in the sample input case, because the common product of all divisors is 96,577, which
                    // makes the max value of each item one less than that number, which can end up being squared if it ends up being thrown to a monkey with that operation,
                    // and 96,576^2 is way too big to fit in a 32-bit integer.

                    newWorryLevel = newWorryLevel % _worryReductionModulus;
                }

                output.Add((newWorryLevel, newWorryLevel % _testDivisor == 0 ? _trueMonkey : _falseMonkey));
            }

            // All items have been thrown at this point so empty out the item list
            _items = new List<long>();

            return output;
        }

        private IOperation ParseOperation(string operation)
        {
            // the operation string always starts with "old " followed by a '+' or a '*', followed by a space and then the operand
            char operationSymbol = operation[4];
            string operand = operation.Substring(6);
            if (operand == "old")
            {
                // old * old, i.e. square the input
                return new Square();
            }
            switch(operationSymbol)
            {
                case '+':
                    return new Add(int.Parse(operand));
                case '*':
                    return new Multiply(int.Parse(operand));
                default:
                    throw new InvalidOperationException("Unknown operation " + operationSymbol);
            }
        }

        public void Catch(long item)
        {
            _items.Add(item);
        }

        public void GetScary(int worryReductionModulus)
        {
            _simpleWorryReduction = false;
            _worryReductionModulus = worryReductionModulus;
        }
    }

    public interface IOperation
    {
        long Apply(long input);
    }

    public class Add : IOperation
    {
        private int _operand;

        public Add(int operand)
        {
            _operand = operand;
        }

        public long Apply(long input)
        {
            return input + _operand;
        }
    }

    public class Multiply : IOperation
    {
        private int _operand;

        public Multiply(int operand)
        {
            _operand = operand;
        }

        public long Apply(long input)
        {
            return input * _operand;
        }
    }

    public class Square : IOperation
    {
        public Square()
        {
        }

        public long Apply(long input)
        {
            return input * input;
        }
    }

}