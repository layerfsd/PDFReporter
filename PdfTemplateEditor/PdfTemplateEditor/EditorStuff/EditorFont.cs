using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using AxiomCoders.PdfTemplateEditor.Common;
using System.Xml;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
    /// <summary>
    /// This is font class used in editor. It should be used instead of regular Font everywhere in application
    /// </summary>
    public class EditorFont: SaveLoadMechanism
    {
        private Font font;

        /// <summary>
        /// number used for referencing on what is saved and loaded
        /// </summary>
        private int saveID;

        /// <summary>
        /// Number used for saving and loading
        /// </summary>
        public int SaveID
        {
            get { return saveID; }
            set { saveID = value; }
        } 

        /// <summary>
        /// font object
        /// </summary>
        public Font Font
        {
            get { return font; }
            set { font = value; }
        }

        /// <summary>
        /// Constructor from regular font
        /// </summary>
        /// <param name="font"></param>
        public EditorFont(Font font): this()
        {
            this.font = font;
        }

        private static int lastSaveId = 1;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public EditorFont()
        {
            this.saveID = lastSaveId;
            lastSaveId++;
        }

        #region SaveLoadMechanism Members

        /// <summary>
        /// Save font. It will create this
        ///  <font Name="something" StemV="20" Width="[234 34]"><EmbeddedFont>dasjdlkajsdljxlkjsaldjasldjxflj</embeddedFont></font>        
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="element"></param>
        public void Save(System.Xml.XmlDocument doc, System.Xml.XmlElement element)
        {
            // save font
            XmlElement el = doc.CreateElement("Font");
            XmlAttribute attr = doc.CreateAttribute("Name");
            attr.Value = this.Font.Name;
            el.SetAttributeNode(attr);

            attr = doc.CreateAttribute("SaveID");
            attr.Value = this.SaveID.ToString();
            el.SetAttributeNode(attr);

            // save metrics
            attr = doc.CreateAttribute("EmHeight");
            attr.Value = this.Font.FontFamily.GetEmHeight(this.Font.Style).ToString();
            el.SetAttributeNode(attr);

            attr = doc.CreateAttribute("Ascent");
            attr.Value = this.Font.FontFamily.GetCellAscent(this.font.Style).ToString();
            el.SetAttributeNode(attr);

            attr = doc.CreateAttribute("Descent");
            attr.Value = this.Font.FontFamily.GetCellDescent(this.font.Style).ToString();
            el.SetAttributeNode(attr);

            attr = doc.CreateAttribute("Bold");
            attr.Value = this.Font.Bold ? "1" : "0";
            el.SetAttributeNode(attr);

            attr = doc.CreateAttribute("Italic");
            attr.Value = this.Font.Italic ? "1" : "0";
            el.SetAttributeNode(attr);

            //  Get font metrics to save some properties of font
            FontMetrics metrics = new FontMetrics(this.Font);

            attr = doc.CreateAttribute("EmbeddingLicense");
            attr.Value = ((int)metrics.EmbeddingLicense).ToString();
            el.SetAttributeNode(attr);

            attr = doc.CreateAttribute("ItalicAngle");
            attr.Value = metrics.ItalicAngle.ToString();
            el.SetAttributeNode(attr);

            attr = doc.CreateAttribute("BBoxLeft"); attr.Value = metrics.FontBBox.Left.ToString(); el.SetAttributeNode(attr);
            attr = doc.CreateAttribute("BBoxRight"); attr.Value = metrics.FontBBox.Right.ToString(); el.SetAttributeNode(attr);
            attr = doc.CreateAttribute("BBoxTop"); attr.Value = metrics.FontBBox.Top.ToString(); el.SetAttributeNode(attr);
            attr = doc.CreateAttribute("BBoxBottom"); attr.Value = metrics.FontBBox.Bottom.ToString(); el.SetAttributeNode(attr);

            attr = doc.CreateAttribute("StemV");
            attr.Value = metrics.StemV.ToString();
            el.SetAttributeNode(attr);

            attr = doc.CreateAttribute("Flags");
            attr.Value = metrics.Flags.ToString();
            el.SetAttributeNode(attr);

            attr = doc.CreateAttribute("FirstChar"); attr.Value = metrics.FirstChar.ToString(); el.SetAttributeNode(attr);
            attr = doc.CreateAttribute("LastChar"); attr.Value = metrics.LastChar.ToString(); el.SetAttributeNode(attr);

            // make glyph widths
            string glyphwidth = "[ ";
            for (int i = metrics.FirstChar; i <= metrics.LastChar; i++)
            {
                int gw = metrics.GetGlyphWidth(this.font, i);
                glyphwidth += gw.ToString() + " ";
            }
            glyphwidth += " ]";

            attr = doc.CreateAttribute("Widths"); attr.Value = glyphwidth; el.SetAttributeNode(attr);        
            
            // embedd font if not std font
            if (!this.IsStandardFont)
            {
                byte[] fontBuffer = FontMetrics.GetFontData(this.Font);
                attr = doc.CreateAttribute("EmbeddedDecodedFontLength"); attr.Value = fontBuffer.Length.ToString(); el.SetAttributeNode(attr);

                XmlText textValue = doc.CreateTextNode("EmdeddedFont");
                textValue.Value = Convert.ToBase64String(fontBuffer);
                attr = doc.CreateAttribute("EmbeddedFontLength"); attr.Value = textValue.Value.Length.ToString(); el.SetAttributeNode(attr);
                el.AppendChild(textValue);                
            }
            element.AppendChild(el);
        }

        /// <summary>
        /// Return true if this is standard font like Arial
        /// </summary>
        public bool IsStandardFont
        {
            get
            {
                if (this.Font.Name.ToLower() == "arial")
                {
                    return true;
                }
                if (this.Font.Name.ToLower() == "courier")
                {
                    return true;
                }
                if (this.Font.Name.ToLower() == "helvetica")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Load font object
        /// </summary>
        /// <param name="node"></param>
        public void Load(System.Xml.XmlNode node)
        {
            UnitsManager unitMng = new UnitsManager();

            this.saveID = Convert.ToInt32(node.Attributes["SaveID"].Value);
                        
            int italic = System.Convert.ToInt32(node.Attributes["Italic"].Value);
            int bold = System.Convert.ToInt32(node.Attributes["Bold"].Value);

            FontStyle style = FontStyle.Regular;
            if (italic == 1)
            {
                style = FontStyle.Italic;
            }
            if (bold == 1)
            {
                style |= FontStyle.Bold;
            }

            this.Font = new Font(node.Attributes["Name"].Value, 12.0f, style);                
        }

        #endregion
    }
}
