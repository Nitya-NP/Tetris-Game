//Tetris Game 
// Nitya Patel 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame__WPF
{
    // Abstract class for Tetris blocks
    // Each specific block (I, O, T, etc.) will inherit from this
    public abstract class Block
    {
        public abstract int ID { get; }// for the block number
        protected abstract Position[][] Tiles { get; }
        protected abstract Position StartOffSet {get; }
        private Position offSet;
        private int rotation;


        public Block()
        {
            offSet = new Position(StartOffSet.Row, StartOffSet.Col);
            rotation = 0;
         
        }

        public void RotateCW()
        {

            rotation = (rotation + 1) % Tiles.Length;

        }

        public void RotateCCW()
        {
            if (rotation == 0)
                rotation = 3;
            else rotation--;


        }

        public void Move(int rows, int cols)
        {
            offSet.Row += rows;
            offSet.Col += cols;
        }

        public void Reset()
        {
            offSet.Row = StartOffSet.Row;
            offSet.Col = StartOffSet.Col;
            rotation = 0;
        }

        
        public IEnumerable<Position> TilePositions() //it's read only
        {
            foreach(Position p in Tiles[rotation])
            {
                yield return new Position(p.Row + offSet.Row, p.Col + offSet.Col);
            }
        }

    }
}
