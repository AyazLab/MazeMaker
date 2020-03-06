using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MazeMaker
{
    public partial class QuickRunSettingsDialog : Form
    {
        QuickRunSettings theSettings;
        public QuickRunSettingsDialog(ref QuickRunSettings inp)
        {
            InitializeComponent();
            theSettings = inp;
        }

        private void QuickRunSettingsDialog_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("640x480");
            comboBox1.Items.Add("800x600");
            comboBox1.Items.Add("1024x768");
            comboBox1.Items.Add("1280x720");

            switch (theSettings.width)
            {
                case 640:
                    comboBox1.SelectedIndex = 0;
                    break;
                case 800:
                    comboBox1.SelectedIndex = 1;
                    break;
                case 1024:
                    comboBox1.SelectedIndex = 2;
                    break;
                case 1280:
                    comboBox1.SelectedIndex = 3;
                    break;
            }

            comboBox2.Items.Add("16 Bits");
            comboBox2.Items.Add("24 Bits");
            comboBox2.Items.Add("32 Bits");

            switch (theSettings.bits)
            {
                case 16:
                    comboBox2.SelectedIndex = 0;
                    break;
                case 24:
                    comboBox2.SelectedIndex = 1;
                    break;
                case 32:
                    comboBox2.SelectedIndex = 2;
                    break;
            }

            if (theSettings.fullscreen)
                checkBoxFullScreen.Checked = true;

            if (theSettings.lights)
                checkBoxLights.Checked = true;

            if (theSettings.shaders)
                checkBoxShaders.Checked = true;

            if (theSettings.devKeys)
                checkBoxDeveloperKeys.Checked = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ok
            theSettings.fullscreen = checkBoxFullScreen.Checked;
            theSettings.lights = checkBoxLights.Checked;
            theSettings.shaders = checkBoxShaders.Checked;
            theSettings.devKeys = checkBoxDeveloperKeys.Checked;

            
            
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    theSettings.width = 640;
                    theSettings.height = 480;
                    break;
                case 1:
                    theSettings.width = 800;
                    theSettings.height = 600;
                    break;
                case 2:
                    theSettings.width = 1024;
                    theSettings.height = 768;
                    break;
                case 3:
                    theSettings.width = 1280;
                    theSettings.height = 720;
                    break;
                default:
                    theSettings.width = 800;
                    theSettings.height = 600;
                    break;
            }

            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    theSettings.bits = 16;
                    break;
                case 1:
                    theSettings.bits = 24;
                    break;
                case 2:
                    theSettings.bits = 32;
                    break;
                default:
                    theSettings.bits = 32;
                    break;
            }

            CurrentSettings.quickRunSettings = theSettings;
            CurrentSettings.SaveSettings();
            this.Close();
        }
    }
}
