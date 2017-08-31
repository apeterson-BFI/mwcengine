using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWCChessEngine
{
    public interface Action
    {
        int naturalValue(Position p, IEngine e);
        bool isLegal(Position p);
        string showNotation(Position p);
    }
}
