using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using AxiomCoders.PdfTemplateEditor.Common;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
	public class MovingCommandItem: CommandItem
	{

		public MovingCommandItem(EditorItem owner): base(CommandPosition.TopLeft, owner)
		{
			this.commandSize = 10;
			this.widthInPixels = 10;
			this.heightInPixels = 10;
			this.cursor = Cursors.SizeAll;
		}

		private float startLocX;
		private float startLocY;

		public override void StartMoving()
		{
			// save scaling of owner
			startLocX = this.Owner.LocationInUnitsX;
			startLocY = this.Owner.LocationInUnitsY;
            Owner.StartMoving();
		}

		public override void StopMoving()
		{
			//ActionManager.Instance.EditorItemUpdate(this.Owner, new string[] { "LocationInUnitsX", "LocationInUnitsY"},
			//new object[] { startLocX, startLocY },
			//new object[] { this.Owner.LocationInUnitsX, this.Owner.LocationInUnitsY });
            Owner.StopMoving(true);
		}

		public override void Move(float dx, float dy)
		{           
            Owner.MoveBy(dx, dy);           

			// convert dx, dy to vector in object space
			/*System.Drawing.Drawing2D.Matrix mat = this.Owner.DrawMatrix.Clone();
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
			dx = UnitsManager.Instance.ConvertUnit(tmpPoint.X, MeasureUnits.pixel, this.Owner.MeasureUnit);
			dy = UnitsManager.Instance.ConvertUnit(tmpPoint.Y, MeasureUnits.pixel, this.Owner.MeasureUnit);	
			
			this.Owner.LocationInUnitsX = startLocX + dx;
			this.Owner.LocationInUnitsY = startLocY + dy;

			this.Owner.DockAll();
			if (this.Owner.Parent != null)
			{
				this.Owner.Parent.DockAll();
			}*/
		}

		private void UpdateLocations(float zoomLevel)
		{
			float commandSize = this.commandSize / zoomLevel;			
			this.LocationInPixelsX = 10*(1.0f /zoomLevel) + (-commandSize / 2);
			this.LocationInPixelsY = -10*(1.0f/zoomLevel) + (-commandSize / 2);
		}


		public override void Draw(float zoomLevel, System.Drawing.Graphics gc)
		{
			UpdateLocations(zoomLevel);
			base.Draw(zoomLevel, gc);
		}
	}
}
