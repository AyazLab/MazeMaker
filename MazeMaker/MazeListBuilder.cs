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

        List<ListItem> mazeList = new List<ListItem>();
        int selectedIndex;

        public static bool madeChanges = false;

        string curFileName = "";

        public MazeListBuilder()
        {
            InitializeComponent();

            treeView.HideSelection = false;

            il.Images.Add("MazeListOptions", Properties.Resources.MazeListOptionsItemIcon);
            il.Images.Add("ListItems", Properties.Resources.ListItemsIcon);
            il.Images.Add("Maze", Properties.Resources.MazeItemIcon);
            il.Images.Add("Text", Properties.Resources.TextItemIcon);
            il.Images.Add("Image", Properties.Resources.ImageItemIcon);
            il.Images.Add("MultipleChoice", Properties.Resources.MultipleChoiceItemIcon);
            il.Images.Add("RecordAudio", Properties.Resources.RecordAudioItemIcon);
            il.Images.Add("Command", Properties.Resources.CommandItemIcon);

            treeView.ImageList = il;
        }

        void UpdateMazeList()
        {
            treeView.Nodes.Clear();

            if (mazeList.Count == 0)
            {
                mazeList.Add(new MazeListOptionsListItem());
            }

            for (int i = 0; i < mazeList.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        treeView.Nodes.Add(mazeList[i].ToString());
                        treeView.Nodes[treeView.Nodes.Count - 1].ImageKey = "MazeListOptions";
                        treeView.Nodes[treeView.Nodes.Count - 1].SelectedImageKey = "MazeListOptions";

                        treeView.Nodes.Add("ListItems");
                        treeView.Nodes[treeView.Nodes.Count - 1].ImageKey = "ListItems";
                        treeView.Nodes[treeView.Nodes.Count - 1].SelectedImageKey = "ListItems";
                        break;

                    default:
                        treeView.Nodes[1].Nodes.Add(i.ToString() + ") " + mazeList[i].ToString());

                        treeView.Nodes[1].Nodes[treeView.Nodes[1].Nodes.Count - 1].ImageKey = mazeList[i].Type.ToString();
                        treeView.Nodes[1].Nodes[treeView.Nodes[1].Nodes.Count - 1].SelectedImageKey = mazeList[i].Type.ToString();
                        break;
                }
            }

            treeView.ExpandAll();
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
                mazeList.Add(new MazeListOptionsListItem());
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
            sfd.Filter = "MazeList XML-File (*.melx)|*.melx|MazeList File (*.mel)|*.mel";
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
                    mazeList.Add(new MazeListItem());
                    break;

                case "Text":
                    mazeList.Add(new TextListItem());
                    break;

                case "Image":
                    mazeList.Add(new ImageListItem());
                    break;

                case "Multiple\nChoice":
                    mazeList.Add(new MultipleChoiceListItem(new ListChangedEventHandler(Updated)));
                    break;

                case "Record\nAudio":
                    mazeList.Add(new RecordAudioListItem());
                    break;

                case "Command":
                    mazeList.Add(new CommandListItem());
                    break;
            }

            UpdateMazeList();
        }

        private void L_Up_Click(object sender, EventArgs e)
        {
            madeChanges = true;

            ListItem temp = mazeList[selectedIndex - 1];
            mazeList[selectedIndex - 1] = mazeList[selectedIndex];
            mazeList[selectedIndex] = temp;
            UpdateMazeList();
            treeView.SelectedNode = treeView.Nodes[1].Nodes[selectedIndex - 2];
        }

        private void L_Down_Click(object sender, EventArgs e)
        {
            madeChanges = true;

            ListItem temp = mazeList[selectedIndex + 1];
            mazeList[selectedIndex + 1] = mazeList[selectedIndex];
            mazeList[selectedIndex] = temp;
            UpdateMazeList();
            treeView.SelectedNode = treeView.Nodes[1].Nodes[selectedIndex];
        }

        private void L_Del_Click(object sender, EventArgs e)
        {
            madeChanges = true;
            mazeList.RemoveAt(selectedIndex);
            UpdateMazeList();

            deleteButton.Enabled = false;
            upButton.Enabled = false;
            downButton.Enabled = false;
        }

        private void treeViewMazeList_AfterSelect(object sender, EventArgs e)
        {
            deleteButton.Enabled = false;
            upButton.Enabled = false;
            downButton.Enabled = false;

            selectedIndex = treeView.Nodes[1].Nodes.IndexOf(treeView.SelectedNode) + 1;
            if (selectedIndex >= 1)
            {
                deleteButton.Enabled = true;

                if (selectedIndex > 1)
                {
                    upButton.Enabled = true;
                }

                if (selectedIndex < treeView.Nodes[1].Nodes.Count)
                {
                    downButton.Enabled = true;
                }

                propertyGrid.SelectedObject = mazeList[selectedIndex];
            }
            else if (treeView.Nodes[0].IsSelected)
            {
                propertyGrid.SelectedObject = mazeList[0];
            }
            else if (treeView.Nodes[1].IsSelected)
            {
                propertyGrid.SelectedObject = null;
            }
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            madeChanges = true;

            ListItem listItem = (ListItem)propertyGrid.SelectedObject;
            switch (listItem.Type)
            {
                case ItemType.Maze:
                    MazeListItem maze = (MazeListItem)listItem;
                    maze.MazeFile = ManageItems("Maze", e.OldValue.ToString(), maze.MazeFile);
                    break;

                case ItemType.Text:
                    TextListItem text = (TextListItem)listItem;
                    text.AudioFile = ManageItems("Audio", e.OldValue.ToString(), text.AudioFile);
                    break;

                case ItemType.Image:
                    ImageListItem image = (ImageListItem)listItem;
                    image.ImageFile = ManageItems("Image", e.OldValue.ToString(), image.ImageFile);
                    image.AudioFile = ManageItems("Audio", e.OldValue.ToString(), image.AudioFile);
                    break;

                case ItemType.MultipleChoice:
                    MultipleChoiceListItem multipleChoice = (MultipleChoiceListItem)listItem;
                    multipleChoice.AudioFile = ManageItems("Audio", e.OldValue.ToString(), multipleChoice.AudioFile);
                    break;

                case ItemType.RecordAudio:
                    RecordAudioListItem recordAudio = (RecordAudioListItem)listItem;
                    recordAudio.ImageFile = ManageItems("Image", e.OldValue.ToString(), recordAudio.ImageFile);
                    break;
            }

            ReplaceFiles();
            UpdateMazeList();
            switch (selectedIndex)
            {
                case 0:
                    treeView.SelectedNode = treeView.Nodes[0];
                    break;

                default:
                    treeView.SelectedNode = treeView.Nodes[1].Nodes[selectedIndex - 1];
                    break;
            }
        }

        public static Dictionary<string, string> mazeFilePaths = new Dictionary<string, string>();
        public static Dictionary<string, string> imageFilePaths = new Dictionary<string, string>();
        public static Dictionary<string, string> audioFilePaths = new Dictionary<string, string>();
        public static List<string[]> replaceOrder = new List<string[]>();
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
                            replaceOrder = mmce.GetReplaceOrder();

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
                            replaceOrder = mmce.GetReplaceOrder();

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
                WriteToMelx(melxPath);

                if (Directory.Exists(melxPath + "_assets"))
                    Directory.Delete(melxPath + "_assets", true);

                Directory.CreateDirectory(melxPath + "_assets\\maze");
                Directory.CreateDirectory(melxPath + "_assets\\image");
                Directory.CreateDirectory(melxPath + "_assets\\audio");

                string copiedFiles = "";
                foreach (ListItem item in mazeList)
                {
                    string copiedFile0 = "no new file";
                    string copiedFile1 = "no new file";
                    switch (item.Type)
                    {
                        case ItemType.Maze:
                            MazeListItem maze = (MazeListItem)item;
                            if (maze.MazeFile != "")
                            {
                                string oldFilePath = mazeFilePaths[maze.MazeFile];
                                string newFilePath = melxPath + "_assets\\maze\\" + maze.MazeFile;

                                copiedFile0 = RecursiveFileCopy(oldFilePath, melxPath, "maze", newFilePath, ref replaceOrder);
                            }
                            break;

                        case ItemType.Text:
                            TextListItem text = (TextListItem)item;
                            if (text.AudioFile != "")
                            {
                                string oldFilePath = audioFilePaths[text.AudioFile];
                                string newFilePath = melxPath + "_assets\\audio\\" + text.AudioFile;

                                copiedFile0 = RecursiveFileCopy(oldFilePath, melxPath, "audio", newFilePath, ref replaceOrder);
                            }
                            break;

                        case ItemType.Image:
                            ImageListItem image = (ImageListItem)item;
                            if (image.ImageFile != "")
                            {
                                string oldFilePath = imageFilePaths[image.ImageFile];
                                string newFilePath = melxPath + "_assets\\image\\" + image.ImageFile;

                                copiedFile0 = RecursiveFileCopy(oldFilePath, melxPath, "image", newFilePath, ref replaceOrder);
                            }
                            if (image.AudioFile != "")
                            {
                                string oldFilePath = audioFilePaths[image.AudioFile];
                                string newFilePath = melxPath + "_assets\\audio\\" + image.AudioFile;

                                copiedFile1 = RecursiveFileCopy(oldFilePath, melxPath, "audio", newFilePath, ref replaceOrder);
                            }
                            break;

                        case ItemType.MultipleChoice:
                            MultipleChoiceListItem multipleChoice = (MultipleChoiceListItem)item;
                            if (multipleChoice.AudioFile != "")
                            {
                                string oldFilePath = audioFilePaths[multipleChoice.AudioFile];
                                string newFilePath = melxPath + "_assets\\audio\\" + multipleChoice.AudioFile;

                                copiedFile0 = RecursiveFileCopy(oldFilePath, melxPath, "audio", newFilePath, ref replaceOrder);
                            }
                            break;


                        case ItemType.RecordAudio:
                            RecordAudioListItem recordAudio = (RecordAudioListItem)item;
                            if (recordAudio.ImageFile != "")
                            {
                                string oldFilePath = imageFilePaths[recordAudio.ImageFile];
                                string newFilePath = melxPath + "_assets\\image\\" + recordAudio.ImageFile;

                                copiedFile0 = RecursiveFileCopy(oldFilePath, melxPath, "image", newFilePath, ref replaceOrder);
                            }
                            break;
                    }

                    ReplaceFiles();

                    if (!AddToLog(copiedFile0, ref copiedFiles) || !AddToLog(copiedFile1, ref copiedFiles))
                    {
                        ShowPM(melxPath, "\nPackage failed.", copiedFiles);
                        return;
                    }
                }

                ShowPM(melxPath, "\nPackage successfully generated", copiedFiles);
            }
        }

        public static void ShowPM(string filePath, string status, string log)
        {
            string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
            if (fileName == "")
                fileName = filePath;

            if (log == "")
                log = "\nNo assets present in this file";

            PackageMessage pm = new PackageMessage
            {
                log = "Generating package for " + fileName + " in " + filePath +
                "\nCreated asset folder " + fileName + "_assets in " + filePath + "_assets" +
                "\nCopying assets to folder..." +
                log +
                status,
            };

            pm.ShowDialog();
        }

        public static bool AddToLog(string copiedFile, ref string copiedFiles)
        {
            switch (copiedFile)
            {
                case "no new file":
                    return true;

                case "Maze List package failed.":
                    return false;

                default:
                    copiedFiles += "\n" + copiedFile;
                    return true;
            }
        }

        public static string RecursiveFileCopy(string oldFilePath, string melxPath, string type, string newFilePath, ref List<string[]> replaceOrder)
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

            oldFilePath = "..\\" + type + "\\" + fileName;
            if (File.Exists(oldFilePath))
            {
                File.Copy(oldFilePath, newFilePath);
                return oldFilePath;
            }

            oldFilePath = "..\\" + type + "s\\" + fileName;
            if (File.Exists(oldFilePath))
            {
                File.Copy(oldFilePath, newFilePath);
                return oldFilePath;
            }

            MessageBox.Show("\'" + fileName + "\' could not be found. If it can't be found, the package will be abandoned");
            while (true)
            {
                string fileExt = Path.GetExtension(fileName).ToLower();
                string[] imageExt = new string[] { ".bmp", ".jpg", ".jpeg", ".gif", ".png" };

                OpenFileDialog ofd = new OpenFileDialog();
                if (fileExt == ".maz")
                    ofd.Filter = "Maze File (*.maz)|*.maz";
                else if (imageExt.Contains(fileExt))
                    ofd.Filter = "Image File (*.bmp,*.jpg,*.jpeg,*.gif,*.png)|*.bmp;*.jpg;*.jpeg;*.gif;*.png";
                else if (fileExt == ".wav" || fileExt == ".mp3")
                    ofd.Filter = "Audio File (*.wav,*.mp3)|*.wav;*.mp3";
                else if (fileExt == ".obj")
                    ofd.Filter = "Model File (*.obj)|*.obj";
                ofd.Title = "Finding/Replacing " + fileName;

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
                                return "Maze List package failed.";
                        }
                    }

                    newFilePath = newFilePath.Substring(0, newFilePath.Length - newFilePath.Substring(newFilePath.LastIndexOf("\\") + 1).Length) + newFileName;

                    if (File.Exists(newFilePath))
                    {
                        return "no new file";
                    }

                    File.Copy(ofd.FileName, newFilePath);
                    replaceOrder.Add(new string[] { type, fileName, newFileName, newFilePath });
                    return ofd.FileName;
                }

                return "Maze List package failed.";
            }
        }

        void ReplaceFiles()
        {
            if (replaceOrder.Count != 0)
            {
                foreach (ListItem listItem in mazeList)
                {
                    switch (listItem.Type)
                    {
                        case ItemType.Maze:
                            MazeListItem maze = (MazeListItem)listItem;
                            foreach (string[] replaceInfo in replaceOrder)
                            {
                                if (replaceInfo[0] == "maze" && maze.MazeFile == replaceInfo[1])
                                {
                                    maze.MazeFile = replaceInfo[2];
                                    if (replaceInfo[2] != "")
                                        mazeFilePaths[replaceInfo[2]] = replaceInfo[3];
                                    mazeFilePaths.Remove(replaceInfo[1]);
                                }
                            }
                            break;

                        case ItemType.Text:
                            TextListItem text = (TextListItem)listItem;
                            foreach (string[] replaceInfo in replaceOrder)
                            {
                                if (replaceInfo[0] == "audio" && text.AudioFile == replaceInfo[1])
                                {
                                    text.AudioFile = replaceInfo[2];
                                    if (replaceInfo[2] != "")
                                        audioFilePaths[replaceInfo[2]] = replaceInfo[3];
                                    audioFilePaths.Remove(replaceInfo[1]);
                                }
                            }
                            break;

                        case ItemType.Image:
                            ImageListItem image = (ImageListItem)listItem;
                            foreach (string[] replaceInfo in replaceOrder)
                            {
                                if (replaceInfo[0] == "image" && image.ImageFile == replaceInfo[1])
                                {
                                    image.ImageFile = replaceInfo[2];
                                    if (replaceInfo[2] != "")
                                        imageFilePaths[replaceInfo[2]] = replaceInfo[3];
                                    imageFilePaths.Remove(replaceInfo[1]);
                                }
                                if (replaceInfo[0] == "audio" && image.AudioFile == replaceInfo[1])
                                {
                                    image.AudioFile = replaceInfo[2];
                                    if (replaceInfo[2] != "")
                                        audioFilePaths[replaceInfo[2]] = replaceInfo[3];
                                    audioFilePaths.Remove(replaceInfo[1]);
                                }
                            }
                            break;

                        case ItemType.MultipleChoice:
                            MultipleChoiceListItem multipleChoice = (MultipleChoiceListItem)listItem;
                            foreach (string[] replaceInfo in replaceOrder)
                            {
                                if (replaceInfo[0] == "audio" && multipleChoice.AudioFile == replaceInfo[1])
                                {
                                    multipleChoice.AudioFile = replaceInfo[2];
                                    if (replaceInfo[2] != "")
                                        audioFilePaths[replaceInfo[2]] = replaceInfo[3];
                                    audioFilePaths.Remove(replaceInfo[1]);
                                }
                            }
                            break;

                        case ItemType.RecordAudio:
                            RecordAudioListItem recordAudio = (RecordAudioListItem)listItem;
                            foreach (string[] replaceInfo in replaceOrder)
                            {
                                if (replaceInfo[0] == "audio" && recordAudio.ImageFile == replaceInfo[1])
                                {
                                    recordAudio.ImageFile = replaceInfo[2];
                                    if (replaceInfo[2] != "")
                                        imageFilePaths[replaceInfo[2]] = replaceInfo[3];
                                    imageFilePaths.Remove(replaceInfo[1]);
                                }
                            }
                            break;
                    }
                }

                replaceOrder.Clear();
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

            foreach (ListItem item in mazeList)
            {
                XmlElement mz = melx.CreateElement(item.Type.ToString());

                switch (item.Type)
                {
                    case ItemType.MazeListOptions:
                        MazeListOptionsListItem mazeListOptions = (MazeListOptionsListItem)item;

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
                        MazeListItem maze = (MazeListItem)item;

                        XmlElement mazeLibraryItem = melx.CreateElement("Maze");
                        int mazeID = mazeIDCounter;
                        if (mazeLibraryItems.ContainsKey(maze.MazeFile))
                        {
                            mazeID = Convert.ToInt32(mazeLibraryItems[maze.MazeFile]);
                        }
                        else if (maze.MazeFile != "")
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
                        TextListItem text = (TextListItem)item;
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
                        ImageListItem image = (ImageListItem)item;
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
                        MultipleChoiceListItem multipleChoice = (MultipleChoiceListItem)item;
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
                        RecordAudioListItem recordAudio = (RecordAudioListItem)item;
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
                        CommandListItem command = (CommandListItem)item;
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

            foreach (ListItem listItem in mazeList)
            {
                mel.Write(listItem.Type + "\t");

                switch (listItem.Type)
                {
                    case ItemType.Maze:
                        MazeListItem maze = (MazeListItem)listItem;
                        mel.Write(maze.MazeFile);
                        break;

                    case ItemType.Text:
                        TextListItem text = (TextListItem)listItem;
                        mel.Write(text.Text + "\t" + text.TextDisplayType + "\t" + text.Duration + "\t" + text.X + "\t" + text.Y + "\t ");
                        break;

                    case ItemType.Image:
                        ImageListItem image = (ImageListItem)listItem;
                        mel.Write(image.Text + "\t" + image.TextDisplayType + "\t" + image.Duration + "\t" + image.X + "\t" + image.Y + "\t" + image.ImageFile);
                        break;

                    case ItemType.MultipleChoice:
                        MultipleChoiceListItem multipleChoice = (MultipleChoiceListItem)listItem;
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

                        MazeListOptionsListItem mazeListOptions = new MazeListOptionsListItem();

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

                                    MazeListItem maze = new MazeListItem
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

                                    TextListItem text = new TextListItem
                                    {
                                        Text = listItem.InnerText,
                                        TextDisplayType = (TextListItem.DisplayType)Enum.Parse(typeof(TextListItem.DisplayType), listItem.GetAttribute("TextDisplayType")),
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

                                    ImageListItem image = new ImageListItem
                                    {
                                        Text = listItem.InnerText,
                                        TextDisplayType = (ImageListItem.DisplayType)Enum.Parse(typeof(ImageListItem.DisplayType), listItem.GetAttribute("TextDisplayType")),
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

                                    MultipleChoiceListItem multipleChoice = new MultipleChoiceListItem
                                    {
                                        TextDisplayType = (MultipleChoiceListItem.DisplayType)Enum.Parse(typeof(MultipleChoiceListItem.DisplayType), listItem.GetAttribute("TextDisplayType")),
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

                                    RecordAudioListItem recordAudio = new RecordAudioListItem
                                    {
                                        Text = listItem.InnerText,
                                        TextDisplayType = (RecordAudioListItem.DisplayType)Enum.Parse(typeof(RecordAudioListItem.DisplayType), listItem.GetAttribute("TextDisplayType")),
                                        Duration = Convert.ToInt64(listItem.GetAttribute("Duration")),

                                        BackgroundColor = Color.FromArgb(Convert.ToByte(backgroundColor[0]), Convert.ToByte(backgroundColor[1]), Convert.ToByte(backgroundColor[2]), Convert.ToByte(backgroundColor[3])),
                                        ImageFile = imageFileName,
                                    };

                                    mazeList.Add(recordAudio);
                                    break;

                                case "Command":
                                    CommandListItem command = new CommandListItem
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
                            MazeListItem maze = new MazeListItem();
                            maze.MazeFile = parse[1];

                            mazeList.Add(maze);
                            break;

                        case "Text":
                            TextListItem text = new TextListItem();
                            text.Text = parse[1];

                            if (parse[2].CompareTo(TextListItem.DisplayType.OnFramedDialog.ToString()) == 0 || parse[2].CompareTo("OnDialog") == 0)
                            {
                                text.TextDisplayType = TextListItem.DisplayType.OnFramedDialog;
                            }
                            else
                            {
                                text.TextDisplayType = TextListItem.DisplayType.OnBackground;
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
                            ImageListItem image = new ImageListItem();
                            image.Text = parse[1];

                            if (parse[2].CompareTo(ImageListItem.DisplayType.OnFramedDialog.ToString()) == 0 || parse[2].CompareTo("OnDialog") == 0)
                            {
                                image.TextDisplayType = ImageListItem.DisplayType.OnFramedDialog;
                            }
                            else
                            {
                                image.TextDisplayType = ImageListItem.DisplayType.OnBackground;
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
                            MultipleChoiceListItem multipleChoice = new MultipleChoiceListItem(new ListChangedEventHandler(Updated));
                            multipleChoice.LoadString(parse[1]);

                            if (parse[2].CompareTo(TextListItem.DisplayType.OnFramedDialog.ToString()) == 0 || parse[2].CompareTo("OnDialog") == 0)
                            {
                                multipleChoice.TextDisplayType = MultipleChoiceListItem.DisplayType.OnFramedDialog;
                            }
                            else
                            {
                                multipleChoice.TextDisplayType = MultipleChoiceListItem.DisplayType.OnBackground;
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
            treeView.Width = upButton.Left - treeView.Left - upButton.Width / 4;
            treeView.Height = closeButton.Bottom - treeView.Top;
            propertyGrid.Height = closeButton.Top - propertyGrid.Top - upButton.Height / 4;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutMazeListBuilder amlb = new AboutMazeListBuilder();
            amlb.ShowDialog();
        }

        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void treeView_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            TreeNode targetNode = treeView.GetNodeAt(treeView.PointToClient(new Point(e.X, e.Y)));

            if (!treeView.Nodes.Contains(draggedNode) && targetNode != null)
            {
                int draggedIndex = treeView.Nodes[1].Nodes.IndexOf(draggedNode) + 1;
                int targetIndex = treeView.Nodes[1].Nodes.IndexOf(targetNode) + 1;

                mazeList.Insert(targetIndex + 1, mazeList[draggedIndex]);

                if (targetIndex <= draggedIndex)
                    mazeList.RemoveAt(draggedIndex + 1);
                else
                    mazeList.RemoveAt(draggedIndex);

                UpdateMazeList();
            }
        }

        private void Cut(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null && treeView.SelectedNode != treeView.Nodes[0] && treeView.SelectedNode != treeView.Nodes[1])
            {
                Copy();
                mazeList.RemoveAt(selectedIndex);
                UpdateMazeList();
            }
        }

        private void Copy(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null && treeView.SelectedNode != treeView.Nodes[0] && treeView.SelectedNode != treeView.Nodes[1])
            {
                Copy();
            }
        }

        private void Paste(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null && treeView.SelectedNode != treeView.Nodes[0] && treeView.SelectedNode != treeView.Nodes[1])
            {
                mazeList.Insert(selectedIndex + 1, copiedListItem);
            }
            else
            {
                mazeList.Add(copiedListItem);
            }

            UpdateMazeList();
        }

        ListItem copiedListItem;
        void Copy()
        {
            copiedListItem = mazeList[selectedIndex];
        }
    }
}