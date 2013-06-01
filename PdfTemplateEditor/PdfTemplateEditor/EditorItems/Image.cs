using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;



using AxiomCoders.PdfTemplateEditor.EditorStuff;
using AxiomCoders.PdfTemplateEditor.Common;
using System.Xml;
using System.IO;



namespace AxiomCoders.PdfTemplateEditor.EditorItems
{
    /// <summary>
    /// Used for open dialog and customization of images
    /// </summary>
    [Serializable, System.Reflection.Obfuscation(Exclude=true)]
    public class ImageNameEditor : UITypeEditor
    {
        OpenFileDialog editorDialog;
        public ImageNameEditor()
        {

        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (this.editorDialog == null)
            {
                this.editorDialog = new OpenFileDialog();
                this.editorDialog.Filter = "All picture files...|*.jpg;*.bmp;*.gif;*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|PNG|*.png";
                editorDialog.RestoreDirectory = true;
                this.editorDialog.ShowDialog();
            }

            value = editorDialog.FileName;
            editorDialog = null;
            return value;// base.EditValue(context, provider, value);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
    

    /// <summary>
    /// Main image item
    /// </summary>
    public class ImageItem : EditorItem, EditorToolBarPlugin
    {
        private string imageName;
        private System.Drawing.Image pictureForDisplay = AxiomCoders.PdfTemplateEditor.Properties.Resources.NoImage;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public ImageItem()
        {
			this.WidthInUnits = 3;
			this.HeightInUnits = 3;
        }

        /// <summary>
        /// Save image
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="element"></param>
        public override void SaveItem(XmlDocument doc, XmlElement element)
        {
			XmlElement el = doc.CreateElement("Item");
			base.SaveItem(doc, el);

			XmlAttribute attr = doc.CreateAttribute("Type");
			attr.Value = "StaticImage";
			el.SetAttributeNode(attr);

			attr = doc.CreateAttribute("Version");
			attr.Value = "1.0";
			el.SetAttributeNode(attr);


			XmlElement el2 = doc.CreateElement("Src");
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

			element.AppendChild(el);           
        }

        /// <summary>
        /// Image data
        /// </summary>
        private byte[] imageData; 

        /// <summary>
        /// Load image from template file
        /// </summary>
        /// <param name="element"></param>
        public override void Load(System.Xml.XmlNode element)
        {
			base.Load(element);           

			XmlNode node = element.SelectSingleNode("Src");
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

        }
        
        /// <summary>
        /// Image name used
        /// </summary>
        [Browsable(true), DisplayName("Image Name"), Description("Browse for image that you want to insert..."), Category("Standard"), EditorAttribute(typeof(ImageNameEditor), typeof(UITypeEditor))]
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
            }
            get
            {
                return imageName;
            }
        }

        /// <summary>
        /// Display image
        /// </summary>
        /// <param name="gc"></param>
		public override void DisplayItem(Graphics gc)
		{
			if (IsSelected)
			{
				if (pictureForDisplay != null)
				{
					gc.DrawImage(pictureForDisplay, 0, 0, (float)this.WidthInPixels, (float)this.HeightInPixels);
				}
				//gc.DrawRectangle(Pens.Red, 0, 0, (float)this.WidthInPixels, (float)this.HeightInPixels);
			}
			else
			{
				if (pictureForDisplay != null)
				{
					gc.DrawImage(pictureForDisplay, 0, 0, (float)this.WidthInPixels, (float)this.HeightInPixels);
				}
				else
				{
                    //if (Disabled)
                    //{
                    //    gc.DrawRectangle(Pens.LightGray, 0, 0, (float)this.WidthInPixels, (float)this.HeightInPixels);
                    //}
                    //else
                    //{
						gc.DrawRectangle(Pens.Black, 0, 0, (float)this.WidthInPixels, (float)this.HeightInPixels);
					//}
				}
			}
		}
    

        #region EditorToolBarPlugin Members

        void EditorToolBarPlugin.AddToToolStrip(ToolStrip toolStrip, ToolStripGroup group)
        {
            //ToolStripButton tbbNew = new ToolStripButton("Image");
            //tbbNew.Image = AxiomCoders.PdfTemplateEditor.Properties.Resources.picture;
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
