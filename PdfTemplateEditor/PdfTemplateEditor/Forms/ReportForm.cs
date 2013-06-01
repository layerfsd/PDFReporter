using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


using AxiomCoders.PdfTemplateEditor.EditorStuff;
using AxiomCoders.PdfTemplateEditor.EditorItems;
using AxiomCoders.PdfTemplateEditor.Common;
using AxiomCoders.SharedNet20.Forms;
using System.Threading;

namespace AxiomCoders.PdfTemplateEditor.Forms
{
    public enum GeneratePrivewReturn
    {
        GENERATE_BALLOONS_FAILED,
        GENERATE_BALLOONS_NOT_ENOUGH_SPACE,
        GENERATE_BALLOONS_OK
    }



	public partial class ReportForm : formBase
	{
		public ReportForm()
		{
			InitializeComponent();
			ActionManager.Instance.AfterUndoRedo += new ActionManager.UndoRedoPerformedDelegate(Instance_AfterUndoRedo);
		}

        protected override void OnHelpRequested(HelpEventArgs hevent)
        {        
        }

        private bool addToSelection = false;

		/// <summary>
		/// Refresh panels after undo redo
		/// </summary>
		void Instance_AfterUndoRedo()
		{
			RefreshPanels();
		}
		
		private EditorProject project;

		/// <summary>
		/// Project that owns this form
		/// </summary>
		public AxiomCoders.PdfTemplateEditor.EditorStuff.EditorProject Project
		{
			get { return project; }
			set { project = value; }
		}
		/// <summary>
		/// This constructor should be used
		/// </summary>
		/// <param name="project"></param>
		public ReportForm(EditorProject project): this()
		{
			this.project = project;
		}

		private void ReportForm_Load(object sender, EventArgs e)
		{
			this.MouseWheel += new MouseEventHandler(ReportForm_MouseWheel);			
		}


		/// <summary>
		/// Call this to refresh everything
		/// </summary>
		public void RefreshPanels()
		{
			pnlMain.Refresh();
			pnlTopRuler.Invalidate();
			pnlLeftRuler.Invalidate();
			pnlRulerGuide.Invalidate();
		}

		void ReportForm_MouseWheel(object sender, MouseEventArgs e)
		{
			project.ReportPage.ZoomLevel += e.Delta / 40;
			/*float tmp;
			tmp = (float)project.ReportPage.Viewer.OffsetXValue;
			tmp *= (float)project.ReportPage.ZoomLevel / 100;
			project.ReportPage.Viewer.OffsetXValue = (int)tmp;
			//ScrollBarHorizontal.Value = project.ReportPage.Viewer.OffsetXValue;

			tmp = (float)project.ReportPage.Viewer.OffsetYValue;
			tmp *= (float)project.ReportPage.ZoomLevel / 100;
			project.ReportPage.Viewer.OffsetYValue = (int)tmp;*/
			//ScrollBarVertical.Value = project.ReportPage.Viewer.OffsetYValue;

			RefreshPanels();
		}


		/// <summary>
		/// Do not pain background
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			
		}

		/// <summary>
		/// Paint main panel with report page
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void panel1_Paint(object sender, PaintEventArgs e)
		{
			if (Renderer.Instance.Graphics != null)
			{
				Renderer.Instance.Graphics.Clear(SystemColors.GrayText);
				
				EditorController.Instance.EditorViewer.ShowEditor(Renderer.Instance.Graphics, pnlMain.ClientRectangle);
				Renderer.Instance.Graphics.ResetTransform();

				if (drawingCreationRectangle)
				{
					float sx, sy;
					Renderer.Instance.Graphics.ResetTransform();
					sx = creationRectangleX > mouseMoveX ? mouseMoveX : creationRectangleX;
					sy = creationRectangleY > mouseMoveY ? mouseMoveY : creationRectangleY;

					Renderer.Instance.Graphics.DrawRectangle(Pens.Gray, sx, sy, Math.Abs(mouseMoveX - creationRectangleX), Math.Abs(mouseMoveY - creationRectangleY));
				}

				// check scrollbars
				this.ScrollBarVertical.Enabled = false; // project.ReportPage.Viewer.VerticalScrollRequired;
				this.ScrollBarHorizontal.Enabled = false; // project.ReportPage.Viewer.HorizontalScrollRequired;

				if (this.ScrollBarVertical.Maximum != project.ReportPage.Viewer.VerticalScrollerMaxValue)
				{
					this.ScrollBarVertical.Minimum = 0;
					this.ScrollBarVertical.Maximum = project.ReportPage.Viewer.VerticalScrollerMaxValue;
					this.ScrollBarVertical.SmallChange = 8;
					this.ScrollBarVertical.LargeChange = 50;
				}

				if (this.ScrollBarHorizontal.Maximum != project.ReportPage.Viewer.HorizontalScrollerMaxValue)
				{
					this.ScrollBarHorizontal.Minimum = 0;
					this.ScrollBarHorizontal.Maximum = project.ReportPage.Viewer.HorizontalScrollerMaxValue;
					this.ScrollBarHorizontal.SmallChange = 8;
					this.ScrollBarHorizontal.LargeChange = 50;
				}


				// display it 
				Renderer.Instance.Render(e.Graphics);
			}
		}

		
		private void pnlTopRuler_Paint(object sender, PaintEventArgs e)
		{
			EditorController.Instance.EditorViewer.ShowTopRuler(e.Graphics, pnlTopRuler.ClientRectangle);
		}

		private void pnlRulerGuide_Paint(object sender, PaintEventArgs e)
		{
			EditorController.Instance.EditorViewer.ShowRulerGuide(e.Graphics, pnlRulerGuide.ClientRectangle);
		}

		private void pnlLeftRuler_Paint(object sender, PaintEventArgs e)
		{
			EditorController.Instance.EditorViewer.ShowLeftRuler(e.Graphics, pnlLeftRuler.ClientRectangle);
		}



        /// <summary>
        /// Activates when user press key on the keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void ReportForm_KeyDown(object sender, KeyEventArgs e)
		{			
			if (e.KeyCode == Keys.Delete)
			{
				if (EditorController.Instance.SelectedItems.Count > 0)
				{
					EditorController.Instance.DeleteSelected();
					RefreshPanels();
				}
			}
			else if (e.KeyCode == Keys.ControlKey)
			{
				EditorController.Instance.Dockable = true;
			}
			
            if(e.KeyCode == Keys.ShiftKey)
            {
                addToSelection = true;
            }
		}


        private void ReportForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                EditorController.Instance.Dockable = false;
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                EditorController.Instance.StopMovingSelection();
            }
            if(e.KeyCode == Keys.ShiftKey)
            {
                addToSelection = false;
            }
        }

	
		private void ScrollBarVertical_Scroll(object sender, ScrollEventArgs e)
		{
			project.ReportPage.Viewer.OffsetYValue = e.NewValue;
			pnlLeftRuler.Invalidate();
			pnlMain.Invalidate();
		}

		private void ScrollBarHorizontal_Scroll(object sender, ScrollEventArgs e)
		{
			project.ReportPage.Viewer.OffsetXValue = e.NewValue;
			pnlTopRuler.Invalidate();
			pnlMain.Invalidate();
		}

		/// <summary>
		/// What is the size of form when resize began
		/// </summary>
		private int resizeBeginWidth = 0;
		private int beginOffsetX = 0;

		/// <summary>
		/// What is the size of form when resize began
		/// </summary>
		private int resizeBeginHeight = 0;
		private int beginOffsetY = 0;
		
		/// <summary>
		/// Remember form dimensions
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ReportForm_ResizeBegin(object sender, EventArgs e)
		{
			resizeBeginWidth = pnlMain.ClientSize.Width;
			resizeBeginHeight = pnlMain.ClientSize.Height;
			beginOffsetX = (int)project.ReportPage.Viewer.OffsetXValue;
			beginOffsetY = (int)project.ReportPage.Viewer.OffsetYValue;
		}

		/// <summary>
		/// On resize update offset in case they should be updated
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ReportForm_Resize(object sender, EventArgs e)
		{
			if (project.ReportPage.Viewer.OffsetXValue + pnlMain.ClientSize.Width > project.ReportPage.Viewer.DrawWidth)
			{
				int delta = pnlMain.ClientSize.Width - resizeBeginWidth;
				if (delta > 0)
				{
					project.ReportPage.Viewer.OffsetXValue = 0; // beginOffsetX - delta;
					//ScrollBarHorizontal.Value = (int)project.ReportPage.Viewer.OffsetXValue;
				}
			}

			if (project.ReportPage.Viewer.OffsetYValue + pnlMain.ClientSize.Height > project.ReportPage.Viewer.DrawHeight)
			{
				int delta = pnlMain.ClientSize.Height - resizeBeginHeight;
				if (delta > 0)
				{
					project.ReportPage.Viewer.OffsetYValue = 0; // beginOffsetY - delta;
					//ScrollBarVertical.Value = (int)project.ReportPage.Viewer.OffsetYValue;
				}
			}
			
			if (this.WindowState != FormWindowState.Minimized)
			{
				Renderer.Instance.CreateBuffer(pnlMain.ClientSize.Width, pnlMain.ClientSize.Height);
				pnlMain.Invalidate();
			}

		}

		/// <summary>
		/// These coordinates are in screen space in pixels
		/// </summary>
		private int creationRectangleX;
		private int creationRectangleY;

		/// <summary>
		/// These coordinates are local only
		/// </summary>
		private int mouseMoveX;
		private int mouseMoveY;

		private int startMovingMouseX;
		private int startMovingMouseY;

		private bool drawingCreationRectangle = false;
		private bool drawingSelectionRectangle = false;

		private bool shouldStartMoving = false;
		private bool shouldStartMovingCommand = false;
		private bool shouldScrollPaper = false;
		private bool scrollingPaper = false;

		private float offsetStartX = 0;
		private float offsetStartY = 0;

		private void pnlMain_MouseDown(object sender, MouseEventArgs e)
		{
			if (EditorController.Instance.EditorItemSelectedForCreation != null)
			{
				if (!drawingCreationRectangle)
				{					
					creationRectangleX = e.X;
					creationRectangleY = e.Y;
					drawingCreationRectangle = true;
				}				
			}
			else if (!drawingSelectionRectangle && EditorController.Instance.EditorItemSelectedForCreation == null)
            {
                // in case we are not drawing selection rect and have nothing to create select item								
                float x = 0, y = 0;
				x = mouseMoveX;
				y = mouseMoveY;

				if (EditorController.Instance.SelectCommand(x, y))
				{
					// we have selected some command on some item
					startMovingMouseX = e.X;
					startMovingMouseY = e.Y;
					shouldStartMovingCommand = true;
					drawingSelectionRectangle = false;
				}
				else
				{
					EditorController.Instance.SelectItem(x, y, addToSelection);
					if (EditorController.Instance.SelectedItems.Count > 0)
					{
						drawingSelectionRectangle = false;
						startMovingMouseX = e.X;
						startMovingMouseY = e.Y;
						shouldStartMoving = true;
					}
					else
					{
						startMovingMouseX = e.X;
						startMovingMouseY = e.Y;
						offsetStartX = EditorController.Instance.EditorProject.CurrentReportPage.Viewer.OffsetXValue;
						offsetStartY = EditorController.Instance.EditorProject.CurrentReportPage.Viewer.OffsetYValue;
						this.shouldScrollPaper = true;
					}
				}
                pnlMain.Invalidate();
            }                       
		}

        
        private void ReportForm_Click(object sender, EventArgs e)
        {
           
        }				
		/// <summary>
		/// Mouse up should do creation of editor items 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pnlMain_MouseUp(object sender, MouseEventArgs e)
		{
			// in case we have object to create
			if (EditorController.Instance.EditorItemSelectedForCreation != null)
			{			
				// we shuold create new object
				EditorItem item = EditorItemFactory.Instance.CreateItem(EditorController.Instance.EditorItemSelectedForCreation);
				if (item != null)
				{
					// make place coordinates in pixels
					int sx, sy;
					float x = 0, y = 0;

					sx = creationRectangleX > mouseMoveX ? mouseMoveX : creationRectangleX;
					sy = creationRectangleY > mouseMoveY ? mouseMoveY : creationRectangleY;
					x = sx;
					y = sy;
									
					// now place item in object hierarchy					
					EditorController.Instance.EditorProject.CurrentReportPage.AddNewItem(item, x, y, Math.Abs(mouseMoveX - creationRectangleX), Math.Abs(mouseMoveY - creationRectangleY), true);					
				}
				else
				{
					MessageBox.Show("Item does not recognized and could not be created");
				}

				// we don't need object for selection anymore
				drawingCreationRectangle = false;
				EditorController.Instance.EditorItemSelectedForCreation = null;
				pnlMain.Invalidate();
			}
			else
			{
				if (EditorController.Instance.InMovingItemsState)
				{					
					EditorController.Instance.StopMovingSelection();
                    /*if(EditorController.Instance.Dockable)
                    {
                        foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
                        {
                            tmpItem.LocationInUnitsX = UnitsManager.Instance.ConvertUnit(tmpItem.GetDockingCordinates().X, MeasureUnits.pixel, tmpItem.MeasureUnit);
                            tmpItem.LocationInUnitsY = UnitsManager.Instance.ConvertUnit(tmpItem.GetDockingCordinates().Y, MeasureUnits.pixel, tmpItem.MeasureUnit);
                            tmpItem.HeightForDocking = tmpItem.HeightInUnits;
                            tmpItem.WidthForDocking = tmpItem.WidthInUnits;
                            tmpItem.dockPosition = DockingPosition.DOCK_NONE;
                            break;
                        }
                    }*/
                    //pnlMain.Invalidate();
				}	
				else if (EditorController.Instance.IsMovingCommand)
				{
					EditorController.Instance.StopMovingCommand();
					pnlMain.Invalidate();
				}
			}
			this.scrollingPaper = false;
			this.Cursor = Cursors.Default;
            RefreshPanels();
		}
	

		/// <summary>
		/// On mouse move event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pnlMain_MouseMove(object sender, MouseEventArgs e)
		{
			mouseMoveX = e.X;
			mouseMoveY = e.Y;
            
            if (EditorController.Instance.EditorItemSelectedForCreation != null)
			{
				this.Cursor = Cursors.Cross;			
				if (drawingCreationRectangle)
				{
					pnlMain.Invalidate();
				}				
			}
			else if (EditorController.Instance.InMovingItemsState)
			{				
				// we should move items by 
                this.Cursor = Cursors.SizeAll;
				float dx = (float)(mouseMoveX - startMovingMouseX);
				float dy = (float)(mouseMoveY - startMovingMouseY);
				EditorController.Instance.MoveSelection(dx, dy);
				pnlMain.Invalidate();
			}
			else if (EditorController.Instance.IsMovingCommand)
			{
				float dx = (float)(mouseMoveX - startMovingMouseX);
				float dy = (float)(mouseMoveY - startMovingMouseY);
				EditorController.Instance.MoveCommand(dx, dy);
				pnlMain.Invalidate();
			}
			else if (scrollingPaper)
			{
				this.Cursor = Cursors.Hand;
				float dx = (float)(mouseMoveX - startMovingMouseX) / 2;
				float dy = (float)(mouseMoveY - startMovingMouseY) / 2;

				EditorController.Instance.EditorProject.CurrentReportPage.Viewer.OffsetXValue = offsetStartX + dx;
				EditorController.Instance.EditorProject.CurrentReportPage.Viewer.OffsetYValue = offsetStartY + dy;				
				this.RefreshPanels();

			}
			// in case we selected something we should start moving 
			else if (shouldStartMoving)
			{
				if (e.Button == MouseButtons.Left)
				{
					if (mouseMoveX - startMovingMouseX != 0 || mouseMoveY - startMovingMouseY != 0)
					{
						EditorController.Instance.StartMovingSelection();
						shouldStartMoving = false;
					}
				}
				else 
				{
					shouldStartMoving = false;
				}
			}
			else if (shouldStartMovingCommand)
			{
				if (e.Button == MouseButtons.Left)
				{
					if (mouseMoveX - startMovingMouseX != 0 || mouseMoveY - startMovingMouseY != 0)
					{
						EditorController.Instance.StartMovingCommand();
						shouldStartMoving = false;
					}
				}
				else 
				{
					shouldStartMovingCommand = false;
				}
			}
			else if (shouldScrollPaper)
			{
				if (e.Button == MouseButtons.Left)
				{
					shouldScrollPaper = false;
					scrollingPaper = true;
					this.Cursor = Cursors.Hand;
				}
				else
				{
					shouldScrollPaper = false;
				}

			}
			else
			{
				this.Cursor = EditorController.Instance.GetCursor(mouseMoveX, mouseMoveY);
			}
		}

        /// <summary>
        /// When user select PropertyGrid, then main MDIForm leave focus, which is used for user interaction.
        /// To set back focus we use mouse click on form content.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pnlMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.ParentForm != null)
            {
                this.ParentForm.ActiveControl = this;
            }
        }      

        /// <summary>
        /// Drop down menu that represents reseting rotation on 0 degrees
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetRotation_Click(object sender, EventArgs e)
        {
            foreach(EditorItem tmpItem in EditorController.Instance.SelectedItems)
            {
                tmpItem.RotationAngle = 0;
                break;
            }
            pnlMain.Invalidate();
        }	

        /// <summary>
        /// Add non balloon children to created balloon. It will copy all items from template to created one
        /// </summary>
        /// <param name="templateBalloon"></param>
        /// <param name="createdBalloon"></param>
        private void AddNonBalloonChildren(Balloon templateBalloon, Balloon createdBalloon)
        {
            foreach(EditorItem tmpItem in templateBalloon.Children)
            {
                if(tmpItem is Balloon)
                {
                    //We only copy NonBalloon Items!
                    continue;
                }
                else
                {
					EditorItem clone = (EditorItem)tmpItem.SimpleClone();
					clone.Parent = createdBalloon;
					clone.Disabled = true;					
					//createdBalloon.Children.Add(clone);                  
                }
            }
        }

        /// <summary>
        /// This will create new Balloon object based from Balloon information. 
        /// </summary>
        /// <param name="balloon">Original from what new should be created</param>
        /// <param name="page">Current page</param>
        /// <param name="parentBalloon">parent balloon that is generated</param>
        /// <param name="isStatic">If new balloon should be created as StaticBalloon</param>
        /// <param name="level">level of recursion, for logging purposes</param>
        /// <returns></returns>
        Balloon CreateNewBalloon(Balloon balloon, ReportPage page, Balloon parentBalloon, bool isStatic)
        {
            if (isStatic)
            {
                StaticBalloon newStaticBalloon = new StaticBalloon();
                newStaticBalloon.TemplateBalloon = balloon;
                newStaticBalloon.LocationInUnitsX = balloon.LocationInUnitsX;
                newStaticBalloon.LocationInUnitsY = balloon.LocationInUnitsY;

                newStaticBalloon.WidthInPixels = balloon.GetFitToContentWidthInPixels();
                newStaticBalloon.HeightInPixels = balloon.GetFitToContentHeightInPixels();

                newStaticBalloon.zoomLevel = balloon.zoomLevel;
                newStaticBalloon.sourceBallon = balloon;
                newStaticBalloon.Disabled = true;
                newStaticBalloon.CanGrow = balloon.DockPosition == EditorItem.DockingPosition.DOCK_BOTTOM ? false : balloon.CanGrow;
                newStaticBalloon.Borders = balloon.Borders;
                newStaticBalloon.FillCapacity = balloon.FillCapacity;
                newStaticBalloon.AvailableOnEveryPage = balloon.AvailableOnEveryPage;
                newStaticBalloon.FillingGeneratesNew = balloon.FillingGeneratesNew;
                newStaticBalloon.FitToContent = balloon.FitToContent;
                newStaticBalloon.FillColor = balloon.FillColor;
                newStaticBalloon.MeasureUnit = balloon.MeasureUnit;

                float relLocationX = balloon.LocationInPixelsX; //unitConverter.ConvertUnit(balloon.LocationInUnitsX, balloon.MeasureUnit, MeasureUnits.point);
                float relLocationY = balloon.LocationInPixelsY; //unitConverter.ConvertUnit(balloon.LocationInUnitsY, balloon.MeasureUnit, MeasureUnits.point);
                float width = balloon.GetFitToContentWidthInPixels();
                float height = balloon.GetFitToContentHeightInPixels();

                newStaticBalloon.containerRect.left = relLocationX;
                newStaticBalloon.containerRect.bottom = relLocationY + height;
                newStaticBalloon.containerRect.right = relLocationX + width;
                newStaticBalloon.containerRect.top = relLocationY;
                newStaticBalloon.positionRect.top = relLocationY;
                newStaticBalloon.positionRect.left = relLocationX;
                return newStaticBalloon;
            }
            else
            {
                DynamicBalloon newDynamicBalloon = new DynamicBalloon();
                newDynamicBalloon.FillColor = balloon.FillColor;
                newDynamicBalloon.TemplateBalloon = balloon;
                newDynamicBalloon.LocationInUnitsX = balloon.LocationInUnitsX;
                newDynamicBalloon.LocationInUnitsY = balloon.LocationInUnitsY;
                newDynamicBalloon.zoomLevel = balloon.zoomLevel;
                newDynamicBalloon.sourceBallon = balloon;
                newDynamicBalloon.Disabled = true;
                newDynamicBalloon.FitToContent = balloon.FitToContent;
                newDynamicBalloon.CanGrow = balloon.CanGrow;
                newDynamicBalloon.Borders = balloon.Borders;
                newDynamicBalloon.FillCapacity = balloon.FillCapacity;
                newDynamicBalloon.AvailableOnEveryPage = balloon.AvailableOnEveryPage;
                newDynamicBalloon.FillingGeneratesNew = balloon.FillingGeneratesNew;
                newDynamicBalloon.MeasureUnit = balloon.MeasureUnit;                

                newDynamicBalloon.WidthInPixels = balloon.GetFitToContentWidthInPixels();
                newDynamicBalloon.HeightInPixels = balloon.GetFitToContentHeightInPixels();
                newDynamicBalloon.positionRect.top = balloon.LocationInPixelsY;
                newDynamicBalloon.positionRect.left = balloon.LocationInPixelsX;
                return newDynamicBalloon;
            }
            return null;
        }

             
        /// <summary>
        /// Write balloon, make its coordinates and draw all child items like static texts, dynamic texts, etc.        /// 
        /// </summary>
        /// <param name="balloon">current ballon that should be used as template for creating new one</param>
        /// <param name="page">current page</param>
        /// <param name="parentBalloon">parent ballon in which this new one should be stored</param>
        /// <param name="isStaticBalloon">is new ballon of static type, meaning that it will be placed where put</param>
        /// <param name="level">just level of recursion parameter</param>
        /// <param name="drawChildren">should this method draw items in ballon or just allocate space</param>
        /// <param name="staticBottomDockedFlag">This flag is set if this should create static Bottom docked balloon</param>
        /// <returns>newly created balloon that should be placed on page</returns>
        Balloon WriteBalloon(Balloon balloon, ReportPage page, Balloon parentBalloon, 
            bool isStaticBalloon, bool drawChildren, bool staticBottomDockedFlag,
            int level)
        {
			string logTab = "";
			for (int i = 0; i < level; i++)
			{
				logTab += "\t";
			}

			Loger.LogMessage(logTab + "WriteBalloon Begin");

	        StaticBalloon newStaticBalloon;
            DynamicBalloon newDynamicBalloon;
            UnitsManager unitConverter;        

	        unitConverter = UnitsManager.Instance;

	        // if this balloon is static then place it exactly where it should go
	        if (isStaticBalloon)
	        {
                newStaticBalloon = (StaticBalloon)CreateNewBalloon(balloon, page, parentBalloon, isStaticBalloon);                     

		        // we don't check if static balloons fall inside parents as they should always be included. Just allocate space for it in parent
                if(parentBalloon != null)
                {
                    parentBalloon.AddChild(newStaticBalloon, true, false);
                    if (drawChildren)
                    {
                        AddNonBalloonChildren(balloon, newStaticBalloon);
                    }
                }
                else
                { 
                    page.AddChild(newStaticBalloon, true, false);
                    if (drawChildren)
                    {
                        AddNonBalloonChildren(balloon, newStaticBalloon);
                    }
                }

				Loger.LogMessage(logTab + "WriteBalloon End - return static balloon");
                return newStaticBalloon;
	        }
	        else 
	        {
                newDynamicBalloon = (DynamicBalloon)CreateNewBalloon(balloon, page, parentBalloon, isStaticBalloon);          

		        // in case of dynamic balloons we do next thing
                if(parentBalloon != null)
                {
                    if(!parentBalloon.AddChild(newDynamicBalloon, false, staticBottomDockedFlag))
                    {
						Loger.LogMessage(logTab + "WriteBalloon End - return NULL - AddChild failed when Parent balloon != null");
                        return null;
                    }
					else
					{
                        if (drawChildren)
                        {
                            AddNonBalloonChildren(balloon, newDynamicBalloon);
                        }
                    }
                }
                else
                {
                    //newDynamicBalloon.Parent = EditorController.Instance.EditorProject.ReportPage;
                    //EditorController.Instance.EditorProject.PreviewList.Add(newDynamicBalloon);
                    if(!page.AddChild(newDynamicBalloon, false, false))
                    {
						Loger.LogMessage(logTab + "WriteBalloon End - return NULL - AddChild failed when Parent balloon == null");
                        return null;
                    }
					else
                    {
                        if (drawChildren)
                        {
                            AddNonBalloonChildren(balloon, newDynamicBalloon);
                        }
                    }
                }

				Loger.LogMessage(logTab + "WriteBalloon End - return dynamic balloon");
                return newDynamicBalloon;
	        }

	        //return null;
	        // 4.    Make balloon graphically. 
	        // 5.    For each item in balloon
	        // 6.      Process each item and request data when needed by using RequestDataCallback
	        // 7.    End
	        // do static balloon logic. Static balloons are placed where specified and have static dimensions
        }


		private int iteratorCounter = 0;


        public GeneratePrivewReturn GeneratePreview(Balloon parentBalloon, Balloon parentGeneratedBalloon, int level)
        {
			string logTab = "";
			for (int i = 0; i < level; i++ )
			{
				logTab += "\t";
			}

                        
			iteratorCounter++;

			Loger.LogMessage(logTab + "Generate Preview " + iteratorCounter.ToString());
            ReportPage sourcePage = EditorController.Instance.EditorProject.SourceReportPage;
			ReportPage currentPage = EditorController.Instance.EditorProject.CurrentReportPage;			
            List<EditorItem> balloons;
            List<Balloon> newBottomDockedPageBalloons = new List<Balloon>();
            StaticBalloon newStaticBalloon;
            DynamicBalloon newDynamicBalloon;
            GeneratePrivewReturn genRes;

	        if (parentBalloon == null)
	        {
		        balloons = sourcePage.Children;
	        } 
	        else 
	        {
		        balloons = parentBalloon.Children;
	        }
        	
			// generate non-balloon items that are on page
			if (parentBalloon == null)
			{
				foreach (EditorItem tmpItem in balloons)
				{
					if (!(tmpItem is Balloon))
					{
						EditorItem clone = (EditorItem)tmpItem.SimpleClone();
						clone.Parent = currentPage;
						clone.Disabled = true;
						//currentPage.Children.Add(clone);
					}
				}
			}


	        // 1. generate static balloons but ignore bottom docked ones as those need to go after dynamics
			Loger.LogMessage(logTab + "Generate static balloons");
	        foreach(EditorItem balloon in balloons)
	        {
                // if balloon is top docked and has prev top docked dynamics skip it
                if (balloon is StaticBalloon && balloon.DockPosition == EditorItem.DockingPosition.DOCK_TOP &&
                    (balloon as StaticBalloon).HasDynamicTopBallons)
                {
                    continue;
                }

                // if balloon is Static and docking is bottom and this balloon is placed on page we need to allocate space for it in rectMatrix
                // of page so now one can take this space later.
                if ((balloon is StaticBalloon && balloon.DockPosition == EditorItem.DockingPosition.DOCK_BOTTOM && parentBalloon == null) ||
                    (balloon is StaticBalloon && balloon.DockPosition == EditorItem.DockingPosition.DOCK_BOTTOM && parentBalloon != null && !parentBalloon.CanGrow))
                {
                    // create it as standard static balloon but do not draw children items yet just allocate space for it
                    newStaticBalloon = (StaticBalloon)WriteBalloon((Balloon)balloon, currentPage, parentGeneratedBalloon, true, false, true, level);
                    newBottomDockedPageBalloons.Add(newStaticBalloon);
                }

                // if balloon is Static and docking position is not bottom
                if (balloon is StaticBalloon && balloon.DockPosition != EditorItem.DockingPosition.DOCK_BOTTOM)
                {
                    newStaticBalloon = (StaticBalloon)WriteBalloon((Balloon)balloon, currentPage, parentGeneratedBalloon, true, true, false, level);
                   
                    // 8.    Recursive call to 1. with parent balloon set as this
                    genRes = GeneratePreview((Balloon)balloon, newStaticBalloon, level+1);
                    if(genRes == GeneratePrivewReturn.GENERATE_BALLOONS_NOT_ENOUGH_SPACE)
                    {
						Loger.LogMessage(logTab + "Not enough space");
                    }
                    else if(genRes == GeneratePrivewReturn.GENERATE_BALLOONS_FAILED)
                    {
						Loger.LogMessage(logTab + "Generate Failed");
                        return genRes;
                    }
                }                
	        } // foreach static balloon

	        // for each dynamic balloon
			Loger.LogMessage(logTab + "Generate dynamic balloons");
            GeneratePrivewReturn result = GeneratePrivewReturn.GENERATE_BALLOONS_OK;

            // 2. generate dynamic balloons
	        foreach(EditorItem tmpItem in balloons)
	        {
                if(tmpItem is Balloon)
                {
                    Balloon balloon = (Balloon)tmpItem;
                    if(balloon is DynamicBalloon)
                    {
                        int count = 0;
						int maxItems = AppOptions.Instance.NumOffPreviewItems;
						
                        while(count < maxItems)
                        {
                            count++;

                            newDynamicBalloon = (DynamicBalloon)WriteBalloon(balloon, currentPage, parentGeneratedBalloon, false, true, false, level);
                            if(newDynamicBalloon == null)
                            {
                                // we failed generating new balloon as not possible to write new one inside parent Generated Balloon. We need new parent. 
								Loger.LogMessage(logTab + "Not enough space");                                
                                result = GeneratePrivewReturn.GENERATE_BALLOONS_NOT_ENOUGH_SPACE;
                                goto ProcessBottomDocked;
                            }

                            // 18.     Recursive call to 1. with parent balloon set as this		
                            genRes = GeneratePreview(balloon, newDynamicBalloon, level+1);
                            if(genRes == GeneratePrivewReturn.GENERATE_BALLOONS_NOT_ENOUGH_SPACE)
                            {
								Loger.LogMessage(logTab + "Not enough space2");
                                // we should continue to next iteration without checking condition and reading data
                                continue;
                            }
                            else if(genRes == GeneratePrivewReturn.GENERATE_BALLOONS_FAILED)
                            {
								Loger.LogMessage(logTab + "Generate Failed");
                                return genRes;
                            }
                        }
                    }
                }
	        }

        
            Loger.LogMessage(logTab + "Generate bottom docked static balloons");

            // 3. generate bottom docked static balloons
            for (int i = balloons.Count - 1; i >= 0; i--)
            {
                EditorItem balloon = balloons[i];

                // if balloon is Static and docking position is bottom
                if (balloon is StaticBalloon && balloon.DockPosition == EditorItem.DockingPosition.DOCK_BOTTOM && parentBalloon != null &&
                    parentBalloon.CanGrow)
                {                                        
                    // write new balloon but handle it as dynamic balloon
                    newDynamicBalloon = (DynamicBalloon)WriteBalloon((Balloon)balloon, currentPage, parentGeneratedBalloon, false, true, true, level);                    

                    // if this newDynamicBalloon is not generated to be bottom one move it down
                    if (newDynamicBalloon == null)
                    {
                        // we failed generating new balloon as not possible to write new one inside parent Generated Balloon. We need new parent. 
                        Loger.LogMessage(logTab + "Not enough space");
                        result = GeneratePrivewReturn.GENERATE_BALLOONS_NOT_ENOUGH_SPACE;
                        goto ProcessBottomDocked;
                    }
                                   
                    genRes = GeneratePreview((Balloon)balloon, newDynamicBalloon, level + 1);

                    if (genRes == GeneratePrivewReturn.GENERATE_BALLOONS_NOT_ENOUGH_SPACE)
                    {
                        Loger.LogMessage(logTab + "Not enough space");
                    }
                    else if (genRes == GeneratePrivewReturn.GENERATE_BALLOONS_FAILED)
                    {
                        Loger.LogMessage(logTab + "Generate Failed");
                        return genRes;
                    }
                }
            } // foreach static balloon docked bottom

            // 4. Generate top docked static balloons         
            foreach (EditorItem tmpItem in balloons)
            {
                if (tmpItem is Balloon && tmpItem is StaticBalloon)
                {
                    StaticBalloon balloon = tmpItem as StaticBalloon;
                    if (balloon.DockPosition == EditorItem.DockingPosition.DOCK_TOP && balloon.HasDynamicTopBallons)
                    {
                        // write new balloon but handle it as dynamic balloon
                        newDynamicBalloon = (DynamicBalloon)WriteBalloon((Balloon)balloon, currentPage, parentGeneratedBalloon, false, true, true, level);

                        // if this newDynamicBalloon is not generated to be bottom one move it down
                        if (newDynamicBalloon == null)
                        {
                            // we failed generating new balloon as not possible to write new one inside parent Generated Balloon. We need new parent. 
                            Loger.LogMessage(logTab + "Not enough space");
                            result = GeneratePrivewReturn.GENERATE_BALLOONS_NOT_ENOUGH_SPACE;
                            goto ProcessBottomDocked;
                        }

                        genRes = GeneratePreview((Balloon)balloon, newDynamicBalloon, level + 1);

                        if (genRes == GeneratePrivewReturn.GENERATE_BALLOONS_NOT_ENOUGH_SPACE)
                        {
                            Loger.LogMessage(logTab + "Not enough space");
                        }
                        else if (genRes == GeneratePrivewReturn.GENERATE_BALLOONS_FAILED)
                        {
                            Loger.LogMessage(logTab + "Generate Failed");
                            return genRes;
                        }
                    }
                }
            }

            // Process Bottom Docked
            ProcessBottomDocked:

            DrawRemainingBalloons(newBottomDockedPageBalloons);

            return result;
        }

        /// <summary>
        /// This will draw list of balloons
        /// </summary>
        /// <param name="remainingBalloons"></param>
        private void DrawRemainingBalloons(List<Balloon> remainingBalloons)
        {
            // 4. generate bottom docked static balloons that are on page             
            foreach (Balloon balloon in remainingBalloons)
            {
                AddNonBalloonChildren(balloon.TemplateBalloon, balloon);
            }
        }

		private void ReportForm_Activated(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Maximized;
			this.Text = this.Project.Name;
		}

		private void ReportForm_Shown(object sender, EventArgs e)
		{
			Renderer.Instance.CreateBuffer(pnlMain.ClientSize.Width, pnlMain.ClientSize.Height);
			this.HorizontalScroll.Enabled = false;
			this.ScrollBarHorizontal.Enabled = false;
			this.ScrollBarVertical.Enabled = false;
			this.VerticalScroll.Enabled = false;
		}


		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Left)
			{
				EditorController.Instance.StartMovingSelection();
				EditorController.Instance.MoveSelection(-2F, 0F);
				EditorController.Instance.StopMovingSelection();
				EditorController.Instance.EditorViewer.RefreshView();
			}
			else if (keyData == Keys.Up)
			{
				EditorController.Instance.StartMovingSelection();
				EditorController.Instance.MoveSelection(0F, -2F);
				EditorController.Instance.StopMovingSelection();
				EditorController.Instance.EditorViewer.RefreshView();
			}
			else if (keyData == Keys.Down)
			{
				EditorController.Instance.StartMovingSelection();
				EditorController.Instance.MoveSelection(0F, 2F);
				EditorController.Instance.StopMovingSelection();
				EditorController.Instance.EditorViewer.RefreshView();
			}
			else if (keyData == Keys.Right)
			{
				EditorController.Instance.StartMovingSelection();
				EditorController.Instance.MoveSelection(2F, 0F);
				EditorController.Instance.StopMovingSelection();
				EditorController.Instance.EditorViewer.RefreshView();
			} 
			
			return base.ProcessCmdKey(ref msg, keyData);
		}



        private void ReportForm_FormClosing(object sender, EventArgs e)
        {
            if(EditorController.Instance.EditorProject == null)
            {
                return;
            }

            if(!EditorController.Instance.ProjectSaved)
            {
                if(DialogResult.Yes == MessageBox.Show("Do you want to save changes?", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    EditorController.Instance.MainFormOfTheProject.miSave_Click(this, EventArgs.Empty);
                }
            }

            EditorController.Instance.CloseProject(false);
            EditorController.Instance.MainFormOfTheProject.clearAllLists();
            EditorController.Instance.MainFormOfTheProject.setPreferencesMenuStatus(false);
        }
    }

}