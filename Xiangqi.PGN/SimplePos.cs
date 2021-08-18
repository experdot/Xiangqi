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
        const int FILE_RIGHT = 11;

        static byte[] IN_BOARD = {
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

        public static String STARTUP_FEN =
            "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 1";

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

        void clearBoard()
        {
            sdPlayer = 0;
            for (int sq = 0; sq < 256; sq++)
            {
                squares[sq] = 0;
            }
        }

        void changeSide()
        {
            sdPlayer = 1 - sdPlayer;
        }

        int fenPiece(char c)
        {
            switch (c)
            {
                case 'K':
                    return PIECE_KING;
                case 'A':
                    return PIECE_ADVISOR;
                case 'B':
                case 'E':
                    return PIECE_BISHOP;
                case 'H':
                case 'N':
                    return PIECE_KNIGHT;
                case 'R':
                    return PIECE_ROOK;
                case 'C':
                    return PIECE_CANNON;
                case 'P':
                    return PIECE_PAWN;
                default:
                    return -1;
            }
        }

        void fromFen(String fen)
        {
            clearBoard();
            int y = RANK_TOP;
            int x = FILE_LEFT;
            int index = 0;
            if (index == fen.Length)
            {
                return;
            }
            char c = fen[index];
            while (c != ' ')
            {
                if (c == '/')
                {
                    x = FILE_LEFT;
                    y++;
                    if (y > RANK_BOTTOM)
                    {
                        break;
                    }
                }
                else if (c >= '1' && c <= '9')
                {
                    x += (c - '0');
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    if (x <= FILE_RIGHT)
                    {
                        int pt = fenPiece(c);
                        if (pt >= 0)
                        {
                            squares[COORD_XY(x, y)] = (byte)(pt + 8);
                        }
                        x++;
                    }
                }
                else if (c >= 'a' && c <= 'z')
                {
                    if (x <= FILE_RIGHT)
                    {
                        int pt = fenPiece((char)(c + 'A' - 'a'));
                        if (pt >= 0)
                        {
                            squares[COORD_XY(x, y)] = (byte)(pt + 16);
                        }
                        x++;
                    }
                }
                index++;
                if (index == fen.Length)
                {
                    return;
                }
                c = fen[index];
            }
            index++;
            if (index == fen.Length)
            {
                return;
            }
            if (sdPlayer == (fen[index] == 'b' ? 0 : 1))
            {
                changeSide();
            }
        }
    }
}
