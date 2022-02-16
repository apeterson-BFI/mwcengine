using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWCChessEngine
{
    public class Program
    {
        public static void Main()
        {
            // blank, white p, white knight, white bishop, white rook, white queen, white king, blank, blank, black p, black knight, black bishop, black rook, black queen, black king, blank,
            // Pawn neighbor bonus, Piece advanced rank bonus.


            LowMemoryEngine e = new LowMemoryEngine(new int[] { 0, 150, 700, 600, 1200, 900, 800, 0, 0, -150, -700, -600, -1200, -900, -800, 0 }, 20, 5);
            ConsolePlayer p = new ConsolePlayer();

            LowMemoryEngine e2 = new LowMemoryEngine(new int[] { 0, 150, 700, 600, 1200, 900, 800, 0, 0, -150, -700, -600, -1200, -900, -800, 0 }, 20, 5);

            ConsoleReferee r = new ConsoleReferee(e, e2);
            EndState end = r.play(true);

            if (end == EndState.WhiteWins)
            {
                Console.WriteLine("White wins.");
            }
            else if (end == EndState.BlackWins)
            {
                Console.WriteLine("Black wins.");
            }
            else if (end == EndState.Draw)
            {
                Console.WriteLine("Draw.");
            }

            Console.WriteLine("Press enter to leave the game.");

            string inp = Console.ReadLine();

            return;
        }

    }
}
