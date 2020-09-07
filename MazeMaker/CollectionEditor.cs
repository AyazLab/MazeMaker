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

        private void RefreshList()
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
            RefreshList();
        }

        private void Add(object sender, EventArgs e)
        {
            if (mazeFilePaths != null)
                AddMaze();
            else if (textures != null)
                AddTexture();
            else if (audios != null)
                AddAudio();
            else if (models != null)
                AddModel();

            RefreshList();
            listBox.SelectedIndex = listBox.Items.Count - 1;
        }

        void AddMaze()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Maze File (*.maz)|*.maz",
                Multiselect = true,
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in ofd.FileNames)
                {
                    string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                    if (fileName == "")
                        fileName = filePath;
                    mazeFilePaths[fileName] = filePath;
                }
            }
        }

        private void AddTexture()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Image File (*.bmp,*.jpg,*.jpeg,*.gif,*png)|*.bmp;*.jpg;*.jpeg;*.gif;*.png",
                Multiselect = true,
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in ofd.FileNames)
                {
                    Texture texture = new Texture(Path.GetDirectoryName(filePath), Path.GetFileName(filePath), 0);
                    
                    for (int i = 0; i < textures.Count; i++)
                    {
                        if (textures[i].name == texture.name)
                        {
                            textures.RemoveAt(i);
                            break;
                        }
                    }
                    
                    textures.Add(texture);
                }
            }
        }

        private void AddAudio()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Audio File (*.wav,*.mp3)| *.wav;*.mp3",
                Multiselect = true,
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in ofd.FileNames)
                {
                    Audio audio = new Audio(Path.GetDirectoryName(filePath), Path.GetFileName(filePath), 0);

                    for (int i = 0; i < audios.Count; i++)
                    {
                        if (audios[i].name == audio.name)
                        {
                            audios.RemoveAt(i);
                            break;
                        }
                    }

                    audios.Add(audio);
                }
            }
        }

        private void AddModel()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Model File (*.obj)|*.obj",
                Multiselect = true,
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in ofd.FileNames)
                {
                    Model model = new Model(Path.GetDirectoryName(filePath), Path.GetFileName(filePath), 0);

                    for (int i = 0; i < models.Count; i++)
                    {
                        if (models[i].name == model.name)
                        {
                            models.RemoveAt(i);
                            break;
                        }
                    }

                    models.Add(model);
                }
            }
        }

        private void Remove(object sender, EventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                if (mazeFilePaths != null)
                {
                    replaceOrder.Add(new string[] { "maze", (string)listBox.SelectedItem, "", "" });
                    mazeFilePaths.Remove((string)listBox.SelectedItem);
                }
                else if (textures != null)
                {
                    replaceOrder.Add(new string[] { "image", textures[listBox.SelectedIndex].Name, "", "" });
                    textures.RemoveAt(listBox.SelectedIndex);
                }
                else if (audios != null)
                {
                    replaceOrder.Add(new string[] { "audio", audios[listBox.SelectedIndex].Name, "", "" });
                    audios.RemoveAt(listBox.SelectedIndex);
                }
                else if (models != null)
                {
                    replaceOrder.Add(new string[] { "model", models[listBox.SelectedIndex].Name, "", "" });
                    models.RemoveAt(listBox.SelectedIndex);
                }

                RefreshList();
                listBox.SelectedIndex = -1;
            }
        }

        private void Replace(object sender, EventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                OpenFileDialog ofd = new OpenFileDialog();

                if (mazeFilePaths != null)
                {
                    ofd.Filter = "Maze File (*.maz)|*.maz";
                    string oldFileName = (string)listBox.SelectedItem;
                    mazeFilePaths.Remove(oldFileName);

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string newFileName = ofd.FileName.Substring(ofd.FileName.LastIndexOf("\\") + 1);
                        if (newFileName == "")
                            newFileName = ofd.FileName;
                        mazeFilePaths[newFileName] = ofd.FileName;
                        replaceOrder.Add(new string[] { "maze", oldFileName, newFileName, ofd.FileName });
                    }
                }
                else if (textures != null)
                {
                    ofd.Filter = "Image File (*.bmp, *.jpg, *.jpeg, *.gif, *png)|*.bmp; *.jpg; *.jpeg; *.gif; *.png";

                    string oldFileName = textures[listBox.SelectedIndex].Name;
                    textures.RemoveAt(listBox.SelectedIndex);

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        textures.Add(new Texture(Path.GetDirectoryName(ofd.FileName), Path.GetFileName(ofd.FileName), 0));
                        replaceOrder.Add(new string[] { "image", oldFileName, Path.GetFileName(ofd.FileName), ofd.FileName });
                    }
                }
                else if (audios != null)
                {
                    ofd.Filter = "Audio File (*.wav,*.mp3)|*.wav;*.mp3";

                    string oldFileName = audios[listBox.SelectedIndex].Name;
                    audios.RemoveAt(listBox.SelectedIndex);

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        audios.Add(new Audio(Path.GetDirectoryName(ofd.FileName), Path.GetFileName(ofd.FileName), 0));
                        replaceOrder.Add(new string[] { "audio", oldFileName, Path.GetFileName(ofd.FileName), ofd.FileName });
                    }
                }
                else if (models != null)
                {
                    ofd.Filter = "Model File (*.obj)|*.obj";

                    string oldFileName = models[listBox.SelectedIndex].Name;
                    models.RemoveAt(listBox.SelectedIndex);

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        models.Add(new Model(Path.GetDirectoryName(ofd.FileName), Path.GetFileName(ofd.FileName), 0));
                        replaceOrder.Add(new string[] { "model", oldFileName, Path.GetFileName(ofd.FileName), ofd.FileName });
                    }
                }

                RefreshList();
                listBox.SelectedIndex = listBox.Items.Count - 1;
            }
        }

        System.Media.SoundPlayer sp;
        WMPLib.WindowsMediaPlayer wmp = new WMPLib.WindowsMediaPlayer();
        string audioPlayer = "";
        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                propertyGrid.SelectedObject = listBox.SelectedItem;

                if (textures != null)
                {
                    pictureBox.Image = ((Texture)listBox.SelectedItem).Image;
                }
                else if (audios != null)
                {
                    StopAudio();

                    string filePath = ((Audio)listBox.SelectedItem).filePath;
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
                pictureBox.Image = null;
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

        private void OKCancel(object sender, EventArgs e)
        {
            StopAudio();
            Close();
        }

        private void Close(object sender, FormClosedEventArgs e)
        {
            StopAudio();
        }

        public string GetMaze()
        {
            ShowDialog();

            if (listBox.SelectedItem != null)
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

            if (listBox.SelectedItem != null)
                return ((Texture)listBox.SelectedItem).name;
            return "";
        }

        public List<Texture> GetTextures()
        {
            return textures;
        }

        public string GetAudio()
        {
            ShowDialog();

            if (listBox.SelectedItem != null)
                return ((Audio)listBox.SelectedItem).name;
            return "";
        }

        public List<Audio> GetAudios()
        {
            return audios;
        }

        public string GetModel()
        {
            ShowDialog();

            if (listBox.SelectedItem != null)
                return ((Model)listBox.SelectedItem).name;
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
    }
}

