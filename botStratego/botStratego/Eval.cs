using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stratego
{
    class Eval
    {
        public static double eval(Player playerA, Player playerB, Game plop)
        {
            /*
              EVAL() function to estimate utility
              weighted sum of differences between the amount of each piece possessed by each player,
              and the difference between the average flag distance for each player.
              +++ Assumes playerA is MAX +++
             */
            // First define weights
            //Getting warnings that these variables are never used, where would we be able to implement them?
           

            double distanceWeight = 0.001; // what is good value for this?
            int[] p1Totals = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // total amount of each piece type lost
            int[] p2Totals = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] differences = new int[12];
            double[] weights = { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 20, 50 }; // adjust these weights
            //int marshalDiff;
            //int generalDiff;
            //int colonelDiff;

            List<List<Piece>> pieceLists = new List<List<Piece>>();
            //player1/2Lost class doesnt exist
            //need to either make, or get that information somehow
            pieceLists.Add(plop.player1Lost);
            pieceLists.Add(plop.player2Lost);
            // count up totals
            foreach (var pieces in pieceLists)
            {
                int[] totals = new int[12];
                foreach (var piece in pieces)
                {
                    switch (piece.pieceName)
                    {
                        case piecesTypes.Marshal:
                            totals[0]++; break;
                        case piecesTypes.General:
                            totals[1]++; break;
                        case piecesTypes.Colonel:
                            totals[2]++; break;
                        case piecesTypes.Major:
                            totals[3]++; break;
                        case piecesTypes.Captain:
                            totals[4]++; break;
                        case piecesTypes.Lieutenant:
                            totals[5]++; break;
                        case piecesTypes.Sergeant:
                            totals[6]++; break;
                        case piecesTypes.Miner:
                            totals[7]++; break;
                        case piecesTypes.Scout:
                            totals[8]++; break;
                        case piecesTypes.Spy:
                            totals[9]++; break;
                        case piecesTypes.Bomb:
                            totals[10]++; break;
                        case piecesTypes.Flag:
                            totals[11]++; break;
                    }
                    if (pieces == plop.player1Lost)
                        p1Totals = totals;
                    else
                        p2Totals = totals;
                }

            }

            double sum = 0.0;
            for (int i = 0; i < 12; i++)
            {
                int difference;
                // Negative because this is based on LOST PIECES
                // positive difference means lost more, so BAD
                difference = -1 * (p1Totals[i] - p2Totals[i]);
                sum += weights[i] * difference;
            }
            // subtract because we want smaller average distance
            //sum -= distanceWeight * averageDistance(playerA, playerB);
            // or do the difference? always the same???
            //Console.WriteLine(distanceWeight * (averageDistance(playerA, playerB) - averageDistance(playerB, playerA)));
            //sum += distanceWeight * -1 * ((averageDistance(playerA, playerB) - averageDistance(playerB, playerA)));

            // or do we use total distances?
            //sum =+ distanceWeight * -1 * ((distanceMetric(playerA, playerB) - distanceMetric(playerB, playerA)));
            sum -= distanceWeight * distanceMetric(playerA, playerB,plop);




            return sum;
        }
        //static double averageDistance(Player playerA, Player playerB)
        //{
        //    /*
        //      Average Manhattan Distance of all pieces belonging to playerA to playerB's flag
        //    */
        //    Game plop = new Game(playerA, playerB);
        //    SpaceType player;
        //    if (Player == playerA)
        //    {
        //        player = SpaceType.Player1;
        //    }
        //    else
        //    {
        //        player = SpaceType.Player2;
        //    }
        //    int total = 0; // total of all distances
        //    int count = 0; // number of pieces being considered
        //    for (int i = 0; i < 10; i++)
        //    {
        //        for (int j = 0; j < 10; j++)
        //        {
        //            if (plop.initialGrid.mainGrid[i, j]._type == player)
        //            {
        //                // need to make sure we only consider pieces that can move/capture the flag
        //                if (!((plop.initialGrid.mainGrid[i, j]._piece.pieceName == piecesTypes.Bomb) ||
        //                      (plop.initialGrid.mainGrid[i, j]._piece.pieceName == piecesTypes.Flag)))
        //                {
        //                    total += distance(i, j, playerB.flagPos.row, playerB.flagPos.col);
        //                    count++;
        //                }
        //            }
        //        }
        //    }
        //    return total / count; // return the average
        //}

        //playerA now max
        //playerB now min
        static int distanceMetric(Player max, Player min, Game plop)
        {
            /*
              sums up Manhattan distances of each players pieces to opposing player's flag
            */
            SpaceType player;
            player = SpaceType.Player1;

            int total = 0; // total of all distances
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (plop.initialGrid.mainGrid[i, j]._type == player)
                    {
                        // need to make sure we only consider pieces that can move/capture the flag
                        if (!((plop.initialGrid.mainGrid[i, j]._piece.pieceName == piecesTypes.Bomb) ||
                              (plop.initialGrid.mainGrid[i, j]._piece.pieceName == piecesTypes.Flag)))
                        {
                            total += distance(i, j, min.flagPos.row, min.flagPos.col);

                        }
                    }
                }
            }
            return total;
        }
        static int distance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }
        /*
        static int distance(Position pos1, Position pos2)
        {
            return Math.Abs(pos1.row - pos2.row) + Math.Abs(pos1.col - pos2.col);
        }
        */

    }
}