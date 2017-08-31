using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWCChessEngine
{
    public class Rewinder
    {
        public int removedPiece;
        public int quietTimeBeforeReset;
        public bool loud;
        public bool melee;

        public Rewinder()
        {
            removedPiece = 0;
            quietTimeBeforeReset = 0;
            loud = false;
            melee = false;
        }

        public bool wind(RawAction rw, Position active)
        {
            if (rw.actionType == ActionType.melee)
            {
                removedPiece = active.LMremoveAt(rw.targetCoordinates);
                active.advanceParity();
                quietTimeBeforeReset = active.resetQuietTime();
                loud = true;
                melee = true;
                return true;
            }
            else if (rw.actionType == ActionType.cannon)
            {
                removedPiece = active.LMremoveAt(rw.targetCoordinates);
                active.advanceParity();
                quietTimeBeforeReset = active.resetQuietTime();
                loud = true;
                melee = false;
                return true;
            }
            else if (rw.actionType == ActionType.movement)
            {
                active.LMmoveTo(rw.sourceCoordinates, rw.targetCoordinates);
                active.advanceParity();
                active.advanceQuietTime();
                removedPiece = 0;
                quietTimeBeforeReset = 0;
                loud = false;
                melee = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void unwind(RawAction rw, Position active)
        {
            active.rewindParity();

            if (loud)
            {
                active.reloadQuietTime(quietTimeBeforeReset);

                if (melee)
                {
                    active.LMaddAt(rw.targetCoordinates, removedPiece);
                }
                else
                {
                    active.LMaddAt(rw.targetCoordinates, removedPiece);
                }
            }
            else
            {
                active.rewindQuietTime();
                active.LMmoveTo(rw.targetCoordinates, rw.sourceCoordinates);
            }
        }
    }
}
