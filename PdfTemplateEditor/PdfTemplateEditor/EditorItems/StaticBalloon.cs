using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using AxiomCoders.PdfTemplateEditor.EditorStuff;
using AxiomCoders.PdfTemplateEditor.Common;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.Xml;

namespace AxiomCoders.PdfTemplateEditor.EditorItems
{
	[System.Reflection.Obfuscation(Exclude = true)]
    public class StaticBalloon : Balloon, EditorToolBarPlugin
    {
        
        public StaticBalloon()
        {            
        }

        /// <summary>
        /// Returns true if there is any balloon on the same level that is before this one and has top docked property set
        /// </summary>
        public bool HasDynamicTopBallons
        {
            get
            {
                if (Parent != null)
                {
                    bool result = false;
                    foreach (EditorItem item in Parent.Children)
                    {
                        if (item is DynamicBalloon && item.DockPosition == DockingPosition.DOCK_TOP)
                        {
                            return true;
                        }

                        if (item == this)
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
        }

        
        public override void SaveItem(XmlDocument doc, XmlElement element)
        {
			// make base element
			XmlElement el = doc.CreateElement("Balloon");
			
			// save base class
			base.SaveItem(doc, el);

			// save my properties

			element.AppendChild(el);
   
        }

        public override void Load(System.Xml.XmlNode element)
        {
			base.Load(element);

        }


        [Browsable(false)]
        public override bool CanGrow
        {
            get
            {
                return false;
            }
            set
            {
                base.CanGrow = false;
            }
        }

        /// <summary>
        /// Fit To content property on static balloon is always false.
        /// </summary>
        [Browsable(false)]
        public override bool FitToContent
        {
            get
            {
                return false;
            }
            set
            {
                base.FitToContent = false;
            }
        }


		public override void DisplayItem(Graphics gc)
		{
            base.DisplayItem(gc);

			using (Pen borderPen = new Pen(Color.LightSkyBlue))
			{
				borderPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
				borderPen.DashCap = System.Drawing.Drawing2D.DashCap.Round;
				borderPen.Width = 1.0f / this.ViewMatrix.Elements[0];
                //if (Disabled)
                //{
                //    borderPen.Brush = Brushes.LightGray;
                //    gc.DrawRectangle(borderPen, 0, 0, (float)this.WidthInPixels, (float)this.HeightInPixels);
                //}
                //else
                //{
                if(AppOptions.Instance.ShowBalloonBorders && !this.IsSelected)
                {
                    gc.DrawRectangle(borderPen, 0, 0, (float)this.WidthInPixels, (float)this.HeightInPixels);
                }
				//}
			}
			
		}


      

        #region EditorToolBarPlugin Members

        void EditorToolBarPlugin.AddToToolStrip(ToolStrip toolStrip, ToolStripGroup group)
        {
            //ToolStripButton tbbNew = new ToolStripButton("Static Balloon");
            //tbbNew.Image = AxiomCoders.PdfTemplateEditor.Properties.Resources.StaticBalloon;
            //tbbNew.DisplayStyle = ToolStripItemDisplayStyle.Image;
            //toolStrip.Items.Add(tbbNew);
            //tbbNew.Click += new EventHandler(tbbNew_Click);
        }

        void tbbNew_Click(object sender, EventArgs e)
        {
            //ToolStripButton tbbButton = sender as ToolStripButton;
            //tbbButton.Checked = !tbbButton.Checked;

            //if (tbbButton.Checked)
            //{
            //    EditorController.Instance.TbbCheckedForCreate = tbbButton;
            //    EditorController.Instance.EditorItemSelectedForCreation = this.GetType();
            //}
            //else
            //{
            //    EditorController.Instance.EditorItemSelectedForCreation = null;
            //}
        }

        #endregion
    }
}
