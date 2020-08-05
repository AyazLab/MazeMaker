

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;

#endregion

namespace MazeMaker
{
    public enum ItemType
    {
        MazeOptions, Maze, Text, Image, MultipleChoice
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

    public class MazeList_MazeOptionsItem : MyBuilderItem
    {
        public MazeList_MazeOptionsItem()
        {
            Type = ItemType.MazeOptions;
        }

        protected new string valueIN = "";
        [Category("General")]
        [Description("Value of the Item")]
        [Browsable(false)]
        public new string Value
        {
            get { return valueIN; }
            set { valueIN = value; }
        }

        private bool? fullScreen;
        [Category("General")]
        [DisplayName("Full Screen")]
        public bool? FullScreen
        {
            get { return fullScreen; }
            set { fullScreen = value; }
        }

        private bool? comPort;
        [Category("General")]
        [DisplayName("COM_Port")]
        public bool? ComPort
        {
            get { return comPort; }
            set { comPort = value; }
        }

        private bool? lsl;
        [Category("General")]
        [DisplayName("LSL")]
        public bool? Lsl
        {
            get { return lsl; }
            set { lsl = value; }
        }

        private bool? lpt;
        [Category("General")]
        [DisplayName("LPT")]
        public bool? Lpt
        {
            get { return lpt; }
            set { lpt = value; }
        }

        private string fontSize = "";
        [Category("General")]
        [DisplayName("Font Size")]
        public string FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }
    }

    public class MazeList_MazeItem : MyBuilderItem
    {
        public MazeList_MazeItem(string inp)
        {
            this.Value = inp;
            this.Type = ItemType.Maze;
        }

        private string defaultStartPosition = "";
        [Category("General")]
        [DisplayName("Default Start Position")]
        public string DefaultStartPosition
        {
            get { return defaultStartPosition; }
            set { defaultStartPosition = value; }
        }

        private string startMessage = "";
        [Category("General")]
        [DisplayName("Default Start Position")]
        public string StartMessage
        {
            get { return startMessage; }
            set { startMessage = value; }
        }

        private string timeout = "";
        [Category("General")]
        [DisplayName("Default Start Position")]
        public string Timeout
        {
            get { return timeout; }
            set { timeout = value; }
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
        [Description("Specify an image name for background")]
        [DisplayName("Background Image")]
        [Browsable(false)]
        public string BackgroundImage
        {
            get { return backgroundImage; }
            set { backgroundImage = value; }
        }

        private string audio = "";
        [Category("General")]
        [Description("Specify an audio filename to be played")]
        [DisplayName("Audio")]
        [TypeConverter(typeof(audioConverter))]
        public string Audio
        {
            get { return audio; }
            set { audio = value; }
        }

        private string fontSize = "";
        [Category("General")]
        [Description("Specify an font size to be displayed")]
        [DisplayName("FontSize")]
        public string FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
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

        private string backgroundColor = "";
        [Category("General")]
        [Description("Set background color using color name or RGB values seperated by a comma: white or 255, 255, 255")]
        [DisplayName("Background Color")]
        public string BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        private string image = "";
        [Category("General")]
        [Description("Specify an image filename to be displayed")]
        [DisplayName("Image")]
        [Editor(typeof(ImageEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        private string audio = "";
        [Category("General")]
        [Description("Specify an audio filename to be played")]
        [DisplayName("Audio")]
        [TypeConverter(typeof(audioConverter))]
        public string Audio
        {
            get { return audio; }
            set { audio = value; }
        }

        private string fontSize = "";
        [Category("General")]
        [Description("Specify an font size to be displayed")]
        [DisplayName("FontSize")]
        public string FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
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

        private string audio = "";
        [Category("General")]
        [Description("Specify an audio filename to be played")]
        [DisplayName("Audio")]
        [TypeConverter(typeof(audioConverter))]
        public string Audio
        {
            get { return audio; }
            set { audio = value; }
        }
    }

    class ImageEditor : UITypeEditor
    {
        List<Texture> images = new List<Texture>();

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            //IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

            if (MazeListBuilder.madeChanges)
            {
                foreach (MyBuilderItem item in MazeListBuilder.myItems)
                {
                    if (item.Type == ItemType.Image)
                    {
                        MazeList_ImageItem image = (MazeList_ImageItem)item;
                        string file = image.Image;

                        if (file != "")
                        {
                            Texture texture = new Texture(file);

                            if (!images.Contains(texture))
                            {
                                images.Add(texture);
                            }
                        }
                    }
                }
            }
            else
            {
                images = new List<Texture>();
                foreach (MyBuilderItem item in MazeListBuilder.myItems)
                {
                    if (item.Type == ItemType.Image)
                    {
                        MazeList_ImageItem image = (MazeList_ImageItem)item;
                        string file = image.Image;

                        if (file != "")
                        {
                            images.Add(new Texture(file));
                        }
                    }
                }
            }

            //if (svc != null)
            //{
            //    MazeMakerCollectionEditor mmce = new MazeMakerCollectionEditor(ref images);
            //    svc.ShowDialog(mmce);
            //}

            MazeMakerCollectionEditor mmce = new MazeMakerCollectionEditor(ref images);
            value = mmce.GetSelectedImage();

            return value;
        }
    }

    public class audioConverter : StringConverter
    {
        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<String> audios = new List<String>();

            foreach (MyBuilderItem item in MazeListBuilder.myItems)
            {
                string audio = "";

                switch (item.Type)
                {
                    case ItemType.Text:
                        MazeList_TextItem text = (MazeList_TextItem)item;
                        audio = text.Audio;
                        break;

                    case ItemType.Image:
                        MazeList_ImageItem image = (MazeList_ImageItem)item;
                        audio = image.Audio;
                        break;

                    case ItemType.MultipleChoice:
                        MazeList_MultipleChoiceItem multipleChoice = (MazeList_MultipleChoiceItem)item;
                        audio = multipleChoice.Audio;
                        break;

                    default:
                        break;
                }

                if (audio != "" && !audios.Contains(audio))
                {
                    audios.Add(audio);
                }
            }

            return new StandardValuesCollection(audios);
        }
    }
}