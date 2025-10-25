using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame__WPF
{
    // Manages the queue of Tetris blocks
    public class BlockQueue
    {
        private Block[] blocks = new Block[]
       {
            new IBlock(),//1
            new JBlock(),//2
            new LBlock(),//3
            new OBlock(),//4
            new SBlock(),//5
            new TBlock(),//6
            new ZBlock() //7

       };

        private Random rand = new Random();
        public Block NextBlock { get; private set; }

        public BlockQueue()
        {
            NextBlock = RandomBlock();
        }

        private Block RandomBlock() { return blocks[rand.Next(blocks.Length)]; }

        public Block GetAndUpdate()
        {
            Block b = NextBlock;
            do
            {
                NextBlock = RandomBlock();
            }
            while (NextBlock.ID == b.ID);

            return b;
        }
    }
}
