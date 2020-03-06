using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MazeMaker
{
    public partial class LogProcessor : Form
    {        
        bool preparing = false;
        int counter = 0;
        public LogProcessor()
        {
            InitializeComponent();
        }
        private void LogProcessor_Load(object sender, EventArgs e)
        {
            Reset();
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {            
            if(preparing)
            {
                if (MessageBox.Show("Do you want to quit without processing the log file", "Close?", MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            this.Close();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            //start
            if (!preparing || textBoxInput.Text == "" || textBoxOutput.Text == "")
                return;
            SetButtons(false);
            preparing = false;
            labelResult.Text = "Process started! Please wait...";
            timer1.Start();            
        }

        private void buttonInput_Click(object sender, EventArgs e)
        {
            OpenFileDialog a = new OpenFileDialog();
            a.Filter = "Text files (*.txt)| *.txt| Excel files (*.xls) | *.xls ";
            if(DialogResult.OK==a.ShowDialog())
            {
                textBoxInput.Text = a.FileName;
                preparing = true;
                counter = 0;
            }
        }

        private void buttonOutput_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog a = new FolderBrowserDialog();
            a.Description = "Please select a folder to save output files";
            if(DialogResult.OK == a.ShowDialog())
            {
                textBoxOutput.Text = a.SelectedPath;
                preparing = true;
            }
        }

        private void Reset()
        {
            textBoxInput.Text = "";
            textBoxOutput.Text = "";
            labelResult.Text = "No result";
            preparing = false;
            counter = 0;
        }

        private string GetShortFileName(string fname)
        {
           int startIndex = fname.LastIndexOf('\\');
           int lastIndex = fname.LastIndexOf('.');
           if (startIndex == -1 || lastIndex == -1) return " ";
           return fname.Substring(startIndex, lastIndex - startIndex);
        }

        private void Process()
        {            
            try
            {
                string fname =  GetShortFileName(textBoxInput.Text);
                string maze="";
                string buf="", line;
                StreamReader st = new StreamReader(textBoxInput.Text);
                bool started = false;
                StreamWriter elog = new StreamWriter(textBoxOutput.Text + fname + "_report.txt");
                StreamWriter elog2 = new StreamWriter(textBoxOutput.Text + fname + "_list.txt");
                elog.WriteLine("Maze Suite - LogProcessor Tool Report File\r\n");
                elog.WriteLine("Maze Suite - LogProcessor Tool Report List File\r\n");
                elog2.WriteLine("Index\tMaze Time\tPath Len\tMaze File");
                long curTime=0;
                bool mazeEnded = false;
                bool mazeTimeStarted = false;

                PointF startPoint = new PointF(0, 0);
                PointF endPoint = new PointF(0,0);
                double pathLen = 0;

                long mazeTime=0;
                long totalTime = 0;
                while(!st.EndOfStream)
                {
                    line = st.ReadLine();
                    if (line.Contains("Walker"))
                    {
                        if (buf.Length > 2)
                        {
                            if (mazeTimeStarted)
                            {
                                elog.WriteLine("Maze " + (counter).ToString());
                                elog.WriteLine("Start Time :\t" + mazeTime.ToString());
                                elog.WriteLine("End Time   :\t" + curTime.ToString());
                                elog.WriteLine("Time       :\t" + (curTime - mazeTime).ToString() + "\r\n\r\n");
                                totalTime += curTime - mazeTime;
                                mazeTimeStarted = false;
                                elog2.WriteLine((counter).ToString() + "\t" + (curTime - mazeTime).ToString() + "\t\t" + pathLen.ToString(".00;.00;0") + "\t\t" + maze);
                            }

                            StreamWriter a = new StreamWriter(textBoxOutput.Text + fname + "_" + (counter++).ToString() + "_" + maze + ".txt");
                            a.WriteLine("");
                            a.Write(buf);
                            a.Close();
                            buf = "";
                            maze = "";

                            pathLen = 0;
                            startPoint = new PointF(0, 0);
                            endPoint = startPoint;
                        }
                        started = true;
                        mazeEnded = true;
                    }
                    else if (line.Contains("Maze\t:"))
                    {
                        maze = GetShortFileName(line).Substring(1);
                    }
                    //else if (line.Contains("Time")) 
                    //{
                    //    //do nothing...
                    //}
                    else
                    {                        
                        try
                        {                            
                            //long temp = long.Parse(line.Substring(0, line.IndexOf('\t')));
                            string[] p = line.Split('\t');
                            if (p.Length > 0 )
                            {
                                long temp = long.Parse(p[0]);
                                if (temp != -1) curTime = temp;
                                mazeTimeStarted = true;

                                if (mazeEnded && mazeTimeStarted)
                                {
                                    mazeTime = curTime;
                                    mazeEnded = false;
                                    //if (p.Length == 5)
                                    //{
                                        startPoint.X = float.Parse(p[1]);
                                        startPoint.Y = float.Parse(p[2]);                                        
                                    //}
                                }
                                else
                                {
                                    startPoint = endPoint;
                                }
                                endPoint.X = float.Parse(p[1]);
                                endPoint.Y = float.Parse(p[2]);

                                pathLen += Math.Sqrt(Math.Pow(endPoint.X - startPoint.X, 2) + Math.Pow(endPoint.Y - startPoint.Y, 2));
                            }                            
                        }
                        catch
                        {
                            //mazeEnded = false;
                        }


                    }
                    if(started)
                    {
                        buf += line + "\r\n";
                    }
                }
                StreamWriter ar = new StreamWriter(textBoxOutput.Text + fname + "_" + (counter++).ToString() + "_" + maze + ".txt");
                ar.WriteLine("");
                ar.Write(buf);
                ar.Close();
                st.Close();

                if (mazeTimeStarted)
                {
                    elog.WriteLine("Maze " + (counter-1).ToString());
                    elog.WriteLine("Start Time :\t" + mazeTime.ToString());
                    elog.WriteLine("End Time   :\t" + curTime.ToString());
                    elog.WriteLine("Time       :\t" + (curTime - mazeTime).ToString() + "\r\n\r\n");
                    totalTime += curTime - mazeTime;
                    elog2.WriteLine((counter-1).ToString() + "\t" + (curTime - mazeTime).ToString() + "\t\t" + pathLen.ToString(".00;.00;0") + "\t\t" + maze);
                }
                elog.WriteLine("Total Maze Time :\t " + totalTime.ToString() + " ms");
                elog.WriteLine("\t\t\t(" + ((double)totalTime / 1000).ToString("#.#") + " sec)");
                elog.WriteLine("\t\t\t(" + ((double)totalTime / 60000).ToString("#.#") + " min)");
                elog.Close();
                elog2.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            Process();
            labelResult.Text = "Created " + counter + " files in the output directory";
            SetButtons(true);
        }


        void SetButtons(bool enable)
        {
            buttonStart.Enabled = enable;
            buttonInput.Enabled = enable;
            buttonOutput.Enabled = enable;
            buttonClose.Enabled = enable;
        }


    }
}