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
    public class StaticText : BaseTextItems, EditorToolBarPlugin
    {
        private string text;

        public StaticText(): base()
        {
            text = "StaticText";
        }


        public override void SaveItem(XmlDocument doc, XmlElement element)
        {
			XmlElement el = doc.CreateElement("Item");
			base.SaveItem(doc, el);

			XmlAttribute attr = doc.CreateAttribute("Type");
			attr.Value = "StaticText";
			el.SetAttributeNode(attr);

			attr = doc.CreateAttribute("Version");
			attr.Value = "1.0";
			el.SetAttributeNode(attr);

			XmlElement el2 = doc.CreateElement("Text");
			attr = doc.CreateAttribute("Value");
			attr.Value = this.Text;
			el2.SetAttributeNode(attr);
			el.AppendChild(el2);

			element.AppendChild(el);

            /*UnitsManager unitMng = new UnitsManager();

            txW.WriteStartElement("Item");
            txW.WriteAttributeString("Type", "StaticText");
            txW.WriteAttributeString("version", "1.0");
            txW.WriteRaw("\n");
            txW.WriteStartElement("Location");
            txW.WriteAttributeString("PositionX", LocationInUnitsX.ToString() + unitMng.UnitToString(MeasureUnit));
            txW.WriteAttributeString("PositionY", LocationInUnitsY.ToString() + unitMng.UnitToString(MeasureUnit));
            txW.WriteEndElement();
            txW.WriteRaw("\n");
            txW.WriteStartElement("DockPosition");
            txW.WriteAttributeString("Dock", DockPosition);
            txW.WriteEndElement();
            txW.WriteRaw("\n");
            txW.WriteStartElement("Scale");
            txW.WriteAttributeString("x", ScaleXFactor.ToString());
            txW.WriteAttributeString("y", ScaleYFactor.ToString());
            txW.WriteEndElement();
            txW.WriteRaw("\n");
            txW.WriteStartElement("Transformation");
            txW.WriteAttributeString("a", TransformationMatrix.Elements[0].ToString());
            txW.WriteAttributeString("b", TransformationMatrix.Elements[1].ToString());
            txW.WriteAttributeString("c", TransformationMatrix.Elements[2].ToString());
            txW.WriteAttributeString("d", TransformationMatrix.Elements[3].ToString());
            txW.WriteEndElement();
            txW.WriteRaw("\n");
            txW.WriteStartElement("Font");
            txW.WriteAttributeString("Name", Font.Name);
            txW.WriteAttributeString("Size", Font.SizeInPoints + "pt");
            txW.WriteAttributeString("Color", "rrggbb");
            txW.WriteEndElement();
            txW.WriteRaw("\n");
            txW.WriteStartElement("Text");
            txW.WriteAttributeString("Value", Text);
            txW.WriteEndElement();
            txW.WriteRaw("\n");
            txW.WriteEndElement();*/
        }

        public override void Load(System.Xml.XmlNode element)
        {
			base.Load(element);

			// Load text
			XmlNode node = element.SelectSingleNode("Text");
			if (node != null)
			{
				this.Text = node.Attributes["Value"].Value;
			}			

            
            //UnitsManager unitMng = new UnitsManager();

          /*  while (txR.Name != "Location")
            {
                txR.Read();
            }
            MeasureUnit = unitMng.StringToUnit(txR.GetAttribute(0));
            LocationInUnitsX = (float)(System.Convert.ToDouble(txR.GetAttribute(0).TrimEnd(unitMng.UnitToString(MeasureUnit).ToCharArray())));
            LocationInUnitsY = (float)(System.Convert.ToDouble(txR.GetAttribute(1).TrimEnd(unitMng.UnitToString(MeasureUnit).ToCharArray())));
            while(txR.Name != "DockPosition")
            {
                txR.Read();
            }
            DockPosition = txR.GetAttribute(0);
            while (txR.Name != "Scale")
            {
                txR.Read();
            }
            ScaleXFactor = (float)System.Convert.ToDouble(txR.GetAttribute(0));
            ScaleYFactor = (float)System.Convert.ToDouble(txR.GetAttribute(1));
            while (txR.Name != "Transformation")
            {
                txR.Read();
            }
            TransformationMatrix.Elements[0] = (float)System.Convert.ToDouble(txR.GetAttribute(0));
            TransformationMatrix.Elements[1] = (float)System.Convert.ToDouble(txR.GetAttribute(1));
            TransformationMatrix.Elements[2] = (float)System.Convert.ToDouble(txR.GetAttribute(2));
            TransformationMatrix.Elements[3] = (float)System.Convert.ToDouble(txR.GetAttribute(3));
            while (txR.Name != "Font")
            {
                txR.Read();
            }
            string tmpFntName = txR.GetAttribute(0);
            float tmpFntSize = (float)(System.Convert.ToDouble(txR.GetAttribute(1).TrimEnd(unitMng.UnitToString(MeasureUnits.point).ToCharArray())));
            Font = new Font(tmpFntName, tmpFntSize);
            while (txR.Name != "Text")
            {
                txR.Read();
            }
            Text = txR.GetAttribute(0);*/
        }

                
        
        [Browsable(true), Description("Text to display in control"), Category("Standard")]
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
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
            //ToolStripButton tbbNew = new ToolStripButton("Static Text");
            //tbbNew.Image = AxiomCoders.PdfTemplateEditor.Properties.Resources.StaticText;
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
