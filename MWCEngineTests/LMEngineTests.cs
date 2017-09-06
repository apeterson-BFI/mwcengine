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
        public void Test_2Move_Combo()
        {
            // 56R
            //
            //
            //
            //
            //
            // 8P
            // 0R
            LowMemoryEngine lme = new LowMemoryEngine(new int[] { 0, 150, 700, 600, 1200, 900, 800, 0, 0, -150, -700, -600, -1200, -900, -800, 0 }, 20, 5);
            Position p = new Position();

            p.white =   0x0000000000000101UL;
            p.black =   0x0100000000000000UL;
            p.pawns =   0x0000000000000100UL;
            p.rooks =   0x0100000000000001UL;
            p.knights = 0x0000000000000000UL;
            p.bishops = 0x0000000000000000UL;
            p.queens =  0x0000000000000000UL;
            p.kings =   0x0000000000000000UL;

            RawAction m = lme.chooseMove(p);
            p.advance(m);

            m = lme.chooseMove(p);
            p.advance(m);

            int sc = p.evaluate(lme);

            Assert.AreEqual(Int32.MaxValue, sc);
        }
    }
}
