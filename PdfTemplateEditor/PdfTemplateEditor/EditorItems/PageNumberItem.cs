using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.Xml;

using AxiomCoders.PdfTemplateEditor.EditorStuff;

namespace AxiomCoders.PdfTemplateEditor.EditorItems
{
	[System.Reflection.Obfuscation(Exclude = true)]
    class PageNumberItem : BaseTextItems, EditorToolBarPlugin
    {
        private string pageNumberFormat; 
        private string formatValue;
        private int pageNumber;
        private int numOfPages;

        public PageNumberItem(): base()
        {           
            formatValue = "Page:{P}"; //P for current page, p for number of pages.           
            pageNumber = 1;
            numOfPages = 1;
        }

        /// <summary>
        /// Standard time format pattern.
        /// </summary>
        [Browsable(true), DisplayName("Format"), Category("Standard")]
        [Description("Standard page number format. Write \"{P}\" for page number.")]
        public string FormatValue
        {
            get { return formatValue; }
            set
            {
                formatValue = value;
                // Update size of item when text is changed				
                needToUpdateSize = true;
            }
        }


        public override void SaveItem(XmlDocument doc, XmlElement element)
        {
			XmlElement el = doc.CreateElement("Item");
			base.SaveItem(doc, el);

			XmlAttribute attr = doc.CreateAttribute("Type");
			attr.Value = "PageNumberItem";
			el.SetAttributeNode(attr);

			attr = doc.CreateAttribute("Version");
			attr.Value = "1.0";
			el.SetAttributeNode(attr);

			XmlElement el2 = doc.CreateElement("Format");
			attr = doc.CreateAttribute("Value");
			attr.Value = this.FormatValue;
			el2.SetAttributeNode(attr);
			el.AppendChild(el2);


			element.AppendChild(el);
        }




        public override void Load(System.Xml.XmlNode element)
        {
			base.Load(element);			

			// Load FormatValue
			XmlNode node = element.SelectSingleNode("Format");
			if (node != null)
			{
				this.FormatValue = node.Attributes["Value"].Value;
			}
        }





        private string GetValueText
        {
            get
            {
                try
                {
                    string result = formatValue.Replace("{P}", "N0");
                    result = result.Replace("{p}", "N0");
                    return result;
                }
                catch
                {
                    return "Invalid format value.";
                }
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
        /// Relative Y location comparing to ballon location.
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


       

		public override string VisibleText
		{
			get
			{
				return this.GetValueText;
			}
		}




        #region EditorToolBarPlugin Members

        void EditorToolBarPlugin.AddToToolStrip(ToolStrip toolStrip, ToolStripGroup group)
        {
            //ToolStripButton tbbNew = new ToolStripButton("PageNumber");
            //tbbNew.Image = AxiomCoders.PdfTemplateEditor.Properties.Resources.PageNum;
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
