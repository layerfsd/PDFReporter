using System;
using System.Collections.Generic;
using System.Text;
using AxiomCoders.PdfTemplateEditor.EditorStuff;
using System.Drawing;
using System.Windows.Forms;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
	/// <summary>
	/// some positions for commands
	/// </summary>
	public enum CommandPosition
	{
		TopLeft,
		TopCenter,
		TopRight,
		MiddleLeft,
		MiddleCenter,
		MiddleRight,
		BottomLeft,
		BottomCenter,
		BottomRight
	}

	/// <summary>
	/// Item used to store command information
	/// </summary>
	public class CommandItem
	{
		private CommandPosition commandPosition;
		public AxiomCoders.PdfTemplateEditor.EditorStuff.CommandPosition CommandPosition
		{
			get { return commandPosition; }		
		}

		private EditorItem owner;
		public AxiomCoders.PdfTemplateEditor.EditorStuff.EditorItem Owner
		{
			get { return owner; }
			set { owner = value; }
		}

		protected CommandItem()
		{

		}

		public virtual void Draw(float zoomLevel, System.Drawing.Graphics gc)
		{
			System.Drawing.Drawing2D.Matrix mat = new System.Drawing.Drawing2D.Matrix();
			mat.Multiply(this.TransformationMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
			mat.Multiply(this.Owner.DrawMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
			mat.Multiply(this.Owner.ViewMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);

			gc.Transform = mat;
			float w = (float)this.WidthInPixels / this.Owner.ViewMatrix.Elements[0];
			float h = (float)this.HeightInPixels / this.Owner.ViewMatrix.Elements[3];
			Pen p = new Pen(Color.Gray, 1.0f / zoomLevel);
			gc.DrawRectangle(p, 0, 0, w, h);
		}


		protected int commandSize = 6;

		protected float widthInPixels;
		public float WidthInPixels
		{
			get { return widthInPixels; }
		
		}
		protected float heightInPixels;


		public float HeightInPixels
		{
			get { return heightInPixels; }
			
		}
		public CommandItem(CommandPosition position, EditorItem owner) 
		{
			this.Owner = owner;
			this.commandPosition = position;

			this.widthInPixels = commandSize;
			this.heightInPixels = commandSize;		
		}

		protected int GetDisplaceX()
		{
			switch (commandPosition)
			{
				case CommandPosition.TopCenter:
					return (int)(this.Owner.WidthInPixels/2) + (this.commandSize / 2);
					
			}
			return 0;
		}

		protected int GetDisplaceY()
		{
			switch (commandPosition)
			{
				case CommandPosition.TopCenter:
					return -this.commandSize / 2;					
			}
			return 0;
		}

		/// <summary>
		/// Current cursor for this command item
		/// </summary>
		protected Cursor cursor = Cursors.Default;


		protected float locationInPixelsX;
		public float LocationInPixelsX
		{
			get { return locationInPixelsX; }
			set { locationInPixelsX = value; }
		}

		protected float locationInPixelsY;
		public float LocationInPixelsY
		{
			get { return locationInPixelsY; }
			set { locationInPixelsY = value; }
		}

		protected System.Drawing.Drawing2D.Matrix transformationMatrix = new System.Drawing.Drawing2D.Matrix();
		public System.Drawing.Drawing2D.Matrix TransformationMatrix
		{
			get
			{
				System.Drawing.Drawing2D.Matrix offsetTrans;

				offsetTrans = new System.Drawing.Drawing2D.Matrix();
				offsetTrans.Translate((float)this.LocationInPixelsX, (float)this.LocationInPixelsY);
			
				// first we scale than rotate and then transform
				this.transformationMatrix.Reset();
				this.transformationMatrix.Multiply(offsetTrans, System.Drawing.Drawing2D.MatrixOrder.Append);

				return this.transformationMatrix;

			}
		}

		

		public virtual void StartMoving()
		{

		}


		public virtual void StopMoving()
		{

		}


		public virtual void Move(float dx, float dy)
		{
			
		}

		/// <summary>
		/// Check if this command can be selected and return true in case it can
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool CanBeSelected(float x, float y, System.Drawing.Drawing2D.Matrix viewMatrix)
		{
			// transform x,y back to object coordinates to check for selection
			System.Drawing.Drawing2D.Matrix mat = new System.Drawing.Drawing2D.Matrix();
			mat.Multiply(this.TransformationMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
			mat.Multiply(this.Owner.DrawMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
			mat.Multiply(viewMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
			mat.Invert();

			PointF tmpPoint = new PointF(x, y);
			PointF[] points = new PointF[1];
			points[0] = tmpPoint;
			mat.TransformPoints(points);

			tmpPoint = points[0];
			// check if this item should be selected
			//float tmpX = LocationInPixelsX * zoomLevel;
			//float tmpY = LocationInPixelsY * zoomLevel;
			//float w = widthInPixels; //** zoomLevel;
			//float h = heightInPixels; //* zoomLevel;
            float w = (float)this.WidthInPixels / this.Owner.ViewMatrix.Elements[0];
            float h = (float)this.HeightInPixels / this.Owner.ViewMatrix.Elements[3];


			// if starting coordinate fall inside this component rect
			if (tmpPoint.X >= 0 && tmpPoint.X <= w && tmpPoint.Y >= 0 && tmpPoint.Y <= h)
			{
				return true;
			}
			else
			{
				return false;
			}         
		}

		/// <summary>
		/// Return cursor for command
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="zoomLevel"></param>
		/// <returns>null if coordinates are not fine</returns>
		public Cursor GetCursor(float x, float y, System.Drawing.Drawing2D.Matrix viewMatrix)
		{
			// transform x,y back to object coordinates to check for selection
			System.Drawing.Drawing2D.Matrix mat = new System.Drawing.Drawing2D.Matrix();			
			mat.Multiply(this.TransformationMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
			mat.Multiply(this.Owner.DrawMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
			mat.Multiply(viewMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
			mat.Invert();

			PointF tmpPoint = new PointF(x, y);
			PointF[] points = new PointF[1];
			points[0] = tmpPoint;
			mat.TransformPoints(points);

			tmpPoint = points[0];
			// check if this item should be selected					

			float w = (float)this.WidthInPixels / this.Owner.ViewMatrix.Elements[0];
			float h = (float)this.HeightInPixels / this.Owner.ViewMatrix.Elements[3];

			// if starting coordinate fall inside this component rect
			if (tmpPoint.X >= 0 && tmpPoint.X <= w && tmpPoint.Y >= 0 && tmpPoint.Y <= h)
			{
				return this.cursor;
			}
			else
			{
				return null;
			}						
		}

	}
}
