using System;

namespace Xiangqi.PGN
{
    public class SimplePos
    {
        public const int PIECE_KING = 0;
        public const int PIECE_ADVISOR = 1;
        public const int PIECE_BISHOP = 2;
        public const int PIECE_KNIGHT = 3;
        public const int PIECE_ROOK = 4;
        public const int PIECE_CANNON = 5;
        public const int PIECE_PAWN = 6;

        public const int RANK_TOP = 3;
        public const int RANK_BOTTOM = 12;
        public const int FILE_LEFT = 3;

        static readonly byte[] IN_BOARD = {
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
        0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
        0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
        0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
        0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
        0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
        0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
        0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
        0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
        0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        };

        public static string STARTUP_FEN = "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 1";

        public static bool Is_IN_BOARD(int sq)
        {
            return IN_BOARD[sq] != 0;
        }

        public static int COORD_XY(int x, int y)
        {
            return x + (y << 4);
        }

        public static int RANK_Y(int sq)
        {
            return sq >> 4;
        }

        public static int FILE_X(int sq)
        {
            return sq & 15;
        }

        public static int SQUARE_FLIP(int sq)
        {
            return 254 - sq;
        }

        public static int SIDE_TAG(int sd)
        {
            return 8 + (sd << 3);
        }

        public static int SRC(int mv)
        {
            return mv & 255;
        }

        public static int DST(int mv)
        {
            return mv >> 8;
        }

        public static int MOVE(int sqSrc, int sqDst)
        {
            return sqSrc + (sqDst << 8);
        }

        public int sdPlayer;
        public byte[] squares = new byte[256];
    }
}
