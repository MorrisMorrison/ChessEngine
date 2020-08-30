using System;
using System.Collections;
using System.Collections.Generic;

namespace ChessEngine.Core
{

    public interface IBoard:IEnumerable<BoardCell>
    {
        void Move(IPiece piece, Coordinates destination);
        void Print();
    }


    public class Board : IBoard
    {
        public BoardCell[,] Cells = new BoardCell[8, 8];

        public IEnumerator<BoardCell> GetEnumerator(){
            foreach (BoardCell cell in Cells){
                yield return cell;
            }
        }

        IEnumerator IEnumerable.GetEnumerator(){
            return GetEnumerator();
        }

        public Board()
        {
            Initialize();
            SetInitialPieces();
        }

        public void Initialize()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Cells[i, j] = new BoardCell()
                    {
                        Coordinates = new Coordinates()
                        {
                            Row = i,
                            Column = j
                        }
                    };
                }
            }
        }

        public void SetInitialPieces()
        {
            // BLACK
            Cells[0, 0].Piece = new Piece(PieceType.ROOK, Color.BLACK, new Coordinates{Row=0, Column=0});
            Cells[0, 1].Piece = new Piece(PieceType.KNIGHT, Color.BLACK, new Coordinates{Row=0, Column=1});
            Cells[0, 2].Piece = new Piece(PieceType.BISHOP, Color.BLACK, new Coordinates{Row=0, Column=2});
            Cells[0, 3].Piece = new Piece(PieceType.QUEEN, Color.BLACK, new Coordinates{Row=0, Column=3});
            Cells[0, 4].Piece = new Piece(PieceType.KING, Color.BLACK, new Coordinates{Row=0, Column=4});
            Cells[0, 5].Piece = new Piece(PieceType.BISHOP, Color.BLACK, new Coordinates{Row=0, Column=5});
            Cells[0, 6].Piece = new Piece(PieceType.KNIGHT, Color.BLACK, new Coordinates{Row=0, Column=6});
            Cells[0, 7].Piece = new Piece(PieceType.ROOK, Color.BLACK, new Coordinates{Row=0, Column=7});
            Cells[1, 0].Piece = new Pawn(PieceType.PAWN, Color.BLACK, new Coordinates{Row=1, Column=0});
            Cells[1, 1].Piece = new Pawn(PieceType.PAWN, Color.BLACK, new Coordinates{Row=1, Column=1});
            Cells[1, 2].Piece = new Pawn(PieceType.PAWN, Color.BLACK, new Coordinates{Row=1, Column=2});
            Cells[1, 3].Piece = new Pawn(PieceType.PAWN, Color.BLACK, new Coordinates{Row=1, Column=3});
            Cells[1, 4].Piece = new Pawn(PieceType.PAWN, Color.BLACK, new Coordinates{Row=1, Column=4});
            Cells[1, 5].Piece = new Pawn(PieceType.PAWN, Color.BLACK, new Coordinates{Row=1, Column=5});
            Cells[1, 6].Piece = new Pawn(PieceType.PAWN, Color.BLACK, new Coordinates{Row=1, Column=6});
            Cells[1, 7].Piece = new Pawn(PieceType.PAWN, Color.BLACK, new Coordinates{Row=1, Column=7});

            // WHITE
            Cells[7, 0].Piece = new Piece(PieceType.ROOK, Color.WHITE, new Coordinates{Row=7, Column=0});
            Cells[7, 1].Piece = new Piece(PieceType.KNIGHT, Color.WHITE, new Coordinates{Row=7, Column=1});
            Cells[7, 2].Piece = new Piece(PieceType.BISHOP, Color.WHITE, new Coordinates{Row=7, Column=2});
            Cells[7, 3].Piece = new Piece(PieceType.QUEEN, Color.WHITE, new Coordinates{Row=7, Column=3});
            Cells[7, 4].Piece = new Piece(PieceType.KING, Color.WHITE, new Coordinates{Row=7, Column=4});
            Cells[7, 5].Piece = new Piece(PieceType.BISHOP, Color.WHITE, new Coordinates{Row=7, Column=5});
            Cells[7, 6].Piece = new Piece(PieceType.KNIGHT, Color.WHITE, new Coordinates{Row=7, Column=6});
            Cells[7, 7].Piece = new Piece(PieceType.ROOK, Color.WHITE, new Coordinates{Row=7, Column=7});
            Cells[6, 0].Piece = new Pawn(PieceType.PAWN, Color.WHITE, new Coordinates{Row=6, Column=0});
            Cells[6, 1].Piece = new Pawn(PieceType.PAWN, Color.WHITE, new Coordinates{Row=6, Column=1});
            Cells[6, 2].Piece = new Pawn(PieceType.PAWN, Color.WHITE, new Coordinates{Row=6, Column=2});
            Cells[6, 3].Piece = new Pawn(PieceType.PAWN, Color.WHITE, new Coordinates{Row=6, Column=3});
            Cells[6, 4].Piece = new Pawn(PieceType.PAWN, Color.WHITE, new Coordinates{Row=6, Column=4});
            Cells[6, 5].Piece = new Pawn(PieceType.PAWN, Color.WHITE, new Coordinates{Row=6, Column=5});
            Cells[6, 6].Piece = new Pawn(PieceType.PAWN, Color.WHITE, new Coordinates{Row=6, Column=6});
            Cells[6, 7].Piece = new Pawn(PieceType.PAWN, Color.WHITE, new Coordinates{Row=6, Column=7});
        }

        public void SetPiece(IPiece piece, Coordinates coordinates)
        {
            Cells[coordinates.Row, coordinates.Column].Piece = piece;
        }

        public void RemovePiece(IPiece piece)
        {
            Cells[piece.Coordinates.Row, piece.Coordinates.Column].Piece = null;
        }

        // TODO Refactor => too slow to loop over the whole board
        public IList<Coordinates> FindValidMoves(IPiece piece){
            IList<Coordinates> coordinates = new List<Coordinates>();

            foreach (BoardCell cell in Cells){
                if (ValidateMove(piece, cell.Coordinates)){
                    coordinates.Add(cell.Coordinates);
                }
            }
        
            return coordinates;
        }


        public bool ValidateMove(IPiece piece, Coordinates destination)
        {
            switch (piece.Type)
            {
                case PieceType.ROOK:
                    return new RookMoveValidation().ValidateMove(Cells, piece, destination);
                case PieceType.KNIGHT:
                    return new KnightMoveValidation().ValidateMove(Cells, piece, destination);
                case PieceType.BISHOP:
                    return new BishopMoveValidation().ValidateMove(Cells, piece, destination);
                case PieceType.QUEEN:
                    return new QueenMoveValidation().ValidateMove(Cells, piece, destination);
                case PieceType.KING:
                    return new KingMoveValidation().ValidateMove(Cells, piece, destination);
                case PieceType.PAWN:
                    return new PawnMoveValidation().ValidateMove(Cells, piece, destination);
            }

            return true;
        }

        public void Move(IPiece piece, Coordinates destination)
        {
            if (ValidateMove(piece, destination))
            {
                RemovePiece(piece);
                piece.Coordinates = destination;
                SetPiece(piece, destination);
            }
        }

        public void Print()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Console.Write((Cells[i, j].Piece == null ? "X" : Enum.Parse(typeof(PieceType), Enum.GetName(typeof(PieceType), Cells[i, j].Piece?.Type))) + "|");
                }

                Console.WriteLine();
            }
        }
    }


    public class BoardCell
    {
        public IPiece? Piece { get; set; }
        public Coordinates Coordinates { get; set; }
    }

}