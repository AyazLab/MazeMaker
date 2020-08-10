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
        List<MyBuilderItem> mazeList = new List<MyBuilderItem>();
        int selectedIndex;

        public static bool madeChanges = false;

        string curFileName = "";

        public MazeListBuilder()
        {
            InitializeComponent();

            treeViewMazeList.HideSelection = false;
        }

        void ReloadList()
        {
            treeViewMazeList.Nodes.Clear();

            for (int i = 0; i < mazeList.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        treeViewMazeList.Nodes.Add(mazeList[i].ToString());
                        treeViewMazeList.Nodes.Add("ListItems");
                        break;

                    default:
                        treeViewMazeList.Nodes[1].Nodes.Add(i.ToString() + ") " + mazeList[i].ToString());
                        break;
                }
            }

            treeViewMazeList.ExpandAll();
        }

        private void MazeListBuilder_Load(object sender, EventArgs e)
        {
            comboBox.Items.Add("Maze File");
            comboBox.Items.Add("Text Display");
            comboBox.Items.Add("Image Display");
            comboBox.Items.Add("Multiple-Choice Display");
            comboBox.SelectedIndex = 0;

            mazeList.Add(new MazeList_MazeListOptionsItem());
            ReloadList();
        }

        private void toolStrip_open_Click(object sender, EventArgs e)
        {
            if (UnsavedChangesCheck() != DialogResult.Cancel)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "MazeList File (*.melx,*.mel)|*.melx;*.mel|All Files|*.*";
                ofd.FilterIndex = 1;
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    ReadFromFile(ofd.FileName, false);
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
                mazeList.Add(new MazeList_MazeListOptionsItem());
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
                mazeList.Clear();
            }
        }

        private void toolStrip_save_Click(object sender, EventArgs e)
        {
            if (curFileName == "")
            {
                DoSaveAs();
            }
            else
            {
                WriteToFile(curFileName);
            }
        }

        private void toolStrip_SaveAs_Click(object sender, EventArgs e)
        {
            DoSaveAs();
        }

        private bool DoSaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "MazeList XML-File|*.melx|Maze List files|*.mel";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                madeChanges = false;
                WriteToFile(sfd.FileName);
                return true;
            }

            return false;
        }

        private void add_Click(object sender, EventArgs e)
        {
            madeChanges = true;

            switch (comboBox.SelectedIndex)
            {
                case 0:
                    mazeList.Add(new MazeList_MazeItem());
                    break;

                case 1:
                    mazeList.Add(new MazeList_TextItem());
                    break;

                case 2:
                    mazeList.Add(new MazeList_ImageItem());
                    break;

                case 3:
                    mazeList.Add(new MazeList_MultipleChoiceItem(new ListChangedEventHandler(Updated)));
                    break;
            }

            ReloadList();
        }

        private void L_Up_Click(object sender, EventArgs e)
        {
            madeChanges = true;

            MyBuilderItem temp = mazeList[selectedIndex - 1];
            mazeList[selectedIndex - 1] = mazeList[selectedIndex];
            mazeList[selectedIndex] = temp;
            ReloadList();
            treeViewMazeList.SelectedNode = treeViewMazeList.Nodes[1].Nodes[selectedIndex - 2];
        }

        private void L_Down_Click(object sender, EventArgs e)
        {
            madeChanges = true;

            MyBuilderItem temp = mazeList[selectedIndex + 1];
            mazeList[selectedIndex + 1] = mazeList[selectedIndex];
            mazeList[selectedIndex] = temp;
            ReloadList();
            treeViewMazeList.SelectedNode = treeViewMazeList.Nodes[1].Nodes[selectedIndex];
        }

        private void L_Del_Click(object sender, EventArgs e)
        {
            madeChanges = true;
            mazeList.RemoveAt(selectedIndex);
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

            selectedIndex = treeViewMazeList.Nodes[1].Nodes.IndexOf(treeViewMazeList.SelectedNode) + 1;
            if (selectedIndex >= 1)
            {
                L_Del.Enabled = true;

                if (selectedIndex > 1)
                {
                    L_Up.Enabled = true;
                }

                if (selectedIndex < treeViewMazeList.Nodes[1].Nodes.Count)
                {
                    L_Down.Enabled = true;
                }

                propertyGrid.SelectedObject = mazeList[selectedIndex];
            }
            else if (treeViewMazeList.Nodes[0].IsSelected)
            {
                propertyGrid.SelectedObject = mazeList[0];
            }
            else if (treeViewMazeList.Nodes[1].IsSelected)
            {
                propertyGrid.SelectedObject = null;
            }
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            madeChanges = true;

            if (propertyGrid.SelectedObject != null)
            // context menu strip, import, & collection editor
            {
                MyBuilderItem listItem = (MyBuilderItem)propertyGrid.SelectedObject;

                switch (listItem.Type)
                {
                    case ItemType.Maze:
                        MazeList_MazeItem maze = (MazeList_MazeItem)listItem;
                        maze.Maze = OpenCollection("Maze", maze.Maze);
                        break;

                    case ItemType.Text:
                        MazeList_TextItem text = (MazeList_TextItem)listItem;
                        text.Audio = OpenCollection("Audio", text.Audio);
                        break;

                    case ItemType.Image:
                        MazeList_ImageItem image = (MazeList_ImageItem)listItem;
                        image.Image = OpenCollection("Image", image.Image);
                        image.Audio = OpenCollection("Audio", image.Audio);
                        break;

                    case ItemType.MultipleChoice:
                        MazeList_MultipleChoiceItem multipleChoice = (MazeList_MultipleChoiceItem)listItem;
                        multipleChoice.Audio = OpenCollection("Audio", multipleChoice.Audio);
                        break;

                    default:
                        break;
                }
            }

            ReloadList();
        }

        public static Dictionary<string, string> mazeLibrary = new Dictionary<string, string>();

        public static Dictionary<string, string> imageLibrary = new Dictionary<string, string>();
        List<Texture> textures = new List<Texture>();

        public static Dictionary<string, string> audioLibrary = new Dictionary<string, string>();
        List<Audio> audios = new List<Audio>();
        string OpenCollection(string type, string listItem)
        {
            string fileName = "";

            switch (listItem)
            {
                case "Import":
                    OpenFileDialog ofd = new OpenFileDialog();

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        fileName = ofd.FileName.Substring(ofd.FileName.LastIndexOf("\\") + 1);

                        switch (type)
                        {
                            case "Maze":
                                mazeLibrary[fileName] = ofd.FileName;
                                break;

                            case "Image":
                                imageLibrary[fileName] = ofd.FileName;
                                break;

                            case "Audio":
                                audioLibrary[fileName] = ofd.FileName;
                                break;

                            default:
                                return fileName;
                        }
                    }

                    return fileName;

                case "Collection Editor":
                    switch (type)
                    {
                        case "Image":
                            MazeMakerCollectionEditor mmce = new MazeMakerCollectionEditor(ref textures);
                            string filePath = mmce.GetTexture();
                            textures = mmce.GetTextures();

                            if (filePath != "")
                            {
                                fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                                imageLibrary[fileName] = filePath;
                            }

                            return fileName;

                        case "Audio":
                            mmce = new MazeMakerCollectionEditor(ref audios);
                            filePath = mmce.GetAudio();
                            audios = mmce.GetAudios();
                            
                            if (filePath != "")
                            {
                                fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                                audioLibrary[fileName] = filePath;
                            }
                            
                            return fileName;

                        default:
                            return "";
                    }

                default:
                    if (listItem != "")
                    {
                        switch (type)
                        {
                            case "Maze":
                                mazeLibrary[listItem] = listItem;
                                break;

                            case "Image":
                                imageLibrary[listItem] = listItem;
                                break;

                            case "Audio":
                                audioLibrary[listItem] = listItem;
                                break;

                            default:
                                break;
                        }
                    }

                    return listItem;
            }
        }

        private void toolStripButton_Package_Click(object sender, EventArgs e)
        {

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

            foreach (MyBuilderItem item in mazeList)
            {
                XmlElement mz = melx.CreateElement(item.Type.ToString());

                switch (item.Type)
                {
                    case ItemType.MazeListOptions:
                        MazeList_MazeListOptionsItem mazeListOptions = (MazeList_MazeListOptionsItem)item;

                        if (mazeListOptions.FullScreen.ToString() != "")
                        {
                            XmlElement fullScreen = melx.CreateElement("FullScreen");
                            fullScreen.InnerText = mazeListOptions.FullScreen.ToString();
                            mz.AppendChild(fullScreen);
                        }

                        if (mazeListOptions.ComPort.ToString() != "")
                        {
                            XmlElement comPort = melx.CreateElement("COM_Port");
                            comPort.SetAttribute("Enabled", mazeListOptions.ComPort.ToString());
                            mz.AppendChild(comPort);
                        }

                        if (mazeListOptions.Lsl.ToString() != "")
                        {
                            XmlElement lsl = melx.CreateElement("LSL");
                            lsl.SetAttribute("Enabled", mazeListOptions.Lsl.ToString());
                            mz.AppendChild(lsl);
                        }

                        if (mazeListOptions.Lpt.ToString() != "")
                        {
                            XmlElement lpt = melx.CreateElement("LPT");
                            lpt.SetAttribute("Enabled", mazeListOptions.Lpt.ToString());
                            mz.AppendChild(lpt);
                        }

                        if (mazeListOptions.FontSize != "")
                        {
                            XmlElement font = melx.CreateElement("Font");
                            font.SetAttribute("Size", mazeListOptions.FontSize);
                            mz.AppendChild(font);
                        }

                        mzLs.AppendChild(mz);
                        break;

                    case ItemType.Maze:
                        MazeList_MazeItem maze = (MazeList_MazeItem)item;

                        XmlElement mazeLibraryItem = melx.CreateElement("Maze");
                        int mazeID = mazeIDCounter;
                        if (mazeLibraryItems.ContainsKey(maze.Maze))
                        {
                            mazeID = Convert.ToInt32(mazeLibraryItems[maze.Maze]);
                        }
                        else
                        {
                            mazeLibraryItems[maze.Maze] = mazeID.ToString();
                            mazeIDCounter++;

                            mazeLibraryItem.SetAttribute("ID", mazeID.ToString());
                            mazeLibraryItem.SetAttribute("File", maze.Maze);

                            mazeLibrary.AppendChild(mazeLibraryItem);
                        }
                        mz.SetAttribute("ID", mazeID.ToString());
                        if (maze.Maze == "")
                        {
                            mz.SetAttribute("ID", "");
                        }

                        mz.SetAttribute("DefaultStartPosition", maze.DefaultStartPosition);
                        mz.SetAttribute("StartMessage", maze.StartMessage);
                        mz.SetAttribute("Timeout", maze.Timeout);
                        break;

                    case ItemType.Text:
                        MazeList_TextItem text = (MazeList_TextItem)item;
                        mz.InnerText = text.Text;
                        mz.SetAttribute("TextDisplayType", text.TextDisplayType.ToString());
                        mz.SetAttribute("LifeTime", text.Duration.ToString());
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
                        mz.InnerText = image.Text;
                        mz.SetAttribute("TextDisplayType", image.TextDisplayType.ToString());
                        mz.SetAttribute("LifeTime", image.Duration.ToString());
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
                        foreach (TextReturn vr in multipleChoice.Text)
                        {
                            if (isQuestion)
                            {
                                XmlElement question = melx.CreateElement("Question");
                                question.InnerText = vr.Text;
                                if (vr.Ret != "")
                                {
                                    question.SetAttribute("Return", vr.Ret);
                                }
                                mz.AppendChild(question);
                                isQuestion = false;
                            }
                            else
                            {
                                XmlElement choice = melx.CreateElement("Choice");
                                choice.InnerText = vr.Text;
                                choice.SetAttribute("Ret", vr.Ret);
                                mz.AppendChild(choice);
                            }
                        }
                        mz.SetAttribute("TextDisplayType", multipleChoice.TextDisplayType.ToString());
                        mz.SetAttribute("LifeTime", multipleChoice.Duration.ToString());
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

                if (mz.Name != "MazeListOptions")
                {
                    listItems.AppendChild(mz);
                }
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

            foreach (MyBuilderItem a in mazeList)
            {

                fp.Write(a.Type + "\t");
                if (a.Type == ItemType.Maze)
                {
                    fp.Write(a.Text);
                }
                else if (a.Type == ItemType.Text)
                {
                    MazeList_TextItem aa = (MazeList_TextItem)a;
                    fp.Write(aa.Text + "\t" + aa.TextDisplayType + "\t" + aa.Duration + "\t" + aa.X + "\t" + aa.Y + "\t ");
                }
                else if (a.Type == ItemType.Image)
                {
                    MazeList_ImageItem aa = (MazeList_ImageItem)a;
                    fp.Write(aa.Text + "\t" + aa.TextDisplayType + "\t" + aa.Duration + "\t" + aa.X + "\t" + aa.Y + "\t" + aa.Image);
                }
                else if (a.Type == ItemType.MultipleChoice)
                {
                    MazeList_MultipleChoiceItem aa = (MazeList_MultipleChoiceItem)a;
                    fp.Write(aa.GetString() + "\t" + aa.TextDisplayType + "\t" + aa.Duration + "\t" + aa.X + "\t" + aa.Y + "\t ");
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
                    case "MazeListOptions":
                        if (mazeList.Count != 0)
                        {
                            break;
                        }

                        MazeList_MazeListOptionsItem mazeListOptions = new MazeList_MazeListOptionsItem();

                        foreach (XmlElement mazeListOption in mz.ChildNodes)
                        {
                            switch (mazeListOption.Name)
                            {
                                case "FullScreen":
                                    mazeListOptions.FullScreen = bool.Parse(mazeListOption.InnerText);
                                    break;

                                case "COM_Port":
                                    mazeListOptions.ComPort = bool.Parse(mazeListOption.GetAttribute("Enabled"));
                                    break;

                                case "LSL":
                                    mazeListOptions.Lsl = bool.Parse(mazeListOption.GetAttribute("Enabled"));
                                    break;

                                case "LPT":
                                    mazeListOptions.Lpt = bool.Parse(mazeListOption.GetAttribute("Enabled"));
                                    break;

                                case "Font":
                                    mazeListOptions.FontSize = mazeListOption.GetAttribute("Size");
                                    break;

                                default:
                                    break;
                            }
                        }

                        mazeList.Add(mazeListOptions);
                        break;

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
                                case "Maze":
                                    string file = listItem.GetAttribute("ID");
                                    if (mazeLibraryItems.ContainsKey(file))
                                    {
                                        file = mazeLibraryItems[file];
                                    }
                                    MazeList_MazeItem maze = new MazeList_MazeItem();
                                    maze.Maze = file;

                                    maze.DefaultStartPosition = listItem.GetAttribute("DefaultStartPosition");
                                    maze.StartMessage = listItem.GetAttribute("StartMessage");
                                    maze.Timeout = listItem.GetAttribute("Timeout");
                                    mazeList.Add(maze);
                                    break;

                                case "Text":
                                    MazeList_TextItem text = new MazeList_TextItem();
                                    text.Text = listItem.InnerText;
                                    text.TextDisplayType = (MazeList_TextItem.DisplayType)Enum.Parse(typeof(MazeList_TextItem.DisplayType), listItem.GetAttribute("TextDisplayType"));
                                    text.Duration = Convert.ToInt64(listItem.GetAttribute("LifeTime"));
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
                                    mazeList.Add(text);
                                    break;

                                case "Image":
                                    MazeList_ImageItem image = new MazeList_ImageItem();
                                    image.Text = listItem.InnerText;
                                    image.TextDisplayType = (MazeList_ImageItem.DisplayType)Enum.Parse(typeof(MazeList_ImageItem.DisplayType), listItem.GetAttribute("TextDisplayType"));
                                    image.Duration = Convert.ToInt64(listItem.GetAttribute("LifeTime"));
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
                                    mazeList.Add(image);
                                    break;

                                case "MultipleChoice":
                                    MazeList_MultipleChoiceItem multipleChoice = new MazeList_MultipleChoiceItem();
                                    multipleChoice.Text.Clear();
                                    foreach (XmlElement node in listItem.ChildNodes)
                                    {
                                        TextReturn vr = new TextReturn(node.InnerText);
                                        vr.Ret = node.GetAttribute("Return");
                                        multipleChoice.Text.Add(vr);
                                    }
                                    multipleChoice.TextDisplayType = (MazeList_MultipleChoiceItem.DisplayType)Enum.Parse(typeof(MazeList_MultipleChoiceItem.DisplayType), listItem.GetAttribute("TextDisplayType"));
                                    multipleChoice.Duration = Convert.ToInt64(listItem.GetAttribute("LifeTime"));
                                    multipleChoice.X = Convert.ToDouble(listItem.GetAttribute("X"));
                                    multipleChoice.Y = Convert.ToDouble(listItem.GetAttribute("Y"));

                                    file = listItem.GetAttribute("Audio");
                                    if (audioLibraryItems.ContainsKey(file))
                                    {
                                        file = audioLibraryItems[file];
                                    }
                                    multipleChoice.Audio = file;

                                    multipleChoice.AudioOnUnhighlight = listItem.GetAttribute("AudioOnUnhighlight");
                                    mazeList.Add(multipleChoice);
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

            curFileName = inp;
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
                        MazeList_MazeItem maze = new MazeList_MazeItem();
                        maze.Maze = parsed[1];
                        mazeList.Add(maze);

                    }
                    else if (parsed[0].CompareTo("Text") == 0)
                    {
                        //Text Line
                        MazeList_TextItem aa = new MazeList_TextItem();
                        aa.Text = parsed[1];
                        if (parsed[2].CompareTo(MazeList_TextItem.DisplayType.OnFramedDialog.ToString()) == 0 || parsed[2].CompareTo("OnDialog") == 0)
                        {
                            aa.TextDisplayType = MazeList_TextItem.DisplayType.OnFramedDialog;
                        }
                        else
                        {
                            aa.TextDisplayType = MazeList_TextItem.DisplayType.OnBackground;
                        }
                        aa.Duration = Int32.Parse(parsed[3]);
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
                        mazeList.Add(aa);
                    }
                    else if (parsed[0].CompareTo("Image") == 0)
                    {
                        //Image Line
                        MazeList_ImageItem aa = new MazeList_ImageItem();
                        aa.Text = parsed[1];
                        if (parsed[2].CompareTo(MazeList_ImageItem.DisplayType.OnFramedDialog.ToString()) == 0 || parsed[2].CompareTo("OnDialog") == 0)
                        {
                            aa.TextDisplayType = MazeList_ImageItem.DisplayType.OnFramedDialog;
                        }
                        else
                        {
                            aa.TextDisplayType = MazeList_ImageItem.DisplayType.OnBackground;
                        }
                        aa.Duration = Int32.Parse(parsed[3]);
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
                        mazeList.Add(aa);
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
                        aa.Duration = Int32.Parse(parsed[3]);
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
                        mazeList.Add(aa);
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
            propertyGrid.Height = treeViewMazeList.Height;
        }
    }
}