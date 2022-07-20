namespace MazeMaker
{
    partial class MazeGen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MazeGen));
            this.cancelButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.previousButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_Rect1 = new System.Windows.Forms.TabPage();
            this.label32 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.thickNumeric = new System.Windows.Forms.NumericUpDown();
            this.lengthNumeric = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.widthNumeric = new System.Windows.Forms.NumericUpDown();
            this.heightNumeric = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage_Rect2 = new System.Windows.Forms.TabPage();
            this.colorBoxLabelReg = new System.Windows.Forms.Label();
            this.comboBoxCeiling = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.comboBoxColor = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tabPage_Rect3 = new System.Windows.Forms.TabPage();
            this.regenerateButton = new System.Windows.Forms.Button();
            this.outMazeText = new System.Windows.Forms.RichTextBox();
            this.tabPage_Circ1 = new System.Windows.Forms.TabPage();
            this.colorBoxLabelCirc = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.comboBoxColorCirc = new System.Windows.Forms.ComboBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.trackBar_corridorPreference = new System.Windows.Forms.TrackBar();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.ringThicknessNumeric = new System.Windows.Forms.NumericUpDown();
            this.ringRadiusNumeric = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.ringFactorUpDown = new System.Windows.Forms.NumericUpDown();
            this.ringNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.tabPage_Init = new System.Windows.Forms.TabPage();
            this.label31 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.button_regMazeGen = new System.Windows.Forms.Button();
            this.button_circMazeGen = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage_Rect1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thickNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lengthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumeric)).BeginInit();
            this.tabPage_Rect2.SuspendLayout();
            this.tabPage_Rect3.SuspendLayout();
            this.tabPage_Circ1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_corridorPreference)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ringThicknessNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ringRadiusNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ringFactorUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ringNumUpDown)).BeginInit();
            this.tabPage_Init.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(12, 369);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Location = new System.Drawing.Point(396, 369);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(75, 23);
            this.nextButton.TabIndex = 2;
            this.nextButton.Text = "Next";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // previousButton
            // 
            this.previousButton.Location = new System.Drawing.Point(315, 369);
            this.previousButton.Name = "previousButton";
            this.previousButton.Size = new System.Drawing.Size(75, 23);
            this.previousButton.TabIndex = 3;
            this.previousButton.Text = "Previous";
            this.previousButton.UseVisualStyleBackColor = true;
            this.previousButton.Visible = false;
            this.previousButton.Click += new System.EventHandler(this.previousButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Right;
            this.tabControl1.Controls.Add(this.tabPage_Rect1);
            this.tabControl1.Controls.Add(this.tabPage_Rect2);
            this.tabControl1.Controls.Add(this.tabPage_Rect3);
            this.tabControl1.Controls.Add(this.tabPage_Circ1);
            this.tabControl1.Controls.Add(this.tabPage_Init);
            this.tabControl1.Location = new System.Drawing.Point(-6, -4);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(512, 367);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage_Rect1
            // 
            this.tabPage_Rect1.BackgroundImage = global::MazeMaker.Properties.Resources.rectMazeBG;
            this.tabPage_Rect1.Controls.Add(this.label32);
            this.tabPage_Rect1.Controls.Add(this.label11);
            this.tabPage_Rect1.Controls.Add(this.label10);
            this.tabPage_Rect1.Controls.Add(this.label9);
            this.tabPage_Rect1.Controls.Add(this.label8);
            this.tabPage_Rect1.Controls.Add(this.label7);
            this.tabPage_Rect1.Controls.Add(this.thickNumeric);
            this.tabPage_Rect1.Controls.Add(this.lengthNumeric);
            this.tabPage_Rect1.Controls.Add(this.label5);
            this.tabPage_Rect1.Controls.Add(this.label6);
            this.tabPage_Rect1.Controls.Add(this.widthNumeric);
            this.tabPage_Rect1.Controls.Add(this.heightNumeric);
            this.tabPage_Rect1.Controls.Add(this.label4);
            this.tabPage_Rect1.Controls.Add(this.label3);
            this.tabPage_Rect1.Controls.Add(this.label2);
            this.tabPage_Rect1.Location = new System.Drawing.Point(4, 4);
            this.tabPage_Rect1.Name = "tabPage_Rect1";
            this.tabPage_Rect1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage_Rect1.Size = new System.Drawing.Size(485, 359);
            this.tabPage_Rect1.TabIndex = 0;
            this.tabPage_Rect1.Text = "RectMaze1";
            this.tabPage_Rect1.UseVisualStyleBackColor = true;
            this.tabPage_Rect1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.Location = new System.Drawing.Point(191, 40);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(196, 29);
            this.label32.TabIndex = 15;
            this.label32.Text = "New Maze Wizard";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(359, 308);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(15, 13);
            this.label11.TabIndex = 14;
            this.label11.Text = "%";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(359, 183);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(28, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "cells";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(359, 282);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "maze units";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(359, 156);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "cells";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(187, 237);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(286, 15);
            this.label7.TabIndex = 10;
            this.label7.Text = "Step 2: Select the wall length and thickness";
            // 
            // thickNumeric
            // 
            this.thickNumeric.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.thickNumeric.Location = new System.Drawing.Point(282, 306);
            this.thickNumeric.Name = "thickNumeric";
            this.thickNumeric.Size = new System.Drawing.Size(71, 20);
            this.thickNumeric.TabIndex = 9;
            this.thickNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // lengthNumeric
            // 
            this.lengthNumeric.Location = new System.Drawing.Point(282, 280);
            this.lengthNumeric.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.lengthNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.lengthNumeric.Name = "lengthNumeric";
            this.lengthNumeric.Size = new System.Drawing.Size(71, 20);
            this.lengthNumeric.TabIndex = 8;
            this.lengthNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(203, 308);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Thickness:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(203, 282);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Cell size";
            // 
            // widthNumeric
            // 
            this.widthNumeric.Location = new System.Drawing.Point(282, 181);
            this.widthNumeric.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.widthNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.widthNumeric.Name = "widthNumeric";
            this.widthNumeric.Size = new System.Drawing.Size(71, 20);
            this.widthNumeric.TabIndex = 5;
            this.widthNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // heightNumeric
            // 
            this.heightNumeric.Location = new System.Drawing.Point(282, 154);
            this.heightNumeric.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.heightNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.heightNumeric.Name = "heightNumeric";
            this.heightNumeric.Size = new System.Drawing.Size(71, 20);
            this.heightNumeric.TabIndex = 4;
            this.heightNumeric.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(203, 183);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Height:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(203, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Width:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(187, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(237, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Step 1: Select the maze dimensions";
            // 
            // tabPage_Rect2
            // 
            this.tabPage_Rect2.BackgroundImage = global::MazeMaker.Properties.Resources.rectMazeBG;
            this.tabPage_Rect2.Controls.Add(this.colorBoxLabelReg);
            this.tabPage_Rect2.Controls.Add(this.comboBoxCeiling);
            this.tabPage_Rect2.Controls.Add(this.label14);
            this.tabPage_Rect2.Controls.Add(this.label13);
            this.tabPage_Rect2.Controls.Add(this.comboBoxColor);
            this.tabPage_Rect2.Controls.Add(this.label12);
            this.tabPage_Rect2.Location = new System.Drawing.Point(4, 4);
            this.tabPage_Rect2.Name = "tabPage_Rect2";
            this.tabPage_Rect2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage_Rect2.Size = new System.Drawing.Size(485, 359);
            this.tabPage_Rect2.TabIndex = 1;
            this.tabPage_Rect2.Text = "RectMaze2";
            this.tabPage_Rect2.UseVisualStyleBackColor = true;
            // 
            // colorBoxLabelReg
            // 
            this.colorBoxLabelReg.AutoSize = true;
            this.colorBoxLabelReg.Location = new System.Drawing.Point(189, 117);
            this.colorBoxLabelReg.Name = "colorBoxLabelReg";
            this.colorBoxLabelReg.Size = new System.Drawing.Size(34, 13);
            this.colorBoxLabelReg.TabIndex = 33;
            this.colorBoxLabelReg.Text = "         ";
            // 
            // comboBoxCeiling
            // 
            this.comboBoxCeiling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCeiling.FormattingEnabled = true;
            this.comboBoxCeiling.Location = new System.Drawing.Point(238, 200);
            this.comboBoxCeiling.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxCeiling.Name = "comboBoxCeiling";
            this.comboBoxCeiling.Size = new System.Drawing.Size(174, 21);
            this.comboBoxCeiling.TabIndex = 4;
            this.comboBoxCeiling.Visible = false;
            this.comboBoxCeiling.SelectedIndexChanged += new System.EventHandler(this.comboBoxCeiling_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(208, 174);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(198, 15);
            this.label14.TabIndex = 3;
            this.label14.Text = "Step 4: Select Ceiling Options";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(200, 275);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(280, 44);
            this.label13.TabIndex = 2;
            this.label13.Text = "NOTE: Wall colors can be changed at any time by selecting one or more wall at onc" +
    "e (from left pane)";
            // 
            // comboBoxColor
            // 
            this.comboBoxColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxColor.FormattingEnabled = true;
            this.comboBoxColor.Location = new System.Drawing.Point(238, 115);
            this.comboBoxColor.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxColor.Name = "comboBoxColor";
            this.comboBoxColor.Size = new System.Drawing.Size(174, 21);
            this.comboBoxColor.TabIndex = 1;
            this.comboBoxColor.SelectedIndexChanged += new System.EventHandler(this.comboBoxColor_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(208, 63);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(186, 15);
            this.label12.TabIndex = 0;
            this.label12.Text = "Step 3: Select the wall color";
            // 
            // tabPage_Rect3
            // 
            this.tabPage_Rect3.BackColor = System.Drawing.Color.Black;
            this.tabPage_Rect3.BackgroundImage = global::MazeMaker.Properties.Resources.rectMazeBG;
            this.tabPage_Rect3.Controls.Add(this.regenerateButton);
            this.tabPage_Rect3.Controls.Add(this.outMazeText);
            this.tabPage_Rect3.Location = new System.Drawing.Point(4, 4);
            this.tabPage_Rect3.Name = "tabPage_Rect3";
            this.tabPage_Rect3.Size = new System.Drawing.Size(485, 359);
            this.tabPage_Rect3.TabIndex = 2;
            this.tabPage_Rect3.Text = "RectMazePreview";
            // 
            // regenerateButton
            // 
            this.regenerateButton.Location = new System.Drawing.Point(355, 333);
            this.regenerateButton.Name = "regenerateButton";
            this.regenerateButton.Size = new System.Drawing.Size(118, 23);
            this.regenerateButton.TabIndex = 0;
            this.regenerateButton.Text = "Regenerate Maze";
            this.regenerateButton.UseVisualStyleBackColor = true;
            this.regenerateButton.Click += new System.EventHandler(this.regenerateButton_Click);
            // 
            // outMazeText
            // 
            this.outMazeText.BackColor = System.Drawing.Color.Black;
            this.outMazeText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.outMazeText.Font = new System.Drawing.Font("Cascadia Code SemiLight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outMazeText.ForeColor = System.Drawing.Color.White;
            this.outMazeText.Location = new System.Drawing.Point(126, 12);
            this.outMazeText.Name = "outMazeText";
            this.outMazeText.ReadOnly = true;
            this.outMazeText.Size = new System.Drawing.Size(347, 319);
            this.outMazeText.TabIndex = 1;
            this.outMazeText.Text = "";
            this.outMazeText.WordWrap = false;
            this.outMazeText.TextChanged += new System.EventHandler(this.outMazeText_TextChanged);
            // 
            // tabPage_Circ1
            // 
            this.tabPage_Circ1.BackgroundImage = global::MazeMaker.Properties.Resources.circMazeBG;
            this.tabPage_Circ1.Controls.Add(this.colorBoxLabelCirc);
            this.tabPage_Circ1.Controls.Add(this.label28);
            this.tabPage_Circ1.Controls.Add(this.comboBoxColorCirc);
            this.tabPage_Circ1.Controls.Add(this.label27);
            this.tabPage_Circ1.Controls.Add(this.label26);
            this.tabPage_Circ1.Controls.Add(this.trackBar_corridorPreference);
            this.tabPage_Circ1.Controls.Add(this.label25);
            this.tabPage_Circ1.Controls.Add(this.label24);
            this.tabPage_Circ1.Controls.Add(this.label21);
            this.tabPage_Circ1.Controls.Add(this.label22);
            this.tabPage_Circ1.Controls.Add(this.label23);
            this.tabPage_Circ1.Controls.Add(this.label15);
            this.tabPage_Circ1.Controls.Add(this.ringThicknessNumeric);
            this.tabPage_Circ1.Controls.Add(this.ringRadiusNumeric);
            this.tabPage_Circ1.Controls.Add(this.label16);
            this.tabPage_Circ1.Controls.Add(this.label17);
            this.tabPage_Circ1.Controls.Add(this.ringFactorUpDown);
            this.tabPage_Circ1.Controls.Add(this.ringNumUpDown);
            this.tabPage_Circ1.Controls.Add(this.label18);
            this.tabPage_Circ1.Controls.Add(this.label19);
            this.tabPage_Circ1.Controls.Add(this.label20);
            this.tabPage_Circ1.Location = new System.Drawing.Point(4, 4);
            this.tabPage_Circ1.Name = "tabPage_Circ1";
            this.tabPage_Circ1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage_Circ1.Size = new System.Drawing.Size(485, 359);
            this.tabPage_Circ1.TabIndex = 3;
            this.tabPage_Circ1.Text = "CirMaze";
            this.tabPage_Circ1.UseVisualStyleBackColor = true;
            // 
            // colorBoxLabelCirc
            // 
            this.colorBoxLabelCirc.AutoSize = true;
            this.colorBoxLabelCirc.Location = new System.Drawing.Point(257, 329);
            this.colorBoxLabelCirc.Name = "colorBoxLabelCirc";
            this.colorBoxLabelCirc.Size = new System.Drawing.Size(34, 13);
            this.colorBoxLabelCirc.TabIndex = 32;
            this.colorBoxLabelCirc.Text = "         ";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(193, 330);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(58, 13);
            this.label28.TabIndex = 31;
            this.label28.Text = "Wall Color:";
            // 
            // comboBoxColorCirc
            // 
            this.comboBoxColorCirc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxColorCirc.FormattingEnabled = true;
            this.comboBoxColorCirc.Location = new System.Drawing.Point(294, 326);
            this.comboBoxColorCirc.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxColorCirc.Name = "comboBoxColorCirc";
            this.comboBoxColorCirc.Size = new System.Drawing.Size(174, 21);
            this.comboBoxColorCirc.TabIndex = 30;
            this.comboBoxColorCirc.SelectedIndexChanged += new System.EventHandler(this.comboBoxColorCirc_SelectedIndexChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(377, 306);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(29, 13);
            this.label27.TabIndex = 29;
            this.label27.Text = "High";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(303, 306);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(114, 13);
            this.label26.TabIndex = 28;
            this.label26.Text = "Low                             ";
            // 
            // trackBar_corridorPreference
            // 
            this.trackBar_corridorPreference.BackColor = System.Drawing.Color.White;
            this.trackBar_corridorPreference.Location = new System.Drawing.Point(300, 278);
            this.trackBar_corridorPreference.Maximum = 4;
            this.trackBar_corridorPreference.Name = "trackBar_corridorPreference";
            this.trackBar_corridorPreference.Size = new System.Drawing.Size(104, 45);
            this.trackBar_corridorPreference.TabIndex = 27;
            this.trackBar_corridorPreference.Value = 2;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(193, 282);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(101, 13);
            this.label25.TabIndex = 26;
            this.label25.Text = "Corridor Preference:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(181, 208);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(214, 15);
            this.label24.TabIndex = 24;
            this.label24.Text = "Step 3: Select Algorithm Options";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(384, 174);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(15, 13);
            this.label21.TabIndex = 23;
            this.label21.Text = "%";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(379, 64);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(29, 13);
            this.label22.TabIndex = 22;
            this.label22.Text = "rings";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(379, 96);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(57, 13);
            this.label23.TabIndex = 21;
            this.label23.Text = "maze units";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(181, 139);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(251, 15);
            this.label15.TabIndex = 20;
            this.label15.Text = "Step 2: Select the ring / wall thickness";
            // 
            // ringThicknessNumeric
            // 
            this.ringThicknessNumeric.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ringThicknessNumeric.Location = new System.Drawing.Point(302, 170);
            this.ringThicknessNumeric.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.ringThicknessNumeric.Name = "ringThicknessNumeric";
            this.ringThicknessNumeric.Size = new System.Drawing.Size(71, 20);
            this.ringThicknessNumeric.TabIndex = 19;
            this.ringThicknessNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // ringRadiusNumeric
            // 
            this.ringRadiusNumeric.Location = new System.Drawing.Point(302, 94);
            this.ringRadiusNumeric.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.ringRadiusNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ringRadiusNumeric.Name = "ringRadiusNumeric";
            this.ringRadiusNumeric.Size = new System.Drawing.Size(71, 20);
            this.ringRadiusNumeric.TabIndex = 18;
            this.ringRadiusNumeric.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(225, 174);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 13);
            this.label16.TabIndex = 17;
            this.label16.Text = "Thickness:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(193, 247);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(91, 13);
            this.label17.TabIndex = 16;
            this.label17.Text = "Branching Factor:";
            // 
            // ringFactorUpDown
            // 
            this.ringFactorUpDown.Location = new System.Drawing.Point(302, 240);
            this.ringFactorUpDown.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.ringFactorUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ringFactorUpDown.Name = "ringFactorUpDown";
            this.ringFactorUpDown.Size = new System.Drawing.Size(71, 20);
            this.ringFactorUpDown.TabIndex = 15;
            this.ringFactorUpDown.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // ringNumUpDown
            // 
            this.ringNumUpDown.Location = new System.Drawing.Point(302, 57);
            this.ringNumUpDown.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.ringNumUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ringNumUpDown.Name = "ringNumUpDown";
            this.ringNumUpDown.Size = new System.Drawing.Size(71, 20);
            this.ringNumUpDown.TabIndex = 14;
            this.ringNumUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(214, 96);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(68, 13);
            this.label18.TabIndex = 13;
            this.label18.Text = "Ring Radius:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(193, 59);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(89, 13);
            this.label19.TabIndex = 12;
            this.label19.Text = "Number of Rings:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(181, 23);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(237, 15);
            this.label20.TabIndex = 11;
            this.label20.Text = "Step 1: Select the maze dimensions";
            // 
            // tabPage_Init
            // 
            this.tabPage_Init.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPage_Init.Controls.Add(this.label31);
            this.tabPage_Init.Controls.Add(this.label1);
            this.tabPage_Init.Controls.Add(this.label30);
            this.tabPage_Init.Controls.Add(this.label29);
            this.tabPage_Init.Controls.Add(this.button_regMazeGen);
            this.tabPage_Init.Controls.Add(this.button_circMazeGen);
            this.tabPage_Init.Location = new System.Drawing.Point(4, 4);
            this.tabPage_Init.Name = "tabPage_Init";
            this.tabPage_Init.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage_Init.Size = new System.Drawing.Size(485, 359);
            this.tabPage_Init.TabIndex = 4;
            this.tabPage_Init.Text = "InitPage";
            this.tabPage_Init.UseVisualStyleBackColor = true;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.ForeColor = System.Drawing.Color.Black;
            this.label31.Location = new System.Drawing.Point(170, 55);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(173, 20);
            this.label31.TabIndex = 21;
            this.label31.Text = "Select a Maze Style:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkBlue;
            this.label1.Location = new System.Drawing.Point(94, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(336, 24);
            this.label1.TabIndex = 20;
            this.label1.Text = "Welcome to the New Maze Wizard!";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(65, 306);
            this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(105, 20);
            this.label30.TabIndex = 19;
            this.label30.Text = "Circular Maze";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(295, 306);
            this.label29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(139, 20);
            this.label29.TabIndex = 18;
            this.label29.Text = "Rectangular Maze";
            // 
            // button_regMazeGen
            // 
            this.button_regMazeGen.BackgroundImage = global::MazeMaker.Properties.Resources.rectMazeLogo;
            this.button_regMazeGen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_regMazeGen.FlatAppearance.BorderSize = 0;
            this.button_regMazeGen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_regMazeGen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_regMazeGen.Location = new System.Drawing.Point(266, 101);
            this.button_regMazeGen.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_regMazeGen.Name = "button_regMazeGen";
            this.button_regMazeGen.Size = new System.Drawing.Size(200, 195);
            this.button_regMazeGen.TabIndex = 17;
            this.button_regMazeGen.UseVisualStyleBackColor = true;
            this.button_regMazeGen.Click += new System.EventHandler(this.button_regMazeGen_Click);
            // 
            // button_circMazeGen
            // 
            this.button_circMazeGen.BackgroundImage = global::MazeMaker.Properties.Resources.circMazeLogo;
            this.button_circMazeGen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_circMazeGen.FlatAppearance.BorderSize = 0;
            this.button_circMazeGen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_circMazeGen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_circMazeGen.Location = new System.Drawing.Point(27, 101);
            this.button_circMazeGen.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_circMazeGen.Name = "button_circMazeGen";
            this.button_circMazeGen.Size = new System.Drawing.Size(200, 195);
            this.button_circMazeGen.TabIndex = 16;
            this.button_circMazeGen.UseVisualStyleBackColor = true;
            this.button_circMazeGen.Click += new System.EventHandler(this.button_circMazeGen_Click);
            // 
            // MazeGen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 396);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.previousButton);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(698, 692);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(454, 419);
            this.Name = "MazeGen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Maze Wizard";
            this.Load += new System.EventHandler(this.MazeGen_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MazeGen_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_Rect1.ResumeLayout(false);
            this.tabPage_Rect1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thickNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lengthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumeric)).EndInit();
            this.tabPage_Rect2.ResumeLayout(false);
            this.tabPage_Rect2.PerformLayout();
            this.tabPage_Rect3.ResumeLayout(false);
            this.tabPage_Circ1.ResumeLayout(false);
            this.tabPage_Circ1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_corridorPreference)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ringThicknessNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ringRadiusNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ringFactorUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ringNumUpDown)).EndInit();
            this.tabPage_Init.ResumeLayout(false);
            this.tabPage_Init.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button previousButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_Rect1;
        private System.Windows.Forms.TabPage tabPage_Rect2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown thickNumeric;
        private System.Windows.Forms.NumericUpDown lengthNumeric;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown widthNumeric;
        private System.Windows.Forms.NumericUpDown heightNumeric;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabPage tabPage_Rect3;
        private System.Windows.Forms.Button regenerateButton;
        private System.Windows.Forms.RichTextBox outMazeText;
        private System.Windows.Forms.ComboBox comboBoxColor;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox comboBoxCeiling;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TabPage tabPage_Circ1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown ringThicknessNumeric;
        private System.Windows.Forms.NumericUpDown ringRadiusNumeric;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown ringFactorUpDown;
        private System.Windows.Forms.NumericUpDown ringNumUpDown;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TabPage tabPage_Init;
        private System.Windows.Forms.Button button_regMazeGen;
        private System.Windows.Forms.Button button_circMazeGen;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TrackBar trackBar_corridorPreference;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.ComboBox comboBoxColorCirc;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label colorBoxLabelCirc;
        private System.Windows.Forms.Label colorBoxLabelReg;
    }
}