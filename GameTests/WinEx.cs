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
            Player p1 = new Player("David", SpaceType.Player1, PlayerColor.Red);
            Player p2 = new Player("Paul", SpaceType.Player2, PlayerColor.Blue);
            Game plop = new Game(p1,p2);
            
            Position pos1 = new Position();
            pos1.row = 0;
            pos1.col = 0;

            Position pos2 = new Position();
            pos2.row = 6;
            pos2.col = 4;

            //plop.setPieceOnGrid(plop.player2Pieces[0], pos1);
            //plop.setPieceOnGrid(plop.player1Pieces[0], pos2);
            //plop.initPlayerPieces(p1, p2);

            // set up a complete board
            //int numPieces = plop.player1Pieces.Count();

            //plop.setPieceOnGrid(plop.player2Pieces[3], pos1);
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
            

            

            plop.start();
            //plop.initialGrid.displayGrid();

            // check the player's piece lists
            
            foreach (var p in plop.player1Pieces) {
                p.displayPiece();
            }
            Console.WriteLine("Player 2");
            foreach (var p in plop.player2Pieces) {
                p.displayPiece();
            }
            
            plop.initialGrid.displayGrid();
            /* Test the getMoves() function */
            Position flag2Pos = new Position();
            flag2Pos.col = 0;
            flag2Pos.row = 3;
            int rc; // return code
            List<Position> flagMoves = plop.getMoves(flag2Pos, out rc);
            Console.WriteLine(rc);
            foreach (var p in flagMoves) {
                Console.WriteLine("row: {0}", p.row);
                Console.WriteLine("col: {0}", p.col);
                Console.WriteLine();
            }

            Position marsh2Pos = new Position();
            marsh2Pos.col = 1;
            marsh2Pos.row = 3;
            List<Position> marshMoves = plop.getMoves(marsh2Pos, out rc);
            Console.WriteLine("Marshall");
            Console.WriteLine(rc);
            foreach (var p in marshMoves) {
                Console.WriteLine("row: {0}", p.row);
                Console.WriteLine("col: {0}", p.col);
                Console.WriteLine();
            }
            //Console.Write(plop.initialGrid.mainGrid[0,6]._piece.displayPiece());
            
            /* Test a WIN */
            /* 
            int column = 0;
            Position start1 = new Position();
            start1.col = column;
            start1.row = 6;
            //Position next1 = new Position();
            //next1.col = column;
            //next1.row = start1
            for (int i = 0; i < 3; i++) {
                // move player 1
                Position next = new Position();
                next.row = 5-i;
                next.col = column;
                switch (plop.movePiece(start1, next))
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
                start1.row = next.row; // update
            }
            */
            
            // test movement
            /* 
            Position firstPos = new Position();
            firstPos.row = 3;
            firstPos.col = 1;
            Position nextPos = new Position();
            nextPos.row = 4;
            nextPos.col = 1;
            switch (plop.movePiece(firstPos, nextPos))
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
            
            // another move
            firstPos.row = nextPos.row;
            firstPos.col = nextPos.col;
            nextPos.row = 5;
            switch (plop.movePiece(firstPos, nextPos))
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
            
            for (int i = 1; i <8;i++ )
            {
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
                pos1.row++;
                plop.initialGrid.displayGrid();
            }
            */
            //Console.ReadLine();
        }
    }
}
