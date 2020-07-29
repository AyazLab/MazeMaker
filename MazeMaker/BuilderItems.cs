

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

#endregion

namespace MazeMaker
{
    public enum ItemType
    {
        Maze, Text, Image, MultipleChoice
    }

    public class MyBuilderItem : Object
    {
        public MyBuilderItem()
        {

        }

        protected string valueIN = "";
        [Category("General")]
        [Description("Value of the Item")]
        public string Value
        {
            get { return valueIN; }
            set { valueIN = value; }
        }

        private ItemType type;
        [Category("General")]
        [Description("Type of the Item")]
        [ReadOnly(true)]
        public ItemType Type
        {
            get { return type; }
            set { type = value; }
        }
        public override string ToString()
        {
            return "[" + type + "] - " + valueIN;
        }

    }
    public class MazeList_MazeItem : MyBuilderItem
    {
        public MazeList_MazeItem(string inp)
        {
            this.Value = inp;
            this.Type = ItemType.Maze;
        }

    }

   public class MazeList_TextItem : MyBuilderItem
    {

        public enum DisplayType
        {
            OnFramedDialog, OnBackground
        }
        public MazeList_TextItem()
        {
           this.Value = "Enter message here!..";
           this.Type = ItemType.Text;
        }

        private double x = 0;
        [Category("Location")]
        [Description("X Coordinate")]
        [Browsable(false)]
        public double X
        {
            get { return x; }
            set { x = value; }
        }

        private double y = 0;
        [Category("Location")]
        [Description("Y Coordinate")]
        [Browsable(false)]
        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        private long lifeTime = 10000;
        [Category("Display")]
        [Description("Time in milliseconds for the text to be displayed. If zero, waits until user clicks OK")]
        public long LifeTime
        {
            get { return lifeTime; }
            set { lifeTime = value; }
        }

        private DisplayType showlike = DisplayType.OnFramedDialog;
        [Category("Display")]
        [Description("Determines how the text is displayed")]
        public DisplayType TextDisplayType
        {
            get { return showlike; }
            set { showlike = value; }
        }

        private string backgroundImage= "";
        [Category("Display")]
        [Description("Specify and image name for background")]
        [DisplayName("Background Image")]
        [Browsable(false)]
        public string BackgroundImage
        {
            get { return backgroundImage; }
            set { backgroundImage = value; }
        }
    }

    public class MazeList_ImageItem : MyBuilderItem
    {

        public enum DisplayType
        {
            OnFramedDialog, OnBackground
        }
        public MazeList_ImageItem()
        {
            this.Value = "Enter caption here!..";
            this.Type = ItemType.Image;
        }

        private double x = 0;
        [Category("Location")]
        [Description("X Coordinate")]
        [Browsable(false)]
        public double X
        {
            get { return x; }
            set { x = value; }
        }

        private double y = 0;
        [Category("Location")]
        [Description("Y Coordinate")]
        [Browsable(false)]
        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        private long lifeTime = 1000;
        [Category("Display")]
        [Description("Time in milliseconds for the text to be displayed. If zero, waits until user clicks OK")]
        public long LifeTime
        {
            get { return lifeTime; }
            set { lifeTime = value; }
        }

        private DisplayType showlike = DisplayType.OnFramedDialog;
        [Category("Display")]
        [Description("Determines how the text is displayed")]
        public DisplayType TextDisplayType
        {
            get { return showlike; }
            set { showlike = value; }
        }

        private string image = "";
        [Category("General")]
        [Description("Specify an image filename to be displayed")]
        [DisplayName("Image")]
        [TypeConverter(typeof(ImageConverter))]
        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        public class ImageConverter : StringConverter
        {
            public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }

            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                List<String> list = new List<String>();

                foreach (MyBuilderItem item in MazeListBuilder.myItems)
                {
                    if (item.Type == ItemType.Image)
                    {
                        MazeList_ImageItem image = (MazeList_ImageItem)item;

                        if (image.Image != "")
                        {
                            list.Add(image.Image);
                        }
                    }
                }

                return new StandardValuesCollection(list);
            }
        }
    }

    public class MazeList_MultipleChoiceItem : MyBuilderItem
    {

        public enum DisplayType
        {
            OnFramedDialog, OnBackground
        }
        public MazeList_MultipleChoiceItem()
        {
            //this.Value = "Enter message here!..";
            this.Type = ItemType.MultipleChoice;
            items.Add("Question?");
            items.Add("Option1");
            items.Add("Option2");
            GetString();
        }

        public MazeList_MultipleChoiceItem(ListChangedEventHandler updated)
        {
            this.Type = ItemType.MultipleChoice;
            items.Add("Question?");
            items.Add("Option1");
            items.Add("Option2");
            GetString();
            items.ListChanged += updated;
        }

    
        public override string ToString()
        {
            return "[" + this.Type + "] - " + GetString();
        }

        public bool LoadString(string inp)
        {
           try
           {
               String[] val = inp.Split(new string[] { "\\a" }, StringSplitOptions.RemoveEmptyEntries);

               items.Clear();
               for (int i = 0; i < val.Length; i++)
               {
                   items.Add(val[i]);
               }
               GetString();

               return true;
           }
            catch
           {
               return false;
           }
            
        }

        public string GetString()
        {
            
            string[] arr = new string[items.Count];
            items.CopyTo(arr, 0);
            this.valueIN = string.Join("\\a", arr);
            return this.valueIN;
        }

        BindingList<string> items = new BindingList<string>(); 
        [Category("General")]
        [Description("Value of the Item")]
        public new BindingList<string> Value
        {
            get 
            { 
                return items; 
            }
            set
            { 
                items = value;
                GetString();
            }
        }

        private double x = 0;
        [Category("Location")]
        [Description("X Coordinate")]
        [Browsable(false)]
        public double X
        {
            get { return x; }
            set { x = value; }
        }

        private double y = 0;
        [Category("Location")]
        [Description("Y Coordinate")]
        [Browsable(false)]
        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        private long lifeTime = 0;
        [Category("Display")]
        [Description("Time in milliseconds for the text to be displayed. If zero, waits until user clicks OK")]
        public long LifeTime
        {
            get { return lifeTime; }
            set { lifeTime = value; }
        }

        private DisplayType showlike = DisplayType.OnFramedDialog;
        [Category("Display")]
        [Description("Determines how the text is displayed")]
        public DisplayType TextDisplayType
        {
            get { return showlike; }
            set { showlike = value; }
        }

        private string backgroundImage = "";
        [Category("Display")]
        [Description("Specify and image name for background")]
        [DisplayName("Background Image")]
        [Browsable(false)]
        public string BackgroundImage
        {
            get { return backgroundImage; }
            set { backgroundImage = value; }
        }
    }




}