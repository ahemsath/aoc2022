using System.Security.Cryptography;
using System;

namespace Solvers
{

    public class Day11 : ISolver
    {
        private string _inputFile;
        private List<Monkey> _monkeyList;

        public Day11(string inputFile)
        {
            _inputFile = inputFile;
            _monkeyList = new List<Monkey>();
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
                List<int> startingItems = startingItemsList.Split(",").Select(x => int.Parse(x.Trim())).ToList();
                index++;

                // read operation
                string operation = lines[index].Split(':')[1].Split('=')[1].Trim();
                index++;

                // read test divisor
                int testDivisor = int.Parse(lines[index].Split(':')[1].Trim().Replace("divisible by ", ""));
                index++;

                // read monkey to throw item to if test passes
                int trueMonkey = int.Parse(lines[index].Split(':')[1].Trim().Replace("throw to monkey ", ""));
                index++;

                // read monkey to throw item to if test false
                int falseMonkey = int.Parse(lines[index].Split(':')[1].Trim().Replace("throw to monkey ", ""));
                index++;

                _monkeyList.Add(new Monkey(startingItems, operation, testDivisor, trueMonkey, falseMonkey));

                // skip blank line before next monkey, if any
                index++;
            }
        }

        public string Answer1()
        {
            for (int round = 0; round < 20; round++)
            {
                foreach(var monkey in _monkeyList)
                {
                    var throwList = monkey.TakeTurn();
                    foreach ((int item, int targetMonkey) in throwList)
                    {
                        _monkeyList[targetMonkey].Catch(item);
                    }
                }
            }

            // calculate "monkey business"
            var inspectionCounts = _monkeyList.Select(x => x.InspectionCount).ToList().OrderByDescending(x => x).ToArray();
            return (inspectionCounts[0] * inspectionCounts[1]).ToString();
        }

        public string Answer2()
        {
            return string.Empty;
        }

    }

    public class Monkey
    {
        private List<int> _items;
        private IOperation _operation;
        private int _testDivisor;
        private int _trueMonkey;
        private int _falseMonkey;

        private int _inspectionCount;
        public int InspectionCount => _inspectionCount;

        public Monkey(List<int> items, string operation, int testDivisor, int trueMonkey, int falseMonkey)
        {
            _items = items;
            _operation = ParseOperation(operation);
            _testDivisor = testDivisor;
            _trueMonkey = trueMonkey;
            _falseMonkey = falseMonkey;
            _inspectionCount = 0;
        }

        // The output of this method is a list of items paired with the monkey they are being thrown to
        // The reason we can't throw the items from the Monkey class is that it has no reference the the list of monkeys
        public List<(int item, int monkey)> TakeTurn()
        {
            var output = new List<(int item, int monkey)>();

            foreach(var item in _items)
            {
                // inspect item
                _inspectionCount++;
                // perform operation
                var newWorryLevel = _operation.Apply(item) / 3;

                output.Add((newWorryLevel, newWorryLevel % _testDivisor == 0 ? _trueMonkey : _falseMonkey));
            }

            // All items have been thrown at this point so empty out the item list
            _items = new List<int>();

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

        public void Catch(int item)
        {
            _items.Add(item);
        }

    }

    public interface IOperation
    {
        int Apply(int input);
    }

    public class Add : IOperation
    {
        private int _operand;

        public Add(int operand)
        {
            _operand = operand;
        }

        public int Apply(int input)
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

        public int Apply(int input)
        {
            return input * _operand;
        }
    }

    public class Square : IOperation
    {
        public Square()
        {
        }

        public int Apply(int input)
        {
            return input * input;
        }
    }

}