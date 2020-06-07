using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace LineGathering
{
    class MobileRobot
    {
        public int X, Y, id, dimension;
        public static int counts; // this is added
        public bool isMoving;
        Random r;
        private Object ThreadLock = new object();
        public MobileRobot(int a, int b, int i)
        {

            X = a;
            Y = b;
            id = i;
            dimension = 15;
            isMoving = false;
            r = new Random();
            counts = 0;
        }

        public MobileRobot(int a, int b)
        {
            X = a;
            Y = b;
        }

        public MobileRobot()
        {}

        public void calculateNextPosition(ref List<MobileRobot> robots, ref List<int> cycles) // now used
        {
            
            while (true)
            {
                Random rand = new Random();
                int XMin = 4000;
                int YMin=0;
                bool FLAG = false;
                int FLAGLinear = 0;
                int upper=0;
                int offset = 5;
                int Yset = 0;
                int GAP=0;
                int squareofdist = 201 * 201;
                //MobileRobot near=new MobileRobot();                
                List<MobileRobot> neighbours = new List<MobileRobot>();
                foreach (MobileRobot r in robots)
                {

                    int dx = r.X - this.X;
                    int dy = r.Y - this.Y;
                    //if (Math.Abs(r.X - this.X) > 200 || Math.Abs(r.Y - this.Y) > 200)
                    if (((dx * dx) + (dy * dy)) <= (3000 * 3000))
                    {
                        if(this.id != r.id)
                            neighbours.Add(r);
                    }
                    
                    //if (r.isMoving)
                      //  continue;
                    //if (this.id != r.id)
                    //{
                    //    if (r.X > this.X)
                    //    {

                    //        FLAG = true;
                    //        //if (((this.Y +20)=> r.Y)  || ( (this.Y -20) <= r.Y))
                    //        //if(((this.Y + 20) >= r.Y) || ((this.Y -20) <=r.Y))
                    //      /*  if((this.Y >= (r.Y -20)) && (this.Y <= (r.Y + 20)))
                    //        {
                    //            FLAGLinear += 1;
                    //            if (this.Y >= r.Y)
                    //            {
                    //                upper = 1;
                    //                GAP= this.Y - r.Y;
                    //            }
                    //            else
                    //            {
                    //                upper = 2;
                    //                GAP=r.Y - this.Y;
                    //            }
                    //        }*/
                    //        int dist = (dx * dx) + (dy * dy);
                    //        if ( dist < squareofdist)
                    //        {

                    //            squareofdist = dist;
                    //            XMin = r.X;
                    //            YMin = r.Y;
                    //            near = r;
                    //        }
                    //    }
                        
                    //  /*  if (r.Y > this.Y)
                    //    {
                    //        if (r.Y < YMinPositive)
                    //            YMinPositive = r.Y;
                    //    }
                    //    else
                    //    {
                    //        if (r.Y > YMinNegative)
                    //            YMinNegative = r.Y;
                    //    }  */
                    //}

                   }

                List<MobileRobot> ahead = new List<MobileRobot>();
                List<MobileRobot> sameaxis = new List<MobileRobot>();
                List<MobileRobot> behind = new List<MobileRobot>();

                foreach (MobileRobot r in neighbours)
                {
                    if (r.X > this.X)
                        ahead.Add(r);
                    else
                    {
                        if (r.X < this.X)
                            behind.Add(r);
                        else
                            sameaxis.Add(r);
                    }
                }

                if(ahead.Count>0)
                {
                    if (behind.Count == 0)
                    FLAG = true;

                    else
                    FLAG=false;
                }
                else
                FLAG=false;
                
                bool intersectionempty = true, YPositiveEmpty = true, YNegativeEmpty = true;

                if (FLAG == true) // when needed to move
                {
                    int Xnear = 999; // near X axis. {system using global knowledge guessing}
                    List<MobileRobot> nearAxis = new List<MobileRobot>();
                    foreach (MobileRobot r in ahead)
                    {
                        if ((r.X - this.X) <= Xnear)
                        {
                            Xnear = r.X - this.X;
                            nearAxis.Add(r);
                        }
                    }

                    foreach (MobileRobot r in nearAxis)
                    {
                        if (this.Y == r.Y)
                            intersectionempty = false;  // at same height
                        if (this.Y > r.Y)
                            YNegativeEmpty = false;
                        if (this.Y < r.Y)
                            YPositiveEmpty = false;
                    }

                    if (intersectionempty)
                    {
                        this.X += Xnear;
                    }
                    else
                    {
                        if (YPositiveEmpty)
                        {
                            this.X += Xnear;
                            this.Y += 10;
                        }
                        else
                        {
                            if (YNegativeEmpty)
                            {
                                this.X += Xnear;
                                this.Y -= 10;
                            }
                            else
                                this.Y += 10;
                        }
                    }
                    
                }
            //    if (FLAG)
            //    {
            //        if ((this.Y >= (near.Y - 10)) && (this.Y <= (near.Y + 10)))
            //        {
            //            FLAGLinear = 1;
            //            if (this.Y >= near.Y)
            //            {
            //                upper = 1;
            //             //   GAP = this.Y - near.Y;
            //            }
            //            else
            //            {
            //                upper = 2;
            //              //  GAP = r.Y - this.Y;
            //            }
            //        }
                     
            //        int x = XMin - this.X;
            //        if (FLAGLinear > 0)
            //        {
            //            offset = (int)(((2 * Math.Pow(x, 2)) - Math.Pow(2 * dimension, 2)) / (2 * x));

            //            //if (offset >= 10)
            //              //  offset = 10;
            //            if (upper == 1)
            //                this.Y += (int)(Math.Pow(2 * dimension, 2) - (Math.Pow(2 * dimension, 4) / (4 * Math.Pow(x, 2))));
            //            if (upper == 2)
            //                this.Y -= (int)(Math.Pow(2 * dimension, 2) - (Math.Pow(2 * dimension, 4) / (4 * Math.Pow(x, 2))));


            //        }
            //        else
            //        {
            //            //offset = (x >= 10) ? 10 : x;
            //            offset = x;
            //        }
            //        //int Yset = (YMinPositive + YMinNegative) / 2;
            //        //int Yset = (YMin - this.Y) / 4;
            //       // if (offset >= 10)
            //       // {
            //            //Yset = (Yset / offset) * 5;
            //         //   offset = 10;

            //       // }
            //        Monitor.Enter(ThreadLock);
            //        try

            //        {
            //            move(offset, Yset, ref near);
            //            //robots.ElementAt(this.id).X += offset;
            //        }
            //        finally
            //        {
            //            Monitor.Exit(ThreadLock);
            //        }
            //    }



                cycles[id]++;
                counts++; // added number of moves
                Thread.Sleep(rand.Next(500,5000));
            }
        }


        public void distributeUniformly(ref List<MobileRobot> robots, ref List<int> cycles) // has to use or implement(modified)
        {

            Random rand = new Random();
            MobileRobot left = new MobileRobot(this.X, -999);
            MobileRobot right = new MobileRobot(this.X, 999);

            while (true)
            {
                left.Y = -9999;
                right.Y = 9999;
                foreach (MobileRobot r in robots)
                {
                    if (r.id != this.id)
                    {
                        if (r.Y > this.Y && r.Y < right.Y)
                            right.Y = r.Y;

                        if (r.Y < this.Y && r.Y > left.Y)
                            left.Y = r.Y;
                    }
                }

                int midY;
                if (left.Y == -9999)
                    midY = this.Y;
                else
                {
                    if (right.Y == 9999)
                        midY = this.Y;
                    else
                        midY = Math.Abs((left.Y + right.Y) / 2);
                }
                if (midY != this.Y)
                    this.Y = midY;

                cycles[id]++;
                counts++; // added number of moves
                Thread.Sleep(rand.Next(500, 5000));
                //Thread.Sleep(1000);
            }
        }

        public void move(int offset, int Yset, ref MobileRobot bots)
        {
            this.isMoving = true;
            for (int i = 1; i <= offset; i++)
            {
                this.X += 1;
                Thread.Sleep(100);
            }
            //  this.Y += Yset;
            this.isMoving = false;
        }
    }
}


/////// Intermediates ////////
/*public void distributeUniformly(ref List<MobileRobot> robots, ref List<int> cycles)
        {
            Random rand = new Random();
            MobileRobot left = new MobileRobot(this.X, 999);
            MobileRobot right = new MobileRobot(this.X, -999);

            while (true)
            {
                left.Y = 9999;
                right.Y = -9999;
                foreach (MobileRobot r in robots)
                {
                    if (r.id != this.id)
                    {
                        if (r.Y > this.Y && r.Y < right.Y)
                            left.Y = r.Y;
                        if (r.Y < this.Y && r.Y > left.Y)
                            right.Y = r.Y;
                    }
                }
                int midY;
                if (right.Y == -9999)
                    midY = this.Y;
                else
                {
                    if (left.Y == 9999)
                        midY = this.Y;
                    else
                        midY = Math.Abs((left.Y + right.Y) / 2);
                    Console.WriteLine("hatt bara");
                }
                if (midY != this.Y)
                    this.Y = midY;

                cycles[id]++;
                counts++; // added number of moves
                Thread.Sleep(rand.Next(500, 5000));
                //Thread.Sleep(1000);
            }
        }*/

/*public void distributeUniformly(ref List<MobileRobot> robots, ref List<int> cycles)
{
    Random rand = new Random();
    MobileRobot left = new MobileRobot(-999, this.Y);
    MobileRobot right = new MobileRobot(999, this.Y);

    while (true)
    {
        left.X = -9999;
        right.X = 9999;
        foreach (MobileRobot r in robots)
        {
            if (r.id != this.id)
            {
                if (r.X > this.X && r.X < right.X)
                    right.X = r.X;
                if (r.X < this.X && r.X > left.X)
                    left.X = r.X;
            }
        }
        int midX;
        if (left.X == -9999)
            midX = this.X;
        else
        {
            if (right.X == 9999)
                midX = this.X;
            else
                midX = Math.Abs((left.X + right.X) / 2);
        }
        if (midX != this.X)
            this.X = midX;

        cycles[id]++;
        counts++; // added number of moves
        Thread.Sleep(rand.Next(500, 5000));
        //Thread.Sleep(1000);
    }
}*/
