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
    class DynamicImage : EditorItem , EditorToolBarPlugin, DynamicEditorItemInterface
    {
        private string sourceColumn;
        private string sourceDataStream;
        Image pictureForDisplay = AxiomCoders.PdfTemplateEditor.Properties.Resources.NoDynamicImage;

		/// <summary>
		/// Constructor
		/// </summary>
        public DynamicImage()
        {
            sourceColumn = string.Empty;
            sourceDataStream = string.Empty;

            this.WidthInUnits = 3;
            this.HeightInUnits = 3;
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
			attr.Value = "DynamicImage";
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

            attr = doc.CreateAttribute("ImageType");
            attr.Value = this.ImageType;
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
                this.ImageType = node.Attributes["ImageType"].Value;
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
            }
        }


        private string imageType = "Data";
        /// <summary>
        /// Type of image, bmp, DataJpg, DataGif, DataBmp, FileSystem
        /// </summary>
        [Browsable(true), DisplayName("Image Type"), Description("Image type"), Category("Standard"), EditorAttribute(typeof(ComboBoxPropertyEditor), typeof(UITypeEditor))]
        public string ImageType
        {
            get
            {
                ComboBoxPropertyEditor.ItemList = null;
                
                string[] tmpList = new string[2];
                tmpList[0] = "FileSystem";
                tmpList[1] = "Data";
                ComboBoxPropertyEditor.ItemList = tmpList;

                return imageType;
            }
            set
            {
                imageType = value;
            }
        }


        

        /// <summary>
        /// Relative X location comparing to ballon location.
        /// </summary>
        [Browsable(true), DisplayName("Location X In Pixels"), Category("Standard")]
        [Description("Relative X location comparing to balloon location.")]
        public override float LocationInPixelsX
        {
            get
            {
                return base.LocationInPixelsX;
            }
        }


        /// <summary>
        /// Relative Y location comparing to balloon location.
        /// </summary>
        [Browsable(true), DisplayName("Location Y In Pixels"), Category("Standard")]
        [Description("Relative Y location comparing to balloon location.")]
        public override float LocationInPixelsY
        {
            get
            {
                return base.LocationInPixelsY;
            }
        }


        public override void DisplayItem(Graphics gc)
        {
            if(IsSelected)
            {
                if(pictureForDisplay != null)
                {
                    gc.DrawImage(pictureForDisplay, 0, 0, (float)this.WidthInPixels, (float)this.HeightInPixels);
                }
                //gc.DrawRectangle(Pens.Red, 0, 0, (float)this.WidthInPixels, (float)this.HeightInPixels);
            }
            else
            {
                if(pictureForDisplay != null)
                {
                    gc.DrawImage(pictureForDisplay, 0, 0, (float)this.WidthInPixels, (float)this.HeightInPixels);
                }
                else
                {
                    //if (Disabled)
                    //{
                    //    gc.DrawRectangle(Pens.LightGray, 0, 0, (float)this.WidthInPixels, (float)this.HeightInPixels);
                    //}
                    //else
                    //{
                    gc.DrawRectangle(Pens.Black, 0, 0, (float)this.WidthInPixels, (float)this.HeightInPixels);
                    //}
                }
            }
        }
     

        #region EditorToolBarPlugin Members

        void EditorToolBarPlugin.AddToToolStrip(ToolStrip toolStrip, ToolStripGroup group)
        {
            //ToolStripButton tbbNew = new ToolStripButton("DynamicImage");
            //tbbNew.Image = AxiomCoders.PdfTemplateEditor.Properties.Resources.DynamicImage;
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
