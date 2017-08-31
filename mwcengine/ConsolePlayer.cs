using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWCChessEngine
{
    public class ConsolePlayer : Agent
    {
        public static RawAction readMove(string algNotation, Position node)
        {
            RawAction a;

            string line = algNotation;

            line = line.ToLower();

            if (line.Contains("melee"))
            {
                a = readMeleeAction(node, line);
            }
            else if (line.Contains("cannon"))
            {
                a = readCannonAction(node, line);
            }
            else if (line.Contains("-"))
            {
                a = readMovementAction(node, line);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid action.");
            }

            bool legal = node.isLegal(a);

            if (!legal)
            {
                throw new ArgumentException("Illegal action.");
            }

            return a;   
        }

        public RawAction chooseMove(Position node)
        {
            Console.Out.Write("Move: ");

            string line = Console.In.ReadLine();

            RawAction rw;

            line = line.ToLower();

            try
            {
                if (line.Contains("melee"))
                {
                    rw = readMeleeAction(node, line);
                }
                else if (line.Contains("cannon"))
                {
                    rw = readCannonAction(node, line);
                }
                else if (line.Contains("-"))
                {
                    rw = readMovementAction(node, line);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Invalid action.");
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.Out.WriteLine();
                Console.Out.WriteLine("Unrecognized action format. Movement looks like: 'a4-a5', Melee: 'melee e6', Cannon: 'a3 cannon f8'");
                return chooseMove(node);
            }

            bool legal = node.isLegal(rw);

            if(!legal)
            {
                Console.Out.WriteLine();
                Console.Out.WriteLine("Illegal action. You may not move into any occupied square.");
                Console.Out.WriteLine("In order to melee attack, you must have equal or more strength than your opponent.");
                Console.Out.WriteLine("In order to cannon attack, the ray from you to your target and beyond must contain only your target.");
                return chooseMove(node);
            }

            return rw;   
        }

        public static RawAction readMeleeAction(Position node, string line)
        {
            // 01234567
            // melee fr

            if(line == null || line.Length < 8)
            {
                throw new ArgumentOutOfRangeException("Invalid action.");
            }

            int color = node.getPriorityColor();

            string algCoor = line.Substring(6, 2);
            int ordCoor;

            try
            {
                ordCoor = StUtility.getOrdinalCoords(algCoor);
            }
            catch(Exception)
            {
                throw new ArgumentOutOfRangeException("Invalid action.");
            }


            RawAction rw = new RawAction();
            rw.targetCoordinates = ordCoor;
            rw.actingColor = color;
            rw.actionType = ActionType.melee;

            return rw;
        }

        public static RawAction readCannonAction(Position node, string line)
        {
            // 012345678901
            // fr cannon fr

            if(line == null || line.Length < 12)
            {
                throw new ArgumentOutOfRangeException("Invalid action.");
            }

            int color = node.getPriorityColor();

            string cannonAlg = line.Substring(0, 2);
            string targetAlg = line.Substring(10, 2);

            int cannonOrd;
            int targetOrd;

            try
            {
                cannonOrd = StUtility.getOrdinalCoords(cannonAlg);
                targetOrd = StUtility.getOrdinalCoords(targetAlg);
            }
            catch (Exception)
            {
                throw new ArgumentOutOfRangeException("Invalid action.");
            }

            RawAction rw = new RawAction();
            rw.actionType = ActionType.cannon;
            rw.sourceCoordinates = cannonOrd;
            rw.targetCoordinates = targetOrd;
            rw.actingColor = color;

            return rw;
        }

        public static RawAction readMovementAction(Position node, string line)
        {
            // 01234
            // fr-fr

            if (line == null || line.Length < 5)
            {
                throw new ArgumentOutOfRangeException("Invalid action.");
            }

            int color = node.getPriorityColor();

            string fromAlg = line.Substring(0, 2);
            string toAlg = line.Substring(3, 2);

            int fromOrd;
            int toOrd;

            try
            {
                fromOrd = StUtility.getOrdinalCoords(fromAlg);
                toOrd = StUtility.getOrdinalCoords(toAlg);
            }
            catch (Exception)
            {
                throw new ArgumentOutOfRangeException("Invalid action.");
            }

            RawAction rw = new RawAction();
            rw.actionType = ActionType.movement;
            rw.sourceCoordinates = fromOrd;
            rw.targetCoordinates = toOrd;
            rw.actingColor = color;

            return rw;
        }
    }
}
