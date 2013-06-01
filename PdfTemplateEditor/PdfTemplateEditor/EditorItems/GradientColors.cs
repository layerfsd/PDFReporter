using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml;
using AxiomCoders.PdfTemplateEditor.Common;

namespace AxiomCoders.PdfTemplateEditor.EditorItems
{
    [Serializable]
    [TypeConverter(typeof(GradientColorConverter))]
    [Editor(typeof(GradientEditor), typeof(UITypeEditor))]
	[System.Reflection.Obfuscation(Exclude = true)]
    public class GradientDefinition
    {
        private Color color1;
        private Color color2;

		private float angle;
		public float Angle
		{
			get { return angle; }
			set { angle = value; }
		}

		private float blendPosition1;
		public float BlendPosition1
		{
			get { return blendPosition1; }
			set { blendPosition1 = value; }
		}

		private float blendPosition2;
		public float BlendPosition2
		{
			get { return blendPosition2; }
			set { blendPosition2 = value; }
		}

        private PointF point1;
        private PointF point2;
        private GradientType gradientType;
		private float startLocationX;
		private float startLocationY;
		private bool useCMYK;
		private float gradientSize;
		private float factor;
		private int functionType = 2;

        public GradientDefinition()
        {
            color1 = Color.White;
            color2 = Color.Black;
			point1 = new PointF(0.0f, 0.0f);
			point2 = new PointF(1.0f, 0.0f);
			blendPosition1 = 0.0f;
			blendPosition2 = 1.0f;
            gradientType = GradientType.Linear;
        }

        public GradientDefinition(Color c1,Color c2,PointF p1,PointF p2,GradientType type)
        {
            color1 = c1;
            color2 = c2;
            point1 = p1;
            point2 = p2;
            gradientType = type;
        }

		public void Save(XmlDocument doc, XmlElement element)
		{
			UnitsManager unitMng = new UnitsManager();

			XmlElement el = doc.CreateElement("Shading");
			XmlAttribute attr = doc.CreateAttribute("Type");
			attr.Value = ((int)gradientType).ToString();
			el.SetAttributeNode(attr);			
			element.AppendChild(el);		
			

			el = doc.CreateElement("AxialCoords");
			attr = doc.CreateAttribute("FromX");
			attr.Value = this.Point1.X.ToString();
			el.SetAttributeNode(attr);

			attr = doc.CreateAttribute("FromY");
			attr.Value = this.Point1.Y.ToString();
			el.SetAttributeNode(attr);

			attr = doc.CreateAttribute("ToX");
			attr.Value = this.Point2.X.ToString();
			el.SetAttributeNode(attr);

			attr = doc.CreateAttribute("ToY");
			attr.Value = this.Point2.Y.ToString();
			el.SetAttributeNode(attr);

			element.AppendChild(el);

			// function type
			el = doc.CreateElement("FunctionType");
			attr = doc.CreateAttribute("Type");
			attr.Value = this.functionType.ToString();
			el.SetAttributeNode(attr);
			element.AppendChild(el);

			el = doc.CreateElement("Color");
			attr = doc.CreateAttribute("FromR"); attr.Value = this.Color1.R.ToString(); el.SetAttributeNode(attr);
			attr = doc.CreateAttribute("FromG"); attr.Value = this.Color1.G.ToString(); el.SetAttributeNode(attr);
			attr = doc.CreateAttribute("FromB"); attr.Value = this.Color1.B.ToString(); el.SetAttributeNode(attr);
			attr = doc.CreateAttribute("ToR"); attr.Value = this.Color2.R.ToString(); el.SetAttributeNode(attr);
			attr = doc.CreateAttribute("ToG"); attr.Value = this.Color2.G.ToString(); el.SetAttributeNode(attr);
			attr = doc.CreateAttribute("ToB"); attr.Value = this.Color2.B.ToString(); el.SetAttributeNode(attr);
			element.AppendChild(el);

            el = doc.CreateElement("GradientDefinition");
            attr = doc.CreateAttribute("Angle"); attr.Value = this.Angle.ToString(); el.SetAttributeNode(attr);
            attr = doc.CreateAttribute("BlendPosition1"); attr.Value = this.BlendPosition1.ToString(); el.SetAttributeNode(attr);
            attr = doc.CreateAttribute("BlendPosition2"); attr.Value = this.BlendPosition2.ToString(); el.SetAttributeNode(attr);
            element.AppendChild(el);

		}

		public void Load(RectangleShape shape, XmlNode element)
		{
			UnitsManager unitMng = new UnitsManager();
			XmlNode node;		

			// Load shading definition
			node = element.SelectSingleNode("Shading");
			if (node != null)
			{			
				this.gradientType = (GradientType)Convert.ToInt32(node.Attributes["Type"].Value);
			}

			// Load axial cords
			node = element.SelectSingleNode("AxialCoords");
			if (node != null)
			{
				float x = (float)Convert.ToDouble(node.Attributes["FromX"].Value);
				float y = (float)Convert.ToDouble(node.Attributes["FromY"].Value);
				this.Point1 = new PointF(x, y);

				x = (float)Convert.ToDouble(node.Attributes["ToX"].Value);
				y = (float)Convert.ToDouble(node.Attributes["ToY"].Value);
				this.Point2 = new PointF(x, y);
			}

			// Load function
			node = element.SelectSingleNode("Function");
			if (node != null)
			{
				this.functionType = Convert.ToInt32(node.Attributes["Type"]);
			}

			// Load color
			node = element.SelectSingleNode("Color");
			if (node != null)
			{
				int r = Convert.ToInt32(node.Attributes["FromR"].Value);
				int g = Convert.ToInt32(node.Attributes["FromG"].Value);
				int b = Convert.ToInt32(node.Attributes["FromB"].Value);
				this.Color1 = Color.FromArgb(r, g, b);

				r = Convert.ToInt32(node.Attributes["ToR"].Value);
				g = Convert.ToInt32(node.Attributes["ToG"].Value);
				b = Convert.ToInt32(node.Attributes["ToB"].Value);
				this.Color2 = Color.FromArgb(r, g, b);
			}

            //Load gradient definition
            node = element.SelectSingleNode("GradientDefinition");
            if(node != null)
            {
                this.Angle = (float)Convert.ToDouble(node.Attributes["Angle"].Value);
                this.BlendPosition1 = (float)Convert.ToDouble(node.Attributes["BlendPosition1"].Value);
                this.BlendPosition2 = (float)Convert.ToDouble(node.Attributes["BlendPosition2"].Value);
            }
		}


        public Color Color1
        {
            get
            {
                return color1;
            }
            set
            {
                color1 = value;
            }
        }

        public Color Color2
        {
            get
            {
                return color2;
            }
            set
            {
                color2 = value;
            }
        }

        
        [Browsable(false)]
        public PointF Point1
        {
            get
            {
                return point1;
            }
            set
            {
                point1 = value;
            }
        }

        [Browsable(false)]
        public PointF Point2
        {
            get
            {
                return point2;
            }
            set
            {
                point2 = value;
            }
        }


        public GradientType GradientType
        {
            get
            {
                return gradientType;
            }
            set
            {
                gradientType = value;
            }
        }
    }
}
