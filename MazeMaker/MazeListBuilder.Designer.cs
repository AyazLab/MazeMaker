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
            this.toolStrip_SaveAs = new System.Windows.Forms.ToolStripButton();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.add = new System.Windows.Forms.Button();
            this.L_Up = new System.Windows.Forms.Button();
            this.L_Down = new System.Windows.Forms.Button();
            this.L_Del = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
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
            this.toolStrip_open.Size = new System.Drawing.Size(128, 45);
            this.toolStrip_open.Text = "Open";
            this.toolStrip_open.Click += new System.EventHandler(this.toolStrip_open_Click);
            // 
            // toolStrip_save
            // 
            this.toolStrip_save.Image = global::MazeMaker.Properties.Resources.save;
            this.toolStrip_save.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStrip_save.Name = "toolStrip_save";
            this.toolStrip_save.Size = new System.Drawing.Size(115, 45);
            this.toolStrip_save.Text = "Save";
            this.toolStrip_save.Click += new System.EventHandler(this.toolStrip_save_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 52);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStrip_open,
            this.toolStrip_save,
            this.toolStripSeparator1,
            this.toolStrip_SaveAs});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1394, 52);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStrip_SaveAs
            // 
            this.toolStrip_SaveAs.Image = global::MazeMaker.Properties.Resources.saveas;
            this.toolStrip_SaveAs.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStrip_SaveAs.Name = "toolStrip_SaveAs";
            this.toolStrip_SaveAs.Size = new System.Drawing.Size(155, 45);
            this.toolStrip_SaveAs.Text = "Save As";
            this.toolStrip_SaveAs.Click += new System.EventHandler(this.toolStrip_SaveAs_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(18, 103);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(382, 39);
            this.comboBox1.TabIndex = 1;
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(432, 99);
            this.add.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(200, 54);
            this.add.TabIndex = 2;
            this.add.Text = "Add";
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // L_Up
            // 
            this.L_Up.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.L_Up.Enabled = false;
            this.L_Up.Image = global::MazeMaker.Properties.Resources.arr_up;
            this.L_Up.Location = new System.Drawing.Point(650, 271);
            this.L_Up.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.L_Up.Name = "L_Up";
            this.L_Up.Size = new System.Drawing.Size(62, 54);
            this.L_Up.TabIndex = 4;
            this.L_Up.Click += new System.EventHandler(this.L_Up_Click);
            // 
            // L_Down
            // 
            this.L_Down.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.L_Down.Enabled = false;
            this.L_Down.Image = global::MazeMaker.Properties.Resources.arr_down;
            this.L_Down.Location = new System.Drawing.Point(650, 341);
            this.L_Down.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.L_Down.Name = "L_Down";
            this.L_Down.Size = new System.Drawing.Size(62, 54);
            this.L_Down.TabIndex = 5;
            this.L_Down.Click += new System.EventHandler(this.L_Down_Click);
            // 
            // L_Del
            // 
            this.L_Del.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.L_Del.Enabled = false;
            this.L_Del.Image = global::MazeMaker.Properties.Resources.del;
            this.L_Del.Location = new System.Drawing.Point(650, 411);
            this.L_Del.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.L_Del.Name = "L_Del";
            this.L_Del.Size = new System.Drawing.Size(62, 54);
            this.L_Del.TabIndex = 6;
            this.L_Del.Click += new System.EventHandler(this.L_Del_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(728, 200);
            this.propertyGrid1.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(640, 709);
            this.propertyGrid1.TabIndex = 7;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 159);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 32);
            this.label1.TabIndex = 8;
            this.label1.Text = "Members";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(722, 159);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 32);
            this.label2.TabIndex = 9;
            this.label2.Text = "Properties";
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(1168, 918);
            this.closeButton.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(200, 54);
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 983);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 38, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1394, 54);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip_Status
            // 
            this.toolStrip_Status.Name = "toolStrip_Status";
            this.toolStrip_Status.Size = new System.Drawing.Size(554, 41);
            this.toolStrip_Status.Text = "Open a MazeList file or start a new one...";
            // 
            // treeViewMazeList
            // 
            this.treeViewMazeList.Location = new System.Drawing.Point(18, 200);
            this.treeViewMazeList.Name = "treeViewMazeList";
            this.treeViewMazeList.Size = new System.Drawing.Size(614, 709);
            this.treeViewMazeList.TabIndex = 12;
            this.treeViewMazeList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewMazeList_AfterSelect);
            // 
            // MazeListBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1394, 1037);
            this.Controls.Add(this.treeViewMazeList);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.L_Del);
            this.Controls.Add(this.L_Down);
            this.Controls.Add(this.L_Up);
            this.Controls.Add(this.add);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.MinimizeBox = false;
            this.Name = "MazeListBuilder";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MazeListBuilder";
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
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Button L_Up;
        private System.Windows.Forms.Button L_Down;
        private System.Windows.Forms.Button L_Del;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.ToolStripButton toolStrip_SaveAs;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStrip_Status;
        private System.Windows.Forms.TreeView treeViewMazeList;
    }
}