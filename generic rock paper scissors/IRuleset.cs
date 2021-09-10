using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generic_rock_paper_scissors
{
    interface IRuleset
    {
        ICollection<Move> Moves {  get; }

        GameResult GetPlayerResult(Move playerMove, Move enemyMove);
    }
}
