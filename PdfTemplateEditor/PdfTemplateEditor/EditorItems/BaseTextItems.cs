using System;
using System.Collections.Generic;
using System.Text;
using AxiomCoders.PdfTemplateEditor.EditorStuff;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;

using System.Xml;
using AxiomCoders.PdfTemplateEditor.Common;

namespace AxiomCoders.PdfTemplateEditor.EditorItems
{
	[System.Reflection.Obfuscation(Exclude = true)]
	public class BaseTextItems: EditorItem
	{
		protected Font font;
        private int fontSaveId;

        /// <summary>
        /// Font Save ID
        /// </summary>
        [Browsable(false)]
        public int FontSaveId
        {
            get { return fontSaveId; }
            set { fontSaveId = value; }
        }
        
		protected bool needToUpdateSize;
		protected Color foreColor;

        /// <summary>
        /// Override and hide this for browsing
        /// </summary>
        [Browsable(false)]
        public override string DockPositionString
        {
            get
            {
                return base.DockPositionString;
            }
            set
            {
                base.DockPositionString = value;
            }
        }

		public override void MakeDrawMatrix(System.Drawing.Drawing2D.Matrix parentMatrix)
		{			
			drawMatrix.Reset();
			// translate by size of text in y
			float height = this.Font.GetHeight(UnitsManager.Instance.Resolution);
			int   iCellSpace  = Font.FontFamily.GetLineSpacing(Font.Style);
			int   iCellAscent = Font.FontFamily.GetCellAscent(Font.Style);
			float cyAscent    = height * iCellAscent / iCellSpace;

			drawMatrix.Translate(0, -cyAscent);
			drawMatrix.Multiply(this.TransformationMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
			drawMatrix.Multiply(parentMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
			
			foreach (EditorItem item in Children)
			{
				item.MakeDrawMatrix(drawMatrix);
			}			
		}

		protected BaseTextItems()
		{
            font = new Font("Arial", 12.0f);
			foreColor = Color.FromArgb(0,0,0);            
            foreColor = Color.Black;
            needToUpdateSize = true;
            scaleXFactor = 1;
            scaleYFactor = 1;
		}

		/// <summary>
		/// Fore color - text color
		/// </summary>
        [Browsable(true),DisplayName("Fore Color"), Category("Standard"), Description("Text color")]
		public Color ForeColor
		{
			get { return foreColor; }
			set { foreColor = value; }
		}


        /// <summary>
        /// This will return drawing font by actually use it from editorFont
        /// </summary>
		[Browsable(true), Description("Font"), Category("Standard")]
		public System.Drawing.Font Font
		{
			get { return font; }
			set
			{
				font = value;
				// update size of item when font is changed
				needToUpdateSize = true;
			}
		}

		/// <summary>
		/// Main load method
		/// </summary>
		/// <param name="element"></param>
		public override void Load(System.Xml.XmlNode element)
		{
			base.Load(element);

			UnitsManager unitMng = new UnitsManager();
            EditorFont editorFont = null;
            float fontSize = 12.0f;

			// Load font for older versions is kept here.
            // should be removed on first public release 
			XmlNode node = element.SelectSingleNode("Font");
			if (node != null)
			{
                int fontId = Convert.ToInt32(node.Attributes["SaveID"].Value);
                fontSize = (float)Convert.ToDouble(node.Attributes["FontSize"].Value);
                // Load font from font manager
                editorFont = FontManager.Instance.FindFont(fontId);
            }           

            // Load font color
			int r = 0, g = 0, b = 0;
            if (node != null)
            {
                XmlAttribute attr = (XmlAttribute) node.Attributes.GetNamedItem("FontColorR");
                if (attr != null)
                {
                    r = Convert.ToInt32(attr.Value);
                }
                attr = (XmlAttribute) node.Attributes.GetNamedItem("FontColorG");
                if (attr != null)
                {
                    g = Convert.ToInt32(attr.Value);
                }
                attr = (XmlAttribute) node.Attributes.GetNamedItem("FontColorB");
                if (attr != null)
                {
                    b = Convert.ToInt32(attr.Value);
                }
            }

		    this.ForeColor = Color.FromArgb(r, g, b);

            if (editorFont == null)
            {
                MessageBox.Show("Font cannot be found in saved document. It will be replaced with the default one");
                font = new Font("Arial", fontSize);
            }
            else
            {
                // make new font based on loaded editor font 
               this.Font = new Font(editorFont.Font.Name, fontSize,
                               editorFont.Font.Style, editorFont.Font.Unit,
                               editorFont.Font.GdiCharSet, editorFont.Font.GdiVerticalFont);
            }
		}

        

		public override void SaveItem(XmlDocument doc, XmlElement element)
		{
			// temporarly change location to bottom text line			
			base.SaveItem(doc, element);		

			// save font reference and properites
			XmlElement el = doc.CreateElement("Font");
			XmlAttribute attr = doc.CreateAttribute("SaveID");
            attr.Value = this.FontSaveId.ToString();
			el.SetAttributeNode(attr);

            attr = doc.CreateAttribute("FontSize");
            attr.Value = this.Font.SizeInPoints.ToString();
            el.SetAttributeNode(attr);
            
            // save fore color of font            
			attr = doc.CreateAttribute("FontColorR");
			attr.Value = this.ForeColor.R.ToString();
			el.SetAttributeNode(attr);

            attr = doc.CreateAttribute("FontColorG");
			attr.Value = this.ForeColor.G.ToString();
			el.SetAttributeNode(attr);

            attr = doc.CreateAttribute("FontColorB");
			attr.Value = this.ForeColor.B.ToString();
			el.SetAttributeNode(attr);

			element.AppendChild(el);
		}

		private string visibleText = "none";

		[Browsable(false)]
		public virtual string VisibleText
		{
			get { return visibleText; }
			set { visibleText = value; }
		}

		/// <summary>
		/// This will update size of control according to text property
		/// </summary>
		protected void UpdateSize(Graphics gc, Font font)
		{
			if (needToUpdateSize)
			{
				SizeF strSize = gc.MeasureString(this.VisibleText, font);
				WidthInPixels = strSize.Width;
				HeightInPixels = strSize.Height;
				needToUpdateSize = false;
			}
		}

		public override void DisplayItem(Graphics gc)
		{
			gc.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

			float size = UnitsManager.Instance.ConvertUnit(this.Font.Size, UnitsManager.Instance.ConvertFromGraphicUnit(this.Font.Unit), MeasureUnits.pixel);
			Font visibleFont = new Font(this.Font.FontFamily, size, this.Font.Style, GraphicsUnit.Pixel);
						
			//visibleFont = this.Font;
			UpdateSize(gc, visibleFont);			

            //if (Disabled)
            //{
            //    gc.DrawString(this.VisibleText, visibleFont, Brushes.LightGray, 0, 0);
            //}
            //else
            //{
				Brush b = new SolidBrush(this.ForeColor);
				gc.DrawString(this.VisibleText, visibleFont, b, 0, 0);
			//}						
		}

	

	}
}
