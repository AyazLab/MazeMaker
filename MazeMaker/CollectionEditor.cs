using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static System.Windows.Forms.ListBox;

namespace MazeMaker
{
    public partial class CollectionEditor : Form
    {
        readonly Dictionary<string, string> mazeFilePaths;
        readonly List<Texture> textures;
        readonly List<Audio> audios;
        readonly List<Model> models;

        readonly List<string[]> replaceOrder = new List<string[]>();

        public CollectionEditor(Dictionary<string, string> mazeFilePaths)
        {
            InitializeComponent();
            this.mazeFilePaths = mazeFilePaths;

            Text = "Maze Collection Editor";
            Icon = Properties.Resources.MazeCollectionIcon;
        }

        public CollectionEditor(List<Texture> textures)
        {
            InitializeComponent();
            this.textures = textures;

            Text = "Texture/Image Collection Editor";
            Icon = Properties.Resources.ImageCollectionIcon;
        }

        public CollectionEditor(List<Model> models)
        {
            InitializeComponent();
            this.models = models;

            Text = "Model/Object Collection Editor";
            Icon = Properties.Resources.ModelCollectionIcon;
        }

        public CollectionEditor(List<Audio> audios)
        {
            InitializeComponent();
            this.audios = audios;

            Text = "Audio/Sound Collection Editor";
            Icon = Properties.Resources.AudioCollectionIcon;
        }

        private void UpdateCollection()
        {
            listBox.Items.Clear();

            if (mazeFilePaths != null)
                foreach (string key in mazeFilePaths.Keys)
                    listBox.Items.Add(key);
            else if (textures != null)
                foreach (Texture texture in textures)
                    listBox.Items.Add(texture);
            else if (audios != null)
                foreach (Audio audio in audios)
                    listBox.Items.Add(audio);
            else if (models != null)
                foreach (Model model in models)
                    listBox.Items.Add(model);
        }

        private void MazeMakerCollectionEditor_Load(object sender, EventArgs e)
        {
            UpdateCollection();
            listBox.SelectionMode = SelectionMode.MultiExtended;
        }

        private void Add(object sender, EventArgs e)
        {
            if (mazeFilePaths != null)
                AddMaze(true, "Add Maze Files");
            else if (textures != null)
                AddTexture(true, "Add Image Files");
            else if (audios != null)
                AddAudio(true, "Add Audio Files");
            else if (models != null)
                AddModel(true, "Add Model Files");

            UpdateCollection();
            listBox.SelectedIndex = listBox.Items.Count - 1;
        }

        List<string> AddMaze(bool multiselect, string title)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Maze Files (*.maz;*.mazx)|*.maz;*.mazx",
                Multiselect = multiselect,
                Title = title,
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in ofd.FileNames)
                {
                    string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                    if (fileName == "")
                        fileName = filePath;
                    mazeFilePaths[fileName] = filePath;

                    if (!multiselect)
                        return new List<string> { fileName, filePath };
                }
            }
            return new List<string>();
        }

        List<string> AddTexture(bool multiselect, string title)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.gif;*png)|*.bmp;*.jpg;*.jpeg;*.gif;*.png",
                Multiselect = multiselect,
                Title = title,
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in ofd.FileNames)
                {
                    AddTexture(Path.GetDirectoryName(filePath), Path.GetFileName(filePath));

                    if (!multiselect)
                        return new List<string> { Path.GetFileName(filePath), filePath };
                }
            }
            return new List<string>();
        }

        void AddTexture(string directory, string fileName)
        {
            Texture texture = new Texture(directory, fileName, 0);
            for (int i = 0; i < textures.Count; i++)
            {
                if (textures[i].Name == texture.Name)
                {
                    textures.RemoveAt(i);
                    break;
                }
            }
            textures.Add(texture);
        }

        List<string> AddAudio(bool multiselect, string title)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Audio Files (*.wav;*.mp3)|*.wav;*.mp3",
                Multiselect = multiselect,
                Title = title,
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in ofd.FileNames)
                {
                    AddAudio(Path.GetDirectoryName(filePath), Path.GetFileName(filePath));

                    if (!multiselect)
                        return new List<string> { Path.GetFileName(filePath), filePath };
                }
            }
            return new List<string>();
        }

        void AddAudio(string directory, string fileName)
        {
            Audio audio = new Audio(directory, fileName, 0);
            for (int i = 0; i < audios.Count; i++)
            {
                if (audios[i].Name == audio.Name)
                {
                    audios.RemoveAt(i);
                    break;
                }
            }
            audios.Add(audio);
        }

        List<string> AddModel(bool multiselect, string title)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Model Files (*.obj)|*.obj",
                Multiselect = multiselect,
                Title = title,
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in ofd.FileNames)
                {
                    AddModel(Path.GetDirectoryName(filePath), Path.GetFileName(filePath));

                    if (!multiselect)
                        return new List<string> { Path.GetFileName(filePath), filePath };
                }
            }
            return new List<string>();
        }

        void AddModel(string directory, string fileName)
        {
            Model model = new Model(directory, fileName, 0);
            for (int i = 0; i < models.Count; i++)
            {
                if (models[i].Name == model.Name)
                {
                    models.RemoveAt(i);
                    break;
                }
            }
            models.Add(model);
        }

        private void Remove(object sender, EventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                if (listBox.SelectedItems.Count > 1)
                {
                    if (MessageBox.Show("You're about to remove multiple files. Are you sure?", "Removing multiple files detected!", MessageBoxButtons.OKCancel) != DialogResult.OK)
                        return;
                }

                for (int i = listBox.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    string selectedItem = listBox.Items[listBox.SelectedIndices[i]].ToString();
                    int selectedIndex = listBox.SelectedIndices[i];

                    if (mazeFilePaths != null)
                    {
                        replaceOrder.Add(new string[] { "maze", selectedItem, "", "" });
                        mazeFilePaths.Remove(selectedItem);
                    }
                    else if (textures != null)
                    {
                        replaceOrder.Add(new string[] { "image", textures[selectedIndex].Name, "", "" });
                        textures.RemoveAt(selectedIndex);
                    }
                    else if (audios != null)
                    {
                        replaceOrder.Add(new string[] { "audio", audios[selectedIndex].Name, "", "" });
                        audios.RemoveAt(selectedIndex);
                    }
                    else if (models != null)
                    {
                        replaceOrder.Add(new string[] { "model", models[selectedIndex].Name, "", "" });
                        models.RemoveAt(selectedIndex);
                    }
                }

                UpdateCollection();
                listBox.SelectedIndex = -1;
            }
        }

        private void Replace(object sender, EventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                if (listBox.SelectedItems.Count > 1)
                {
                    if (MessageBox.Show("You're about to replace multiple files. Are you sure?", "Replacing multiple files detected!", MessageBoxButtons.OKCancel) != DialogResult.OK)
                        return;
                }

                List<string> oldFileNames = new List<string>(); // grabbing old files to be replaced
                for (int i = listBox.SelectedIndices.Count - 1; i >= 0; i--)
                    oldFileNames.Add(listBox.Items[listBox.SelectedIndices[i]].ToString());

                string title = "Replacing "; // create a custom title for the open file dialog using the old file names
                foreach (string oldFileName in oldFileNames)
                    title += oldFileName + ", ";
                title = title.Substring(0, title.Length - 2);

                if (mazeFilePaths != null) // is maze collection editor?
                {
                    List<string> mazeFile = AddMaze(false, title); // returns file name & file path if successful
                    if (mazeFile.Count == 2)
                    {
                        foreach (string oldFileName in oldFileNames)
                        {
                            replaceOrder.Add(new string[] { "maze", oldFileName, mazeFile[0], mazeFile[1] });
                            List<string> deadKeys = new List<string>();
                            foreach (string key in mazeFilePaths.Keys)
                                if (key == oldFileName && key != mazeFile[0])
                                {
                                    deadKeys.Add(key);
                                    continue;
                                }
                            foreach (string key in deadKeys)
                                mazeFilePaths.Remove(key);
                        }
                    }
                }
                else if (textures != null)
                {
                    List<string> imageFile = AddTexture(false, title);
                    if (imageFile.Count == 2)
                    {
                        foreach (string oldFileName in oldFileNames)
                        {
                            replaceOrder.Add(new string[] { "image", oldFileName, imageFile[0], imageFile[1] });
                            for (int i = 0; i < textures.Count; i++)
                                if (textures[i].ToString() == oldFileName && textures[i].ToString() != imageFile[0])
                                {
                                    textures.RemoveAt(i);
                                    continue;
                                }
                        }
                    }
                }
                else if (audios != null)
                {
                    List<string> audioFile = AddAudio(false, title);
                    if (audioFile.Count == 2)
                    {
                        foreach (string oldFileName in oldFileNames)
                        {
                            replaceOrder.Add(new string[] { "audio", oldFileName, audioFile[0], audioFile[1] });
                            for (int i = 0; i < audios.Count; i++)
                                if (audios[i].ToString() == oldFileName && audios[i].ToString() != audioFile[0])
                                {
                                    audios.RemoveAt(i);
                                    continue;
                                }
                        }
                    }
                }
                else if (models != null)
                {
                    List<string> modelFile = AddModel(false, title);
                    if (modelFile.Count == 2)
                    {
                        foreach (string oldFileName in oldFileNames)
                        {
                            replaceOrder.Add(new string[] { "model", oldFileName, modelFile[0], modelFile[1] });
                            for (int i = 0; i < models.Count; i++)
                                if (models[i].ToString() == oldFileName && models[i].ToString() != modelFile[0])
                                {
                                    models.RemoveAt(i);
                                    continue;
                                }
                        }
                    }
                }

                UpdateCollection();
                listBox.SelectedIndex = listBox.Items.Count - 1;
            }
        }

        System.Media.SoundPlayer sp;
        readonly WMPLib.WindowsMediaPlayer wmp = new WMPLib.WindowsMediaPlayer();
        string audioPlayer = "";
        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedItems.Count == 1)
            {
                propertyGrid.SelectedObject = listBox.SelectedItem;
                if (mazeFilePaths != null)
                {
                    string mazeName = (string)listBox.SelectedItem;
                    string mazePath = mazeFilePaths[mazeName];
                    KeyValue maze = new KeyValue
                    {
                        FileName = mazeName,
                        FilePath = mazePath,
                    };
                    propertyGrid.SelectedObject = maze;
                }
                else if (textures != null)
                {
                    pictureBox.Image = ((Texture)listBox.SelectedItem).Image;
                }
                else if (audios != null)
                {
                    StopAudio();

                    string filePath = ((Audio)listBox.SelectedItem).FilePath;
                    audioPlayer = Path.GetExtension(filePath).ToLower();

                    switch (audioPlayer)
                    {
                        case ".wav":
                            sp = new System.Media.SoundPlayer(filePath);
                            break;

                        case ".mp3":
                            wmp.URL = filePath; // plays the audio, for some reason...
                            wmp.controls.stop();
                            break;

                        default:
                            break;
                    }
                }
                else if (models != null)
                {
                    pictureBox.Image = ((Model)listBox.SelectedItem).Image;
                }
            }
            else
            {
                propertyGrid.SelectedObject = null;
                pictureBox.Image = null;
            }
        }

        void StopAudio()
        {
            switch (audioPlayer)
            {
                case ".wav":
                    sp.Stop();
                    break;

                case ".mp3":
                    wmp.controls.stop();
                    break;

                default:
                    break;
            }

            pictureBox.Image = Properties.Resources.PlayAudioIcon;
            audioPlaying = false;
        }

        bool audioPlaying = false;
        private void PlayPause(object sender, EventArgs e)
        {
            if (audios != null && propertyGrid.SelectedObject != null)
            {
                if (audioPlaying)
                {
                    StopAudio();
                }
                else
                {
                    switch (audioPlayer)
                    {
                        case ".wav":
                            sp.PlayLooping();
                            break;

                        case ".mp3":
                            wmp.controls.play();
                            break;

                        default:
                            break;
                    }

                    pictureBox.Image = Properties.Resources.PauseAudioIcon;
                    audioPlaying = true;
                }
            }
        }

        private void SelectFile(object sender, EventArgs e)
        {
            StopAudio();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Cancel(object sender, EventArgs e)
        {
            StopAudio();
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void Close(object sender, FormClosedEventArgs e)
        {
            StopAudio();
        }

        public string GetMaze()
        {
            ShowDialog();

            if (listBox.SelectedItems.Count == 1 && DialogResult == DialogResult.OK)
                return (string)listBox.SelectedItem;
            return "";
        }

        public Dictionary<string, string> GetMazes()
        {
            return mazeFilePaths;
        }

        public string GetTexture()
        {
            ShowDialog();

            if (listBox.SelectedItems.Count == 1 && DialogResult == DialogResult.OK)
                return ((Texture)listBox.SelectedItem).Name;
            return "";
        }

        public List<Texture> GetTextures()
        {
            return textures;
        }

        public string GetAudio()
        {
            ShowDialog();

            if (listBox.SelectedItems.Count == 1 && DialogResult == DialogResult.OK)
                return ((Audio)listBox.SelectedItem).Name;
            return "";
        }

        public List<Audio> GetAudios()
        {
            return audios;
        }

        public string GetModel()
        {
            ShowDialog();

            if (listBox.SelectedItems.Count == 1 && DialogResult == DialogResult.OK)
                return ((Model)listBox.SelectedItem).Name;
            return "";
        }

        public List<Model> GetModels()
        {
            return models;
        }

        public List<string[]> GetReplaceOrder()
        {
            return replaceOrder;
        }

        private void listBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void listBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string filePath in filePaths)
                {
                    string fileExt = Path.GetExtension(filePath).ToLower();
                    string[] imageExt = new string[] { ".bmp", ".jpg", ".jpeg", ".gif", ".png" };
                    string directory = Path.GetDirectoryName(filePath);
                    string fileName = Path.GetFileName(filePath);

                    if (mazeFilePaths != null && (fileExt == ".maz" || fileExt == ".mazx"))
                        mazeFilePaths[fileName] = filePath;
                    else if (textures != null && imageExt.Contains(fileExt))
                        AddTexture(directory, fileName);
                    else if (audios != null && (fileExt == ".wav" || fileExt == ".mp3"))
                        AddAudio(directory, fileName);
                    else if (models != null && fileExt == ".obj")
                        AddModel(directory, fileName);
                }
                UpdateCollection();
            }
        }
    }

    public class KeyValue
    {
        public KeyValue()
        {
        }

        string fileName = "";
        [Category("File Information")]
        [Description("Name of the Maze")]
        [DisplayName("Name")]
        [ReadOnly(true)]
        public string FileName
        {
            get { return fileName; }
            set{ fileName = value; }
        }

        string filePath = "";
        [Category("File Information")]
        [Description("File Path of the Maze")]
        [DisplayName("File Path")]
        [ReadOnly(true)]
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public override string ToString()
        {
            return fileName;
        }
    }
}

