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
using System.Xml.Schema;

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

        void MakeMazeList()
        {
            treeViewMazeList.Nodes.Clear();

            if (mazeList.Count == 0)
            {
                mazeList.Add(new MazeList_MazeListOptionsItem());
            }

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
            MakeMazeList();

            comboBox.SelectedIndex = 0;
        }

        public void toolStrip_open_Click(object sender, EventArgs e)
        {
            MakeMazeList();

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
                MakeMazeList();
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

            MakeMazeList();
        }

        private void L_Up_Click(object sender, EventArgs e)
        {
            madeChanges = true;

            MyBuilderItem temp = mazeList[selectedIndex - 1];
            mazeList[selectedIndex - 1] = mazeList[selectedIndex];
            mazeList[selectedIndex] = temp;
            MakeMazeList();
            treeViewMazeList.SelectedNode = treeViewMazeList.Nodes[1].Nodes[selectedIndex - 2];
        }

        private void L_Down_Click(object sender, EventArgs e)
        {
            madeChanges = true;

            MyBuilderItem temp = mazeList[selectedIndex + 1];
            mazeList[selectedIndex + 1] = mazeList[selectedIndex];
            mazeList[selectedIndex] = temp;
            MakeMazeList();
            treeViewMazeList.SelectedNode = treeViewMazeList.Nodes[1].Nodes[selectedIndex];
        }

        private void L_Del_Click(object sender, EventArgs e)
        {
            madeChanges = true;
            mazeList.RemoveAt(selectedIndex);
            MakeMazeList();

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
            {
                MyBuilderItem listItem = (MyBuilderItem)propertyGrid.SelectedObject;

                switch (listItem.Type)
                {
                    case ItemType.Maze:
                        MazeList_MazeItem maze = (MazeList_MazeItem)listItem;
                        maze.MazeFile = OpenCollection("Maze", maze.MazeFile);
                        break;

                    case ItemType.Text:
                        MazeList_TextItem text = (MazeList_TextItem)listItem;
                        text.AudioFile = OpenCollection("Audio", text.AudioFile);
                        break;

                    case ItemType.Image:
                        MazeList_ImageItem image = (MazeList_ImageItem)listItem;
                        image.ImageFile = OpenCollection("Image", image.ImageFile);
                        image.AudioFile = OpenCollection("Audio", image.AudioFile);
                        break;

                    case ItemType.MultipleChoice:
                        MazeList_MultipleChoiceItem multipleChoice = (MazeList_MultipleChoiceItem)listItem;
                        multipleChoice.AudioFile = OpenCollection("Audio", multipleChoice.AudioFile);
                        break;

                    default:
                        break;
                }
            }

            MakeMazeList();
            switch (selectedIndex)
            {
                case 0:
                    treeViewMazeList.SelectedNode = treeViewMazeList.Nodes[0];
                    break;

                default:
                    treeViewMazeList.SelectedNode = treeViewMazeList.Nodes[1].Nodes[selectedIndex - 1];
                    break;
            }
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
                                if (!mazeLibrary.ContainsKey(listItem))
                                {
                                    mazeLibrary[listItem] = listItem;
                                }
                                break;

                            case "Image":
                                if (!imageLibrary.ContainsKey(listItem))
                                {
                                    imageLibrary[listItem] = listItem;
                                }
                                break;

                            case "Audio":
                                if (!audioLibrary.ContainsKey(listItem))
                                {
                                    audioLibrary[listItem] = listItem;
                                }
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
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "MazeList File (*.melx)|*.melx|All Files|*.*";

            string filePath = "";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filePath = sfd.FileName;
                WriteToMelx(filePath);
            }

            if (Directory.Exists(filePath + "_assets\\maze"))
            {
                Directory.Delete(filePath + "_assets\\maze", true);
            }
            if (Directory.Exists(filePath + "_assets\\image"))
            {
                Directory.Delete(filePath + "_assets\\image", true);
            }
            if (Directory.Exists(filePath + "_assets\\audio"))
            {
                Directory.Delete(filePath + "_assets\\audio", true);
            }

            Directory.CreateDirectory(filePath + "_assets\\maze");
            Directory.CreateDirectory(filePath + "_assets\\image");
            Directory.CreateDirectory(filePath + "_assets\\audio");

            foreach (string key in mazeLibrary.Keys)
            {
                string newFilePath = filePath + "_assets\\maze\\" + key;

                File.Copy(mazeLibrary[key], newFilePath);
            }

            foreach (string key in imageLibrary.Keys)
            {
                string newFilePath = filePath + "_assets\\image\\" + key;

                File.Copy(imageLibrary[key], newFilePath);
            }

            foreach (string key in audioLibrary.Keys)
            {
                string newFilePath = filePath + "_assets\\audio\\" + key;

                File.Copy(audioLibrary[key], newFilePath);
            }
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

                        if (mazeListOptions.ComPortEnabled.ToString() != "")
                        {
                            XmlElement comPort = melx.CreateElement("COM_Port");
                            comPort.SetAttribute("Enabled", mazeListOptions.ComPortEnabled.ToString());
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

                        if (mazeListOptions.FontSize != null)
                        {
                            XmlElement font = melx.CreateElement("Font");
                            font.SetAttribute("Size", mazeListOptions.FontSize.ToString());
                            mz.AppendChild(font);
                        }

                        mzLs.AppendChild(mz);
                        break;

                    case ItemType.Maze:
                        MazeList_MazeItem maze = (MazeList_MazeItem)item;

                        XmlElement mazeLibraryItem = melx.CreateElement("Maze");
                        int mazeID = mazeIDCounter;
                        if (mazeLibraryItems.ContainsKey(maze.MazeFile))
                        {
                            mazeID = Convert.ToInt32(mazeLibraryItems[maze.MazeFile]);
                        }
                        else
                        {
                            mazeLibraryItems[maze.MazeFile] = mazeID.ToString();
                            mazeIDCounter++;

                            mazeLibraryItem.SetAttribute("ID", mazeID.ToString());
                            mazeLibraryItem.SetAttribute("File", maze.MazeFile);

                            mazeLibrary.AppendChild(mazeLibraryItem);
                        }
                        mz.SetAttribute("ID", mazeID.ToString());
                        if (maze.MazeFile == "")
                        {
                            mz.SetAttribute("ID", "");
                        }

                        mz.SetAttribute("StartPosition", maze.StartPosition);
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
                        if (audioLibraryItems.ContainsKey(text.AudioFile)) // duplicate audio, reuse ID
                        {
                            audioID = Convert.ToInt32(audioLibraryItems[text.AudioFile]);
                        }
                        else if (text.AudioFile != "") // audio not duplicate and audio not empty
                        {
                            audioLibraryItems[text.AudioFile] = audioID.ToString();
                            audioIDCounter++;

                            audioLibraryItem.SetAttribute("ID", audioID.ToString());
                            audioLibraryItem.SetAttribute("File", text.AudioFile);

                            audioLibrary.AppendChild(audioLibraryItem);
                        }
                        mz.SetAttribute("Audio", audioID.ToString());
                        if (text.AudioFile == "") // audio empty
                        {
                            mz.SetAttribute("Audio", "");
                        }

                        mz.SetAttribute("Loop", text.Loop.ToString());
                        mz.SetAttribute("AudioBehavior", text.AudioBehavior);
                        mz.SetAttribute("EndBehavior", text.EndBehavior);
                        mz.SetAttribute("FontSize", text.FontSize.ToString());
                        break;

                    case ItemType.Image:
                        MazeList_ImageItem image = (MazeList_ImageItem)item;
                        mz.InnerText = image.Text;
                        mz.SetAttribute("TextDisplayType", image.TextDisplayType.ToString());
                        mz.SetAttribute("LifeTime", image.Duration.ToString());
                        mz.SetAttribute("X", image.X.ToString());
                        mz.SetAttribute("Y", image.Y.ToString());
                        mz.SetAttribute("BackgroundColor", string.Format("{0}, {1}, {2}, {3}", image.BackgroundColor.A, image.BackgroundColor.R, image.BackgroundColor.G, image.BackgroundColor.B));

                        XmlElement imageLibraryItem = melx.CreateElement("Image");
                        int imageID = imageIDCounter;
                        if (imageLibraryItems.ContainsKey(image.ImageFile.ToString()))
                        {
                            imageID = Convert.ToInt32(imageLibraryItems[image.ImageFile.ToString()]);
                        }
                        else if (image.ImageFile.ToString() != "")
                        {
                            imageLibraryItems[image.ImageFile.ToString()] = imageID.ToString();
                            imageIDCounter++;

                            imageLibraryItem.SetAttribute("ID", imageID.ToString());
                            imageLibraryItem.SetAttribute("File", image.ImageFile.ToString());

                            imageLibrary.AppendChild(imageLibraryItem);
                        }
                        mz.SetAttribute("Image", imageID.ToString());
                        if (image.ImageFile.ToString() == "")
                        {
                            mz.SetAttribute("Image", "");
                        }

                        audioLibraryItem = melx.CreateElement("Audio");
                        audioID = audioIDCounter;
                        if (audioLibraryItems.ContainsKey(image.AudioFile))
                        {
                            audioID = Convert.ToInt32(audioLibraryItems[image.AudioFile]);
                        }
                        else if (image.AudioFile != "")
                        {
                            audioLibraryItems[image.AudioFile] = audioID.ToString();
                            audioIDCounter++;

                            audioLibraryItem.SetAttribute("ID", audioID.ToString());
                            audioLibraryItem.SetAttribute("File", image.AudioFile);

                            audioLibrary.AppendChild(audioLibraryItem);
                        }
                        mz.SetAttribute("Audio", audioID.ToString());
                        if (image.AudioFile == "")
                        {
                            mz.SetAttribute("Audio", "");
                        }

                        mz.SetAttribute("Loop", image.Loop.ToString());
                        mz.SetAttribute("AudioBehavior", image.AudioBehavior);
                        mz.SetAttribute("EndBehavior", image.EndBehavior);
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
                        if (audioLibraryItems.ContainsKey(multipleChoice.AudioFile))
                        {
                            audioID = Convert.ToInt32(audioLibraryItems[multipleChoice.AudioFile]);
                        }
                        else if (multipleChoice.AudioFile != "")
                        {
                            audioLibraryItems[multipleChoice.AudioFile] = audioID.ToString();
                            audioIDCounter++;

                            audioLibraryItem.SetAttribute("ID", audioID.ToString());
                            audioLibraryItem.SetAttribute("File", multipleChoice.AudioFile);

                            audioLibrary.AppendChild(audioLibraryItem);
                        }
                        mz.SetAttribute("Audio", audioID.ToString());
                        if (multipleChoice.AudioFile == "")
                        {
                            mz.SetAttribute("Audio", "");
                        }

                        mz.SetAttribute("Loop", multipleChoice.Loop.ToString());
                        mz.SetAttribute("AudioBehavior", multipleChoice.AudioBehavior);
                        mz.SetAttribute("EndBehavior", multipleChoice.EndBehavior);
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

        private bool WriteToMel(string filePath)
        {
            StreamWriter mel = new StreamWriter(filePath);

            if (mel == null)
            {
                return false;
            }

            mel.WriteLine("Maze List File 1.2");

            foreach (MyBuilderItem listItem in mazeList)
            {
                mel.Write(listItem.Type + "\t");

                switch (listItem.Type)
                {
                    case ItemType.Maze:
                        MazeList_MazeItem maze = (MazeList_MazeItem)listItem;
                        mel.Write(maze.MazeFile);
                        break;

                    case ItemType.Text:
                        MazeList_TextItem text = (MazeList_TextItem)listItem;
                        mel.Write(text.Text + "\t" + text.TextDisplayType + "\t" + text.Duration + "\t" + text.X + "\t" + text.Y + "\t ");
                        break;

                    case ItemType.Image:
                        MazeList_ImageItem image = (MazeList_ImageItem)listItem;
                        mel.Write(image.Text + "\t" + image.TextDisplayType + "\t" + image.Duration + "\t" + image.X + "\t" + image.Y + "\t" + image.ImageFile);
                        break;

                    case ItemType.MultipleChoice:
                        MazeList_MultipleChoiceItem multipleChoice = (MazeList_MultipleChoiceItem)listItem;
                        mel.Write(multipleChoice.GetString() + "\t" + multipleChoice.TextDisplayType + "\t" + multipleChoice.Duration + "\t" + multipleChoice.X + "\t" + multipleChoice.Y + "\t ");
                        break;

                    default:
                        break;
                }

                mel.Write("\n");
            }

            mel.Close();

            return true;
        }

        public void Updated(object e, ListChangedEventArgs c)
        {
            MakeMazeList();
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
                                    mazeListOptions.ComPortEnabled = bool.Parse(mazeListOption.GetAttribute("Enabled"));
                                    break;

                                case "LSL":
                                    mazeListOptions.Lsl = bool.Parse(mazeListOption.GetAttribute("Enabled"));
                                    break;

                                case "LPT":
                                    mazeListOptions.Lpt = bool.Parse(mazeListOption.GetAttribute("Enabled"));
                                    break;

                                case "Font":
                                    mazeListOptions.FontSize = Convert.ToInt32(mazeListOption.GetAttribute("Size"));
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
                            MakeMazeList();
                            switch (listItem.Name)
                            {
                                case "Maze":
                                    string file = listItem.GetAttribute("ID");
                                    if (mazeLibraryItems.ContainsKey(file))
                                    {
                                        file = mazeLibraryItems[file];
                                    }
                                    MazeList_MazeItem maze = new MazeList_MazeItem();
                                    maze.MazeFile = file;

                                    maze.StartPosition = listItem.GetAttribute("StartPosition");
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
                                    text.AudioFile = file;

                                    text.Loop = bool.Parse(listItem.GetAttribute("Loop"));
                                    text.AudioBehavior = listItem.GetAttribute("AudioBehavior");
                                    text.EndBehavior = listItem.GetAttribute("EndBehavior");
                                    text.FontSize = Convert.ToInt32(listItem.GetAttribute("FontSize"));
                                    mazeList.Add(text);
                                    break;

                                case "Image":
                                    MazeList_ImageItem image = new MazeList_ImageItem();
                                    image.Text = listItem.InnerText;
                                    image.TextDisplayType = (MazeList_ImageItem.DisplayType)Enum.Parse(typeof(MazeList_ImageItem.DisplayType), listItem.GetAttribute("TextDisplayType"));
                                    image.Duration = Convert.ToInt64(listItem.GetAttribute("LifeTime"));
                                    image.X = Convert.ToDouble(listItem.GetAttribute("X"));
                                    image.Y = Convert.ToDouble(listItem.GetAttribute("Y"));

                                    string[] backgroundColor = listItem.GetAttribute("BackgroundColor").Split(new string[] { ", " }, StringSplitOptions.None);
                                    image.BackgroundColor = Color.FromArgb(Convert.ToByte(backgroundColor[0]), Convert.ToByte(backgroundColor[1]), Convert.ToByte(backgroundColor[2]), Convert.ToByte(backgroundColor[3]));

                                    file = listItem.GetAttribute("Image");
                                    if (imageLibraryItems.ContainsKey(file))
                                    {
                                        file = imageLibraryItems[file];
                                    }
                                    image.ImageFile = file;

                                    file = listItem.GetAttribute("Audio");
                                    if (audioLibraryItems.ContainsKey(file))
                                    {
                                        file = audioLibraryItems[file];
                                    }
                                    image.AudioFile = file;

                                    image.Loop = bool.Parse(listItem.GetAttribute("Loop"));
                                    image.AudioBehavior = listItem.GetAttribute("AudioBehavior");
                                    image.EndBehavior = listItem.GetAttribute("EndBehavior");
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
                                    multipleChoice.AudioFile = file;

                                    multipleChoice.Loop = bool.Parse(listItem.GetAttribute("Loop"));
                                    multipleChoice.AudioBehavior = listItem.GetAttribute("AudioBehavior");
                                    multipleChoice.EndBehavior = listItem.GetAttribute("EndBehavior");
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

            MakeMazeList();

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

        public bool ReadFromMel(string filePath)
        {
            StreamReader mel = new StreamReader(filePath);
            if (mel == null)
            {
                return false;
            }

            string buffer = mel.ReadLine();
            if (!buffer.Contains("Maze List File"))
            {
                MessageBox.Show("Not a Maze List File or corrupted!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            curFileName = filePath;
            toolStrip_Status.Text = filePath;
            MakeMazeList();

            try
            {
                while (true)
                {
                    buffer = mel.ReadLine();

                    string[] parse = buffer.Split('\t');
                    if (parse.Length == 0)
                    {
                        break;
                    }

                    switch (parse[0])
                    {
                        case "Maze":
                            MazeList_MazeItem maze = new MazeList_MazeItem();
                            maze.MazeFile = parse[1];

                            mazeList.Add(maze);
                            break;

                        case "Text":
                            MazeList_TextItem text = new MazeList_TextItem();
                            text.Text = parse[1];

                            if (parse[2].CompareTo(MazeList_TextItem.DisplayType.OnFramedDialog.ToString()) == 0 || parse[2].CompareTo("OnDialog") == 0)
                            {
                                text.TextDisplayType = MazeList_TextItem.DisplayType.OnFramedDialog;
                            }
                            else
                            {
                                text.TextDisplayType = MazeList_TextItem.DisplayType.OnBackground;
                            }

                            text.Duration = int.Parse(parse[3]);
                            text.X = int.Parse(parse[4]);
                            text.Y = int.Parse(parse[5]);

                            if (parse.Length > 6)
                            {
                                text.BackgroundImage = parse[6];
                            }
                            else
                            {
                                text.BackgroundImage = "";
                            }

                            mazeList.Add(text);
                            break;

                        case "Image":
                            MazeList_ImageItem image = new MazeList_ImageItem();
                            image.Text = parse[1];

                            if (parse[2].CompareTo(MazeList_ImageItem.DisplayType.OnFramedDialog.ToString()) == 0 || parse[2].CompareTo("OnDialog") == 0)
                            {
                                image.TextDisplayType = MazeList_ImageItem.DisplayType.OnFramedDialog;
                            }
                            else
                            {
                                image.TextDisplayType = MazeList_ImageItem.DisplayType.OnBackground;
                            }

                            image.Duration = int.Parse(parse[3]);
                            image.X = int.Parse(parse[4]);
                            image.Y = int.Parse(parse[5]);

                            if (parse.Length > 6)
                            {
                                image.ImageFile = parse[6];
                            }
                            else
                            {
                                image.ImageFile = "";
                            }

                            mazeList.Add(image);
                            break;

                        case "MultipleChoice":
                            MazeList_MultipleChoiceItem multipleChoice = new MazeList_MultipleChoiceItem(new ListChangedEventHandler(Updated));
                            multipleChoice.LoadString(parse[1]);

                            if (parse[2].CompareTo(MazeList_TextItem.DisplayType.OnFramedDialog.ToString()) == 0 || parse[2].CompareTo("OnDialog") == 0)
                            {
                                multipleChoice.TextDisplayType = MazeList_MultipleChoiceItem.DisplayType.OnFramedDialog;
                            }
                            else
                            {
                                multipleChoice.TextDisplayType = MazeList_MultipleChoiceItem.DisplayType.OnBackground;
                            }

                            multipleChoice.Duration = int.Parse(parse[3]);
                            multipleChoice.X = int.Parse(parse[4]);
                            multipleChoice.Y = int.Parse(parse[5]);

                            if (parse.Length > 6)
                            {
                                multipleChoice.BackgroundImage = parse[6];
                            }
                            else
                            {
                                multipleChoice.BackgroundImage = "";
                            }

                            mazeList.Add(multipleChoice);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {

            }

            mel.Close();
            MakeMazeList();

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