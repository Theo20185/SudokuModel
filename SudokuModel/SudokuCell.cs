using System.Collections.Generic;

namespace SudokuModel
{
    public class SudokuCell
    {
        public SudokuCellCollection Row { get; }
        public SudokuCellCollection Column { get; }
        public SudokuCellCollection Box { get; }

        public int Value { get; set; }

        public SudokuCell(SudokuCellCollection row, SudokuCellCollection column, SudokuCellCollection box)
        {
            Row = row;
            Column = column;
            Box = box;
            Value = 0;
        }

        public List<int> Possibilities()
        {
            var result = new List<int>();

            if (Value == 0)
            {
                var rowPossibilities = Row.Possibilities();
                var columnPossibilities = Column.Possibilities();
                var boxPossibilities = Box.Possibilities();

                foreach (int possibility in rowPossibilities)
                {
                    if (columnPossibilities.Contains(possibility) && boxPossibilities.Contains(possibility))
                        result.Add(possibility);
                }
            }

            return result;
        }
    }
}
