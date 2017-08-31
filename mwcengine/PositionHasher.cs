using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWCChessEngine
{
    public class PositionHasher
    {
        private ulong sideToMoveRand;
        private ulong[,] pieceRand;

        private Random rnm;

        public PositionHasher()
        {
            rnm = new Random();

            pieceRand = new ulong[64, 12];

            sideToMoveRand = getRand64();

            for(int i = 0; i < 64; i++)
            {
                for(int j = 0; j < 12; j++)
                {
                    pieceRand[i, j] = getRand64();
                }
            }
        }

        public ulong getRand64()
        {
            byte[] buf = new byte[8];
            rnm.NextBytes(buf);

            ulong v = BitConverter.ToUInt64(buf, 0);
            return v;
        }

        public ulong hash(Position position)
        {
            ulong mask = 0UL;
            
            List<int>[] bboards = new List<int>[12];

            bboards[0] = Position.getBitPositions(position.pawns & position.white);
            bboards[1] = Position.getBitPositions(position.pawns & position.black);
            bboards[2] = Position.getBitPositions(position.knights & position.white);
            bboards[3] = Position.getBitPositions(position.knights & position.black);
            bboards[4] = Position.getBitPositions(position.bishops & position.white);
            bboards[5] = Position.getBitPositions(position.bishops & position.black);
            bboards[6] = Position.getBitPositions(position.rooks & position.white);
            bboards[7] = Position.getBitPositions(position.rooks & position.black);
            bboards[8] = Position.getBitPositions(position.queens & position.white);
            bboards[9] = Position.getBitPositions(position.queens & position.black);
            bboards[10] = Position.getBitPositions(position.kings & position.white);
            bboards[11] = Position.getBitPositions(position.kings & position.black);

            if(position.getPriorityColor() == 0x8)
            {
                mask ^= sideToMoveRand;
            }

            for(int i = 0; i < 12; i++)
            {
                for(int j = 0; j < bboards[i].Count; j++)
                {
                    mask ^= pieceRand[bboards[i][j], i];
                }
            }

            return mask;
        }
    }
}
