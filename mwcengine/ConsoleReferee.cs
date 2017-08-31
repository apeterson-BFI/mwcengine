using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWCChessEngine
{
    public class ConsoleReferee
    {
        public Position canonical;
        private Agent[] agents;

        public ConsoleReferee(Agent white, Agent black)
        {
            agents = new Agent[2];
            agents[0] = white;
            agents[1] = black;

            canonical = new Position();
        }

        public EndState play(bool console)
        {
            EndState result;

            int turn = 1;
            
            while ((result = canonical.getGameStatus()) == EndState.None)
            {
                if (!doTurn(turn, console))
                {
                    return EndState.Draw;
                }

                turn++;

            }

            return result;
        }
        
        // return true if the turn can be processed
        // false if their are no actions remaining
        public bool doTurn(int turn, bool writeConsole)
        {
            RawAction next;
            Agent active;
            int index;
            string[] elip = new string[] { ".", "..." };

            int activeColor;

            activeColor = canonical.getPriorityColor();

            index = (activeColor >> 3);
            active = agents[index];
            next = active.chooseMove(canonical);

            LowMemoryEngine lme;

            // stalemate: no actions could be generated
            if (next.actionType == ActionType.none)
            {
                Console.WriteLine((activeColor == 0 ? "White" : "Black") + "was stalemated.");
                return false;
            }

            canonical.advance(next);

            if (writeConsole)
            {
                if (active is LowMemoryEngine)
                {
                    lme = active as LowMemoryEngine;

                    int eval = canonical.evaluate(lme);
                    int q = canonical.quietTime;
                    long nodes = ((LowMemoryEngine)active).nodeCount;

                    Console.WriteLine("({2:HH:mm:ss}) {0}{1} {3} (eval: {4}, qtime: {5}, nodes: {6})", turn, elip[index], DateTime.Now, next.showNotation(canonical), eval, q, nodes);
                }
                else
                {
                    Console.WriteLine("{0}{1} {2} {3}", turn, elip[index], next.showNotation(canonical), DateTime.Now.ToLongTimeString());
                }
            }

            return true;
        }

        public string getGameType()
        {
            if (agents[0] is ConsolePlayer || agents[1] is ConsolePlayer)
            {
                return "Console";
            }
            else
            {
                return "Computer";
            }

        }
    }
}
