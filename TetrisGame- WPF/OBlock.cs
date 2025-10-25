using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame__WPF
{
    // Represents the "O" shaped Tetris block
    public class OBlock : Block
    {
        private Position[][] tiles = new Position[][]
      {
           new Position[]{new Position(0,0), new Position(1,0), new Position(0,1),new Position(1,1)},
           new Position[]{new Position(0,0), new Position(1,0), new Position(0,1),new Position(1,1)},
           new Position[]{new Position(0,0), new Position(1,0), new Position(0,1),new Position(1,1)},
           new Position[]{new Position(0,0), new Position(1,0), new Position(0,1),new Position(1,1)},

      };
        public override int ID => 4;
        protected override Position StartOffSet => new Position(0, 4);
        protected override Position[][] Tiles=>tiles;

        
    }
}
