using System.Text;

class Ruleset
{

    public HashSet<Move> Moves { get; private set; }


    public GameResult GetPlayerResult(Move playerMove, Move enemyMove)
        => playerMove.Beats.Contains(enemyMove) ? GameResult.Win : playerMove.BeatenBy.Contains(enemyMove) ? GameResult.Lose : GameResult.Draw;

    public Ruleset(string[] moveNames)
    {

        ValidateMovenames(moveNames);

        BuildRuleset(moveNames);


    }

    private void ValidateMovenames(string[] moveNames)
    {
        const int minMovesCount = 3;

        StringBuilder errorMessages = new();

        int length = moveNames.Length; 

        if (length < minMovesCount)
        {
            errorMessages.AppendLine($"Too few possible moves ({length}). Try to add {minMovesCount - length} more.");
        }
        if (length % 2 == 0 && length != 0)
        {
            errorMessages.AppendLine($"Odd number of possible moves ({length}). Try to add or remove one.");
        }
        if (moveNames.Distinct().Count() != length)
        {
            errorMessages.AppendLine($"Non-unique values were chosen as the names of the moves: {string.Join(", ", moveNames.GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key))}. Try to be more original.");
        }

        if (errorMessages.Length != 0)
        {
            throw new ArgumentException(errorMessages.ToString());
        }

    }

    private void BuildRuleset(string[] moveNames)
    {

        var moves = moveNames.Select((name, number) => new Move(name, number + 1)).ToArray();

        int length = moves.Length;

        for (int startIndex = 0; startIndex < length; startIndex++)
        {

            int center = (startIndex + length / 2) % length;
            var central = moves[center];
            bool passedCenter = false;

            for (int offset = 0; offset < length; offset++)
            {

                var move = moves[(startIndex + offset) % length];
                if (!passedCenter)
                {

                    if (move.Equals(central))
                    {
                        passedCenter = true;

                    }
                    else
                    {
                        central.Beats.Add(move);
                    }
                }
                else
                {
                    central.BeatenBy.Add(move);
                }
            }
        }


        Moves = new(moves);


    }



}
