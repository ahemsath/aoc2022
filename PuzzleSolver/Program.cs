using System;
using Solvers;

namespace PuzzleSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = args[0];
            if (inputFile == null)
            {
                Console.WriteLine("Must supply input file");
                return;
            }
            if (inputFile.Contains("Day01"))
            {
                var solver = new Day01(inputFile);
                Console.WriteLine($"Day01: Answer 1 = {solver.Answer1()}, Answer 2 = {solver.Answer2()}");
            }
            if (inputFile.Contains("Day02"))
            {
                var solver = new Day02(inputFile);
                Console.WriteLine($"Day02: Answer 1 = {solver.Answer1()}, Answer 2 = {solver.Answer2()}");
            }
        }
    }
}
