using System;
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
        public string log = "";

        public PackageMessage()
        {
            InitializeComponent();
        }

        private void PackageMessage_Load(object sender, EventArgs e)
        {
            logTextBox.Text = log;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
