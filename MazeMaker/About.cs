using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using MazeLib;

namespace MazeMaker
{   
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            linkLabel1.Links.Add(0, linkLabel1.Text.Length, "http://www.mazesuite.com");
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Completed);
        }

        WebClient webClient = new WebClient();

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void About_Load(object sender, EventArgs e)
        {             
            label6.Text += Application.ProductVersion.ToString();
            //timer1.Start();
            CheckNewVersion();
        }
        private void CheckNewVersion()
        {
            var versionChecker = new VersionChecker(Application.ProductName);
            versionChecker.CheckCompleted += VersionCheckCompleted;
            versionChecker.Check();
        }

        public void VersionCheckCompleted(object sender, MazeLib.VersionCheckerEventArgs e)
        {
            if (e.Error != null)
            {
                label5.Text = "Can not connect to server!";
                label5.ForeColor = Color.Red;
                return;
            }
            Version app = new Version(Application.ProductVersion);

            if (e.Version != null && e.Version > app)
            {
                label5.Text = "Found new version (" + e.Version.ToString() + ")";
                label5.ForeColor = Color.Red;
            }
            else
            {
                label5.Text = "No new version available!";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Process pr = new Process();
                pr.StartInfo.FileName = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\") + 1) + "MazeUpdate.exe";
                if (pr.Start())
                {
                    Application.Exit();
                }
            }
            catch //(System.Exception ex)
            {
                MessageBox.Show("Please quit " + Application.ProductName + " and run MazeUpdate from Start>All Programs","Update",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Determine which link was clicked within the LinkLabel.
            this.linkLabel1.Links[linkLabel1.Links.IndexOf(e.Link)].Visited = true;

            // Display the appropriate link based on the value of the 
            // LinkData property of the Link object.
            string target = e.Link.LinkData as string;
            

            // If the value looks like a URL, navigate to it.
            // Otherwise, display it in a message box.
            if (null != target && target.StartsWith("http"))
            {
                System.Diagnostics.Process.Start(target);
            }
            else
            {
                MessageBox.Show("Please use your browser to visit " + target);
            }
        }
        private void Check()
        {
            try
            {
                webClient.DownloadStringAsync(new Uri("http://www.mazesuite.com/files/update/updates.dat"));
            }
            catch (System.Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            
        }

        //private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        //{
        //    progressBar.Value = e.ProgressPercentage;
        //}

        private void Completed(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null || e.Cancelled == true)
                {
                    //Debug.WriteLine(e.Error);
                    label5.Text = "Can not check updates!";
                    //label5.ForeColor = Color.Red;
                    label5.Visible = true;
                    return;
                }
                //MessageBox.Show(((DownloadStringCompletedEventArgs)e).Result);
                label5.Text = "No update available!";
                string str = e.Result;
                string curName = Application.ProductName.ToLower();
                string[] parsed = str.Split(new char[] { '=', '\n'}, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < parsed.Length; i++)
                {
                    if (parsed[i].ToLower().Contains(curName))
                    {
                        //MessageBox.Show(parsed[i + 1]);
                        int[] curVer = GetNums(Application.ProductVersion);
                        int[] newVer = GetNums(parsed[i + 1]);

                        if (newVer != null && curVer != null)
                        {
                            int len = Math.Min(newVer.Length, curVer.Length);
                            for (int j = 0; j < len; j++)
                            {
                                if (newVer[j] > curVer[j])
                                {
                                    //found...
                                    label5.Text = "Found new version (" + parsed[i + 1].Trim() + ")";
                                    label5.ForeColor = Color.Red;
                                    label5.Visible = true;
                                    button2.Visible = true;
                                    break;
                                }
                                else if (curVer[j] > newVer[j])
                                {
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                label5.Text = "Can not check updates!";
            }
        }

        private int[] GetNums(string inp)
        {
            string[] parsed = inp.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (parsed.Length == 0)
                return null;
            int[] ret = new int[parsed.Length];
            for (int i = 0; i < parsed.Length; i++)
            {
                ret[i] = int.Parse(parsed[i]);
            }
            return ret;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            Check();
        }

    }
}