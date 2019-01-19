# SudokuModel

Modeled a Sudoku in C# as a class library. For use with apps that deal with Sudoku.

Puzzle is the object that should be used in applications to model a puzzle. The default constructor will populate 81 Cell objects with no value. The Puzzle will also contain 9 Row objects, 9 Column objects, and 9 Box objects. Each of these objects is derived from CellCollection and are used to track which Row, Column, and Box each Cell belongs to. Each Row, Column, and Box has a unique Index value to aid with LINQ queries regarding the Cells.

Puzzle.Solve() will return a copy of the first found solution Puzzle object or null if a solution cannot be found. The Solve method utilizes a greedy algorithm. Unique candidates (also known as "crosshatching") are filled first, followed by sole candidates. After that, the algorithm will start branching by placing possibilities where it sees them. Each branch will validate the Puzzle. When the Puzzle cannot validate, the branch terminates and returns to the previous level to continue branching there.
