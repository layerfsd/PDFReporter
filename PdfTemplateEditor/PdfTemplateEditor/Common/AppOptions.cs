using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;

using AxiomCoders.PdfTemplateEditor.EditorStuff;

namespace AxiomCoders.PdfTemplateEditor.Common
{
    [Serializable]
    public class AppOptions : EditorOptions
    {
        private static AppOptions instance = null;

        private bool themeProfessional = true;

        /// <summary>
        /// This indicate theme, if not professional then it is System theme.
        /// </summary>
        public bool ThemeProfessional
        {
            get { return themeProfessional; }
            set { themeProfessional = value; }
        }


        private string lastOpenPath = System.Windows.Forms.Application.StartupPath;

		/// <summary>
		/// Last open path used
		/// </summary>
		public string LastOpenPath
		{
			get { return lastOpenPath; }
			set { lastOpenPath = value; }
		}

        private string lastSavePath = System.Windows.Forms.Application.StartupPath;

		/// <summary>
		/// Last save path
		/// </summary>
		public string LastSavePath
		{
			get { return lastSavePath; }
			set { lastSavePath = value; }
		}


        private int numOffPreviewItems = 10;

        public int NumOffPreviewItems
        {
            get { return numOffPreviewItems; }
            set { numOffPreviewItems = value; }
        }


		private bool showRulers = true;

		/// <summary>
		/// Show rulers
		/// </summary>
		public bool ShowRulers
		{
			get { return showRulers; }
			set { showRulers = value; }
		}

        private bool autoSave = true;
		/// <summary>
		/// Is auto save enabled
		/// </summary>
		public bool AutoSave
		{
			get { return autoSave; }
			set { autoSave = value; }
		}


        private float autoSaveInterval = 60.0F; //In sec

		/// <summary>
		/// AutoSave interval in seconds
		/// </summary>
		public float AutoSaveInterval
		{
			get { return autoSaveInterval; }
			set { autoSaveInterval = value; }
		}


        private bool showBalloonBorders = true;

		/// <summary>
		/// Show balloon borders
		/// </summary>
		public bool ShowBalloonBorders
		{
			get { return showBalloonBorders; }
			set { showBalloonBorders = value; }
		}


        private int unit = 0;
		public int Unit
		{
			get { return unit; }
			set { unit = value; }
		}


        private int gridUnit = 0;

		/// <summary>
		/// Unit of grid
		/// </summary>
		public int GridUnit
		{
			get { return gridUnit; }
			set { gridUnit = value; }
		}


        private int gridLineStyle = 0;

		/// <summary>
		/// Style of grid
		/// </summary>
		public int GridLineStyle
		{
			get { return gridLineStyle; }
			set { gridLineStyle = value; }
		}


        private bool showGrid = false;

		/// <summary>
		/// Show grid
		/// </summary>
		public bool ShowGrid
		{
			get { return showGrid; }
			set { showGrid = value; }
		}

        private float gridInterval = 1.0F;

		/// <summary>
		/// Grid interval
		/// </summary>
		public float GridInterval
		{
			get { return gridInterval; }
			set { gridInterval = value; }
		}

        private float gridSubdivisions = 4.0F;

		/// <summary>
		/// Grid subdivisions
		/// </summary>
		public float GridSubdivisions
		{
			get { return gridSubdivisions; }
			set { gridSubdivisions = value; }
		}


        private bool showGridMinorLines = true;

		/// <summary>
		/// Show grid minor lines
		/// </summary>
		public bool ShowGridMinorLines
		{
			get { return showGridMinorLines; }
			set { showGridMinorLines = value; }
		}

        private bool gridSnap = false;

		/// <summary>
		/// Snap to Grid
		/// </summary>
		public bool GridSnap
		{
			get { return gridSnap; }
			set { gridSnap = value; }
		}

		private int gridColorR;

		public int GridColorR
		{
			get { return gridColorR; }
			set { gridColorR = value; }
		}
		private int gridColorG;

		public int GridColorG
		{
			get { return gridColorG; }
			set { gridColorG = value; }
		}
		private int gridColorB;

		public int GridColorB
		{
			get { return gridColorB; }
			set { gridColorB = value; }
		}

		private Color gridColor = Color.FromArgb(200, 200, 200);

		/// <summary>
		/// Grid color. Default is gray
		/// </summary>
		public System.Drawing.Color GridColor
		{
			get { return gridColor; }
			set { gridColor = value; }
		}
        


        /// <summary>
        /// Default constructor
        /// </summary>
        private AppOptions()
        {
            
        }


		/// <summary>
		/// Singleton
		/// </summary>
        public static AppOptions Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppOptions();                    
                }
                return instance;
            }
        }


        /// <summary>
        /// Loading options from Options.xml (if exists)
        /// if not just leave everything in default values
        /// </summary>
        public override void Load()
        {
			try
			{
				System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(this.GetType());
				System.IO.TextReader read = new System.IO.StreamReader(System.Windows.Forms.Application.StartupPath + "\\Options.cfg");
				instance = (AppOptions)x.Deserialize(read);
				read.Close();

				instance.GridColor = Color.FromArgb(instance.gridColorR, instance.gridColorG, instance.gridColorB);
			}
			catch
			{
			}
        }


        /// <summary>
        /// Saving options to Options.xml
        /// </summary>
        public override void Save()
        {
			try
			{
				gridColorR = gridColor.R;
				gridColorG = gridColor.G;
				gridColorB = gridColor.B;

				System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(this.GetType());
				System.IO.TextWriter wrt = new System.IO.StreamWriter(System.Windows.Forms.Application.StartupPath + "\\Options.cfg", false);
				x.Serialize(wrt, Instance);
				wrt.Close();
			}
			catch { }
        }


        /// <summary>
        /// Apply all options...
        /// </summary>
        /// <param name="saveOptionsToFile"></param>
        public void Apply(bool saveOptionsToFile)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            //Set general properties
            //EditorController.Instance.MainFormOfTheProject.EnableAutoSave = AutoSave;
            //EditorController.Instance.MainFormOfTheProject.IntervalForAutoSave = (int)AutoSaveInterval;

            //Set unit properties
            switch(Unit)
            {
                case 0:
                    EditorController.Instance.EditorViewer.SetRulersUnit(MeasureUnits.inch);
                    break;
                case 1:
                    EditorController.Instance.EditorViewer.SetRulersUnit(MeasureUnits.mm);
                    break;
                case 2:
                    EditorController.Instance.EditorViewer.SetRulersUnit(MeasureUnits.cm);
                    break;
                case 3:
                    EditorController.Instance.EditorViewer.SetRulersUnit(MeasureUnits.pixel);
                    break;
            }

            //Set grid properties
            switch(GridLineStyle)
            {
                case 0: EditorController.Instance.Grid.LineStyle = System.Drawing.Drawing2D.DashStyle.Solid; break;
                case 1: EditorController.Instance.Grid.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash; break;
                case 2: EditorController.Instance.Grid.LineStyle = System.Drawing.Drawing2D.DashStyle.Dot; break;
            }

            switch(GridUnit)
            {
                case 0: EditorController.Instance.Grid.Unit = MeasureUnits.inch; break;
                case 1: EditorController.Instance.Grid.Unit = MeasureUnits.cm; break;
                case 2: EditorController.Instance.Grid.Unit = MeasureUnits.mm; break;
                case 3: EditorController.Instance.Grid.Unit = MeasureUnits.pixel; break;
            }
			           
            
            
            
            EditorController.Instance.Grid.Snap = GridSnap;
            EditorController.Instance.Grid.LineColor = GridColor;
            
            //EditorController.Instance.EditorViewer.RefreshView();

            if(saveOptionsToFile)
            {
                instance.Save();
            }
        }
    }
}
