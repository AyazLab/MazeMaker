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
        readonly ImageList il = new ImageList();

        readonly List<ListItem> MazeList = new List<ListItem>();
        int selectedIndex;

        public static bool madeChanges = false;

        string mLFilePath = "";

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

            if (MazeList.Count == 0)
            {
                MazeList.Add(new MazeListOptionsListItem());
            }

            for (int i = 0; i < MazeList.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        treeView.Nodes.Add(MazeList[i].ToString());
                        treeView.Nodes[treeView.Nodes.Count - 1].ImageKey = "MazeListOptions";
                        treeView.Nodes[treeView.Nodes.Count - 1].SelectedImageKey = "MazeListOptions";

                        treeView.Nodes.Add("ListItems");
                        treeView.Nodes[treeView.Nodes.Count - 1].ImageKey = "ListItems";
                        treeView.Nodes[treeView.Nodes.Count - 1].SelectedImageKey = "ListItems";
                        break;

                    default:
                        treeView.Nodes[1].Nodes.Add(i.ToString() + ") " + MazeList[i].ToString());

                        treeView.Nodes[1].Nodes[treeView.Nodes[1].Nodes.Count - 1].ImageKey = MazeList[i].Type.ToString();
                        treeView.Nodes[1].Nodes[treeView.Nodes[1].Nodes.Count - 1].SelectedImageKey = MazeList[i].Type.ToString();
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

        private void Append(object sender, EventArgs e)
        {
            madeChanges = true;
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "MazeList Files (*.melx;*.mel)|*.melx;*.mel",
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ReadFromFile(ofd.FileName, true);
            }
        }

        private void New(object sender, EventArgs e)
        {
            if (UnsavedMessage() != DialogResult.Cancel)
            {
                ClearMazeList();
                MazeList.Add(new MazeListOptionsListItem());
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
                MazeList.Clear();
            }
        }

        private void Save(object sender, EventArgs e)
        {
            if (mLFilePath == "")
            {
                SaveAs();
            }
            else
            {
                WriteToFile(mLFilePath);
            }
        }

        private void SaveAs(object sender, EventArgs e)
        {
            SaveAs();
        }

        private bool SaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "MazeList XML-Files (*.melx)|*.melx|MazeList Files (*.mel)|*.mel";
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

        private void AddListItem(object sender, EventArgs e)
        {
            madeChanges = true;
            string listItem = (sender as ToolStripButton).Text;

            switch (listItem)
            {
                case "Maze":
                    MazeList.Add(new MazeListItem());
                    break;

                case "Text":
                    MazeList.Add(new TextListItem());
                    break;

                case "Image":
                    MazeList.Add(new ImageListItem());
                    break;

                case "Multiple\nChoice":
                    MazeList.Add(new MultipleChoiceListItem(new ListChangedEventHandler(Updated)));
                    break;

                case "Record\nAudio":
                    MazeList.Add(new RecordAudioListItem());
                    break;

                case "Command":
                    MazeList.Add(new CommandListItem());
                    break;
            }

            UpdateMazeList();
        }

        private void MoveUp(object sender, EventArgs e)
        {
            madeChanges = true;

            ListItem temp = MazeList[selectedIndex - 1];
            MazeList[selectedIndex - 1] = MazeList[selectedIndex];
            MazeList[selectedIndex] = temp;
            UpdateMazeList();
            treeView.SelectedNode = treeView.Nodes[1].Nodes[selectedIndex - 2];
        }

        private void MoveDown(object sender, EventArgs e)
        {
            madeChanges = true;

            ListItem temp = MazeList[selectedIndex + 1];
            MazeList[selectedIndex + 1] = MazeList[selectedIndex];
            MazeList[selectedIndex] = temp;
            UpdateMazeList();
            treeView.SelectedNode = treeView.Nodes[1].Nodes[selectedIndex];
        }

        private void Delete(object sender, EventArgs e)
        {
            madeChanges = true;
            MazeList.RemoveAt(selectedIndex);
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

                propertyGrid.SelectedObject = MazeList[selectedIndex];
            }
            else if (treeView.Nodes[0].IsSelected)
            {
                propertyGrid.SelectedObject = MazeList[0];
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
                    maze.MazeFile = ManageItems("Maze", e.OldValue, maze.MazeFile);
                    break;

                case ItemType.Text:
                    TextListItem text = (TextListItem)listItem;
                    text.AudioFile = ManageItems("Audio", e.OldValue, text.AudioFile);
                    break;

                case ItemType.Image:
                    ImageListItem image = (ImageListItem)listItem;
                    image.ImageFile = ManageItems("Image", e.OldValue, image.ImageFile);
                    image.AudioFile = ManageItems("Audio", e.OldValue, image.AudioFile);
                    break;

                case ItemType.MultipleChoice:
                    MultipleChoiceListItem multipleChoice = (MultipleChoiceListItem)listItem;
                    multipleChoice.AudioFile = ManageItems("Audio", e.OldValue, multipleChoice.AudioFile);
                    break;

                case ItemType.RecordAudio:
                    RecordAudioListItem recordAudio = (RecordAudioListItem)listItem;
                    recordAudio.ImageFile = ManageItems("Image", e.OldValue, recordAudio.ImageFile);
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
        string ManageItems(string type, object oldValue, string newValue)
        {
            string fileName = "";

            switch (newValue)
            {
                case "[Import Item]":
                    OpenFileDialog ofd = new OpenFileDialog();
                    switch (type)
                    {
                        case "Maze":
                            ofd.Filter = "Maze Files (*.maz;*.mazx)|*.maz;*.mazx";
                            break;

                        case "Image":
                            ofd.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.gif;*.png)|*.bmp;*.jpg;*.jpeg;*.gif;*.png";
                            break;

                        case "Audio":
                            ofd.Filter = "Audio Files (*.wav;*.mp3)|*.wav;*.mp3";
                            break;
                    }
                    DialogResult dr = ofd.ShowDialog();

                    if (type == "Maze" && dr == DialogResult.OK)
                    {
                        fileName = ofd.FileName.Substring(ofd.FileName.LastIndexOf("\\") + 1);
                        if (fileName == "")
                            fileName = ofd.FileName;
                        mazeFilePaths[fileName] = ofd.FileName;
                        return fileName;
                    }
                    else if (type == "Image" && dr == DialogResult.OK)
                    {
                        fileName = ofd.FileName.Substring(ofd.FileName.LastIndexOf("\\") + 1);
                        if (fileName == "")
                            fileName = ofd.FileName;
                        imageFilePaths[fileName] = ofd.FileName;
                        return fileName;
                    }
                    else if (type == "Audio" && dr == DialogResult.OK)
                    {
                        fileName = ofd.FileName.Substring(ofd.FileName.LastIndexOf("\\") + 1);
                        if (fileName == "")
                            fileName = ofd.FileName;
                        audioFilePaths[fileName] = ofd.FileName;
                        return fileName;
                    }

                    return (string)oldValue;

                case "[Manage Items]":
                    switch (type)
                    {
                        case "Maze":
                            CollectionEditor collection = new CollectionEditor(mazeFilePaths);

                            fileName = collection.GetMaze();
                            mazeFilePaths = collection.GetMazes();
                            replaceOrder = collection.GetReplaceOrder();

                            break;

                        case "Image":
                            collection = new CollectionEditor(FilesToTextures(imageFilePaths));

                            fileName = collection.GetTexture();
                            TexturesToFiles(collection.GetTextures(), ref imageFilePaths);
                            replaceOrder = collection.GetReplaceOrder();

                            break;

                        case "Audio":
                            collection = new CollectionEditor(FilesToAudios(audioFilePaths));

                            fileName = collection.GetAudio();
                            AudiosToFiles(collection.GetAudios(), ref audioFilePaths);
                            replaceOrder = collection.GetReplaceOrder();

                            break;
                    }

                    if (fileName != "")
                        return fileName;
                    return (string)oldValue;

                case "----------------------------------------":
                    return (string)oldValue;

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
                imageFilePaths[texture.Name] = texture.FilePath;
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
                audioFilePaths[audio.Name] = audio.FilePath;
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
                modelFilePaths[model.Name] = model.FilePath;
            }
        }

        private void Package(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "MazeList XML-Files (*.melx)|*.melx"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                madeChanges = false;

                string melxPath = sfd.FileName;
                WriteToMelx(melxPath);
                string tempPath = Path.GetDirectoryName(sfd.FileName) + "\\Temp";
                if (Directory.Exists(tempPath + "_assets"))
                    Directory.Delete(tempPath + "_assets", true);

                Directory.CreateDirectory(melxPath + "_assets\\maze");
                Directory.CreateDirectory(melxPath + "_assets\\image");
                Directory.CreateDirectory(melxPath + "_assets\\audio");

 

                string copiedFiles = "";
                foreach (ListItem item in MazeList)
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

                                bool zipMode = true;
                                bool packageMaze = true;

                                mazeFilePaths[maze.MazeFile] = newFilePath;

                                string extOrig = Path.GetExtension(newFilePath);

                                if(extOrig.ToLower().EndsWith("x"))
                                {
                                    packageMaze = false;
                                }
                                else if(zipMode)
                                {
                                    mazeFilePaths[maze.MazeFile] = mazeFilePaths[maze.MazeFile] + "x";
                                }

                                copiedFile0 = RecursiveFileCopy(oldFilePath, melxPath, "maze", newFilePath, ref replaceOrder);

                                if(!oldFilePath.Contains(":"))
                                {
                                    oldFilePath = Path.GetDirectoryName(melxPath) + "\\" + oldFilePath;
                                }

                                if(packageMaze)
                                { 
                                    Maze tempMaze = new Maze();
                                    if(tempMaze.ReadFromFileXML(oldFilePath))
                                    {
                                        tempMaze.Package(newFilePath, out copiedFiles, replaceOrder, zipMode);
                                    
                                    }
                                    else
                                        {
                                        copiedFiles = copiedFiles + "\nUnable to package " + oldFilePath;
                                        MessageBox.Show("Not a maze file or corrupted maze file", "MazeMaker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }





                            }
                            break;

                        case ItemType.Text:
                            TextListItem text = (TextListItem)item;
                            if (text.AudioFile != "")
                            {
                                string oldFilePath = audioFilePaths[text.AudioFile];
                                string newFilePath = melxPath + "_assets\\audio\\" + text.AudioFile;

                                audioFilePaths[text.AudioFile] = newFilePath;

                                copiedFile0 = RecursiveFileCopy(oldFilePath, melxPath, "audio", newFilePath, ref replaceOrder);
                            }
                            break;

                        case ItemType.Image:
                            ImageListItem image = (ImageListItem)item;
                            if (image.ImageFile != "")
                            {
                                string oldFilePath = imageFilePaths[image.ImageFile];
                                string newFilePath = melxPath + "_assets\\image\\" + image.ImageFile;

                                imageFilePaths[image.ImageFile] = newFilePath;

                                copiedFile0 = RecursiveFileCopy(oldFilePath, melxPath, "image", newFilePath, ref replaceOrder);

                            }
                            if (image.AudioFile != "")
                            {
                                string oldFilePath = audioFilePaths[image.AudioFile];
                                string newFilePath = melxPath + "_assets\\audio\\" + image.AudioFile;
                                audioFilePaths[image.AudioFile] = newFilePath;

                                copiedFile1 = RecursiveFileCopy(oldFilePath, melxPath, "audio", newFilePath, ref replaceOrder);
                            }
                            break;

                        case ItemType.MultipleChoice:
                            MultipleChoiceListItem multipleChoice = (MultipleChoiceListItem)item;
                            if (multipleChoice.AudioFile != "")
                            {
                                string oldFilePath = audioFilePaths[multipleChoice.AudioFile];
                                string newFilePath = melxPath + "_assets\\audio\\" + multipleChoice.AudioFile;
                                audioFilePaths[multipleChoice.AudioFile] = newFilePath;

                                copiedFile0 = RecursiveFileCopy(oldFilePath, melxPath, "audio", newFilePath, ref replaceOrder);
                            }
                            break;


                        case ItemType.RecordAudio:
                            RecordAudioListItem recordAudio = (RecordAudioListItem)item;
                            if (recordAudio.ImageFile != "")
                            {
                                string oldFilePath = imageFilePaths[recordAudio.ImageFile];
                                string newFilePath = melxPath + "_assets\\image\\" + recordAudio.ImageFile;

                                imageFilePaths[recordAudio.ImageFile] = newFilePath;

                                copiedFile0 = RecursiveFileCopy(oldFilePath, melxPath, "image", newFilePath, ref replaceOrder);
                            }
                            break;
                    }

                    ReplaceFiles();
                    UpdateMazeList();
                    

                    if (!AddToLog(copiedFile0, ref copiedFiles) || !AddToLog(copiedFile1, ref copiedFiles))
                    {
                        ShowPM(melxPath, "\nPackage failed.", copiedFiles);
                        return;
                    }
                }

                WriteToMelx(melxPath);
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
            string oldFileName = oldFilePath.Substring(oldFilePath.LastIndexOf("\\") + 1);
            if (oldFileName == "")
                oldFileName = oldFilePath;
            string melxDirectory = Path.GetDirectoryName(melxPath);

            // file already exists in assets
            if (File.Exists(newFilePath))
            {
                return "no new file";
            }

            // checks original location
            if (oldFilePath[0] == '.' && oldFilePath[1] == '.')
                oldFilePath = melxDirectory + "\\" + oldFilePath;
            //MessageBox.Show(oldFilePath);
            if (File.Exists(oldFilePath))
            {
                File.Copy(oldFilePath, newFilePath);
                return oldFilePath;
            }

            // checks './fileName'
            oldFilePath = melxDirectory + "\\" + oldFileName;
            //MessageBox.Show(oldFilePath);
            if (File.Exists(oldFilePath))
            {
                File.Copy(oldFilePath, newFilePath);
                return oldFilePath;
            }

            // checks './image/fileName'
            oldFilePath = melxDirectory + "\\" + type + "\\" + oldFileName;
            //MessageBox.Show(oldFilePath);
            if (File.Exists(oldFilePath))
            {
                File.Copy(oldFilePath, newFilePath);
                return oldFilePath;
            }

            // checks './images/fileName'
            oldFilePath = melxDirectory + "\\" + type + "s\\" + oldFileName;
            //MessageBox.Show(oldFilePath);
            if (File.Exists(oldFilePath))
            {
                File.Copy(oldFilePath, newFilePath);
                return oldFilePath;
            }

            // checks '../image/fileName'
            oldFilePath = melxDirectory + "\\..\\" + type + "\\" + oldFileName;
            //MessageBox.Show(oldFilePath);
            if (File.Exists(oldFilePath))
            {
                File.Copy(oldFilePath, newFilePath);
                return oldFilePath;
            }

            // checks '../images/fileName'
            oldFilePath = melxDirectory + "\\..\\" + type + "s\\" + oldFileName;
            //MessageBox.Show(oldFilePath);
            if (File.Exists(oldFilePath))
            {
                File.Copy(oldFilePath, newFilePath);
                return oldFilePath;
            }

            // give up
            MessageBox.Show("\'" + oldFileName + "\' could not be found. If it can't be found, the package will be abandoned");
            while (true)
            {
                string fileExt = Path.GetExtension(oldFileName).ToLower();
                string[] imageExt = new string[] { ".bmp", ".jpg", ".jpeg", ".gif", ".png" };

                OpenFileDialog ofd = new OpenFileDialog();
                if (fileExt == ".maz" || fileExt == ".mazx")
                    ofd.Filter = "Maze Files (*.maz;*.mazx)|*.maz;*.mazx";
                else if (imageExt.Contains(fileExt))
                    ofd.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.gif;*.png)|*.bmp;*.jpg;*.jpeg;*.gif;*.png";
                else if (fileExt == ".wav" || fileExt == ".mp3")
                    ofd.Filter = "Audio Files (*.wav;*.mp3)|*.wav;*.mp3";
                else if (fileExt == ".obj")
                    ofd.Filter = "Model Files (*.obj)|*.obj";
                ofd.Title = "Finding/Replacing " + oldFileName;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string newFileName = ofd.FileName.Substring(ofd.FileName.LastIndexOf("\\") + 1);
                    if (newFileName == "")
                        newFileName = ofd.FileName;

                    if (newFileName.Split('.')[0] != oldFileName.Split('.')[0])
                    {
                        switch (MessageBox.Show("The new file \'" + newFileName + "\' is different from \'" + oldFileName + "\'. Are you sure you want to use this file? Press \'No\' to search again! Press \'Cancel\' to abandon package!", "Are you sure?", MessageBoxButtons.YesNoCancel))
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
                    replaceOrder.Add(new string[] { type, oldFileName, newFileName, ofd.FileName });
                    return ofd.FileName;
                }

                return "Maze List package failed.";
            }
        }

        void ReplaceFiles()
        {
            if (replaceOrder.Count != 0)
            {
                foreach (ListItem listItem in MazeList)
                {
                    switch (listItem.Type)
                    {
                        case ItemType.Maze:
                            MazeListItem maze = (MazeListItem)listItem;
                            foreach (string[] replaceInfo in replaceOrder)
                            {
                                if (replaceInfo[0] == "maze" && maze.MazeFile == replaceInfo[1])
                                {
                                    mazeFilePaths.Remove(replaceInfo[1]);
                                    maze.MazeFile = replaceInfo[2];
                                    if (replaceInfo[2] != "")
                                        mazeFilePaths[replaceInfo[2]] = replaceInfo[3];
                                }
                            }
                            break;

                        case ItemType.Text:
                            TextListItem text = (TextListItem)listItem;
                            foreach (string[] replaceInfo in replaceOrder)
                            {
                                if (replaceInfo[0] == "audio" && text.AudioFile == replaceInfo[1])
                                {
                                    audioFilePaths.Remove(replaceInfo[1]);
                                    text.AudioFile = replaceInfo[2];
                                    if (replaceInfo[2] != "")
                                        audioFilePaths[replaceInfo[2]] = replaceInfo[3];
                                }
                            }
                            break;

                        case ItemType.Image:
                            ImageListItem image = (ImageListItem)listItem;
                            foreach (string[] replaceInfo in replaceOrder)
                            {
                                if (replaceInfo[0] == "image" && image.ImageFile == replaceInfo[1])
                                {
                                    imageFilePaths.Remove(replaceInfo[1]);
                                    image.ImageFile = replaceInfo[2];
                                    if (replaceInfo[2] != "")
                                        imageFilePaths[replaceInfo[2]] = replaceInfo[3];
                                }
                                if (replaceInfo[0] == "audio" && image.AudioFile == replaceInfo[1])
                                {
                                    audioFilePaths.Remove(replaceInfo[1]);
                                    image.AudioFile = replaceInfo[2];
                                    if (replaceInfo[2] != "")
                                        audioFilePaths[replaceInfo[2]] = replaceInfo[3];
                                }
                            }
                            break;

                        case ItemType.MultipleChoice:
                            MultipleChoiceListItem multipleChoice = (MultipleChoiceListItem)listItem;
                            foreach (string[] replaceInfo in replaceOrder)
                            {
                                if (replaceInfo[0] == "audio" && multipleChoice.AudioFile == replaceInfo[1])
                                {
                                    audioFilePaths.Remove(replaceInfo[1]);
                                    multipleChoice.AudioFile = replaceInfo[2];
                                    if (replaceInfo[2] != "")
                                        audioFilePaths[replaceInfo[2]] = replaceInfo[3];
                                }
                            }
                            break;

                        case ItemType.RecordAudio:
                            RecordAudioListItem recordAudio = (RecordAudioListItem)listItem;
                            foreach (string[] replaceInfo in replaceOrder)
                            {
                                if (replaceInfo[0] == "image" && recordAudio.ImageFile == replaceInfo[1])
                                {
                                    imageFilePaths.Remove(replaceInfo[1]);
                                    recordAudio.ImageFile = replaceInfo[2];
                                    if (replaceInfo[2] != "")
                                        imageFilePaths[replaceInfo[2]] = replaceInfo[3];
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

        private bool WriteToMelx(string mLFilePath)
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

            foreach (ListItem item in MazeList)
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
                                filePath = MakeRelativePath(mLFilePath, filePath);
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

                        mz.SetAttribute("InitialPoints", maze.InitialPoints.ToString());
                        mz.SetAttribute("InitialPointsMode", maze.InitialPointsMode.ToString());

                        mz.SetAttribute("StartTime", maze.StartTime.ToString());
                        mz.SetAttribute("InitialTimeMode", maze.InitialTimeMode.ToString());
                        mz.SetAttribute("Timeout", maze.Timeout.ToString());

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
                                filePath = MakeRelativePath(mLFilePath, filePath);
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
                        //mz.SetAttribute("AudioBehavior", text.AudioBehavior);
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
                                filePath = MakeRelativePath(mLFilePath, filePath);
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
                                filePath = MakeRelativePath(mLFilePath, filePath);
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
                        //mz.SetAttribute("AudioBehavior", image.AudioBehavior);
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
                                filePath = MakeRelativePath(mLFilePath, filePath);
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
                        //mz.SetAttribute("AudioBehavior", multipleChoice.AudioBehavior);
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
                                filePath = MakeRelativePath(mLFilePath, filePath);
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
                        
                        mz.SetAttribute("WaitForComplete", command.WaitForComplete.ToString());
                        mz.SetAttribute("HideWindow", command.HideCommand.ToString());
                        mz.SetAttribute("Program", command.Command);
                        mz.InnerText = command.CommandParameters;
                   
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
            melx.Save(mLFilePath);

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

        private bool WriteToFile(string mLFilePath)
        {
            string fileExt = Path.GetExtension(mLFilePath).ToLower();

            switch (fileExt)
            {
                case ".xml":
                    WriteToMelx(mLFilePath);
                    break;

                case ".melx":
                    WriteToMelx(mLFilePath);
                    break;

                case ".mel":
                    WriteToMel(mLFilePath);
                    break;

                default:
                    WriteToMelx(mLFilePath);
                    break;
            }

            madeChanges = false;
            return true;
        }

        private bool WriteToMel(string mLFilePath)
        {
            StreamWriter mel = new StreamWriter(mLFilePath);

            if (mel == null)
            {
                return false;
            }

            mel.WriteLine("Maze List File 1.2");

            foreach (ListItem listItem in MazeList)
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

        private bool ReadFromMelx(string mLFilePath)
        {
            this.mLFilePath = mLFilePath;
            statusLabel.Text = mLFilePath;

            XmlDocument melx = new XmlDocument();
            melx.Load(mLFilePath);
            XmlElement mzLs = melx.DocumentElement;

            Dictionary<string, string> mazeLibraryItems = new Dictionary<string, string>();
            Dictionary<string, string> imageLibraryItems = new Dictionary<string, string>();
            Dictionary<string, string> audioLibraryItems = new Dictionary<string, string>();

            foreach (XmlElement mz in mzLs)
            {
                switch (mz.Name)
                {
                    case "MazeListOptions":
                        if (MazeList.Count != 0)
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

                        MazeList.Add(mazeListOptions);
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
                                        //InitialPoints = int.Parse(listItem.GetAttribute("InitialPoints"));
                                        //InitialPointsMode = listItem.GetAttribute("InitialPointsMode"),
                                        //StartTime = listItem.GetAttribute("StartTime"),
                                        //Timeout = double.TryParse(listItem.GetAttribute("Timeout"),0);
                                        Timeout = double.Parse(listItem.GetAttribute("Timeout"))
                                    };

                                    MazeList.Add(maze);
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
                                        //AudioBehavior = listItem.GetAttribute("AudioBehavior"),
                                        EndBehavior = listItem.GetAttribute("EndBehavior"),
                                        FontSize = Convert.ToInt32(listItem.GetAttribute("FontSize"))
                                    };

                                    MazeList.Add(text);
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
                                        //AudioBehavior = listItem.GetAttribute("AudioBehavior"),
                                        EndBehavior = listItem.GetAttribute("EndBehavior")
                                    };

                                    MazeList.Add(image);
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
                                        //AudioBehavior = listItem.GetAttribute("AudioBehavior"),
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

                                    MazeList.Add(multipleChoice);
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

                                    MazeList.Add(recordAudio);
                                    break;

                                case "Command":
                                    CommandListItem command = new CommandListItem
                                    {
                                        Command = listItem.GetAttribute("Program"),
                                        CommandParameters = listItem.InnerText,

                                        WaitForComplete = bool.Parse(listItem.GetAttribute("WaitForComplete")),
                                        HideCommand = bool.Parse(listItem.GetAttribute("HideWindow")),
                                    };

                                    MazeList.Add(command);
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

        public bool ReadFromFile(string mLFilePath, bool append)
        {
            if (!append)
            {
                ClearMazeList();
            }

            string fileExt = Path.GetExtension(mLFilePath).ToLower();

            switch (fileExt)
            {
                case ".xml":
                    return ReadFromMelx(mLFilePath);

                case ".melx":
                    return ReadFromMelx(mLFilePath);

                case ".mel":
                    return ReadFromMel(mLFilePath);

                default:
                    MessageBox.Show("Not mel or melx file!", "MazeMaker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
            }
        }

        public bool ReadFromMel(string mLFilePath)
        {
            StreamReader mel = new StreamReader(mLFilePath);
            if (mel == null)
            {
                return false;
            }

            string buffer = mel.ReadLine();
            if (!buffer.Contains("Maze List File"))
            {
                MessageBox.Show("Not a Maze List File or corrupted!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.mLFilePath = mLFilePath;
            statusLabel.Text = mLFilePath;
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

                            MazeList.Add(maze);
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

                            MazeList.Add(text);
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

                            MazeList.Add(image);
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

                            MazeList.Add(multipleChoice);
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
            TreeNode targetNode = treeView.GetNodeAt(treeView.PointToClient(new Point(e.X, e.Y)));
            int targetIndex = treeView.Nodes[1].Nodes.IndexOf(targetNode) + 1;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string filePath in filePaths)
                {
                    if (targetNode == null)
                        AddFile(MazeList.Count, filePath);
                    else
                        AddFile(targetIndex + 1, filePath);
                }
            }
            else if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
                int draggedIndex = treeView.Nodes[1].Nodes.IndexOf(draggedNode) + 1;

                if (!treeView.Nodes.Contains(draggedNode) && targetNode != null) // if node isn't maze list options or list item
                {
                    if (targetIndex <= draggedIndex)
                    {
                        if (targetIndex == 0) // maze list options or list item index
                            MazeList.Insert(targetIndex + 1, MazeList[draggedIndex]);
                        else
                            MazeList.Insert(targetIndex, MazeList[draggedIndex]);
                        MazeList.RemoveAt(draggedIndex + 1);
                    }
                    else
                    {
                        MazeList.Insert(targetIndex + 1, MazeList[draggedIndex]);
                        MazeList.RemoveAt(draggedIndex);
                    }

                    UpdateMazeList();
                }
            }
        }

        void AddFile(int insertIndex, string filePath)
        {
            string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
            if (fileName == "")
                fileName = filePath;

            string fileExt = Path.GetExtension(filePath).ToLower();
            string[] imageExt = new string[] { ".bmp", ".jpg", ".jpeg", ".gif", ".png" };

            if (fileExt == ".maz" || fileExt == ".mazx")
            {
                MazeListItem maze = new MazeListItem
                {
                    MazeFile = fileName,
                };
                mazeFilePaths[fileName] = filePath;
                MazeList.Insert(insertIndex, maze);
            }
            else if (imageExt.Contains(fileExt))
            {
                ImageListItem image = new ImageListItem
                {
                    ImageFile = fileName,
                };
                imageFilePaths[fileName] = filePath;
                MazeList.Insert(insertIndex, image);
            }
            else if (fileExt == ".wav" || fileExt == ".mp3")
            {
                ImageListItem image = new ImageListItem
                {
                    AudioFile = fileName,
                };
                audioFilePaths[fileName] = filePath;
                MazeList.Insert(insertIndex, image);
            }
            else if (fileExt == ".exe")
            {
                CommandListItem command = new CommandListItem
                {
                    Text = filePath,
                };
                MazeList.Insert(insertIndex, command);
            }

            UpdateMazeList();
        }

        private void Cut(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null && treeView.SelectedNode != treeView.Nodes[0] && treeView.SelectedNode != treeView.Nodes[1])
            {
                Copy();
                MazeList.RemoveAt(selectedIndex);
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
                MazeList.Insert(selectedIndex + 1, copiedListItem);
            }
            else
            {
                MazeList.Add(copiedListItem);
            }

            UpdateMazeList();
        }

        ListItem copiedListItem;
        void Copy()
        {
            copiedListItem = MazeList[selectedIndex];
        }

        private void toolStripLabel_Click(object sender, EventArgs e)
        {

        }
    }
}