using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWCChessEngine
{
    public struct RawAction
    {
        public int sourceCoordinates;
        public int targetCoordinates;
        public int actingColor;
        public ActionType actionType;

        public string showNotation(Position p)
        {
            string sourceAlg;
            string targetAlg;

            sourceAlg = StUtility.getAlgebraicCoords(sourceCoordinates);
            targetAlg = StUtility.getAlgebraicCoords(targetCoordinates);

            switch (actionType)
            {    
                case ActionType.movement:
                    return (sourceAlg + "-" + targetAlg);
                case ActionType.melee:
                    return ("melee " + targetAlg);
                case ActionType.cannon:
                    return (sourceAlg + " cannon " + targetAlg);
                default:
                    return "";
            }
        }
    }

    public enum ActionType
    {
        none,
        movement,
        melee,
        cannon
    }
}
