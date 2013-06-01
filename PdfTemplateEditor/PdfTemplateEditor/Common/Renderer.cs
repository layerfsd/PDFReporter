using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace AxiomCoders.PdfTemplateEditor.Common
{
	/// <summary>
	/// This class is used to enable float buffering
	/// </summary>
	public class Renderer
	{
		private static  Renderer instance = null;
		public static AxiomCoders.PdfTemplateEditor.Common.Renderer Instance
		{
			get 
			{
				if (instance == null )
				{
					instance = new Renderer();
				}
				return instance;
			}			
		}

		private int width = 0;
		private int height = 0;

		private Graphics graphics;
		public System.Drawing.Graphics Graphics
		{
			get { return graphics; }
			set { graphics = value; }
		}

		public static float CalculateGradientAngle(Point startPosition, Point endPosition)
		{
			float angle = 0.0f;
			float a = Math.Abs(startPosition.X - endPosition.X);
			float b = Math.Abs(startPosition.Y - endPosition.Y);
			float h = (float)Math.Sqrt((double)(a * a + b * b));
			angle = (float)Math.Atan((double)(b / a));
			angle = (angle * 180.0f) / (float)Math.PI;

			if (startPosition.X < endPosition.X && startPosition.Y > endPosition.Y)
			{

			}
			else if (startPosition.X > endPosition.X && startPosition.Y > endPosition.Y)
			{
				angle = 180.0f - angle;
			}
			else if (startPosition.X >= endPosition.X && startPosition.Y <= endPosition.Y)
			{
				angle = 180.0f + angle;
			}
			else if (startPosition.X < endPosition.X && startPosition.Y < endPosition.Y)
			{
				angle = -angle;
			}
			angle = -angle;

			return angle;
		}

		private static void MakeBasePoints(PointF startPosition, PointF endPosition, float Width, float Height, out PointF point1, out PointF point2)
		{			
			float slope;
			float xleft;
			float yleft;
			float xright;
			float yright;
			float xbottom;
			float ybottom;
			float xtop;
			float ytop;

			// find where y when left side is hit
			if (endPosition.X - startPosition.X != 0)
			{
				slope = (endPosition.Y - startPosition.Y) / (endPosition.X - startPosition.X);
				xleft = 0;
				yleft = -slope * startPosition.X + startPosition.Y;

				xright = Width;
				yright = slope * (xright - startPosition.X) + startPosition.Y;

				ybottom = Height;
				xbottom = (ybottom - startPosition.Y + slope * startPosition.X) / slope;
				
				ytop = 0;
				xtop = (ytop - startPosition.Y + slope * startPosition.X) / slope;				
			}			
			else 
			{
				xleft = startPosition.X;
				yleft = -10;

				xright = startPosition.X;
				yright = -10;

				xtop = startPosition.X;
				ytop = 0;

				xbottom = startPosition.X;
				ybottom = Height;
			}

			point1 = new PointF();
			point2 = new PointF();

			// find exact line coords
			// find first
			bool point1Found = false;
			if (yleft >= 0 && yleft <= Height)
			{
				point1.X = 0;
				point1.Y = yleft;
				point1Found = true;
			}
			if (yright >= 0 && yright <= Height)
			{
				if (point1Found)
				{
					point2.X = Width;
					point2.Y = yright;					
				}
				else 
				{
					point1.X = Width;
					point1.Y = yright;
					point1Found = true;
				}
			}
			if (xtop >= 0 && xtop <= Width)
			{
				if (point1Found)
				{
					point2.X = xtop;
					point2.Y = 0;
				}
				else
				{
					point1.X = xtop;
					point1.Y = 0;
					point1Found = true;
				}
			}
			if (xbottom >= 0 && xbottom <= Width)
			{
				if (point1Found)
				{
					point2.X = xbottom;
					point2.Y = Height;
				}
				else
				{
					point1.X = xbottom;
					point1.Y = Height;
					point1Found = true;
				}
			}
		}



		public static void MakeBlendPositions(PointF startPosition, PointF endPosition, float width, float height, out float pos1, out float pos2)
		{
			PointF point1, point2;				
			MakeBasePoints(startPosition, endPosition, width, height, out point1, out point2);

			float a, b;

			a = Math.Abs(point1.X - point2.X);
			b = Math.Abs(point1.Y - point2.Y);
			float len = (float)Math.Sqrt(a * a + b * b);									

			a = Math.Abs(startPosition.X - point1.X);
			b = Math.Abs(startPosition.Y - point1.Y);
			float l1 = (float)Math.Sqrt(a*a + b*b);

			a = Math.Abs(startPosition.X - point2.X);
			b = Math.Abs(startPosition.Y - point2.Y);
			float l2 = (float)Math.Sqrt(a * a + b * b);

			
			a = Math.Abs(endPosition.X - point1.X);
			b = Math.Abs(endPosition.Y - point1.Y);
			float l3 = (float)Math.Sqrt(a * a + b * b);

			a = Math.Abs(endPosition.X - point2.X);
			b = Math.Abs(endPosition.Y - point2.Y);
			float l4 = (float)Math.Sqrt(a * a + b * b);
			float slen, elen;

			if (l1 <= l3)
			{
				slen = l1;
				elen = l3;
			}
			else 
			{
				slen = l2;
				elen = l4;
			}
								

			pos1 = slen / len;
			pos2 = elen / len;					
		}


		private Bitmap buffer;

		/// <summary>
		/// Create new buffer. Sizes are in pixels
		/// </summary>
		/// <param name="gc"></param>
		/// <param name="sizeX"></param>
		/// <param name="sizeY"></param>
		public bool CreateBuffer(int width, int height)
		{
			if ((width != this.width) || (height != this.height))
			{
				// if size of buffer is changed 

				this.height = height;
				this.width = width;

				if (buffer != null)
				{
					buffer.Dispose();
					buffer = null;
				}

				if (graphics != null)
				{
					graphics.Dispose();
					graphics = null;
				}

				if (width == 0 || height == 0)
				{
					return false;
				}				

				buffer = new Bitmap(width, height);
				graphics = Graphics.FromImage(buffer);				
			}
			return true;
		}

		/// <summary>
		/// Render buffer to gc
		/// </summary>
		/// <param name="gc"></param>
		public void Render(Graphics gc)
		{
			if (buffer != null)
			{
				gc.DrawImage(buffer, new Rectangle(0, 0, width, height), 0, 0, width, height, GraphicsUnit.Pixel);
			}
		}

		/// <summary>
		/// If buffer can be drawn
		/// </summary>
		public bool CanDraw
		{
			get { return Graphics != null; }
		}
	}
}
