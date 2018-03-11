using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stratego
{
    public class Grid
    {
        private const int gridSize = 10;
        private GridSpace[,] grid;

        public GridSpace[,] mainGrid
        {
            get { return grid; }
        }

        public Grid()
        {
            grid = new GridSpace[gridSize, gridSize];
            initGrid();
        }

        private void initGrid()
        {
            for(int row=0;row<gridSize;row++)
            {
                for(int col=0;col<gridSize;col++)
                {
                    grid[row,col]._isPlayable=true;
                    if (row < 4) grid[row, col]._type = SpaceType.Player2;
                    if (row == 4 || row==5) grid[row, col]._type = SpaceType.Empty;
                    if (row >5) grid[row, col]._type = SpaceType.Player1;

                    //add non-playable area
                    if (row > 3 && row < 6 && ((col > 1 && col < 4)||(col>5 && col<8))) grid[row, col]._isPlayable = false;
                }
                
            }
        }

        //make all gridspaces playable (except for non-playable spaces)
        public void startUpGrid()
        {
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    grid[row, col]._isPlayable = true;

                    //add non-playable area
                    if (row > 3 && row < 6 && ((col > 1 && col < 4) || (col > 5 && col < 8))) grid[row, col]._isPlayable = false;
                }

            }
        }
        public void displayGrid()
        {
            for (int k = 0; k < 71; k++) {
                    Console.Write("-");
                }
            Console.WriteLine();
            for (int row = 0; row < gridSize; row++)
            {
                Console.Write("| ");
                for (int col = 0; col < gridSize; col++)
                {
                    try { 
                        //Console.Write((int)grid[row, col]._piece.pieceName + grid[row, col]._piece.piecePlayer.name + " ");
                        Console.Write("{0,2}{1} ", (int)grid[row, col]._piece.pieceName, grid[row, col]._piece.piecePlayer.name);
                        Console.Write("| ");
                    }
                    catch { 
                        //Console.Write(grid[row, col]._type.ToString()[0] + " ");
                        if (!grid[row,col]._isPlayable) // this is a lake space
                            Console.Write("   X ");
                        else
                            Console.Write("{0,4} ", grid[row, col]._type.ToString()[0]);
                        Console.Write("| ");
                    };
                }
                Console.WriteLine();
                for (int k = 0; k < 71; k++) {
                    Console.Write("-");
                }
                Console.WriteLine();
            }
        /*
        public void displayGrid()
        {
            for (int k = 0; k < 41; k++) {
                    Console.Write("-");
                }
            Console.WriteLine();
            for (int row = 0; row < gridSize; row++)
            {
                Console.Write("| ");
                for (int col = 0; col < gridSize; col++)
                {
                    try { 
                        Console.Write(grid[row, col]._piece.pieceName.ToString()[0] + " "); 
                        Console.Write("| ");
                    }
                    catch { 
                        Console.Write(grid[row, col]._type.ToString()[0] + " "); 
                        Console.Write("| ");
                    };
                }
                Console.WriteLine();
                for (int k = 0; k < 41; k++) {
                    Console.Write("-");
                }
                Console.WriteLine();
            }
            */
        }
    }
}
