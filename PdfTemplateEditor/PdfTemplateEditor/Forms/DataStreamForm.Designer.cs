namespace AxiomCoders.PdfTemplateEditor.Forms
{
    partial class DataStreamForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataStreamForm));
            this.DataStreamControls = new System.Windows.Forms.ToolStrip();
            this.AddDataStream = new System.Windows.Forms.ToolStripButton();
            this.RemoveDataStream = new System.Windows.Forms.ToolStripButton();
            this.RenameDataStream = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.AddColumn = new System.Windows.Forms.ToolStripButton();
            this.RemoveColumn = new System.Windows.Forms.ToolStripButton();
            this.RenameColumn = new System.Windows.Forms.ToolStripButton();
            this.ColumnTypeDrop = new System.Windows.Forms.ToolStripSplitButton();
            this.StringType = new System.Windows.Forms.ToolStripMenuItem();
            this.IntType = new System.Windows.Forms.ToolStripMenuItem();
            this.FloatType = new System.Windows.Forms.ToolStripMenuItem();
            this.DataStreamColections = new System.Windows.Forms.TreeView();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.DataStreamControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataStreamControls
            // 
            this.DataStreamControls.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddDataStream,
            this.RemoveDataStream,
            this.RenameDataStream,
            this.toolStripSeparator1,
            this.AddColumn,
            this.RemoveColumn,
            this.RenameColumn,
            this.ColumnTypeDrop});
            this.DataStreamControls.Location = new System.Drawing.Point(0, 0);
            this.DataStreamControls.Name = "DataStreamControls";
            this.DataStreamControls.Size = new System.Drawing.Size(276, 25);
            this.DataStreamControls.TabIndex = 0;
            this.DataStreamControls.Text = "toolStrip1";
            // 
            // AddDataStream
            // 
            this.AddDataStream.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddDataStream.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.AddDataStream;
            this.AddDataStream.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddDataStream.Name = "AddDataStream";
            this.AddDataStream.Size = new System.Drawing.Size(23, 22);
            this.AddDataStream.Text = "Add Data Stream";
            this.AddDataStream.Click += new System.EventHandler(this.AddDataStream_Click);
            // 
            // RemoveDataStream
            // 
            this.RemoveDataStream.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RemoveDataStream.Enabled = false;
            this.RemoveDataStream.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.RemoveDataStream;
            this.RemoveDataStream.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RemoveDataStream.Name = "RemoveDataStream";
            this.RemoveDataStream.Size = new System.Drawing.Size(23, 22);
            this.RemoveDataStream.Text = "Remove Data Stream";
            this.RemoveDataStream.Click += new System.EventHandler(this.RemoveDataStream_Click);
            // 
            // RenameDataStream
            // 
            this.RenameDataStream.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RenameDataStream.Enabled = false;
            this.RenameDataStream.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.RenameDataStream;
            this.RenameDataStream.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RenameDataStream.Name = "RenameDataStream";
            this.RenameDataStream.Size = new System.Drawing.Size(23, 22);
            this.RenameDataStream.Text = "Rename Data Stream";
            this.RenameDataStream.Click += new System.EventHandler(this.RenameDataStream_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // AddColumn
            // 
            this.AddColumn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddColumn.Enabled = false;
            this.AddColumn.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.AddColumn;
            this.AddColumn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddColumn.Name = "AddColumn";
            this.AddColumn.Size = new System.Drawing.Size(23, 22);
            this.AddColumn.Text = "Add Column";
            this.AddColumn.Click += new System.EventHandler(this.AddColumn_Click);
            // 
            // RemoveColumn
            // 
            this.RemoveColumn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RemoveColumn.Enabled = false;
            this.RemoveColumn.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.RemoveColumn;
            this.RemoveColumn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RemoveColumn.Name = "RemoveColumn";
            this.RemoveColumn.Size = new System.Drawing.Size(23, 22);
            this.RemoveColumn.Text = "Remove Column";
            this.RemoveColumn.Click += new System.EventHandler(this.RemoveColumn_Click);
            // 
            // RenameColumn
            // 
            this.RenameColumn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RenameColumn.Enabled = false;
            this.RenameColumn.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.RenameColumn;
            this.RenameColumn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RenameColumn.Name = "RenameColumn";
            this.RenameColumn.Size = new System.Drawing.Size(23, 22);
            this.RenameColumn.Text = "Rename Column";
            this.RenameColumn.Click += new System.EventHandler(this.RenameColumn_Click);
            // 
            // ColumnTypeDrop
            // 
            this.ColumnTypeDrop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ColumnTypeDrop.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StringType,
            this.IntType,
            this.FloatType});
            this.ColumnTypeDrop.Enabled = false;
            this.ColumnTypeDrop.Image = ((System.Drawing.Image)(resources.GetObject("ColumnTypeDrop.Image")));
            this.ColumnTypeDrop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ColumnTypeDrop.Name = "ColumnTypeDrop";
            this.ColumnTypeDrop.Size = new System.Drawing.Size(95, 22);
            this.ColumnTypeDrop.Text = "Column Type";
            this.ColumnTypeDrop.ToolTipText = "Select type, because items are created with default type - String";
            this.ColumnTypeDrop.Visible = false;
            // 
            // StringType
            // 
            this.StringType.Name = "StringType";
            this.StringType.Size = new System.Drawing.Size(105, 22);
            this.StringType.Text = "String";
            this.StringType.Click += new System.EventHandler(this.StringType_Click);
            // 
            // IntType
            // 
            this.IntType.Name = "IntType";
            this.IntType.Size = new System.Drawing.Size(105, 22);
            this.IntType.Text = "Int";
            this.IntType.Click += new System.EventHandler(this.IntType_Click);
            // 
            // FloatType
            // 
            this.FloatType.Name = "FloatType";
            this.FloatType.Size = new System.Drawing.Size(105, 22);
            this.FloatType.Text = "Float";
            this.FloatType.Click += new System.EventHandler(this.FloatType_Click);
            // 
            // DataStreamColections
            // 
            this.DataStreamColections.HideSelection = false;
            this.DataStreamColections.Location = new System.Drawing.Point(12, 28);
            this.DataStreamColections.Name = "DataStreamColections";
            this.DataStreamColections.Size = new System.Drawing.Size(250, 312);
            this.DataStreamColections.TabIndex = 1;
            this.DataStreamColections.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.DataStreamColections_AfterSelect);
            // 
            // btnOk
            // 
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOk.Location = new System.Drawing.Point(104, 347);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(188, 347);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnHelp.Location = new System.Drawing.Point(13, 347);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 4;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // DataStreamForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(276, 382);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.DataStreamColections);
            this.Controls.Add(this.DataStreamControls);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataStreamForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Streams";
            this.Shown += new System.EventHandler(this.DataStreamForm_Shown);
            this.DataStreamControls.ResumeLayout(false);
            this.DataStreamControls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip DataStreamControls;
        private System.Windows.Forms.ToolStripButton AddDataStream;
        private System.Windows.Forms.ToolStripButton RemoveDataStream;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton AddColumn;
        private System.Windows.Forms.ToolStripButton RemoveColumn;
        private System.Windows.Forms.TreeView DataStreamColections;
        private System.Windows.Forms.ToolStripSplitButton ColumnTypeDrop;
        private System.Windows.Forms.ToolStripMenuItem StringType;
        private System.Windows.Forms.ToolStripMenuItem IntType;
        private System.Windows.Forms.ToolStripMenuItem FloatType;
        private System.Windows.Forms.ToolStripButton RenameDataStream;
        private System.Windows.Forms.ToolStripButton RenameColumn;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnHelp;
    }
}