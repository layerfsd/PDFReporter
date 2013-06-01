namespace AxiomCoders.PdfTemplateEditor.Forms
{
	partial class ReportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportForm));
            this.pnlTopRuler = new System.Windows.Forms.Panel();
            this.pnlLeftRuler = new System.Windows.Forms.Panel();
            this.pnlRulerGuide = new System.Windows.Forms.Panel();
            this.ScrollBarVertical = new System.Windows.Forms.VScrollBar();
            this.ScrollBarHorizontal = new System.Windows.Forms.HScrollBar();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.RightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ResetRotation = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.RightClickMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopRuler
            // 
            this.pnlTopRuler.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTopRuler.BackColor = System.Drawing.Color.White;
            this.pnlTopRuler.Location = new System.Drawing.Point(25, 0);
            this.pnlTopRuler.Name = "pnlTopRuler";
            this.pnlTopRuler.Size = new System.Drawing.Size(441, 25);
            this.pnlTopRuler.TabIndex = 0;
            this.pnlTopRuler.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlTopRuler_Paint);
            // 
            // pnlLeftRuler
            // 
            this.pnlLeftRuler.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlLeftRuler.BackColor = System.Drawing.Color.White;
            this.pnlLeftRuler.Location = new System.Drawing.Point(0, 25);
            this.pnlLeftRuler.Name = "pnlLeftRuler";
            this.pnlLeftRuler.Size = new System.Drawing.Size(25, 424);
            this.pnlLeftRuler.TabIndex = 1;
            this.pnlLeftRuler.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlLeftRuler_Paint);
            // 
            // pnlRulerGuide
            // 
            this.pnlRulerGuide.BackColor = System.Drawing.Color.White;
            this.pnlRulerGuide.Location = new System.Drawing.Point(0, 0);
            this.pnlRulerGuide.Name = "pnlRulerGuide";
            this.pnlRulerGuide.Size = new System.Drawing.Size(25, 25);
            this.pnlRulerGuide.TabIndex = 2;
            this.pnlRulerGuide.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlRulerGuide_Paint);
            // 
            // ScrollBarVertical
            // 
            this.ScrollBarVertical.Dock = System.Windows.Forms.DockStyle.Right;
            this.ScrollBarVertical.Location = new System.Drawing.Point(467, 0);
            this.ScrollBarVertical.Name = "ScrollBarVertical";
            this.ScrollBarVertical.Size = new System.Drawing.Size(17, 447);
            this.ScrollBarVertical.TabIndex = 3;
            this.ScrollBarVertical.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ScrollBarVertical_Scroll);
            // 
            // ScrollBarHorizontal
            // 
            this.ScrollBarHorizontal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ScrollBarHorizontal.Location = new System.Drawing.Point(0, 447);
            this.ScrollBarHorizontal.Name = "ScrollBarHorizontal";
            this.ScrollBarHorizontal.Size = new System.Drawing.Size(484, 17);
            this.ScrollBarHorizontal.TabIndex = 4;
            this.ScrollBarHorizontal.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ScrollBarHorizontal_Scroll);
            // 
            // pnlMain
            // 
            this.pnlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMain.BackColor = System.Drawing.Color.Transparent;
            this.pnlMain.Location = new System.Drawing.Point(25, 25);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(442, 424);
            this.pnlMain.TabIndex = 5;
            this.pnlMain.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.pnlMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlMain_MouseMove);
            this.pnlMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlMain_MouseClick);
            this.pnlMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlMain_MouseDown);
            this.pnlMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlMain_MouseUp);
            // 
            // RightClickMenu
            // 
            this.RightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ResetRotation});
            this.RightClickMenu.Name = "RightClickMenu";
            this.RightClickMenu.Size = new System.Drawing.Size(148, 26);
            // 
            // ResetRotation
            // 
            this.ResetRotation.Name = "ResetRotation";
            this.ResetRotation.Size = new System.Drawing.Size(147, 22);
            this.ResetRotation.Text = "Reset rotation";
            this.ResetRotation.ToolTipText = "This resets object rotation on 0";
            this.ResetRotation.Click += new System.EventHandler(this.ResetRotation_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipTitle = "Object helpers";
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(484, 464);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.ScrollBarVertical);
            this.Controls.Add(this.ScrollBarHorizontal);
            this.Controls.Add(this.pnlLeftRuler);
            this.Controls.Add(this.pnlRulerGuide);
            this.Controls.Add(this.pnlTopRuler);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ReportForm";
            this.Text = "ReportForm";
            this.Load += new System.EventHandler(this.ReportForm_Load);
            this.ResizeBegin += new System.EventHandler(this.ReportForm_ResizeBegin);
            this.Shown += new System.EventHandler(this.ReportForm_Shown);
            this.Activated += new System.EventHandler(this.ReportForm_Activated);
            this.Click += new System.EventHandler(this.ReportForm_Click);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ReportForm_KeyUp);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReportForm_FormClosing);
            this.Resize += new System.EventHandler(this.ReportForm_Resize);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ReportForm_KeyDown);
            this.RightClickMenu.ResumeLayout(false);
            this.ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.Panel pnlTopRuler;
		private System.Windows.Forms.Panel pnlLeftRuler;
		private System.Windows.Forms.Panel pnlRulerGuide;
		private System.Windows.Forms.VScrollBar ScrollBarVertical;
		private System.Windows.Forms.HScrollBar ScrollBarHorizontal;
		private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip RightClickMenu;
		private System.Windows.Forms.ToolStripMenuItem ResetRotation;

	}
}