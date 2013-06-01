using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

using AxiomCoders.PdfTemplateEditor.Common;
using AxiomCoders.PdfTemplateEditor.EditorItems;
using System.Xml;
using System.Drawing.Drawing2D;


namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
	  

	public delegate void ItemPlacedCallback(EditorItem item); 

	/// <summary>
	/// One item in editor that is stored in project
	/// </summary>
	[System.Reflection.Obfuscation(Exclude = true)]
	public abstract class EditorItem: SaveLoadMechanism, ICloneable
	{
		public enum DockingPosition
		{
			DOCK_TOP,
			DOCK_LEFT,
			DOCK_BOTTOM,
			DOCK_RIGHT,
			DOCK_FILL,
			DOCK_NONE
		}

        /// <summary>
        /// Anchor field
        /// </summary>
        private Anchor anchor;

        /// <summary>
        /// Anchor property
        /// </summary>
        public Anchor ItemAnchor
        {
            get { return anchor; }
        }
        
        /// <summary>
        /// Contains anchor properties
        /// </summary>
        [Browsable(true), DisplayName("Anchor"), Description("Set anchor properties"), Category("Standard"), EditorAttribute(typeof(AnchorEditor), typeof(UITypeEditor))]
        public string AnchorProps
        {
            get
            {
                if(anchor == null)
                {
                    anchor = new Anchor(this);
                }
                AnchorEditor.itemsAnchor = anchor;
                return anchor.GetAnchor();
            }
        }

        /// <summary>
        /// Called to anchor item to its parent
        /// </summary>
        /// <param name="duLocX"></param>
        /// <param name="duLocY"></param>
        /// <param name="dWidth"></param>
        /// <param name="dHeight"></param>
        public void AnchorAll(float duLocX, float duLocY, float dWidth, float dHeight)
        {
            foreach(EditorItem child in Children)
            {
                child.ItemAnchor.UpdateOwner(duLocX, duLocY, dWidth, dHeight);
            }
        }

        public void LockAnchorPositions()
        {
            foreach(EditorItem child in this.Children)
            {
                child.ItemAnchor.LockPositions();
            }
        }


		private static Dictionary<string, int> typeNaming = new Dictionary<string, int>();

		private bool notifyCreation = true;
        public const float Epsilon = 0.0001F;
        public List<RectangleNormal> rectMatrix = new List<RectangleNormal>();


		/// <summary>
		/// This event is called whenever item is placed somewhere on page. 
		/// </summary>
		public static event ItemPlacedCallback ItemPlaced;


		/// <summary>
		/// Should notify anyone about creation. Notifications are done trough EditorItemFactory
		/// </summary>
		[Browsable(false)]
		public bool NotifyCreation
		{
			get { return notifyCreation; }			
		}


		protected bool notifyDeletion = true;

		/// <summary>
		/// Should notify anyone about deletion
		/// </summary>
		[Browsable(false)]
		public bool NotifyDeletion
		{
			get { return notifyDeletion; }
			set { notifyDeletion = value; }
		}

		protected string MakeName()
		{
			// make name for editor item
			if (typeNaming.ContainsKey(this.GetType().Name))
			{
				typeNaming[this.GetType().Name]++;
			}
			else
			{
				typeNaming.Add(this.GetType().Name, 0);
			}

			return this.GetType().Name + typeNaming[this.GetType().Name].ToString();			
		}
		
		public EditorItem()
		{
		    anchor = new Anchor(this);
		    this.Name = MakeName();
		}


	    /// <summary>
		/// If this item can contain other items
		/// </summary>
		protected bool isContainer;

		/// <summary>
		/// Where is this item positioned 
		/// </summary>
		protected float locationInUnitsX;

		[Browsable(true), DisplayName("Location In Units X"), Description("Location X of item on report page in current measurement units"), Category("Location")]
		public float LocationInUnitsX
		{
			get { return locationInUnitsX; }
            set { locationInUnitsX = (float)Math.Round((double)value, 4);}
		}

		protected string name;

		/// <summary>
		/// This is name of editorItem. Similar to name of objects in VS editor
		/// </summary>
		[Browsable(true), Description("Instance Name. Not used in generating process")]
		public virtual string Name
		{
			get { return name; }
			set
            { 
                if(value == "")
                {
                    MessageBox.Show("You must enter valid project name!", "Warning!", MessageBoxButtons.OK);
                    return;
                }
                if(this is ReportPage && EditorController.Instance.EditorProject != null)
                {
                    if(EditorController.Instance.EditorProject.FrmReport != null)
                    {
                        EditorController.Instance.EditorProject.FrmReport.Text = name + " - " + System.IO.Path.GetFileName(EditorController.Instance.ProjectFileName);
                    }
                }
                name = value;  
            }
		}
	
		

		/// <summary>
		/// Where is this item positioned
		/// </summary>
		protected float locationInUnitsY;
		[Browsable(true), DisplayName("Location In Units Y"), Description("Location Y of item on report page in current measurement units"), Category("Location")]
		
		public float LocationInUnitsY
		{
			get { return locationInUnitsY; }
            set { locationInUnitsY = (float)Math.Round((double)value, 4);}
		}
		
	
		/// <summary>
		/// Location X in pixels in World coordinates. Using parent location as well. Calculated
		/// </summary>		
		[Browsable(false)]
		public virtual float LocationInPixelsX
		{
			get 
			{								
				/*if (this.Parent != null)
				{
					return this.Parent.LocationInPixelsX + UnitsManager.Instance.ConvertUnit(LocationInUnitsX, MeasureUnit, MeasureUnits.pixel);	
				}
				else */
				{
					return UnitsManager.Instance.ConvertUnit(LocationInUnitsX, MeasureUnit, MeasureUnits.pixel);	
				}
				
			}
		}

		/// <summary>
		/// Location Y in pixels in World coordinates. Using parent location as well. Calculated
		/// </summary>
		[Browsable(false)]
		public virtual float LocationInPixelsY
		{
			get
			{
				/*if (this.Parent != null)
				{
					return this.Parent.LocationInPixelsY + UnitsManager.Instance.ConvertUnit(LocationInUnitsY, MeasureUnit, MeasureUnits.pixel);
				}
				else*/
				{
					return UnitsManager.Instance.ConvertUnit(LocationInUnitsY, MeasureUnit, MeasureUnits.pixel);
				}				
			}
		}	

		protected float widthInPixels;
		
		[Browsable(false)]
		public float WidthInPixels
		{
            get 
			{
				//Loger.LogMessage("Width Get: " + widthInPixels.ToString());
				return widthInPixels; 
			}
            set
            {
				WidthInUnits = UnitsManager.Instance.ConvertUnit(value, MeasureUnits.pixel, MeasureUnit);
                widthInPixels = value;
                
				//Loger.LogMessage("Width Set: " + heightInPixels.ToString());
            }
		}


		private float heightInPixels;

		[Browsable(false)]
		public float HeightInPixels
		{
            get { return heightInPixels; }
            set
            {
				HeightInUnits = UnitsManager.Instance.ConvertUnit(value, MeasureUnits.pixel, MeasureUnit);
                heightInPixels = value;
            }
		}
		

        /// <summary>
        /// Used for generating preview
        /// </summary>
        protected bool disabled = false;

		[Browsable(false)]
        public bool Disabled
        {
            get { return disabled; }
            set { disabled = value; }
        }

		/// <summary>
		/// Matrix used for transformations
		/// </summary>
		protected System.Drawing.Drawing2D.Matrix transformationMatrix = new System.Drawing.Drawing2D.Matrix();

		/// <summary>
		/// rotation angle in degrees
		/// </summary>
		protected float rotationAngle = 0;

		/// <summary>
		/// Clockwise Rotation angle of item
		/// </summary>
        [Browsable(true), DisplayName("Rotation Angle"), Description("Clockwise rotation of item in degrees"), Category("Misc")]
		public float RotationAngle
		{
			get { return rotationAngle; }
			set { rotationAngle = value; }
		}

		/// <summary>
		/// Scale of this item on X axis. This is not used yet and probably will not be used at all
		/// </summary>
		protected float scaleXFactor = 1.0f;

		/// <summary>
		/// Scale of this item on X axis. This is not used yet and probably will not be used at all
		/// </summary>
		[Browsable(false)]
		public float ScaleXFactor
		{
			get { return scaleXFactor; }
			set 
			{ 
				if (value < 0.1f)
				{
					scaleXFactor = 0.1f;
				}
				else 
				{
					scaleXFactor = value; 
				}				
			}
		}

		/// <summary>
		/// scale of this object on Y axis. This is not used yet and probably will not be used at all
		/// </summary>
		protected float scaleYFactor = 1.0f;

		/// <summary>
		/// scale of this item on Y axis. This is not used yet and probably will not be used at all
		/// </summary>
		[Browsable(false)]
		public float ScaleYFactor
		{
			get { return scaleYFactor; }
			set 
			{
				if (value < 0.1f)
				{
					scaleYFactor = 0.1f;
				}
				else
				{
					scaleYFactor = value;
				}		
			}
		}

		/// <summary>
		/// Full Transformation matrix for this item. It is calculated from location, rotation and scale
		/// </summary>
		[Browsable(false)]
		public virtual System.Drawing.Drawing2D.Matrix TransformationMatrix
		{
			get
			{				
				System.Drawing.Drawing2D.Matrix rotMatrix, scaleMatrix, transMatrix;

				rotMatrix = new System.Drawing.Drawing2D.Matrix();
				rotMatrix.Rotate(this.RotationAngle);
				scaleMatrix = new System.Drawing.Drawing2D.Matrix();
				scaleMatrix.Scale(this.scaleXFactor, this.scaleYFactor);
				transMatrix = new System.Drawing.Drawing2D.Matrix();
				transMatrix.Translate((float)this.LocationInPixelsX, (float)this.LocationInPixelsY);
			

				// first we scale than rotate and then transform
				this.transformationMatrix.Reset();
				this.transformationMatrix.Multiply(scaleMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
				this.transformationMatrix.Multiply(rotMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
				this.transformationMatrix.Multiply(transMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);				

				return this.transformationMatrix;
			}			
		}

		/// <summary>
		/// Width in current measure units. Calculated
		/// </summary>
        [Browsable(true), DisplayName("Width In Units"), Description("Width of item in current measurement unit"), Category("Size")]
		public virtual float WidthInUnits
		{
			get { return UnitsManager.Instance.ConvertUnit(widthInPixels, MeasureUnits.pixel, this.MeasureUnit); }
			set
			{
                float oldValue = widthInPixels;
				widthInPixels = UnitsManager.Instance.ConvertUnit(value, this.MeasureUnit, MeasureUnits.pixel);
                if (widthInPixels < 1)
                {
                    //MessageBox.Show("Value out of range!", "Set value...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    widthInPixels = oldValue;
                }
			}
		}

		/// <summary>
		/// What is height in current measurement unit. Calculated
		/// </summary>
        [Browsable(true), DisplayName("Height In Units"), Description("Height of item in current measurement unit"), Category("Size")]
		public virtual float HeightInUnits
		{
			get { return UnitsManager.Instance.ConvertUnit(heightInPixels, MeasureUnits.pixel, this.MeasureUnit); }
			set
			{
                float oldValue = heightInPixels;
				heightInPixels = UnitsManager.Instance.ConvertUnit(value, this.MeasureUnit, MeasureUnits.pixel);
                if(heightInPixels < 1)
                {
                    //MessageBox.Show("Value out of range!", "Set value...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    heightInPixels = oldValue;
                }
			}
		}

		
	
		/// <summary>
		/// Measure unit used for positioning properties
		/// </summary>
		protected MeasureUnits measureUnit = MeasureUnits.inch;

		/// <summary>
		/// Measure Unit used for positioning properties
		/// </summary>
        [Browsable(true), ReadOnly(true), DisplayName("Measure Unit"), Description("Measurement Unit"), Category("Standard")]
		public virtual AxiomCoders.PdfTemplateEditor.Common.MeasureUnits MeasureUnit
		{
			get { return measureUnit; }
			set 
			{
				if (measureUnit != value)
				{
					this.LocationInUnitsX = UnitsManager.Instance.ConvertUnit(this.LocationInUnitsX, measureUnit, value);
					this.LocationInUnitsY = UnitsManager.Instance.ConvertUnit(this.LocationInUnitsY, measureUnit, value);
				}
				measureUnit = value; 

                // change measure unit of all children
                foreach (EditorItem item in Children)
                {
                    item.MeasureUnit = value;
                }
			}
		}

		/// <summary>
		/// Is this item selected
		/// </summary>
		protected bool isSelected = false;

		/// <summary>
		/// Is this editor item selected
		/// </summary>
		[Browsable(false)]
		public virtual bool IsSelected
		{
			get { return isSelected; }
			set
			{
				if (!isSelected && value)
				{
					// if becomes selected then create commands
					CreateCommands();
				}
				else if (!value)
				{
					this.commands.Clear();
				}
				isSelected = value; 
			}
		}
	
		protected virtual void CreateCommands()
		{
			//CommandItem command = new MovingCommandItem(CommandPosition.MiddleCenter, this);
			//this.commands.Add(command);
			this.commands.Clear();
            this.commands = new List<CommandItem>();

			CommandItem command;			

			if (dockPosition == DockingPosition.DOCK_NONE)
			{
				command = new SizingCommandItem(CommandPosition.TopLeft, this);
				this.commands.Add(command);
			}
			if (dockPosition == DockingPosition.DOCK_NONE || dockPosition == DockingPosition.DOCK_BOTTOM)
			{
				command = new SizingCommandItem(CommandPosition.TopCenter, this);
				this.commands.Add(command);
			}
			if (dockPosition == DockingPosition.DOCK_NONE)
			{
				command = new SizingCommandItem(CommandPosition.TopRight, this);
				this.commands.Add(command);
			}
			if (dockPosition == DockingPosition.DOCK_NONE || dockPosition == DockingPosition.DOCK_RIGHT)
			{
				command = new SizingCommandItem(CommandPosition.MiddleLeft, this);
				this.commands.Add(command);
			}
			if (dockPosition == DockingPosition.DOCK_NONE || dockPosition == DockingPosition.DOCK_LEFT)
			{
				command = new SizingCommandItem(CommandPosition.MiddleRight, this);
				this.commands.Add(command);
			}
			if (dockPosition == DockingPosition.DOCK_NONE)
			{
				command = new SizingCommandItem(CommandPosition.BottomLeft, this);
				this.commands.Add(command);
			}
			if (dockPosition == DockingPosition.DOCK_NONE || dockPosition == DockingPosition.DOCK_TOP)
			{
				command = new SizingCommandItem(CommandPosition.BottomCenter, this);
				this.commands.Add(command);
			}
			if (dockPosition == DockingPosition.DOCK_NONE)
			{
				command = new SizingCommandItem(CommandPosition.BottomRight, this);
				this.commands.Add(command);
			}
		}

		/// <summary>
		/// Parent item for this item. Can be null
		/// </summary>
		protected EditorItem parent;

		/// <summary>
		/// Parent item of this item or null if no parent
		/// </summary>
		[Browsable(false)]
		public virtual AxiomCoders.PdfTemplateEditor.EditorStuff.EditorItem Parent
		{
			get { return parent; }
			set 
            {
                EditorItem oldParent = parent;
                parent = value; 

                if(oldParent != parent && oldParent != null)
                {
                    if(oldParent.Children.Contains(this))
                    {
                        oldParent.Children.Remove(this);
                    }
                }
                if(parent != null)
                {
                    if(!parent.Children.Contains(this))
                        parent.Children.Add(this);
                }
            }
		}		

		/// <summary>
		/// Children items for this item
		/// </summary>
		protected List<EditorItem> children = new List<EditorItem>();

		/// <summary>
		/// Children items for this item
		/// </summary>
		[Browsable(false)]
		public virtual List<EditorItem> Children
		{
			get { return children; }
            set { children = value; }
		}

		/// <summary>
		/// Is this item deleted
		/// </summary>
		protected bool isDeleted = false;

		/// <summary>
		/// Check if this item is deleted
		/// </summary>
		[Browsable(false)]
		public virtual bool IsDeleted
		{
			get { return isDeleted; }
			set { isDeleted = value; }
		}

		/// <summary>
		/// Indicate if this item is moved
		/// </summary>
		protected bool isMoved = false;

		/// <summary>
		/// Indicates if this item is moved
		/// </summary>
		[Browsable(false)]
		protected bool IsMoved
		{
			get { return isMoved; }
			set { isMoved = value; }
		}

		protected float startMovingX;
		protected float startMovingY;
        protected EditorItem startParent;

		public void StartMoving()
		{
			if (this.dockPosition == DockingPosition.DOCK_NONE)
			{
				this.IsMoved = true;
				startMovingX = this.LocationInUnitsX;
				startMovingY = this.LocationInUnitsY;
                startParent = this.Parent;
			}
		}

		/// <summary>
		/// This will stop moving
		/// </summary>
		public void StopMoving(bool updateLocation)
		{
			if (this.IsMoved)
			{
				// in case we were moving and now we stopped update location properties
				if (updateLocation)
				{
					System.Drawing.Drawing2D.Matrix mat = this.LastDrawMatrix.Clone();									

					PointF tmpPoint = new PointF(0, 0);
					PointF[] points = new PointF[1];
					points[0] = tmpPoint;
					mat.TransformPoints(points);

					tmpPoint = points[0];
					EditorController.Instance.EditorProject.ReportPage.AddNewItem(this, tmpPoint.X, tmpPoint.Y, (int)this.WidthInPixels, (int)this.HeightInPixels, false);

					ActionManager.Instance.EditorItemUpdate(this, new string[] { "LocationInUnitsX", "LocationInUnitsY", "Parent" }, 
						new object[] { startMovingX, startMovingY, startParent },
						new object[] { this.LocationInUnitsX, this.LocationInUnitsY, this.Parent } );					
				}

				moveDeltaX = 0;
				moveDeltaY = 0;
				isMoved = false;
			}
		}

		/// <summary>
		/// Delta move value when isMoved is true. 
		/// </summary>
		private float moveDeltaX = 0;

		/// <summary>
		/// Delta move value when isMoved is true. 
		/// </summary>		
		private float moveDeltaY = 0;

		/// <summary>
		/// This will move item by delta x, delta y. Coordinates are in pixels
		/// </summary>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		public void MoveBy(float dx, float dy)
		{
			if (IsMoved)
			{
				// convert dx, dy to vector in object space
				System.Drawing.Drawing2D.Matrix mat = this.ViewMatrix.Clone();
                if (this.Parent != null)
                {
                    mat.Multiply(this.Parent.DrawMatrix);
                }

				mat.Invert();

				PointF tmpPoint = new PointF(dx, dy);
				PointF[] points = new PointF[1];
				points[0] = tmpPoint;
              
				mat.TransformVectors(points);

				tmpPoint = points[0];

				moveDeltaX = UnitsManager.Instance.ConvertUnit(tmpPoint.X, MeasureUnits.pixel, this.MeasureUnit);
				moveDeltaY = UnitsManager.Instance.ConvertUnit(tmpPoint.Y, MeasureUnits.pixel, this.MeasureUnit);

				this.LocationInUnitsX = startMovingX + moveDeltaX;
				this.LocationInUnitsY = startMovingY + moveDeltaY;

                this.DockAll();
                if (this.Parent != null)
                {
                    this.Parent.DockAll();
                }
			}
		}

		/// <summary>
		/// This will check if possibleParent is somewhere in parent list
		/// </summary>
		/// <param name="possibleParent"></param>
		/// <param name="fullCheck"></param>
		public bool CheckIfParent(EditorItem possibleParent)
		{
			if (this.Parent == possibleParent)
			{
				return true;
			}
			else 
			{
				if (this.Parent != null)
				{
					return this.Parent.CheckIfParent(possibleParent);
				}
			}
			return false;
		}

		/// <summary>
		/// Here is actual save method. Element should be already created one
		/// </summary>
		/// <param name="node"></param>
		public virtual void SaveItem(XmlDocument doc, System.Xml.XmlElement element)
		{
			UnitsManager unitMng = new UnitsManager();

			// save name attribute
			XmlAttribute attr = doc.CreateAttribute("Name");
			attr.Value = Name;
			element.SetAttributeNode(attr);

			// save location
			XmlElement el = doc.CreateElement("Location");
			attr = doc.CreateAttribute("PositionX");
			attr.Value = LocationInUnitsX.ToString() + unitMng.UnitToString(MeasureUnit);
			el.SetAttributeNode(attr);

			attr = doc.CreateAttribute("PositionY");
			attr.Value = LocationInUnitsY.ToString() + unitMng.UnitToString(MeasureUnit);
			el.SetAttributeNode(attr);

			element.AppendChild(el);

			// save Width and Height. This is same as shape but generator wants those information as well
			el = doc.CreateElement("Width");
			attr = doc.CreateAttribute("Value");
			attr.Value = this.WidthInUnits.ToString() + unitMng.UnitToString(MeasureUnit);
			el.SetAttributeNode(attr);
			element.AppendChild(el);

			el = doc.CreateElement("Height");
			attr = doc.CreateAttribute("Value");
			attr.Value = this.HeightInUnits.ToString() + unitMng.UnitToString(MeasureUnit);
			el.SetAttributeNode(attr);
			element.AppendChild(el);

			el = doc.CreateElement("WidthInPixels");
			attr = doc.CreateAttribute("Value");
			attr.Value = this.WidthInPixels.ToString();
			el.SetAttributeNode(attr);
			element.AppendChild(el);

			el = doc.CreateElement("HeightInPixels");
			attr = doc.CreateAttribute("Value");
			attr.Value = this.HeightInPixels.ToString();
			el.SetAttributeNode(attr);
			element.AppendChild(el);


			// save Shape
			el = doc.CreateElement("Shape");
			attr = doc.CreateAttribute("Type");
			attr.Value = "Rectangle";
			el.SetAttributeNode(attr);

			XmlElement el2 = doc.CreateElement("Dimensions");
			attr = doc.CreateAttribute("Width");
			attr.Value = this.WidthInUnits.ToString() + unitMng.UnitToString(MeasureUnit);
			el2.SetAttributeNode(attr);
			el.AppendChild(el2);

			attr = doc.CreateAttribute("Height");
			attr.Value = this.HeightInUnits.ToString() + unitMng.UnitToString(MeasureUnit);
			el2.SetAttributeNode(attr);
			el.AppendChild(el2);

			element.AppendChild(el);

			// Save Scale
			el = doc.CreateElement("Scale");
			attr = doc.CreateAttribute("x");
			attr.Value = this.ScaleXFactor.ToString();
			el.SetAttributeNode(attr);

			attr = doc.CreateAttribute("y");
			attr.Value = this.ScaleXFactor.ToString();
			el.SetAttributeNode(attr);

			element.AppendChild(el);

			// save transformations
			el = doc.CreateElement("Transformation");
			attr = doc.CreateAttribute("a");
			attr.Value = this.TransformationMatrix.Elements[0].ToString();
			el.SetAttributeNode(attr);

			attr = doc.CreateAttribute("b");
			attr.Value = this.TransformationMatrix.Elements[1].ToString();
			el.SetAttributeNode(attr);

			attr = doc.CreateAttribute("c");
			attr.Value = this.TransformationMatrix.Elements[2].ToString();
			el.SetAttributeNode(attr);

			attr = doc.CreateAttribute("d");
			attr.Value = this.TransformationMatrix.Elements[3].ToString();
			el.SetAttributeNode(attr);

			element.AppendChild(el);

            // save rotation
            el = doc.CreateElement("Rotation");
            attr = doc.CreateAttribute("Value");
            attr.Value = this.RotationAngle.ToString();
            el.SetAttributeNode(attr);

            element.AppendChild(el);

			// save dock position
			el = doc.CreateElement("DockPosition");
			attr = doc.CreateAttribute("Dock");
			attr.Value = this.DockPositionString;
			el.SetAttributeNode(attr);

			element.AppendChild(el);			

            // save anchor properties
            if(anchor != null)
            {
                el = doc.CreateElement("Anchor");
                attr = doc.CreateAttribute("Top");
                attr.Value = this.anchor.TopAnchor.ToString();
                el.SetAttributeNode(attr);
                attr = doc.CreateAttribute("Bottom");
                attr.Value = this.anchor.Bottomanchor.ToString();
                el.SetAttributeNode(attr);
                attr = doc.CreateAttribute("Left");
                attr.Value = this.anchor.LeftAnchor.ToString();
                el.SetAttributeNode(attr);
                attr = doc.CreateAttribute("Right");
                attr.Value = this.anchor.RightAnchor.ToString();
                el.SetAttributeNode(attr);

                element.AppendChild(el);
            }
		}

		/// <summary>
		/// Called by other consumers of this object.
		/// </summary>
		/// <param name="node"></param>
		public void Save(XmlDocument doc, XmlElement element)
		{
			if (!isDeleted)
			{
				SaveItem(doc, element);
			}
		}

		/// <summary>
		/// How this item is loaded
		/// </summary>
		/// <param name="node"></param>
		public virtual void Load(System.Xml.XmlNode element)
		{
			UnitsManager unitMng = new UnitsManager();

			// load name
			XmlAttribute attr = (XmlAttribute)element.Attributes.GetNamedItem("Name");
			if (attr != null)
			{
				this.Name = attr.Value;
			}

			// Load location if available
			XmlNode locationNode = element.SelectSingleNode("Location");
			if (locationNode != null)
			{
				this.MeasureUnit = unitMng.StringToUnit(locationNode.Attributes["PositionX"].Value);
				this.LocationInUnitsX = (float)(System.Convert.ToDouble(locationNode.Attributes["PositionX"].Value.TrimEnd(unitMng.UnitToString(MeasureUnit).ToCharArray())));
				this.LocationInUnitsY = (float)(System.Convert.ToDouble(locationNode.Attributes["PositionY"].Value.TrimEnd(unitMng.UnitToString(MeasureUnit).ToCharArray())));
			}

			

			// Load dimensions and shape definition
			XmlNode shapeNode = element.SelectSingleNode("Shape");
			if (shapeNode != null)
			{
				string shapeType = shapeNode.Attributes["Type"].Value;
				if (shapeType == "Rectangle")
				{
					// load width and height
					XmlNode dimensionNode = shapeNode.SelectSingleNode("Dimensions");
					if (dimensionNode != null)
					{						
						this.WidthInUnits = (float)(System.Convert.ToDouble(dimensionNode.Attributes["Width"].Value.TrimEnd(unitMng.UnitToString(MeasureUnit).ToCharArray())));
						this.HeightInUnits = (float)(System.Convert.ToDouble(dimensionNode.Attributes["Height"].Value.TrimEnd(unitMng.UnitToString(MeasureUnit).ToCharArray())));
					}
				}
			}

			// Load scale
			XmlNode scaleNode = element.SelectSingleNode("Scale");
			if (scaleNode != null)
			{
				MeasureUnits mUnit = unitMng.StringToUnit(scaleNode.Attributes["x"].Value);
				this.ScaleXFactor = (float)(System.Convert.ToDouble(scaleNode.Attributes["x"].Value.TrimEnd(unitMng.UnitToString(mUnit).ToCharArray())));
				this.ScaleYFactor = (float)(System.Convert.ToDouble(scaleNode.Attributes["y"].Value.TrimEnd(unitMng.UnitToString(mUnit).ToCharArray())));
			}			

			// Load Transformation 
			XmlNode transNode = element.SelectSingleNode("Transformation");
			if (transNode != null)
			{
				TransformationMatrix.Elements[0] = (float)System.Convert.ToDouble(transNode.Attributes["a"].Value);
				TransformationMatrix.Elements[1] = (float)System.Convert.ToDouble(transNode.Attributes["b"].Value);
				TransformationMatrix.Elements[2] = (float)System.Convert.ToDouble(transNode.Attributes["c"].Value);
				TransformationMatrix.Elements[3] = (float)System.Convert.ToDouble(transNode.Attributes["d"].Value);
			}

            // Load Rotation
            XmlNode rotationNode = element.SelectSingleNode("Rotation");
            if(rotationNode != null)
            {
                this.RotationAngle = Convert.ToInt16(rotationNode.Attributes[0].Value);
            }

			// load width in pixels and height in pixels as editor works with those
			XmlNode widthInPixelsNode = element.SelectSingleNode("WidthInPixels");
			if (widthInPixelsNode != null)
			{
				this.WidthInPixels = (float) Convert.ToDouble(widthInPixelsNode.Attributes[0].Value);
			}

			XmlNode heightInPixelsNode = element.SelectSingleNode("HeightInPixels");
			if (heightInPixelsNode != null)
			{
				this.HeightInPixels = (float) Convert.ToDouble(heightInPixelsNode.Attributes[0].Value);
			}



			// Load dock position
			XmlNode dockPos = element.SelectSingleNode("DockPosition");
			if (dockPos != null)
			{				
				this.DockPositionString = dockPos.Attributes["Dock"].Value;				
			}

            // load anchor property
            XmlNode anchorNode = element.SelectSingleNode("Anchor");
            if(anchorNode != null)
            {
                if(this.anchor == null) this.anchor = new Anchor(this);
                this.ItemAnchor.TopAnchor = Convert.ToBoolean(anchorNode.Attributes["Top"].Value);
                this.ItemAnchor.Bottomanchor = Convert.ToBoolean(anchorNode.Attributes["Bottom"].Value);
                this.ItemAnchor.LeftAnchor = Convert.ToBoolean(anchorNode.Attributes["Left"].Value);
                this.ItemAnchor.RightAnchor = Convert.ToBoolean(anchorNode.Attributes["Right"].Value);
            }

			this.commands.Clear();
            if(this is EditorProject || this is ReportPage)
            { 
            }
            else
            {
                EditorItemFactory.Instance.CreateItem(this);
            }
		}		

		/// <summary>
		/// This should place new editor item correctly in project hierarchy on location x, y in pixels - later these coordinates will become transformed to appropriate values
		/// </summary>
		/// <param name="newItem"></param>
		public virtual bool AddNewItem(EditorItem newItem, float x, float y, float width, float height, bool transformSize)
		{
			// if this item is not container return false
			if (!isContainer)
			{
				return false;
			}
			if (newItem == this)
			{
				return false;
			}

			// check if any of children items would return true on this method			
			foreach(EditorItem child in Children)
			{
				if (child.AddNewItem(newItem, x, y, width, height, transformSize))
				{
					return true;
				}
			}			

			// invert coordinates by view matrix and width and height vectors			
			PointF point = new PointF(x, y);
			PointF[] points = new PointF[1];
			points[0] = point;

            Matrix tmpMatrix = LastDrawMatrix.Clone();

			tmpMatrix.Invert();
            tmpMatrix.TransformPoints(points);
			point = points[0];
			x = point.X;
			y = point.Y;

			point.X = width;
			point.Y = height;

			if (transformSize)
			{
				points[0] = point;
                tmpMatrix.TransformVectors(points);
				point = points[0];
			}

			width = point.X;
			height = point.Y;			
			

			// does this new item falls inside this component
			float sx = 0; // *zoomLevel;
			float sy = 0; // *zoomLevel;
			float w = WidthInPixels; //  *zoomLevel;
			float h = HeightInPixels; // *zoomLevel;

			// if starting coordinate fall inside this component rect
			if (x >= sx && x <= sx + w && y >= sy && y <= sy + h)
			{
				if (newItem.Parent != this)
				{
					if (newItem.Parent != null)
					{
						//remove it from previous parent
						newItem.Parent.Children.Remove(newItem);
					}

					newItem.Parent = this;
					//this.Children.Add(newItem);

					// update and set location and size properties of newItem
					newItem.MeasureUnit = this.MeasureUnit;										
					
					newItem.LocationInUnitsX = UnitsManager.Instance.ConvertUnit(x, MeasureUnits.pixel, MeasureUnit);
					newItem.LocationInUnitsY = UnitsManager.Instance.ConvertUnit(y, MeasureUnits.pixel, MeasureUnit);
					newItem.LocationInUnitsX = (float)Math.Round((double)newItem.LocationInUnitsX, 3);
					newItem.LocationInUnitsY = (float)Math.Round((double)newItem.LocationInUnitsY, 3);

					if (width == 0 && height == 0)
					{

					}
					else
					{
						newItem.WidthInUnits = UnitsManager.Instance.ConvertUnit(width, MeasureUnits.pixel, MeasureUnit);
						newItem.HeightInUnits = UnitsManager.Instance.ConvertUnit(height, MeasureUnits.pixel, MeasureUnit);                    
					}

					// notify that we have placed item
					if (EditorItem.ItemPlaced != null)
					{
						EditorItem.ItemPlaced(newItem);
					}
				}
			
				return true;
			}
			else 
			{
				return false;
			}			
		}
			
		/// <summary>
		/// TODO: Check if this is necessary
		/// </summary>
		public float zoomLevel;

		/// <summary>
		/// Final matrix used for drawing. Composition of world and view matrices
		/// </summary>
		public System.Drawing.Drawing2D.Matrix LastDrawMatrix = new System.Drawing.Drawing2D.Matrix();

		/// <summary>
		///  this matrix is calculated
		/// </summary>
		protected System.Drawing.Drawing2D.Matrix drawMatrix = new System.Drawing.Drawing2D.Matrix();

		/// <summary>
		/// Draw matrix. Like World matrix in directX
		/// </summary>
		[Browsable(false)]
		public System.Drawing.Drawing2D.Matrix DrawMatrix
		{
			get { return drawMatrix; }			
		}

		/// <summary>
		/// Make draw matrix
		/// </summary>
		/// <param name="parentMatrix"></param>
		public virtual void MakeDrawMatrix(System.Drawing.Drawing2D.Matrix parentMatrix)
		{
			drawMatrix.Reset();
			drawMatrix.Multiply(this.TransformationMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
			drawMatrix.Multiply(parentMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);			
			
			
			foreach(EditorItem item in Children)
			{
				item.MakeDrawMatrix(drawMatrix);
			}
		}
		

		/// <summary>
		/// Disable mode
		/// </summary>
		/// <param name="disableMode"></param>
        public virtual void SetDisableMode(bool disableMode)
        {
            this.Disabled = disableMode;
            foreach(EditorItem child in Children)
            {
                child.SetDisableMode(disableMode);
            }
        }		


		/// <summary>
		/// drawing method that will show item
		/// </summary>
		/// <param name="gc"></param>
		public virtual void DisplayItem(Graphics gc)
		{            
			gc.DrawRectangle(Pens.Black, 0, 0, this.WidthInPixels, this.HeightInPixels);
		}

		protected System.Drawing.Drawing2D.Matrix viewMatrix = new System.Drawing.Drawing2D.Matrix();
		
		/// <summary>
		/// View matrix used for drawing
		/// </summary>
		[Browsable(false)]
		public virtual System.Drawing.Drawing2D.Matrix ViewMatrix
		{
			get { return viewMatrix; }		
		}

		/// <summary>
		/// Main Draw method
		/// </summary>
		/// <param name="viewMatrix"></param>
		/// <param name="gc"></param>
		/// <param name="clipRect"></param>
		public virtual void Draw(System.Drawing.Drawing2D.Matrix viewMatrix, Graphics gc, Rectangle clipRect)		
		{
			// set correct matrix
			this.viewMatrix = viewMatrix.Clone();

			System.Drawing.Drawing2D.Matrix mat = this.DrawMatrix.Clone();
			mat.Multiply(viewMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
			gc.Transform = mat;
			this.LastDrawMatrix = mat.Clone();
			
			// display item
			this.DisplayItem(gc);			
			this.DrawSelected(viewMatrix.Elements[0], gc);

			// draw children
			foreach(EditorItem item in Children)
			{				
				item.Draw(viewMatrix, gc, clipRect);
			}


            if(this is Balloon)
            {
                Balloon tmp = (Balloon)this;
                tmp.DrawBorders(gc);
            }
		}

		/// <summary>
		/// Draw Selection
		/// </summary>
		/// <param name="zoomLevel"></param>
		/// <param name="gc"></param>
		protected void DrawSelected(float zoomLevel, Graphics gc)
		{
			if (IsSelected)
			{
				float locX = -1.0f / zoomLevel;
				float locY = -1.0f / zoomLevel;
				float w = this.WidthInPixels + 1.0f / zoomLevel;
				float h = this.HeightInPixels + 1.0f / zoomLevel;
				Pen p = new Pen(Color.Gray, 1.0f / zoomLevel);
				p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
				gc.DrawRectangle(p, locX, locY, w, h);
			}
		}


		/// <summary>
		/// This will draw commands
		/// </summary>
		/// <param name="offsetX"></param>
		/// <param name="offsetY"></param>
		/// <param name="parentX"></param>
		/// <param name="parentY"></param>
		/// <param name="zoomLevel"></param>
		/// <param name="gc"></param>
		/// <param name="clipRect"></param>
		public virtual void DrawCommands(float zoomLevel, Graphics gc)
		{
			foreach(CommandItem cmd in this.commands)
			{
				cmd.Draw(zoomLevel, gc);
			}

			foreach (EditorItem child in this.Children)
			{
				child.DrawCommands(zoomLevel, gc);
			}

		}

		/// <summary>
		/// Cursor that this object uses
		/// </summary>
		protected Cursor cursor = Cursors.SizeAll;

		/// <summary>
		/// list of command for this object
		/// </summary>
		protected List<CommandItem> commands = new List<CommandItem>();

		/// <summary>
		/// return cursor for this item
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="zoomLevel"></param>
		/// <returns></returns>
		public virtual Cursor GetCursor(float x, float y, float zoomLevel)
		{
			if (Disabled == true)
			{
				return null;
			}

			// check for child cursors
			foreach (EditorItem child in Children)
			{
				Cursor cursor = child.GetCursor(x, y, zoomLevel);
				if (cursor != null)
				{
					return cursor;
				}
			}

			// get cursor from commands if available
			foreach (CommandItem cmd in commands)
			{
				Cursor cur = cmd.GetCursor(x, y, this.viewMatrix);
				if (cur != null)
				{
					return cur;
				}
			}

			// transform x,y back to object coordinates to check for selection			
			System.Drawing.Drawing2D.Matrix mat = LastDrawMatrix.Clone();
			mat.Invert();

			//mat.Multiply(this.TransformationMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
			//mat.Scale(zoomLevel, zoomLevel, System.Drawing.Drawing2D.MatrixOrder.Append);
			//mat.Invert();

			PointF tmpPoint = new PointF(x, y);
			PointF[] points = new PointF[1];
			points[0] = tmpPoint;
			mat.TransformPoints(points);

			tmpPoint = points[0];
			// check if this item should be selected
			//float tmpX = LocationInPixelsX * zoomLevel;
			//float tmpY = LocationInPixelsY * zoomLevel;
			float w = WidthInPixels; //** zoomLevel;
			float h = HeightInPixels; //* zoomLevel;


			// if starting coordinate fall inside this component rect
			if (tmpPoint.X >= 0 && tmpPoint.X <= w && tmpPoint.Y >= 0 && tmpPoint.Y <= h)
			{				
				return this.cursor;
			}
			else
			{
				return null;
			}			
		}


		/// <summary>
		/// Select command and return it
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public virtual CommandItem SelectCommand(float x, float y)
		{
			if (Disabled)
			{
				return null;
			}
			foreach (EditorItem child in Children)
			{
				CommandItem selectedCmd = child.SelectCommand(x, y);
				if (selectedCmd != null)
				{
					return selectedCmd;
				}
			}

			// check if we can select some of this commands
			foreach(CommandItem cmd in this.commands)
			{
				if (cmd.CanBeSelected(x, y, this.viewMatrix))
				{
					return cmd;
				}
			}

			return null;
		}


		/// <summary>
		/// This will perform selection. Deepest child is selected
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public virtual EditorItem SelectItem(float x, float y)
		{
            if (Disabled == true)
            {
                return null;
            }

			for (int i = Children.Count - 1; i >= 0; i-- )
			{
				EditorItem selectedItem = Children[i].SelectItem(x, y);
				if (selectedItem != null)
				{
					return selectedItem;
				}
			}

			// transform x,y back to object coordinates to check for selection
			System.Drawing.Drawing2D.Matrix mat = this.LastDrawMatrix.Clone();							
			mat.Invert();

			PointF tmpPoint = new PointF(x, y);
			PointF[] points = new PointF[1];
			points[0] = tmpPoint;
			mat.TransformPoints(points);

			tmpPoint = points[0];
			// check if this item should be selected
			//float tmpX = LocationInPixelsX * zoomLevel;
			//float tmpY = LocationInPixelsY * zoomLevel;
			float w = WidthInPixels; //** zoomLevel;
			float h = HeightInPixels; //* zoomLevel;
			
	
			// if starting coordinate fall inside this component rect
			if (tmpPoint.X >= 0 && tmpPoint.X <= w && tmpPoint.Y >= 0 && tmpPoint.Y <= h)
			{
				this.IsSelected = true;
				return this;
			}
			else 
			{
				return null;
			}
        }


        #region Generating preview

		private struct GeneratorGridCell
		{
			public float sx;
			public float sy;
			public bool allocated;
		}

		// this will return smaller float if their epsilon is 0.001 or less
		private static void CorrectMeasurements(ref float value1, ref float value2)
		{
			if (Math.Abs(value1 - value2) <= 0.001f)
			{
				if (value1 < value2)
				{ 
					value1 = value2;
				}
				else 
				{
					value2 = value1;
				}
			}
		}

        public bool GetNewRectRightBottom(Balloon generatedBalloon, ref RectangleNormal outRect)
        {
            RectangleNormal resRect = new RectangleNormal();
            RectangleNormal containRect = new RectangleNormal();
            //RectangleNormal firstFound = null;
            //IEnumerator<RectangleNormal> tempEnum = null;
            //RectangleNormal tempRect;
            ////RectangleNormal intersectFound = null;
            //RectangleNormal maxXRect = null;
            //RectangleNormal minXRect = null;
            //RectangleNormal minYRect = null;

            //bool tmpB = false;

            // make rect to be at 0,0 position. 0,0 is top, left coordinate in this situation
            if(outRect == null)
            {
                return false;
            }

            resRect.left = 0;
            resRect.top = 0;
            resRect.right = generatedBalloon.WidthInPixels;
            resRect.bottom = generatedBalloon.HeightInPixels;

            containRect.left = 0;
            containRect.top = 0;
            containRect.right = this.WidthInPixels;
            containRect.bottom = this.HeightInPixels;
			//System.IO.TextWriter writer;
						
			// make grid from rectMatrix
			GeneratorGridCell[,] grid;

			int rowCount = 0;
			int colCount = 0;

			List<float> horizLines = new List<float>();
			List<float> vertLines = new List<float>();

			vertLines.Add(0.0f);
			vertLines.Add(containRect.right);
			horizLines.Add(0.0f);
			horizLines.Add(containRect.bottom);


			// Make rect lines
			foreach(RectangleNormal rect in this.rectMatrix)
			{
				// add to horiz lines
				if (!horizLines.Contains(rect.top))
				{
					horizLines.Add(rect.top);
				}
				if (!horizLines.Contains(rect.bottom))
				{
					horizLines.Add(rect.bottom);
				}
				// add vertical lines
				if (!vertLines.Contains(rect.left))
				{
					vertLines.Add(rect.left);
				}
				if (!vertLines.Contains(rect.right))
				{
					vertLines.Add(rect.right);
				}
			}

			colCount = vertLines.Count - 1;
			rowCount = horizLines.Count - 1;

			vertLines.Sort();
			horizLines.Sort();

			if (rowCount <= 0 || colCount <= 0)
			{
				resRect.MoveTo(0, 0);
				outRect.left = resRect.left;
				outRect.right = resRect.right;
				outRect.bottom = resRect.bottom;
				outRect.top = resRect.top;
				return true;
			}

			grid = new GeneratorGridCell[rowCount, colCount];


			// fill grid			
			foreach (RectangleNormal rect in this.rectMatrix)
			{
				int xind1 = vertLines.IndexOf(rect.left);
				int xind2 = vertLines.IndexOf(rect.right);
				int yind1 = horizLines.IndexOf(rect.top);
				int yind2 = horizLines.IndexOf(rect.bottom);
				
				for(int i = xind1; i < xind2; i++)
				{
					for(int j = yind1; j < yind2; j++)
					{
						grid[j, i].allocated = true;
					}
				}				
			}

			// make coordinates
			for (int i = 0; i < rowCount; i++)
			{
				for (int j = 0; j < colCount; j++)
				{
					grid[i, j].sx = vertLines[j];
					grid[i, j].sy = horizLines[i];
				}
			}				

            // if all rects all allocated then outRect should be set to bottom line
            bool allAlocated = true;
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    if (!grid[i, j].allocated)
                    {
                        allAlocated = false;
                        break;
                    }
                }
            }

            if (allAlocated)
            {
                // move this to bottom line in case all blocks are allocated
                resRect.left = 0;
                resRect.top = containRect.bottom;
                resRect.right = generatedBalloon.WidthInPixels;
                resRect.bottom = generatedBalloon.HeightInPixels;
            }
            else
            {
                // find first empty place for new rect
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        if (!grid[i, j].allocated)
                        {
                            // check if rect can be fitted here
                            resRect.MoveTo(grid[i, j].sx, grid[i, j].sy);
                            // check for any intersection with any rect
                            bool anyIntersection = false;
                            foreach (RectangleNormal tmpRect in this.rectMatrix)
                            {
                                if (tmpRect.Intersect(resRect))
                                {
                                    anyIntersection = true;
                                    break;
                                }
                            }
                           
                            if (!anyIntersection && containRect.Contains(resRect))
                            {
                                outRect.left = resRect.left;
                                outRect.right = resRect.right;
                                outRect.bottom = resRect.bottom;
                                outRect.top = resRect.top;
                                return true;
                            }
                        }
                    }
                }
            }

            if (Math.Abs(resRect.bottom - containRect.bottom) < Epsilon || 
                resRect.bottom < containRect.bottom)
            {
                outRect.left = resRect.left;
                outRect.top = containRect.bottom;
                outRect.right = resRect.right;
                outRect.bottom = containRect.bottom + generatedBalloon.heightInPixels;
            }
            else
            {
                // make output as we are interested in last free position
                outRect.left = resRect.left;
                outRect.right = resRect.right;
                outRect.bottom = resRect.bottom;
                outRect.top = resRect.top;
            }
			return false;        
        }


        public bool GetNewRect(Balloon generatedBalloon, ref RectangleNormal outRect)
        {
            return this.GetNewRectRightBottom(generatedBalloon, ref outRect);
        }


        public virtual bool AddChild(Balloon generatedBalloon, bool isFromStatic, bool staticDockedBottomFlag)
        {
            return false;
        }

        public void AddChildToList(Balloon generatedBalloon)
        {
            // add to rect matrix this rectangle
            generatedBalloon.positionRect.right = generatedBalloon.positionRect.left + generatedBalloon.WidthInPixels;
            generatedBalloon.positionRect.bottom = generatedBalloon.positionRect.top + generatedBalloon.HeightInPixels;
            generatedBalloon.LocationInUnitsX = UnitsManager.Instance.ConvertUnit(generatedBalloon.positionRect.left, MeasureUnits.pixel, generatedBalloon.MeasureUnit);
            generatedBalloon.LocationInUnitsY = UnitsManager.Instance.ConvertUnit(generatedBalloon.positionRect.top, MeasureUnits.pixel, generatedBalloon.MeasureUnit);

            this.rectMatrix.Add(generatedBalloon.positionRect);

            // add parent/child connections
            generatedBalloon.Parent = this;           	
        }

        #endregion



        #region Docking region


        protected DockingPosition dockPosition = DockingPosition.DOCK_NONE;
        [Browsable(false)]
		public AxiomCoders.PdfTemplateEditor.EditorStuff.EditorItem.DockingPosition DockPosition
		{
			get { return dockPosition; }
			set { dockPosition = value; }
		}


		[Browsable(true), DisplayName("Dock"), Description("Set docking position"), Category("Standard"), Editor(typeof(ComboBoxPropertyEditor), typeof(UITypeEditor))]
		public virtual string DockPositionString
		{
			get
			{
				ComboBoxPropertyEditor.ItemList = GetDockingList();
				return StringFromDocking(dockPosition);
			}
			set
			{
				dockPosition = DockingFromString(value);
				if (dockPosition != DockingPosition.DOCK_NONE)
				{
					if (!NonDockingValuesSaved)
					{
						NonDockingHight = HeightInUnits;
						NonDockingWidth = WidthInUnits;
						NonDockingLocationX = LocationInUnitsX;
						NonDockingLocationY = LocationInUnitsY;
						NonDockingValuesSaved = true;
					}
					else 
					{
						WidthInUnits = NonDockingWidth;
						LocationInUnitsX = NonDockingLocationX;
						LocationInUnitsY = NonDockingLocationY;
						HeightInUnits = NonDockingHight;						
					}
				}
				else 
				{
					if (NonDockingValuesSaved)
					{
						WidthInUnits = NonDockingWidth;
						LocationInUnitsX = NonDockingLocationX;
						LocationInUnitsY = NonDockingLocationY;
						HeightInUnits = NonDockingHight;	
						NonDockingValuesSaved = false;
					}
				}

				if (this.Parent != null)
				{
					// tell parent we want to dock
					this.Parent.DockAll();
				}

				CreateCommands();
			}
		}

        public DockingPosition DockingFromString(string dockingName)
        {
            switch (dockingName)
            {
                case "Top": return DockingPosition.DOCK_TOP;
                case "Left": return DockingPosition.DOCK_LEFT;
                case "Bottom": return DockingPosition.DOCK_BOTTOM;
                case "Right": return DockingPosition.DOCK_RIGHT;
                case "Fill": return DockingPosition.DOCK_FILL;
                case "None": return DockingPosition.DOCK_NONE;
                default: return DockingPosition.DOCK_NONE;
            }
        }

        public static string StringFromDocking(DockingPosition docking)
        {
            switch (docking)
            {
                case DockingPosition.DOCK_TOP: return "Top";
                case DockingPosition.DOCK_LEFT: return "Left";
                case DockingPosition.DOCK_BOTTOM: return "Bottom";
                case DockingPosition.DOCK_RIGHT: return "Right";
                case DockingPosition.DOCK_FILL: return "Fill";
                case DockingPosition.DOCK_NONE: return "None";
                default: return "None";
            }
        }

        public string[] GetDockingList()
        {
            string[] comboList = new string[6];
            comboList[0] = "Top";
            comboList[1] = "Left";
            comboList[2] = "Bottom";
            comboList[3] = "Right";
            comboList[4] = "Fill";
            comboList[5] = "None";

            return comboList;
        }

		protected bool NonDockingValuesSaved = false;
		protected float NonDockingHight = 0;
		protected float NonDockingWidth = 0;
		protected float NonDockingLocationX = 0;
		protected float NonDockingLocationY = 0;


        private float Koef(float a, float b, float c, float d)
        {
            float ret = 0;

            ret = a - (b + c + d);

            return ret < 0 ? -(ret) : 0;
        }

		
		public void DockAll()
		{
			float lx = 0, ly = 0;
			float wx = 0, wy = 0;
            float toH = 0, botH = 0, leW = 0, rigW = 0;
			wx = this.WidthInPixels;
			wy = this.HeightInPixels;

			foreach(EditorItem child in Children)
			{
				if (child.dockPosition != DockingPosition.DOCK_NONE)
				{
					switch (child.dockPosition)
					{
						case DockingPosition.DOCK_TOP:
							child.LocationInUnitsX = UnitsManager.Instance.ConvertUnit(lx + leW, MeasureUnits.pixel, this.MeasureUnit);
							child.LocationInUnitsY = UnitsManager.Instance.ConvertUnit(ly + toH, MeasureUnits.pixel, this.MeasureUnit);
							child.WidthInPixels = wx - (leW + rigW);
                            child.HeightInPixels -= Koef(wy, toH, botH, child.HeightInPixels);
                            toH += child.HeightInPixels;
							break;
						case DockingPosition.DOCK_LEFT:
							child.LocationInUnitsX = UnitsManager.Instance.ConvertUnit(lx + leW, MeasureUnits.pixel, this.MeasureUnit);
							child.LocationInUnitsY = UnitsManager.Instance.ConvertUnit(ly + toH, MeasureUnits.pixel, this.MeasureUnit);
							child.HeightInPixels = wy - botH - toH;
                            child.WidthInPixels -= Koef(wx, leW, rigW, child.WidthInPixels);
                            leW += child.WidthInPixels;
							break;
						case DockingPosition.DOCK_RIGHT:
                            child.WidthInPixels -= Koef(wx, leW, rigW, child.WidthInPixels);
							child.LocationInUnitsX = UnitsManager.Instance.ConvertUnit(wx - child.WidthInPixels - rigW, MeasureUnits.pixel, this.MeasureUnit);
							child.LocationInUnitsY = UnitsManager.Instance.ConvertUnit(ly + toH, MeasureUnits.pixel, this.MeasureUnit);
                            child.HeightInPixels = wy - botH - toH;
                            rigW += child.WidthInPixels;
							break;
						case DockingPosition.DOCK_BOTTOM:
                            child.HeightInPixels -= Koef(wy, toH, botH, child.HeightInPixels);
							child.LocationInUnitsX = UnitsManager.Instance.ConvertUnit(lx + leW, MeasureUnits.pixel, this.MeasureUnit);
							child.LocationInUnitsY = UnitsManager.Instance.ConvertUnit(wy - child.HeightInPixels - botH, MeasureUnits.pixel, this.MeasureUnit);
							child.WidthInPixels = wx - (leW + rigW);
                            botH += child.HeightInPixels;
							break;
						case DockingPosition.DOCK_FILL:
							child.LocationInUnitsX = UnitsManager.Instance.ConvertUnit(lx + leW, MeasureUnits.pixel, this.MeasureUnit);
							child.LocationInUnitsY = UnitsManager.Instance.ConvertUnit(ly + toH, MeasureUnits.pixel, this.MeasureUnit);
                            child.WidthInPixels = wx; 
                            child.HeightInPixels = wy; 
                            child.WidthInPixels -= Koef(wx, leW, rigW, child.WidthInPixels);
                            child.HeightInPixels -= Koef(wy, toH, botH, child.HeightInPixels);
							break;
					}					
				}
				child.DockAll();
			}
		}
      

#endregion


		#region ICloneable Members

		/// <summary>
		/// Little different clone where parent is set to new one
		/// </summary>
		/// <param name="parent"></param>
		/// <returns></returns>
		protected object Clone(EditorItem newParent)
		{
			EditorItem newItem = (EditorItem)this.MemberwiseClone();

			// make new name			
			newItem.Name = MakeName();

			// make new matrices 
			newItem.drawMatrix = new System.Drawing.Drawing2D.Matrix();
			newItem.LastDrawMatrix = new System.Drawing.Drawing2D.Matrix();
			newItem.viewMatrix = new System.Drawing.Drawing2D.Matrix();

			newItem.Parent = newParent;
			// attach to parent
            //if (newItem.Parent != null)
            //{
            //    newItem.Parent.Children.Add(newItem);
            //}

			// copy children
			newItem.children = new List<EditorItem>();
			foreach (EditorItem child in this.Children)
			{
                // clone child giving them new parent as parameter
                child.Clone(newItem);
			}

			// tell factory that we created some new item
			EditorItemFactory.Instance.CreateItem(newItem);
			return newItem;			
		}

        [Browsable(false)]
        protected int creationId = 0;

        private int lastCreationId = 0;

		/// <summary>
		/// This will clone basic item properties. Children are not cloned. Parent is set to null. Item is cloned but not stored anywhere. Can be used by generator
		/// </summary>
		/// <param name="newParent"></param>
		/// <returns></returns>
		public virtual object SimpleClone()
		{
			EditorItem newItem = (EditorItem)this.MemberwiseClone();

			// make new name			
			newItem.Name = MakeName();

			// make new matrices 
			newItem.drawMatrix = new System.Drawing.Drawing2D.Matrix();
			newItem.LastDrawMatrix = new System.Drawing.Drawing2D.Matrix();
			newItem.viewMatrix = new System.Drawing.Drawing2D.Matrix();

			newItem.Parent = null;			
			newItem.children = new List<EditorItem>();
            newItem.creationId = lastCreationId++;
			return newItem;			
		}

		/// <summary>
		/// Pure clone, remains same parent in heirarchy
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			EditorItem newItem = (EditorItem)this.MemberwiseClone();

			// make new name			
			newItem.Name = MakeName();
         
			// make new matrices 
			newItem.drawMatrix = new System.Drawing.Drawing2D.Matrix();
			newItem.LastDrawMatrix = new System.Drawing.Drawing2D.Matrix();
			newItem.viewMatrix = new System.Drawing.Drawing2D.Matrix();
            
			newItem.LocationInUnitsX += newItem.WidthInUnits / 3.0f;
			newItem.LocationInUnitsY += newItem.HeightInUnits / 3.0f;
									
			// attach to parent
			if (newItem.Parent != null)
			{
				newItem.Parent.Children.Add(newItem);
			}			
			
			// copy children
			newItem.children = new List<EditorItem>();
			foreach(EditorItem child in this.Children)
			{
                // create new children and specify new parent
                child.Clone(newItem);
			}			
			
			return newItem;			
		}

		#endregion
	}
}
