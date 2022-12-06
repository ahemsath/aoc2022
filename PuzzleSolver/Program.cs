using System;
using System.IO;
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
            ISolver solver;
            string baseName = Path.GetFileName(inputFile);

            string puzzleName = baseName.Remove(baseName.Length - 4);
            Console.WriteLine($"puzzleName = {puzzleName}");

            Type solverType = Type.GetType($"Solvers.{puzzleName}, Solvers");

            solver = (ISolver)Activator.CreateInstance(solverType, inputFile);
            Console.WriteLine($"{puzzleName}: Answer 1 = {solver.Answer1()}, Answer 2 = {solver.Answer2()}");
        }
    }
}
