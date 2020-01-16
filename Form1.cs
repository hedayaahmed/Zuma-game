using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Zuma
{
    public class CActorFrog
    {
        public int X, Y;
        public float angle;
        public Bitmap im;
    }

    public class CActorLine
    {
        public float XS, YS, XE, YE;
        public float dx, dy, m, inv_m;
    }

    public class CActorBall
    {
        public float xC, yC;
        public int Clr;
        public int iL;
        public Bitmap im;
        public int dir;
        public int flag = 1;
    }

    public partial class Form1 : Form
    {
        Bitmap off;
        Bitmap Background = new Bitmap("Background.png");
        Timer t = new Timer();

        List<CActorFrog> Frog = new List<CActorFrog>();
        List<CActorLine> Line = new List<CActorLine>();
        List<CActorBall> Ball = new List<CActorBall>();
        List<CActorLine> LBall = new List<CActorLine>();

        BezierCurve Curve = new BezierCurve();
        List<CActorBall> B = new List<CActorBall>();

        //int XS, YS;
        Bitmap image;
        int FlagIm = 0;
        float move1 = 0;
        int FlagB = 0;
        int CountTick = 0;
        int FlagL = 0;

        Random RR = new Random();

        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Load += new EventHandler(Form1_Load);
            this.Paint += new PaintEventHandler(Form1_Paint);
            this.MouseDown += new MouseEventHandler(Form1_MouseDown);
            this.MouseMove += new MouseEventHandler(Form1_MouseMove);
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            t.Tick += new EventHandler(t_Tick);
            t.Interval = 50;
            t.Start();
        }

        void t_Tick(object sender, EventArgs e)
        {
            if (FlagB == 1)
            {
                if (CountTick % 8 == 0)
                {
                    CActorBall pnn = new CActorBall();
                    pnn.xC = Line[0].XS;
                    pnn.yC = Line[0].YS;
                    pnn.dir = 1;
                    pnn.iL = 0;
                    int N = RR.Next(0, 4);
                    if (N == 0)
                    {
                        pnn.Clr = 1;
                        pnn.im = new Bitmap("b1.png");
                        pnn.im.MakeTransparent(pnn.im.GetPixel(0, 0));
                    }
                    if (N == 1)
                    {
                        pnn.Clr = 2;
                        pnn.im = new Bitmap("b2.png");
                        pnn.im.MakeTransparent(pnn.im.GetPixel(0, 0));
                    }
                    if (N == 2)
                    {
                        pnn.Clr = 3;
                        pnn.im = new Bitmap("b3.png");
                        pnn.im.MakeTransparent(pnn.im.GetPixel(0, 0));
                    }
                    if (N == 3)
                    {
                        pnn.im = new Bitmap("b4.png");
                        pnn.im.MakeTransparent(pnn.im.GetPixel(0, 0));
                        pnn.Clr = 4;
                    }
                    Ball.Add(pnn);
                }
                for (int i = 0; i < Ball.Count; i++)
                {
                    if (Ball[i].dir == 1)
                    {
                        if (Math.Abs(Line[Ball[i].iL].dx) > Math.Abs(Line[Ball[i].iL].dy))
                        {
                            if (Line[Ball[i].iL].XS < Line[Ball[i].iL].XE)
                            {
                                Ball[i].xC += 5;
                                Ball[i].yC += Line[Ball[i].iL].m * 5;
                                if (Ball[i].xC > Line[Ball[i].iL].XE)
                                {
                                    Ball[i].iL++;

                                    if (Ball[i].iL >= Line.Count)
                                    {
                                        Ball.Remove(Ball[i]);
                                        i++;
                                    }
                                }
                            }
                            else
                            {
                                Ball[i].xC -= 5;
                                Ball[i].yC -= Line[Ball[i].iL].m * 5;
                                if (Ball[i].xC < Line[Ball[i].iL].XE)
                                {
                                    Ball[i].iL++;
                                    if (Ball[i].iL >= Line.Count)
                                    {
                                        Ball.Remove(Ball[i]);
                                        i++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Line[Ball[i].iL].YS < Line[Ball[i].iL].YE)
                            {
                                Ball[i].yC += 5;
                                Ball[i].xC += Line[Ball[i].iL].inv_m * 5;
                                if (Ball[i].yC > Line[Ball[i].iL].YE)
                                {
                                    Ball[i].iL++;
                                    if (Ball[i].iL >= Line.Count)
                                    {
                                        Ball.Remove(Ball[i]);
                                        i++;
                                    }
                                }
                            }
                            else
                            {
                                Ball[i].yC -= 5;
                                Ball[i].xC -= Line[Ball[i].iL].inv_m * 5;
                                if (Ball[i].yC < Line[Ball[i].iL].YE)
                                {
                                    Ball[i].iL++;
                                    if (Ball[i].iL >= Line.Count)
                                    {
                                        Ball.Remove(Ball[i]);
                                        i++;
                                    }
                                }
                            }
                        }
                    }
                }
                ////////////////////////////////////////////////Curve Ball Creation & Movement////////////////////////////////////

                if (FlagL == 0)
                {
                    int flag = -1;
                    if (Math.Abs(LBall[0].dx) > Math.Abs(LBall[0].dy))
                    {
                        if (LBall[0].XS < LBall[0].XE)
                        {
                            B[0].xC += 8;
                            B[0].yC += LBall[0].m * 8;
                            for (int i = 0; i < Ball.Count; i++)
                            {
                                if (B[0].xC >= Ball[i].xC && B[0].xC <= (Ball[i].xC + Ball[i].im.Width)
                                        && B[0].yC >= Ball[i].yC && B[0].yC <= (Ball[i].yC + Ball[i].im.Height))
                                {
                                    //MessageBox.Show("ok");
                                    
                                    
                                    int c = B[0].Clr;
                                    if(Ball[i].Clr == c)
                                    {
                                        for (int k = i + 1; k >=0; k--)
                                        {
                                            int cl= Ball[k].Clr;
                                            if (cl == c)
                                            {
                                                Ball.Remove(Ball[k]);
                                                k++;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        for (int k = i - 1; k <Ball.Count; k++)
                                        {
                                            int cl = Ball[k].Clr;
                                            if (cl == c)
                                            {
                                                Ball.Remove(Ball[k]);
                                                k--;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }

                                        /*for (int k = i; k < Ball.Count; k++)
                                        {
                                            Ball[i].dir = 0;
                                        }*/
                                    }
                                    else
                                    {
                                        B[0].xC = Ball[i].xC;
                                        B[0].yC = Ball[i].yC;
                                        B[0].iL = Ball[i].iL;
                                        Ball.Add(B[0]);
                                    }
                                    /*
                                    for (int k = i+1; k >= 0; k--)
                                    {
                                        char c = Ball[k].Clr;
                                        if (c == Cb)
                                        {
                                            flag = 1;
                                            Ball.Remove(Ball[k]);
                                            Ball.Remove(Ball[Ball.Count - 1]);
                                            k--;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }*/
                                    FlagL = 1;
                                    flag = i;
                                    B.Remove(B[0]);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            B[0].xC -= 8;
                            B[0].yC -= LBall[0].m * 8;

                            for (int i = 0; i < Ball.Count; i++)
                            {
                                if (B[0].xC >= Ball[i].xC && B[0].xC <= (Ball[i].xC + Ball[i].im.Width)
                                        && B[0].yC >= Ball[i].yC && B[0].yC <= (Ball[i].yC + Ball[i].im.Height))
                                {
                                    //MessageBox.Show("ok");
                                    int c = B[0].Clr;
                                    if (Ball[i].Clr == c)
                                    {
                                        for (int k = i + 1; k >= 0; k--)
                                        {
                                            int cl = Ball[k].Clr;
                                            if (cl == c)
                                            {
                                                Ball.Remove(Ball[k]);
                                                k++;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        for (int k = i - 1; k < Ball.Count; k++)
                                        {
                                            int cl = Ball[k].Clr;
                                            if (cl == c)
                                            {
                                                Ball.Remove(Ball[k]);
                                                k--;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {


                                        B[0].xC = Ball[i].xC;
                                        B[0].yC = Ball[i].yC;
                                        B[0].iL = Ball[i].iL;
                                        Ball.Add(B[0]);
                                    }
                                    FlagL = 1;
                                    flag = i;
                                    B.Remove(B[0]);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (LBall[0].YS < LBall[0].YE)
                        {
                            B[0].yC += 8;
                            B[0].xC += LBall[0].inv_m * 8;

                            for (int i = 0; i < Ball.Count; i++)
                            {
                                if (B[0].xC >= Ball[i].xC && B[0].xC <= (Ball[i].xC + Ball[i].im.Width)
                                        && B[0].yC >= Ball[i].yC && B[0].yC <= (Ball[i].yC + Ball[i].im.Height))
                                {
                                    //MessageBox.Show("ok");
                                    int c = B[0].Clr;
                                    if (Ball[i].Clr == c)
                                    {
                                        for (int k = i + 1; k >= 0; k--)
                                        {
                                            int cl = Ball[k].Clr;
                                            if (cl == c)
                                            {
                                                Ball.Remove(Ball[k]);
                                                k++;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        for (int k = i - 1; k < Ball.Count; k++)
                                        {
                                            int cl = Ball[k].Clr;
                                            if (cl == c)
                                            {
                                                Ball.Remove(Ball[k]);
                                                k--;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        B[0].xC = Ball[i].xC;
                                        B[0].yC = Ball[i].yC;
                                        B[0].iL = Ball[i].iL;
                                        Ball.Add(B[0]);
                                    }
                                    FlagL = 1;
                                    flag = i;
                                    B.Remove(B[0]);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            B[0].yC -= 8;
                            B[0].xC -= LBall[0].inv_m * 8;

                            for (int i = 0; i < Ball.Count; i++)
                            {
                                if (B[0].xC >= Ball[i].xC && B[0].xC <= (Ball[i].xC + Ball[i].im.Width)
                                        && B[0].yC >= Ball[i].yC && B[0].yC <= (Ball[i].yC + Ball[i].im.Height))
                                {
                                    //MessageBox.Show("ok");
                                    int c = B[0].Clr;
                                    if (Ball[i].Clr == c)
                                    {
                                        for (int k = i + 1; k >= 0; k--)
                                        {
                                            int cl = Ball[k].Clr;
                                            if (cl == c)
                                            {
                                                Ball.Remove(Ball[k]);
                                                k++;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        for (int k = i - 1; k < Ball.Count; k++)
                                        {
                                            int cl = Ball[k].Clr;
                                            if (cl == c)
                                            {
                                                Ball.Remove(Ball[k]);
                                                k--;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        B[0].xC = Ball[i].xC;
                                        B[0].yC = Ball[i].yC;
                                        B[0].iL = Ball[i].iL;
                                        Ball.Add(B[0]);
                                    }
                                    FlagL = 1;
                                    flag = i;
                                    B.Remove(B[0]);
                                    break;
                                }
                            }
                        }
                    }
                    if (flag != -1)
                    {
                        
                       /* for (int i = 0; i < flag; i++)
                        {
                            MessageBox.Show("ok");
                            if (Ball[i].dir == 1)
                            {
                                if (Math.Abs(Line[Ball[i].iL].dx) > Math.Abs(Line[Ball[i].iL].dy))
                                {
                                    if (Line[Ball[i].iL].XS < Line[Ball[i].iL].XE)
                                    {
                                        Ball[i].xC += 5;
                                        Ball[i].yC += Line[Ball[i].iL].m * 5;
                                        if (Ball[i].xC > Line[Ball[i].iL].XE)
                                        {
                                            Ball[i].iL++;

                                            if (Ball[i].iL >= Line.Count)
                                            {
                                                Ball.Remove(Ball[i]);
                                                i++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Ball[i].xC -= 5;
                                        Ball[i].yC -= Line[Ball[i].iL].m * 5;
                                        if (Ball[i].xC < Line[Ball[i].iL].XE)
                                        {
                                            Ball[i].iL++;
                                            if (Ball[i].iL >= Line.Count)
                                            {
                                                Ball.Remove(Ball[i]);
                                                i++;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (Line[Ball[i].iL].YS < Line[Ball[i].iL].YE)
                                    {
                                        Ball[i].yC += 5;
                                        Ball[i].xC += Line[Ball[i].iL].inv_m * 5;
                                        if (Ball[i].yC > Line[Ball[i].iL].YE)
                                        {
                                            Ball[i].iL++;
                                            if (Ball[i].iL >= Line.Count)
                                            {
                                                Ball.Remove(Ball[i]);
                                                i++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Ball[i].yC -= 5;
                                        Ball[i].xC -= Line[Ball[i].iL].inv_m * 5;
                                        if (Ball[i].yC < Line[Ball[i].iL].YE)
                                        {
                                            Ball[i].iL++;
                                            if (Ball[i].iL >= Line.Count)
                                            {
                                                Ball.Remove(Ball[i]);
                                                i++;
                                            }
                                        }
                                    }
                                }
                            }
                        }*/
                    }
                }

                CountTick++;
            }
            
            DrawDubb();
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    while (move1 <= 1)
                    {

                        PointF Point1 = Curve.CalcCurvePointAtTime(move1);
                        CActorLine pnn = new CActorLine();
                        pnn.XS = Point1.X;
                        pnn.YS = Point1.Y;
                        move1 += 0.01f;
                        Point1 = Curve.CalcCurvePointAtTime(move1);
                        pnn.XE = Point1.X;
                        pnn.YE = Point1.Y;

                        pnn.dx = pnn.XE - pnn.XS;
                        pnn.dy = pnn.YE - pnn.YS;
                        pnn.m = pnn.dy / pnn.dx;
                        pnn.inv_m = pnn.dx / pnn.dy;
                        Line.Add(pnn);
                    }
                    FlagB = 1;
                    FlagL = 1;
                    break;
                case Keys.Space:
                    FlagL = 0;
                    LBall[0].dx = LBall[0].XE - LBall[0].XS;
                    LBall[0].dy = LBall[0].YE - LBall[0].YS;
                    LBall[0].m = LBall[0].dy / LBall[0].dx;
                    LBall[0].inv_m = LBall[0].dx / LBall[0].dy;

                    CActorBall pnn2 = new CActorBall();
                    pnn2.xC = LBall[0].XS;
                    pnn2.yC = LBall[0].YS;
                    pnn2.dir = 0;
                    int N = RR.Next(0, 4);
                    if (N == 0)
                    {
                        pnn2.Clr = 1;
                        pnn2.im = new Bitmap("b1.png");
                        pnn2.im.MakeTransparent(pnn2.im.GetPixel(0, 0));
                    }
                    if (N == 1)
                    {
                        pnn2.Clr = 2;
                        pnn2.im = new Bitmap("b2.png");
                        pnn2.im.MakeTransparent(pnn2.im.GetPixel(0, 0));
                    }
                    if (N == 2)
                    {
                        pnn2.Clr = 3;
                        pnn2.im = new Bitmap("b3.png");
                        pnn2.im.MakeTransparent(pnn2.im.GetPixel(0, 0));
                    }
                    if (N == 3)
                    {
                        pnn2.im = new Bitmap("b4.png");
                        pnn2.im.MakeTransparent(pnn2.im.GetPixel(0, 0));
                        pnn2.Clr = 4;
                    }
                    B.Add(pnn2);
                    B[0].dir = 1;
                    break;
            }
        }

        public Bitmap rotateImage(Bitmap bitmap, float angle)
        {
            Bitmap returnBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            Graphics graphics = Graphics.FromImage(returnBitmap);
            graphics.TranslateTransform((float)bitmap.Width / 2, (float)bitmap.Height / 2);
            graphics.RotateTransform(angle);
            graphics.TranslateTransform(-(float)bitmap.Width / 2, -(float)bitmap.Height / 2);
            graphics.DrawImage(bitmap, new Point(0, 0));
            return returnBitmap;
        }

        void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (FlagB == 1 && FlagL == 1)
            {
                LBall[0].XE = e.X;
                LBall[0].YE = e.Y;
            }

            FlagIm = 1;
            Frog[0].angle = (float)(Math.Atan2((e.Y - Frog[0].Y), (e.X - Frog[0].X)));
            Frog[0].angle = (float)(Frog[0].angle * 180 / (Math.PI));

            Frog[0].im = rotateImage(image, Frog[0].angle-90);

            // double x = (e.X - LBall[0].XS) * Math.Cos(90 * Math.PI / 180) + LBall[0].XS;
            // double y = (e.Y - LBall[0].YS) * Math.Cos(90 * Math.PI / 180) + LBall[0].YS;
            if (FlagL == 1)
            {
                LBall[0].XE = e.X - 10;
                LBall[0].YE = e.Y - 10;
            }
        }

        void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                BezierCurve pnn = new BezierCurve();
                Curve.SetControlPoint(new Point(e.X, e.Y));
            }
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb();
        }

        void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            CActorFrog pnn = new CActorFrog();
            pnn.X = this.ClientSize.Width / 2 - 150;
            pnn.Y = this.ClientSize.Height / 2 - 80;
            pnn.angle = 0;
            pnn.im = new Bitmap("frog.png");
            Frog.Add(pnn);


            image = Frog[0].im;
           /* XS = this.ClientSize.Width / 2 - 100;
            YS = this.ClientSize.Height / 2 - 50;*/
            /////////////////////////////////////////////////Frog Creation/////////////////////////////

            CActorLine pnn1 = new CActorLine();
            pnn1.XS = this.ClientSize.Width / 2 - 50;
            pnn1.YS = this.ClientSize.Height / 2;
            pnn1.XE = this.ClientSize.Width / 2 - 50;
            pnn1.YE = (this.ClientSize.Height / 2 )+ 200;
            LBall.Add(pnn1);

            CActorBall pnn2 = new CActorBall();
            pnn2.xC = this.ClientSize.Width / 2 - 50;
            pnn2.yC = this.ClientSize.Height / 2;
            pnn2.dir = 0;
            int N = RR.Next(0, 4);
            if (N == 0)
            {
                pnn2.Clr = 1;
                pnn2.im = new Bitmap("b1.png");
                pnn2.im.MakeTransparent(pnn2.im.GetPixel(0, 0));
            }
            if (N == 1)
            {
                pnn2.Clr = 2;
                pnn2.im = new Bitmap("b2.png");
                pnn2.im.MakeTransparent(pnn2.im.GetPixel(0, 0));
            }
            if (N == 2)
            {
                pnn2.Clr = 3;
                pnn2.im = new Bitmap("b3.png");
                pnn2.im.MakeTransparent(pnn2.im.GetPixel(0, 0));
            }
            if (N == 3)
            {
                pnn2.im = new Bitmap("b4.png");
                pnn2.im.MakeTransparent(pnn2.im.GetPixel(0, 0));
                pnn2.Clr = 4;
            }
            B.Add(pnn2);
            
        }

        void DrawScene(Graphics g2)
        {
            g2.Clear(Color.Black);
            int i;

            Rectangle rcDst = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            Rectangle rcScr = new Rectangle(0, 0, this.ClientSize.Width + 500, this.ClientSize.Height + 300);
            g2.DrawImage(Background, rcDst, rcScr, GraphicsUnit.Pixel);


            Pen P = new Pen(Color.Yellow, 5);
            if (FlagIm == 1)
            {
                if (FlagB == 1)
                {
                    for (i = 0; i < LBall.Count; i++)
                    {
                        g2.DrawLine(P, LBall[i].XS, LBall[i].YS, LBall[i].XE, LBall[i].YE);
                    }
                }
                
                g2.DrawImage(Frog[0].im, Frog[0].X, Frog[0].Y);
                for (i = 0; i < B.Count; i++)
                {
                    g2.DrawImage(B[i].im, B[i].xC, B[i].yC);
                }
            }

            Curve.DrawCurve(g2);

            P = new Pen(Color.Black, 3);
            for (i = 0; i < Line.Count; i++)
            {
                g2.DrawLine(P, Line[i].XS, Line[i].YS, Line[i].XE, Line[i].YE);
            }

            for (i = 0; i < Ball.Count; i++)
            {
                g2.DrawImage(Ball[i].im, Ball[i].xC, Ball[i].yC);
            }

        }

        void DrawDubb()
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            Graphics g = this.CreateGraphics();
            g.DrawImage(off, 0, 0);
        }
    }
}
