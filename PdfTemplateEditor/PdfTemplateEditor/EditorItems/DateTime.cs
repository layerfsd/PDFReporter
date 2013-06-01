using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;

using AxiomCoders.PdfTemplateEditor.EditorStuff;
using AxiomCoders.PdfTemplateEditor.Common;
using System.Xml;

namespace AxiomCoders.PdfTemplateEditor.EditorItems
{
	[System.Reflection.Obfuscation(Exclude = true)]
    public class DateTime : BaseTextItems, EditorToolBarPlugin
    {
        private string formatValue;

        public DateTime(): base()
        {   
            formatValue = "dddd, dd. MMMM yyyy. Ti\\me: HH:mm:ss";
        }


        /// <summary>
        /// Standard time format pattern.
        /// </summary>
        [Browsable(true), DisplayName("Format"), Category("Standard")]
        [Description("Standard time format pattern. For legend consult help.")]
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

        public override void SaveItem(XmlDocument doc, XmlElement element)
        {
			XmlElement el = doc.CreateElement("Item");			
			base.SaveItem(doc, el);

			XmlAttribute attr = doc.CreateAttribute("Type");
			attr.Value = "DateTime";
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

			// Load format
			XmlNode node = element.SelectSingleNode("Format");
			if (node != null)
			{
				this.FormatValue = node.Attributes["Value"].Value;
			}

           
        }


		public override string VisibleText
		{
			get
			{
				return this.GetValueText;
			}
		}

        private string GetValueText
        {
            get
            {
                try
                {
                    return System.DateTime.Now.ToString(formatValue);
                }
                catch
                {
                    return "Invalid format value.";
                }
            }
        }




        #region EditorToolBarPlugin Members

        void EditorToolBarPlugin.AddToToolStrip(ToolStrip toolStrip, ToolStripGroup group)
        {
            //ToolStripButton tbbNew = new ToolStripButton("Date Time");
            //tbbNew.Image = AxiomCoders.PdfTemplateEditor.Properties.Resources.Date;
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
