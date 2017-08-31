using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWCChessEngine
{
    public class Position
    {
        // 0,1,2 - White's 1st, 2nd, 3rd action
        // 3,4,5 - Black's 1st, 2nd, 3rd action
        // After Black's 3rd, parity resets to 0
        public int parity;

        public int quietTime;  // 120 means a draw (20 action sets each)

        // White: 0x0
        // Black: 0x8
        // Pawn: 0x1
        // Knight: 0x2
        // Bishop: 0x3
        // Rook: 0x4
        // Queen: 0x5
        // King: 0x6
        // Piece value = color + piece
        // Test color: value & 0x8
        // Value 0x0 : empty

        //black back row:  56 57 58 59 60 61 62 63
        //black front row: 48 49 50 51 52 53 54 55
        //white front row:  8  9 10 11 12 13 14 15
        //white back row:   0  1  2  3  4  5  6  7
        
        public ulong white;
        public ulong black;
        public ulong pawns;
        public ulong knights;
        public ulong bishops;
        public ulong rooks;
        public ulong queens;
        public ulong kings;



        #region Constants

        public static int[] whiteRankBits = new int[8];
        public static int[] blackRankBits = new int[8];

        public const int pawnNeighborBonus = 1;

        // Pawn -   0 att.  , 1 def.
        // Knight - 3 att.  , 6 def.
        // Bishop - 4 att.  , 4 def.
        // Rook -   0 att.  , 1 def.
        // Queen -  5 att.  , 6 def.
        // King -   6 att.  , 4 def.

        // n/a  - pawn - knight - bishop - rook - queen - king
        public static int[] attackStrength = new int[] { 0, 0, 3, 4, 0, 5, 6 };
        public static int[] defenseStrength = new int[] { 0, 1, 6, 4, 1, 6, 4 };

        public static ulong[] powers = new ulong[] {0x1UL, 0x2UL, 0x4UL, 0x8UL, 0x10UL, 0x20UL, 0x40UL, 0x80UL, 0x100UL, 0x200UL, 0x400UL, 0x800UL, 0x1000UL, 0x2000UL, 0x4000UL, 0x8000UL, // 15
                                                    0x10000UL, 0x20000UL, 0x40000UL, 0x80000UL, 0x100000UL, 0x200000UL, 0x400000UL, 0x800000UL, 0x1000000UL, 0x2000000UL, 0x4000000UL,      // 26
                                                    0x8000000UL, 0x10000000UL, 0x20000000UL, 0x40000000UL, 0x80000000UL, 0x100000000UL, 0x200000000UL, 0x400000000UL, 0x800000000UL,        // 35
                                                    0x1000000000UL, 0x2000000000UL, 0x4000000000UL, 0x8000000000UL, 0x10000000000UL, 0x20000000000UL, 0x40000000000UL, 0x80000000000UL,     // 43
                                                    0x100000000000UL, 0x200000000000UL, 0x400000000000UL, 0x800000000000UL, 0x1000000000000UL, 0x2000000000000UL, 0x4000000000000UL,        // 50
                                                    0x8000000000000UL, 0x10000000000000UL, 0x20000000000000UL, 0x40000000000000UL, 0x80000000000000UL, 0x100000000000000UL,                 // 56
                                                    0x200000000000000UL, 0x400000000000000UL, 0x800000000000000UL, 0x1000000000000000UL, 0x2000000000000000UL, 0x4000000000000000UL,        // 62
                                                    0x8000000000000000UL };                                                                                                                // 63

        public static ulong[] notPowers;


        public static ulong[] neighbors = new ulong[] {  0x0000000000000302UL,0x0000000000000705UL,0x0000000000000E0AUL, 0x0000000000001C14UL
                                                        ,0x0000000000003828UL,0x0000000000007050UL,0x000000000000E0A0UL
                                                        ,0x000000000000C040UL,0x0000000000030203UL,0x0000000000070507UL
                                                        ,0x00000000000E0A0EUL,0x00000000001C141CUL,0x0000000000382838UL
                                                        ,0x0000000000705070UL,0x0000000000E0A0E0UL,0x0000000000C040C0UL
                                                        ,0x0000000003020300UL,0x0000000007050700UL,0x000000000E0A0E00UL
                                                        ,0x000000001C141C00UL,0x0000000038283800UL,0x0000000070507000UL
                                                        ,0x00000000E0A0E000UL,0x00000000C040C000UL,0x0000000302030000UL
                                                        ,0x0000000705070000UL,0x0000000E0A0E0000UL,0x0000001C141C0000UL
                                                        ,0x0000003828380000UL,0x0000007050700000UL,0x000000E0A0E00000UL
                                                        ,0x000000C040C00000UL,0x0000030203000000UL,0x0000070507000000UL
                                                        ,0x00000E0A0E000000UL,0x00001C141C000000UL,0x0000382838000000UL
                                                        ,0x0000705070000000UL,0x0000E0A0E0000000UL,0x0000C040C0000000UL
                                                        ,0x0003020300000000UL,0x0007050700000000UL,0x000E0A0E00000000UL
                                                        ,0x001C141C00000000UL,0x0038283800000000UL,0x0070507000000000UL
                                                        ,0x00E0A0E000000000UL,0x00C040C000000000UL,0x0302030000000000UL
                                                        ,0x0705070000000000UL,0x0E0A0E0000000000UL,0x1C141C0000000000UL
                                                        ,0x3828380000000000UL,0x7050700000000000UL,0xE0A0E00000000000UL
                                                        ,0xC040C00000000000UL,0x0203000000000000UL,0x0507000000000000UL
                                                        ,0x0A0E000000000000UL,0x141C000000000000UL,0x2838000000000000UL
                                                        ,0x5070000000000000UL,0xA0E0000000000000UL,0x40C0000000000000UL};

        public static ulong[,] rayMasks = new ulong[,] {
            { 
            0x0101010101010100UL,0x0202020202020200UL,0x0404040404040400UL,0x0808080808080800UL,
            0x1010101010101000UL,0x2020202020202000UL,0x4040404040404000UL,0x8080808080808000UL,
            0x0101010101010000UL,0x0202020202020000UL,0x0404040404040000UL,0x0808080808080000UL,
            0x1010101010100000UL,0x2020202020200000UL,0x4040404040400000UL,0x8080808080800000UL,
            0x0101010101000000UL,0x0202020202000000UL,0x0404040404000000UL,0x0808080808000000UL,
            0x1010101010000000UL,0x2020202020000000UL,0x4040404040000000UL,0x8080808080000000UL,
            0x0101010100000000UL,0x0202020200000000UL,0x0404040400000000UL,0x0808080800000000UL,
            0x1010101000000000UL,0x2020202000000000UL,0x4040404000000000UL,0x8080808000000000UL,
            0x0101010000000000UL,0x0202020000000000UL,0x0404040000000000UL,0x0808080000000000UL,
            0x1010100000000000UL,0x2020200000000000UL,0x4040400000000000UL,0x8080800000000000UL,
            0x0101000000000000UL,0x0202000000000000UL,0x0404000000000000UL,0x0808000000000000UL,
            0x1010000000000000UL,0x2020000000000000UL,0x4040000000000000UL,0x8080000000000000UL,
            0x0100000000000000UL,0x0200000000000000UL,0x0400000000000000UL,0x0800000000000000UL,
            0x1000000000000000UL,0x2000000000000000UL,0x4000000000000000UL,0x8000000000000000UL,
            0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,
            0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL},

            {
            0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,
            0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,
            0x0000000000000001UL,0x0000000000000002UL,0x0000000000000004UL,0x0000000000000008UL,
            0x0000000000000010UL,0x0000000000000020UL,0x0000000000000040UL,0x0000000000000080UL,
            0x0000000000000101UL,0x0000000000000202UL,0x0000000000000404UL,0x0000000000000808UL,
            0x0000000000001010UL,0x0000000000002020UL,0x0000000000004040UL,0x0000000000008080UL,
            0x0000000000010101UL,0x0000000000020202UL,0x0000000000040404UL,0x0000000000080808UL,
            0x0000000000101010UL,0x0000000000202020UL,0x0000000000404040UL,0x0000000000808080UL,
            0x0000000001010101UL,0x0000000002020202UL,0x0000000004040404UL,0x0000000008080808UL,
            0x0000000010101010UL,0x0000000020202020UL,0x0000000040404040UL,0x0000000080808080UL,
            0x0000000101010101UL,0x0000000202020202UL,0x0000000404040404UL,0x0000000808080808UL,
            0x0000001010101010UL,0x0000002020202020UL,0x0000004040404040UL,0x0000008080808080UL,
            0x0000010101010101UL,0x0000020202020202UL,0x0000040404040404UL,0x0000080808080808UL,
            0x0000101010101010UL,0x0000202020202020UL,0x0000404040404040UL,0x0000808080808080UL,
            0x0001010101010101UL,0x0002020202020202UL,0x0004040404040404UL,0x0008080808080808UL,
            0x0010101010101010UL,0x0020202020202020UL,0x0040404040404040UL,0x0080808080808080UL},

            {
            0x0000000000000000UL,0x0000000000000001UL,0x0000000000000003UL,0x0000000000000007UL,
            0x000000000000000FUL,0x000000000000001FUL,0x000000000000003FUL,0x000000000000007FUL,
            0x0000000000000000UL,0x0000000000000100UL,0x0000000000000300UL,0x0000000000000700UL,
            0x0000000000000F00UL,0x0000000000001F00UL,0x0000000000003F00UL,0x0000000000007F00UL,
            0x0000000000000000UL,0x0000000000010000UL,0x0000000000030000UL,0x0000000000070000UL,
            0x00000000000F0000UL,0x00000000001F0000UL,0x00000000003F0000UL,0x00000000007F0000UL,
            0x0000000000000000UL,0x0000000001000000UL,0x0000000003000000UL,0x0000000007000000UL,
            0x000000000F000000UL,0x000000001F000000UL,0x000000003F000000UL,0x000000007F000000UL,
            0x0000000000000000UL,0x0000000100000000UL,0x0000000300000000UL,0x0000000700000000UL,
            0x0000000F00000000UL,0x0000001F00000000UL,0x0000003F00000000UL,0x0000007F00000000UL,
            0x0000000000000000UL,0x0000010000000000UL,0x0000030000000000UL,0x0000070000000000UL,
            0x00000F0000000000UL,0x00001F0000000000UL,0x00003F0000000000UL,0x00007F0000000000UL,
            0x0000000000000000UL,0x0001000000000000UL,0x0003000000000000UL,0x0007000000000000UL,
            0x000F000000000000UL,0x001F000000000000UL,0x003F000000000000UL,0x007F000000000000UL,
            0x0000000000000000UL,0x0100000000000000UL,0x0300000000000000UL,0x0700000000000000UL,
            0x0F00000000000000UL,0x1F00000000000000UL,0x3F00000000000000UL,0x7F00000000000000UL},

            {
            0x00000000000000FEUL,0x00000000000000FCUL,0x00000000000000F8UL,0x00000000000000F0UL,
            0x00000000000000E0UL,0x00000000000000C0UL,0x0000000000000080UL,0x0000000000000000UL,
            0x000000000000FE00UL,0x000000000000FC00UL,0x000000000000F800UL,0x000000000000F000UL,
            0x000000000000E000UL,0x000000000000C000UL,0x0000000000008000UL,0x0000000000000000UL,
            0x0000000000FE0000UL,0x0000000000FC0000UL,0x0000000000F80000UL,0x0000000000F00000UL,
            0x0000000000E00000UL,0x0000000000C00000UL,0x0000000000800000UL,0x0000000000000000UL,
            0x00000000FE000000UL,0x00000000FC000000UL,0x00000000F8000000UL,0x00000000F0000000UL,
            0x00000000E0000000UL,0x00000000C0000000UL,0x0000000080000000UL,0x0000000000000000UL,
            0x000000FE00000000UL,0x000000FC00000000UL,0x000000F800000000UL,0x000000F000000000UL,
            0x000000E000000000UL,0x000000C000000000UL,0x0000008000000000UL,0x0000000000000000UL,
            0x0000FE0000000000UL,0x0000FC0000000000UL,0x0000F80000000000UL,0x0000F00000000000UL,
            0x0000E00000000000UL,0x0000C00000000000UL,0x0000800000000000UL,0x0000000000000000UL,
            0x00FE000000000000UL,0x00FC000000000000UL,0x00F8000000000000UL,0x00F0000000000000UL,
            0x00E0000000000000UL,0x00C0000000000000UL,0x0080000000000000UL,0x0000000000000000UL,
            0xFE00000000000000UL,0xFC00000000000000UL,0xF800000000000000UL,0xF000000000000000UL,
            0xE000000000000000UL,0xC000000000000000UL,0x8000000000000000UL,0x0000000000000000UL},

            {
            0x0000000000000000UL,0x0000000000000100UL,0x0000000000010200UL,0x0000000001020400UL,
            0x0000000102040800UL,0x0000010204081000UL,0x0001020408102000UL,0x0102040810204000UL,
            0x0000000000000000UL,0x0000000000010000UL,0x0000000001020000UL,0x0000000102040000UL,
            0x0000010204080000UL,0x0001020408100000UL,0x0102040810200000UL,0x0204081020400000UL,
            0x0000000000000000UL,0x0000000001000000UL,0x0000000102000000UL,0x0000010204000000UL,
            0x0001020408000000UL,0x0102040810000000UL,0x0204081020000000UL,0x0408102040000000UL,
            0x0000000000000000UL,0x0000000100000000UL,0x0000010200000000UL,0x0001020400000000UL,
            0x0102040800000000UL,0x0204081000000000UL,0x0408102000000000UL,0x0810204000000000UL,
            0x0000000000000000UL,0x0000010000000000UL,0x0001020000000000UL,0x0102040000000000UL,
            0x0204080000000000UL,0x0408100000000000UL,0x0810200000000000UL,0x1020400000000000UL,
            0x0000000000000000UL,0x0001000000000000UL,0x0102000000000000UL,0x0204000000000000UL,
            0x0408000000000000UL,0x0810000000000000UL,0x1020000000000000UL,0x2040000000000000UL,
            0x0000000000000000UL,0x0100000000000000UL,0x0200000000000000UL,0x0400000000000000UL,
            0x0800000000000000UL,0x1000000000000000UL,0x2000000000000000UL,0x4000000000000000UL,
            0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,
            0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL},

            {
            0x8040201008040200UL,0x0080402010080400UL,0x0000804020100800UL,0x0000008040201000UL,
            0x0000000080402000UL,0x0000000000804000UL,0x0000000000008000UL,0x0000000000000000UL,
            0x4020100804020000UL,0x8040201008040000UL,0x0080402010080000UL,0x0000804020100000UL,
            0x0000008040200000UL,0x0000000080400000UL,0x0000000000800000UL,0x0000000000000000UL,
            0x2010080402000000UL,0x4020100804000000UL,0x8040201008000000UL,0x0080402010000000UL,
            0x0000804020000000UL,0x0000008040000000UL,0x0000000080000000UL,0x0000000000000000UL,
            0x1008040200000000UL,0x2010080400000000UL,0x4020100800000000UL,0x8040201000000000UL,
            0x0080402000000000UL,0x0000804000000000UL,0x0000008000000000UL,0x0000000000000000UL,
            0x0804020000000000UL,0x1008040000000000UL,0x2010080000000000UL,0x4020100000000000UL,
            0x8040200000000000UL,0x0080400000000000UL,0x0000800000000000UL,0x0000000000000000UL,
            0x0402000000000000UL,0x0804000000000000UL,0x1008000000000000UL,0x2010000000000000UL,
            0x4020000000000000UL,0x8040000000000000UL,0x0080000000000000UL,0x0000000000000000UL,
            0x0200000000000000UL,0x0400000000000000UL,0x0800000000000000UL,0x1000000000000000UL,
            0x2000000000000000UL,0x4000000000000000UL,0x8000000000000000UL,0x0000000000000000UL,
            0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,
            0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL},

            {0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,
            0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,
            0x0000000000000000UL,0x0000000000000001UL,0x0000000000000002UL,0x0000000000000004UL,
            0x0000000000000008UL,0x0000000000000010UL,0x0000000000000020UL,0x0000000000000040UL,
            0x0000000000000000UL,0x0000000000000100UL,0x0000000000000201UL,0x0000000000000402UL,
            0x0000000000000804UL,0x0000000000001008UL,0x0000000000002010UL,0x0000000000004020UL,
            0x0000000000000000UL,0x0000000000010000UL,0x0000000000020100UL,0x0000000000040201UL,
            0x0000000000080402UL,0x0000000000100804UL,0x0000000000201008UL,0x0000000000402010UL,
            0x0000000000000000UL,0x0000000001000000UL,0x0000000002010000UL,0x0000000004020100UL,
            0x0000000008040201UL,0x0000000010080402UL,0x0000000020100804UL,0x0000000040201008UL,
            0x0000000000000000UL,0x0000000100000000UL,0x0000000201000000UL,0x0000000402010000UL,
            0x0000000804020100UL,0x0000001008040201UL,0x0000002010080402UL,0x0000004020100804UL,
            0x0000000000000000UL,0x0000010000000000UL,0x0000020100000000UL,0x0000040201000000UL,
            0x0000080402010000UL,0x0000100804020100UL,0x0000201008040201UL,0x0000402010080402UL,
            0x0000000000000000UL,0x0001000000000000UL,0x0002010000000000UL,0x0004020100000000UL,
            0x0008040201000000UL,0x0010080402010000UL,0x0020100804020100UL,0x0040201008040201UL},

            {
            0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,
            0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,0x0000000000000000UL,
            0x0000000000000002UL,0x0000000000000004UL,0x0000000000000008UL,0x0000000000000010UL,
            0x0000000000000020UL,0x0000000000000040UL,0x0000000000000080UL,0x0000000000000000UL,
            0x0000000000000204UL,0x0000000000000408UL,0x0000000000000810UL,0x0000000000001020UL,
            0x0000000000002040UL,0x0000000000004080UL,0x0000000000008000UL,0x0000000000000000UL,
            0x0000000000020408UL,0x0000000000040810UL,0x0000000000081020UL,0x0000000000102040UL,
            0x0000000000204080UL,0x0000000000408000UL,0x0000000000800000UL,0x0000000000000000UL,
            0x0000000002040810UL,0x0000000004081020UL,0x0000000008102040UL,0x0000000010204080UL,
            0x0000000020408000UL,0x0000000040800000UL,0x0000000080000000UL,0x0000000000000000UL,
            0x0000000204081020UL,0x0000000408102040UL,0x0000000810204080UL,0x0000001020408000UL,
            0x0000002040800000UL,0x0000004080000000UL,0x0000008000000000UL,0x0000000000000000UL,
            0x0000020408102040UL,0x0000040810204080UL,0x0000081020408000UL,0x0000102040800000UL,
            0x0000204080000000UL,0x0000408000000000UL,0x0000800000000000UL,0x0000000000000000UL,
            0x0002040810204080UL,0x0004081020408000UL,0x0008102040800000UL,0x0010204080000000UL,
            0x0020408000000000UL,0x0040800000000000UL,0x0080000000000000UL,0x0000000000000000UL}
        };

        const ulong notAFile = 0xfefefefefefefefeUL; // ~0x0101010101010101
        const ulong notHFile = 0x7f7f7f7f7f7f7f7fUL; // ~0x8080808080808080

        const ulong k1 = 0x5555555555555555UL; /*  -1/3   */
        const ulong k2 = 0x3333333333333333UL; /*  -1/5   */
        const ulong k4 = 0x0f0f0f0f0f0f0f0fUL; /*  -1/17  */
        const ulong kf = 0x0101010101010101UL; /*  -1/255 */

        #endregion

        // Default constructor builds initial position
        public Position()
        {
            parity = 0;
            quietTime = 0;

            white =     0x000000000000FFFFUL;
            black =     0xFFFF000000000000UL;
            pawns =     0x00FF00000000FF00UL;   // 8-15, 48-55
            rooks =     0x8100000000000081UL;   // 0, 7, 56, 63
            knights =   0x4200000000000042UL;   // 1, 6, 57, 62
            bishops =   0x2400000000000024UL;   // 2, 5, 58, 61
            queens =    0x0800000000000008UL;   // 3, 59
            kings =     0x1000000000000010UL;   // 4, 60

            if (notPowers == null)
            {
                notPowers = new ulong[64];

                for (int i = 0; i < 64; i++)
                {
                    notPowers[i] = ~powers[i];
                }
            }
        }

        public Position(Position baseState)
        {
            this.parity = baseState.parity;
            this.quietTime = baseState.quietTime;

            this.white = baseState.white;
            this.black = baseState.black;
            this.rooks = baseState.rooks;
            this.knights = baseState.knights;
            this.bishops = baseState.bishops;
            this.queens = baseState.queens;
            this.kings = baseState.kings;
            this.pawns = baseState.pawns;
        }

        public void playMoves(List<string> algNotationList)
        {
            RawAction a;

            for (int i = 0; i < algNotationList.Count; i++)
            {
                a = ConsolePlayer.readMove(algNotationList[i], this);

                advance(a);
            }
        }

        public void advance(RawAction a)
        {
            switch (a.actionType)
            {
                case ActionType.melee:
                    LMremoveAt(a.targetCoordinates);
                    advanceParity();
                    resetQuietTime();
                    break;
                case ActionType.cannon:
                    LMremoveAt(a.targetCoordinates);
                    advanceParity();
                    resetQuietTime();
                    break;
                case ActionType.movement:
                    LMmoveTo(a.sourceCoordinates, a.targetCoordinates);
                    advanceParity();
                    advanceQuietTime();
                    break;
                default:
                    throw new Exception("Invalid positional advance. Action of unknown type.");
            }

        }

        public static int popCount(ulong x) 
        {
            x = x - ((x >> 1) & k1); /* put count of each 2 bits into those 2 bits */
            x = (x & k2) + ((x >> 2) & k2); /* put count of each 4 bits into those 4 bits */
            x = (x + (x >> 4)) & k4; /* put count of each 8 bits into those 8 bits */
            x = (x * kf) >> 56; /* returns 8 most significant bits of x + (x<<8) + (x<<16) + (x<<24) + ...  */
            return (int)x;
        }

        public static ulong leastSigOneBit(ulong x)
        {
            ulong r = x & (0 - x);

            return r;
        }

        public static ulong mostSigOneBit(ulong x)
        {
            x |= x >> 32;
            x |= x >> 16;
            x |= x >> 8;
            x |= x >> 4;
            x |= x >> 2;
            x |= x >> 1;

            ulong r = (x >> 1) + 1UL;

            return r;
        }

        private int getPieceScore(IEngine e)
        {
            // White: 0x0
            // Black: 0x8
            // Pawn: 0x1
            // Knight: 0x2
            // Bishop: 0x3
            // Rook: 0x4
            // Queen: 0x5
            // King: 0x6
            // Piece value = color + piece
            // Test color: value & 0x8
            // Value 0x0 : empty
            
            int score = 0;

            ulong work = white & pawns;
            score += popCount(work) * e.getPieceScore(1);

            work = white & knights;
            score += popCount(work) * e.getPieceScore(2);

            work = white & bishops;
            score += popCount(work) * e.getPieceScore(3);

            work = white & rooks;
            score += popCount(work) * e.getPieceScore(4);

            work = white & queens;
            score += popCount(work) * e.getPieceScore(5);

            work = white & kings;
            score += popCount(work) * e.getPieceScore(6);

            work = black & pawns;
            score += popCount(work) * e.getPieceScore(9);

            work = black & knights;
            score += popCount(work) * e.getPieceScore(10);

            work = black & bishops;
            score += popCount(work) * e.getPieceScore(11);

            work = black & rooks;
            score += popCount(work) * e.getPieceScore(12);

            work = black & queens;
            score += popCount(work) * e.getPieceScore(13);

            work = black & kings;
            score += popCount(work) * e.getPieceScore(14);

            return score;
        }

        private int getPositionalScore(IEngine e)
        {
            int pawnWeight = e.getPawnNeighborWeight();

            // slide in each direction, and measure intersection with pawn board. Each intersection is a connection, worth 2*PawnNeighborWeight
            // We only need to do half the directions, because connections are symmetric
            // upleft, up, upright, and right
            // correspond to
            // downright, down, downleft and left

            int whiteCount = 0;
            int blackCount = 0;

            ulong whiteWork;
            ulong blackWork;

            ulong whitePawns = white & pawns;
            ulong blackPawns = black & pawns;

            whiteWork = nortOne(whitePawns) & whitePawns;
            blackWork = nortOne(blackPawns) & blackPawns;

            whiteCount += popCount(whiteWork);
            blackCount += popCount(blackWork);

            whiteWork = noWeOne(whitePawns) & whitePawns;
            blackWork = noWeOne(blackPawns) & blackPawns;

            whiteCount += popCount(whiteWork);
            blackCount += popCount(blackWork);

            whiteWork = noEaOne(whitePawns) & whitePawns;
            blackWork = noEaOne(blackPawns) & blackPawns;

            whiteCount += popCount(whiteWork);
            blackCount += popCount(blackWork);

            whiteWork = eastOne(whitePawns) & whitePawns;
            blackWork = eastOne(blackPawns) & blackPawns;

            whiteCount += popCount(whiteWork);
            blackCount += popCount(blackWork);

            return (whiteCount - blackCount) * 2 * pawnWeight;
        }

        private int getAdvancedRankScore(IEngine e)
        {
            int score = 0;
            int advWeight = e.getRankAdvancedWeight();

            ulong xW = white;
            ulong xB = black;

            int bitsW;
            int bitsB;

            xW = xW - ((xW >> 1) & k1); /* put count of each 2 bits into those 2 bits */
            xB = xB - ((xB >> 1) & k1);
            xW = (xW & k2) + ((xW >> 2) & k2); /* put count of each 4 bits into those 4 bits */
            xB = (xB & k2) + ((xB >> 2) & k2); /* put count of each 4 bits into those 4 bits */
            xW = (xW + (xW >> 4)) & k4; /* put count of each 8 bits into those 8 bits */
            xB = (xB + (xB >> 4)) & k4; /* put count of each 8 bits into those 8 bits */

            for (int row = 0; row < 8; row++)
            {
                bitsW = (int)(xW & 0xFFUL);
                bitsB = (int)(xB & 0xFFUL);

                xW = xW >> 8;
                xB = xB >> 8;

                score += advWeight * (row * (bitsW + bitsB) - 7 * bitsB);
            }

            return score;
        }

        public int evaluate(IEngine e)
        {
            int score = 0;

            if (white == 0UL)
            {
                return Int32.MinValue;
            }
            else if (black == 0UL)
            {
                return Int32.MaxValue;
            }

            score += getPieceScore(e);

            score += getPositionalScore(e);

            score += getAdvancedRankScore(e);
    
            return score;
        }

        public void advanceParity()
        {
            parity++;

            if (parity > 5)
            {
                parity = 0;
            }
        }

        public void rewindParity()
        {
            parity--;

            if (parity < 0)
            {
                parity = 5;
            }
        }

        public int resetQuietTime()
        {
            int q = quietTime;

            quietTime = 0;
            return q;
        }

        public void advanceQuietTime()
        {
            quietTime++;
        }

        public void rewindQuietTime()
        {
            quietTime--;
        }

        public void reloadQuietTime(int q)
        {
            quietTime = q;
        }

        public EndState getGameStatus()
        {
            if (quietTime > 240)
            {
                throw new Exception("Invalid position state: Quiet Time has advanced beyond the maximum allowed without a draw being declared");
            }

            if (white == 0UL && black == 0UL)
            {
                throw new Exception("Invalid position state: Both boards are empty. No sequence of moves could lead to this state.");
            }

            if (quietTime == 240)
            {
                return EndState.Draw;
            }

            if (white == 0UL)
            {
                return EndState.BlackWins;
            }

            if (black == 0UL)
            {
                return EndState.WhiteWins;
            }

            return EndState.None;
        }

        public int getPriorityColor()
        {
            if (parity >= 3)
            {
                return 0x8;
            }
            else
            {
                return 0x0;
            }
        }

        public bool isSquareOccupied(int coor)
        {
            ulong allmask = white | black;

            return ((allmask & powers[coor]) != 0UL);
        }

        public bool isOccupiedByColor(int coor, int color)
        {
            ulong colormask = (color == 0 ? white : black);

            return((colormask & powers[coor]) == 0UL);
        }

        // 0-7 (a-h traditional)
        public static int getColumnIndex(int coor)
        {
            return (coor % 8);
        }

        // 0-7 (1-8 traditional)
        public static int getRowIndex(int coor)
        {
            return (coor / 8);
        }

        public static int getOpposingColor(int color)
        {
            return (8 - color);
        }

        public bool isLegal(RawAction a)
        {
            switch (a.actionType)
            {
                case ActionType.cannon: return isCannonActionLegal(a);
                case ActionType.melee: return isMeleeActionLegal(a);
                case ActionType.movement: return isMovementActionLegal(a);
                default: return false;
            }
        }

        private bool isCannonActionLegal(RawAction a)
        {
            if (getPriorityColor() != a.actingColor)
            {
                return false;
            }

            ulong colorMask = (a.actingColor == 0 ? white : black);
            ulong allMask = white | black;
            ulong enemyMask = (a.actingColor == 0 ? black : white);
            bool foundRaySolution = false;

            ulong work;

            work = colorMask & Position.powers[a.sourceCoordinates];

            // cannon coors unoccupied
            if (work == 0UL)
            {
                return false;
            }

            for (int i = 0; i < 8; i++)
            {
                work = rayMasks[i, a.sourceCoordinates];
                foundRaySolution = checkActionWRayMask(work, allMask, enemyMask, a);

                if (foundRaySolution)
                {
                    return true;
                }
            }

            // if none of the rays were successful, then false
            return false;
        }

        internal bool isMeleeActionLegal(RawAction a)
        {
            if (getPriorityColor() != a.actingColor)
            {
                return false;
            }

            int attackScore = 0;
            int defenseScore = 0;
            int opposingColor = 8 - a.actingColor;

            ulong ourColorMask = (a.actingColor == 0 ? white : black);
            ulong theirColorMask = (opposingColor == 0 ? white : black);

            ulong work = theirColorMask & Position.powers[a.targetCoordinates];
            ulong pieceWork;

            bool defenseDone = false;

            // enemy not at target coordinates
            if (work == 0UL)
            {
                return false;
            }

            pieceWork = work & pawns;

            if (pieceWork != 0UL)
            {
                defenseScore += defenseStrength[1];
                defenseScore += getPawnsNeighborhoodStrength(pieceWork, pawns & theirColorMask);
                defenseDone = true;
            }

            pieceWork = work & knights;

            if (!defenseDone && pieceWork != 0UL)
            {
                defenseScore += defenseStrength[2];
                defenseDone = true;
            }

            pieceWork = work & bishops;

            if (!defenseDone && pieceWork != 0UL)
            {
                defenseScore += defenseStrength[3];
                defenseDone = true;
            }

            pieceWork = work & rooks;

            if (!defenseDone && pieceWork != 0UL)
            {
                defenseScore += defenseStrength[4];
                defenseDone = true;
            }

            pieceWork = work & queens;

            if (!defenseDone && pieceWork != 0UL)
            {
                defenseScore += defenseStrength[5];
                defenseDone = true;
            }

            pieceWork = work & kings;

            if (!defenseDone && pieceWork != 0UL)
            {
                defenseScore += defenseStrength[6];
                defenseDone = true;
            }

            work = Position.neighbors[a.targetCoordinates] & ourColorMask;

            pieceWork = work & pawns;

            if (pieceWork != 0UL)
            {
                attackScore += Position.popCount(pieceWork) * attackStrength[1];
                attackScore += getPawnsNeighborhoodStrength(pieceWork, pawns & ourColorMask);
            }

            pieceWork = work & knights;

            if (pieceWork != 0UL)
            {
                attackScore += Position.popCount(pieceWork) * attackStrength[2];
            }

            pieceWork = work & bishops;

            if (pieceWork != 0UL)
            {
                attackScore += Position.popCount(pieceWork) * attackStrength[3];
            }

            pieceWork = work & rooks;

            if (pieceWork != 0UL)
            {
                attackScore += Position.popCount(pieceWork) * attackStrength[4];
            }

            pieceWork = work & queens;

            if (pieceWork != 0UL)
            {
                attackScore += Position.popCount(pieceWork) * attackStrength[5];
            }

            pieceWork = work & kings;

            if (pieceWork != 0UL)
            {
                attackScore += Position.popCount(pieceWork) * attackStrength[6];
            }

            return attackScore >= defenseScore;
        }

        private bool isMovementActionLegal(RawAction a)
        {
            if (getPriorityColor() != a.actingColor)
            {
                return false;
            }

            ulong colorMask = (a.actingColor == 0 ? white : black);
            ulong allMask = white | black;

            ulong work = powers[a.sourceCoordinates] & colorMask;

            // if currentCoor is not occupied by a piece of acting color.
            if (work == 0UL)
            {
                return false;
            }

            work = Position.powers[a.targetCoordinates] & allMask;

            // target location is occupied.
            if (work != 0UL)
            {
                return false;
            }

            return true;
        }

        public int getPawnsNeighborhoodStrength(ulong contact, ulong allPawns)
        {
            ulong work;
            int l2;
            int score = 0;
            ulong work2;

            while (contact != 0UL)
            {
                work = leastSigOneBit(contact);
                contact -= work;

                l2 = log2(work);

                work2 = neighbors[l2] & allPawns;
                score += popCount(work2);
            }

            return score;
        }

        private bool checkActionWRayMask(ulong mask, ulong board, ulong enemyMask, RawAction a)
        {
            ulong work = mask & board;

            int count = Position.popCount(work);

            int targetCoor;

            if (count == 1)
            {
                work &= enemyMask;

                if (work != 0UL)
                {
                    targetCoor = Position.log2(work);

                    if (targetCoor == a.targetCoordinates)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public int LMremoveAt(int coor)
        {
            int pieceVal;

            ulong work = white & powers[coor];

            if (work != 0UL)
            {
                pieceVal = 0;
                white = white ^ work;      // reset the set bit
            }
            else
            {
                work = black & powers[coor];

                if (work != 0UL)
                {
                    pieceVal = 8;
                    black = black ^ work;
                }
                else
                {
                    throw new ArgumentException("Piece to remove not found in color masks.");
                }
            }

            work = pawns & powers[coor];

            if (work != 0UL)
            {
                pawns ^= work;
                pieceVal += 1;
                return pieceVal;
            }

            work = knights & powers[coor];

            if (work != 0UL)
            {
                knights ^= work;
                pieceVal += 2;
                return pieceVal;
            }

            work = bishops & powers[coor];

            if (work != 0UL)
            {
                bishops ^= work;
                pieceVal += 3;
                return pieceVal;
            }

            work = rooks & powers[coor];

            if (work != 0UL)
            {
                rooks ^= work;
                pieceVal += 4;
                return pieceVal;
            }

            work = queens & powers[coor];

            if (work != 0UL)
            {
                queens ^= work;
                pieceVal += 5;
                return pieceVal;
            }

            work = kings & powers[coor];

            if (work != 0UL)
            {
                kings ^= work;
                pieceVal += 6;
                return pieceVal;
            }

            throw new ArgumentException("Piece to remove not found in piece masks.");
        }

        public void LMaddAt(int coor, int pieceVal)
        {
            if (pieceVal < 8)
            {
                white |= powers[coor];
            }
            else
            {
                black |= powers[coor];
            }

            pieceVal = pieceVal % 8;

            switch (pieceVal)
            {
                case 1: pawns |= powers[coor]; break;
                case 2: knights |= powers[coor]; break;
                case 3: bishops |= powers[coor]; break;
                case 4: rooks |= powers[coor]; break;
                case 5: queens |= powers[coor]; break;
                case 6: kings |= powers[coor]; break;
                default: throw new ArgumentException("Invalid pieceVal (1-6)");
            }
        }

        public void LMmoveTo(int currentCoor, int desiredCoor)
        {
            white = ulongMoveCoor(white, currentCoor, desiredCoor);
            black = ulongMoveCoor(black, currentCoor, desiredCoor);
            pawns = ulongMoveCoor(pawns, currentCoor, desiredCoor);
            knights = ulongMoveCoor(knights, currentCoor, desiredCoor);
            bishops = ulongMoveCoor(bishops, currentCoor, desiredCoor);
            rooks = ulongMoveCoor(rooks, currentCoor, desiredCoor);
            queens = ulongMoveCoor(queens, currentCoor, desiredCoor);
            kings = ulongMoveCoor(kings, currentCoor, desiredCoor);
        }

        private ulong ulongMoveCoor(ulong baseVal, int currentCoor, int desiredCoor)
        {
            ulong work = (baseVal & powers[currentCoor]) >> currentCoor;    // if bit is set, work = 2^currentCoor. Shifting by currentCoor produces 1
                                                                            // if bit is not set, 0
            ulong xorCurr = powers[currentCoor] * work;
            ulong xorDes = powers[desiredCoor] * work;

            work = baseVal;

            work ^= xorCurr;
            work ^= xorDes;

            return work;
        }

        public ulong getNeighbors(int coor)
        {
            return neighbors[coor];
        }

        public static ulong getHighNZByte(ulong source)
        {
            ulong high;
            ulong low;

            high = source & 0xFFFFFFFF00000000UL;
            low =  source & 0x00000000FFFFFFFFUL;

            high = (high == 0UL ? low : (high >> 32));

            high = source & 0x00000000FFFF0000UL;
            low =  source & 0x000000000000FFFFUL;

            high = (high == 0UL ? low : (high >> 16));

            high = source & 0x000000000000FF00UL;
            low =  source & 0x00000000000000FFUL;

            high = (high == 0UL ? low : (high >> 8));

            return high;
        }

        public static ulong kingAttacks(ulong kingSet) 
        {
           ulong attacks = eastOne(kingSet) | westOne(kingSet);
           kingSet    |= attacks;
           attacks    |= nortOne(kingSet) | soutOne(kingSet);
           return attacks;
        }

        public static ulong eastOne (ulong b) {return (b << 1) & notAFile;}
        public static ulong noEaOne (ulong b) {return (b << 9) & notAFile;}
        public static ulong soEaOne (ulong b) {return (b >> 7) & notAFile;}
        public static ulong westOne (ulong b) {return (b >> 1) & notHFile;}
        public static ulong soWeOne (ulong b) {return (b >> 9) & notHFile;}
        public static ulong noWeOne (ulong b) {return (b << 7) & notHFile;}

        public static ulong soutOne (ulong b) {return  b >> 8;}
        public static ulong nortOne (ulong b) {return  b << 8;}

        public int countFriendlyPawnNeighbors(int coor, int color)
        {
            ulong neigh = neighbors[coor];
            ulong pawnBits = (color == 0 ? white : black) & pawns;

            ulong andBits = neigh & pawnBits;

            return popCount(andBits);
        }

        public static bool onBoard(int row, int column)
        {
            if (row < 0 || row > 7)
            {
                return false;
            }

            if (column < 0 || column > 7)
            {
                return false;
            }

            return true;
        }

        public static bool atOtherLocation(int activeRow, int activeColumn, int otherRow, int otherColumn)
        {
            return (activeRow == otherRow && activeColumn == otherColumn);
        }

        public static List<int> getBitPositions(ulong mask)
        {
            ulong work;

            List<int> workList = new List<int>();

            while (mask != 0UL)
            {
                work = leastSigOneBit(mask);
                
                mask -= work;

                workList.Add(log2(work));
            }

            return workList;
        }

        public static int log2(ulong val)
        {
            if(val == 0UL)
            {
                throw new ArgumentException("Log of 0 is invalid.");
            }

            int counter = 0;

            ulong low = val & 0x00000000FFFFFFFFUL;

            if (low == 0UL)
            {
                counter += 32;
                val = val >> 32;
            }

            low = val & 0x000000000000FFFFUL;

            if (low == 0UL)
            {
                counter += 16;
                val = val >> 16;
            }

            low = val & 0x00000000000000FFUL;

            if (low == 0UL)
            {
                counter += 8;
                val = val >> 8;
            }

            while (val > 1UL)
            {
                val = val >> 1;
                counter++;
            }

            return counter;
        }

        public int[] bitboardToBoardArray()
        {
            int[] board = new int[64];

            ulong whitePawn = white & pawns ;    // 1
            ulong whiteKnight = white & knights;  // 2
            ulong whiteBishop = white & bishops;  // 3
            ulong whiteRook = white & rooks;    // 4
            ulong whiteQueen = white & queens;  // 5
            ulong whiteKing = white & kings;    // 6
            ulong blackPawn = black & pawns;    // 9
            ulong blackKnight = black & knights;  // 10
            ulong blackBishop = black & bishops;  // 11
            ulong blackRook = black & rooks;    // 12
            ulong blackQueen = black & queens;   // 13
            ulong blackKing = black & kings;    // 14

            List<int> coors = getBitPositions(whitePawn);
            indirectArraySet(board, coors, 1);

            coors = getBitPositions(whiteKnight);
            indirectArraySet(board, coors, 2);

            coors = getBitPositions(whiteBishop);
            indirectArraySet(board, coors, 3);

            coors = getBitPositions(whiteRook);
            indirectArraySet(board, coors, 4);

            coors = getBitPositions(whiteQueen);
            indirectArraySet(board, coors, 5);

            coors = getBitPositions(whiteKing);
            indirectArraySet(board, coors, 6);

            coors = getBitPositions(blackPawn);
            indirectArraySet(board, coors, 9);

            coors = getBitPositions(blackKnight);
            indirectArraySet(board, coors, 10);

            coors = getBitPositions(blackBishop);
            indirectArraySet(board, coors, 11);

            coors = getBitPositions(blackRook);
            indirectArraySet(board, coors, 12);

            coors = getBitPositions(blackQueen);
            indirectArraySet(board, coors, 13);

            coors = getBitPositions(blackKing);
            indirectArraySet(board, coors, 14);

            return board;
        }

        private void indirectArraySet(int[] arr, List<int> indices, int val)
        {
            for (int i = 0; i < indices.Count; i++)
            {
                arr[indices[i]] = val;
            }
        }

        public override string ToString()
        {
            string text = "";

            int[] board = bitboardToBoardArray();

            int color = getPriorityColor();
            int actionsLeft = 3 - (parity % 3);
            string colorName;

            if (color == 0)
            {
                colorName = "White's";
            }
            else
            {
                colorName = "Black's";
            }

            text += colorName + " turn. " + actionsLeft + " actions left. \n ";
            text += "Quiet Time (120 = draw): " + quietTime + "\n ";
            
            for(int r = 0; r < 8; r++)
            {
                for(int c = 0; c < 8; c++)
                {
                    text += " " + board[r * 8 + c].ToString();
                }

                text += "\n";
            }

            return text;
        }
    }
}
