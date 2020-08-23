using System;
using ChessEngine.Core;

namespace ChessEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            board.Print();
            Console.WriteLine("-------------------");
            board.Move(board.Cells[0, 0].Piece, new Coordinates()
            {
                Row = 2,
                Column = 0
            });
            board.Print();
            Console.WriteLine("-------------------");

                   board.Move(board.Cells[1, 0].Piece, new Coordinates()
                   {
                       Row = 3,
                       Column = 0
                   });
            board.Print();
            Console.WriteLine("-------------------");

                             board.Move(board.Cells[6, 1].Piece, new Coordinates()
                             {
                                 Row = 4,
                                 Column = 1
                             });
            board.Print();
            Console.WriteLine("-------------------");

                             board.Move(board.Cells[3, 0].Piece, new Coordinates()
                             {
                                 Row = 4,
                                 Column = 1
                             });
            board.Print();
        }
    }

}
