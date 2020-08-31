﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MazeMaker
{
    public partial class PackageMessage : Form
    {
        public string status = "";
        public string copiedFiles = "";

        public PackageMessage()
        {
            InitializeComponent();
        }

        private void PackageMessage_Load(object sender, EventArgs e)
        {
            statusLabel.Text = status;
            copiedFilesTextBox.Text = copiedFiles;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}