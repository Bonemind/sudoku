using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            Sudoku s = new Sudoku();
            s.solveIt();
            Console.WriteLine("Press enter to exit");
            Console.ReadKey();
            Console.ReadLine();
        }
    }
}
