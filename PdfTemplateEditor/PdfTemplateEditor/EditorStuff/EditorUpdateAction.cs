using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;


namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
	public class EditorUpdateAction: EditorAction
	{
		private string[] properties = null;
		private object[] oldValues;
		private object[] newValues;

		private string property;
		private object oldValue;
		private object newValue;
		private EditorItem item;

		public EditorUpdateAction(EditorItem item, string property, object oldValue, object newValue)
		{
			this.item = item;
			this.property = property;
			this.oldValue = oldValue;
			this.newValue = newValue;
		}
		

		public EditorUpdateAction(EditorItem item, string[] property, object[] oldValues, object[] newValues)
		{
			this.item = item;
			this.properties = property;
			this.oldValues = oldValues;
			this.newValues = newValues;
		}

		public override void Redo()
		{
			if (properties != null)
			{
				for(int i = 0; i < properties.Length; i++)
				{
					PropertyInfo pi = item.GetType().GetProperty(properties[i]);
					pi.SetValue(item, newValues[i], null);			
				}
			}
			else
			{
				PropertyInfo pi = item.GetType().GetProperty(property);
				pi.SetValue(item, newValue, null);			
			}
			item.DockAll();
		}

		public override void Undo()
		{
			if (properties != null)
			{
				for (int i = 0; i < properties.Length; i++)
				{
					PropertyInfo pi = item.GetType().GetProperty(properties[i]);
					pi.SetValue(item, oldValues[i], null);
				}
			}
			else
			{
				PropertyInfo pi = item.GetType().GetProperty(property);
				pi.SetValue(item, oldValue, null);
			}
			item.DockAll();
		}
	}
}
