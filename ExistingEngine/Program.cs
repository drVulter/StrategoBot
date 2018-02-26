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

        private Move alphaBetaSearch(char[][] state)
        {
            int alpha = int.MinValue;
            int beta = int.MaxValue;
            List<Move> actions = actionsForMax(state, alpha, beta);
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

        private int maxValue(char[][] state, int alpha, int beta)
        {
            if (checkWin('O', state) == true)
                return 1;
            if (checkWin('X', state) == true)
                return -1;
            if (noMoreMoves(state) == true)
                return 0;
            int v = int.MinValue;
            List<Move> actions = actionsForMax(state, alpha, beta);
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

        private int minValue(char[][] state, int alpha, int beta)
        {
            if (checkWin('O', state) == true)
                return 1;
            if (checkWin('X', state) == true)
                return -1;
            if (noMoreMoves(state) == true)
                return 0;
            int v = int.MaxValue;
            List<Move> actions = actionsForMin(state, alpha, beta);
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

        private List<Move> actionsForMax(char[][] state, int alpha, int beta)
        {
            List<Move> actions = new List<Move>();
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (state[i][j] == ' ')
                    {
                        state[i][j] = 'O';

                        int alphaNew = alpha;
                        int betaNew = beta;

                        Move move = new Move(i, j, minValue(state, alphaNew, betaNew));
                        actions.Add(move);
                        state[i][j] = ' ';
                    }
                }
            }
            return actions;
        }

        private List<Move> actionsForMin(char[][] state, int alpha, int beta)
        {
            List<Move> actions = new List<Move>();
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (state[i][j] == ' ')
                    {
                        state[i][j] = 'X';
                        int alphaNew = alpha;
                        int betaNew = beta;
                        Move move = new Move(i, j, maxValue(state, alphaNew, betaNew));
                        actions.Add(move);
                        state[i][j] = ' ';
                    }
                }
            }
            return actions;
        }
    }
}
