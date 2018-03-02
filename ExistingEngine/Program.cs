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

        //we should probably move this to the Game class
        bool checkWin(Player player, Game state){
            if (player == p1){
                for (int i = 0; i < state.player2Lost.Count; i++){
                        if (state.player2Lost[i].pieceName == "Flag") return true;
                }
            }

            if (player == p2){
                for (int i = 0; i < state.player1Lost.Count; i++){
                        if (state.player1Lost[i].pieceName == "Flag") return true;
                }
            }

            return false;
        }

        //encapsulates the moves within a set of actions within a state
        private class Move
        {
            Position start;
            Position end;
            public int value;

            Move(Position start, Position end, int value)
            {
                this.start = start;
                this.end = end;
                this.value = value;
            }
        }

        //runs minimax search with alpha-beta pruning
        private Move alphaBetaSearch(Game state, Player max, Player min, int depth)
        {
            int alpha = int.MinValue;
            int beta = int.MaxValue;
            int depthCurrent = 1;
            List<Move> actions = actionsForMax(state, max, min, alpha, beta, depth, depthCurrent);
            int bestMoveVal = int.MinValue;
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

        private int maxValue(Game state, Player max, Player min, int alpha, int beta, int depthFinal, int depthCurrent)
        {
            if (checkWin(max, state) == true)
                return int.maxValue;
            if (checkWin(min, state) == true)
                return int.minValue;
            //saving this section for the heuristic value to be calculated and returned once we hit max search depth
            if (depthFinal == depthCurrent)
                return //heuristicValue(state,max);

            int v = int.MinValue;
            List<Move> actions = actionsForMax(state, max, min, ref alpha, ref beta, depthFinal, depthCurrent);
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

        private int minValue(Game state, Player max, Player min, int alpha, int beta, int depthFinal, int depthCurrent)
        {
            if (checkWin(max, state) == true)
                return int.maxValue;
            if (checkWin(min, state) == true)
                return int minValue;
            if (depthFinal == depthCurrent)
                return //heuristicValue(state,min);

            int v = int.MaxValue;
            List<Move> actions = actionsForMin(state, max, min, ref alpha, ref beta, depthFinal, depthCurrent);
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
        private List<Move> actionsForMax(Game state, Player max, Player min, int alpha, int beta, int depthCurrent, int DepthFinal)
        {

            List<> moves = new List<Move>();
            //looks through all the positions on the board
            for (int i = 0; i < 10; i++){
                for (int j = 0; j < 10; j++){
                    Position searchPos = new Position();
                    searchPos.row = i;
                    searchPos.col = j;
                    
                    //if this position holds a piece that is max's...
                    if (state.initialGrid.GridSpace[i,j]._Piece._Player == max){//TODO: == is not defined for Player class
                        int returncode;
                        List<Position> posMoves = state.getMoves(searchPos, returncode);//let's see if it has some moves
                        if (returncode == 1){//if it's max's piece and it can move...
                            for (int k = 0; k < posMoves.Count; k++){
                                Move move = new Move(searchPos,posMoves[k],null);//we don't know the value of these moves yet but we need them, so null value
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
                int alphaNew = alpha;
                int betaNew = beta;
                depthCurrent++;
                Move move = new Move(moves[i].start,moves[i].end,minValue(newStateNode, max, min, alphaNew, betaNew, depthCurrent, depthFinal));
                actions.Add(move);
                //revert all the changes
                newStateNode.movePiece(moves[i].end,moves[i].start)
                depthCurrent--;
            }
            return actions;            
        }

        //works the same as actionsForMax
        private List<Move> actionsForMin(Game state, Player max, Player min, int alpha, int beta, int depthCurrent, int DepthFinal)
        {
            List<> moves = new List<Move>();
            //looks through all the positions on the board
            for (int i = 0; i < 10; i++){
                for (int j = 0; j < 10; j++){
                    Position searchPos = new Position();
                    searchPos.row = i;
                    searchPos.col = j;
                    
                    //if this position holds a piece that is min's...
                    if (state.initialGrid.GridSpace[i,j]._Piece._Player == min){//TODO: == is not defined for the class Player
                        int returncode;
                        List<Position> posMoves = state.getMoves(searchPos, returncode);//let's see if it has some moves
                        if (returncode == 1){//if it's min's piece and it can move...
                            for (int k = 0; k < posMoves.Count; k++){
                                Move move = new Move(searchPos,posMoves[k],null);//we don't know the value of these moves yet but we need them, so null value
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
                int alphaNew = alpha;
                int betaNew = beta;
                depthCurrent++;
                Move move = new Move(moves[i].start,moves[i].end,maxValue(newStateNode, max, min, alphaNew, betaNew, depthCurrent, depthFinal));
                actions.Add(move);
                //revert all the changes
                newStateNode.movePiece(moves[i].end,moves[i].start)
                depthCurrent--;
            }
            return actions; 
        }
    }
}
