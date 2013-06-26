using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    class Sudoku
    {
        /// <summary>
        /// The size of the sudoku grid
        /// </summary>
        const int SIZE = 9;

        /// <summary>
        /// The maximum digit we are allowed to enter in a cell
        /// </summary>
        const int DMAX = 9;

        /// <summary>
        /// The number of cells (x,y) in a box
        /// </summary>
        const int BOXSIZE = 3;

        /// <summary>
        /// The array containing the grid
        /// </summary>
        int[,] grid;

        /// <summary>
        /// The current solution number we have found
        /// </summary>
        int solutionnr = 0;

        /// <summary>
        /// The number of the next empty column
        /// </summary>
        int cempty;

        /// <summary>
        /// The number of the next empty row
        /// </summary>
        int rempty;

        /// <summary>
        /// Check if there is a conflict 
        /// </summary>
        /// <param name="r">The row coordinate</param>
        /// <param name="c">The column coordinate</param>
        /// <param name="d">The current digit we are validating</param>
        /// <returns>True if it can be filled in, false otherwise</returns>
        private bool givesConflict(int r, int c, int d)
        {
            return rowConflict(r, d) || colConflict(c, d) || boxConflict(r, c, d);
        }

        /// <summary>
        /// Check if there is a row conflict
        /// </summary>
        /// <param name="r">The row we want to check</param>
        /// <param name="d">The digit we want to insert</param>
        /// <returns>True if we can insert it, false otherwise</returns>
        private bool rowConflict(int r, int d)
        {
            for (int c = 0; c < SIZE; c++)
            {
                if (grid[r,c] == d)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if there is a column conflict
        /// </summary>
        /// <param name="c">The column we want to check</param>
        /// <param name="d">The digit we want to insert</param>
        /// <returns>True if we can insert it, false otherwise</returns>
        private bool colConflict(int c, int d)
        {
            for (int r = 0; r < SIZE; r++)
            {
                if (grid[r,c] == d)
                {
                    return true;
                }
            }
            
            return false;

        }

        /// <summary>
        /// Check if there is a conflict in the box
        /// </summary>
        /// <param name="rr"></param>
        /// <param name="cc"></param>
        /// <param name="d">The digit we want to insert</param>
        /// <returns>Returns true if we can insert it, false otherwise</returns>
        private bool boxConflict(int rr, int cc, int d)
        {
            rr -= rr % BOXSIZE;
            cc -= cc % BOXSIZE;
            for (int r = 0; r < BOXSIZE; r++)
            {
                for (int c = 0; c < BOXSIZE; c++)
                {
                    if (grid[rr + r,cc + c] == d)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if there is an empty square left
        /// </summary>
        /// <returns>True if there is an empty square left, false otherwise</returns>
        private bool findEmptySquare()
        {
            for (rempty = 0; rempty < SIZE; rempty++)
            {
                for (cempty = 0; cempty < SIZE; cempty++)
                {
                    if (grid[rempty,cempty] == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// This is the actual recursive function that handles the solving
        /// </summary>
        private void solve()
        {
            //if there are no empty squares left we print the current solution
            if (!findEmptySquare())
            {
                solutionnr++;
                print();
                return;
            }
            //Set r and c to the next empty cell
            int r = rempty, c = cempty;

            //we check if any digit between 1 and 9, inclusive can be filled in, if so, we fill it in and call solve again
            //Since the current digit was filled in calling solve again will go on to fill in the next digit
            //A function calling itself is called recursion, feel free to look it up :)
            //After solve returns, we set the current cell back to 0 to be able to compute the next solution
            for (int i = 1; i <= DMAX; i++)
            {
                if (!givesConflict(r, c, i))
                {
                    grid[r,c] = i;
                    solve();
                    grid[r,c] = 0;
                }
            }
        }

        /// <summary>
        /// Prints the current sudoku grid
        /// </summary>
        private void print()
        {
            Console.WriteLine("-------------------------");
            for (int i = 0; i < SIZE; i++)
            {
                Console.Write("|");
                for (int j = 0; j < SIZE; j++)
                {
                    Console.Write(" ");
                    Console.Write((grid[i,j] > 0) ? grid[i,j].ToString() : " ");
                    Console.Write(((j + 1) % BOXSIZE == 0) ? " |" : "");
                }
                Console.Write("\n");
                if ((i + 1) % BOXSIZE == 0)
                {
                    Console.WriteLine("-------------------------");
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Reads the sudoku from the console
        /// </summary>
        private void readSudoku()
        {
    	    char s;
            //The characters we accept for the sudoku, . is an empty cell
    	    String vc = "123456789.";

            //We keep reading as long as we dont have a complete sudoku
    	    for (int r = 0; r < SIZE; r++) {
                for (int c = 0; c < SIZE; c++)
                {
                    //Read the next character
                    s = Convert.ToChar(Console.Read());
                    if (vc.Contains(s) && !s.Equals('.'))
                    {
                        //Parse it to an int if it is a character we accept and not a .
                        //An int-array is initalized with 0's so we dont actually have to do anything with '.'
                        grid[r,c] = int.Parse(s.ToString());
                    }
                    //That characted was not valid, lower c by 1 because we don't have a valid int for this row
                    else if (!s.Equals('.'))
                    {
                        c--;
                    }
                }
    	    }
        }

        /// <summary>
        /// This reads the sudoku and prints calls the solve method
        /// </summary>
        public void solveIt()
        {
            grid = new int[SIZE,SIZE];
            readSudoku();
            Console.WriteLine("Started solving");
            solve();    
            Console.WriteLine("Found " + solutionnr + " solution" + (solutionnr != 1 ? "s" : ""));
        }
    }
}
