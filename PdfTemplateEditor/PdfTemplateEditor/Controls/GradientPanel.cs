using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using AxiomCoders.PdfTemplateEditor.EditorItems;
using AxiomCoders.PdfTemplateEditor.Common;

namespace AxiomCoders.PdfTemplateEditor.Controls
{
    public class GradientPanel : Control
    {
        private Point startPosition;		
        private Point endPosition;
        private Color color1;
        private Color color2;
        private GradientType gradType;
        private Pen borderPen;
		private float angle;


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

        public GradientPanel()
        {
            startPosition = Point.Empty;
            endPosition = Point.Empty;
            borderPen = new Pen(Color.Black, 2);
            gradType = GradientType.Linear;

            //to reduce blinking
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, false);
        }


        public GradientDefinition GradientColors
        {
            set
            {
                color1 = value.Color1;
                color2 = value.Color2;

                float x1 = Width * value.Point1.X;
                float x2 = Width * value.Point2.X;
                float y1 = Height * value.Point1.Y;
                float y2 = Height * value.Point2.Y;
                startPosition = new Point((int)x1, (int)y1);
                endPosition = new Point((int)x2, (int)y2);
                gradType = value.GradientType;
                Invalidate();
            }
            get
            {
                float x1 = (float)decimal.Divide(startPosition.X,Width);
                float x2 = (float)decimal.Divide(endPosition.X, Width);
                float y1 = (float)decimal.Divide(startPosition.Y, Height);
                float y2 = (float)decimal.Divide(endPosition.Y, Height);
                PointF p1 = new PointF(x1, y1);
                PointF p2 = new PointF(x2, y2);

				GradientDefinition def = new GradientDefinition(color1, color2, p1, p2, gradType);
				def.BlendPosition1 = this.BlendPosition1;
				def.BlendPosition2 = this.BlendPosition2;
				def.Angle = this.angle;
				return def;
            }
        }


        public GradientType GradientType
        {
            get
            {
                return gradType;
            }
            set
            {
                gradType = value;
            }
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Control.MouseButtons == MouseButtons.Left)
            {
                endPosition = e.Location;
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPosition = e.Location;
                endPosition = e.Location;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            endPosition = e.Location;
            Invalidate();
        }


		
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.CompositingMode = CompositingMode.SourceOver;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            if (startPosition != endPosition)
            {
				this.angle = Renderer.CalculateGradientAngle(startPosition, endPosition);

				e.Graphics.CompositingMode = CompositingMode.SourceOver;
				e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
				e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
							
				//using (LinearGradientBrush brush = new LinearGradientBrush(this.startPosition, this.endPosition, GradientColors.Color1, GradientColors.Color2))
                using (LinearGradientBrush brush = new LinearGradientBrush(this.DisplayRectangle, GradientColors.Color1, GradientColors.Color2, angle))
                {

					Blend tmpBlend = new Blend(4);

                    /*float width = (float)Math.Sqrt(Width * Width + Height * Height);
                    width *= 2;
                    using (Pen p = new Pen(brush, width))
                    {
                        e.Graphics.DrawLine(p, startPosition, endPosition);
                    }*/


					// find who is nearer point1 spos or epos

					float pos1, pos2;
					PointF spos = new PointF(startPosition.X, startPosition.Y);
					PointF epos = new PointF(endPosition.X, endPosition.Y);

					Renderer.MakeBlendPositions(spos, epos, Width, Height, out pos1, out pos2);
					this.BlendPosition1 = pos1;
					this.BlendPosition2 = pos2;

					tmpBlend.Factors = new float[] { 0.0f, 0.0f, 1.0f, 1.0f };
					tmpBlend.Positions = new float[] { 0.0f, pos1, pos2, 1.0f };

					brush.Blend = tmpBlend;					
					e.Graphics.FillRectangle(brush, this.DisplayRectangle);
					//e.Graphics.FillEllipse(brush, this.DisplayRectangle);
                }

				//startPosition.X = e.ClipRectangle.Width / 2;
				//startPosition.Y = e.ClipRectangle.Height / 2;
				
                e.Graphics.DrawLine(new Pen(Color.Red, 2), startPosition, endPosition);
            }
            else
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(ClientRectangle, GradientColors.Color1, GradientColors.Color2, 0f))
                {
                    e.Graphics.FillRectangle(brush, e.ClipRectangle);
                }
            }
            e.Graphics.DrawRectangle(borderPen, e.ClipRectangle);
        }

				
        
    }
}
