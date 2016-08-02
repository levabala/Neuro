using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace NeuroWFA
{
    public partial class Form1 : Form
    {
        int sizex = 3;
        int sizey = 5;
        int[,] input;
        Net WebN;
        bool Answered = false;
        string[] files;
        bool randomOn = false;
        Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            input = new int[sizex, sizey];

            LoadWeights();
        }

        public void LoadWeights()
        {
            WebN = new Net(sizex,sizey);

            StreamReader sr;
            try
            {
                sr = File.OpenText(Environment.CurrentDirectory + @"/Weights.txt");
            }
            catch (Exception e)
            {
                SaveWeights();
                sr = File.OpenText(Environment.CurrentDirectory + "/Weights.txt");
            }
            string line;
            int yIndex = 0;
            string[] s1;
            while ((line = sr.ReadLine()) != null)
            {
                s1 = line.Split(' ');
                for (int i = 0; i < s1.Length-1; i++)
                {
                    WebN.weights[i, yIndex] = Convert.ToInt16(s1[i]);                    
                }
                listBox1.Items.Add(line);
                yIndex++;
            }
            sr.Close();
        }

        public void SaveWeights()
        {
            string s = "";
            for (int y = 0; y < sizey; y++)
            {
                for (int x = 0; x < sizex; x++)
                {
                    s += WebN.weights[x, y].ToString();
                    if (x != sizex) s += " ";
                }                
                s += '\n';
            }
            File.WriteAllText(Environment.CurrentDirectory + @"\Weights.txt", s);
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            /*int x = this.PointToClient(new Point(e.X, e.Y)).X;
            int y = this.PointToClient(new Point(e.X, e.Y)).Y;

            if (x >= pictureBox1.Location.X && x <= pictureBox1.Location.X + pictureBox1.Width && y >= pictureBox1.Location.Y && y <= pictureBox1.Location.Y + pictureBox1.Height)
            {
                
            }*/            
            files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Length == 1)
            {            
                pictureBox1.Image = Image.FromFile(files[0]);
                GetInput();
                return;
            }            
            pictureBox1.Image = Image.FromFile(files[0]);
            GetInput();
            randomOn = true;
        }

        private void GetInput()
        {
            Bitmap im = pictureBox1.Image as Bitmap;
            for (var y = 0; y <= 4; y++)
                listBox1.Items[y] += "  ";
            for (var x = 0; x <= 2; x++)
            {                
                for (var y = 0; y <= 4; y++)
                {
                    int n = (im.GetPixel(x, y).R);
                    if (n >= 250) n = 0; // Определяем, закрашен ли пиксель
                    else n = 1;
                    listBox1.Items[y] = listBox1.Items[y] + " " + Convert.ToString(n) + " ";

                    input[x, y] = Convert.ToInt16(n); // Присваиваем соответствующее значение каждой ячейке входных данных
                }
            }

            WebN.setInput(input);
            WebN.MulAndSum();
            Text = WebN.Compare().ToString() + " (" + WebN.sum.ToString() + (WebN.sum > WebN.limit ? " > " : " <= ") + WebN.limit.ToString() + ")";

            Answered = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            randomOn = false;
            if (!Answered) return;
            Answered = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!Answered) return;

            WebN.Fail();

            listBox1.Items.Clear();

            SaveWeights();
            LoadWeights();            

            if (randomOn)            
                pictureBox1.Image = Image.FromFile(files[rnd.Next(0,files.Length-1)]);                            
            GetInput();

            Answered = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(files[rnd.Next(0, files.Length - 1)]);
            GetInput();
        }
    }
}
