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
    public class Counter : BaseTextItems, EditorToolBarPlugin
    {
        private string cunterFormat;
        private int startValue;
        private int endValue;
        private int intervalValue;
        private bool looping;        
        private string formatValue;
        private int currentValue;

        public Counter(): base()
        {
            startValue = 0;
            endValue = 100;
            looping = false;
            intervalValue = 1;
            formatValue = "Something: {C}, of {E} where begin is: {S}";          
            currentValue = startValue;
        }


        /// <summary>
        /// Initial counter value.
        /// </summary>
        [Browsable(true), DisplayName("Start Value"), Category("Standard")]
        [Description("Initial counter value.")]
        public int StartValue
        {
            get
            {
                return startValue;
            }
            set
            {
                startValue = value;
            }
        }
        

        /// <summary>
        /// Maximal counter value, 0 means that there is no END.
        /// </summary>
        [Browsable(true), DisplayName("End Value"), Category("Standard")]
        [Description("Maximal counter value, 0 means that there is no END.")]
        public int EndValue
        {
            get
            {
                return endValue;
            }
            set
            {
                endValue = value;
            }
        }

        /// <summary>
        /// Interval value for counter.
        /// </summary>
        [Browsable(true), Category("Standard")]
        [Description("Interval value for counter.")]
        public int IntervalValue
        {
            get
            {
                return intervalValue;
            }
            set
            {
                intervalValue = value;
                needToUpdateSize = true;
            }
        }

        /// <summary>
        /// Loop counter. 
        /// If value set to true, then counter will looping, otherwise whene set to false then counter don't loop.
        /// </summary>
        [Browsable(true), Category("Standard")]
        [Description("Loop counter. If value set to true, then counter will looping, otherwise whene set to false then counter don't loop.")]
        public bool Looping
        {
            get
            {
                return looping;
            }
            set
            {
                looping = value;
            }
        }


        /// <summary>
        /// Standard time format pattern.
        /// </summary>
        [Browsable(true), DisplayName("Format"), Category("Standard")]
        [Description("Formating string:\n {S} - Start value\n {E} - End value\n {C} - Counting value")]
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
			attr.Value = "Counter";
			el.SetAttributeNode(attr);

			attr = doc.CreateAttribute("Version");
			attr.Value = "1.0";
			el.SetAttributeNode(attr);

			XmlElement el2 = doc.CreateElement("Start");
			attr = doc.CreateAttribute("Value");
			attr.Value = this.StartValue.ToString();
			el2.SetAttributeNode(attr);
			el.AppendChild(el2);

			el2 = doc.CreateElement("End");
			attr = doc.CreateAttribute("Value");
			attr.Value = this.EndValue.ToString();
			el2.SetAttributeNode(attr);
			el.AppendChild(el2);

			el2 = doc.CreateElement("Loop");
			attr = doc.CreateAttribute("Value");
			attr.Value = this.Looping.ToString();
			el2.SetAttributeNode(attr);
			el.AppendChild(el2);

			el2 = doc.CreateElement("Interval");
			attr = doc.CreateAttribute("Value");
			attr.Value = this.IntervalValue.ToString();
			el2.SetAttributeNode(attr);
			el.AppendChild(el2);

			el2 = doc.CreateElement("Format");
			attr = doc.CreateAttribute("Value");
			attr.Value = this.FormatValue;
			el2.SetAttributeNode(attr);
			el.AppendChild(el2);

			element.AppendChild(el);
         
        }

        public override void Load(System.Xml.XmlNode element)
        {
			base.Load(element);

			// Load StartValue
			XmlNode node = element.SelectSingleNode("Start");
			if (node != null)
			{
				this.StartValue = Convert.ToInt32(node.Attributes["Value"].Value);
			}			
			// Load EndValue
			node = element.SelectSingleNode("End");
			if (node != null)
			{
				this.EndValue = Convert.ToInt32(node.Attributes["Value"].Value);
			}		
			// Load Looping
			node = element.SelectSingleNode("Loop");
			if (node != null)
			{
				this.Looping = Convert.ToBoolean(node.Attributes["Value"].Value);
			}		
			// Load IntervalValue
			node = element.SelectSingleNode("Interval");
			if (node != null)
			{
				this.IntervalValue = Convert.ToInt32(node.Attributes["Value"].Value);
			}		

			// Load FormatValue
			node = element.SelectSingleNode("Format");
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
                    string result = formatValue.Replace("{S}", startValue.ToString("N0"));
                    result = result.Replace("{E}", endValue.ToString("N0"));
                    return result.Replace("{C}", currentValue.ToString("N0"));
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
            //ToolStripButton tbbNew = new ToolStripButton("Counter");
            //tbbNew.Image = AxiomCoders.PdfTemplateEditor.Properties.Resources.Counter;
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
