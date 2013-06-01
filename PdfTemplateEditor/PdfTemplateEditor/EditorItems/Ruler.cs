using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using AxiomCoders.PdfTemplateEditor.EditorStuff;
using AxiomCoders.PdfTemplateEditor.Common;

namespace AxiomCoders.PdfTemplateEditor.EditorItems
{
	/// <summary>
	/// show ruler
	/// </summary>
	public class Ruler: EditorItemViewer
	{
		public enum RulerPosition
		{
			TOP,
			LEFT,
			GUIDE
		}

		/// <summary>
		/// Position of this ruler
		/// </summary>
		private RulerPosition position;

		/// <summary>
		/// Measure unit used for ruler
		/// </summary>
		private MeasureUnits measureUnit = MeasureUnits.inch;
		public AxiomCoders.PdfTemplateEditor.Common.MeasureUnits MeasureUnit
		{
			get 
            {
                switch (AppOptions.Instance.Unit)
                {
                    case 0: measureUnit = MeasureUnits.inch; break;
                    case 1: measureUnit = MeasureUnits.mm; break;
                    case 2: measureUnit = MeasureUnits.cm; break;
                    case 3: measureUnit = MeasureUnits.pixel; break;
                }
                return measureUnit; 
            }
			set 
            { 
                measureUnit = value; 
            }
		}

		/// <summary>
		/// Report page that is shown and attached to this ruler
		/// </summary>
		private ReportPage reportPage;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="position"></param>
		public Ruler(RulerPosition position, ReportPage reportPage)
		{
			this.position = position;
			this.reportPage = reportPage;
		}

	
		/// <summary>
		/// Draw top ruler
		/// </summary>
		/// <param name="gc"></param>
		private void DrawTop(Graphics gc, Rectangle clipRect)
		{
			float pixelsPerUnit = UnitsManager.Instance.ConvertUnit(1, this.MeasureUnit, MeasureUnits.pixel);
			pixelsPerUnit *= reportPage.ZoomLevel / 100.0f;

			float shortLineInterval = 0;
			float increaseValue = 1;
			Font font = new Font(FontFamily.GenericSansSerif, 8);
			float value = 0; 
			float smallMarks = 5; // short lines
			float smallMarksDistance = 10; // in pixels

			while (true)
			{
				shortLineInterval = (pixelsPerUnit*(float)increaseValue) / (float)smallMarks;
				if (shortLineInterval < smallMarksDistance)
				{
					increaseValue++;
				}
				else if (shortLineInterval > smallMarksDistance*2)
				{
					increaseValue /= 2;
				}
				else 
				{
					break;
				}
			}

			// draw from 0 to right
			int tick = 0;
			for (float i = (int)reportPage.LastDrawMatrix.Elements[4]; i < clipRect.Width; i += shortLineInterval)
			{
				if (tick % smallMarks == 0)
				{
					gc.DrawLine(Pens.Black,  + (int)i, clipRect.Height, (int)i, clipRect.Height - 20);
					gc.DrawString(value.ToString(), font, Brushes.Black, (int)i + 3, 5);
					value += increaseValue;
				}
				else 
				{
					gc.DrawLine(Pens.Black, (int)i, clipRect.Height, (int)i, clipRect.Height - 5);
				}
				tick++;				
			}
			
			// draw from 0 to left
			tick = 0;
			value = 0;
            for(float i = (int)reportPage.LastDrawMatrix.Elements[4]; i > 0; i -= shortLineInterval)
            {
                if(tick % smallMarks == 0)
                {
                    gc.DrawLine(Pens.Black, (int)i, clipRect.Height, (int)i, clipRect.Height - 20);
                    gc.DrawString(value.ToString(), font, Brushes.Black, (int)i + 3, 5);
                    value -= increaseValue;
                }
                else
                {
                    gc.DrawLine(Pens.Black, (int)i, clipRect.Height, (int)i, clipRect.Height - 5);
                }
                tick++;
            }
		}

		/// <summary>
		/// Draw left ruler
		/// </summary>
		/// <param name="gc"></param>
		/// <param name="clipRect"></param>
		private void DrawLeft(Graphics gc, Rectangle clipRect)
		{
			float pixels = UnitsManager.Instance.ConvertUnit(1, this.MeasureUnit, MeasureUnits.pixel);
			pixels *= reportPage.ZoomLevel / 100.0f;

			float shortLineInterval = 0;
			float increaseValue = 1;
			Font font = new Font(FontFamily.GenericSansSerif, 8);
			float value = 0;
			float smallMarks = 5; // short lines
			float smallMarksDistance = 10; // in pixels

			while (true)
			{
				shortLineInterval = (pixels * (float)increaseValue) / (float)smallMarks;
				if (shortLineInterval < smallMarksDistance)
				{
					increaseValue++;
				}
				else if (shortLineInterval > smallMarksDistance * 2)
				{
					increaseValue /= 2;
				}
				else
				{
					break;
				}
			}

			// from 0 to bottom
			int tick = 0;
            for(float i = (int)reportPage.LastDrawMatrix.Elements[5]; i < clipRect.Height; i += shortLineInterval)
			{
				if (tick % smallMarks == 0)
				{
					gc.DrawLine(Pens.Black, 0, (int)i, clipRect.Width, (int)i);
					gc.DrawString(value.ToString(), font, Brushes.Black, 3, (int)i + 3);
					value += increaseValue;
				}
				else
				{
					gc.DrawLine(Pens.Black, clipRect.Width-5, (int)i, clipRect.Width, (int)i);
				}
				tick++;
			}


			// from 0 to top
			tick = 0;
			value = 0;
            for(float i = (int)reportPage.LastDrawMatrix.Elements[5]; i > 0; i -= shortLineInterval)
			{
				if (tick % smallMarks == 0)
				{
					gc.DrawLine(Pens.Black, 0, (int)i, clipRect.Width, (int)i);
					gc.DrawString(value.ToString(), font, Brushes.Black, 3, (int)i + 3);
					value -= increaseValue;
				}
				else
				{
					gc.DrawLine(Pens.Black, clipRect.Width - 5, (int)i, clipRect.Width, (int)i);
				}
				tick++;
			}

		}

		private void DrawGuide(Graphics gc, Rectangle clipRect)
		{

		}

		#region EditorItemViewer Members

		public void Draw(int offsetX, int offsetY, int parentX, int parentY,  float zoomLevel, Graphics gc, Rectangle clipRect)
		{
			switch (position)
			{
				case RulerPosition.TOP:
					DrawTop(gc, clipRect);
					break;
				case RulerPosition.LEFT:
					DrawLeft(gc, clipRect);
					break;
				case RulerPosition.GUIDE:
					DrawGuide(gc, clipRect);
					break;
			}
		}

		#endregion
	}
}
