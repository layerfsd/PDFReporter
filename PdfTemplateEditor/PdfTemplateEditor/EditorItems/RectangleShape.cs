using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;

using AxiomCoders.PdfTemplateEditor.EditorStuff;
using AxiomCoders.PdfTemplateEditor.Common;
using System.Drawing.Drawing2D;
using System.Xml;

namespace AxiomCoders.PdfTemplateEditor.EditorItems
{
	[System.Reflection.Obfuscation(Exclude = true)]
    public class RectangleShape : EditorItem, EditorToolBarPlugin
    {
        private int strokeWidth;
        private Color strokeColor;
        private Color fillColor;
        private GradientDefinition gradientColors;
        private bool useGradient;
		private bool useStroke;

        [Browsable(true), DisplayName("Use Stroke"), Description("Should stroke be used when drawing rectangle")]
		public bool UseStroke
		{
			get { return useStroke; }
			set { useStroke = value; }
		}


        public RectangleShape()
        {
            strokeWidth = 1;
            strokeColor = Color.Black;
			fillColor = Color.Orange;
            gradientColors = new GradientDefinition();
            useGradient = false;
			useStroke = false;
			this.WidthInUnits = 3;
			this.HeightInUnits = 1;
        }

        [Browsable(true), DisplayName("Stroke Width")]
        public int StrokeWidth
        {
            get
            {
                return strokeWidth;
            }
            set
            {
                if(value >= 1)
                    strokeWidth = value;
            }
        }

        [Browsable(true), DisplayName("Stroke Color")]
        public Color StrokeColor
        {
            get
            {
                return strokeColor;
            }
            set
            {
                strokeColor = value;
            }
        }

		[Browsable(true), DisplayName("Use Gradient"), Description("Should rectangle be drawn with gradient")]
        public bool UseGradient
        {
            get
            {
                return useGradient;
            }
            set
            {
                useGradient = value;
            }
        }


        [Browsable(true), DisplayName("Gradient Definition")]
        public GradientDefinition GradientDefinition
        {
            get
            {
                return gradientColors;
            }
            set
            {
                gradientColors = value;
            }
        }

        [Browsable(true), DisplayName("Fill Color")]
        public Color FillColor
        {
            get
            {
                return fillColor;
            }
            set
            {
                fillColor = value;
            }
        }




        public override void SaveItem(XmlDocument doc, XmlElement element)
        {
			XmlElement el = doc.CreateElement("Item");
			base.SaveItem(doc, el);

			XmlAttribute attr = doc.CreateAttribute("Type");
			attr.Value = "ShapeRectangle";
			el.SetAttributeNode(attr);

			attr = doc.CreateAttribute("Version");
			attr.Value = "1.0";
			el.SetAttributeNode(attr);

			// save rectangle properties
			// use shading
			XmlElement el2 = doc.CreateElement("UseShading");
			attr = doc.CreateAttribute("Value");
			attr.Value = this.UseGradient.ToString();
			el2.SetAttributeNode(attr);
			el.AppendChild(el2);

			el2 = doc.CreateElement("UseStroke");
			attr = doc.CreateAttribute("Value");
			attr.Value = this.UseStroke.ToString();
			el2.SetAttributeNode(attr);
			el.AppendChild(el2);			
			
			// FillColor
			el2 = doc.CreateElement("FillColor");
			attr = doc.CreateAttribute("R"); attr.Value = this.FillColor.R.ToString(); el2.SetAttributeNode(attr);
			attr = doc.CreateAttribute("G"); attr.Value = this.FillColor.G.ToString(); el2.SetAttributeNode(attr);
			attr = doc.CreateAttribute("B"); attr.Value = this.FillColor.B.ToString(); el2.SetAttributeNode(attr);			
			el.AppendChild(el2);

			el2 = doc.CreateElement("StrokeColor");
			attr = doc.CreateAttribute("R"); attr.Value = this.StrokeColor.R.ToString(); el2.SetAttributeNode(attr);
			attr = doc.CreateAttribute("G"); attr.Value = this.StrokeColor.G.ToString(); el2.SetAttributeNode(attr);
			attr = doc.CreateAttribute("B"); attr.Value = this.StrokeColor.B.ToString(); el2.SetAttributeNode(attr);
			el.AppendChild(el2);

			el2 = doc.CreateElement("StrokeWidth");
			attr = doc.CreateAttribute("Value"); 
			attr.Value = this.StrokeWidth.ToString(); 
			el2.SetAttributeNode(attr);
			el.AppendChild(el2);

			el2 = doc.CreateElement("ShadingItem");
			this.GradientDefinition.Save(doc, el2);
			el.AppendChild(el2);

			element.AppendChild(el);            
        }

        /// <summary>
        /// Load item
        /// </summary>
        /// <param name="element"></param>
        public override void Load(System.Xml.XmlNode element)
        {
			base.Load(element);            

			XmlNode node = element.SelectSingleNode("UseShading");
			if (node != null)
			{
				this.UseGradient = Convert.ToBoolean(node.Attributes["Value"].Value);
			}

			node = element.SelectSingleNode("UseStroke");
			if (node != null)
			{
				this.UseStroke = Convert.ToBoolean(node.Attributes["Value"].Value);
			}

			node = element.SelectSingleNode("FillColor");
			if (node != null)
			{
				int r = Convert.ToInt32(node.Attributes["R"].Value);
				int g = Convert.ToInt32(node.Attributes["G"].Value);
				int b = Convert.ToInt32(node.Attributes["B"].Value);
				this.FillColor = Color.FromArgb(r, g, b);
			}

			node = element.SelectSingleNode("StrokeColor");
			if (node != null)
			{
				int r = Convert.ToInt32(node.Attributes["R"].Value);
				int g = Convert.ToInt32(node.Attributes["G"].Value);
				int b = Convert.ToInt32(node.Attributes["B"].Value);
				this.StrokeColor = Color.FromArgb(r, g, b);
			}

			node = element.SelectSingleNode("StrokeWidth");
			if (node != null)
			{
				this.StrokeWidth = Convert.ToInt32(node.Attributes["Value"].Value);				
			}

			node = element.SelectSingleNode("ShadingItem");
			if (node != null)
			{
				this.GradientDefinition = new GradientDefinition();
				this.GradientDefinition.Load(this, node);
			}			
        }

        /// <summary>
        /// Display this item
        /// </summary>
        /// <param name="gc"></param>
		public override void DisplayItem(Graphics gc)
		{
			if (useGradient && gradientColors != null)
			{
				if (gradientColors.Point1 != gradientColors.Point2)
				{
					gc.CompositingMode = CompositingMode.SourceOver;
					gc.CompositingQuality = CompositingQuality.HighQuality;
					gc.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    
					using (LinearGradientBrush brush = new LinearGradientBrush(new RectangleF(0, 0, this.WidthInPixels, this.HeightInPixels), gradientColors.Color1, gradientColors.Color2, this.GradientDefinition.Angle))
					{
						
						Blend tmpBlend = new Blend(4);						
						tmpBlend.Factors = new float[] { 0.0f, 0.0f, 1.0f, 1.0f };
						tmpBlend.Positions = new float[] { 0.0f, this.GradientDefinition.BlendPosition1, this.GradientDefinition.BlendPosition2, 1.0f };

						brush.Blend = tmpBlend;
						gc.FillRectangle(brush, 0, 0, this.WidthInPixels, this.HeightInPixels);												
					}
				}
			}
			else
			{
				using (SolidBrush brush = new SolidBrush(fillColor))
				{
					gc.FillRectangle(brush, 0, 0, (float)this.WidthInPixels, (float)this.HeightInPixels);
				}
			}

			if (this.useStroke)
			{
				using (Pen borderPen = new Pen(StrokeColor, StrokeWidth))
				{
                    //if (Disabled)
                    //{
                    //    borderPen.Brush = Brushes.LightGray;
                    //    gc.DrawRectangle(borderPen, 0, 0, (float)this.WidthInPixels, (float)this.HeightInPixels);
                    //}
                    //else
                    //{
						gc.DrawRectangle(borderPen, 0, 0, (float)this.WidthInPixels, (float)this.HeightInPixels);
					//}
				}
			}
		}

       


        #region EditorToolBarPlugin Members

        void EditorToolBarPlugin.AddToToolStrip(ToolStrip toolStrip, ToolStripGroup group)
        {
            //ToolStripButton tbbNew = new ToolStripButton("Rectangle Shape");
            //tbbNew.Image = AxiomCoders.PdfTemplateEditor.Properties.Resources.RectangleShape;
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
