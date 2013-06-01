using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
	/// <summary>
	/// Manages history of actions and provides capability for redo/undo
	/// </summary>
	public class ActionManager
	{
		/// <summary>
		/// Singleton variable
		/// </summary>
		private static ActionManager instance;
		
		/// <summary>
		/// Singleton 
		/// </summary>
		public static ActionManager Instance
		{
		  get 
		  {
		     if (instance == null)
			 {
				instance = new ActionManager();
			 }
			 return instance;			 
		  }
		}

		/// <summary>
		/// Should action manager be disabled 
		/// </summary>
		private bool disabled;

		/// <summary>
		/// If this is true action manager will be disabled and all calls will fail
		/// </summary>
		public bool Disabled
		{
			get { return disabled; }
			set { disabled = value; }
		}
		


		/// <summary>
		/// List of actions available
		/// </summary>
		private List<EditorAction> actions = new List<EditorAction>();

		/// <summary>
		/// Place where new action should be put
		/// </summary>
		private int currentActionIndex = 0; 

		/// <summary>
		/// Call this when EditorItem is created
		/// </summary>
		/// <param name="obj"></param>
		public void EditorItemCreated(EditorItem item)
		{
			if (!Disabled)
			{
				EditorCreateAction action = new EditorCreateAction(item);
				actions.Insert(currentActionIndex, action);
				currentActionIndex++;

				RemoveNotValidItems();
				EditorController.Instance.ProjectSaved = false;
			}
		}

		/// <summary>
		/// Call this when editor item is deleted
		/// </summary>
		/// <param name="item"></param>
		public void EditorItemDeleted(EditorItem item, int parentIndex)
		{
			if (!Disabled)
			{
				EditorDeleteAction action = new EditorDeleteAction(item, parentIndex);
				actions.Insert(currentActionIndex, action);
				currentActionIndex++;

				RemoveNotValidItems();
				EditorController.Instance.ProjectSaved = false;
			}
		}
		
		/// <summary>
		/// This is called when some property is changed so undo/redo works in that case
		/// </summary>
		/// <param name="item"></param>
		/// <param name="property"></param>
		/// <param name="newValue"></param>
		/// <param name="oldValue"></param>
		public void EditorItemUpdate(EditorItem item, string property, object oldValue, object newValue)
		{
			if (!Disabled)
			{
				EditorUpdateAction action = new EditorUpdateAction(item, property, oldValue, newValue);
				actions.Insert(currentActionIndex, action);
				currentActionIndex++;

				RemoveNotValidItems();
				EditorController.Instance.ProjectSaved = false;
			}
		}

		/// <summary>
		/// group of properties is changed. This is used for example when locationx,y are changed
		/// </summary>
		/// <param name="item"></param>
		/// <param name="properties"></param>
		/// <param name="newValues"></param>
		/// <param name="oldValues"></param>
		public void EditorItemUpdate(EditorItem item, string[] properties, object[] oldValues, object[] newValues)
		{
			if (!Disabled)
			{
				EditorUpdateAction action = new EditorUpdateAction(item, properties, oldValues, newValues);
				actions.Insert(currentActionIndex, action);
				currentActionIndex++;

				RemoveNotValidItems();
				EditorController.Instance.ProjectSaved = false;
			}
		}	

		
		/// <summary>
		/// This removed items that are after currentAction index in list. Usually this is called when new item is added to make sure other action items do not exist anymore
		/// </summary>
		private void RemoveNotValidItems()
		{
			// remove all items from currentActionIndex as it should point to empty last place
			if (actions.Count - currentActionIndex > 0)
			{
				actions.RemoveRange(currentActionIndex, actions.Count - currentActionIndex);
			}			
		}

		public delegate void UndoRedoPerformedDelegate();

		/// <summary>
		/// this event is called after undo/redo operation
		/// </summary>
		public event UndoRedoPerformedDelegate AfterUndoRedo;


		/// <summary>
		/// Call this for undo
		/// </summary>
		public void Undo()
		{
			if (currentActionIndex-1 >= 0)
			{
				actions[currentActionIndex - 1].Undo();
				currentActionIndex--;
                EditorController.Instance.ProjectSaved = false;
			}

			if (AfterUndoRedo != null)
			{
				AfterUndoRedo();
			}
		}

		/// <summary>
		/// Call this for redo
		/// </summary>
		public void Redo()
		{
			if (currentActionIndex < actions.Count)
			{
				actions[currentActionIndex].Redo();
				currentActionIndex++;

                EditorController.Instance.ProjectSaved = false;
			}

			if (AfterUndoRedo != null)
			{
				AfterUndoRedo();
			}
		}

        
        /// <summary>
        /// Clears all actions from action list
        /// </summary>
        public void ClearAllActions()
        {
            actions.Clear();
        }



		/// <summary>
		/// default constructor
		/// </summary>
		public ActionManager()
		{

		}
	}
}
