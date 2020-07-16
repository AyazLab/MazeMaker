using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MazeMaker
{
    public partial class MazeListBuilder : Form
    {

        List<MyBuilderItem> myItems = new List<MyBuilderItem>();

        String curFilename = "";

        public MazeListBuilder()
        {
            InitializeComponent();
        }


        private void ReloadList()
        {
            listBox1.Items.Clear();
            int i = 1;
            foreach(MyBuilderItem a in myItems)
            {
                listBox1.Items.Add(i.ToString() + ") " + a.ToString());
                i++;
            }
            propertyGrid1.SelectedObject = null;
        }

        private void MazeListBuilder_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Maze File");
            comboBox1.Items.Add("Text Display");
            comboBox1.Items.Add("Image Display");
            comboBox1.Items.Add("Multiple-Choice Display");
            comboBox1.SelectedIndex = 0;
                       
        }

        private void toolStrip_open_Click(object sender, EventArgs e)
        {
            OpenFileDialog a = new OpenFileDialog();
            a.Filter = "Maze List files | *.xml";
            a.FilterIndex = 1;
            a.RestoreDirectory = true;
            if (a.ShowDialog() == DialogResult.OK)
            {
                ReadFromFileXml(a.FileName);
            } 
        }


        private void toolStrip_save_Click(object sender, EventArgs e)
        {
            if(curFilename=="")
            {
                DoSaveAs();
            }
            else
            {
                WriteToFileXml(curFilename);
            }
        }
        private void toolStrip_SaveAs_Click(object sender, EventArgs e)
        {

            DoSaveAs();
        }

        private void DoSaveAs()
        {
            SaveFileDialog a = new SaveFileDialog();
            a.Filter = "Maze List files | *.mel";
            a.FilterIndex = 1;
            a.RestoreDirectory = true;
            if (a.ShowDialog() == DialogResult.OK)
            {
                WriteToFile(a.FileName);
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            //listBox1.SelectedIndex = -1;
           
            //Add the Item
            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    //Maze
                     OpenFileDialog a = new OpenFileDialog();
                     a.Filter = "Maze files | *.maz";
                     a.FilterIndex = 1;
                     a.RestoreDirectory = true;
                     if (a.ShowDialog() == DialogResult.OK)
                     {
                            //listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            //listView1.Items[listView1.Items.Count - 1].SubItems.Add(a.FileName);
                         int i = a.FileName.LastIndexOf("\\");
                         //int count = listBox1.Items.Count;
                         myItems.Add(new MazeList_MazeItem(a.FileName.Substring(i + 1)));                           
                                              
                     } 
                    break;
                case 1:
                    myItems.Add(new MazeList_TextItem());  
                    //listBox1.Items.Add("TEXT MESSAGE");
                    break;
                case 2:
                    myItems.Add(new MazeList_ImageItem()); 
                    break;
                case 3:
                    myItems.Add(new MazeList_MultipleChoiceItem(new ListChangedEventHandler(Updated)));
                    break;
            }
            ReloadList();
        }

        private void L_Up_Click(object sender, EventArgs e)
        {
            //Object temp;
            //int cur = listBox1.SelectedIndex;
            //temp = listBox1.Items[cur-1];
            //listBox1.Items[cur - 1] = listBox1.Items[cur];
            //listBox1.Items[cur] = temp;
            //listBox1.SelectedIndex -= 1;
            MyBuilderItem temp;
            int cur = listBox1.SelectedIndex;
            temp = myItems[cur - 1];
            myItems[cur - 1] = myItems[cur];
            myItems[cur] = temp;
            ReloadList();
            listBox1.SelectedIndex = cur - 1;

        }

        private void L_Down_Click(object sender, EventArgs e)
        {
            //Object temp;
            //int cur = listBox1.SelectedIndex;
            //temp = listBox1.Items[cur + 1];
            //listBox1.Items[cur + 1] = listBox1.Items[cur];
            //listBox1.Items[cur] = temp;
            //listBox1.SelectedIndex += 1;
            MyBuilderItem temp;
            int cur = listBox1.SelectedIndex;
            temp = myItems[cur + 1];
            myItems[cur + 1] = myItems[cur];
            myItems[cur] = temp;
            ReloadList();
            listBox1.SelectedIndex = cur + 1;
        }

        private void L_Del_Click(object sender, EventArgs e)
        {
            //listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            myItems.RemoveAt(listBox1.SelectedIndex);
            ReloadList();
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            L_Del.Enabled = false;
            L_Up.Enabled = false;
            L_Down.Enabled = false;
            
            if (listBox1.SelectedIndex >= 0)
            {
                L_Del.Enabled = true;
                if (listBox1.SelectedIndex > 0)
                {
                    L_Up.Enabled = true;
                }
                if (listBox1.SelectedIndex < listBox1.Items.Count - 1)
                {
                    L_Down.Enabled = true;
                }
                propertyGrid1.SelectedObject = myItems[listBox1.SelectedIndex];
            }
            
 
        }

        private void listBox1_Leave(object sender, EventArgs e)
        {
            
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            int cur = listBox1.SelectedIndex;
            ReloadList();
            listBox1.SelectedIndex = cur;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool WriteToFileXml(string inp)
        {
            return false;
        }
        private bool  WriteToFile(string inp)
        {
            StreamWriter fp = new StreamWriter(inp);
            if (fp == null)
            {
                return false;
            }
            fp.WriteLine("Maze List File 1.2");

            foreach (MyBuilderItem a in myItems)
            {

                fp.Write(a.Type + "\t" );
                if(a.Type == ItemType.Maze)
                {
                    fp.Write(a.Value); 
                }
                else if (a.Type == ItemType.Text)
                {
                    MazeList_TextItem aa = (MazeList_TextItem)a;
                    fp.Write(aa.Value + "\t" + aa.TextDisplayType + "\t" + aa.LifeTime + "\t" + aa.X + "\t" + aa.Y + "\t ");
                }
                else if (a.Type == ItemType.Image)
                {
                    MazeList_ImageItem aa = (MazeList_ImageItem)a;
                    fp.Write(aa.Value + "\t" + aa.TextDisplayType + "\t" + aa.LifeTime + "\t" + aa.X + "\t" + aa.Y + "\t" + aa.Image);
                }
                else if(a.Type == ItemType.MultipleChoice)
                {
                    MazeList_MultipleChoiceItem aa = (MazeList_MultipleChoiceItem)a;
                    fp.Write(aa.GetString() + "\t" + aa.TextDisplayType + "\t" + aa.LifeTime + "\t" + aa.X + "\t" + aa.Y + "\t ");
                }
                fp.Write("\n");
            }

            fp.Close();

            return true;          
        }

        public void Updated(object e, ListChangedEventArgs c)
        {
            ReloadList();
        }

        private bool ReadFromFileXml(string inp)
        {
            return false;
        }

        public bool ReadFromFile(string inp)
        {
            StreamReader fp = new StreamReader(inp);
            if (fp == null)
            {
                return false;
            }
            myItems.Clear();
            String buf;
            buf = fp.ReadLine();
            if (!buf.Contains("Maze List File"))
            {
                MessageBox.Show("Not a Maze List File or corrupted!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

            curFilename = inp;
            toolStrip_Status.Text = inp;

            try
            {
                while (true)
                {
                    buf = fp.ReadLine();
                    //int tab1 = buf.IndexOf("\t");
                    //if (tab1 == -1)
                    //    break;
                    string[] parsed = buf.Split('\t');
                    if (parsed.Length == 0)
                        break;
                    if (parsed[0].CompareTo("Maze")==0)
                    {
                        //Maze Line
                        myItems.Add(new MazeList_MazeItem(parsed[1]));
                        
                    }
                    else if (parsed[0].CompareTo("Text") == 0)
                    {
                        //Text Line
                        MazeList_TextItem aa = new MazeList_TextItem();
                        aa.Value = parsed[1];
                        if(parsed[2].CompareTo(MazeList_TextItem.DisplayType.OnFramedDialog.ToString())==0 || parsed[2].CompareTo( "OnDialog") == 0 )
                        {
                            aa.TextDisplayType = MazeList_TextItem.DisplayType.OnFramedDialog;
                        }
                        else
                        {
                            aa.TextDisplayType = MazeList_TextItem.DisplayType.OnBackground;
                        }
                        aa.LifeTime = Int32.Parse(parsed[3]);
                        aa.X = Int32.Parse(parsed[4]);
                        aa.Y = Int32.Parse(parsed[5]);

                        if (parsed.Length > 6)
                        {
                            aa.BackgroundImage = parsed[6];
                        }
                        else
                        {
                            aa.BackgroundImage = "";
                        }
                        myItems.Add(aa);                       
                    }
                    else if (parsed[0].CompareTo("Image") == 0)
                    {
                        //Image Line
                        MazeList_ImageItem aa = new MazeList_ImageItem();
                        aa.Value = parsed[1];
                        if (parsed[2].CompareTo(MazeList_ImageItem.DisplayType.OnFramedDialog.ToString()) == 0 || parsed[2].CompareTo("OnDialog") == 0)
                        {
                            aa.TextDisplayType = MazeList_ImageItem.DisplayType.OnFramedDialog;
                        }
                        else
                        {
                            aa.TextDisplayType = MazeList_ImageItem.DisplayType.OnBackground;
                        }
                        aa.LifeTime = Int32.Parse(parsed[3]);
                        aa.X = Int32.Parse(parsed[4]);
                        aa.Y = Int32.Parse(parsed[5]);

                        if (parsed.Length > 6)
                        {
                            aa.Image = parsed[6];
                        }
                        else
                        {
                            aa.Image = "";
                        }
                        myItems.Add(aa);
                    }
                    else if (parsed[0].CompareTo("MultipleChoice") == 0)
                    {
                        //Text Line
                        MazeList_MultipleChoiceItem aa = new MazeList_MultipleChoiceItem(new ListChangedEventHandler(Updated));
                        aa.LoadString(parsed[1]);
                        if (parsed[2].CompareTo(MazeList_TextItem.DisplayType.OnFramedDialog.ToString()) == 0 || parsed[2].CompareTo("OnDialog") == 0)
                        {
                            aa.TextDisplayType = MazeList_MultipleChoiceItem.DisplayType.OnFramedDialog;
                        }
                        else
                        {
                            aa.TextDisplayType = MazeList_MultipleChoiceItem.DisplayType.OnBackground;
                        }
                        aa.LifeTime = Int32.Parse(parsed[3]);
                        aa.X = Int32.Parse(parsed[4]);
                        aa.Y = Int32.Parse(parsed[5]);

                        if (parsed.Length > 6)
                        {
                            aa.BackgroundImage = parsed[6];
                        }
                        else
                        {
                            aa.BackgroundImage = "";
                        }
                        myItems.Add(aa);
                    }
                }
            }
            catch
            {

            }
            fp.Close();
            ReloadList();
            return true;
        }

        private void MazeListBuilder_Resize(object sender, EventArgs e)
        {
            listBox1.Width = L_Up.Left - listBox1.Left - 10;
            listBox1.Height = closeButton.Top - listBox1.Top - 10;
            propertyGrid1.Height = listBox1.Height;
        }





    }
}