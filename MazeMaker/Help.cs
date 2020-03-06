using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MazeMaker
{
    public partial class Help : Form
    {

        //Stack<string> liste;
        List<string> liste = new List<string>();
        int currentPos = 0;
        

        public Help()
        {
            InitializeComponent();
        }

        private void Help_Load(object sender, EventArgs e)
        {
            webBrowser1.IsWebBrowserContextMenuEnabled = false;
            //webBrowser1.IsOffline = true;
            
            webBrowser1.DocumentText = MazeMaker.Properties.Resources.Home;
            liste.Add("Home");
            //currentPos++;
            PrintAll();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
           //webBrowser1.GoBack();           
            if (currentPos > 0)
            {
                currentPos--;
            }
            webBrowser1.DocumentText = MazeMaker.Properties.Resources.ResourceManager.GetString(liste[currentPos]);
            PrintAll();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //webBrowser1.GoForward();
            if (currentPos < liste.Count-1)
            {
                currentPos++;
            }
            webBrowser1.DocumentText = MazeMaker.Properties.Resources.ResourceManager.GetString(liste[currentPos]);
            PrintAll();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //home
            webBrowser1.DocumentText = MazeMaker.Properties.Resources.Home;
            if (liste[currentPos] != "Home")
            {
                //RemoveAfterCurrent();
                //currentPos++;                
                //liste.Add("Home");
                currentPos++;
                liste.Insert(currentPos, "Home");
                RemoveAfterCurrent(); 
            }
            PrintAll();
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            //webBrowser1.DocumentText.Insert(0, e.Url.ToString());
            String str = e.Url.ToString();
            if (str == "about:blank") return;
            //String togo = str.Substring(11, str.Length - 15);
            int startindex= str.IndexOf(':');
            if (startindex<0) startindex=-1;
            String togo = str.Substring(startindex + 1, str.IndexOf('.') - startindex - 1);
            webBrowser1.DocumentText = MazeMaker.Properties.Resources.ResourceManager.GetString(togo);
            if (liste[currentPos] != togo)
            {
                currentPos++;
                liste.Insert(currentPos,togo);
                RemoveAfterCurrent();                
            }
            PrintAll();
        }

        private void RemoveAfterCurrent()
        {
            try
            {
                for (int i = currentPos + 1; i <= liste.Count; i++)
                    liste.RemoveAt(currentPos + 1);
                //liste.RemoveRange(currentPos + 1, liste.Count - currentPos);
            }
            catch
            {
            }

        }
        private void PrintAll()
        {
            txtlabel.Text = currentPos + "|";
            for (int i = 0; i < liste.Count; i++)
                txtlabel.Text += ("-" + liste[i]);
        }

        private void txtlabel_Click(object sender, EventArgs e)
        {
            PrintAll();
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            //   MessageBox.Show(e.Url.AbsolutePath);            
            //webBrowser1.DocumentText = MazeMaker.Properties.Resources.ResourceManager.GetString(e.Url.AbsolutePath);
        }


    }
}