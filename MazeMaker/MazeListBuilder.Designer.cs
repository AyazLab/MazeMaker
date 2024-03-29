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
            this.openButton = new System.Windows.Forms.ToolStripButton();
            this.saveButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.newButton = new System.Windows.Forms.ToolStripButton();
            this.appendButton = new System.Windows.Forms.ToolStripButton();
            this.saveAsButton = new System.Windows.Forms.ToolStripButton();
            this.packageButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
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
            this.treeView = new System.Windows.Forms.TreeView();
            this.closeButton = new System.Windows.Forms.Button();
            this.propertiesLabel = new System.Windows.Forms.Label();
            this.listItemsLabel = new System.Windows.Forms.Label();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.deleteButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
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
            // openButton
            // 
            this.openButton.Image = global::MazeMaker.Properties.Resources.open;
            this.openButton.ImageTransparentColor = System.Drawing.Color.White;
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(92, 36);
            this.openButton.Text = "Open";
            this.openButton.Click += new System.EventHandler(this.Open);
            // 
            // saveButton
            // 
            this.saveButton.Image = global::MazeMaker.Properties.Resources.save;
            this.saveButton.ImageTransparentColor = System.Drawing.Color.White;
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(85, 36);
            this.saveButton.Text = "Save";
            this.saveButton.Click += new System.EventHandler(this.Save);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 41);
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newButton,
            this.openButton,
            this.appendButton,
            this.toolStripSeparator1,
            this.saveButton,
            this.saveAsButton,
            this.packageButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 33);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1015, 41);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // newButton
            // 
            this.newButton.Image = global::MazeMaker.Properties.Resources.NewMazeListIcon;
            this.newButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(83, 36);
            this.newButton.Text = "New";
            this.newButton.Click += new System.EventHandler(this.New);
            // 
            // appendButton
            // 
            this.appendButton.Image = global::MazeMaker.Properties.Resources.add;
            this.appendButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.appendButton.Name = "appendButton";
            this.appendButton.Size = new System.Drawing.Size(112, 36);
            this.appendButton.Text = "Append";
            this.appendButton.Click += new System.EventHandler(this.Append);
            // 
            // saveAsButton
            // 
            this.saveAsButton.Image = global::MazeMaker.Properties.Resources.saveas;
            this.saveAsButton.ImageTransparentColor = System.Drawing.Color.White;
            this.saveAsButton.Name = "saveAsButton";
            this.saveAsButton.Size = new System.Drawing.Size(110, 36);
            this.saveAsButton.Text = "Save As";
            this.saveAsButton.Click += new System.EventHandler(this.SaveAs);
            // 
            // packageButton
            // 
            this.packageButton.Image = global::MazeMaker.Properties.Resources.PackageItemsIcon;
            this.packageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.packageButton.Name = "packageButton";
            this.packageButton.Size = new System.Drawing.Size(112, 36);
            this.packageButton.Text = "Package";
            this.packageButton.Click += new System.EventHandler(this.Package);
            // 
            // statusStrip
            // 
            this.statusStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 683);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 21, 0);
            this.statusStrip.Size = new System.Drawing.Size(1015, 32);
            this.statusStrip.TabIndex = 11;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(331, 25);
            this.statusLabel.Text = "Open a MazeList file or start a new one...";
            // 
            // panelListItems
            // 
            this.panelListItems.AutoScroll = true;
            this.panelListItems.AutoScrollMinSize = new System.Drawing.Size(0, 454);
            this.panelListItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelListItems.Controls.Add(this.toolStripListItems);
            this.panelListItems.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelListItems.Location = new System.Drawing.Point(0, 74);
            this.panelListItems.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelListItems.MinimumSize = new System.Drawing.Size(136, 567);
            this.panelListItems.Name = "panelListItems";
            this.panelListItems.Size = new System.Drawing.Size(150, 609);
            this.panelListItems.TabIndex = 15;
            // 
            // toolStripListItems
            // 
            this.toolStripListItems.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripListItems.ImageScalingSize = new System.Drawing.Size(50, 50);
            this.toolStripListItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel,
            this.toolStripButtonMazeItem,
            this.toolStripButtonTextItem,
            this.toolStripButtonImageItem,
            this.toolStripButtonRecordAudio,
            this.toolStripButtonMultipleChoiceItem,
            this.toolStripButtonCommand});
            this.toolStripListItems.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.toolStripListItems.Location = new System.Drawing.Point(0, 0);
            this.toolStripListItems.MinimumSize = new System.Drawing.Size(133, 500);
            this.toolStripListItems.Name = "toolStripListItems";
            this.toolStripListItems.Size = new System.Drawing.Size(148, 542);
            this.toolStripListItems.Stretch = true;
            this.toolStripListItems.TabIndex = 15;
            this.toolStripListItems.Text = "toolStrip1";
            // 
            // toolStripLabel
            // 
            this.toolStripLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripLabel.Name = "toolStripLabel";
            this.toolStripLabel.Size = new System.Drawing.Size(145, 25);
            this.toolStripLabel.Text = "Add Items";
            this.toolStripLabel.Click += new System.EventHandler(this.toolStripLabel_Click);
            // 
            // toolStripButtonMazeItem
            // 
            this.toolStripButtonMazeItem.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.toolStripButtonMazeItem.Image = global::MazeMaker.Properties.Resources.MazeItemIcon;
            this.toolStripButtonMazeItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMazeItem.Name = "toolStripButtonMazeItem";
            this.toolStripButtonMazeItem.Size = new System.Drawing.Size(145, 75);
            this.toolStripButtonMazeItem.Text = "Maze";
            this.toolStripButtonMazeItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonMazeItem.Click += new System.EventHandler(this.AddListItem);
            // 
            // toolStripButtonTextItem
            // 
            this.toolStripButtonTextItem.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.toolStripButtonTextItem.Image = global::MazeMaker.Properties.Resources.TextItemIcon;
            this.toolStripButtonTextItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTextItem.Name = "toolStripButtonTextItem";
            this.toolStripButtonTextItem.Size = new System.Drawing.Size(145, 75);
            this.toolStripButtonTextItem.Text = "Text";
            this.toolStripButtonTextItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonTextItem.Click += new System.EventHandler(this.AddListItem);
            // 
            // toolStripButtonImageItem
            // 
            this.toolStripButtonImageItem.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.toolStripButtonImageItem.Image = global::MazeMaker.Properties.Resources.ImageItemIcon;
            this.toolStripButtonImageItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImageItem.Name = "toolStripButtonImageItem";
            this.toolStripButtonImageItem.Size = new System.Drawing.Size(145, 75);
            this.toolStripButtonImageItem.Text = "Image";
            this.toolStripButtonImageItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonImageItem.Click += new System.EventHandler(this.AddListItem);
            // 
            // toolStripButtonMultipleChoiceItem
            // 
            this.toolStripButtonMultipleChoiceItem.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.toolStripButtonMultipleChoiceItem.Image = global::MazeMaker.Properties.Resources.MultipleChoiceItemIcon;
            this.toolStripButtonMultipleChoiceItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMultipleChoiceItem.Name = "toolStripButtonMultipleChoiceItem";
            this.toolStripButtonMultipleChoiceItem.Size = new System.Drawing.Size(145, 75);
            this.toolStripButtonMultipleChoiceItem.Text = "Multiple Choice";
            this.toolStripButtonMultipleChoiceItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonMultipleChoiceItem.ToolTipText = "Multiple Choice";
            this.toolStripButtonMultipleChoiceItem.Click += new System.EventHandler(this.AddListItem);
            // 
            // toolStripButtonRecordAudio
            // 
            this.toolStripButtonRecordAudio.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.toolStripButtonRecordAudio.Image = global::MazeMaker.Properties.Resources.RecordAudioItemIcon;
            this.toolStripButtonRecordAudio.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRecordAudio.Name = "toolStripButtonRecordAudio";
            this.toolStripButtonRecordAudio.Size = new System.Drawing.Size(145, 75);
            this.toolStripButtonRecordAudio.Text = "Record Audio";
            this.toolStripButtonRecordAudio.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonRecordAudio.Click += new System.EventHandler(this.AddListItem);
            // 
            // toolStripButtonCommand
            // 
            this.toolStripButtonCommand.Font = new System.Drawing.Font("Segoe UI", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButtonCommand.Image = global::MazeMaker.Properties.Resources.CommandItemIcon;
            this.toolStripButtonCommand.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCommand.Name = "toolStripButtonCommand";
            this.toolStripButtonCommand.Size = new System.Drawing.Size(145, 77);
            this.toolStripButtonCommand.Text = "Command";
            this.toolStripButtonCommand.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonCommand.Click += new System.EventHandler(this.AddListItem);
            // 
            // panelProperties
            // 
            this.panelProperties.Controls.Add(this.treeView);
            this.panelProperties.Controls.Add(this.closeButton);
            this.panelProperties.Controls.Add(this.propertiesLabel);
            this.panelProperties.Controls.Add(this.listItemsLabel);
            this.panelProperties.Controls.Add(this.propertyGrid);
            this.panelProperties.Controls.Add(this.deleteButton);
            this.panelProperties.Controls.Add(this.downButton);
            this.panelProperties.Controls.Add(this.upButton);
            this.panelProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProperties.Location = new System.Drawing.Point(150, 74);
            this.panelProperties.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelProperties.Name = "panelProperties";
            this.panelProperties.Size = new System.Drawing.Size(865, 609);
            this.panelProperties.TabIndex = 16;
            // 
            // treeView
            // 
            this.treeView.AllowDrop = true;
            this.treeView.Location = new System.Drawing.Point(11, 38);
            this.treeView.Margin = new System.Windows.Forms.Padding(2);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(368, 544);
            this.treeView.TabIndex = 20;
            this.treeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewMazeList_AfterSelect);
            this.treeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView_DragDrop);
            this.treeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView_DragEnter);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(737, 555);
            this.closeButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(112, 41);
            this.closeButton.TabIndex = 19;
            this.closeButton.Text = "Close";
            this.closeButton.Click += new System.EventHandler(this.Close);
            // 
            // propertiesLabel
            // 
            this.propertiesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.propertiesLabel.AutoSize = true;
            this.propertiesLabel.Location = new System.Drawing.Point(440, 14);
            this.propertiesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.propertiesLabel.Name = "propertiesLabel";
            this.propertiesLabel.Size = new System.Drawing.Size(81, 20);
            this.propertiesLabel.TabIndex = 18;
            this.propertiesLabel.Text = "Properties";
            // 
            // listItemsLabel
            // 
            this.listItemsLabel.AutoSize = true;
            this.listItemsLabel.Location = new System.Drawing.Point(8, 14);
            this.listItemsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.listItemsLabel.Name = "listItemsLabel";
            this.listItemsLabel.Size = new System.Drawing.Size(117, 20);
            this.listItemsLabel.TabIndex = 17;
            this.listItemsLabel.Text = "MazeList Items";
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(444, 38);
            this.propertyGrid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(406, 494);
            this.propertyGrid.TabIndex = 16;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::MazeMaker.Properties.Resources.del;
            this.deleteButton.Location = new System.Drawing.Point(399, 166);
            this.deleteButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(35, 35);
            this.deleteButton.TabIndex = 15;
            this.deleteButton.Click += new System.EventHandler(this.Delete);
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.Enabled = false;
            this.downButton.Image = global::MazeMaker.Properties.Resources.arr_down;
            this.downButton.Location = new System.Drawing.Point(399, 121);
            this.downButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(35, 35);
            this.downButton.TabIndex = 14;
            this.downButton.Click += new System.EventHandler(this.MoveDown);
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.Enabled = false;
            this.upButton.Image = global::MazeMaker.Properties.Resources.arr_up;
            this.upButton.Location = new System.Drawing.Point(399, 76);
            this.upButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(35, 35);
            this.upButton.TabIndex = 13;
            this.upButton.Click += new System.EventHandler(this.MoveUp);
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
            this.menuStrip.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1015, 33);
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
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = global::MazeMaker.Properties.Resources.NewMazeListIcon;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(239, 34);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.New);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::MazeMaker.Properties.Resources.open;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(239, 34);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.Open);
            // 
            // appendToolStripMenuItem
            // 
            this.appendToolStripMenuItem.Image = global::MazeMaker.Properties.Resources.add;
            this.appendToolStripMenuItem.Name = "appendToolStripMenuItem";
            this.appendToolStripMenuItem.Size = new System.Drawing.Size(239, 34);
            this.appendToolStripMenuItem.Text = "Append";
            this.appendToolStripMenuItem.Click += new System.EventHandler(this.Append);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::MazeMaker.Properties.Resources.save;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(239, 34);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.Save);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = global::MazeMaker.Properties.Resources.saveas;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(239, 34);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAs);
            // 
            // packageToolStripMenuItem
            // 
            this.packageToolStripMenuItem.Image = global::MazeMaker.Properties.Resources.PackageItemsIcon;
            this.packageToolStripMenuItem.Name = "packageToolStripMenuItem";
            this.packageToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.packageToolStripMenuItem.Size = new System.Drawing.Size(239, 34);
            this.packageToolStripMenuItem.Text = "Package";
            this.packageToolStripMenuItem.Click += new System.EventHandler(this.Package);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(239, 34);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.Close);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(58, 29);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(218, 34);
            this.cutToolStripMenuItem.Text = "Cut";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.Cut);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(218, 34);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.Copy);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(218, 34);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.Paste);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(65, 29);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(164, 34);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // MazeListBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 715);
            this.Controls.Add(this.panelProperties);
            this.Controls.Add(this.panelListItems);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1024, 710);
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

        private System.Windows.Forms.ToolStripButton openButton;
        private System.Windows.Forms.ToolStripButton saveButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton saveAsButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripButton appendButton;
        private System.Windows.Forms.ToolStripButton newButton;
        private System.Windows.Forms.ToolStripButton packageButton;
        private System.Windows.Forms.Panel panelListItems;
        private System.Windows.Forms.ToolStrip toolStripListItems;
        private System.Windows.Forms.ToolStripLabel toolStripLabel;
        private System.Windows.Forms.ToolStripButton toolStripButtonMazeItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonTextItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonImageItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonMultipleChoiceItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonRecordAudio;
        private System.Windows.Forms.Panel panelProperties;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label propertiesLabel;
        private System.Windows.Forms.Label listItemsLabel;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButton;
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