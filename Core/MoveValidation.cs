using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessEngine.Core
{
    // TODO REFACTOR HOLY MOLY
    // - check if valid moves contain destination
    // - check if path to destination is clear for walking pieces
    // - check if destination is occupied by an enemy or a teammate

    /// <summary>
    /// Provides an interface for MoveValidation-Classes of different PieceTypes.
    /// </summary>
    public interface IMoveValidation
    {
        /// <summary>
        /// Validates if a move is valid.
        /// </summary>
        /// <param name="cells"></param>
        /// <param name="piece"></param>
        /// <param name="destination"></param>
        /// <returns>True if the move is valid. False if the move is invalid.</returns>
        bool ValidateMove(BoardCell[,] cells, IPiece piece, Coordinates destination);
    }

    public class RookMoveValidation : IMoveValidation
    {

        public bool ValidateMove(BoardCell[,] cells, IPiece piece, Coordinates destination)
        {
            bool isOccupiedByTeammember = cells[destination.Row, destination.Column].Piece != null && cells[destination.Row, destination.Column].Piece?.Color == piece.Color;
            bool isDiagonalMove = piece.Coordinates.Row != destination.Row && piece.Coordinates.Column != destination.Column;
            if (isOccupiedByTeammember || isDiagonalMove) return false;

            BoardCell? occupiedCell = FindOccupiedCell(cells, piece, destination);
            if (occupiedCell != null) return false;

            return true;
        }

        /// <summary>
        /// checks if any cell except the destination is occupied
        /// </summary>
        /// <param name="cells"></param>
        /// <param name="piece"></param>
        /// <param name="destination"></param>
        /// <returns>BoardCell that is occupied and prevents the move.</returns>
        private BoardCell? FindOccupiedCell(BoardCell[,] cells, IPiece piece, Coordinates destination)
        {
            // find out if row or column changed
            if (piece.Coordinates.Row != destination.Row)
            {
                // find direction
                if (piece.Coordinates.Row > destination.Row)
                {
                    for (int i = piece.Coordinates.Row - 1 + 1; i >= destination.Row; i--)
                    {
                        if (cells[i, piece.Coordinates.Column].Piece != null) return cells[i, piece.Coordinates.Column];
                    }
                }
                else
                {
                    for (int i = piece.Coordinates.Row + 1; i < destination.Row; i++)
                    {
                        if (cells[i, piece.Coordinates.Column].Piece != null) return cells[i, piece.Coordinates.Column];
                    }
                }
            }
            else
            {
                if (piece.Coordinates.Column > destination.Column)
                {
                    for (int i = piece.Coordinates.Column - 1; i > destination.Column; i--)
                    {
                        if (cells[piece.Coordinates.Row, i].Piece != null) return cells[piece.Coordinates.Row, i];
                    }
                }
                else
                {
                    for (int i = piece.Coordinates.Column + 1; i < destination.Column; i++)
                    {
                        if (cells[piece.Coordinates.Row, i].Piece != null) return cells[piece.Coordinates.Row, i];
                    }
                }
            }

            return null;
        }
    }
    public class KnightMoveValidation : IMoveValidation
    {

        public IList<Coordinates> FindValidMoves(IPiece piece)
        {
            IList<Coordinates> validMoves = new List<Coordinates>();

            // Bottom

            validMoves.Add(new Coordinates()
            {
                Row = piece.Coordinates.Row - 2,
                Column = piece.Coordinates.Column + 1
            });

            validMoves.Add(new Coordinates()
            {
                Row = piece.Coordinates.Row - 2,
                Column = piece.Coordinates.Column - 1
            });

            validMoves.Add(new Coordinates()
            {
                Row = piece.Coordinates.Row - 1,
                Column = piece.Coordinates.Column + 2
            });


            validMoves.Add(new Coordinates()
            {
                Row = piece.Coordinates.Row - 1,
                Column = piece.Coordinates.Column - 2
            });

            // Top

            validMoves.Add(new Coordinates()
            {
                Row = piece.Coordinates.Row + 2,
                Column = piece.Coordinates.Column - 1
            });

            validMoves.Add(new Coordinates()
            {
                Row = piece.Coordinates.Row + 2,
                Column = piece.Coordinates.Column + 1
            });

            validMoves.Add(new Coordinates()
            {
                Row = piece.Coordinates.Row + 1,
                Column = piece.Coordinates.Column - 2
            });


            validMoves.Add(new Coordinates()
            {
                Row = piece.Coordinates.Row + 1,
                Column = piece.Coordinates.Column + 2
            });

            return validMoves.Where(move => move.Row < 8 && move.Row >= 0 && move.Column < 8 && move.Column >= 0).ToList();
        }


        public bool ValidateMove(BoardCell[,] cells, IPiece piece, Coordinates destination)
        {
            bool cellIsOccupiedByTeammember = cells[destination.Row, destination.Column].Piece != null && cells[destination.Row, destination.Column].Piece?.Color == piece.Color;
            if (cellIsOccupiedByTeammember) return false;

            IList<Coordinates> validMoves = FindValidMoves(piece);

            return validMoves.Contains(destination);
        }
    }
    public class BishopMoveValidation : IMoveValidation
    {

        public IList<Coordinates> FindValidMoves(IPiece piece)
        {
            IList<Coordinates> validMoves = new List<Coordinates>();
            for (int i = piece.Coordinates.Row + 1; i <= 8; i++)
            {
                //bottom right
                for (int j = piece.Coordinates.Column + 1; j <= 8; j++)
                {
                    validMoves.Add(new Coordinates()
                    {
                        Row = i,
                        Column = j
                    });
                }
                //bottom left;
                for (int k = piece.Coordinates.Column - 1; k >= 0; k--)
                {
                    validMoves.Add(new Coordinates()
                    {
                        Row = i,
                        Column = k
                    });
                }
            }

            for (int j = piece.Coordinates.Row - 1; j >= 0; j--)
            {

                //top right
                for (int l = piece.Coordinates.Column + 1; l <= 8; l++)
                {
                    validMoves.Add(new Coordinates()
                    {
                        Row = j,
                        Column = l
                    });
                }
                //top left;
                for (int k = piece.Coordinates.Column - 1; k >= 0; k--)
                {
                    validMoves.Add(new Coordinates()
                    {
                        Row = j,
                        Column = k
                    });
                }
            }
            return validMoves;
        }

        public bool ValidateMove(BoardCell[,] cells, IPiece piece, Coordinates destination)
        {
            if (cells[destination.Row, destination.Column].Piece != null && cells[destination.Row, destination.Column].Piece?.Color == piece.Color) return false;
            IList<Coordinates> coordinatesUntilDestination = new List<Coordinates>();
            IList<Coordinates> validMoves = FindValidMoves(piece);
            // find valid moves
            if (!validMoves.Contains(destination)) return false;
            if (piece.Coordinates.Row > destination.Row)
            {
                if (piece.Coordinates.Column > destination.Column)
                {
                    // both decrease
                    int j = piece.Coordinates.Column + 1;
                    for (int i = piece.Coordinates.Row + 1; i >= destination.Row; i--)
                    {
                        coordinatesUntilDestination.Add(new Coordinates()
                        {
                            Row = i,
                            Column = j
                        });
                        j--;
                    }
                }
                else
                {
                    int j = piece.Coordinates.Column + 1;
                    for (int i = piece.Coordinates.Row + 1; i >= destination.Row; i--)
                    {
                        coordinatesUntilDestination.Add(new Coordinates()
                        {
                            Row = i,
                            Column = j
                        });
                        j++;
                    }
                }
            }
            else
            {
                if (piece.Coordinates.Column > destination.Column)
                {
                    int j = piece.Coordinates.Column + 1;
                    for (int i = piece.Coordinates.Row + 1; i <= destination.Row; i++)
                    {
                        coordinatesUntilDestination.Add(new Coordinates()
                        {
                            Row = i,
                            Column = j
                        });
                        j--;
                    }
                }
                else
                {
                    int j = piece.Coordinates.Column + 1;
                    for (int i = piece.Coordinates.Row + 1; i <= destination.Row; i++)
                    {
                        coordinatesUntilDestination.Add(new Coordinates()
                        {
                            Row = i,
                            Column = j
                        });
                        j++;
                    }
                }
            }


            // check if every cell until destination isnt occupied
            foreach (Coordinates coordinates in coordinatesUntilDestination)
            {
                if (cells[coordinates.Row, coordinates.Column].Piece != null) return false;
            }

            return true;
        }

    }
    public class QueenMoveValidation : IMoveValidation
    {

        public IList<Coordinates> FindValidMoves(IPiece piece)
        {
            IList<Coordinates> validMoves = new List<Coordinates>();
            for (int i = piece.Coordinates.Row + 1; i <= 8; i++)
            {
                // bottom
                validMoves.Add(new Coordinates()
                {
                    Row = i,
                    Column = piece.Coordinates.Column
                });
                //bottom right
                for (int j = piece.Coordinates.Column + 1; j <= 8; j++)
                {
                    validMoves.Add(new Coordinates()
                    {
                        Row = i,
                        Column = j
                    });
                }
                //bottom left;
                for (int k = piece.Coordinates.Column - 1; k >= 0; k--)
                {
                    validMoves.Add(new Coordinates()
                    {
                        Row = i,
                        Column = k
                    });
                }
            }

            for (int j = piece.Coordinates.Row - 1; j >= 0; j--)
            {
                // top
                validMoves.Add(new Coordinates()
                {
                    Row = j,
                    Column = piece.Coordinates.Column
                });

                //top right
                for (int l = piece.Coordinates.Column + 1; l <= 8; l++)
                {
                    validMoves.Add(new Coordinates()
                    {
                        Row = j,
                        Column = l
                    });
                }
                //top left;
                for (int k = piece.Coordinates.Column - 1; k >= 0; k--)
                {
                    validMoves.Add(new Coordinates()
                    {
                        Row = j,
                        Column = k
                    });
                }
            }
            return validMoves;
        }

        public bool ValidateMove(BoardCell[,] cells, IPiece piece, Coordinates destination)
        {
            if (cells[destination.Row, destination.Column].Piece != null && cells[destination.Row, destination.Column].Piece?.Color == piece.Color) return false;
            IList<Coordinates> validMoves = FindValidMoves(piece);
            // find valid moves
            if (!validMoves.Contains(destination)) return false;

            // find all coordinates until destination
            IList<Coordinates> coordinatesUntilDestination = new List<Coordinates>();
            if (piece.Coordinates.Row != destination.Row)
            {
                if (piece.Coordinates.Column != destination.Column)
                {
                    // row and column changed -> diagonal

                    if (piece.Coordinates.Row > destination.Row)
                    {
                        if (piece.Coordinates.Column > destination.Column)
                        {
                            // both decrease
                            int j = piece.Coordinates.Column + 1;
                            for (int i = piece.Coordinates.Row + 1; i >= destination.Row; i--)
                            {
                                coordinatesUntilDestination.Add(new Coordinates()
                                {
                                    Row = i,
                                    Column = j
                                });
                                j--;
                            }
                        }
                        else
                        {
                            int j = piece.Coordinates.Column + 1;
                            for (int i = piece.Coordinates.Row + 1; i >= destination.Row; i--)
                            {
                                coordinatesUntilDestination.Add(new Coordinates()
                                {
                                    Row = i,
                                    Column = j
                                });
                                j++;
                            }
                        }
                    }
                    else
                    {
                        if (piece.Coordinates.Column > destination.Column)
                        {
                            int j = piece.Coordinates.Column + 1;
                            for (int i = piece.Coordinates.Row + 1; i <= destination.Row; i++)
                            {
                                coordinatesUntilDestination.Add(new Coordinates()
                                {
                                    Row = i,
                                    Column = j
                                });
                                j--;
                            }
                        }
                        else
                        {
                            int j = piece.Coordinates.Column + 1;
                            for (int i = piece.Coordinates.Row + 1; i <= destination.Row; i++)
                            {
                                coordinatesUntilDestination.Add(new Coordinates()
                                {
                                    Row = i,
                                    Column = j
                                });
                                j++;
                            }
                        }
                    }
                }
                else
                {
                    // only row changed -> up/down
                    if (piece.Coordinates.Row > destination.Row)
                    {
                        for (int i = piece.Coordinates.Row + 1; i >= destination.Row; i--)
                        {
                            coordinatesUntilDestination.Add(new Coordinates()
                            {
                                Row = i,
                                Column = piece.Coordinates.Column
                            });
                        }
                    }
                    else
                    {
                        for (int i = piece.Coordinates.Row + 1; i <= destination.Row; i++)
                        {
                            coordinatesUntilDestination.Add(new Coordinates()
                            {
                                Row = i,
                                Column = piece.Coordinates.Column
                            });
                        }
                    }
                }
            }
            else
            {
                if (piece.Coordinates.Column != destination.Column)
                {
                    // column changed -> left or right
                    if (piece.Coordinates.Column > destination.Column)
                    {
                        for (int i = piece.Coordinates.Column - 1; i >= destination.Column; i--)
                        {
                            coordinatesUntilDestination.Add(new Coordinates()
                            {
                                Row = piece.Coordinates.Row,
                                Column = i
                            });
                        }
                    }
                    else
                    {
                        for (int i = piece.Coordinates.Column + 1; i <= destination.Column; i++)
                        {
                            coordinatesUntilDestination.Add(new Coordinates()
                            {
                                Row = piece.Coordinates.Row,
                                Column = i
                            });
                        }
                    }
                }
            }

            // check if every cell until destination isnt occupied
            foreach (Coordinates coordinates in coordinatesUntilDestination)
            {
                if (cells[coordinates.Row, coordinates.Column].Piece != null) return false;
            }


            return true;
        }
    }

    // TODO implement Castle
    public class KingMoveValidation : IMoveValidation
    {
        public bool ValidateMove(BoardCell[,] cells, IPiece piece, Coordinates destination)
        {
            if (Math.Abs(piece.Coordinates.Row - destination.Row) > 1 || Math.Abs(piece.Coordinates.Column - destination.Column) > 1) return false;
            if (cells[destination.Row, destination.Column].Piece != null && cells[destination.Row, destination.Column].Piece?.Color == piece.Color) return false;

            return true;
        }
    }

    // TODO implement EnPassent
    // TODO refactor in knight validation style by setting up a list of valid moves and validating against it
    public class PawnMoveValidation : IMoveValidation
    {

        public bool ValidateMove(BoardCell[,] cells, IPiece piece, Coordinates destination)
        {
            if (Math.Abs(piece.Coordinates.Row - destination.Row) > 2) return false;
            if (Math.Abs(piece.Coordinates.Column - destination.Column) > 1) return false;

            Pawn pawn = (Pawn)piece;
            bool isForwardMove = piece.Coordinates.Row != destination.Row && piece.Coordinates.Column == destination.Column;

            if (!pawn.IsFirstMoveDone)
            {
                if (isForwardMove)
                {
                    bool isSingleMove = Math.Abs(piece.Coordinates.Row - destination.Row) == 1;
                    if (cells[destination.Row, destination.Column].Piece != null) return false;
                    if (piece.Coordinates.Row > destination.Row)
                    {
                        if (isSingleMove)
                        {
                            if (cells[piece.Coordinates.Row - 1, destination.Column].Piece != null) return false;
                        }
                        else
                        {
                            if (cells[piece.Coordinates.Row - 2, destination.Column].Piece != null) return false;
                        }
                    }
                    else
                    {
                        if (isSingleMove)
                        {
                            if (cells[piece.Coordinates.Row + 1, destination.Column].Piece != null) return false;
                        }
                        else
                        {
                            if (cells[piece.Coordinates.Row + 2, destination.Column].Piece != null) return false;
                        }
                    }

                    return true;
                }

                return false;
            }

            if (Math.Abs(piece.Coordinates.Row - destination.Row) > 1) return false;

            if (isForwardMove)
            {
                if (cells[destination.Row, destination.Column].Piece != null) return false;
            }
            else
            {
                if (cells[destination.Row, destination.Column].Piece != null && cells[destination.Row, destination.Column].Piece?.Color == piece.Color) return false;
            }


            return true;
        }
    }

}