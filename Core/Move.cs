using System;
using System.Collections.Generic;

namespace ChessEngine.Core
{
    public class Move
    {
        IPiece Piece { get; }
        Coordinates Coordinates { get; }

        public Move(IPiece piece, Coordinates coordinates)
        {
            Piece = piece;
            Coordinates = coordinates;
        }
    }
    
   
}