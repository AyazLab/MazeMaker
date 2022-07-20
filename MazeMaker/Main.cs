using System; 
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using MazeLib;
using System.Reflection;
using System.ComponentModel.Design;
using System.IO;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.ObjectModel;
using System.IO.Compression;

namespace MazeMaker
{
    public partial class Main : Form
    {
        //bool bLicensed = false;

        Cursor cursorWall;
        Cursor cursorFloor;
        Cursor cursorStart;
        Cursor cursorEnd;

        bool bCTRLdown = false;
        bool bMouseDown = false;
        bool bMiddleMouseDown = false;


        int iGridStepSize = 17;

        int GridStepX = 17;
        int GridStepY = 17;

        int iViewOffsetX = 0;
        int iViewOffsetY = 0;
        int iViewOffsetStep = 17 * 6;

        Pen myDashedPen = new Pen(Color.Black);

        int mazeNumber = 0;
        enum Mode
        {
            none,
            actReg0,
            actReg1,
            floor0,
            floor1,
            wall0,
            wall1,
            curveWall0,
            curveWall1,
            curveWall2,
            start,
            end0,
            end1,
            object0,
            object1,
            light,
            drag,
            pan,
            staticmodel,
            dynamicmodel,
            multiSelect
        }
        Mode curMode = Mode.none;
        Maze curMaze = null;
        Maze preChangeMaze = null;// for undo

        string prevSaveDirMaze = "";

        PointF tempPoint1, tempPoint2,tempPoint3,tempPoint4;
        bool hotpoint = false;

        public MazeLib.MazeItemTheme curMazeTheme = new MazeLib.MazeItemTheme();
        public MazeLib.MazeItemThemeLibrary mazeThemeLibrary=new MazeItemThemeLibrary();
        public Main()
        {
            InitializeComponent();
            SetToolStripLocations();

            MainInitialize();

            this.DoubleBuffered = true;
            typeof(TabControl).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, tabControlMazeDisplay, new object[] { true });
            panelWelcome.Visible = true;
            
        }

        void MainInitialize()
        {
            foreach (MazeItemTheme m in mazeThemeLibrary.themeItems)
            {
                ToolStripMenuItem t = new ToolStripMenuItem(m.name);
                t.Click += themeButton_Click;
                themeToolStripMenuItem.DropDownItems.Add(t);

                t = new ToolStripMenuItem(m.name);
                t.Click += themeButton_Click;
                toolStripDropdown_Theme.DropDownItems.Add(t);
            }
        }

        bool bCommandLineFormat = false;
        string sCommandLineFile = "";

        PointF initMouseDownLocation;  //for string initial drag start point...
        
        public Main(string inp)
        {
            InitializeComponent();
            SetToolStripLocations();
            bCommandLineFormat = true;
            sCommandLineFile = inp;

            MainInitialize();

        }
        


        private void ts_core_new_Click(object sender, EventArgs e)
        {
            //New Maze...

            if (curMaze != null)
            {
                if (CloseMaze() == false)
                    return;
            }
            NewMaze();

        }

        private void ts_core_open_Click(object sender, EventArgs e)
        {
            //Open Maze... 
            ChangeModeTo0();
            Open();
        }

        private void ts_core_save_Click(object sender, EventArgs e)
        {
            //Save Maze...
            ChangeModeTo0();
            if (curMaze == null)
                return;
            if (curMaze.FileName == null)            
                SaveAs();
            else           
                SaveMaze();
        }

        private void ts_core_saveas_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            SaveAs();
        }

        private void ts_maze_pointer_Click(object sender, EventArgs e)
        {
            //Change Mode to Pointer
            //arrow
            ChangeMode(Mode.none);
            RedrawFrame();
     
        }

        private void ts_maze_wall_Click(object sender, EventArgs e)
        {
            //Change Mode to Wall
            //wall
            ChangeMode(Mode.wall0);
        }

        private void ts_maze_floor_Click(object sender, EventArgs e)
        {
            //Change Mode to Floor
            //floor
            ChangeMode(Mode.floor0);

        }

        private void ts_maze_start_Click(object sender, EventArgs e)
        {
            //Change Mode to Start Region
            //start pos          
            ChangeMode(Mode.start);
        }

        private void ts_maz_end_Click(object sender, EventArgs e)
        {
            //Change Mode to End Region
            //end region            
            ChangeMode(Mode.end0);
        }

        private void ts_maz_light_Click(object sender, EventArgs e)
        {
            ChangeMode(Mode.light);
        }

        private void MarkSelectedChanged()
        {
            int i = 0;
            for (i = 0; i < curMaze.cStart.Count; i++)
            {
                if (curMaze.cStart[i].IsSelected())
                {
                    curMaze.cStart[i].changed = true;

                }
            }
            for (i = 0; i < curMaze.cEndRegions.Count; i++)
            {
                if (curMaze.cEndRegions[i].IsSelected())
                {
                    curMaze.cEndRegions[i].changed = true;

                }
            }
            for (i = 0; i < curMaze.cWall.Count; i++)
            {
                if (curMaze.cWall[i].IsSelected())
                {
                    curMaze.cWall[i].changed = true;
                }
            }
            for (i = 0; i < curMaze.cCurveWall.Count; i++)
            {
                if (curMaze.cCurveWall[i].IsSelected())
                {
                    curMaze.cCurveWall[i].changed = true;
                }
            }
            for (i = 0; i < curMaze.cStaticModels.Count; i++)
            {
                if (curMaze.cStaticModels[i].IsSelected())
                {
                    curMaze.cStaticModels[i].changed = true;
                }
            }
            for (i = 0; i < curMaze.cDynamicObjects.Count; i++)
            {
                if (curMaze.cDynamicObjects[i].IsSelected())
                {
                    curMaze.cDynamicObjects[i].changed = true;
                }

            }

            for (i = 0; i < curMaze.cLight.Count; i++)
            {
                if (curMaze.cLight[i].IsSelected())
                {
                    curMaze.cLight[i].changed = true;
                }

            }
            for (i = 0; i < curMaze.cFloor.Count; i++)
            {
                if (curMaze.cFloor[i].IsSelected())
                {
                    curMaze.cFloor[i].changed = true;
                }
            }
            SyncSelections();
        }

        


        private void DeleteSelected()
        {
            //Delete selected...

            MarkSelectedChanged();
            StorePreviousMaze();
            SyncSelections();

            bool bDeleted = false;
            int i = 0;

            #region Delete All Selected
            //try
            //
                for (i = 0; i < curMaze.cStart.Count; i++)
                {
                    if (curMaze.cStart[i].IsSelected())
                    {
                        curMaze.cStart.RemoveAt(i);
                        i--;
                        bDeleted = true;

                    }
                }
                //foreach (EndRegion en in curMaze.cEndRegions)
                for (i = 0; i < curMaze.cEndRegions.Count; i++)
                {
                    if (curMaze.cEndRegions[i].IsSelected())
                    {
                        curMaze.cEndRegions.RemoveAt(i); 
                        i--;
                        bDeleted = true;                      

                    }
                }
                for (i = 0; i < curMaze.cActRegions.Count; i++)
                {
                    if (curMaze.cActRegions[i].IsSelected())
                    {
                        curMaze.cActRegions.RemoveAt(i);
                        i--;
                        bDeleted = true;

                    }
                }
            //if (curMaze.cEnd != null)
            //{
            //    if (curMaze.cEnd.IsSelected())
            //    {
            //        curMaze.cEnd = null;
            //        //UnSelect();
            //       // throw new Exception();
            //    }
            //}

            //foreach (Wall w in curMaze.cWall)
            for (i = 0; i < curMaze.cWall.Count;i++ )
                {
                    if (curMaze.cWall[i].IsSelected())
                    {
                        curMaze.cWall.RemoveAt(i);
                        i--;
                        bDeleted = true;                      

                    }
                }
            for (i = 0; i < curMaze.cCurveWall.Count; i++)
            {
                if (curMaze.cCurveWall[i].IsSelected())
                {
                    curMaze.cCurveWall.RemoveAt(i);
                    i--;
                    bDeleted = true;

                }
            }

            //foreach (StaticModel l in curMaze.cStaticModels)
            for (i = 0; i < curMaze.cStaticModels.Count; i++)
                {
                    if (curMaze.cStaticModels[i].IsSelected())
                    {
                        curMaze.cStaticModels.RemoveAt(i);
                        i--;
                        bDeleted = true;                      

                        //UnSelect();
                        //throw new Exception();
                    }
                }

                //foreach (DynamicModel l in curMaze.cDynamicModels)
                for (i = 0; i < curMaze.cDynamicObjects.Count; i++)
                {
                    if (curMaze.cDynamicObjects[i].IsSelected())
                    {
                        curMaze.cDynamicObjects.RemoveAt(i);
                        i--;
                        bDeleted = true;                      

                        //UnSelect();
                        //throw new Exception();
                    }

                }
                //foreach (Light l in curMaze.cLight)
                for (i = 0; i < curMaze.cLight.Count; i++)
                {
                    if (curMaze.cLight[i].IsSelected())
                    {
                        curMaze.cLight.RemoveAt(i);
                        i--;
                        bDeleted = true;                      

                        //UnSelect();
                       // throw new Exception();
                    }

                }
                //foreach (Floor f in curMaze.cFloor)
                for (i = 0; i < curMaze.cFloor.Count; i++)
                {
                    if (curMaze.cFloor[i].IsSelected())
                    {
                        curMaze.cFloor.RemoveAt(i);
                        i--;
                        bDeleted = true;                      

                        //UnSelect();
                        //throw new Exception();
                    }
                }
            //}
            //catch
            //{
            //}
            #endregion
            if (bDeleted == false)
                    toolStripStatusLabel1.Text = "Nothing to delete! Please select an item to delete!";

            MazeChanged(true);
        }

        

        private void undo()
        {
            
            SyncSelections();
            if (changedItems.Count>0)
            {
                redoItemsStack.Push(redoItems);
                redoItems = new List<Object>();
                tabPageMazeEdit.Focus();
                foreach (Object mazeItem in changedItems) // For each changed item
                {

                    if (mazeItem.GetType() == typeof(Wall))
                    {
                        Wall w = (Wall)mazeItem;
                        int curIndex = curMaze.GetMazeItemByID(w.GetID(), MazeItemType.Wall); //find the item in the current maze
                        if (curIndex >= 0)
                        {
                            redoItems.Add(curMaze.cWall[curIndex].Copy(true)); ////find the item in the current maze
                            if (w.justCreated)
                                curMaze.DeleteByID(w.GetID(), MazeItemType.Wall);
                            else
                            {
                                curMaze.cWall[curIndex] = w;
                                curMaze.cWall[curIndex].changed = false;
                            }
                        }
                        else
                        {
                            //redoItems.Add(w.Copy(true));
                            curMaze.cWall.Add(w.Copy(true));
                            
                            curMaze.cWall[curMaze.cWall.Count - 1].changed = false;

                            w.justCreated = true;
                            redoItems.Add(w.Copy(true));
                        }
                    }
                    else if (mazeItem.GetType() == typeof(CurvedWall))
                    {
                        CurvedWall w = (CurvedWall)mazeItem;
                        int curIndex = curMaze.GetMazeItemByID(w.GetID(), MazeItemType.CurvedWall);
                        if (curIndex >= 0)
                        {
                            redoItems.Add(curMaze.cCurveWall[curIndex].Copy(true));
                            if (w.justCreated)
                                curMaze.DeleteByID(w.GetID(), MazeItemType.CurvedWall);
                            else
                            {
                                curMaze.cCurveWall[curIndex] = w;
                                curMaze.cCurveWall[curIndex].changed = false;
                            }
                        }
                        else
                        {
                            //redoItems.Add(w.Copy(true));
                            curMaze.cCurveWall.Add(w.Copy(true));

                            curMaze.cCurveWall[curMaze.cCurveWall.Count - 1].changed = false;

                            w.justCreated = true;
                            redoItems.Add(w.Copy(true));
                        }
                    }
                    else if (mazeItem.GetType() == typeof(CustomObject))
                    {
                        CustomObject c = (CustomObject)mazeItem;
                        /*int curIndex = GetMazeItemByID(c.GetID(), MazeItemType.CustomObject);
                        if (curIndex >= 0)
                        {
                            redoItems.Add(curMaze.cObject[curIndex].Copy(true));
                            if (c.justCreated)
                                DeleteByID(c.GetID(), MazeItemType.CustomObject);
                            else
                            {
                                c.changed = false;
                                curMaze.cObject[curIndex] = c;
                                curMaze.cObject[curIndex].changed = false;
                            }
                        }
                        else
                        {
                            curMaze.cObject.Add(c);
                            curMaze.cObject[curMaze.cObject.Count - 1].changed = false;
                        }*/
                    }
                    else if (mazeItem.GetType() == typeof(Floor))
                    {
                        Floor f = (Floor)mazeItem;
                        int curIndex = curMaze.GetMazeItemByID(f.GetID(), MazeItemType.Floor);
                        if (curIndex >= 0)
                        {
                            redoItems.Add(curMaze.cFloor[curIndex].Copy(true));
                            if (f.justCreated)
                                curMaze.DeleteByID(f.GetID(), MazeItemType.Floor);
                            else
                            {
                                curMaze.cFloor[curIndex] = f;
                                curMaze.cFloor[curIndex].changed = false;
                            }
                        }
                        else
                        {
                           // redoItems.Add(f.Copy(true));
                            curMaze.cFloor.Add(f.Copy(true));
                            curMaze.cFloor[curMaze.cFloor.Count - 1].changed = false;

                            f.justCreated = true;
                            redoItems.Add(f.Copy(true));
                        }
                    }
                    else if (mazeItem.GetType() == typeof(Light))
                    {
                        Light l = (Light)mazeItem;
                        int curIndex = curMaze.GetMazeItemByID(l.GetID(), MazeItemType.Light);
                        if (curIndex >= 0)
                        {
                            redoItems.Add(curMaze.cLight[curIndex].Copy(true));
                            if (l.justCreated)
                                curMaze.DeleteByID(l.GetID(), MazeItemType.Light);
                            else
                            {
                                l.changed = false;
                                curMaze.cLight[curIndex] = l;
                                curMaze.cLight[curIndex].changed = false;
                            }
                        }
                        else
                        {
                            curMaze.cLight.Add(l);
                            curMaze.cLight[curMaze.cLight.Count - 1].changed = false;

                            l.justCreated = true;
                            redoItems.Add(l.Copy(true));
                        }

                    }
                    else if (mazeItem.GetType() == typeof(StaticModel))
                    {
                        StaticModel s = (StaticModel)mazeItem;
                        int curIndex = curMaze.GetMazeItemByID(s.GetID(), MazeItemType.Static);
                        if (curIndex >= 0)
                        {
                            redoItems.Add(curMaze.cStaticModels[curIndex].Copy(true));
                            if (s.justCreated)
                                curMaze.DeleteByID(s.GetID(), MazeItemType.Static);
                            else
                            {
                                s.changed = false;
                                curMaze.cStaticModels[curIndex] = s;
                                curMaze.cStaticModels[curIndex].changed = false;
                            }
                        }
                        else
                        {
                            curMaze.cStaticModels.Add(s);
                            curMaze.cStaticModels[curMaze.cStaticModels.Count - 1].changed = false;

                            s.justCreated = true;
                            redoItems.Add(s.Copy(true));
                        }
                    }
                    else if (mazeItem.GetType() == typeof(DynamicObject))
                    {
                        DynamicObject d = (DynamicObject)mazeItem;
                        int curIndex = curMaze.GetMazeItemByID(d.GetID(), MazeItemType.Dynamic);
                        if (curIndex >= 0)
                        {
                            redoItems.Add(curMaze.cDynamicObjects[curIndex].Copy(true));
                            if (d.justCreated)
                                curMaze.DeleteByID(d.GetID(), MazeItemType.Dynamic);
                            else
                            {
                                d.changed = false;
                                curMaze.cDynamicObjects[curIndex] = d;
                                curMaze.cDynamicObjects[curIndex].changed = false;
                            }
                        }
                        else
                        {
                            curMaze.cDynamicObjects.Add(d);
                            curMaze.cDynamicObjects[curMaze.cDynamicObjects.Count - 1].changed = false;

                            d.justCreated = true;
                            redoItems.Add(d.Copy(true));
                        }
                    }
                    else if (mazeItem.GetType() == typeof(EndRegion))
                    {
                        EndRegion en = (EndRegion)mazeItem;
                        int curIndex = curMaze.GetMazeItemByID(en.GetID(), MazeItemType.End);
                        if (curIndex >= 0)
                        {
                            redoItems.Add(curMaze.cEndRegions[curIndex].Copy(true));
                            if (en.justCreated)
                                curMaze.DeleteByID(en.GetID(), MazeItemType.End);
                            else
                            {
                                en.changed = false;
                                curMaze.cEndRegions[curIndex] = en;
                                curMaze.cEndRegions[curIndex].changed = false;
                            }
                        }
                        else
                        {
                            curMaze.cEndRegions.Add(en);
                            curMaze.cEndRegions[curMaze.cEndRegions.Count - 1].changed = false;

                            en.justCreated = true;
                            redoItems.Add(en.Copy(true));
                        }
                    }
                    else if (mazeItem.GetType() == typeof(ActiveRegion))
                    {
                        ActiveRegion en = (ActiveRegion)mazeItem;
                        int curIndex = curMaze.GetMazeItemByID(en.GetID(), MazeItemType.ActiveRegion);
                        if (curIndex >= 0)
                        {
                            redoItems.Add(curMaze.cActRegions[curIndex].Copy(true));
                            if (en.justCreated)
                                curMaze.DeleteByID(en.GetID(), MazeItemType.ActiveRegion);
                            else
                            {
                                en.changed = false;
                                curMaze.cActRegions[curIndex] = en;
                                curMaze.cActRegions[curIndex].changed = false;
                            }
                        }
                        else
                        {
                            curMaze.cActRegions.Add(en);
                            curMaze.cActRegions[curMaze.cActRegions.Count - 1].changed = false;

                            en.justCreated = true;
                            redoItems.Add(en.Copy(true));
                        }
                    }
                    else if (mazeItem.GetType() == typeof(StartPos))
                    {
                        StartPos sPos = (StartPos)mazeItem;

                        int curIndex = curMaze.GetMazeItemByID(sPos.GetID(), MazeItemType.Start);
                        if (curIndex >= 0)
                        {
                            redoItems.Add(curMaze.cStart[curIndex].Copy(true));
                            if (sPos.justCreated)
                                curMaze.DeleteByID(sPos.GetID(), MazeItemType.Start);
                            else
                            {
                                sPos.changed = false;
                                curMaze.cStart[curIndex] = sPos;
                                curMaze.cStart[curIndex].changed = false;
                            }
                        }
                        else
                        {
                            curMaze.cStart.Add(sPos);
                            curMaze.cStart[curMaze.cStart.Count - 1].changed = false;

                            sPos.justCreated = true;
                            redoItems.Add(sPos.Copy(true));
                        }
                    
                    }
                }
                //SyncSelections();
                MazeChanged(true);

                //redoItems = changedItems;
                if (changedItemsStack.Count > 0)
                    changedItems = (List<Object>)changedItemsStack.Pop();
                else
                    changedItems = new List<Object>();
                //changedItems2 = changedItems3;
                //changedItems3 = changedItems4;
                //changedItems4 = new List<Object>();
            }
        }

        private void redo()
        {
            SyncSelections();
            if (redoItems.Count > 0)
            {
                tabPageMazeEdit.Focus();
                StorePreviousMaze();
                bool bDelete = false;
               
                foreach (Object mazeItem in redoItems)
                {

                    
                    if (mazeItem.GetType() == typeof(Wall))
                    {
                        Wall w = (Wall)mazeItem;
                        int curIndex = curMaze.GetMazeItemByID(w.GetID(), MazeItemType.Wall);
                        if (curIndex >= 0)
                        {
                            //DeleteByID(w.GetID(), MazeItemType.Wall);
                            curMaze.cWall[curIndex] = w;
                            curMaze.cWall[curIndex].changed = true;
                            if (w.justCreated)
                            {
                                if (!bDelete) { UnSelect(); bDelete = true; }
                                curMaze.cWall[curIndex].Select(true);

                            }
                        }
                        else
                        {
                            //redoItems.Add(w.Copy(true));
                            curMaze.cWall.Add(w);
                            curMaze.cWall[curMaze.cWall.Count - 1].changed = true;
                        }
                    }
                    else if (mazeItem.GetType() == typeof(CurvedWall))
                    {
                        CurvedWall w = (CurvedWall)mazeItem;
                        int curIndex = curMaze.GetMazeItemByID(w.GetID(), MazeItemType.CurvedWall);
                        if (curIndex >= 0)
                        {
                            //DeleteByID(w.GetID(), MazeItemType.Wall);
                            curMaze.cCurveWall[curIndex] = w;
                            curMaze.cCurveWall[curIndex].changed = true;
                            if (w.justCreated)
                            {
                                if (!bDelete) { UnSelect(); bDelete = true; }
                                curMaze.cCurveWall[curIndex].Select(true);

                            }
                        }
                        else
                        {
                            //redoItems.Add(w.Copy(true));
                            curMaze.cCurveWall.Add(w);
                            curMaze.cCurveWall[curMaze.cCurveWall.Count - 1].changed = true;
                        }
                    }
                    else if (mazeItem.GetType() == typeof(CustomObject))
                    {
                        CustomObject c = (CustomObject)mazeItem;
                        /*int curIndex = GetMazeItemByID(c.GetID(), MazeItemType.CustomObject);
                        if (curIndex >= 0)
                        {
                            redoItems.Add(curMaze.cObject[curIndex].Copy(true));
                            if (c.justCreated)
                                DeleteByID(c.GetID(), MazeItemType.CustomObject);
                            else
                            {
                                c.changed = true;
                                curMaze.cObject[curIndex] = c;
                                curMaze.cObject[curIndex].changed = true;
                            }
                        }
                        else
                        {
                            curMaze.cObject.Add(c);
                            curMaze.cObject[curMaze.cObject.Count - 1].changed = true;
                        }*/
                    }
                    else if (mazeItem.GetType() == typeof(Floor))
                    {
                        Floor f = (Floor)mazeItem;
                        int curIndex = curMaze.GetMazeItemByID(f.GetID(), MazeItemType.Floor);
                        if (curIndex >= 0)
                        {
                            
                            curMaze.cFloor[curIndex] = f;
                            curMaze.cFloor[curIndex].changed = true;

                            if (f.justCreated)
                            {
                                if (!bDelete) { UnSelect(); bDelete = true; }
                                f.Select(true);
                            }

                        }
                        else
                        {
                            // redoItems.Add(f.Copy(true));
                            curMaze.cFloor.Add(f);
                            curMaze.cFloor[curMaze.cFloor.Count - 1].changed = true;
                        }
                    }
                    else if (mazeItem.GetType() == typeof(Light))
                    {
                        Light l = (Light)mazeItem;
                        int curIndex = curMaze.GetMazeItemByID(l.GetID(), MazeItemType.Light);
                        if (curIndex >= 0)
                        {
                            
                            //DeleteByID(l.GetID(), MazeItemType.Light);

                            l.changed = true;
                            curMaze.cLight[curIndex] = l;
                            curMaze.cLight[curIndex].changed = true;

                            if (l.justCreated)
                            {
                                if (!bDelete) { UnSelect(); bDelete = true; }
                                l.Select(true);
                            }
                        }
                        else
                        {
                            curMaze.cLight.Add(l);
                            curMaze.cLight[curMaze.cLight.Count - 1].changed = true;
                        }

                    }
                    else if (mazeItem.GetType() == typeof(StaticModel))
                    {
                        StaticModel s = (StaticModel)mazeItem;
                        int curIndex = curMaze.GetMazeItemByID(s.GetID(), MazeItemType.Static);
                        if (curIndex >= 0)
                        {
                            
                            //DeleteByID(s.GetID(), MazeItemType.Static);
                            s.changed = true;
                            curMaze.cStaticModels[curIndex] = s;
                            curMaze.cStaticModels[curIndex].changed = true;

                            if (s.justCreated)
                            {
                                if (!bDelete) { UnSelect(); bDelete = true; }
                                s.Select(true);
                            }
                        }
                        else
                        {
                            curMaze.cStaticModels.Add(s);
                            curMaze.cStaticModels[curMaze.cStaticModels.Count - 1].changed = true;
                        }
                    }
                    else if (mazeItem.GetType() == typeof(DynamicObject))
                    {
                        DynamicObject d = (DynamicObject)mazeItem;
                        int curIndex = curMaze.GetMazeItemByID(d.GetID(), MazeItemType.Dynamic);
                        if (curIndex >= 0)
                        {
                            d.changed = true;
                            curMaze.cDynamicObjects[curIndex] = d;
                            curMaze.cDynamicObjects[curIndex].changed = true;

                            if (d.justCreated)
                            {
                                if (!bDelete) { UnSelect(); bDelete = true; }
                                d.Select(true);
                            }
                        }
                        else
                        {
                            curMaze.cDynamicObjects.Add(d);
                            curMaze.cDynamicObjects[curMaze.cDynamicObjects.Count - 1].changed = true;
                        }
                    }
                    else if (mazeItem.GetType() == typeof(EndRegion))
                    {
                        EndRegion en = (EndRegion)mazeItem;
                        int curIndex = curMaze.GetMazeItemByID(en.GetID(), MazeItemType.End);
                        if (curIndex >= 0)
                        {

                            en.changed = true;
                            curMaze.cEndRegions[curIndex] = en;
                            curMaze.cEndRegions[curIndex].changed = true;

                            if (en.justCreated)
                            {
                                if (!bDelete) { UnSelect(); bDelete = true; }
                                en.Select(true);
                            }
                        }
                        else
                        {
                            curMaze.cEndRegions.Add(en);
                            curMaze.cEndRegions[curMaze.cEndRegions.Count - 1].changed = true;
                        }
                    }
                    else if (mazeItem.GetType() == typeof(ActiveRegion))
                    {
                        ActiveRegion en = (ActiveRegion)mazeItem;
                        int curIndex = curMaze.GetMazeItemByID(en.GetID(), MazeItemType.ActiveRegion);
                        if (curIndex >= 0)
                        {

                            en.changed = true;
                            curMaze.cActRegions[curIndex] = en;
                            curMaze.cActRegions[curIndex].changed = true;

                            if (en.justCreated)
                            {
                                if (!bDelete) { UnSelect(); bDelete = true; }
                                en.Select(true);
                            }
                        }
                        else
                        {
                            curMaze.cActRegions.Add(en);
                            curMaze.cActRegions[curMaze.cActRegions.Count - 1].changed = true;
                        }
                    }
                    else if (mazeItem.GetType() == typeof(StartPos))
                    {
                        StartPos sPos = (StartPos)mazeItem;
                        int curIndex = curMaze.GetMazeItemByID(sPos.GetID(), MazeItemType.Start);
                        if (curIndex >= 0)
                        {

                            sPos.changed = true;
                            curMaze.cStart[curIndex] = sPos;
                            curMaze.cStart[curIndex].changed = true;

                            if (sPos.justCreated)
                            {
                                if (!bDelete) { UnSelect(); bDelete = true; }
                                sPos.Select(true);
                            }
                        }
                        else
                        {
                            curMaze.cStart.Add(sPos);
                            curMaze.cStart[curMaze.cStart.Count - 1].changed = true;
                        }
                    }
                }
                SyncSelections();
                if (bDelete)
                    DeleteSelected();
                MazeChanged(true);
                
                //undo();
               
            
            }
        }

        private void ts_delete_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            DeleteSelected();
            
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Close current Maze...
            CloseMaze();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Exit the Program...
            Application.Exit();
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {

            //Dropped a file to open...
            //MessageBox.Show("Form1 - Drop");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About bx = new About();
            bx.ShowDialog();
            bx.Dispose();
        }

        public void NewMaze()
        {
            this.isClassicFormat = false;
            tabPageMazeEdit.BackgroundImage = null;
            panelWelcome.Visible = false;
            XYZ_axisImagebox.Visible = true;
            //TextureCounter.Reset();
            curMaze = new Maze();
            toogleElementsToolStrip(true);
            ChangeMode(Mode.none);
            RedrawFrame();
            mazeNumber++;
            this.Text = "MazeMaker - Maze" + mazeNumber.ToString();
            curMaze.Name = "Maze" + mazeNumber.ToString();
            Maze.mzP = curMaze;
            propertyGrid.SelectedObject = curMaze;
            UpdateTree();
            ShowViewMoveButtons(true);

            preChangeMaze = null;

            resetUndoBuffer();
        }

        public bool OpenTheFile(string inp)
        {
            if (File.Exists(inp)==false)
            {
                MessageBox.Show("Can not find or access file: \n\n" + inp,"Open File",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return false;
            }
            int iExt = CheckInputFileExtension(inp);
            if (iExt == 1) // .maz
            {
                if (curMaze != null)
                {
                    if (!CloseMaze())
                        return false;
                }
                NewMaze();
                if (curMaze.ReadFromFileXML(inp))
                {
                    isClassicFormat = false;
                    CurrentSettings.AddMazeFileToPrevious(inp);
                    this.Text = "MazeMaker - " + inp;

                }
                else
                {
                    if (curMaze.ReadFromClassicFile(inp))
                    {
                        isClassicFormat = true;
                        CurrentSettings.AddMazeFileToPrevious(inp);
                        this.Text = "MazeMaker - " + inp;
                    }
                    else
                    {
                        CloseMaze();
                        MessageBox.Show("Not a maze file or corrupted maze file", "MazeMaker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
            }
            else if (iExt == 2) // .mel
            {
                MazeListBuilder dia = new MazeListBuilder();
                dia.ReadFromFile(inp, false);
                dia.ShowDialog();
            }
            

            UpdateTree();
            ResetCanvasOffset();
            resetUndoBuffer();
            StorePreviousMaze();
            return true;
        }
        bool openDialogActive =false;
        public void Open()
        {
            //Open Maze...  
            OpenFileDialog a = new OpenFileDialog();
            a.Filter = "Maze Files (*.maz)|*.maz|MazeList Files (*.mel)|*.mel";
            a.FilterIndex = 1;
            if (prevSaveDirMaze != "")
            {
                a.InitialDirectory = prevSaveDirMaze;
            }
            openDialogActive = true;
            if (a.ShowDialog() == DialogResult.OK)
            {
                prevSaveDirMaze = Path.GetDirectoryName(a.FileName);
                OpenTheFile(a.FileName);
            }
           
        }

        bool isClassicFormat = false;
        public void SaveMaze()
        {
            if (curMaze == null)
            {
                return;
            }
            if (curMaze.FileName == "")
            {
                SaveAs();
                return;
            }
            if (curMaze.cStart.Count == 0)
            {
                if (MessageBox.Show("No start location has been specified!\n\nDo you want to save without start location?", "MazeMaker", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;
            }
            if (isClassicFormat && MessageBox.Show("This maze was created using the old format.\nClick OK to convert and save this maze in the new format\n\nIf you would like to use the old format, select Cancel and goto File>Export To Classic Format from top menu", "MazeMaker", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {

                isClassicFormat = false;
            }
            else if (isClassicFormat)
                return;
            
            if (isClassicFormat)
            { 
                curMaze.SaveToClassicFile(curMaze.FileName);
            }
            else
                curMaze.SaveToMazeXML(curMaze.FileName);
            this.Text = "MazeMaker - " + curMaze.FileName;
            CurrentSettings.AddMazeFileToPrevious(curMaze.FileName);
        }

        public void SaveAs()
        {
            if (curMaze == null)
                return;
            if (curMaze.cStart.Count == 0)
            {
                if (MessageBox.Show("No start location has been specified!\n\nDo you want to save without start location?", "MazeMaker", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;
            }
            if (isClassicFormat && MessageBox.Show("This maze was created using the old format.\nClick OK to convert and save this maze in the new format\n\nIf you would like to use the old format, select Cancel and goto File>Export To Classic Format from top menu", "MazeMaker", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                isClassicFormat = false;
            else if (isClassicFormat)
                return;

            SaveFileDialog a = new SaveFileDialog();
            a.Filter = "Maze Files (*.maz)|*.maz";
            a.FilterIndex = 1;
            a.DefaultExt = ".maz";
            if (prevSaveDirMaze != "")
            {
                a.InitialDirectory = prevSaveDirMaze;
            }

            if (this.Text[this.Text.Length - 1] != '*')
                a.FileName = this.Text.Substring(12);
            else
                a.FileName = this.Text.Substring(12, this.Text.Length - 13);
            
            if (a.ShowDialog() == DialogResult.OK)
            {

                if (isClassicFormat)
                    curMaze.SaveToClassicFile(a.FileName);
                else
                    curMaze.SaveToMazeXML(a.FileName);

                prevSaveDirMaze = Path.GetDirectoryName(a.FileName); 
                this.Text = "MazeMaker - " + a.FileName;
                CurrentSettings.AddMazeFileToPrevious(a.FileName);
                UpdateTree();
            }
        }

        public bool CloseMaze()
        {            
            if (curMaze != null)
            {
                if (curMaze.changed) {
                    DialogResult dResult = MessageBox.Show("Current maze (" + curMaze.Name + ") has been changed.\t\n\nDo you want to save?", "MazeMaker", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                    if (dResult == DialogResult.Yes)
                        SaveMaze();
                    else if (dResult == DialogResult.Cancel)
                        return false;
                }
                //if (MessageBox.Show("Do you want to close current maze!", "MazeMaker", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                //{

                ChangeMode(Mode.none);
                    ResetCanvasOffset();
                    curMaze = null;
                    toogleElementsToolStrip(false);
                    
                    ts_delete.Enabled = false;
                    RedrawFrame();
                    propertyGrid.SelectedObject = null;
                    this.Text = "MazeMaker";
                    tabPageMazeEdit.BackgroundImage = MazeMaker.Properties.Resources.mazeMakerBG;
                    ClearTree();
                    ChangeMode(Mode.none);
                    panelWelcome.Visible = true;
                    XYZ_axisImagebox.Visible = false;
                    PositionWelcomePanel();
                    ShowViewMoveButtons(false);
                    return true;
                //}
            }
            return false;
        }

        //Also in Registry dialog/class
        ContextMenu cm = new ContextMenu();

        private void SendToBack_Click(object sender, EventArgs e)
        {
            foreach (MazeItem item in selected)
            {
                curMaze.DeleteItemInCollection(item);
                curMaze.AddItemToCollection(item, 0);
            }
            UnSelect();
            MazeChanged(true);
        }

        private void SendToFront_Click(object sender, EventArgs e)
        {
            foreach (MazeItem item in selected)
            {
                curMaze.DeleteItemInCollection(item);
                curMaze.AddItemToCollection(item);
            }
            MazeChanged(true);
        }

        private void generateContextMenu(string objDescrip="")
        {
            cm.Name = "RightClickMenu";
            cm.MenuItems.Clear();
            if (selected.Count > 0)
            {
                MenuItem cutItem = new MenuItem("Cut", new EventHandler(this.cutToolStripMenuItem_Click));

                cm.MenuItems.Add(cutItem);
            }
            if (selected.Count > 0)
            {
                MenuItem copyItem = new MenuItem("Copy", new EventHandler(this.copyToolStripMenuItem_Click));
                
                cm.MenuItems.Add(copyItem);
            }
            if(mazeClipboard.Count>0)
            {
                MenuItem pasteItem = new MenuItem("Paste", new EventHandler(this.pasteToolStripMenuItem_Click));
                cm.MenuItems.Add(pasteItem);
            }
            if(selected.Count==1)
            {
                MenuItem menuBreak = new MenuItem("-");
                cm.MenuItems.Add(menuBreak);
                MenuItem sendToBack = new MenuItem("Send To Back", new EventHandler(this.SendToBack_Click));
                cm.MenuItems.Add(sendToBack);
                MenuItem sendToFront = new MenuItem("Send To Front", new EventHandler(this.SendToFront_Click));
                cm.MenuItems.Add(sendToFront);
            }
            if (selected.Count > 0)
            {
                MenuItem menuBreak = new MenuItem("-");
                cm.MenuItems.Add(menuBreak);
                MenuItem rotateItem = new MenuItem("Rotate");
                rotateItem.MenuItems.Add(new MenuItem("90° Clockwise", new EventHandler(degrees90ToolStripMenuItem_Click)));
                rotateItem.MenuItems.Add(new MenuItem("180°", new EventHandler(degrees180ToolStripMenuItem_Click)));
                rotateItem.MenuItems.Add(new MenuItem("90° Counterclockwise", new EventHandler(degrees270ToolStripMenuItem_Click)));
                cm.MenuItems.Add(rotateItem);
            }
            if (selected.Count > 0)
            {
                MenuItem menuBreak = new MenuItem("-");
                cm.MenuItems.Add(menuBreak);
                MenuItem deleteItem = new MenuItem("Delete", new EventHandler(this.ts_delete_Click));
                cm.MenuItems.Add(deleteItem);
            }

            if (objDescrip.Length > 0) //For individual items and breakout for all selected items
            {
                MenuItem menuBreak = new MenuItem("-");
                cm.MenuItems.Add(menuBreak);
                MenuItem descrip = new MenuItem(objDescrip);

                descrip.Enabled = false;

                cm.MenuItems.Add(descrip);

                if(selected.Count>1)
                {
                    descrip.Enabled = true;
                    foreach(MazeItem mItem in selected)
                    {
                        String itemName = mItem.ID;
                        if (mItem.Label.Length > 0)
                            itemName += " (" + mItem.Label + ")";
                        descrip.MenuItems.Add(new MenuItem(itemName,new EventHandler(this.context_itemClick)));
                    }
                }
            }

            
        }

        private void context_itemClick(object sender, EventArgs e)
        {
            if(selected.Count>((MenuItem)sender).Index)
            {
                MazeItem indexedItem = (MazeItem)selected[((MenuItem)sender).Index];
                UnSelect();
                indexedItem.Select();
                SyncSelections();
                RedrawFrame();

            }
        }


        private int CountNumSelected(List<object> mazeItems)
        {
            int numSelected=0;

            foreach(object mazeItem in mazeItems)
            {
                if(((MazeItem)mazeItem).IsSelected())
                    numSelected++;
            }

            return numSelected;
        }

        private void SelectItems(List<object> mazeItems, bool invertSelection = false, bool clearSelection = false)
        {
            if (clearSelection)
                UnSelect();
            foreach (object mazeItem in mazeItems)
            {
                if (invertSelection&&((MazeItem)mazeItem).IsSelected())
                    ((MazeItem)mazeItem).Select(false);
                else
                    ((MazeItem)mazeItem).Select(true);
            }
        }

        private void MazeItemContextMenu_Show(object sender, MouseEventArgs e)
        {
            if (curMaze == null)
                return;

            tempPoint1 = e.Location;

            List<object> ItemsInContext=GetItemsInArea(e.X - iViewOffsetX, e.Y - iViewOffsetY);
            int numSelectedInContext = CountNumSelected(ItemsInContext);

            if (selected.Count == 0 || numSelectedInContext == 0) //Only re-select if no items are selected or if no items in area are selected
                SelectItems(ItemsInContext, false, true);
            SyncSelections();
            RedrawFrame();
            

            if (selected.Count > 1)
                generateContextMenu(selected.Count + " Items Selected");
            else if (selected.Count == 1)
            {
                String descrip = "Error";
                foreach (Object mazeItem in selected)
                {
                    if (mazeItem.GetType() == typeof(Wall))
                    {
                        Wall w = (Wall)mazeItem;
                        descrip = w.ID;
                            if(w.Label.Length>0)
                                descrip+="(" + w.Label + ")";
                    }
                    else if (mazeItem.GetType() == typeof(CustomObject))
                    {
                        CustomObject c = (CustomObject)mazeItem;
                        //descrip = w.ID + "(" + w.Label + ")";
                    }
                    else if (mazeItem.GetType() == typeof(Floor))
                    {
                        Floor f = (Floor)mazeItem;
                        descrip = f.ID;
                        if (f.Label.Length > 0)
                            descrip += "(" + f.Label + ")";
                    }
                    else if (mazeItem.GetType() == typeof(Light))
                    {
                        Light l = (Light)mazeItem;
                        descrip = l.ID;
                        if (l.Label.Length > 0)
                            descrip += "(" + l.Label + ")";

                    }
                    else if (mazeItem.GetType() == typeof(StaticModel))
                    {
                        StaticModel s = (StaticModel)mazeItem;
                        descrip = s.ID;
                        if (s.Label.Length > 0)
                            descrip += "(" + s.Label + ")";
                    }
                    else if (mazeItem.GetType() == typeof(DynamicObject))
                    {
                        DynamicObject d = (DynamicObject)mazeItem;
                        descrip = d.ID;
                        if (d.Label.Length > 0)
                            descrip += "(" + d.Label + ")";
                    }
                    else if (mazeItem.GetType() == typeof(EndRegion))
                    {
                        EndRegion en = (EndRegion)mazeItem;
                        descrip = en.ID;
                        if (en.Label.Length > 0)
                            descrip += "(" + en.Label + ")";

                    }
                    else if (mazeItem.GetType() == typeof(ActiveRegion))
                    {
                        ActiveRegion en = (ActiveRegion)mazeItem;
                        descrip = en.ID;
                        if (en.Label.Length > 0)
                            descrip += "(" + en.Label + ")";

                    }
                    else if (mazeItem.GetType() == typeof(StartPos))
                    {
                        StartPos sPos = (StartPos)mazeItem;
                        descrip = sPos.ID;
                        if (sPos.Label.Length > 0)
                            descrip += "(" + sPos.Label + ")";


                    }
                }
                generateContextMenu(descrip);
            }
            else
                generateContextMenu("");

                cm.Show(tabControlMazeDisplay, e.Location);
        }

            private void Form1_Load(object sender, EventArgs e)
        {
           SplashScreen.SetStatus("Reading settings");

           System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
           splitContainer2.Panel1Collapsed = false;
           leftPaneToolStripMenuItem.Checked = true;
           rightPaneToolStripMenuItem.Checked = true;
           toogleElementsToolStrip(false);
           ts_delete.Enabled = false;

           ShowViewMoveButtons(false);
            XYZ_axisImagebox.Visible = false;
           toolStripStatusLabel1.Text = "";
           tabControlMazeDisplay.SelectedIndex = 0;
            //toolStrip_object.Hide();
            
            //cm.MenuItems.Add("Cut", new EventHandler(Removepicture_Click));

            tabControlMazeDisplay.ContextMenu = cm;
            
                ///V Really slow
            //try
            //{
            //    cursorWall = new Cursor("1.dat");
            //    cursorFloor = new Cursor("2.dat");
            //    cursorStart = new Cursor("3.dat");
            //    cursorEnd = new Cursor("4.dat");
            //}
            //catch
            //{
                cursorWall = Cursors.Hand;
                cursorFloor = Cursors.Cross;
                cursorStart = Cursors.SizeAll;
                cursorEnd = Cursors.Cross;
            //}

            

            //Process parameters

            if (bCommandLineFormat)
            {
                timer1.Enabled = true;                
            }
            //ToolStripManager.LoadSettings(this);

            Settings.InitializeSettings();   //from ini file (for compatibility with MW
            CurrentSettings.ReadSettings();  //from custom xml file
            setTheme(CurrentSettings.themeIndex);
            quickRunSettings = CurrentSettings.quickRunSettings;


            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            myDashedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            if (this.WindowState != FormWindowState.Minimized)
            {
                this.Activate();
                this.Select();
                this.Focus();
                this.BringToFront();
            }

            SplashScreen.CloseForm();
            PositionWelcomePanel();
        }

        private void basicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            basicToolStripMenuItem.Checked = !basicToolStripMenuItem.Checked;
            //toogle file toolbar
            if (basicToolStripMenuItem.Checked)
            {
                toolStrip_coreIO.Show();
            }
            else
            {
                toolStrip_coreIO.Hide();
            }
        }

        private void mazeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            mazeToolStripMenuItem.Checked = !mazeToolStripMenuItem.Checked; 
            //toogle elements toolbar
            if (mazeToolStripMenuItem.Checked)
            {
                toolStrip_maze.Show();
            }
            else
            {
                toolStrip_maze.Hide();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            deleteToolStripMenuItem.Checked = !deleteToolStripMenuItem.Checked;
            //toogle elements toolbar
            if (deleteToolStripMenuItem.Checked)
            {
                toolStrip_delete.Show();
            }
            else
            {
                toolStrip_delete.Hide();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.S:
                    if (e.Control == false)
                        break;
                    SaveMaze();
                    break;
                case Keys.O:
                    if (e.Control == false)
                        break;
                    Open();
                    break;
                case Keys.C:
                    if (e.Control == false)
                        break;
                    Copy();
                    break;
                case Keys.V:
                    if (e.Control == false)
                        break;
                    Paste();
                    break;
                case Keys.Left:
                    if (e.Shift)
                    {
                        buttonViewMoveLeft_Click(null, null);
                        e.SuppressKeyPress = true;
                        break;
                    }
                    if (e.Control == false || curMode != Mode.none)
                        break;                 
                    FindSelectedAndMove(-1,-1,-1, 0,true);                   
                    RedrawFrame();
                    break;
                case Keys.Right:
                    if (e.Shift)
                    {
                        buttonViewMoveRight_Click(null, null);
                        e.SuppressKeyPress = true;
                        break;
                    }
                    if (e.Control == false || curMode != Mode.none)
                        break;                   
                    FindSelectedAndMove(-1, -1, 1, 0,true);
                    RedrawFrame();
                    break;
                case Keys.Up:
                    if (e.Shift)
                    {
                        buttonViewMoveUp_Click(null, null);
                        e.SuppressKeyPress = true;
                        break;
                    }
                    if (e.Control == false || curMode != Mode.none)
                        break;
                    FindSelectedAndMove(-1, -1, 0, -1,true);
                    RedrawFrame();
                    break;
                case Keys.Down:
                    if (e.Shift)
                    {
                        buttonViewMoveDown_Click(null, null);
                        e.SuppressKeyPress = true;
                        break;
                    }
                    if (e.Control == false || curMode != Mode.none)
                       break;
                    FindSelectedAndMove(-1, -1, 0, 1,true);
                    RedrawFrame();
                    break;
                case Keys.Escape:
                    ChangeMode(Mode.none);
                    UnSelect(true);
                    break;
                case Keys.W:
                    if(e.Alt == true)
                        ChangeMode(Mode.wall0);
                    break;
                case Keys.F:
                    if (e.Alt== true)
                        ChangeMode(Mode.floor0);
                    break;
                case Keys.P:
                    if (e.Alt == true)
                        ChangeMode(Mode.start);
                    break;
                case Keys.E:
                    if (e.Alt == true)
                        ChangeMode(Mode.end0);
                    break;
                case Keys.M:
                    if (e.Alt == true)
                        ChangeMode(Mode.staticmodel);
                    break;
                case Keys.D:
                    if (e.Alt == true)
                        ChangeMode(Mode.dynamicmodel);
                    break;
                case Keys.L:
                    if (e.Alt == true)
                        ChangeMode(Mode.light);
                    break;
                case Keys.Delete:
                        DeleteSelected();

                       // toolStripStatusLabel1.Text = "To delete maze items, use Ctrl + Del key combination!";
                    break;
                case Keys.N:
                    if (e.Control == true)
                    {
                        if (curMaze != null)
                        {
                            if (CloseMaze() == false)
                                return;
                        }
                        NewMaze();
                    }
                    break;
                case Keys.ControlKey:
                    bCTRLdown = true;
                    break;
                case Keys.ShiftKey:
                    GridStepX = 1;
                    GridStepY = 1;
                    break;
                case Keys.F5:
                    Run();
                    break;
                case Keys.Space:
                    if (e.Shift == false)
                        break;
                    SelectNextObject();
                    RedrawFrame();
                    #region old selection code
                    /*
                    if (propertyGrid1.SelectedObject != null)
                    {
                        try
                        {
                            
                            if (propertyGrid1.SelectedObject == curMaze)
                            {
                                if (curMaze.cWall.Count > 0)
                                {
                                    UnSelect();
                                    curMaze.cWall[0].Select(true);
                                    propertyGrid1.SelectedObject = curMaze.cWall[0];
                                }
                                else if (curMaze.cFloor.Count > 0)
                                {
                                    UnSelect();
                                    curMaze.cFloor[0].Select(true);
                                    propertyGrid1.SelectedObject = curMaze.cFloor[0];
                                }
                                else if (curMaze.cStart != null)
                                {
                                    UnSelect();
                                    curMaze.cStart.Select(true);
                                    propertyGrid1.SelectedObject = curMaze.cStart;
                                }
                                //else if (curMaze.cEnd != null)
                                //{
                                //    UnSelect();
                                //    curMaze.cEnd.Select(true);
                                //    propertyGrid1.SelectedObject = curMaze.cEnd;
                                //}
                                else if (curMaze.cEndRegions.Count > 0 )
                                {
                                    UnSelect();
                                    curMaze.cEndRegions[0].Select(true);
                                    propertyGrid1.SelectedObject = curMaze.cEndRegions[0];
                                }                                   
                                else if( curMaze.cLight.Count >0 )
                                {
                                    UnSelect();
                                    curMaze.cLight[0].Select(true);
                                    propertyGrid1.SelectedObject = curMaze.cLight[0];
                                }
                                else if(curMaze.cStaticModels.Count>0)
                                {
                                    UnSelect();
                                    curMaze.cStaticModels[0].Select(true);
                                    propertyGrid1.SelectedObject = curMaze.cStaticModels[0];
                                }
                                else if (curMaze.cDynamicModels.Count > 0)
                                {
                                    UnSelect();
                                    curMaze.cDynamicModels[0].Select(true);
                                    propertyGrid1.SelectedObject = curMaze.cDynamicModels[0];
                                }
                                throw new Exception();
                            }
                            for (int i = 0; i < curMaze.cWall.Count; i++)
                            {
                                if (curMaze.cWall[i].IsSelected())
                                {
                                    if (i + 1 < curMaze.cWall.Count)
                                    {
                                        UnSelect();
                                        curMaze.cWall[i + 1].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cWall[i + 1];
                                    }
                                    else if (curMaze.cFloor.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cFloor[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cFloor[0];

                                    }
                                    else if (curMaze.cStart != null)
                                    {
                                        UnSelect();
                                        curMaze.cStart.Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cStart;
                                    }
                                    //else if (curMaze.cEnd != null)
                                    //{
                                    //    UnSelect();
                                    //    curMaze.cEnd.Select(true);
                                    //    propertyGrid1.SelectedObject = curMaze.cEnd;
                                    //}
                                    else if (curMaze.cEndRegions.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cEndRegions[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cEndRegions[0];
                                    }
                                    else if (curMaze.cLight.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cLight[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cLight[0];
                                    }
                                    else if (curMaze.cStaticModels.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cStaticModels[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cStaticModels[0];
                                    }
                                    else if (curMaze.cDynamicModels.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cDynamicModels[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cDynamicModels[0];
                                    }
                                    else
                                    {
                                        UnSelect();
                                        curMaze.cWall[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cWall[0];

                                    }
                                    ts_delete.Enabled = true;
                                    throw new Exception();
                                }
                            }
                            for (int i = 0; i < curMaze.cFloor.Count; i++)
                            {
                                if (curMaze.cFloor[i].IsSelected())
                                {
                                    if (i + 1 < curMaze.cFloor.Count)
                                    {
                                        UnSelect();
                                        curMaze.cFloor[i + 1].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cFloor[i + 1];
                                    }
                                    else if (curMaze.cStart != null)
                                    {
                                        UnSelect();
                                        curMaze.cStart.Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cStart;
                                    }
                                    //else if (curMaze.cEnd != null)
                                    //{
                                    //    UnSelect();
                                    //    curMaze.cEnd.Select(true);
                                    //    propertyGrid1.SelectedObject = curMaze.cEnd;
                                    //}
                                    else if (curMaze.cEndRegions.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cEndRegions[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cEndRegions[0];
                                    }
                                    else if (curMaze.cLight.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cLight[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cLight[0];
                                    }
                                    else if (curMaze.cStaticModels.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cStaticModels[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cStaticModels[0];
                                    }
                                    else if (curMaze.cDynamicModels.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cDynamicModels[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cDynamicModels[0];
                                    }
                                    else if (curMaze.cWall.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cWall[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cWall[0];
                                    }
                                    else if (curMaze.cLight.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cLight[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cLight[0];
                                    }
                                    else if (curMaze.cStaticModels.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cStaticModels[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cStaticModels[0];
                                    }
                                    else if (curMaze.cDynamicModels.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cDynamicModels[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cDynamicModels[0];
                                    }
                                    else
                                    {
                                        UnSelect();
                                        curMaze.cFloor[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cFloor[0];
                                    }
                                    ts_delete.Enabled = true;
                                    throw new Exception();
                                }
                            }
                            if (curMaze.cStart != null && curMaze.cStart.IsSelected())                                                       
                            {
                                
                                //if (curMaze.cEnd != null)
                                //{
                                //    UnSelect();
                                //    curMaze.cEnd.Select(true);
                                //    propertyGrid1.SelectedObject = curMaze.cEnd;
                                //}
                                if (curMaze.cEndRegions.Count > 0 )
                                {
                                    UnSelect();
                                    curMaze.cEndRegions[0].Select(true);
                                    propertyGrid1.SelectedObject = curMaze.cEndRegions[0];
                                }
                                else if (curMaze.cLight.Count > 0)
                                {
                                    UnSelect();
                                    curMaze.cLight[0].Select(true);
                                    propertyGrid1.SelectedObject = curMaze.cLight[0];
                                }
                                else if (curMaze.cStaticModels.Count > 0)
                                {
                                    UnSelect();
                                    curMaze.cStaticModels[0].Select(true);
                                    propertyGrid1.SelectedObject = curMaze.cStaticModels[0];
                                }
                                else if (curMaze.cDynamicModels.Count > 0)
                                {
                                    UnSelect();
                                    curMaze.cDynamicModels[0].Select(true);
                                    propertyGrid1.SelectedObject = curMaze.cDynamicModels[0];
                                }
                                else if (curMaze.cWall.Count > 0)
                                {
                                    UnSelect();
                                    curMaze.cWall[0].Select(true);
                                    propertyGrid1.SelectedObject = curMaze.cWall[0];
                                }
                                else if (curMaze.cFloor.Count > 0)
                                {
                                    UnSelect();
                                    curMaze.cFloor[0].Select(true);
                                    propertyGrid1.SelectedObject = curMaze.cFloor[0];
                                }
                                ts_delete.Enabled = true;
                                throw new Exception();
                            }
                            //if (curMaze.cEnd != null && curMaze.cEnd.IsSelected())
                            for (int i = 0; i < curMaze.cEndRegions.Count; i++) 
                            {
                                if (curMaze.cEndRegions[i].IsSelected())
                                {
                                    if (curMaze.cLight.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cLight[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cLight[0];
                                    }
                                    else if (curMaze.cStaticModels.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cStaticModels[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cStaticModels[0];
                                    }
                                    else if (curMaze.cDynamicModels.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cDynamicModels[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cDynamicModels[0];
                                    }
                                    else if (curMaze.cWall.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cWall[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cWall[0];
                                    }
                                    else if (curMaze.cFloor.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cFloor[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cFloor[0];
                                    }
                                    else if (curMaze.cStart != null)
                                    {
                                        UnSelect();
                                        curMaze.cStart.Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cStart;
                                    }
                                    ts_delete.Enabled = true;
                                    throw new Exception();
                                }
                            }

                            for (int i = 0; i < curMaze.cLight.Count; i++)
                            {
                                if (curMaze.cLight[i].IsSelected())
                                {
                                    if (i + 1 < curMaze.cLight.Count)
                                    {
                                        UnSelect();
                                        curMaze.cLight[i + 1].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cLight[i + 1];
                                    }
                                    else if (curMaze.cStaticModels.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cStaticModels[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cStaticModels[0];
                                    }
                                    else if (curMaze.cDynamicModels.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cDynamicModels[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cDynamicModels[0];
                                    }
                                    else if (curMaze.cWall.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cWall[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cWall[0];
                                    }
                                    else if (curMaze.cFloor.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cFloor[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cFloor[0];

                                    }
                                    else if (curMaze.cStart != null)
                                    {
                                        UnSelect();
                                        curMaze.cStart.Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cStart;
                                    }
                                    //else if (curMaze.cEnd != null)
                                    //{
                                    //    UnSelect();
                                    //    curMaze.cEnd.Select(true);
                                    //    propertyGrid1.SelectedObject = curMaze.cEnd;
                                    //}      
                                    else if (curMaze.cEndRegions.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cEndRegions[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cEndRegions[0];
                                    }                           
                                    else
                                    {
                                        UnSelect();
                                        curMaze.cLight[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cLight[0];

                                    }
                                    ts_delete.Enabled = true;
                                    throw new Exception();
                                }
                            }


                            for (int i = 0; i < curMaze.cStaticModels.Count; i++)
                            {
                                if (curMaze.cStaticModels[i].IsSelected())
                                {
                                    if (i + 1 < curMaze.cStaticModels.Count)
                                    {
                                        UnSelect();
                                        curMaze.cStaticModels[i + 1].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cStaticModels[i + 1];
                                    } 
                                    else if (curMaze.cDynamicModels.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cDynamicModels[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cDynamicModels[0];
                                    }
                                    else if (curMaze.cWall.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cWall[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cWall[0];
                                    }
                                    else if (curMaze.cFloor.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cFloor[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cFloor[0];

                                    }
                                    else if (curMaze.cStart != null)
                                    {
                                        UnSelect();
                                        curMaze.cStart.Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cStart;
                                    }
                                    //else if (curMaze.cEnd != null)
                                    //{
                                    //    UnSelect();
                                    //    curMaze.cEnd.Select(true);
                                    //    propertyGrid1.SelectedObject = curMaze.cEnd;
                                    //}
                                    else if (curMaze.cEndRegions.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cEndRegions[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cEndRegions[0];
                                    }
                                    else if (curMaze.cLight.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cLight[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cLight[0];
                                    }
                                    else
                                    {
                                        UnSelect();
                                        curMaze.cStaticModels[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cStaticModels[0];

                                    }
                                    ts_delete.Enabled = true;
                                    throw new Exception();
                                }
                            }

                            for (int i = 0; i < curMaze.cDynamicModels.Count; i++)
                            {
                                if (curMaze.cDynamicModels[i].IsSelected())
                                {
                                    if (i + 1 < curMaze.cDynamicModels.Count)
                                    {
                                        UnSelect();
                                        curMaze.cDynamicModels[i + 1].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cDynamicModels[i + 1];
                                    }
                                    else if (curMaze.cWall.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cWall[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cWall[0];
                                    }
                                    else if (curMaze.cFloor.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cFloor[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cFloor[0];

                                    }
                                    else if (curMaze.cStart != null)
                                    {
                                        UnSelect();
                                        curMaze.cStart.Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cStart;
                                    }
                                    //else if (curMaze.cEnd != null)
                                    //{
                                    //    UnSelect();
                                    //    curMaze.cEnd.Select(true);
                                    //    propertyGrid1.SelectedObject = curMaze.cEnd;
                                    //}
                                    else if (curMaze.cEndRegions.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cEndRegions[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cEndRegions[0];
                                    }
                                    else if (curMaze.cLight.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cLight[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cLight[0];
                                    }
                                    else if (curMaze.cStaticModels.Count > 0)
                                    {
                                        UnSelect();
                                        curMaze.cStaticModels[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cStaticModels[0];
                                    }
                                    else
                                    {
                                        UnSelect();
                                        curMaze.cDynamicModels[0].Select(true);
                                        propertyGrid1.SelectedObject = curMaze.cDynamicModels[0];

                                    }
                                    ts_delete.Enabled = true;
                                    throw new Exception();
                                }
                            }

                        }
                        catch
                        {
                        }
                        RedrawFrame();
                    }
                    */
                    #endregion
                    break;
            }

        }

        private void enlargeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            try
            {
                curMaze.ResizeAllCoordinates(1.25);
                RedrawFrame();
                propertyGrid.Refresh();
                MazeChanged();
            }
            catch
            {
            }
        }

        private void shrinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            try
            {
                curMaze.ResizeAllCoordinates(0.8);
                RedrawFrame();
                propertyGrid.Refresh();
                MazeChanged();
            }
            catch
            {
            }
        }

        private void autoPlaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            try
            {
                curMaze.AutoFixPlacement();
                RedrawFrame();
                propertyGrid.Refresh();
                MazeChanged();
                SyncSelections();
            }
            catch
            {
            }
        }
        private void UnCheckAll()
        {
            foreach(ToolStripButton tp in toolStrip_maze.Items)
            {
                tp.Checked = false;
            }
        }

        private void ChangeModeTo0()
        {
            if (ts_maze_wall.Enabled == false)
                return;
            ts_delete.Enabled = false;
            switch (curMode)
            {
                case Mode.none:
                    curMode = Mode.none;
                    tabPageMazeEdit.Cursor = Cursors.Arrow;
                    UnCheckAll();
                    ts_maze_pointer.Checked = true;
                    break;
                case Mode.pan:
                    curMode = Mode.pan;
                    tabPageMazeEdit.Cursor = Cursors.NoMove2D;
                    UnCheckAll();
                    ts_maze_pan.Checked = true;
                    break;
                case Mode.wall0:
                    curMode = Mode.wall0;
                    tabPageMazeEdit.Cursor = cursorWall;
                    UnSelect();
                    UnCheckAll();
                    ts_maze_wall.Checked = true;
                    break;
                case Mode.wall1:
                    curMode = Mode.wall0;
                    tabPageMazeEdit.Cursor = cursorWall;
                    UnSelect();
                    UnCheckAll();
                    ts_maze_wall.Checked = true;
                    break;
                case Mode.curveWall0:
                    curMode = Mode.curveWall0;
                    tabPageMazeEdit.Cursor = cursorWall;
                    UnSelect();
                    UnCheckAll();
                    ts_maze_curve_wall.Checked = true;
                    break;
                case Mode.curveWall1:
                    curMode = Mode.curveWall0;
                    tabPageMazeEdit.Cursor = cursorWall;
                    UnSelect();
                    UnCheckAll();
                    ts_maze_curve_wall.Checked = true;
                    break;
                case Mode.curveWall2:
                    curMode = Mode.curveWall0;
                    tabPageMazeEdit.Cursor = cursorWall;
                    UnSelect();
                    UnCheckAll();
                    ts_maze_curve_wall.Checked = true;
                    break;
                case Mode.floor0:
                    UnCheckAll();
                    ts_maze_floor.Checked = true;
                    curMode = Mode.floor0;
                    tabPageMazeEdit.Cursor = cursorFloor;
                    UnSelect();
                    break;
                case Mode.floor1:
                    UnCheckAll();
                    ts_maze_floor.Checked = true;
                    curMode = Mode.floor0;
                    tabPageMazeEdit.Cursor = cursorFloor;
                    UnSelect();
                    break;
                case Mode.start:
                    curMode = Mode.start;
                    tabPageMazeEdit.Cursor = cursorStart;
                    UnSelect();
                    UnCheckAll();
                    ts_maze_start.Checked = true;
                    break;
                case Mode.end0:
                    curMode = Mode.end0;
                    tabPageMazeEdit.Cursor = cursorEnd;
                    UnSelect();
                    UnCheckAll();
                    ts_maz_end.Checked = true;
                    break;
                case Mode.actReg0:
                    curMode = Mode.actReg0;
                    tabPageMazeEdit.Cursor = cursorEnd;
                    UnSelect();
                    UnCheckAll();
                    ts_maz_actReg.Checked = true;
                    break;
                case Mode.actReg1:
                    curMode = Mode.actReg0;
                    tabPageMazeEdit.Cursor = cursorEnd;
                    UnSelect();
                    UnCheckAll();
                    ts_maz_actReg.Checked = true;
                    break;
                case Mode.end1:
                    curMode = Mode.end0;
                    tabPageMazeEdit.Cursor = cursorEnd;
                    UnSelect();
                    UnCheckAll();
                    ts_maz_end.Checked = true;
                    break;
                case Mode.light:
                    curMode = Mode.light;
                    tabPageMazeEdit.Cursor = cursorStart;
                    UnCheckAll();
                    ts_maz_light.Checked = true;
                    UnSelect();
                    break;
                case Mode.object0:
                    curMode = Mode.object0;
                    UnCheckAll();
                    ts_maz_object.Checked = true;
                    break;
                case Mode.object1:
                    curMode = Mode.object1;
                    break;
                case Mode.staticmodel:
                    curMode = Mode.staticmodel;
                    tabPageMazeEdit.Cursor = cursorStart;
                    UnCheckAll();
                    ts_maz_object.Checked = true;
                    UnSelect();
                    break;
                case Mode.dynamicmodel:
                    curMode = Mode.dynamicmodel;
                    tabPageMazeEdit.Cursor = cursorStart;
                    UnCheckAll();
                    ts_maz_dobject.Checked = true;
                    UnSelect();
                    break;
                case Mode.drag:
                    curMode = Mode.none;
                    tabPageMazeEdit.Cursor = Cursors.Arrow;
                    UnCheckAll();
                    ts_maz_dobject.Checked = true;
                    UnSelect();
                    break;
            }

            SyncSelections();
        }

        private void ChangeMode(Mode inp)
        {
            
            if(ts_maze_wall.Enabled==false)
                return;
            ts_delete.Enabled = false;
            switch (inp)
            {
                case Mode.none:
                    curMode = Mode.none;
                    tabPageMazeEdit.Cursor = Cursors.Arrow;
                    UnCheckAll();
                    ts_maze_pointer.Checked = true;                  
                    break;
                case Mode.pan:
                    curMode = Mode.pan;
                    tabPageMazeEdit.Cursor = Cursors.NoMove2D;
                    UnCheckAll();
                    ts_maze_pan.Checked = true;
                    break;
                case Mode.wall0:
                    curMode = Mode.wall0;
                    tabPageMazeEdit.Cursor = cursorWall;
                    UnSelect();
                    UnCheckAll();
                    ts_maze_wall.Checked = true;
                    break;
                case Mode.wall1:
                    curMode = Mode.wall1;
                    tabPageMazeEdit.Cursor = cursorWall;
                    break;
                case Mode.curveWall0:
                    curMode = Mode.curveWall0;
                    tabPageMazeEdit.Cursor = cursorWall;
                    UnSelect();
                    UnCheckAll();
                    ts_maze_curve_wall.Checked = true;
                    break;
                case Mode.curveWall1:
                    curMode = Mode.curveWall1;
                    tabPageMazeEdit.Cursor = cursorWall;
                    break;
                case Mode.curveWall2:
                    curMode = Mode.curveWall2;
                    tabPageMazeEdit.Cursor = cursorWall;
                    break;
                case Mode.floor0:
                    UnCheckAll();
                    ts_maze_floor.Checked = true;
                    curMode = Mode.floor0;
                    tabPageMazeEdit.Cursor = cursorFloor;
                    UnSelect();
                    break;
                case Mode.floor1:
                    curMode = Mode.floor1;
                    tabPageMazeEdit.Cursor = cursorFloor;
                    break;
                case Mode.start:
                    curMode = Mode.start;
                    tabPageMazeEdit.Cursor = cursorStart;
                    UnSelect();
                    UnCheckAll();
                    ts_maze_start.Checked = true;
                    break;
                case Mode.end0:
                    curMode = Mode.end0;
                    tabPageMazeEdit.Cursor = cursorEnd;
                    UnSelect();
                    UnCheckAll();
                    ts_maz_end.Checked = true;
                    break;
                case Mode.actReg0:
                    curMode = Mode.actReg0;
                    tabPageMazeEdit.Cursor = cursorEnd;
                    UnSelect();
                    UnCheckAll();
                    ts_maz_actReg.Checked = true;
                    break;
                case Mode.actReg1:
                    curMode = Mode.actReg1;
                    break;
                case Mode.end1:
                    curMode = Mode.end1;
                    break;
                case Mode.light:
                    curMode = Mode.light;
                    tabPageMazeEdit.Cursor = cursorStart;
                    UnCheckAll();
                    ts_maz_light.Checked = true;
                    UnSelect();
                    break;
                case Mode.object0:
                    curMode = Mode.object0;
                    UnCheckAll();
                    ts_maz_object.Checked = true;
                    break;
                case Mode.object1:
                    curMode = Mode.object1;
                    break;
                case Mode.staticmodel:
                    curMode = Mode.staticmodel;
                    tabPageMazeEdit.Cursor = cursorStart;
                    UnCheckAll();
                    ts_maz_object.Checked = true;
                    UnSelect();
                    break;
                case Mode.dynamicmodel:
                    curMode = Mode.dynamicmodel;
                    tabPageMazeEdit.Cursor = cursorStart;
                    UnCheckAll();
                    ts_maz_dobject.Checked = true;
                    UnSelect();
                    break;
            }
            SyncSelections();

        }


        public List<object> GetItemsInArea(int x, int y, bool regionSelect = false, int x2 = 0, int y2 = 0)  //Returns all objects in area
        {
            //hold Selection indicates that items that are selected are not unselected (ctrl)
            //regionSelect if for drag and drop (all items are added, none are unselected)

            List<object> itemsInArea=new List<object>();
            tabPageMazeEdit.Focus();


            if (curMaze == null)
                return itemsInArea;

            if (regionSelect == false)
            {
                x2 = x;
                y2 = y;
            }
           

            foreach (StartPos sPos in curMaze.cStart)
            {
                if (sPos.InRegion(x, y, x2, y2)) //If its in the region
                {
                    itemsInArea.Add(sPos);
                }
            }

            foreach (Light l in curMaze.cLight)
            {
                if (l.InRegion(x, y, x2, y2))
                {
                     itemsInArea.Add(l);
                }
            }

            foreach (StaticModel l in curMaze.cStaticModels)
            {
                if (l.InRegion(x, y, x2, y2))
                {
                     itemsInArea.Add(l);
                }
            }

            foreach (DynamicObject l in curMaze.cDynamicObjects)
            {
                if (l.InRegion(x, y, x2, y2))
                {
                     itemsInArea.Add(l);
                }
            }

            foreach (Wall w in curMaze.cWall)
            {
                if (w.InRegion(x, y, x2, y2))
                {
                        itemsInArea.Add(w);
                }
            }

            foreach (CurvedWall w in curMaze.cCurveWall)
            {
                if (w.InRegion(x, y, x2, y2))
                {
                    itemsInArea.Add(w);
                }
            }

            foreach (CustomObject c in curMaze.cObject)
            {
                if (c.InRegion(x, y, x2, y2))
                {

                    itemsInArea.Add(c);
                }
            }

 

            foreach (ActiveRegion en in curMaze.cActRegions)
            {
                if (en.InRegion(x, y, x2, y2))
                {
                    itemsInArea.Add(en); 
                }
            }



            foreach (EndRegion en in curMaze.cEndRegions)
            {
                if (en.InRegion(x, y, x2, y2))
                {
                    itemsInArea.Add(en);
                }
            }



            //check for floor if we don't have any fit before...
            foreach (Floor f in curMaze.cFloor)
            {
                if (f.InRegion(x, y, x2, y2))
                {
                    itemsInArea.Add(f);
                }
            }

            return itemsInArea; //Rebuilds selected item collection
        }


        public int CheckSelection(int x, int y, bool holdLastSelection = false, bool regionSelect = false, int x2 = 0, int y2 = 0)  //Selects & Unselects objects in an area
        {
            //hold Selection indicates that items that are selected are not unselected (ctrl)
            //regionSelect if for drag and drop (all items are added, none are unselected)

            bool allowUnselect = true; //selected items can be unselected
            tabPageMazeEdit.Focus();

            if (curMaze == null)
                return -1;

            if(holdLastSelection==false)
                UnSelect(true);

            if (regionSelect == false)
            { 
                x2 = x;
                y2 = y;
            }
            else
            {
                allowUnselect = false; 
            }

            int numAdded = 0; //Floors and Regions underlying regions are selected last, then walls, models and individual points are selected first
            //both unselect and select count as adding

            foreach (StartPos sPos in curMaze.cStart)
            { 
                if (sPos.InRegion(x, y, x2, y2)) //If its in the region
                {
                    if (sPos.IsSelected()) //and it is selected
                    {
                        if (allowUnselect) //trash it or do nothing
                        {
                            sPos.Select(false);
                            numAdded++;
                        }
                    }
                    else //otherwise select
                    {
                        sPos.Select(true);
                        numAdded++;
                    }                   
                }
            }

            foreach (Light l in curMaze.cLight)
            {
                if (l.InRegion(x, y, x2, y2))
                {
                    if (l.IsSelected())
                    {
                        if (allowUnselect)
                        {
                            l.Select(false);
                            numAdded++;
                        }
                    }
                    else
                    {
                        l.Select(true);
                        numAdded++;
                    }
                }
            }

            foreach (StaticModel l in curMaze.cStaticModels)
            {
                if (l.InRegion(x, y, x2, y2))
                {
                    if (l.IsSelected())
                    {
                        if (allowUnselect)
                        {
                            l.Select(false);
                            numAdded++;
                        }
                    }
                    else
                    {
                        l.Select(true);
                        numAdded++;
                    }
                }
            }

            foreach (DynamicObject l in curMaze.cDynamicObjects)
            {
                if (l.InRegion(x, y, x2, y2))
                {
                    if (l.IsSelected())
                    {
                        if (allowUnselect)
                        { 
                            l.Select(false);
                            numAdded++;
                        }
                    }
                    else
                    {
                        l.Select(true);
                        numAdded++;
                    }
                }
            }

            if (numAdded > 0 && !regionSelect) //if dealing with walls then deal with walls only
            {
                SyncSelections();
                return numAdded;
            }

            //if (!regionSelect)
            //{ 
            //    double value = 0, minValue = 0;
            //    Wall bestFit = null;
            //    foreach (Wall w in curMaze.cWall)
            //    {
            //        if (w.IfSelected(x, y, ref value))
            //        {
            //            if (value < minValue)
            //            {
            //                minValue = value;
            //                bestFit = w;
            //            }
            //        }
            //    }
            //    if (bestFit != null)
            //    {
            //        if (bestFit.IsSelected())
            //        {
            //            if (allowUnselect)
            //            { 
            //                bestFit.Select(false);
            //                numAdded++;
            //            }
            //        }
            //        else
            //        {
            //            bestFit.Select(true);
            //            numAdded++;
            //            //selected.Add(bestFit);
            //        }
            //    }

            //}
            //else
            //{

            foreach (Wall w in curMaze.cWall)
            {
                if (w.InRegion(x, y, x2, y2))
                {
                    if (w.IsSelected())
                    {
                        if (allowUnselect)
                        {
                            numAdded++;
                            w.Select(false);
                        }
                    }
                    else
                    {
                        w.Select(true);
                        numAdded++;
                    }
                }
            }

           
            foreach (CurvedWall w in curMaze.cCurveWall)
            {
                if (w.InRegion(x, y, x2, y2))
                {
                    if (w.IsSelected())
                    {
                        if (allowUnselect)
                        {
                            numAdded++;
                            w.Select(false);
                        }
                    }
                    else
                    {
                        w.Select(true);
                        numAdded++;
                    }
                }
            }

            if (numAdded > 0 && !regionSelect) //if dealing with walls then deal with walls only
            {
                SyncSelections();
                return numAdded;
            }



            foreach (CustomObject c in curMaze.cObject)
            {
                if (c.InRegion(x, y,x2,y2))
                {

                    if (c.IsSelected())
                    { 
                        if (allowUnselect)
                        { 
                            c.Select(false);
                            numAdded++;
                        }
                    }
                    else
                    {
                        c.Select(true);
                        numAdded++;
                        //selected.Add(c);
                    }
                }
            }

            if (numAdded > 0 && !regionSelect) //if dealing with walls then deal with walls only
            {
                SyncSelections();
                return numAdded;
            }

            foreach (ActiveRegion en in curMaze.cActRegions)
            {
                if (en.InRegion(x, y, x2, y2))
                {
                    if (en.IsSelected())
                    {
                        if (allowUnselect)
                        {
                            en.Select(false);
                            numAdded++;
                        }
                    }
                    else
                    {
                        en.Select(true);
                        numAdded++;
                    }
                }
            }

            if (numAdded > 0 && !regionSelect) //if dealing with walls then deal with walls only
            {
                SyncSelections();
                return numAdded;
            }

            foreach (EndRegion en in curMaze.cEndRegions)
            {
                if (en.InRegion(x, y,x2,y2))
                {
                    if (en.IsSelected())
                    {
                        if (allowUnselect)
                        {
                            en.Select(false);
                            numAdded++;
                        }
                    }
                    else
                    { 
                        en.Select(true);
                        numAdded++;
                    }
                }
            }



            if (numAdded > 0 && !regionSelect) //if dealing with walls then deal with walls only
            {
                SyncSelections();
                return numAdded;
            }

            //check for floor if we don't have any fit before...
            foreach (Floor f in curMaze.cFloor)
            {
                if (f.InRegion(x, y,x2,y2))
                {
                    if (f.IsSelected())
                    {
                        if (allowUnselect)
                        {
                            f.Select(false);
                            numAdded++;
                        }
                    }
                    else
                    {
                        f.Select(true);
                        numAdded++;
                        //selected.Add(f);
                    }
                }
            }


            SyncSelections();
            return numAdded; //Rebuilds selected item collection
        }

        List<Object> selected = new List<Object>();
        List<Object> mazeClipboard = new List<Object>();
        List<Object> redoItems = new List<Object>();
        Stack<Object> changedItemsStack = new Stack<Object>();
        Stack<Object> redoItemsStack = new Stack<Object>();
        List<Object> changedItems = new List<Object>();
        //List<Object> changedItems2 = new List<Object>();
        //List<Object> changedItems3 = new List<Object>();
        //List<Object> changedItems4 = new List<Object>();

        public static Stack<Object> trimStack(Stack<Object> orig,int numItems=10)
        {
            if (numItems < 0)
                numItems = 0;

            if (orig.Count < numItems)
                return orig;

            Stack<Object> trimmed = new Stack<Object>();

            for (int i=0;i<numItems;i++)
            {
                trimmed.Push(orig.Pop());
            }

            orig.Clear();

            for (int i = 0; i < numItems; i++)
            {
                orig.Push(trimmed.Pop());
            }

            trimmed.Clear();

            return orig;
        }

        public int SyncSelections() //Rebuilds selected item collection & Any changed items
        {
            bool bUpdateTree = false;

            selected = new List<Object>();
            if (curMaze != null)
            {
                if (preChangeMaze == null)
                    StorePreviousMaze();

                List<Object> tempChangedItems = new List<Object>();
                if (curMaze.cStart != null)
                {
                    
                    //curMaze.cStart.Select(false);
                    foreach(StartPos sPos in curMaze.cStart)
                    { 
                        if (sPos.IsSelected())
                            selected.Add(sPos);
                        if (sPos.changed)
                        {
                            if (sPos.IsDefaultStartPos)
                            {
                                foreach (StartPos sPos2 in curMaze.cStart)
                                { if (sPos2.IsDefaultStartPos) { sPos2.IsDefaultStartPos = false; sPos2.changed = true; } }
                                sPos.IsDefaultStartPos = true;
                                sPos.changed = true;
                                curMaze.DefaultStartPos = sPos;
                            }

                            int prevIndex = preChangeMaze.GetMazeItemByID(sPos.GetID(), MazeItemType.Start);
                            if (prevIndex >= 0)
                                tempChangedItems.Add(preChangeMaze.cStart[prevIndex].Copy(true));
                            else
                                tempChangedItems.Add(sPos.Copy(true));

                            

                            if(sPos.treeChanged)
                            {
                                sPos.treeChanged = false;
                                bUpdateTree = true;
                            }
                        }

                        sPos.changed = false;
                        sPos.justCreated = false;
                        
                    }
                }

                if (curMaze.cEndRegions != null)
                {
                    ////curMaze.cEnd.Select(false);
                    //if (curMaze.cEnd.IsSelected())
                    //    selected.Add(curMaze.cEnd);  
                    foreach (EndRegion en in curMaze.cEndRegions)
                    {
                        if (en.IsSelected())
                            selected.Add(en);
                        if (en.changed)
                        {
                            int prevIndex = preChangeMaze.GetMazeItemByID(en.GetID(), MazeItemType.End);
                            if (prevIndex >= 0)
                                tempChangedItems.Add(preChangeMaze.cEndRegions[prevIndex].Copy(true));
                            else
                                tempChangedItems.Add(en.Copy(true));
                            en.changed = false;
                            en.justCreated = false;
                        }
                        if (en.treeChanged)
                        {
                            en.treeChanged = false;
                            bUpdateTree = true;
                        }
                    }
                }

                if (curMaze.cActRegions != null)
                {
                    ////curMaze.cEnd.Select(false);
                    //if (curMaze.cEnd.IsSelected())
                    //    selected.Add(curMaze.cEnd);  
                    foreach (ActiveRegion en in curMaze.cActRegions)
                    {
                        if (en.IsSelected())
                            selected.Add(en);
                        if (en.changed)
                        {
                            int prevIndex = preChangeMaze.GetMazeItemByID(en.GetID(), MazeItemType.ActiveRegion);
                            if (prevIndex >= 0)
                                tempChangedItems.Add(preChangeMaze.cActRegions[prevIndex].Copy(true));
                            else
                                tempChangedItems.Add(en.Copy(true));
                            en.changed = false;
                            en.justCreated = false;
                        }
                        if (en.treeChanged)
                        {
                            en.treeChanged = false;
                            bUpdateTree = true;
                        }
                    }
                }

                if (curMaze.cLight != null)
                {
                    foreach (Light l in curMaze.cLight)
                    {
                        //l.Select(false);
                        if (l.IsSelected())
                            selected.Add(l);
                        if (l.changed)
                        {

                            int prevIndex = preChangeMaze.GetMazeItemByID(l.GetID(), MazeItemType.Light);
                            if (prevIndex >= 0)
                                tempChangedItems.Add(preChangeMaze.cLight[prevIndex].Copy(true));
                            else
                                tempChangedItems.Add(l.Copy(true));
                            l.changed = false;
                            l.justCreated = false;
                        }

                        if (l.treeChanged)
                        {
                            l.treeChanged = false;
                            bUpdateTree = true;
                        }
                    }
                }
                if (curMaze.cModels != null)
                {
                    foreach (StaticModel l in curMaze.cStaticModels)
                    {
                        // l.Select(false);
                        if (l.IsSelected())
                            selected.Add(l);
                        if (l.changed)
                        {
                            int prevIndex = preChangeMaze.GetMazeItemByID(l.GetID(), MazeItemType.Static);
                            if (prevIndex >= 0)
                                tempChangedItems.Add(preChangeMaze.cStaticModels[prevIndex].Copy(true));
                            else
                                tempChangedItems.Add(l.Copy(true));
                            l.changed = false;
                            l.justCreated = false;
                        }

                        if (l.treeChanged)
                        {
                            l.treeChanged = false;
                            bUpdateTree = true;
                        }
                    }
                }
                if (curMaze.cDynamicObjects != null)
                {
                    foreach (DynamicObject l in curMaze.cDynamicObjects)
                    {
                        if (l.IsSelected())
                            selected.Add(l);
                        if (l.changed)
                        {
                            int prevIndex = preChangeMaze.GetMazeItemByID(l.GetID(), MazeItemType.Dynamic);
                            if (prevIndex >= 0)
                                tempChangedItems.Add(preChangeMaze.cDynamicObjects[prevIndex].Copy(true));
                            else
                                tempChangedItems.Add(l.Copy(true));
                            l.changed = false;
                            l.justCreated = false;
                        }
                        if (l.treeChanged)
                        {
                            l.treeChanged = false;
                            bUpdateTree = true;
                        }
                    }
                }

                if (curMaze.cWall != null)
                {
                    foreach (Wall w in curMaze.cWall)
                    {
                        if (w.IsSelected())
                            selected.Add(w);
                        if (w.changed)
                        {
                            int prevIndex = preChangeMaze.GetMazeItemByID(w.GetID(), MazeItemType.Wall);
                            if (prevIndex >= 0)
                                tempChangedItems.Add(preChangeMaze.cWall[prevIndex].Copy(true));
                            else
                                tempChangedItems.Add(w.Copy(true));
                            w.changed = false;
                            w.justCreated = false;
                        }
                        if (w.treeChanged)
                        {
                            w.treeChanged = false;
                            bUpdateTree = true;
                        }
                    }
                }
                if (curMaze.cCurveWall != null)
                {
                    foreach (CurvedWall w in curMaze.cCurveWall)
                    {
                        if (w.IsSelected())
                            selected.Add(w);
                        if (w.changed)
                        {
                            int prevIndex = preChangeMaze.GetMazeItemByID(w.GetID(), MazeItemType.CurvedWall);
                            if (prevIndex >= 0)
                                tempChangedItems.Add(preChangeMaze.cCurveWall[prevIndex].Copy(true));
                            else
                                tempChangedItems.Add(w.Copy(true));
                            w.changed = false;
                            w.justCreated = false;
                        }
                        if (w.treeChanged)
                        {
                            w.treeChanged = false;
                            bUpdateTree = true;
                        }
                    }
                }
                if (curMaze.cObject != null)
                {
                    foreach (CustomObject c in curMaze.cObject)
                    {
                        if (c.IsSelected())
                            selected.Add(c);
                        if (c.changed)
                        {
                            /*  int prevIndex = GetMazeItemByID(c.GetID(), MazeItemType.CustomObject, true);
                                if (prevIndex >= 0)
                                    tempChangedItems.Add(preChangeMaze.cObject[prevIndex].Copy(true));
                                else
                                    tempChangedItems.Add(c.Copy(true));
                                c.changed = false;
                                c.justCreated = false;*/
                        }
                        //if (c.treeChanged)
                        //{
                         //   c.treeChanged = false;
                          //  bUpdateTree = true;
                        //}
                    }
                }
                if (curMaze.cFloor != null)
                {
                    foreach (Floor f in curMaze.cFloor)
                    {
                        if (f.IsSelected())
                            selected.Add(f);
                        if (f.changed)
                        {
                            int prevIndex = preChangeMaze.GetMazeItemByID(f.GetID(), MazeItemType.Floor);
                            if (prevIndex >= 0)
                                tempChangedItems.Add(preChangeMaze.cFloor[prevIndex].Copy(true));
                            else
                                tempChangedItems.Add(f.Copy(true));
                            f.changed = false;
                            f.justCreated = false;
                        }
                        if (f.treeChanged)
                        {
                            f.treeChanged = false;
                            bUpdateTree = true;
                        }
                    }
                }

                if (bUpdateTree)
                    UpdateTree();

                if (tempChangedItems.Count > 0)
                {
                    StorePreviousMaze();
                    //changedItems4 = changedItems3;
                    //changedItems3 = changedItems2;
                    //changedItems2 = changedItems;
                    changedItemsStack.Push(changedItems);
                    if (changedItemsStack.Count > 15)
                        changedItemsStack = trimStack(changedItemsStack, 10);
                    changedItems = tempChangedItems;

                    if (redoItemsStack.Count == 0)
                    { 
                        redoItems = new List<Object>();
                        redoItemsStack.Clear();
                    }
                    else
                        redoItems = (List<Object>) redoItemsStack.Pop();
                    
                }


                if (selected.Count == 0)
                {
                    propertyGrid.SelectedObject = curMaze;
                    ts_delete.Enabled = false;
                }
                else if (selected.Count == 1)
                {
                    propertyGrid.SelectedObject = selected[0];
                    ts_delete.Enabled = true;
                }
                else
                {
                    propertyGrid.SelectedObjects = selected.ToArray();
                    ts_delete.Enabled = true;
                }
               
            }
            return selected.Count;
        }

        public void StorePreviousMaze()
        {
            if (curMaze == null)
                return;

            if (preChangeMaze == null)
            {
                preChangeMaze = new Maze();
                preChangeMaze.cWall = new List<Wall>();
                preChangeMaze.cCurveWall = new List<CurvedWall>();
                preChangeMaze.cFloor = new List<Floor>();
                preChangeMaze.cEndRegions = new List<EndRegion>();
                preChangeMaze.cObject = new List<CustomObject>();
                preChangeMaze.cStaticModels = new List<StaticModel>();
                preChangeMaze.cDynamicObjects= new List<DynamicObject>();
                preChangeMaze.cLight = new List<Light>();
                preChangeMaze.cStart = new List<StartPos>();

            }

            preChangeMaze.cLight= curMaze.cLight.ConvertAll(light => light.Clone());
            preChangeMaze.cWall = curMaze.cWall.ConvertAll(wall => wall.Clone());
            preChangeMaze.cCurveWall = curMaze.cCurveWall.ConvertAll(wall => wall.Clone());
            preChangeMaze.cFloor = curMaze.cFloor.ConvertAll(floor => floor.Clone());
            preChangeMaze.cEndRegions = curMaze.cEndRegions.ConvertAll(endRegion => endRegion.Clone());
            preChangeMaze.cObject = curMaze.cObject.ConvertAll(cObject => cObject.Clone());
            preChangeMaze.cStaticModels = curMaze.cStaticModels.ConvertAll(staticModel => staticModel.Clone());
            preChangeMaze.cDynamicObjects = curMaze.cDynamicObjects.ConvertAll(dynamicModel => dynamicModel.Clone());
            preChangeMaze.cStart = curMaze.cStart.ConvertAll(startPos => startPos.Clone());

        }

        public void UnSelect()
        {
            UnSelect(false);
        }

        public void SelectAll()
        {
            if (curMaze != null)
            {
                foreach (StartPos sPos in curMaze.cStart)
                {
                    sPos.Select(true);
                }
                //if (curMaze.cEnd != null)
                //    curMaze.cEnd.Select(true);
                foreach (EndRegion en in curMaze.cEndRegions)
                {
                    en.Select(true);
                }
                foreach (ActiveRegion en in curMaze.cActRegions)
                {
                    en.Select(true);
                }
                foreach (Wall w in curMaze.cWall)
                {
                    w.Select(true);
                }
                foreach (CurvedWall w in curMaze.cCurveWall)
                {
                    w.Select(true);
                }
                foreach (CustomObject c in curMaze.cObject)
                {
                    c.Select(true);
                }
                foreach (Floor f in curMaze.cFloor)
                {
                    f.Select(true);
                }
                foreach (Light s in curMaze.cLight)
                {
                    s.Select(true);
                }
                foreach (StaticModel s in curMaze.cStaticModels)
                {
                    s.Select(true);
                }
                foreach (DynamicObject d in curMaze.cDynamicObjects)
                {
                    d.Select(true);
                }
            }
            SyncSelections();
            RedrawFrame();
        }

        public void UnSelect(bool force)
        {            
            if (force==false && (bCTRLdown )) return;
            if (curMaze != null)
            {
                foreach (StartPos s in curMaze.cStart)
                {
                    s.Select(false);
                }
                //if (curMaze.cEnd != null)
                //    curMaze.cEnd.Select(false);
                foreach (EndRegion en in curMaze.cEndRegions)
                {
                    en.Select(false);
                }
                foreach (ActiveRegion en in curMaze.cActRegions)
                {
                    en.Select(false);
                }
                foreach (Wall w in curMaze.cWall)
                {
                    w.Select(false);
                }
                foreach (CurvedWall w in curMaze.cCurveWall)
                {
                    w.Select(false);
                }
                foreach (CustomObject c in curMaze.cObject)
                {
                    c.Select(false);
                }
                foreach (Floor f in curMaze.cFloor)
                {
                    f.Select(false);
                }
                foreach (Light s in curMaze.cLight)
                {
                    s.Select(false);
                }
                foreach (StaticModel s in curMaze.cStaticModels)
                {
                    s.Select(false);
                }
                foreach (DynamicObject d in curMaze.cDynamicObjects)
                {
                    d.Select(false);
                }
            }
            ts_delete.Enabled = false;
            propertyGrid.SelectedObject = curMaze;
            SyncSelections();
            RedrawFrame();
        }

        public void toogleElementsToolStrip(bool inp)
        {
                foreach (ToolStripItem w in toolStrip_maze.Items)
                {
                    w.Enabled = inp;
                }

                textureCollectionToolStripMenuItem.Enabled = inp;
                modelCollectionToolStripMenuItem.Enabled = inp;
                audioCollectionToolStripMenuItem1.Enabled = inp;

                toolStripDropDownButtonRunConfig.Enabled = inp;
                toolStripButtonQuickRun.Enabled = inp;
                toolStripCollectionsDropDownButton.Enabled = inp;
                toolStripButtonItems.Enabled = inp;

        }

        List<string[]> replaceOrder = new List<string[]>();
        bool manageItem = false;
        string previousFile = "";
        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.GetType() == e.ChangedItem.Label.GetType())
                MazeChanged(true);
            else
                MazeChanged(false);

            if (propertyGrid.SelectedObject.GetType().Name != "Maze")
            // Manage Items Dropdown
            {
                foreach (object selectedObject in propertyGrid.SelectedObjects)
                {
                    MazeItem mazeItem = (MazeItem)selectedObject;
                    switch (mazeItem.itemType)
                    {
                        case MazeItemType.Floor:
                            Floor floor = (Floor)mazeItem;
                            floor.FloorTexture = ManageItems("Image", e.OldValue, floor.FloorTexture);
                            floor.CeilingTexture = ManageItems("Image", e.OldValue, floor.CeilingTexture);
                            break;

                        case MazeItemType.CurvedWall:
                            CurvedWall curvedWall = (CurvedWall)mazeItem;
                            curvedWall.Texture = ManageItems("Image", e.OldValue, curvedWall.Texture);
                            break;

                        case MazeItemType.Wall:
                            Wall wall = (Wall)mazeItem;
                            wall.Texture = ManageItems("Image", e.OldValue, wall.Texture);
                            break;

                        case MazeItemType.ActiveRegion:
                            ActiveRegion activeRegion = (ActiveRegion)mazeItem;
                            activeRegion.Phase1HighlightAudio = ManageItems("Audio", e.OldValue, activeRegion.Phase1HighlightAudio);
                            activeRegion.Phase2EventAudio = ManageItems("Audio", e.OldValue, activeRegion.Phase2EventAudio);
                            break;

                        case MazeItemType.Dynamic:
                            DynamicObject dynamicObject = (DynamicObject)mazeItem;

                            dynamicObject.Phase1HighlightAudio = ManageItems("Audio", e.OldValue, dynamicObject.Phase1HighlightAudio);
                            dynamicObject.Phase2EventAudio = ManageItems("Audio", e.OldValue, dynamicObject.Phase2EventAudio);

                            dynamicObject.Model = ManageItems("Model", e.OldValue, dynamicObject.Model);
                            dynamicObject.SwitchToModel = ManageItems("Model", e.OldValue, dynamicObject.SwitchToModel);
                            break;

                        case MazeItemType.Static:
                            StaticModel staticModel = (StaticModel)mazeItem;
                            staticModel.Model = ManageItems("Model", e.OldValue, staticModel.Model);
                            break;
                    }
                }
            }
            else
            {
                curMaze.SkyBoxTexture = ManageItems("Image", e.OldValue, curMaze.SkyBoxTexture);
                curMaze.AvatarModel = ManageItems("Model", e.OldValue, curMaze.AvatarModel);
            }

           
            curMaze.ReplaceFiles(replaceOrder);
            manageItem = false;
        }

        string ManageItems(string type, object oldValue, string newValue)
        {
            if (manageItem == true && propertyGrid.SelectedObjects.Length > 1)
                return previousFile;

            string fileName = "";
            
            switch (newValue)
            {
                case "[Import Item]":
                    manageItem = true;

                    OpenFileDialog ofd = new OpenFileDialog();
                    switch (type)
                    {
                        case "Image":
                            ofd.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.gif;*.png)|*.bmp;*.jpg;*.jpeg;*.gif;*.png";
                            break;

                        case "Audio":
                            ofd.Filter = "Audio Files (*.wav;*.mp3)|*.wav;*.mp3";
                            break;

                        case "Model":
                            ofd.Filter = "Model Files (*.obj)|*.obj";
                            break;
                    }

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        fileName = ofd.FileName.Substring(ofd.FileName.LastIndexOf("\\") + 1);
                        if (fileName == "")
                            fileName = ofd.FileName;

                        switch (type)
                        {
                            case "Image":
                                ImagePathConverter.Paths[fileName] = ofd.FileName;
                                break;

                            case "Audio":
                                AudioPathConverter.Paths[fileName] = ofd.FileName;
                                break;

                            case "Model":
                                ModelPathConverter.Paths[fileName] = ofd.FileName;
                                break;
                        }

                        previousFile = fileName;
                        return fileName;
                    }

                    previousFile = (string)oldValue;
                    return (string)oldValue;

                case "[Manage Items]":
                    manageItem = true;

                    switch (type)
                    {
                        case "Image":
                            CollectionEditor collection = new CollectionEditor(MazeListBuilder.FilesToTextures(ImagePathConverter.Paths));

                            fileName = collection.GetTexture();
                            MazeListBuilder.TexturesToFiles(collection.GetTextures(), ref ImagePathConverter.Paths);
                            replaceOrder = collection.GetReplaceOrder();

                            break;

                        case "Audio":
                            collection = new CollectionEditor(MazeListBuilder.FilesToAudios(AudioPathConverter.Paths));

                            fileName = collection.GetAudio();
                            MazeListBuilder.AudiosToFiles(collection.GetAudios(), ref AudioPathConverter.Paths);
                            replaceOrder = collection.GetReplaceOrder();

                            break;

                        case "Model":
                            collection = new CollectionEditor(MazeListBuilder.FilesToModels(ModelPathConverter.Paths));

                            fileName = collection.GetModel();
                            MazeListBuilder.ModelsToFiles(collection.GetModels(), ref ModelPathConverter.Paths);
                            replaceOrder = collection.GetReplaceOrder();

                            break;
                    }

                    if (fileName != "")
                    {
                        previousFile = fileName;
                        return fileName;
                    }
                    previousFile = (string)oldValue;
                    return (string)oldValue;

                case "----------------------------------------":
                    return (string)oldValue;

                default:
                    if (type == "Image" && newValue != "" && !ImagePathConverter.Paths.ContainsKey(newValue))
                        ImagePathConverter.Paths[newValue] = newValue;

                    else if (type == "Audio" && newValue != "" && !AudioPathConverter.Paths.ContainsKey(newValue))
                        AudioPathConverter.Paths[newValue] = newValue;

                    else if (type == "Model" && newValue != "" && !ModelPathConverter.Paths.ContainsKey(newValue))
                        ModelPathConverter.Paths[newValue] = newValue;

                    return newValue;
            }
        }

        private void Package(object sender, EventArgs e)
        {
            ChangeModeTo0();
            if (curMaze == null)
                return;
            if (curMaze.FileName == null)
                SaveAs();
            else
                SaveMaze();

            SaveFileDialog sfd = new SaveFileDialog{ Filter = "Maze Files (*.maz)|*.maz|Maze Package Files (*.mazx)|*.mazx" };
            if (prevSaveDirMaze != "")
            {
                sfd.InitialDirectory = prevSaveDirMaze;
            }

            bool zip = false;

            string directory = "";
            string fileName = "";
            string fileExt = ".maz";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(sfd.FileName).ToLower() == ".mazx")
                    zip = true;


                directory = Path.GetDirectoryName(sfd.FileName);
                fileName = Path.GetFileName(sfd.FileName).Split('.')[0] + fileExt;
                sfd.FileName = directory + "\\" + fileName;

                //curMaze.SaveToMazeXML(sfd.FileName);  
                prevSaveDirMaze = Path.GetDirectoryName(sfd.FileName);
                this.Text = "MazeMaker - " + sfd.FileName;
                CurrentSettings.AddMazeFileToPrevious(sfd.FileName);
                UpdateTree();
            }

            string mazPath = sfd.FileName;
       
            string copiedFiles = "";


            //Instead of everything below, use Package from mazelib, maze.cs

            bool successPackage=curMaze.Package(mazPath, out copiedFiles, replaceOrder, zip);//, zip);

            
                //curMaze.SaveToMazeXML(sfd.FileName);

            if(!successPackage)
                MazeListBuilder.ShowPM(mazPath, "\nPackage failed!", copiedFiles);
            else if (zip)
            {
                MazeListBuilder.ShowPM(mazPath, "\nZipping files...\n" + mazPath + "\nPackage successfully generated", copiedFiles);
            }
            else
            {
                MazeListBuilder.ShowPM(mazPath, "\nPackage successfully generated", copiedFiles);
            }
        }

        

        private void Extract(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Maze Package Files (*.mazx)|*.mazx",
                Title = "Open Maze Package Files",
                InitialDirectory = prevSaveDirMaze,
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Maze Files (*.maz)|*.maz",
                    Title = "Save Maze Files",
                    InitialDirectory = prevSaveDirMaze,
                };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string directory = Path.GetDirectoryName(sfd.FileName);
                    string extractDir = directory + "\\Temp";

                    if (Directory.Exists(extractDir))
                        Directory.Delete(extractDir, true);
                    ZipFile.ExtractToDirectory(ofd.FileName, extractDir);

                    string oldFileName_NoExt = Path.GetFileName(ofd.FileName).Split('.')[0];
                    string oldMaz = extractDir + "\\" + oldFileName_NoExt + ".maz";
                    string oldAssets = extractDir + "\\" + oldFileName_NoExt + ".maz_assets";
                    string newAssets = sfd.FileName + "_assets";

                    if (File.Exists(sfd.FileName))
                        File.Delete(sfd.FileName);
                    Directory.Move(oldMaz, sfd.FileName);
                    if (Directory.Exists(newAssets))
                        Directory.Delete(newAssets, true);
                    Directory.Move(oldAssets, newAssets);
                    Directory.Delete(extractDir, true);
                    prevSaveDirMaze = Path.GetDirectoryName(sfd.FileName);
                    OpenTheFile(sfd.FileName);
                }
            }
        }

        [DllImport("user32.dll")]
        private static extern uint GetMessageExtraInfo();

        public static bool IsTouchOrPen()
        {
            uint extra = GetMessageExtraInfo();
            bool bIsTouchOrPen = ((extra & 0xFFFFFF00) == 0xFF515700);

            return bIsTouchOrPen;
            //if (!isTouchOrPen)
            //  return MouseEventSource.Mouse;

            //bool isTouch = ((extra & 0x00000080) == 0x00000080);

            //return isTouch ? MouseEventSource.Touch : MouseEventSource.Pen;
        }


        private void tabPageMazeEdit_MouseMove(object sender, MouseEventArgs e)
        {
             
            float dist2FromLast = (e.X - initMouseDownLocation.X)* (e.X - initMouseDownLocation.X) + (e.Y - initMouseDownLocation.Y)* (e.Y - initMouseDownLocation.Y);
            float minDist2 = 200;//minimum distance in pixels squared
            PointF p = new PointF(0, 0);

            if(dist2FromLast>minDist2&& curMode == Mode.none&&!bMiddleMouseDown)
            {
             

                if (selected.Count > 0 && !bCTRLdown && bMouseDown ) //enable drag if >10 pixels, mouse is down, and objects are selected
                {

                    curMode = Mode.drag;
                    tempPoint1 = new PointF(e.X, e.Y);
                    tempPoint2 = tempPoint1;
                    tempPoint3 = tempPoint2;
                    tempPoint4 = tempPoint3;
                }
                else if (bMouseDown&&!panelWelcome.Visible)
                {
                    if (!bCTRLdown)
                        UnSelect(true);

                    tempPoint1 = new PointF(e.X, e.Y);
                    tempPoint2 = tempPoint1;
                    tempPoint3 = tempPoint2;
                    tempPoint4 = tempPoint3;
                    curMode = Mode.multiSelect;
                }
            }

            curMouseLocation = e.Location;

            if (((curMode == Mode.pan&& bMouseDown)||bMiddleMouseDown) && !panelWelcome.Visible)
            {
                tabPageMazeEdit.Cursor = Cursors.NoMove2D;
                if (!IsTouchOrPen()&&false)
                { 
                    iViewOffsetX -= curMouseLocation.X - Point.Round(initMouseDownLocation).X;
                    iViewOffsetY -= curMouseLocation.Y - Point.Round(initMouseDownLocation).Y;
                }
                else
                {
                    iViewOffsetX += curMouseLocation.X - Point.Round(initMouseDownLocation).X;
                    iViewOffsetY += curMouseLocation.Y - Point.Round(initMouseDownLocation).Y;
                }
                initMouseDownLocation = curMouseLocation;

                RedrawFrame();
            }


            
            UpdateOffsetReport();

            hotpoint = false;

            switch (curMode)
            {  
                case Mode.wall0:
                    //if( Keys.Control ==false)
                    hotpoint = curMaze.CheckForClosePoint(e.X, e.Y, ref tempPoint1, iViewOffsetX, iViewOffsetY);
                    tabPageMazeEdit.Invalidate(new Rectangle(e.X - 300, e.Y - 300, 600, 600));
                    break;
                case Mode.wall1:
                    p = new PointF(0, 0);
                    if (curMaze.CheckForClosePoint(e.X, e.Y, ref p, iViewOffsetX, iViewOffsetY) == true)
                    {
                        tempPoint2 = new PointF(p.X, p.Y);
                    }
                    else
                    {

                            float xoff = ((float)iViewOffsetX / GridStepX) % GridStepX;
                            float yoff = ((float)iViewOffsetY / GridStepY) % GridStepY;
     

                        tempPoint2 = new PointF(e.X, e.Y);

                        tempPoint2.X = ((int)(tempPoint2.X / GridStepX)) * GridStepX;//+(int)Math.Floor(xoff);
                        tempPoint2.Y = ((int)(tempPoint2.Y / GridStepY)) * GridStepY;// + (int)Math.Floor(yoff);
                    }

                    //RedrawFrame();
                    //tabPageMazeEdit.Invalidate(new Rectangle(Math.Min(tempPoint1.X, e.X) - 20, Math.Min(tempPoint1.Y, e.Y) - 20, Math.Abs(tempPoint1.X - e.X) + 40, Math.Abs(tempPoint1.Y - e.Y) + 40));
                    tabPageMazeEdit.Invalidate(new Rectangle((int)Math.Min(tempPoint1.X, tempPoint2.X) - 200, (int) Math.Min(tempPoint1.Y, tempPoint2.Y) - 200, (int) Math.Abs(tempPoint1.X - tempPoint2.X) + 400, (int)Math.Abs(tempPoint1.Y - tempPoint2.Y) + 400));
                    break;
                case Mode.curveWall0:
                    //if( Keys.Control ==false)
                    hotpoint = curMaze.CheckForClosePoint(e.X, e.Y, ref tempPoint1, iViewOffsetX, iViewOffsetY);
                    tabPageMazeEdit.Invalidate(new Rectangle(e.X - 600, e.Y - 600, 1200, 1200));
                    break;
                case Mode.curveWall1:
                    p = new PointF(0, 0);
                    if (curMaze.CheckForClosePoint(e.X, e.Y, ref p, iViewOffsetX, iViewOffsetY) == true)
                    {
                        tempPoint2 = new PointF(p.X, p.Y);
                    }
                    else
                    {
                        tempPoint2 = new PointF(e.X, e.Y);

                        tempPoint2.X = ((int)(tempPoint2.X / GridStepX)) * GridStepX;
                        tempPoint2.Y = ((int)(tempPoint2.Y / GridStepY)) * GridStepY;
                    }

                    //RedrawFrame();
                    //tabPageMazeEdit.Invalidate(new Rectangle(Math.Min(tempPoint1.X, e.X) - 20, Math.Min(tempPoint1.Y, e.Y) - 20, Math.Abs(tempPoint1.X - e.X) + 40, Math.Abs(tempPoint1.Y - e.Y) + 40));
                    tabPageMazeEdit.Invalidate(new Rectangle((int)Math.Min(tempPoint1.X, tempPoint2.X) - 600, (int)Math.Min(tempPoint1.Y, tempPoint2.Y) - 600, (int)Math.Abs(tempPoint1.X - tempPoint2.X) + 1200, (int)Math.Abs(tempPoint1.Y - tempPoint2.Y) + 1200));
                    break;
                case Mode.curveWall2:
                    p = new PointF(0, 0);
                    if (curMaze.CheckForClosePoint(e.X, e.Y, ref p, iViewOffsetX, iViewOffsetY) == true)
                    {
                        tempPoint3 = new PointF(p.X, p.Y);
                    }
                    else
                    {
                        tempPoint3 = new PointF(e.X, e.Y);

                        tempPoint3.X = ((int)(tempPoint3.X / GridStepX)) * GridStepX;
                        tempPoint3.Y = ((int)(tempPoint3.Y / GridStepY)) * GridStepY;
                    }

                    //RedrawFrame();
                    //tabPageMazeEdit.Invalidate(new Rectangle(Math.Min(tempPoint1.X, e.X) - 20, Math.Min(tempPoint1.Y, e.Y) - 20, Math.Abs(tempPoint1.X - e.X) + 40, Math.Abs(tempPoint1.Y - e.Y) + 40));
                    tabPageMazeEdit.Invalidate(new Rectangle((int)Math.Min(tempPoint1.X, tempPoint2.X) - 600, (int)Math.Min(tempPoint1.Y, tempPoint2.Y) - 600, (int)Math.Abs(tempPoint1.X - tempPoint2.X) + 1200, (int)Math.Abs(tempPoint1.Y - tempPoint2.Y) + 1200));
                    break;
                case Mode.actReg1:
                    tempPoint2 = new PointF(e.X, e.Y);
                    tabPageMazeEdit.Invalidate(new Rectangle((int)Math.Min(tempPoint1.X, tempPoint2.X) - 200, (int)Math.Min(tempPoint1.Y, tempPoint2.Y) - 200, (int)Math.Abs(tempPoint1.X - tempPoint2.X) + 400, (int)Math.Abs(tempPoint1.Y - tempPoint2.Y) + 400));

                    //RedrawFrame();
                    break;
                case Mode.floor1:
                    tempPoint2 = new PointF(e.X, e.Y);
                    tabPageMazeEdit.Invalidate(new Rectangle((int)Math.Min(tempPoint1.X, tempPoint2.X) - 200, (int)Math.Min(tempPoint1.Y, tempPoint2.Y) - 200, (int)Math.Abs(tempPoint1.X - tempPoint2.X) + 400, (int)Math.Abs(tempPoint1.Y - tempPoint2.Y) + 400));
                    //tabPageMazeEdit.Invalidate(new Rectangle((int)Math.Min(tempPoint1.X, tempPoint2.X) - 20, (int)Math.Min(tempPoint1.Y, tempPoint2.Y) - 20, (int)Math.Abs(tempPoint1.X - tempPoint2.X) + 40, (int)Math.Abs(tempPoint1.Y - tempPoint2.Y) + 400));
                    break;
                case Mode.end1:
                    tempPoint2 = new PointF(e.X, e.Y);
                    tabPageMazeEdit.Invalidate(new Rectangle((int)Math.Min(tempPoint1.X, tempPoint2.X) - 200, (int)Math.Min(tempPoint1.Y, tempPoint2.Y) - 200, (int)Math.Abs(tempPoint1.X - tempPoint2.X) + 400, (int)Math.Abs(tempPoint1.Y - tempPoint2.Y) + 400));

                    //RedrawFrame();
                    break;
                case Mode.object1:
                    tempPoint2 = new PointF(e.X, e.Y);
                    RedrawFrame();
                    break;
                case Mode.multiSelect:
                    tempPoint2 = new PointF(e.X, e.Y);
                    tabPageMazeEdit.Invalidate(new Rectangle((int)Math.Min(tempPoint1.X, tempPoint2.X) - 200, (int)Math.Min(tempPoint1.Y, tempPoint2.Y) - 200, (int)Math.Abs(tempPoint1.X - tempPoint2.X) + 400, (int)Math.Abs(tempPoint1.Y - tempPoint2.Y) + 400));
                    //tabPageMazeEdit.Invalidate(new Rectangle((int)Math.Min(tempPoint1.X, tempPoint2.X) - 20, (int)Math.Min(tempPoint1.Y, tempPoint2.Y) - 20, (int)Math.Abs(tempPoint1.X - tempPoint2.X) + 40, (int)Math.Abs(tempPoint1.Y - tempPoint2.Y) + 400));
                    break;
                case Mode.drag:
                    tempPoint2 = new PointF(e.X, e.Y);
                   
                    RedrawFrame();
                    break;
            }
            
            

        }

        private void tabPageMazeEdit_MouseUp(object sender, MouseEventArgs e)
        {
            float dist2FromLast = (e.X - tempPoint1.X) * (e.X - tempPoint1.X) + (e.Y - tempPoint1.Y) * (e.Y - tempPoint1.Y);
            float minDist2 = 225; //minimum distance in pixels squared

            bMouseDown = false;
            if (bMiddleMouseDown)
            {bMiddleMouseDown= false;
                ChangeModeTo0();
            }

            

            
            switch(curMode)
            {
                case Mode.multiSelect:
                {
                        if (e.Button == MouseButtons.Left)
                        {
                            curMode = Mode.none;
                            int minX, minY, maxX, maxY;
                            minX = (int) Math.Min(tempPoint1.X, tempPoint2.X) - iViewOffsetX;
                            maxX = (int) Math.Max(tempPoint1.X, tempPoint2.X) - iViewOffsetX;
                            minY = (int) Math.Min(tempPoint1.Y, tempPoint2.Y) - iViewOffsetY;
                            maxY = (int) Math.Max(tempPoint1.Y, tempPoint2.Y) - iViewOffsetY;

                            tempPoint2 = tempPoint1;
                            CheckSelection(minX , minY, bCTRLdown, true,maxX,maxY);
                            RedrawFrame();
                            return;
                        }
                        break;
                }
                case Mode.drag:
                    if (e.Button == MouseButtons.Left)
                    {
                        //ChangeMode(Mode.none); 
                        curMode = Mode.none;
                        FindSelectedAndMove(e.X,e.Y,(e.X - initMouseDownLocation.X ), (e.Y - initMouseDownLocation.Y ), false);
                        
                        RedrawFrame();
                        return;
                    }
                    break;
                case Mode.wall1:
                    if (e.Button == MouseButtons.Left && dist2FromLast> minDist2)
                    {
                        if (curMaze.CheckForClosePoint(e.X, e.Y, ref tempPoint2, iViewOffsetX, iViewOffsetY) == false)
                        {
                            tempPoint2 = new PointF(e.X, e.Y);

                            tempPoint2.X = ((int)(tempPoint2.X / GridStepX)) * GridStepX;
                            tempPoint2.Y = ((int)(tempPoint2.Y / GridStepY)) * GridStepY;
                        }

                        Wall temp = new Wall(curMaze.Scale);
                        tempPoint1.X -= iViewOffsetX;
                        tempPoint1.Y -= iViewOffsetY;
                        tempPoint2.X -= iViewOffsetX;
                        tempPoint2.Y -= iViewOffsetY;
                        if (tempPoint1.X < tempPoint2.X)
                        {
                            temp.ScrPoint1 = tempPoint1;
                            temp.ScrPoint2 = tempPoint2;
                        }
                        else
                        {
                            temp.ScrPoint1 = tempPoint2;
                            temp.ScrPoint2 = tempPoint1;
                        }

                        curMaze.cWall.Add(temp);
                        //ChangeMode(Mode.wall0); //We want to go connected...
                        tempPoint2.X += iViewOffsetX;
                        tempPoint2.Y += iViewOffsetY;
                        tempPoint1 = tempPoint2;
                        MazeChanged(true);
                        
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        ChangeMode(Mode.wall0);
                    }
                    break;
                case Mode.curveWall0:
                    if (e.Button == MouseButtons.Left)
                    {
                        if (curMaze.CheckForClosePoint(e.X, e.Y, ref tempPoint1, iViewOffsetX, iViewOffsetY) == false)
                        {
                            tempPoint1 = new PointF(e.X, e.Y);

                            tempPoint1.X = ((int)(tempPoint1.X / GridStepX)) * GridStepX;
                            tempPoint1.Y = ((int)(tempPoint1.Y / GridStepY)) * GridStepY;

                        }
                        else
                        {
                            tempPoint1 = new PointF(e.X, e.Y);
                        }
                        tempPoint2 = tempPoint1;
                        ChangeMode(Mode.curveWall1);
                    }
                    break;
                case Mode.curveWall1: //set point 1 and 2
                    if (e.Button == MouseButtons.Left && dist2FromLast > minDist2)
                    {
                        if (curMaze.CheckForClosePoint(e.X, e.Y, ref tempPoint2, iViewOffsetX, iViewOffsetY) == false)
                        {
                            tempPoint2 = new PointF(e.X, e.Y);

                            tempPoint2.X = ((int)(tempPoint2.X / GridStepX)) * GridStepX;
                            tempPoint2.Y = ((int)(tempPoint2.Y / GridStepY)) * GridStepY;
                        }
                        else
                        {
                            tempPoint2 = new PointF(e.X, e.Y);
                        }

                        ChangeMode(Mode.curveWall2);
                        // Wall temp = new Wall(curMaze.Scale);
                        // tempPoint1.X -= iViewOffsetX;
                        //tempPoint1.Y -= iViewOffsetY;
                        // tempPoint2.X -= iViewOffsetX;
                        // tempPoint2.Y -= iViewOffsetY;
                        // if (tempPoint1.X < tempPoint2.X)
                        // {
                        //     temp.ScrPoint1 = tempPoint1;
                        //      temp.ScrPoint2 = tempPoint2;
                        //  }
                        // else
                        // {
                        //     temp.ScrPoint1 = tempPoint2;
                        //      temp.ScrPoint2 = tempPoint1;
                        //  }

                        // curMaze.cWall.Add(temp);
                        //ChangeMode(Mode.wall0); //We want to go connected...
                        //// tempPoint2.X += iViewOffsetX;
                        //  tempPoint2.Y += iViewOffsetY;
                        // tempPoint1 = tempPoint2;
                        //  MazeChanged(true);

                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        ChangeMode(Mode.curveWall0);
                    }
                    break;
                case Mode.curveWall2: // set angle
                    if (e.Button == MouseButtons.Left)
                    {
                        if (curMaze.CheckForClosePoint(e.X, e.Y, ref tempPoint3, iViewOffsetX, iViewOffsetY) == false)
                        {
                            tempPoint3 = new PointF(e.X, e.Y);

                            tempPoint3.X = ((int)(tempPoint3.X / GridStepX)) * GridStepX;
                            tempPoint3.Y = ((int)(tempPoint3.Y / GridStepY)) * GridStepY;
                        }
                        else
                        {
                            tempPoint3 = new PointF(e.X, e.Y);
                        }

                        CurvedWall temp = new CurvedWall(curMaze.Scale);
                        tempPoint1.X -= iViewOffsetX;
                        tempPoint1.Y -= iViewOffsetY;
                        tempPoint2.X -= iViewOffsetX;
                        tempPoint2.Y -= iViewOffsetY;
                        tempPoint3.X -= iViewOffsetX;
                        tempPoint3.Y -= iViewOffsetY;

                        temp.ScrPoint1 = tempPoint1;
                        temp.ScrPointMid = tempPoint2;
                        temp.ScrPoint2 = tempPoint3;

           

                        curMaze.cCurveWall.Add(temp);
                        temp.EndPointFromFirstAndCenterAndCursor(bCTRLdown);
                        //ChangeMode(Mode.wall0); //We want to go connected...

                        tempPoint1 = temp.ScrPoint2;
                        tempPoint1.X += iViewOffsetX;
                        tempPoint1.Y += iViewOffsetY;
                        tempPoint3 = tempPoint1;

                        ChangeMode(Mode.curveWall0);
                        MazeChanged(true);
                        RedrawFrame();
                        ChangeMode(Mode.curveWall1);



                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        ChangeMode(Mode.curveWall1);
                    }
                    break;

                case Mode.floor1:
                    if (e.Button == MouseButtons.Left && dist2FromLast > minDist2)
                    {
                        Floor temp2 = new Floor(curMaze.Scale);
                        tempPoint1.X -= iViewOffsetX;
                        tempPoint1.Y -= iViewOffsetY;
                        tempPoint2.X -= iViewOffsetX;
                        tempPoint2.Y -= iViewOffsetY;
                        RectangleF rc = new RectangleF(new PointF(Math.Min(tempPoint1.X, tempPoint2.X), Math.Min(tempPoint1.Y, tempPoint2.Y)), new SizeF(Math.Abs(tempPoint1.X - tempPoint2.X), Math.Abs(tempPoint1.Y - tempPoint2.Y)));
                        temp2.Rect = rc;
                        curMaze.cFloor.Add(temp2);
                        ChangeMode(Mode.floor0);
                        MazeChanged(true);
                        
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        ChangeMode(Mode.floor0);
                    }
                    break;
                case Mode.end1:
                    if (e.Button == MouseButtons.Left && dist2FromLast > minDist2)
                    {
                        //if (curMaze.cEnd == null)
                        //{
                        //    curMaze.cEnd = new EndRegion(curMaze.Scale);
                        //}
                        //curMaze.cEnd.Rect = new Rectangle(new Point(Math.Min(tempPoint1.X, tempPoint2.X), Math.Min(tempPoint1.Y, tempPoint2.Y)), new Size(Math.Abs(tempPoint1.X - tempPoint2.X), Math.Abs(tempPoint1.Y - tempPoint2.Y)));
                        //MazeChanged();
                        //UpdateTree();
                        EndRegion a = new EndRegion(curMaze.Scale);
                        tempPoint1.X -= iViewOffsetX;
                        tempPoint1.Y -= iViewOffsetY;
                        tempPoint2.X -= iViewOffsetX;
                        tempPoint2.Y -= iViewOffsetY;
                        a.Rect = new RectangleF(new PointF(Math.Min(tempPoint1.X, tempPoint2.X), Math.Min(tempPoint1.Y, tempPoint2.Y)), new SizeF((float)Math.Abs(tempPoint1.X - tempPoint2.X), Math.Abs(tempPoint1.Y - tempPoint2.Y)));
                        curMaze.cEndRegions.Add(a);
                        MazeChanged(true);
                        ChangeMode(Mode.end0);
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        ChangeMode(Mode.end0);
                    }
                    break;
                case Mode.actReg1:
                    if (e.Button == MouseButtons.Left && dist2FromLast > minDist2)
                    {
                        //if (curMaze.cEnd == null)
                        //{
                        //    curMaze.cEnd = new EndRegion(curMaze.Scale);
                        //}
                        //curMaze.cEnd.Rect = new Rectangle(new Point(Math.Min(tempPoint1.X, tempPoint2.X), Math.Min(tempPoint1.Y, tempPoint2.Y)), new Size(Math.Abs(tempPoint1.X - tempPoint2.X), Math.Abs(tempPoint1.Y - tempPoint2.Y)));
                        //MazeChanged();
                        //UpdateTree();
                        ActiveRegion a = new ActiveRegion(curMaze.Scale);
                        tempPoint1.X -= iViewOffsetX;
                        tempPoint1.Y -= iViewOffsetY;
                        tempPoint2.X -= iViewOffsetX;
                        tempPoint2.Y -= iViewOffsetY;
                        a.Rect = new RectangleF(new PointF(Math.Min(tempPoint1.X, tempPoint2.X), Math.Min(tempPoint1.Y, tempPoint2.Y)), new SizeF((float)Math.Abs(tempPoint1.X - tempPoint2.X), Math.Abs(tempPoint1.Y - tempPoint2.Y)));
                        curMaze.cActRegions.Add(a);
                        MazeChanged(true);
                        ChangeMode(Mode.actReg0);
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        ChangeMode(Mode.actReg0);
                    }
                    break;
                default:

                    tabPageMazeEdit_MouseClick(sender, e);
                    break;
                
            }
        }

        private void tabPageMazeEdit_MouseClick(object sender, MouseEventArgs e)
        {
            curMouseLocation = e.Location;
            UpdateOffsetReport();

            if (openDialogActive)
            {
                openDialogActive = false;
                return;
            }

            switch (curMode)
            {

                case Mode.drag: //Cancels if for some reason mousedown wasn't reported earlier
                    if (e.Button == MouseButtons.Left)
                    {
                        //ChangeMode(Mode.none);
                        curMode = Mode.none;

                    }
                    break;
                case Mode.none:
                    if (e.Button == MouseButtons.Left)
                    {
                        SyncSelections();
                        if (CheckSelection(e.X - iViewOffsetX, e.Y - iViewOffsetY, bCTRLdown) > 0)
                        {
                            RedrawFrame();
                            //curMode = Mode.drag;
                            
                        }
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        SyncSelections();
                        MazeItemContextMenu_Show(sender, e);
                    }
                    break;

                case Mode.wall0:
                    if (e.Button == MouseButtons.Right)
                    {
                        
                        ChangeMode(Mode.none);
                    }
                    break;
                case Mode.curveWall0:
                    if (e.Button == MouseButtons.Right)
                    {

                        ChangeMode(Mode.none);
                    }
                    break;

                case Mode.start:
                    if (e.Button == MouseButtons.Left)
                    {
                        StartPos sPos  = new StartPos(curMaze.Scale);
                        sPos.ScrPoint = new PointF(e.X - iViewOffsetX, e.Y - iViewOffsetY);
                       // sPos.MzPoint.Y = 0;
                        
                        if (curMaze.cStart.Count == 0)
                        {
                            //sPos.IsDefaultStartPos = true;
                            curMaze.DefaultStartPos = sPos;
                        }

                        curMaze.cStart.Add(sPos);

                        MazeChanged(true);
                    }
                    else if (e.Button == MouseButtons.Right)
                    {

                        ChangeMode(Mode.none);
                    }
                    break;
                case Mode.end0:
                    if (e.Button == MouseButtons.Right)
                    {
                        
                        ChangeMode(Mode.none);
                    }
                    break;
                case Mode.floor0:
                    if (e.Button == MouseButtons.Right)
                    {

                        ChangeMode(Mode.none);
                    }
                    break;
                case Mode.actReg0:
                    if (e.Button == MouseButtons.Right)
                    {

                        ChangeMode(Mode.none);
                    }
                    break;
                case Mode.light:
                    if (e.Button == MouseButtons.Left)
                    {
                        if (curMaze.cLight.Count >= 8)
                        {
                            MessageBox.Show("Maze can not have more then 8 light sources!");
                        }
                        else
                        {
                            tempPoint1.X = e.X - iViewOffsetX;
                            tempPoint1.Y = e.Y - iViewOffsetY;
                            Light t = new Light(curMaze.Scale);
                            t.ScrPoint = tempPoint1;
                            curMaze.cLight.Add(t);
                            MazeChanged(true);
                        }
                    }
                    else if (e.Button == MouseButtons.Right)
                    {

                        ChangeMode(Mode.none);
                    }
                    break;
                case Mode.staticmodel:
                    if (e.Button == MouseButtons.Left)
                    {
                        {
                            tempPoint1.X = e.X - iViewOffsetX;
                            tempPoint1.Y = e.Y - iViewOffsetY;
                            StaticModel t = new StaticModel(curMaze.Scale);
                            t.ScrPoint = tempPoint1;
                            curMaze.cStaticModels.Add(t);
                            MazeChanged(true);
                        }
                    }
                    else if (e.Button == MouseButtons.Right)
                    {

                        ChangeMode(Mode.none);
                    } 

                    break;
                case Mode.dynamicmodel:
                    if (e.Button == MouseButtons.Left)
                    {
                        {
                            tempPoint1.X = e.X - iViewOffsetX;
                            tempPoint1.Y = e.Y - iViewOffsetY;

                            DynamicObject t = new DynamicObject(curMaze.Scale);
                            t.ScrPoint = tempPoint1;
                            curMaze.cDynamicObjects.Add(t);
                            MazeChanged(true);
                        }
                    }
                    else if (e.Button == MouseButtons.Right)
                    {

                        ChangeMode(Mode.none);
                    }
                    break;
                case Mode.object0:
                    if (e.Button == MouseButtons.Left)
                    {
                        tempPoint1.X = e.X;
                        tempPoint1.Y = e.Y;
                        tempPoint2 = tempPoint1;
                        ChangeMode(Mode.object1);
                    }
                    else if(e.Button==MouseButtons.Right)
                    {
                        ChangeMode(Mode.none);
                    }
                    break;
                case Mode.object1: //Custom Object?
                    CustomObject aa = new CustomObject(curMaze.Scale);
                    tempPoint1.X -= iViewOffsetX;
                    tempPoint1.Y -= iViewOffsetY;
                    tempPoint2.X -= iViewOffsetX;
                    tempPoint2.Y -= iViewOffsetY;
                    aa.Rect = new RectangleF((float)Math.Min(tempPoint1.X, tempPoint2.X), Math.Min(tempPoint1.Y, tempPoint2.Y), Math.Abs(tempPoint1.X - tempPoint2.X), Math.Abs(tempPoint1.Y - tempPoint2.Y));
                    curMaze.cObject.Add(aa);
                    ChangeMode(Mode.object0);
                    MazeChanged(true);
                    break;
            }



        }


        private void tabPageMazeEdit_MouseDown(object sender, MouseEventArgs e)
        {
            if (openDialogActive)
            {
                openDialogActive = false;
                return;
            }

            bMouseDown = true;
            if (e.Button == MouseButtons.Right)
            {
                //bRightMouseDown = true; //only used for context menu
                cm.MenuItems.Clear();
            }

            if(e.Button==MouseButtons.Middle)
            {
                bMiddleMouseDown = true;
            }
            else
                bMiddleMouseDown = false;

            initMouseDownLocation = new PointF(e.X, e.Y);

            curMouseLocation = e.Location;
            UpdateOffsetReport();

            switch (curMode)
            {


                case Mode.wall0:
                    if (e.Button == MouseButtons.Left)
                    {
                        if (curMaze.CheckForClosePoint(e.X, e.Y, ref tempPoint1, iViewOffsetX, iViewOffsetY) == false)
                        {
                            tempPoint1 = new PointF(e.X, e.Y);

                            tempPoint1.X = ((int)(tempPoint1.X / GridStepX)) * GridStepX;
                            tempPoint1.Y = ((int)(tempPoint1.Y / GridStepY)) * GridStepY;

                        }
                        tempPoint2 = tempPoint1;
                        ChangeMode(Mode.wall1);
                    }
                    break;

                

                case Mode.floor0:
                    if (e.Button == MouseButtons.Left)
                    {
                        tempPoint1 = new PointF(e.X, e.Y);
                        tempPoint2 = tempPoint1;
                        ChangeMode(Mode.floor1);
                    }
                    break;
                case Mode.end0:
                    if (e.Button == MouseButtons.Left)
                    {
                        tempPoint1 = new PointF(e.X, e.Y);
                        tempPoint2 = tempPoint1;
                        ChangeMode(Mode.end1);
                    }
                    break;
                case Mode.actReg0:
                    if (e.Button == MouseButtons.Left)
                    {
                        tempPoint1 = new PointF(e.X, e.Y);
                        tempPoint2 = tempPoint1;
                        ChangeMode(Mode.actReg1);
                    }
                    break;
            }


        }
        bool bShowGrid = false;

        private Image tabPageMazeEdit_PaintToBuffer()
        {
            Image buffer = new Bitmap(tabPageMazeEdit.Width, tabPageMazeEdit.Height); //I usually use 32BppARGB as my color depth
            Graphics gr = Graphics.FromImage(buffer);

            gr.SmoothingMode =
                     System.Drawing.Drawing2D.SmoothingMode.AntiAlias;



            if (curMaze!=null&&gr!=null)
            {
                gr.TranslateTransform(iViewOffsetX, iViewOffsetY);

                if(bShowGrid)
                {
                    float barsPerUnit = 2;

                    float minX = -1*iViewOffsetX;
                    float maxX = -1*iViewOffsetX + tabPageMazeEdit.Width;
                    float minY = -1*iViewOffsetY;
                    float maxY = -1*iViewOffsetY + tabPageMazeEdit.Height;

                    for (int i=0;i< tabPageMazeEdit.Width/ curMaze.Scale/ barsPerUnit+1; i++)
                    {
                        PointF pt1, pt2;

                        

                        pt1 = new PointF((float)(minX+i*barsPerUnit* curMaze.Scale - (minX / curMaze.Scale)% barsPerUnit*curMaze.Scale), minY);
                        pt2 = new PointF((float)(minX + i  *barsPerUnit *curMaze.Scale - (minX / curMaze.Scale) % barsPerUnit * curMaze.Scale), maxY);
                        gr.DrawLine(Pens.Black, pt1, pt2);
                    }

                    for (int i = 0; i < tabPageMazeEdit.Height / curMaze.Scale / barsPerUnit+1; i++)
                    {
                        PointF pt1, pt2;

                        pt1 = new PointF(minX, (float)(minY + i * barsPerUnit * curMaze.Scale - (minY / curMaze.Scale) % barsPerUnit * curMaze.Scale));
                        pt2 = new PointF(maxX, (float)(minY + i * barsPerUnit * curMaze.Scale - (minY / curMaze.Scale) % barsPerUnit * curMaze.Scale));
                        gr.DrawLine(Pens.Black, pt1, pt2);
                    }

                }





                foreach (Floor f in curMaze.cFloor)
                {
                    curMazeTheme.SetColor(f);
                    f.Paint(ref gr);
                }
                foreach (MazeItem mItem in selected)
                {
                    if (mItem.GetType() == typeof(Floor))
                        mItem.Paint(ref gr);
                }
                foreach (EndRegion en in curMaze.cEndRegions)
                {
                    curMazeTheme.SetColor(en);
                    en.Paint(ref gr);
                }
                foreach (MazeItem mItem in selected)
                {
                    if (mItem.GetType() == typeof(EndRegion))
                        mItem.Paint(ref gr);
                }
                foreach (ActiveRegion actR in curMaze.cActRegions)
                {
                    curMazeTheme.SetColor(actR);
                    actR.Paint(ref gr);
                }
                foreach (MazeItem mItem in selected)
                {
                    if (mItem.GetType() == typeof(ActiveRegion))
                        mItem.Paint(ref gr);
                }
                foreach (Wall w in curMaze.cWall)
                {
                    curMazeTheme.SetColor(w);
                    w.Paint(ref gr);
                }
                foreach (CurvedWall w in curMaze.cCurveWall)
                {
                    curMazeTheme.SetColor(w);
                    w.Paint(ref gr);
                }
                foreach (Light l in curMaze.cLight)
                {
                    l.Paint(ref gr);
                }
                foreach (StaticModel c in curMaze.cStaticModels)
                {
                    c.Paint(ref gr);
                }
                foreach (DynamicObject c in curMaze.cDynamicObjects)
                {
                    c.Paint(ref gr);
                }
                foreach (CustomObject c in curMaze.cObject)
                {
                    c.Paint(ref gr);
                }
                foreach (StartPos sPos in curMaze.cStart)
                {
                    sPos.Paint(ref gr);
                }

                foreach (MazeItem mItem in selected)
                {
                    if (mItem.GetType() != typeof(Floor)&& mItem.GetType() != typeof(EndRegion)&& mItem.GetType() != typeof(ActiveRegion))
                        mItem.Paint(ref gr);
                }
                //if (curMaze.cEnd != null)
                //{
                //    curMaze.cEnd.Paint(ref gr);
                //}                

                //toolStripStatusLabel1.Text = "Painted...";



                gr.TranslateTransform(-iViewOffsetX, -iViewOffsetY);

                Pen p;
            switch (curMode)
            {
                
                case Mode.wall0:
                    if (hotpoint)
                        gr.DrawEllipse(Pens.Black, tempPoint1.X - 3, tempPoint1.Y - 3, 6, 6);
                    break;
                case Mode.wall1:
                    gr.DrawEllipse(Pens.Black, tempPoint1.X - 3, tempPoint1.Y - 3, 6, 6);
                    gr.DrawLine(Pens.Black, tempPoint1, tempPoint2);
                    gr.DrawEllipse(Pens.Black, tempPoint2.X - 3, tempPoint2.Y - 3, 6, 6);
                    break;
                case Mode.curveWall0:
                    if (hotpoint)
                        gr.DrawEllipse(Pens.Black, tempPoint1.X - 3, tempPoint1.Y - 3, 6, 6);
                    break;
                case Mode.curveWall1:
                    gr.DrawEllipse(Pens.Black, tempPoint1.X - 3, tempPoint1.Y - 3, 6, 6);
                    gr.DrawEllipse(Pens.Black, tempPoint2.X - 3, tempPoint2.Y - 3, 6, 6);
                    float tempRadius = Tools.Distance(tempPoint1, tempPoint2);
                    RectangleF rectTemp = new RectangleF((tempPoint2.X - tempRadius), (tempPoint2.Y - tempRadius), (tempRadius * 2), (tempRadius * 2));
                    if(rectTemp.Height>0&& rectTemp.Width>0)
                        gr.DrawArc(Pens.Black, rectTemp, 0,360);

                    break;
                case Mode.curveWall2:
                        gr.DrawEllipse(Pens.Black, tempPoint1.X - 3, tempPoint1.Y - 3, 6, 6);
                        //gr.DrawLine(Pens.Black, tempPoint1, tempPoint3);
                        gr.DrawEllipse(Pens.Black, tempPoint2.X - 3, tempPoint2.Y - 3, 6, 6);
                        gr.DrawEllipse(Pens.Black, tempPoint3.X - 3, tempPoint3.Y - 3, 6, 6);

                        PointF centerPoint = new PointF(tempPoint2.X,tempPoint2.Y);
                        float circRadius = Tools.Distance(tempPoint1, tempPoint2);
                        RectangleF rectFullCirc = new RectangleF((centerPoint.X - circRadius), (centerPoint.Y - circRadius), (circRadius * 2), (circRadius * 2));
                        if (rectFullCirc.Height > 0 && rectFullCirc.Width > 0)
                        { 
                            gr.DrawArc(Pens.Black, rectFullCirc, 0, 360);

                        float angleStart = 180- Tools.GetAngleDegree(centerPoint, tempPoint1);
                        float angleEnd =  180-Tools.GetAngleDegree(centerPoint, tempPoint3);

                        if (angleStart > angleEnd)
                            angleEnd += 360;

                        if(bCTRLdown)
                        {
                            if (angleEnd > angleStart)
                                angleStart += 360;
                        }

                        p = new Pen(Color.Red, 4);
                        if((angleEnd - angleStart) % 360!=0)
                            gr.DrawArc(p, rectFullCirc, angleStart,(angleEnd- angleStart)%360);
                        else
                            gr.DrawArc(p, rectFullCirc, angleStart,360);
                        }

                        break;
                case Mode.actReg0:
                    if (hotpoint)
                        gr.DrawEllipse(Pens.Black, tempPoint1.X - 3, tempPoint1.Y - 3, 6, 6);
                    break;
                case Mode.actReg1:
                    gr.DrawRectangle(Pens.Black, Math.Min(tempPoint1.X, tempPoint2.X), Math.Min(tempPoint1.Y, tempPoint2.Y), Math.Abs(tempPoint1.X - tempPoint2.X), Math.Abs(tempPoint1.Y - tempPoint2.Y));
                    break;
                case Mode.floor0:
                    break;
                case Mode.floor1:
                    gr.DrawRectangle(Pens.Black, Math.Min(tempPoint1.X, tempPoint2.X), Math.Min(tempPoint1.Y, tempPoint2.Y), Math.Abs(tempPoint1.X - tempPoint2.X), Math.Abs(tempPoint1.Y - tempPoint2.Y));
                    break;
                case Mode.end1:
                    gr.DrawRectangle(Pens.Black, Math.Min(tempPoint1.X, tempPoint2.X), Math.Min(tempPoint1.Y, tempPoint2.Y), Math.Abs(tempPoint1.X - tempPoint2.X), Math.Abs(tempPoint1.Y - tempPoint2.Y));
                    break;
                case Mode.object1:
                    gr.DrawRectangle(Pens.Black, Math.Min(tempPoint1.X, tempPoint2.X), Math.Min(tempPoint1.Y, tempPoint2.Y), Math.Abs(tempPoint1.X - tempPoint2.X), Math.Abs(tempPoint1.Y - tempPoint2.Y));
                    break;
                case Mode.multiSelect:
                    //gr.DrawRectangle(Pens.Black, Math.Min(tempPoint1.X, tempPoint2.X), Math.Min(tempPoint1.Y, tempPoint2.Y), Math.Abs(tempPoint1.X - tempPoint2.X), Math.Abs(tempPoint1.Y - tempPoint2.Y));
                    gr.DrawRectangle(myDashedPen, Math.Min(tempPoint1.X, tempPoint2.X), Math.Min(tempPoint1.Y, tempPoint2.Y), Math.Abs(tempPoint1.X - tempPoint2.X), Math.Abs(tempPoint1.Y - tempPoint2.Y));
 
                    break;
                case Mode.drag:
                    gr.TranslateTransform(iViewOffsetX, iViewOffsetY);
                    DrawTempSelected(gr, tempPoint2.X, tempPoint2.Y, (tempPoint2.X - initMouseDownLocation.X), (tempPoint2.Y - initMouseDownLocation.Y));
                    gr.TranslateTransform(-iViewOffsetX, -iViewOffsetY);
                    break;

                  }   

            }

            //Do all your drawing with "gr"

            gr.Dispose();

            return buffer;
        }

        

        private void tabPageMazeEdit_Paint(object sender, PaintEventArgs e)
        {
            //Graphics gr = e.Graphics;

            Image buffer = tabPageMazeEdit_PaintToBuffer();
            
            e.Graphics.DrawImage(buffer, 0, 0);
            buffer.Dispose();
            
        }

        private void tabPageMazeEdit_DragDrop(object sender, DragEventArgs e)
        {
            //MessageBox.Show("Tab1 Drag dropped");
            if (curMaze != null)
            {
                if (!CloseMaze())
                    return;
            }
            
            string[] a = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (a.Length >= 1)
            {
                int ext = CheckInputFileExtension(a[0]);
                if (ext == 1)
                {
                    if (curMaze != null)
                        CloseMaze();
                    NewMaze();

                    if (curMaze.ReadFromFileXML(a[0]))
                    {
                        isClassicFormat = false;
                        CurrentSettings.AddMazeFileToPrevious(a[0]);
                        this.Text = "MazeMaker - " + a[0];

                    }
                    else
                    {
                        if (curMaze.ReadFromClassicFile(a[0]))
                        {
                            isClassicFormat = true;
                            CurrentSettings.AddMazeFileToPrevious(a[0]);
                            this.Text = "MazeMaker - " + a[0];
                        }
                        else
                        {
                            CloseMaze();
                            MessageBox.Show("Not a maze file or corrupted maze file", "MazeMaker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else if (ext == 2)
                {
                    MazeListBuilder dia = new MazeListBuilder();
                    dia.ReadFromFile(a[0], false);
                    dia.ShowDialog();                    
                }
            }
            UpdateTree();
            UnSelect(true);
        }

        private void tabPageMazeEdit_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;

        }

        private void MazeChanged(bool updateTree=false) // Redraws frame, Updates Tree, Sync slections
        {
            if (ts_maze_wall.Enabled == false)
                return;
            if (!curMaze.changed)
                this.Text += "*";
            curMaze.changed = true;
            
            if(updateTree)
            {
                UpdateTree();
            }
            propertyGrid.Refresh();
            SyncSelections();
            RedrawFrame();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(curMaze!=null)
                e.Cancel=!CloseMaze();
            //ToolStripManager.SaveSettings(this);

            
            #region very old
            //Save Application Settings...
            //System.IO.BinaryWriter set = new System.IO.BinaryWriter(System.IO.File.Open("Settings.dat",System.IO.FileMode.Create));
            
            //set.Write(Application.VisualStyleState.ToString());

            //set.Write((byte)(toolStrip_coreIO.Visible?1:0));
            //set.Write((byte)toolStrip_coreIO.Top);
            //set.Write((byte)toolStrip_coreIO.Left);

            //set.Write((byte)(toolStrip_maze.Visible?1:0));
            //set.Write((byte)toolStrip_maze.Top);
            //set.Write((byte)toolStrip_maze.Left);

            //set.Write((byte)(toolStrip_delete.Visible?1:0));
            //set.Write((byte)toolStrip_delete.Top);
            //set.Write((byte)toolStrip_delete.Left);

            //set.Close();
            #endregion
        }

        private void tabControlMazeDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Current tab changed...

            switch (tabControlMazeDisplay.SelectedIndex)
            {
                case 0:
                    toolStrip_maze.Show();
                    break;
                case 1:
                    toolStrip_maze.Hide();
                    UnSelect();
                    break;
            }
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Help Contents...
            Help a = new Help();
            a.Show();
        }

        private void toolStrip_objectLib_Click(object sender, EventArgs e)
        {

        }

        private int CheckInputFileExtension(string inp)
        {
            int index = inp.LastIndexOf(".");
            string ext = inp.Substring(index + 1);
            ext.ToLower();
            if (ext.CompareTo("maz") == 0)
                return 1;
            if (ext.CompareTo("mel") == 0)
                return 2;
            if (ext.CompareTo("mazX") == 0)
                return 3;
            return 1; //Make default to Maze files...
        }


        struct Vector2
        {
            public float X, Y;
            public Vector2(float x, float y)
            {
                this.X = x;
                this.Y = y;
            }
            public static Vector2 ReturnMin(Vector2 v1,Vector2 v2)
            {
                Vector2 minV;
                if (Math.Abs(v1.X) < Math.Abs(v2.X))
                    minV.X = v1.X;
                else
                    minV.X = v2.X;

                if (Math.Abs(v1.Y) < Math.Abs(v2.Y))
                    minV.Y = v1.Y;
                else
                    minV.Y = v2.Y;

                return minV;
            }
        };


        private Vector2 RectifyMovement(Vector2 pos,Vector2 direction)
        {
            if (pos.X< 0)
            {
                direction.X -= pos.X;
            }
            else if (pos.X> tabPageMazeEdit.Width)
            {
                direction.X -= pos.X-tabPageMazeEdit.Width  ;
            }

            if (pos.Y < 0)
            {
                direction.Y -= pos.Y ;
            }
            else if (pos.Y > tabPageMazeEdit.Height)
            {
                direction.Y -= pos.Y-tabPageMazeEdit.Height;
            }

            return direction;
        }

        private void DrawTempSelected(Graphics gr,float mouseX, float mouseY, float Xdirection, float Ydirection)
        {
            if (curMaze == null)
            {
                return;
            }
            if (Xdirection == 0 && Ydirection == 0)
                return;

            if (selected.Count <= 0)
                return;

            tabPageMazeEdit.Focus();

            Vector2 mousePos, directionV;
            mousePos = new Vector2(mouseX, mouseY);

            directionV = new Vector2(Xdirection, Ydirection);
            directionV = RectifyMovement(mousePos, directionV);

            int pointSize = 20;

            PointF drawPoint1, drawPoint2;
           
            
            //gr.DrawEllipse(Pens.Black, drawPoint2.X - 3, drawPoint2.Y - 3, 6, 6);
           
            //gr.DrawRectangle(Pens.Black, Math.Min(drawPoint1.X, drawPoint2.X), Math.Min(drawPoint1.Y, drawPoint2.Y), Math.Abs(drawPoint1.X - drawPoint2.X), Math.Abs(drawPoint1.Y - drawPoint2.Y));
        

            foreach (Object mazeItem in selected)
            {
                tabPageMazeEdit.Focus();

                if (mazeItem.GetType() == typeof(Wall))
                {
                    Wall w = (Wall)mazeItem;

                    drawPoint1 = new PointF(w.ScrPoint1.X + directionV.X, w.ScrPoint1.Y + directionV.Y);
                    drawPoint2 = new PointF(w.ScrPoint2.X + directionV.X, w.ScrPoint2.Y + directionV.Y);
                    gr.DrawLine(Pens.Black, drawPoint1, drawPoint2);
                }
                if (mazeItem.GetType() == typeof(CurvedWall))
                {
                    CurvedWall w = (CurvedWall)mazeItem;

                    PointF centerPoint = new PointF(w.ScrPointMid.X, w.ScrPointMid.Y);
                    
                    RectangleF rectFullCirc = new RectangleF((w.ScrPointMid.X - (float)w.scrRadius)+ directionV.X, (w.ScrPointMid.Y - (float)w.scrRadius)+ directionV.Y, ((float)w.scrRadius * 2), ((float)w.scrRadius * 2));
                    if (rectFullCirc.Height > 0 && rectFullCirc.Width > 0)
                    {

                        if ((w.AngleEnd - w.AngleBegin) % 360 != 0)
                            gr.DrawArc(Pens.Black, rectFullCirc, (float) w.AngleBegin, (float)(w.AngleEnd - w.AngleBegin) % 360);
                        else
                            gr.DrawArc(Pens.Black, rectFullCirc, (float)w.AngleBegin, 360);
                    }

                }
                else if (mazeItem.GetType() == typeof(CustomObject))
                {
                    CustomObject c = (CustomObject)mazeItem;

                    drawPoint1 = new PointF(c.Rect.X + directionV.X, c.Rect.Y + directionV.Y);
                    gr.DrawRectangle(Pens.Black, drawPoint1.X, drawPoint1.Y, c.Rect.Width, c.Rect.Height);

                    //c.Rect = new RectangleF(c.Rect.X + directionV.X, c.Rect.Y + directionV.Y, c.Rect.Width, c.Rect.Height);
                }
                else if (mazeItem.GetType() == typeof(Floor))
                {
                    Floor f = (Floor)mazeItem;
                    drawPoint1 = new PointF(f.Rect.X + directionV.X, f.Rect.Y + directionV.Y);
                    gr.DrawRectangle(Pens.Black, drawPoint1.X, drawPoint1.Y, f.Rect.Width, f.Rect.Height);
                    //f.Rect = new RectangleF(f.Rect.X + directionV.X, f.Rect.Y + directionV.Y, f.Rect.Width, f.Rect.Height);
                }
                else if (mazeItem.GetType() == typeof(Light))
                {
                    Light l = (Light)mazeItem;


                    drawPoint1 = new PointF(l.ScrPoint.X + directionV.X, l.ScrPoint.Y + directionV.Y);
                    gr.DrawEllipse(Pens.Black, drawPoint1.X - pointSize/2, drawPoint1.Y - pointSize/2, pointSize, pointSize);

                }
                else if (mazeItem.GetType() == typeof(StaticModel))
                {
                    StaticModel s = (StaticModel)mazeItem;

                    drawPoint1 = new PointF(s.ScrPoint.X + directionV.X, s.ScrPoint.Y + directionV.Y);
                    gr.DrawEllipse(Pens.Black, drawPoint1.X - pointSize/2, drawPoint1.Y - pointSize/2, pointSize, pointSize);
                }
                else if (mazeItem.GetType() == typeof(DynamicObject))
                {
                    DynamicObject s = (DynamicObject)mazeItem;

                    drawPoint1 = new PointF(s.ScrPoint.X + directionV.X, s.ScrPoint.Y + directionV.Y);
                    gr.DrawEllipse(Pens.Black, drawPoint1.X - pointSize/2, drawPoint1.Y - pointSize/2, pointSize, pointSize);
                }
                else if (mazeItem.GetType() == typeof(EndRegion))
                {
                    EndRegion en = (EndRegion)mazeItem;

                    drawPoint1 = new PointF(en.Rect.X + directionV.X, en.Rect.Y + directionV.Y);
                    gr.DrawRectangle(Pens.Black, drawPoint1.X, drawPoint1.Y, en.Rect.Width, en.Rect.Height);
                    //drawPoint1 = new RectangleF(en.Rect.X + directionV.X, en.Rect.Y + directionV.Y, en.Rect.Width, en.Rect.Height);
                }
                else if (mazeItem.GetType() == typeof(ActiveRegion))
                {
                    ActiveRegion en = (ActiveRegion)mazeItem;

                    drawPoint1 = new PointF(en.Rect.X + directionV.X, en.Rect.Y + directionV.Y);
                    gr.DrawRectangle(Pens.Black, drawPoint1.X, drawPoint1.Y, en.Rect.Width, en.Rect.Height);
                    //drawPoint1 = new RectangleF(en.Rect.X + directionV.X, en.Rect.Y + directionV.Y, en.Rect.Width, en.Rect.Height);
                }
                else if (mazeItem.GetType() == typeof(StartPos))
                {
                    StartPos sPos = (StartPos)mazeItem;

                    drawPoint1 = new PointF(sPos.ScrPoint.X + directionV.X, sPos.ScrPoint.Y + directionV.Y);
                    gr.DrawEllipse(Pens.Black, drawPoint1.X - pointSize/2, drawPoint1.Y - pointSize/2, pointSize, pointSize);
                    //DoRefresh();
                    //return;
                }
            }

           // DoRefresh();
            return;
        }

        private void FindSelectedAndMove(float mouseX,float mouseY,float Xdirection,float Ydirection,bool manual)
        {
            if(curMaze==null)
            {
                return;
            }
            if (Xdirection == 0 && Ydirection == 0)
                return;

            if (!(manual) && Math.Abs(Xdirection) < iGridStepSize && Math.Abs(Ydirection) < iGridStepSize) //to prevent unintended movements
                return;

            if (selected.Count <= 0)
                return;
            
            tabPageMazeEdit.Focus();

            Vector2 mousePos, directionV;
            mousePos = new Vector2(mouseX, mouseY);
            
            directionV = new Vector2(Xdirection, Ydirection);
            if (!manual) //ignore mouse position if manual movement
                directionV = RectifyMovement(mousePos, directionV);

            foreach (Object mazeItem in selected)
            {
                tabPageMazeEdit.Focus();

                if (mazeItem.GetType() == typeof(Wall))
                {
                    Wall w = (Wall)mazeItem;

                    w.ScrPoint1 = new PointF(w.ScrPoint1.X + directionV.X, w.ScrPoint1.Y + directionV.Y);
                    w.ScrPoint2 = new PointF(w.ScrPoint2.X + directionV.X, w.ScrPoint2.Y + directionV.Y);
                    w.changed = true;
                }
                if (mazeItem.GetType() == typeof(CurvedWall))
                {
                    CurvedWall w = (CurvedWall)mazeItem;

                    w.ScrPoint1 = new PointF(w.ScrPoint1.X + directionV.X, w.ScrPoint1.Y + directionV.Y);
                    w.ScrPoint2 = new PointF(w.ScrPoint2.X + directionV.X, w.ScrPoint2.Y + directionV.Y);
                    w.ScrPointMid = new PointF(w.ScrPointMid.X + directionV.X, w.ScrPointMid.Y + directionV.Y);
                    w.changed = true;
                }
                else if (mazeItem.GetType() == typeof(CustomObject))
                {
                    CustomObject c = (CustomObject)mazeItem;

                    c.Rect = new RectangleF(c.Rect.X + directionV.X, c.Rect.Y + directionV.Y, c.Rect.Width, c.Rect.Height);
                    c.changed = true;
                }
                else if(mazeItem.GetType() == typeof(Floor))
                {
                    Floor f = (Floor)mazeItem;

                    f.Rect = new RectangleF(f.Rect.X + directionV.X, f.Rect.Y + directionV.Y, f.Rect.Width, f.Rect.Height);
                    f.changed = true;
                }
                else if (mazeItem.GetType() == typeof(Light))
                {
                    Light l = (Light)mazeItem;


                    l.ScrPoint = new PointF(l.ScrPoint.X + directionV.X, l.ScrPoint.Y + directionV.Y);
                    l.changed = true;
                }
                else if (mazeItem.GetType() == typeof(StaticModel))
                {
                    StaticModel s = (StaticModel)mazeItem;

                    s.ScrPoint = new PointF(s.ScrPoint.X + directionV.X, s.ScrPoint.Y + directionV.Y);
                    s.changed = true;
                }
                else if(mazeItem.GetType() == typeof(DynamicObject))
                {
                    DynamicObject s = (DynamicObject)mazeItem;

                    s.ScrPoint = new PointF(s.ScrPoint.X + directionV.X, s.ScrPoint.Y + directionV.Y);
                    s.changed = true;
                }
                else if (mazeItem.GetType() == typeof(EndRegion))
                {
                    EndRegion en = (EndRegion)mazeItem;

                    en.Rect = new RectangleF(en.Rect.X + directionV.X, en.Rect.Y + directionV.Y, en.Rect.Width, en.Rect.Height);
                    en.changed = true;
                }
                else if (mazeItem.GetType() == typeof(ActiveRegion))
                {
                    ActiveRegion en = (ActiveRegion)mazeItem;

                    en.Rect = new RectangleF(en.Rect.X + directionV.X, en.Rect.Y + directionV.Y, en.Rect.Width, en.Rect.Height);
                    en.changed = true;
                }
                else if (mazeItem.GetType() == typeof(StartPos))
                {
                    StartPos sPos = (StartPos)mazeItem;

                    sPos.ScrPoint = new PointF(sPos.ScrPoint.X + directionV.X, sPos.ScrPoint.Y + directionV.Y);
                    sPos.changed = true;
                    //DoRefresh();
                    //return;
                }
            }

            MazeChanged();
            return;
        }

        private void mazeListBuilderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            //MazeList Builder
            MazeListBuilder a = new MazeListBuilder();
            a.ShowDialog();
          
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (bCommandLineFormat)
                OpenTheFile(sCommandLineFile);            
        }

        private void ts_maz_object_Click(object sender, EventArgs e)
        {
            ChangeMode(Mode.staticmodel);
        }

        private void logProcessorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            //LogProcessor a = new LogProcessor();
            //a.ShowDialog();
            //a.Dispose();
            MessageBox.Show("Latest version of LogProcess is now available at MazeAnalyzer!.");
        }

        private void RotateSelected(float degrees)
        {
            
            if (curMaze == null)
            {
                return;
            }
            else
            {
                SyncSelections();
                PointF selectionMidPoint=calcMidPoint(selected,true);
                foreach(MazeItem m in selected)
                {
                    if(selected.Count==1)
                        m.Rotate(degrees);
                    else
                        m.Rotate(degrees, selectionMidPoint.X, selectionMidPoint.Y);

                }
                MarkSelectedChanged();
                MazeChanged();
                RedrawFrame();
            }
        }

        private void degrees90ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateSelected(90);
        }

        private void degrees180ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateSelected(180);
        }

        private void degrees270ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateSelected(270);
        }

        private void customToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int angle = 0;
            GetAngle ga = new GetAngle();
            angle = ga.Goster();
            RotateSelected(angle);

        }

        /*      
        /////////////////////////////////////////////////////////////////////////
        ///DEBUG

        private static bool IsSerializable(object obj)
        {
          System.IO.MemoryStream mem = new System.IO.MemoryStream();
          BinaryFormatter bin = new BinaryFormatter();
          try
          {
            bin.Serialize(mem, obj);
            return true;
          }
          catch(Exception ex)
          {
            MessageBox.Show("Your object cannot be serialized." + 
                             " The reason is: " + ex.ToString());
            return false;
          }
        }
        ///////////////////////////////////////////////////////////////////////////
        */
        private void Cut()
        {
            if (curMaze == null)
                return;
            else
            {
                SyncSelections();
                if (selected.Count > 0)
                {
                    Copy();
                    DeleteSelected();
                }
            }
        }

        private void Copy()
        {
            if (curMaze == null)
            {
                return;
            }
            try
            {
                DataFormats.Format dataFormat = DataFormats.GetFormat(typeof(List<Object>).FullName);

                // Construct data object from selected slides
                IDataObject dataObject = new DataObject();

                mazeClipboard = new List<Object>();
                dataObject.SetData(dataFormat.Name, false, mazeClipboard);

                // Put data into clipboard
                Clipboard.SetDataObject(dataObject, false);
                

                tabPageMazeEdit.Focus();
                foreach (Wall w in curMaze.cWall)
                {
                    if (w.IsSelected())
                    {
                        mazeClipboard.Add(w);
                        //Clipboard.SetDataObject(w);
                    }
                }
                foreach (CurvedWall w in curMaze.cCurveWall)
                {
                    if (w.IsSelected())
                    {
                        mazeClipboard.Add(w);
                        //Clipboard.SetDataObject(w);
                    }
                }
                foreach (Light l in curMaze.cLight)
                {                    
                    if (l.IsSelected())
                    {
                        mazeClipboard.Add(l);
                        //Clipboard.SetDataObject(l);
                    }
                }
                foreach (StaticModel l in curMaze.cStaticModels)
                {
                    if (l.IsSelected())
                    {
                        mazeClipboard.Add(l);
                        //Clipboard.SetDataObject(l);
                    }
                }
                foreach (DynamicObject l in curMaze.cDynamicObjects)
                {
                    if (l.IsSelected())
                    {
                        mazeClipboard.Add(l);
                        //Clipboard.SetDataObject(l);
                    }
                }
                foreach (CustomObject c in curMaze.cObject)
                {
                    if (c.IsSelected())
                    {
                        mazeClipboard.Add(c);
                    }
                }
                foreach (Floor f in curMaze.cFloor)
                {
                    if (f.IsSelected())
                    {
                        mazeClipboard.Add(f);
                    }
                }
                foreach (StartPos sPos in curMaze.cStart)
                {
                    if(sPos.IsSelected())
                    {
                        mazeClipboard.Add(sPos);
                    }

                }

                foreach (EndRegion en in curMaze.cEndRegions)
                {
                    if (en.IsSelected())
                    {
                        mazeClipboard.Add(en);
                    }

                }

                foreach (ActiveRegion en in curMaze.cActRegions)
                {
                    if (en.IsSelected())
                    {
                        mazeClipboard.Add(en);
                    }

                }
                //if (curMaze.cEnd != null && curMaze.cEnd.IsSelected())
                //{

                //}


            }
            catch
            {
            }
        }

        private void Paste(bool defaultOffset=true,float centerAtX=0,float centerAtY=0)
        {



            if (curMaze != null)
            {
               
                UnSelect(true);
                IDataObject dataObject = Clipboard.GetDataObject();
                if (dataObject != null)
                {
                    // Check if a collection of Slides is in the clipboard
                    string dataFormat = typeof(List<Object>).FullName;
                    if (dataObject.GetDataPresent(dataFormat))
                    {
                        // Retrieve slides from the clipboard
                        mazeClipboard = dataObject.GetData(dataFormat) as List<Object>;

                    }
                }
                else
                    return;

               
                if (mazeClipboard == null)
                {
                    mazeClipboard = new List<Object>();
                    return;
                }
                    
                int offsetX, offsetY;

                if (defaultOffset)
                {
                    offsetX = 15;
                    offsetY = 15;
                }
                else
                {
                    PointF midPoint = calcMidPoint(mazeClipboard);
                    offsetX = (int)centerAtX-(int)midPoint.X-iViewOffsetX;
                    offsetY = (int)centerAtY -(int)midPoint.Y - iViewOffsetY;
                }

                foreach (Object mazeItem in mazeClipboard)
                {
                    tabPageMazeEdit.Focus();

                    if (mazeItem.GetType() == typeof(Wall))
                    {
                        Wall w = (Wall)mazeItem;

                        Wall temp = w.Copy(false,offsetX,offsetY);

                        curMaze.cWall.Add(temp);

                        temp.Select(true);
                    }
                    else if(mazeItem.GetType() == typeof(CurvedWall))
                    {
                        CurvedWall w = (CurvedWall)mazeItem;

                        CurvedWall temp = w.Copy(false, offsetX, offsetY);

                        curMaze.cCurveWall.Add(temp);

                        temp.Select(true);
                    }
                    else if (mazeItem.GetType() == typeof(CustomObject))
                    {
                        CustomObject c = (CustomObject)mazeItem;
                        //CustomObject temp = c.Copy(false,offsetX,offsetY);
                        //temp.Rect = new RectangleF(temp.Rect.Left + 25, temp.Rect.Top + 25, temp.Rect.Width, temp.Rect.Height);
                        //temp.SetID();
                        //curMaze.cObject.Add(temp);

                        //temp.Select(true);
                    }
                    else if (mazeItem.GetType() == typeof(Floor))
                    {
                        Floor f = (Floor)mazeItem;
                        Floor temp = f.Copy(false,offsetX,offsetY);

                        //temp.Rect = new RectangleF(temp.Rect.Left + 25, temp.Rect.Top + 25, temp.Rect.Width, temp.Rect.Height);
                        //temp.SetID();
                        curMaze.cFloor.Add(temp);
                        temp.Select(true);
                    }
                    else if (mazeItem.GetType() == typeof(Light))
                    {
                        Light l = (Light)mazeItem;
                        Light temp = l.Copy(false,offsetX,offsetY);
                        //temp.SetID();
                        //temp.ScrPoint = new PointF(temp.ScrPoint.X + 15, temp.ScrPoint.Y + 15);

                        if (curMaze.cLight.Count >= 8) MessageBox.Show("Can not add more then 8 light sources");
                        else
                        {
                            curMaze.cLight.Add(temp);
                            temp.Select(true);
                        }

                    }
                    else if (mazeItem.GetType() == typeof(StaticModel))
                    {
                        StaticModel s = (StaticModel)mazeItem;
                        StaticModel temp = s.Copy(false,offsetX,offsetY);
                        //temp.SetID();
                        //temp.ScrPoint = new PointF(temp.ScrPoint.X + 15, temp.ScrPoint.Y + 15);
                        curMaze.cStaticModels.Add(temp);
                        temp.Select(true);
                    }
                    else if (mazeItem.GetType() == typeof(DynamicObject))
                    {
                        DynamicObject d = (DynamicObject)mazeItem;
                        DynamicObject temp = d.Copy(false,offsetX,offsetY);
                        //emp.SetID();
                        //temp.ScrPoint = new PointF(temp.ScrPoint.X + 15, temp.ScrPoint.Y + 15);
                        curMaze.cDynamicObjects.Add(temp);
                        temp.Select(true);
                    }
                    else if (mazeItem.GetType() == typeof(EndRegion))
                    {
                        EndRegion en = (EndRegion)mazeItem;
                        EndRegion temp = en.Copy(false,offsetX,offsetY);
                        //emp.Rect = new RectangleF(temp.Rect.Left + 25, temp.Rect.Top + 25, temp.Rect.Width, temp.Rect.Height);
                        //emp.SetID();
                        
                        curMaze.cEndRegions.Add(temp);
                        temp.Select(true);
                    }
                    else if (mazeItem.GetType() == typeof(ActiveRegion))
                    {
                        ActiveRegion en = (ActiveRegion)mazeItem;
                        ActiveRegion temp = en.Copy(false, offsetX, offsetY);
                        //emp.Rect = new RectangleF(temp.Rect.Left + 25, temp.Rect.Top + 25, temp.Rect.Width, temp.Rect.Height);
                        //emp.SetID();

                        curMaze.cActRegions.Add(temp);
                        temp.Select(true);
                    }
                    else if (mazeItem.GetType() == typeof(StartPos))
                    {
                        StartPos sPos = (StartPos)mazeItem;
                        StartPos temp = sPos.Copy(false, offsetX, offsetY);
                        //emp.Rect = new RectangleF(temp.Rect.Left + 25, temp.Rect.Top + 25, temp.Rect.Width, temp.Rect.Height);
                        //emp.SetID();

                        curMaze.cStart.Add(temp);
                        temp.Select(true);
                    }
                }
                MazeChanged(true);
            }
        }

        private PointF addPoints(PointF p1,PointF p2)
        {
            return new PointF(p1.X + p2.X, p1.Y+ p2.Y);
        }
        private PointF calcMidPoint(List<Object> mazeItems, bool trueMidPoint=false)
        {
            PointF retPoint = new Point(0, 0);
            foreach(Object mazeItem in mazeItems)
            { 
         
            if (mazeItem.GetType() == typeof(Wall))
            {
                Wall w = (Wall)mazeItem;

                if(!trueMidPoint)
                    retPoint=addPoints(retPoint, w.ScrPoint1);
                else
                    retPoint = addPoints(retPoint, w.CalcMidPoint());
            }
            else if (mazeItem.GetType() == typeof(CustomObject))
            {
                CustomObject c = (CustomObject)mazeItem;
                
            }
            else if (mazeItem.GetType() == typeof(Floor))
            {
                Floor f = (Floor)mazeItem;
                if (!trueMidPoint)
                    retPoint = addPoints(retPoint, f.Rect.Location);
                else
                    retPoint = addPoints(retPoint, f.CalcMidPoint());
            }
            else if (mazeItem.GetType() == typeof(Light))
            {
                Light l = (Light)mazeItem;

                retPoint = addPoints(retPoint, l.ScrPoint);
            }
            else if (mazeItem.GetType() == typeof(StaticModel))
            {
                StaticModel s = (StaticModel)mazeItem;
                retPoint = addPoints(retPoint, s.ScrPoint);
            }
            else if (mazeItem.GetType() == typeof(DynamicObject))
            {
                DynamicObject d = (DynamicObject)mazeItem;
                retPoint = addPoints(retPoint, d.ScrPoint);
            }
            else if (mazeItem.GetType() == typeof(EndRegion))
            {
                EndRegion en = (EndRegion)mazeItem;
                if (!trueMidPoint)
                    retPoint = addPoints(retPoint, en.Rect.Location);
                else
                    retPoint = addPoints(retPoint, en.CalcMidPoint());
            }
            else if (mazeItem.GetType() == typeof(ActiveRegion))
            {
                ActiveRegion en = (ActiveRegion)mazeItem;
                if (!trueMidPoint)
                    retPoint = addPoints(retPoint, en.Rect.Location);
                else
                    retPoint = addPoints(retPoint, en.CalcMidPoint());
            }
            else if (mazeItem.GetType() == typeof(StartPos))
            {
                StartPos sPos = (StartPos)mazeItem;
                retPoint = addPoints(retPoint, sPos.ScrPoint);
            }
            }

            return new PointF(retPoint.X/mazeItems.Count,retPoint.Y/mazeItems.Count);

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            Copy();
            
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            if (sender is MenuItem)
            { 

                var b = (MenuItem)sender;
                if(b.Parent is ContextMenu)
                    Paste(false,tempPoint1.X,tempPoint1.Y);
            }
            else
                Paste();             
        }



        private void editToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            ChangeModeTo0();
            SyncSelections();

            if (selected.Count > 0)
            { 
                copyToolStripMenuItem.Enabled = true;
                cutToolStripMenuItem.Enabled = true;
            }
            else
            { 
                copyToolStripMenuItem.Enabled = false;
                cutToolStripMenuItem.Enabled = true;
            }

            if (mazeClipboard.Count>0)
                pasteToolStripMenuItem.Enabled = true;
            else
                pasteToolStripMenuItem.Enabled = false;

            if (changedItems.Count > 0)
                undoToolStripMenuItem.Enabled = true;
            else
                undoToolStripMenuItem.Enabled = false;

            if (redoItems.Count > 0)
                redoToolStripMenuItem.Enabled = true;
            else
                redoToolStripMenuItem.Enabled = false;


            deleteToolStripMenuItem1.Enabled = ts_delete.Enabled;
            
        }

        private void modeToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem mn in modeToolStripMenuItem.DropDownItems)
            {
                mn.Enabled = toolStrip_maze.Items[0].Enabled;
            }           
        }

        private void SetToolStripLocations()
        {
            Point pt = new Point(3, 0);
            this.toolStrip_coreIO.Location = pt;           
            this.toolStrip_maze.Location = new Point(this.toolStrip_coreIO.Width + pt.X, 0);
            this.toolStripRun.Location = new Point(6 + toolStrip_delete.Width, this.toolStrip_coreIO.Height + pt.X);
            this.toolStripCollections.Location = new Point(9 + toolStrip_delete.Width + this.toolStripRun.Width, this.toolStrip_coreIO.Height + pt.X);
            this.toolStrip_delete.Location = new Point(3, this.toolStrip_coreIO.Height + pt.X);

        }

        private void toolStripContainer1_TopToolStripPanel_Layout(object sender, LayoutEventArgs e)
        {
            //SetToolStripLocations();
        }

        private void ts_maz_dobject_Click(object sender, EventArgs e)
        {
            ChangeMode(Mode.dynamicmodel);
        }

        private void textureCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();

            CollectionEditor collection = new CollectionEditor(MazeListBuilder.FilesToTextures(ImagePathConverter.Paths));
            collection.ShowDialog();

            MazeListBuilder.TexturesToFiles(collection.GetTextures(), ref ImagePathConverter.Paths);
            replaceOrder = collection.GetReplaceOrder();
            curMaze.ReplaceFiles(replaceOrder);
        }

        private void modelCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();

            CollectionEditor collection = new CollectionEditor(MazeListBuilder.FilesToModels(ModelPathConverter.Paths));
            collection.ShowDialog();
            
            MazeListBuilder.ModelsToFiles(collection.GetModels(), ref ModelPathConverter.Paths);
            replaceOrder = collection.GetReplaceOrder();
            curMaze.ReplaceFiles(replaceOrder);
        }

        private void audioCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();

            CollectionEditor collection = new CollectionEditor(MazeListBuilder.FilesToAudios(AudioPathConverter.Paths));
            collection.ShowDialog();

            MazeListBuilder.AudiosToFiles(collection.GetAudios(), ref AudioPathConverter.Paths);
            replaceOrder = collection.GetReplaceOrder();
            curMaze.ReplaceFiles(replaceOrder);
        }

        private void quickRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            quickRunToolStripMenuItem.Checked = !quickRunToolStripMenuItem.Checked;
            if(quickRunToolStripMenuItem.Checked)
            {
                toolStripRun.Visible = true;
            }
            else
            {
                toolStripRun.Visible = false;
            }
        }


        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            //configure...
            QuickRunSettingsDialog dlg = new QuickRunSettingsDialog(ref quickRunSettings);
            dlg.ShowDialog();
        }


        private bool running = false;
        System.Diagnostics.Process presentationProcess;

        QuickRunSettings quickRunSettings = new QuickRunSettings();

        private void Run()
        {
            string path = Application.ExecutablePath;
            int i = path.LastIndexOf('\\');
            path = path.Substring(0, i + 1);
 
            try
            {
                if (curMaze.FileName == "")
                {
                    SaveAs();
                    
                    return;
                }
                if(curMaze.changed)
                {
                    if (MessageBox.Show("QuickRun requires that you save your Maze first.\nDo you want to Save now?", "MazeMaker", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        return;
                    SaveMaze();
                }
                presentationProcess = System.Diagnostics.Process.Start(path + "MazeWalker.exe", "\"" + curMaze.FileName + "\" " + quickRunSettings.GetArgumentsString());
                //presentationProcess = System.Diagnostics.Process.Start(path + "MazeWalker.exe", curMaze.FileName);

            }
            catch (System.Exception ex)
            {
                running = false;
                MessageBox.Show("Couldn't initiate Quick Run!\n\n" + ex.Message, "MazeMaker");
                return;
            }

            //Thread.Sleep(a.Timeout * 1000);                    
            //presentationProcess.EnableRaisingEvents = true;
            //presentationProcess.Exited += new EventHandler(presentationProcess_Exited);

            running = false;

        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            Run();
        }

        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.ControlKey:
                    bCTRLdown = false;
                    break;
                case Keys.ShiftKey:
                    GridStepX = iGridStepSize;
                    GridStepY = iGridStepSize;
                    break;
            }
        }

        private void toolStripButtonItems_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            splitContainer2.Panel1Collapsed = !splitContainer2.Panel1Collapsed;
            UpdateTree();
            RedrawFrame();
        }

        private void leftPaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            leftPaneToolStripMenuItem.Checked = !leftPaneToolStripMenuItem.Checked;
            splitContainer2.Panel1Collapsed = !splitContainer2.Panel1Collapsed;

            if (curMaze!=null)
            {                
                UpdateTree();
                RedrawFrame();
            }
        }


        private void UpdateTree()
        {
            //if (splitContainer2.Panel1Collapsed) return;

            if (curMaze == null) return;

            //treeView1.Nodes.Clear();
            ClearTree();

            int i = 0;
            Font parentFont = new Font(treeView1.Font.FontFamily,treeView1.Font.Size+1,FontStyle.Bold);

            String mzString = "Maze: "+ this.curMaze.Name+"         ";
            TreeNode maze = new TreeNode("Maze Items");
            maze.NodeFont = parentFont;
            maze.Text = mzString;


            TreeNode wall = new TreeNode("Walls");
            wall.NodeFont = parentFont;
            wall.Text = "Walls   ";
            for ( i = 0; i < curMaze.cWall.Count;i++ )
            {
                //wall.Nodes.Add("Wall" + i.ToString());    
                wall.Nodes.Add((i + 1).ToString() + ". " + curMaze.cWall[i].PrintToTreeItem());
            }
            maze.Nodes.Add(wall);

            TreeNode curveWall = new TreeNode("CurvedWalls");
            curveWall.NodeFont = parentFont;
            curveWall.Text = "Curved Walls   ";
            for (i = 0; i < curMaze.cCurveWall.Count; i++)
            {
                //wall.Nodes.Add("Wall" + i.ToString());    
                curveWall.Nodes.Add((i + 1).ToString() + ". " + curMaze.cCurveWall[i].PrintToTreeItem());
            }
            maze.Nodes.Add(curveWall);


            TreeNode floor = new TreeNode("Floors");
            floor.NodeFont = parentFont;
            floor.Text = "Floors   ";
            for (i = 0; i < curMaze.cFloor.Count; i++)
            {
                //floor.Nodes.Add("Floor" + i.ToString());
                floor.Nodes.Add((i + 1).ToString() + ". " + curMaze.cFloor[i].PrintToTreeItem());
            }
            maze.Nodes.Add(floor);


            TreeNode lights = new TreeNode("Lights");
            lights.NodeFont = parentFont;
            lights.Text = "Lights";
            for (i = 0; i < curMaze.cLight.Count; i++)
            {
                //lights.Nodes.Add("Light" + i.ToString());
                lights.Nodes.Add((i + 1).ToString() + ". " + curMaze.cLight[i].PrintToTreeItem());
            }
            maze.Nodes.Add(lights);


            TreeNode staticModels = new TreeNode("Models");
            staticModels.NodeFont = parentFont;
            staticModels.Text = "Models";
            for (i = 0; i < curMaze.cStaticModels.Count; i++)
            {
                //staticModels.Nodes.Add("Static" + i.ToString());
                staticModels.Nodes.Add((i + 1).ToString() + ". " + curMaze.cStaticModels[i].PrintToTreeItem());
            }
            maze.Nodes.Add(staticModels);

            TreeNode dynamicModels = new TreeNode("Dynamic Objects");
            dynamicModels.NodeFont = parentFont;
            dynamicModels.Text = "Dynamic Objects";
            for (i = 0; i < curMaze.cDynamicObjects.Count; i++)
            {
                //dynamicModels.Nodes.Add("Dynamic" + i.ToString());
                dynamicModels.Nodes.Add((i + 1).ToString() + ". " + curMaze.cDynamicObjects[i].PrintToTreeItem());
            }
            maze.Nodes.Add(dynamicModels);

            TreeNode activeRegionsNode = new TreeNode("Active Regions");
            activeRegionsNode.NodeFont = parentFont;
            activeRegionsNode.Text = "Active Regions";
            for (i = 0; i < curMaze.cActRegions.Count; i++)
            {
                //dynamicModels.Nodes.Add("Dynamic" + i.ToString());
                activeRegionsNode.Nodes.Add((i + 1).ToString() + ". " + curMaze.cActRegions[i].PrintToTreeItem());
            }
            maze.Nodes.Add(activeRegionsNode);

            TreeNode ends = new TreeNode("End Regions");
            ends.NodeFont = parentFont;
            ends.Text = "End Regions";
            for (i = 0; i < curMaze.cEndRegions.Count; i++)
            {
                //ends.Nodes.Add("End" + i.ToString());
                ends.Nodes.Add((i + 1).ToString() + ". " + curMaze.cEndRegions[i].PrintToTreeItem());
            }
            maze.Nodes.Add(ends);

            TreeNode starts = new TreeNode("Start Positions");
            starts.NodeFont = parentFont;
            starts.Text = "Start Positions";
            for (i = 0; i < curMaze.cStart.Count; i++)
            {
                //ends.Nodes.Add("End" + i.ToString());
                starts.Nodes.Add((i + 1).ToString() + ". " + curMaze.cStart[i].PrintToTreeItem());
            }
            maze.Nodes.Add(starts);

            treeView1.Nodes.Add(maze);
            maze.Expand();
            treeView1.ExpandAll();
        }

        private void ClearTree()
        {
            treeView1.Nodes.Clear();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {

                int index = 0;
                if(!bCTRLdown)
                    UnSelect(true); //force unselect

                if (e.Node.Text.Contains("Curved"))
                {
                    //this is a wall
                    if (e.Node.Text.Contains("Curved Walls"))
                    {
                        //parent node, select all...
                        foreach (CurvedWall w in curMaze.cCurveWall)
                        {
                            w.Select(true);
                        }
                    }
                    else
                    {
                        //child nodes..
                        index = int.Parse(e.Node.Text.Substring(0, e.Node.Text.IndexOf('.'))) - 1;
                        curMaze.cCurveWall[index].Select(!curMaze.cCurveWall[index].IsSelected() || !bCTRLdown);
                    }
                }
                else if (e.Node.Text.Contains("Wall"))
                {
                    //this is a wall
                    if (e.Node.Text.Contains("Walls"))
                    {
                        //parent node, select all...
                        foreach (Wall w in curMaze.cWall)
                        {
                            w.Select(true);
                        }
                    }
                    else
                    {
                        //child nodes..
                        index = int.Parse(e.Node.Text.Substring(0,e.Node.Text.IndexOf('.')))-1;
                        curMaze.cWall[index].Select(!curMaze.cWall[index].IsSelected()||!bCTRLdown);
                    }
                }
                
                else if (e.Node.Text.Contains("Floor"))
                {
                    if (e.Node.Text.Contains("Floors"))
                    {
                        //parent node, select all...
                        foreach (Floor w in curMaze.cFloor)
                        {
                            w.Select(true);
                        }
                    }
                    else
                    {
                        index = int.Parse(e.Node.Text.Substring(0,e.Node.Text.IndexOf('.')))-1;
                        curMaze.cFloor[index].Select(!curMaze.cFloor[index].IsSelected() || !bCTRLdown);
                    }
                }
                else if (e.Node.Text.Contains("Light"))
                {
                    if (e.Node.Text.Contains("Lights"))
                    {
                        //parent node, select all...
                        foreach (Light w in curMaze.cLight)
                        {
                            w.Select(true);
                        }
                    }
                    else
                    {
                        index = int.Parse(e.Node.Text.Substring(0,e.Node.Text.IndexOf('.')))-1;
                        curMaze.cLight[index].Select(!curMaze.cLight[index].IsSelected() || !bCTRLdown);
                    }
                }
                else if (e.Node.Text.Contains("Model"))
                {
                    if (e.Node.Text.Contains("Models"))
                    {
                        //parent node, select all...
                        foreach (StaticModel w in curMaze.cStaticModels)
                        {
                            w.Select(true);
                        }
                    }
                    else
                    {
                        index = int.Parse(e.Node.Text.Substring(0,e.Node.Text.IndexOf('.')))-1;
                        curMaze.cStaticModels[index].Select(!curMaze.cStaticModels[index].IsSelected() || !bCTRLdown);
                    }
                }
                else if (e.Node.Text.Contains("Dynamic"))
                {
                    if (e.Node.Text.Contains("Dynamic Objects"))
                    {
                        //parent node, select all...
                        foreach (DynamicObject w in curMaze.cDynamicObjects)
                        {
                            w.Select(true);
                        }
                    }
                    else
                    {
                        index = int.Parse(e.Node.Text.Substring(0,e.Node.Text.IndexOf('.')))-1;
                        curMaze.cDynamicObjects[index].Select(!curMaze.cDynamicObjects[index].IsSelected() || !bCTRLdown);
                    }
                }
                else if (e.Node.Text.Contains("Start"))
                {
                    if (e.Node.Text.Contains("Start Positions"))
                    {
                        //parent node, select all...
                        foreach (StartPos sPos in curMaze.cStart)
                        {
                            sPos.Select(true);
                        }
                    }
                    else
                    {
                        index = int.Parse(e.Node.Text.Substring(0, e.Node.Text.IndexOf('.'))) - 1;
                        curMaze.cStart[index].Select(!curMaze.cStart[index].IsSelected() || !bCTRLdown);
                    }
                    //index = int.Parse(e.Node.Text.Substring(6));
                }
                //else if (e.Node.Text.Contains("End Region"))
                //{
                //    //index = int.Parse(e.Node.Text.Substring(6));
                //    curMaze.cEnd.Select(true);
                //}
                else if(e.Node.Text.Contains("End"))
                {
                    if(e.Node.Text.Contains("End Regions"))
                    {
                        //select all..
                        //parent node, select all...
                        foreach (EndRegion en in curMaze.cEndRegions)
                        {
                            en.Select(true);
                        }
                    }
                    else
                    {
                        index = int.Parse(e.Node.Text.Substring(0,e.Node.Text.IndexOf('.')))-1;
                        curMaze.cEndRegions[index].Select(!curMaze.cEndRegions[index].IsSelected() || !bCTRLdown);
                    }
                }
                else if (e.Node.Text.Contains("Active"))
                {
                    if (e.Node.Text.Contains("Active Regions"))
                    {
                        //select all..
                        //parent node, select all...
                        foreach (ActiveRegion ar in curMaze.cActRegions)
                        {
                            ar.Select(true);
                        }
                    }
                    else
                    {
                        index = int.Parse(e.Node.Text.Substring(0, e.Node.Text.IndexOf('.'))) - 1;
                        curMaze.cActRegions[index].Select(!curMaze.cActRegions[index].IsSelected() || !bCTRLdown);
                    }
                }
                /*else if(e.Node.Text.Contains("Start"))
                {
                    if(curMaze.cStart!=null)
                        curMaze.cStart.Select(true);
                    //if (curMaze.cEnd != null)
                    //    curMaze.cEnd.Select(true);
                }*/
                SyncSelections();
                RedrawFrame();
                propertyGrid.Refresh();
            }
            catch //(System.Exception ex)
            {

            }
        }

        private void SelectNextObject()
        {
            bool flag = false;
            int selected = -1;

            int loopCount = 0;
            while (loopCount < 2)
            {
                #region Current Maze
                if (flag)
                {
                    UnSelect();
                    propertyGrid.SelectedObject = curMaze;
                    break;
                }
                if (propertyGrid.SelectedObject == curMaze)
                {
                    flag = true;
                }
                #endregion
                #region Walls
                if (curMaze.cWall.Count > 0)
                {
                    selected = -1;
                    for (int i = 0; i < curMaze.cWall.Count; i++)
                    {
                        if (curMaze.cWall[i].IsSelected())
                        {
                            selected = i;
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        selected++;
                        if (selected < curMaze.cWall.Count)
                        {
                            UnSelect();
                            curMaze.cWall[selected].Select(true);
                            propertyGrid.SelectedObject = curMaze.cWall[selected];
                            break;
                        }
                    }
                }
                #endregion
                #region CurvedWalls
                if (curMaze.cCurveWall.Count > 0)
                {
                    selected = -1;
                    for (int i = 0; i < curMaze.cCurveWall.Count; i++)
                    {
                        if (curMaze.cCurveWall[i].IsSelected())
                        {
                            selected = i;
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        selected++;
                        if (selected < curMaze.cCurveWall.Count)
                        {
                            UnSelect();
                            curMaze.cCurveWall[selected].Select(true);
                            propertyGrid.SelectedObject = curMaze.cCurveWall[selected];
                            break;
                        }
                    }
                }
                #endregion
                #region Floors
                if (curMaze.cFloor.Count > 0)
                {
                    selected = -1;
                    for (int i = 0; i < curMaze.cFloor.Count; i++)
                    {
                        if (curMaze.cFloor[i].IsSelected())
                        {
                            selected = i;
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        selected++;
                        if (selected < curMaze.cFloor.Count)
                        {
                            UnSelect();
                            curMaze.cFloor[selected].Select(true);
                            propertyGrid.SelectedObject = curMaze.cFloor[selected];
                            break;
                        }
                    }
                }
                #endregion
                #region Start Pos
                if (curMaze.cStart.Count > 0)
                {
                    selected = -1;
                    for (int i = 0; i < curMaze.cStart.Count; i++)
                    {
                        if (curMaze.cStart[i].IsSelected())
                        {
                            selected = i;
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        selected++;
                        if (selected < curMaze.cStart.Count)
                        {
                            UnSelect();
                            curMaze.cStart[selected].Select(true);
                            propertyGrid.SelectedObject = curMaze.cStart[selected];
                            break;
                        }
                    }
                }
                #endregion
                #region End Regions
                if (curMaze.cEndRegions.Count > 0)
                {
                    selected = -1;
                    for (int i = 0; i < curMaze.cEndRegions.Count; i++)
                    {
                        if (curMaze.cEndRegions[i].IsSelected())
                        {
                            selected = i;
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        selected++;
                        if (selected < curMaze.cEndRegions.Count)
                        {
                            UnSelect();
                            curMaze.cEndRegions[selected].Select(true);
                            propertyGrid.SelectedObject = curMaze.cEndRegions[selected];
                            break;
                        }
                    }
                }
                #endregion
                #region Lights
                if (curMaze.cLight.Count > 0)
                {
                    selected = -1;
                    for (int i = 0; i < curMaze.cLight.Count; i++)
                    {
                        if (curMaze.cLight[i].IsSelected())
                        {
                            selected = i;
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        selected++;
                        if (selected < curMaze.cLight.Count)
                        {
                            UnSelect();
                            curMaze.cLight[selected].Select(true);
                            propertyGrid.SelectedObject = curMaze.cLight[selected];
                            break;
                        }
                    }
                }
                #endregion
                #region Static Models
                if (curMaze.cStaticModels.Count > 0)
                {
                    selected = -1;
                    for (int i = 0; i < curMaze.cStaticModels.Count; i++)
                    {
                        if (curMaze.cStaticModels[i].IsSelected())
                        {
                            selected = i;
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        selected++;
                        if (selected < curMaze.cStaticModels.Count)
                        {
                            UnSelect();
                            curMaze.cStaticModels[selected].Select(true);
                            propertyGrid.SelectedObject = curMaze.cStaticModels[selected];
                            break;
                        }
                    }
                }
                #endregion
                #region Dynamic Models
                if (curMaze.cDynamicObjects.Count > 0)
                {
                    selected = -1;
                    for (int i = 0; i < curMaze.cDynamicObjects.Count; i++)
                    {
                        if (curMaze.cDynamicObjects[i].IsSelected())
                        {
                            selected = i;
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        selected++;
                        if (selected < curMaze.cDynamicObjects.Count)
                        {
                            UnSelect();
                            curMaze.cDynamicObjects[selected].Select(true);
                            propertyGrid.SelectedObject = curMaze.cDynamicObjects[selected];
                            break;
                        }
                    }
                }
                #endregion
                loopCount++;
            }

            if(flag==false)
            {
                //nothing is selected, initiate with the maze...
                propertyGrid.SelectedObject = curMaze;
            }
        }

        private void recentMazeFilesToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            recentMazeFilesToolStripMenuItem.DropDownItems.Clear();
            if (CurrentSettings.previousMazeFiles.Count == 0)
            {
                recentMazeFilesToolStripMenuItem.DropDownItems.Add("No previous items");
            }
            else
            {
                for (int i = CurrentSettings.previousMazeFiles.Count - 1; i >= 0; i--)
                {
                    recentMazeFilesToolStripMenuItem.DropDownItems.Add(CurrentSettings.previousMazeFiles[i], null, new EventHandler(temp_Click));
                }
            }
        }

        void temp_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //MessageBox.Show(e.ToString());
            if (OpenTheFile(sender.ToString()) == false)
            {
                CurrentSettings.RemoveMazeFileFromPrevious(sender.ToString());
            }
        }

        private void mazeUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            try
            {
                System.Diagnostics.Process.Start(Application.ExecutablePath.Substring(0,Application.ExecutablePath.LastIndexOf("\\")) + "\\MazeUpdate.exe");
                this.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Couldn't find MazeUpdate Utility. Please re-install MazeSuite with latest setup","MazeMaker",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }                       
        }

        private void newMazeWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            if (curMaze != null)
            {
                if (CloseMaze() == false)
                    return;
            }

            MazeGen a = new MazeGen();
            if (a.ShowDialog() == DialogResult.OK)
            {
                NewMaze();
                curMaze.cFloor.AddRange(a.GetFloors(curMaze.Scale));
                curMaze.cWall.AddRange(a.GetWalls(curMaze.Scale));
                curMaze.cCurveWall.AddRange(a.GetCurvedWalls(curMaze.Scale));

                StartPos sPos;
                sPos = a.GetStartPos(curMaze.Scale);
                sPos.justCreated = false;
                
                curMaze.cStart.Add(sPos);
                curMaze.DefaultStartPos = sPos;
                MazeChanged(true);
                resetUndoBuffer();
            }

        }

        

        private void resetUndoBuffer()
        {
            if(curMaze!=null)
            {
                SyncSelections();
            }
            changedItems = new List<Object>();
            changedItemsStack = new Stack<Object>();
            mazeClipboard = new List<Object>();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
        }

        private void rightPaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            //if(curMaze!=null)
            {
                splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
                rightPaneToolStripMenuItem.Checked = !rightPaneToolStripMenuItem.Checked;
            }
        }

        private void userManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            try
            {
                int index = Application.ExecutablePath.LastIndexOf('\\');
                string str = Application.ExecutablePath.Substring(0, index) + "\\MazeSuiteManual.pdf";
                //if (File.Exists(str))
                // {
                System.Diagnostics.Process.Start(str);
                //}
            }
            catch //(System.Exception ex)
            {
                MessageBox.Show("Manual file cannot be found!\r\n\r\nPlease see http://www.mazesuite.com/ for documentation.", "MazeSuite");
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            ChangeModeTo0();
            if (CheckIfValidSelectedTreeItem()==false)
            {
                //e.Cancel = true;
                toolStripMenuItemTop.Visible = false;
                toolStripMenuItemBottom.Visible = false;
                toolStripMenuItemDown.Visible = false;
                toolStripMenuItemUp.Visible = false;
                toolStripSeparator10.Visible = false;
            }
            else
            {
                toolStripSeparator10.Visible = true;
                toolStripMenuItemTop.Visible = true;
                toolStripMenuItemBottom.Visible = true;
                toolStripMenuItemDown.Visible = false; //Disabled for now
                toolStripMenuItemUp.Visible = false;
            }
        }

        private bool CheckIfValidSelectedTreeItem()
        {
            //child item should be selected for context menu
            if (curMaze == null || treeView1.SelectedNode == null)
            {
                return false;
            }
            if (treeView1.SelectedNode.Parent == null || treeView1.SelectedNode.Parent.Text == treeView1.Nodes[0].Text)
            {
                return false;
            }
            if (treeView1.SelectedNode.Text.Contains(".") == false)
            {
                return false;
            }
            return true;
        }

        private void toolStripMenuItemTop_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            if (CheckIfValidSelectedTreeItem() == false)
            {
                return;
            }

            if (treeView1.SelectedNode.Index == 0)
            {
                //already at top
                return;
            }
            int index=0;
            if (treeView1.SelectedNode.Text.Contains("Wall"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.')))-1;
                MazeItem b = (MazeItem) curMaze.cWall[index];
                curMaze.DeleteItemInCollection(b);
                curMaze.AddItemToCollection(b, 0);
                
                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("Curved"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.'))) - 1;
                MazeItem b = (MazeItem)curMaze.cCurveWall[index];
                curMaze.DeleteItemInCollection(b);
                curMaze.AddItemToCollection(b, 0);

                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("Floor"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.')))-1;
                MazeItem b = (MazeItem)curMaze.cFloor[index];
                curMaze.DeleteItemInCollection(b);
                curMaze.AddItemToCollection(b, 0);

                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("EndRegion"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.'))) - 1;
                MazeItem b = (MazeItem)curMaze.cEndRegions[index];
                curMaze.DeleteItemInCollection(b);
                curMaze.AddItemToCollection(b, 0);

                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("ActiveRegion"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.'))) - 1;
                MazeItem b = (MazeItem)curMaze.cActRegions[index];
                curMaze.DeleteItemInCollection(b);
                curMaze.AddItemToCollection(b, 0);

                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("StartPosition"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.'))) - 1;
                MazeItem b = (MazeItem)curMaze.cStart[index];
                curMaze.DeleteItemInCollection(b);
                curMaze.AddItemToCollection(b, 0);

                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("StaticModel"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.'))) - 1;
                MazeItem b = (MazeItem)curMaze.cStaticModels[index];
                curMaze.DeleteItemInCollection(b);
                curMaze.AddItemToCollection(b, 0);

                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("DynamicObject"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.'))) - 1;
                MazeItem b = (MazeItem)curMaze.cDynamicObjects[index];
                curMaze.DeleteItemInCollection(b);
                curMaze.AddItemToCollection(b, 0);

                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("Light"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.'))) - 1;
                MazeItem b = (MazeItem)curMaze.cLight[index];
                curMaze.DeleteItemInCollection(b);
                curMaze.AddItemToCollection(b, 0);

                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }

        }

        private void toolStripMenuItemUp_Click(object sender, EventArgs e)
        {
            if (CheckIfValidSelectedTreeItem() == false)
            {
                return;
            }
            if (treeView1.SelectedNode.Index == 0)
            {
                //already at top
                return;
            }
            int index = 0;
            if (treeView1.SelectedNode.Text.Contains("Wall"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.')))-1;
                Wall a = curMaze.cWall[index];
                curMaze.cWall.RemoveAt(index);
                index--;
                curMaze.cWall.Insert(index, a);
                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("Curved Wall"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.'))) - 1;
                CurvedWall a = curMaze.cCurveWall[index];
                curMaze.cCurveWall.RemoveAt(index);
                index--;
                curMaze.cCurveWall.Insert(index, a);
                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("Floor"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.')))-1;
                Floor a = curMaze.cFloor[index];
                curMaze.cFloor.RemoveAt(index);
                index--;
                curMaze.cFloor.Insert(index, a);
                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }
        }

        private void toolStripMenuItemDown_Click(object sender, EventArgs e)
        {
            if (CheckIfValidSelectedTreeItem() == false)
            {
                return;
            }
            if (treeView1.SelectedNode.Index == treeView1.SelectedNode.Parent.Nodes.Count-1)
            {
                //already at bottom
                return;
            }
            int index = 0;
            if (treeView1.SelectedNode.Text.Contains("Wall"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.')))-1;
                Wall a = curMaze.cWall[index];
                curMaze.cWall.RemoveAt(index);
                index++;
                curMaze.cWall.Insert(index, a);
                MazeChanged();
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("Curved Wall"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.'))) - 1;
                CurvedWall a = curMaze.cCurveWall[index];
                curMaze.cCurveWall.RemoveAt(index);
                index++;
                curMaze.cCurveWall.Insert(index, a);
                MazeChanged();
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("Floor"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.')))-1;
                Floor a = curMaze.cFloor[index];
                curMaze.cFloor.RemoveAt(index);
                index++;
                curMaze.cFloor.Insert(index, a);
                MazeChanged();
                UnSelect();
                //treeView1.SelectedNode = null;
            }

        }

        private void toolStripMenuItemBottom_Click(object sender, EventArgs e)
        {
            if (CheckIfValidSelectedTreeItem() == false)
            {
                return;
            }
            if (treeView1.SelectedNode.Index == treeView1.SelectedNode.Parent.Nodes.Count - 1)
            {
                //already at bottom
                return;
            }
            int index = 0;
            if (treeView1.SelectedNode.Text.Contains("Wall"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.'))) - 1;
                MazeItem b = (MazeItem)curMaze.cWall[index];
                curMaze.DeleteItemInCollection(b);
                curMaze.AddItemToCollection(b);

                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("Curved"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.'))) - 1;
                MazeItem b = (MazeItem)curMaze.cCurveWall[index];
                curMaze.DeleteItemInCollection(b);
                curMaze.AddItemToCollection(b);

                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("Floor"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.'))) - 1;
                MazeItem b = (MazeItem)curMaze.cFloor[index];
                curMaze.DeleteItemInCollection(b);
                curMaze.AddItemToCollection(b);

                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("EndRegion"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.'))) - 1;
                MazeItem b = (MazeItem)curMaze.cEndRegions[index];
                curMaze.DeleteItemInCollection(b);
                curMaze.AddItemToCollection(b);

                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("ActiveRegion"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.'))) - 1;
                MazeItem b = (MazeItem)curMaze.cActRegions[index];
                curMaze.DeleteItemInCollection(b);
                curMaze.AddItemToCollection(b);

                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("StartPosition"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.'))) - 1;
                MazeItem b = (MazeItem)curMaze.cStart[index];
                curMaze.DeleteItemInCollection(b);
                curMaze.AddItemToCollection(b);

                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("StaticModel"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.'))) - 1;
                MazeItem b = (MazeItem)curMaze.cStaticModels[index];
                curMaze.DeleteItemInCollection(b);
                curMaze.AddItemToCollection(b);

                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("DynamicObject"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.'))) - 1;
                MazeItem b = (MazeItem)curMaze.cDynamicObjects[index];
                curMaze.DeleteItemInCollection(b);
                curMaze.AddItemToCollection(b);

                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }
            else if (treeView1.SelectedNode.Text.Contains("Light"))
            {
                index = int.Parse(treeView1.SelectedNode.Text.Substring(0, treeView1.SelectedNode.Text.IndexOf('.'))) - 1;
                MazeItem b = (MazeItem)curMaze.cLight[index];
                curMaze.DeleteItemInCollection(b);
                curMaze.AddItemToCollection(b);

                MazeChanged(true);
                UnSelect();
                //treeView1.SelectedNode = null;
            }

        }

        private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.CollapseAll();
        }

        private void expandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            treeView1.ExpandAll();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ChangeModeTo0();
            treeView1.SelectedNode = e.Node;
        }

        private void buttonPanelGallery1_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            //Visit Gallery...
            LaunchLink("http://www.mazesuite.com/gallery/");
        }

        private void buttonPanelForum1_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            LaunchLink("http://www.mazesuite.com/forum/");
        }

        void LaunchLink(string link)
        {
            try
            {
                System.Diagnostics.Process.Start(link);
            }
            catch//(Exception ex)
            {
                MessageBox.Show("Can not launch default browser to visit web link: " + link);
            }
        }

        void PositionWelcomePanel()
        {
            if (panelWelcome.Visible)
            {
                panelWelcome.Left = (tabPageMazeEdit.Width - panelWelcome.Width) / 2;
                panelWelcome.Top = (tabPageMazeEdit.Height - panelWelcome.Height) / 2;
            }
        }

        private void tabPageMazeEdit_Resize(object sender, EventArgs e)
        {
            PositionWelcomePanel();
            ChangeModeTo0();
            RedrawFrame();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChangeModeTo0();
            LaunchLink("http://www.mazesuite.com/");
  
        }

        private void scrollUpDown(int viewStep)
        {
            ChangeModeTo0();
            iViewOffsetY += viewStep;
            UpdateOffsetReport();
            RedrawFrame();
        }

        private void scrollLeftRight(int viewStep)
        {
            ChangeModeTo0();
            iViewOffsetX += viewStep;
            UpdateOffsetReport();
            RedrawFrame();
        }

        private void buttonViewMoveUp_Click(object sender, EventArgs e)
        {
            scrollUpDown(iViewOffsetStep);

        }

        private void buttonViewMoveDown_Click(object sender, EventArgs e)
        {
            scrollUpDown(-iViewOffsetStep);
        }

        private void buttonViewMoveRight_Click(object sender, EventArgs e)
        {
            scrollLeftRight(-iViewOffsetStep);
        }

        private void buttonViewMoveLeft_Click(object sender, EventArgs e)
        {
            scrollLeftRight(iViewOffsetStep);
        }

        private void ShowViewMoveButtons(bool show)
        {
            buttonViewMoveDown.Visible = show;
            buttonViewMoveLeft.Visible = show;
            buttonViewMoveRight.Visible = show;
            buttonViewMoveUp.Visible = show;
            buttonViewZoomIn.Visible = show;
            buttonViewZoomOut.Visible = show;
            buttonViewCenter.Visible = show;
            navControlPanel.Visible = show;
        }

        private void buttonViewMoveDown_MouseEnter(object sender, EventArgs e)
        {
           
            this.buttonViewMoveDown.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonDown_mouseover));
            this.buttonViewMoveDown.Invalidate();
        }

        private void buttonViewMoveDown_MouseLeave(object sender, EventArgs e)
        {
            this.buttonViewMoveDown.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonDown));
        }

        private void buttonViewMoveDown_MouseDown(object sender, MouseEventArgs e)
        {
            ChangeModeTo0();
            this.buttonViewMoveDown.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonDown_onclick));
        }

        private void buttonViewMoveUp_MouseEnter(object sender, EventArgs e)
        {
            this.buttonViewMoveUp.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonUp_mouseover));
        }

        private void buttonViewMoveUp_MouseLeave(object sender, EventArgs e)
        {
            this.buttonViewMoveUp.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonUp));
        }

        private void buttonViewMoveUp_MouseDown(object sender, MouseEventArgs e)
        {
            ChangeModeTo0();
            this.buttonViewMoveUp.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonUp_onclick));
        }

        private void buttonViewMoveRight_MouseEnter(object sender, EventArgs e)
        {
            this.buttonViewMoveRight.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonRight_mouseover));
        }

        private void buttonViewMoveRight_MouseLeave(object sender, EventArgs e)
        {
            this.buttonViewMoveRight.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonRight));
        }

        private void buttonViewMoveRight_MouseDown(object sender, MouseEventArgs e)
        {
            ChangeModeTo0();
            this.buttonViewMoveRight.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonRight_onclick));
        }

        private void buttonViewMoveLeft_MouseEnter(object sender, EventArgs e)
        {
            this.buttonViewMoveLeft.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonLeft_mouseover));
        }

        private void buttonViewMoveLeft_MouseLeave(object sender, EventArgs e)
        {
            this.buttonViewMoveLeft.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonLeft));
        }

        private void buttonViewMoveLeft_MouseDown(object sender, MouseEventArgs e)
        {
            ChangeModeTo0();
            this.buttonViewMoveLeft.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonLeft_onclick));
        }

        private void buttonViewMoveUp_MouseUp(object sender, MouseEventArgs e)
        {
            ChangeModeTo0();
            this.buttonViewMoveUp.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonUp_mouseover));
        }

        private void buttonViewMoveRight_MouseUp(object sender, MouseEventArgs e)
        {
            ChangeModeTo0();
            this.buttonViewMoveRight.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonRight_mouseover));
        }

        private void buttonViewMoveLeft_MouseUp(object sender, MouseEventArgs e)
        {
            ChangeModeTo0();
            this.buttonViewMoveLeft.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonLeft_mouseover));
        }

        private void buttonViewMoveDown_MouseUp(object sender, MouseEventArgs e)
        {
            ChangeModeTo0();
            this.buttonViewMoveDown.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonDown_mouseover));
        }

        private void ResetCanvasOffset()
        {
            iViewOffsetX = 0;
            iViewOffsetY = 0;
            UpdateOffsetReport();
        }

        private void buttonViewCenter_Click(object sender, EventArgs e)
        {
            ResetCanvasOffset();
            RedrawFrame();
        }

        private void buttonViewCenter_MouseDown(object sender, MouseEventArgs e)
        {
            this.buttonViewCenter.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonCenter_onclick));
        }

        private void buttonViewCenter_MouseLeave(object sender, EventArgs e)
        {
            this.buttonViewCenter.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonCenter));
        }

        private void buttonViewCenter_MouseUp(object sender, MouseEventArgs e)
        {
            this.buttonViewCenter.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonCenter_mouseover));
        }

        private void buttonViewCenter_MouseEnter(object sender, EventArgs e)
        {
            this.buttonViewCenter.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonCenter_mouseover));
        }

        private void tabPageMazeEdit_Click(object sender, EventArgs e)
        {

        }

        private void zoomMazeView(float multiplier, Vector2 mouseLoc)
        {
            UpdateOffsetReport();

            Vector2 viewOffset = Mouse2Maze(0, 0);

            Vector2 mouseLocMaze = Mouse2Maze(mouseLoc.X,mouseLoc.Y);
            curMaze.Scale *= multiplier;
            //curMaze.Scale=Math.Ceiling(curMaze.Scale);
            //iGridStepSize = (int)curMaze.Scale;
            Vector2 newMouseLoc = Maze2Mouse(mouseLocMaze.X, mouseLocMaze.Y);
            iViewOffsetX += (int)(mouseLoc.X - newMouseLoc.X); //-(e.Location.X*0.25f);
            iViewOffsetY += (int)(mouseLoc.Y - newMouseLoc.Y); //(int)(e.Location.Y * 0.25f);
   
  
            UpdateOffsetReport();
            RedrawFrame();

        }

        private const int WM_SCROLL = 276; // Horizontal scroll 
        private const int SB_LINELEFT = 0; // Scrolls one cell left 
        private const int SB_LINERIGHT = 1; // Scrolls one line right
        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);


        private void tabPageMazeEdit_MouseWheel(object sender, MouseEventArgs e)
        {

            var direction = e.Delta > 0 ? SB_LINELEFT : SB_LINERIGHT;
            if (ModifierKeys == Keys.Control)
            {

                if (direction == 0)
                {
                    Vector2 mouseLoc = new Vector2(curMouseLocation.X, curMouseLocation.Y);
                    zoomMazeView(1.1f, mouseLoc);
                }
                else
                {
                    Vector2 mouseLoc = new Vector2(curMouseLocation.X, curMouseLocation.Y);
                    zoomMazeView(0.9f, mouseLoc);
                }
               
                //SendMessage(this.tabPageMazeEdit.Handle, WM_SCROLL, (IntPtr)direction, IntPtr.Zero);
            }
            else if (ModifierKeys == Keys.Shift)
            {
                if (direction == 0)
                    scrollLeftRight(-iViewOffsetStep);
                else
                    scrollLeftRight(+iViewOffsetStep);

                //SendMessage(this.tabPageMazeEdit.Handle, WM_SCROLL, (IntPtr)direction, IntPtr.Zero);
            }
            else { 
                if (direction == 1)
                    scrollUpDown(-iViewOffsetStep);
                else
                    scrollUpDown(+iViewOffsetStep);

            }
        }

        private void UpdateOffsetReport()
        {
            if(curMaze != null)
            { 
                Vector2 viewOffset = Mouse2Maze(0, 0);
                Vector2 mouseLoc = Mouse2Maze(curMouseLocation.X, curMouseLocation.Y);
                toolStripStatusLabel1.Text = "Offset: " + Math.Round(viewOffset.X, 2) + ", " + Math.Round(viewOffset.Y, 2);

                if (selected.Count > 0)
                    toolStripStatusLabel1.Text += "     Selected: " + selected.Count;

                if (curMouseLocation.X > 0 && curMouseLocation.Y > 0 && curMouseLocation.X < tabPageMazeEdit.Width&&curMouseLocation.Y<tabPageMazeEdit.Height)
                    toolStripStatusLabel1.Text += "     " +"Position: "+Math.Round(mouseLoc.X,2) + ","+Math.Round(mouseLoc.Y,2);

                
            }
        }

        Point curMouseLocation = new Point(0, 0);


        private void XYZ_axisImagebox_MouseDown(object sender, MouseEventArgs e)
        {
            MouseEventArgs newE = new MouseEventArgs(e.Button, e.Clicks, e.Location.X + XYZ_axisImagebox.Location.X, e.Location.Y + XYZ_axisImagebox.Location.Y, e.Delta);
            tabPageMazeEdit_MouseDown(sender, newE);
        }

        private void XYZ_axisImagebox_MouseUp(object sender, MouseEventArgs e)
        {
            MouseEventArgs newE = new MouseEventArgs(e.Button, e.Clicks, e.Location.X + XYZ_axisImagebox.Location.X, e.Location.Y + XYZ_axisImagebox.Location.Y, e.Delta);
            tabPageMazeEdit_MouseUp(sender, newE);
        }

        private Vector2 Mouse2Maze(float X,float Y)
        {
            Vector2 mzCoord;
            X = X - iViewOffsetX;
            Y = Y - iViewOffsetY;
            mzCoord.X = X / (float)curMaze.Scale;
            mzCoord.Y = Y / (float)curMaze.Scale;

            return mzCoord;
        }

        private Vector2 Maze2Mouse(float X, float Y)
        {
            Vector2 mouseCoord;
            
            mouseCoord.X = X * (float)curMaze.Scale;
            mouseCoord.Y = Y * (float)curMaze.Scale;
            mouseCoord.X = mouseCoord.X + iViewOffsetX;
            mouseCoord.Y = mouseCoord.Y + iViewOffsetY;
            return mouseCoord;
        }


        private void RedrawFrame()
        {
            if (this.panelWelcome.Visible|| (curMaze == null))
            {
                tabPageMazeEdit.BackgroundImage = MazeMaker.Properties.Resources.mazeMakerBG;
                tabPageMazeEdit.BackgroundImageLayout = ImageLayout.Tile;
            }
            else
            { 
                Image buffer = tabPageMazeEdit_PaintToBuffer();
                tabPageMazeEdit.BackgroundImage = buffer;
                tabPageMazeEdit.BackgroundImageLayout = ImageLayout.Center;
                tabControlMazeDisplay.Invalidate();
                //tabControlMazeDisplay.Refresh();
                //this.InvokePaint(tabControlMazeDisplay, null);

            }

            //var g = Graphics.FromHwnd(this.Handle);
            //g.DrawImage(buffer, 0, 0);

            //buffer.Dispose();
        }

        private void pictureBoxMainWindowRightTopLogo_MouseEnter(object sender, EventArgs e)
        {
            this.pictureBoxMainWindowRightTopLogo.Image = ((System.Drawing.Image)(Properties.Resources.MazeSuite_MouseOver));
        }

        private void pictureBoxMainWindowRightTopLogo_MouseLeave(object sender, EventArgs e)
        {
            this.pictureBoxMainWindowRightTopLogo.Image = ((System.Drawing.Image)(Properties.Resources.MazeSuite));
        }

 
        

        private void XYZ_axisImagebox_MouseClick(object sender, MouseEventArgs e)
        {
            MouseEventArgs newE = new MouseEventArgs(e.Button, e.Clicks, e.Location.X + XYZ_axisImagebox.Location.X, e.Location.Y + XYZ_axisImagebox.Location.Y, e.Delta);
            tabPageMazeEdit_Click(sender, newE);
        }


        private void pictureBoxMainWindowRightTopLogo_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            LaunchLink("http://www.mazesuite.com");
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            redo();
        }

        private void ts_maz_actReg_Click(object sender, EventArgs e)
        {
            ChangeMode(Mode.actReg0);
        }


        private void navControlPanel_MouseDown(object sender, MouseEventArgs e)
        {
            MouseEventArgs newE = new MouseEventArgs(e.Button, e.Clicks, e.Location.X + navControlPanel.Location.X, e.Location.Y + navControlPanel.Location.Y, e.Delta);
            tabPageMazeEdit_MouseDown(sender, newE);
        }

        private void navControlPanel_MouseUp(object sender, MouseEventArgs e)
        {
            MouseEventArgs newE = new MouseEventArgs(e.Button, e.Clicks, e.Location.X + navControlPanel.Location.X, e.Location.Y + navControlPanel.Location.Y, e.Delta);
            tabPageMazeEdit_MouseUp(sender, newE);
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            Cut();
        }

        public void themeButton_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem t = (ToolStripMenuItem)sender;

            int i = 0;
            foreach (ToolStripMenuItem t2 in themeToolStripMenuItem.DropDownItems)
            {
                if (String.Compare(t2.Text,t.Text)==0)
                    break;
                i++;
            }

            setTheme(i);
        }

        public void setTheme(int index)
        {
            markAllThemesFalse();

            curMazeTheme = mazeThemeLibrary.GetThemeByIndex(index);
            RedrawFrame();
            CurrentSettings.themeIndex = curMazeTheme.themeIndex;
            CurrentSettings.SaveSettings();

            ToolStripMenuItem t = (ToolStripMenuItem)themeToolStripMenuItem.DropDownItems[index];
            t.Checked = true;
            t = (ToolStripMenuItem)toolStripDropdown_Theme.DropDownItems[index];
            t.Checked = true;


        }

        private void markAllThemesFalse()
        {
            foreach (ToolStripMenuItem t in themeToolStripMenuItem.DropDownItems)
            {
                t.Checked = false;
            }

            foreach (ToolStripMenuItem t in toolStripDropdown_Theme.DropDownItems)
            {
                t.Checked = false;
            }
        }
        


        private void buttonViewZoomIn_Click(object sender, EventArgs e)
        {
            Vector2 midPoint= new Vector2(tabPageMazeEdit.Width/2+tabPageMazeEdit.Location.X,tabPageMazeEdit.Height / 2 + tabPageMazeEdit.Location.Y);
            zoomMazeView(1.1f, midPoint);
        }

        private void buttonViewZoomOut_Click(object sender, EventArgs e)
        {
            Vector2 midPoint = new Vector2(tabPageMazeEdit.Width / 2 + tabControlMazeDisplay.Location.X, tabPageMazeEdit.Height / 2 + tabControlMazeDisplay.Location.Y);
            zoomMazeView(0.9f, midPoint);
        }

        private void buttonViewZoomOut_MouseEnter(object sender, EventArgs e)
        {
            this.buttonViewZoomOut.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonZoomOut_MouseOver));
            this.buttonViewZoomOut.Invalidate();
        }

        private void buttonViewZoomIn_MouseEnter(object sender, EventArgs e)
        {
            this.buttonViewZoomIn.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonZoomIn_Mouseover));
            this.buttonViewZoomIn.Invalidate();
        }

        private void buttonViewZoomIn_MouseLeave(object sender, EventArgs e)
        {
            this.buttonViewZoomIn.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonZoomIn));
        }

        private void buttonViewZoomOut_MouseLeave(object sender, EventArgs e)
        {
            this.buttonViewZoomOut.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonZoomOut));
        }

        private void buttonViewZoomOut_MouseDown(object sender, MouseEventArgs e)
        {
            ChangeModeTo0();
            this.buttonViewZoomOut.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonZoomOut_MouseDown));
        }

        private void buttonViewZoomIn_MouseDown(object sender, MouseEventArgs e)
        {
            ChangeModeTo0();
            this.buttonViewZoomIn.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.buttonZoomIn_MouseDown));
        }

        private void ts_maze_pan_Click(object sender, EventArgs e)
        {
            ChangeMode(Mode.pan);
        }

        private void showHideToolLabelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showHideToolLabelsToolStripMenuItem.Checked = !showHideToolLabelsToolStripMenuItem.Checked;
            foreach (ToolStripButton tp in toolStrip_maze.Items)
            {
                if (showHideToolLabelsToolStripMenuItem.Checked)
                    tp.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                else
                    tp.DisplayStyle = ToolStripItemDisplayStyle.Image;
                tp.Invalidate();
            }

            foreach (ToolStripButton tp in toolStrip_coreIO.Items)
            {
                if (showHideToolLabelsToolStripMenuItem.Checked)
                    tp.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                else
                    tp.DisplayStyle = ToolStripItemDisplayStyle.Image;
                tp.Invalidate();
            }
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Vector2 midPoint = new Vector2(tabPageMazeEdit.Width / 2 + tabPageMazeEdit.Location.X, tabPageMazeEdit.Height / 2 + tabPageMazeEdit.Location.Y);
            zoomMazeView(1.1f, midPoint);
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Vector2 midPoint = new Vector2(tabPageMazeEdit.Width / 2 + tabPageMazeEdit.Location.X, tabPageMazeEdit.Height / 2 + tabPageMazeEdit.Location.Y);
            zoomMazeView(0.9f, midPoint);
        }

        private void resetZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {

            double curScale=curMaze.Scale;
            Vector2 midPoint = new Vector2(tabPageMazeEdit.Width / 2 + tabPageMazeEdit.Location.X, tabPageMazeEdit.Height / 2 + tabPageMazeEdit.Location.Y);


            zoomMazeView((float)17.0f/(float)curScale, midPoint);

            
            UpdateOffsetReport();
            RedrawFrame();
        }

        private void showGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            showGridToolStripMenuItem.Checked = !showGridToolStripMenuItem.Checked;
            bShowGrid = showGridToolStripMenuItem.Checked;
            RedrawFrame();
        }

        private void toolStripDropdown_Theme_Click(object sender, EventArgs e)
        {

        }

        private void resizeMazeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            try
            {
                ResizeMazeDialog testDialog = new ResizeMazeDialog();

                // Show testDialog as a modal dialog and determine if DialogResult = OK.
                if (testDialog.ShowDialog(this) == DialogResult.OK)
                {
                    // Read the contents of testDialog's TextBox.
                    //this.txtResult.Text = testDialog.TextBox1.Text;
                    curMaze.ResizeAllCoordinatesXYZ(testDialog.horizontalResize/100.0f, testDialog.heightResize / 100.0f, testDialog.verticalResize / 100.0f);
                    RedrawFrame();
                    propertyGrid.Refresh();
                    MazeChanged();
                }
                else
                {
                    //this.txtResult.Text = "Cancelled";
                }
                testDialog.Dispose();

               
            }
            catch
            {
            }
        }

        private void tabPageMazeEdit_Click_1(object sender, EventArgs e)
        {

        }

        private void button_NewMazeList_Click(object sender, EventArgs e)
        {
            NewMaze();

            MazeListBuilder mlb = new MazeListBuilder();
            mlb.ShowDialog();
        }

        private void button_OpenMazeList_Click(object sender, EventArgs e)
        {
            NewMaze();

            MazeListBuilder mlb = new MazeListBuilder();
            mlb.Open(sender, e);
            mlb.ShowDialog();
        }

        private void exportToClassicFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            if (curMaze == null)
            {
                return;
            }

            if(MessageBox.Show("Warning: Some Functionality May Be Lost\nDo you wish to save in the old format?", "MazeMaker", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            if (curMaze.cStart.Count == 0)
            {
                if (MessageBox.Show("No start location has been specified!\n\nDo you want to save without start location?", "MazeMaker", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;
            }

            SaveFileDialog a = new SaveFileDialog();
            a.Filter = "Maze Files (*.maz)|*.maz";
            a.FilterIndex = 1;
            a.DefaultExt = ".maz";
            a.RestoreDirectory = true;

            if (this.Text[this.Text.Length - 1] != '*')
                a.FileName = this.Text.Substring(12);
            else
                a.FileName = this.Text.Substring(12, this.Text.Length - 13);

            if (a.ShowDialog() == DialogResult.OK)
            {
                curMaze.SaveToClassicFile(a.FileName);
                this.Text = "MazeMaker - " + a.FileName;
                CurrentSettings.AddMazeFileToPrevious(a.FileName);

                UpdateTree();
            }
        }

        private void ts_maze_curve_wall_Click(object sender, EventArgs e)
        {
            ChangeMode(Mode.curveWall0);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModeTo0();
            SelectAll();
        }

    }
}