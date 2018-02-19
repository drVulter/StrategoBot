/* Try to implement basic A* algorithm
   
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Stratego
{
    class Program
    {
        static void Main(string[] args)
        {
            Player p1 = new Player("P1", SpaceType.Player1, PlayerColor.Red);
            Player p2 = new Player("P2", SpaceType.Player2, PlayerColor.Blue);
            Game plop = new Game(p1,p2);
            
            Position pos1 = new Position();
            pos1.row = 0;
            pos1.col = 0;

            Position pos2 = new Position();
            pos2.row = 6;
            pos2.col = 4;

            int pieceIndex = 0;
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 4; j++) {
                    Position posB = new Position(); // player 2
                    posB.row = j;
                    posB.col = i;
                    plop.setPieceOnGrid(plop.player2Pieces[pieceIndex], posB);
                    // Player 1
                    Position posA = new Position();
                    posA.row = 9 - j;
                    posA.col = 9-i;
                    plop.setPieceOnGrid(plop.player1Pieces[pieceIndex], posA);
                    pieceIndex++; // go to next piece 
                }

            }
            

            

            plop.start(); // start the game

            // check the player's piece lists
            foreach (var p in plop.player1Pieces) {
                p.displayPiece();
            }
            Console.WriteLine("Player 2");
            foreach (var p in plop.player2Pieces) {
                p.displayPiece();
            }


            // display the initial grid
            plop.initialGrid.displayGrid();
            

            // Test random movement
            int rc; // return code, needed for getMoves()
            Random rnd = new Random(); // random number generator
            int numMoves = 5; // how many moves do you want to test???
            Position pos = new Position();// initial position
            // this position will be where player 2s leftmost miner is, next to their flag
            // note that all movement is based on position, whatever piece is at that position will be moved.
            pos.row = 3;
            pos.col = 1;
            Position nextPos = new Position();
            int moveIndex; // this helps choose a random move from the list of possible moves at a given state
            for (int i = 0; i < numMoves; i++) {
                List<Position> moves = plop.getMoves(pos, out rc);
                Console.WriteLine("Possible moves at {0}", i);
                foreach (var p in moves) {
                    Console.WriteLine("row: {0}", p.row);
                    Console.WriteLine("col: {0}", p.col);
                    Console.WriteLine();
                }
                moveIndex = rnd.Next(moves.Count); // random number no bigger than size of list
                Position next = nextPos = moves[moveIndex]; // update position
                // movePiece() moves whatever piece is at pos to next.
                // it returns a numeric code that represents what happened at the move.
                switch (plop.movePiece(pos, next))
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
                plop.initialGrid.displayGrid(); // show the grid
                pos = next; // update the piece's current position


            }

        }
    }
}
