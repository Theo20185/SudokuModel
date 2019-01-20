using System;
using System.Collections.Generic;
using System.Text;
using SudokuModel;

namespace SudokuCLI
{
    public class Program
    {
        private static Puzzle _sudoku;

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var load = args[0];
            }

            _sudoku = new Puzzle();
            var input = "";

            while (!string.Equals(input, "quit", StringComparison.OrdinalIgnoreCase))
            {
                Header();

                Cell minimumPossibilitiesCell = _sudoku.MinimumPossibilities();

                if (minimumPossibilitiesCell != null)
                {
                    Console.WriteLine("\nMinimum Possibilities (Row, Column): ({0}, {1}) {2}",
                        minimumPossibilitiesCell.Row.Index,
                        minimumPossibilitiesCell.Column.Index,
                        PossibilitiesToString(minimumPossibilitiesCell.Possibilities()));
                }

                Console.WriteLine("\nCommand\tDescription");
                Console.WriteLine("'QUIT'\tExit the Sudoku CLI.");
                Console.WriteLine("'NEW'\tCreate a new Sudoku.");
                Console.WriteLine("'FILL'\tFill the entire puzzle with values.");
                Console.WriteLine("'CELL'\tPull information on a cell with options to set value.");
                Console.WriteLine("'SOLVE'\tValidate and solve the current puzzle.");

                Console.Write("\n>");

                input = Console.ReadLine();

                Console.WriteLine();

                switch(input.ToLower())
                {
                    case "new":
                        _sudoku = new Puzzle();
                        Console.WriteLine("New puzzle created.");
                        break;

                    case "fill":
                        Fill();
                        break;

                    case "cell":
                        Cell();
                        break;

                    case "solve":
                        Solve();
                        break;

                    default:
                        Console.WriteLine("Unknown command. Type 'HELP' for a list of commands.");
                        break;

                }
            }
        }

        private static void Header()
        {
            Console.Clear();
            Console.WriteLine("Sudoku CLI");

            Console.WriteLine("\n" + _sudoku.ToString());
        }

        private static void Fill()
        {
            var input = "";
            
            foreach (Row row in _sudoku.Rows)
            {
                foreach (Column column in _sudoku.Columns)
                {                    
                    var inputInt = -1;
                    Cell cell = _sudoku.Cells.Find(x => x.Row.Index == row.Index && x.Column.Index == column.Index);

                    var possibilities = cell.Possibilities();

                    while (inputInt == -1)
                    {
                        Header();

                        Console.WriteLine("Command\tDescription");
                        Console.WriteLine("'#'\tAny valid integer that is a possibility. Sets the cell value.");
                        Console.WriteLine("'STOP'\tStops the fill process.");
                        Console.WriteLine("\nCell (Row, Column): ({0},{1})", cell.Row.Index, cell.Column.Index);
                        Console.WriteLine("\nPossibilities: " + PossibilitiesToString(possibilities));
                        Console.Write("\n>");

                        input = Console.ReadLine();

                        if (string.Equals(input, "stop", StringComparison.OrdinalIgnoreCase))
                            return;

                        int.TryParse(input, out inputInt);
                    }

                    if (possibilities.Contains(inputInt))
                        cell.Value = inputInt;
                }
            }
        }

        private static void Cell()
        {
            var input = "";

            while (!string.Equals(input, "stop", StringComparison.OrdinalIgnoreCase))
            {
                var rowIndex = -1;
                var columnIndex = -1;

                while (_sudoku.Rows.Find(x => x.Index == rowIndex) == null)
                {
                    Header();

                    Console.WriteLine("Command\tDescription");
                    Console.WriteLine("'RETURN'\tReturn to main menu.");
                    Console.WriteLine("'#'\tValid index.");

                    Console.Write("\nRow: >");
                    input = Console.ReadLine();

                    if (string.Equals(input, "stop", StringComparison.OrdinalIgnoreCase))
                        return;

                    int.TryParse(input, out rowIndex);
                }

                while (_sudoku.Columns.Find(x => x.Index == columnIndex) == null)
                {
                    Header();

                    Console.WriteLine("Command\tDescription");
                    Console.WriteLine("'STOP'\tReturn to main menu.");
                    Console.WriteLine("'#'\tValid index.");

                    Console.WriteLine("\nRow: {0}", rowIndex);
                    Console.Write("Column: >");
                    input = Console.ReadLine();

                    if (string.Equals(input, "stop", StringComparison.OrdinalIgnoreCase))
                        return;

                    int.TryParse(input, out columnIndex);
                }

                while (!string.Equals(input, "stop", StringComparison.OrdinalIgnoreCase))
                {
                    var cell = _sudoku.Cells.Find(x => x.Row.Index == rowIndex && x.Column.Index == columnIndex);
                    var possibilities = cell.Possibilities();

                    Header();

                    Console.WriteLine("Command\tDescription");
                    Console.WriteLine("'STOP'\tReturn to main menu.");
                    Console.WriteLine("'#'\tValid possibility.");

                    Console.WriteLine("\nRow: {0}", cell.Row.Index);
                    Console.WriteLine("Column: {0}", cell.Column.Index);
                    Console.WriteLine("Box: {0}", cell.Box.Index);

                    Console.WriteLine("\nPossibilities: " + PossibilitiesToString(possibilities));

                    Console.Write("\n>");
                    input = Console.ReadLine();

                    var inputInt = -1;
                    int.TryParse(input, out inputInt);

                    if (possibilities.Contains(inputInt))
                    {
                        cell.Value = inputInt;
                        return;
                    }
                }
            }
        }

        private static void Solve()
        {
            Header();

            Console.Out.WriteLine("Solving...");

            var preSolve = DateTime.Now;
            var solution = _sudoku.Solve();
            var postSolve = DateTime.Now;
            var span = postSolve - preSolve;

            if (solution != null)
                _sudoku = solution;

            Header();

            if (solution != null)
                Console.Out.Write("\nFound solution in: {0} seconds.", span.TotalSeconds.ToString());
            else
                Console.Out.Write("\nError: Unable to find solution.");

            Console.Out.Write(" Press enter to continue...");
            Console.ReadLine();
        }

        private static string PossibilitiesToString(List<int> possibilities)
        {
            StringBuilder result = new StringBuilder();

            foreach (int possibility in possibilities)
            {
                if (result.Length == 0)
                    result.Append(possibility);
                else
                    result.Append(", " + possibility);
            }

            result.Insert(0, "{ ");
            result.Insert(result.Length, " }");

            return result.ToString();
        }
    }
}
