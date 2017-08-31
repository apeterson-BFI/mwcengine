using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MWCChessEngine;
using System.Collections.Generic;

namespace MWCEngineTests
{
    [TestClass]
    public class LMEngineTests
    {
        [TestMethod]
        public void GameSituation1()
        {
            LowMemoryEngine lme = new LowMemoryEngine(new int[] { 0, 50, 700, 600, 1200, 900, 1400, 0, 0, -50, -700, -600, -1200, -900, -1400, 0 }, 50, 10);

            Position p = new Position();

            List<string> moveSet = new List<string>()
            {
                "a2-b3",
                "b1-a2",
                "e2-d3",
                "a7-b6",
                "b6-c6",
                "b8-a7",
                "d2-c3",
                "f2-e2",
                "a1-b1",
                "a7-b6",
                "a8 cannon a2",
                "b6-a6",
                "e2-d2",
                "d1-e2"
            };

            p.playMoves(moveSet);

            RawAction a = lme.chooseMove(p);

            bool condition;

            // make sure LME didn't put B1-A1, which just throws the Rook away

            if (a.actionType == ActionType.movement)
            {
                condition = (a.sourceCoordinates == 1 && a.targetCoordinates == 0);
            }
            else
            {
                condition = false;
            }

            Assert.IsFalse(condition);
        }

        [TestMethod]
        public void TestPlayAsBlackOnStartBoard()
        {
            LowMemoryEngine lme = new LowMemoryEngine(new int[] { 0, 50, 700, 600, 1200, 900, 1400, 0, 0, -50, -700, -600, -1200, -900, -1400, 0 }, 50, 10);

            Position p = new Position();
            p.parity = 3;

            // Black should be able to get to at least -20 eval on an start board, just like white can get to 20.
            RawAction a = lme.chooseMove(p);
            p.advance(a);
            a = lme.chooseMove(p);
            p.advance(a);
            a = lme.chooseMove(p);
            p.advance(a);

            int score = p.evaluate(lme);

            Assert.IsTrue(score <= -20);
        }
    }
}
