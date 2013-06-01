using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;

using AxiomCoders.PdfTemplateEditor.EditorItems;


namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
    public enum BorderTypes
    {
        TOP_BORDER,
        LEFT_BORDER,
        BOTTOM_BORDER,
        RIGHT_BORDER
    }


    public struct BalloonBorders
    {
        public BalloonBorder top;
        public BalloonBorder left;
        public BalloonBorder bottom;
        public BalloonBorder right;
    }

    [System.Reflection.Obfuscation(Exclude = true)]
    public class BalloonBorder
    {
        private BorderTypes Type;
        private bool enabled;

        public BalloonBorder(BorderTypes type)
        {
            line.Brush = new SolidBrush(Color.Black);
            Type = type;
        }

        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }

        private Color lineColor = Color.Black;
        public Color LineColor
        {
            get
            {
                return lineColor;
            }
            set
            {
                lineColor = value;
                line.Brush = new SolidBrush(lineColor);
            }
        }

        private float lineWidth = 1.0f;
        public float LineWidth
        {
            get
            {
                return lineWidth;
            }
            set
            {
                lineWidth = value;
                line.Width = lineWidth;
            }
        }

        private DashStyle lineStyle = DashStyle.Solid;
        public DashStyle LineStyle
        {
            get
            {
                return lineStyle;
            }
            set
            {
                lineStyle = value;
                line.DashStyle = lineStyle;
            }
        }

        private Pen line = new Pen(Brushes.Black);
        public Pen Line
        {
            get
            {
                return line;
            }
        }

        public void Draw(Balloon parent, Graphics gc)
        {
            float x1 = 0.0f, y1 = 0.0f, x2 = 0.0f, y2 = 0.0f;

            switch(this.Type)
            {
                case BorderTypes.TOP_BORDER:
                    x1 = 0;
                    y1 = 0;
                    x2 = parent.WidthInPixels;
                    y2 = y1;
                    x1 -= parent.Borders.left.Enabled ? parent.Borders.left.LineWidth / 2.0f : 0.0f;
                    x2 += parent.Borders.right.Enabled ? parent.Borders.right.LineWidth / 2.0f : 0.0f;
                    break;
                case BorderTypes.LEFT_BORDER:
                    x1 = 0;
                    y1 = 0;
                    x2 = x1;
                    y2 = parent.HeightInPixels;
                    y1 -= parent.Borders.top.Enabled ? parent.Borders.top.LineWidth / 2.0f : 0.0f;
                    y2 += parent.Borders.bottom.Enabled ? parent.Borders.bottom.LineWidth / 2.0f : 0.0f;
                    break;
                case BorderTypes.BOTTOM_BORDER:
                    x1 = 0;
                    y1 = parent.HeightInPixels;
                    x2 = parent.WidthInPixels;
                    y2 = y1;
                    x1 -= parent.Borders.left.Enabled ? parent.Borders.left.LineWidth / 2.0f : 0.0f;
                    x2 += parent.Borders.right.Enabled ? parent.Borders.right.LineWidth / 2.0f : 0.0f;
                    break;
                case BorderTypes.RIGHT_BORDER:
                    x1 = parent.WidthInPixels;
                    y1 = 0;
                    x2 = x1;
                    y2 = parent.HeightInPixels;
                    y1 -= parent.Borders.top.Enabled ? parent.Borders.top.LineWidth / 2.0f : 0.0f;
                    y2 += parent.Borders.bottom.Enabled ? parent.Borders.bottom.LineWidth / 2.0f : 0.0f;
                    break;
            }

            if(this.Enabled)
            {
                gc.Transform = parent.LastDrawMatrix;
                gc.DrawLine(line, x1, y1, x2, y2);
            }
        }

        public void SaveItem(XmlDocument doc, XmlElement element)
        {
            string nameForSave = "";
            switch(this.Type)
            {
                case BorderTypes.TOP_BORDER:
                    nameForSave = "TopBorder";
                    break;
                case BorderTypes.LEFT_BORDER:
                    nameForSave = "LeftBorder";
                    break;
                case BorderTypes.BOTTOM_BORDER:
                    nameForSave = "BottomBorder";
                    break;
                case BorderTypes.RIGHT_BORDER:
                    nameForSave = "RightBorder";
                    break;
            }

            XmlElement el = doc.CreateElement(nameForSave);
            XmlAttribute attr = doc.CreateAttribute("Enabled");
            attr.Value = this.Enabled.ToString();
            el.SetAttributeNode(attr);

            attr = doc.CreateAttribute("Width");
            attr.Value = this.LineWidth.ToString();
            el.SetAttributeNode(attr);

            attr = doc.CreateAttribute("R");
            attr.Value = this.LineColor.R.ToString();
            el.SetAttributeNode(attr);

            attr = doc.CreateAttribute("G");
            attr.Value = this.LineColor.G.ToString();
            el.SetAttributeNode(attr);

            attr = doc.CreateAttribute("B");
            attr.Value = this.LineColor.B.ToString();
            el.SetAttributeNode(attr);

            element.AppendChild(el);
        }

        public void Load(System.Xml.XmlNode element)
        {
            string nameForSave = "";
            switch(this.Type)
            {
                case BorderTypes.TOP_BORDER:
                    nameForSave = "TopBorder";
                    break;
                case BorderTypes.LEFT_BORDER:
                    nameForSave = "LeftBorder";
                    break;
                case BorderTypes.BOTTOM_BORDER:
                    nameForSave = "BottomBorder";
                    break;
                case BorderTypes.RIGHT_BORDER:
                    nameForSave = "RightBorder";
                    break;
            }

            XmlNode node = element.SelectSingleNode(nameForSave);
            if(node != null)
            {
                this.Enabled = Convert.ToBoolean(node.Attributes["Enabled"].Value);
                this.LineWidth = (float)Convert.ToDouble(node.Attributes["Width"].Value);
                int tmpR = 0, tmpG = 0, tmpB = 0;
                tmpR = Convert.ToInt16(node.Attributes["R"].Value);
                tmpG = Convert.ToInt16(node.Attributes["G"].Value);
                tmpB = Convert.ToInt16(node.Attributes["B"].Value);
                this.LineColor = Color.FromArgb(tmpR,tmpG,tmpB);
            }
        }
    }
}
