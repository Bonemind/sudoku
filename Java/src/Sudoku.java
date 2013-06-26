import java.util.Scanner;

class Sudoku {
    int SIZE = 9;     // size of the grid
    int DMAX = 9;     // maximal digit to be filled in
    int BOXSIZE = 3;  // size of the boxes (subgrids that should contain all digits)
    int[][] grid; // the puzzle grid; 0 represents empty
     
    int[][] somesudoku = new int[][] { // a challenge-sudoku from the web
        //{ 0, 6, 0,   0, 0, 1,    0, 9, 4 },    //original      
        { 0, 0, 0,   0, 0, 1,    0, 9, 4 }, //to get more solutions
        { 3, 0, 0,   0, 0, 7,    1, 0, 0 }, 
        { 0, 0, 0,   0, 9, 0,    0, 0, 0 }, 

        { 7, 0, 6,   5, 0, 0,    2, 0, 9 }, 
        { 0, 3, 0,   0, 2, 0,    0, 6, 0 }, 
        { 9, 0, 2,   0, 0, 6,    3, 0, 1 }, 

        { 0, 0, 0,   0, 5, 0,    0, 0, 0 }, 
        { 0, 0, 7,   3, 0, 0,    0, 0, 2 }, 
        { 4, 1, 0,   7, 0, 0,    0, 8, 0 }, 
    };
    
    int solutionnr = 0; //solution counter
    
    // coordinates of next empty square
    int rempty;
    int cempty;
       
    // ----------------- conflict calculation --------------------
    
    // is there a conflict when we fill in d at position r,c?
    //@pre: 1<=d && d<=DMAX && grid[r][c]==0 
    //@post: ...
    boolean givesConflict(int r, int  c, int d) {
    	return rowConflict(r, d) || colConflict(c, d) || boxConflict(r, c, d);
    }
    
    boolean rowConflict(int r, int d) {
    	for (int c = 0; c < SIZE; c++) {
    		if (grid[r][c] == d) {
    			return true;
    		}
    	}
    	return false;
    }
    
    boolean colConflict(int c, int d) {
    	for (int r = 0; r < SIZE; r++) {
    		 if (grid[r][c] == d) {
    			return true;
    		 }
    	}
    	return false;
    }
    
    boolean boxConflict(int rr, int cc, int d) {
    	rr -= rr % BOXSIZE;
    	cc -= cc % BOXSIZE;
    	for (int r = 0; r < BOXSIZE; r++) {
    		for (int c = 0; c < BOXSIZE; c++) {
    			if (grid[rr+r][cc+c] == d) {
    				return true;
    			}
    		}
    	}
    	return false;
    }

    // --------- solving ----------
       
    // finds the next empty square (in "reading order")
    // writes the coordinates in rempty and cempty
    // returns false if there is no empty square in the current grid
    // @post: returns false, if grid[x][y] != 0 for all 0 <= x < SIZE, 0 <= y < SIZE.
    //		  return true && grid[rempty][cempty] == 0, otherwise.
    boolean findEmptySquare() {
    	for (rempty = 0; rempty < SIZE; rempty++) {
    		for (cempty = 0; cempty < SIZE; cempty++) {
    			if (grid[rempty][cempty] == 0) {
    				return true;
    			}
    		}
    	}
    	return false;
    }
    
    // prints all solutions that are extensions of current grid
    void solve() {
    	if (!findEmptySquare()) {
    		solutionnr++;
			print();
			return;
		}
    	int r = rempty, c = cempty;
    	for (int i = 1; i <= DMAX; i++) {;
    		if (!givesConflict(r, c, i))
    		{
    			grid[r][c] = i;
    			solve();
    			grid[r][c] = 0;
    		}
    	}
    }
    
    // ------------------------- misc -------------------------
    
    // print the grid, 0s are printed as spaces
    // this function is not compatible with non-standard sudoku dimensions. That is, it won't look pretty.
    void print() {
    	System.out.println("-------------------------");
    	for (int i = 0; i < SIZE; i++) {
    		System.out.print("|");
    		for (int j = 0; j < SIZE; j++) {
    			System.out.print(" ");
    			System.out.print((grid[i][j] > 0) ? grid[i][j] : " ");
    			System.out.print(((j + 1) % BOXSIZE == 0) ? " |" : "");
    		}
    		System.out.print("\n");
    		if ((i + 1) % BOXSIZE == 0) {
				System.out.println("-------------------------");
    		}
    	}
    	System.out.println();
    }
     
    void readSudoku()
    {
    	Scanner sc = new Scanner(System.in);
    	String s;
    	String vc = "123456789.";
    	for (int r = 0; r < SIZE; r++) {
    		for (int c = 0; c < SIZE; c++) {
    			do {
    				s = sc.next();
    			} while (vc.indexOf(s) < 0);
    			if (!s.equals(".")) {
    				grid[r][c] = Integer.parseInt(s); 	
    			}
    		}
    	}
    }
    
    // --------------- where it all starts --------------------
    
    void solveIt() {
    	grid = new int[SIZE][SIZE];
    	readSudoku();
    	//grid = somesudoku;
    	solve();
    	System.out.println("Found " + solutionnr + " solution" + (solutionnr != 1 ? "s" : ""));
    }

    
    public static void main(String[] args) {
        new Sudoku().solveIt();
    }
}