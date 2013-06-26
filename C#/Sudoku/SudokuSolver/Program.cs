using System;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            Sudoku s = new Sudoku();
            s.solveIt();

            System.Threading.Thread.Sleep(500);
            Console.WriteLine("Press enter to exit");
            Console.ReadKey();
        }
    }
}
