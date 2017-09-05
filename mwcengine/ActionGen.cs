using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWCChessEngine
{
    public class ActionGen
    {
        public RawAction[] storedActions;
        public Position active;

        public int activeColor;
        public int otherColor;

        private ulong friendMask;
        private ulong enemyMask;
        private ulong board;
        private ulong emptyMask;

        public int actionCount;

        public ActionGen(Position active, int activeColor)
        {
            this.active = active;

            // Melee - 16 targets : 16 enemy pieces
            // Cannon - 2 cannons * 8 rays per cannon = 16
            // Movement - 16 pieces * 8 directions per piece = 128
            // 16 + 16 + 128 = 160
            storedActions = new RawAction[160];

            this.activeColor = activeColor;
            otherColor = 8 - activeColor;

            friendMask = (activeColor == 0 ? active.white : active.black);
            enemyMask = (otherColor == 0 ? active.white : active.black);

            board = active.white | active.black;
            emptyMask = ~board;

            actionCount = 0;
        }

        public void generate()
        {
            createCannonActions();
            createMeleeActions();
            createMovementActions();
        }

        public RawAction this[int i]
        {
            get
            {
                if (i >= 160)
                {
                    throw new ArgumentOutOfRangeException("StoredActions is an 160 element array. Cannot access elements higher than 159.");
                }

                return storedActions[i];
            }
        }

        private void createCannonActions()
        {
            ulong cannons = friendMask & active.rooks;

            RawAction rw = new RawAction();

            if (cannons != 0UL)
            {
                ulong fstCannon = Position.leastSigOneBit(cannons);
                ulong lstCannon = Position.mostSigOneBit(cannons);

                if(fstCannon == lstCannon)
                {
                    lstCannon = 0UL;
                }

                if (fstCannon != 0UL)
                {
                    int fstCoor = Position.log2(fstCannon);

                    for (int i = 0; i < 8; i++)
                    {
                        rw = findCannonActionsForMask(fstCoor, Position.rayMasks[i, fstCoor]);

                        if (rw.actionType == ActionType.none)
                        {
                            continue;
                        }
                        else
                        {
                            storedActions[actionCount] = rw;
                            actionCount++;
                        }
                    }
                }

                if (lstCannon != 0UL)
                {
                    int lstCoor = Position.log2(lstCannon);

                    for (int i = 0; i < 8; i++)
                    {
                        rw = findCannonActionsForMask(lstCoor, Position.rayMasks[i, lstCoor]);

                        if (rw.actionType == ActionType.none)
                        {
                            continue;
                        }
                        else
                        {
                            storedActions[actionCount] = rw;
                            actionCount++;
                        }
                    }
                }
            }
        }

        private void createMeleeActions()
        {
            ulong eWork;
            ulong eWork2;
            ulong eWork3;
            int el2;

            RawAction rw = new RawAction();

            eWork = enemyMask;

            while (eWork != 0UL)
            {
                eWork2 = Position.leastSigOneBit(eWork);
                eWork -= eWork2;

                el2 = Position.log2(eWork2);

                eWork3 = Position.neighbors[el2] & friendMask;

                if (eWork3 == 0UL)
                {
                    continue;
                }

                rw.actingColor = activeColor;
                rw.targetCoordinates = el2;
                rw.actionType = ActionType.melee;

                if (active.isMeleeActionLegal(rw))
                {
                    storedActions[actionCount] = rw;
                    actionCount++;
                }
            }
        }

        private void createMovementActions()
        {
            ulong mWork;
            ulong mWork2;
            ulong tWork;
            ulong tWork2;
            ulong mWork3;

            int ml2;
            int tl2;

            RawAction rw = new RawAction();

            mWork = friendMask;

            while (mWork != 0UL)
            {
                mWork2 = Position.leastSigOneBit(mWork);
                mWork -= mWork2;

                ml2 = Position.log2(mWork2);

                mWork3 = Position.neighbors[ml2] & emptyMask;

                tWork = mWork3;

                while (tWork != 0UL)
                {
                    tWork2 = Position.leastSigOneBit(tWork);
                    tWork -= tWork2;

                    tl2 = Position.log2(tWork2);

                    rw.actingColor = activeColor;
                    rw.actionType = ActionType.movement;
                    rw.sourceCoordinates = ml2;
                    rw.targetCoordinates = tl2;

                    storedActions[actionCount] = rw;
                    actionCount++;
                }
            }
        }

        private RawAction findCannonActionsForMask(int sourceCoor, ulong mask)
        {
            RawAction rw = new RawAction();

            ulong work = mask & board;

            int count = Position.popCount(work);
            int targetCoor;

            if (count == 1)
            {
                work &= enemyMask;

                if (work != 0UL)
                {
                    targetCoor = Position.log2(work);

                    rw.actionType = ActionType.cannon;
                    rw.actingColor = activeColor;
                    rw.sourceCoordinates = sourceCoor;
                    rw.targetCoordinates = targetCoor;

                    return rw;
                }
            }

            rw.actionType = ActionType.none;
            return rw;
        }
    }
}
