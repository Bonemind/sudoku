using System;
using System.Linq;

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
        /// The number of cells (x, y) in a box
        /// </summary>
        const int BOXSIZE = 3;

        /// <summary>
        /// The array containing the grid
        /// </summary>
        private int[,] grid;

        /// <summary>
        /// The current solution number we have found
        /// </summary>
        private int solutionnr = 0;

        /// <summary>
        /// The number of the next empty column
        /// </summary>
        private int cempty;

        /// <summary>
        /// The number of the next empty row
        /// </summary>
        private int rempty;

        // ----------------- Conflict calculation --------------------

        /// <summary>
        /// Check if there is a conflict 
        /// </summary>
        /// <param name="row">The row coordinate</param>
        /// <param name="column">The column coordinate</param>
        /// <param name="digit">The current digit we are validating</param>
        /// <returns>True if inserting that number would cause a conflict, false if it's safe to insert</returns>
        private bool givesConflict(int row, int column, int digit)
        {
            return rowConflict(row, digit) || colConflict(column, digit) || boxConflict(row, column, digit);
        }

        /// <summary>
        /// Check if there is a row conflict
        /// </summary>
        /// <param name="row">The row we want to check</param>
        /// <param name="digit">The digit we want to insert</param>
        /// <returns>True if inserting that number would cause a conflict, false if it's safe to insert</returns>
        private bool rowConflict(int row, int digit)
        {
            for (int column = 0; column < SIZE; column++)
            {
                if (grid[row, column] == digit)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if there is a column conflict
        /// </summary>
        /// <param name="column">The column we want to check</param>
        /// <param name="digit">The digit we want to insert</param>
        /// <returns>True if inserting that number would cause a conflict, false if it's safe to insert</returns>
        private bool colConflict(int column, int digit)
        {
            for (int row = 0; row < SIZE; row++)
            {
                if (grid[row, column] == digit)
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
        /// <param name="digit">The digit we want to insert</param>
        /// <returns>Returns true if we can insert it, false otherwise</returns>
        private bool boxConflict(int row, int column, int digit)
        {
            row -= row % BOXSIZE;
            column -= column % BOXSIZE;
            for (int rr = 0; rr < BOXSIZE; rr++)
            {
                for (int cc = 0; cc < BOXSIZE; cc++)
                {
                    if (grid[row + rr, column + cc] == digit)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // --------- Solving ----------

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
                    if (grid[rempty, cempty] == 0)
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
            // if there are no empty squares left we print the current solution
            if (!findEmptySquare())
            {
                solutionnr++;
                print();
                return;
            }

            // Set row and column to the next empty cell
            int row = rempty, column = cempty;

            // We check if any digit between 1 and 9, inclusive can be filled in, if so, we fill it in and call solve again
            // Since the current digit was filled in calling solve again will go on to fill in the next digit
            // A function calling itself is called recursion, feel free to look it up :)
            // After solve returns, we set the current cell back to 0 to be able to compute the next solution
            for (int i = 1; i <= DMAX; i++)
            {
                if (!givesConflict(row, column, i))
                {
                    grid[row, column] = i;
                    solve();
                    grid[row, column] = 0;
                }
            }
        }

        // ------------------------- I/O -------------------------

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
                    Console.Write((grid[i, j] > 0) ? grid[i, j].ToString() : " ");
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
            // To store the current character.
    	    char s;

            // The characters we accept for the sudoku, '.' is an empty cell
    	    String vc = "123456789.";

            // We keep reading as long as we dont have a complete sudoku
    	    for (int row = 0; row < SIZE; row++) 
            {
                for (int column = 0; column < SIZE; column++)
                {
                    // Read the next character
                    do 
                    {
                        s = Convert.ToChar(Console.Read());
                    } while (!vc.Contains(s));
                    // Keep going until we have a valid input.

                    // Parse it to an int if it is a character we accept and not a '.'
                    // An int-array is initalized with 0's so we dont actually have to do anything with '.'
                    if (!s.Equals('.'))
                    {
                        grid[row, column] = int.Parse(s.ToString());
                    }
                }
    	    }
        }

        // --------------- Where it all starts --------------------

        /// <summary>
        /// This reads the sudoku and calls the solve method
        /// </summary>
        public void solveIt()
        {
            Console.WriteLine("Enter sudoku:");
            grid = new int[SIZE, SIZE];
            readSudoku();

            Console.WriteLine("Started solving");
            solve();    
            Console.WriteLine("Found " + solutionnr + " solution" + (solutionnr != 1 ? "s" : ""));
        }
    }
}
