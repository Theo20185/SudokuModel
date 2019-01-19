using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuModel
{
    public class Cell
    {
        public Row Row { get; }
        public Column Column { get; }
        public Box Box { get; }

        public int Value { get; set; }

        public Cell(Row row, Column column, Box box)
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
