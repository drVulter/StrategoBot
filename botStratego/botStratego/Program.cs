using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stratego
{
    class Program
    {
        static int depthFinal = 5;
        static void Main(string[] args)
        {
            Player p1 = new Player("Max", SpaceType.Player1, PlayerColor.Red);
            Player p2 = new Player("Min", SpaceType.Player2, PlayerColor.Blue);
            Game plop = new Game(p1,p2);


            Position pos1 = new Position();
            pos1.row = 3;
            pos1.col = 4;

            Position pos2 = new Position();
            pos2.row = 6;
            pos2.col = 4;

            plop.setPieceOnGrid(plop.player2Pieces[2], pos1);
            plop.setPieceOnGrid(plop.player1Pieces[0], pos2);

            plop.start();

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
            //Console.ReadLine();
        }


        //from here on down is the code added by Keller, copied from his TicTacToe AI
        //I've done some initial Java to C# syntax changes, actually fully adapting this to what we need in Stratego is soon to come



        //encapsulates the moves within a set of actions within a state
        public class Move
        {
            public Position start;
            public Position end;
            public double value;

            public Move(Position start, Position end, double value)
            {
                this.start = start;
                this.end = end;
                this.value = value;
            }
        }

        //runs minimax search with alpha-beta pruning
        private Move alphaBetaSearch(Game state, Player max, Player min, int depth)
        {
            double alpha = double.MinValue;
            double beta = double.MaxValue;
            int depthCurrent = 1;
            List<Move> actions = actionsForMax(state, max, min, alpha, beta, depth, depthCurrent);
            double bestMoveVal = double.MinValue;
            int bestMoveIndex = 0;
            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i].value > bestMoveVal)
                {
                    bestMoveVal = actions[i].value;
                    bestMoveIndex = i;
                }
            }
            return actions[bestMoveIndex];
        }

        private double maxValue(Game state, Player max, Player min, double alpha, double beta, int depthFinal, int depthCurrent)
        {
            if (state.checkWin(max, state) == true)
                return double.MaxValue;
            if (state.checkWin(min, state) == true)
                return double.MinValue;
            //saving this section for the heuristic value to be calculated and returned once we hit max search depth
            if (depthFinal == depthCurrent)
                return 1; //heuristicValue(state,max);

            double v = double.MinValue;
            List<Move> actions = actionsForMax(state, max, min, (double)alpha, beta, depthFinal, depthCurrent);
            for (int i = 0; i < actions.Count; i++)
            {
                if (v < actions[i].value)
                    v = actions[i].value;
                if (v >= beta)
                    return v;
                if (v > alpha)
                    alpha = v;
            }
            return v;
        }

        private double minValue(Game state, Player max, Player min, double alpha, double beta, int depthFinal, int depthCurrent)
        {
            Game plop = new Game(min, max);
            if (plop.checkWin(max, state) == true)
                return double.MaxValue;
            if (plop.checkWin(min, state) == true)
                return double.MinValue;
            if (depthFinal == depthCurrent)
            {
                //create eval object
                //call functions, make it run return the value gotten
                return; //evaluation function goes here
                        //I am not sure where we would call the Eval class
            }

            double v = double.MaxValue;
            List<Move> actions = actionsForMin(state, max, min, alpha, beta, depthFinal, depthCurrent);
            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i].value < v)
                    v = actions[i].value;
                if (v <= alpha)
                    return v;
                if (v < beta)
                    beta = v;
            }
            return v;
        }



        //this method returns a list of all the moves the max player can take given a particular game state
        //the value of those moves involves the actions that the min player game take when given the new situation
        //caused by max's actions, creating the recursive tree. It works by first finding all the valid pieces max
        //can move, then finding all the places max can move those pieces to, and making a list of all the those
        //begining-end moves. After attaining that information, that move is "simulated" in the newStateNode, and 
        //that new state is passed into the minValue method, furthering the recursion to find the actual value of
        //that potential game state. After the value of that state is determined, the move is undone so that the
        //original game object is left the same as it was before the search
        private List<Move> actionsForMax(Game state, Player max, Player min, double alpha, double beta, int depthCurrent, int DepthFinal)
        {

            List<Move> moves = new List<Move>();
            //looks through all the positions on the board
            for (int i = 0; i < 10; i++){
                for (int j = 0; j < 10; j++){
                    Position searchPos = new Position();
                    searchPos.row = i;
                    searchPos.col = j;
                    
                    //if this position holds a piece that is max's...
                    if (state.initialGrid.grid[i,j]._piece.piecePlayer == max){//TODO: == is not defined for Player class
                        int returncode;
                        List<Position> posMoves = state.getMoves(searchPos, out returncode);//let's see if it has some moves
                        if (returncode == 1){//if it's max's piece and it can move...
                            for (int k = 0; k < posMoves.Count; k++){
                                int z = 0;
                                Move move = new Move(searchPos,posMoves[k],z);//we don't know the value of these moves yet but we need them, so null value
                                moves.Add(move);//add all those moves to the total list
                            }
                        }     
                    }
                }
            }

            //now we're going to minmax all those new potential states that we'll be in
            for (int i = 0; i < moves.Count; i++){
                List<Move> actions = new List<Move>();
                Game newStateNode = state;
                newStateNode.movePiece(moves[i].start,moves[i].end);
                double alphaNew = alpha;
                double betaNew = beta;
                depthCurrent++;
                Move move = new Move(moves[i].start,moves[i].end,minValue(newStateNode, max, min, alphaNew, betaNew, depthCurrent, depthFinal));
                actions.Add(move);
                //revert all the changes
                newStateNode.movePiece(moves[i].end, moves[i].start);
                depthCurrent--;
            }
            return moves; //returns moves?            
        }

        //works the same as actionsForMax
        private List<Move> actionsForMin(Game state, Player max, Player min, double alpha, double beta, int depthCurrent, int DepthFinal)
        {
            List<Move> moves = new List<Move>();
            //looks through all the positions on the board
            for (int i = 0; i < 10; i++){
                for (int j = 0; j < 10; j++){
                    Position searchPos = new Position();
                    searchPos.row = i;
                    searchPos.col = j;
                    
                    //if this position holds a piece that is min's...
                    if (state.initialGrid.grid[i,j]._piece.piecePlayer == min){//TODO: == is not defined for the class Player
                        int returncode;
                        List<Position> posMoves = state.getMoves(searchPos, out returncode);//let's see if it has some moves
                        if (returncode == 1){//if it's min's piece and it can move...
                            for (int k = 0; k < posMoves.Count; k++){
                                int z = 0;
                                Move move = new Move(searchPos,posMoves[k],z);//we don't know the value of these moves yet but we need them, so null value
                                moves.Add(move);//add all those moves to the total list
                            }
                        }     
                    }
                }
            }

            //now we're going to minmax all those new potential states that we'll be in
            for (int i = 0; i < moves.Count; i++){
                List<Move> actions = new List<Move>();
                Game newStateNode = state;
                newStateNode.movePiece(moves[i].start,moves[i].end);
                double alphaNew = alpha;
                double betaNew = beta;
                depthCurrent++;
                Move move = new Move(moves[i].start,moves[i].end,maxValue(newStateNode, max, min, alphaNew, betaNew, depthCurrent, depthFinal));
                actions.Add(move);
                //revert all the changes
                newStateNode.movePiece(moves[i].end, moves[i].start);
                depthCurrent--;
            }
            return actions; 
        }

        static void setUpBoard(Game plop)
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
