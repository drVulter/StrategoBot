using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stratego
{
    class Eval
    {
        static double eval(Player playerA, Player playerB)
        {
            Game plop = new Game(playerA, playerB);
            /*
              EVAL() function to estimate utility
              weighted sum of differences between the amount of each piece possessed by each player,
              and the difference between the average flag distance for each player.
              +++ Assumes playerA is MAX +++
             */
            // First define weights
            //Getting warnings that these variables are never used, where would we be able to implement them?
            double marshal = 10;
            double general = 9;
            double colonel = 8;
            double major = 7;
            double captain = 6;
            double lieutenant = 5;
            double sergeant = 4;
            double miner = 3; // Adjust?
            double scout = 2;
            double spy = 1; // adjust
            double bomb = 20; // adjust
            double flag = 50; // adjust? should be arbitrarily high

            double distanceWeight = 0.001; // what is good value for this?
            int[] p1Totals = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // total amount of each piece type lost
            int[] p2Totals = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] differences = new int[12];
            double[] weights = { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 20, 50 }; // adjust these weights
            int marshalDiff;
            int generalDiff;
            int colonelDiff;

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
                if (playerA == p1) // MAX
                    difference = -1 * (p1Totals[i] - p2Totals[i]);
                else
                    difference = -1 * (p2Totals[i] - p1Totals[i]);
                sum += weights[i] * difference;
            }
            // subtract because we want smaller average distance
            //sum -= distanceWeight * averageDistance(playerA, playerB);
            // or do the difference? always the same???
            //Console.WriteLine(distanceWeight * (averageDistance(playerA, playerB) - averageDistance(playerB, playerA)));
            //sum += distanceWeight * -1 * ((averageDistance(playerA, playerB) - averageDistance(playerB, playerA)));

            // or do we use total distances?
            //sum =+ distanceWeight * -1 * ((distanceMetric(playerA, playerB) - distanceMetric(playerB, playerA)));
            sum -= distanceWeight * distanceMetric(playerA, playerB);




            return sum;
        }
        static double averageDistance(Player playerA, Player playerB)
        {
            /*
              Average Manhattan Distance of all pieces belonging to playerA to playerB's flag
            */
            Game plop = new Game(playerA, playerB);
            SpaceType player;
            if (Player == playerA)
            {
                player = SpaceType.Player1;
            }
            else
            {
                player = SpaceType.Player2;
            }
            int total = 0; // total of all distances
            int count = 0; // number of pieces being considered
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
                            total += distance(i, j, playerB.flagPos.row, playerB.flagPos.col);
                            count++;
                        }
                    }
                }
            }
            return total / count; // return the average
        }

        static int distanceMetric(Player playerA, Player playerB)
        {
            /*
              sums up Manhattan distances of each players pieces to opposing player's flag
            */
            SpaceType player;
            if (playerA == p1)
            {
                player = SpaceType.Player1;
            }
            else
            {
                player = SpaceType.Player2;
            }
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
                            total += distance(i, j, playerB.flagPos.row, playerB.flagPos.col);

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
        static void setUpBoard()
        {
            /* Initial Set up
               1. FLAG: goes in back two rows
               2. BOMBS: 2-3 go near FLAG, rest go in rows 2-3
               3. SCOUTS: Go in front 2 rows
               4. MINERS: Go anywhere but front row
               5. Everything else is RANDOM
             */
            // First determine flag position 0 <= flagPos <= 19
            // in back two rows
            Random rnd = new Random(); // random number generator

            // Make lists of all available positions for each player, updated during process
            List<Position> availableP1 = new List<Position>();
            List<Position> availableP2 = new List<Position>();
            // lists of all used positions
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    //
                    availableP2.Add(new Position(i, j));
                    availableP1.Add(new Position(9 - i, 9 - j));
                }
            }
            //Console.WriteLine(availableP2.Count);
            /* Flags First  playerXPieces[3] ------------------------------------------ */
            //Position flag2Pos = new Position();
            int flag2Index = rnd.Next(19);
            Position flag2Pos = availableP2[flag2Index];
            p2.flagPos = flag2Pos; // SETTING FOR THE PLAYER
            //flag2Pos.row = flag2Index / 10;
            //flag2Pos.col = flag2Index % 10;
            plop.setPieceOnGrid(plop.player2Pieces[3], flag2Pos);
            availableP2.RemoveAt(flag2Index);
            //Position flag1Pos = new Position();
            int flag1Index = rnd.Next(19);
            Position flag1Pos = availableP1[flag1Index];
            p1.flagPos = flag1Pos; // SETTING FOR THE PLAYER!!!!!
            //flag1Pos.row = flag1Index / 10;
            //flag1Pos.col = flag1Index % 10;
            plop.setPieceOnGrid(plop.player1Pieces[3], flag1Pos);
            availableP1.RemoveAt(flag1Index);
            //Console.WriteLine("Done with flag...");
            int numBombs = rnd.Next(2) + 2; // number of bombs for flag, 2 or 3
            List<Position> bombs = new List<Position>();
            List<Position> possibleBombs = new List<Position>();
            if (flag2Pos.col == 0)
            { // edge or corner
                possibleBombs.Add(new Position(flag2Pos.row + 1, flag2Pos.col));
                possibleBombs.Add(new Position(flag2Pos.row, flag2Pos.col + 1));
                possibleBombs.Add(new Position(flag2Pos.row + 1, flag2Pos.col + 1));
            }
            else if (flag2Pos.col == 9)
            { // Other edge or corner
                possibleBombs.Add(new Position(flag2Pos.row + 1, flag2Pos.col));
                possibleBombs.Add(new Position(flag2Pos.row, flag2Pos.col - 1));
                possibleBombs.Add(new Position(flag2Pos.row + 1, flag2Pos.col - 1));
            }
            else
            {
                possibleBombs.Add(new Position(flag2Pos.row, flag2Pos.col - 1));
                possibleBombs.Add(new Position(flag2Pos.row, flag2Pos.col + 1));
                possibleBombs.Add(new Position(flag2Pos.row + 1, flag2Pos.col - 1));
                possibleBombs.Add(new Position(flag2Pos.row + 1, flag2Pos.col));
                possibleBombs.Add(new Position(flag2Pos.row + 1, flag2Pos.col + 1));
            }

            // actually add the bombs to list
            int index;
            for (int i = 0; i < numBombs; i++)
            {
                index = rnd.Next(possibleBombs.Count);
                bombs.Add(possibleBombs[index]); // add the chosen pos
                availableP2.Remove(possibleBombs[index]);
                possibleBombs.RemoveAt(index); //  then remove that pos from the list of possibles
            }
            //Console.WriteLine("Done with first bombs...");
            int bombIndex = 34; // first bomb in list
            foreach (var p in bombs)
            {
                plop.setPieceOnGrid(plop.player2Pieces[bombIndex], p);
                bombIndex++;
            }
            //Console.WriteLine("Doing random bombs...");
            // Place the rest of the bombs, go anywhere but row 1
            for (int i = 0; i < (6 - numBombs); i++)
            {
                index = rnd.Next(28 - numBombs);
                plop.setPieceOnGrid(plop.player2Pieces[bombIndex], availableP2[index]);
                availableP2.RemoveAt(index);
            }
            //Console.WriteLine(availableP2.Count);

            // PLAYER 1 -----------------------------------------------------
            numBombs = rnd.Next(2) + 2; // number of bombs for flag, 2 or 3
            bombs = new List<Position>();
            possibleBombs = new List<Position>();
            if (flag1Pos.col == 0)
            { // edge or corner
                possibleBombs.Add(new Position(flag1Pos.row - 1, flag1Pos.col));
                possibleBombs.Add(new Position(flag1Pos.row, flag1Pos.col + 1));
                possibleBombs.Add(new Position(flag1Pos.row - 1, flag1Pos.col + 1));
            }
            else if (flag1Pos.col == 9)
            { // Other edge or corner
                possibleBombs.Add(new Position(flag1Pos.row - 1, flag1Pos.col));
                possibleBombs.Add(new Position(flag1Pos.row, flag1Pos.col - 1));
                possibleBombs.Add(new Position(flag1Pos.row - 1, flag1Pos.col - 1));
            }
            else
            {
                possibleBombs.Add(new Position(flag1Pos.row, flag1Pos.col - 1));
                possibleBombs.Add(new Position(flag1Pos.row, flag1Pos.col + 1));
                possibleBombs.Add(new Position(flag1Pos.row - 1, flag1Pos.col - 1));
                possibleBombs.Add(new Position(flag1Pos.row - 1, flag1Pos.col));
                possibleBombs.Add(new Position(flag1Pos.row - 1, flag1Pos.col + 1));
            }

            // actually add the bombs to list
            for (int i = 0; i < numBombs; i++)
            {
                index = rnd.Next(possibleBombs.Count);
                bombs.Add(possibleBombs[index]); // add the chosen pos
                availableP1.Remove(possibleBombs[index]);
                possibleBombs.RemoveAt(index); //  then remove that pos from the list of possibles
            }
            //Console.WriteLine("plop.player1Pieces: {0}", plop.player1Pieces.Count);
            bombIndex = 34; // first bomb in list
            foreach (var p in bombs)
            {
                plop.setPieceOnGrid(plop.player1Pieces[bombIndex], p);
                bombIndex++;
            }
            // Place the rest of the bombs, go anywhere but row 1
            for (int i = 0; i < (6 - numBombs); i++)
            {
                index = rnd.Next(28 - numBombs);
                plop.setPieceOnGrid(plop.player1Pieces[bombIndex], availableP1[index]);
                availableP1.RemoveAt(index);
            }
            //Console.WriteLine(availableP1.Count);

            List<List<Piece>> playerPieceLists = new List<List<Piece>>();
            playerPieceLists.Add(plop.player1Pieces);
            playerPieceLists.Add(plop.player2Pieces);
            foreach (var pieces in playerPieceLists)
            {
                List<Position> available = new List<Position>();
                if (pieces == plop.player1Pieces)
                {
                    available = availableP1;
                }
                else
                {
                    available = availableP2;
                }
                /* Now for SCOUTS, playerXPieces[26 - 33]*/
                int scoutIndex = 26; // first scout in list
                int count = 20;
                for (int i = 0; i < 8; i++)
                {
                    index = available.Count - rnd.Next(count) - 1;
                    plop.setPieceOnGrid(pieces[scoutIndex], available[index]);
                    available.RemoveAt(index);
                    scoutIndex++;
                    count--;
                }

                /* Now for MINERS, playerXPieces[21-]
                   could be 10 spots left in first row
                */
                int minerIndex = 21;
                count = 10;
                for (int i = 0; i < 5; i++)
                {
                    index = rnd.Next(available.Count - count);
                    plop.setPieceOnGrid(pieces[minerIndex], available[index]);
                    available.RemoveAt(index);
                    minerIndex++;
                    //count--;
                }
                /* Now do the rest of the pieces, should be RANDOM! */
                for (int i = 0; i < 21; i++)
                {
                    if (i != 3)
                    {
                        index = rnd.Next(available.Count); // random remaining position
                        plop.setPieceOnGrid(pieces[i], available[index]);
                        available.RemoveAt(index);
                    }

                }
            }



        }
    }
}