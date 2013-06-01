namespace AxiomCoders.PdfTemplateEditor.Forms
{
	partial class NewProjectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewProjectForm));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.projectName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.projectSizeProperty = new System.Windows.Forms.ComboBox();
            this.projectWidthProperty = new System.Windows.Forms.ComboBox();
            this.projectHeightProperty = new System.Windows.Forms.ComboBox();
            this.projectResolutionProperty = new System.Windows.Forms.ComboBox();
            this.projectResolution = new AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox();
            this.projectHeight = new AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox();
            this.projectWidth = new AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.projectSubject = new System.Windows.Forms.TextBox();
            this.projectAuthor = new System.Windows.Forms.TextBox();
            this.projectTitle = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOk.Location = new System.Drawing.Point(348, 12);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(348, 41);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // projectName
            // 
            this.projectName.Location = new System.Drawing.Point(80, 24);
            this.projectName.Name = "projectName";
            this.projectName.Size = new System.Drawing.Size(244, 20);
            this.projectName.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.projectSizeProperty);
            this.groupBox1.Controls.Add(this.projectWidthProperty);
            this.groupBox1.Controls.Add(this.projectHeightProperty);
            this.groupBox1.Controls.Add(this.projectResolutionProperty);
            this.groupBox1.Controls.Add(this.projectResolution);
            this.groupBox1.Controls.Add(this.projectHeight);
            this.groupBox1.Controls.Add(this.projectWidth);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 174);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(330, 146);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Page Properties";
            // 
            // projectSizeProperty
            // 
            this.projectSizeProperty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.projectSizeProperty.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.projectSizeProperty.FormattingEnabled = true;
            this.projectSizeProperty.Items.AddRange(new object[] {
            "Custom",
            "Letter",
            "Legal",
            "Tabloid",
            "640 x 480",
            "800 x 600",
            "1024 x 768",
            "A4",
            "A3",
            "B5",
            "B4",
            "B3"});
            this.projectSizeProperty.Location = new System.Drawing.Point(80, 27);
            this.projectSizeProperty.Name = "projectSizeProperty";
            this.projectSizeProperty.Size = new System.Drawing.Size(244, 21);
            this.projectSizeProperty.TabIndex = 0;
            this.projectSizeProperty.SelectedIndexChanged += new System.EventHandler(this.projectSizeProperty_SelectedIndexChanged);
            // 
            // projectWidthProperty
            // 
            this.projectWidthProperty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.projectWidthProperty.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.projectWidthProperty.FormattingEnabled = true;
            this.projectWidthProperty.Items.AddRange(new object[] {
            "inches",
            "cm",
            "mm",
            "pixels"});
            this.projectWidthProperty.Location = new System.Drawing.Point(194, 54);
            this.projectWidthProperty.Name = "projectWidthProperty";
            this.projectWidthProperty.Size = new System.Drawing.Size(130, 21);
            this.projectWidthProperty.TabIndex = 2;
            this.projectWidthProperty.SelectionChangeCommitted += new System.EventHandler(this.projectWidthProperty_SelectedIndexChanged);
            // 
            // projectHeightProperty
            // 
            this.projectHeightProperty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.projectHeightProperty.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.projectHeightProperty.FormattingEnabled = true;
            this.projectHeightProperty.Items.AddRange(new object[] {
            "inches",
            "cm",
            "mm",
            "pixels"});
            this.projectHeightProperty.Location = new System.Drawing.Point(194, 84);
            this.projectHeightProperty.Name = "projectHeightProperty";
            this.projectHeightProperty.Size = new System.Drawing.Size(130, 21);
            this.projectHeightProperty.TabIndex = 4;
            this.projectHeightProperty.SelectionChangeCommitted += new System.EventHandler(this.projectHeightProperty_SelectedIndexChanged);
            // 
            // projectResolutionProperty
            // 
            this.projectResolutionProperty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.projectResolutionProperty.Enabled = false;
            this.projectResolutionProperty.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.projectResolutionProperty.FormattingEnabled = true;
            this.projectResolutionProperty.Items.AddRange(new object[] {
            "pixels/inch",
            "pixels/cm"});
            this.projectResolutionProperty.Location = new System.Drawing.Point(194, 114);
            this.projectResolutionProperty.Name = "projectResolutionProperty";
            this.projectResolutionProperty.Size = new System.Drawing.Size(130, 21);
            this.projectResolutionProperty.TabIndex = 6;
            this.projectResolutionProperty.SelectedIndexChanged += new System.EventHandler(this.projectResolutionProperty_SelectedIndexChanged);
            // 
            // projectResolution
            // 
            this.projectResolution.AllowSpace = false;
            this.projectResolution.Enabled = false;
            this.projectResolution.Location = new System.Drawing.Point(80, 115);
            this.projectResolution.Name = "projectResolution";
            this.projectResolution.Size = new System.Drawing.Size(108, 20);
            this.projectResolution.TabIndex = 5;
            this.projectResolution.TextChanged += new System.EventHandler(this.projectResolution_TextChanged);
            // 
            // projectHeight
            // 
            this.projectHeight.AllowSpace = false;
            this.projectHeight.Location = new System.Drawing.Point(80, 85);
            this.projectHeight.Name = "projectHeight";
            this.projectHeight.Size = new System.Drawing.Size(108, 20);
            this.projectHeight.TabIndex = 3;
            this.projectHeight.TextChanged += new System.EventHandler(this.projectHeight_TextChanged);
            this.projectHeight.GotFocus += new System.EventHandler(this.projectHeight_GotFocus);
            this.projectHeight.MouseClick += new System.Windows.Forms.MouseEventHandler(this.projectHeight_MouseClick);
            // 
            // projectWidth
            // 
            this.projectWidth.AllowSpace = false;
            this.projectWidth.Location = new System.Drawing.Point(80, 55);
            this.projectWidth.Name = "projectWidth";
            this.projectWidth.Size = new System.Drawing.Size(108, 20);
            this.projectWidth.TabIndex = 1;
            this.projectWidth.TextChanged += new System.EventHandler(this.projectWidth_TextChanged);
            this.projectWidth.GotFocus += new System.EventHandler(this.projectWidth_GotFocus);
            this.projectWidth.MouseClick += new System.Windows.Forms.MouseEventHandler(this.projectWidth_MouseClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Resolution:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Height:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Width:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Size:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textBox4);
            this.groupBox2.Controls.Add(this.projectSubject);
            this.groupBox2.Controls.Add(this.projectAuthor);
            this.groupBox2.Controls.Add(this.projectTitle);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.projectName);
            this.groupBox2.Location = new System.Drawing.Point(12, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(330, 161);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Project Properties";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 131);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Producer:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(28, 105);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Subject:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Author:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(44, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Title:";
            // 
            // textBox4
            // 
            this.textBox4.Enabled = false;
            this.textBox4.Location = new System.Drawing.Point(80, 128);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(244, 20);
            this.textBox4.TabIndex = 4;
            this.textBox4.Text = "Axiomcoders - PdfFactory (www.axiomcoders.com)";
            // 
            // projectSubject
            // 
            this.projectSubject.Location = new System.Drawing.Point(80, 102);
            this.projectSubject.Name = "projectSubject";
            this.projectSubject.Size = new System.Drawing.Size(244, 20);
            this.projectSubject.TabIndex = 3;
            this.projectSubject.Text = "No Subject";
            // 
            // projectAuthor
            // 
            this.projectAuthor.Location = new System.Drawing.Point(80, 76);
            this.projectAuthor.Name = "projectAuthor";
            this.projectAuthor.Size = new System.Drawing.Size(244, 20);
            this.projectAuthor.TabIndex = 2;
            this.projectAuthor.Text = "Unnamed";
            // 
            // projectTitle
            // 
            this.projectTitle.Location = new System.Drawing.Point(80, 50);
            this.projectTitle.Name = "projectTitle";
            this.projectTitle.Size = new System.Drawing.Size(244, 20);
            this.projectTitle.TabIndex = 1;
            this.projectTitle.Text = "Untitled";
            // 
            // NewProjectForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(459, 330);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewProjectForm";
            this.Text = "New Project...";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox projectName;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox projectWidthProperty;
		private System.Windows.Forms.ComboBox projectHeightProperty;
        private System.Windows.Forms.ComboBox projectResolutionProperty;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox projectSizeProperty;
        private AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox projectResolution;
        private AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox projectHeight;
        private AxiomCoders.PdfTemplateEditor.Controls.NumericTextBox projectWidth;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox projectSubject;
        private System.Windows.Forms.TextBox projectAuthor;
        private System.Windows.Forms.TextBox projectTitle;
	}
}