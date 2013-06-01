using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;


namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
	/// <summary>
	/// Classes should implement this interface in order to appear in toolbar
	/// </summary>
	[Obfuscation(Exclude=true)]
	interface EditorToolBarPlugin
	{
		/// <summary>
		/// This method is invoked when object should add itself to toolstrip
		/// </summary>
		/// <param name="toolStrip"></param>
		void AddToToolStrip(ToolStrip toolStrip, ToolStripGroup group);
	}
}
