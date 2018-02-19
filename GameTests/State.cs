/*
  Represents a since game state
  */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stratego
{
    class State
    {
        // player one/two's pieces
        List<Piece> pieces1;
        List<Piece> pieces2;

        // list of possible moves for each player
        List<Position> moves1;
        List<Position> moves2;

        // should we cutoff search and calculate Eval() function?
        bool cutoff;
    }
}
