namespace ChessEngine.Core
{
     public struct Coordinates
    {
        public int Row;
        public int Column;

        public bool Equals(Coordinates coordinates)
        {
            return coordinates.Row == Row && coordinates.Column == Column;
        }

        public static bool operator ==(Coordinates source, Coordinates destination)
        {
            return source.Row == destination.Row && source.Column == destination.Column;
        }

        public static bool operator !=(Coordinates source, Coordinates destination)
        {
            return source.Row != destination.Row || source.Column != destination.Column;
        }

    }
}