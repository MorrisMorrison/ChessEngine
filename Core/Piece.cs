
namespace ChessEngine.Core
{
    public interface IPiece
    {
        PieceType Type { get; set; }
        // Valid Moves
        // Current Position
        Color Color { get; set; }
        Coordinates Coordinates { get; set; }
    }

    public class Piece : IPiece
    {
        public PieceType Type { get; set; }
        public Color Color { get; set; }
        public Coordinates Coordinates { get; set; }

        public Piece(PieceType type, Color color, Coordinates coordinates)
        {
            Color = color;
            Type = type;
            Coordinates = coordinates;
        }
    }

    public class Pawn : Piece
    {

        public bool IsFirstMoveDone { get; set; }
        public Pawn(PieceType type, Color color, Coordinates coordinates) : base(type, color, coordinates)
        {

        }
    }

    public enum PieceType
    {
        KING,
        QUEEN,
        ROOK,
        KNIGHT,
        BISHOP,
        PAWN,
    }

    public enum Color
    {
        BLACK, WHITE
    }
}
