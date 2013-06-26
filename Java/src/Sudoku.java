import java.util.Scanner;
import java.lang.Thread;
import java.io.IOException;

class Sudoku 
{
    /**
     * The size of the sudoku grid
     */
    final int SIZE = 9;

    /**
     * The maximum digit we are allowed to enter in a cell
     */
    final int DMAX = 9;

    /**
     * The number of cells (x, y) in a box
     */
    final int BOXSIZE = 3; 

    /**
     * The array containing the grid
     */
    private int[][] grid; 

    /**
     * The current solution number we have found
     */
    private int solutionnr = 0; 

    /**
     * The number of the next empty column
     */
    private int cempty;

    /**
     * The number of the next empty row
     */
    private int rempty;

    // ----------------- Conflict calculation --------------------

    /**
     * Check if there is a conflict 
     *
     * @param row The row coordinate
     * @param column The column coordinate
     * @param digit The current digit we are validating
     * @return True if inserting that number would cause a conflict, false if it's safe to insert
     */
    private boolean givesConflict(int row, int  column, int digit) 
    {
        return rowConflict(row, digit) || colConflict(column, digit) || boxConflict(row, column, digit);
    }

    /**
     * Check if there is a row conflict
     *
     * @param row The row we want to check
     * @param digit The digit we want to insert
     * @return True if inserting that number would cause a conflict, false if it's safe to insert
     */
    private boolean rowConflict(int row, int digit) 
    {
        for (int column = 0; column < SIZE; column++) 
        {
            if (grid[row][column] == digit) 
            {
                return true;
            }
        }
        return false;
    }

    /**
     * Check if there is a column conflict
     *
     * @param column The column we want to check
     * @param digit The digit we want to insert
     * @return True if inserting that number would cause a conflict, false if it's safe to insert
     */
    private boolean colConflict(int column, int digit) 
    {
        for (int row = 0; row < SIZE; row++) 
        {
            if (grid[row][column] == digit) 
            {
                return true;
            }
        }
        return false;
    }

    /**
     * Check if there is a conflict in the box
     *
     * @param row 
     * @param column 
     * @param digit The digit we want to insert
     * @return Returns true if we can insert it, false otherwise
     */
    private boolean boxConflict(int row, int column, int digit) 
    {
        row -= row % BOXSIZE;
        column -= column % BOXSIZE;
        for (int rr = 0; rr < BOXSIZE; rr++) 
        {
            for (int cc = 0; cc < BOXSIZE; cc++) 
            {
                if (grid[row + rr][column + cc] == digit) 
                {
                    return true;
                }
            }
        }
        return false;
    }

    // --------- Solving ----------

    /**
     * Checks if there is an empty square left
     *
     * @return True if there is an empty square left, false otherwise
     */
    private boolean findEmptySquare() 
    {
        for (rempty = 0; rempty < SIZE; rempty++) 
        {
            for (cempty = 0; cempty < SIZE; cempty++) 
            {
                if (grid[rempty][cempty] == 0) 
                {
                    return true;
                }
            }
        }
        return false;
    }

    /**
     * This is the actual recursive function that handles the solving
     */
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
                grid[row][column] = i;
                solve();
                grid[row][column] = 0;
            }
        }
    }

    // ------------------------- I/O -------------------------

    /**
     * Prints the current sudoku grid
     */
    private void print() 
    {
        System.out.println("-------------------------");
        for (int i = 0; i < SIZE; i++) 
        {
            System.out.print("|");
            for (int j = 0; j < SIZE; j++) 
            {
                System.out.print(" ");
                System.out.print((grid[i][j] > 0) ? grid[i][j] : " ");
                System.out.print(((j + 1) % BOXSIZE == 0) ? " |" : "");
            }
            System.out.print("\n");
            if ((i + 1) % BOXSIZE == 0) 
            {
                System.out.println("-------------------------");
            }
        }
        System.out.println();
    }

    /**
     * Reads the sudoku from the console
     */
    private void readSudoku()
    {
        // To store the current character.
        String s;

        // The characters we accept for the sudoku, '.' is an empty cell
        String vc = "123456789.";

        // We keep reading as long as we dont have a complete sudoku
        Scanner sc = new Scanner(System.in);
        for (int row = 0; row < SIZE; row++) 
        {
            for (int column = 0; column < SIZE; column++) 
            {
                // Read the next character
                do 
                {
                    s = sc.next();
                } while (vc.indexOf(s) < 0);
                // Keep going until we have a valid input.

                // Parse it to an int if it is a character we accept and not a '.'
                // An int-array is initalized with 0's so we dont actually have to do anything with '.'
                if (!s.equals(".")) 
                {
                    grid[row][column] = Integer.parseInt(s);     
                }
            }
        }

        // Doing this closes System.in. 
        // We don't want this, so we DON'T close the scanner.
        //sc.close();
    }

    // --------------- Where it all starts --------------------

    /**
     * This reads the sudoku and calls the solve method
     */
    public void solveIt() 
    {
        System.out.println("Enter sudoku:");
        grid = new int[SIZE][SIZE];
        readSudoku();

        System.out.println("Started solving");
        solve();
        System.out.println("Found " + solutionnr + " solution" + (solutionnr != 1 ? "s" : ""));
    }

    public static void main(String[] args) 
        throws InterruptedException, IOException
    {
        Sudoku s = new Sudoku();
        s.solveIt();

        Thread.sleep(500);
        System.out.println("Press enter to exit");
        System.in.read();
    }
}
