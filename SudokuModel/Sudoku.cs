using System.Collections.Generic;
using System.Text;

namespace SudokuModel
{
    public class Sudoku
    {
        public List<SudokuCell> Cells { get; set; }
        public List<SudokuCellCollection> Rows { get; set; }
        public List<SudokuCellCollection> Columns { get; set; }
        public List<SudokuCellCollection> Boxes { get; set; }

        public Sudoku()
        {
            Cells = new List<SudokuCell>();
            Rows = new List<SudokuCellCollection>();
            Columns = new List<SudokuCellCollection>();
            Boxes = new List<SudokuCellCollection>();

            for (int i = 0; i < 9; i++)
            {
                Rows.Add(new SudokuCellCollection(i));
                Columns.Add(new SudokuCellCollection(i));
                Boxes.Add(new SudokuCellCollection(i));
            }

            foreach (SudokuCellCollection row in Rows)
            {
                foreach (SudokuCellCollection column in Columns)
                {
                    var box = Boxes.Find(x => x.Index == ((row.Index / 3) * 3) + (column.Index / 3));

                    var cell = new SudokuCell(row, column, box);

                    row.Cells.Add(cell);
                    column.Cells.Add(cell);
                    box.Cells.Add(cell);

                    Cells.Add(cell);
                }
            }
        }

        public bool Validate()
        {
            foreach (SudokuCell cell in Cells)
            {
                if (cell.Possibilities().Count == 0 && cell.Value == 0)
                    return false;
            }

            foreach (SudokuCellCollection row in Rows)
                if (!row.Validate())
                    return false;

            foreach (SudokuCellCollection column in Columns)
                if (!column.Validate())
                    return false;

            foreach (SudokuCellCollection box in Boxes)
                if (!box.Validate())
                    return false;

            return true;
        }

        public bool IsSolved()
        {
            foreach (SudokuCell cell in Cells)
                if (cell.Value == 0)
                    return false;

            return true;
        }

        public SudokuCell MinimumPossibilities()
        {
            if (IsSolved())
                return null;

            SudokuCell result = null;

            foreach (SudokuCell cell in Cells)
            {
                if (result == null && cell.Value == 0)
                    result = cell;
                else if (cell.Value == 0 && result.Possibilities().Count > cell.Possibilities().Count)
                    result = cell;
            }

            return result;
        }

        public Sudoku Copy()
        {
            var copy = new Sudoku();

            foreach (SudokuCell cell in Cells)
            {
                SudokuCellCollection row = copy.Rows.Find(x => x.Index == cell.Row.Index);
                SudokuCellCollection column = copy.Columns.Find(x => x.Index == cell.Column.Index);
                SudokuCellCollection box = copy.Boxes.Find(x => x.Index == cell.Box.Index);

                var copyCell = copy.Cells.Find(x => x.Row.Index == cell.Row.Index && x.Column.Index == cell.Column.Index);
                copyCell.Value = cell.Value;
            }

            return copy;
        }

        public Sudoku Solve()
        {
            if (!Validate())
                return null;

            if (IsSolved())
                return this;

            var crossHatch = true;
            var crossHatched = false;

            while (crossHatch)
            {
                crossHatch = false;

                foreach (SudokuCellCollection row in Rows)
                    if (row.Crosshatch())
                    {
                        crossHatch = true;
                        crossHatched = true;
                    }

                foreach (SudokuCellCollection column in Columns)
                    if (column.Crosshatch())
                    {
                        crossHatch = true;
                        crossHatched = true;
                    }

                foreach (SudokuCellCollection box in Boxes)
                    if (box.Crosshatch())
                    {
                        crossHatched = true;
                        crossHatch = true;
                    }
            }

            if (crossHatched)
                return Solve();

            var copyPuzzle = Copy();
            var possibilitiesOrdered = new List<SudokuCell>();
            
            for(int i = 1; i < 10; i++)
            {
                foreach(SudokuCell cell in copyPuzzle.Cells.FindAll(x => x.Possibilities().Count == i))
                    possibilitiesOrdered.Add(cell);
            }

            foreach (SudokuCell cell in possibilitiesOrdered)
            {
                var possibilities = cell.Possibilities();

                foreach(int possibility in possibilities)
                {
                    cell.Value = possibility;
                    var solution = copyPuzzle.Solve();

                    if (solution != null)
                        return solution;
                }
            }

            return null;
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.Append("   0 1 2   3 4 5   6 7 8\n\n");

            foreach (SudokuCellCollection row in Rows)
            {
                result.Append(row.Index + "  ");

                foreach (SudokuCell cell in row.Cells)
                {
                    if (cell.Value > 0)
                        result.Append(cell.Value + " ");
                    else
                        result.Append("* ");

                    if (cell.Column.Index == 2 || cell.Column.Index == 5)
                        result.Append("  ");
                }

                result.Append("\n");

                if (row.Index == 2 || row.Index == 5)
                    result.Append("\n");
            }

            return result.ToString();
        }
    }
}
