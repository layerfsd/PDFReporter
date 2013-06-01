using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using AxiomCoders.PdfTemplateEditor.EditorItems;

using AxiomCoders.PdfTemplateEditor.Common;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
	/// <summary>
	/// singleton class used for displaying editor items
	/// </summary>
	public class EditorViewer
	{		
		/// <summary>
		/// Project used 
		/// </summary>
		private EditorProject project;

		/// <summary>
		/// Default constructor
		/// </summary>
		public EditorViewer(EditorProject project)
		{
			this.project = project;
            //topRuler = new Ruler(Ruler.RulerPosition.TOP, project.ReportPage);
            //leftRuler = new Ruler(Ruler.RulerPosition.LEFT,project.ReportPage);
            //guideRuler = new Ruler(Ruler.RulerPosition.GUIDE, project.ReportPage);		
            ReCreateRulers();
		}



        public void ReCreateRulers()
        {
            topRuler = new Ruler(Ruler.RulerPosition.TOP, project.ReportPage);
            leftRuler = new Ruler(Ruler.RulerPosition.LEFT, project.ReportPage);
            guideRuler = new Ruler(Ruler.RulerPosition.GUIDE, project.ReportPage);	
        }



		private EditorItemViewer topRuler;
		private EditorItemViewer leftRuler;
		private EditorItemViewer guideRuler;		
	
        public void SetRulersUnit(MeasureUnits unit)
        {
            Ruler tmpRuler = (Ruler)topRuler;
            tmpRuler.MeasureUnit = unit;
            tmpRuler = (Ruler)leftRuler;
            tmpRuler.MeasureUnit = unit;
            tmpRuler = (Ruler)guideRuler;
            tmpRuler.MeasureUnit = unit;
            RefreshView();
        }

        public MeasureUnits GetRulersUnit()
        {
            Ruler tmpRuler = (Ruler)topRuler;
            return tmpRuler.MeasureUnit;
        }

		/// <summary>
		/// Here we will show editor
		/// </summary>
		public void ShowEditor(Graphics gc, Rectangle clipRect)
		{
			gc.ResetTransform();
			gc.FillRectangle(Brushes.LightGray, clipRect);
			// draw Report
			//project.PrepareDraw(0, 0, 0, 0, 0, gc, clipRect);			
			System.Drawing.Drawing2D.Matrix mat = new System.Drawing.Drawing2D.Matrix();
			project.CurrentReportPage.Viewer.UpdateValues(clipRect);
			project.CurrentReportPage.MakeViewMatrix(clipRect);

			project.CurrentReportPage.MakeDrawMatrix(mat);
			project.CurrentReportPage.Draw(gc, clipRect);
			// draw commands



			
			// draw page boundries
			/*gc.DrawLine(Pens.Black, 100, 100, clipRect.Width - 100, 100);
			gc.DrawLine(Pens.Black, 100, 100, 100, clipRect.Height-100);
			gc.DrawLine(Pens.Black, 100, clipRect.Height-100, clipRect.Width - 100, clipRect.Height-100);
			gc.DrawLine(Pens.Black, clipRect.Width-100, 100, clipRect.Width - 100, clipRect.Height-100);*/

			// draw items on page

			// draw gray mask
			//gc.FillRectangle(Brushes.LightGray, 0, 0, 99, clipRect.Height);
			//gc.FillRectangle(Brushes.LightGray, 0, 0, clipRect.Width, 99);
			//gc.FillRectangle(Brushes.LightGray, clipRect.Width-99, 0, clipRect.Width, clipRect.Height);
			//gc.FillRectangle(Brushes.LightGray, 0, clipRect.Height-99, clipRect.Width, clipRect.Height);					
		}

		public void ShowTopRuler(Graphics gc, Rectangle clipRect)
		{
			topRuler.Draw(0, 0, 0, 0, 0, gc, clipRect);
		}

		public void ShowLeftRuler(Graphics gc, Rectangle clipRect)
		{
			leftRuler.Draw(0, 0, 0, 0, 0, gc, clipRect);
		}

		public void ShowRulerGuide(Graphics gc, Rectangle clipRect)
		{
			guideRuler.Draw(0, 0, 0, 0, 0, gc, clipRect);
		}

		/// <summary>
		/// This will refresh view
		/// </summary>
		public void RefreshView()
		{
			this.project.FrmReport.RefreshPanels();
		}
	}
}
