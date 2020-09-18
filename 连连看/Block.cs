using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 连连看
{
    public class Block
    {
        static int blockheight = 30;
        public static int Blockheight
        {
            get { return Block.blockheight; }
            set { Block.blockheight = value; }
        }
        static int blockwidth = 30;
        public Block(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static int Blockwidth
        {
            get { return Block.blockwidth; }
            set { Block.blockwidth = value; }
        }
        int x;
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        int y;
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
    }
}
