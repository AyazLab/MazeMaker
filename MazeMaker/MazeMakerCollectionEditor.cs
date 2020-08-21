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

        public string GetTexture()
        {
            ShowDialog();

            if (listBoxCollection.SelectedItem != null)
            {
                Texture texture = (Texture)listBoxCollection.SelectedItem;
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

            if (listBoxCollection.SelectedItem != null)
            {
                Audio audio = (Audio)listBoxCollection.SelectedItem;
                return audio.filePath;
            }

            return "";
        }

        public List<Audio> GetAudios()
        {
            return curListA2;
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
            this.Text = "Texture/Image Collection Editor";
        }

        public MazeMakerCollectionEditor(ref List<Model> inp)
        {
            InitializeComponent();
            curListM = inp;
            curListM2 = new List<Model>();
            foreach (Model t in curListM)
            {
                curListM2.Add(t);
            } this.Text = "Model/Object Collection Editor";
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
            this.Text = "Audio/Sound Collection Editor";
        }

        private void RefreshList()
        {
            listBoxCollection.Items.Clear();
            propertyGrid.SelectedObject = null;
            if (curListT2 != null)
            {
                for (int i = 0; i < curListT2.Count; i++)
                {
                    listBoxCollection.Items.Add(curListT2[i]);
                }
               // this.Text = curListT2.Count + " images in the Texture Collection";
            }
            if (curListM2 != null)
            {
                for (int i = 0; i < curListM2.Count; i++)
                {
                    listBoxCollection.Items.Add(curListM2[i]);
                }
               // this.Text = curListM2.Count + " models in the Model Collection";
            }
            if (curListA2 != null)
            {
                for (int i = 0; i < curListA2.Count; i++)
                {
                    listBoxCollection.Items.Add(curListA2[i]);
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

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //add...
            if (curListT2 != null)
                AddTexture();
            else if (curListM2 != null)
                AddModel();
            else if (curListA2 != null)
                AddAudio();

        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            //remove..
            if (listBoxCollection.SelectedIndex >= 0)
            {
                if (curListT2 != null)
                {
                    curListT2.RemoveAt(listBoxCollection.SelectedIndex);
                    listBoxCollection.Items.Remove(listBoxCollection.SelectedItem);
                    propertyGrid.SelectedObject = null;
                    pictureBox.Image = null;
                }
                if (curListM2 != null)
                {
                    curListM2.RemoveAt(listBoxCollection.SelectedIndex);
                    listBoxCollection.Items.Remove(listBoxCollection.SelectedItem);
                    propertyGrid.SelectedObject = null;
                    pictureBox.Image = null;
                }
                if (curListA2 != null)
                {
                    curListA2.RemoveAt(listBoxCollection.SelectedIndex);
                    listBoxCollection.Items.Remove(listBoxCollection.SelectedItem);
                    propertyGrid.SelectedObject = null;
                    pictureBox.Image = null;
                }
            }
        }

        System.Media.SoundPlayer sp;
        WMPLib.WindowsMediaPlayer wmp = new WMPLib.WindowsMediaPlayer();
        string audioPlayer = "";
        private void listBoxCollection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxCollection.SelectedIndex >= 0)
            {
                propertyGrid.SelectedObject = listBoxCollection.SelectedItem;
                if (curListT2 != null)
                {
                    pictureBox.Image = ((Texture)listBoxCollection.SelectedItem).Image;
                }
                else if (curListM2!= null)
                {
                    pictureBox.Image = ((Model)listBoxCollection.SelectedItem).Image;
                }
                else if (curListA2 != null)
                {
                    pictureBox.Image  = Properties.Resources.AudioPlayerIcon;

                    StopAudio();
                    string filePath = ((Audio)listBoxCollection.SelectedItem).filePath;
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
            try
            {
                Texture a = new Texture();

                if (a.Image != null)
                {
                    curListT2.Add(a);
                    RefreshList();
                    listBoxCollection.SelectedIndex = listBoxCollection.Items.Count - 1;
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

        private void AddTexture(string dir)
        {
            try
            {
                Texture a = new Texture(dir);

                if (a.Image != null)
                {
                    curListT2.Add(a);
                    RefreshList();
                    listBoxCollection.SelectedIndex = listBoxCollection.Items.Count - 1;
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
            try
            {
                Model a = new Model();
                if (a.Name != null)
                {
                    curListM2.Add(a);
                    RefreshList();
                    listBoxCollection.SelectedIndex = listBoxCollection.Items.Count - 1;
                }
            }
            catch
            {

            }
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
                    listBoxCollection.SelectedIndex = listBoxCollection.Items.Count - 1;
                }
            }
            catch
            {

            }
        }

        private void AddAudio()
        {
            try
            {
                Audio a = new Audio();

                //if (a.Image != null)
                if (a.Name != null)
                {
                    curListA2.Add(a);
                    RefreshList();
                    listBoxCollection.SelectedIndex = listBoxCollection.Items.Count - 1;
                    //if (checkBoxCopy.Checked)
                    //{
                        //try
                        //{
                        //    //Tools.CreateMissingFolder(Settings.userLibraryFolder);
                        //    //copy to the user library...
                        //    //if (Tools.FileExists(Settings.userLibraryFolder + "\\" + a.name) == false && Tools.FileExists(Settings.standardLibraryFolder + "\\" + a.name) == false)
                        //    //{
                        //    //    //a.Image.Save(Settings.userLibraryFolder + "\\" + a.name);
                        //    //}
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

        private void AddAudio(string dir)
        {
            try
            {
                Audio a = new Audio(dir);

                if (a.Name != null)
                {
                    curListA2.Add(a);
                    RefreshList();
                    listBoxCollection.SelectedIndex = listBoxCollection.Items.Count - 1;
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

        private void buttonOk_Click(object sender, EventArgs e)
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

