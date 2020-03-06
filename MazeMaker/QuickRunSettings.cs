using System;
using System.Collections.Generic;
using System.Text;

namespace MazeMaker
{
    public class QuickRunSettings
    {

        public bool fullscreen = false;

        public int bits = 32;

        public int width = 800;
        public int height = 600;

        public bool lights = true;
        public bool shaders = true;

        public Boolean devKeys = true;

        public string GetArgumentsString()
        {
            string ret="";
            if(fullscreen)
            {
                ret += "+f ";
            }
            else
            {
                ret += "-f ";
            }
            ret += "+r " + height + " " + width + " +b " + bits.ToString() + " ";
            if (lights)
            {
                ret += "+l ";
            }
            else
            {
                ret += "-l ";
            }
            if (shaders)
            {
                ret += "+s 0 ";
            }
            else
            {
                ret += "-s 0 ";
            }
            if(devKeys)
            {
                ret += "-d ";
            }
            else
            {
                ret += "-d ";
            }
            return ret;
        }

    }
}
