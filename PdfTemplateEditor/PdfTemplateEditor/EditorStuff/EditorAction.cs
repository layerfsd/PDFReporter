using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
	/// <summary>
	/// One action in editor. It is inherited by Create, Update, Delete actions
	/// </summary>
	public abstract class EditorAction
	{
		public abstract void Redo();
		public abstract void Undo();
	}
}
