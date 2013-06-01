using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using AxiomCoders.PdfTemplateEditor.EditorStuff;
using AxiomCoders.PdfTemplateEditor.Common;

namespace AxiomCoders.PdfTemplateEditor.EditorItems
{
	/// <summary>
	/// Class used to show report page 
	/// </summary>
	public class ReportPageViewer: EditorItemViewer
	{
		private ReportPage owner;

		public ReportPageViewer(ReportPage owner)
		{
			this.owner = owner;
		}

		/// <summary>
		/// Check if horizontal scrollbar is required
		/// </summary>
		private bool horizontalScrollRequired = false;
		public bool HorizontalScrollRequired
		{
			get { return horizontalScrollRequired; }
			set { horizontalScrollRequired = value; }
		}

		/// <summary>
		/// Check if vertical scroller is required
		/// </summary>
		private bool verticalScrollRequired = false;
		public bool VerticalScrollRequired
		{
			get { return verticalScrollRequired; }
			set { verticalScrollRequired = value; }
		}

		/// <summary>
		/// Current X position for scrolling 
		/// </summary>
		private float offsetXValue = 0;
		public float OffsetXValue
		{
			get { return offsetXValue; }
			set
			{
				offsetXValue = value;
			}
		}		

		/// <summary>
		/// Current Y position for scrolling
		/// </summary>
		private float offsetYValue = 0;
		public float OffsetYValue
		{
			get { return offsetYValue; }
			set { offsetYValue = value; }
		}
		
		/// <summary>
		/// What is max value for horizontal scroller
		/// </summary>
		private int horizontalScrollerMaxValue = 0;
		public int HorizontalScrollerMaxValue
		{
			get { return horizontalScrollerMaxValue; }
			set { horizontalScrollerMaxValue = value; }
		}

		/// <summary>
		/// What is max value for vertical scroller
		/// </summary>
		private int verticalScrollerMaxValue = 0;
		public int VerticalScrollerMaxValue
		{
			get { return verticalScrollerMaxValue; }
			set { verticalScrollerMaxValue = value; }
		}

			

		/// <summary>
		/// Where is top-left corner of page
		/// </summary>
		private float pageStartX;
		public float PageStartX
		{
			get { return pageStartX; }			
		}

		private float pageStartY;
		public float PageStartY
		{
			get { return pageStartY; }			
		}

		/// <summary>
		/// Width of whole page in pixels 
		/// </summary>
		private float drawWidth;
		public float DrawWidth
		{
			get { return drawWidth; }			
		}
		
		/// <summary>
		/// Height of whole page in pixels 
		/// </summary>
		private float drawHeight;
		public float DrawHeight
		{
			get { return drawHeight; }		
		}

		private float centerX;
		public float CenterX
		{
			get { return centerX; }
			set { centerX = value; }
		}

		private float centerY;
		public float CenterY
		{
			get { return centerY; }
			set { centerY = value; }
		}
		

		public void UpdateValues(Rectangle clipRect)
		{
			centerX = ((float)clipRect.Width / 2.0f);
			centerY = ((float)clipRect.Height / 2.0f);
			
			// draw white background in size of page considering zoom level. Also initiate drawing of other items on page
			drawWidth = owner.WidthInPixels; //UnitsManager.Instance.ConvertUnit(, owner.MeasureUnit, MeasureUnits.pixel);
			drawHeight = owner.HeightInPixels; // UnitsManager.Instance.ConvertUnit(owner.Size.Height, owner.MeasureUnit, MeasureUnits.pixel);

			drawWidth *= (float)owner.ZoomLevel / 100.0f;
			drawHeight *= (float)owner.ZoomLevel / 100.0f;
			

			pageStartX = (float)clipRect.Width / 2.0f - (drawWidth / 2.0f);
			if (pageStartX < 0)
			{
				pageStartX = 0;
			}

			pageStartY = (float)clipRect.Height / 2.0f - (drawHeight / 2.0f);
			if (pageStartY < 0)
			{
				pageStartY = 0;
			}

			if (drawWidth < clipRect.Width)
			{
				horizontalScrollRequired = false;
			}
			else
			{
				horizontalScrollRequired = true;
				horizontalScrollerMaxValue = (int)drawWidth - clipRect.Width + 50;
			}

			if (drawHeight < clipRect.Height)
			{
				verticalScrollRequired = false;
			}
			else
			{
				verticalScrollRequired = true;
				verticalScrollerMaxValue = (int)drawHeight - clipRect.Height + 50;
			}

		}


	
		#region EditorItemViewer Members

		public void Draw(int offsetX, int offsetY, int parentX, int parentY,  float zoomLevel, Graphics gc, Rectangle clipRect)
		{
			// draw white background in size of page considering zoom level. Also initiate drawing of other items on page
			/*drawWidth = owner.WidthInPixels; //UnitsManager.Instance.ConvertUnit(, owner.MeasureUnit, MeasureUnits.pixel);
			drawHeight = owner.HeightInPixels; // UnitsManager.Instance.ConvertUnit(owner.Size.Height, owner.MeasureUnit, MeasureUnits.pixel);
								
			drawWidth *= owner.ZoomLevel / 100.0f;
			drawHeight *= owner.ZoomLevel / 100.0f;

			pageStartX = (float)clipRect.Width / 2.0f - (drawWidth / 2.0f);
			if (pageStartX < 0)
			{
				pageStartX = 0;
			}

			pageStartY = (float)clipRect.Height / 2.0f - (drawHeight / 2.0f);
			if (pageStartY < 0)
			{
				pageStartY = 0;
			}

			if (drawWidth < clipRect.Width)
			{
				horizontalScrollRequired = false;
			}
			else
			{
				horizontalScrollRequired = true;
				horizontalScrollerMaxValue = (int)drawWidth - clipRect.Width + 50;
			}

			if (drawHeight < clipRect.Height)
			{
				verticalScrollRequired = false;
			}
			else
			{
				verticalScrollRequired = true;
				verticalScrollerMaxValue = (int)drawHeight - clipRect.Height + 50;
			}

			gc.FillRectangle(Brushes.White, (float)pageStartX, (float)pageStartY, (float)drawWidth, (float)drawHeight);						*/
		}

		#endregion
	}
}
