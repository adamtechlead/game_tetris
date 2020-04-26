using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTetris
{
    class TetrisGame
    {
        public bool IsPlaying { get; set; }
        public int[,] Matrix;
        public int CurrentX;
        public int CurrentY;
        public int[,] CurrentPiece;

        public void GameLoop()
        {
            if (!IsPlaying) return;

            if (!MoveDown())
            {
                PlacePiece(Matrix, CurrentX, CurrentY, CurrentPiece);
                DeleteFullLines(Matrix);
                GenerateNewPiece();
            }
        }

        public void StartNewGame()
        {
            Matrix = new int[12, 20];
            IsPlaying = true;

            GenerateNewPiece();
        }

        public void MoveLeft()
        {
            if (!IsMatrixEmpty(CurrentX - 1, CurrentY, CurrentPiece, Matrix)) return;
            CurrentX--;
        }

        public void MoveRight()
        {
            if (!IsMatrixEmpty(CurrentX + 1, CurrentY, CurrentPiece, Matrix)) return;
            CurrentX++;
        }

        public void TurnPiece()
        {
            if (CurrentPiece == null) return;

            var len = CurrentPiece.GetUpperBound(0) + 1;
            var newPiece = new int[len, len];
            MatrixEach(CurrentPiece, (x, y) => newPiece[len - y -1, x] = CurrentPiece[x, y]);
            if (!IsMatrixEmpty(CurrentX, CurrentY, newPiece, Matrix)) return;

            CurrentPiece = newPiece;
        }

        public bool MoveDown()
        {
            if (!IsMatrixEmpty(CurrentX, CurrentY + 1, CurrentPiece, Matrix)) return false;

            CurrentY++;
            return true;
        }

        private void GenerateNewPiece()
        {
            int pieceIndex = new Random().Next(7);
            CurrentX = 3;
            CurrentY = 0;
            CurrentPiece = GetPieceArray(pieceIndex);
            if (!IsMatrixEmpty(CurrentX, CurrentY, CurrentPiece, Matrix)) IsPlaying = false;
        }


        private static void MatrixEach(int[,] matrix, Action<int, int> action)
        {
            for (int x = 0; x <= matrix.GetUpperBound(0); x++)
                for (int y = 0; y <= matrix.GetUpperBound(1); y++) action(x, y);
        }
        
        private static void PlacePiece(int[,] matrix, int x, int y, int[,] piece)
        {
            for (int u = 0; u <= piece.GetUpperBound(0); u++)
            {
                for (int v = 0; v <= piece.GetUpperBound(1); v++)
                {
                    if (piece[u, v] == 0) continue;

                    matrix[x + u, y + v] = piece[u, v];
                }
            }
        }

        private static bool IsMatrixEmpty(int x, int y, int[,] piece, int[,] matrix)
        {
            for (int u = 0; u <= piece.GetUpperBound(0); u++)
            {
                for (int v = 0; v <= piece.GetUpperBound(1); v++)
                {
                    if (piece[u, v] == 0) continue;

                    var a = x + u;
                    var b = y + v;

                    if ( a < 0
                        || a > matrix.GetUpperBound(0)
                        || b > matrix.GetUpperBound(1)) return false;

                    if (matrix[a, b] > 0) return false;
                }
            }

            return true;
        }

        private static void DeleteFullLines(int[,] matrix)
        {
            int lineIndex = matrix.GetUpperBound(1);
            for (int i = 0; i <= matrix.GetUpperBound(1); i++)
            {
                if (LineIsFull(matrix, lineIndex))
                {
                    DeleteLine(matrix, lineIndex);
                    continue;
                }
                lineIndex--;
            }
        }

        private static bool LineIsFull(int[,] matrix, int yCoord)
        {
            for (int x = 0; x <= matrix.GetUpperBound(0); x++)
            {
                if (matrix[x, yCoord] == 0) return false;
            }

            return true;
        }

        private static void DeleteLine(int[,] matrix, int yCoord)
        {
            var xLength = matrix.GetUpperBound(0) + 1;
            for (int y = yCoord; y > 1; y--)
            {
                for (int x = 0; x < xLength; x++)
                {
                    matrix[x, y] = matrix[x, y - 1];
                }
            }
            for (int x = 0; x < xLength; x++)
            {
                matrix[x, 0] = 0;
            }
        }

        private static int[,] GetPieceArray(int pieceIndex)
        {
            switch (pieceIndex)
            {
                case 0: return new int[4, 4] { { 0, 0, 0, 0 }, { 1, 1, 1, 1 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
                case 1: return new int[4, 4] { { 0, 0, 0, 0 }, { 1, 1, 1, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 0 } };
                case 2: return new int[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 1, 0 }, { 1, 1, 1, 0 }, { 0, 0, 0, 0 } };
                case 3: return new int[4, 4] { { 0, 1, 0, 0 }, { 0, 1, 1, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 0 } };
                case 4: return new int[4, 4] { { 0, 0, 1, 0 }, { 0, 1, 1, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, 0 } };
                case 5: return new int[4, 4] { { 1, 1, 1, 0 }, { 1, 1, 1, 0 }, { 1, 1, 1, 0 }, { 0, 0, 0, 0 } };
                case 6: return new int[4, 4] { { 0, 1, 0, 0 }, { 0, 1, 1, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, 0 } };
            }
            throw new Exception("invalid piece index");
        }
    }
}