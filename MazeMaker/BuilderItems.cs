#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Drawing;
using System.ComponentModel.Design;
using System.Collections.ObjectModel;
using System.Linq;

#endregion

namespace MazeMaker
{
    public enum ItemType
    {
        MazeListOptions, Maze, Text, Image, MultipleChoice
    }

    public class MyBuilderItem : Object
    {
        public MyBuilderItem()
        {

        }

        protected string text = "";
        [Category("Text Display")]
        [Description("Text")]
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        private ItemType type;
        [Category("General")]
        [Description("Type of the Item")]
        [ReadOnly(true)]
        [Browsable(false)]
        public ItemType Type
        {
            get { return type; }
            set { type = value; }
        }

        public override string ToString()
        {
            return "[" + type + "] - " + text;
        }
    }

    public class MazeList_MazeListOptionsItem : MyBuilderItem
    {
        public MazeList_MazeListOptionsItem()
        {
            Type = ItemType.MazeListOptions;
        }

        protected new string text = "";
        [Browsable(false)]
        public new string Text
        {
            get { return text; }
            set { text = value; }
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

        public override string ToString()
        {
            return "[" + Type + "]";
        }
    }

    public class MazeList_MazeItem : MyBuilderItem
    {
        public MazeList_MazeItem()
        {
            this.Type = ItemType.Maze;
        }

        protected new string text = "";
        [Browsable(false)]
        public new string Text
        {
            get { return text; }
            set { text = value; }
        }

        string maze = "";
        [Category("General")]
        [Description("Specify an maze filename to be shown")]
        [TypeConverter(typeof(MazeConverter))]
        public string Maze
        {
            get { return maze; }
            set { maze = value; }
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
        [DisplayName("Start Message")]
        public string StartMessage
        {
            get { return startMessage; }
            set { startMessage = value; }
        }

        private string timeout = "";
        [Category("Timing")]
        [DisplayName("Timeout")]
        public string Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        public override string ToString()
        {
            return "[" + Type + "] - " + maze;
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
           this.Text = "Enter message here!..";
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

        private long duration = 10000;
        [Category("Timing")]
        [Description("Time in milliseconds for the text to be displayed. If zero, waits until user clicks OK")]
        public long Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        private DisplayType showlike = DisplayType.OnFramedDialog;
        [Category("Style")]
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
        [Category("Highlight")]
        [Description("Specify an audio filename to be played")]
        [DisplayName("Audio")]
        [TypeConverter(typeof(audioConverter))]
        public string Audio
        {
            get { return audio; }
            set { audio = value; }
        }

        private string audioOnUnhighlight;
        [Category("Highlight")]
        [Description("Descibes behavior for Audio play back when highlight ends")]
        [DisplayName("Audio On Unhighlight")]
        [TypeConverter(typeof(AudioOnUnhighlightConverter))]
        public string AudioOnUnhighlight
        {
            get { return audioOnUnhighlight; }
            set { audioOnUnhighlight = value; }
        }

        private string fontSize = "";
        [Category("Text Display")]
        [Description("Specify an font size to be displayed")]
        [DisplayName("Font Size")]
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
            this.Text = "Enter caption here!..";
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

        private long duration = 1000;
        [Category("Timing")]
        [Description("Time in milliseconds for the text to be displayed. If zero, waits until user clicks OK")]
        public long Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        private DisplayType showlike = DisplayType.OnFramedDialog;
        [Category("Style")]
        [Description("Determines how the text is displayed")]
        public DisplayType TextDisplayType
        {
            get { return showlike; }
            set { showlike = value; }
        }

        private string backgroundColor = "";
        [Category("Display")]
        [Description("Set background color using color name or RGB values seperated by a comma: white or 255, 255, 255")]
        [DisplayName("Background Color")]
        public string BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        private string image = "";
        [Category("Display")]
        [Description("Specify an image filename to be displayed")]
        [DisplayName("Image")]
        [TypeConverter(typeof(ImageConverter))]
        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        private string audio = "";
        [Category("Highlight")]
        [Description("Specify an audio filename to be played")]
        [DisplayName("Audio")]
        [TypeConverter(typeof(audioConverter))]
        public string Audio
        {
            get { return audio; }
            set { audio = value; }
        }

        private string audioOnUnhighlight;
        [Category("Highlight")]
        [Description("Descibes behavior for Audio play back when highlight ends")]
        [DisplayName("Audio On Unhighlight")]
        [TypeConverter(typeof(AudioOnUnhighlightConverter))]
        public string AudioOnUnhighlight
        {
            get { return audioOnUnhighlight; }
            set { audioOnUnhighlight = value; }
        }

        private string fontSize = "";
        [Category("Text Display")]
        [Description("Specify an font size to be displayed")]
        [DisplayName("Font Size")]
        public string FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }
    }

    public class TextReturn
    {
        public override string ToString()
        {
            return Text;
        }

        public TextReturn()
        {

        }

        public TextReturn(string text)
        {
            Text = text;
        }

        string text = "";
        public string Text
        {
            get { return text; }
            set { this.text = value; }
        }

        string ret = "";
        [DisplayName("Return")]
        public string Ret
        {
            get { return ret; }
            set { ret = value; }
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
            items.Add(new TextReturn("Question?"));
            items.Add(new TextReturn("Option1"));
            items.Add(new TextReturn("Option2"));
            GetString();
        }

        public MazeList_MultipleChoiceItem(ListChangedEventHandler updated)
        {
            this.Type = ItemType.MultipleChoice;
            items.Add(new TextReturn("Question?"));
            items.Add(new TextReturn("Option1"));
            items.Add(new TextReturn("Option2"));
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
                   items.Add(new TextReturn(val[i]));
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

            for (int i = 0; i < items.Count; i++)
            {
                arr[i] = items[i].Text;
            }

            //items.CopyTo(arr, 0);
            this.text = string.Join("\\a", arr);
            return this.text;
        }

        BindingList<TextReturn> items = new BindingList<TextReturn>(); 
        [Category("Text Display")]
        [Description("Text")]
        public new BindingList<TextReturn> Text
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

        private long duration = 0;
        [Category("Timing")]
        [Description("Time in milliseconds for the text to be displayed. If zero, waits until user clicks OK")]
        public long Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        private DisplayType showlike = DisplayType.OnFramedDialog;
        [Category("Style")]
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
        [Category("Highlight")]
        [Description("Specify an audio filename to be played")]
        [DisplayName("Audio")]
        [TypeConverter(typeof(audioConverter))]
        public string Audio
        {
            get { return audio; }
            set { audio = value; }
        }

        private string audioOnUnhighlight;
        [Category("Highlight")]
        [Description("Descibes behavior for Audio play back when highlight ends")]
        [DisplayName("Audio On Unhighlight")]
        [TypeConverter(typeof(AudioOnUnhighlightConverter))]
        public string AudioOnUnhighlight
        {
            get { return audioOnUnhighlight; }
            set { audioOnUnhighlight = value; }
        }
    }

    public class MazeConverter : StringConverter
    {
        List<string> mazes = new List<string>();

        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (!MazeListBuilder.madeChanges)
            {
                mazes = new List<string>();
            }

            if (!mazes.Contains("Import"))
            {
                mazes.Add("Import");
                //mazes.Add("Collection Editor");
            }

            foreach (MyBuilderItem item in MazeListBuilder.mazeList)
            {
                string maze = "";

                if (item.Type == ItemType.Maze)
                {
                    MazeList_MazeItem listItem = (MazeList_MazeItem)item;
                    maze = listItem.Maze;
                }

                if (maze != "" && !mazes.Contains(maze))
                {
                    mazes.Add(maze);
                }
            }

            return new StandardValuesCollection(mazes);
        }
    }

    public class ImageConverter : StringConverter
    {
        List<string> images = new List<string>();

        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (!MazeListBuilder.madeChanges)
            {
                images = new List<string>();
            }

            if (!images.Contains("Import"))
            {
                images.Add("Import");
                images.Add("Collection Editor");
            }

            foreach (MyBuilderItem item in MazeListBuilder.mazeList)
            {
                string image = "";

                if (item.Type == ItemType.Image)
                {
                    MazeList_ImageItem listItem = (MazeList_ImageItem)item;
                    image = listItem.Image;
                }

                if (image != "" && !images.Contains(image))
                {
                    images.Add(image);
                }
            }

            return new StandardValuesCollection(images);
        }
    }

    public class audioConverter : StringConverter
    {
        List<string> audios = new List<string>();

        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (!MazeListBuilder.madeChanges)
            {
                audios = new List<string>();
            }

            if (!audios.Contains("Import"))
            {
                audios.Add("Import");
                audios.Add("Collection Editor");
            }

            foreach (MyBuilderItem item in MazeListBuilder.mazeList)
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

    public class AudioOnUnhighlightConverter : StringConverter
    {
        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override Boolean GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<String> audioOnUnhighlightTypes = new List<String>();

            audioOnUnhighlightTypes.Add("Stop");
            audioOnUnhighlightTypes.Add("Pause");
            audioOnUnhighlightTypes.Add("Continue");
            audioOnUnhighlightTypes.Add("VolumeByDistance");

            return new StandardValuesCollection(audioOnUnhighlightTypes);
        }
    }
}