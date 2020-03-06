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
    public partial class ResizeMazeDialog : Form
    {
        public ResizeMazeDialog()
        {
            InitializeComponent();
        }

        public double horizontalResize = 100;
        public double verticalResize = 100;
        public double heightResize = 100;


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ResizeMazeDialog_Load(object sender, EventArgs e)
        {
            
        }

        private void textBox_horizontal_TextChanged(object sender, EventArgs e)
        {
            double origHoriz = horizontalResize;
            textBox_horizontal.Text = validateTextToNumber(textBox_horizontal.Text);
            double.TryParse(textBox_horizontal.Text, out horizontalResize);
            if (checkBox_aspectRatioLocked.Checked && !((origHoriz - horizontalResize) == 0)&&!Double.IsNaN(horizontalResize)&&horizontalResize>0)
            {
                verticalResize = verticalResize * (horizontalResize / origHoriz);
                textBox_vertical.Text = verticalResize.ToString();
            }

            if (verticalResize < 1 || horizontalResize < 1 || heightResize < 1 || Double.IsNaN(verticalResize) || Double.IsNaN(horizontalResize) || Double.IsNaN(heightResize))
                button_ok.Enabled = false;
            else
                button_ok.Enabled = true;
        }

        private void textBox_vertical_TextChanged(object sender, EventArgs e)
        {
            double origVertical = verticalResize;
            textBox_vertical.Text = validateTextToNumber(textBox_vertical.Text);
            double.TryParse(textBox_vertical.Text, out verticalResize);
            if (checkBox_aspectRatioLocked.Checked && !((verticalResize - origVertical) == 0) && !Double.IsNaN(verticalResize) && verticalResize > 0)
            {
                horizontalResize = horizontalResize * (verticalResize / origVertical);
                textBox_horizontal.Text = horizontalResize.ToString();
            }


            if (verticalResize < 1 || horizontalResize < 1 || heightResize < 1 || Double.IsNaN(verticalResize) || Double.IsNaN(horizontalResize) || Double.IsNaN(heightResize))
                button_ok.Enabled = false;
            else
                button_ok.Enabled = true;
        }

        private string validateTextToNumber(string textboxText)
        {
            string outString = "";
            bool decimalFlag = false;
            foreach (char c in textboxText)
            {
                if (!char.IsDigit(c) || (c == '.' && !decimalFlag))
                    if (!decimalFlag && c == '.')
                        decimalFlag = true;
                

                    outString = outString + c;

            }
            return outString;

        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            textBox_vertical.Text = validateTextToNumber(textBox_vertical.Text);

            

            double.TryParse(textBox_vertical.Text, out verticalResize);

            textBox_horizontal.Text = validateTextToNumber(textBox_horizontal.Text);
            double.TryParse(textBox_horizontal.Text, out horizontalResize);

            textBox_height.Text = validateTextToNumber(textBox_height.Text);
            double.TryParse(textBox_height.Text, out heightResize);

            if (verticalResize < 1 || horizontalResize < 1 || heightResize < 1|| Double.IsNaN(verticalResize) || Double.IsNaN(horizontalResize) || Double.IsNaN(heightResize))
            { 
                MessageBox.Show( "Invalid Resize Input", "Input Error");

                if (verticalResize < 1 || Double.IsNaN(verticalResize))
                {
                    verticalResize = 100;
                    textBox_vertical.Text = "100";

                }
                if (horizontalResize < 1 || Double.IsNaN(horizontalResize))
                {
                    horizontalResize = 100;
                    textBox_horizontal.Text = "100";
                }
                if (heightResize < 1 || Double.IsNaN(heightResize))
                {
                    heightResize = 100;
                    textBox_height.Text = "100";
                }
                
            }
            else
                this.Close();
        }

        private void textBox_height_TextChanged(object sender, EventArgs e)
        {
            double origHeight = heightResize;
            textBox_height.Text = validateTextToNumber(textBox_height.Text);
            double.TryParse(textBox_height.Text, out heightResize);
            //if (checkBox_aspectRatioLocked.Checked && !((heightResize - origHeight) == 0) && !Double.IsNaN(heightResize) && heightResize > 0)
            //{
            //    horizontalResize = horizontalResize * (heightResize / origHeight);
            //    textBox_horizontal.Text = horizontalResize.ToString();
            //}

            if (verticalResize < 1 || horizontalResize < 1 || heightResize < 1 || Double.IsNaN(verticalResize) || Double.IsNaN(horizontalResize) || Double.IsNaN(heightResize))
                button_ok.Enabled = false;
            else
                button_ok.Enabled = true;
        }
    }
}
