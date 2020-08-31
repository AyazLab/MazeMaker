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
        ImageList il = new ImageList();

        List<MyBuilderItem> mazeList = new List<MyBuilderItem>();
        int selectedIndex;

        public static bool madeChanges = false;

        string curFileName = "";

        public MazeListBuilder()
        {
            InitializeComponent();

            treeViewMazeList.HideSelection = false;

            il.Images.Add("MazeListOptions", Properties.Resources.MazeListOptionsItemIcon);
            il.Images.Add("ListItems", Properties.Resources.ListItemsIcon);
            il.Images.Add("Maze", Properties.Resources.MazeItemIcon);
            il.Images.Add("Text", Properties.Resources.TextItemIcon);
            il.Images.Add("Image", Properties.Resources.ImageItemIcon);
            il.Images.Add("MultipleChoice", Properties.Resources.MultipleChoiceItemIcon);
            il.Images.Add("RecordAudio", Properties.Resources.RecordAudioItemIcon);
            il.Images.Add("Command", Properties.Resources.CommandItemIcon);

            treeViewMazeList.ImageList = il;
        }

        void UpdateMazeList()
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
                        treeViewMazeList.Nodes[treeViewMazeList.Nodes.Count - 1].ImageKey = "MazeListOptions";
                        treeViewMazeList.Nodes[treeViewMazeList.Nodes.Count - 1].SelectedImageKey = "MazeListOptions";

                        treeViewMazeList.Nodes.Add("ListItems");
                        treeViewMazeList.Nodes[treeViewMazeList.Nodes.Count - 1].ImageKey = "ListItems";
                        treeViewMazeList.Nodes[treeViewMazeList.Nodes.Count - 1].SelectedImageKey = "ListItems";
                        break;

                    default:
                        treeViewMazeList.Nodes[1].Nodes.Add(i.ToString() + ") " + mazeList[i].ToString());

                        treeViewMazeList.Nodes[1].Nodes[treeViewMazeList.Nodes[1].Nodes.Count - 1].ImageKey = mazeList[i].Type.ToString();
                        treeViewMazeList.Nodes[1].Nodes[treeViewMazeList.Nodes[1].Nodes.Count - 1].SelectedImageKey = mazeList[i].Type.ToString();
                        break;
                }
            }

            treeViewMazeList.ExpandAll();
        }

        private void MazeListBuilder_Load(object sender, EventArgs e)
        {
            toolStripLabel.Text = "Add Item\nTo MazeList\n";
            toolStripButtonMultipleChoiceItem.Text = "Multiple\nChoice";
            toolStripButtonRecordAudio.Text = "Record\nAudio";

            UpdateMazeList();

            MazeListBuilder_Resize(sender, e);
        }

        public void Open(object sender, EventArgs e)
        {
            UpdateMazeList();

            if (UnsavedMessage() != DialogResult.Cancel)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "MazeList File (*.melx,*.mel)|*.melx;*.mel";
                ofd.FilterIndex = 1;
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    ReadFromFile(ofd.FileName, false);
                }

                madeChanges = false;
            }
        }

        private void AppendToMazeList(object sender, EventArgs e)
        {
            madeChanges = true;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MazeList File (*.melx,*.mel)|*.melx;*.mel";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ReadFromFile(ofd.FileName, true);
            }
        }

        private void NewMazeList(object sender, EventArgs e)
        {
            if (UnsavedMessage() != DialogResult.Cancel)
            {
                ClearMazeList();
                mazeList.Add(new MazeList_MazeListOptionsItem());
                UpdateMazeList();
            }
        }

        private DialogResult UnsavedMessage()
        {
            if (madeChanges)
            {
                DialogResult dr = MessageBox.Show("Do you want to save changes to this file?", "MazeList Editor", MessageBoxButtons.YesNoCancel);
                switch (dr)
                {
                    case DialogResult.Yes:
                        if (SaveAs())
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

        void ClearMazeList()
        {
            if (UnsavedMessage() != DialogResult.Cancel)
            {
                mazeList.Clear();
            }
        }

        private void Save(object sender, EventArgs e)
        {
            if (curFileName == "")
            {
                SaveAs();
            }
            else
            {
                WriteToFile(curFileName);
            }
        }

        private void SaveAs(object sender, EventArgs e)
        {
            SaveAs();
        }

        private bool SaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "MazeList File (*.melx,*.mel)|*.melx;*.mel";
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

        private void AddToMazeList(object sender, EventArgs e)
        {
            madeChanges = true;
            string listItem = (sender as ToolStripButton).Text;

            switch (listItem)
            {
                case "Maze":
                    mazeList.Add(new MazeList_MazeItem());
                    break;

                case "Text":
                    mazeList.Add(new MazeList_TextItem());
                    break;

                case "Image":
                    mazeList.Add(new MazeList_ImageItem());
                    break;

                case "Multiple\nChoice":
                    mazeList.Add(new MazeList_MultipleChoiceItem(new ListChangedEventHandler(Updated)));
                    break;

                case "Record\nAudio":
                    mazeList.Add(new MazeList_RecordAudioItem());
                    break;

                case "Command":
                    mazeList.Add(new MazeList_CommandItem());
                    break;
            }

            UpdateMazeList();
        }

        private void L_Up_Click(object sender, EventArgs e)
        {
            madeChanges = true;

            MyBuilderItem temp = mazeList[selectedIndex - 1];
            mazeList[selectedIndex - 1] = mazeList[selectedIndex];
            mazeList[selectedIndex] = temp;
            UpdateMazeList();
            treeViewMazeList.SelectedNode = treeViewMazeList.Nodes[1].Nodes[selectedIndex - 2];
        }

        private void L_Down_Click(object sender, EventArgs e)
        {
            madeChanges = true;

            MyBuilderItem temp = mazeList[selectedIndex + 1];
            mazeList[selectedIndex + 1] = mazeList[selectedIndex];
            mazeList[selectedIndex] = temp;
            UpdateMazeList();
            treeViewMazeList.SelectedNode = treeViewMazeList.Nodes[1].Nodes[selectedIndex];
        }

        private void L_Del_Click(object sender, EventArgs e)
        {
            madeChanges = true;
            mazeList.RemoveAt(selectedIndex);
            UpdateMazeList();

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

            MyBuilderItem listItem = (MyBuilderItem)propertyGrid.SelectedObject;
            switch (listItem.Type)
            {
                case ItemType.Maze:
                    MazeList_MazeItem maze = (MazeList_MazeItem)listItem;
                    maze.MazeFile = ManageItems("Maze", e.OldValue.ToString(), maze.MazeFile);
                    break;

                case ItemType.Text:
                    MazeList_TextItem text = (MazeList_TextItem)listItem;
                    text.AudioFile = ManageItems("Audio", e.OldValue.ToString(), text.AudioFile);
                    break;

                case ItemType.Image:
                    MazeList_ImageItem image = (MazeList_ImageItem)listItem;
                    image.ImageFile = ManageItems("Image", e.OldValue.ToString(), image.ImageFile);
                    image.AudioFile = ManageItems("Audio", e.OldValue.ToString(), image.AudioFile);
                    break;

                case ItemType.MultipleChoice:
                    MazeList_MultipleChoiceItem multipleChoice = (MazeList_MultipleChoiceItem)listItem;
                    multipleChoice.AudioFile = ManageItems("Audio", e.OldValue.ToString(), multipleChoice.AudioFile);
                    break;

                case ItemType.RecordAudio:
                    MazeList_RecordAudioItem recordAudio = (MazeList_RecordAudioItem)listItem;
                    recordAudio.ImageFile = ManageItems("Image", e.OldValue.ToString(), recordAudio.ImageFile);
                    break;
            }

            UpdateMazeList();
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

        public static Dictionary<string, string> mazeFilePaths = new Dictionary<string, string>();
        public static Dictionary<string, string> imageFilePaths = new Dictionary<string, string>();
        public static Dictionary<string, string> audioFilePaths = new Dictionary<string, string>();
        string ManageItems(string type, string oldValue, string newValue)
        {
            switch (newValue)
            {
                case "[Import Item]":
                    OpenFileDialog ofd = new OpenFileDialog();
                    switch (type)
                    {
                        case "Maze":
                            ofd.Filter = "Maze File (*.maz)|*.maz";
                            break;

                        case "Image":
                            ofd.Filter = "Image File (*.bmp,*.jpg,*.jpeg,*.gif,*.png)|*.bmp;*.jpg;*.jpeg;*.gif;*.png";
                            break;

                        case "Audio":
                            ofd.Filter = "Audio File (*.wav,*.mp3)|*.wav;*.mp3";
                            break;
                    }
                    DialogResult dr = ofd.ShowDialog();

                    if (type == "Maze" && dr == DialogResult.OK)
                    {
                        string fileName = ofd.FileName.Substring(ofd.FileName.LastIndexOf("\\") + 1);
                        if (fileName == "")
                            fileName = ofd.FileName;
                        mazeFilePaths[fileName] = ofd.FileName;
                        return fileName;
                    }
                    else if (type == "Image" && dr == DialogResult.OK)
                    {
                        string fileName = ofd.FileName.Substring(ofd.FileName.LastIndexOf("\\") + 1);
                        if (fileName == "")
                            fileName = ofd.FileName;
                        imageFilePaths[fileName] = ofd.FileName;
                        return fileName;
                    }
                    else if (type == "Audio" && dr == DialogResult.OK)
                    {
                        string fileName = ofd.FileName.Substring(ofd.FileName.LastIndexOf("\\") + 1);
                        if (fileName == "")
                            fileName = ofd.FileName;
                        audioFilePaths[fileName] = ofd.FileName;
                        return fileName;
                    }

                    return oldValue;

                case "[Manage Items]":
                    switch (type)
                    {
                        case "Image":
                            List<Texture> textures = FilesToTextures(imageFilePaths);
                            MazeMakerCollectionEditor mmce = new MazeMakerCollectionEditor(ref textures);
                            string filePath = mmce.GetTexture();
                            TexturesToFiles(mmce.GetTextures(), ref imageFilePaths);

                            if (filePath != "")
                            {
                                string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                                if (fileName == "")
                                    fileName = filePath;
                                imageFilePaths[fileName] = filePath;
                                return fileName;
                            }

                            break;

                        case "Audio":
                            List<Audio> audios = FilesToAudios(audioFilePaths);
                            mmce = new MazeMakerCollectionEditor(ref audios);
                            filePath = mmce.GetAudio();
                            AudiosToFiles(mmce.GetAudios(), ref audioFilePaths);

                            if (filePath != "")
                            {
                                string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                                if (fileName == "")
                                    fileName = filePath;
                                audioFilePaths[fileName] = filePath;
                                return fileName;
                            }

                            break;
                    }

                    return oldValue;

                case "----------------------------------------":
                    return oldValue;

                default:
                    if (type == "Maze" && newValue != "" && !mazeFilePaths.ContainsKey(newValue))
                        mazeFilePaths[newValue] = newValue;

                    else if (type == "Image" && newValue != "" && !imageFilePaths.ContainsKey(newValue))
                        imageFilePaths[newValue] = newValue;

                    else if (type == "Audio" && newValue != "" && !audioFilePaths.ContainsKey(newValue))
                        audioFilePaths[newValue] = newValue;

                    return newValue;
            }
        }

        public static List<Texture> FilesToTextures(Dictionary<string, string> imageFilePaths)
        {
            List<Texture> textures = new List<Texture>();

            foreach (string value in imageFilePaths.Values)
            {
                string directory = Path.GetDirectoryName(value);
                string fileName = Path.GetFileName(value);
                textures.Add(new Texture(directory, fileName, 0));
            }

            return textures;
        }

        public static void TexturesToFiles(List<Texture> textures, ref Dictionary<string, string> imageFilePaths)
        {
            foreach (Texture texture in textures)
            {
                imageFilePaths[texture.name] = texture.filePath;
            }
        }

        public static List<Audio> FilesToAudios(Dictionary<string, string> audioFilePaths)
        {
            List<Audio> audios = new List<Audio>();

            foreach (string value in audioFilePaths.Values)
            {
                string directory = Path.GetDirectoryName(value);
                string fileName = Path.GetFileName(value);
                audios.Add(new Audio(directory, fileName, 0));
            }

            return audios;
        }

        public static void AudiosToFiles(List<Audio> audios, ref Dictionary<string, string> audioFilePaths)
        {
            foreach (Audio audio in audios)
            {
                audioFilePaths[audio.name] = audio.filePath;
            }
        }
        
        public static List<Model> FilesToModels(Dictionary<string, string> modelFilePaths)
        {
            List<Model> models = new List<Model>();

            foreach (string value in modelFilePaths.Values)
            {
                string directory = Path.GetDirectoryName(value);
                string fileName = Path.GetFileName(value);
                models.Add(new Model(directory, fileName, 0));
            }

            return models;
        }

        public static void ModelsToFiles(List<Model> models, ref Dictionary<string, string> modelFilePaths)
        {
            foreach (Model model in models)
            {
                modelFilePaths[model.name] = model.filePath;
            }
        }

        private void Package(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "MazeList File (*.melx)|*.melx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                madeChanges = false;
                string melxPath = sfd.FileName;

                if (Directory.Exists(melxPath + "_assets"))
                    Directory.Delete(melxPath + "_assets", true);

                Directory.CreateDirectory(melxPath + "_assets\\maze");
                Directory.CreateDirectory(melxPath + "_assets\\image");
                Directory.CreateDirectory(melxPath + "_assets\\audio");

                string copiedFiles = "";

                foreach (MyBuilderItem item in mazeList)
                {
                    string copiedFile0 = "no new file";
                    string copiedFile1 = "no new file";
                    switch (item.Type)
                    {
                        case ItemType.Maze:
                            MazeList_MazeItem maze = (MazeList_MazeItem)item;
                            if (maze.MazeFile != "")
                            {
                                string oldFilePath = mazeFilePaths[maze.MazeFile];
                                string newFilePath = melxPath + "_assets\\maze\\" + maze.MazeFile;

                                copiedFile0 = RecursiveFileCopy(oldFilePath, melxPath, "maze", newFilePath);
                            }
                            break;

                        case ItemType.Text:
                            MazeList_TextItem text = (MazeList_TextItem)item;
                            if (text.AudioFile != "")
                            {
                                string oldFilePath = audioFilePaths[text.AudioFile];
                                string newFilePath = melxPath + "_assets\\audio\\" + text.AudioFile;

                                copiedFile0 = RecursiveFileCopy(oldFilePath, melxPath, "audio", newFilePath);
                            }
                            break;

                        case ItemType.Image:
                            MazeList_ImageItem image = (MazeList_ImageItem)item;
                            if (image.ImageFile != "")
                            {
                                string oldFilePath = imageFilePaths[image.ImageFile];
                                string newFilePath = melxPath + "_assets\\image\\" + image.ImageFile;

                                copiedFile0 = RecursiveFileCopy(oldFilePath, melxPath, "image", newFilePath);
                            }
                            if (image.AudioFile != "")
                            {
                                string oldFilePath = audioFilePaths[image.AudioFile];
                                string newFilePath = melxPath + "_assets\\audio\\" + image.AudioFile;

                                copiedFile1 = RecursiveFileCopy(oldFilePath, melxPath, "audio", newFilePath);
                            }
                            break;

                        case ItemType.MultipleChoice:
                            MazeList_MultipleChoiceItem multipleChoice = (MazeList_MultipleChoiceItem)item;
                            if (multipleChoice.AudioFile != "")
                            {
                                string oldFilePath = audioFilePaths[multipleChoice.AudioFile];
                                string newFilePath = melxPath + "_assets\\audio\\" + multipleChoice.AudioFile;

                                copiedFile0 = RecursiveFileCopy(oldFilePath, melxPath, "audio", newFilePath);
                            }
                            break;


                        case ItemType.RecordAudio:
                            MazeList_RecordAudioItem recordAudio = (MazeList_RecordAudioItem)item;
                            if (recordAudio.ImageFile != "")
                            {
                                string oldFilePath = imageFilePaths[recordAudio.ImageFile];
                                string newFilePath = melxPath + "_assets\\image\\" + recordAudio.ImageFile;

                                copiedFile0 = RecursiveFileCopy(oldFilePath, melxPath, "image", newFilePath);
                            }
                            break;
                    }

                    AddToLog(copiedFile0, ref copiedFiles);
                    AddToLog(copiedFile1, ref copiedFiles);
                }

                WriteToMelx(melxPath);

                PackageMessage mpl = new PackageMessage
                {
                    status = "Maze List successfully packaged.",
                    copiedFiles = copiedFiles,
                };
                mpl.ShowDialog();
            }
        }

        public static void AddToLog(string copiedFile, ref string copiedFiles)
        {
            switch (copiedFile)
            {
                case "no new file":
                    break;

                case "Maze List package failed.":
                    return;

                default:
                    copiedFiles += copiedFile + "\n";
                    break;
            }
        }

        public static string RecursiveFileCopy(string oldFilePath, string melxPath, string type, string newFilePath)
        {
            string fileName = oldFilePath;
            string melxDirectory = melxPath.Substring(0, melxPath.Length - melxPath.Substring(melxPath.LastIndexOf("\\") + 1).Length);
            if (oldFilePath.Contains("\\"))
            {
                fileName = oldFilePath.Substring(oldFilePath.LastIndexOf("\\") + 1);
                if (fileName == "")
                    fileName = oldFilePath;
            }

            if (File.Exists(newFilePath))
            {
                return "no new file";
            }

            if (File.Exists(oldFilePath))
            {
                File.Copy(oldFilePath, newFilePath);
                return oldFilePath;
            }

            oldFilePath = melxDirectory + fileName;
            if (File.Exists(oldFilePath))
            {
                File.Copy(oldFilePath, newFilePath);
                return oldFilePath;
            }

            oldFilePath = melxDirectory + type + "\\" + fileName;
            if (File.Exists(oldFilePath))
            {
                File.Copy(oldFilePath, newFilePath);
                return oldFilePath;
            }

            oldFilePath = melxDirectory + type + "s\\" + fileName;
            if (File.Exists(oldFilePath))
            {
                File.Copy(oldFilePath, newFilePath);
                return oldFilePath;
            }

            MessageBox.Show("\'" + fileName + "\' could not be found. If it can't be found, the package will be abandoned");
            PackageMessage mpl = new PackageMessage();
            while (true)
            {
                OpenFileDialog ofd = new OpenFileDialog();

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string newFileName = ofd.FileName.Substring(ofd.FileName.LastIndexOf("\\") + 1);
                    if (newFileName == "")
                        newFileName = ofd.FileName;

                    if (newFileName.Split('.')[0] != fileName.Split('.')[0])
                    {
                        switch (MessageBox.Show("The new file \'" + newFileName + "\' is different from \'" + fileName + "\'. Are you sure you want to use this file? Press \'No\' to search again! Press \'Cancel\' to abandon package!", "Are you sure?", MessageBoxButtons.YesNoCancel))
                        {
                            case DialogResult.Yes:
                                break;

                            case DialogResult.No: // search again
                                continue;

                            default:
                                mpl.status = "Maze List package failed.";
                                mpl.ShowDialog();
                                return mpl.status;
                        }
                    }

                    newFilePath = newFilePath.Substring(0, newFilePath.Length - newFilePath.Substring(newFilePath.LastIndexOf("\\") + 1).Length) + newFileName;

                    if (File.Exists(newFilePath))
                    {
                        return "no new file";
                    }

                    File.Copy(ofd.FileName, newFilePath);
                    return ofd.FileName;
                }

                mpl.status = "Maze List package failed.";
                mpl.ShowDialog();
                return mpl.status;
            }
        }

        private void Close(object sender, EventArgs e)
        {
            if (UnsavedMessage() != DialogResult.Cancel)
            {
                ClearMazeList();
                Close();
            }
        }

        private void MazeListBuilder_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UnsavedMessage() != DialogResult.Cancel)
            {
                ClearMazeList();
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

                            mazeLibraryItem.SetAttribute("MazeID", mazeID.ToString());

                            string filePath = mazeFilePaths[maze.MazeFile];
                            if (filePath[1] == ':')
                            {
                                MessageBox.Show("from: " + inp);
                                MessageBox.Show("to: " + filePath);
                                filePath = MakeRelativePath(inp, filePath);
                                MessageBox.Show("rel:" + filePath);
                            }
                            mazeLibraryItem.SetAttribute("File", filePath);

                            mazeLibrary.AppendChild(mazeLibraryItem);
                        }
                        mz.SetAttribute("MazeID", mazeID.ToString());
                        if (maze.MazeFile == "")
                        {
                            mz.SetAttribute("MazeID", "");
                        }

                        mz.SetAttribute("StartPosition", maze.StartPosition);
                        mz.SetAttribute("StartMessage", maze.StartMessage);
                        mz.SetAttribute("Timeout", maze.Timeout);
                        break;

                    case ItemType.Text:
                        MazeList_TextItem text = (MazeList_TextItem)item;
                        mz.InnerText = text.Text;
                        mz.SetAttribute("TextDisplayType", text.TextDisplayType.ToString());
                        mz.SetAttribute("Duration", text.Duration.ToString());
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

                            audioLibraryItem.SetAttribute("AudioID", audioID.ToString());

                            string filePath = audioFilePaths[text.AudioFile];
                            if (filePath[1] == ':')
                            {
                                filePath = MakeRelativePath(inp, filePath);
                            }
                            audioLibraryItem.SetAttribute("File", filePath);

                            audioLibrary.AppendChild(audioLibraryItem);
                        }
                        mz.SetAttribute("AudioID", audioID.ToString());
                        if (text.AudioFile == "") // audio empty
                        {
                            mz.SetAttribute("AudioID", "");
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
                        mz.SetAttribute("Duration", image.Duration.ToString());
                        mz.SetAttribute("X", image.X.ToString());
                        mz.SetAttribute("Y", image.Y.ToString());
                        mz.SetAttribute("BackgroundColor", string.Format("{0}, {1}, {2}, {3}", image.BackgroundColor.A, image.BackgroundColor.R, image.BackgroundColor.G, image.BackgroundColor.B));

                        XmlElement imageLibraryItem = melx.CreateElement("Image");
                        int imageID = imageIDCounter;
                        if (imageLibraryItems.ContainsKey(image.ImageFile))
                        {
                            imageID = Convert.ToInt32(imageLibraryItems[image.ImageFile]);
                        }
                        else if (image.ImageFile != "")
                        {
                            imageLibraryItems[image.ImageFile] = imageID.ToString();
                            imageIDCounter++;

                            imageLibraryItem.SetAttribute("ImageID", imageID.ToString());

                            string filePath = imageFilePaths[image.ImageFile];
                            if (filePath[1] == ':')
                            {
                                filePath = MakeRelativePath(inp, filePath);
                            }
                            imageLibraryItem.SetAttribute("File", filePath);

                            imageLibrary.AppendChild(imageLibraryItem);
                        }
                        mz.SetAttribute("ImageID", imageID.ToString());
                        if (image.ImageFile == "")
                        {
                            mz.SetAttribute("ImageID", "");
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

                            audioLibraryItem.SetAttribute("AudioID", audioID.ToString());

                            string filePath = audioFilePaths[image.AudioFile];
                            if (filePath[1] == ':')
                            {
                                filePath = MakeRelativePath(inp, filePath);
                            }
                            audioLibraryItem.SetAttribute("File", filePath);

                            audioLibrary.AppendChild(audioLibraryItem);
                        }
                        mz.SetAttribute("AudioID", audioID.ToString());
                        if (image.AudioFile == "")
                        {
                            mz.SetAttribute("AudioID", "");
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
                                choice.SetAttribute("Return", vr.Ret);
                                mz.AppendChild(choice);
                            }
                        }
                        mz.SetAttribute("TextDisplayType", multipleChoice.TextDisplayType.ToString());
                        mz.SetAttribute("Duration", multipleChoice.Duration.ToString());
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

                            audioLibraryItem.SetAttribute("AudioID", audioID.ToString());

                            string filePath = audioFilePaths[multipleChoice.AudioFile];
                            if (filePath[1] == ':')
                            {
                                filePath = MakeRelativePath(inp, filePath);
                            }
                            audioLibraryItem.SetAttribute("File", filePath);

                            audioLibrary.AppendChild(audioLibraryItem);
                        }
                        mz.SetAttribute("AudioID", audioID.ToString());
                        if (multipleChoice.AudioFile == "")
                        {
                            mz.SetAttribute("AudioID", "");
                        }

                        mz.SetAttribute("Loop", multipleChoice.Loop.ToString());
                        mz.SetAttribute("AudioBehavior", multipleChoice.AudioBehavior);
                        mz.SetAttribute("EndBehavior", multipleChoice.EndBehavior);
                        break;

                    case ItemType.RecordAudio:
                        MazeList_RecordAudioItem recordAudio = (MazeList_RecordAudioItem)item;
                        mz.InnerText = recordAudio.Text;
                        mz.SetAttribute("TextDisplayType", recordAudio.TextDisplayType.ToString());
                        mz.SetAttribute("Duration", recordAudio.Duration.ToString());
                        mz.SetAttribute("BackgroundColor", string.Format("{0}, {1}, {2}, {3}", recordAudio.BackgroundColor.A, recordAudio.BackgroundColor.R, recordAudio.BackgroundColor.G, recordAudio.BackgroundColor.B));

                        imageLibraryItem = melx.CreateElement("Image");
                        imageID = imageIDCounter;
                        if (imageLibraryItems.ContainsKey(recordAudio.ImageFile))
                        {
                            imageID = Convert.ToInt32(imageLibraryItems[recordAudio.ImageFile]);
                        }
                        else if (recordAudio.ImageFile != "")
                        {
                            imageLibraryItems[recordAudio.ImageFile] = imageID.ToString();
                            imageIDCounter++;

                            imageLibraryItem.SetAttribute("ImageID", imageID.ToString());

                            string filePath = imageFilePaths[recordAudio.ImageFile];
                            if (filePath[1] == ':')
                            {
                                filePath = MakeRelativePath(inp, filePath);
                            }
                            imageLibraryItem.SetAttribute("File", filePath);

                            imageLibrary.AppendChild(imageLibraryItem);
                        }
                        mz.SetAttribute("ImageID", imageID.ToString());
                        if (recordAudio.ImageFile == "")
                        {
                            mz.SetAttribute("ImageID", "");
                        }
                        break;

                    case ItemType.Command:
                        MazeList_CommandItem command = (MazeList_CommandItem)item;
                        mz.InnerText = command.Command;
                        mz.SetAttribute("WaitForComplete", command.Wait4Complete.ToString());
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

        string MakeRelativePath(string fromPath, string toPath)
        {
            try
            {
                Uri fromUri = new Uri(fromPath);
                Uri toUri = new Uri(toPath);

                if (fromUri.Scheme != toUri.Scheme) { return toPath; }

                String relativePath = Uri.UnescapeDataString(fromUri.MakeRelativeUri(toUri).ToString());

                if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
                    return relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

                return relativePath;
            }
            catch { return toPath; }
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
            UpdateMazeList();
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
                            mazeLibraryItems[mazeLibraryItem.GetAttribute("MazeID")] = mazeLibraryItem.GetAttribute("File");
                        }
                        break;

                    case "ImageLibrary":
                        foreach (XmlElement imageLibraryItem in mz.ChildNodes)
                        {
                            imageLibraryItems[imageLibraryItem.GetAttribute("ImageID")] = imageLibraryItem.GetAttribute("File");
                        }
                        break;

                    case "AudioLibrary":
                        foreach (XmlElement audioLibraryItem in mz.ChildNodes)
                        {
                            audioLibraryItems[audioLibraryItem.GetAttribute("AudioID")] = audioLibraryItem.GetAttribute("File");
                        }
                        break;

                    case "ListItems":
                        foreach (XmlElement listItem in mz.ChildNodes)
                        {
                            UpdateMazeList();
                            switch (listItem.Name)
                            {
                                case "Maze":
                                    string mazeFilePath = listItem.GetAttribute("MazeID"); // storing file path & getting file name
                                    string mazeFileName = mazeFilePath;
                                    if (mazeLibraryItems.ContainsKey(mazeFileName))
                                    {
                                        mazeFilePath = mazeLibraryItems[mazeFileName];
                                        mazeFileName = mazeFilePath.Substring(mazeFilePath.LastIndexOf("\\") + 1);
                                        if (mazeFileName == "")
                                            mazeFileName = mazeFilePath;
                                    }
                                    mazeFilePaths[mazeFileName] = mazeFilePath;

                                    MazeList_MazeItem maze = new MazeList_MazeItem
                                    {
                                        MazeFile = mazeFileName,
                                        StartPosition = listItem.GetAttribute("StartPosition"),
                                        StartMessage = listItem.GetAttribute("StartMessage"),
                                        Timeout = listItem.GetAttribute("Timeout")
                                    };

                                    mazeList.Add(maze);
                                    break;

                                case "Text":
                                    string audioFilePath = listItem.GetAttribute("AudioID");
                                    string audioFileName = audioFilePath;
                                    if (audioLibraryItems.ContainsKey(audioFileName))
                                    {
                                        audioFilePath = audioLibraryItems[audioFileName];
                                        audioFileName = audioFilePath.Substring(audioFilePath.LastIndexOf("\\") + 1);
                                        if (audioFileName == "")
                                            audioFileName = audioFilePath;
                                    }
                                    audioFilePaths[audioFileName] = audioFilePath;

                                    MazeList_TextItem text = new MazeList_TextItem
                                    {
                                        Text = listItem.InnerText,
                                        TextDisplayType = (MazeList_TextItem.DisplayType)Enum.Parse(typeof(MazeList_TextItem.DisplayType), listItem.GetAttribute("TextDisplayType")),
                                        Duration = Convert.ToInt64(listItem.GetAttribute("Duration")),
                                        X = Convert.ToDouble(listItem.GetAttribute("X")),
                                        Y = Convert.ToDouble(listItem.GetAttribute("Y")),

                                        AudioFile = audioFileName,
                                        Loop = bool.Parse(listItem.GetAttribute("Loop")),
                                        AudioBehavior = listItem.GetAttribute("AudioBehavior"),
                                        EndBehavior = listItem.GetAttribute("EndBehavior"),
                                        FontSize = Convert.ToInt32(listItem.GetAttribute("FontSize"))
                                    };

                                    mazeList.Add(text);
                                    break;

                                case "Image":
                                    string[] backgroundColor = listItem.GetAttribute("BackgroundColor").Split(new string[] { ", " }, StringSplitOptions.None);

                                    string imageFilePath = listItem.GetAttribute("ImageID");
                                    string imageFileName = imageFilePath;
                                    if (imageLibraryItems.ContainsKey(imageFileName))
                                    {
                                        imageFilePath = imageLibraryItems[imageFileName];
                                        imageFileName = imageFilePath.Substring(imageFilePath.LastIndexOf("\\") + 1);
                                        if (imageFileName == "")
                                            imageFileName = imageFilePath;
                                    }
                                    imageFilePaths[imageFileName] = imageFilePath;

                                    audioFilePath = listItem.GetAttribute("AudioID");
                                    audioFileName = audioFilePath;
                                    if (audioLibraryItems.ContainsKey(audioFileName))
                                    {
                                        audioFilePath = audioLibraryItems[audioFileName];
                                        audioFileName = audioFilePath.Substring(audioFilePath.LastIndexOf("\\") + 1);
                                        if (audioFileName == "")
                                            audioFileName = audioFilePath;
                                    }
                                    audioFilePaths[audioFileName] = audioFilePath;

                                    MazeList_ImageItem image = new MazeList_ImageItem
                                    {
                                        Text = listItem.InnerText,
                                        TextDisplayType = (MazeList_ImageItem.DisplayType)Enum.Parse(typeof(MazeList_ImageItem.DisplayType), listItem.GetAttribute("TextDisplayType")),
                                        Duration = Convert.ToInt64(listItem.GetAttribute("Duration")),
                                        X = Convert.ToDouble(listItem.GetAttribute("X")),
                                        Y = Convert.ToDouble(listItem.GetAttribute("Y")),

                                        BackgroundColor = Color.FromArgb(Convert.ToByte(backgroundColor[0]), Convert.ToByte(backgroundColor[1]), Convert.ToByte(backgroundColor[2]), Convert.ToByte(backgroundColor[3])),
                                        ImageFile = imageFileName,

                                        AudioFile = audioFileName,
                                        Loop = bool.Parse(listItem.GetAttribute("Loop")),
                                        AudioBehavior = listItem.GetAttribute("AudioBehavior"),
                                        EndBehavior = listItem.GetAttribute("EndBehavior")
                                    };

                                    mazeList.Add(image);
                                    break;

                                case "MultipleChoice":
                                    audioFilePath = listItem.GetAttribute("AudioID");
                                    audioFileName = audioFilePath;
                                    if (audioLibraryItems.ContainsKey(audioFileName))
                                    {
                                        audioFilePath = audioLibraryItems[audioFileName];
                                        audioFileName = audioFilePath.Substring(audioFilePath.LastIndexOf("\\") + 1);
                                        if (audioFileName == "")
                                            audioFileName = audioFilePath;
                                    }
                                    audioFilePaths[audioFileName] = audioFilePath;

                                    MazeList_MultipleChoiceItem multipleChoice = new MazeList_MultipleChoiceItem
                                    {
                                        TextDisplayType = (MazeList_MultipleChoiceItem.DisplayType)Enum.Parse(typeof(MazeList_MultipleChoiceItem.DisplayType), listItem.GetAttribute("TextDisplayType")),
                                        Duration = Convert.ToInt64(listItem.GetAttribute("Duration")),
                                        X = Convert.ToDouble(listItem.GetAttribute("X")),
                                        Y = Convert.ToDouble(listItem.GetAttribute("Y")),

                                        AudioFile = audioFileName,
                                        Loop = bool.Parse(listItem.GetAttribute("Loop")),
                                        AudioBehavior = listItem.GetAttribute("AudioBehavior"),
                                        EndBehavior = listItem.GetAttribute("EndBehavior")
                                    };

                                    multipleChoice.Text.Clear();
                                    foreach (XmlElement node in listItem.ChildNodes)
                                    {
                                        TextReturn vr = new TextReturn(node.InnerText)
                                        {
                                            Ret = node.GetAttribute("Return")
                                        };
                                        multipleChoice.Text.Add(vr);
                                    }

                                    mazeList.Add(multipleChoice);
                                    break;

                                case "RecordAudio":
                                    backgroundColor = listItem.GetAttribute("BackgroundColor").Split(new string[] { ", " }, StringSplitOptions.None);

                                    imageFilePath = listItem.GetAttribute("ImageID");
                                    imageFileName = imageFilePath;
                                    if (imageLibraryItems.ContainsKey(imageFileName))
                                    {
                                        imageFilePath = imageLibraryItems[imageFileName];
                                        imageFileName = imageFilePath.Substring(imageFilePath.LastIndexOf("\\") + 1);
                                        if (imageFileName == "")
                                            imageFileName = imageFilePath;
                                    }
                                    imageFilePaths[imageFileName] = imageFilePath;

                                    MazeList_RecordAudioItem recordAudio = new MazeList_RecordAudioItem
                                    {
                                        Text = listItem.InnerText,
                                        TextDisplayType = (MazeList_RecordAudioItem.DisplayType)Enum.Parse(typeof(MazeList_RecordAudioItem.DisplayType), listItem.GetAttribute("TextDisplayType")),
                                        Duration = Convert.ToInt64(listItem.GetAttribute("Duration")),

                                        BackgroundColor = Color.FromArgb(Convert.ToByte(backgroundColor[0]), Convert.ToByte(backgroundColor[1]), Convert.ToByte(backgroundColor[2]), Convert.ToByte(backgroundColor[3])),
                                        ImageFile = imageFileName,
                                    };

                                    mazeList.Add(recordAudio);
                                    break;

                                case "Command":
                                    MazeList_CommandItem command = new MazeList_CommandItem
                                    {
                                        Command = listItem.InnerText,
                                        Wait4Complete = bool.Parse(listItem.GetAttribute("WaitForComplete")),
                                    };

                                    mazeList.Add(command);
                                    break;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            UpdateMazeList();

            return true;
        }

        public bool ReadFromFile(string inp, bool append)
        {
            if (!append)
            {
                ClearMazeList();
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
            UpdateMazeList();

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
            UpdateMazeList();

            return true;
        }

        private void MazeListBuilder_Resize(object sender, EventArgs e)
        {
            treeViewMazeList.Width = L_Up.Left - treeViewMazeList.Left - L_Up.Width / 4;
            treeViewMazeList.Height = closeButton.Bottom - treeViewMazeList.Top;
            propertyGrid.Height = closeButton.Top - propertyGrid.Top - L_Up.Height / 4;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutMazeListBuilder amlb = new AboutMazeListBuilder();
            amlb.ShowDialog();
        }
    }
}