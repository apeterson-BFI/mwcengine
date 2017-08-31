using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWCChessEngine
{
    public class LowMemoryEngine : Agent, IEngine
    {
        public int pawnNeighborWeight;
        public int advancedRankWeight;
        public int[] pieceScore;

        private Random rnm;

        public long nodeCount;

        public int nEquals;

        public Position active;

        public LowMemoryEngine(int[] pieceScore, int pawnNeighborWeight, int advancedRankWeight)
        {
            this.pieceScore = pieceScore;
            this.pawnNeighborWeight = pawnNeighborWeight;
            this.advancedRankWeight = advancedRankWeight;
            this.rnm = new Random();

            active = new Position();

            nodeCount = 0;
            nEquals = 0;
        }

        public void broadcastMove(Position node, Action move)
        {
            return;
        }

        public RawAction chooseMove(Position node)
        {
            active = node;

            nodeCount = 0;
            nEquals = 0;
            return findBestAction(6);
        }

        public RawAction findBestAction(int depth)
        {
            RawAction rw = new RawAction();

            int alpha = int.MinValue;
            int beta = int.MaxValue;

            int s;
            int activeColor = active.getPriorityColor();

            RawAction bestAction = new RawAction();

            ActionGen gen = new ActionGen(active, activeColor);
            gen.generate();

            nodeCount = gen.actionCount;

            Rewinder rewind = new Rewinder();

            for (int i = 0; i < nodeCount; i++)
            {
                rw = gen[i];

                bool goodAction = rewind.wind(rw, active);

                if (!goodAction)
                {
                    throw new ArgumentException("No RawAction action type found.");
                }

                s = AlphaBeta(depth - 1, alpha, beta);

                if (activeColor == 0)
                {
                    if (s > alpha)
                    {
                        bestAction = gen[i];
                        alpha = s;
                        nEquals = 0;
                    }
                    else if (s == alpha)
                    {
                        if (rnm.Next(nEquals + 2) == 0)
                        {
                            bestAction = gen[i];
                            alpha = s;
                        }

                        // either way, increase nEquals because the next equal valued action has to compete with the previous ones that fought
                        nEquals++;
                    }
                }
                else
                {
                    if(s < beta)
                    {
                        bestAction = gen[i];
                        beta = s;
                        nEquals = 0;
                    }
                    else if (s == beta)
                    {
                        if (rnm.Next(nEquals + 2) == 0)
                        {
                            bestAction = gen[i];
                            beta = s;
                        }

                        // either way, increase nEquals because the next equal valued action has to compete with the previous ones that fought
                        nEquals++;
                    }
                }

                rewind.unwind(rw, active);

                if (beta <= alpha)
                {
                    break;
                }
            }

            return bestAction;
        }

        private int AlphaBeta(int depth, int alpha, int beta)
        {
            if (active.white == 0)
            {
                return Int32.MinValue;
            }
            else if (active.black == 0)
            {
                return Int32.MaxValue;
            }

            // Melee - 16 targets : 16 enemy pieces
            // Cannon - 2 cannons * 8 rays per cannon = 16
            // Movement - 16 pieces * 8 directions per piece = 128
            //
            // 16 + 16 + 128 = 160
            RawAction rw = new RawAction();

            int s;

            int activeColor = active.getPriorityColor();

            ActionGen gen = new ActionGen(active, activeColor);
            gen.generate();

            nodeCount = gen.actionCount;

            if (depth == 0)
            {
                return active.evaluate(this);
            }

            // index = 0 : no actions could be generated - stalemate
            if (nodeCount == 0)
            {
                return 0;
            }

            Rewinder rewind = new Rewinder();

            for (int i = 0; i < nodeCount; i++)
            {
                rw = gen[i];

                bool goodAction = rewind.wind(rw, active);

                if (!goodAction)
                {
                    throw new ArgumentException("No RawAction action type found.");
                }

                if (depth == 1)
                {
                    s = active.evaluate(this);
                }
                else
                {
                    s = AlphaBeta(depth - 1, alpha, beta);
                }

                if (activeColor == 0 && s > alpha)
                {
                    alpha = s;
                }
                else if (activeColor == 8 && s < beta)
                {
                    beta = s;
                }

                rewind.unwind(rw, active);

                if (beta <= alpha)
                {
                    break;
                }
            }

            if (activeColor == 0)
            {
                return alpha;
            }
            else
            {
                return beta;
            }
        }

        public int getPieceScore(int piece)
        {
            return pieceScore[piece];
        }

        public int getPawnNeighborWeight()
        {
            return pawnNeighborWeight;
        }

        public int getRankAdvancedWeight()
        {
            return advancedRankWeight;
        }
    }
}
