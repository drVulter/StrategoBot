using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stratego
{
    class Program
    {
        static Player p1 = new Player("P1", SpaceType.Player1, PlayerColor.Red);
        static Player p2 = new Player("P2", SpaceType.Player2, PlayerColor.Blue);
        static Game plop = new Game(p1,p2);
        static void Main(string[] args)
        {

            setUpBoard();
            // start the game
            plop.start();
            plop.initialGrid.displayGrid();
            Console.WriteLine(distanceMetric(p2,p1));

            Position pos1 = new Position(3, 0);
            for (int i = 0; i < 3; i++) {
                Position nextPos = new Position();
                nextPos.row = pos1.row+1;
                nextPos.col = pos1.col;
                switch (plop.movePiece(pos1, nextPos))
                {
                    case 1: Console.WriteLine("Piece MOVED !");
                        break;
                    case 10: Console.WriteLine("WIN !!!");
                        break;
                    case 20: Console.WriteLine("TIE !");
                        break;
                    case 30: Console.WriteLine("LOST !!!");
                        break;
                    case 50: Console.WriteLine("You found the flag !");
                        break;
                    default: Console.WriteLine("Move not allowed !");
                        break;
                }
                plop.initialGrid.displayGrid();
                pos1 = nextPos;

            }
            Console.WriteLine(plop.initialGrid.mainGrid[3,0]._type);
            Console.WriteLine(plop.initialGrid.mainGrid[4,0]._type);
            Console.WriteLine(distanceMetric(p2,p1));
            //Console.ReadLine();
        }
        
        static int distanceMetric(Player playerA, Player playerB)
        {
            /*
              sums up Manhattan distances of each players pieces to opposing player's flag
            */
            List<Piece> pieces;
            SpaceType player;
            if (playerA == p1) {
                pieces = plop.player1Pieces;
                player = SpaceType.Player1;
            } else {
                pieces = plop.player2Pieces;
                player = SpaceType.Player2;
            }
            int total = 0;
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++) {
                    if (plop.initialGrid.mainGrid[i,j]._type == player) {
                        // need to make sure we only consider pieces that can move/capture the flag
                        if (!((plop.initialGrid.mainGrid[i,j]._piece.pieceName == piecesTypes.Bomb) ||
                              (plop.initialGrid.mainGrid[i,j]._piece.pieceName == piecesTypes.Flag))) {
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
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 10; j++) {
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
                if (flag2Pos.col == 0) { // edge or corner
                    possibleBombs.Add(new Position(flag2Pos.row + 1, flag2Pos.col));
                    possibleBombs.Add(new Position(flag2Pos.row, flag2Pos.col + 1));
                    possibleBombs.Add(new Position(flag2Pos.row + 1, flag2Pos.col + 1));
                } else if (flag2Pos.col == 9) { // Other edge or corner
                    possibleBombs.Add(new Position(flag2Pos.row + 1, flag2Pos.col));
                    possibleBombs.Add(new Position(flag2Pos.row, flag2Pos.col - 1));
                    possibleBombs.Add(new Position(flag2Pos.row + 1, flag2Pos.col - 1));
                } else { 
                    possibleBombs.Add(new Position(flag2Pos.row, flag2Pos.col - 1));
                    possibleBombs.Add(new Position(flag2Pos.row, flag2Pos.col + 1));
                    possibleBombs.Add(new Position(flag2Pos.row + 1, flag2Pos.col - 1));
                    possibleBombs.Add(new Position(flag2Pos.row + 1, flag2Pos.col));
                    possibleBombs.Add(new Position(flag2Pos.row + 1, flag2Pos.col + 1));
                }
                
                // actually add the bombs to list
                int index;
                for (int i = 0; i < numBombs; i++) {
                    index = rnd.Next(possibleBombs.Count);
                    bombs.Add(possibleBombs[index]); // add the chosen pos
                    availableP2.Remove(possibleBombs[index]);
                    possibleBombs.RemoveAt(index); //  then remove that pos from the list of possibles
                }
                //Console.WriteLine("Done with first bombs...");
                int bombIndex = 34; // first bomb in list
                foreach (var p in bombs) {
                    plop.setPieceOnGrid(plop.player2Pieces[bombIndex], p);
                    bombIndex++;
                }
                //Console.WriteLine("Doing random bombs...");
                // Place the rest of the bombs, go anywhere but row 1
                for (int i = 0; i < (6 - numBombs); i++) {
                    index = rnd.Next(28 - numBombs);
                    plop.setPieceOnGrid(plop.player2Pieces[bombIndex], availableP2[index]);
                    availableP2.RemoveAt(index);
                }
                //Console.WriteLine(availableP2.Count);

                // PLAYER 1 -----------------------------------------------------
                numBombs = rnd.Next(2) + 2; // number of bombs for flag, 2 or 3
                bombs = new List<Position>();
                possibleBombs = new List<Position>();
                if (flag1Pos.col == 0) { // edge or corner
                    possibleBombs.Add(new Position(flag1Pos.row - 1, flag1Pos.col));
                    possibleBombs.Add(new Position(flag1Pos.row, flag1Pos.col + 1));
                    possibleBombs.Add(new Position(flag1Pos.row - 1, flag1Pos.col + 1));
                } else if (flag1Pos.col == 9) { // Other edge or corner
                    possibleBombs.Add(new Position(flag1Pos.row - 1, flag1Pos.col));
                    possibleBombs.Add(new Position(flag1Pos.row, flag1Pos.col - 1));
                    possibleBombs.Add(new Position(flag1Pos.row - 1, flag1Pos.col - 1));
                } else { 
                    possibleBombs.Add(new Position(flag1Pos.row, flag1Pos.col - 1));
                    possibleBombs.Add(new Position(flag1Pos.row, flag1Pos.col + 1));
                    possibleBombs.Add(new Position(flag1Pos.row - 1, flag1Pos.col - 1));
                    possibleBombs.Add(new Position(flag1Pos.row - 1, flag1Pos.col));
                    possibleBombs.Add(new Position(flag1Pos.row - 1, flag1Pos.col + 1));
                }
                
                // actually add the bombs to list
                for (int i = 0; i < numBombs; i++) {
                    index = rnd.Next(possibleBombs.Count);
                    bombs.Add(possibleBombs[index]); // add the chosen pos
                    availableP1.Remove(possibleBombs[index]);
                    possibleBombs.RemoveAt(index); //  then remove that pos from the list of possibles
                }
                //Console.WriteLine("plop.player1Pieces: {0}", plop.player1Pieces.Count);
                bombIndex = 34; // first bomb in list
                foreach (var p in bombs) {
                    plop.setPieceOnGrid(plop.player1Pieces[bombIndex], p);
                    bombIndex++;
                }
                // Place the rest of the bombs, go anywhere but row 1
                for (int i = 0; i < (6 - numBombs); i++) {
                    index = rnd.Next(28 - numBombs);
                    plop.setPieceOnGrid(plop.player1Pieces[bombIndex], availableP1[index]);
                    availableP1.RemoveAt(index);
                }
                //Console.WriteLine(availableP1.Count);

            List<List<Piece>> playerPieceLists = new List<List<Piece>>();
            playerPieceLists.Add(plop.player1Pieces);
            playerPieceLists.Add(plop.player2Pieces);
            foreach (var pieces in playerPieceLists) {
                List<Position> available = new List<Position>();
                if (pieces == plop.player1Pieces) {
                    available = availableP1;
                } else {
                    available = availableP2;
                }
                /* Now for SCOUTS, playerXPieces[26 - 33]*/
                int scoutIndex = 26; // first scout in list
                int count = 20;
                for (int i = 0; i < 8; i++) {
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
                for (int i = 0; i < 5; i++) {
                    index = rnd.Next(available.Count - count);
                    plop.setPieceOnGrid(pieces[minerIndex], available[index]);
                    available.RemoveAt(index);
                    minerIndex++;
                    //count--;
                }
                /* Now do the rest of the pieces, should be RANDOM! */
                for (int i = 0; i < 21; i++) {
                    if (i != 3) {
                        index = rnd.Next(available.Count); // random remaining position
                        plop.setPieceOnGrid(pieces[i], available[index]);
                        available.RemoveAt(index);
                    }
                    
                }
        }
            
            
            
        }
    }
}
