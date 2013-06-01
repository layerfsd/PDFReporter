using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using AxiomCoders.PdfTemplateEditor.EditorStuff;
using AxiomCoders.PdfTemplateEditor.Common;
using AxiomCoders.PdfTemplateEditor.Forms;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using System.Drawing.Design;
using System.Diagnostics;

namespace AxiomCoders.PdfTemplateEditor.EditorItems
{
    [Serializable, System.Reflection.Obfuscation(Exclude = true)]
    public class BorderEditor : UITypeEditor
    {
        BorderPropertyForm editorDialog;
        public static Balloon parentBalloon;
        public BorderEditor()
        {

        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            editorDialog = new BorderPropertyForm();
            editorDialog.SourceBalloon = parentBalloon;			
            if(DialogResult.OK == editorDialog.ShowDialog())
            {
                value = true;
				EditorController.Instance.ProjectSaved = false;
            }else
            {
                value = false;
            }

            //value = "Border properties...";
            EditorController.Instance.EditorViewer.RefreshView();
            return value;// base.EditValue(context, provider, value);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }


	[System.Reflection.Obfuscation(Exclude = true)]
    public abstract class Balloon : EditorItem, DynamicEditorItemInterface
    {
        private bool availableOnEveryPage = true;
        private bool fillingGeneratesNew;
        private int fillCapacity;
        private bool canGrow = true;
        private bool fitToContent = true; 
        private List<RectangleNormal> rectMatrix = new List<RectangleNormal>();
        public RectangleNormal containerRect = new RectangleNormal();
        public RectangleNormal positionRect = new RectangleNormal();
        public BalloonBorders Borders;

        /// <summary>
        /// Fill Color for item
        /// </summary>
        private Color fillColor;

        /// <summary>
        /// Fill color for balloon
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
        /// balloon from which is this one created. Can be null. Used for preview mode
        /// </summary>
        private Balloon templateBalloon;

        /// <summary>
        /// balloon from which is this one created. Can be null. Used for preview mode
        /// </summary>
        public Balloon TemplateBalloon
        {
            get { return templateBalloon; }
            set { templateBalloon = value; }
        }       	

        /// <summary>
        /// Default constructor for balloon
        /// </summary>		
        public Balloon()
        {
            isContainer = true;            
            fillingGeneratesNew = false;            
			this.cursor = Cursors.Default;
			this.WidthInUnits = 3;
			this.HeightInUnits = 3;
            this.fillColor = Color.Transparent;
            Borders.top = new BalloonBorder(BorderTypes.TOP_BORDER);
            Borders.left = new BalloonBorder(BorderTypes.LEFT_BORDER);
            Borders.bottom = new BalloonBorder(BorderTypes.BOTTOM_BORDER);
            Borders.right = new BalloonBorder(BorderTypes.RIGHT_BORDER);
        }


		private string sourceDataStream = string.Empty;


        [Browsable(true), DisplayName("Border Properties"), Category("Standard"), Description("Edit border properties"), EditorAttribute(typeof(BorderEditor), typeof(UITypeEditor))]
        public string BorderProps
        {
            get
            {
                BorderEditor.parentBalloon = this;
                return "Border properties...";
            }
        }
        
		/// <summary>
		/// Data stream that this balloon uses
		/// </summary>
        [Browsable(true), DisplayName("Data Stream"), Description("Source Data Stream name of item."), Category("Standard"), EditorAttribute(typeof(ComboBoxPropertyEditor), typeof(UITypeEditor))]
		public string SourceDataStream
		{
            get
            {
                ComboBoxPropertyEditor.ItemList = null;

                int tmpCount = EditorController.Instance.EditorProject.DataStreams.Count;
                if(tmpCount > 0)
                {
                    string[] tmpList = new string[tmpCount];
                    int i = 0;
                    foreach(DataStream tmpItem in EditorController.Instance.EditorProject.DataStreams)
                    {
                        tmpList[i] = tmpItem.Name;
                        i++;
                    }
                    ComboBoxPropertyEditor.ItemList = tmpList;
                }

                return sourceDataStream;
            }
            set
            {
                string tmpValue = this.sourceDataStream;
                if(this is DynamicBalloon && this.Parent is DynamicBalloon)
                {
                    DynamicBalloon tmpItem = (DynamicBalloon)this.Parent;
                    if(tmpItem.SourceDataStream == value)
                    {
                        MessageBox.Show("You can't put same data stream as it is on Items parent!", "Invalid value");
                        sourceDataStream = tmpValue;
                    }else{
                        sourceDataStream = value;
                    }
                }else{
                    sourceDataStream = value;
                }                
            }
		}

        [Browsable(false)]
        public string SourceColumn
        {
            get
            {
                return string.Empty;
            }
            set
            {
                //sourceColumn = value;
            }
        }

        public Balloon(string name, string dataStream)
		{
			this.Name = name;
			this.SourceDataStream = dataStream;			
		}      

		protected override void CreateCommands()
		{
			base.CreateCommands();

			if (dockPosition == DockingPosition.DOCK_NONE)
			{
				CommandItem command;
				command = new MovingCommandItem(this);
				this.commands.Add(command);
			}
		}

		/// <summary>
		/// Load balloon properties
		/// </summary>
		/// <param name="txR"></param>
		public override void Load(System.Xml.XmlNode element)
		{
 			base.Load(element);

			XmlAttribute attr = (XmlAttribute)element.Attributes.GetNamedItem("DataStream");
			if (attr != null)
			{
				this.SourceDataStream = attr.Value;
			}

			XmlNode node = element.SelectSingleNode("AvailableOnEveryPage");
			if (node != null)
			{
				this.AvailableOnEveryPage = Convert.ToBoolean(node.Attributes["Value"].Value);
			}

            node = element.SelectSingleNode("FitToContent");
            if (node != null)
            {
                this.FitToContent = Convert.ToBoolean(node.Attributes["Value"].Value);
            }

			node = element.SelectSingleNode("FillingGeneratesNew");
			if (node != null)
			{
				this.FillingGeneratesNew = Convert.ToBoolean(node.Attributes["Value"].Value);
			}

			node = element.SelectSingleNode("FillCapacity");
			if (node != null)
			{
				this.FillCapacity = Convert.ToInt32(node.Attributes["Value"].Value);
			}

			node = element.SelectSingleNode("CanGrow");
			if (node != null)
			{
				this.CanGrow = Convert.ToBoolean(node.Attributes["Value"].Value);
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

            this.Borders.top.Load(element);
            this.Borders.left.Load(element);
            this.Borders.bottom.Load(element);
            this.Borders.right.Load(element);

           // EditorController.Instance.MainFormOfTheProject.AddNewEditorItemToList(this);
            

			// Load items
			node = element.SelectSingleNode("Items");
			if (node != null)
			{
				foreach(XmlNode itemNode in node.ChildNodes)
				{
					if (itemNode.Name == "Item")
					{
						LoadItem(itemNode);
					}
				}
			}


			// Load other child balloons
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
				}
			}
		}
		
		/// <summary>
		/// Load item from node
		/// </summary>
		/// <param name="itemNode"></param>
		private void LoadItem(XmlNode itemNode)
		{
			string itemType = "";
			if (itemNode.Attributes.Count > 0)
			{
				itemType = itemNode.Attributes["Type"].Value;
				EditorItem newItem = ReportPage.CreateItemFromType(itemType);
                //newItem.Parent = this;
				newItem.Load(itemNode);
                newItem.Parent = this;
				//Children.Add(newItem);               
			}
		}


		/// <summary>
		/// Save item
		/// </summary>
		/// <param name="txW"></param>
		public override void SaveItem(XmlDocument doc, XmlElement element)
		{
			// save base attributes and child nodes
			base.SaveItem(doc, element);
			
			// save Type
			XmlAttribute attr = doc.CreateAttribute("Type");
			attr.Value = this is StaticBalloon ? "Static" : "Dynamic";
			element.SetAttributeNode(attr);

			attr = doc.CreateAttribute("Version");
			attr.Value = "1.0";
			element.SetAttributeNode(attr);

			attr = doc.CreateAttribute("DataStream");
			attr.Value = this.SourceDataStream;
			element.SetAttributeNode(attr);


			XmlElement el = doc.CreateElement("AvailableOnEveryPage");
			attr = doc.CreateAttribute("Value");
			attr.Value = AvailableOnEveryPage.ToString();
			el.SetAttributeNode(attr);
			element.AppendChild(el);

            el = doc.CreateElement("FitToContent");
            attr = doc.CreateAttribute("Value");
            attr.Value = FitToContent.ToString();
            el.SetAttributeNode(attr);
            element.AppendChild(el);

			el = doc.CreateElement("FillingGeneratesNew");
			attr = doc.CreateAttribute("Value");
			attr.Value = this.FillingGeneratesNew.ToString();
			el.SetAttributeNode(attr);
			element.AppendChild(el);

			el = doc.CreateElement("FillCapacity");
			attr = doc.CreateAttribute("Value");
			attr.Value = this.FillCapacity.ToString();
			el.SetAttributeNode(attr);
			element.AppendChild(el);

			el = doc.CreateElement("CanGrow");
			attr = doc.CreateAttribute("Value");
			attr.Value = this.CanGrow.ToString();
			el.SetAttributeNode(attr);
			element.AppendChild(el);

            // FillColor
            el = doc.CreateElement("FillColor");
            attr = doc.CreateAttribute("R"); attr.Value = this.FillColor.R.ToString(); el.SetAttributeNode(attr);
            attr = doc.CreateAttribute("G"); attr.Value = this.FillColor.G.ToString(); el.SetAttributeNode(attr);
            attr = doc.CreateAttribute("B"); attr.Value = this.FillColor.B.ToString(); el.SetAttributeNode(attr);
            attr = doc.CreateAttribute("A"); attr.Value = this.FillColor.A.ToString(); el.SetAttributeNode(attr);
            element.AppendChild(el);

            this.Borders.top.SaveItem(doc, element);
            this.Borders.left.SaveItem(doc, element);
            this.Borders.bottom.SaveItem(doc, element);
            this.Borders.right.SaveItem(doc, element);

			// save children items and child balloons
			el = doc.CreateElement("Balloons");
			foreach(EditorItem item in this.Children)
			{
				if (item is Balloon)
				{
					item.Save(doc, el);
				}
			}			
			element.AppendChild(el);


			// save items
			el = doc.CreateElement("Items");
			foreach(EditorItem item in this.Children)
			{
				if (!(item is Balloon))
				{
					item.Save(doc, el);
				}
			}
			element.AppendChild(el);					
		}

        public Balloon sourceBallon = null;

        /// <summary>
        /// If this balloon should be used on all pages.
        /// </summary>
        [Browsable(true), DisplayName("Available On Every Page"), Description("If this balloon should be used on all pages.")]
        public bool AvailableOnEveryPage
        {
            get
            {
                return availableOnEveryPage;
            }
            set
            {
                availableOnEveryPage = value;
            }
        }

        /// <summary>
        /// If this balloon should be used on all pages.
        /// </summary>
        [Browsable(true), DisplayName("Fit To Content"), Description("Should balloon size be auto-changed to size of its content during generating process.")]
        public virtual bool FitToContent
        {
            get
            {
                return fitToContent;
            }
            set
            {
                fitToContent = value;
            }
        }

        /// <summary>
        /// If this balloon is filled with some other balloons then new balloon of this type will be generated.
        /// </summary>
        [Browsable(false), Description("If this balloon is filled with some other balloons then new balloon of this type will be generated.")]
        public bool FillingGeneratesNew
        {
            get
            {
                return fillingGeneratesNew;
            }
            set
            {
                fillingGeneratesNew = value;
            }
        }

        /// <summary>
        /// If more than this number of dynamic balloons is reached balloon will try to generate another one, or stop dynamic generation, 0 = infinite.
        /// </summary>
        [Browsable(true), DisplayName("Fill Capacity"), Description("If more than this number of dynamic balloons is reached balloon will try to generate another one, or stop dynamic generation, 0 = infinite.")]
        public int FillCapacity
        {
            get
            {
                return fillCapacity;
            }
            set
            {
                if (value < 0)
                {
                    fillCapacity = 0;
                }
                else
                {
                    fillCapacity = value;
                }
            }
        }

        /// <summary>
        /// Count number of dynamic child balloons
        /// </summary>
        private int DynamicChildBalloons
        {
            get
            {
                // count child balloons
                int childBalloons = 0;
                foreach (EditorItem item in this.Children)
                {
                    if (item is DynamicBalloon)
                    {
                        childBalloons++;
                    }
                }
                return childBalloons;
            }
        }

		private int ChildBalloons
		{
			get 
			{
				// count child balloons
				int childBalloons = 0;				
				foreach (EditorItem item in this.Children)
				{
					if (item is Balloon)
					{
						childBalloons++;
					}
				}
				return childBalloons;
			}
		}

        /// <summary>
        /// Is this balloon allowed to grow when there is no more place. Page is the only entity that cannot grow
        /// </summary>
        [Description("Is this balloon allowed to grow when there is no more place. Page is the only entity that cannot grow")]
        public virtual bool CanGrow
        {
            get
            {
				if (canGrow)
				{					
					if (this.FillCapacity <= 0 || DynamicChildBalloons <= this.FillCapacity)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}	
            }
            set
            {
                canGrow = value;
            }
        }
        		
		
        public bool GrowBalloon(Balloon generatedBalloon, float growDelta)
        {
            RectangleNormal rect1 = new RectangleNormal();
            RectangleNormal rect2 = new RectangleNormal();

            Debug.Assert(growDelta >= -Epsilon , "Grow Delta Should never be negative");

            // check what generate algorithm is used and do logic from that
            //if (generatedBalloon->generatingAlgorithm == GENERATING_ALGORITHM_RIGHT_BOTTOM)
            {
                // this means we can grow only on bottom. growDelta contains info how much we need to grow
                // 1. Grow Parent if can grow
                if(this.Parent != null)
                {
                    // check if parent needs to grow at all
                    // rect of parent
                    rect1.top = 0;
                    rect1.left = 0;
                    rect1.right = this.Parent.WidthInPixels;
                    rect1.bottom = this.Parent.HeightInPixels;

                    //desired rect after growth. Growth to allow generatedBalloon to fall in
                    rect2.left = this.positionRect.left;
                    rect2.top = this.positionRect.top;
                    rect2.bottom = (rect2.top + this.HeightInPixels + growDelta);
                    rect2.right = (rect2.left + this.WidthInPixels);

                    if(rect1.Contains(rect2))
                    {
						// check if we intersect some rectangle in rectMatrix and in case we do return false						
						foreach(RectangleNormal rect in this.Parent.rectMatrix)
						{
                            // if rect.top != rect2.top && rect.left != rect2.left - or if this is not the same rect which already has taken place
                            // and if it does insterest something return false
							if (!(System.Math.Abs(rect.top - rect2.top) < Epsilon &&
                                System.Math.Abs(rect.left - rect2.left) < Epsilon) &&
                                rect.Intersect(rect2))
							{
								return false;
							}
						}

                        // we don't need to grow parent just increase size of self				
                        this.HeightInPixels += growDelta;
                        this.containerRect.bottom += growDelta;
                    }
                    else
                    {
                        // can parent grow at all
                        if(this.CanGrow)
                        {
                            // grow parent					
                            if(this.Parent is Balloon)
                            {
                                Balloon parentBallon = (Balloon)this.Parent;
                                if(parentBallon.CanGrow && parentBallon.GrowBalloon(generatedBalloon, growDelta))
                                {
                                    this.HeightInPixels += growDelta;
                                    this.containerRect.bottom += growDelta;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            // cannot increase size return FALSE
                            return false;
                        }
                    }
                }
                else
                {
                    // no parent ... grow in height by required size.
                    this.HeightInPixels += growDelta;
                    this.containerRect.bottom += growDelta;
                }

                this.positionRect.bottom = rect2.bottom;
            }
            //if (generatedBalloon->generatingAlgorithm == GENERATING_ALGORITHM_BOTTOM_RIGHT)
            //{
            //    // this means we can grow only on left
            //    // TODO: Not implemented yet
            //}

            return true;
        }

        /// <summary>
        /// This will return real width of balloon considering its static ballon children
        /// If FitToContent is false it will return real WidthInPixels value
        /// </summary>
        /// <returns></returns>
        public float GetFitToContentWidthInPixels()
        {
            return this.WidthInPixels;
        }

        /// <summary>
        /// This will return real height of balloon considering its static ballon children
        /// </summary>
        /// <returns></returns>
        public float GetFitToContentHeightInPixels()
        {            
            float res = 0;
            float heightToAdd = 0;
           
            if (this.FitToContent)
            {
                // if it is empty and has no other child balloons then result should be its own Width
                if (this.ChildBalloons == 0)
                {
                    res = this.HeightInPixels;
                }
                else
                {
                    // count child static balloons and width should be the right most one
                    foreach (EditorItem item in this.Children)
                    {
                        if (item is StaticBalloon)
                        {
                            StaticBalloon sb = item as StaticBalloon;
                          
                            // handle docking to get this information
                            if (sb.DockPosition == DockingPosition.DOCK_BOTTOM)
                            {
                                // static docked balloons are just adding additional minimal hight 
                                heightToAdd += sb.HeightInPixels;                            
                            }
                            else
                            {
                                float tmpRes = sb.LocationInPixelsY + sb.GetFitToContentHeightInPixels();
                                if (tmpRes > res)
                                {
                                    res = tmpRes;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                // FitToContent is false, so just return HeightInPixels
                res = this.HeightInPixels;
            }
                       
            return res + heightToAdd;
        }

        /// <summary>
        /// Adds new child to this balloon and performs resize if necessary.
        ///  
        /// </summary>
        /// <param name="generatedBalloon">newly created balloon that needs to fit into this one</param>
        /// <param name="staticBalloonFlag">this flag is true if generated balloon should be threated as static. if true
        /// then it is put exactly where it should be. If this is true staticDockedBottomFlag is ignored</param>
        /// <param name="staticDockedBottomFlag">Flag that tells if this should be threated as static bottom docked ballooon. </param>
        /// <returns></returns>
        public override bool AddChild(Balloon generatedBalloon, bool staticBalloonFlag, bool staticDockedBottomFlag)
        {            
            RectangleNormal newRect = new RectangleNormal();

            if(staticBalloonFlag)
            {
                // add it to the list
                this.AddChildToList(generatedBalloon);
            }
            else
            {
				// if there is no room for another child here
				if (this.FillCapacity > 0 && (this.DynamicChildBalloons + 1 > this.FillCapacity) && !staticDockedBottomFlag)
				{
					return false;
				}

                if(!this.GetNewRect(generatedBalloon, ref newRect))
                {
                    if(this.CanGrow)
                    {
                        // we established that balloon need to grow to put this new one inside and determined ammount of grow
                        // we need to check if grow is allowed on parents side (maybe it is already taken on parents size and allocated in 
                        // rect matrix. this is checked before grow
                        if(this.GrowBalloon(generatedBalloon, generatedBalloon.HeightInPixels - (this.HeightInPixels - newRect.top)))
                        {
                            return this.AddChild(generatedBalloon, staticBalloonFlag, staticDockedBottomFlag);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (staticDockedBottomFlag && !this.FitToContent)
                    {
                        // in case this one is static docked bottom and we wont to put him on the bottom of this balloon
                        newRect.top = this.containerRect.bottom - (newRect.bottom - newRect.top);
                        newRect.bottom = this.containerRect.bottom;

                        generatedBalloon.WidthInPixels = newRect.right - newRect.left;
                        generatedBalloon.HeightInPixels = newRect.bottom - newRect.top;
                        generatedBalloon.positionRect.top = newRect.top;
                        generatedBalloon.positionRect.left = newRect.left;
                        generatedBalloon.containerRect = newRect;
                    }
                    else
                    {
                        generatedBalloon.WidthInPixels = newRect.right - newRect.left;
                        generatedBalloon.HeightInPixels = newRect.bottom - newRect.top;
                        generatedBalloon.positionRect.top = newRect.top;
                        generatedBalloon.positionRect.left = newRect.left;
                        generatedBalloon.containerRect = newRect;
                    }
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

        /// <summary>
        /// Display item
        /// </summary>
        /// <param name="gc"></param>
        public override void DisplayItem(Graphics gc)
        {
            using (SolidBrush brush = new SolidBrush(fillColor))
            {
                gc.FillRectangle(brush, 0, 0, (float)this.WidthInPixels, (float)this.HeightInPixels);
            }
        }

        public void DrawBorders(Graphics gc)
        {
            this.Borders.top.Draw(this, gc);
            this.Borders.left.Draw(this, gc);
            this.Borders.bottom.Draw(this, gc);
            this.Borders.right.Draw(this, gc);
        }
        
    }
}
