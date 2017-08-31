using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWCChessEngine
{
    public interface IEngine
    {
        RawAction findBestAction(int depth);
        int getPieceScore(int piece);
        int getPawnNeighborWeight();
        int getRankAdvancedWeight();
    }
}
