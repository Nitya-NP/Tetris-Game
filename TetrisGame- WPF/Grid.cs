using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame__WPF
{
    // Represents the Tetris game grid
    public class Grid
    {
        private int[,] grid;
        public int Rows { get; }
        public int Cols { get; }

        public int this[int r, int c] //indexer to provide easy accesss to array
        {
            get => grid[r, c]; // lamda expression...shortcut
            set => grid[r, c] = value;
        }

        public Grid(int r, int c)
        {
            Rows = r;
            Cols = c;
            grid = new int[r, c];
        }
        private bool IsInside(int r, int c)
        {
            return (r >= 0 && r < Rows && c >= 0 && c < Cols);

        }
        public bool IsEmpty(int r, int c)
        {
            return IsInside(r, c) && grid[r, c] == 0;
        }

        private bool IsRowFull(int r)
        {
            //for (int i = 0; i < grid.GetLength(0); i++)

            for (int c = 0; c < Cols; c++)
            {
                if (grid[r, c] == 0) return false; //kicks u out
            }
            return true;

        }
        public bool IsRowEmpty(int r)
        {
            for (int c = 0; c < Cols; c++)
            {
                if (grid[r, c] != 0) return false;
            }
            return true;
        }

        public void ClearRow(int r)
        {
            for (int c = 0; c < Cols; c++)
            {
                grid[r, c] = 0;
            }
        }

        public void MoveRowDown(int r, int numRows)
        {
            for (int c = 0; c < Cols; c++)
            {
                grid[r + numRows, c] = grid[r, c];
                grid[r, c] = 0;
            }
        }

        public int ClearFullRows()
        {
            int cleared = 0;

            for (int r = Rows - 1; r >= 0; r--)
            {
                if (IsRowFull(r))
                {
                    cleared++;
                    ClearRow(r);
                }
                else if(cleared > 0)
                {
                    
                        MoveRowDown(r, cleared);
                }

            }
            return cleared;

        }

    }
}
