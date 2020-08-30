using System;
using System.Collections.Generic;

namespace ChessEngine.Core
{
    public class Move
    {
        public IPiece Piece { get; }
        public Coordinates Coordinates { get; }

        public Move(IPiece piece, Coordinates coordinates)
        {
            Piece = piece;
            Coordinates = coordinates;
        }
    }
    
   
}