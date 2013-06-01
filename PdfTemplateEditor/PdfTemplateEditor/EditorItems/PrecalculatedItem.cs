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
using System.Globalization;

namespace AxiomCoders.PdfTemplateEditor.EditorItems
{
    [System.Reflection.Obfuscation(Exclude = true)]
    public class PrecalculatedItem : BaseTextItems
    {
        private string formula;
        private string formulaFormat = "%f*.2";
        private string decimalSeparator = "";
        private string groupSeparator = "";
        private string text;

        public PrecalculatedItem(): base()
        {
            decimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
            groupSeparator = NumberFormatInfo.CurrentInfo.NumberGroupSeparator;            
            formula = "";
            text = "Precalculated";            
        }

        [Browsable(false), Description("Specify format for result display. Please consult with help for details"), Category("Standard")]
        public string FormulaFormat
        {
            get { return formulaFormat; }
            set { formulaFormat = value; }
        }

        [Browsable(true), Description("This is formula to calculate resulting string.\nConsult help."), Category("Standard")]
        public string Formula
        {
            get
            {
                return formula;
            }
            set
            {
                formula = value;
            }
        }

        [Browsable(true), Description("This text is displayed when formula doesn't exist."), Category("Standard")]
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }



        public override void SaveItem(XmlDocument doc, XmlElement element)
        {
            XmlElement el = doc.CreateElement("Item");
            base.SaveItem(doc, el);

            XmlAttribute attr = doc.CreateAttribute("Type");
            attr.Value = "Precalculated";
            el.SetAttributeNode(attr);

            attr = doc.CreateAttribute("Version");
            attr.Value = "1.0";
            el.SetAttributeNode(attr);

            XmlElement el2 = doc.CreateElement("Text");
            attr = doc.CreateAttribute("Value");
            attr.Value = this.Text;
            el2.SetAttributeNode(attr);
            el.AppendChild(el2);

            XmlElement el3 = doc.CreateElement("Formula");
            attr = doc.CreateAttribute("Value");
            attr.Value = this.Formula;
            el3.SetAttributeNode(attr);
            el.AppendChild(el3);

            el3 = doc.CreateElement("FormulaFormat");
            attr = doc.CreateAttribute("Value");
            attr.Value = this.FormulaFormat;
            el3.SetAttributeNode(attr);
            el.AppendChild(el3);           

            element.AppendChild(el);
        }

        public override void Load(System.Xml.XmlNode element)
        {
            base.Load(element);

            // Load text
            XmlNode node = element.SelectSingleNode("Text");
            if(node != null)
            {
                this.Text = node.Attributes["Value"].Value;
            }	


            // Load formula
            node = element.SelectSingleNode("Formula");
            if(node != null)
            {
                this.Formula = node.Attributes["Value"].Value;
            }

            // Load formula format
            node = element.SelectSingleNode("FormulaFormat");
            if (node != null)
            {
                this.FormulaFormat = node.Attributes["Value"].Value;
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
    }
}
