# SudokuModel
Modeled a Sudoku in C# as a class library. For use with apps that deal with Sudoku.

Sudoku is the class intended for use as an application. SudokuCell and SudokuCellCollection are exposed for use as needed. 

#####Sudoku Properties:#####

Cells: A List<SudokuCell> containing all 81 cells in the Sudoku.
Rows: A List<SudokuCellCollection> containing all 9 rows in the Sudoku.
Columns: A List<SudokuCellCollection> containing all 9 columns in the Sudoku.
Boxes: A List<SudokuCellCollection> containing all 9 boxes in the sudoku.

#####Sudoku Methods#####

public bool Validate(): Returns true if the puzzle is valid, false if not. An invalid puzzle will be a puzzle where an incorrect value was placed in a Cell (2 or more of the same value in any SudokuCellCollection), or any Cell has no more possibilities based on other placements.

public bool IsSolved(): Returns true if the puzzle is fully solved.

public SudokuCell MinimumPossibilities(): Returns the single cell that has the least amount of possibilities based on other placements. Useful for identifying which cells have sole candidates.

public Sudoku Copy(): Returns a new Sudoku object that is a deep copy of this.

public Sudoku Solve(): Returns a new Sudoku object that is fully solved based on this. If no solution can be found, returns null.

public override string ToString(): The Sudoku represented as a string. Useful for debugging and console output.

######SudokuCell Properties#####

Row: A SudokuCellCollection object representing the member row.
Column: A SudokuCellCollection object representing the member column.
Box: A SudokuCellCollection object representing the member box.
Value: The int representation of the value of the SudokuCell. A value of 0 indicates no placement has been made. A value of 1 through 9 indicates the SudokuCell is locked to that placement.

#####SudokuCell Methods#####

public SudokuCell(SudokuCellCollection row, SudokuCellCollection column, SudokuCellCollection box): Constructor. Member row, column, and box references are required.

public List<int> Possibilities(): Returns a List<int> collection of possible value placements for this SudokuCell based on placements in the member row, column, and box.

#####SudokuCellCollection Properties#####

Cells: A List<SudokuCell> collection of all Cells in this SudokuCellCollection object.
Index: An int representing a unique index identifier.

#####SudokuCellCollection Methods#####

public SudokuCellCollection(int index): Constructor. Index is required for identification.

public bool Validate(): Returns true if the SudokuCellCollection contains all unique placements, false if not.

public List<int> Possibilities(): Returns a collection of values that need to be placed within this collection.

public bool Crosshatch(): Scans for unique candidate Cells where the only option for any placement is a single cell. Returns true if a unique candidate was placed in any cell, false if not.
