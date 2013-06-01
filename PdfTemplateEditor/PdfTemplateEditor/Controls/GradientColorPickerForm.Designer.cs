namespace AxiomCoders.PdfTemplateEditor.Controls
{
    partial class GradientColorPickerForm
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
            this.rbtnLinear = new System.Windows.Forms.RadioButton();
            this.rbtnRadial = new System.Windows.Forms.RadioButton();
            this.grpGradientType = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.grpColors = new System.Windows.Forms.GroupBox();
            this.btnColor2 = new System.Windows.Forms.Button();
            this.pnlColor2 = new System.Windows.Forms.Panel();
            this.btnColor1 = new System.Windows.Forms.Button();
            this.pnlColor1 = new System.Windows.Forms.Panel();
            this.btnHelp = new System.Windows.Forms.Button();
            this.pnlPreview = new AxiomCoders.PdfTemplateEditor.Controls.GradientPanel();
            this.grpGradientType.SuspendLayout();
            this.grpColors.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbtnLinear
            // 
            this.rbtnLinear.AutoSize = true;
            this.rbtnLinear.Checked = true;
            this.rbtnLinear.Location = new System.Drawing.Point(15, 21);
            this.rbtnLinear.Name = "rbtnLinear";
            this.rbtnLinear.Size = new System.Drawing.Size(95, 17);
            this.rbtnLinear.TabIndex = 0;
            this.rbtnLinear.TabStop = true;
            this.rbtnLinear.Text = "Linear gradient";
            this.rbtnLinear.UseVisualStyleBackColor = true;
            // 
            // rbtnRadial
            // 
            this.rbtnRadial.AutoSize = true;
            this.rbtnRadial.Enabled = false;
            this.rbtnRadial.Location = new System.Drawing.Point(15, 45);
            this.rbtnRadial.Name = "rbtnRadial";
            this.rbtnRadial.Size = new System.Drawing.Size(96, 17);
            this.rbtnRadial.TabIndex = 1;
            this.rbtnRadial.Text = "Radial gradient";
            this.rbtnRadial.UseVisualStyleBackColor = true;
            // 
            // grpGradientType
            // 
            this.grpGradientType.Controls.Add(this.rbtnLinear);
            this.grpGradientType.Controls.Add(this.rbtnRadial);
            this.grpGradientType.Location = new System.Drawing.Point(12, 21);
            this.grpGradientType.Name = "grpGradientType";
            this.grpGradientType.Size = new System.Drawing.Size(239, 74);
            this.grpGradientType.TabIndex = 0;
            this.grpGradientType.TabStop = false;
            this.grpGradientType.Text = "Gradient type";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(380, 216);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(299, 216);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // grpColors
            // 
            this.grpColors.Controls.Add(this.btnColor2);
            this.grpColors.Controls.Add(this.pnlColor2);
            this.grpColors.Controls.Add(this.btnColor1);
            this.grpColors.Controls.Add(this.pnlColor1);
            this.grpColors.Location = new System.Drawing.Point(13, 103);
            this.grpColors.Name = "grpColors";
            this.grpColors.Size = new System.Drawing.Size(238, 101);
            this.grpColors.TabIndex = 1;
            this.grpColors.TabStop = false;
            this.grpColors.Text = "Gradient colors";
            // 
            // btnColor2
            // 
            this.btnColor2.Location = new System.Drawing.Point(138, 68);
            this.btnColor2.Name = "btnColor2";
            this.btnColor2.Size = new System.Drawing.Size(32, 21);
            this.btnColor2.TabIndex = 3;
            this.btnColor2.Text = "...";
            this.btnColor2.UseVisualStyleBackColor = true;
            this.btnColor2.Click += new System.EventHandler(this.btnColor2_Click);
            // 
            // pnlColor2
            // 
            this.pnlColor2.BackColor = System.Drawing.Color.Black;
            this.pnlColor2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlColor2.Location = new System.Drawing.Point(15, 69);
            this.pnlColor2.Name = "pnlColor2";
            this.pnlColor2.Size = new System.Drawing.Size(117, 19);
            this.pnlColor2.TabIndex = 2;
            // 
            // btnColor1
            // 
            this.btnColor1.Location = new System.Drawing.Point(138, 33);
            this.btnColor1.Name = "btnColor1";
            this.btnColor1.Size = new System.Drawing.Size(32, 21);
            this.btnColor1.TabIndex = 1;
            this.btnColor1.Text = "...";
            this.btnColor1.UseVisualStyleBackColor = true;
            this.btnColor1.Click += new System.EventHandler(this.btnColor1_Click);
            // 
            // pnlColor1
            // 
            this.pnlColor1.BackColor = System.Drawing.Color.White;
            this.pnlColor1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlColor1.Location = new System.Drawing.Point(15, 34);
            this.pnlColor1.Name = "pnlColor1";
            this.pnlColor1.Size = new System.Drawing.Size(117, 19);
            this.pnlColor1.TabIndex = 0;
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHelp.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnHelp.Location = new System.Drawing.Point(13, 216);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 5;
            this.btnHelp.Text = "&Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // pnlPreview
            // 
            this.pnlPreview.BlendPosition1 = 0F;
            this.pnlPreview.BlendPosition2 = 0F;
            this.pnlPreview.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pnlPreview.GradientType = AxiomCoders.PdfTemplateEditor.EditorItems.GradientType.Linear;
            this.pnlPreview.Location = new System.Drawing.Point(266, 21);
            this.pnlPreview.Name = "pnlPreview";
            this.pnlPreview.Size = new System.Drawing.Size(188, 188);
            this.pnlPreview.TabIndex = 2;
            // 
            // GradientColorPickerForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(467, 251);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.grpColors);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.grpGradientType);
            this.Controls.Add(this.pnlPreview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GradientColorPickerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gradient Definitions";
            this.grpGradientType.ResumeLayout(false);
            this.grpGradientType.PerformLayout();
            this.grpColors.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rbtnLinear;
        private System.Windows.Forms.RadioButton rbtnRadial;
        private GradientPanel pnlPreview;
        private System.Windows.Forms.GroupBox grpGradientType;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox grpColors;
        private System.Windows.Forms.Button btnColor2;
        private System.Windows.Forms.Panel pnlColor2;
        private System.Windows.Forms.Button btnColor1;
        private System.Windows.Forms.Panel pnlColor1;
		private System.Windows.Forms.Button btnHelp;
    }
}