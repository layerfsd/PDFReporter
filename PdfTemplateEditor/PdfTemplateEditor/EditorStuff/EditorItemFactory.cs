using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{

	public delegate void EditorItemDeletedCallback(EditorItem item);
	public delegate void EditorItemCreatedCallback(EditorItem itemCreated);

	/// <summary>
	/// singleton class used to create EditorItems
	/// </summary>
	public class EditorItemFactory
	{
		/// <summary>
		/// singleton variable
		/// </summary>
		private static EditorItemFactory instance;

		/// <summary>
		/// Singleton
		/// </summary>
		public static EditorItemFactory Instance
		{
			get 
			{
				if (instance == null)
				{
					instance = new EditorItemFactory();
				}
				return instance;
			}
		}


		public event EditorItemDeletedCallback EditorItemDeleted;
		public event EditorItemCreatedCallback EditorItemCreated;		

		public EditorItem CreateItem(EditorItem newItem)
		{
			// tell action manager that we created new item
			ActionManager.Instance.EditorItemCreated(newItem);
			if (EditorItemCreated != null && newItem.NotifyCreation)
			{
				EditorItemCreated(newItem);
			}
                       
			return newItem;
		}

		/// <summary>
		/// This should create item or return null in case item does not inherit EditorItem. It is connected with ActionManager so Undo/Redo should work
		/// </summary>
		/// <param name="typeOfItem"></param>
		/// <returns></returns>
		public EditorItem CreateItem(Type typeOfItem)
		{
            if (IsEditorItem(typeOfItem))
			{
				EditorItem newItem = typeOfItem.GetConstructor(Type.EmptyTypes).Invoke(null) as EditorItem;
				// tell action manager that we created new item
				ActionManager.Instance.EditorItemCreated(newItem);
				if (EditorItemCreated != null && newItem.NotifyCreation)
				{
					EditorItemCreated(newItem);
				}
				return newItem;
			}
			else 
			{
				return null;
			}			
		}		

		/// <summary>
		/// this will delete item 
		/// </summary>
		/// <param name="item"></param>
		public void DeleteItem(EditorItem item)
		{
			// mark item as deleted
			//item.IsDeleted = true;			
			// detach it from parent
			if (item.Parent == null)
			{
				throw new Exception("Item for removal has no parent");
			}
			else 
			{
				// find exact child and remember index of it
				int parentIndex = 0;
				foreach(EditorItem child in item.Parent.Children)
				{
					if (child == item)
					{
						break;
					}
					parentIndex++;
				}
				item.Parent.Children.Remove(item);
				item.StopMoving(true);
				item.IsSelected = false;
				if (EditorItemDeleted != null && item.NotifyDeletion)
				{
					EditorItemDeleted(item);
				}

				ActionManager.Instance.EditorItemDeleted(item, parentIndex);

				// perform docking on parent to rearrange items
				item.Parent.DockAll();
			}
		}

        /// <summary>
        /// Added by: Vladan Spasojevic, 5.Januar.2009
        /// Recursive iterate ower base types, and determine is given type derivated from EditorItem.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsEditorItem(Type type)
        {
            if (type == null)
            {
                return false;
            }

            if (type.BaseType == typeof(EditorItem))
            {
                return true;
            }
            return IsEditorItem(type.BaseType);
        }

		/// <summary>
		/// Default constructor
		/// </summary>
		public EditorItemFactory()
		{

		}
	}
}
