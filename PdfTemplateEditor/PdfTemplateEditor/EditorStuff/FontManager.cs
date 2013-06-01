using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Drawing;
using AxiomCoders.PdfTemplateEditor.EditorItems;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
    /// <summary>
    /// This manager singleton will make sure all fonts are saved in one place and later just reused
    /// </summary>
    internal class FontManager
    {
        /// <summary>
        /// Singleton
        /// </summary>
        private static FontManager instance = null;

        /// <summary>
        /// Singleton
        /// </summary>
        public static FontManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FontManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// List of fonts used 
        /// </summary>
        private List<EditorFont> fonts = new List<EditorFont>();

        /// <summary>
        /// Fonts 
        /// </summary>
        internal List<EditorFont> Fonts
        {
            get { return fonts; }           
        }
        
        /// <summary>
        /// Create <fonts> node and put all fonts here by analyzing BaseTextItems in parentItem
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="element"></param>
        public void Save(EditorItem parentItem, System.Xml.XmlDocument doc, System.Xml.XmlElement element)
        {
            XmlElement el = doc.CreateElement("Fonts");

            List<BaseTextItems> baseTextItems = new List<BaseTextItems>();
            EditorFont editorFont;
            GetBaseTextItems(parentItem, baseTextItems);

            this.fonts.Clear();
            foreach (BaseTextItems textItem in baseTextItems)
            {
                // create and save each font still not created. We first check if it exists so not to save duplicates
                // currently font is unique if has same name and style
                editorFont = FindFont(textItem.Font.Name, textItem.Font.Style);
                if (editorFont == null)
                {
                    editorFont = CreateFont(textItem.Font);
                    editorFont.Save(doc, el);
                }
                // so we later know how to load it
                textItem.FontSaveId = editorFont.SaveID;
            }
            element.AppendChild(el);
        }

        /// <summary>
        /// Load all fonts. Element must be <Fonts> node 
        /// </summary>
        /// <param name="element"></param>
        public void Load(System.Xml.XmlNode element)
        {
            if (element.Name == "Fonts")
            {
                XmlNodeList fontNodes = element.SelectNodes("Font");
                foreach (XmlNode fontNode in fontNodes)
                {
                    EditorFont newFont = new EditorFont();
                    newFont.Load(fontNode);
                    this.fonts.Add(newFont);
                }
            }
        }     

        /// <summary>
        /// Get all base test items recursively
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="outBaseTextItems"></param>
        private void GetBaseTextItems(EditorItem parent, List<BaseTextItems> outBaseTextItems)
        {
            if (parent is BaseTextItems)
            {
                outBaseTextItems.Add((BaseTextItems)parent);
            }

            foreach (EditorItem item in parent.Children)
            {
                GetBaseTextItems(item, outBaseTextItems);
            }
        }

        /// <summary>
        /// Find font by SaveId. This is used by BaseTextItems
        /// </summary>
        /// <param name="saveId"></param>
        /// <returns></returns>
        public EditorFont FindFont(int saveId)
        {
            foreach (EditorFont editorFont in fonts)
            {
                if (editorFont.SaveID == saveId)
                {
                    return editorFont;
                }
            }
            return null;
        }       

        /// <summary>
        /// Find if we alrady have this font so not to create new one
        /// </summary>
        /// <param name="fontName"></param>
        /// <returns>null if nothing is found</returns>
        private EditorFont FindFont(string fontName, FontStyle fontStyle)
        {
            foreach (EditorFont font in fonts)
            {
                if (font.Font.Name == fontName && font.Font.Style == fontStyle)
                {
                    return font;
                }
            }
            return null;
        }

        /// <summary>
        /// Create font. This should be used for creation instead of EditorFont constructor
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public EditorFont CreateFont(Font font)
        {
            EditorFont newFont;
            newFont = FindFont(font.Name, font.Style);
            if (newFont == null)
            {
                newFont = new EditorFont(font);
                this.fonts.Add(newFont);
            }            
            return newFont;
        }

     
    }
}
