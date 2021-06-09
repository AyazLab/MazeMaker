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
using System.Security.AccessControl;

#endregion

namespace MazeMaker
{
    public enum ItemType
    {
        MazeListOptions, Maze, Text, Image, MultipleChoice, RecordAudio, Command
    }

    public class ListItem : Object
    {
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

    public class MazeListOptionsListItem : ListItem
    {
        public MazeListOptionsListItem()
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
        [Description("Forces full screen for all items when using this MazeList")]
        [DisplayName("Full Screen")]
        public bool? FullScreen
        {
            get { return fullScreen; }
            set { fullScreen = value; }
        }

        private bool? comPortEnabled;
        [Category("General")]
        [Description("Overrides default COM/serial port when using this MazeList")]
        [DisplayName("COM Port Enabled")]
        public bool? ComPortEnabled
        {
            get { return comPortEnabled; }
            set { comPortEnabled = value; }
        }

        private bool? lsl;
        [Category("General")]
        [Description("Overrides default LSL when using this MazeList")]
        [DisplayName("LSL")]
        public bool? Lsl
        {
            get { return lsl; }
            set { lsl = value; }
        }

        private bool? lpt;
        [Category("General")]
        [Description("Overrides LPT when using this MazeList")]
        [DisplayName("LPT")]
        public bool? Lpt
        {
            get { return lpt; }
            set { lpt = value; }
        }

        private int? fontSize;
        [Category("General")]
        [Description("Overides font size for all items when using this MazeList")]
        [DisplayName("Font Size")]
        public int? FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }

        public override string ToString()
        {
            return "[" + Type + "]";
        }
    }

    public class MazeListItem : ListItem
    {
        public MazeListItem()
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

        string mazeFile = "";
        [Category("General")]
        [Description("Specify an maze filename to be shown")]
        [DisplayName("Maze File")]
        [TypeConverter(typeof(MazeFileConverter))]
        public string MazeFile
        {
            get { return mazeFile; }
            set { mazeFile = value; }
        }

        private string startPosition = "";
        [Category("General")]
        [DisplayName("Start Position")]
        public string StartPosition
        {
            get { return startPosition; }
            set { startPosition = value; }
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
            return "[" + Type + "] - " + mazeFile;
        }
    }

    public class TextListItem : ListItem
    {
        public enum DisplayType
        {
            OnFramedDialog, OnBackground
        }

        public TextListItem()
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

        private string audioFile = "";
        [Category("Audio")]
        [Description("Specify an audio filename to be played")]
        [DisplayName("Audio File")]
        [TypeConverter(typeof(AudioFileConverter))]
        public string AudioFile
        {
            get { return audioFile; }
            set { audioFile = value; }
        }

        private bool loop = false;
        [Category("Audio")]
        [Description("If true, enables continuous play of selected audo during highlight.")]
        public bool Loop
        {
            get { return loop; }
            set { loop = value; }
        }

        private string audioBehavior = "";
        [Category("Audio")]
        [Description("Audio behavior when audio plays")]
        [DisplayName("Audio Behavior")]
        [TypeConverter(typeof(AudioBehaviorConverter))]
        public string AudioBehavior
        {
            get { return audioBehavior; }
            set { audioBehavior = value; }
        }

        private string endBehavior = "";
        [Category("Audio")]
        [Description("Audio behavior when stimulus ends")]
        [DisplayName("End Behavior")]
        [TypeConverter(typeof(EndBehaviorConverter))]
        public string EndBehavior
        {
            get { return endBehavior; }
            set { endBehavior = value; }
        }

        private int fontSize = 12;
        [Category("Text Display")]
        [Description("Specify an font size to be displayed")]
        [DisplayName("Font Size")]
        public int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }
    }

    public class ImageListItem : ListItem
    {
        public enum DisplayType
        {
            OnFramedDialog, OnBackground
        }

        public ImageListItem()
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

        private Color backgroundColor;
        [Category("Display")]
        [Description("Set background color using color name or RGB values seperated by a comma: white or 255, 255, 255")]
        [DisplayName("Background Color")]
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        private string imageFile = "";
        [Category("Display")]
        [Description("Specify an image filename to be displayed")]
        [DisplayName("Image File")]
        [TypeConverter(typeof(ImageFileConverter))]
        public string ImageFile
        {
            get { return imageFile; }
            set { imageFile = value; }
        }

        private string audioFile = "";
        [Category("Audio")]
        [Description("Specify an audio filename to be played")]
        [DisplayName("Audio File")]
        [TypeConverter(typeof(AudioFileConverter))]
        public string AudioFile
        {
            get { return audioFile; }
            set { audioFile = value; }
        }

        private bool loop = false;
        [Category("Audio")]
        [Description("If true, enables continuous play of selected audo during highlight.")]
        public bool Loop
        {
            get { return loop; }
            set { loop = value; }
        }

        private string audioBehavior = "";
        [Category("Audio")]
        [Description("Audio behavior when audio plays")]
        [DisplayName("Audio Behavior")]
        [TypeConverter(typeof(AudioBehaviorConverter))]
        public string AudioBehavior
        {
            get { return audioBehavior; }
            set { audioBehavior = value; }
        }

        private string endBehavior = "";
        [Category("Audio")]
        [Description("Audio behavior when stimulus ends")]
        [DisplayName("End Behavior")]
        [TypeConverter(typeof(EndBehaviorConverter))]
        public string EndBehavior
        {
            get { return endBehavior; }
            set { endBehavior = value; }
        }

        private int fontSize = 12;
        [Category("Text Display")]
        [Description("Specify an font size to be displayed")]
        [DisplayName("Font Size")]
        public int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }
    }

    public class TextReturn
    {
        public TextReturn(string text)
        {
            Text = text;
        }

        string text = "";
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        string ret = "";
        [DisplayName("Return")]
        public string Ret
        {
            get { return ret; }
            set { ret = value; }
        }

        public override string ToString()
        {
            return Text;
        }
    }

    public class MultipleChoiceListItem : ListItem
    {
        public enum DisplayType
        {
            OnFramedDialog, OnBackground
        }

        public MultipleChoiceListItem()
        {
            //this.Value = "Enter message here!..";
            this.Type = ItemType.MultipleChoice;
            items.Add(new TextReturn("Question?"));
            items.Add(new TextReturn("Option1"));
            items.Add(new TextReturn("Option2"));
            GetString();
        }

        public MultipleChoiceListItem(ListChangedEventHandler updated)
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

        private string audioFile = "";
        [Category("Audio")]
        [Description("Specify an audio filename to be played")]
        [DisplayName("Audio File")]
        [TypeConverter(typeof(AudioFileConverter))]
        public string AudioFile
        {
            get { return audioFile; }
            set { audioFile = value; }
        }

        private bool loop = false;
        [Category("Audio")]
        [Description("If true, enables continuous play of selected audo during highlight.")]
        public bool Loop
        {
            get { return loop; }
            set { loop = value; }
        }

        private string audioBehavior = "";
        [Category("Audio")]
        [Description("Audio behavior when audio plays")]
        [DisplayName("Audio Behavior")]
        [TypeConverter(typeof(AudioBehaviorConverter))]
        public string AudioBehavior
        {
            get { return audioBehavior; }
            set { audioBehavior = value; }
        }

        private string endBehavior = "";
        [Category("Audio")]
        [Description("Audio behavior when stimulus ends")]
        [DisplayName("End Behavior")]
        [TypeConverter(typeof(EndBehaviorConverter))]
        public string EndBehavior
        {
            get { return endBehavior; }
            set { endBehavior = value; }
        }
    }

    public class RecordAudioListItem : ListItem
    {
        public enum DisplayType
        {
            OnFramedDialog, OnBackground
        }

        public RecordAudioListItem()
        {
            Type = ItemType.RecordAudio;
        }

        protected new string text = "";
        [Browsable(false)]

        public new string Text
        {
            get { return text; }
            set { text = value; }
        }

        public override string ToString()
        {
            return "[" + Type + "]";
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

        private Color backgroundColor;
        [Category("Display")]
        [Description("Set background color using color name or RGB values seperated by a comma: white or 255, 255, 255")]
        [DisplayName("Background Color")]
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        private string imageFile = "";
        [Category("Display")]
        [Description("Specify an image filename to be displayed")]
        [DisplayName("Image File")]
        [TypeConverter(typeof(ImageFileConverter))]
        public string ImageFile
        {
            get { return imageFile; }
            set { imageFile = value; }
        }

        private int fontSize = 12;
        [Category("Text Display")]
        [Description("Specify an font size to be displayed")]
        [DisplayName("Font Size")]
        public int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }
    }

    public class CommandListItem : ListItem
    {
        public CommandListItem()
        {
            Type = ItemType.Command;
            
        }

        protected new string text = "";
        [Browsable(false)]
        public new string Text
        {
            get { return text; }
            set { text = value; }
        }

        private string command = "";
        [Description("Windows Shell Command")]
        public string Command
        {
            get { return command; }
            set { command = value; }
        }

        private bool wait4Complete = true;
        [Description("Wait for shell program to exit before continuing maze list")]
        [DisplayName("Wait For Complete")]
        public bool Wait4Complete
        {
            get { return wait4Complete; }
            set { wait4Complete = value; }
        }
        
        public override string ToString()
        {
            return "[" + Type + "] - " + Command;
        }

        private bool hideCommand = false;
        [Description("Show/Hide Command Options")]
        [DisplayName("Hide Command")]
        public bool HideCommand
        {
            get { return hideCommand; }
            set { hideCommand = value; }
        }
    }

    public class MazeFileConverter : StringConverter
    {
        List<string> mazes = new List<string>();

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (!MazeListBuilder.madeChanges)
                mazes = new List<string>();

            if (!mazes.Contains("[Import Item]"))
            {
                mazes.Add("[Import Item]");
                mazes.Add("[Manage Items]");
                mazes.Add("----------------------------------------");
            }

            foreach (string maze in MazeListBuilder.mazeFilePaths.Keys)
            {
                if (!mazes.Contains(maze))
                {
                    mazes.Add(maze);
                }
            }

            return new StandardValuesCollection(mazes);
        }
    }

    public class ImageFileConverter : StringConverter
    {
        List<string> images = new List<string>();

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (!MazeListBuilder.madeChanges)
                images = new List<string>();

            if (!images.Contains("[Import Item]"))
            {
                images.Add("[Import Item]");
                images.Add("[Manage Items]");
                images.Add("----------------------------------------");
            }

            foreach (string image in MazeListBuilder.imageFilePaths.Keys)
            {
                if (!images.Contains(image))
                {
                    images.Add(image);
                }
            }

            return new StandardValuesCollection(images);
        }
    }

    public class AudioFileConverter : StringConverter
    {
        List<string> audios = new List<string>();

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (!MazeListBuilder.madeChanges)
                audios = new List<string>();

            if (!audios.Contains("[Import Item]"))
            {
                audios.Add("[Import Item]");
                audios.Add("[Manage Items]");
                audios.Add("----------------------------------------");
            }

            foreach (string audio in MazeListBuilder.audioFilePaths.Keys)
            {
                if (!audios.Contains(audio))
                {
                    audios.Add(audio);
                }
            }

            return new StandardValuesCollection(audios);
        }
    }

    public class EndBehaviorConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<string> audioBehaviorTypes = new List<string>();

            audioBehaviorTypes.Add("Stop");
            audioBehaviorTypes.Add("Pause");
            audioBehaviorTypes.Add("Continue");

            return new StandardValuesCollection(audioBehaviorTypes);
        }
    }

    public class AudioBehaviorConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<string> audioBehaviorTypes = new List<string>();

            audioBehaviorTypes.Add("Stop");
            audioBehaviorTypes.Add("Pause");
            audioBehaviorTypes.Add("Continue");
            audioBehaviorTypes.Add("VolumeByDistance");

            return new StandardValuesCollection(audioBehaviorTypes);
        }
    }
}