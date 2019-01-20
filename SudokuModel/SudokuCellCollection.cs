using System.Collections.Generic;

namespace SudokuModel
{
    public class SudokuCellCollection
    {
        public List<SudokuCell> Cells { get; }
        public int Index { get; }

        public SudokuCellCollection(int index)
        {
            Cells = new List<SudokuCell>();
            Index = index;
        }

        public bool Validate()
        {
            var counts = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            foreach (SudokuCell cell in Cells)
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

            foreach (SudokuCell cell in Cells)
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
                SudokuCell tCell = null;

                foreach (SudokuCell cell in Cells)
                {
                    if (cell.Value == 0 && cell.Possibilities().Contains(i))
                    {
                        count++;
                        tCell = cell;
                    }
                }

                if (count == 1 && tCell != null)
                {
                    tCell.Value = i;
                    return true;
                }
            }

            return false;
        }
    }
}
