using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MWCChessEngine;
using System.Linq;

namespace MWCEngineTests
{
    [TestClass]
    public class PositionTests
    {
        [TestMethod]
        public void TestLMRemoveAt1()
        {
            // remove from cannon attack, not working.
            Position p = new Position();

            p.white = 0x000000000006FDFEUL;
            p.black = 0xFEFF020000000000UL;
            p.pawns = 0x00FD02000006FC00UL;
            p.rooks = 0x8002000000000180UL;

            ActionGen ag = new ActionGen(p, 0);
            ag.generate();

            var saCannonAction
                = ag.storedActions
                     .Where(sact => sact.actionType == ActionType.cannon)
                     .FirstOrDefault();

            Rewinder wind = new Rewinder();
            wind.wind(saCannonAction, p);

            ulong black = p.black & 0x0001000000000000UL;
            ulong pawns = p.pawns & 0x0001000000000000UL;

            Assert.AreEqual(black, 0UL);
            Assert.AreEqual(pawns, 0UL);
        }

        [TestMethod]
        public void EvalTest_OneSide()
        {
            LowMemoryEngine lme = new LowMemoryEngine(new int[] { 0, 150, 700, 600, 1200, 900, 800, 0, 0, -150, -700, -600, -1200, -900, -800, 0 }, 20, 5);

            Position p = new Position();

            p.white = 0x003FUL;
            p.black = 0x0100UL;
            p.pawns = 0x0101UL;
            p.knights = 0x0002UL;
            p.bishops = 0x0004UL;
            p.rooks = 0x0008UL;
            p.queens = 0x0010UL;
            p.kings = 0x0020UL;

            int score = p.evaluate(lme);

            Assert.AreEqual(4170, score);
        }

        [TestMethod]
        public void EvalStart()
        {
            LowMemoryEngine lme = new LowMemoryEngine(new int[] { 0, 150, 700, 600, 1200, 900, 800, 0, 0, -150, -700, -600, -1200, -900, -800, 0 }, 20, 5);
            Position p = new Position();

            int score = p.evaluate(lme);

            Assert.AreEqual(0, score);
        }
        
        [TestMethod]
        public void EvalOneMove()
        {
            LowMemoryEngine lme = new LowMemoryEngine(new int[] { 0, 150, 700, 600, 1200, 900, 800, 0, 0, -150, -700, -600, -1200, -900, -800, 0 }, 20, 5);
            Position p = new Position();

            Rewinder wind = new Rewinder();
            RawAction ra = new RawAction() { actingColor = 0x0, actionType = ActionType.movement, sourceCoordinates = 12, targetCoordinates = 19 };
            wind.wind(ra, p);

            int score = p.evaluate(lme);

            // move one up, get 5 adv. rank points.
            Assert.AreEqual(5, score);
        }

    }
}
