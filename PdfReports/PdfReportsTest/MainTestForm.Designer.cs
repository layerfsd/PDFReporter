namespace axiomPdfTest
{
	partial class MainTestForm
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
            this.btnGenerate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTemplateFile = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.OD = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPDF = new System.Windows.Forms.TextBox();
            this.btnBrowsePdfOutput = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.SD = new System.Windows.Forms.SaveFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.tabDataDesc = new System.Windows.Forms.TabControl();
            this.tabData1 = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabData2 = new System.Windows.Forms.TabPage();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tabData3 = new System.Windows.Forms.TabPage();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.tabResults = new System.Windows.Forms.TabPage();
            this.lbxResults = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.edtNumberOfItems = new System.Windows.Forms.TextBox();
            this.rbtDemoData1 = new System.Windows.Forms.RadioButton();
            this.rbtDemoData2 = new System.Windows.Forms.RadioButton();
            this.rbtDemoData3 = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDatabaseConnection = new System.Windows.Forms.Button();
            this.rbtDataSourceDatabase = new System.Windows.Forms.RadioButton();
            this.rbtDataSourceApplication = new System.Windows.Forms.RadioButton();
            this.txtSerial = new System.Windows.Forms.TextBox();
            this.lblSerial = new System.Windows.Forms.Label();
            this.btnCallbackGeneration = new System.Windows.Forms.Button();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.tabDataDesc.SuspendLayout();
            this.tabData1.SuspendLayout();
            this.tabData2.SuspendLayout();
            this.tabData3.SuspendLayout();
            this.tabResults.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(139, 289);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Template Name:";
            // 
            // txtTemplateFile
            // 
            this.txtTemplateFile.Location = new System.Drawing.Point(139, 23);
            this.txtTemplateFile.Name = "txtTemplateFile";
            this.txtTemplateFile.Size = new System.Drawing.Size(198, 20);
            this.txtTemplateFile.TabIndex = 3;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(343, 21);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(38, 23);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // OD
            // 
            this.OD.DefaultExt = "xml";
            this.OD.FileName = "template.xml";
            this.OD.Filter = "Template Files|*.prtp|All Files|*.*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "PDF Name:";
            // 
            // txtPDF
            // 
            this.txtPDF.Location = new System.Drawing.Point(139, 47);
            this.txtPDF.Name = "txtPDF";
            this.txtPDF.Size = new System.Drawing.Size(198, 20);
            this.txtPDF.TabIndex = 6;
            // 
            // btnBrowsePdfOutput
            // 
            this.btnBrowsePdfOutput.Location = new System.Drawing.Point(343, 47);
            this.btnBrowsePdfOutput.Name = "btnBrowsePdfOutput";
            this.btnBrowsePdfOutput.Size = new System.Drawing.Size(38, 23);
            this.btnBrowsePdfOutput.TabIndex = 7;
            this.btnBrowsePdfOutput.Text = "...";
            this.btnBrowsePdfOutput.UseVisualStyleBackColor = true;
            this.btnBrowsePdfOutput.Click += new System.EventHandler(this.btnBrowsePdfOutput_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 468);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(571, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(93, 17);
            this.lblStatus.Text = "Not Generated...";
            // 
            // SD
            // 
            this.SD.Filter = "Pdf Files|*.pdf";
            this.SD.Title = "Save Pdf File";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Data Source Type:";
            // 
            // tabDataDesc
            // 
            this.tabDataDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tabDataDesc.Controls.Add(this.tabData1);
            this.tabDataDesc.Controls.Add(this.tabData2);
            this.tabDataDesc.Controls.Add(this.tabData3);
            this.tabDataDesc.Controls.Add(this.tabResults);
            this.tabDataDesc.Location = new System.Drawing.Point(12, 319);
            this.tabDataDesc.Name = "tabDataDesc";
            this.tabDataDesc.SelectedIndex = 0;
            this.tabDataDesc.Size = new System.Drawing.Size(547, 146);
            this.tabDataDesc.TabIndex = 13;
            // 
            // tabData1
            // 
            this.tabData1.Controls.Add(this.textBox1);
            this.tabData1.Location = new System.Drawing.Point(4, 22);
            this.tabData1.Name = "tabData1";
            this.tabData1.Padding = new System.Windows.Forms.Padding(3);
            this.tabData1.Size = new System.Drawing.Size(539, 120);
            this.tabData1.TabIndex = 0;
            this.tabData1.Text = "Data 1 Description";
            this.tabData1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(3, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(533, 114);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "DataStream:\r\n - Company \r\n      Id, Name, Logo, BinaryLogo, Description\r\n- Manufa" +
                "cturer\r\n    Id, Name, Description\r\n- Product\r\n   Id, Name, Price, Description, Q" +
                "uantity\r\n\r\n";
            // 
            // tabData2
            // 
            this.tabData2.Controls.Add(this.textBox2);
            this.tabData2.Location = new System.Drawing.Point(4, 22);
            this.tabData2.Name = "tabData2";
            this.tabData2.Padding = new System.Windows.Forms.Padding(3);
            this.tabData2.Size = new System.Drawing.Size(539, 138);
            this.tabData2.TabIndex = 1;
            this.tabData2.Text = "Data 2 Description";
            this.tabData2.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Location = new System.Drawing.Point(3, 3);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(533, 132);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "DataStream:\r\n - Company \r\n      Id, Name, Logo, BinaryLogo, Description\r\n- Produc" +
                "t\r\n   Id, Name, Description\r\n- Price\r\n  ProductId, Value, Date, Quantity\r\n\r\n";
            // 
            // tabData3
            // 
            this.tabData3.Controls.Add(this.textBox3);
            this.tabData3.Location = new System.Drawing.Point(4, 22);
            this.tabData3.Name = "tabData3";
            this.tabData3.Size = new System.Drawing.Size(539, 138);
            this.tabData3.TabIndex = 2;
            this.tabData3.Text = "Data 3 Description";
            this.tabData3.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox3.Location = new System.Drawing.Point(0, 0);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(539, 138);
            this.textBox3.TabIndex = 2;
            this.textBox3.Text = "DataStream:\r\n - Bank\r\n      Id, Name, Description, Address\r\n- Client\r\n   BankId, " +
                "Id, Name, PhotoInline, Description\r\n- Account\r\n  ClientId, Id, AccNumber, LastCh" +
                "ange, Balance\r\n\r\n";
            // 
            // tabResults
            // 
            this.tabResults.Controls.Add(this.lbxResults);
            this.tabResults.Location = new System.Drawing.Point(4, 22);
            this.tabResults.Name = "tabResults";
            this.tabResults.Size = new System.Drawing.Size(539, 138);
            this.tabResults.TabIndex = 3;
            this.tabResults.Text = "Results";
            this.tabResults.UseVisualStyleBackColor = true;
            // 
            // lbxResults
            // 
            this.lbxResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxResults.FormattingEnabled = true;
            this.lbxResults.Location = new System.Drawing.Point(0, 0);
            this.lbxResults.Name = "lbxResults";
            this.lbxResults.Size = new System.Drawing.Size(539, 134);
            this.lbxResults.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 266);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Number of items (seed):";
            // 
            // edtNumberOfItems
            // 
            this.edtNumberOfItems.Location = new System.Drawing.Point(139, 263);
            this.edtNumberOfItems.Name = "edtNumberOfItems";
            this.edtNumberOfItems.Size = new System.Drawing.Size(100, 20);
            this.edtNumberOfItems.TabIndex = 15;
            this.edtNumberOfItems.Text = "50";
            // 
            // rbtDemoData1
            // 
            this.rbtDemoData1.AutoSize = true;
            this.rbtDemoData1.Checked = true;
            this.rbtDemoData1.Location = new System.Drawing.Point(139, 124);
            this.rbtDemoData1.Name = "rbtDemoData1";
            this.rbtDemoData1.Size = new System.Drawing.Size(255, 17);
            this.rbtDemoData1.TabIndex = 16;
            this.rbtDemoData1.TabStop = true;
            this.rbtDemoData1.Text = "Demo Data 1 (Company,manufacturers,products)";
            this.rbtDemoData1.UseVisualStyleBackColor = true;
            this.rbtDemoData1.Click += new System.EventHandler(this.rbtDemoData1_Click);
            // 
            // rbtDemoData2
            // 
            this.rbtDemoData2.AutoSize = true;
            this.rbtDemoData2.Location = new System.Drawing.Point(139, 145);
            this.rbtDemoData2.Name = "rbtDemoData2";
            this.rbtDemoData2.Size = new System.Drawing.Size(218, 17);
            this.rbtDemoData2.TabIndex = 17;
            this.rbtDemoData2.Text = "Demo Data 2 (Company,Products,Prices)";
            this.rbtDemoData2.UseVisualStyleBackColor = true;
            this.rbtDemoData2.Click += new System.EventHandler(this.rbtDemoData2_Click);
            // 
            // rbtDemoData3
            // 
            this.rbtDemoData3.AutoSize = true;
            this.rbtDemoData3.Location = new System.Drawing.Point(139, 168);
            this.rbtDemoData3.Name = "rbtDemoData3";
            this.rbtDemoData3.Size = new System.Drawing.Size(194, 17);
            this.rbtDemoData3.TabIndex = 18;
            this.rbtDemoData3.Text = "Demo Data 3 (Bank,Client,Account)";
            this.rbtDemoData3.UseVisualStyleBackColor = true;
            this.rbtDemoData3.Click += new System.EventHandler(this.rbtDemoData3_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(63, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Data Source:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDatabaseConnection);
            this.groupBox1.Controls.Add(this.rbtDataSourceDatabase);
            this.groupBox1.Controls.Add(this.rbtDataSourceApplication);
            this.groupBox1.Location = new System.Drawing.Point(139, 191);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(269, 66);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            // 
            // btnDatabaseConnection
            // 
            this.btnDatabaseConnection.Location = new System.Drawing.Point(188, 40);
            this.btnDatabaseConnection.Name = "btnDatabaseConnection";
            this.btnDatabaseConnection.Size = new System.Drawing.Size(67, 20);
            this.btnDatabaseConnection.TabIndex = 23;
            this.btnDatabaseConnection.Text = "Set up..";
            this.btnDatabaseConnection.UseVisualStyleBackColor = true;
            this.btnDatabaseConnection.Click += new System.EventHandler(this.btnDatabaseConnection_Click);
            // 
            // rbtDataSourceDatabase
            // 
            this.rbtDataSourceDatabase.AutoSize = true;
            this.rbtDataSourceDatabase.Location = new System.Drawing.Point(6, 39);
            this.rbtDataSourceDatabase.Name = "rbtDataSourceDatabase";
            this.rbtDataSourceDatabase.Size = new System.Drawing.Size(148, 17);
            this.rbtDataSourceDatabase.TabIndex = 22;
            this.rbtDataSourceDatabase.Text = "Data taken from database";
            this.rbtDataSourceDatabase.UseVisualStyleBackColor = true;
            // 
            // rbtDataSourceApplication
            // 
            this.rbtDataSourceApplication.AutoSize = true;
            this.rbtDataSourceApplication.Checked = true;
            this.rbtDataSourceApplication.Location = new System.Drawing.Point(6, 17);
            this.rbtDataSourceApplication.Name = "rbtDataSourceApplication";
            this.rbtDataSourceApplication.Size = new System.Drawing.Size(196, 17);
            this.rbtDataSourceApplication.TabIndex = 21;
            this.rbtDataSourceApplication.TabStop = true;
            this.rbtDataSourceApplication.Text = "Random data taken from application";
            this.rbtDataSourceApplication.UseVisualStyleBackColor = true;
            // 
            // txtSerial
            // 
            this.txtSerial.Location = new System.Drawing.Point(139, 98);
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.Size = new System.Drawing.Size(198, 20);
            this.txtSerial.TabIndex = 22;
            // 
            // lblSerial
            // 
            this.lblSerial.AutoSize = true;
            this.lblSerial.Location = new System.Drawing.Point(57, 101);
            this.lblSerial.Name = "lblSerial";
            this.lblSerial.Size = new System.Drawing.Size(76, 13);
            this.lblSerial.TabIndex = 21;
            this.lblSerial.Text = "Serial Number:";
            // 
            // btnCallbackGeneration
            // 
            this.btnCallbackGeneration.Location = new System.Drawing.Point(296, 289);
            this.btnCallbackGeneration.Name = "btnCallbackGeneration";
            this.btnCallbackGeneration.Size = new System.Drawing.Size(112, 23);
            this.btnCallbackGeneration.TabIndex = 23;
            this.btnCallbackGeneration.Text = "Callback gen.";
            this.btnCallbackGeneration.UseVisualStyleBackColor = true;
            this.btnCallbackGeneration.Click += new System.EventHandler(this.btnCallbackGeneration_Click);
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.Location = new System.Drawing.Point(139, 72);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new System.Drawing.Size(198, 20);
            this.txtCompanyName.TabIndex = 25;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(57, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Company:";
            // 
            // MainTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 490);
            this.Controls.Add(this.txtCompanyName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCallbackGeneration);
            this.Controls.Add(this.txtSerial);
            this.Controls.Add(this.lblSerial);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.rbtDemoData3);
            this.Controls.Add(this.rbtDemoData2);
            this.Controls.Add(this.rbtDemoData1);
            this.Controls.Add(this.edtNumberOfItems);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tabDataDesc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnBrowsePdfOutput);
            this.Controls.Add(this.txtPDF);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtTemplateFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGenerate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainTestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test PdfReporter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabDataDesc.ResumeLayout(false);
            this.tabData1.ResumeLayout(false);
            this.tabData1.PerformLayout();
            this.tabData2.ResumeLayout(false);
            this.tabData2.PerformLayout();
            this.tabData3.ResumeLayout(false);
            this.tabData3.PerformLayout();
            this.tabResults.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtTemplateFile;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.OpenFileDialog OD;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtPDF;
		private System.Windows.Forms.Button btnBrowsePdfOutput;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
		private System.Windows.Forms.SaveFileDialog SD;
        private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TabControl tabDataDesc;
		private System.Windows.Forms.TabPage tabData1;
		private System.Windows.Forms.TabPage tabData2;
		private System.Windows.Forms.TabPage tabData3;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox edtNumberOfItems;
		private System.Windows.Forms.TabPage tabResults;
		private System.Windows.Forms.ListBox lbxResults;
        private System.Windows.Forms.RadioButton rbtDemoData1;
        private System.Windows.Forms.RadioButton rbtDemoData2;
        private System.Windows.Forms.RadioButton rbtDemoData3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtDataSourceDatabase;
        private System.Windows.Forms.RadioButton rbtDataSourceApplication;
        private System.Windows.Forms.Button btnDatabaseConnection;
        private System.Windows.Forms.TextBox txtSerial;
        private System.Windows.Forms.Label lblSerial;
        private System.Windows.Forms.Button btnCallbackGeneration;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.Label label6;


	}
}

