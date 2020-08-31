namespace MazeMaker
{
    partial class MazeListBuilder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStripButton_Open = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_New = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Append = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_SaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Package = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStrip_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelListItems = new System.Windows.Forms.Panel();
            this.toolStripListItems = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButtonMazeItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTextItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImageItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMultipleChoiceItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRecordAudio = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCommand = new System.Windows.Forms.ToolStripButton();
            this.panelProperties = new System.Windows.Forms.Panel();
            this.treeViewMazeList = new System.Windows.Forms.TreeView();
            this.closeButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.labelMazeListItems = new System.Windows.Forms.Label();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.L_Del = new System.Windows.Forms.Button();
            this.L_Down = new System.Windows.Forms.Button();
            this.L_Up = new System.Windows.Forms.Button();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.appendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panelListItems.SuspendLayout();
            this.toolStripListItems.SuspendLayout();
            this.panelProperties.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripButton_Open
            // 
            this.toolStripButton_Open.Image = global::MazeMaker.Properties.Resources.open;
            this.toolStripButton_Open.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton_Open.Name = "toolStripButton_Open";
            this.toolStripButton_Open.Size = new System.Drawing.Size(81, 36);
            this.toolStripButton_Open.Text = "Open";
            this.toolStripButton_Open.Click += new System.EventHandler(this.toolStrip_open_Click);
            // 
            // toolStripButton_Save
            // 
            this.toolStripButton_Save.Image = global::MazeMaker.Properties.Resources.save;
            this.toolStripButton_Save.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton_Save.Name = "toolStripButton_Save";
            this.toolStripButton_Save.Size = new System.Drawing.Size(76, 36);
            this.toolStripButton_Save.Text = "Save";
            this.toolStripButton_Save.Click += new System.EventHandler(this.toolStrip_save_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_New,
            this.toolStripButton_Open,
            this.toolStripButton_Append,
            this.toolStripSeparator1,
            this.toolStripButton_Save,
            this.toolStripButton_SaveAs,
            this.toolStripButton_Package});
            this.toolStrip.Location = new System.Drawing.Point(0, 28);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip.Size = new System.Drawing.Size(902, 39);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripButton_New
            // 
            this.toolStripButton_New.Image = global::MazeMaker.Properties.Resources._new;
            this.toolStripButton_New.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_New.Name = "toolStripButton_New";
            this.toolStripButton_New.Size = new System.Drawing.Size(75, 36);
            this.toolStripButton_New.Text = "New";
            this.toolStripButton_New.Click += new System.EventHandler(this.toolStripButton_New_Click);
            // 
            // toolStripButton_Append
            // 
            this.toolStripButton_Append.Image = global::MazeMaker.Properties.Resources.add;
            this.toolStripButton_Append.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Append.Name = "toolStripButton_Append";
            this.toolStripButton_Append.Size = new System.Drawing.Size(98, 36);
            this.toolStripButton_Append.Text = "Append";
            this.toolStripButton_Append.Click += new System.EventHandler(this.toolStripButton_Append_Click);
            // 
            // toolStripButton_SaveAs
            // 
            this.toolStripButton_SaveAs.Image = global::MazeMaker.Properties.Resources.saveas;
            this.toolStripButton_SaveAs.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton_SaveAs.Name = "toolStripButton_SaveAs";
            this.toolStripButton_SaveAs.Size = new System.Drawing.Size(96, 36);
            this.toolStripButton_SaveAs.Text = "Save As";
            this.toolStripButton_SaveAs.Click += new System.EventHandler(this.toolStrip_SaveAs_Click);
            // 
            // toolStripButton_Package
            // 
            this.toolStripButton_Package.Image = global::MazeMaker.Properties.Resources.PackageItemsIcon;
            this.toolStripButton_Package.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Package.Name = "toolStripButton_Package";
            this.toolStripButton_Package.Size = new System.Drawing.Size(99, 36);
            this.toolStripButton_Package.Text = "Package";
            this.toolStripButton_Package.Click += new System.EventHandler(this.toolStripButton_Package_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStrip_Status});
            this.statusStrip.Location = new System.Drawing.Point(0, 547);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip.Size = new System.Drawing.Size(902, 26);
            this.statusStrip.TabIndex = 11;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStrip_Status
            // 
            this.toolStrip_Status.Name = "toolStrip_Status";
            this.toolStrip_Status.Size = new System.Drawing.Size(276, 20);
            this.toolStrip_Status.Text = "Open a MazeList file or start a new one...";
            // 
            // panelListItems
            // 
            this.panelListItems.AutoScroll = true;
            this.panelListItems.AutoScrollMinSize = new System.Drawing.Size(0, 454);
            this.panelListItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelListItems.Controls.Add(this.toolStripListItems);
            this.panelListItems.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelListItems.Location = new System.Drawing.Point(0, 67);
            this.panelListItems.MinimumSize = new System.Drawing.Size(121, 454);
            this.panelListItems.Name = "panelListItems";
            this.panelListItems.Size = new System.Drawing.Size(134, 480);
            this.panelListItems.TabIndex = 15;
            // 
            // toolStripListItems
            // 
            this.toolStripListItems.CanOverflow = false;
            this.toolStripListItems.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripListItems.ImageScalingSize = new System.Drawing.Size(50, 50);
            this.toolStripListItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel,
            this.toolStripButtonMazeItem,
            this.toolStripButtonTextItem,
            this.toolStripButtonImageItem,
            this.toolStripButtonMultipleChoiceItem,
            this.toolStripButtonRecordAudio,
            this.toolStripButtonCommand});
            this.toolStripListItems.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.toolStripListItems.Location = new System.Drawing.Point(0, 0);
            this.toolStripListItems.MinimumSize = new System.Drawing.Size(118, 554);
            this.toolStripListItems.Name = "toolStripListItems";
            this.toolStripListItems.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripListItems.Size = new System.Drawing.Size(118, 554);
            this.toolStripListItems.Stretch = true;
            this.toolStripListItems.TabIndex = 15;
            this.toolStripListItems.Text = "toolStrip1";
            // 
            // toolStripLabel
            // 
            this.toolStripLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripLabel.Name = "toolStripLabel";
            this.toolStripLabel.Size = new System.Drawing.Size(115, 20);
            this.toolStripLabel.Text = "Replace mee";
            // 
            // toolStripButtonMazeItem
            // 
            this.toolStripButtonMazeItem.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.toolStripButtonMazeItem.Image = global::MazeMaker.Properties.Resources.MazeItemIcon;
            this.toolStripButtonMazeItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMazeItem.Name = "toolStripButtonMazeItem";
            this.toolStripButtonMazeItem.Size = new System.Drawing.Size(115, 73);
            this.toolStripButtonMazeItem.Text = "Maze";
            this.toolStripButtonMazeItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonMazeItem.Click += new System.EventHandler(this.add_Click);
            // 
            // toolStripButtonTextItem
            // 
            this.toolStripButtonTextItem.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.toolStripButtonTextItem.Image = global::MazeMaker.Properties.Resources.TextItemIcon;
            this.toolStripButtonTextItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTextItem.Name = "toolStripButtonTextItem";
            this.toolStripButtonTextItem.Size = new System.Drawing.Size(115, 73);
            this.toolStripButtonTextItem.Text = "Text";
            this.toolStripButtonTextItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonTextItem.Click += new System.EventHandler(this.add_Click);
            // 
            // toolStripButtonImageItem
            // 
            this.toolStripButtonImageItem.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.toolStripButtonImageItem.Image = global::MazeMaker.Properties.Resources.ImageItemIcon;
            this.toolStripButtonImageItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImageItem.Name = "toolStripButtonImageItem";
            this.toolStripButtonImageItem.Size = new System.Drawing.Size(115, 73);
            this.toolStripButtonImageItem.Text = "Image";
            this.toolStripButtonImageItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonImageItem.Click += new System.EventHandler(this.add_Click);
            // 
            // toolStripButtonMultipleChoiceItem
            // 
            this.toolStripButtonMultipleChoiceItem.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.toolStripButtonMultipleChoiceItem.Image = global::MazeMaker.Properties.Resources.MultipleChoiceItemIcon;
            this.toolStripButtonMultipleChoiceItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMultipleChoiceItem.Name = "toolStripButtonMultipleChoiceItem";
            this.toolStripButtonMultipleChoiceItem.Size = new System.Drawing.Size(115, 73);
            this.toolStripButtonMultipleChoiceItem.Text = "Multiple Choice";
            this.toolStripButtonMultipleChoiceItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonMultipleChoiceItem.ToolTipText = "Multiple Choice";
            this.toolStripButtonMultipleChoiceItem.Click += new System.EventHandler(this.add_Click);
            // 
            // toolStripButtonRecordAudio
            // 
            this.toolStripButtonRecordAudio.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.toolStripButtonRecordAudio.Image = global::MazeMaker.Properties.Resources.RecordAudioItemIcon;
            this.toolStripButtonRecordAudio.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRecordAudio.Name = "toolStripButtonRecordAudio";
            this.toolStripButtonRecordAudio.Size = new System.Drawing.Size(115, 73);
            this.toolStripButtonRecordAudio.Text = "Record Audio";
            this.toolStripButtonRecordAudio.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonRecordAudio.Click += new System.EventHandler(this.add_Click);
            // 
            // toolStripButtonCommand
            // 
            this.toolStripButtonCommand.Font = new System.Drawing.Font("Segoe UI", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButtonCommand.Image = global::MazeMaker.Properties.Resources.CommandItemIcon;
            this.toolStripButtonCommand.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCommand.Name = "toolStripButtonCommand";
            this.toolStripButtonCommand.Size = new System.Drawing.Size(115, 73);
            this.toolStripButtonCommand.Text = "Command";
            this.toolStripButtonCommand.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonCommand.Click += new System.EventHandler(this.add_Click);
            // 
            // panelProperties
            // 
            this.panelProperties.Controls.Add(this.treeViewMazeList);
            this.panelProperties.Controls.Add(this.closeButton);
            this.panelProperties.Controls.Add(this.label2);
            this.panelProperties.Controls.Add(this.labelMazeListItems);
            this.panelProperties.Controls.Add(this.propertyGrid);
            this.panelProperties.Controls.Add(this.L_Del);
            this.panelProperties.Controls.Add(this.L_Down);
            this.panelProperties.Controls.Add(this.L_Up);
            this.panelProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProperties.Location = new System.Drawing.Point(134, 67);
            this.panelProperties.Name = "panelProperties";
            this.panelProperties.Size = new System.Drawing.Size(768, 480);
            this.panelProperties.TabIndex = 16;
            // 
            // treeViewMazeList
            // 
            this.treeViewMazeList.Location = new System.Drawing.Point(10, 30);
            this.treeViewMazeList.Margin = new System.Windows.Forms.Padding(2);
            this.treeViewMazeList.Name = "treeViewMazeList";
            this.treeViewMazeList.Size = new System.Drawing.Size(328, 436);
            this.treeViewMazeList.TabIndex = 20;
            this.treeViewMazeList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewMazeList_AfterSelect);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(654, 437);
            this.closeButton.Margin = new System.Windows.Forms.Padding(4);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(100, 33);
            this.closeButton.TabIndex = 19;
            this.closeButton.Text = "Close";
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(390, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 18;
            this.label2.Text = "Properties";
            // 
            // labelMazeListItems
            // 
            this.labelMazeListItems.AutoSize = true;
            this.labelMazeListItems.Location = new System.Drawing.Point(7, 11);
            this.labelMazeListItems.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMazeListItems.Name = "labelMazeListItems";
            this.labelMazeListItems.Size = new System.Drawing.Size(101, 17);
            this.labelMazeListItems.TabIndex = 17;
            this.labelMazeListItems.Text = "MazeList Items";
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(394, 30);
            this.propertyGrid.Margin = new System.Windows.Forms.Padding(4);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(361, 395);
            this.propertyGrid.TabIndex = 16;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // L_Del
            // 
            this.L_Del.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.L_Del.Enabled = false;
            this.L_Del.Image = global::MazeMaker.Properties.Resources.del;
            this.L_Del.Location = new System.Drawing.Point(354, 133);
            this.L_Del.Margin = new System.Windows.Forms.Padding(4);
            this.L_Del.Name = "L_Del";
            this.L_Del.Size = new System.Drawing.Size(31, 28);
            this.L_Del.TabIndex = 15;
            this.L_Del.Click += new System.EventHandler(this.L_Del_Click);
            // 
            // L_Down
            // 
            this.L_Down.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.L_Down.Enabled = false;
            this.L_Down.Image = global::MazeMaker.Properties.Resources.arr_down;
            this.L_Down.Location = new System.Drawing.Point(354, 97);
            this.L_Down.Margin = new System.Windows.Forms.Padding(4);
            this.L_Down.Name = "L_Down";
            this.L_Down.Size = new System.Drawing.Size(31, 28);
            this.L_Down.TabIndex = 14;
            this.L_Down.Click += new System.EventHandler(this.L_Down_Click);
            // 
            // L_Up
            // 
            this.L_Up.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.L_Up.Enabled = false;
            this.L_Up.Image = global::MazeMaker.Properties.Resources.arr_up;
            this.L_Up.Location = new System.Drawing.Point(354, 61);
            this.L_Up.Margin = new System.Windows.Forms.Padding(4);
            this.L_Up.Name = "L_Up";
            this.L_Up.Size = new System.Drawing.Size(31, 28);
            this.L_Up.TabIndex = 13;
            this.L_Up.Click += new System.EventHandler(this.L_Up_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(902, 28);
            this.menuStrip.TabIndex = 17;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.appendToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.packageToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = global::MazeMaker.Properties.Resources._new;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton_New_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // appendToolStripMenuItem
            // 
            this.appendToolStripMenuItem.Name = "appendToolStripMenuItem";
            this.appendToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.appendToolStripMenuItem.Text = "Append";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.saveAsToolStripMenuItem.Text = "Save As";
            // 
            // packageToolStripMenuItem
            // 
            this.packageToolStripMenuItem.Name = "packageToolStripMenuItem";
            this.packageToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.packageToolStripMenuItem.Text = "Package";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(49, 24);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(126, 26);
            this.cutToolStripMenuItem.Text = "Cut";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(126, 26);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(126, 26);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // MazeListBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 573);
            this.Controls.Add(this.panelProperties);
            this.Controls.Add(this.panelListItems);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(920, 620);
            this.Name = "MazeListBuilder";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MazeList Builder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MazeListBuilder_FormClosing);
            this.Load += new System.EventHandler(this.MazeListBuilder_Load);
            this.Resize += new System.EventHandler(this.MazeListBuilder_Resize);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panelListItems.ResumeLayout(false);
            this.panelListItems.PerformLayout();
            this.toolStripListItems.ResumeLayout(false);
            this.toolStripListItems.PerformLayout();
            this.panelProperties.ResumeLayout(false);
            this.panelProperties.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripButton toolStripButton_Open;
        private System.Windows.Forms.ToolStripButton toolStripButton_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton_SaveAs;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStrip_Status;
        private System.Windows.Forms.ToolStripButton toolStripButton_Append;
        private System.Windows.Forms.ToolStripButton toolStripButton_New;
        private System.Windows.Forms.ToolStripButton toolStripButton_Package;
        private System.Windows.Forms.Panel panelListItems;
        private System.Windows.Forms.ToolStrip toolStripListItems;
        private System.Windows.Forms.ToolStripLabel toolStripLabel;
        private System.Windows.Forms.ToolStripButton toolStripButtonMazeItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonTextItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonImageItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonMultipleChoiceItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonRecordAudio;
        private System.Windows.Forms.Panel panelProperties;
        private System.Windows.Forms.TreeView treeViewMazeList;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelMazeListItems;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button L_Del;
        private System.Windows.Forms.Button L_Down;
        private System.Windows.Forms.Button L_Up;
        private System.Windows.Forms.ToolStripButton toolStripButtonCommand;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem appendToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem packageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}