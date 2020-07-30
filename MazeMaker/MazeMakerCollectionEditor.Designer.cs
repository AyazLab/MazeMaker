namespace MazeMaker
{
    partial class MazeMakerCollectionEditor
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
            this.components = new System.ComponentModel.Container();
            this.listBoxCollection = new System.Windows.Forms.ListBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.checkBoxCopy = new System.Windows.Forms.CheckBox();
            this.buttonUserLibrary = new System.Windows.Forms.Button();
            this.buttonStandardLibrary = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttonAddFromUserLib = new System.Windows.Forms.Button();
            this.buttonAddFromStandard = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxCollection
            // 
            this.listBoxCollection.FormattingEnabled = true;
            this.listBoxCollection.ItemHeight = 20;
            this.listBoxCollection.Location = new System.Drawing.Point(4, 19);
            this.listBoxCollection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listBoxCollection.Name = "listBoxCollection";
            this.listBoxCollection.Size = new System.Drawing.Size(291, 344);
            this.listBoxCollection.TabIndex = 0;
            this.listBoxCollection.SelectedIndexChanged += new System.EventHandler(this.listBoxCollection_SelectedIndexChanged);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(306, 218);
            this.propertyGrid1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(343, 286);
            this.propertyGrid1.TabIndex = 1;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(537, 512);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(112, 35);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Cancel";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAdd.Location = new System.Drawing.Point(4, 369);
            this.buttonAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(138, 35);
            this.buttonAdd.TabIndex = 3;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRemove.Location = new System.Drawing.Point(151, 369);
            this.buttonRemove.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(146, 35);
            this.buttonRemove.TabIndex = 4;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(415, 512);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(112, 35);
            this.buttonOk.TabIndex = 5;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(306, 19);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(343, 190);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // checkBoxCopy
            // 
            this.checkBoxCopy.AutoSize = true;
            this.checkBoxCopy.Checked = true;
            this.checkBoxCopy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCopy.Location = new System.Drawing.Point(4, 478);
            this.checkBoxCopy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBoxCopy.Name = "checkBoxCopy";
            this.checkBoxCopy.Size = new System.Drawing.Size(298, 24);
            this.checkBoxCopy.TabIndex = 7;
            this.checkBoxCopy.Text = "Copy the addition to the \'User Library\'";
            this.toolTip1.SetToolTip(this.checkBoxCopy, "Copies texture and model files to the user library");
            this.checkBoxCopy.UseVisualStyleBackColor = true;
            // 
            // buttonUserLibrary
            // 
            this.buttonUserLibrary.Location = new System.Drawing.Point(107, 507);
            this.buttonUserLibrary.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonUserLibrary.Name = "buttonUserLibrary";
            this.buttonUserLibrary.Size = new System.Drawing.Size(86, 30);
            this.buttonUserLibrary.TabIndex = 8;
            this.buttonUserLibrary.Text = "User";
            this.toolTip1.SetToolTip(this.buttonUserLibrary, "Open User Library Folder");
            this.buttonUserLibrary.UseVisualStyleBackColor = true;
            this.buttonUserLibrary.Click += new System.EventHandler(this.buttonUserLibrary_Click);
            // 
            // buttonStandardLibrary
            // 
            this.buttonStandardLibrary.Location = new System.Drawing.Point(199, 505);
            this.buttonStandardLibrary.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonStandardLibrary.Name = "buttonStandardLibrary";
            this.buttonStandardLibrary.Size = new System.Drawing.Size(100, 35);
            this.buttonStandardLibrary.TabIndex = 9;
            this.buttonStandardLibrary.Text = "Standard";
            this.toolTip1.SetToolTip(this.buttonStandardLibrary, "Open Standard Library Folder");
            this.buttonStandardLibrary.UseVisualStyleBackColor = true;
            this.buttonStandardLibrary.Click += new System.EventHandler(this.buttonStandardLibrary_Click);
            // 
            // buttonAddFromUserLib
            // 
            this.buttonAddFromUserLib.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddFromUserLib.Location = new System.Drawing.Point(4, 414);
            this.buttonAddFromUserLib.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonAddFromUserLib.Name = "buttonAddFromUserLib";
            this.buttonAddFromUserLib.Size = new System.Drawing.Size(138, 55);
            this.buttonAddFromUserLib.TabIndex = 10;
            this.buttonAddFromUserLib.Text = "Add From User Library";
            this.buttonAddFromUserLib.UseVisualStyleBackColor = true;
            this.buttonAddFromUserLib.Click += new System.EventHandler(this.buttonAddFromUserLib_Click);
            // 
            // buttonAddFromStandard
            // 
            this.buttonAddFromStandard.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddFromStandard.Location = new System.Drawing.Point(150, 414);
            this.buttonAddFromStandard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonAddFromStandard.Name = "buttonAddFromStandard";
            this.buttonAddFromStandard.Size = new System.Drawing.Size(147, 55);
            this.buttonAddFromStandard.TabIndex = 11;
            this.buttonAddFromStandard.Text = "Add From Standard Library";
            this.buttonAddFromStandard.UseVisualStyleBackColor = true;
            this.buttonAddFromStandard.Click += new System.EventHandler(this.buttonAddFromStandard_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 512);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "Open LIbrary:";
            // 
            // MazeMakerCollectionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 539);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAddFromStandard);
            this.Controls.Add(this.buttonAddFromUserLib);
            this.Controls.Add(this.buttonStandardLibrary);
            this.Controls.Add(this.buttonUserLibrary);
            this.Controls.Add(this.checkBoxCopy);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.listBoxCollection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(674, 595);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(674, 595);
            this.Name = "MazeMakerCollectionEditor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MazeMakerCollectionEditor";
            this.Load += new System.EventHandler(this.MazeMakerCollectionEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxCollection;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox checkBoxCopy;
        private System.Windows.Forms.Button buttonUserLibrary;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button buttonStandardLibrary;
        private System.Windows.Forms.Button buttonAddFromUserLib;
        private System.Windows.Forms.Button buttonAddFromStandard;
        private System.Windows.Forms.Label label1;
    }
}