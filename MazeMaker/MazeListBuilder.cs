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
using System.Xml;

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
            a.Filter = "MazeList File (*.melx,*.mel)|*.melx;*.mel";
            a.FilterIndex = 1;
            a.RestoreDirectory = true;
            if (a.ShowDialog() == DialogResult.OK)
            {
                ReadFromFile(a.FileName);
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
                WriteToXml(curFilename);
            }
        }
        private void toolStrip_SaveAs_Click(object sender, EventArgs e)
        {

            DoSaveAs();
        }

        private void DoSaveAs()
        {
            SaveFileDialog a = new SaveFileDialog();
            // a.Filter = "Maze List files | *.mel";
            // a.Filter = "XML-File | *.xml";
            a.Filter = "MazeList XML-File|*.melx|Maze List files|*.mel";
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

        private bool WriteToXml(string inp)
        {
            XmlDocument xml = new XmlDocument();
            XmlElement mzLs = xml.CreateElement("MazeList");

            foreach (MyBuilderItem item in myItems)
            {
                XmlElement mz = xml.CreateElement(item.Type.ToString());

                if (item.Type == ItemType.Maze)
                {
                    mz.InnerText = item.Value;
                }
                else if (item.Type == ItemType.Text)
                {
                    MazeList_TextItem text = (MazeList_TextItem)item;
                    mz.InnerText = text.Value;
                    mz.SetAttribute("TextDisplayType", text.TextDisplayType.ToString());
                    mz.SetAttribute("LifeTime", text.LifeTime.ToString());
                    mz.SetAttribute("x", text.X.ToString());
                    mz.SetAttribute("y", text.Y.ToString());
                }
                else if (item.Type == ItemType.Image)
                {
                    MazeList_ImageItem image = (MazeList_ImageItem)item;
                    mz.InnerText = image.Value;
                    mz.SetAttribute("TextDisplayType", image.TextDisplayType.ToString());
                    mz.SetAttribute("LifeTime", image.LifeTime.ToString());
                    mz.SetAttribute("x", image.X.ToString());
                    mz.SetAttribute("y", image.Y.ToString());
                    mz.SetAttribute("image", image.Image.ToString());
                }
                else if (item.Type == ItemType.MultipleChoice)
                {
                    MazeList_MultipleChoiceItem mc = (MazeList_MultipleChoiceItem)item;
                    bool isQuestion = true;
                    foreach (string value in mc.Value)
                    {
                        if (isQuestion)
                        {
                            XmlElement question = xml.CreateElement("Question");
                            question.InnerText = value;
                            mz.AppendChild(question);
                            isQuestion = false;
                        }
                        else
                        {
                            XmlElement choice = xml.CreateElement("Choice");
                            choice.InnerText = value;
                            mz.AppendChild(choice);
                        }
                    }
                    mz.SetAttribute("TextDisplayType", mc.TextDisplayType.ToString());
                    mz.SetAttribute("LifeTime", mc.LifeTime.ToString());
                    mz.SetAttribute("x", mc.X.ToString());
                    mz.SetAttribute("y", mc.Y.ToString());
                }

                mzLs.AppendChild(mz);
            }

            xml.AppendChild(mzLs);
            xml.Save(inp);

            return true;
        }

        private bool WriteToFile(string inp)
        {
            string fileExt = Path.GetExtension(inp).ToLower();

            switch (fileExt)
            {
                case ".xml":
                    WriteToXml(inp);
                    break;

                case ".melx":
                    WriteToXml(inp);
                    break;

                case ".mel":
                    WriteToMel(inp);
                    break;

                default:
                    WriteToXml(inp);
                    break;
            }

            return true;
        }

        private bool WriteToMel(string inp)
        {
            StreamWriter fp = new StreamWriter(inp);
            if (fp == null)
            {
                return false;
            }
            fp.WriteLine("Maze List File 1.2");

            foreach (MyBuilderItem a in myItems)
            {

                fp.Write(a.Type + "\t");
                if (a.Type == ItemType.Maze)
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
                else if (a.Type == ItemType.MultipleChoice)
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

        private bool ReadFromXml(string inp)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(inp);
            XmlElement mzLs = xml.DocumentElement;

            foreach (XmlNode mz in mzLs)
            {
                switch (mz.Name)
                {
                    case "Maze":
                        myItems.Add(new MazeList_MazeItem(mz.InnerText));
                        break;

                    case "Text":
                        MazeList_TextItem text = new MazeList_TextItem();
                        text.Value = mz.InnerText;
                        text.TextDisplayType = (MazeList_TextItem.DisplayType)Enum.Parse(typeof(MazeList_TextItem.DisplayType), mz.Attributes["TextDisplayType"].Value);
                        text.LifeTime = Convert.ToInt64(mz.Attributes["LifeTime"].Value);
                        text.X = Convert.ToDouble(mz.Attributes["x"].Value);
                        text.Y = Convert.ToDouble(mz.Attributes["y"].Value);
                        myItems.Add(text);
                        break;

                    case "Image":
                        MazeList_ImageItem image = new MazeList_ImageItem();
                        image.Value = mz.InnerText;
                        image.TextDisplayType = (MazeList_ImageItem.DisplayType)Enum.Parse(typeof(MazeList_ImageItem.DisplayType), mz.Attributes["TextDisplayType"].Value);
                        image.LifeTime = Convert.ToInt64(mz.Attributes["LifeTime"].Value);
                        image.X = Convert.ToDouble(mz.Attributes["x"].Value);
                        image.Y = Convert.ToDouble(mz.Attributes["y"].Value);
                        image.Image = mz.Attributes["image"].Value;
                        myItems.Add(image);
                        break;

                    case "MultipleChoice":
                        MazeList_MultipleChoiceItem mc = new MazeList_MultipleChoiceItem();
                        mc.Value.Clear();
                        foreach (XmlNode node in mz.ChildNodes)
                        {
                            mc.Value.Add(node.InnerText);
                        }
                        mc.TextDisplayType = (MazeList_MultipleChoiceItem.DisplayType)Enum.Parse(typeof(MazeList_MultipleChoiceItem.DisplayType), mz.Attributes["TextDisplayType"].Value);
                        mc.LifeTime = Convert.ToInt64(mz.Attributes["LifeTime"].Value);
                        mc.X = Convert.ToDouble(mz.Attributes["x"].Value);
                        mc.Y = Convert.ToDouble(mz.Attributes["y"].Value);
                        myItems.Add(mc);
                        break;

                    default:
                        break;
                }
            }

            ReloadList();

            return true;
        }

        public bool ReadFromFile(string inp)
        {
            string fileExt = Path.GetExtension(inp).ToLower();

            switch (fileExt)
            {
                case ".xml":
                    return ReadFromXml(inp);

                case ".melx":
                    return ReadFromXml(inp);

                case ".mel":
                    return ReadFromMel(inp);

                default:
                    MessageBox.Show("Not mel or melx file!", "MazeMaker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
            }
        }

        public bool ReadFromMel(string inp)
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
                MessageBox.Show("Not a Maze List File or corrupted!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    if (parsed[0].CompareTo("Maze") == 0)
                    {
                        //Maze Line
                        myItems.Add(new MazeList_MazeItem(parsed[1]));

                    }
                    else if (parsed[0].CompareTo("Text") == 0)
                    {
                        //Text Line
                        MazeList_TextItem aa = new MazeList_TextItem();
                        aa.Value = parsed[1];
                        if (parsed[2].CompareTo(MazeList_TextItem.DisplayType.OnFramedDialog.ToString()) == 0 || parsed[2].CompareTo("OnDialog") == 0)
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