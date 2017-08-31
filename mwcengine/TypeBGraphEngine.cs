using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWCChessEngine
{
    // explores a small number of paths based on a graph based analysis, that transforms the chessboard into a graph structure.
    // tries melee / cannons, and avoiding enemy melee / cannons as a primary strategy.
    // otherwise setting up melee regions / against enemy region zoning.
    // and works for cannon attacks, against enemy cannon attacks.
    //
    // Melee regions are relative to an enemy piece that may potentially be a target
    // region 0 - immediately helpful.
    // region 1 - 1 move to help.
    // region 2 - 2 moves to help.
    // etc (a piece at a1 would be region 7 with respect to h8).

    // we can use these regions, and the attack / defense of the pieces on the board to determine how to approach for an attack while defending ourselves.
    // the board becomes graph centric based on the board's cartesian max-norm graph.

    // we also have to analyze the board relative to each side's cannons and the 8 rays they produce. These 32 rays determine cannon possibilities.
    // it makes sense to use one data structure to worry about movement re/ melee, and another for cannon concerns. 
    // Melee is more generally the controlling factor, as until the late game cannons mostly constrain players actions rather than forcing losses.
    // movement actions should be made with the purpose of a long term setup for melee, cannons, or avoiding an opponents setup.

    // we could even have 3 data structures, and integrate them for this strategic planning, but we need to tie MovementGraph & MeleeDistances together for pawn attack / defense
    // (a pawns move could always update cached melee strengths)
    // we need to converge these structures to handle scenarios like cannoning an enemy pawn 
    // to reduce the attack or def strength of a pawn cluster, and then following on with a melee attack, or thereby preventing an opponents' assault.
    // also, meleeing something to open up a cannon attack.
    //
    // These data structures are a start though.

    // MovementGraph
    // MeleeDistances   : list of enemy distances
    // CannonRays

    // MovementGraph : graph structure of the chessboard, with occupancy.
    // MeleeThreats : attack / def strengths, and distances between pieces.
    // CannonRays : 8 rays of each cannon and the pieces occupying those rays. "distances" to unlock a cannon attack (as in no. pieces to move to enable it).
    //
    // translations and integrations between the three.
    //
    // graphs are directionless, so how do we meld the cannonrays with movementgraph?
    // edges on the graph are always created starting with 0-N, 1-NE, 2-E, 3-SE, 4-S, 5-SW, 6-W, 7-NW
    // so navigate the node's edge list and keep each the same one.
    //
    // how do we address searching for, and avoiding dominos effects (a violent action causing follow on changes in our structures)?
    // we can expend enemy aps in our analysis, and evaluate 1, 2 ply actions and their follow on.
    //
    // we can also count both hard-obstacles (ones controlled by the rook's opposition), 
    // vs. easy-obstacles (ones controlled by the rooks own side), and consider
    // ply spending to remove hard obstacles to be a cost 1 ply and up.
    //
    // ray adjencies : the potential of the rook itself to move rather than its obstacles being moved or removed is another snag.
    // how do we determine what the rook in movement can do?
    //
    // Keep track of potential rays that are reachable by rooks, giving them a base ply spending lessened by the movement time.
    // So a set a potential rays one move away would get a budget of 2 ply instead of 3
    //
    // we could unify this data in the graph, by giving each node a melee neighbor analysis, and ray analysis.

    public class TypeBGraphEngine
    {

    }
}
