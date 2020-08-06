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
using System.Linq;

namespace MazeMaker
{
    public partial class MazeListBuilder : Form
    {
        public static List<MyBuilderItem> myItems = new List<MyBuilderItem>();

        string curFilename = "";

        public static bool madeChanges = false;

        public MazeListBuilder()
        {
            InitializeComponent();

            treeViewMazeList.HideSelection = false;
        }

        private void ReloadList()
        {
            treeViewMazeList.BeginUpdate();
            treeViewMazeList.Nodes.Clear();

            int i = 1;
            foreach(MyBuilderItem a in myItems)
            {
                treeViewMazeList.Nodes.Add(i.ToString() + ") " + a.ToString());
                i++;
            }

            //propertyGrid1.SelectedObject = null;
            treeViewMazeList.EndUpdate();
        }

        private void MazeListBuilder_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Maze Options");
            comboBox1.Items.Add("Maze File");
            comboBox1.Items.Add("Text Display");
            comboBox1.Items.Add("Image Display");
            comboBox1.Items.Add("Multiple-Choice Display");
            comboBox1.SelectedIndex = 0;
        }

        private void toolStrip_open_Click(object sender, EventArgs e)
        {
            if (UnsavedChangesCheck() != DialogResult.Cancel)
            {
                OpenFileDialog a = new OpenFileDialog();
                a.Filter = "MazeList File (*.melx,*.mel)|*.melx;*.mel|All Files|*.*";
                a.FilterIndex = 1;
                a.RestoreDirectory = true;

                if (a.ShowDialog() == DialogResult.OK)
                {
                    ReadFromFile(a.FileName, false);
                }

                madeChanges = false;
            }
        }

        private void toolStripButton_Append_Click(object sender, EventArgs e)
        {
            madeChanges = true;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MazeList File (*.melx,*.mel)|*.melx;*.mel|All Files|*.*";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ReadFromFile(ofd.FileName, true);
            }
        }

        private void toolStripButton_New_Click(object sender, EventArgs e)
        {
            if (UnsavedChangesCheck() != DialogResult.Cancel)
            {
                ClearListMessage();
                ReloadList();
            }
        }

        private DialogResult UnsavedChangesCheck()
        {
            if (madeChanges)
            {
                DialogResult dr = MessageBox.Show("Do you want to save changes to this file?", "MazeList Editor", MessageBoxButtons.YesNoCancel);
                switch (dr)
                {
                    case DialogResult.Yes:
                        if (DoSaveAs())
                        {
                            return DialogResult.Yes;
                        }
                        else
                        {
                            return DialogResult.Cancel;
                        }

                    case DialogResult.No:
                        madeChanges = false;
                        return dr;

                    case DialogResult.Cancel:
                        return dr;

                    default:
                        return dr;
                }
            }
            else
            {
                return DialogResult.No;
            }
        }

        void ClearListMessage()
        {
            if (UnsavedChangesCheck() != DialogResult.Cancel)
            {
                myItems.Clear();
            }
        }

        private void toolStrip_save_Click(object sender, EventArgs e)
        {
            if (curFilename=="")
            {
                DoSaveAs();
            }
            else
            {
                WriteToFile(curFilename);
            }
        }

        private void toolStrip_SaveAs_Click(object sender, EventArgs e)
        {
            DoSaveAs();
        }

        private bool DoSaveAs()
        {
            SaveFileDialog a = new SaveFileDialog();
            a.Filter = "MazeList XML-File|*.melx|Maze List files|*.mel";
            a.FilterIndex = 1;
            a.RestoreDirectory = true;

            if (a.ShowDialog() == DialogResult.OK)
            {
                madeChanges = false;
                WriteToFile(a.FileName);
                return true;
            }

            return false;
        }

        private void add_Click(object sender, EventArgs e)
        {
            madeChanges = true;
            //listBox1.SelectedIndex = -1;

            //Add the Item
            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    myItems.Add(new MazeList_MazeOptionsItem());
                    break;

                case 1:
                    //MazeMakerCollectionEditor a = new MazeMakerCollectionEditor(ref curMaze.cImages);
                    //a.ShowDialog();
                    
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

                case 2:
                    myItems.Add(new MazeList_TextItem());  
                    //listBox1.Items.Add("TEXT MESSAGE");
                    break;

                case 3:
                    myItems.Add(new MazeList_ImageItem());
                    break;

                case 4:
                    myItems.Add(new MazeList_MultipleChoiceItem(new ListChangedEventHandler(Updated)));
                    break;
            }
            ReloadList();
        }

        private void L_Up_Click(object sender, EventArgs e)
        {
            madeChanges = true;

            int selectedIndex = treeViewMazeList.Nodes.IndexOf(treeViewMazeList.SelectedNode);
            MyBuilderItem temp = myItems[selectedIndex - 1];
            myItems[selectedIndex - 1] = myItems[selectedIndex];
            myItems[selectedIndex] = temp;
            ReloadList();
            treeViewMazeList.SelectedNode = treeViewMazeList.Nodes[selectedIndex - 1];
        }

        private void L_Down_Click(object sender, EventArgs e)
        {
            madeChanges = true;

            int selectedIndex = treeViewMazeList.Nodes.IndexOf(treeViewMazeList.SelectedNode);
            MyBuilderItem temp = myItems[selectedIndex + 1];
            myItems[selectedIndex + 1] = myItems[selectedIndex];
            myItems[selectedIndex] = temp;
            ReloadList();
            treeViewMazeList.SelectedNode = treeViewMazeList.Nodes[selectedIndex + 1];
        }

        private void L_Del_Click(object sender, EventArgs e)
        {
            madeChanges = true;
            myItems.RemoveAt(treeViewMazeList.Nodes.IndexOf(treeViewMazeList.SelectedNode));
            ReloadList();

            L_Del.Enabled = false;
            L_Up.Enabled = false;
            L_Down.Enabled = false;
        }

        private void treeViewMazeList_AfterSelect(object sender, EventArgs e)
        {
            L_Del.Enabled = false;
            L_Up.Enabled = false;
            L_Down.Enabled = false;

            int selectedIndex = treeViewMazeList.Nodes.IndexOf(treeViewMazeList.SelectedNode);
            if (selectedIndex >= 0)
            {
                L_Del.Enabled = true;
                if (selectedIndex > 0)
                {
                    L_Up.Enabled = true;
                }
                if (selectedIndex < treeViewMazeList.Nodes.Count - 1)
                {
                    L_Down.Enabled = true;
                }
                propertyGrid1.SelectedObject = myItems[selectedIndex];
            }
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            madeChanges = true;
            //int cur = listBox1.SelectedIndex;
            ReloadList();       
            //listBox1.SelectedIndex = cur;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            if (UnsavedChangesCheck() != DialogResult.Cancel)
            {
                ClearListMessage();
                Close();
            }
        }

        private void MazeListBuilder_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UnsavedChangesCheck() != DialogResult.Cancel)
            {
                ClearListMessage();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private bool WriteToMelx(string inp)
        {
            XmlDocument melx = new XmlDocument();
            XmlElement mzLs = melx.CreateElement("MazeList");
            mzLs.SetAttribute("Version", "1");

            XmlElement mazeLibrary = melx.CreateElement("MazeLibrary");
            XmlElement imageLibrary = melx.CreateElement("ImageLibrary");
            XmlElement audioLibrary = melx.CreateElement("AudioLibrary");

            Dictionary<string, string> mazeLibraryItems = new Dictionary<string, string>();
            Dictionary<string, string> imageLibraryItems = new Dictionary<string, string>();
            Dictionary<string, string> audioLibraryItems = new Dictionary<string, string>();

            int mazeIDCounter = 100;
            int imageIDCounter = 100;
            int audioIDCounter = 100;

            XmlElement listItems = melx.CreateElement("ListItems");

            foreach (MyBuilderItem item in myItems)
            {
                XmlElement mz = melx.CreateElement(item.Type.ToString());

                switch (item.Type)
                {
                    case ItemType.MazeOptions:
                        MazeList_MazeOptionsItem mazeOptions = (MazeList_MazeOptionsItem)item;

                        if (mazeOptions.FullScreen.ToString() != "")
                        {
                            XmlElement fullScreen = melx.CreateElement("FullScreen");
                            fullScreen.InnerText = mazeOptions.FullScreen.ToString();
                            mz.AppendChild(fullScreen);
                        }

                        if (mazeOptions.ComPort.ToString() != "")
                        {
                            XmlElement comPort = melx.CreateElement("COM_Port");
                            comPort.SetAttribute("Enabled", mazeOptions.ComPort.ToString());
                            mz.AppendChild(comPort);
                        }

                        if (mazeOptions.Lsl.ToString() != "")
                        {
                            XmlElement lsl = melx.CreateElement("LSL");
                            lsl.SetAttribute("Enabled", mazeOptions.Lsl.ToString());
                            mz.AppendChild(lsl);
                        }

                        if (mazeOptions.Lpt.ToString() != "")
                        {
                            XmlElement lpt = melx.CreateElement("LPT");
                            lpt.SetAttribute("Enabled", mazeOptions.Lpt.ToString());
                            mz.AppendChild(lpt);
                        }

                        if (mazeOptions.FontSize != "")
                        {
                            XmlElement font = melx.CreateElement("Font");
                            font.SetAttribute("Size", mazeOptions.FontSize);
                            mz.AppendChild(font);
                        }
                        break;

                    case ItemType.Maze:
                        MazeList_MazeItem maze = (MazeList_MazeItem)item;

                        XmlElement mazeLibraryItem = melx.CreateElement("Maze");
                        int mazeID = mazeIDCounter;
                        if (mazeLibraryItems.ContainsKey(maze.Value))
                        {
                            mazeID = Convert.ToInt32(mazeLibraryItems[maze.Value]);
                        }
                        else
                        {
                            mazeLibraryItems[maze.Value] = mazeID.ToString();
                            mazeIDCounter++;

                            mazeLibraryItem.SetAttribute("ID", mazeID.ToString());
                            mazeLibraryItem.SetAttribute("File", maze.Value);

                            mazeLibrary.AppendChild(mazeLibraryItem);
                        }
                        mz.SetAttribute("ID", mazeID.ToString());
                        if (maze.Value == "")
                        {
                            mz.SetAttribute("ID", "");
                        }

                        mz.SetAttribute("DefaultStartPosition", maze.DefaultStartPosition);
                        mz.SetAttribute("StartMessage", maze.StartMessage);
                        mz.SetAttribute("Timeout", maze.Timeout);
                        break;

                    case ItemType.Text:
                        MazeList_TextItem text = (MazeList_TextItem)item;
                        mz.InnerText = text.Value;
                        mz.SetAttribute("TextDisplayType", text.TextDisplayType.ToString());
                        mz.SetAttribute("LifeTime", text.LifeTime.ToString());
                        mz.SetAttribute("X", text.X.ToString());
                        mz.SetAttribute("Y", text.Y.ToString());

                        XmlElement audioLibraryItem = melx.CreateElement("Audio");
                        int audioID = audioIDCounter;
                        if (audioLibraryItems.ContainsKey(text.Audio)) // duplicate audio, reuse ID
                        {
                            audioID = Convert.ToInt32(audioLibraryItems[text.Audio]);
                        }
                        else if (text.Audio != "") // audio not duplicate and audio not empty
                        {
                            audioLibraryItems[text.Audio] = audioID.ToString();
                            audioIDCounter++;

                            audioLibraryItem.SetAttribute("ID", audioID.ToString());
                            audioLibraryItem.SetAttribute("File", text.Audio);

                            audioLibrary.AppendChild(audioLibraryItem);
                        }
                        mz.SetAttribute("Audio", audioID.ToString());
                        if (text.Audio == "") // audio empty
                        {
                            mz.SetAttribute("Audio", "");
                        }

                        mz.SetAttribute("AudioOnUnhighlight", text.AudioOnUnhighlight);
                        mz.SetAttribute("FontSize", text.FontSize);
                        break;

                    case ItemType.Image:
                        MazeList_ImageItem image = (MazeList_ImageItem)item;
                        mz.InnerText = image.Value;
                        mz.SetAttribute("TextDisplayType", image.TextDisplayType.ToString());
                        mz.SetAttribute("LifeTime", image.LifeTime.ToString());
                        mz.SetAttribute("X", image.X.ToString());
                        mz.SetAttribute("Y", image.Y.ToString());
                        mz.SetAttribute("BackgroundColor", image.BackgroundColor);

                        XmlElement imageLibraryItem = melx.CreateElement("Image");
                        int imageID = imageIDCounter;
                        if (imageLibraryItems.ContainsKey(image.Image.ToString()))
                        {
                            imageID = Convert.ToInt32(imageLibraryItems[image.Image.ToString()]);
                        }
                        else if (image.Image.ToString() != "")
                        {
                            imageLibraryItems[image.Image.ToString()] = imageID.ToString();
                            imageIDCounter++;

                            imageLibraryItem.SetAttribute("ID", imageID.ToString());
                            imageLibraryItem.SetAttribute("File", image.Image.ToString());

                            imageLibrary.AppendChild(imageLibraryItem);
                        }
                        mz.SetAttribute("Image", imageID.ToString());
                        if (image.Image.ToString() == "")
                        {
                            mz.SetAttribute("Image", "");
                        }

                        audioLibraryItem = melx.CreateElement("Audio");
                        audioID = audioIDCounter;
                        if (audioLibraryItems.ContainsKey(image.Audio))
                        {
                            audioID = Convert.ToInt32(audioLibraryItems[image.Audio]);
                        }
                        else if (image.Audio != "")
                        {
                            audioLibraryItems[image.Audio] = audioID.ToString();
                            audioIDCounter++;

                            audioLibraryItem.SetAttribute("ID", audioID.ToString());
                            audioLibraryItem.SetAttribute("File", image.Audio);

                            audioLibrary.AppendChild(audioLibraryItem);
                        }
                        mz.SetAttribute("Audio", audioID.ToString());
                        if (image.Audio == "")
                        {
                            mz.SetAttribute("Audio", "");
                        }

                        mz.SetAttribute("AudioOnUnhighlight", image.AudioOnUnhighlight);
                        break;

                    case ItemType.MultipleChoice:
                        MazeList_MultipleChoiceItem multipleChoice = (MazeList_MultipleChoiceItem)item;
                        bool isQuestion = true;
                        foreach (ValueRet vr in multipleChoice.Value)
                        {
                            if (isQuestion)
                            {
                                XmlElement question = melx.CreateElement("Question");
                                question.InnerText = vr.Value;
                                if (vr.Ret != "")
                                {
                                    question.SetAttribute("Ret", vr.Ret);
                                }
                                mz.AppendChild(question);
                                isQuestion = false;
                            }
                            else
                            {
                                XmlElement choice = melx.CreateElement("Choice");
                                choice.InnerText = vr.Value;
                                choice.SetAttribute("Ret", vr.Ret);
                                mz.AppendChild(choice);
                            }
                        }
                        mz.SetAttribute("TextDisplayType", multipleChoice.TextDisplayType.ToString());
                        mz.SetAttribute("LifeTime", multipleChoice.LifeTime.ToString());
                        mz.SetAttribute("X", multipleChoice.X.ToString());
                        mz.SetAttribute("Y", multipleChoice.Y.ToString());

                        audioLibraryItem = melx.CreateElement("Audio");
                        audioID = audioIDCounter;
                        if (audioLibraryItems.ContainsKey(multipleChoice.Audio))
                        {
                            audioID = Convert.ToInt32(audioLibraryItems[multipleChoice.Audio]);
                        }
                        else if (multipleChoice.Audio != "")
                        {
                            audioLibraryItems[multipleChoice.Audio] = audioID.ToString();
                            audioIDCounter++;

                            audioLibraryItem.SetAttribute("ID", audioID.ToString());
                            audioLibraryItem.SetAttribute("File", multipleChoice.Audio);

                            audioLibrary.AppendChild(audioLibraryItem);
                        }
                        mz.SetAttribute("Audio", audioID.ToString());
                        if (multipleChoice.Audio == "")
                        {
                            mz.SetAttribute("Audio", "");
                        }

                        mz.SetAttribute("AudioOnUnhighlight", multipleChoice.AudioOnUnhighlight);
                        break;

                    default:
                        break;
                }

                listItems.AppendChild(mz);
            }

            mzLs.AppendChild(mazeLibrary);
            mzLs.AppendChild(imageLibrary);
            mzLs.AppendChild(audioLibrary);
            
            mzLs.AppendChild(listItems);

            melx.AppendChild(mzLs);
            melx.Save(inp);

            return true;
        }

        private bool WriteToFile(string inp)
        {
            string fileExt = Path.GetExtension(inp).ToLower();

            switch (fileExt)
            {
                case ".xml":
                    WriteToMelx(inp);
                    break;

                case ".melx":
                    WriteToMelx(inp);
                    break;

                case ".mel":
                    WriteToMel(inp);
                    break;

                default:
                    WriteToMelx(inp);
                    break;
            }

            madeChanges = false;
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

        private bool ReadFromMelx(string inp)
        {
            XmlDocument melx = new XmlDocument();
            melx.Load(inp);
            XmlElement mzLs = melx.DocumentElement;

            Dictionary<string, string> mazeLibraryItems = new Dictionary<string, string>();
            Dictionary<string, string> imageLibraryItems = new Dictionary<string, string>();
            Dictionary<string, string> audioLibraryItems = new Dictionary<string, string>();

            foreach (XmlElement mz in mzLs)
            {
                switch (mz.Name)
                {
                    case "MazeLibrary":
                        foreach (XmlElement mazeLibraryItem in mz.ChildNodes)
                        {
                            mazeLibraryItems[mazeLibraryItem.GetAttribute("ID")] = mazeLibraryItem.GetAttribute("File");
                        }
                        break;

                    case "ImageLibrary":
                        foreach (XmlElement imageLibraryItem in mz.ChildNodes)
                        {
                            imageLibraryItems[imageLibraryItem.GetAttribute("ID")] = imageLibraryItem.GetAttribute("File");
                        }
                        break;

                    case "AudioLibrary":
                        foreach (XmlElement audioLibraryItem in mz.ChildNodes)
                        {
                            audioLibraryItems[audioLibraryItem.GetAttribute("ID")] = audioLibraryItem.GetAttribute("File");
                        }
                        break;

                    case "ListItems":
                        foreach (XmlElement listItem in mz.ChildNodes)
                        {
                            switch (listItem.Name)
                            {
                                case "MazeOptions":
                                    MazeList_MazeOptionsItem mazeOptions = new MazeList_MazeOptionsItem();

                                    foreach (XmlElement mazeOption in listItem.ChildNodes)
                                    {
                                        switch (mazeOption.Name)
                                        {
                                            case "FullScreen":
                                                mazeOptions.FullScreen = bool.Parse(mazeOption.InnerText);
                                                break;

                                            case "COM_Port":
                                                mazeOptions.ComPort = bool.Parse(mazeOption.GetAttribute("Enabled"));
                                                break;

                                            case "LSL":
                                                mazeOptions.Lsl = bool.Parse(mazeOption.GetAttribute("Enabled"));
                                                break;

                                            case "LPT":
                                                mazeOptions.Lpt = bool.Parse(mazeOption.GetAttribute("Enabled"));
                                                break;

                                            case "Font":
                                                mazeOptions.FontSize = mazeOption.GetAttribute("Size");
                                                break;

                                            default:
                                                break;
                                        }
                                    }

                                    myItems.Add(mazeOptions);
                                    break;

                                case "Maze":
                                    string file = listItem.GetAttribute("ID");
                                    if (mazeLibraryItems.ContainsKey(file))
                                    {
                                        file = mazeLibraryItems[file];
                                    }
                                    MazeList_MazeItem maze = new MazeList_MazeItem(file);

                                    maze.DefaultStartPosition = listItem.GetAttribute("DefaultStartPosition");
                                    maze.StartMessage = listItem.GetAttribute("StartMessage");
                                    maze.Timeout = listItem.GetAttribute("Timeout");
                                    myItems.Add(maze);
                                    break;

                                case "Text":
                                    MazeList_TextItem text = new MazeList_TextItem();
                                    text.Value = listItem.InnerText;
                                    text.TextDisplayType = (MazeList_TextItem.DisplayType)Enum.Parse(typeof(MazeList_TextItem.DisplayType), listItem.GetAttribute("TextDisplayType"));
                                    text.LifeTime = Convert.ToInt64(listItem.GetAttribute("LifeTime"));
                                    text.X = Convert.ToDouble(listItem.GetAttribute("X"));
                                    text.Y = Convert.ToDouble(listItem.GetAttribute("Y"));

                                    file = listItem.GetAttribute("Audio");
                                    if (audioLibraryItems.ContainsKey(file))
                                    {
                                        file = audioLibraryItems[file];
                                    }
                                    text.Audio = file;

                                    text.AudioOnUnhighlight = listItem.GetAttribute("AudioOnUnhighlight");
                                    text.FontSize = listItem.GetAttribute("FontSize");
                                    myItems.Add(text);
                                    break;

                                case "Image":
                                    MazeList_ImageItem image = new MazeList_ImageItem();
                                    image.Value = listItem.InnerText;
                                    image.TextDisplayType = (MazeList_ImageItem.DisplayType)Enum.Parse(typeof(MazeList_ImageItem.DisplayType), listItem.GetAttribute("TextDisplayType"));
                                    image.LifeTime = Convert.ToInt64(listItem.GetAttribute("LifeTime"));
                                    image.X = Convert.ToDouble(listItem.GetAttribute("X"));
                                    image.Y = Convert.ToDouble(listItem.GetAttribute("Y"));
                                    image.BackgroundColor = listItem.GetAttribute("Background Color");

                                    file = listItem.GetAttribute("Image");
                                    if (imageLibraryItems.ContainsKey(file))
                                    {
                                        file = imageLibraryItems[file];
                                    }
                                    image.Image = file;

                                    file = listItem.GetAttribute("Audio");
                                    if (audioLibraryItems.ContainsKey(file))
                                    {
                                        file = audioLibraryItems[file];
                                    }
                                    image.Audio = file;

                                    image.AudioOnUnhighlight = listItem.GetAttribute("AudioOnUnhighlight");
                                    myItems.Add(image);
                                    break;

                                case "MultipleChoice":
                                    MazeList_MultipleChoiceItem multipleChoice = new MazeList_MultipleChoiceItem();
                                    multipleChoice.Value.Clear();
                                    foreach (XmlElement node in listItem.ChildNodes)
                                    {
                                        ValueRet vr = new ValueRet(node.InnerText);
                                        vr.Ret = node.GetAttribute("Ret");
                                        multipleChoice.Value.Add(vr);
                                    }
                                    multipleChoice.TextDisplayType = (MazeList_MultipleChoiceItem.DisplayType)Enum.Parse(typeof(MazeList_MultipleChoiceItem.DisplayType), listItem.GetAttribute("TextDisplayType"));
                                    multipleChoice.LifeTime = Convert.ToInt64(listItem.GetAttribute("LifeTime"));
                                    multipleChoice.X = Convert.ToDouble(listItem.GetAttribute("X"));
                                    multipleChoice.Y = Convert.ToDouble(listItem.GetAttribute("Y"));

                                    file = listItem.GetAttribute("Audio");
                                    if (audioLibraryItems.ContainsKey(file))
                                    {
                                        file = audioLibraryItems[file];
                                    }
                                    multipleChoice.Audio = file;

                                    multipleChoice.AudioOnUnhighlight = listItem.GetAttribute("AudioOnUnhighlight");
                                    myItems.Add(multipleChoice);
                                    break;

                                default:
                                    break;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            ReloadList();

            return true;
        }

        public bool ReadFromFile(string inp, bool append)
        {
            if (!append)
            {
                ClearListMessage();
            }

            string fileExt = Path.GetExtension(inp).ToLower();

            switch (fileExt)
            {
                case ".xml":
                    return ReadFromMelx(inp);

                case ".melx":
                    return ReadFromMelx(inp);

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
            string buf;
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
            treeViewMazeList.Width = L_Up.Left - treeViewMazeList.Left - 10;
            treeViewMazeList.Height = closeButton.Top - treeViewMazeList.Top - 10;
            propertyGrid1.Height = treeViewMazeList.Height;
        }
    }
}