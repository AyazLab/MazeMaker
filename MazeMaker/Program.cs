using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MazeLib;

namespace MazeMaker
{ 

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            
            try
            {
                SplashScreen.ShowSplashScreen<Splash>();
                SplashScreen.SetStatus("Initializing...");

                System.Threading.Thread.Sleep(2000);
            }
            catch
            {
            }
            if(args.Length==0)
                Application.Run(new Main());            
            else
                Application.Run(new Main(args[0]));  

        }
    }
}