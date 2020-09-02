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
    public partial class MazeMakerCollectionEditor : Form
    {
        List<Texture> curListT;
        List<Texture> curListT2;

        List<Model> curListM;
        List<Model> curListM2;
        
        List<Audio> curListA;
        List<Audio> curListA2;

        List<string[]> replaceOrder = new List<string[]>();

        public string GetTexture()
        {
            ShowDialog();

            if (listBox.SelectedItem != null)
            {
                Texture texture = (Texture)listBox.SelectedItem;
                return texture.FilePath;
            }

            return "";
        }

        public List<Texture> GetTextures()
        {
            return curListT2;
        }

        public string GetAudio()
        {
            ShowDialog();

            if (listBox.SelectedItem != null)
            {
                Audio audio = (Audio)listBox.SelectedItem;
                return audio.filePath;
            }

            return "";
        }

        public List<Audio> GetAudios()
        {
            return curListA2;
        }

        public string GetModel()
        {
            ShowDialog();

            if (listBox.SelectedItem != null)
            {
                Model model = (Model)listBox.SelectedItem;
                return model.filePath;
            }

            return "";
        }

        public List<Model> GetModels()
        {
            return curListM2;
        }

        public List<string[]> GetReplaceOrder()
        {
            return replaceOrder;
        }

        public MazeMakerCollectionEditor(ref List<Texture> inp)
        {
            InitializeComponent();
            curListT = inp;
            curListT2 = new List<Texture>();

            foreach (Texture t in curListT)
            {
                curListT2.Add(t);
            }

            Text = "Texture/Image Collection Editor";
            Icon = Properties.Resources.ImageCollectionIcon;
        }

        public MazeMakerCollectionEditor(ref List<Model> inp)
        {
            InitializeComponent();
            curListM = inp;
            curListM2 = new List<Model>();

            foreach (Model t in curListM)
            {
                curListM2.Add(t);
            }

            Text = "Model/Object Collection Editor";
            Icon = Properties.Resources.ModelCollectionIcon;
        }

        public MazeMakerCollectionEditor(ref List<Audio> inp)
        {
            InitializeComponent();
            curListA = inp;
            curListA2 = new List<Audio>();

            foreach (Audio t in curListA)
            {
                curListA2.Add(t);
            }
            
            Text = "Audio/Sound Collection Editor";
            Icon = Properties.Resources.AudioCollectionIcon;
        }

        private void RefreshList()
        {
            listBox.Items.Clear();
            propertyGrid.SelectedObject = null;
            if (curListT2 != null)
            {
                for (int i = 0; i < curListT2.Count; i++)
                {
                    listBox.Items.Add(curListT2[i]);
                }
               // this.Text = curListT2.Count + " images in the Texture Collection";
            }
            if (curListM2 != null)
            {
                for (int i = 0; i < curListM2.Count; i++)
                {
                    listBox.Items.Add(curListM2[i]);
                }
               // this.Text = curListM2.Count + " models in the Model Collection";
            }
            if (curListA2 != null)
            {
                for (int i = 0; i < curListA2.Count; i++)
                {
                    listBox.Items.Add(curListA2[i]);
                }
                // this.Text = curListT2.Count + " images in the Texture Collection";
            }
        }

        private void MazeMakerCollectionEditor_Load(object sender, EventArgs e)
        {
            RefreshList();
            if(curListM2 != null || curListA2==null)
            {
                //checkBoxCopy.Checked = false;
                //checkBoxCopy.Enabled = false;
            }
            //this.Text = "Collection Editor";
        }

        private void Close(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Add(object sender, EventArgs e)
        {
            if (curListT2 != null)
                AddTexture();
            else if (curListM2 != null)
                AddModel();
            else if (curListA2 != null)
                AddAudio();
        }

        private void Remove(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex >= 0)
            {
                if (curListT2 != null)
                {
                    replaceOrder.Add(new string[] { "image", curListT2[listBox.SelectedIndex].Name, "", ""});

                    curListT2.RemoveAt(listBox.SelectedIndex);
                    listBox.Items.Remove(listBox.SelectedItem);
                    propertyGrid.SelectedObject = null;
                    pictureBox.Image = null;
                }
                if (curListM2 != null)
                {
                    replaceOrder.Add(new string[] { "model", curListM2[listBox.SelectedIndex].Name, "", "" });

                    curListM2.RemoveAt(listBox.SelectedIndex);
                    listBox.Items.Remove(listBox.SelectedItem);
                    propertyGrid.SelectedObject = null;
                    pictureBox.Image = null;
                }
                if (curListA2 != null)
                {
                    replaceOrder.Add(new string[] { "audio", curListA2[listBox.SelectedIndex].Name, "", "" });

                    curListA2.RemoveAt(listBox.SelectedIndex);
                    listBox.Items.Remove(listBox.SelectedItem);
                    propertyGrid.SelectedObject = null;
                    pictureBox.Image = null;
                }
            }
        }

        private void Replace(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex >= 0)
            {
                OpenFileDialog ofd = new OpenFileDialog();

                if (curListT2 != null)
                {
                    ofd.Filter = "Image File(*.bmp, *.jpg, *.jpeg, *.gif, *png) | *.bmp; *.jpg; *.jpeg; *.gif; *.png";

                    string oldFileName = curListT2[listBox.SelectedIndex].Name;
                    curListT2.RemoveAt(listBox.SelectedIndex);

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string directory = Path.GetDirectoryName(ofd.FileName);
                        string fileName = Path.GetFileName(ofd.FileName);
                        Texture texture = new Texture(directory, fileName, 0);

                        if (texture.Image != null)
                        {
                            curListT2.Add(texture);
                            RefreshList();

                            listBox.SelectedIndex = listBox.Items.Count - 1;
                            replaceOrder.Add(new string[] { "image", oldFileName, fileName, ofd.FileName });
                        }
                    }

                    propertyGrid.SelectedObject = null;
                    pictureBox.Image = null;
                }
                if (curListM2 != null)
                {
                    ofd.Filter = "Model File (*.obj)|*.obj";

                    string oldFileName = curListM2[listBox.SelectedIndex].Name;
                    curListM2.RemoveAt(listBox.SelectedIndex);

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string directory = Path.GetDirectoryName(ofd.FileName);
                        string fileName = Path.GetFileName(ofd.FileName);
                        Model model = new Model(directory, fileName, 0);

                        if (model.Name != null)
                        {
                            curListM2.Add(model);
                            RefreshList();

                            listBox.SelectedIndex = listBox.Items.Count - 1;
                            replaceOrder.Add(new string[] { "model", oldFileName, fileName, ofd.FileName });
                        }
                    }

                    propertyGrid.SelectedObject = null;
                    pictureBox.Image = null;
                }
                if (curListA2 != null)
                {
                    ofd.Filter = "Audio File (*.wav,*.mp3)| *.wav;*.mp3";

                    string oldFileName = curListA2[listBox.SelectedIndex].Name;
                    curListA2.RemoveAt(listBox.SelectedIndex);

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string directory = Path.GetDirectoryName(ofd.FileName);
                        string fileName = Path.GetFileName(ofd.FileName);
                        Audio audio = new Audio(directory, fileName, 0);

                        if (audio.Name != null)
                        {
                            curListA2.Add(audio);
                            RefreshList();

                            listBox.SelectedIndex = listBox.Items.Count - 1;
                            replaceOrder.Add(new string[] { "audio", oldFileName, fileName, ofd.FileName });
                        }
                    }

                    propertyGrid.SelectedObject = null;
                    pictureBox.Image = null;
                }
            }
        }

        System.Media.SoundPlayer sp;
        WMPLib.WindowsMediaPlayer wmp = new WMPLib.WindowsMediaPlayer();
        string audioPlayer = "";
        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex >= 0)
            {
                propertyGrid.SelectedObject = listBox.SelectedItem;
                if (curListT2 != null)
                {
                    pictureBox.Image = ((Texture)listBox.SelectedItem).Image;
                }
                else if (curListM2 != null)
                {
                    pictureBox.Image = ((Model)listBox.SelectedItem).Image;
                }
                else if (curListA2 != null)
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
        private void pictureBox_Click(object sender, EventArgs e)
        {
            if (curListA2 != null && propertyGrid.SelectedObject != null)
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

        private void MazeMakerCollectionEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopAudio();
        }

        private void AddTexture()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image File (*.bmp,*.jpg,*.jpeg,*.gif,*png)|*.bmp;*.jpg;*.jpeg;*.gif;*.png";
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in ofd.FileNames)
                {
                    string directory = Path.GetDirectoryName(filePath);
                    string fileName = Path.GetFileName(filePath);
                    Texture texture = new Texture(directory, fileName, 0);
                    if (texture.Image != null)
                    {
                        curListT2.Add(texture);
                        RefreshList();
                        listBox.SelectedIndex = listBox.Items.Count - 1;
                    }
                }
            }

            //try
            //{
            //    Texture a = new Texture();

            //    if (a.Image != null)
            //    {
            //        curListT2.Add(a);
            //        RefreshList();
            //        listBoxCollection.SelectedIndex = listBoxCollection.Items.Count - 1;
            //        //if (checkBoxCopy.Checked)
            //        //{
            //        //    try
            //        //    {
            //        //        Tools.CreateMissingFolder(Settings.userLibraryFolder);
            //        //        //copy to the user library...
            //        //        if (Tools.FileExists(Settings.userLibraryFolder + "\\" + a.name) == false && Tools.FileExists(Settings.standardLibraryFolder + "\\" + a.name) == false)
            //        //        {
            //        //            a.Image.Save(Settings.userLibraryFolder + "\\" + a.name);
            //        //        }
            //        //    }
            //        //    catch (System.Exception ex)
            //        //    {
            //        //        MessageBox.Show("Couldn't copy the selected file to User Library\n\n" + ex.Message, "MazeMaker", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        //    }
            //        //}
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //}
        }

        private void AddTexture(string dir)
        {
            try
            {
                Texture a = new Texture(dir);

                if (a.Image != null)
                {
                    curListT2.Add(a);
                    RefreshList();
                    listBox.SelectedIndex = listBox.Items.Count - 1;
                    //if (checkBoxCopy.Checked)
                    //{
                    //    try
                    //    {
                    //        Tools.CreateMissingFolder(Settings.userLibraryFolder);
                    //        //copy to the user library...
                    //        if (Tools.FileExists(Settings.userLibraryFolder + "\\" + a.name) == false && Tools.FileExists(Settings.standardLibraryFolder + "\\" + a.name) == false)
                    //        {
                    //            a.Image.Save(Settings.userLibraryFolder + "\\" + a.name);
                    //        }
                    //    }
                    //    catch (System.Exception ex)
                    //    {
                    //        MessageBox.Show("Couldn't copy the selected file to User Library\n\n" + ex.Message, "MazeMaker", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    }
                    //}
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void AddModel()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Model File (*.obj)|*.obj";
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in ofd.FileNames)
                {
                    string directory = Path.GetDirectoryName(filePath);
                    string fileName = Path.GetFileName(filePath);
                    Model model = new Model(directory, fileName, 0);
                    if (model.Name != null)
                    {
                        curListM2.Add(model);
                        RefreshList();
                        listBox.SelectedIndex = listBox.Items.Count - 1;
                    }
                }
            }

            //try
            //{
            //    Model a = new Model();
            //    if (a.Name != null)
            //    {
            //        curListM2.Add(a);
            //        RefreshList();
            //        listBoxCollection.SelectedIndex = listBoxCollection.Items.Count - 1;
            //    }
            //}
            //catch
            //{
            //}
        }

        private void AddModel(string dir)
        {
            try
            {
                Model a = new Model(dir);
                if (a.Name != null)
                {
                    curListM2.Add(a);
                    RefreshList();
                    listBox.SelectedIndex = listBox.Items.Count - 1;
                }
            }
            catch
            {
            }
        }

        private void AddAudio()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Audio File (*.wav,*.mp3)| *.wav;*.mp3";
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in ofd.FileNames)
                {
                    string directory = Path.GetDirectoryName(filePath);
                    string fileName = Path.GetFileName(filePath);
                    Audio audio = new Audio(directory, fileName, 0);
                    if (audio.Name != null)
                    {
                        curListA2.Add(audio);
                        RefreshList();
                        listBox.SelectedIndex = listBox.Items.Count - 1;
                    }
                }
            }

            //try
            //{
            //    Audio a = new Audio();

            //    //if (a.Image != null)
            //    if (a.Name != null)
            //    {
            //        curListA2.Add(a);
            //        RefreshList();
            //        listBoxCollection.SelectedIndex = listBoxCollection.Items.Count - 1;
            //        //if (checkBoxCopy.Checked)
            //        //{
            //            //try
            //            //{
            //            //    //Tools.CreateMissingFolder(Settings.userLibraryFolder);
            //            //    //copy to the user library...
            //            //    //if (Tools.FileExists(Settings.userLibraryFolder + "\\" + a.name) == false && Tools.FileExists(Settings.standardLibraryFolder + "\\" + a.name) == false)
            //            //    //{
            //            //    //    //a.Image.Save(Settings.userLibraryFolder + "\\" + a.name);
            //            //    //}
            //            //}
            //            //catch (System.Exception ex)
            //            //{
            //            //    MessageBox.Show("Couldn't copy the selected file to User Library\n\n" + ex.Message, "MazeMaker", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            //}
            //        //}
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //}
        }

        private void AddAudio(string dir)
        {
            try
            {
                Audio a = new Audio(dir);

                if (a.Name != null)
                {
                    curListA2.Add(a);
                    RefreshList();
                    listBox.SelectedIndex = listBox.Items.Count - 1;
                    //if (checkBoxCopy.Checked)
                    //{
                        //try
                        //{
                        //    Tools.CreateMissingFolder(Settings.userLibraryFolder);
                        //    //copy to the user library...
                        //    if (Tools.FileExists(Settings.userLibraryFolder + "\\" + a.name) == false && Tools.FileExists(Settings.standardLibraryFolder + "\\" + a.name) == false)
                        //    {
                        //        a.Image.Save(Settings.userLibraryFolder + "\\" + a.name);
                        //    }
                        //}
                        //catch (System.Exception ex)
                        //{
                        //    MessageBox.Show("Couldn't copy the selected file to User Library\n\n" + ex.Message, "MazeMaker", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //}
                    //}
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if(curListT2!=null)
            {
                curListT.Clear();
                foreach (Texture t in curListT2)
                {
                    curListT.Add(t);
                }
                
            }
            if (curListM2 != null)
            {
                curListM.Clear();
                foreach (Model t in curListM2)
                {
                    curListM.Add(t);
                }
            }
            if (curListA2 != null)
            {
                curListA.Clear();
                foreach (Audio t in curListA2)
                {
                    curListA.Add(t);
                }

            }
            this.Close();
        }

        private void buttonUserLibrary_Click(object sender, EventArgs e)
        {
            try
            {
                Tools.CreateMissingFolder(Settings.userLibraryFolder);
                //open user library
                Process.Start(Settings.userLibraryFolder);
            }
            catch//(Exception ex)
            {
                MessageBox.Show("Can not open library folder:\n\n" + Settings.userLibraryFolder,"MazeMaker");
            }
        }

        private void buttonStandardLibrary_Click(object sender, EventArgs e)
        {
            try
            {
                //standard library
                Process.Start(Settings.standardLibraryFolder);
            }
            catch //(Exception ex)
            {
                MessageBox.Show("Can not open library folder:\n\n" + Settings.standardLibraryFolder,"MazeMaker");
            }
        }

        private void buttonAddFromUserLib_Click(object sender, EventArgs e)
        {
            
            //add...
            if (curListT2 != null)
            {
                Tools.CreateMissingFolder(Settings.userLibraryFolder);
                AddTexture(Settings.userLibraryFolder);
            }
            else if (curListM2 != null)
            {
                Tools.CreateMissingFolder(Settings.userLibraryFolder + "\\Objs");
                AddModel(Settings.userLibraryFolder + "\\Objs");
            }
            else if (curListA2 != null)
            {
                Tools.CreateMissingFolder(Settings.userLibraryFolder);
                AddAudio(Settings.userLibraryFolder);
            }
        }

        private void buttonAddFromStandard_Click(object sender, EventArgs e)
        {
            //add...
            if (curListT2 != null)
                AddTexture(Settings.standardLibraryFolder);
            else if (curListM2 != null)
                AddModel(Settings.standardLibraryFolder + "\\Objs");
            else if (curListA2 != null)
                AddAudio(Settings.standardLibraryFolder);           
        }
    }
}

