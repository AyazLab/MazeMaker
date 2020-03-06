using System.Drawing;
using System.Windows.Forms;
using MazeLib;

namespace MazeMaker
{
    public partial class Splash : SplashScreen
    {
        public Splash()
        {
            InitializeComponent();
            labelVersion.Text = "  v" + Application.ProductVersion;
            this.ProgressColor1 = Color.FromArgb(65, 77, 125); //Color.FromArgb(204, 204, 153);//Color.White;
            //this.ProgressColor2 = Color.FromArgb(0, 153, 0); //Color.Red;
            this.ProgressColor2 = Color.FromArgb(204, 153, 153); //Color.Red;

            labelVersion.BackColor = Color.FromArgb(65, 77, 125);// Color.FromArgb(204, 204, 153);
            lblStatus.BackColor = Color.FromArgb(65, 77, 125); //Color.FromArgb(204, 204, 153);
            lblTimeRemaining.BackColor = Color.FromArgb(65, 77, 125); // Color.FromArgb(204, 204, 153); 
        }
    }
}
