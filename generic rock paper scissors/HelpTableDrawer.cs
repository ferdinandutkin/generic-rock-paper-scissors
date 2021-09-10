using ConsoleTables;
using generic_rock_paper_scissors;

class HelpTableDrawer
{

    private readonly ConsoleTable table;
    public HelpTableDrawer(IRuleset ruleset)
    {
        var moves = ruleset.Moves;

        table = new ConsoleTable(moves.Select(move => move.ToString()).Prepend("PC \\ USER ").ToArray()).Configure(o => o.EnableCount = false);

        foreach (var movesPC in moves)
        {

            List<string> rowValues = new() { movesPC.ToString() };


            foreach (var movesUser in moves)
            {

                string cellValue = ruleset.GetPlayerResult(movesUser, movesPC).ToString();
                rowValues.Add(cellValue);
            }
            table.AddRow(rowValues.ToArray());
        }

    }

    public void Draw() => table.Write();
}
