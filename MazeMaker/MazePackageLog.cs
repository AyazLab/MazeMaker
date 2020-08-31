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
    public partial class MazePackageLog : Form
    {
        public string statusMessage = "";
        public string logText = "";
        public bool successful = true;

        public MazePackageLog()
        {
            InitializeComponent();
        }
    }
}
