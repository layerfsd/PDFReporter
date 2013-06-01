using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
    public class Column : SaveLoadMechanism
    {
        public Column()
        {
        }

		public Column(string name, string type)
		{
			this.name = name;
			this.type = type;
		}


        private string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string type = "string";
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public void Save(XmlDocument doc, XmlElement element)
        {
			XmlElement el = doc.CreateElement("Column");
			XmlAttribute attr = doc.CreateAttribute("Name");
			attr.Value = this.Name;
			el.SetAttributeNode(attr);

			attr = doc.CreateAttribute("Type");
			attr.Value = "string";
			el.SetAttributeNode(attr);

			element.AppendChild(el);

            /*txW.WriteStartElement("Column");
            txW.WriteAttributeString("Name", Name);
            txW.WriteAttributeString("Type", "string");
            txW.WriteEndElement();
            txW.WriteRaw("\n");*/
        }


        public void Load(System.Xml.XmlNode txR)
        {         
        }
    }

    
}
