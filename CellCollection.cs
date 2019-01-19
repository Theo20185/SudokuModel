using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SudokuModel
{
    public class CellCollection
    {
        public List<Cell> Cells { get; }
        public int Index { get; }

        public CellCollection(int index)
        {
            Cells = new List<Cell>();
            Index = index;
        }

        public bool Validate()
        {
            var counts = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            foreach (Cell cell in Cells)
                if (cell.Value > 0)
                    counts[cell.Value - 1]++;

            foreach (int count in counts)
                if (count > 1)
                    return false;

            return true;
        }

        public List<int> Possibilities()
        {
            var result = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            foreach (Cell cell in Cells)
            {
                if (result.Contains(cell.Value))
                    result.Remove(cell.Value);
            }

            return result;
        }

        public bool Crosshatch()
        {
            for (int i = 1; i <= 9; i++)
            {
                var count = 0;
                Cell tCell = null;

                foreach (Cell cell in Cells)
                {
                    if (cell.Value == 0 && cell.Possibilities().Contains(i))
                    {
                        count++;
                        tCell = cell;
                    }
                }

                if (count == 1 && tCell != null)
                {
                    Debug.WriteLine("\nPlacing value {2} in cell ({0},{1}) from crosshatch process.", tCell.Row.Index, tCell.Column.Index, i);

                    tCell.Value = i;
                    return true;
                }
            }

            return false;
        }
    }
}
