using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MazeMaker
{
    public partial class MazeGen : Form
    {
        public const int LeftWall = 0;
        public const int TopWall = 1;
        public MazeGen()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.No;
            this.tabControl1.SelectedIndex = 4; //Go to init page
            nextButton.Visible = false;
            previousButton.Visible = false;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            int i = tabControl1.SelectedIndex;
            if (i == 0)
            {
                tabControl1.SelectedIndex = i + 1;
                previousButton.Visible = true;
            }
            if (i == 1)
            {
                tabControl1.SelectedIndex = i + 1;
                previousButton.Visible = true;
                nextButton.Text = "Finish";
                generateMaze();
            }
            if (i == 2)
            {
                // create the maze
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            if(i==3)
            {
                generateCircularMaze();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

        }

        private autoMaze M;

        public List<Wall> GetWalls(double scale)
        {
            List<mazeWall> myList= M.maze2coords();
            List<Wall> newList = new List<Wall>();
            
            foreach (mazeWall w in myList)
            {
                newList.AddRange(w.ThickWallToWalls(scale,comboBoxColor.Text));
            }

            if(M.circularMaze)
            {
                List<mazeCurve> myListCurve = M.maze2coordsCurve();
                foreach (mazeCurve w in myListCurve)
                {

                    newList.AddRange(w.ThickWallToWalls(scale, comboBoxColor.Text));
                }
            }
            foreach (Wall w in newList)
            {
                w.Move(M.offset, M.offset);
            }

            return newList;
        }

        public List<CurvedWall> GetCurvedWalls(double scale)
        {
            List<mazeCurve> myList = M.maze2coordsCurve();
            List<CurvedWall> newList = new List<CurvedWall>();

            if(M.circularMaze)
            { 
                foreach (mazeCurve w in myList)
                {
                    newList.AddRange(w.ThickCurveToCurves(scale, comboBoxColor.Text));
                }
            }
            foreach (CurvedWall w in newList)
            {
                w.Move(M.offset, M.offset);
            }

            return newList;
        }

        public StartPos GetStartPos(double scale)
        {
            StartPos s = new StartPos(scale);
            MPoint p = new MPoint();
            p.X = 1 * (M.length + M.length* M.thickness) / 2; 
            p.Y = 0;
            p.Z = 1 * (M.length + M.length * M.thickness) / 2;

            if(M.circularMaze)
            {
                p.X = M.offset;
                p.Z = M.offset;
            }
            s.MzPoint = p;
            return s;
        }

        public List<Floor> GetFloors(double scale)
        {
            List<Floor> newList = new List<Floor>();
            Floor f = new Floor(scale);
            if (!M.circularMaze)
            { 
                f.MzPoint1.X = 0;
                f.MzPoint1.Y = -1;
                f.MzPoint1.Z = 0;

                f.MzPoint2.X = M.X * M.length*(M.thickness+1);
                f.MzPoint2.Y = -1;
                f.MzPoint2.Z = 0;

                f.MzPoint3.X = M.X * M.length * (M.thickness + 1);
                f.MzPoint3.Y = -1;
                f.MzPoint3.Z = M.Y * M.length * (M.thickness + 1);

                f.MzPoint4.X = 0;
                f.MzPoint4.Y = -1;
                f.MzPoint4.Z = M.Y * M.length * (M.thickness + 1);


                f.FloorVertex1.X = 0;
                f.FloorVertex1.Y = 0;

                f.FloorVertex2.X = M.X * M.length;
                f.FloorVertex2.Y = 0;

                f.FloorVertex3.X = 0;
                f.FloorVertex3.Y = M.Y * M.length;

                f.FloorVertex4.X = M.X * M.length;
                f.FloorVertex4.Y = M.Y * M.length;

                f.Ceiling = M.hasCeiling;

                f.CeilingVertex1.X = 0;
                f.CeilingVertex1.Y = 0;

                f.CeilingVertex2.X = M.X * M.length;
                f.CeilingVertex2.Y = 0;

                f.CeilingVertex3.X = 0;
                f.CeilingVertex3.Y = M.Y * M.length;

                f.CeilingVertex4.X = M.X * M.length;
                f.CeilingVertex4.Y = M.Y * M.length;

                f.UpdateAfterLoading();

                newList.Add(f);
            }
            else
            {
                f.MzPoint1.X = -M.rings * M.length * (M.thickness + 1);
                f.MzPoint1.Y = -1;
                f.MzPoint1.Z = -M.rings * M.length * (M.thickness + 1);

                f.MzPoint2.X = M.rings * M.length * (M.thickness + 1);
                f.MzPoint2.Y = -1;
                f.MzPoint2.Z = -M.rings * M.length * (M.thickness + 1);

                f.MzPoint3.X = M.rings * M.length * (M.thickness + 1);
                f.MzPoint3.Y = -1;
                f.MzPoint3.Z = M.rings * M.length * (M.thickness + 1);

                f.MzPoint4.X = -M.rings * M.length * (M.thickness + 1);
                f.MzPoint4.Y = -1;
                f.MzPoint4.Z = M.rings * M.length * (M.thickness + 1);


                f.FloorVertex1.X = 0;
                f.FloorVertex1.Y = 0;

                f.FloorVertex2.X = M.X * M.length;
                f.FloorVertex2.Y = 0;

                f.FloorVertex3.X = 0;
                f.FloorVertex3.Y = M.Y * M.length;

                f.FloorVertex4.X = M.X * M.length;
                f.FloorVertex4.Y = M.Y * M.length;

                f.Ceiling = M.hasCeiling;

                f.CeilingVertex1.X = 0;
                f.CeilingVertex1.Y = 0;

                f.CeilingVertex2.X = M.X * M.length;
                f.CeilingVertex2.Y = 0;

                f.CeilingVertex3.X = 0;
                f.CeilingVertex3.Y = M.Y * M.length;

                f.CeilingVertex4.X = M.X * M.length;
                f.CeilingVertex4.Y = M.Y * M.length;

                f.UpdateAfterLoading();

                newList.Add(f);
            }

            foreach (Floor w in newList)
            {
                w.Move(M.offset, M.offset);
            }

            return newList;
        }

        private void generateMaze()
        {
            int X = (int)heightNumeric.Value;
            int Y = (int)widthNumeric.Value;
            double thickness = (double)thickNumeric.Value/100;
            double length = (double)lengthNumeric.Value;
            int ceilingOpt = comboBoxCeiling.SelectedIndex;

        
             M = new autoMaze(X, Y, thickness, length, ceilingOpt);

            outMazeText.Text = M.ToString();
        }

        private void generateCircularMaze()
        {
            int ringFactor = (int)ringFactorUpDown.Value;
            int ringNum = (int)ringNumUpDown.Value;
            double thickness = (double)ringThicknessNumeric.Value / 100;
            double ringRadius = (double)ringRadiusNumeric.Value;
            int ceilingOpt = comboBoxCeiling.SelectedIndex;
            int corridorPref = (4-trackBar_corridorPreference.Value)*10+5;


            M = new autoMaze(ringNum,ringFactor,true,thickness,ringRadius, corridorPref);

   
            //outMazeText.Text = M.ToString();
        }


        private void previousButton_Click(object sender, EventArgs e)
        {
            int i = tabControl1.SelectedIndex;
            if (i == 0)
            {
                tabControl1.SelectedIndex = 4;
                previousButton.Visible = false;
                nextButton.Text = "Next";
                nextButton.Visible = false;
            }
            else if (i == 1)
            {
                //previousButton.Enabled = false;
                tabControl1.SelectedIndex = 0;
                nextButton.Text = "Next";
            }
            else if (i == 2)
            {
                tabControl1.SelectedIndex = i - 1;
                nextButton.Text = "Next";
            }
            else if (i==3)
            {
                tabControl1.SelectedIndex = 4;
                previousButton.Visible = false;
                nextButton.Visible = false;
            }
        }

        private void regenerateButton_Click(object sender, EventArgs e)
        {
            generateMaze();
        }

        class autoMaze
        {
            private List<wall> walls; // = new List<mazeCell>();
            
            private mazeCell[,] mazeArr;
            public readonly int X; //maze size
            public int Y; //maze size
            public double thickness; //wall size
            public double length; //size of each block
            public bool wallCeilings;
            public bool hasCeiling;
            public bool circularMaze = false;
            public int corridorPreference = 25;
            public readonly int rings = 0;
            public readonly int factor = 2;
            public float offset=0;
            


            public autoMaze(int numRings, int factor, bool circular, double thickness = 0.1, double length = 5,int corridorPreference=25)
            {
                this.circularMaze = true;
                this.rings = numRings;
                this.factor = factor;
                this.wallCeilings = false;
                this.hasCeiling = false;
                this.length = length;
                this.X = numRings;
                
                this.corridorPreference = corridorPreference;



                if (thickness < 0)
                    thickness = 0;
                if (thickness > 1)
                    thickness = 1;
                this.thickness = thickness;

                offset = (float)((numRings) * (1 + (float)thickness) * (float)length)+1.0f;

                walls = new List<wall>();
                GenerateCircular();
            }


            

            public autoMaze(int X = 20, int Y = 20, double thickness = 0.1, double length = 5, int wallOpt=-1)
            {
                this.circularMaze = false;
                this.X = X;
                this.Y = Y;
                this.rings = 0;
                this.factor = 0;
                this.length = length;
                offset = 0;

                if (thickness < 0)
                    thickness = 0;
                if (thickness > 1)
                    thickness = 1;

                this.thickness = thickness;

                if(wallOpt==0)
                {
                    this.wallCeilings = false;
                    this.hasCeiling = true;
                }
                else if (wallOpt == 1)
                {
                    this.wallCeilings = true;
                    this.hasCeiling = false;
                }
                else
                {
                    this.wallCeilings = false;
                    this.hasCeiling = false;
                }
                    
                
                walls = new List<wall>();
                Generate();
            }


            private mazeCell[,] InitializeArr()
            {
                mazeCell[,] arr = new mazeCell[X, Y];
                for (int y = 0; y < Y; y++)
                {
                    for (int x = 0; x < X; x++)
                    {
                            arr[x, y] = new mazeCell();   
                    }
                }
                return arr;
            }

            private void Generate()
            {
                mazeArr = InitializeArr(); //set up empty matrix

                int curX = 0; //define starting cell
                int curY = 0;

                markCell(curX, curY);

                Random rand = new Random();

                while (walls.Count > 0)
                {
                    int r = rand.Next() %walls.Count;
                    
                    wall selectedWall = walls[r];


                    curX = selectedWall.cellx;
                    curY = selectedWall.celly;

                    switch (selectedWall.walltype)
                    {
                        case 0:  //Left
                            if (!mazeArr[curX, curY].IsIn || !mazeArr[curX - 1, curY].IsIn) //is cell and cell to left marked
                            {
                                markCell(curX, curY);
                                markCell(curX - 1, curY);
                                mazeArr[curX, curY].leftWall = false;
                            }
                            break;
                        case 1:   //Top
                            if ((!mazeArr[curX, curY].IsIn || !mazeArr[curX, curY - 1].IsIn)) //is cell and cell above marked
                            {
                    
                                markCell(curX, curY);
                                
                                markCell(curX, curY -1);
                                mazeArr[curX, curY].topWall = false;
                            }
                            break;
                    }

                    walls.Remove(selectedWall);
                }
            }

            private int[] cellsInRings;
            private int maxInRing = 0;
            private int maxCells = 256; // maximum cells allowed
            private mazeCell[,] InitializeCircularArr()
            {
                cellsInRings = new int[rings];
                int lastB = 1;
                int tempFactor = factor;

                for (int i = 0; i < rings; i++)
                {
                    if (i == 0)
                        cellsInRings[i] = 1;
                    else
                    {
                        
                        int b =(int) Math.Floor((float)(i) / (Math.Pow((float)lastB,1f)))+1;
                        if (b > lastB)
                        {
                            lastB = b;
                            cellsInRings[i] = cellsInRings[i - 1]*tempFactor;
                            tempFactor = Math.Max(tempFactor / 4,2);
                        }
                        else
                            cellsInRings[i] = cellsInRings[i - 1];
                        
                    }
                    if (cellsInRings[i] > maxCells)
                        cellsInRings[i] = cellsInRings[i-1];
                    if (cellsInRings[i] > maxInRing)
                        maxInRing = cellsInRings[i];
                }


                mazeCell[,] arr = new mazeCell[rings, maxInRing];
                this.Y = maxInRing;
                for (int x = 0; x < rings; x++)
                {

                    for (int y = 0; y < maxInRing; y++)
                    {
                        if (y < (cellsInRings[x]))
                        {
                            Point parent=new Point(0,0);
                            if (x>0&&cellsInRings[x]==cellsInRings[x-1])
                                parent = new Point(x - 1, y);
                            else if(x>1)
                                parent = new Point(x - 1, (int)Math.Floor((float)y/factor));

                            arr[x, y] = new mazeCell(false,parent);
                        }
                        else
                            arr[x, y] = new mazeCell(true);
                    }
                }
                return arr;
            }

            private void GenerateCircular()
            {
                mazeArr = InitializeCircularArr(); //set up empty matrix

                int curX = 0; //define starting cell
                int curY = 0;

                markCell(curX, curY);

                Random rand = new Random();

                bool skipNextTop = false;
                


                while (walls.Count > 0)
                {
                    int r = rand.Next() % walls.Count;

                    wall selectedWall = walls[r];


                    curX = selectedWall.cellx;
                    curY = selectedWall.celly;

                    int r2 = rand.Next(1, 100);
                    if (r2 >= Math.Max(100/ (curX*factor),corridorPreference))
                    {
                        skipNextTop = true;
                    }
                    else
                        skipNextTop = false;

                    

                    //int cellsInRing = (int)Math.Pow(factor, curX);
                    Point left,parent;
                    parent = new Point(mazeArr[curX, curY].parentCell.X, mazeArr[curX, curY].parentCell.Y);
                    left = new Point(curX, ((curY+ cellsInRings[curX] - 1) % cellsInRings[curX]));

                    switch (selectedWall.walltype)
                    {
                        case 0:  //Left
                            if (!mazeArr[curX, curY].IsIn || !mazeArr[left.X, left.Y].IsIn) //is cell and cell to left marked
                            {
                                markCell(curX, curY);
                                markCell(left.X, left.Y);
                                mazeArr[curX, curY].leftWall = false;
                            }
                            break;
                        case 1:   //Top
                            if (!skipNextTop&&(!mazeArr[curX, curY].IsIn || !mazeArr[parent.X, parent.Y].IsIn)) //is cell and cell above marked
                            {

                                markCell(curX, curY);
                               
                                markCell(parent.X, parent.Y);
                                mazeArr[curX, curY].topWall = false;
                            }
                            break;
                    }

                    if(!skipNextTop|| selectedWall.walltype==0)
                        walls.Remove(selectedWall);
                }


            }

            private void markCell(int x, int y)
            {
                if (!mazeArr[x, y].IsIn && !mazeArr[x, y].invalid)
                {
                    mazeArr[x, y].IsIn = true;

                    if (this.circularMaze)
                    {
                        int cellsInRing = cellsInRings[x];
                        Point parent, left, right;
                        parent = mazeArr[x, y].parentCell;
                        //parent = new Point(x - 1, (int)Math.Floor((float)y / factor));
                        if (x > 0 && !mazeArr[parent.X, parent.Y].IsIn) //top wall (Y-1)
                            walls.Add(new wall(x, y, TopWall));

                        left = new Point(x, (y + cellsInRing - 1) % cellsInRing);
                        right = new Point(x, (y + cellsInRing + 1) % cellsInRing);
                        if (!mazeArr[left.X, left.Y].IsIn) //Left wall (Y-1)
                            walls.Add(new wall(x, y, LeftWall));
                        if (!mazeArr[right.X, right.Y].IsIn) //Right wall (Y-1)
                            walls.Add(new wall(right.X, right.Y, LeftWall));

                        if (x < rings - 1)
                        {

                            int altFactor = factor;
                            if (cellsInRings[x] == cellsInRings[x + 1])
                                altFactor = 1;
                            else
                                altFactor = (int)Math.Floor((double)cellsInRings[x + 1] / (double)cellsInRings[x]);

                            mazeArr[x, y].children = new Point[altFactor];
                            for (int i = 0; i < altFactor; i++)
                            {
                                mazeArr[x, y].children[i] = new Point(x + 1, y * altFactor + i);
                                if (!mazeArr[mazeArr[x, y].children[i].X, mazeArr[x, y].children[i].Y].IsIn) //Bottom wall (Y-1)
                                    walls.Add(new wall(mazeArr[x, y].children[i].X, mazeArr[x, y].children[i].Y, TopWall));
                            }
                        }
                    }
                    else {  //regular maze 
                        if (x > 0 && !mazeArr[x - 1, y].IsIn) //left wall  (x-1)
                            walls.Add(new wall(x, y, LeftWall));
                        if (y > 0 && !mazeArr[x, y - 1].IsIn) //top wall (Y-1)
                            walls.Add(new wall(x, y, TopWall));
                        if (x + 1 < X && !mazeArr[x + 1, y].IsIn)  //right wall (x+1)
                            walls.Add(new wall(x + 1, y, LeftWall));
                        if (y + 1 < Y && !mazeArr[x, y + 1].IsIn)  //bottom wall (Y+1
                            walls.Add(new wall(x, y + 1, TopWall));
                    }
                }
                else
                    mazeArr[x, y].IsIn = true;
            }

            public override String ToString()
            {
                String output = "";
                String output2 = "";

                String blockChar=" ";
                String emptyChar = "█";
                String borderChar = " ";

                for (int y = 0; y < Y; y++)
                {
                    for (int x = 0; x < X; x++)
                    {
                        output += "1\t";//"█";
                        output2 += borderChar;
                        if (mazeArr[x, y].topWall)
                        {
                            output += "1\t";
                            output2 += blockChar;
                        }
                        else
                        {
                            output += "0\t";
                            output2 += emptyChar;
                        }
                        
                    }
                    output += "\n";
                    output2 += blockChar+"\n";
                    for (int x = 0; x < X; x++)
                    {
                        if (mazeArr[x, y].leftWall)
                        {
                            output += "1\t";
                            output2 += borderChar;
                        }
                        else
                        {
                            output += "0\t";
                            output2 += emptyChar;
                        }
                        output += "0\t";
                        output2 += emptyChar;
                    }
                    output += "\n";
                    output2 += blockChar+"\n";
                }

                for (int x = 0; x < X*2+1; x++)
                {
                    output2 += blockChar;
                }

                return output2;
            } 

            public List<mazeWall> maze2coords()
            {
                List<mazeWall> wallList=new List<mazeWall>();
                double thickFactor=length*(1+thickness);

                int curX = X;
                int curY = Y;

                if (circularMaze)
                {
                    curX = rings;
                    curY=maxInRing;
                }

                for (int x = 0; x < curX; x++)
                {
                    if (circularMaze)
                    {
                        curY = cellsInRings[x];
                    }
                    for (int y=0; y< curY; y++)
                    {
                        if(mazeArr[x,y].leftWall)
                        { 
                            if(!circularMaze)
                                wallList.Add(new mazeWall(x*thickFactor,y*thickFactor,x*thickFactor,y*thickFactor+length,thickness,wallCeilings));
                            else if(!mazeArr[x, y].invalid&&x>0)
                            {
                                double angle=(double) y /(double) cellsInRings[x] * 360;
                                double r1 = x * length;
                                double r2 = (x + 1) * length;

                                bool hasBottom = false;
                                if (x < rings - 1 && mazeArr[x, y].children != null)
                                    hasBottom = mazeArr[mazeArr[x, y].children[0].X, mazeArr[x, y].children[0].Y].topWall;
                                else
                                    hasBottom = true;

                                wallList.Add(new mazeWall(r1,r2,angle, thickness, mazeArr[x, y].topWall, hasBottom));
                            }
                        }
                        if (mazeArr[x, y].topWall)
                        {
                            if(!circularMaze)
                                wallList.Add(new mazeWall(x * thickFactor, y * thickFactor, x * thickFactor + length, y * thickFactor, thickness, wallCeilings));

                        }

                        if(thickness!=0&&!circularMaze)
                            wallList.Add(new mazeWall(x * thickFactor, y * thickFactor, thickness, length, mazeArr[x, y].leftWall,mazeArr[x, y].topWall,wallCeilings));
                    }
                
                     
                }
                double factor2 = length * thickness/2;
                if(!circularMaze)
                { 
                    wallList.Add(new mazeWall(X * thickFactor - factor2, Y * thickFactor - factor2, X * thickFactor - factor2, 0, 0,false));
                    wallList.Add(new mazeWall(X * thickFactor - factor2, Y * thickFactor - factor2, 0, Y * thickFactor - factor2, 0,false));
                }

                return wallList;  
            }

            public List<mazeCurve> maze2coordsCurve()
            {
                List<mazeCurve> wallList = new List<mazeCurve>();
                double thickFactor = length * (1 + thickness);

                for (int x = 0; x < rings; x++)
                {
                    for (int y = 0; y < cellsInRings[x]; y++)
                    {
                        //if (mazeArr[x, y].leftWall)
                        //{
                        //    wallList.Add(new mazeWall(x * thickFactor, y * thickFactor, x * thickFactor, y * thickFactor + length, thickness, wallCeilings));
                        //}
                        if (mazeArr[x, y].topWall&&!mazeArr[x,y].invalid&&x!=0)
                        {
                            Point left = new Point(x, 0);
                            Point right=new Point(x,0);
                            if (y == 0)
                                left.Y = cellsInRings[x] - 1;
                            else
                                left.Y = y - 1;
                            if (y == cellsInRings[x]-1)
                                right.Y =0;
                            else
                                right.Y = y +1;

                            bool hasLeft = !mazeArr[left.X, left.Y].invalid && mazeArr[left.X, left.Y].topWall;
                            bool hasRight = !mazeArr[right.X, right.Y].invalid && mazeArr[right.X, right.Y].topWall;

                            wallList.Add(new mazeCurve((float)y/ (float)cellsInRings[x]*360, (float)(y+1) / (float)cellsInRings[x] * 360, x,length, thickness,hasLeft,hasRight));

                        }

                        
                    }


                }
                double factor2 = length * thickness / 2;
                wallList.Add(new mazeCurve(0, 360, rings-thickness, length));


                return wallList;
            }

            public String toMazeString()
            {
                String s = "";

                List<mazeWall> wallList = maze2coords();

                foreach (mazeWall w in wallList)
                {
                    s += w.ToString();
                }

                return s;
            }
        }

        


        struct coord
        {
            public double X;
            public double Y;
            public double Z;

            public coord (double X, double Y, double Z)
            {
                this.X = X;
                this.Y = Y;
                this.Z = Z;
            }

        }

        struct mazePlane
        {
            public coord[] coords;

            public mazePlane(double x1, double z1, double x2, double z2)
            {
                coords = new coord[4];
                coords[0] =new coord(x1, 1, z1);
                coords[1] = new coord(x1, -1, z1);
                coords[2] = new coord(x2, -1, z2);
                coords[3] = new coord(x2, 1, z2);
            }

            public mazePlane(double x1, double z1, double x2, double z2, double y) //For ceilings
            {
                coords = new coord[4];
                coords[0] = new coord(x1, y, z1);
                coords[1] = new coord(x1, y, z2);
                coords[2] = new coord(x2, y, z2);
                coords[3] = new coord(x2, y, z1);
            }

            public mazePlane(double angle, double r1, double r2) //For ceilings
            {
                coords = new coord[4];
                PointF xy;
                xy = Tools.RotatePoint(new PointF(1,0), new PointF(0, 0), angle);
                double x1, y1, x2, y2;
                x1 = xy.X*r1;
                y1 = xy.Y * r1;
                x2 = xy.X * r2;
                y2 = xy.Y * r2;

                coords[0] = new coord(x1, 1, y1);
                coords[1] = new coord(x1, -1, y1);
                coords[2] = new coord(x2, -1, y2);
                coords[3] = new coord(x2, 1, y2);
            }
        }

        struct mazeArc
        {
            public double angleStart;
            public double angleEnd;
            public double distFromRadius;

            public mazeArc(double angleStart, double angleEnd, double distFromRadius)
            {
                this.angleStart = angleStart;
                this.angleEnd = angleEnd;
                this.distFromRadius = distFromRadius;
            }

        }

        struct mazeWall
        {
            public mazePlane[] walls;

            public mazeWall(double x1, double z1, double x2, double z2, double thickness, bool wallCeilings)
            {
                if (thickness == 0)
                {
                    walls = new mazePlane[1];
                    walls[0] = new mazePlane(x1, z1, x2, z2);
                }
                else
                {
                    double length = Math.Abs(x1 - x2) + Math.Abs(z1 - z2);
                    bool vertical=Math.Abs(z1 - z2) > Math.Abs(x1 - x2);
                    double endLength = length * thickness/2;

                    if (wallCeilings)
                        walls = new mazePlane[3];
                    else
                        walls = new mazePlane[2];

                    if (vertical)
                    {
                        walls[0] = new mazePlane(x1 + endLength, z1 + endLength, x2 + endLength, z2 + endLength);
                        walls[1] = new mazePlane(x1 - endLength, z1 + endLength, x2 - endLength, z2 + endLength);
                    }
                    else
                    {
                        walls[0] = new mazePlane(x1 + endLength, z1 + endLength, x2 + endLength, z2 + endLength);
                        walls[1] = new mazePlane(x1 + endLength, z1 - endLength, x2 + endLength, z2 - endLength);
                    }

                    if(wallCeilings)
                        walls[2]= new mazePlane(x1 - endLength,z1 - endLength,x2 + endLength,z2 + endLength,1);


                }
            }
            public mazeWall(double x, double z, double thickness, double length,bool hasLeft,bool hasTop,bool wallCeilings) //Draw Corner Boxe
            {
                double l=length*thickness/2;
                if (thickness == 0)
                    walls = new mazePlane[0];
                else
                {
                    int wallcount = 2;
                    hasLeft = false;
                    if (!hasLeft)
                        wallcount++;
                    if (!hasTop)
                        wallcount++;
                    if (wallCeilings)
                        wallcount++;
                    walls = new mazePlane[wallcount];

                    wallcount = 2;
                    walls[0] = new mazePlane(x - l, z - l, x + l, z - l);//top
                    walls[1] = new mazePlane(x - l, z - l, x - l, z + l);//left


                    if (!hasLeft)
                    {
                        walls[wallcount] = new mazePlane(x - l, z + l, x + l, z + l); //bottom
                        wallcount++;
                    }
                                                                                                        
                                if (!hasTop)
                    {
                        walls[wallcount] = new mazePlane(x + l, z - l, x + l, z + l);  //right
                        wallcount++;
                    }
                    if (wallCeilings)
                    {
                        walls[wallcount] = new mazePlane(x - l, z - l, x + l, z+l, 1);
                        wallcount++;
                    }
                    
                    

                }

            }

            public mazeWall(double r1, double r2, double angle, double thickness, bool hasTop, bool hasBottom) //for circular
            {
          

                if (thickness <= 0)
                {
                    walls = new mazePlane[1];
                    walls[0] = new mazePlane(angle, r1, r2);
                }
                else
                {
                    double rlength = r2 - r1;
                    double wallWidth = thickness * rlength;
                    double angularDisp1 =wallWidth/ (r1 * 2 * Math.PI)*(360)*2;
                    double angularDisp2 = wallWidth / (r2 * 2 * Math.PI) * (360) * 2;

                    PointF xy = Tools.RotatePoint(new PointF(1, 0), new PointF(0, 0), angle);
                    PointF xy2_inner = Tools.RotatePoint(new PointF(1, 0), new PointF(0, 0), angle+angularDisp1);
                    PointF xy2_outer = Tools.RotatePoint(new PointF(1, 0), new PointF(0, 0), angle + angularDisp2);
                    double x1, z1, x2, z2;
                    double x3, z3, x4, z4;

                    if (hasTop)
                    {
                        x1 = xy.X * (r1 + wallWidth);
                        z1 = xy.Y * (r1 + wallWidth);
                    }
                    else
                    {
                        x1 = xy.X * (r1 - wallWidth);
                        z1 = xy.Y * (r1 - wallWidth);
                    }
                    if(!hasBottom)
                    {
                        x2 = xy.X * (r2 + wallWidth);
                        z2 = xy.Y * (r2 + wallWidth);
                    }
                    else
                    {
                        x2 = xy.X * (r2 - wallWidth);
                        z2 = xy.Y * (r2 - wallWidth);
                    }
                    

                    int numWalls = 2;
                    if (!hasBottom)
                        numWalls++;
                    if (!hasTop)
                        numWalls++;
                    
                     walls = new mazePlane[numWalls];

                    walls[0] = new mazePlane(x1, z1, x2, z2);



                    if (hasTop)
                    {
                        x3 = xy2_inner.X * (r1 + wallWidth);
                        z3 = xy2_inner.Y * (r1 + wallWidth);
                    }
                    else
                    {
                        x3 = xy2_inner.X * (r1 - wallWidth);
                        z3 = xy2_inner.Y * (r1 - wallWidth);
                    }
                    
                    if (!hasBottom)
                    {
                        x4 = xy2_outer.X * (r2 + wallWidth);
                        z4 = xy2_outer.Y * (r2 + wallWidth);
                    }
                    else
                    {
                        x4 = xy2_outer.X * (r2 - wallWidth);
                        z4 = xy2_outer.Y * (r2 - wallWidth);
                    }

                    walls[1] = new mazePlane(x3, z3, x4, z4);

                    int wallIndex = 1;
                    if (!hasTop)
                    {
                        wallIndex++;
                        walls[wallIndex] = new mazePlane(x1, z1, x3, z3);
                    }
                    if (!hasBottom)
                    {
                        wallIndex++;
                        walls[wallIndex] = new mazePlane(x2, z2, x4, z4);
                        
                    }


                }
            }

            public List<Wall> ThickWallToWalls(double scale, String color=null)
            {
                List<Wall> w = new List<Wall>();
                foreach (mazePlane wall in walls)
                {
                    if (wall.coords.Length > 1)
                    {
                        //s += "0\t6\n0\t1\t1\t1\n"; //first two lines
                        //s += "1\t1\t" + wall.coords[0].X + "\t" + wall.coords[0].Y + "\t" + wall.coords[0].Z + "\n";
                        //s += "1\t0\t" + wall.coords[1].X + "\t" + wall.coords[1].Y + "\t" + wall.coords[1].Z + "\n";
                        //s += "0\t0\t" + wall.coords[2].X + "\t" + wall.coords[2].Y + "\t" + wall.coords[2].Z + "\n";
                        //s += "0\t1\t" + wall.coords[3].X + "\t" + wall.coords[3].Y + "\t" + wall.coords[3].Z + "\n";
                        //s += "0\t0\t0\t0\t0\n"; // last line
                        Wall nw = new Wall(scale);
                        nw.MzPoint1.X = wall.coords[0].X;
                        nw.MzPoint1.Y = wall.coords[0].Y;
                        nw.MzPoint1.Z = wall.coords[0].Z;

                        nw.MzPoint2.X = wall.coords[1].X;
                        nw.MzPoint2.Y = wall.coords[1].Y;
                        nw.MzPoint2.Z = wall.coords[1].Z;

                        nw.MzPoint3.X = wall.coords[2].X;
                        nw.MzPoint3.Y = wall.coords[2].Y;
                        nw.MzPoint3.Z = wall.coords[2].Z;

                        nw.MzPoint4.X = wall.coords[3].X;
                        nw.MzPoint4.Y = wall.coords[3].Y;
                        nw.MzPoint4.Z = wall.coords[3].Z;

                        nw.UpdateAfterLoading();

                        if (string.IsNullOrWhiteSpace(color) == false)
                        {
                            nw.Color = Color.FromName(color);
                        }
                        
                        w.Add(nw);

                    }
                }

                return w;
            }



            //public override String ToString()
            //{
            //    String s = "";
            //    foreach (mazePlane wall in walls)
            //    {
            //        if (wall.coords.Length >1 )
            //        {
            //            s += "0\t6\n0\t1\t1\t1\n"; //first two lines
            //            s += "1\t1\t" + wall.coords[0].X + "\t" + wall.coords[0].Y + "\t" + wall.coords[0].Z + "\n";
            //            s += "1\t0\t" + wall.coords[1].X + "\t" + wall.coords[1].Y + "\t" + wall.coords[1].Z + "\n";
            //            s += "0\t0\t" + wall.coords[2].X + "\t" + wall.coords[2].Y + "\t" + wall.coords[2].Z + "\n";
            //            s += "0\t1\t" + wall.coords[3].X + "\t" + wall.coords[3].Y + "\t" + wall.coords[3].Z + "\n";
            //            s += "0\t0\t0\t0\t0\n"; // last line
            //        }
            //    }

            //    return s;
            //}
        }

        

        struct mazeCurve
        {
            public mazePlane[] walls;
            public mazeArc[] curves;

            public mazeCurve(double angleStart, double angleEnd, double ring, double ringSpacing, double thickness = 0,bool leftNeighbor=false,bool rightNeighbor=false)
            {
                if (thickness <= 0)
                {
                    walls = new mazePlane[0];
                    curves = new mazeArc[1];
                    curves[0] = new mazeArc(angleStart, angleEnd, ring * ringSpacing+thickness*ringSpacing);


                }
                else 
                {
                    curves = new mazeArc[2];
                    curves[0] = new mazeArc(angleStart, angleEnd, ring * ringSpacing + thickness * ringSpacing);
                    curves[1] = new mazeArc(angleStart, angleEnd, ring * ringSpacing - thickness * ringSpacing);

                    int numWalls = 0;
                    
                    if (leftNeighbor == false)
                        numWalls++;
                    if (rightNeighbor == false)
                        numWalls++;

                    walls = new mazePlane[numWalls];

                    int wallIndex = 0;

                    if(!leftNeighbor)
                    { 
                        walls[wallIndex] = new mazePlane(angleStart, ring * ringSpacing - thickness * ringSpacing, ring * ringSpacing+thickness*ringSpacing);
                        wallIndex++;
                    }
                    if(!rightNeighbor)
                    {
                        //int numwallsinring = (int)Math.Ceiling(360/(angleEnd - angleStart)); //-(float)8/(float)numwallsinring
                        walls[wallIndex] = new mazePlane(angleEnd, ring * ringSpacing - thickness * ringSpacing, ring * ringSpacing+ thickness * ringSpacing);/**/
                        wallIndex++;
                    }

                    //double length = Math.Abs(x1 - x2) + Math.Abs(z1 - z2);
                    //bool vertical = Math.Abs(z1 - z2) > Math.Abs(x1 - x2);
                    //double endLength = length * thickness / 2;
                }



            }
        

            public List<Wall> ThickWallToWalls(double scale, String color = null)
            {
                List<Wall> w = new List<Wall>();
                foreach (mazePlane wall in walls)
                {
                    if (wall.coords.Length > 1)
                    {
                        Wall nw = new Wall(scale);
                        nw.MzPoint1.X = wall.coords[0].X;
                        nw.MzPoint1.Y = wall.coords[0].Y;
                        nw.MzPoint1.Z = wall.coords[0].Z;

                        nw.MzPoint2.X = wall.coords[1].X;
                        nw.MzPoint2.Y = wall.coords[1].Y;
                        nw.MzPoint2.Z = wall.coords[1].Z;

                        nw.MzPoint3.X = wall.coords[2].X;
                        nw.MzPoint3.Y = wall.coords[2].Y;
                        nw.MzPoint3.Z = wall.coords[2].Z;

                        nw.MzPoint4.X = wall.coords[3].X;
                        nw.MzPoint4.Y = wall.coords[3].Y;
                        nw.MzPoint4.Z = wall.coords[3].Z;

                        nw.UpdateAfterLoading();

                        if (string.IsNullOrWhiteSpace(color) == false)
                        {
                            nw.Color = Color.FromName(color);
                        }

                        w.Add(nw);

                    }
                }

                return w;
            }

            public List<CurvedWall> ThickCurveToCurves(double scale, String color = null)
            {
                List<CurvedWall> w = new List<CurvedWall>();
                foreach (mazeArc wall in curves)
                {
                    if (wall.distFromRadius > 0)
                    {
                        CurvedWall nw = new CurvedWall(scale);


                        PointF center = new PointF(0, 0);
                        PointF mzPointBegin = new PointF((float)wall.distFromRadius, 0);
                        PointF mzPointEnd = new PointF((float)wall.distFromRadius, 0);
                        mzPointBegin = Tools.RotatePoint(mzPointBegin, center, wall.angleStart);
                        mzPointEnd = Tools.RotatePoint(mzPointEnd, center, wall.angleEnd);
                        nw.AngleBegin = wall.angleStart;
                        nw.AngleEnd = wall.angleEnd;

                        nw.MzPoint1.Y = 1;
                        nw.MzPoint1.SetPointF(mzPointBegin);

                        nw.MzPoint2.Y = -1;
                        nw.MzPoint2.SetPointF(mzPointBegin);


                        nw.MzPoint3.Y = -1;
                        nw.MzPoint3.SetPointF(mzPointEnd);


                        nw.MzPoint4.Y = 1;
                        nw.MzPoint4.SetPointF(mzPointEnd);


                        nw.CircleRadius = wall.distFromRadius;
                        nw.MzPointCenter.SetPointF(center);

                        nw.MzPoint1.Y = 1;
                        nw.MzPoint1.SetPointF(mzPointBegin);
            
                        nw.MzPoint2.Y = -1;
                        nw.MzPoint2.SetPointF(mzPointBegin);

                    
                        nw.MzPoint3.Y = -1;
                        nw.MzPoint3.SetPointF(mzPointEnd);

                     
                        nw.MzPoint4.Y = 1;
                        nw.MzPoint4.SetPointF(mzPointEnd);

                        

                        

                        

                        nw.UpdateAfterLoading();

                        if (string.IsNullOrWhiteSpace(color) == false)
                        {
                            nw.Color = Color.FromName(color);
                        }

                        w.Add(nw);

                    }
                }

                return w;
            }
        }


        //public override String ToString()
        //{
        //    String s = "";
        //    foreach (mazePlane wall in walls)
        //    {
        //        if (wall.coords.Length >1 )
        //        {
        //            s += "0\t6\n0\t1\t1\t1\n"; //first two lines
        //            s += "1\t1\t" + wall.coords[0].X + "\t" + wall.coords[0].Y + "\t" + wall.coords[0].Z + "\n";
        //            s += "1\t0\t" + wall.coords[1].X + "\t" + wall.coords[1].Y + "\t" + wall.coords[1].Z + "\n";
        //            s += "0\t0\t" + wall.coords[2].X + "\t" + wall.coords[2].Y + "\t" + wall.coords[2].Z + "\n";
        //            s += "0\t1\t" + wall.coords[3].X + "\t" + wall.coords[3].Y + "\t" + wall.coords[3].Z + "\n";
        //            s += "0\t0\t0\t0\t0\n"; // last line
        //        }
        //    }

        //    return s;
        //}


        private void outMazeText_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void MazeGen_Load(object sender, EventArgs e)
        {
            int whiteIndex = 0;
            int i = 0;
            foreach (System.Reflection.PropertyInfo prop in typeof(Color).GetProperties())
            {
                if (prop.PropertyType.FullName == "System.Drawing.Color")
                {                    
                    comboBoxColor.Items.Add(prop.Name);
                    comboBoxColorCirc.Items.Add(prop.Name);
                    if (prop.Name == "White")
                        whiteIndex = i;

                    i++;
                }
            }
            if(comboBoxColor.Items.Count>0)
            comboBoxColor.SelectedIndex = whiteIndex;

            if (comboBoxColorCirc.Items.Count > 0)
                comboBoxColorCirc.SelectedIndex = whiteIndex;

            comboBoxCeiling.Visible = true;
            comboBoxCeiling.Items.Add("Full Ceiling");
            comboBoxCeiling.Items.Add("Wall-only Ceilings");
            comboBoxCeiling.Items.Add("No Ceiling");
            comboBoxCeiling.SelectedIndex = 1;
        }

        private void MazeGen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
        }

        private void comboBoxCeiling_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void button_regMazeGen_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            nextButton.Visible = true;
            previousButton.Visible = true;
            nextButton.Text = "Next";
        }

        private void button_circMazeGen_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
            nextButton.Visible = true;
            previousButton.Visible = true;
            nextButton.Text = "Finish";
        }

        private void comboBoxColorCirc_SelectedIndexChanged(object sender, EventArgs e)
        {
            Color cl;
            comboBoxColor.SelectedIndex = comboBoxColorCirc.SelectedIndex;
            String color = comboBoxColor.Text;
            if (string.IsNullOrWhiteSpace(color) == false)
            {
                cl = Color.FromName(color);
                colorBoxLabelCirc.BackColor = cl;
                colorBoxLabelReg.BackColor = cl;
            }
            else
            {
                comboBoxColor.SelectedIndex = 0;
                comboBoxColorCirc.SelectedIndex = 0;
            }

        }

        private void comboBoxColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            Color cl;
            comboBoxColorCirc.SelectedIndex = comboBoxColor.SelectedIndex;
            String color = comboBoxColor.Text;
            if (string.IsNullOrWhiteSpace(color) == false)
            {
                cl = Color.FromName(color);
                colorBoxLabelCirc.BackColor = cl;
                colorBoxLabelReg.BackColor = cl;
            }
            else
            {
                comboBoxColor.SelectedIndex =0;
                comboBoxColorCirc.SelectedIndex =0;
            }

        }
    }

    public class mazeCell
    {
        public bool IsIn;
        public bool topWall;
        public bool leftWall;
        public bool invalid;
        public Point parentCell;
        public Point[] children;
        public int timesMarked;

        public mazeCell(bool invalidCell=false)
        {
            
            topWall = true;
            leftWall = true;
            invalid=invalidCell;
            IsIn = false||invalid;
            parentCell = new Point(0,0);
        }

        public mazeCell(bool invalidCell,Point parent)
        {
            IsIn = false;
            topWall = true;
            leftWall = true;
            invalid = invalidCell;
            parentCell = parent;
        }
    }



    struct wall
    {

        public int cellx;
        public int celly;
        public int walltype; //0 = Left 1=Top
        public wall(int x, int y, int type)
        {
            cellx = x;
            celly = y;
            walltype = type;
        }
    }

}
