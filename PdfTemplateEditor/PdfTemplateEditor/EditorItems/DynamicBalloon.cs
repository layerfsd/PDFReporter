using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;

using AxiomCoders.PdfTemplateEditor.EditorStuff;
using AxiomCoders.PdfTemplateEditor.Common;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;

namespace AxiomCoders.PdfTemplateEditor.EditorItems
{
	[System.Reflection.Obfuscation(Exclude = true)]
    class DynamicBalloon : Balloon, EditorToolBarPlugin
    {
        public DynamicBalloon()
        {

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

        public override bool CanGrow
        {
            get
            {
                return base.CanGrow;
            }
            set
            {
                if (!value && FitToContent)
                {
                    MessageBox.Show("Cannot set \"Can Grow\" property to false while keeping \"Fit To Content\" property true. Make sure \"Fit To Content\" property is set to false first.");                    

                }
                else
                {
                    base.CanGrow = value;
                }                
            }
        }

        /// <summary>
        /// Setting this to true must first include canGrow on true
        /// </summary>
        public override bool FitToContent
        {
            get
            {
                return base.FitToContent;
            }
            set
            {
                if (value && !CanGrow)
                {
                    MessageBox.Show("Cannot set \"Fit To Content\" property to true while \"Can Grow\" property is set to false. Make sure \"Can Grow\" property is set to true first");
                }
                else
                {
                    base.FitToContent = value;
                }
            }
        }

     	public override void DisplayItem(Graphics gc)
		{
            base.DisplayItem(gc);

			using (Pen borderPen = new Pen(Color.IndianRed))
			{
				borderPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
				borderPen.DashCap = System.Drawing.Drawing2D.DashCap.Round;
				borderPen.Width = 1.0f / this.ViewMatrix.Elements[0];
              
                if(AppOptions.Instance.ShowBalloonBorders && !this.IsSelected)
                {
                    gc.DrawRectangle(borderPen, 0, 0, (float)this.WidthInPixels, (float)this.HeightInPixels);
                }
				
			}
		}       


        #region EditorToolBarPlugin Members

        void EditorToolBarPlugin.AddToToolStrip(ToolStrip toolStrip, ToolStripGroup group)
        {
            //ToolStripButton tbbNew = new ToolStripButton("Dynamic Balloon");
            //tbbNew.Image = AxiomCoders.PdfTemplateEditor.Properties.Resources.DynamicBalloon;
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
