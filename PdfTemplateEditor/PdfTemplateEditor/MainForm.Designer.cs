namespace AxiomCoders.PdfTemplateEditor
{
	partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pnlProperties = new System.Windows.Forms.Panel();
            this.PropertySplitContainer = new System.Windows.Forms.SplitContainer();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.tvItems = new System.Windows.Forms.TreeView();
            this.cmbObjects = new System.Windows.Forms.ComboBox();
            this.AutoSaveTimer = new System.Windows.Forms.Timer(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.mainToolContainer = new System.Windows.Forms.ToolStripContainer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miNewProject = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenProject = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miSave = new System.Windows.Forms.ToolStripMenuItem();
            this.miSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.miClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.editProjectInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.miUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.miRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.miCut = new System.Windows.Forms.ToolStripMenuItem();
            this.miCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.miPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.miPreferences = new System.Windows.Forms.ToolStripMenuItem();
            this.miGeneral = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.miUnitsRulers = new System.Windows.Forms.ToolStripMenuItem();
            this.miGuidesGrids = new System.Windows.Forms.ToolStripMenuItem();
            this.miTools = new System.Windows.Forms.ToolStripMenuItem();
            this.miDataStreams = new System.Windows.Forms.ToolStripMenuItem();
            this.miView = new System.Windows.Forms.ToolStripMenuItem();
            this.miShowGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.miShowRulers = new System.Windows.Forms.ToolStripMenuItem();
            this.miShowBalloonBorders = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.themesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.professionalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.pdfReportsHelpContentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pdfReportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.axiomCodersOnlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.axiomCodersWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pdfReportsWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pdfReportsSuportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.purchaseALicenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.releaseNotesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.miAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.tbbNewProject = new System.Windows.Forms.ToolStripButton();
            this.tbbOpenProject = new System.Windows.Forms.ToolStripButton();
            this.tbbSaveProject = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbCut = new System.Windows.Forms.ToolStripButton();
            this.tbbCopy = new System.Windows.Forms.ToolStripButton();
            this.tbbPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbUndo = new System.Windows.Forms.ToolStripButton();
            this.tbbRedo = new System.Windows.Forms.ToolStripButton();
            this.AlignStrip = new System.Windows.Forms.ToolStrip();
            this.toolSnapToGrid = new System.Windows.Forms.ToolStripButton();
            this.toolLeftAlign = new System.Windows.Forms.ToolStripButton();
            this.toolAlignCentarV = new System.Windows.Forms.ToolStripButton();
            this.toolAlignRight = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolAlignTop = new System.Windows.Forms.ToolStripButton();
            this.toolAlignCentarH = new System.Windows.Forms.ToolStripButton();
            this.toolAlignBottom = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolSameWidth = new System.Windows.Forms.ToolStripButton();
            this.toolSameHeight = new System.Windows.Forms.ToolStripButton();
            this.toolSameSize = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolSameHSpacing = new System.Windows.Forms.ToolStripButton();
            this.toolncHSpacing = new System.Windows.Forms.ToolStripButton();
            this.toolDecHSpacing = new System.Windows.Forms.ToolStripButton();
            this.toolNoHSpacing = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolSameVSpacing = new System.Windows.Forms.ToolStripButton();
            this.toolIncVSpacing = new System.Windows.Forms.ToolStripButton();
            this.toolDecVSpacing = new System.Windows.Forms.ToolStripButton();
            this.toolNoVSpacing = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbbHeight2 = new System.Windows.Forms.ToolStripButton();
            this.tbbHeight3 = new System.Windows.Forms.ToolStripButton();
            this.tbbHeight4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbWidth2 = new System.Windows.Forms.ToolStripButton();
            this.tbbWidth3 = new System.Windows.Forms.ToolStripButton();
            this.tbbWidth4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbSendToBack = new System.Windows.Forms.ToolStripButton();
            this.tbbBringToFront = new System.Windows.Forms.ToolStripButton();
            this.tbbStripOne = new System.Windows.Forms.ToolStrip();
            this.tbbArrowItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbZoom = new System.Windows.Forms.ToolStripSplitButton();
            this.miZoom50 = new System.Windows.Forms.ToolStripMenuItem();
            this.miZoom100 = new System.Windows.Forms.ToolStripMenuItem();
            this.miZoom150 = new System.Windows.Forms.ToolStripMenuItem();
            this.miZoom200 = new System.Windows.Forms.ToolStripMenuItem();
            this.tbbPreviewButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton11 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
            this.pnlProperties.SuspendLayout();
            this.PropertySplitContainer.Panel1.SuspendLayout();
            this.PropertySplitContainer.Panel2.SuspendLayout();
            this.PropertySplitContainer.SuspendLayout();
            this.mainToolContainer.TopToolStripPanel.SuspendLayout();
            this.mainToolContainer.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.AlignStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tbbStripOne.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlProperties
            // 
            this.pnlProperties.Controls.Add(this.PropertySplitContainer);
            this.pnlProperties.Controls.Add(this.cmbObjects);
            this.pnlProperties.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlProperties.Location = new System.Drawing.Point(898, 133);
            this.pnlProperties.Name = "pnlProperties";
            this.pnlProperties.Size = new System.Drawing.Size(292, 461);
            this.pnlProperties.TabIndex = 2;
            // 
            // PropertySplitContainer
            // 
            this.PropertySplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertySplitContainer.Location = new System.Drawing.Point(0, 21);
            this.PropertySplitContainer.Name = "PropertySplitContainer";
            this.PropertySplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // PropertySplitContainer.Panel1
            // 
            this.PropertySplitContainer.Panel1.Controls.Add(this.propertyGrid);
            // 
            // PropertySplitContainer.Panel2
            // 
            this.PropertySplitContainer.Panel2.Controls.Add(this.tvItems);
            this.PropertySplitContainer.Size = new System.Drawing.Size(292, 440);
            this.PropertySplitContainer.SplitterDistance = 233;
            this.PropertySplitContainer.TabIndex = 2;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(292, 233);
            this.propertyGrid.TabIndex = 1;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // tvItems
            // 
            this.tvItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvItems.HideSelection = false;
            this.tvItems.Location = new System.Drawing.Point(0, 0);
            this.tvItems.Name = "tvItems";
            this.tvItems.Size = new System.Drawing.Size(292, 203);
            this.tvItems.TabIndex = 0;
            this.tvItems.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvItems_AfterSelect);
            this.tvItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvItems_KeyDown);
            // 
            // cmbObjects
            // 
            this.cmbObjects.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbObjects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbObjects.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbObjects.FormattingEnabled = true;
            this.cmbObjects.Location = new System.Drawing.Point(0, 0);
            this.cmbObjects.Name = "cmbObjects";
            this.cmbObjects.Size = new System.Drawing.Size(292, 21);
            this.cmbObjects.TabIndex = 1;
            this.cmbObjects.SelectedValueChanged += new System.EventHandler(this.cmbObjects_SelectedValueChanged);
            // 
            // AutoSaveTimer
            // 
            this.AutoSaveTimer.Enabled = true;
            this.AutoSaveTimer.Interval = 1000;
            this.AutoSaveTimer.Tick += new System.EventHandler(this.myTimer_Tick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "magnifying-glass-search-find.png");
            // 
            // mainToolContainer
            // 
            this.mainToolContainer.BottomToolStripPanelVisible = false;
            // 
            // mainToolContainer.ContentPanel
            // 
            this.mainToolContainer.ContentPanel.BackColor = System.Drawing.SystemColors.Control;
            this.mainToolContainer.ContentPanel.Size = new System.Drawing.Size(1190, 9);
            this.mainToolContainer.ContentPanel.Visible = false;
            this.mainToolContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.mainToolContainer.LeftToolStripPanelVisible = false;
            this.mainToolContainer.Location = new System.Drawing.Point(0, 0);
            this.mainToolContainer.Name = "mainToolContainer";
            this.mainToolContainer.RightToolStripPanelVisible = false;
            this.mainToolContainer.Size = new System.Drawing.Size(1190, 133);
            this.mainToolContainer.TabIndex = 11;
            this.mainToolContainer.Text = "toolStripContainer1";
            // 
            // mainToolContainer.TopToolStripPanel
            // 
            this.mainToolContainer.TopToolStripPanel.BackColor = System.Drawing.Color.Transparent;
            this.mainToolContainer.TopToolStripPanel.Controls.Add(this.menuStrip1);
            this.mainToolContainer.TopToolStripPanel.Controls.Add(this.toolStrip2);
            this.mainToolContainer.TopToolStripPanel.Controls.Add(this.AlignStrip);
            this.mainToolContainer.TopToolStripPanel.Controls.Add(this.toolStrip1);
            this.mainToolContainer.TopToolStripPanel.Controls.Add(this.tbbStripOne);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miEdit,
            this.miTools,
            this.miView,
            this.miHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1190, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miNewProject,
            this.miOpenProject,
            this.toolStripSeparator2,
            this.miSave,
            this.miSaveAs,
            this.miClose,
            this.toolStripMenuItem1,
            this.editProjectInformationToolStripMenuItem,
            this.toolStripMenuItem2,
            this.miExit});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(37, 20);
            this.miFile.Text = "&File";
            // 
            // miNewProject
            // 
            this.miNewProject.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.NewProject;
            this.miNewProject.Name = "miNewProject";
            this.miNewProject.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.miNewProject.Size = new System.Drawing.Size(249, 22);
            this.miNewProject.Text = "&New Project...";
            this.miNewProject.Click += new System.EventHandler(this.miNewProject_Click);
            // 
            // miOpenProject
            // 
            this.miOpenProject.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Open;
            this.miOpenProject.Name = "miOpenProject";
            this.miOpenProject.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.miOpenProject.Size = new System.Drawing.Size(249, 22);
            this.miOpenProject.Text = "&Open Project...";
            this.miOpenProject.Click += new System.EventHandler(this.miOpenProject_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(246, 6);
            // 
            // miSave
            // 
            this.miSave.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.SaveAs;
            this.miSave.Name = "miSave";
            this.miSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.miSave.Size = new System.Drawing.Size(249, 22);
            this.miSave.Text = "&Save";
            this.miSave.Click += new System.EventHandler(this.miSave_Click);
            // 
            // miSaveAs
            // 
            this.miSaveAs.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Save;
            this.miSaveAs.Name = "miSaveAs";
            this.miSaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.S)));
            this.miSaveAs.Size = new System.Drawing.Size(249, 22);
            this.miSaveAs.Text = "Save &As...";
            this.miSaveAs.Click += new System.EventHandler(this.miSaveAs_Click);
            // 
            // miClose
            // 
            this.miClose.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Close;
            this.miClose.Name = "miClose";
            this.miClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.miClose.Size = new System.Drawing.Size(249, 22);
            this.miClose.Text = "&Close";
            this.miClose.Click += new System.EventHandler(this.miClose_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(246, 6);
            // 
            // editProjectInformationToolStripMenuItem
            // 
            this.editProjectInformationToolStripMenuItem.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.application_edit;
            this.editProjectInformationToolStripMenuItem.Name = "editProjectInformationToolStripMenuItem";
            this.editProjectInformationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.editProjectInformationToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.editProjectInformationToolStripMenuItem.Text = "&Edit Project Information...";
            this.editProjectInformationToolStripMenuItem.Click += new System.EventHandler(this.editProjectInformationToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(246, 6);
            // 
            // miExit
            // 
            this.miExit.Image = ((System.Drawing.Image)(resources.GetObject("miExit.Image")));
            this.miExit.Name = "miExit";
            this.miExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.miExit.Size = new System.Drawing.Size(249, 22);
            this.miExit.Text = "E&xit";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // miEdit
            // 
            this.miEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miUndo,
            this.miRedo,
            this.toolStripMenuItem3,
            this.miCut,
            this.miCopy,
            this.miPaste,
            this.toolStripSeparator11,
            this.miPreferences});
            this.miEdit.Name = "miEdit";
            this.miEdit.Size = new System.Drawing.Size(39, 20);
            this.miEdit.Text = "&Edit";
            // 
            // miUndo
            // 
            this.miUndo.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Undo;
            this.miUndo.Name = "miUndo";
            this.miUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.miUndo.Size = new System.Drawing.Size(147, 22);
            this.miUndo.Text = "&Undo";
            this.miUndo.Click += new System.EventHandler(this.miUndo_Click);
            // 
            // miRedo
            // 
            this.miRedo.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Redo;
            this.miRedo.Name = "miRedo";
            this.miRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.miRedo.Size = new System.Drawing.Size(147, 22);
            this.miRedo.Text = "&Redo";
            this.miRedo.Click += new System.EventHandler(this.miRedo_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(144, 6);
            // 
            // miCut
            // 
            this.miCut.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Cut;
            this.miCut.Name = "miCut";
            this.miCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.miCut.Size = new System.Drawing.Size(147, 22);
            this.miCut.Text = "Cu&t";
            this.miCut.Click += new System.EventHandler(this.miCut_Click);
            // 
            // miCopy
            // 
            this.miCopy.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Copy;
            this.miCopy.Name = "miCopy";
            this.miCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.miCopy.Size = new System.Drawing.Size(147, 22);
            this.miCopy.Text = "&Copy ";
            this.miCopy.Click += new System.EventHandler(this.miCopy_Click);
            // 
            // miPaste
            // 
            this.miPaste.Enabled = false;
            this.miPaste.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Paste;
            this.miPaste.Name = "miPaste";
            this.miPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.miPaste.Size = new System.Drawing.Size(147, 22);
            this.miPaste.Text = "&Paste";
            this.miPaste.Click += new System.EventHandler(this.miPaste_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(144, 6);
            // 
            // miPreferences
            // 
            this.miPreferences.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miGeneral,
            this.toolStripSeparator12,
            this.miUnitsRulers,
            this.miGuidesGrids});
            this.miPreferences.Enabled = false;
            this.miPreferences.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Preferences;
            this.miPreferences.Name = "miPreferences";
            this.miPreferences.Size = new System.Drawing.Size(147, 22);
            this.miPreferences.Text = "&Preferences";
            // 
            // miGeneral
            // 
            this.miGeneral.Name = "miGeneral";
            this.miGeneral.Size = new System.Drawing.Size(148, 22);
            this.miGeneral.Text = "&General";
            this.miGeneral.Click += new System.EventHandler(this.miGeneral_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(145, 6);
            // 
            // miUnitsRulers
            // 
            this.miUnitsRulers.Name = "miUnitsRulers";
            this.miUnitsRulers.Size = new System.Drawing.Size(148, 22);
            this.miUnitsRulers.Text = "&Units, Rulers...";
            this.miUnitsRulers.Click += new System.EventHandler(this.miUnitsRulers_Click);
            // 
            // miGuidesGrids
            // 
            this.miGuidesGrids.Name = "miGuidesGrids";
            this.miGuidesGrids.Size = new System.Drawing.Size(148, 22);
            this.miGuidesGrids.Text = "Guides, G&rid...";
            this.miGuidesGrids.Click += new System.EventHandler(this.miGuidesGrids_Click);
            // 
            // miTools
            // 
            this.miTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDataStreams});
            this.miTools.Name = "miTools";
            this.miTools.Size = new System.Drawing.Size(48, 20);
            this.miTools.Text = "&Tools";
            // 
            // miDataStreams
            // 
            this.miDataStreams.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.DataStream;
            this.miDataStreams.Name = "miDataStreams";
            this.miDataStreams.Size = new System.Drawing.Size(143, 22);
            this.miDataStreams.Text = "&Data Streams";
            this.miDataStreams.Click += new System.EventHandler(this.miDataStreams_Click);
            // 
            // miView
            // 
            this.miView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miShowGrid,
            this.miShowRulers,
            this.miShowBalloonBorders,
            this.toolStripSeparator18,
            this.themesToolStripMenuItem});
            this.miView.Name = "miView";
            this.miView.Size = new System.Drawing.Size(44, 20);
            this.miView.Text = "&View";
            // 
            // miShowGrid
            // 
            this.miShowGrid.CheckOnClick = true;
            this.miShowGrid.Name = "miShowGrid";
            this.miShowGrid.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.miShowGrid.Size = new System.Drawing.Size(230, 22);
            this.miShowGrid.Text = "Show &Grid";
            this.miShowGrid.Click += new System.EventHandler(this.miShowGrid_Click);
            // 
            // miShowRulers
            // 
            this.miShowRulers.CheckOnClick = true;
            this.miShowRulers.Enabled = false;
            this.miShowRulers.Name = "miShowRulers";
            this.miShowRulers.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.miShowRulers.Size = new System.Drawing.Size(230, 22);
            this.miShowRulers.Text = "Show &Ruler";
            this.miShowRulers.Click += new System.EventHandler(this.miShowRulers_Click);
            // 
            // miShowBalloonBorders
            // 
            this.miShowBalloonBorders.CheckOnClick = true;
            this.miShowBalloonBorders.Name = "miShowBalloonBorders";
            this.miShowBalloonBorders.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.miShowBalloonBorders.Size = new System.Drawing.Size(230, 22);
            this.miShowBalloonBorders.Text = "Show &Balloon Borders";
            this.miShowBalloonBorders.Click += new System.EventHandler(this.miShowBalloonBorders_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(227, 6);
            // 
            // themesToolStripMenuItem
            // 
            this.themesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.professionalToolStripMenuItem,
            this.systemToolStripMenuItem});
            this.themesToolStripMenuItem.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.palette;
            this.themesToolStripMenuItem.Name = "themesToolStripMenuItem";
            this.themesToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.themesToolStripMenuItem.Text = "&Themes";
            // 
            // professionalToolStripMenuItem
            // 
            this.professionalToolStripMenuItem.Checked = true;
            this.professionalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.professionalToolStripMenuItem.Name = "professionalToolStripMenuItem";
            this.professionalToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.professionalToolStripMenuItem.Text = "&Professional";
            this.professionalToolStripMenuItem.Click += new System.EventHandler(this.professionalToolStripMenuItem_Click);
            // 
            // systemToolStripMenuItem
            // 
            this.systemToolStripMenuItem.Name = "systemToolStripMenuItem";
            this.systemToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.systemToolStripMenuItem.Text = "&System";
            this.systemToolStripMenuItem.Click += new System.EventHandler(this.systemToolStripMenuItem_Click);
            // 
            // miHelp
            // 
            this.miHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pdfReportsHelpContentsToolStripMenuItem,
            this.pdfReportsToolStripMenuItem,
            this.toolStripSeparator8,
            this.axiomCodersOnlineToolStripMenuItem,
            this.toolStripSeparator15,
            this.releaseNotesToolStripMenuItem,
            this.toolStripSeparator16,
            this.miAbout});
            this.miHelp.Name = "miHelp";
            this.miHelp.Size = new System.Drawing.Size(44, 20);
            this.miHelp.Text = "&Help";
            // 
            // pdfReportsHelpContentsToolStripMenuItem
            // 
            this.pdfReportsHelpContentsToolStripMenuItem.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.book_help;
            this.pdfReportsHelpContentsToolStripMenuItem.Name = "pdfReportsHelpContentsToolStripMenuItem";
            this.pdfReportsHelpContentsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.pdfReportsHelpContentsToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.pdfReportsHelpContentsToolStripMenuItem.Text = "Pdf Reports Help &Contents...";
            this.pdfReportsHelpContentsToolStripMenuItem.Click += new System.EventHandler(this.pdfReportsHelpContentsToolStripMenuItem_Click);
            // 
            // pdfReportsToolStripMenuItem
            // 
            this.pdfReportsToolStripMenuItem.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.book_open_help;
            this.pdfReportsToolStripMenuItem.Name = "pdfReportsToolStripMenuItem";
            this.pdfReportsToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.pdfReportsToolStripMenuItem.Text = "Pdf Reports Help &Index...";
            this.pdfReportsToolStripMenuItem.Click += new System.EventHandler(this.pdfReportsToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(239, 6);
            // 
            // axiomCodersOnlineToolStripMenuItem
            // 
            this.axiomCodersOnlineToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.axiomCodersWebsiteToolStripMenuItem,
            this.pdfReportsWebsiteToolStripMenuItem,
            this.pdfReportsSuportToolStripMenuItem,
            this.toolStripSeparator17,
            this.purchaseALicenseToolStripMenuItem});
            this.axiomCodersOnlineToolStripMenuItem.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Online;
            this.axiomCodersOnlineToolStripMenuItem.Name = "axiomCodersOnlineToolStripMenuItem";
            this.axiomCodersOnlineToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.axiomCodersOnlineToolStripMenuItem.Text = "AxiomCoders &Online";
            // 
            // axiomCodersWebsiteToolStripMenuItem
            // 
            this.axiomCodersWebsiteToolStripMenuItem.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.OnLineLink;
            this.axiomCodersWebsiteToolStripMenuItem.Name = "axiomCodersWebsiteToolStripMenuItem";
            this.axiomCodersWebsiteToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.axiomCodersWebsiteToolStripMenuItem.Text = "AxiomCoders &Website...";
            this.axiomCodersWebsiteToolStripMenuItem.Click += new System.EventHandler(this.axiomCodersWebsiteToolStripMenuItem_Click);
            // 
            // pdfReportsWebsiteToolStripMenuItem
            // 
            this.pdfReportsWebsiteToolStripMenuItem.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.OnLineLink;
            this.pdfReportsWebsiteToolStripMenuItem.Name = "pdfReportsWebsiteToolStripMenuItem";
            this.pdfReportsWebsiteToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.pdfReportsWebsiteToolStripMenuItem.Text = "&Pdf Reports Website...";
            this.pdfReportsWebsiteToolStripMenuItem.Click += new System.EventHandler(this.pdfReportsWebsiteToolStripMenuItem_Click);
            // 
            // pdfReportsSuportToolStripMenuItem
            // 
            this.pdfReportsSuportToolStripMenuItem.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.OnLineLink;
            this.pdfReportsSuportToolStripMenuItem.Name = "pdfReportsSuportToolStripMenuItem";
            this.pdfReportsSuportToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.pdfReportsSuportToolStripMenuItem.Text = "Pdf Reports &Suport...";
            this.pdfReportsSuportToolStripMenuItem.Click += new System.EventHandler(this.pdfReportsSuportToolStripMenuItem_Click);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(196, 6);
            // 
            // purchaseALicenseToolStripMenuItem
            // 
            this.purchaseALicenseToolStripMenuItem.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.OnLineLink;
            this.purchaseALicenseToolStripMenuItem.Name = "purchaseALicenseToolStripMenuItem";
            this.purchaseALicenseToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.purchaseALicenseToolStripMenuItem.Text = "&Purchase a License...";
            this.purchaseALicenseToolStripMenuItem.Click += new System.EventHandler(this.purchaseALicenseToolStripMenuItem_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(239, 6);
            // 
            // releaseNotesToolStripMenuItem
            // 
            this.releaseNotesToolStripMenuItem.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.note;
            this.releaseNotesToolStripMenuItem.Name = "releaseNotesToolStripMenuItem";
            this.releaseNotesToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.releaseNotesToolStripMenuItem.Text = "&Release Notes...";
            this.releaseNotesToolStripMenuItem.Click += new System.EventHandler(this.releaseNotesToolStripMenuItem_Click);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(239, 6);
            // 
            // miAbout
            // 
            this.miAbout.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.About;
            this.miAbout.Name = "miAbout";
            this.miAbout.Size = new System.Drawing.Size(242, 22);
            this.miAbout.Text = "&About...";
            this.miAbout.Click += new System.EventHandler(this.miAbout_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbbNewProject,
            this.tbbOpenProject,
            this.tbbSaveProject,
            this.toolStripSeparator13,
            this.tbbCut,
            this.tbbCopy,
            this.tbbPaste,
            this.toolStripSeparator14,
            this.tbbUndo,
            this.tbbRedo});
            this.toolStrip2.Location = new System.Drawing.Point(3, 24);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(208, 25);
            this.toolStrip2.TabIndex = 10;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // tbbNewProject
            // 
            this.tbbNewProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbNewProject.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.NewProject;
            this.tbbNewProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbNewProject.Name = "tbbNewProject";
            this.tbbNewProject.Size = new System.Drawing.Size(23, 22);
            this.tbbNewProject.Text = "New";
            this.tbbNewProject.Click += new System.EventHandler(this.tbbNewProject_Click);
            // 
            // tbbOpenProject
            // 
            this.tbbOpenProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbOpenProject.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Open;
            this.tbbOpenProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbOpenProject.Name = "tbbOpenProject";
            this.tbbOpenProject.Size = new System.Drawing.Size(23, 22);
            this.tbbOpenProject.Text = "Open";
            this.tbbOpenProject.Click += new System.EventHandler(this.tbbOpenProject_Click);
            // 
            // tbbSaveProject
            // 
            this.tbbSaveProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbSaveProject.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.SaveAs;
            this.tbbSaveProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbSaveProject.Name = "tbbSaveProject";
            this.tbbSaveProject.Size = new System.Drawing.Size(23, 22);
            this.tbbSaveProject.Text = "Save";
            this.tbbSaveProject.Click += new System.EventHandler(this.tbbSaveProject_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(6, 25);
            // 
            // tbbCut
            // 
            this.tbbCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbCut.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Cut;
            this.tbbCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbCut.Name = "tbbCut";
            this.tbbCut.Size = new System.Drawing.Size(23, 22);
            this.tbbCut.Text = "Cut";
            this.tbbCut.Click += new System.EventHandler(this.tbbCut_Click);
            // 
            // tbbCopy
            // 
            this.tbbCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbCopy.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Copy;
            this.tbbCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbCopy.Name = "tbbCopy";
            this.tbbCopy.Size = new System.Drawing.Size(23, 22);
            this.tbbCopy.Text = "Copy";
            this.tbbCopy.Click += new System.EventHandler(this.tbbCopy_Click);
            // 
            // tbbPaste
            // 
            this.tbbPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbPaste.Enabled = false;
            this.tbbPaste.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Paste;
            this.tbbPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbPaste.Name = "tbbPaste";
            this.tbbPaste.Size = new System.Drawing.Size(23, 22);
            this.tbbPaste.Text = "Paste";
            this.tbbPaste.Click += new System.EventHandler(this.tbbPaste_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(6, 25);
            // 
            // tbbUndo
            // 
            this.tbbUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbUndo.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Undo;
            this.tbbUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbUndo.Name = "tbbUndo";
            this.tbbUndo.Size = new System.Drawing.Size(23, 22);
            this.tbbUndo.Text = "Undo";
            this.tbbUndo.Click += new System.EventHandler(this.tbbUndo_Click);
            // 
            // tbbRedo
            // 
            this.tbbRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbRedo.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Redo;
            this.tbbRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbRedo.Name = "tbbRedo";
            this.tbbRedo.Size = new System.Drawing.Size(23, 22);
            this.tbbRedo.Text = "Redo";
            this.tbbRedo.Click += new System.EventHandler(this.tbbRedo_Click);
            // 
            // AlignStrip
            // 
            this.AlignStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.AlignStrip.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.AlignStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolSnapToGrid,
            this.toolLeftAlign,
            this.toolAlignCentarV,
            this.toolAlignRight,
            this.toolStripSeparator4,
            this.toolAlignTop,
            this.toolAlignCentarH,
            this.toolAlignBottom,
            this.toolStripSeparator5,
            this.toolSameWidth,
            this.toolSameHeight,
            this.toolSameSize,
            this.toolStripSeparator6,
            this.toolSameHSpacing,
            this.toolncHSpacing,
            this.toolDecHSpacing,
            this.toolNoHSpacing,
            this.toolStripSeparator7,
            this.toolSameVSpacing,
            this.toolIncVSpacing,
            this.toolDecVSpacing,
            this.toolNoVSpacing});
            this.AlignStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.AlignStrip.Location = new System.Drawing.Point(3, 49);
            this.AlignStrip.Name = "AlignStrip";
            this.AlignStrip.Size = new System.Drawing.Size(427, 25);
            this.AlignStrip.TabIndex = 11;
            // 
            // toolSnapToGrid
            // 
            this.toolSnapToGrid.CheckOnClick = true;
            this.toolSnapToGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolSnapToGrid.Enabled = false;
            this.toolSnapToGrid.Image = ((System.Drawing.Image)(resources.GetObject("toolSnapToGrid.Image")));
            this.toolSnapToGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSnapToGrid.Name = "toolSnapToGrid";
            this.toolSnapToGrid.Size = new System.Drawing.Size(62, 22);
            this.toolSnapToGrid.Text = "Snap tp G";
            this.toolSnapToGrid.ToolTipText = "Snaps object to grid";
            this.toolSnapToGrid.Visible = false;
            // 
            // toolLeftAlign
            // 
            this.toolLeftAlign.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolLeftAlign.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.AlignLeft;
            this.toolLeftAlign.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolLeftAlign.Name = "toolLeftAlign";
            this.toolLeftAlign.Size = new System.Drawing.Size(23, 22);
            this.toolLeftAlign.Text = "Left";
            this.toolLeftAlign.Click += new System.EventHandler(this.toolLeftAlign_Click);
            // 
            // toolAlignCentarV
            // 
            this.toolAlignCentarV.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolAlignCentarV.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.AlignCenter;
            this.toolAlignCentarV.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolAlignCentarV.Name = "toolAlignCentarV";
            this.toolAlignCentarV.Size = new System.Drawing.Size(23, 22);
            this.toolAlignCentarV.Text = "Centar (V)";
            this.toolAlignCentarV.Click += new System.EventHandler(this.toolAlignCentarV_Click);
            // 
            // toolAlignRight
            // 
            this.toolAlignRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolAlignRight.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.AlignRight;
            this.toolAlignRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolAlignRight.Name = "toolAlignRight";
            this.toolAlignRight.Size = new System.Drawing.Size(23, 22);
            this.toolAlignRight.Text = "Right";
            this.toolAlignRight.Click += new System.EventHandler(this.toolAlignRight_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolAlignTop
            // 
            this.toolAlignTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolAlignTop.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.AlignTop;
            this.toolAlignTop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolAlignTop.Name = "toolAlignTop";
            this.toolAlignTop.Size = new System.Drawing.Size(23, 22);
            this.toolAlignTop.Text = "Top";
            this.toolAlignTop.Click += new System.EventHandler(this.toolAlignTop_Click);
            // 
            // toolAlignCentarH
            // 
            this.toolAlignCentarH.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolAlignCentarH.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.AlignMiddle;
            this.toolAlignCentarH.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolAlignCentarH.Name = "toolAlignCentarH";
            this.toolAlignCentarH.Size = new System.Drawing.Size(23, 22);
            this.toolAlignCentarH.Text = "Centar (H)";
            this.toolAlignCentarH.Click += new System.EventHandler(this.toolAlignCentarH_Click);
            // 
            // toolAlignBottom
            // 
            this.toolAlignBottom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolAlignBottom.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.AlignBottom;
            this.toolAlignBottom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolAlignBottom.Name = "toolAlignBottom";
            this.toolAlignBottom.Size = new System.Drawing.Size(23, 22);
            this.toolAlignBottom.Text = "Bottom";
            this.toolAlignBottom.Click += new System.EventHandler(this.toolAlignBottom_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolSameWidth
            // 
            this.toolSameWidth.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolSameWidth.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.make_same_width;
            this.toolSameWidth.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSameWidth.Name = "toolSameWidth";
            this.toolSameWidth.Size = new System.Drawing.Size(23, 22);
            this.toolSameWidth.Text = "Make Same Width";
            this.toolSameWidth.Click += new System.EventHandler(this.toolSameWidth_Click);
            // 
            // toolSameHeight
            // 
            this.toolSameHeight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolSameHeight.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.make_same_height;
            this.toolSameHeight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSameHeight.Name = "toolSameHeight";
            this.toolSameHeight.Size = new System.Drawing.Size(23, 22);
            this.toolSameHeight.Text = "Make Same Height";
            this.toolSameHeight.Click += new System.EventHandler(this.toolSameHeight_Click);
            // 
            // toolSameSize
            // 
            this.toolSameSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolSameSize.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.make_same_sizes;
            this.toolSameSize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSameSize.Name = "toolSameSize";
            this.toolSameSize.Size = new System.Drawing.Size(23, 22);
            this.toolSameSize.Text = "Make Same Sizes";
            this.toolSameSize.Click += new System.EventHandler(this.toolSameSize_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // toolSameHSpacing
            // 
            this.toolSameHSpacing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolSameHSpacing.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.make_same_h_space;
            this.toolSameHSpacing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSameHSpacing.Name = "toolSameHSpacing";
            this.toolSameHSpacing.Size = new System.Drawing.Size(23, 22);
            this.toolSameHSpacing.Text = "Same H Space";
            this.toolSameHSpacing.ToolTipText = "Same Horizontal Spacing";
            this.toolSameHSpacing.Click += new System.EventHandler(this.toolSameHSpacing_Click);
            // 
            // toolncHSpacing
            // 
            this.toolncHSpacing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolncHSpacing.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.increase_h_space;
            this.toolncHSpacing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolncHSpacing.Name = "toolncHSpacing";
            this.toolncHSpacing.Size = new System.Drawing.Size(23, 22);
            this.toolncHSpacing.Text = "Inc H Space";
            this.toolncHSpacing.ToolTipText = "Increase Horizontal Spacing";
            this.toolncHSpacing.Click += new System.EventHandler(this.toolncHSpacing_Click);
            // 
            // toolDecHSpacing
            // 
            this.toolDecHSpacing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolDecHSpacing.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.decrease_h_space;
            this.toolDecHSpacing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolDecHSpacing.Name = "toolDecHSpacing";
            this.toolDecHSpacing.Size = new System.Drawing.Size(23, 22);
            this.toolDecHSpacing.Text = "Dec H Space";
            this.toolDecHSpacing.ToolTipText = "Decrease Horizontal Spacing";
            this.toolDecHSpacing.Click += new System.EventHandler(this.toolDecHSpacing_Click);
            // 
            // toolNoHSpacing
            // 
            this.toolNoHSpacing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolNoHSpacing.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.make_no_h_space;
            this.toolNoHSpacing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolNoHSpacing.Name = "toolNoHSpacing";
            this.toolNoHSpacing.Size = new System.Drawing.Size(23, 22);
            this.toolNoHSpacing.Text = "No Horizontal Spacing";
            this.toolNoHSpacing.Click += new System.EventHandler(this.toolNoHSpacing_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // toolSameVSpacing
            // 
            this.toolSameVSpacing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolSameVSpacing.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.make_same_v_space;
            this.toolSameVSpacing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSameVSpacing.Name = "toolSameVSpacing";
            this.toolSameVSpacing.Size = new System.Drawing.Size(23, 22);
            this.toolSameVSpacing.Text = "Same V Spacing";
            this.toolSameVSpacing.ToolTipText = "Same Vertical Spacing";
            this.toolSameVSpacing.Click += new System.EventHandler(this.toolSameVSpacing_Click);
            // 
            // toolIncVSpacing
            // 
            this.toolIncVSpacing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolIncVSpacing.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.increase_w_space;
            this.toolIncVSpacing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolIncVSpacing.Name = "toolIncVSpacing";
            this.toolIncVSpacing.Size = new System.Drawing.Size(23, 22);
            this.toolIncVSpacing.Text = "Inc V Spacing";
            this.toolIncVSpacing.ToolTipText = "Increase Vertical Spacing";
            this.toolIncVSpacing.Click += new System.EventHandler(this.toolIncVSpacing_Click);
            // 
            // toolDecVSpacing
            // 
            this.toolDecVSpacing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolDecVSpacing.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.decrease_w_space;
            this.toolDecVSpacing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolDecVSpacing.Name = "toolDecVSpacing";
            this.toolDecVSpacing.Size = new System.Drawing.Size(23, 22);
            this.toolDecVSpacing.Text = "Dec V Spacing";
            this.toolDecVSpacing.ToolTipText = "Decrease Vertical Spacing";
            this.toolDecVSpacing.Click += new System.EventHandler(this.toolDecVSpacing_Click);
            // 
            // toolNoVSpacing
            // 
            this.toolNoVSpacing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolNoVSpacing.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.make_no_v_space;
            this.toolNoVSpacing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolNoVSpacing.Name = "toolNoVSpacing";
            this.toolNoVSpacing.Size = new System.Drawing.Size(23, 22);
            this.toolNoVSpacing.Text = "No Vertical Spacing";
            this.toolNoVSpacing.Click += new System.EventHandler(this.toolNoVSpacing_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbbHeight2,
            this.tbbHeight3,
            this.tbbHeight4,
            this.toolStripSeparator10,
            this.tbbWidth2,
            this.tbbWidth3,
            this.tbbWidth4,
            this.toolStripSeparator9,
            this.tbbSendToBack,
            this.tbbBringToFront});
            this.toolStrip1.Location = new System.Drawing.Point(3, 74);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(208, 25);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tbbHeight2
            // 
            this.tbbHeight2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbHeight2.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.height_1_2;
            this.tbbHeight2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbHeight2.Name = "tbbHeight2";
            this.tbbHeight2.Size = new System.Drawing.Size(23, 22);
            this.tbbHeight2.Text = "1/2 Height";
            this.tbbHeight2.Click += new System.EventHandler(this.tbbHeight2_Click);
            // 
            // tbbHeight3
            // 
            this.tbbHeight3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbHeight3.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.height_1_3;
            this.tbbHeight3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbHeight3.Name = "tbbHeight3";
            this.tbbHeight3.Size = new System.Drawing.Size(23, 22);
            this.tbbHeight3.Text = "1/3 Height";
            this.tbbHeight3.Click += new System.EventHandler(this.tbbHeight3_Click);
            // 
            // tbbHeight4
            // 
            this.tbbHeight4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbHeight4.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.height_1_4;
            this.tbbHeight4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbHeight4.Name = "tbbHeight4";
            this.tbbHeight4.Size = new System.Drawing.Size(23, 22);
            this.tbbHeight4.Text = "1/4 Height";
            this.tbbHeight4.Click += new System.EventHandler(this.tbbHeight4_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
            // 
            // tbbWidth2
            // 
            this.tbbWidth2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbWidth2.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.width_1_2;
            this.tbbWidth2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbWidth2.Name = "tbbWidth2";
            this.tbbWidth2.Size = new System.Drawing.Size(23, 22);
            this.tbbWidth2.Text = "1/2 Width";
            this.tbbWidth2.Click += new System.EventHandler(this.tbbWidth2_Click);
            // 
            // tbbWidth3
            // 
            this.tbbWidth3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbWidth3.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.width_1_3;
            this.tbbWidth3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbWidth3.Name = "tbbWidth3";
            this.tbbWidth3.Size = new System.Drawing.Size(23, 22);
            this.tbbWidth3.Text = "1/3 Width";
            this.tbbWidth3.Click += new System.EventHandler(this.tbbWidth6_Click);
            // 
            // tbbWidth4
            // 
            this.tbbWidth4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbWidth4.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.width_1_4;
            this.tbbWidth4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbWidth4.Name = "tbbWidth4";
            this.tbbWidth4.Size = new System.Drawing.Size(23, 22);
            this.tbbWidth4.Text = "1/4 Width";
            this.tbbWidth4.Click += new System.EventHandler(this.tbbWidth4_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // tbbSendToBack
            // 
            this.tbbSendToBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbSendToBack.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.MoveBack;
            this.tbbSendToBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbSendToBack.Name = "tbbSendToBack";
            this.tbbSendToBack.Size = new System.Drawing.Size(23, 22);
            this.tbbSendToBack.Text = "STB <<-";
            this.tbbSendToBack.ToolTipText = "Send to back";
            this.tbbSendToBack.Click += new System.EventHandler(this.tbbSendToBack_Click);
            // 
            // tbbBringToFront
            // 
            this.tbbBringToFront.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbBringToFront.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.MoveFront;
            this.tbbBringToFront.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbBringToFront.Name = "tbbBringToFront";
            this.tbbBringToFront.Size = new System.Drawing.Size(23, 22);
            this.tbbBringToFront.Text = "->> BTF";
            this.tbbBringToFront.ToolTipText = "Bring to front";
            this.tbbBringToFront.Click += new System.EventHandler(this.tbbBringToFront_Click);
            // 
            // tbbStripOne
            // 
            this.tbbStripOne.Dock = System.Windows.Forms.DockStyle.None;
            this.tbbStripOne.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbbArrowItem,
            this.toolStripSeparator3,
            this.tbbZoom,
            this.tbbPreviewButton,
            this.toolStripSeparator1,
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton5,
            this.toolStripButton6,
            this.toolStripButton7,
            this.toolStripButton11,
            this.toolStripButton8,
            this.toolStripButton9,
            this.toolStripButton10});
            this.tbbStripOne.Location = new System.Drawing.Point(3, 99);
            this.tbbStripOne.Name = "tbbStripOne";
            this.tbbStripOne.Size = new System.Drawing.Size(355, 25);
            this.tbbStripOne.TabIndex = 13;
            // 
            // tbbArrowItem
            // 
            this.tbbArrowItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbArrowItem.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Cursor;
            this.tbbArrowItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbArrowItem.Name = "tbbArrowItem";
            this.tbbArrowItem.Size = new System.Drawing.Size(23, 22);
            this.tbbArrowItem.Text = "Arrow item";
            this.tbbArrowItem.Click += new System.EventHandler(this.tbbArrowItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tbbZoom
            // 
            this.tbbZoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbZoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miZoom50,
            this.miZoom100,
            this.miZoom150,
            this.miZoom200});
            this.tbbZoom.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.MagnifyingGlass;
            this.tbbZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbZoom.Name = "tbbZoom";
            this.tbbZoom.Size = new System.Drawing.Size(32, 22);
            this.tbbZoom.Text = "Zoom...";
            // 
            // miZoom50
            // 
            this.miZoom50.Name = "miZoom50";
            this.miZoom50.Size = new System.Drawing.Size(105, 22);
            this.miZoom50.Text = "50 %";
            this.miZoom50.Click += new System.EventHandler(this.miZoom50_Click);
            // 
            // miZoom100
            // 
            this.miZoom100.Name = "miZoom100";
            this.miZoom100.Size = new System.Drawing.Size(105, 22);
            this.miZoom100.Text = "100 %";
            this.miZoom100.Click += new System.EventHandler(this.miZoom100_Click);
            // 
            // miZoom150
            // 
            this.miZoom150.Name = "miZoom150";
            this.miZoom150.Size = new System.Drawing.Size(105, 22);
            this.miZoom150.Text = "150 %";
            this.miZoom150.Click += new System.EventHandler(this.miZoom150_Click);
            // 
            // miZoom200
            // 
            this.miZoom200.Name = "miZoom200";
            this.miZoom200.Size = new System.Drawing.Size(105, 22);
            this.miZoom200.Text = "200 %";
            this.miZoom200.Click += new System.EventHandler(this.miZoom200_Click);
            // 
            // tbbPreviewButton
            // 
            this.tbbPreviewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbPreviewButton.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Preview;
            this.tbbPreviewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbPreviewButton.Name = "tbbPreviewButton";
            this.tbbPreviewButton.Size = new System.Drawing.Size(23, 22);
            this.tbbPreviewButton.Text = "Preview is OFF";
            this.tbbPreviewButton.Click += new System.EventHandler(this.tbbPreview_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.StaticBalloon;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Static Balloon";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.DynamicBalloon;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Dynamic Balloon";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.StaticText;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "Static Text";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.DynamicText;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "Dynamic Text";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.picture;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton5.Text = "Static Image";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.DynamicImage;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton6.Text = "Dynamic Image";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Counter;
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton7.Text = "Counter";
            this.toolStripButton7.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // toolStripButton11
            // 
            this.toolStripButton11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton11.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Precalculated;
            this.toolStripButton11.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton11.Name = "toolStripButton11";
            this.toolStripButton11.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton11.Text = "Precalculated";
            this.toolStripButton11.Click += new System.EventHandler(this.toolStripButton11_Click);
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton8.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.Date;
            this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton8.Text = "Date Time";
            this.toolStripButton8.Click += new System.EventHandler(this.toolStripButton8_Click);
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton9.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.PageNum;
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton9.Text = "Page Number";
            this.toolStripButton9.Click += new System.EventHandler(this.toolStripButton9_Click);
            // 
            // toolStripButton10
            // 
            this.toolStripButton10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton10.Image = global::AxiomCoders.PdfTemplateEditor.Properties.Resources.RectangleShape;
            this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton10.Name = "toolStripButton10";
            this.toolStripButton10.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton10.Text = "Rectangle Shape";
            this.toolStripButton10.Click += new System.EventHandler(this.toolStripButton10_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1190, 594);
            this.Controls.Add(this.pnlProperties);
            this.Controls.Add(this.mainToolContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.ShowInTaskbar = true;
            this.Text = "PdfReports Template Editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.pnlProperties.ResumeLayout(false);
            this.PropertySplitContainer.Panel1.ResumeLayout(false);
            this.PropertySplitContainer.Panel2.ResumeLayout(false);
            this.PropertySplitContainer.ResumeLayout(false);
            this.mainToolContainer.TopToolStripPanel.ResumeLayout(false);
            this.mainToolContainer.TopToolStripPanel.PerformLayout();
            this.mainToolContainer.ResumeLayout(false);
            this.mainToolContainer.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.AlignStrip.ResumeLayout(false);
            this.AlignStrip.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tbbStripOne.ResumeLayout(false);
            this.tbbStripOne.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlProperties;
		private System.Windows.Forms.ComboBox cmbObjects;
		private System.Windows.Forms.Timer AutoSaveTimer;
		private System.Windows.Forms.SplitContainer PropertySplitContainer;
		private System.Windows.Forms.PropertyGrid propertyGrid;
		private System.Windows.Forms.TreeView tvItems;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolStripContainer mainToolContainer;
		private System.Windows.Forms.ToolStrip toolStrip2;
		private System.Windows.Forms.ToolStripButton tbbNewProject;
		private System.Windows.Forms.ToolStripButton tbbOpenProject;
		private System.Windows.Forms.ToolStripButton tbbSaveProject;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
		private System.Windows.Forms.ToolStripButton tbbCut;
		private System.Windows.Forms.ToolStripButton tbbCopy;
		private System.Windows.Forms.ToolStripButton tbbPaste;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
		private System.Windows.Forms.ToolStripButton tbbUndo;
		private System.Windows.Forms.ToolStripButton tbbRedo;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton tbbHeight2;
		private System.Windows.Forms.ToolStripButton tbbHeight3;
		private System.Windows.Forms.ToolStripButton tbbHeight4;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
		private System.Windows.Forms.ToolStripButton tbbWidth2;
		private System.Windows.Forms.ToolStripButton tbbWidth3;
		private System.Windows.Forms.ToolStripButton tbbWidth4;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
		private System.Windows.Forms.ToolStripButton tbbSendToBack;
		private System.Windows.Forms.ToolStripButton tbbBringToFront;
		private System.Windows.Forms.ToolStrip AlignStrip;
        private System.Windows.Forms.ToolStripButton toolSnapToGrid;
		private System.Windows.Forms.ToolStripButton toolLeftAlign;
		private System.Windows.Forms.ToolStripButton toolAlignCentarV;
		private System.Windows.Forms.ToolStripButton toolAlignRight;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripButton toolAlignTop;
		private System.Windows.Forms.ToolStripButton toolAlignCentarH;
		private System.Windows.Forms.ToolStripButton toolAlignBottom;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripButton toolSameWidth;
		private System.Windows.Forms.ToolStripButton toolSameHeight;
		private System.Windows.Forms.ToolStripButton toolSameSize;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripButton toolSameHSpacing;
		private System.Windows.Forms.ToolStripButton toolncHSpacing;
		private System.Windows.Forms.ToolStripButton toolDecHSpacing;
		private System.Windows.Forms.ToolStripButton toolNoHSpacing;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripButton toolSameVSpacing;
		private System.Windows.Forms.ToolStripButton toolIncVSpacing;
		private System.Windows.Forms.ToolStripButton toolDecVSpacing;
		private System.Windows.Forms.ToolStripButton toolNoVSpacing;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem miFile;
		private System.Windows.Forms.ToolStripMenuItem miNewProject;
		private System.Windows.Forms.ToolStripMenuItem miOpenProject;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem miSave;
		private System.Windows.Forms.ToolStripMenuItem miSaveAs;
		private System.Windows.Forms.ToolStripMenuItem miClose;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem miExit;
		private System.Windows.Forms.ToolStripMenuItem miEdit;
		private System.Windows.Forms.ToolStripMenuItem miUndo;
		private System.Windows.Forms.ToolStripMenuItem miRedo;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem miCut;
		private System.Windows.Forms.ToolStripMenuItem miCopy;
		private System.Windows.Forms.ToolStripMenuItem miPaste;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
		private System.Windows.Forms.ToolStripMenuItem miPreferences;
		private System.Windows.Forms.ToolStripMenuItem miGeneral;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
		private System.Windows.Forms.ToolStripMenuItem miUnitsRulers;
		private System.Windows.Forms.ToolStripMenuItem miGuidesGrids;
		private System.Windows.Forms.ToolStripMenuItem miTools;
		private System.Windows.Forms.ToolStripMenuItem miDataStreams;
		private System.Windows.Forms.ToolStripMenuItem miView;
		private System.Windows.Forms.ToolStripMenuItem miShowGrid;
		private System.Windows.Forms.ToolStripMenuItem miShowRulers;
        private System.Windows.Forms.ToolStripMenuItem miShowBalloonBorders;
		private System.Windows.Forms.ToolStripMenuItem miHelp;
		private System.Windows.Forms.ToolStripMenuItem miAbout;
		private System.Windows.Forms.ToolStrip tbbStripOne;
		private System.Windows.Forms.ToolStripButton tbbArrowItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripSplitButton tbbZoom;
		private System.Windows.Forms.ToolStripMenuItem miZoom50;
		private System.Windows.Forms.ToolStripMenuItem miZoom100;
		private System.Windows.Forms.ToolStripMenuItem miZoom150;
		private System.Windows.Forms.ToolStripMenuItem miZoom200;
		private System.Windows.Forms.ToolStripButton tbbPreviewButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private System.Windows.Forms.ToolStripButton toolStripButton10;
        private System.Windows.Forms.ToolStripMenuItem pdfReportsHelpContentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pdfReportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem axiomCodersOnlineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem axiomCodersWebsiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pdfReportsWebsiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pdfReportsSuportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.ToolStripMenuItem purchaseALicenseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripMenuItem releaseNotesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem themesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem professionalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem systemToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton11;
        private System.Windows.Forms.ToolStripMenuItem editProjectInformationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
	}
}

