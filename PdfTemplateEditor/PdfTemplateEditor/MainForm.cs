using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using AxiomCoders.SharedNet20.Forms;

using AxiomCoders.PdfTemplateEditor.Forms;
using AxiomCoders.PdfTemplateEditor.EditorStuff;
using AxiomCoders.PdfTemplateEditor.EditorItems;
using AxiomCoders.PdfTemplateEditor.Common;
using AxiomCoders.SharedNet20;

namespace AxiomCoders.PdfTemplateEditor
{
	public partial class MainForm : formBase
	{
        private bool inPreviewMode;        

        /// <summary>
        /// If report is in preview mode
        /// </summary>
        public bool InPreviewMode
        {
            get { return inPreviewMode; }
            set { inPreviewMode = value; }
        }

        private string[] args;


		public MainForm(string[] args)
		{
			InitializeComponent();
            AppOptions.Instance.Load();
            if(AppOptions.Instance.ThemeProfessional)
            {
                this.professionalToolStripMenuItem_Click(this, EventArgs.Empty);
            }
            else
            {
                this.systemToolStripMenuItem_Click(this, EventArgs.Empty);
            }

            this.args = args;
            ProductHelp.OfflineHelpSource = "C:/Projects/PdfReporter/Help/AxiomCoders Pdf Reports Help.chm";
            ProductHelp.OnlineHelpSource = "http://axiomcoders.com/support_files/help/pdfreports/html/index.htm";
            ProductHelp.UseOfflineHelpFirst = true;
		}

		/// <summary>
		/// Create new project
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void miNewProject_Click(object sender, EventArgs e)
		{
            if(InPreviewMode)
            {
                return;
            }
            if(EditorController.Instance.EditorProject != null && !EditorController.Instance.ProjectSaved)
            {
                if(DialogResult.Yes == MessageBox.Show("Current project will be closed... \n\nDo you want to save changes?", "New peoject...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    miSave_Click(this, EventArgs.Empty);
                }
                else
                {
                    EditorController.Instance.ProjectSaved = true;
                }
            }
            if(EditorController.Instance.NewProject(this, false))
            {
                miPreferences.Enabled = true;
                EditorController.Instance.ProjectSaved = false;
            }
            else
            {
                if(EditorController.Instance.EditorProject != null)
                {
                    miPreferences.Enabled = true;
                    EditorController.Instance.ProjectSaved = false;
                }else{
                    miPreferences.Enabled = false;
                }                
            }
		}

		private void miExit_Click(object sender, EventArgs e)
		{
            if(InPreviewMode)
            {
                return;
            }
			Application.Exit();
		}

		/// <summary>
		/// Copy options to GUI
		/// </summary>
		private void GetOptionsToGUI()
		{
			miShowGrid.Checked = AppOptions.Instance.ShowGrid;
			miShowRulers.Checked = AppOptions.Instance.ShowRulers;
			miShowBalloonBorders.Checked = AppOptions.Instance.ShowBalloonBorders;
			
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
            if(InPreviewMode)
            {
                return;
            }
			// Load options
			
			AppOptions.Instance.Load();
			GetOptionsToGUI();

			// This will initialize toolstrip with editor items
			EditorController.Instance.ShowObjectProperties += new EditorController.ShowObjectPropertiesDelegate(Instance_ShowObjectProperties);
            EditorController.Instance.SelectionChanged += new EditorController.SelectionChangedDelegate(Instance_SelectionChanged);
            EditorController.Instance.EditorItemHierarchyChanged += new EditorController.EditorItemHierarchyChangedCallback(UpdateObjectHierarchyList);

			// listen to editor items creations
			EditorItemFactory.Instance.EditorItemCreated += new EditorItemCreatedCallback(EditorItem_EditorItemCreated);
			EditorItemFactory.Instance.EditorItemDeleted += new EditorItemDeletedCallback(Instance_EditorItemDeleted);
			EditorItem.ItemPlaced += new ItemPlacedCallback(EditorItem_ItemPlaced);

            // Load project if it is set by arguments
            if (args.Length >= 1)
            {
                OpenProject(args[0]);
            }
		}

        /// <summary>
        /// Called when selection is changed
        /// </summary>
        /// <param name="selectedItems"></param>
        void Instance_SelectionChanged(List<EditorItem> selectedItems)
        {
            if (selectedItems.Count == 1)
            {
                // perform selection in object hierarchy tree view
                TreeNode node = FindNodeWithEditorItem(null, selectedItems[0]);
                if (node != null)
                {
                    tvItems.AfterSelect -= tvItems_AfterSelect;
                    tvItems.SelectedNode = node;
                    tvItems.AfterSelect += tvItems_AfterSelect;
                }
            }
        }

		/// <summary>
		/// When some item is placed
		/// </summary>
		/// <param name="item"></param>
		void EditorItem_ItemPlaced(EditorItem item)
		{
            if(InPreviewMode)
            {
                return;
            }
			AddNewEditorItemToList(item);
		}
		
		void Instance_EditorItemDeleted(EditorItem item)
		{
			// add to combo box
			cmbObjects.SelectedValueChanged -= cmbObjects_SelectedValueChanged;
			EditorItemComboItem foundCbxItem = null;
			foreach (EditorItemComboItem cbxItem in itemsCreated)
			{
				if (cbxItem.Item == item)
				{
					foundCbxItem = cbxItem;
					break;
				}
			}

			itemsCreated.Remove(foundCbxItem);
			cmbObjects.DataSource = null;
			cmbObjects.DataSource = itemsCreated;
			cmbObjects.DisplayMember = "Text";
			cmbObjects.ValueMember = "Item";
			cmbObjects.Refresh();
			cmbObjects.SelectedValueChanged += cmbObjects_SelectedValueChanged;			
		}	

		[System.Reflection.Obfuscation(Exclude=true)]
		private class EditorItemComboItem
		{
			private string text;
			public string Text
			{
				get { return text; }
				set { text = value; }
			}
			private EditorItem item;
			public AxiomCoders.PdfTemplateEditor.EditorStuff.EditorItem Item
			{
				get { return item; }
				set { item = value; }
			}

			public EditorItemComboItem(EditorItem item)
			{
				this.Text = string.Format("{0} : {1}", item.Name, item.GetType().Name);
				this.item = item;
			}
		}

		private List<EditorItemComboItem> itemsCreated = new List<EditorItemComboItem>();


		private void PopulateTreeNode(TreeNode parentNode, EditorItem parentItem)
		{
			foreach (EditorItem child in parentItem.Children)
			{
				TreeNode node = new TreeNode(string.Format("{0} : {1}", child.Name, child.GetType().Name));
				parentNode.Nodes.Add(node);
				PopulateTreeNode(node, child);
			}
		}

        /// <summary>
        /// Find node that contains editor item. This is recursive one
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="item"></param>
        /// <returns></returns>
		private TreeNode FindNodeWithEditorItem(TreeNode parent, EditorItem item)
		{
            if(tvItems.Nodes.Count == 0)
            {
                return null;
            }

			if (parent == null)
			{
				foreach (TreeNode node in tvItems.Nodes)
				{
					if ((EditorItem)node.Tag == item)
					{
						return node;
					}
					TreeNode res = FindNodeWithEditorItem(node, item);
					if (res != null)
					{
						return res;
					}
				}
			}
			else
			{
				foreach (TreeNode node in parent.Nodes)
				{
					if ((EditorItem)node.Tag == item)
					{
						return node;
					}
					TreeNode res = FindNodeWithEditorItem(node, item);
					if (res != null)
					{
						return res;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Add new item to tvItems list
		/// </summary>
		/// <param name="itemsCreated"></param>
		public void AddNewEditorItemToList(EditorItem itemsCreated)
		{
			// if node exists then remove it first and later add again
			TreeNode fNode = FindNodeWithEditorItem(null, itemsCreated);
			if (fNode != null)
			{
                // turn off notifications for this removal
                tvItems.AfterSelect -= tvItems_AfterSelect;
				if (fNode.Parent != null)
				{
					fNode.Parent.Nodes.Remove(fNode);
				}
				else 
				{                    
					tvItems.Nodes.Remove(fNode);                    
				}
                tvItems.AfterSelect += tvItems_AfterSelect;
			}

            // Now add new item
			TreeNode foundNode = FindNodeWithEditorItem(null, itemsCreated.Parent);
			if (foundNode != null)
			{				
				if (fNode != null)
				{
					foundNode.Nodes.Add(fNode);
				}
				else 
				{
					TreeNode newNode = foundNode.Nodes.Add(itemsCreated.Name);
					newNode.Tag = itemsCreated;
				}
			}
			else 
			{
				if (fNode != null)
				{
					tvItems.Nodes.Add(fNode);
				}
				else
				{
					TreeNode newNode = tvItems.Nodes.Add(itemsCreated.Name);
					newNode.Tag = itemsCreated;
				}
			}
		}

        /// <summary>
        /// Adds new item to object hierarchy tree view
        /// </summary>
        /// <param name="newItem"></param>
        private void AddNewToObjectHierarchy(TreeNode parentNode, EditorItem newItem)
        {
            TreeNode newNode;
            if (parentNode == null)
            {
                newNode = tvItems.Nodes.Add(newItem.Name);
            }
            else
            {
                newNode = parentNode.Nodes.Add(newItem.Name);
            }
            newNode.Tag = newItem;            
            foreach (EditorItem item in newItem.Children)
            {
                AddNewToObjectHierarchy(newNode, item);
            }            
        }

        /// <summary>
        /// Update object hierarchy list from report page
        /// </summary>
        private void UpdateObjectHierarchyList()
        {
            tvItems.AfterSelect -= tvItems_AfterSelect;
            // save any selections from tree view
            // clear tree view
            tvItems.Nodes.Clear();
            if (EditorController.Instance.EditorProject.CurrentReportPage != null)
            {
                ReportPage reportPage = EditorController.Instance.EditorProject.CurrentReportPage;
                foreach (EditorItem item in reportPage.Children)
                {
                    AddNewToObjectHierarchy(null, item);
                }
            }

            tvItems.AfterSelect += tvItems_AfterSelect;
        }

		/// <summary>
		/// Remove item
		/// </summary>
		/// <param name="item"></param>
		private void RemoveEditorItemFromList(EditorItem item)
		{
			TreeNode foundNode = FindNodeWithEditorItem(null, item.Parent);
			TreeNode foundNode2 = FindNodeWithEditorItem(null, item);
			if (foundNode != null)
			{
				foundNode.Nodes.Remove(foundNode2);
			}
			else if(foundNode2 != null) 
			{
				tvItems.Nodes.Remove(foundNode2);
			}
		}


		void EditorItem_EditorItemCreated(EditorItem itemCreated)
		{
            if(InPreviewMode)
            {
                return;
            }
			// add to combo box
			cmbObjects.SelectedValueChanged -= cmbObjects_SelectedValueChanged;
			itemsCreated.Add(new EditorItemComboItem(itemCreated));
			cmbObjects.DataSource = null;
			cmbObjects.DataSource = itemsCreated;
			cmbObjects.DisplayMember = "Text";
			cmbObjects.ValueMember = "Item";			
			cmbObjects.Refresh();
			cmbObjects.SelectedValueChanged += cmbObjects_SelectedValueChanged;
		}




        private void SelectItemInComboBox(object obj)
		{
            if(InPreviewMode)
            {
                return;
            }
			for (int i = 0; i < cmbObjects.Items.Count; i++)
			{
				if ((cmbObjects.Items[i] as EditorItemComboItem).Item == obj)
				{
					cmbObjects.SelectedValueChanged -= cmbObjects_SelectedValueChanged;
					cmbObjects.SelectedIndex = i;
					cmbObjects.SelectedValueChanged += cmbObjects_SelectedValueChanged;
					return;
				}

			}
		}


		/// <summary>
		/// Show object properties in property grid
		/// </summary>
		/// <param name="obj"></param>
		void Instance_ShowObjectProperties(object obj)
		{
			this.propertyGrid.SelectedObject = obj;
			SelectItemInComboBox(obj);
			
		}

		private void tbbArrowItem_Click(object sender, EventArgs e)
		{
			EditorController.Instance.EditorItemSelectedForCreation = null;
		}

		private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			if (propertyGrid.SelectedObject != null)
			{
				ActionManager.Instance.EditorItemUpdate((EditorItem)propertyGrid.SelectedObject, e.ChangedItem.PropertyDescriptor.Name, e.OldValue, e.ChangedItem.Value);
				EditorController.Instance.EditorViewer.RefreshView();
			}
		}

        /// <summary>
        /// Added by: Vladan Spasojevic
        /// Solved problem when user click on ToolStripItem, to uncheck all another items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbbStripOne_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            if (e.ClickedItem != null)
            {
                if (e.ClickedItem is ToolStripButton)
                {
                    for (int i = 0; i < tbbStripOne.Items.Count; i++)
                    {
                        if (tbbStripOne.Items[i] is ToolStripButton)
                        {
                            ((ToolStripButton)tbbStripOne.Items[i]).Checked = false;
                        }
                    }
                }
            }
        }
        	
        /// <summary>
        /// Perform undo 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void miUndo_Click(object sender, EventArgs e)
		{
            if(InPreviewMode)
            {
                return;
            }
            EditorController.Instance.Undo();		
			propertyGrid.Refresh();		
		}

        /// <summary>
        /// Perform redo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void miRedo_Click(object sender, EventArgs e)
		{
            if(InPreviewMode)
            {
                return;
            }
			EditorController.Instance.Redo();			
			propertyGrid.Refresh();
		}



        /// <summary>
        /// Function returns false if there is dynamic items with no data stream or column set
        /// </summary>
        private List<string> noDataList = new List<string>();
        private bool ValidateDynamicItems(EditorItem item)
        {
            bool result = true;
            if(item is DynamicEditorItemInterface)
            {
                DynamicEditorItemInterface tmpItem = (DynamicEditorItemInterface)item;
                if(tmpItem.SourceDataStream == string.Empty || tmpItem.SourceColumn == string.Empty)
                {
                    if(!(item is Balloon) || (tmpItem.SourceDataStream == string.Empty && item is DynamicBalloon))
                    {
                        noDataList.Add(item.Name);
                        result = false;
                    }
                }
            }
            foreach(EditorItem item2 in item.Children)
            {
                if(ValidateDynamicItems(item2) == false)
                    result = false;
            }
            return result;
        }



        /// <summary>
        /// This function returns false when dynamic balloon have same data stream as his parent, if parent is also dynamic balloon
        /// </summary>
        private List<string> invalidBalloons = new List<string>();
        private bool ValidateDynamicBalloons(EditorItem item)
        {
            bool result = true;
            foreach(EditorItem child in item.Children)
            {
                bool interResult = true;
                interResult = ValidateDynamicBalloons(child);
                if(result)
                    result = interResult;
            }

            if(item is DynamicBalloon && item.Parent is DynamicBalloon)
            {
                DynamicBalloon tmpItem1 = (DynamicBalloon)item;
                DynamicBalloon tmpItem2 = (DynamicBalloon)item.Parent;
                if (tmpItem1.SourceDataStream == tmpItem2.SourceDataStream)
                {
                    string tmpStr = "";
                    tmpStr = tmpItem1.Name + " AND " + tmpItem2.Name;
                    if(!invalidBalloons.Contains(tmpStr))
                    {
                        invalidBalloons.Add(tmpStr);
                    }
                    result = false;
                }
            }
            return result;
        }


        /// <summary>
        /// This function return false if there is dynamic items on the page and they don't have parent balloon.
        /// </summary>
        /// <returns></returns>
        private bool CheckDynamicItemsOnPage()
        {
            foreach (EditorItem tmpItem in EditorController.Instance.EditorProject.ReportPage.Children)
            {
                if(tmpItem is DynamicEditorItemInterface && !(tmpItem is Balloon))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// This function checks to see if there is balloons that are in the same level
        /// </summary>
        /// <returns></returns>
        private List<string> balloonIntersections = new List<string>();
        private bool ValidateBalloonIntersections(EditorItem item)
        {
            if(!(item is ReportPage) && item is Balloon)
            {
                foreach(EditorItem child in item.Parent.Children)
                {
                    if(child == item || !(child is Balloon))
                        continue;

                    if(!(item.LocationInPixelsX + item.WidthInPixels - 0.5f < child.LocationInPixelsX ||
                        item.LocationInPixelsX + 0.5f > child.LocationInPixelsX + child.WidthInPixels ||
                        item.LocationInPixelsY + item.HeightInPixels - 0.5f < child.LocationInPixelsY ||
                        item.LocationInPixelsY + 0.5f > child.LocationInPixelsY + child.HeightInPixels))

                    {
                        string result = item.Name + " AND " + child.Name;
                        string result2 = child.Name + " AND " + item.Name;
                        if(!balloonIntersections.Contains(result) && !balloonIntersections.Contains(result2))
                        {
                            balloonIntersections.Add(result);
                        }
                        continue;
                    }
                }
            }

            foreach (EditorItem child in  item.Children)
            {
                ValidateBalloonIntersections(child);
            }

            if(balloonIntersections.Count > 0)
                return false;
            
            return true;
        }


        /// <summary>
        /// This function checks if is it OK to save project to template file.
        /// </summary>
        /// <returns></returns>
        private bool ValidateSave()
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return false;
            }

            if(!CheckDynamicItemsOnPage())
            {
                MessageBox.Show("You have dynamic items on page without balloon parent!", "Save Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            

            if(!ValidateDynamicBalloons(EditorController.Instance.EditorProject.Children[0]))
            {
                string tmpMessage = "";
                int tmpCount = 0;
                foreach(string text in invalidBalloons)
                {
                    tmpCount++;
                    tmpMessage += tmpCount.ToString() + ". " + text + "\r\n";
                }
                MessageBox.Show("You have Dynamic Balloons that have same Data Streams!\r\nBalloons:\r\n" + tmpMessage, "Save Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                invalidBalloons.Clear();
                return false;
            }


            if(!ValidateBalloonIntersections(EditorController.Instance.EditorProject.ReportPage))
            {
                string result = "";
                int count = 0;
                foreach (string item in balloonIntersections)
                {
                    count++;
                    result += count.ToString() + ". " + item + "\r\n";
                }
                MessageBox.Show("There is balloon with invalid intersections:\r\n" + result, "Save Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                balloonIntersections.Clear();
                return false;
            }



            bool Result = true;
            foreach(EditorItem item in EditorController.Instance.EditorProject.Children)
            {
                if(ValidateDynamicItems(item) == false)
                {
                    Result = false;
                    break;
                }
            }

            if(!Result)
            {
                string tmpMessage = "";
                int tmpCount = 0;
                foreach(string text in noDataList)
                {
                    tmpCount++;
                    tmpMessage += tmpCount.ToString() + ". " + text + "\r\n";
                }
                MessageBox.Show("You have items in project with no \"Data Stream\" or \"Column\" set!\r\nItems:\r\n" + tmpMessage, "Save...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                noDataList.Clear();
            }

            return true;
        }




        private void miSaveAs_Click(object sender, EventArgs e)
        {
            if(!ValidateSave())
            {
                return;
            }


            if(inPreviewMode)
            {
                if(DialogResult.Yes == MessageBox.Show("You can't save in preview mode. \n\n Do you want to exit preview mode end save project now?", "Saving...", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1))
                {
                    tbbPreview_Click(this, EventArgs.Empty);
                }
                else
                {
                    return;
                }
            }

            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "Pdf report template project files (*.prtp)|*.prtp";
            saveDlg.Title = "Save your project...";
            saveDlg.InitialDirectory = AppOptions.Instance.LastSavePath;//Application.StartupPath;
            //saveDlg.RestoreDirectory = true;
            
            if (saveDlg.ShowDialog() != DialogResult.Cancel && saveDlg.FileName != "")
            {
                EditorController.Instance.Save(saveDlg.FileName, false);
                AppOptions.Instance.LastSavePath = System.IO.Path.GetDirectoryName(saveDlg.FileName);
                MessageBox.Show("Save successful!", "Save");
            }
        }
        
        private void miOpenProject_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            if(inPreviewMode)
            {
                tbbPreview_Click(this, EventArgs.Empty);
            }

            OpenFileDialog opnDlg = new OpenFileDialog();
            opnDlg.Title = "Open project...";
            opnDlg.Filter = "Pdf report template project files (*.prtp)|*.prtp";
            opnDlg.InitialDirectory = AppOptions.Instance.LastOpenPath;//Application.StartupPath;
            //opnDlg.RestoreDirectory = true;
            
            if(opnDlg.ShowDialog() == DialogResult.OK && opnDlg.FileName != "")
            {
                if(EditorController.Instance.EditorProject != null)
                {
                    EditorController.Instance.CloseProject(true);
                }

                OpenProject(opnDlg.FileName);
            }
        }

        /// <summary>
        /// Open Project with file name
        /// </summary>
        /// <param name="fileName"></param>
        private void OpenProject(string fileName)
        {
            if(InPreviewMode)
            {
                return;
            }
            EditorController.Instance.Load(fileName);
            AppOptions.Instance.LastOpenPath = System.IO.Path.GetDirectoryName(fileName);
            AppOptions.Instance.Apply(false);
            miPreferences.Enabled = true;
        }

        public void setPreferencesMenuStatus(bool enable)
        {
            miPreferences.Enabled = enable;
        }
        
        public void miSave_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject != null)
            {
                if(EditorController.Instance.ProjectSaved)
                {
                    return;
                }

                if(inPreviewMode)
                {
                    if(DialogResult.Yes == MessageBox.Show("You can't save in preview mode. \n\n Do you want to exit preview mode end save project now?", "Saving...", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1))
                    {
                        tbbPreview_Click(this, EventArgs.Empty);
                    }else{
                        return;
                    }
                }


                
                if(EditorController.Instance.ProjectFileName == "")
                {
                    miSaveAs_Click(this, EventArgs.Empty); 
                }
                else
                {
                    if(!ValidateSave())
                    {
                        return;
                    }

                    EditorController.Instance.Save(EditorController.Instance.ProjectFileName, false);
                    MessageBox.Show("Save successful!", "Save");
                }
            }
        }


        private void miZoom50_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            EditorController.Instance.EditorProject.ReportPage.ZoomLevel = 50;
            Refresh();
        }

        private void miZoom100_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            EditorController.Instance.EditorProject.ReportPage.ZoomLevel = 100;
            Refresh();
        }

        private void miZoom150_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            EditorController.Instance.EditorProject.ReportPage.ZoomLevel = 150;
            Refresh();
        }

        private void miZoom200_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            EditorController.Instance.EditorProject.ReportPage.ZoomLevel = 200;
            Refresh();
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            EditorController.Instance.EditorProject.ReportPage.ZoomLevel = 100;
            Refresh();
        }

        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            OptionsForm vopForm = new OptionsForm();
            vopForm.Owner = this;
            vopForm.Show();
        }

        private void miShowGrid_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
			AppOptions.Instance.ShowGrid = !AppOptions.Instance.ShowGrid;

			if (EditorController.Instance.EditorViewer != null)
			{
				EditorController.Instance.EditorViewer.RefreshView();
			}			
        }


        private void MainForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            AppOptions.Instance.Apply(true);
        }


#region Align tools and still in progress




        /// <summary>
        /// Align object at top side
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolAlignTop_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            PointF alignPoint = new PointF(0F, 0F);
            EditorItem tmpFirst = null;
            bool first = true;
            foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
            {
                if(first)
                {
                    tmpFirst = tmpItem;
                    alignPoint.X = tmpItem.LocationInPixelsX;
                    alignPoint.Y = tmpItem.LocationInPixelsY;
                    first = false;
                }
                else
                {
                    if(ValidateSelectionForMoving(tmpFirst, tmpItem))
                    {
                        float oldVal = tmpItem.LocationInUnitsY;
                        if(!tmpItem.Parent.IsSelected)
                        {
                            tmpItem.LocationInUnitsY = UnitsManager.Instance.ConvertUnit(alignPoint.Y, MeasureUnits.pixel, tmpItem.MeasureUnit);
                        }else{
                            tmpItem.LocationInUnitsY = 0;// UnitsManager.Instance.ConvertUnit(alignPoint.Y, MeasureUnits.pixel, tmpItem.MeasureUnit);
                        }
                        ActionManager.Instance.EditorItemUpdate(tmpItem, "LocationInUnitsY", oldVal, tmpItem.LocationInUnitsY);
                    }
                }
            }
            EditorController.Instance.EditorViewer.RefreshView();
            EditorController.Instance.ProjectSaved = false;
        }


        /// <summary>
        /// Align object to horizontal-center
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolAlignCentarH_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            PointF alignPoint = new PointF(0F, 0F);
            PointF alignSize = new PointF(0F, 0F);
            EditorItem tmpFirst = null;
            bool first = true;
            foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
            {
                if(first)
                {
                    tmpFirst = tmpItem;
                    alignPoint.Y = tmpItem.LocationInPixelsY;
                    alignSize.Y = tmpItem.HeightInPixels;
                    first = false;
                }
                else
                {
                    if(ValidateSelectionForMoving(tmpFirst, tmpItem))
                    {
                        float oldVal = tmpItem.LocationInUnitsY;
                        if(!tmpItem.Parent.IsSelected)
                        {
                            tmpItem.LocationInUnitsY = UnitsManager.Instance.ConvertUnit(alignPoint.Y + (alignSize.Y / 2F) - (tmpItem.HeightInPixels / 2F), MeasureUnits.pixel, tmpItem.MeasureUnit);
                        }else{
                            tmpItem.LocationInUnitsY = UnitsManager.Instance.ConvertUnit((tmpItem.Parent.HeightInPixels / 2F) - (tmpItem.HeightInPixels / 2F), MeasureUnits.pixel, tmpItem.MeasureUnit);
                        }
                        ActionManager.Instance.EditorItemUpdate(tmpItem, "LocationInUnitsY", oldVal, tmpItem.LocationInUnitsY);
                    }
                }
            }
            EditorController.Instance.EditorViewer.RefreshView();
            EditorController.Instance.ProjectSaved = false;
        }


        /// <summary>
        /// Align objects to bottom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolAlignBottom_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            PointF alignPoint = new PointF(0F, 0F);
            PointF alignSize = new PointF(0F, 0F);
            EditorItem tmpFirst = null;
            bool first = true;
            foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
            {
                if(first)
                {
                    tmpFirst = tmpItem;
                    alignPoint.Y = tmpItem.LocationInPixelsY;
                    alignSize.Y = tmpItem.HeightInPixels;
                    first = false;
                }
                else
                {
                    if(ValidateSelectionForMoving(tmpFirst, tmpItem))
                    {
                        float oldVal = tmpItem.LocationInUnitsY;
                        if(!tmpItem.Parent.IsSelected)
                        {
                            tmpItem.LocationInUnitsY = UnitsManager.Instance.ConvertUnit(alignPoint.Y + alignSize.Y - (tmpItem.HeightInPixels), MeasureUnits.pixel, tmpItem.MeasureUnit);
                        }else{
                            tmpItem.LocationInUnitsY = UnitsManager.Instance.ConvertUnit(tmpItem.Parent.HeightInPixels - tmpItem.HeightInPixels, MeasureUnits.pixel, tmpItem.MeasureUnit);
                        }
                        ActionManager.Instance.EditorItemUpdate(tmpItem, "LocationInUnitsY", oldVal, tmpItem.LocationInUnitsY);
                    }
                }
            }
            EditorController.Instance.EditorViewer.RefreshView();
            EditorController.Instance.ProjectSaved = false;
        }



        private bool ValidateSelectionForMoving(EditorItem firstItem, EditorItem item)
        {
            bool result = true;
            EditorItem checkItem = item;
            while(true)
            {
                if(checkItem.Parent == null)
                {
                    result = false;
                    break;
                }else if(checkItem.Parent != firstItem.Parent)
                {
                    if(checkItem.Parent.IsSelected)
                    {
                        if(checkItem.Parent != firstItem)
                        {
                            checkItem = checkItem.Parent;
                        }else{
                            result = true;
                            break;
                        }
                    }
                    else
                    {
                        result = false;
                        break;
                    }
                }else{
                    result = true;
                    break;
                }
            }
            return result;
        }




        /// <summary>
        /// Align objects to left side
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolLeftAlign_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            PointF alignPoint = new PointF(0F, 0F);
            bool first = true;
            EditorItem tmpFirst = null;
            foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
            {
                if(first)
                {
                    tmpFirst = tmpItem;
                    alignPoint.X = tmpItem.LocationInPixelsX;
                    first = false;
                }
                else
                {
                    if(ValidateSelectionForMoving(tmpFirst, tmpItem))
                    {
                        float oldVal = tmpItem.LocationInUnitsX;
                        if(!tmpItem.Parent.IsSelected)
                        {
                            tmpItem.LocationInUnitsX = UnitsManager.Instance.ConvertUnit(alignPoint.X, MeasureUnits.pixel, tmpItem.MeasureUnit);
                        }else{
                            tmpItem.LocationInUnitsX = 0;
                        }
                        ActionManager.Instance.EditorItemUpdate(tmpItem, "LocationInUnitsX", oldVal, tmpItem.LocationInUnitsX);
                    }
                }
            }
            EditorController.Instance.EditorViewer.RefreshView();
            EditorController.Instance.ProjectSaved = false;
        }


        /// <summary>
        /// Align objects at vertical center
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolAlignCentarV_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            PointF alignPoint = new PointF(0F, 0F);
            PointF alignSize = new PointF(0F, 0F);
            EditorItem tmpFirst = null;

            bool first = true;
            foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
            {
                if(first)
                {
                    tmpFirst = tmpItem;
                    alignPoint.X = tmpItem.LocationInPixelsX;
                    alignSize.X = tmpItem.WidthInPixels;
                    first = false;
                }
                else
                {
                    if(ValidateSelectionForMoving(tmpFirst, tmpItem))
                    {
                        float oldVal = tmpItem.LocationInUnitsX;
                        if(!tmpItem.Parent.IsSelected)
                        {
                            tmpItem.LocationInUnitsX = UnitsManager.Instance.ConvertUnit(alignPoint.X + (alignSize.X / 2) - (tmpItem.WidthInPixels / 2F), MeasureUnits.pixel, tmpItem.MeasureUnit);
                        }else{
                            tmpItem.LocationInUnitsX = UnitsManager.Instance.ConvertUnit((tmpItem.Parent.WidthInPixels / 2F) - (tmpItem.WidthInPixels / 2F), MeasureUnits.pixel, tmpItem.MeasureUnit);
                        }
                        ActionManager.Instance.EditorItemUpdate(tmpItem, "LocationInUnitsX", oldVal, tmpItem.LocationInUnitsX);
                    }
                }
            }
            EditorController.Instance.EditorViewer.RefreshView();
            EditorController.Instance.ProjectSaved = false;
        }

        /// <summary>
        /// Align objects to right
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolAlignRight_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            PointF alignPoint = new PointF(0F, 0F);
            PointF alignSize = new PointF(0F, 0F);
            EditorItem tmpFirst = null;

            bool first = true;
            foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
            {
                if(first)
                {
                    tmpFirst = tmpItem;
                    alignPoint.X = tmpItem.LocationInPixelsX;
                    alignSize.X = tmpItem.WidthInPixels;
                    first = false;
                }
                else
                {
                    if(ValidateSelectionForMoving(tmpFirst, tmpItem))
                    {
                        float oldVal = tmpItem.LocationInUnitsX;
                        if(!tmpItem.Parent.IsSelected)
                        {
                            tmpItem.LocationInUnitsX = UnitsManager.Instance.ConvertUnit(alignPoint.X + alignSize.X - tmpItem.WidthInPixels, MeasureUnits.pixel, tmpItem.MeasureUnit);
                        }else{
                            tmpItem.LocationInUnitsX = UnitsManager.Instance.ConvertUnit(tmpItem.Parent.WidthInPixels - tmpItem.WidthInPixels, MeasureUnits.pixel, tmpItem.MeasureUnit);
                        }
                        ActionManager.Instance.EditorItemUpdate(tmpItem, "LocationInUnitsX", oldVal, tmpItem.LocationInUnitsX);
                    }
                }
            }
            EditorController.Instance.EditorViewer.RefreshView();
            EditorController.Instance.ProjectSaved = false;
        }


        /// <summary>
        /// Make object same width
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSameWidth_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            PointF alignSize = new PointF(0F, 0F);

            bool first = true;
            float oldVal;
            foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
            {
                if(first)
                {
                    alignSize.X = tmpItem.WidthInPixels;
                    first = false;
                }
                else
                {
                    oldVal = tmpItem.WidthInPixels;
					tmpItem.WidthInPixels = alignSize.X; // UnitsManager.Instance.ConvertUnit(alignSize.X, MeasureUnits.pixel, tmpItem.MeasureUnit);
                    ActionManager.Instance.EditorItemUpdate(tmpItem, "WidthInPixels", oldVal, tmpItem.WidthInPixels);
                }
            }
            EditorController.Instance.EditorViewer.RefreshView();
            EditorController.Instance.ProjectSaved = false;
        }


        /// <summary>
        /// Make objects same height
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSameHeight_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            PointF alignSize = new PointF(0F, 0F);

            bool first = true;
            float oldVal;
            foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
            {
                if(first)
                {
                    alignSize.Y = tmpItem.HeightInPixels;
                    first = false;
                }
                else
                {
                    oldVal = tmpItem.HeightInUnits;
                    tmpItem.HeightInUnits = UnitsManager.Instance.ConvertUnit(alignSize.Y, MeasureUnits.pixel, tmpItem.MeasureUnit);
                    ActionManager.Instance.EditorItemUpdate(tmpItem, "HeightInUnits", oldVal, tmpItem.HeightInUnits);
                }
            }
            EditorController.Instance.EditorViewer.RefreshView();
            EditorController.Instance.ProjectSaved = false;
        }


        /// <summary>
        /// Make objects same size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSameSize_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            PointF alignPoint = new PointF(0F, 0F);
            PointF alignSize = new PointF(0F, 0F);

            bool first = true;
            float oldVal1, oldVal2;
            foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
            {
                if(first)
                {
                    alignPoint.X = tmpItem.LocationInPixelsX;
                    alignPoint.Y = tmpItem.LocationInPixelsY;
                    alignSize.X = tmpItem.WidthInPixels;
                    alignSize.Y = tmpItem.HeightInPixels;
                    first = false;
                }
                else
                {
                    oldVal1 = tmpItem.WidthInPixels;
                    tmpItem.WidthInPixels = alignSize.X;
                    oldVal2 = tmpItem.HeightInPixels;
                    tmpItem.HeightInPixels = alignSize.Y;
                    ActionManager.Instance.EditorItemUpdate(tmpItem, new string[] { "WidthInPixels", "HeightInPixels" }, 
                                                                    new object[] { oldVal1, oldVal2 }, 
                                                                    new object[] { tmpItem.WidthInPixels, tmpItem.HeightInPixels });
                }
            }
            EditorController.Instance.EditorViewer.RefreshView();
            EditorController.Instance.ProjectSaved = false;
        }




        private EditorItem LastLeft = null;
        private EditorItem NextRight = null;
        private EditorItem NextLeft = null;
        private EditorItem LastRight = null;
        private EditorItem LastTop = null;
        private EditorItem NextTop = null;
        private EditorItem NextBottom = null;
        private EditorItem LastBottom = null;
        private EditorItem FirstItem = null;


        /// <summary>
        /// Find all horizontal items needed for adjusting spacing
        /// </summary>
        private void FindHItems()
        {
            FirstItem = null;
            LastLeft = null;
            LastRight = null;

            // Find objects that are far left and far right...
            foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
            {
                if(FirstItem == null)
                {
                    FirstItem = tmpItem;
                }

                if(LastLeft == null)
                {
                    LastLeft = tmpItem;
                }
                else
                {
                    if(tmpItem.LocationInPixelsX < LastLeft.LocationInPixelsX)
                    {
                        LastLeft = tmpItem;
                    }
                }

                if(LastRight == null)
                {
                    LastRight = tmpItem;
                }
                else
                {
                    if(tmpItem.LocationInPixelsX > LastRight.LocationInPixelsX)
                    {
                        LastRight = tmpItem;
                    }
                }
            }
        }


        /// <summary>
        /// Find all vertical items needed for adjusting spacing
        /// </summary>
        private void FindVItems()
        {
            FirstItem = null;
            LastTop = null;
            LastBottom = null;

            // Find objects that are far left and far right...
            foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
            {
                if(FirstItem == null)
                {
                    FirstItem = tmpItem;
                }

                if(LastTop == null)
                {
                    LastTop = tmpItem;
                }
                else
                {
                    if(tmpItem.LocationInPixelsY < LastTop.LocationInPixelsY)
                    {
                        LastTop = tmpItem;
                    }
                }

                if(LastBottom == null)
                {
                    LastBottom = tmpItem;
                }
                else
                {
                    if (tmpItem.LocationInPixelsY > LastBottom.LocationInPixelsY)
                    {
                        LastBottom = tmpItem;
                    }
                }
            }
        }


        /// <summary>
        /// Sets horizontal spacing for selected objects
        /// </summary>
        /// <param name="spacing"></param>
        /// <param name="minSpacing"></param>
        private void SetHSpacing(float spacing, float minSpacing, bool equalSpacing)
        {
            EditorItem currentItem = FirstItem;
            float numOfItem = 1F;
            NextRight = null;
            NextLeft = null;
            bool AllDone = false;
            float oldVal;
            float eqSpacing = spacing;

            while(!AllDone)
            {
                foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
                {
                    if(tmpItem == FirstItem || tmpItem.Parent != FirstItem.Parent)
                        continue;
                    if(NextRight == null)
                    {
                        if(tmpItem.LocationInPixelsX > currentItem.LocationInPixelsX)
                        {
                            NextRight = tmpItem;
                        }
                    }
                    else
                    {
                        if(tmpItem.LocationInPixelsX > currentItem.LocationInPixelsX && tmpItem.LocationInPixelsX < NextRight.LocationInPixelsX)
                        {
                            NextRight = tmpItem;
                        }
                    }
                }

                if(NextRight != null)
                {
                    float CurrentSpacing = NextRight.LocationInPixelsX - (currentItem.LocationInPixelsX + currentItem.WidthInPixels);
                    if(equalSpacing)
                    {
                        CurrentSpacing = eqSpacing;
                    }
                    else
                    {
                        CurrentSpacing += spacing * numOfItem;
                        if(CurrentSpacing < minSpacing)
                        {
                            CurrentSpacing = minSpacing;
                        }
                    }
                    oldVal = NextRight.LocationInUnitsX;
                    NextRight.LocationInUnitsX = UnitsManager.Instance.ConvertUnit(currentItem.LocationInPixelsX + currentItem.WidthInPixels + CurrentSpacing, MeasureUnits.pixel, NextRight.MeasureUnit);
                    ActionManager.Instance.EditorItemUpdate(NextRight, "LocationInUnitsX", oldVal, NextRight.LocationInUnitsX);
                }

                if(NextRight != LastRight && NextRight != null)
                {
                    currentItem = NextRight;
                    NextRight = null;
                    numOfItem += 1F;
                }
                else
                {
                    AllDone = true;
                }
            }

            currentItem = FirstItem;
            numOfItem = 1F;
            AllDone = false;
            while(!AllDone)
            {
                foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
                {
                     if(tmpItem == FirstItem || tmpItem.Parent != FirstItem.Parent)
                        continue;
                    if(NextLeft == null)
                    {
                        if(tmpItem.LocationInPixelsX < currentItem.LocationInPixelsX)
                        {
                            NextLeft = tmpItem;
                        }
                    }
                    else
                    {
                        if(tmpItem.LocationInPixelsX > currentItem.LocationInPixelsX && tmpItem.LocationInPixelsX < NextLeft.LocationInPixelsX)
                        {
                            NextLeft = tmpItem;
                        }
                    }
                }

                if(NextLeft != null)
                {
                    float CurrentSpacing = currentItem.LocationInPixelsX - (NextLeft.LocationInPixelsX + NextLeft.WidthInPixels);
                    if(equalSpacing)
                    {
                        CurrentSpacing = eqSpacing;
                    }
                    else
                    {
                        CurrentSpacing += spacing * numOfItem;
                        if(CurrentSpacing < minSpacing)
                        {
                            CurrentSpacing = minSpacing;
                        }
                    }
                    oldVal = NextLeft.LocationInUnitsX;
                    NextLeft.LocationInUnitsX = UnitsManager.Instance.ConvertUnit(currentItem.LocationInPixelsX - (NextLeft.WidthInPixels + CurrentSpacing), MeasureUnits.pixel, NextLeft.MeasureUnit);
                    ActionManager.Instance.EditorItemUpdate(NextLeft, "LocationInUnitsX", oldVal, NextLeft.LocationInUnitsX);
                }

                if(NextLeft != LastLeft && NextLeft != null)
                {
                    currentItem = NextLeft;
                    NextLeft = null;
                    numOfItem += 1F;
                }
                else
                {
                    AllDone = true;
                }
            }
        }


        /// <summary>
        /// Sets vertical spacing for selected objects
        /// </summary>
        /// <param name="spacing"></param>
        /// <param name="minSpacing"></param>
        /// <param name="equalSpacing"></param>
        private void SetVSpacing(float spacing, float minSpacing, bool equalSpacing)
        {
            EditorItem currentItem = FirstItem;
            float numOfItem = 1F;
            NextTop = null;
            NextBottom = null;
            bool AllDone = false;
            float oldVal;
            float eqSpacing = spacing;

            while(!AllDone)
            {
                foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
                {
                    if(tmpItem.Parent != FirstItem.Parent)
                        continue;
                    if(NextBottom == null)
                    {
                        if(tmpItem.LocationInPixelsY > currentItem.LocationInPixelsY)
                        {
                            NextBottom = tmpItem;
                        }
                    }
                    else
                    {
                        if(tmpItem.LocationInPixelsY > currentItem.LocationInPixelsY && tmpItem.LocationInPixelsY < NextBottom.LocationInPixelsY)
                        {
                            NextBottom = tmpItem;
                        }
                    }
                }

                if(NextBottom != null)
                {
                    float CurrentSpacing = NextBottom.LocationInPixelsY - (currentItem.LocationInPixelsY + currentItem.HeightInPixels);
                    if(equalSpacing)
                    {
                        CurrentSpacing = eqSpacing;
                    }
                    else
                    {
                        CurrentSpacing += spacing * numOfItem;
                        if(CurrentSpacing < minSpacing)
                        {
                            CurrentSpacing = minSpacing;
                        }
                    }
                    oldVal = NextBottom.LocationInUnitsY;
                    NextBottom.LocationInUnitsY = UnitsManager.Instance.ConvertUnit(currentItem.LocationInPixelsY + currentItem.HeightInPixels + CurrentSpacing, MeasureUnits.pixel, NextBottom.MeasureUnit);
                    ActionManager.Instance.EditorItemUpdate(NextBottom, "LocationInUnitsY", oldVal, NextBottom.LocationInUnitsY);
                }

                if(NextBottom != LastBottom && NextBottom != null)
                {
                    currentItem = NextBottom;
                    NextBottom = null;
                    numOfItem += 1F;
                }
                else
                {
                    AllDone = true;
                }
            }

            currentItem = FirstItem;
            numOfItem = 1F;
            AllDone = false;
            while(!AllDone)
            {
                foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
                {
                    if(tmpItem.Parent != FirstItem.Parent)
                        continue;
                    if(NextTop == null)
                    {
                        if(tmpItem.LocationInPixelsY < currentItem.LocationInPixelsY)
                        {
                            NextTop = tmpItem;
                        }
                    }
                    else
                    {
                        if(tmpItem.LocationInPixelsY > currentItem.LocationInPixelsY && tmpItem.LocationInPixelsY < NextTop.LocationInPixelsY)
                        {
                            NextTop = tmpItem;
                        }
                    }
                }

                if(NextTop != null)
                {
                    float CurrentSpacing = currentItem.LocationInPixelsY - (NextTop.LocationInPixelsY + NextTop.HeightInPixels);
                    if(equalSpacing)
                    {
                        CurrentSpacing = spacing;
                    }
                    else
                    {
                        CurrentSpacing += spacing * numOfItem;
                        if(CurrentSpacing < minSpacing)
                        {
                            CurrentSpacing = minSpacing;
                        }
                    }
                    oldVal = NextTop.LocationInUnitsY;
                    NextTop.LocationInUnitsY = UnitsManager.Instance.ConvertUnit(currentItem.LocationInPixelsY - (NextTop.HeightInPixels + CurrentSpacing), MeasureUnits.pixel, NextTop.MeasureUnit);
                    ActionManager.Instance.EditorItemUpdate(NextTop, "LocationInUnitsY", oldVal, NextTop.LocationInUnitsY);
                }

                if(NextTop != LastTop && NextTop != null)
                {
                    currentItem = NextTop;
                    NextTop = null;
                    numOfItem += 1F;
                }
                else
                {
                    AllDone = true;
                }
            }
        }
       

        /// <summary>
        /// Make objects to have same horizontal spacing between them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSameHSpacing_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            FindHItems();
            SetHSpacing(25F, 1F, true);

            EditorController.Instance.EditorViewer.RefreshView();
            EditorController.Instance.ProjectSaved = false;
        }


        /// <summary>
        /// Make objects to have no horizontal spacing between them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolNoHSpacing_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            FindHItems();
            SetHSpacing(0F, 1F, true);

            EditorController.Instance.EditorViewer.RefreshView();
            EditorController.Instance.ProjectSaved = false;
        }



       

        /// <summary>
        /// Make objects horizontal spacing increase
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolncHSpacing_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            FindHItems();
            SetHSpacing(5F, 1F, false);
            
            EditorController.Instance.EditorViewer.RefreshView();
            EditorController.Instance.ProjectSaved = false;
        }


        /// <summary>
        /// Make objects horizontal spacing decrease
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolDecHSpacing_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            FindHItems();
            SetHSpacing(-5F, 1F, false);

            EditorController.Instance.EditorViewer.RefreshView();
            EditorController.Instance.ProjectSaved = false;
        }


        /// <summary>
        /// Make object to have same vertical spacing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSameVSpacing_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            FindVItems();
            SetVSpacing(25F, 1F, true);

            EditorController.Instance.EditorViewer.RefreshView();
            EditorController.Instance.ProjectSaved = false;
        }


        /// <summary>
        /// Make object to have no vertical spacing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolNoVSpacing_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            FindVItems();
            SetVSpacing(0F, 1F, true);

            EditorController.Instance.EditorViewer.RefreshView();
            EditorController.Instance.ProjectSaved = false;
        }


        /// <summary>
        /// Make objects vertical spacing increase
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolIncVSpacing_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            FindVItems();
            SetVSpacing(5F, 1F, false);

            EditorController.Instance.EditorViewer.RefreshView();
            EditorController.Instance.ProjectSaved = false;
        }


        /// <summary>
        /// Make objects vertical spacing decrease
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolDecVSpacing_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            FindVItems();
            SetVSpacing(-5F, 1F, false);

            EditorController.Instance.EditorViewer.RefreshView();
            EditorController.Instance.ProjectSaved = false;
        }

#endregion

        private void miDataStreams_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            if (EditorController.Instance.EditorProject == null)
            {
                MessageBox.Show("Create project first!", "Data Stream Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataStreamForm dataStreamForm = new DataStreamForm();
            dataStreamForm.ShowDialog();
        }

		private void cmbObjects_SelectedValueChanged(object sender, EventArgs e)
		{
			if (cmbObjects.SelectedItem != null)
			{
				EditorController.Instance.SelectItem(((EditorItemComboItem)cmbObjects.SelectedItem).Item);
				
			}			
		}

        public void clearAllLists()
        {
            tvItems.Nodes.Clear();
            cmbObjects.DataSource = null;
            itemsCreated.Clear();
            propertyGrid.SelectedObject = null;
        }

		private void miClose_Click(object sender, EventArgs e)
		{
            if(InPreviewMode)
            {
                return;
            }
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

			EditorController.Instance.CloseProject(true);
		}

		private void miCopy_Click(object sender, EventArgs e)
		{
            if(InPreviewMode)
            {
                return;
            }
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }
            if(EditorController.Instance.SelectedItems.Count != 0)
            {
                miPaste.Enabled = true;
                tbbPaste.Enabled = true;
            }
			EditorController.Instance.Copy();
		}

		private void miCut_Click(object sender, EventArgs e)
		{
            if(InPreviewMode)
            {
                return;
            }
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            if(EditorController.Instance.SelectedItems.Count != 0)
            {
                miPaste.Enabled = true;
                tbbPaste.Enabled = true;
            }

            EditorController.Instance.Cut();
			if (EditorController.Instance.EditorProject != null && EditorController.Instance.EditorProject.CurrentReportPage != null)
			{
				EditorController.Instance.EditorProject.CurrentReportPage.DockAll();
			}
			
			EditorController.Instance.EditorProject.FrmReport.RefreshPanels();
		}

		private void miPaste_Click(object sender, EventArgs e)
		{
            if(InPreviewMode)
            {
                return;
            }
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

			EditorController.Instance.Paste();
			if (EditorController.Instance.EditorProject != null && EditorController.Instance.EditorProject.CurrentReportPage != null)
			{
				EditorController.Instance.EditorProject.CurrentReportPage.DockAll();
			}
			EditorController.Instance.EditorProject.FrmReport.RefreshPanels();
            miPaste.Enabled = false;
            tbbPaste.Enabled = false;
		}


        private bool IsBalloonBorders = false;
        private void tbbPreview_Click(object sender, EventArgs e)
        {
            //Just in case if project doesn't exist
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            if(!inPreviewMode)
            {
                foreach(EditorItem tmpItem in EditorController.Instance.EditorProject.Children)
                {
                    tmpItem.SetDisableMode(true);
                }
                EditorController.Instance.DeselectAll();

				Loger.ReInitialize();
				Loger.LogMessage("----------- Generate Preview --------");

				EditorController.Instance.MakePreviewItemList();
                EditorController.Instance.EditorProject.FrmReport.GeneratePreview(null, null, 0);                
                EditorController.Instance.EditorProject.FrmReport.Invalidate();
                inPreviewMode = true;
                this.tbbPreviewButton.Text = "Preview is ON";
                this.tbbPreviewButton.Checked = true;
                IsBalloonBorders = AppOptions.Instance.ShowBalloonBorders;
				AppOptions.Instance.ShowBalloonBorders = false;
                miShowBalloonBorders.Enabled = false;
                tvItems.Enabled = false;
                cmbObjects.Enabled = false;
                propertyGrid.Enabled = false;
                EditorController.Instance.EditorViewer.ReCreateRulers();
            }
            else
            {
                EditorController.Instance.RemovePreviewItemList();
                foreach(EditorItem tmpItem in EditorController.Instance.EditorProject.Children)
                {
                    tmpItem.SetDisableMode(false);
                }
                EditorController.Instance.EditorProject.FrmReport.Invalidate();
                inPreviewMode = false;
                this.tbbPreviewButton.Text = "Preview is OFF";
                this.tbbPreviewButton.Checked = false;
				AppOptions.Instance.ShowBalloonBorders = IsBalloonBorders;
                miShowBalloonBorders.Enabled = true;
                tvItems.Enabled = true;
                cmbObjects.Enabled = true;
                propertyGrid.Enabled = true;
                EditorController.Instance.EditorViewer.ReCreateRulers();
            }
            miShowBalloonBorders.Checked = AppOptions.Instance.ShowBalloonBorders;
        }

		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			
		}

        private void tbbHeight2_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }
            foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
            {
                if(!(tmpItem is Balloon) || 
                    tmpItem.Parent == null || 
                    tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_LEFT ||
					tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_RIGHT ||
					tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_FILL)
                {
                    return;
                }

				float oldHeight = tmpItem.HeightInPixels;
				float oldLoc = tmpItem.LocationInUnitsY;

				if (tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_BOTTOM)
                {
                    tmpItem.LocationInUnitsY = tmpItem.Parent.HeightInUnits / 2.0F;
                }
                tmpItem.HeightInPixels = tmpItem.Parent.HeightInPixels / 2.0F - 0.0001f;
				// make undo item
				ActionManager.Instance.EditorItemUpdate(tmpItem, new string[] { "LocationInUnitsY", "HeightInPixels" },
					new object[] { oldLoc, oldHeight },
					new object[] { tmpItem.LocationInUnitsY, tmpItem.HeightInPixels });					


				tmpItem.DockAll();
                break;
            }
            EditorController.Instance.EditorViewer.RefreshView();
        }

        private void tbbHeight3_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }
            foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
            {
                if(!(tmpItem is Balloon) || 
                    tmpItem.Parent == null ||
					tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_LEFT ||
					tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_RIGHT ||
					tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_FILL)
                {
                    return;
                }
				float oldHeight = tmpItem.HeightInPixels;
				float oldLoc = tmpItem.LocationInUnitsY;


				if (tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_BOTTOM)
                {
                    tmpItem.LocationInUnitsY = 2.0F * (tmpItem.Parent.HeightInUnits / 3.0F);
                }
                tmpItem.HeightInPixels = tmpItem.Parent.HeightInPixels / 3.0F - 0.0001f;

				// make undo item
				ActionManager.Instance.EditorItemUpdate(tmpItem, new string[] { "LocationInUnitsY", "HeightInPixels" },
					new object[] { oldLoc, oldHeight },
					new object[] { tmpItem.LocationInUnitsY, tmpItem.HeightInPixels });					

				tmpItem.DockAll();
                break;
            }
            EditorController.Instance.EditorViewer.RefreshView();
        }

        private void tbbHeight4_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }
            foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
            {
                if(!(tmpItem is Balloon) || 
                    tmpItem.Parent == null ||
					tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_LEFT ||
					tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_RIGHT ||
					tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_FILL)
                {
                    return;
                }

				float oldHeight = tmpItem.HeightInPixels;
				float oldLoc = tmpItem.LocationInUnitsY;

				if (tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_BOTTOM)
                {
                    tmpItem.LocationInUnitsY = 3.0F * (tmpItem.Parent.HeightInUnits / 4.0F);
                }
                tmpItem.HeightInPixels = tmpItem.Parent.HeightInPixels / 4.0F - 0.0001f;

				// make undo item
				ActionManager.Instance.EditorItemUpdate(tmpItem, new string[] { "LocationInUnitsY", "HeightInPixels" },
					new object[] { oldLoc, oldHeight },
					new object[] { tmpItem.LocationInUnitsY, tmpItem.HeightInPixels });					


				tmpItem.DockAll();
                break;
            }
            EditorController.Instance.EditorViewer.RefreshView();
        }

        private void tbbWidth2_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }
            foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
            {
                if(!(tmpItem is Balloon) ||
                    tmpItem.Parent == null ||
					tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_TOP ||
					tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_BOTTOM ||
					tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_FILL)
                {
                    return;
                }

				float oldWidth = tmpItem.WidthInPixels;
				float oldLoc = tmpItem.LocationInUnitsX;


				if (tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_RIGHT)
                {
                    tmpItem.LocationInUnitsX = tmpItem.Parent.WidthInUnits / 2.0F;
                }
                tmpItem.WidthInPixels = tmpItem.Parent.WidthInPixels / 2.0F - 0.0001f;

				// make undo item
				ActionManager.Instance.EditorItemUpdate(tmpItem, new string[] { "LocationInUnitsX", "WidthInPixels" },
					new object[] { oldLoc, oldWidth },
					new object[] { tmpItem.LocationInUnitsX, tmpItem.WidthInPixels });					

				tmpItem.DockAll();
                break;
            }
            EditorController.Instance.EditorViewer.RefreshView();
        }

        private void tbbWidth6_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }
            foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
            {
                if(!(tmpItem is Balloon) ||
                    tmpItem.Parent == null ||
					tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_TOP ||
					tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_BOTTOM ||
					tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_FILL)
                {
                    return;
                }

				float oldWidth = tmpItem.WidthInPixels;
				float oldLoc = tmpItem.LocationInUnitsX;

				if (tmpItem.DockingFromString(tmpItem.DockPositionString) == EditorItem.DockingPosition.DOCK_RIGHT)
                {
                    tmpItem.LocationInUnitsX = 2.0F * (tmpItem.Parent.WidthInUnits / 3.0F);
                }
                tmpItem.WidthInPixels = tmpItem.Parent.WidthInPixels / 3.0F - 0.0001f;

				// make undo item
				ActionManager.Instance.EditorItemUpdate(tmpItem, new string[] { "LocationInUnitsX", "WidthInPixels" },
					new object[] { oldLoc, oldWidth },
					new object[] { tmpItem.LocationInUnitsX, tmpItem.WidthInPixels });					

				tmpItem.DockAll();
                break;
            }
            EditorController.Instance.EditorViewer.RefreshView();
        }

        private void tbbWidth4_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }
            foreach(EditorItem editorItem in EditorController.Instance.SelectedItems)
            {
                if(!(editorItem is Balloon) ||
                    editorItem.Parent == null ||
					editorItem.DockingFromString(editorItem.DockPositionString) == EditorItem.DockingPosition.DOCK_TOP ||
					editorItem.DockingFromString(editorItem.DockPositionString) == EditorItem.DockingPosition.DOCK_BOTTOM ||
					editorItem.DockingFromString(editorItem.DockPositionString) == EditorItem.DockingPosition.DOCK_FILL)
                {
                    return;
                }

				float oldWidth = editorItem.WidthInPixels;
				float oldLoc = editorItem.LocationInUnitsX;

				if (editorItem.DockingFromString(editorItem.DockPositionString) == EditorItem.DockingPosition.DOCK_RIGHT)
                {
                    editorItem.LocationInUnitsX = 3.0F * (editorItem.Parent.WidthInUnits / 4.0F);
                }
				
                editorItem.WidthInPixels = editorItem.Parent.WidthInPixels / 4.0F - 0.0001f;

				// make undo item
				ActionManager.Instance.EditorItemUpdate(editorItem, new string[] { "LocationInUnitsX", "WidthInPixels" },
					new object[] { oldLoc, oldWidth },
					new object[] { editorItem.LocationInUnitsX, editorItem.WidthInPixels });					

				editorItem.DockAll();
                break;
            }
            EditorController.Instance.EditorViewer.RefreshView();
        }


		private int autoSaveTicks = 0;

        /// <summary>
        /// This works like this: If user isn't saved project, it will be saved in AutoSave.prtp, if project is saved it will be saved again in ProjectFileName
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myTimer_Tick(object sender, EventArgs e)
        {
			if (AppOptions.Instance.AutoSave && !inPreviewMode)
			{
				autoSaveTicks++;
				if (autoSaveTicks >= AppOptions.Instance.AutoSaveInterval)
				{
                    if(EditorController.Instance.EditorProject == null)
                    {
                        return;
                    }

                    //if (EditorController.Instance.ProjectFileName != "")
                    //{
                    //    EditorController.Instance.Save(EditorController.Instance.ProjectFileName, false);
                    //}
                    //else
					{
						EditorController.Instance.Save(".\\AutoSave.prtp", true);
					}
					autoSaveTicks = 0;
				}
			}
        }

        private void miGeneral_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            OptionsForm option = new OptionsForm();
			option.OpenDialog(OptionsForm.OptionsTab.General);
            miShowGrid.Checked = AppOptions.Instance.ShowGrid;
            miShowBalloonBorders.Checked = AppOptions.Instance.ShowBalloonBorders;
        }

        private void miUnitsRulers_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            OptionsForm option = new OptionsForm();
            option.OpenDialog(OptionsForm.OptionsTab.Rulers);
            miShowGrid.Checked = AppOptions.Instance.ShowGrid;
            miShowBalloonBorders.Checked = AppOptions.Instance.ShowBalloonBorders;
        }

        private void miGuidesGrids_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            OptionsForm option = new OptionsForm();
            option.OpenDialog(OptionsForm.OptionsTab.Grid);
            miShowGrid.Checked = AppOptions.Instance.ShowGrid;
            miShowBalloonBorders.Checked = AppOptions.Instance.ShowBalloonBorders;
        }

		private void miAbout_Click(object sender, EventArgs e)
		{
			formAbout frmAbout = new formAbout(AxiomCoders.PdfTemplateEditor.Properties.Resources.PdfReports);
            frmAbout.Details = "This product uses Silk icon set: http://www.famfamfam.com/lab/icons/silk";
			frmAbout.ShowDialog();
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			// save options on form close
			AppOptions.Instance.Save();
		}

		private void miShowBalloonBorders_Click(object sender, EventArgs e)
		{
			AppOptions.Instance.ShowBalloonBorders = !AppOptions.Instance.ShowBalloonBorders;
			if (EditorController.Instance.EditorViewer != null)
			{
				EditorController.Instance.EditorViewer.RefreshView();
			}			
		}

		private void miShowRulers_Click(object sender, EventArgs e)
		{
            if(InPreviewMode)
            {
                return;
            }
			if (EditorController.Instance.EditorViewer != null)
			{
				EditorController.Instance.EditorViewer.RefreshView();
			}			
		}

        private void tbbSendToBack_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            foreach (EditorItem tmpItem in EditorController.Instance.SelectedItems)
            {
                int tmpInd = tmpItem.Parent.Children.IndexOf(tmpItem);
                tmpItem.Parent.Children.Insert(0, tmpItem);
                tmpItem.Parent.Children.RemoveAt(tmpInd + 1);
                tmpItem.Parent.DockAll();
                break;
            }

            EditorController.Instance.EditorViewer.RefreshView();
        }

        private void tbbBringToFront_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
            {
                int tmpInd = tmpItem.Parent.Children.IndexOf(tmpItem);
                tmpItem.Parent.Children.Insert(tmpItem.Parent.Children.Count, tmpItem);
                tmpItem.Parent.Children.RemoveAt(tmpInd);
                tmpItem.Parent.DockAll();
                break;
            }

            EditorController.Instance.EditorViewer.RefreshView();
        }

        private void tbbNewProject_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            miNewProject_Click(null, EventArgs.Empty);
        }

        private void tbbOpenProject_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            miOpenProject_Click(null, EventArgs.Empty);
        }

        private void tbbSaveProject_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            miSave_Click(null, EventArgs.Empty);
        }

        private void tbbCut_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            miCut_Click(null, EventArgs.Empty);
        }

        private void tbbCopy_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            miCopy_Click(null, EventArgs.Empty);
        }

        private void tbbPaste_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            miPaste_Click(null, EventArgs.Empty);
        }

        private void tbbUndo_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            miUndo_Click(null, EventArgs.Empty);
        }

        private void tbbRedo_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            miRedo_Click(null, EventArgs.Empty);
        }

		private void tvItems_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node != null)
			{
				EditorController.Instance.SelectItem((EditorItem)e.Node.Tag);
			}
		}

		private void tvItems_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				if (tvItems.SelectedNode != null)
				{
					EditorController.Instance.DeleteSelected();
					EditorController.Instance.EditorViewer.RefreshView();
				}
			}
		}


        private void checkItemForCreation(object sender, Type type)
        {
            if(InPreviewMode)
            {
                return;
            }
            ToolStripButton tbbButton = sender as ToolStripButton;
            tbbButton.Checked = !tbbButton.Checked;

            if(tbbButton.Checked)
            {
                if(EditorController.Instance.TbbCheckedForCreate != null)
                {
                    EditorController.Instance.TbbCheckedForCreate.Checked = false;
                }

                if(EditorController.Instance.EditorItemSelectedForCreation != null)
                {
                    EditorController.Instance.EditorItemSelectedForCreation = null;
                }


                EditorController.Instance.TbbCheckedForCreate = tbbButton;
                EditorController.Instance.EditorItemSelectedForCreation = type;
            }
            else
            {
                EditorController.Instance.EditorItemSelectedForCreation = null;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            checkItemForCreation(sender, typeof(StaticBalloon));
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            checkItemForCreation(sender, typeof(DynamicBalloon));
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            checkItemForCreation(sender, typeof(StaticText));
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            checkItemForCreation(sender, typeof(DynamicText));
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            checkItemForCreation(sender, typeof(ImageItem));
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            checkItemForCreation(sender, typeof(DynamicImage));
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            checkItemForCreation(sender, typeof(Counter));
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
           checkItemForCreation(sender, typeof(AxiomCoders.PdfTemplateEditor.EditorItems.DateTime));    
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            checkItemForCreation(sender, typeof(PageNumberItem));
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            checkItemForCreation(sender, typeof(RectangleShape));
        }

        private void pdfReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductHelp.OpenHelpIndex(this);
        }

        private void systemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripManager.RenderMode = ToolStripManagerRenderMode.System;
            systemToolStripMenuItem.Checked = true;
            professionalToolStripMenuItem.Checked = false;
            AppOptions.Instance.ThemeProfessional = false;
            AppOptions.Instance.Save();
        }

        private void professionalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripManager.RenderMode = ToolStripManagerRenderMode.Professional;
            systemToolStripMenuItem.Checked = false;
            professionalToolStripMenuItem.Checked = true;
            AppOptions.Instance.ThemeProfessional = true;
            AppOptions.Instance.Save();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            if(InPreviewMode)
            {
                return;
            }
            checkItemForCreation(sender, typeof(PrecalculatedItem));
        }

        private void editProjectInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EditorController.Instance.EditorProject == null)
            {
                return;
            }
            projectInformation PJ = new projectInformation();
            PJ.ShowInformationDialog();
        }

        protected override void OnHelpRequested(HelpEventArgs hevent)
        {
            
        }

        private void pdfReportsHelpContentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(EditorController.Instance.SelectedItems.Count == 0)
            {
                //ProductHelp.OpenHelpTopic(this, "Introduction.htm");
                ProductHelp.OpenHelpContents(this);
            }else{
                if(EditorController.Instance.SelectedItems[0] is StaticBalloon)
                {
                    ProductHelp.OpenHelpTopic(this, "PDF%20Reports%20Template%20Editor_Objects_StaticBalloon.html");
                }
                if(EditorController.Instance.SelectedItems[0] is DynamicBalloon)
                {
                    ProductHelp.OpenHelpTopic(this, "PDF%20Reports%20Template%20Editor_Objects_DynamicBalloon.html");
                }
                if(EditorController.Instance.SelectedItems[0] is StaticText)
                {
                    ProductHelp.OpenHelpTopic(this, "PDF%20Reports%20Template%20Editor_Object_StaticText.html");
                }
                if(EditorController.Instance.SelectedItems[0] is DynamicText)
                {
                    ProductHelp.OpenHelpTopic(this, "PDF%20Reports%20Template%20Editor_Object_DynamicText.html");
                }
                if(EditorController.Instance.SelectedItems[0] is ImageItem)
                {
                    ProductHelp.OpenHelpTopic(this, "PDF%20Reports%20Template%20Editor_Object_StaticImage.html");
                }
                if(EditorController.Instance.SelectedItems[0] is DynamicImage)
                {
                    ProductHelp.OpenHelpTopic(this, "PDF%20Reports%20Template%20Editor_Objects_DynamicImage.html");
                }
                if(EditorController.Instance.SelectedItems[0] is Counter)
                {
                    ProductHelp.OpenHelpTopic(this, "PDF%20Reports%20Template%20Editor_Object_Counter.html");
                }
                if(EditorController.Instance.SelectedItems[0] is PrecalculatedItem)
                {
                    ProductHelp.OpenHelpTopic(this, "PDF%20Reports%20Template%20Editor_Object_PrecalculatedItem.html");
                }
                if(EditorController.Instance.SelectedItems[0] is AxiomCoders.PdfTemplateEditor.EditorItems.DateTime)
                {
                    ProductHelp.OpenHelpTopic(this, "PDF%20Reports%20Template%20Editor_Object_DateTime.html");
                }
                if(EditorController.Instance.SelectedItems[0] is PageNumberItem)
                {
                    ProductHelp.OpenHelpTopic(this, "PDF%20Reports%20Template%20Editor_Object_PageNumber.html");
                }
                if(EditorController.Instance.SelectedItems[0] is RectangleShape)
                {
                    ProductHelp.OpenHelpTopic(this, "PDF%20Reports%20Template%20Editor_Object_RectangleShape.html");
                }
            }
        }

        private void axiomCodersWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.axiomcoders.com");
        }

        private void pdfReportsWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.axiomcoders.com/products/pdf-report-generator");            
        }

        private void pdfReportsSuportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.axiomcoders.com/support/pdf-report-generator-support");
        }

        private void purchaseALicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.axiomcoders.com/purchase");
        }

        private void releaseNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("PDFTemplateEditor_ReadMe.txt");
        }
    }
}