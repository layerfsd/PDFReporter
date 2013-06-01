using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Xml;

using AxiomCoders.PdfTemplateEditor.Common;
using AxiomCoders.PdfTemplateEditor.Forms;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
	/// <summary>
	/// Group used for adding to toolstrip
	/// </summary>
	public enum ToolStripGroup
	{
		NORMAL
	}

	/// <summary>
	/// singleton class used to control how editor is working. It contains info about selected objects, actual project object, etc.
	/// </summary>
	public class EditorController
	{

		public delegate void ShowObjectPropertiesDelegate(object obj);

		/// <summary>
		/// This event is called when properties for a object should be shown
		/// </summary>
		public event ShowObjectPropertiesDelegate ShowObjectProperties; 

		/// <summary>
		/// Singleton variable
		/// </summary>
		private static EditorController instance;
		
		/// <summary>
		/// Singleton 
		/// </summary>
		public static EditorController Instance
		{
		  get 
		  {
		     if (instance == null)
			 {
				instance = new EditorController();
			 }
			 return instance;			 
		  }
		}

        private EditorGrid grid;
        private bool dockable;

        private bool projectSaved;
        public bool ProjectSaved
        {
            get { return projectSaved; }
            set 
            { 
                projectSaved = value; 
                if(projectSaved == false && EditorController.Instance.EditorProject != null)
                {
                    EditorController.Instance.EditorProject.FrmReport.Text = EditorController.Instance.EditorProject.ReportPage.Name + (this.ProjectFileName == "" ? "" : (" - " + System.IO.Path.GetFileName(this.ProjectFileName))) + " *";
                }
                else if(projectSaved == true && EditorController.Instance.EditorProject != null)
                {
                    EditorController.Instance.EditorProject.FrmReport.Text = EditorController.Instance.EditorProject.ReportPage.Name + " - " + System.IO.Path.GetFileName(this.ProjectFileName);
                }
            }
        }

        private string projectFileName = "";
        public string ProjectFileName
        {
            get { return projectFileName; }
        }

        protected MainForm mainProjectForm = null;  
        public MainForm MainFormOfTheProject
        {
            get { return mainProjectForm; }
        }

        public string MainFormText
        {
            get 
            { 
                if(mainProjectForm != null) 
                { 
                    return mainProjectForm.Text; 
                } 
                else 
                {
                    return ""; 
                } 
            }
            set
            {
                if(mainProjectForm != null)
                {
                    mainProjectForm.Text = value;
                }
            }
        }

		/// <summary>
		/// Default constructor
		/// </summary>
		public EditorController()
		{
		}

        public EditorGrid Grid
        {
            get
            {
                if (grid != null)
                {
                    return grid;
                }else{
                    grid = new EditorGrid();
                    return grid;
                }
            }
        }

        public bool Dockable
        {
            get { return dockable; }
            set { dockable = value; }
        }
		
		private EditorViewer editorViewer;
		public AxiomCoders.PdfTemplateEditor.EditorStuff.EditorViewer EditorViewer
		{
			get { return editorViewer; }			
		}

		/// <summary>
		/// Editor project used in this controller
		/// </summary>
		private EditorProject editorProject;
		public AxiomCoders.PdfTemplateEditor.EditorStuff.EditorProject EditorProject
		{
			get { return editorProject; }			
		}				


		/// <summary>
		/// Create new project
		/// </summary>
		/// <param name="owner"></param>
		public bool NewProject(Form owner, bool basicForm)
		{
            if(!basicForm)
            {
                NewProjectForm newProj = new NewProjectForm();
                if(DialogResult.OK == newProj.ShowDialog())
                {
                    if(this.EditorProject != null)
                    {
                        this.EditorProject.Close(true);
                    }

                    mainProjectForm = (MainForm)owner;
                    this.editorProject = new EditorProject();
                    //Set params for project
                    this.EditorProject.ReportPage.Name = newProj.ProjectName;
                    this.EditorProject.ReportPage.MeasureUnit = newProj.MeasureUnit;
                    this.EditorProject.ReportPage.WidthInUnits = newProj.ProjectWidth;
                    this.EditorProject.ReportPage.HeightInUnits = newProj.ProjectHeight;
                    this.EditorProject.ReportPage.Resolution = newProj.ProjectResolution;
                    this.EditorProject.Title = newProj.ProjectTitle;
                    this.EditorProject.Author = newProj.ProjectAuthor;
                    this.EditorProject.Subject = newProj.ProjectSubject;
                    //================================================================
                    this.editorViewer = new EditorViewer(this.editorProject);
                    editorProject.New();
                    editorProject.InitiliazeGUI(owner);
                    //this.EditorProject.FrmReport.Text = newProj.ProjectName + " - " + System.IO.Path.GetFileName(this.ProjectFileName);
                }else
                {
                    return false;
                }
            }
            else 
            {
                mainProjectForm = (MainForm)owner;
                this.editorProject = new EditorProject();
                this.editorViewer = new EditorViewer(this.editorProject);
                editorProject.New();
                editorProject.InitiliazeGUI(owner);
            }

            projectFileName = "";
            projectSaved = false;
            this.EditorProject.FrmReport.Text = EditorProject.Name + " - " + System.IO.Path.GetFileName(this.ProjectFileName);
            OnEditorItemHierarchyChanged();
            return true;
		}

		public void CloseProject(bool closeForm)
		{
            OnEditorItemHierarchyChanged();
			this.editorProject.Close(closeForm);
			this.editorProject = null;
			this.editorViewer = null;
		}



        /// <summary>
        /// Saves project in xml file
        /// </summary>
        public void Save(string fileName, bool isFromAutoSave)
        {
			XmlDocument doc = new XmlDocument();
			XmlElement element = doc.CreateElement("PdfFactoryTemplate");
			doc.AppendChild(element);

			EditorProject.Save(doc, element);

			doc.Save(fileName);

            if(!isFromAutoSave)
            {
                ProjectSaved = true;
                this.projectFileName = fileName;
                this.EditorProject.FrmReport.Text = this.EditorProject.ReportPage.Name + " - " + System.IO.Path.GetFileName(fileName);
            }         
        }

        /// <summary>
        /// Loads actual file from given file name
        /// </summary>
        /// <param name="fileName"></param>
        public void Load(string fileName)
        {
			try
			{
				// disable ActionManager with undo/redo
				ActionManager.Instance.Disabled = true;

				//XmlTextReader txR = new XmlTextReader(fileName);
				XmlDocument doc = new XmlDocument();
				doc.Load(fileName);

				XmlElement element = doc.DocumentElement;

				NewProject(MainForm.ActiveForm, true);
				EditorProject.Load(element);

				this.projectFileName = fileName;
				this.ProjectSaved = true;
				this.EditorProject.FrmReport.Text = this.EditorProject.ReportPage.Name + " - " + System.IO.Path.GetFileName(fileName);
                
                OnEditorItemHierarchyChanged();

			}
			catch (Exception e)
			{
				MessageBox.Show(string.Format("Loading of {0} failed.", e));
			}
			finally
			{
				ActionManager.Instance.Disabled = false;
			}
        }


		private bool inMovingItemsState = false;
		public bool InMovingItemsState
		{
			get { return inMovingItemsState; }		
		}

		/// <summary>
		/// Only one command can be active at a time
		/// </summary>
		private CommandItem currentCommand = null;

		/// <summary>
		/// Select command and return true if selected
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool SelectCommand(float x, float y)
		{
			currentCommand = this.EditorProject.CurrentReportPage.SelectCommand(x, y);
			return currentCommand != null;
		}


		private bool isMovingCommand = false;
		public bool IsMovingCommand
		{
			get { return isMovingCommand; }
		}

		/// <summary>
		/// Start moving command
		/// </summary>
		public void StartMovingCommand()
		{
			if (currentCommand != null)
			{
				currentCommand.StartMoving();
				isMovingCommand = true;
			}			
		}

		/// <summary>
		/// Stop moving command
		/// </summary>
		public void StopMovingCommand()
		{
			if (currentCommand != null)
			{
				currentCommand.StopMoving();
			}
			isMovingCommand = false;
			currentCommand = null;
            OnEditorItemHierarchyChanged();
		}

        /// <summary>
        /// Move command is initiated
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
		public void MoveCommand(float dx, float dy)
		{
			if (IsMovingCommand && currentCommand != null)
			{				
				currentCommand.Move(dx, dy);
			}
		}

		/// <summary>
		/// This will set all selected items to moving state
		/// </summary>
		public void StartMovingSelection()
		{
			foreach(EditorItem item in SelectedItems)
			{
				item.StartMoving();
			}
			inMovingItemsState = true;
		}

		/// <summary>
		/// Call this when moving is stopped
		/// </summary>
		public void StopMovingSelection()
		{
			foreach (EditorItem item in SelectedItems)
			{
				item.StopMoving(true);
			}
			inMovingItemsState = false;
			if (this.SelectedItems.Count == 1 && ShowObjectProperties != null)
			{
				this.ShowObjectProperties(this.SelectedItems[0]);
			}
            OnEditorItemHierarchyChanged();
		}

		/// <summary>
		/// Move selection by delta x, delta y 
		/// </summary>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		public void MoveSelection(float dx, float dy)
		{
			// we need to make ignore list for those items that have parents already selected
			List<EditorItem> ignoreList = new List<EditorItem>();

			foreach(EditorItem item in SelectedItems)
			{
				// check to see if there is any other selected item that is parent for this one
				foreach(EditorItem possibleParent in SelectedItems)
				{
					if (item.CheckIfParent(possibleParent))
					{
						ignoreList.Add(item);
						break;
					}
				}
			}

			foreach (EditorItem item in SelectedItems)
			{
				if (!ignoreList.Contains(item))
				{
					item.MoveBy(dx, dy);
				}				
			}
		}

		/// <summary>
		/// This will delete selected items
		/// </summary>
		public void DeleteSelected()
		{
            EditorItem[] itemList = new EditorItem[SelectedItems.Count];
            itemList = SelectedItems.ToArray();

            for(int i = 0; i < SelectedItems.Count; i++)
            {
                EditorItemFactory.Instance.DeleteItem(itemList[i]);
            }
			SelectedItems.Clear();
            OnEditorItemHierarchyChanged();
		}


		public void DeleteItem(EditorItem item)
		{
			if (this.SelectedItems.Contains(item))
			{
				this.SelectedItems.Remove(item);
			}
			EditorItemFactory.Instance.DeleteItem(item);
            OnEditorItemHierarchyChanged();
		}

		/// <summary>
		/// This will select one item
		/// </summary>
		/// <param name="item"></param>
		public void SelectItem(EditorItem item)
		{
			DeselectAll();
			if (item != null)
			{				
				item.IsSelected = true;
				SelectedItems.Add(item);
				ShowObjectProperties(this.SelectedItems[0]);
                if (SelectionChanged != null)
                {
                    SelectionChanged(SelectedItems);
                }

				//if (EditorViewer != null)
				{
					EditorProject.FrmReport.RefreshPanels();
				}				
			}
		}

		List<EditorItem> clipboard = new List<EditorItem>();

		/// <summary>
		/// This will copy selected
		/// </summary>
		public void Copy()
		{
			clipboard.Clear();
			foreach(EditorItem item in SelectedItems)
			{
				clipboard.Add(item);
			}
		}
	
		/// <summary>
		/// This will cut selected
		/// </summary>
		public void Cut()
		{
			clipboard.Clear();
			foreach (EditorItem item in SelectedItems)
			{
				clipboard.Add(item);
				EditorItemFactory.Instance.DeleteItem(item);
			}
            OnEditorItemHierarchyChanged();
		}

        public delegate void EditorItemHierarchyChangedCallback();

        /// <summary>
        /// Called whenever editor item hierarchy could be changed. Due to pasting, undo/redo, moving, etc
        /// </summary>
        public event EditorItemHierarchyChangedCallback EditorItemHierarchyChanged;

		/// <summary>
		/// This will paste clipboard
		/// </summary>
		public void Paste()
		{
			DeselectAll();
			foreach(EditorItem item in clipboard)
			{
				EditorItem newItem = (EditorItem)item.Clone();
				EditorItemFactory.Instance.CreateItem(newItem);
				this.SelectedItems.Add(newItem);
				newItem.IsSelected = true;              
			}

			ShowPropertiesOfSelectedItems();
            OnEditorItemHierarchyChanged();
		}

        /// <summary>
        /// Left so anyone can initiate this call when hierarcy is changed
        /// </summary>
        public void OnEditorItemHierarchyChanged()
        {
            if (EditorItemHierarchyChanged != null)
            {
                EditorItemHierarchyChanged();
            }
        }

		/// <summary>
		/// this will return cursor for current control
		/// </summary>
		/// <param name="mouseX"></param>
		/// <param name="mouseY"></param>
		/// <returns></returns>
		public Cursor GetCursor(int mouseX, int mouseY)
		{
			//float x = 0, y = 0;
			//x = this.EditorProject.ReportPage.Viewer.OffsetXValue - EditorProject.ReportPage.Viewer.PageStartX + (float)mouseX;
			//y = this.EditorProject.ReportPage.Viewer.OffsetYValue - EditorProject.ReportPage.Viewer.PageStartY + (float)mouseY;
            if (this.EditorProject != null && this.EditorProject.CurrentReportPage != null)
            {
                return this.EditorProject.CurrentReportPage.GetCursor(mouseX, mouseY);
            }
            else
            {
                return Cursor.Current;
            }
		}

        /// <summary>
        /// Callback for selection changed event
        /// </summary>
        /// <param name="selectedItems"></param>
        public delegate void SelectionChangedDelegate(List<EditorItem> selectedItems);

        /// <summary>
        /// Called whenever selection is changed
        /// </summary>
        public event SelectionChangedDelegate SelectionChanged;

		/// <summary>
		/// This will select item. You can use list of selected items to check what is selected. Selection is done so deepest object becomes selected. 
		/// </summary>
		/// <param name="x">world x coordinate</param>
		/// <param name="y">world y coordinate</param>
		/// <returns></returns>
		public void SelectItem(float mx, float my, bool addToSelection)
		{			
			EditorItem selectedItem = editorProject.CurrentReportPage.SelectItem(mx, my);			
			if (selectedItem != null)
			{
				if (!this.SelectedItems.Contains(selectedItem))
				{
					if (!addToSelection)
					{
						DeselectAll();
						selectedItem.IsSelected = true;
						this.SelectedItems.Add(selectedItem);
					}
					else
					{
						selectedItem.IsSelected = true;
						this.SelectedItems.Add(selectedItem);
					}
				}
			}
			else if (!addToSelection)
			{
				DeselectAll();
			}
            
			ShowPropertiesOfSelectedItems();

            // notify for selection changed
            if (this.SelectionChanged != null)
            {
                this.SelectionChanged(this.SelectedItems);
            }
		}

		/// <summary>
		/// Show properties for selected items
		/// </summary>
		private void ShowPropertiesOfSelectedItems()
		{
			// show properties for objects
			if (this.SelectedItems.Count == 1)
			{
				if (this.ShowObjectProperties != null)
				{
					this.ShowObjectProperties(this.SelectedItems[0]);
				}
			}
			else if (this.SelectedItems.Count > 1)
			{

			}
			else
			{
				// show report page properties if nothing is selected
				this.ShowObjectProperties(this.editorProject.ReportPage);
			}
		}
		
		/// <summary>
		/// Deselect all items
		/// </summary>
		public void DeselectAll()
		{
			if (SelectedItems.Count > 0)
			{
				//CommandItems.Clear();
			}
			foreach(EditorItem selItem in SelectedItems)
			{
				selItem.IsSelected = false;
			}
			SelectedItems.Clear();
            if (SelectionChanged != null)
            {
                SelectionChanged(SelectedItems);
            }
		}

		/// <summary>
		/// List of selected items
		/// </summary>
		private List<EditorItem> selectedItems = new List<EditorItem>();

		/// <summary>
		/// List of selected items
		/// </summary>
		public List<EditorItem> SelectedItems
		{
			get { return selectedItems; }		
		}

		/// <summary>
		/// List of items in toobar
		/// </summary>
		private List<EditorToolBarPlugin> toolBarItems = new List<EditorToolBarPlugin>();

		

        public void MakePreviewItemList()
        {
            EditorProject.SetPreviewListActive(true);
        }

        public void RemovePreviewItemList()
        {
            EditorProject.SetPreviewListActive(false);
            this.EditorProject.ReportPage.rectMatrix = new List<RectangleNormal>();
        }
        


		/// <summary>
		/// This is toolbar button that is valid when editorItemSelectedForcreation type is valid. It is used to disable check state when required
		/// </summary>
		private ToolStripButton tbbCheckedForCreate = null;

		/// <summary>
		/// This is toolbar button that is valid when editorItemSelectedForcreation type is valid. It is used to disable check state when required
		/// You don't need to set this to null when type for creation becomes null.
		/// </summary>
		public System.Windows.Forms.ToolStripButton TbbCheckedForCreate
		{
			get { return tbbCheckedForCreate; }
			set { tbbCheckedForCreate = value; }
		}
		/// <summary>
		/// This is type of object that is about to be created
		/// </summary>
		private Type editorItemSelectedForCreation = null;        

		/// <summary>
		/// This is type of item to create. This becomes valid when something is checked on toolbar
		/// </summary>
		public Type EditorItemSelectedForCreation
		{
			get { return editorItemSelectedForCreation; }
			set 
			{
				if (value == null)
				{
					if (TbbCheckedForCreate != null)
					{
						TbbCheckedForCreate.Checked = false;
					}					
					tbbCheckedForCreate = null;
				}
				editorItemSelectedForCreation = value; 
			}
		}

        /// <summary>
        /// Call this to perform undo
        /// </summary>
        public void Undo()
        {
            if (EditorProject == null)
            {
                return;
            }

            ActionManager.Instance.Undo();
            EditorViewer.RefreshView();
            OnEditorItemHierarchyChanged();
        }

        /// <summary>
        /// Call this to perform redo
        /// </summary>
        public void Redo()
        {
            if (EditorProject == null)
            {
                return;
            }

            ActionManager.Instance.Redo();
            EditorViewer.RefreshView();
            OnEditorItemHierarchyChanged();
        }
	}

}
