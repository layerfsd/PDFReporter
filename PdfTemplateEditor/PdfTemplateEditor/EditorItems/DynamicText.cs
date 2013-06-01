using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;

using AxiomCoders.PdfTemplateEditor.EditorStuff;
using AxiomCoders.PdfTemplateEditor.Common;
using System.Xml;



namespace AxiomCoders.PdfTemplateEditor.EditorItems
{

	[System.Reflection.Obfuscation(Exclude = true)]
    public class DynamicText : BaseTextItems, EditorToolBarPlugin, DynamicEditorItemInterface
    {
        private string sourceColumn;
        private string sourceDataStream;


		/// <summary>
		/// Constructor
		/// </summary>
        public DynamicText(): base()
        {           
            sourceColumn = string.Empty;
            sourceDataStream = string.Empty;			
            scaleXFactor = 1;
            scaleYFactor = 1;
        }

		/// <summary>
		/// Save Item
		/// </summary>
		/// <param name="txW"></param>
        public override void SaveItem(XmlDocument doc, XmlElement element)
        {
			XmlElement el = doc.CreateElement("Item");
			base.SaveItem(doc, el);

			XmlAttribute attr = doc.CreateAttribute("Type");
			attr.Value = "DynamicText";
			el.SetAttributeNode(attr);

			attr = doc.CreateAttribute("Version");
			attr.Value = "1.0";
			el.SetAttributeNode(attr);

			XmlElement el2 = doc.CreateElement("Text");
			attr = doc.CreateAttribute("DataStream");
			attr.Value = this.SourceDataStream;
			el2.SetAttributeNode(attr);
						
			attr = doc.CreateAttribute("SourceColumn");
			attr.Value = this.SourceColumn;
			el2.SetAttributeNode(attr);

			el.AppendChild(el2);

			element.AppendChild(el);          
        }

		/// <summary>
		/// Load Item
		/// </summary>
		/// <param name="txR"></param>
        public override void Load(System.Xml.XmlNode element)
        {
			base.Load(element);

			// Load source data stream and column
			XmlNode node = element.SelectSingleNode("Text");
			if (node != null)
			{
				this.SourceDataStream = node.Attributes["DataStream"].Value;
				this.SourceColumn = node.Attributes["SourceColumn"].Value;
			}           
        }



        /// <summary>
        /// Gets or sets source column name of item.
        /// </summary>
        [Browsable(true), DisplayName("Column"), Description("Source column name of item."), Category("Standard"), EditorAttribute(typeof(ComboBoxPropertyEditor), typeof(UITypeEditor))]
        public string SourceColumn
        {
            get 
            {
                
                if(SourceDataStream != "")
                {
                    int tmpCount = 0;
                    string[] tmpList = null;
                    int i = 0;

                    foreach(DataStream tmpStream in EditorController.Instance.EditorProject.DataStreams)
                    {
                        if(tmpStream.Name == SourceDataStream)
                        {
                            tmpCount = tmpStream.Columns.Count;
                            tmpList = new string[tmpCount];
                            foreach(Column tmpCol in tmpStream.Columns)
                            {
                                tmpList[i] = tmpCol.Name;
                                i++;
                            }
                            ComboBoxPropertyEditor.ItemList = tmpList;
                            break;
                        }
                    }
                }

                return sourceColumn; 
            }
            set
            {
                sourceColumn = value;
                // Update size of item when text is changed
                needToUpdateSize = true;
            }
        }


        /// <summary>
        /// Gets or sets source Data Stream of item.
        /// </summary>
        [Browsable(true), DisplayName("Data Stream"), Description("Source Data Stream name of item."), Category("Standard"), EditorAttribute(typeof(ComboBoxPropertyEditor), typeof(UITypeEditor))]
        public string SourceDataStream
        {
            get 
            {
                ComboBoxPropertyEditor.ItemList = null;

                int tmpCount = EditorController.Instance.EditorProject.DataStreams.Count;
                if(tmpCount > 0)
                {
                    string[] tmpList = new string[tmpCount];
                    int i = 0;
                    foreach(DataStream tmpItem in EditorController.Instance.EditorProject.DataStreams)
                    {
                        tmpList[i] = tmpItem.Name;
                        i++;
                    }
                    ComboBoxPropertyEditor.ItemList = tmpList;
                }

                return sourceDataStream; 
            }
            set 
            {
                SourceColumn = "";
                sourceDataStream = value;
                needToUpdateSize = true;
            }
        }


        [Browsable(false)]
        public string Text
        {
            get
			{ 
				if (SourceColumn.Length > 0 && SourceDataStream.Length > 0)
				{                    
                    if (EditorController.Instance.MainFormOfTheProject != null && EditorController.Instance.MainFormOfTheProject.InPreviewMode)
                    {
                        return string.Format("{0}:{1}:{2}", this.SourceDataStream, this.SourceColumn, this.creationId);
                    }
                    else
                    {
                        return string.Format("{0}:{1}", this.SourceDataStream, this.SourceColumn);
                    }
				}
				else 
				{
					return "DataStream:ColumnName";
				}
				
			}          
        }

        

        /// <summary>
        /// Relative X location comparing to ballon location.
        /// </summary>
        [Browsable(true), DisplayName("Location X In Pixels"), Category("Standard")]
        [Description("Relative X location comparing to ballon location.")]
        public override float LocationInPixelsX
        {
            get
            {
                return base.LocationInPixelsX;
            }
        }


        /// <summary>
        /// Relative Y location comparing to ballon location.
        /// </summary>
        [Browsable(true), DisplayName("Location Y In Pixels"), Category("Standard")]
        [Description("Relative Y location comparing to ballon location.")]
        public override float LocationInPixelsY
        {
            get
            {
                return base.LocationInPixelsY;
            }
        }
		
		
		public override string VisibleText
		{
			get
			{
				return this.Text;
			}		
		}
               
     

        #region EditorToolBarPlugin Members

        void EditorToolBarPlugin.AddToToolStrip(ToolStrip toolStrip, ToolStripGroup group)
        {
            //ToolStripButton tbbNew = new ToolStripButton("Dynamic Text");
            //tbbNew.Image = AxiomCoders.PdfTemplateEditor.Properties.Resources.DynamicText;
            //tbbNew.DisplayStyle = ToolStripItemDisplayStyle.Image;
            //toolStrip.Items.Add(tbbNew);
            //tbbNew.Click += new EventHandler(tbbNew_Click);
        }

        void tbbNew_Click(object sender, EventArgs e)
        {
            //ToolStripButton tbbButton = sender as ToolStripButton;
            //tbbButton.Checked = !tbbButton.Checked;

            //if (tbbButton.Checked)
            //{
            //    EditorController.Instance.TbbCheckedForCreate = tbbButton;
            //    EditorController.Instance.EditorItemSelectedForCreation = this.GetType();
            //}
            //else
            //{
            //    EditorController.Instance.EditorItemSelectedForCreation = null;
            //}
        }



        #endregion
    }
}
