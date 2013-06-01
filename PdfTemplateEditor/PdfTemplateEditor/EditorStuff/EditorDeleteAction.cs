using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
	public class EditorDeleteAction: EditorAction
	{
		/// <summary>
		/// Item that is deleted
		/// </summary>
		private EditorItem itemDeleted;

		/// <summary>
		/// place in parent children list where item removed was located
		/// </summary>
		private int parentIndex; 

		public EditorDeleteAction(EditorItem itemDeleted, int parentIndex)
		{
			this.itemDeleted = itemDeleted;
			this.parentIndex = parentIndex;
		}

		/// <summary>
		/// remove it from parent once again
		/// </summary>
		public override void Redo()
		{			
			itemDeleted.Parent.Children.Remove(itemDeleted);
			itemDeleted.Parent.DockAll();
		}

		/// <summary>
		/// This will remove item to parent
		/// </summary>
		public override void Undo()
		{
			itemDeleted.Parent.Children.Insert(this.parentIndex, itemDeleted);
			itemDeleted.Parent.DockAll();
		}
	}
}
