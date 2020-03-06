using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace MazeMaker
{
    public static class CurrentSettings
    {
        //public static string curDirectory;
        //public static List<string> curPaths = new List<string>(10);


        //public static string curAppDirectory;
        public static string curRegDirectory;  // Application.CommonAppDataPath

        private static string curSettingsFile = "\\settings.xml";
        public static string curSettingsPath = "";
        public static int themeIndex = 0;
        public static QuickRunSettings quickRunSettings=new QuickRunSettings();


        //public static bool showPropertiesPane = true;
        //public static bool showListPane = true;

        //public static Queue<string> previousFiles = new Queue<string>(10);

        public static List<string> previousMazeFiles = new List<string>(10);


        public static bool ReadSettings()
        {
            return ReadSettings(Application.CommonAppDataPath);
        }

        public static bool SaveSettings()
        {
            return SaveSettings(Application.CommonAppDataPath);
        }

        //inp: Application.CommonAppDataPath
        public static bool ReadSettings(string inp)
        {
            if (curSettingsPath == "")
            {
                curSettingsPath = inp.Substring(0, inp.LastIndexOf('\\'));
                curRegDirectory = inp + "\\";
            }
            try
            {
                String label;
                String fullSettingsPath = curSettingsPath + curSettingsFile;
                if (!System.IO.File.Exists(fullSettingsPath))
                    return false;

                XmlTextReader sw = new XmlTextReader(fullSettingsPath);
                while (sw.Read())
                {
                    if (sw.NodeType == XmlNodeType.Element)
                    {
                        label = sw.Name;
                        /*
                        if (label.Contains("Panes"))
                        {
                            sw.Read();
                            sw.ReadStartElement("properties");
                            if (sw.ReadString().CompareTo("False")==0)
                                showPropertiesPane = false;
                            else
                                showPropertiesPane = true;
                            sw.ReadEndElement();
                            sw.ReadStartElement("list");
                            if (sw.ReadString().CompareTo("False")==0)
                                showListPane = false;
                            else
                                showListPane = true;
                            sw.ReadEndElement();

                        }
                        //else if (label.Contains("Path"))
                        //{
                        //    sw.Read();
                        //    sw.ReadStartElement("name");
                        //    curPaths.Add(sw.ReadString());
                        //    sw.ReadEndElement();
                        //}
                        */
                        if (label.Contains("MazeFiles"))
                        {
                            sw.Read();
                            sw.ReadStartElement("Count");
                            int num = int.Parse(sw.ReadString());
                            sw.ReadEndElement();
                            if (num > 10) num = 10;
                            previousMazeFiles.Clear();
                            if (num > 0)
                            {
                                for (int i = 0; i < num; i++)
                                {
                                    sw.ReadStartElement("File");
                                    //previousFiles.Enqueue(sw.ReadString());
                                    previousMazeFiles.Add(sw.ReadString());
                                    sw.ReadEndElement();
                                }
                            }
                            //sw.ReadEndElement();
                        }
                        else if (label.Contains("Theme"))
                        {
                            sw.Read();
                            sw.ReadStartElement("style");
                            int.TryParse(sw.ReadString(), out themeIndex);

                            sw.ReadEndElement();
                        }

                        else if (label.Contains("QuickRun"))
                        {
                            sw.Read();
                            sw.ReadStartElement("height");
                            int.TryParse(sw.ReadString(), out quickRunSettings.height);
                            sw.ReadEndElement();
                            sw.ReadStartElement("width");
                            int.TryParse(sw.ReadString(), out quickRunSettings.width);
                            sw.ReadEndElement();
                            sw.ReadStartElement("fullscreen");
                            bool.TryParse(sw.ReadString(), out quickRunSettings.fullscreen);
                            sw.ReadEndElement();
                            sw.ReadStartElement("bits");
                            int.TryParse(sw.ReadString(), out quickRunSettings.bits);
                            sw.ReadEndElement();
                            sw.ReadStartElement("devKeys");
                            bool.TryParse(sw.ReadString(), out quickRunSettings.devKeys);
                            sw.ReadEndElement();
                            sw.ReadStartElement("shaders");
                            bool.TryParse(sw.ReadString(), out quickRunSettings.shaders);
                            sw.ReadEndElement();
                            sw.ReadStartElement("lights");
                            bool.TryParse(sw.ReadString(), out quickRunSettings.lights);
                            sw.ReadEndElement();

                            sw.ReadEndElement();
                        }

                    }
                }
                sw.Close();
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }

        //inp: Application.CommonAppDataPath
        public static bool SaveSettings(string inp)
        {
            if (curSettingsPath == "")
            {
                curSettingsPath = inp.Substring(0, inp.LastIndexOf('\\'));
                curRegDirectory = inp + "\\";
            }

            try
            {
                XmlTextWriter sw = new XmlTextWriter(curSettingsPath + curSettingsFile, System.Text.ASCIIEncoding.UTF8);
                sw.WriteStartDocument();
                sw.WriteStartElement("CT");

                sw.WriteStartElement("Theme");
                sw.WriteElementString("style", themeIndex.ToString());
                sw.WriteEndElement();

                sw.WriteStartElement("MazeFiles");
                sw.WriteElementString("Count", previousMazeFiles.Count.ToString());
                foreach (string s in previousMazeFiles)
                {
                    sw.WriteElementString("File", s);
                }
                sw.WriteEndElement();

                sw.WriteStartElement("QuickRun");
                sw.WriteElementString("height", quickRunSettings.height.ToString());
                sw.WriteElementString("width", quickRunSettings.width.ToString());
                sw.WriteElementString("fullscreen", quickRunSettings.fullscreen.ToString());
                sw.WriteElementString("bits", quickRunSettings.bits.ToString());
                sw.WriteElementString("devKeys", quickRunSettings.devKeys.ToString());
                sw.WriteElementString("shaders", quickRunSettings.shaders.ToString());
                sw.WriteElementString("lights", quickRunSettings.lights.ToString());
                sw.WriteEndElement();


                

                /*
                                sw.WriteStartElement("LogFiles");
                                sw.WriteElementString("Count", previousLogFiles.Count.ToString());
                                foreach (string s in previousLogFiles)
                                {
                                    sw.WriteElementString("File", s);
                                }
                                sw.WriteEndElement();


                                sw.WriteStartElement("Panes");
                                sw.WriteElementString("properties", showPropertiesPane.ToString());
                                sw.WriteElementString("list", showListPane.ToString());
                                sw.WriteEndElement();

                */
                //foreach (string s in curPaths)
                //{
                //    sw.WriteStartElement("Path");
                //    sw.WriteElementString("name", s);
                //    sw.WriteEndElement();
                //}
                sw.WriteEndElement();
                sw.WriteEndDocument();
                sw.Close();
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }


        //public static bool AddPreviousFile(string str)
        //{
        //    try
        //    {
        //        if (previousFiles.Count == 0)
        //        {
        //            previousFiles.Enqueue(str);
        //        }
        //        else if (previousFiles.Contains(str) == false)
        //        {
        //            if(previousFiles.Count>=10)
        //                previousFiles.Dequeue();
        //            previousFiles.Enqueue(str);
        //        }                
        //        return true;
        //    }
        //    catch //(System.Exception ex)
        //    {
        //        return false;
        //    }
        //}

        public static bool AddMazeFileToPrevious(string str)
        {
            try
            {
                if (previousMazeFiles.Contains(str))
                {
                    previousMazeFiles.Remove(str);
                }
                previousMazeFiles.Add(str);

                if (previousMazeFiles.Count > 10)
                    previousMazeFiles.RemoveAt(0);

                SaveSettings();
                return true;
            }
            catch //(System.Exception ex)
            {
                return false;
            }
        }

        public static bool RemoveMazeFileFromPrevious(string str)
        {
            try
            {
                if (previousMazeFiles.Contains(str))
                {
                    previousMazeFiles.Remove(str);
                    SaveSettings();
                }

                return true;
            }
            catch //(System.Exception ex)
            {
                return false;
            }
        }
        
    }
}
