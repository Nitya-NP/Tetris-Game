//Tetris Game 
//Nitya Patel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace TetrisGame__WPF
{
    // GameState class manages the overall state of the Tetris game
    // including the grid, current block, held block, score, and game over status
    public class GameState
    {
        // Represents the Tetris grid (22 rows x 10 columns)
        public Grid GameGrid { get; }

        // Stores the currently falling block
        private Block currentBlock;


        // Current block property
        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset(); // Reset block position
            }
        }

        // Queue of upcoming blocks
        public BlockQueue BlockQueue { get; }

        // Game over flag
        public bool GameOver { get; private set; }

        // Player score
        public int Score { get; private set; }

        // Block held by player
        public Block HeldBlock { get; private set; }

        // Can player hold a block
        public bool canHold { get; private set; }

        // Initialize game
        public GameState()
        {
            GameGrid = new Grid(22, 10);
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetAndUpdate();
            canHold = true;
        }

        // Hold or swap block
        public void HoldBlock()
        {
            if (!canHold) return;

            if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.GetAndUpdate();
            }
            else
            {
                Block tmp = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = tmp;
            }

            canHold = false;
        }

        // Check if block fits
        private bool Blockfits()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                if (!GameGrid.IsEmpty(p.Row, p.Col))
                    return false;
            }
            return true;
        }

        // Rotate block clockwise
        public void RotateBlockCW()
        {
            CurrentBlock.RotateCW();
            if (!Blockfits()) CurrentBlock.RotateCCW();
        }

        // Rotate block counter-clockwise
        public void RotateBlockCCW()
        {
            CurrentBlock.RotateCCW();
            if (!Blockfits()) CurrentBlock.RotateCW();
        }

        // Move block left
        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);
            if (!Blockfits()) CurrentBlock.Move(0, 1);
        }

        // Move block right
        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);
            if (!Blockfits()) CurrentBlock.Move(0, -1);
        }

        // Move block down
        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);
            if (!Blockfits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock(); // Place block on grid
            }
        }

        // Check game over
        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        // Place block on grid
        private void PlaceBlock()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                GameGrid[p.Row, p.Col] = CurrentBlock.ID;
            }

            Score += GameGrid.ClearFullRows();

            if (IsGameOver())
                GameOver = true;
            else
            {
                CurrentBlock = BlockQueue.GetAndUpdate();
                canHold = true;
            }
        }

        // Check how far a tile can drop
        private int TileDropDistance(Position p)
        {
            int drop = 0;
            while (GameGrid.IsEmpty(p.Row + drop + 1, p.Col))
            {
                drop++;
            }
            return drop;
        }

        // Check how far block can drop
        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;

            foreach (Position p in CurrentBlock.TilePositions())
            {
                drop = System.Math.Min(drop, TileDropDistance(p));
            }

            return drop;
        }

        // Drop block instantly
        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }
    }

}
    





