using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
	public class EditorCreateAction: EditorAction
	{
		private EditorItem itemCreated;
		private int parentIndex;

		public EditorCreateAction(EditorItem itemCreated)
		{
			this.itemCreated = itemCreated;
		}

		/// <summary>
		/// Attach item created again to parent
		/// </summary>
		public override void Redo()
		{
			itemCreated.Parent.Children.Insert(parentIndex, itemCreated);
			itemCreated.Parent.DockAll();
		}

		/// <summary>
		/// Detach item from parent
		/// </summary>
		public override void Undo()
		{
			// find exact child and remember index of it
			parentIndex = 0;
			foreach (EditorItem child in itemCreated.Parent.Children)
			{
				if (child == itemCreated)
				{
					break;
				}
				parentIndex++;
			}
			itemCreated.StopMoving(false);
			itemCreated.IsSelected = false;
			itemCreated.Parent.Children.Remove(itemCreated);
			itemCreated.Parent.DockAll();
		}
	}
}
