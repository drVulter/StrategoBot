using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Stratego
{
    using CustomExtensions;
    class GameTrial
    {
        static void Main(string[] args)
        {
            Player p1 = new Player("P1", SpaceType.Player1, PlayerColor.Red);
            Player p2 = new Player("P2", SpaceType.Player2, PlayerColor.Blue);
            Game plop = new Game(p1,p2);
            //setUpBoard(p1, p2, plop);//calls Quinn's majestic randomized setup method that I copied over from his Eval class
            randomBoard(p1,p2,plop);
            plop.start();
            //Console.WriteLine("Here is the initial board");
            //plop.initialGrid.displayGrid();
            //Console.WriteLine("");
            int count = 0;
            int depth1 = 3;//change as needed
            int depth2 = 2;
            int depth = 3;
            List<Move> p1Moves = new List<Move>(); // keep track of moves for each player
            List<Move> p2Moves = new List<Move>();
            while (true)
            //for (int i = 0; i < 2; i++)
            {

                Move p1Move =  alphaBetaSearch(plop, p1, p2, depth);
                p1Moves.Add(p1Move);
                plop.movePiece(p1Move.start, p1Move.end);
                //Console.WriteLine("After player 1 move");
                //plop.initialGrid.displayGrid();
                //Console.WriteLine("");

                //update the machine learning record data on this line

                //view the grid
                //Console.WriteLine("After player 1 move");
                if (plop.checkWin(p1))
                {
                    //update and complete this instance of the record data with the fact that p1 won this game on this line
                    break;
                }
                Move p2Move = alphaBetaSearch(plop, p2, p1, depth);
                p2Moves.Add(p2Move);
                plop.movePiece(p2Move.start, p2Move.end);
                //Console.WriteLine("After player 2 move");
                //plop.initialGrid.displayGrid();
                //Console.WriteLine("");

                if (plop.checkWin(p2))
                {
                    //update and complete this instance of the record data with the fact that p2 won this game on this line
                    break;
                }
                if (count > 10) {
                    // check for repeated moves
                    if (checkEquilibrium(p1Moves) && checkEquilibrium(p2Moves)) {
                        break;
                    }
                }
                //Console.WriteLine(count);
                count++; // increase move count
            }
            
            Console.WriteLine(count);

        }
        public static bool checkEquilibrium(List<Move> moves)
        {
            int count = 0;
            for (int i = moves.Count - 1; i > moves.Count - 7; i--) {
                if ((moves[i].start == moves[moves.Count - 1].start) && (moves[i].end == moves[moves.Count - 1].end)) {
                    count++;
                }
            }
            return (count >= 3);
        }
        public static object DeepClone(object obj)
            {
                object objResult = null;
                using (MemoryStream  ms = new MemoryStream())
                {
                    BinaryFormatter  bf =   new BinaryFormatter();
                    bf.Serialize(ms, obj);

                    ms.Position = 0;
                    objResult = bf.Deserialize(ms);
                }
                return objResult;
            }

        //encapsulates the moves within a set of actions within a state
        [Serializable]
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
        private static Move alphaBetaSearch(Game state, Player max, Player min, int depth)
        {
            double alpha = double.MinValue;
            double beta = double.MaxValue;
            int depthCurrent = 1;
            //Console.WriteLine("alphaBetaSearch");
            //if (max.name == "P1") {
                List<Move> actions = actionsForMax(state, max, min, alpha, beta, depthCurrent, depth);
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
                /*} else {
                Console.WriteLine("trigger");
                List<Move> actions = actionsForMin(state, max, min, alpha, beta, depthCurrent, depth);
                double bestMoveVal = double.MaxValue;
                int bestMoveIndex = 0;
                for (int i = 0; i < actions.Count; i++)
                {
                    if (actions[i].value < bestMoveVal)
                    {
                        bestMoveVal = actions[i].value;
                        bestMoveIndex = i;
                    }
                }

                return actions[bestMoveIndex];
            }*/
        }

        private static double maxValue(Game state, Player max, Player min, double alpha, double beta, int depthCurrent, int depthFinal)
        {
            //Console.WriteLine("maxValue called");
            //Game plop = new Game(max, min);
            if (state.checkWin(max) == true)
                return double.MaxValue;
            if (state.checkWin(min) == true)
                return double.MinValue;
            //saving this section for the heuristic value to be calculated and returned once we hit max search depth
            if (depthFinal == depthCurrent) {
                //Console.WriteLine("max value depth reached");
                //Console.WriteLine("max value" + eval(max,min,state));
                return eval(max, min, state);
            }

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

        private static double minValue(Game state, Player max, Player min, double alpha, double beta, int depthCurrent, int depthFinal)
        {
            //Console.WriteLine("minValue");
            //Game plop = new Game(min, max);
            if (state.checkWin(max) == true)
                return double.MaxValue;
            if (state.checkWin(min) == true)
                return double.MinValue;
            if (depthFinal == depthCurrent) {
                //Console.WriteLine("depth reached");
                //Console.WriteLine("min value" + eval(min,max,state));
                return 1.0 - eval(max, min, state);
            }

            double v = double.MaxValue;
            List<Move> actions = actionsForMin(state, max, min, alpha, beta, depthCurrent, depthFinal);
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
        //the value of those moves involves the actions that the min player will take when given the new situation
        //caused by max's actions, creating the recursive tree. It works by first finding all the valid pieces max
        //can move, then finding all the places max can move those pieces to, and making a list of all the those
        //begining-end moves. After attaining that information, that move is "simulated" in the newStateNode, and
        //that new state is passed into the minValue method, furthering the recursion to find the actual value of
        //that potential game state. After the value of that state is determined, the move is undone so that the
        //original game object is left the same as it was before the search
        private static List<Move> actionsForMax(Game state, Player max, Player min, double alpha, double beta, int depthCurrent, int depthFinal)
        {
            //Console.WriteLine("actionsForMax");

            List<Move> moves = new List<Move>();
            //looks through all the positions on the board
            for (int i = 0; i < 10; i++){
                for (int j = 0; j < 10; j++){
                    Position searchPos = new Position();
                    searchPos.row = i;
                    searchPos.col = j;

                    //if this position holds a piece that is max's...
                    try
                    {
                        if (state.initialGrid.grid[i, j]._piece.piecePlayer == max)
                        {
                            int returncode;
                            List<Position> posMoves = state.getMoves(searchPos, out returncode);//let's see if it has some moves
                            if (returncode == 1)
                            {//if it's max's piece and it can move...
                                for (int k = 0; k < posMoves.Count; k++)
                                {
                                    int z = 0;
                                    Move move = new Move(searchPos, posMoves[k], z);//we don't know the value of these moves yet but we need them, so null value
                                    moves.Add(move);//add all those moves to the total list
                                }
                            }
                        }
                    }
                    catch (System.NullReferenceException) { }
                }
            }
            List<Move> actions = new List<Move>();
            //now we're going to minmax all those new potential states that we'll be in
            for (int i = 0; i < moves.Count; i++){
                Game newStateNode = (Game)DeepClone(state);
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
            return actions;
        }

        //works the same as actionsForMax
        private static List<Move> actionsForMin(Game state, Player max, Player min, double alpha, double beta, int depthCurrent, int depthFinal)
        {
            List<Move> moves = new List<Move>();
            //looks through all the positions on the board
            for (int i = 0; i < 10; i++){
                for (int j = 0; j < 10; j++){
                    Position searchPos = new Position();
                    searchPos.row = i;
                    searchPos.col = j;

                    //if this position holds a piece that is min's...
                    try
                    {
                        if (state.initialGrid.grid[i, j]._piece.piecePlayer == min)
                        {//TODO: == is not defined for the class Player
                            int returncode;
                            List<Position> posMoves = state.getMoves(searchPos, out returncode);//let's see if it has some moves
                            if (returncode == 1)
                            {//if it's min's piece and it can move...
                                for (int k = 0; k < posMoves.Count; k++)
                                {
                                    int z = 0;
                                    Move move = new Move(searchPos, posMoves[k], z);//we don't know the value of these moves yet but we need them, so null value
                                    moves.Add(move);//add all those moves to the total list
                                }
                            }
                        }
                    }
                    catch (System.NullReferenceException) {/*Console.WriteLine("2nd trigger");*/ }
                }
            }

            List<Move> actions = new List<Move>();
            //now we're going to minmax all those new potential states that we'll be in
            for (int i = 0; i < moves.Count; i++){
                Game newStateNode = (Game)DeepClone(state);
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
        static double eval(Player playerA, Player playerB, Game plop)
        {
            /*
              EVAL() function to estimate utility
              weighted sum of differences between the amount of each piece possessed by each player,
              and the difference between the average flag distance for each player.
              +++ Assumes playerA is MAX +++
              */
            // First define weights
            //Getting warnings that these variables are never used, where would we be able to implement them?
            double distanceWeight = 0.000000000001; // what is good value for this?
            int[] p1Totals = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // total amount of each piece type lost
            int[] p2Totals = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] differences = new int[12];
            double[] weights = { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 20, double.MaxValue}; // adjust these weights

            List<List<Piece>> pieceLists = new List<List<Piece>>();
            try {
            pieceLists.Add(plop.player1Lost);
            pieceLists.Add(plop.player2Lost);
            // count up totals
            foreach (var pieces in pieceLists)
            {
                int[] totals = {0,0,0,0,0,0,0,0,0,0,0,0};
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
                //sum += weights[i] * difference;
            }
            // subtract because we want smaller average distance
            //sum -= distanceWeight * averageDistance(playerA, playerB, plop);
            // or do the difference? always the same???
            //Console.WriteLine(distanceWeight * (averageDistance(playerA, playerB) - averageDistance(playerB, playerA)));
            //sum += distanceWeight * -1 * ((averageDistance(playerA, playerB) - averageDistance(playerB, playerA)));

            // or do we use total distances?
            //sum =+ distanceWeight * -1 * ((distanceMetric(playerA, playerB) - distanceMetric(playerB, playerA)));
            //Console.WriteLine("distance metric for move by " + playerA.name);
            //Console.WriteLine(distanceWeight * distanceMetric(playerA, playerB, plop) - distanceMetric(playerB, playerA, plop));
            //Console.WriteLine("value of dist metric: " + (distanceMetric(playerA, playerB, plop) - distanceMetric(playerB, playerA, plop)));
            //Console.WriteLine(distanceMetric(playerA,playerB,plop));
            //Console.WriteLine(distanceMetric(playerB,playerA,plop));

            // DIFFERENCE
            //sum -= distanceWeight * (distanceMetric(playerA, playerB, plop) - distanceMetric(playerB, playerA, plop));
            // just dist metric!
            //Console.WriteLine("before dist " + sum);
            sum -= distanceWeight * 10.0 * distanceMetric(playerA, playerB, plop);
            //sum -= distanceWeight * averageDistance(playerA, playerB, plop);
            //Console.WriteLine("after dist " + sum);
            return 1.0 / (1.0 + Math.Exp(sum)); // return logistic
            //return sum;
            } catch (System.NullReferenceException) {
                Console.WriteLine("Eval trigger");
                return -1 * distanceWeight * distanceMetric(playerA, playerB, plop);
            }
        }

        static double averageDistance(Player playerA, Player playerB, Game plop)
        {
            /*
              Average Manhattan Distance of all pieces belonging to playerA to playerB's flag
            */
            SpaceType player;
            //if (playerA == p1) {
                player = SpaceType.Player1;
            //} else {
                player = SpaceType.Player2;
            //}
            int total = 0; // total of all distances
            int count = 0; // number of pieces being considered
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++) {
                    if (plop.initialGrid.mainGrid[i,j]._type == player) {
                        // need to make sure we only consider pieces that can move/capture the flag
                        if (!((plop.initialGrid.mainGrid[i,j]._piece.pieceName == piecesTypes.Bomb) ||
                              (plop.initialGrid.mainGrid[i,j]._piece.pieceName == piecesTypes.Flag))) {
                            total += distance(i, j, playerB.flagPos.row, playerB.flagPos.col);
                            count++;
                        }
                    }
                }
            }
            return total / count; // return the average
        }


        static int distance(int x1, int y1, int x2, int y2)
        {
                return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }
        static int distanceMetric(Player playerA, Player playerB, Game plop)
        {
            /*
              sums up Manhattan distances of each players pieces to opposing player's flag
            */
            SpaceType player;
            if (playerA.name == "P1")
            {
                player = SpaceType.Player1;
            }
            else
            {
                player = SpaceType.Player2;
            }
            int total = 0; // total of all distances
            //plop.initialGrid.displayGrid();
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
            //Console.WriteLine(total);
            return total;
        }
        static void shuffleList(List<Piece> list)
        {
            // method to use the Fisher-Yates algorithm to shuffle a list
            Random rnd = new Random();
            for (int i = list.Count - 1; i > 0; i--) {
                int r = rnd.Next(i);
                Piece temp = list[i];
                list[i] = list[r];
                list[r] = temp;
            }
        }
        static void randomBoard(Player p1, Player p2, Game plop)
        {
            // similar to setUpBoard but TOTALLY RANDOM

            // shuffle the pieces
            shuffleList(plop.player1Pieces);
            shuffleList(plop.player2Pieces);
            int pieceIndex = 0; // index in piece lists
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 10; j++) {
                    Position pos1 = new Position(i,j);
                    Position pos2 = new Position(9 - i,9 - j);
                    plop.setPieceOnGrid(plop.player1Pieces[pieceIndex], pos2);
                    pieceIndex++;
                }
            }
            shuffleList(plop.player2Pieces);
            pieceIndex = 0;
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 10; j++) {
                    plop.setPieceOnGrid(plop.player2Pieces[pieceIndex], new Position(i,j));
                    pieceIndex++;
                }
            }

        }
        static void setUpBoard(Player p1, Player p2, Game plop)
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
