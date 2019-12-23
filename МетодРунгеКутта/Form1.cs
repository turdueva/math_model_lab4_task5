using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace МетодРунгеКутта
{
    public partial class Form1 : Form
    {
        double E, T, tay, fi, sigma, y1, y2, y3, m; 
        double v0, a, b;
        double[] t; 
        double[] U;

    


        public Form1()
        {
            InitializeComponent();
            task.Items.Add("Система");
          
            for (int k = 1; k < 10; k++)
            {
                cmb4.Items.Add(Math.Pow(2, -k));
            }
            for (int k = 2; k < 10; k++)
            {
                cmb3.Items.Add(1 + Math.Pow(2, k));
            }

            GraphPane pane = zedGraphControl1.GraphPane;
            pane.XAxis.Title.Text = "Ось t";
            pane.XAxis.Title.FontSpec.FontColor = Color.Black;
            pane.YAxis.Title.Text = "Ось Y";
            pane.YAxis.Title.FontSpec.FontColor = Color.Black;
            pane.Title.Text = "Графики";
            pane.Title.FontSpec.FontColor = Color.Black;
        }
          
        

        private void Form1_Load(object sender, EventArgs e)
        {
           task.Text= "Система";
           cmb4.Text = Convert.ToString(0.5);
           cmb3.Text = Convert.ToString(5);
            txtb1.Text = Convert.ToString(1);
            textBox3.Text = Convert.ToString(1);
            textBox5.Text = Convert.ToString(1);
            textBox4.Text = Convert.ToString(1);
            comboBox1.Text = Convert.ToString(1);
            txtb2.Text = Convert.ToString(1);
            m = Convert.ToDouble(cmb3.Text);
            E = Convert.ToDouble(cmb4.Text);
            T = Convert.ToDouble(comboBox1.Text);

        }
      
        //******************************одностадийный метод для системы************************************************
       private void Method1system()
        {
            t = new double[Convert.ToInt32(m + 1)];
            for (int k = 0; k <= m; k++)
            {
                t[k] = k * tay;
            }
            double[,] S = new double[Convert.ToInt32(m + 1),Convert.ToInt32(m + 1)];
            fi = 0;
            v0 = 0;
            fi = Convert.ToDouble(txtb1.Text);
            v0 = Convert.ToDouble(textBox4.Text);

            S[0, 0] = fi;
            S[1, 0] = v0 ;
            for (int i = 0; i <= m - 1; i++)
            {
                S[0, i + 1] = S[0, i] + tay* Fu(t[i], S[0, i], S[1, i]);
                S[1, i + 1] = S[1, i] + tay * Fv(t[i], S[0, i], S[1, i]);
            }
            GraphPane pane = zedGraphControl1.GraphPane;
            PointPairList listsys1 = new PointPairList();
            for (int i = 0; i < m; i++)
            {

                listsys1.Add(S[0, i], S[1, i]);
            }
            LineItem curve = pane.AddCurve("Одностадийный", listsys1, Color.Green, SymbolType.None);
            curve.Symbol.Fill.Type = FillType.Solid;
            curve.Line.Width = 3;
            curve.Symbol.Size = 2;
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            tbErr1.Text = Convert.ToString(Error1(S));
       
        }

        //*****************************трехстадийный метод для системы************************************** 
        private void Method3system()
        {
            t = new double[Convert.ToInt32(m + 1)];
            for (int k = 0; k <= m; k++)
            {
                t[k] = k * tay;
            }
            double[,] S = new double[Convert.ToInt32(m + 1),Convert.ToInt32(m + 1)];
            fi = 0;
            fi = Convert.ToDouble(txtb1.Text);
            sigma = Convert.ToDouble(txtb2.Text);
            S[0, 0] = fi;
            S[1, 0] = v0 ;
            double[] y1 = new double[Convert.ToInt32(m + 1)];
            double[] y2 = new double[Convert.ToInt32(m + 1)];
            for (int i = 0; i <= m - 1; i++)
            {
                y1[0] = S[0, i] + (2.0 * tay / 3.0) * Fu(t[i], S[0, i], S[1, i]);
                y1[1] = S[1, i] + (2.0 * tay / 3.0) * Fv(t[i], S[0, i], S[1, i]);
                y2[0] = S[0, i] + (2.0 * tay / 3) * ((1 - 3.0 / (8 * sigma)) * Fu(t[i], S[0, i], S[1, i]) + (3.0 / (8.0 * sigma)) * Fu(t[i] + 2.0 * tay / 3.0, y1[0], y1[1]));
                y2[1] = S[1, i] + (2.0 * tay / 3) * ((1 - 3.0 / (8 * sigma)) * Fv(t[i], S[0, i], S[1, i]) + (3.0 / (8.0 * sigma)) * Fv(t[i] + 2.0 * tay / 3.0, y1[0], y1[1]));
                S[0, i + 1] = S[0, i] + tay * (1.0 / 4.0 * Fu(t[i], S[0, i], S[1, i]) + (3.0 / 4.0 - sigma) * Fu(t[i] + 2.0 * tay / 3.0, y1[0], y1[1]) + sigma * Fu(t[i] + 2.0 * tay / 3.0, y2[0], y2[1]));
                S[1, i + 1] = S[1, i] + tay * (1.0 / 4.0 * Fv(t[i], S[0, i], S[1, i]) + (3.0 / 4.0 - sigma) * Fv(t[i] + 2.0 * tay / 3.0, y1[0], y1[1]) + sigma * Fv(t[i] + 2.0 * tay / 3.0, y2[0], y2[1]));
               }
            GraphPane pane = zedGraphControl1.GraphPane;
            PointPairList listsys3 = new PointPairList();
            for (int i = 0; i < m; i++)
            {

                listsys3.Add(S[0, i], S[1, i]);
            }
            LineItem curve = pane.AddCurve("Трехстадийный", listsys3, Color.Blue, SymbolType.None);
            curve.Symbol.Fill.Type = FillType.Solid;
            curve.Line.Width = 3;
            curve.Symbol.Size = 2;
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            tbErr3.Text = Convert.ToString(Error1(S));
        }

        //*************************четырехстадийный метод для системы**************************************
        private void Method4system()
        {
            t = new double[Convert.ToInt32(m + 1)];
            for (int k = 0; k <= m; k++)
            {
                t[k] = k * tay;
            }
            double[,] S = new double[Convert.ToInt32(m + 1), Convert.ToInt32(m + 1)];
            fi = 0;
            fi = Convert.ToDouble(txtb1.Text);
            S[0, 0] = fi;
            S[1, 0] = v0;
            double[] y1 = new double[Convert.ToInt32(m + 1)];
            double[] y2 = new double[Convert.ToInt32(m + 1)];
            double[] y3 = new double[Convert.ToInt32(m + 1)];

            for (int i = 0; i <= m - 1; i++)
            {
                y1[0] = S[0, i] + (tay / 4.0) * Fu(t[i], S[0, i], S[1, i]);
                y1[1] = S[1, i] + (tay / 4.0) * Fv(t[i], S[0, i], S[1, i]);
                y2[0] = S[0, i] + (tay * Fu(t[i] + (tay / 4.0), y1[0], y1[1])) / 2.0;
                y2[1] = S[1, i] + (tay * Fv(t[i] + (tay / 4.0), y1[0], y1[1])) / 2.0;
                y3[0] = S[0, i] + tay * (Fu(t[i], S[0, i], S[1, i]) - 2.0 * Fu(t[i] + (tay / 4.0), y1[0], y1[1]) + 2.0 * Fu(t[i] + (tay / 2.0), y2[0], y2[1]));
                y3[1] = S[1, i] + tay * (Fv(t[i], S[0, i], S[1, i]) - 2.0 * Fv(t[i] + (tay / 4.0), y1[0], y1[1]) + 2.0 * Fv(t[i] + (tay / 2.0), y2[0], y2[1]));
                S[0, i + 1] = S[0, i] + (tay / 6.0) * (Fu(t[i], S[0, i], S[1, i]) + (4.0 * Fu(t[i] + (tay / 2.0), y2[0], y2[1])) + Fu(t[i + 1], y3[0], y3[1]));
                S[1, i + 1] = S[1, i] + (tay / 6.0) * (Fv(t[i], S[0, i], S[1, i]) + (4.0 * Fv(t[i] + (tay / 2.0), y2[0], y2[1])) + Fv(t[i + 1], y3[0], y3[1]));
            }

            GraphPane pane = zedGraphControl1.GraphPane;
            PointPairList listsys4 = new PointPairList();
            for (int i = 0; i < m; i++)
            {

                listsys4.Add(S[0, i], S[1, i]);
            }
            LineItem curve = pane.AddCurve("Четырехстадийный", listsys4, Color.Black, SymbolType.None);
            curve.Symbol.Fill.Type = FillType.Solid;
            curve.Line.Width = 3;
            curve.Symbol.Size = 2;
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            tbErr4.Text = Convert.ToString(Error1(S));
        }

       //*******************************для системы***************************************************
       public double Fu(double t, double u, double v)
       {
           return -v;
       }

       public double Fv(double t, double u, double v)
       {
           a = Convert.ToDouble(textBox3.Text);
           b = Convert.ToDouble(textBox5.Text);
           return b * u + a * v;
       }

     //*********************************************точное решение системы*****************************************
       public double SystemU(double t)
       {
           double y = 0;
         
           fi = 0;
           fi = Convert.ToDouble(txtb1.Text);
           v0 = 0;
           v0 = Convert.ToDouble(textBox4.Text);
           a = Convert.ToDouble(textBox3.Text);
           b = Convert.ToDouble(textBox5.Text);
           double Dis = Math.Pow(a / 2, 2) + b;
           if (Dis < 0)
           {
               double Re = a / 2;
               double Im = Math.Sqrt(Math.Abs(Dis));
               y = Math.Exp(t * Re) * (fi * Math.Cos(t * Im) + ((v0 - fi * Re) / Im * Math.Sin(t * Im)));
           }
           else
               if (Dis > 0)
               {
                   double lymda1 = a / 2 + Math.Sqrt(Dis);
                   double lymda2 = a / 2 - Math.Sqrt(Dis);
                   y = 1 / (lymda1 - lymda2) * ((v0 - lymda2 * fi) * Math.Exp(t * lymda1) - (v0 - lymda1 * fi) * Math.Exp(t * lymda2));
               }
               else
               {
                   y = Math.Exp(t * a / 2) * (fi + (v0 - fi * a / 2) * t);
               }
           return y;
       }
       public double SystemV(double t)
       {
           double y = 0;
          
           fi = 0;
           fi = Convert.ToDouble(txtb1.Text);
           v0 = 0;
           v0 = Convert.ToDouble(textBox4.Text);
           a = Convert.ToDouble(textBox3.Text);
           b = Convert.ToDouble(textBox5.Text);
           double Dis = Math.Pow(a / 2, 2) + b;
           if (Dis < 0)
           {
               double Re = a / 2;
               double Im = Math.Sqrt(Math.Abs(Dis));
               y = Math.Exp(t * Re) * (v0 * Math.Cos(t * Im) + ((v0 * Re - fi * Math.Pow(Math.Pow(Re, 2) + Math.Pow(Im, 2), 2)) / Im) * Math.Sin(t * Im));
           }
           else
               if (Dis > 0)
               {
                   double lymda1 = a / 2 + Math.Sqrt(Dis);
                   double lymda2 = a / 2 - Math.Sqrt(Dis);
                   y = 1 / (lymda1 - lymda2) * (lymda1 * (v0 - lymda2 * fi) * Math.Exp(t * lymda1) - lymda2 * (v0 - lymda1 * fi) * Math.Exp(t * lymda2));
               }
               else
               {
                   y = Math.Exp(t * a / 2) * (v0 + a / 2 * (v0 - fi * a / 2) * t);
               }
           return y;
       }
        //****************************отрисовка точного решения ****************************************
           private void DrawGraph()
        {
            tbErr1.Text = "";
            tbErr3.Text = "";
            GraphPane pane = zedGraphControl1.GraphPane;
            pane.CurveList.Clear();
            PointPairList list = new PointPairList();
            if (task.SelectedIndex != 0)
            {
                for (double t = 0.001; t <= Convert.ToInt32(T); t += 0.001)
                {

                    list.Add(t, Ut(t));
                }
            }
            else if (task.SelectedIndex == 0)
            {
                
                pane.XAxis.Title.Text = "Ось U";
            pane.XAxis.Title.FontSpec.FontColor = Color.Black;
            pane.YAxis.Title.Text = "Ось V";
            pane.YAxis.Title.FontSpec.FontColor = Color.Black;
                for (double t = 0.001; t <= Convert.ToInt32(T); t += 0.001)
                {

                    list.Add(SystemU(t), SystemV(t));
                }
            }
            
            LineItem fcurve = pane.AddCurve("Точное решение", list, Color.Red, SymbolType.None);
            fcurve.Line.Width = 2;
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }         

        //*****************************Погрешность********************************
        private double Error(double[] Um)
        {
            double[] t = new double[Convert.ToInt32(m + 1)];
            for (int i = 0; i < m; i++)
            {
                t[i] = i * tay;
            }
            double max1 = Math.Abs(Ut(t[0]) - U[0]);
            double m1;
            for (int i = 1; i < m; i++)
            {
                m1 = Math.Abs(Ut(t[i]) - U[i]);
                if (m1 > max1)
                    max1 = m1;
            }
            double max2 = Math.Abs(Ut(t[0]));
            for (int i = 0; i < m; i++)
            {
                m1 = Math.Abs(Ut(t[i]));
                if (m1 > max2)
                    max2 = m1;
            }
            return Convert.ToDouble(100 * (max1 / max2));
        }

        //*****************************для систем*******************
      private double Error1(double[,] S)
        {
            double[] t = new double[Convert.ToInt32(m + 1)];
            for (int i = 0; i < m; i++)
            {
                t[i] = i * tay;
            }
            double max1 = Math.Abs(SystemV(t[0]) - S[1, 0]);
            double m1;
            for (int i = 1; i < m; i++)
            {
                m1 = Math.Abs(SystemV(t[i]) - S[1, i]);
                if (m1 > max1)
                    max1 = m1;
            }
            double max2 = Math.Abs(SystemV(t[0]));
            for (int i = 0; i < m; i++)
            {
                m1 = Math.Abs(SystemV(t[i]));
                if (m1 > max2)
                    max2 = m1;
            }
            return Convert.ToDouble(100 * (max1 / max2));
        }
       
      

        //****************************точные решения**********************************
        private double Ut(double t)
        {
            double a = Convert.ToDouble(textBox3.Text);
            //задача#2 
            if (task.SelectedIndex == 0)
                return ((2.0 + (Math.Pow(t, 2) / (2.0 * E))) * Math.Exp(-(Math.Pow(t, 2)) / E));
            //задача#7 
            else if (task.SelectedIndex == 1)
                return ((Math.Pow((1.0) / ((2.0 /( Math.Pow(1.0 - t, 1.0 / E))) - 1.0), 1.0 / 3.0)));
            //задача#12.2
            else if (task.SelectedIndex == 2)
            {
                return (1.0 / (E - t - (10 + E) * Math.Exp(-t / E)));
            } 
            //задача#13.4
            else if (task.SelectedIndex == 3)
            return (a * (fi + a * Math.Tanh(t / E))) / (a + fi * Math.Tanh(t / E));
            else return 0;
          
        }
      
        private double F(double t, double u)
        {
            double a = Convert.ToDouble(textBox3.Text);
            //задача#2 
            if (task.SelectedIndex == 0)
                return ((t * Math.Exp(-Math.Pow(t, 2) / E)) - (2.0 * t * u)) / E;
            //задача#7 
            else if (task.SelectedIndex == 1)
                return (u + Math.Pow(u, 4)) / (3.0 * E * (t - 1.0));
            //задача#12.2
            else if (task.SelectedIndex == 2)
                return (u + t * Math.Pow(u, 2)) / E;
            //задача#13.4
            else if (task.SelectedIndex == 3)
                return (a - (Math.Pow(u, 2) / a)) / E;
            else return 0;
        }

        private void Draw()
        {
            if (checkBox1.Checked)
            {
                if (task.SelectedIndex == 0)
                    Method1system();
 
            }
            if (checkBox2.Checked)
            {
                if (task.SelectedIndex == 0)
                    Method3system();
                
            }
            if (checkBox3.Checked)
            {
                if (task.SelectedIndex == 0)
                    Method4system();
              
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DrawGraph();
            Draw(); 
        }

        private void task_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (task.SelectedIndex == 0)
            {
                pictureBox5.Visible = true;
                txtb1.Text = Convert.ToString(0);
                comboBox1.Text = Convert.ToString(1);
                textBox4.Visible = true;
                textBox4.Text = Convert.ToString(1);
                label9.Visible = true;
                label10.Visible = true;
                label11.Visible = true;
                textBox3.Text = Convert.ToString(0);
                textBox5.Text = Convert.ToString(-5);
                textBox3.Visible = true;
                textBox5.Visible = true;
                label1.Visible = false;
                textBox1.Visible = false;
            }
        }
        
        private void cmb3_SelectedIndexChanged(object sender, EventArgs e)
        {
            m = Convert.ToDouble(cmb3.SelectedItem);
            T = Convert.ToDouble(comboBox1.SelectedItem);
            tay = T / (m - 1);

        }

        private void cmb4_SelectedIndexChanged(object sender, EventArgs e)
        {
            E = Convert.ToDouble(cmb4.SelectedItem);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            T = Convert.ToDouble(comboBox1.SelectedItem);
            m = Convert.ToDouble(cmb3.SelectedItem);
            tay = T / (m - 1);
        }
        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
