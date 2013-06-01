using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using PdfTemplateEditor.Forms;
using PdfTemplateEditor.EditorItems;

namespace PdfTemplateEditor.EditorStuff
{
	public class PdfReportProject2: EditorProject
	{
		#region SaveLoadMechanism Members

		public void Save(System.Xml.XmlNode node)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void Load(System.Xml.XmlNode node)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion

		/// <summary>
		/// New project 
		/// </summary>
		public override void New()
		{
			
		}

		/// <summary>
		/// Close project
		/// </summary>
		public override void Close()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		/// <summary>
		/// Form used to show report
		/// </summary>
		private ReportForm frmReport;
		
		public PdfTemplateEditor.Forms.ReportForm FrmReport
		{
			get { return frmReport; }
			set { frmReport = value; }
		}


		/// <summary>
		/// Parent form
		/// </summary>
		private Form parentForm;

		/// <summary>
		/// Report page on template. This will be changed to list of report pages later but currently this is enough
		/// </summary>
		private ReportPage reportPage = new ReportPage();
		public PdfTemplateEditor.EditorItems.ReportPage ReportPage
		{
			get { return reportPage; }
			set { reportPage = value; }
		}
		/// <summary>
		/// Create new project
		/// </summary>
		/// <param name="parent"></param>
		public PdfReportProject2()
		{
			
		}

		/// <summary>
		/// Draw project by first drawing page
		/// </summary>
		/// <param name="gc"></param>
		/// <param name="clipRect"></param>
		public override void Draw(System.Drawing.Graphics gc, System.Drawing.Rectangle clipRect)
		{
			reportPage.Viewer.Draw(gc, clipRect);
		}

		/// <summary>
		/// InitiliazeGUI
		/// </summary>
		/// <param name="parent"></param>
		public void InitiliazeGUI(Form parent)
		{
			// initialize GUI part 
			this.parentForm = parent;						
			frmReport = new ReportForm(this);
			frmReport.MdiParent = parent;			
			frmReport.Show();
		}
	}
}
