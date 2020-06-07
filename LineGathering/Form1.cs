using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
namespace LineGathering
{
    public partial class Form1 : Form
    {

        //public Bitmap bmp;
        public int count, robotNo, epsilon = 10000;
        List<LineGathering.MobileRobot> m, bots;
        public List<Thread> threads, threa;
        public List<int> cycles;

        public Form1()
        {
            InitializeComponent();
            //  bmp=new Bitmap(pictureBox1.Width, pictureBox1.Height);
            //  pictureBox1.Image = bmp;
            m = new List<LineGathering.MobileRobot>();
            bots = new List<LineGathering.MobileRobot>();

            cycles = new List<int>();

            threads = new List<Thread>();
            threa = new List<Thread>();
            count = 0; //added
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            label1.Text = this.Width + " " + this.Height;
        }

        

        public void drawBMP()
        {
            Bitmap bmp = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                this.Text = "";
                foreach (LineGathering.MobileRobot n in m)
                {
                    g.FillEllipse(Brushes.Blue, n.X, n.Y, 15, 15); //red it was, 5
                    //this.Text +=  (n.X + " "+ n.Y+ " ").ToString();
                }
                pictureBox2.Image = bmp;
            }
        }

        public void drawplot()
        {
            Bitmap bmp = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            int height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height; // checking height
            //int width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;   // and width
            using (var g = Graphics.FromImage(bmp))
            {
                this.Text = "";
                int y = 0;
                foreach (LineGathering.MobileRobot n in m)
                {
                    y += 50;
                    g.FillEllipse(Brushes.Blue, n.X, y, 15, 15); //red it was, 5
                    //this.Text +=  (n.X + " "+ n.Y+ " ").ToString();
                }
                pictureBox2.Image = bmp;
            }
        }

        /*private bool Going() // periodically check(used before)
        {
            
            //bool checking = true;
            int dist = 0, d1 = 999, d2 = 999;
            int len = m.Count;
            for (int i = 0; i < len - 1; i++)
                dist += Math.Abs(m[i].Y - m[i + 1].Y);
                //dist += (m[i].Y - m[i + 1].Y) * (m[i].Y - m[i + 1].Y);

            
            if (dist < (epsilon * len))
                return false;

            return true;
        }*/

        private bool Going() // periodically check
        {

            //bool checking = true;
            int dist = 0, d1 = 0, d2 = 0, n1, n2, allowed = 550, dmin = 999, dmax = 0;
            int len = m.Count;

            if (len < 3) return false;

            n1 = Math.Abs(m[1].Y - m[2].Y);
            n2 = Math.Abs(m[1].Y - m[0].Y);

            dmin = Math.Min(n1, n2);
            dmax = Math.Max(n1, n2);

            if (Math.Abs(n1 - n2) > allowed)
                return true;
            

            for (int i = 2; i < len - 1; i++)
            {
                n1 = Math.Abs(m[i].Y - m[i+1].Y);
                n2 = Math.Abs(m[i].Y - m[i-1].Y);

                if (n1 < dmin) dmin = n1;
                else if (n1 > dmax)
                    dmax = n2;

                if (Math.Abs(n1 - n2) > allowed || (dmax - dmin) > allowed)
                return true;
            }

            if ((dmax - dmin) > allowed)
                return true;

            return false;
        }

        private void workTest(ref List<Thread> m)//(object threads)
        {
            List<Thread> th = (List<Thread>)threa;
            while (true)
            {
                Thread.Sleep(5000);
                if (!Going())
                {
                    string l = noOfcycles();
                    using (StreamWriter w = File.CreateText("d:\\TotalNoCycles.txt"))
                    {
                        w.WriteLine("Total number of cycles (LCM) : " + l);
                    }

                    timer2.Enabled = false;
                    foreach (Thread t in threa)
                        t.Abort();

                    break;
                }
            }

            lastWorktoDo();
        }


        public void lastWorktoDo()
        {
            string title = "Uniform Line formed.";
            string message = "total No of cycles(LCM) by Swarms : " + noOfcycles();
            /*NewMessageBox msgResized = new NewMessageBox(title, message);
            msgResized.StartPosition = FormStartPosition.CenterScreen;
            msgResized.Show();*/

            MessageBox.Show(message, title);

        }

        public void myBuiltFnc()
        {
            foreach (LineGathering.MobileRobot n in m)
            {
                threa.Add(new Thread(() => n.distributeUniformly(ref m, ref cycles))); // n.calculateNextPosition() distributeUniformly
            }

            //if (threa.Count == 0) System.Windows.Forms.Application.Exit();
            (new Thread(() => workTest(ref threa))).Start();
            Bitmap bmp = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            using (var g = Graphics.FromImage(bmp))
            {

                foreach (LineGathering.MobileRobot n in m)
                {
                    g.FillEllipse(Brushes.Red, n.X, n.Y, 15, 15);
                    //g.DrawEllipse(Pens.Blue, n.X, n.Y, 15, 15);
                }

                pictureBox2.Image = bmp;
            }

            foreach (Thread t in threa)
            {
                t.Start();
                //t.IsBackground = true;
            }

            timer3.Enabled = true;
        }

        private void myTest(ref List<Thread> m)//(object threads)
        {
            List<Thread> th = (List<Thread>)threads;
            while (true)
            {
                Thread.Sleep(5000);
                if (!isGoing())
                {
                    //MessageBox.Show("Hello");
                    //Thread.Sleep(200000);
                    string l = noOfcycles();
                    using (StreamWriter w = File.CreateText("d:\\TotalNoCycles.txt"))
                    {
                        w.WriteLine("Total number of cycles (LCM) : " + l);
                    }

                    /*var myForm = new Form1();
                    myForm.Show();*/
                    // drawplot();
                    ////MessageBox.Show("\n\nThe Line is formed.\n\n Total no of cycles are : " + l); // uncomment
                    //Thread.Sleep(2000);

                    /*try
                    {
                        foreach (Thread t in th)
                            t.Abort();
                    }
                    catch (ThreadAbortException)
                    {
                        Console.WriteLine("First");
                        //Try to swallow it.
                    }*/

                    //drawplot();
                    //System.Windows.Forms.Application.Exit();
                    timer2.Enabled = false; 
                    foreach (Thread t in threads)
                        t.Abort();

                    break;
                }
            }

            //drawplot();
            myBuiltFnc();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();

            int noOfRobots = int.Parse(textBox2.Text);
            robotNo = noOfRobots;
            for (int i = 0; i < noOfRobots; i++)
            {
                m.Add(new LineGathering.MobileRobot(rnd.Next(pictureBox2.Width-20), rnd.Next(pictureBox2.Height-10), i));
                cycles.Add(0);
            }
            using (StreamWriter sw = File.CreateText("D:\\LOGINITIALL.txt"))
            {
                foreach (MobileRobot r in m)
                    sw.WriteLine("Initial position of robot " + r.id + "is X: " + r.X + " Y: " + r.Y);
            }
            foreach (LineGathering.MobileRobot n in m)
            {
                threads.Add(new Thread(() => n.calculateNextPosition(ref m, ref cycles))); // n.calculateNextPosition()
            }

            (new Thread(() => myTest(ref threads))).Start(); //threads.Add(new Thread(() => myTest(ref m)));
            //threads.Add(new Thread(new ThreadStart(m[i].calculateNextPosition)));
           
            Bitmap bmp = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            using (var g = Graphics.FromImage(bmp))
            {

                foreach (LineGathering.MobileRobot n in m)
                {
                    g.DrawEllipse(Pens.Red , n.X, n.Y, 5, 5);

                }
                pictureBox2.Image = bmp;
            }
            
            foreach (Thread t in threads)
            {
                t.Start();
                //t.IsBackground = true;
            }

            timer3.Enabled = true;
           
        }

        private string noOfcycles()         //modular
        {
            count = 0;
            int l = cycles.Count;
            for (int i = 0; i < l; i++)
            {
                //w.WriteLine("Total number of cycles for Robot " + i + ": " + cycles[i]);
                count = count + cycles[i];
            }
            string scount = count.ToString();
            return scount;
        }

        private bool isGoing() // periodically check
        {
                bool checking = false;
                int xp = m[0].X; // checking the X co-ordinate 
                foreach (MobileRobot r in m)
                if (xp != r.X)
                {
                    checking = true;
                    break;
                }
                return checking;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int moving = 0;
            using (StreamWriter w = File.AppendText("d:\\LOG.txt"))
            {
                foreach (MobileRobot r in m)
                {
                    if (r.isMoving)
                        moving++;
                    w.WriteLine("Current Position of Robots " + r.id + ": X= " + r.X + " Y: " + r.Y);
                }
            }

            
            drawBMP();
        }

       
        private void button3_Click(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            List<MobileRobot> ordered = m.OrderBy(o => o.X).ToList();
            using (StreamWriter w = File.CreateText("d:\\LOG.txt"))
            {
                foreach (MobileRobot r in ordered)
                {
                    w.WriteLine("Current Position of Robot " + r.id + ": X= " + r.X + " Y: " + r.Y);
                }

            }

            using (StreamWriter w = File.CreateText("d:\\Cycles.txt"))
            {
                int l = cycles.Count;
                for(int i=0; i<l; i++)
                {
                    w.WriteLine("Total number of cycles for Robot " + i + ": " + cycles[i]);
                    //count = count + cycles[i];
                }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int noOfRobots = int.Parse(textBox2.Text);
            robotNo = noOfRobots;
            for (int i = 0; i < noOfRobots; i++)
            {
                m.Add(new LineGathering.MobileRobot(pictureBox2.Width - 20, rnd.Next(pictureBox2.Height - 100), i));
                cycles.Add(0);
            }
            using (StreamWriter sw = File.CreateText("D:\\LOGINITIALL.txt"))
            {
                foreach (MobileRobot r in m)
                    sw.WriteLine("Initial position of robot " + r.id + "is X: " + r.X + " Y: " + r.Y);
            }
            foreach (LineGathering.MobileRobot n in m)
            {
                threads.Add(new Thread(() => n.distributeUniformly(ref m, ref cycles))); // n.calculateNextPosition() distributeUniformly
            }

            Bitmap bmp = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            using (var g = Graphics.FromImage(bmp))
            {

                foreach (LineGathering.MobileRobot n in m)
                {
                    g.DrawEllipse(Pens.Red, n.X, n.Y, 5, 5);

                }
                pictureBox2.Image = bmp;
            }

            foreach (Thread t in threads)
            {
                t.Start();
                //t.IsBackground = true;
            }

            timer3.Enabled = true;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            drawBMP();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string title = "Total No of cycles(LCM) by Swarms : ";
            string message = " " + noOfcycles();
            NewMessageBox msgResized = new NewMessageBox(title, message);
            msgResized.StartPosition = FormStartPosition.CenterScreen;
            msgResized.Show();
        }
        


        private void button6_Click(object sender, EventArgs e)
        {
            bool checking = true;
            int xp = m[0].X;
            foreach (MobileRobot r in m)
            if (xp != r.X)
            {
                checking = false;
                break;
            }

            if (checking)
            {
                //MessageBox.Show("We Have formed a line");
                //MessageBox.Show(noOfcycles());
                //drawmap();
                drawplot();
                System.Windows.Forms.Application.Exit();
            }

            else
            {
                //MessageBox.Show("Sorry! Still on the way.");
                string title = "Status of the robots : ";
                string message = "Sorry! Still on the way.";
                NewMessageBox msgResized = new NewMessageBox(title, message);
                msgResized.StartPosition = FormStartPosition.CenterScreen;
                msgResized.Show();
            }
        }

       

        
        
    }



    public class NewMessageBox : Form
    {
        private TextBox textBoxMessage;
        private Button buttonOK;
        public static int totalRobot;


        public NewMessageBox(string title, string message)
        {
            InitializeComponent();
            this.Text = title;
            this.textBoxMessage.Text = message;
            this.Deactivate += MyDeactivateHandler;
            this.textBoxMessage.ReadOnly = true;
        }


        private void InitializeComponent()
        {
            this.buttonOK = new System.Windows.Forms.Button();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(171, 161);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(107, 44);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMessage.Location = new System.Drawing.Point(29, 36);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.ReadOnly = true;
            this.textBoxMessage.Size = new System.Drawing.Size(411, 38);
            this.textBoxMessage.TabIndex = 1;
            // 
            // NewMessageBox
            // 
            this.ClientSize = new System.Drawing.Size(468, 236);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.buttonOK);
            this.Name = "NewMessageBox";
            this.Load += new System.EventHandler(this.NewMessageBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        public string message { get; set; }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected void MyDeactivateHandler(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NewMessageBox_Load(object sender, EventArgs e)
        {

        }
    }   // ends here
}
