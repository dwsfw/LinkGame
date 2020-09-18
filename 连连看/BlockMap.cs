using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 连连看
{
    public class Dpoint
    {
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
        public Dpoint(int x,int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public class BlockMap
    {
        public int Width = 6;
        public int Height = 5;
        const int images = 39;//图样总数
        int[,] blocks;
        public int this[int i,int j]
        {
            get { return this.blocks[i, j]; }
            set { this.blocks[i, j] = value; }
        }
        public BlockMap()
        {
            blocks = new int[Height + 2, Width + 2];
            InitializeBlocks();
        }
        public BlockMap(int h,int w)
        {
            Height = h;
            Width = w;
            blocks = new int[Height + 2, Width + 2];
            InitializeBlocks();
        }
        //初始化图块，保证外圈都为0，且成对出现
        private void InitializeBlocks()
        {
            Random ran = new Random(DateTime.Now.Millisecond);
            int r;
            for(int i=1;i<Height+1;i++)
            {
                for(int j=1;j<Width/2+1;j++)
                {
                    r = ran.Next(images) + 1;
                    blocks[i, j] = blocks[i, Width + 1 - j] = r;
                }
            }
        }
        public void DeOrderMap()
        {
            Random a = new Random(DateTime.Now.Millisecond);
            Random b = new Random(DateTime.Now.Millisecond);
            int r, t;
            for (int i = 1; i < Height + 1; i++)
            {
                for (int j = 1; j < Width + 1; j++)
                {
                    r = a.Next(Height) + 1;
                    t = b.Next(Width) + 1;
                    int temp = blocks[i, j];
                    blocks[i, j] = blocks[r, t];
                    blocks[r, t] = temp;
                }
            }
        }
        /// <summary>
        /// 直线连接
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="y1"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        private bool CanLinkDirectly(int x1, int y1, int x2, int y2)
        {
            //if (blocks[y1, x1] == blocks[y2, x2]) return false;
            if (y1 == y2)
            {
                if(x1<x2)
                {
                    int t = x1 + 1;
                    while(t<x2)
                    {
                        if(blocks[y1, t] != 0) return false;
                        t++;
                    }
                }
                else
                {
                    int t = x2 + 1;
                    while (t < x1)
                    {
                        if (blocks[y1, t] != 0) return false;
                        t++;
                    }
                }
                return true;
            }
            if (x1 == x2)
            {
                if(y1<y2)
                {
                    int t = y1 + 1;
                    while (t < y2)
                    {
                        if (blocks[t, x1] != 0) return false;
                        t++;
                    }
                }
                else
                {
                    int t = y2 + 1;
                    while (t < y1)
                    {
                        if (blocks[t, x1] != 0) return false;
                        t++;
                    }
                }
                return true;
            }
            return false;
        }
        private bool CanLinkDirectlyGo(int x1, int y1, int x2, int y2)
        {
            if (blocks[y1, x1] != 0 && blocks[y2, x2] != 0)
            {
                if (y1 == y2)
                {
                    if (x1 < x2)
                    {
                        int t = x1 + 1;
                        while (t < x2)
                        {
                            if (blocks[y1, t] != 0) return false;
                            t++;
                        }
                    }
                    else
                    {
                        int t = x2 + 1;
                        while (t < x1)
                        {
                            if (blocks[y1, t] != 0) return false;
                            t++;
                        }
                    }
                    return true;
                }
                if (x1 == x2)
                {
                    if (y1 < y2)
                    {
                        int t = y1 + 1;
                        while (t < y2)
                        {
                            if (blocks[t, x1] != 0) return false;
                            t++;
                        }
                    }
                    else
                    {
                        int t = y2 + 1;
                        while (t < y1)
                        {
                            if (blocks[t, x1] != 0) return false;
                            t++;
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        public bool Onelink(int x1, int y1, int x2, int y2, out List<Dpoint> cps)
        {
            cps = new List<Dpoint>();
            if (blocks[y1, x1] != 0 && blocks[y2, x2] != 0)
            {
                if (CanLinkDirectly(x1, y1, x1, y2) && CanLinkDirectly(x2, y2, x1, y2) && blocks[y2, x1] == 0)
                {
                    Dpoint p = new Dpoint(x1, y2);
                    cps.Add(p);
                    return true;
                }
                else if (CanLinkDirectly(x1, y1, x2, y1) && CanLinkDirectly(x2, y2, x2, y1) && blocks[y1, x2] == 0)
                {
                    Dpoint p = new Dpoint(x2, y1);
                    cps.Add(p);
                    return true;
                }
            }
            return false;
        }
        public bool Twolink(int x1, int y1, int x2, int y2, out List<Dpoint> cps)
        {
            int i;
            cps = new List<Dpoint>();
            if (blocks[y1, x1] != 0 && blocks[y2, x2] != 0)
            {

                for (i = 0; i <= Width + 1; i++)
                {
                    if (CanLinkDirectly(i, y1, x1, y1) && CanLinkDirectly(i, y2, x2, y2) && CanLinkDirectly(i, y1, i, y2) && blocks[y1, i] == 0 && blocks[y2, i] == 0)
                    {
                        Dpoint p1 = new Dpoint(i, y1);
                        Dpoint p2 = new Dpoint(i, y2);
                        cps.Add(p1);
                        cps.Add(p2);
                        return true;
                    }
                }
                for (i = 0; i <= Height + 1; i++)
                {
                    if (CanLinkDirectly(x1, y1, x1, i) && CanLinkDirectly(x2, y2, x2, i) && CanLinkDirectly(x1, i, x2, i) && blocks[i, x1] == 0 && blocks[i, x2] == 0)
                    {
                        Dpoint p1 = new Dpoint(x1, i);
                        Dpoint p2 = new Dpoint(x2, i);
                        cps.Add(p1);
                        cps.Add(p2);
                        return true;
                    }
                }
            }
            return false;
        }
        public bool LinkMatch(int x1, int y1, int x2, int y2, out List<Dpoint> cps)
        {
           if(CanLinkDirectlyGo(x1,y1,x2,y2))
           {
               cps = new List<Dpoint>();
               return true;
           }
           else if(Onelink(x1,y1,x2,y2,out cps))
           {
               return true;
           }
            else if(Twolink(x1,y1,x2,y2,out cps))
           {
               return true;
           }
           return false;
        }
        public bool GameWin()
        {
            for (int i = 1; i < Height + 1; i++)
            {
                for (int j = 1; j < Width + 1; j++)
                {
                    if (blocks[i, j] != 0) return false;
                }
            }
            return true;
        }
        public void ClearBlock(Block b)
        {
            blocks[b.Y, b.X] = 0;
        }
        public Size GetMapSize()
        {
            return new Size((Width + 2) * Block.Blockwidth, (Height + 2) * Block.Blockheight);
        }
        public override string ToString()
        {
            string s = "";
            for(int i=0;i<=Height+1;i++)
            {
                for(int j=0;j<=Width+1;j++)
                {
                    s += string.Format("{0,-3}", blocks[i, j]);
                }
                s += "\r\n";
            }
            return s;
        }
    }
}
