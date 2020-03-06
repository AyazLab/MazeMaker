using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MazeMaker
{
    public partial class GetAngle : Form
    {
        public GetAngle()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            this.Close();
        }
        
        public int Goster()
        {
            this.ShowDialog();
            return Int32.Parse(textBox1.Text.ToString());
        }
    }
}