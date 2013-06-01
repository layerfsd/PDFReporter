using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

using AxiomCoders.PdfTemplateEditor.Common;
using AxiomCoders.PdfTemplateEditor.EditorStuff;
using System.Xml;
using System.Windows.Forms;
using System.Drawing.Design;
using System.IO;

namespace AxiomCoders.PdfTemplateEditor.EditorItems
{
	/// <summary>
	/// Presents page that is shown on editor
	/// </summary>	
	public class ReportPage: EditorItem
	{
		private ReportPageViewer viewer = null;
        private string description;

        /// <summary>
        /// Fill Color for page
        /// </summary>
        private Color fillColor;

        /// <summary>
        /// Fill color for page
        /// </summary>
        [Browsable(true), DisplayName("Fill Color")]
        public Color FillColor
        {
            get
            {
                return fillColor;
            }
            set
            {
                fillColor = value;
            }
        }

        /// <summary>
        /// Image data
        /// </summary>
        private byte[] imageData;

        /// <summary>
        /// name of background image
        /// </summary>
        private string imageName;

        private Image pictureForDisplay = null;

        /// <summary>
        /// Image name used
        /// </summary>
        [Browsable(true), DisplayName("Image Name"), Description("Browse for image that you want to be background..."), Category("Misc"), EditorAttribute(typeof(ImageNameEditor), typeof(UITypeEditor))]
        public string ImageName
        {
            set
            {
                imageName = value;
                if (imageName != "")
                {
                    try
                    {
                        if (File.Exists(imageName))
                        {
                            imageData = File.ReadAllBytes(imageName);
                            pictureForDisplay = Bitmap.FromStream(new MemoryStream(this.imageData));
                        }
                        else
                        {
                            MessageBox.Show(string.Format("Image {0} does not exists.", imageName));
                            imageName = "";
                        }
                    }
                    catch
                    {
                        // TODO: add some image not found picture
                        pictureForDisplay = null;
                    }
                }
                else
                {
                    pictureForDisplay = null;
                    imageData = null;
                }
            }
            get
            {
                return imageName;
            }
        }


		[Browsable(false)]
		public AxiomCoders.PdfTemplateEditor.EditorItems.ReportPageViewer Viewer
		{
			get { return viewer; }			
		}

		public ReportPage()
		{
			viewer = new ReportPageViewer(this);
			isContainer = true;
            description = string.Empty;
            fillColor = Color.White;	
		}
	
		/// <summary>
		/// What resolution is used
		/// </summary>		
		[Browsable(true), ReadOnly(true), Description("Resolution of page")]
		public float Resolution
		{
			get { return UnitsManager.Instance.Resolution; }
			set { UnitsManager.Instance.Resolution = value; }
		}
		

        /// <summary>
        /// Page description.
        /// </summary>
        [Browsable(true)]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

		/// <summary>
		/// What resolution measurement is used
		/// </summary>		
        [Browsable(true), DisplayName("Resolution Unit"), Description("Resolution measurement unit")]
		public AxiomCoders.PdfTemplateEditor.Common.ResolutionMeasure ResolutionMeasure
		{
			get { return UnitsManager.Instance.ResolutionMeasure; }
			set { UnitsManager.Instance.ResolutionMeasure = value; }
		}

		[Browsable(false)]
		public new float RotationAngle
		{
			get { return rotationAngle; }
			set { rotationAngle = value; }
		}

		[Browsable(false)]
		public override string DockPositionString
		{
			get
			{
				return base.DockPositionString;
			}
			set
			{
				base.DockPositionString = value;
			}
		}

		/// <summary>
		/// This is zoom level in percents
		/// </summary>
		private new int zoomLevel = 100;

		/// <summary>
		/// Zoom level from 10 - 1000
		/// </summary>
        [Browsable(true), DisplayName("Zoom (%)"), Description("Zoom level. From 5...800")]
		public int ZoomLevel
		{
			get { return zoomLevel; }
			set 			
			{				
				zoomLevel = value; 
				if (zoomLevel < 5) 
				{
					zoomLevel = 5;
				}
				if (zoomLevel > 800)
				{
					zoomLevel = 800;
				}
			}
		}

		private List<CommandItem> commandItems = new List<CommandItem>();

		/// <summary>
		/// List of commands
		/// </summary>
		[Browsable(false)]
		public List<CommandItem> CommandItems
		{
			get { return commandItems; }
			set { commandItems = value; }
		}

        		
		public override void SaveItem(XmlDocument doc, XmlElement element)
		{						            
			XmlElement el = doc.CreateElement("Page");
			base.SaveItem(doc, el);


			UnitsManager unitMng = new UnitsManager();

			XmlAttribute attr = doc.CreateAttribute("Version");
			attr.Value = "1.0";
			el.SetAttributeNode(attr);

			// save info
			XmlElement el2 = doc.CreateElement("Info");
			attr = doc.CreateAttribute("Description");
			attr.Value = this.Description;
			el2.SetAttributeNode(attr);
			el.AppendChild(el2);

			// save size
			el2 = doc.CreateElement("Size");
			attr = doc.CreateAttribute("Width");
			attr.Value = this.WidthInUnits.ToString() + unitMng.UnitToString(MeasureUnit);
			el2.SetAttributeNode(attr);

			attr = doc.CreateAttribute("Height");
			attr.Value = this.HeightInUnits.ToString() + unitMng.UnitToString(MeasureUnit);
			el2.SetAttributeNode(attr);
			el.AppendChild(el2);

            // FillColor
            el2 = doc.CreateElement("FillColor");
            attr = doc.CreateAttribute("R"); attr.Value = this.FillColor.R.ToString(); el2.SetAttributeNode(attr);
            attr = doc.CreateAttribute("G"); attr.Value = this.FillColor.G.ToString(); el2.SetAttributeNode(attr);
            attr = doc.CreateAttribute("B"); attr.Value = this.FillColor.B.ToString(); el2.SetAttributeNode(attr);
            attr = doc.CreateAttribute("A"); attr.Value = this.FillColor.A.ToString(); el2.SetAttributeNode(attr);
            el.AppendChild(el2);

            el2 = doc.CreateElement("BackgroundImage");
            attr = doc.CreateAttribute("Name");
            attr.Value = this.ImageName;
            el2.SetAttributeNode(attr);
            el.AppendChild(el2);


            // embedd image data here if embeddImageToTemplate is used          
            el2 = doc.CreateElement("EmbeddedImage");
            attr = doc.CreateAttribute("Name");
            attr.Value = this.ImageName;
            el2.SetAttributeNode(attr);

            if (imageData != null && imageData.Length > 0)
            {
                attr = doc.CreateAttribute("EmbeddedDecodedImageLength"); attr.Value = imageData.Length.ToString(); el2.SetAttributeNode(attr);
                XmlText textValue = doc.CreateTextNode("EmdeddedImageData");
                textValue.Value = Convert.ToBase64String(imageData);
                attr = doc.CreateAttribute("EmbeddedImageLength"); attr.Value = textValue.Value.Length.ToString(); el2.SetAttributeNode(attr);
                el2.AppendChild(textValue);
            }
            el.AppendChild(el2);

            // save children without balloons
            el2 = doc.CreateElement("Items");
            foreach(EditorItem child in this.Children)
            {
                if(child is Balloon) { }
                else
                {
                    child.Save(doc, el2);
                }
            }
            el.AppendChild(el2);


			// save children with balloons
			el2 = doc.CreateElement("Balloons");			
			foreach(EditorItem child in this.Children)
			{
                if(child is Balloon)
                {
                    child.Save(doc, el2);
                }
			}
			el.AppendChild(el2);

			element.AppendChild(el);
		}

		/// <summary>
		/// Create item from string type
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static EditorItem CreateItemFromType(string type)
		{
			switch (type)
			{
				case "StaticText":
					return new StaticText();
				case "DynamicText":
					return new DynamicText();					
				case "Counter":
					return new Counter();					
				case "DateTime":
					return new DateTime();
				case "ShapeRectangle":
					return new RectangleShape();
				case "StaticImage":
					return  new ImageItem();
                case "PageNumberItem":
                    return new PageNumberItem();
                case "DynamicImage":
                    return new DynamicImage();
                case "Precalculated":
                    return new PrecalculatedItem();

			}
			return null;
		}

		public override void Load(System.Xml.XmlNode element)
		{
			base.Load(element);

            UnitsManager unitMng = new UnitsManager();

			// Load info
			XmlNode node = element.SelectSingleNode("Info");			
			if (node != null)
			{
				this.Description = node.Attributes["Description"].Value;
			}

			// Load page size
			node = element.SelectSingleNode("Size");
			if (node != null)
			{
				MeasureUnit = unitMng.StringToUnit(node.Attributes["Width"].Value);
				WidthInUnits = (float)(System.Convert.ToDouble(node.Attributes["Width"].Value.TrimEnd(unitMng.UnitToString(MeasureUnit).ToCharArray())));
				HeightInUnits = (float)(System.Convert.ToDouble(node.Attributes["Height"].Value.TrimEnd(unitMng.UnitToString(MeasureUnit).ToCharArray())));
			}
			else 
			{
				// set default sizes
				MeasureUnit = MeasureUnits.cm;
				WidthInUnits = (float)unitMng.ConvertUnit(19, MeasureUnits.cm, MeasureUnits.point);
				HeightInPixels = (float)unitMng.ConvertUnit(21, MeasureUnits.cm, MeasureUnits.point);
			}

            // Load fill color
            node = element.SelectSingleNode("FillColor");
            if (node != null)
            {
                try
                {
                    int r = Convert.ToInt32(node.Attributes["R"].Value);
                    int g = Convert.ToInt32(node.Attributes["G"].Value);
                    int b = Convert.ToInt32(node.Attributes["B"].Value);
                    int a = Convert.ToInt32(node.Attributes["A"].Value);
                    this.FillColor = Color.FromArgb(a, r, g, b);
                }
                catch { }
            }

            // Load BackgroundImageName
            node = element.SelectSingleNode("BackgroundImage");
            if (node != null)
            {
                this.imageName = node.Attributes["Name"].Value;
            }       

            // load embedded image if available
            node = element.SelectSingleNode("EmbeddedImage");
            if (node != null)
            {
                if (node.InnerText != null && node.InnerText.Length > 0)
                {
                    this.imageData = Convert.FromBase64String(node.InnerText);
                    pictureForDisplay = Bitmap.FromStream(new MemoryStream(this.imageData));
                }
            }



            // Load none balloons items
            node = element.SelectSingleNode("Items");
            if(node != null)
            {
                foreach(XmlNode childNode in node.ChildNodes)
                {                   
                    if(childNode.Name == "Item")
                    {
                        // this is not valid for generator but this can be loaded into editor
                        string itemType = childNode.Attributes["Type"].Value;
                        EditorItem newItem = ReportPage.CreateItemFromType(itemType);
                        newItem.Load(childNode);
                        newItem.Parent = this;
                        //Children.Add(newItem);
                    }
                }
            }			


			// Load balloons
			node = element.SelectSingleNode("Balloons");
			if (node != null)
			{
				foreach(XmlNode balloonNode in node.ChildNodes)
				{
					if (balloonNode.Name == "Balloon")
					{
						string tmpType;
						tmpType = balloonNode.Attributes["Type"].Value;
						if (tmpType == "Static")
						{
                            StaticBalloon child = new StaticBalloon();
                            //child.Parent = this;
							child.Load(balloonNode);
                            child.Parent = this;
							//Children.Add(child);
						}
						else if (tmpType == "Dynamic")
						{
                            DynamicBalloon child = new DynamicBalloon();
                            //child.Parent = this;
							child.Load(balloonNode);
                            child.Parent = this;
							//Children.Add(child);
						}
					}
					else if (balloonNode.Name == "Item")
					{
						// this is not valid for generator but this can be loaded into editor
						string itemType = balloonNode.Attributes["Type"].Value;
						EditorItem newItem = ReportPage.CreateItemFromType(itemType);
						newItem.Load(balloonNode);
						newItem.Parent = this;
						//Children.Add(newItem);
					}
				}
			}		
		}

		private float centerX;
		private float centerY;




		/// <summary>
		/// Make view matrix params and other params
		/// </summary>
		/// <param name="clipRect"></param>
		public void MakeViewMatrix(Rectangle clipRect)
		{
			centerX = ((float)clipRect.Width / 2.0f);
			centerY = ((float)clipRect.Height / 2.0f);			
		}


		public override void MakeDrawMatrix(System.Drawing.Drawing2D.Matrix parentMatrix)
		{
			base.MakeDrawMatrix(parentMatrix);

			// take offset
			/*System.Drawing.Drawing2D.Matrix finalMatrix = new System.Drawing.Drawing2D.Matrix();
			finalMatrix = this.drawMatrix.Clone();
			finalMatrix.Multiply(this.ViewMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);

			float offX = finalMatrix.Elements[4];
			float offY = finalMatrix.Elements[5];
			float sizeX = finalMatrix.Elements[0] * this.WidthInPixels;
			float sizeY = finalMatrix.Elements[3] * this.HeightInPixels;
			
			if (offX < 0 || )
			{
				Viewer.HorizontalScrollRequired = true;
				Viewer.HorizontalScrollerMaxValue = (int)-offX;
			}
			else 
			{
				Viewer.HorizontalScrollRequired = false;
				
			}

			if (offY < 0)
			{
				Viewer.VerticalScrollRequired = true;
				Viewer.VerticalScrollerMaxValue = (int)-offY;
			}
			else
			{
				Viewer.VerticalScrollRequired = false;
			}*/
			
		}
		
		/// <summary>
		/// Draw page and its children
		/// </summary>
		/// <param name="gc"></param>
		/// <param name="clipRect"></param>
		public void Draw(Graphics gc, Rectangle clipRect)
		{
			System.Drawing.Drawing2D.Matrix finalMatrix = new System.Drawing.Drawing2D.Matrix();
			finalMatrix = this.drawMatrix.Clone();
			finalMatrix.Multiply(this.ViewMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
			
			gc.Transform = finalMatrix;
            
            // draw fill color
            using (SolidBrush brush = new SolidBrush(fillColor))
            {
                gc.FillRectangle(brush, 0, 0, (float)this.WidthInPixels, (float)this.HeightInPixels);
            }

            // draw background image
            if (pictureForDisplay != null)
            {
                gc.DrawImage(pictureForDisplay, 0, 0, (float)this.WidthInPixels, (float)this.HeightInPixels);
            }

			this.LastDrawMatrix = finalMatrix;

			EditorController.Instance.Grid.Draw(gc);


			foreach(EditorItem item in Children)
			{
				item.Draw(this.ViewMatrix, gc, clipRect);
			}

			// draw item commands
			foreach (EditorItem child in Children)
			{
				child.DrawCommands((float)this.ZoomLevel / 100, gc);
			}						
		}


		public override System.Drawing.Drawing2D.Matrix ViewMatrix
		{
			get 
			{
				System.Drawing.Drawing2D.Matrix scaleMatrix, transMatrix, offMatrix;
				float zoom = (float)this.zoomLevel / 100.0f;

				scaleMatrix = new System.Drawing.Drawing2D.Matrix();
				scaleMatrix.Scale(zoom, zoom);
				transMatrix = new System.Drawing.Drawing2D.Matrix();
				transMatrix.Translate(centerX, centerY);
				offMatrix = new System.Drawing.Drawing2D.Matrix();
				offMatrix.Translate(Viewer.OffsetXValue, Viewer.OffsetYValue);

				// first we scale than rotate and then transform
				this.viewMatrix.Reset();
				this.viewMatrix.Multiply(offMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
				this.viewMatrix.Multiply(scaleMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
				this.viewMatrix.Multiply(transMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);
				

				return this.viewMatrix;
			}
		}

		public override System.Drawing.Drawing2D.Matrix TransformationMatrix
		{
			get
			{		
				System.Drawing.Drawing2D.Matrix scaleMatrix, transMatrix;

				float zoom = (float)this.zoomLevel / 100.0f;
				scaleMatrix = new System.Drawing.Drawing2D.Matrix();
				scaleMatrix.Scale(zoom, zoom);
				transMatrix = new System.Drawing.Drawing2D.Matrix();
				transMatrix.Translate(-this.WidthInPixels / 2.0f, -this.HeightInPixels / 2.0f);

				// first we scale than rotate and then transform
				this.transformationMatrix.Reset();
				this.transformationMatrix.Multiply(transMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);				
				//this.transformationMatrix.Multiply(scaleMatrix, System.Drawing.Drawing2D.MatrixOrder.Append);								
				

				return this.transformationMatrix;
			}
		}



		
		/// <summary>
		/// Return cursor for item
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public Cursor GetCursor(float x, float y)
		{
			foreach (EditorItem child in Children)
			{
				Cursor cursor = child.GetCursor(x, y, (float)this.ZoomLevel / 100);
				if (cursor != null)
				{
					return cursor;
				}

			}
			return Cursors.Default;			
		}

		/// <summary>
		/// Select item. Check only child items as report page cannot be selected
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public override EditorItem SelectItem(float x, float y)
		{
			foreach (EditorItem child in Children)
			{
				EditorItem selectedItem = child.SelectItem(x, y);
				if (selectedItem != null)
				{
					return selectedItem;
				}
				
			}
			return null;
		}

		/// <summary>
		/// Hide location in unitsX
		/// </summary>
		[Browsable(false)]
		public new float LocationInUnitsX
		{
			get { return locationInUnitsX; }
			set { locationInUnitsX = (float)Math.Round((double)value, 4); }
		}

		/// <summary>
		/// Hide location in unitsY
		/// </summary>
		[Browsable(false)]
		public new float LocationInUnitsY
		{
			get { return locationInUnitsY; }
			set { locationInUnitsY = (float)Math.Round((double)value, 4); }
		}


        public override bool AddChild(Balloon generatedBalloon, bool isFromStatic, bool staticDockedBottomFlag)
        {     
            RectangleNormal newRect = new RectangleNormal();

            if(isFromStatic)
            {
                // add it to the list
                this.AddChildToList(generatedBalloon);
            }
            else
            {
                if(!this.GetNewRect(generatedBalloon, ref newRect))
                {
                    return false;
                }
                else
                {
                    generatedBalloon.WidthInPixels = newRect.right - newRect.left;
                    generatedBalloon.HeightInPixels = newRect.bottom - newRect.top;
                    generatedBalloon.positionRect.top = newRect.top;
                    generatedBalloon.positionRect.left = newRect.left;
                    generatedBalloon.containerRect = newRect;
                    this.AddChildToList(generatedBalloon);
                }

                // 1. ask for free space and get new rect for it 
                // 2. in case there is no space 
                //       check for conditions to grow
                //          try to grow this balloon (also parents). Return which one failed growing if anyone failed and return FALSE
                //          if balloon grew goto 1.
                //		if cannot grow return false
                // 3. in case there is enough space. 
                //		move new balloon to the rect position and set position and sizes to structure
                //	    add balloon to rect List and return true.		
            }
            return true;
        }
	}
}
