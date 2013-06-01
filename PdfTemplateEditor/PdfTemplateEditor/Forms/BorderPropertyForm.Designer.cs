namespace AxiomCoders.PdfTemplateEditor.Forms
{
    partial class BorderPropertyForm
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
            if(disposing && (components != null))
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
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkBoxEnableRight = new System.Windows.Forms.CheckBox();
            this.chkBoxEnableBottom = new System.Windows.Forms.CheckBox();
            this.chkBoxEnableLeft = new System.Windows.Forms.CheckBox();
            this.chkBoxEnableTop = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonRightColor = new System.Windows.Forms.Button();
            this.buttonBottomColor = new System.Windows.Forms.Button();
            this.buttonLeftColor = new System.Windows.Forms.Button();
            this.buttonTopColor = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.numTexBoxRightWidth = new AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox();
            this.numTexBoxBottomWidth = new AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox();
            this.numTexBoxLeftWidth = new AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox();
            this.numTexBoxTopWidth = new AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbBoxRightStyle = new System.Windows.Forms.ComboBox();
            this.cmbBoxBottomStyle = new System.Windows.Forms.ComboBox();
            this.cmbBoxLeftStyle = new System.Windows.Forms.ComboBox();
            this.cmbBoxTopStyle = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lablTopBorder = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lablBalloonName = new System.Windows.Forms.Label();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(179, 199);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "&Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(260, 199);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonHelp.Location = new System.Drawing.Point(12, 199);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(75, 23);
            this.buttonHelp.TabIndex = 3;
            this.buttonHelp.Text = "&Help";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.chkAll);
            this.groupBox1.Controls.Add(this.chkBoxEnableRight);
            this.groupBox1.Controls.Add(this.chkBoxEnableBottom);
            this.groupBox1.Controls.Add(this.chkBoxEnableLeft);
            this.groupBox1.Controls.Add(this.chkBoxEnableTop);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.buttonRightColor);
            this.groupBox1.Controls.Add(this.buttonBottomColor);
            this.groupBox1.Controls.Add(this.buttonLeftColor);
            this.groupBox1.Controls.Add(this.buttonTopColor);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.numTexBoxRightWidth);
            this.groupBox1.Controls.Add(this.numTexBoxBottomWidth);
            this.groupBox1.Controls.Add(this.numTexBoxLeftWidth);
            this.groupBox1.Controls.Add(this.numTexBoxTopWidth);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbBoxRightStyle);
            this.groupBox1.Controls.Add(this.cmbBoxBottomStyle);
            this.groupBox1.Controls.Add(this.cmbBoxLeftStyle);
            this.groupBox1.Controls.Add(this.cmbBoxTopStyle);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lablTopBorder);
            this.groupBox1.Location = new System.Drawing.Point(12, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(323, 163);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Border";
            // 
            // chkBoxEnableRight
            // 
            this.chkBoxEnableRight.AutoSize = true;
            this.chkBoxEnableRight.Location = new System.Drawing.Point(291, 127);
            this.chkBoxEnableRight.Name = "chkBoxEnableRight";
            this.chkBoxEnableRight.Size = new System.Drawing.Size(15, 14);
            this.chkBoxEnableRight.TabIndex = 15;
            this.chkBoxEnableRight.UseVisualStyleBackColor = true;
            this.chkBoxEnableRight.CheckedChanged += new System.EventHandler(this.chkBoxEnableRight_CheckedChanged);
            // 
            // chkBoxEnableBottom
            // 
            this.chkBoxEnableBottom.AutoSize = true;
            this.chkBoxEnableBottom.Location = new System.Drawing.Point(291, 100);
            this.chkBoxEnableBottom.Name = "chkBoxEnableBottom";
            this.chkBoxEnableBottom.Size = new System.Drawing.Size(15, 14);
            this.chkBoxEnableBottom.TabIndex = 11;
            this.chkBoxEnableBottom.UseVisualStyleBackColor = true;
            this.chkBoxEnableBottom.CheckedChanged += new System.EventHandler(this.chkBoxEnableBottom_CheckedChanged);
            // 
            // chkBoxEnableLeft
            // 
            this.chkBoxEnableLeft.AutoSize = true;
            this.chkBoxEnableLeft.Location = new System.Drawing.Point(291, 73);
            this.chkBoxEnableLeft.Name = "chkBoxEnableLeft";
            this.chkBoxEnableLeft.Size = new System.Drawing.Size(15, 14);
            this.chkBoxEnableLeft.TabIndex = 7;
            this.chkBoxEnableLeft.UseVisualStyleBackColor = true;
            this.chkBoxEnableLeft.CheckedChanged += new System.EventHandler(this.chkBoxEnableLeft_CheckedChanged);
            // 
            // chkBoxEnableTop
            // 
            this.chkBoxEnableTop.AutoSize = true;
            this.chkBoxEnableTop.Location = new System.Drawing.Point(291, 45);
            this.chkBoxEnableTop.Name = "chkBoxEnableTop";
            this.chkBoxEnableTop.Size = new System.Drawing.Size(15, 14);
            this.chkBoxEnableTop.TabIndex = 3;
            this.chkBoxEnableTop.UseVisualStyleBackColor = true;
            this.chkBoxEnableTop.CheckedChanged += new System.EventHandler(this.chkBoxEnableTop_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(229, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Color";
            // 
            // buttonRightColor
            // 
            this.buttonRightColor.BackColor = System.Drawing.Color.Black;
            this.buttonRightColor.Enabled = false;
            this.buttonRightColor.Location = new System.Drawing.Point(210, 122);
            this.buttonRightColor.Name = "buttonRightColor";
            this.buttonRightColor.Size = new System.Drawing.Size(75, 23);
            this.buttonRightColor.TabIndex = 14;
            this.buttonRightColor.UseVisualStyleBackColor = false;
            this.buttonRightColor.Click += new System.EventHandler(this.buttonRightColor_Click);
            // 
            // buttonBottomColor
            // 
            this.buttonBottomColor.BackColor = System.Drawing.Color.Black;
            this.buttonBottomColor.Enabled = false;
            this.buttonBottomColor.Location = new System.Drawing.Point(210, 95);
            this.buttonBottomColor.Name = "buttonBottomColor";
            this.buttonBottomColor.Size = new System.Drawing.Size(75, 23);
            this.buttonBottomColor.TabIndex = 10;
            this.buttonBottomColor.UseVisualStyleBackColor = false;
            this.buttonBottomColor.Click += new System.EventHandler(this.buttonBottomColor_Click);
            // 
            // buttonLeftColor
            // 
            this.buttonLeftColor.BackColor = System.Drawing.Color.Black;
            this.buttonLeftColor.Enabled = false;
            this.buttonLeftColor.Location = new System.Drawing.Point(210, 68);
            this.buttonLeftColor.Name = "buttonLeftColor";
            this.buttonLeftColor.Size = new System.Drawing.Size(75, 23);
            this.buttonLeftColor.TabIndex = 6;
            this.buttonLeftColor.UseVisualStyleBackColor = false;
            this.buttonLeftColor.Click += new System.EventHandler(this.buttonLeftColor_Click);
            // 
            // buttonTopColor
            // 
            this.buttonTopColor.BackColor = System.Drawing.Color.Black;
            this.buttonTopColor.Enabled = false;
            this.buttonTopColor.Location = new System.Drawing.Point(210, 41);
            this.buttonTopColor.Name = "buttonTopColor";
            this.buttonTopColor.Size = new System.Drawing.Size(75, 23);
            this.buttonTopColor.TabIndex = 2;
            this.buttonTopColor.UseVisualStyleBackColor = false;
            this.buttonTopColor.Click += new System.EventHandler(this.buttonTopColor_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(159, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Width";
            // 
            // numTexBoxRightWidth
            // 
            this.numTexBoxRightWidth.AllowSpace = false;
            this.numTexBoxRightWidth.Enabled = false;
            this.numTexBoxRightWidth.Location = new System.Drawing.Point(153, 124);
            this.numTexBoxRightWidth.Name = "numTexBoxRightWidth";
            this.numTexBoxRightWidth.Size = new System.Drawing.Size(51, 20);
            this.numTexBoxRightWidth.TabIndex = 13;
            this.numTexBoxRightWidth.Text = "1.0";
            // 
            // numTexBoxBottomWidth
            // 
            this.numTexBoxBottomWidth.AllowSpace = false;
            this.numTexBoxBottomWidth.Enabled = false;
            this.numTexBoxBottomWidth.Location = new System.Drawing.Point(153, 97);
            this.numTexBoxBottomWidth.Name = "numTexBoxBottomWidth";
            this.numTexBoxBottomWidth.Size = new System.Drawing.Size(51, 20);
            this.numTexBoxBottomWidth.TabIndex = 9;
            this.numTexBoxBottomWidth.Text = "1.0";
            // 
            // numTexBoxLeftWidth
            // 
            this.numTexBoxLeftWidth.AllowSpace = false;
            this.numTexBoxLeftWidth.Enabled = false;
            this.numTexBoxLeftWidth.Location = new System.Drawing.Point(153, 70);
            this.numTexBoxLeftWidth.Name = "numTexBoxLeftWidth";
            this.numTexBoxLeftWidth.Size = new System.Drawing.Size(51, 20);
            this.numTexBoxLeftWidth.TabIndex = 5;
            this.numTexBoxLeftWidth.Text = "1.0";
            // 
            // numTexBoxTopWidth
            // 
            this.numTexBoxTopWidth.AllowSpace = false;
            this.numTexBoxTopWidth.Enabled = false;
            this.numTexBoxTopWidth.Location = new System.Drawing.Point(153, 43);
            this.numTexBoxTopWidth.Name = "numTexBoxTopWidth";
            this.numTexBoxTopWidth.Size = new System.Drawing.Size(51, 20);
            this.numTexBoxTopWidth.TabIndex = 1;
            this.numTexBoxTopWidth.Text = "1.0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(76, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Style";
            // 
            // cmbBoxRightStyle
            // 
            this.cmbBoxRightStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxRightStyle.Enabled = false;
            this.cmbBoxRightStyle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbBoxRightStyle.FormattingEnabled = true;
            this.cmbBoxRightStyle.Items.AddRange(new object[] {
            "Solid Line",
            "Dashed Line",
            "Dots"});
            this.cmbBoxRightStyle.Location = new System.Drawing.Point(52, 124);
            this.cmbBoxRightStyle.Name = "cmbBoxRightStyle";
            this.cmbBoxRightStyle.Size = new System.Drawing.Size(95, 21);
            this.cmbBoxRightStyle.TabIndex = 12;
            // 
            // cmbBoxBottomStyle
            // 
            this.cmbBoxBottomStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxBottomStyle.Enabled = false;
            this.cmbBoxBottomStyle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbBoxBottomStyle.FormattingEnabled = true;
            this.cmbBoxBottomStyle.Items.AddRange(new object[] {
            "Solid Line",
            "Dashed Line",
            "Dots"});
            this.cmbBoxBottomStyle.Location = new System.Drawing.Point(52, 97);
            this.cmbBoxBottomStyle.Name = "cmbBoxBottomStyle";
            this.cmbBoxBottomStyle.Size = new System.Drawing.Size(95, 21);
            this.cmbBoxBottomStyle.TabIndex = 8;
            // 
            // cmbBoxLeftStyle
            // 
            this.cmbBoxLeftStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxLeftStyle.Enabled = false;
            this.cmbBoxLeftStyle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbBoxLeftStyle.FormattingEnabled = true;
            this.cmbBoxLeftStyle.Items.AddRange(new object[] {
            "Solid Line",
            "Dashed Line",
            "Dots"});
            this.cmbBoxLeftStyle.Location = new System.Drawing.Point(52, 70);
            this.cmbBoxLeftStyle.Name = "cmbBoxLeftStyle";
            this.cmbBoxLeftStyle.Size = new System.Drawing.Size(95, 21);
            this.cmbBoxLeftStyle.TabIndex = 4;
            // 
            // cmbBoxTopStyle
            // 
            this.cmbBoxTopStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxTopStyle.Enabled = false;
            this.cmbBoxTopStyle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbBoxTopStyle.FormattingEnabled = true;
            this.cmbBoxTopStyle.Items.AddRange(new object[] {
            "Solid Line",
            "Dashed Line",
            "Dots"});
            this.cmbBoxTopStyle.Location = new System.Drawing.Point(52, 43);
            this.cmbBoxTopStyle.Name = "cmbBoxTopStyle";
            this.cmbBoxTopStyle.Size = new System.Drawing.Size(95, 21);
            this.cmbBoxTopStyle.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Right";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Bottom";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Left";
            // 
            // lablTopBorder
            // 
            this.lablTopBorder.AutoSize = true;
            this.lablTopBorder.Location = new System.Drawing.Point(20, 46);
            this.lablTopBorder.Name = "lablTopBorder";
            this.lablTopBorder.Size = new System.Drawing.Size(26, 13);
            this.lablTopBorder.TabIndex = 0;
            this.lablTopBorder.Text = "Top";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Active balloon name:";
            // 
            // lablBalloonName
            // 
            this.lablBalloonName.AutoSize = true;
            this.lablBalloonName.Location = new System.Drawing.Point(124, 9);
            this.lablBalloonName.Name = "lablBalloonName";
            this.lablBalloonName.Size = new System.Drawing.Size(35, 13);
            this.lablBalloonName.TabIndex = 5;
            this.lablBalloonName.Text = "Name";
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(291, 19);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(15, 14);
            this.chkAll.TabIndex = 19;
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // BorderPropertyForm
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(347, 234);
            this.Controls.Add(this.lablBalloonName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BorderPropertyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Border Properties";
            this.Shown += new System.EventHandler(this.BorderPropertyForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lablBalloonName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lablTopBorder;
        private System.Windows.Forms.ComboBox cmbBoxRightStyle;
        private System.Windows.Forms.ComboBox cmbBoxBottomStyle;
        private System.Windows.Forms.ComboBox cmbBoxLeftStyle;
        private System.Windows.Forms.ComboBox cmbBoxTopStyle;
        private System.Windows.Forms.Button buttonRightColor;
        private System.Windows.Forms.Button buttonBottomColor;
        private System.Windows.Forms.Button buttonLeftColor;
        private System.Windows.Forms.Button buttonTopColor;
        private System.Windows.Forms.Label label6;
        private AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox numTexBoxRightWidth;
        private AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox numTexBoxBottomWidth;
        private AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox numTexBoxLeftWidth;
        private AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox numTexBoxTopWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkBoxEnableRight;
        private System.Windows.Forms.CheckBox chkBoxEnableBottom;
        private System.Windows.Forms.CheckBox chkBoxEnableLeft;
        private System.Windows.Forms.CheckBox chkBoxEnableTop;
        private System.Windows.Forms.CheckBox chkAll;
    }
}