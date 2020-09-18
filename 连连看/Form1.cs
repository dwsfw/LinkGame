using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 连连看
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        BlockMap map = new BlockMap();
        Block first, second;
        int cnt = 0;
        int sum = 3;
        int won = 1;
        int fen = 0;
        int zong = 0;
        int h = 5;
        int w = 6;
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Size = map.GetMapSize();
            if (cnt == 0)
            {
                if (map != null) map.DeOrderMap();
                first = null;
                this.pictureBox1.Invalidate();
                button2.Text = "刷新（" + sum + ")";
                cnt = 1;
                button1.Enabled = false;
            }
            Graphics g = this.pictureBox1.CreateGraphics();
            PaintMap(g);
        }
        private void PaintMap(Graphics g)
        {
            for(int i=1;i<map.Height+1;i++)
            {
                for(int j=1;j<map.Width+1;j++)
                {
                    if (map[i, j] == 0) continue;
                    bmp = (Bitmap)images.ResourceManager.GetObject("_" + map[i, j]);
                    g.DrawImage(bmp, j * Block.Blockwidth, i * Block.Blockheight);
                }
            }
            if (first != null)
            {
                if (map[first.Y, first.X] > 0)
                {
                    bmp = (Bitmap)images.ResourceManager.GetObject("_" + map[first.Y, first.X] + "_L2");
                    if(bmp!=null)
                    g.DrawImage(bmp, first.X * Block.Blockwidth, first.Y * Block.Blockheight);
                }
            }
            if (second != null)
            {
                if (map[second.Y, second.X] > 0)
                {
                    bmp = (Bitmap)images.ResourceManager.GetObject("_" + map[second.Y, second.X] + "_L2");
                    if(bmp!=null)
                    g.DrawImage(bmp, second.X * Block.Blockwidth, second.Y * Block.Blockheight);
                }
            }
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (map == null) return;
            PaintMap(e.Graphics);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (sum > 0)
            {
                if (map != null) map.DeOrderMap();
                first = null;
                this.pictureBox1.Invalidate();
                sum--;
                button2.Text = "刷新（" + sum + ")";
                if (sum == 0) button2.Enabled = false;
            }
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (map.GameWin())
            {
                if (won ==2)
                {
                    MessageBox.Show("恭喜你，通关啦！！！");
                }
                else
                {
                    MessageBox.Show("You are winner");
                    won++;
                    h += 1;
                    w += 2;
                    map = new BlockMap(h, w);
                    zong = zong + fen;
                    fen = 0;
                    this.pictureBox1.Size = map.GetMapSize();
                    if (map != null) map.DeOrderMap();
                    first = null;
                    this.pictureBox1.Invalidate();
                    sum = 3;
                    button2.Text = "刷新（" + sum + ")";
                    Graphics g = this.pictureBox1.CreateGraphics();
                    PaintMap(g);
                    label1.Text = "第" + won + "关";
                    label2.Text = "总分数：" + zong;
                    label3.Text = "本关分数：" + fen;
                }
            }
            else
            {
                int w = e.X / Block.Blockwidth;
                int h = e.Y / Block.Blockheight;
                if (first == null)
                {
                    first = new Block(w, h);
                    second = null;
                    this.pictureBox1.Invalidate();
                    return;
                }
                else if (map[first.Y, first.X] == map[h, w] && (first.X != w || first.Y != h))
                {
                    second = new Block(w, h);
                    Pen p = new Pen(Color.Red, 2);
                    Graphics g = this.pictureBox1.CreateGraphics();
                    Link(first, second, g, p);
                    this.pictureBox1.Invalidate();
                }
                else
                {
                    first = new Block(w, h);
                    second = null;
                    this.pictureBox1.Invalidate();
                }
            }
        }
        public void Link(Block first, Block second, Graphics g, Pen p)
        {
            List<Dpoint> cps = null;
            if (map.LinkMatch(first.X, first.Y, second.X, second.Y, out cps))
            {
                Point[] ps = new Point[2 + cps.Count];
                ps[0] = new Point(first.X * Block.Blockwidth + Block.Blockwidth / 2,first.Y * Block.Blockheight + Block.Blockheight / 2);
                int i = 1;
                while (i <= cps.Count)
                {   ps[i] = new Point(cps[i - 1].X * Block.Blockwidth + Block.Blockwidth / 2,cps[i - 1].Y * Block.Blockheight + Block.Blockheight / 2);
                    i++;}
                ps[i] = new Point(second.X * Block.Blockwidth + Block.Blockwidth / 2,second.Y * Block.Blockheight + Block.Blockheight / 2);
                g.DrawLines(p, ps);
                Thread.Sleep(100);
                map.ClearBlock(first);
                map.ClearBlock(second);//消去这两个图形
                fen += 20*won;
                label3.Text = "分数：" + fen;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            map = new BlockMap(h,w);
            sum = 3;
            button2.Text = "刷新（" + sum + ")";
            button2.Enabled = true;
            fen = 0;
            label3.Text = "本关分数：" + fen;
            if (map != null) map.DeOrderMap();
            first = null;
            this.pictureBox1.Invalidate();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            h = 5;
            w = 6;
            map = new BlockMap(h, w);
            sum = 3;
            zong = 0;
            button2.Text = "刷新（" + sum + ")";
            button2.Enabled = true;
            fen = 0;
            won = 1;
            label1.Text = "第" + won + "关";
            label2.Text = "总分数：" + zong;
            label3.Text = "本关分数：" + fen;
            if (map != null) map.DeOrderMap();
            first = null;
            this.pictureBox1.Invalidate();
        }
    }
}
