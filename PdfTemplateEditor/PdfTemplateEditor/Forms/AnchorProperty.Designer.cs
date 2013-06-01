namespace AxiomCoders.PdfTemplateEditor.Forms
{
    partial class AnchorProperty
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnNone = new System.Windows.Forms.Button();
            this.btnAll = new System.Windows.Forms.Button();
            this.chkBoxRight = new System.Windows.Forms.CheckBox();
            this.chkBoxLeft = new System.Windows.Forms.CheckBox();
            this.chkBoxBottom = new System.Windows.Forms.CheckBox();
            this.chkBoxTop = new System.Windows.Forms.CheckBox();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnNone);
            this.groupBox1.Controls.Add(this.btnAll);
            this.groupBox1.Controls.Add(this.chkBoxRight);
            this.groupBox1.Controls.Add(this.chkBoxLeft);
            this.groupBox1.Controls.Add(this.chkBoxBottom);
            this.groupBox1.Controls.Add(this.chkBoxTop);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(284, 112);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Anchor";
            // 
            // btnNone
            // 
            this.btnNone.Location = new System.Drawing.Point(203, 67);
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(75, 23);
            this.btnNone.TabIndex = 5;
            this.btnNone.Text = "None";
            this.btnNone.UseVisualStyleBackColor = true;
            this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(203, 31);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(75, 23);
            this.btnAll.TabIndex = 4;
            this.btnAll.Text = "All";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // chkBoxRight
            // 
            this.chkBoxRight.AutoSize = true;
            this.chkBoxRight.Location = new System.Drawing.Point(116, 73);
            this.chkBoxRight.Name = "chkBoxRight";
            this.chkBoxRight.Size = new System.Drawing.Size(51, 17);
            this.chkBoxRight.TabIndex = 3;
            this.chkBoxRight.Text = "Right";
            this.chkBoxRight.UseVisualStyleBackColor = true;
            this.chkBoxRight.CheckedChanged += new System.EventHandler(this.chkBoxRight_CheckedChanged);
            // 
            // chkBoxLeft
            // 
            this.chkBoxLeft.AutoSize = true;
            this.chkBoxLeft.Checked = true;
            this.chkBoxLeft.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxLeft.Location = new System.Drawing.Point(17, 73);
            this.chkBoxLeft.Name = "chkBoxLeft";
            this.chkBoxLeft.Size = new System.Drawing.Size(44, 17);
            this.chkBoxLeft.TabIndex = 2;
            this.chkBoxLeft.Text = "Left";
            this.chkBoxLeft.UseVisualStyleBackColor = true;
            this.chkBoxLeft.CheckedChanged += new System.EventHandler(this.chkBoxLeft_CheckedChanged);
            // 
            // chkBoxBottom
            // 
            this.chkBoxBottom.AutoSize = true;
            this.chkBoxBottom.Location = new System.Drawing.Point(116, 37);
            this.chkBoxBottom.Name = "chkBoxBottom";
            this.chkBoxBottom.Size = new System.Drawing.Size(59, 17);
            this.chkBoxBottom.TabIndex = 1;
            this.chkBoxBottom.Text = "Bottom";
            this.chkBoxBottom.UseVisualStyleBackColor = true;
            this.chkBoxBottom.CheckedChanged += new System.EventHandler(this.chkBoxBottom_CheckedChanged);
            // 
            // chkBoxTop
            // 
            this.chkBoxTop.AutoSize = true;
            this.chkBoxTop.Checked = true;
            this.chkBoxTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxTop.Location = new System.Drawing.Point(17, 37);
            this.chkBoxTop.Name = "chkBoxTop";
            this.chkBoxTop.Size = new System.Drawing.Size(45, 17);
            this.chkBoxTop.TabIndex = 0;
            this.chkBoxTop.Text = "Top";
            this.chkBoxTop.UseVisualStyleBackColor = true;
            this.chkBoxTop.CheckedChanged += new System.EventHandler(this.chkBoxTop_CheckedChanged);
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHelp.Location = new System.Drawing.Point(12, 133);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 3;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(140, 133);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(221, 133);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AnchorProperty
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(308, 168);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AnchorProperty";
            this.Text = "Anchor Property";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkBoxRight;
        private System.Windows.Forms.CheckBox chkBoxLeft;
        private System.Windows.Forms.CheckBox chkBoxBottom;
        private System.Windows.Forms.CheckBox chkBoxTop;
        private System.Windows.Forms.Button btnNone;
        private System.Windows.Forms.Button btnAll;
    }
}