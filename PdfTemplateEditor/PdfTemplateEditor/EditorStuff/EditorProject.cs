using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using AxiomCoders.PdfTemplateEditor.Forms;
using AxiomCoders.PdfTemplateEditor.EditorItems;
using System.Xml;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
	/// <summary>
	/// this is base class for editor projects
	/// </summary>
	public class EditorProject: EditorItem
	{	

		/// <summary>
		/// New project 
		/// </summary>
		public void New()
		{
			
		}

		/// <summary>
		/// Close project
		/// </summary>
		public void Close(bool closeForm)
		{
            if(closeForm)
            {
                this.frmReport.Close();
            }
		}

		/// <summary>
		/// Form used to show report
		/// </summary>
		private ReportForm frmReport;
		
		public AxiomCoders.PdfTemplateEditor.Forms.ReportForm FrmReport
		{
			get { return frmReport; }
			set { frmReport = value; }
		}


		/// <summary>
		/// Default constructor
		/// </summary>
		public EditorProject()
		{
            bool ActionState = ActionManager.Instance.Disabled;
            ActionManager.Instance.Disabled = true;
            reportPage = (ReportPage)EditorItemFactory.Instance.CreateItem(typeof(ReportPage));
            ActionManager.Instance.Disabled = ActionState;
			isContainer = true;
			Children.Add(reportPage);
		}

		/// <summary>
		/// Parent form
		/// </summary>
		private Form parentForm;

		/// <summary>
		/// Report page on template. This will be changed to list of report pages later but currently this is enough
		/// </summary>
		private ReportPage reportPage;
		public AxiomCoders.PdfTemplateEditor.EditorItems.ReportPage ReportPage
		{
			get { return reportPage; }			
		}

		/// <summary>
		/// return current report page
		/// </summary>
		public ReportPage CurrentReportPage
		{
			get { return ReportPage; }
		}


		public override System.Drawing.Drawing2D.Matrix TransformationMatrix
		{
			get
			{
				this.transformationMatrix.Reset();
				return this.transformationMatrix;
			}
		}

		/// <summary>
		/// Does selection
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public override EditorItem SelectItem(float x, float y)
		{
			return ReportPage.SelectItem(x, y);
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


        
        
        private List<DataStream> dataStreams = new List<DataStream>();
        public List<DataStream> DataStreams
        {
            get { return dataStreams; }
        }



        /*private List<EditorItem> previewList = new List<EditorItem>();
        public List<EditorItem> PreviewList
        {
            get { return previewList; }
            set { previewList = value; }
        }*/


        //private List<EditorItem> ActualChildList = new List<EditorItem>();


		private ReportPage previewReportPage;
		
		/// <summary>
		/// Used to store information where was real report page before preview
		/// </summary>
		private ReportPage sourceReportPage;
		public ReportPage SourceReportPage
		{
			get { return sourceReportPage; }		
		}


        public void SetPreviewListActive(bool active)
        {
            if(active)
            {
				sourceReportPage = ReportPage; // save current report page
				this.reportPage = (ReportPage)sourceReportPage.SimpleClone();  // report page is now emtpy clone that has no children and ready to be used by generator
            } 
			else
            {
				this.reportPage = sourceReportPage; // restore page to what was saved                
            }
        }

		private string author = "PdfReports";
		public string Author
		{
			get { return author; }
			set { author = value; }
		}

        private string title = "Untitled";
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string subject = "No Subject";
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        private string producer = "Axiomcoders - PdfFactory (www.axiomcoders.com)";

		public override void SaveItem(XmlDocument doc, XmlElement element)
		{
			base.SaveItem(doc, element);

			// save header
			XmlElement el = doc.CreateElement("Header");
			XmlAttribute attr = doc.CreateAttribute("Version");
			attr.Value = "1.0";
			el.SetAttributeNode(attr);
			XmlElement el2 = doc.CreateElement("TemplateInfo");
			attr = doc.CreateAttribute("Author");
			attr.Value = this.Author;
            el2.SetAttributeNode(attr);

            attr = doc.CreateAttribute("Title");
            attr.Value = this.Title;
            el2.SetAttributeNode(attr);

            attr = doc.CreateAttribute("Subject");
            attr.Value = this.Subject;
            el2.SetAttributeNode(attr);

            attr = doc.CreateAttribute("Producer");
            attr.Value = this.producer;
			el2.SetAttributeNode(attr);

			attr = doc.CreateAttribute("Date");
			attr.Value = "1.1.2009";
			el2.SetAttributeNode(attr);			

			el.AppendChild(el2);
			element.AppendChild(el);

			// save dataStreams
			el = doc.CreateElement("DataStreams");
			foreach(DataStream ds in this.DataStreams)
			{
				ds.Save(doc, el);
			}
			element.AppendChild(el);

            // save fonts
            FontManager.Instance.Save(this.ReportPage, doc, element);

			// save page(s)			
			this.ReportPage.Save(doc, element);			
		}	

		public override void Load(System.Xml.XmlNode element)
		{
			base.Load(element);

			XmlNodeList nodes = element.SelectNodes("Header");
			if (nodes.Count == 1)
			{
				// TODO: Load header
                foreach(XmlNode node in nodes[0].ChildNodes)
                {
                    if(node.Name == "TemplateInfo")
                    {
                        try
                        {
                            this.Title = node.Attributes["Title"].Value;
                            this.Author = node.Attributes["Author"].Value;
                            this.Subject = node.Attributes["Subject"].Value;
                            this.producer = node.Attributes["Producer"].Value;
                        }
                        catch
                        {
                            this.Title = "Untitled";
                            this.Author = "Unnamed";
                            this.Subject = "No Subject";
                            this.producer = "AxiomCoders PdfReports (www.axiomcoders.com)";
                        }
                    }
                }
			}

			// Load data streams
			nodes = element.SelectNodes("DataStreams");
			if (nodes.Count == 1)
			{
				foreach (XmlNode node in nodes[0].ChildNodes)
				{
					if (node.Name == "DataStream")
					{
						DataStream ds = new DataStream();
						ds.Load(node);
						this.DataStreams.Add(ds);
					}
				}
			}

            // load fonts if they exists
            XmlNode fontsNode = element.SelectSingleNode("Fonts");
            if (fontsNode != null)
            {
                FontManager.Instance.Load(fontsNode);
            }


			// load Page
			nodes = element.SelectNodes("Page");
			if (nodes.Count == 1)
			{
				ReportPage.Load(nodes[0]);				
			}		          

            
		}
    }
}
