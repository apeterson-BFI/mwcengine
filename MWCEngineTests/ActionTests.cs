﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MWCChessEngine;
using System.Linq;

namespace MWCEngineTests
{
    [TestClass]
    public class ActionTests
    {
        [TestMethod]
        public void CannonAttackTest2()
        {
            Position p = new Position();

            p.parity = 0;
            p.quietTime = 0;
            
            p.bishops = 0UL;
            p.rooks = 1UL;
            p.kings = 0x8000000000000000UL;
            p.knights = 0UL;
            p.queens = 0UL;
            p.pawns = 0UL;

            p.white = 1UL;
            p.black = 0x8000000000000000UL;

            RawAction ca = new RawAction();
            ca.sourceCoordinates = 0;
            ca.targetCoordinates = 63;
            ca.actingColor = 0;
            ca.actionType = ActionType.cannon;

            Assert.IsTrue(p.isLegal(ca));
        }

        [TestMethod]
        public void CannonAttackGenTest1()
        {
            Position p = new Position();

            p.parity = 0;
            p.quietTime = 0;

            p.bishops = 0UL;
            p.rooks = 1UL;
            p.kings = 0x8000000000000000UL;
            p.knights = 0UL;
            p.queens = 0UL;
            p.pawns = 0UL;

            p.white = 1UL;
            p.black = 0x8000000000000000UL;

            ActionGen ag = new ActionGen(p, 0);
            ag.generate();

            var saCannonCount
                = ag.storedActions
                     .Where(sact => sact.actionType == ActionType.cannon)
                     .Count();

            Assert.AreEqual(1, saCannonCount);
        }

        [TestMethod]
        public void CannonAttackGenTestNoPawns()
        {
            Position p = new Position();

            p.white = 0x00000000000000FFUL;
            p.black = 0xFF00000000000000UL;
            p.pawns = 0x0000000000000000UL;
            

            ActionGen ag = new ActionGen(p, 0);
            ag.generate();

            var saCannonCount
                = ag.storedActions
                     .Where(sact => sact.actionType == ActionType.cannon)
                     .Count();

            Assert.AreEqual(4, saCannonCount);
        }

        [TestMethod]
        public void CannonAttackFullPositionTest()
        {
            Position p = new Position();

            p.white = 0x000000000006FDFEUL;
            p.black = 0xFEFF020000000000UL;
            p.pawns = 0x00FD02000006FC00UL;
            p.rooks = 0x8002000000000180UL;

            ActionGen ag = new ActionGen(p, 0);
            ag.generate();

            var saCannonCount
                = ag.storedActions
                     .Where(sact => sact.actionType == ActionType.cannon)
                     .Count();

            Assert.AreEqual(1, saCannonCount);
        }

        [TestMethod]
        public void CannonAttackEvalTest()
        {
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

            LowMemoryEngine lme = new LowMemoryEngine(new int[] { 0, 50, 700, 600, 1200, 900, 1400, 0, 0, -50, -700, -600, -1200, -900, -1400, 0 }, 50, 10);

            int priorEval = p.evaluate(lme);

            Rewinder wind = new Rewinder();
            wind.wind(saCannonAction, p);

            int newEval = p.evaluate(lme);

            Assert.IsTrue(newEval > priorEval);
        }

        [TestMethod]
        public void meleePawnAttackTest()
        {
            Position p = new Position();
            p.white = 0x00702UL;
            p.black = 0x20000UL;
            p.pawns = 0x00702UL;
            p.knights = 0x20000UL;
            p.bishops = 0UL;
            p.rooks = 0UL;
            p.queens = 0UL;
            p.kings = 0UL;

            p.parity = 0;
            p.quietTime = 0;

            RawAction ca = new RawAction();
            ca.sourceCoordinates = 0;
            ca.targetCoordinates = 17;
            ca.actingColor = 0;
            ca.actionType = ActionType.melee;

            Assert.IsTrue(p.isLegal(ca));
        }
    }
}
