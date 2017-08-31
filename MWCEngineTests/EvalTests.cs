using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MWCChessEngine;
using System.Collections.Generic;

namespace MWCEngineTests
{
    [TestClass]
    public class EvalTests
    {
        [TestMethod]
        public void EvalBoardHalf()
        {
            // default position with no black pieces
            // material eval: 8 pawns, 2 knights, 2 bishops, 2 rooks, 1 king, 1 queen
            // pawn neighbor eval: 6 pawns with 2 neighbors each + 2 pawns with 1 neighbor each = 14 neighbor points
            // advanced rank eval: 8 pawns advanced 1 rank each = 8 adv rank points

            Position p = new Position();
            p.parity = 0;
            p.quietTime = 0;

            p.white = 0x000000000000FFFFUL;
            p.black = 0x0000001000000000UL;  // Set black to non-zero somewhere that no pieces are set at, so the end game evals don't take over.
            p.pawns = 0x00FF00000000FF00UL;   // 8-15, 48-55
            p.rooks = 0x8100000000000081UL;   // 0, 7, 56, 63
            p.knights = 0x4200000000000042UL;   // 1, 6, 57, 62
            p.bishops = 0x2400000000000024UL;   // 2, 5, 58, 61
            p.queens = 0x0800000000000008UL;   // 3, 59
            p.kings = 0x1000000000000010UL;   // 4, 60

            LowMemoryEngine lme = new LowMemoryEngine(new int[] { 0, 50, 700, 600, 1200, 900, 1400, 0, 0, -50, -700, -600, -1200, -900, -1400, 0 }, 50, 10);

            int eval = p.evaluate(lme);

            // blank, white p, white knight, white bishop, white rook, white queen, white king

            int expectedEval = 8 * lme.pieceScore[1] + 2 * lme.pieceScore[2] + 2 * lme.pieceScore[3] + 2 * lme.pieceScore[4] + lme.pieceScore[5] + lme.pieceScore[6];
            expectedEval += 14 * lme.pawnNeighborWeight;
            expectedEval += 8 * lme.advancedRankWeight;
            expectedEval -= 3 * lme.advancedRankWeight;     // black piece to avoid status evals gets +3 rank adv.

            Assert.AreEqual(expectedEval, eval);
                
        }

        // right now: A2-A3, A1-A2, B1-A1
        // doesn't seem optimal according to current weights
        [TestMethod]
        public void EvalFullBoardAfterWhiteFavMove()
        {
            Position p = new Position();

            p.LMmoveTo(8, 16);
            p.LMmoveTo(0, 8);
            p.LMmoveTo(1, 0);

            LowMemoryEngine lme = new LowMemoryEngine(new int[] { 0, 50, 700, 600, 1200, 900, 1400, 0, 0, -50, -700, -600, -1200, -900, -1400, 0 }, 50, 10);

            int eval = p.evaluate(lme);

            // expect that the 3 moves only resulted in 2 extra rank advancement credits for 20 net points.
            int expectedEval = 20;

            Assert.AreEqual(expectedEval, eval);
        }

        // d2-d3
        // f2-f3
        // d3-c4
        [TestMethod]
        public void MoveSequence1Eval()
        {
            LowMemoryEngine e = new LowMemoryEngine(new int[] { 0, 50, 700, 600, 1200, 900, 1400, 0, 0, -50, -700, -600, -1200, -900, -1400, 0 }, 50, 10);

            Position p = new Position();
            
            List<string> moves = new List<string>()
            {
                "d2-d3",
                "f2-f3",
                "d3-c4"
            };

            p.playMoves(moves);

            int eval = p.evaluate(e);

            Assert.AreEqual(-170, eval);
        }

    }
}