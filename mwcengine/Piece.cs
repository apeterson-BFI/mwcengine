using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWCChessEngine
{
    [Flags]
    public enum Piece
    {
        white = 0,
        pawn = 1,
        knight = 2,
        bishop = 3,
        rook = 4,
        queen = 5,
        king = 6,
        black = 8
    }
}
