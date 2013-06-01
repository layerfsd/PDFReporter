using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using AxiomCoders.PdfTemplateEditor.Common;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
	

	public class SizingCommandItem: CommandItem
	{
		public SizingCommandItem(CommandPosition position, EditorItem owner): base(position, owner)
		{
			this.commandSize = 6;
			this.widthInPixels = 6;
			this.heightInPixels = 6;
			switch (this.CommandPosition)
			{
				case CommandPosition.TopLeft:
				case CommandPosition.BottomRight:
					this.cursor = Cursors.SizeNWSE;					
					
					break;
				case CommandPosition.TopCenter:
				case CommandPosition.BottomCenter:
					this.cursor = Cursors.SizeNS;
					break;
				case CommandPosition.MiddleLeft:
				case CommandPosition.MiddleRight:
					this.cursor = Cursors.SizeWE;
					break;
				case CommandPosition.TopRight:
				case CommandPosition.BottomLeft:
					this.cursor = Cursors.SizeNESW;
					break;
			}		
		}


		private void UpdateLocations(float zoomLevel)
		{
			float commandSize = this.commandSize / zoomLevel;
			switch (this.CommandPosition)
			{
				case CommandPosition.TopLeft:
					this.LocationInPixelsX = -commandSize / 2;
					this.LocationInPixelsY = -commandSize / 2;					
					break;
				case CommandPosition.TopCenter:
					this.LocationInPixelsX = (this.Owner.WidthInPixels / 2)-commandSize / 2;
					this.LocationInPixelsY = -commandSize / 2;															
					break;
				case CommandPosition.TopRight:
					this.LocationInPixelsX = (this.Owner.WidthInPixels) - commandSize / 2;
					this.LocationInPixelsY = -commandSize / 2;															
					break;
				case CommandPosition.MiddleLeft:
					this.LocationInPixelsX = -commandSize / 2;
					this.LocationInPixelsY = (this.Owner.HeightInPixels / 2)-commandSize / 2;
					break;
				case CommandPosition.MiddleRight:
					this.LocationInPixelsX = (this.Owner.WidthInPixels) - commandSize / 2;
					this.LocationInPixelsY = (this.Owner.HeightInPixels / 2) - commandSize / 2;
					break;
				case CommandPosition.BottomLeft:
					this.LocationInPixelsX = -commandSize / 2;
					this.LocationInPixelsY = (this.Owner.HeightInPixels) - commandSize / 2;
					break;
				case CommandPosition.BottomCenter:
					this.LocationInPixelsX = (this.Owner.WidthInPixels / 2) - commandSize / 2;
					this.LocationInPixelsY = (this.Owner.HeightInPixels) - commandSize / 2;
					break;
				case CommandPosition.BottomRight:
					this.LocationInPixelsX = (this.Owner.WidthInPixels) - commandSize / 2;
					this.LocationInPixelsY = (this.Owner.HeightInPixels) - commandSize / 2;
					break;
			}			
		}

		private float startLocX = 0;
		private float startLocY = 0;
		private float startWidth = 0;
		private float startHeight = 0;

		public override void Move(float dx, float dy)
		{
			// convert dx, dy to vector in object space
			System.Drawing.Drawing2D.Matrix mat = this.Owner.DrawMatrix.Clone();
			mat.Multiply(this.TransformationMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
			mat.Multiply(this.Owner.ViewMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
			mat.Invert();

			PointF tmpPoint = new PointF(dx, dy);
			PointF[] points = new PointF[1];
			points[0] = tmpPoint;
			mat.TransformVectors(points);

			tmpPoint = points[0];

			dx = tmpPoint.X;
			dy = tmpPoint.Y;
			float udx = UnitsManager.Instance.ConvertUnit(tmpPoint.X, MeasureUnits.pixel, this.Owner.MeasureUnit);
			float udy = UnitsManager.Instance.ConvertUnit(tmpPoint.Y, MeasureUnits.pixel, this.Owner.MeasureUnit);	
			//dx = UnitsManager.Instance.ConvertUnit(tmpPoint.X, MeasureUnits.pixel, this.Owner.MeasureUnit);
			//dy = UnitsManager.Instance.ConvertUnit(tmpPoint.Y, MeasureUnits.pixel, this.Owner.MeasureUnit);	

			switch (this.CommandPosition)
			{
				case CommandPosition.TopLeft:
					this.Owner.LocationInUnitsX = startLocX + udx;
					this.Owner.LocationInUnitsY = startLocY + udy;
					this.Owner.WidthInPixels = startWidth - dx;
					this.Owner.HeightInPixels = startHeight - dy;
                    this.Owner.AnchorAll(udx, udy, -dx, -dy);
					break;
				case CommandPosition.TopCenter:
					this.Owner.LocationInUnitsY = startLocY + udy;
					this.Owner.HeightInPixels = startHeight - dy;
                    this.Owner.AnchorAll(0.0f, udy, 0.0f, -dy);
					break;
				case CommandPosition.TopRight:
					this.Owner.LocationInUnitsY = startLocY + udy;
					this.Owner.WidthInPixels = startWidth + dx;
					this.Owner.HeightInPixels = startHeight - dy;
                    this.Owner.AnchorAll(0.0f, udy, dx, -dy);
					break;
				case CommandPosition.MiddleLeft:
					this.Owner.LocationInUnitsX = startLocX + udx;
					this.Owner.WidthInPixels = startWidth - dx;
                    this.Owner.AnchorAll(udx, 0.0f, -dx, 0.0f);
					break;
				case CommandPosition.MiddleRight:
					this.Owner.WidthInPixels = startWidth + dx;
                    this.Owner.AnchorAll(0.0f, 0.0f, dx, 0.0f);
					break;
				case CommandPosition.BottomLeft:
					this.Owner.LocationInUnitsX = startLocX + udx;
					this.Owner.WidthInPixels = startWidth - dx;
					this.Owner.HeightInPixels = startHeight + dy;
                    this.Owner.AnchorAll(udx, 0.0f, -dx, dy);
					break;
				case CommandPosition.BottomCenter:
					this.Owner.HeightInPixels = startHeight + dy;
                    this.Owner.AnchorAll(0.0f, 0.0f, 0.0f, dy);
					break;
				case CommandPosition.BottomRight:
					this.Owner.WidthInPixels = startWidth + dx;
					this.Owner.HeightInPixels = startHeight + dy;
                    this.Owner.AnchorAll(0.0f, 0.0f, dx, dy);
					break;		
			}

            if(this.Owner.WidthInPixels < UnitsManager.Instance.ConvertUnit(1.0f, MeasureUnits.mm, MeasureUnits.pixel))
            {
                this.Owner.WidthInPixels = UnitsManager.Instance.ConvertUnit(1.0f, MeasureUnits.mm, MeasureUnits.pixel);
            }
            if(this.Owner.HeightInPixels < UnitsManager.Instance.ConvertUnit(1.0f, MeasureUnits.mm, MeasureUnits.pixel))
            {
                this.Owner.HeightInPixels = UnitsManager.Instance.ConvertUnit(1.0f, MeasureUnits.mm, MeasureUnits.pixel);
            }

			this.Owner.DockAll();
			if (this.Owner.Parent != null)
			{
				this.Owner.Parent.DockAll();
			}
		}


		public override void StartMoving()
		{
			// save scaling of owner
			startLocX = this.Owner.LocationInUnitsX;
			startLocY = this.Owner.LocationInUnitsY;
			startWidth = this.Owner.WidthInPixels;
			startHeight = this.Owner.HeightInPixels;
            this.Owner.LockAnchorPositions();
		}


		public override void StopMoving()
		{
			ActionManager.Instance.EditorItemUpdate(this.Owner, new string[] { "LocationInUnitsX", "LocationInUnitsY", "WidthInPixels", "HeightInPixels"},
				new object[] { startLocX, startLocY, startWidth, startHeight },
				new object[] { this.Owner.LocationInUnitsX, this.Owner.LocationInUnitsY, this.Owner.WidthInPixels, this.Owner.HeightInPixels});					


			// TODO: Update scaling of owner
		}

		public override void Draw(float zoomLevel, Graphics gc)
		{
			UpdateLocations(zoomLevel);
			base.Draw(zoomLevel, gc);
		}

		/*public override void Draw(int offsetX, int offsetY, int parentX, int parentY, float zoomLevel, System.Drawing.Graphics gc, System.Drawing.Rectangle clipRect)
		{			
			
			gc.Transform = this.Owner.LastDrawMatrix;
			switch (this.CommandPosition)
			{
				case CommandPosition.TopLeft:
					gc.DrawRectangle(Pens.Black, -commandSize/2, -commandSize/2, (float)this.WidthInPixels, (float)this.HeightInPixels);
					break;
				case CommandPosition.TopCenter:
					gc.DrawRectangle(Pens.Black, (this.Owner.WidthInPixels / 2) - (commandSize / 2), -commandSize / 2, (float)this.WidthInPixels, (float)this.HeightInPixels);
					break;
				case CommandPosition.TopRight:
					gc.DrawRectangle(Pens.Black, (this.Owner.WidthInPixels) - (commandSize / 2), -commandSize / 2, (float)this.WidthInPixels, (float)this.HeightInPixels);
					break;
				case CommandPosition.MiddleLeft:
					gc.DrawRectangle(Pens.Black, - (commandSize / 2), (this.Owner.HeightInPixels/2) - (commandSize / 2), (float)this.WidthInPixels, (float)this.HeightInPixels);
					break;
				case CommandPosition.MiddleRight:
					gc.DrawRectangle(Pens.Black, this.Owner.WidthInPixels -(commandSize / 2), (this.Owner.HeightInPixels / 2) - (commandSize / 2), (float)this.WidthInPixels, (float)this.HeightInPixels);
					break;
				case CommandPosition.BottomLeft:
					gc.DrawRectangle(Pens.Black, -commandSize / 2, (this.Owner.HeightInPixels)-(commandSize / 2), (float)this.WidthInPixels, (float)this.HeightInPixels);
					break;
				case CommandPosition.BottomCenter:
					gc.DrawRectangle(Pens.Black, (this.Owner.WidthInPixels / 2) - (commandSize / 2), (this.Owner.HeightInPixels) - (commandSize / 2), (float)this.WidthInPixels, (float)this.HeightInPixels);
					break;
				case CommandPosition.BottomRight:
					gc.DrawRectangle(Pens.Black, (this.Owner.WidthInPixels) - (commandSize / 2), (this.Owner.HeightInPixels) - (commandSize / 2), (float)this.WidthInPixels, (float)this.HeightInPixels);
					break;
			}			
		}*/
		
	}
}
