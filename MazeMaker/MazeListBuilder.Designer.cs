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
            this.toolStrip_open = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_New = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Append = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_SaveAs = new System.Windows.Forms.ToolStripButton();
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.add = new System.Windows.Forms.Button();
            this.L_Up = new System.Windows.Forms.Button();
            this.L_Down = new System.Windows.Forms.Button();
            this.L_Del = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.treeViewMazeList = new System.Windows.Forms.TreeView();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip_open
            // 
            this.toolStrip_open.Image = global::MazeMaker.Properties.Resources.open;
            this.toolStrip_open.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStrip_open.Name = "toolStrip_open";
            this.toolStrip_open.Size = new System.Drawing.Size(81, 36);
            this.toolStrip_open.Text = "Open";
            this.toolStrip_open.Click += new System.EventHandler(this.toolStrip_open_Click);
            // 
            // toolStrip_save
            // 
            this.toolStrip_save.Image = global::MazeMaker.Properties.Resources.save;
            this.toolStrip_save.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStrip_save.Name = "toolStrip_save";
            this.toolStrip_save.Size = new System.Drawing.Size(76, 36);
            this.toolStrip_save.Text = "Save";
            this.toolStrip_save.Click += new System.EventHandler(this.toolStrip_save_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_New,
            this.toolStrip_open,
            this.toolStripButton_Append,
            this.toolStripSeparator1,
            this.toolStrip_save,
            this.toolStrip_SaveAs});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.Size = new System.Drawing.Size(697, 39);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
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
            // toolStrip_SaveAs
            // 
            this.toolStrip_SaveAs.Image = global::MazeMaker.Properties.Resources.saveas;
            this.toolStrip_SaveAs.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStrip_SaveAs.Name = "toolStrip_SaveAs";
            this.toolStrip_SaveAs.Size = new System.Drawing.Size(96, 36);
            this.toolStrip_SaveAs.Text = "Save As";
            this.toolStrip_SaveAs.Click += new System.EventHandler(this.toolStrip_SaveAs_Click);
            // 
            // comboBox1
            // 
            this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new System.Drawing.Point(9, 53);
            this.comboBox.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox.Name = "comboBox1";
            this.comboBox.Size = new System.Drawing.Size(193, 24);
            this.comboBox.TabIndex = 1;
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(216, 51);
            this.add.Margin = new System.Windows.Forms.Padding(4);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(100, 28);
            this.add.TabIndex = 2;
            this.add.Text = "Add";
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // L_Up
            // 
            this.L_Up.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.L_Up.Enabled = false;
            this.L_Up.Image = global::MazeMaker.Properties.Resources.arr_up;
            this.L_Up.Location = new System.Drawing.Point(325, 140);
            this.L_Up.Margin = new System.Windows.Forms.Padding(4);
            this.L_Up.Name = "L_Up";
            this.L_Up.Size = new System.Drawing.Size(31, 28);
            this.L_Up.TabIndex = 4;
            this.L_Up.Click += new System.EventHandler(this.L_Up_Click);
            // 
            // L_Down
            // 
            this.L_Down.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.L_Down.Enabled = false;
            this.L_Down.Image = global::MazeMaker.Properties.Resources.arr_down;
            this.L_Down.Location = new System.Drawing.Point(325, 176);
            this.L_Down.Margin = new System.Windows.Forms.Padding(4);
            this.L_Down.Name = "L_Down";
            this.L_Down.Size = new System.Drawing.Size(31, 28);
            this.L_Down.TabIndex = 5;
            this.L_Down.Click += new System.EventHandler(this.L_Down_Click);
            // 
            // L_Del
            // 
            this.L_Del.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.L_Del.Enabled = false;
            this.L_Del.Image = global::MazeMaker.Properties.Resources.del;
            this.L_Del.Location = new System.Drawing.Point(325, 212);
            this.L_Del.Margin = new System.Windows.Forms.Padding(4);
            this.L_Del.Name = "L_Del";
            this.L_Del.Size = new System.Drawing.Size(31, 28);
            this.L_Del.TabIndex = 6;
            this.L_Del.Click += new System.EventHandler(this.L_Del_Click);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(364, 103);
            this.propertyGrid.Margin = new System.Windows.Forms.Padding(4);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(320, 366);
            this.propertyGrid.TabIndex = 7;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 82);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Members";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(361, 82);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "Properties";
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(584, 474);
            this.closeButton.Margin = new System.Windows.Forms.Padding(4);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(100, 28);
            this.closeButton.TabIndex = 10;
            this.closeButton.Text = "Close";
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStrip_Status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 509);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(697, 26);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip_Status
            // 
            this.toolStrip_Status.Name = "toolStrip_Status";
            this.toolStrip_Status.Size = new System.Drawing.Size(276, 20);
            this.toolStrip_Status.Text = "Open a MazeList file or start a new one...";
            // 
            // treeViewMazeList
            // 
            this.treeViewMazeList.Location = new System.Drawing.Point(9, 103);
            this.treeViewMazeList.Margin = new System.Windows.Forms.Padding(2);
            this.treeViewMazeList.Name = "treeViewMazeList";
            this.treeViewMazeList.Size = new System.Drawing.Size(309, 291);
            this.treeViewMazeList.TabIndex = 12;
            this.treeViewMazeList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewMazeList_AfterSelect);
            // 
            // MazeListBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 535);
            this.Controls.Add(this.treeViewMazeList);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.L_Del);
            this.Controls.Add(this.L_Down);
            this.Controls.Add(this.L_Up);
            this.Controls.Add(this.add);
            this.Controls.Add(this.comboBox);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeBox = false;
            this.Name = "MazeListBuilder";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MazeListBuilder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MazeListBuilder_FormClosing);
            this.Load += new System.EventHandler(this.MazeListBuilder_Load);
            this.Resize += new System.EventHandler(this.MazeListBuilder_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripButton toolStrip_open;
        private System.Windows.Forms.ToolStripButton toolStrip_save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ComboBox comboBox;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Button L_Up;
        private System.Windows.Forms.Button L_Down;
        private System.Windows.Forms.Button L_Del;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.ToolStripButton toolStrip_SaveAs;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStrip_Status;
        private System.Windows.Forms.TreeView treeViewMazeList;
        private System.Windows.Forms.ToolStripButton toolStripButton_Append;
        private System.Windows.Forms.ToolStripButton toolStripButton_New;
    }
}