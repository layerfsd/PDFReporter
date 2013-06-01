namespace AxiomCoders.PdfTemplateEditor.Forms
{
    partial class OptionsForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage_General = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.showBalloonBordersCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.autoSaveEnableCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage_Rulers = new System.Windows.Forms.TabPage();
            this.unitComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage_Grid = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.gridUnitComboBox = new System.Windows.Forms.ComboBox();
            this.snapToGridCheckBox = new System.Windows.Forms.CheckBox();
            this.showMinorLinesCheckBox = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.showGridCheckBox = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.styleComboBox = new System.Windows.Forms.ComboBox();
            this.gridColorPicker = new System.Windows.Forms.Button();
            this.OkApplyButton = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNumOffPreviewItems = new AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox();
            this.autoSaveIntervalTextBox = new AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox();
            this.subsValueTextBox = new AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox();
            this.gridValueTextBox = new AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox();
            this.tabControl.SuspendLayout();
            this.tabPage_General.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage_Rulers.SuspendLayout();
            this.tabPage_Grid.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage_General);
            this.tabControl.Controls.Add(this.tabPage_Rulers);
            this.tabControl.Controls.Add(this.tabPage_Grid);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(335, 199);
            this.tabControl.TabIndex = 0;
            // 
            // tabPage_General
            // 
            this.tabPage_General.Controls.Add(this.groupBox3);
            this.tabPage_General.Controls.Add(this.groupBox2);
            this.tabPage_General.Location = new System.Drawing.Point(4, 22);
            this.tabPage_General.Name = "tabPage_General";
            this.tabPage_General.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_General.Size = new System.Drawing.Size(327, 173);
            this.tabPage_General.TabIndex = 2;
            this.tabPage_General.Text = "General";
            this.tabPage_General.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtNumOffPreviewItems);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.showBalloonBordersCheckBox);
            this.groupBox3.Location = new System.Drawing.Point(8, 98);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(308, 69);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Common";
            // 
            // showBalloonBordersCheckBox
            // 
            this.showBalloonBordersCheckBox.AutoSize = true;
            this.showBalloonBordersCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.showBalloonBordersCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.showBalloonBordersCheckBox.Location = new System.Drawing.Point(7, 19);
            this.showBalloonBordersCheckBox.Name = "showBalloonBordersCheckBox";
            this.showBalloonBordersCheckBox.Size = new System.Drawing.Size(137, 18);
            this.showBalloonBordersCheckBox.TabIndex = 0;
            this.showBalloonBordersCheckBox.Text = "Show balloon borders:";
            this.showBalloonBordersCheckBox.UseVisualStyleBackColor = true;
            this.showBalloonBordersCheckBox.CheckedChanged += new System.EventHandler(this.showBalloonBordersCheckBox_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.autoSaveEnableCheckBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.autoSaveIntervalTextBox);
            this.groupBox2.Location = new System.Drawing.Point(8, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(308, 80);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Auto save";
            // 
            // autoSaveEnableCheckBox
            // 
            this.autoSaveEnableCheckBox.AutoSize = true;
            this.autoSaveEnableCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.autoSaveEnableCheckBox.Checked = true;
            this.autoSaveEnableCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoSaveEnableCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.autoSaveEnableCheckBox.Location = new System.Drawing.Point(8, 49);
            this.autoSaveEnableCheckBox.Name = "autoSaveEnableCheckBox";
            this.autoSaveEnableCheckBox.Size = new System.Drawing.Size(118, 18);
            this.autoSaveEnableCheckBox.TabIndex = 1;
            this.autoSaveEnableCheckBox.Text = "Enable auto save:";
            this.autoSaveEnableCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.autoSaveEnableCheckBox.UseVisualStyleBackColor = true;
            this.autoSaveEnableCheckBox.CheckedChanged += new System.EventHandler(this.autoSaveEnableCheckBox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Auto save interval:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(159, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "sec";
            // 
            // tabPage_Rulers
            // 
            this.tabPage_Rulers.Controls.Add(this.unitComboBox);
            this.tabPage_Rulers.Controls.Add(this.label1);
            this.tabPage_Rulers.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Rulers.Name = "tabPage_Rulers";
            this.tabPage_Rulers.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Rulers.Size = new System.Drawing.Size(327, 173);
            this.tabPage_Rulers.TabIndex = 0;
            this.tabPage_Rulers.Text = "Rulers & Units";
            this.tabPage_Rulers.UseVisualStyleBackColor = true;
            // 
            // unitComboBox
            // 
            this.unitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.unitComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.unitComboBox.FormattingEnabled = true;
            this.unitComboBox.Items.AddRange(new object[] {
            "inch",
            "mm",
            "cm",
            "pixel"});
            this.unitComboBox.Location = new System.Drawing.Point(38, 18);
            this.unitComboBox.Name = "unitComboBox";
            this.unitComboBox.Size = new System.Drawing.Size(103, 21);
            this.unitComboBox.TabIndex = 0;
            this.unitComboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Unit";
            // 
            // tabPage_Grid
            // 
            this.tabPage_Grid.Controls.Add(this.groupBox1);
            this.tabPage_Grid.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Grid.Name = "tabPage_Grid";
            this.tabPage_Grid.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Grid.Size = new System.Drawing.Size(327, 173);
            this.tabPage_Grid.TabIndex = 1;
            this.tabPage_Grid.Text = "Guides, Grid";
            this.tabPage_Grid.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.subsValueTextBox);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.gridUnitComboBox);
            this.groupBox1.Controls.Add(this.snapToGridCheckBox);
            this.groupBox1.Controls.Add(this.gridValueTextBox);
            this.groupBox1.Controls.Add(this.showMinorLinesCheckBox);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.showGridCheckBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.styleComboBox);
            this.groupBox1.Controls.Add(this.gridColorPicker);
            this.groupBox1.Location = new System.Drawing.Point(6, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(311, 154);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Grid";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 80);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Subdivisions:";
            // 
            // gridUnitComboBox
            // 
            this.gridUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gridUnitComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gridUnitComboBox.FormattingEnabled = true;
            this.gridUnitComboBox.Items.AddRange(new object[] {
            "inch",
            "cm",
            "mm",
            "pixels"});
            this.gridUnitComboBox.Location = new System.Drawing.Point(130, 50);
            this.gridUnitComboBox.Name = "gridUnitComboBox";
            this.gridUnitComboBox.Size = new System.Drawing.Size(73, 21);
            this.gridUnitComboBox.TabIndex = 2;
            this.gridUnitComboBox.SelectedIndexChanged += new System.EventHandler(this.gridUnitComboBox_SelectedIndexChanged);
            // 
            // snapToGridCheckBox
            // 
            this.snapToGridCheckBox.AutoSize = true;
            this.snapToGridCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.snapToGridCheckBox.Location = new System.Drawing.Point(210, 108);
            this.snapToGridCheckBox.Name = "snapToGridCheckBox";
            this.snapToGridCheckBox.Size = new System.Drawing.Size(83, 17);
            this.snapToGridCheckBox.TabIndex = 7;
            this.snapToGridCheckBox.Text = "Snap to grid";
            this.snapToGridCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.snapToGridCheckBox.UseVisualStyleBackColor = true;
            this.snapToGridCheckBox.Visible = false;
            this.snapToGridCheckBox.CheckedChanged += new System.EventHandler(this.snapToGridCheckBox_CheckedChanged);
            // 
            // showMinorLinesCheckBox
            // 
            this.showMinorLinesCheckBox.AutoSize = true;
            this.showMinorLinesCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.showMinorLinesCheckBox.Checked = true;
            this.showMinorLinesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showMinorLinesCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.showMinorLinesCheckBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.showMinorLinesCheckBox.Location = new System.Drawing.Point(92, 107);
            this.showMinorLinesCheckBox.Name = "showMinorLinesCheckBox";
            this.showMinorLinesCheckBox.Size = new System.Drawing.Size(111, 18);
            this.showMinorLinesCheckBox.TabIndex = 6;
            this.showMinorLinesCheckBox.Text = "Show minor lines";
            this.showMinorLinesCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.showMinorLinesCheckBox.UseVisualStyleBackColor = true;
            this.showMinorLinesCheckBox.CheckedChanged += new System.EventHandler(this.ShowMiLines_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 53);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Gridline every:";
            // 
            // showGridCheckBox
            // 
            this.showGridCheckBox.AutoSize = true;
            this.showGridCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.showGridCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.showGridCheckBox.Location = new System.Drawing.Point(7, 107);
            this.showGridCheckBox.Name = "showGridCheckBox";
            this.showGridCheckBox.Size = new System.Drawing.Size(79, 18);
            this.showGridCheckBox.TabIndex = 5;
            this.showGridCheckBox.Text = "Show grid";
            this.showGridCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.showGridCheckBox.UseVisualStyleBackColor = true;
            this.showGridCheckBox.CheckedChanged += new System.EventHandler(this.ShowGr_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(47, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Style:";
            // 
            // styleComboBox
            // 
            this.styleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.styleComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.styleComboBox.FormattingEnabled = true;
            this.styleComboBox.Items.AddRange(new object[] {
            "Lines",
            "Dashed Lines",
            "Dots"});
            this.styleComboBox.Location = new System.Drawing.Point(86, 23);
            this.styleComboBox.Name = "styleComboBox";
            this.styleComboBox.Size = new System.Drawing.Size(97, 21);
            this.styleComboBox.TabIndex = 0;
            this.styleComboBox.SelectedIndexChanged += new System.EventHandler(this.styleComboBox_SelectedIndexChanged);
            // 
            // gridColorPicker
            // 
            this.gridColorPicker.BackColor = System.Drawing.Color.DarkGray;
            this.gridColorPicker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gridColorPicker.Location = new System.Drawing.Point(246, 24);
            this.gridColorPicker.Name = "gridColorPicker";
            this.gridColorPicker.Size = new System.Drawing.Size(47, 47);
            this.gridColorPicker.TabIndex = 3;
            this.gridColorPicker.UseVisualStyleBackColor = false;
            this.gridColorPicker.Click += new System.EventHandler(this.MjColor_Click);
            // 
            // OkApplyButton
            // 
            this.OkApplyButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.OkApplyButton.Location = new System.Drawing.Point(353, 32);
            this.OkApplyButton.Name = "OkApplyButton";
            this.OkApplyButton.Size = new System.Drawing.Size(75, 23);
            this.OkApplyButton.TabIndex = 1;
            this.OkApplyButton.Text = "&Ok";
            this.OkApplyButton.UseVisualStyleBackColor = true;
            this.OkApplyButton.Click += new System.EventHandler(this.OkApplyButton_Click);
            // 
            // applyButton
            // 
            this.applyButton.Enabled = false;
            this.applyButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.applyButton.Location = new System.Drawing.Point(353, 102);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 3;
            this.applyButton.Text = "&Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cancelButton.Location = new System.Drawing.Point(353, 61);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnHelp.Location = new System.Drawing.Point(353, 188);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 4;
            this.btnHelp.Text = "&Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Num of preview items:";
            // 
            // txtNumOffPreviewItems
            // 
            this.txtNumOffPreviewItems.AllowSpace = false;
            this.txtNumOffPreviewItems.Location = new System.Drawing.Point(132, 43);
            this.txtNumOffPreviewItems.Name = "txtNumOffPreviewItems";
            this.txtNumOffPreviewItems.Size = new System.Drawing.Size(60, 20);
            this.txtNumOffPreviewItems.TabIndex = 3;
            this.txtNumOffPreviewItems.Text = "10";
            this.txtNumOffPreviewItems.TextChanged += new System.EventHandler(this.txtNumOffPreviewItems_TextChanged);
            // 
            // autoSaveIntervalTextBox
            // 
            this.autoSaveIntervalTextBox.AllowSpace = false;
            this.autoSaveIntervalTextBox.Location = new System.Drawing.Point(107, 23);
            this.autoSaveIntervalTextBox.Name = "autoSaveIntervalTextBox";
            this.autoSaveIntervalTextBox.Size = new System.Drawing.Size(46, 20);
            this.autoSaveIntervalTextBox.TabIndex = 0;
            this.autoSaveIntervalTextBox.TextChanged += new System.EventHandler(this.autoSaveIntervalTextBox_TextChanged);
            // 
            // subsValueTextBox
            // 
            this.subsValueTextBox.AllowSpace = false;
            this.subsValueTextBox.Location = new System.Drawing.Point(86, 77);
            this.subsValueTextBox.Name = "subsValueTextBox";
            this.subsValueTextBox.Size = new System.Drawing.Size(38, 20);
            this.subsValueTextBox.TabIndex = 4;
            this.subsValueTextBox.TextChanged += new System.EventHandler(this.subsValueTextBox_TextChanged);
            // 
            // gridValueTextBox
            // 
            this.gridValueTextBox.AllowSpace = false;
            this.gridValueTextBox.Location = new System.Drawing.Point(86, 50);
            this.gridValueTextBox.Name = "gridValueTextBox";
            this.gridValueTextBox.Size = new System.Drawing.Size(38, 20);
            this.gridValueTextBox.TabIndex = 1;
            this.gridValueTextBox.TextChanged += new System.EventHandler(this.gridValueTextBox_TextChanged);
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.OkApplyButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(440, 225);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.OkApplyButton);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.Text = "Preferences";
            this.TopMost = true;
            this.tabControl.ResumeLayout(false);
            this.tabPage_General.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage_Rulers.ResumeLayout(false);
            this.tabPage_Rulers.PerformLayout();
            this.tabPage_Grid.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage_Rulers;
        private System.Windows.Forms.ComboBox unitComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage_Grid;
        private System.Windows.Forms.Button OkApplyButton;
        private System.Windows.Forms.Button gridColorPicker;
        private System.Windows.Forms.CheckBox showGridCheckBox;
        private System.Windows.Forms.CheckBox showMinorLinesCheckBox;
        private System.Windows.Forms.CheckBox snapToGridCheckBox;
        private System.Windows.Forms.TabPage tabPage_General;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox gridUnitComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox styleComboBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox autoSaveEnableCheckBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox showBalloonBordersCheckBox;
        private System.Windows.Forms.Button applyButton;
		private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.TextBox textBox1;
        private AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox gridValueTextBox;
        private AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox subsValueTextBox;
        private AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox autoSaveIntervalTextBox;
        private AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox txtNumOffPreviewItems;
        private System.Windows.Forms.Label label4;
    }
}