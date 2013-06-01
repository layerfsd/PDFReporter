using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
	/// <summary>
	/// Item needs to implement this in order to show it self
	/// </summary>
	interface EditorItemViewer
	{
		void Draw(int offsetX, int offsetY, int parentX, int parentY,  float zoomLevel, Graphics gc, Rectangle clipRect);
	}
}
