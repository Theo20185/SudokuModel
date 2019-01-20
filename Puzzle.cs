using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SudokuModel
{
    public class Puzzle
    {
        public List<Cell> Cells { get; set; }
        public List<Row> Rows { get; set; }
        public List<Column> Columns { get; set; }
        public List<Box> Boxes { get; set; }

        public Puzzle()
        {
            Cells = new List<Cell>();
            Rows = new List<Row>();
            Columns = new List<Column>();
            Boxes = new List<Box>();

            for (int i = 0; i < 9; i++)
            {
                Rows.Add(new Row(i));
                Columns.Add(new Column(i));
                Boxes.Add(new Box(i));
            }

            foreach (Row row in Rows)
            {
                foreach (Column column in Columns)
                {
                    var box = Boxes.Find(x => x.Index == ((row.Index / 3) * 3) + (column.Index / 3));

                    var cell = new Cell(row, column, box);

                    row.Cells.Add(cell);
                    column.Cells.Add(cell);
                    box.Cells.Add(cell);

                    Cells.Add(cell);
                }
            }
        }

        public bool Validate()
        {
            foreach (Cell cell in Cells)
            {
                if (cell.Possibilities().Count == 0 && cell.Value == 0)
                    return false;
            }

            foreach (Row row in Rows)
                if (!row.Validate())
                    return false;

            foreach (Column column in Columns)
                if (!column.Validate())
                    return false;

            foreach (Box box in Boxes)
                if (!box.Validate())
                    return false;

            return true;
        }

        public bool IsSolved()
        {
            foreach (Cell cell in Cells)
                if (cell.Value == 0)
                    return false;

            return true;
        }

        public Cell MinimumPossibilities()
        {
            if (IsSolved())
                return null;

            Cell result = null;

            foreach (Cell cell in Cells)
            {
                if (result == null && cell.Value == 0)
                    result = cell;
                else if (cell.Value == 0 && result.Possibilities().Count > cell.Possibilities().Count)
                    result = cell;
            }

            return result;
        }

        public Puzzle Copy()
        {
            var copy = new Puzzle();

            foreach (Cell cell in Cells)
            {
                Row row = copy.Rows.Find(x => x.Index == cell.Row.Index);
                Column column = copy.Columns.Find(x => x.Index == cell.Column.Index);
                Box box = copy.Boxes.Find(x => x.Index == cell.Box.Index);

                var copyCell = copy.Cells.Find(x => x.Row.Index == cell.Row.Index && x.Column.Index == cell.Column.Index);
                copyCell.Value = cell.Value;
            }

            return copy;
        }

        public Puzzle Solve()
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

                foreach (Row row in Rows)
                    if (row.Crosshatch())
                    {
                        crossHatch = true;
                        crossHatched = true;
                    }

                foreach (Column column in Columns)
                    if (column.Crosshatch())
                    {
                        crossHatch = true;
                        crossHatched = true;
                    }

                foreach (Box box in Boxes)
                    if (box.Crosshatch())
                    {
                        crossHatched = true;
                        crossHatch = true;
                    }
            }

            if (crossHatched)
                return Solve();

            var copyPuzzle = Copy();
            var possibilitiesOrdered = new List<Cell>();
            
            for(int i = 1; i < 10; i++)
            {
                foreach(Cell cell in copyPuzzle.Cells.FindAll(x => x.Possibilities().Count == i))
                    possibilitiesOrdered.Add(cell);
            }

            foreach (Cell cell in possibilitiesOrdered)
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

            foreach (Row row in Rows)
            {
                result.Append(row.Index + "  ");

                foreach (Cell cell in row.Cells)
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
