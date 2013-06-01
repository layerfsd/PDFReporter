using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;



namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
    public class DataStream : SaveLoadMechanism
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public DataStream()
        {

        }		


        private string name = "";

		/// <summary>
		/// Name of data stream
		/// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }



        private List<Column> columns = new List<Column>();
        public List<Column> Columns
        {
            get { return columns; }
        }

        public void Save(XmlDocument doc, XmlElement element)        
		{
			XmlElement el = doc.CreateElement("DataStream");
			XmlAttribute attr = doc.CreateAttribute("Name");
			attr.Value = this.Name;
			el.SetAttributeNode(attr);

			XmlElement el2 = doc.CreateElement("Columns");
			foreach(Column col in this.Columns)
			{
				col.Save(doc, el2);
			}
			el.AppendChild(el2);
			element.AppendChild(el);


			/*
            txW.WriteStartElement("DataStream");
            txW.WriteAttributeString("Name", Name);
            txW.WriteRaw("\n");
            txW.WriteStartElement("Columns");
            txW.WriteRaw("\n");
            foreach (Column tmpCol in Columns)
            {
                tmpCol.Save(txW);
            }
            txW.WriteEndElement();
            txW.WriteRaw("\n");
            txW.WriteEndElement();
            txW.WriteRaw("\n");*/
        }

        public void Load(System.Xml.XmlNode element)
        {
			this.Name = element.Attributes["Name"].Value;

			XmlNodeList nodes = element.SelectNodes("Columns");
			if (nodes.Count == 1)
			{
				foreach(XmlNode node in nodes[0].ChildNodes)
				{
					if (node.Name == "Column") 
					{
						Column tmpCol = new Column(node.Attributes["Name"].Value, node.Attributes["Type"].Value);
						tmpCol.Load(node);
						Columns.Add(tmpCol);
					}
				}
			}

			/*while (txR.Read())
			{
				switch (txR.Name)
				{
					case "Columns":
						{
							// Load columns
							XmlTextReader columnReader = txR.ReadSubtree();
							while(columnReader.Read())
							{
								if (columnReader.Name == "Column") 
								{
									Column tmpCol = new Column(columnReader.GetAttribute("Name"), columnReader.GetAttribute("Type"));
									tmpCol.Load(columnReader.ReadSubtree());
									Columns.Add(tmpCol);
								}
							}
							break;						
						}						
				}
			}*/

           /*Name = txR.GetAttribute(0);
            while (txR.Name != "Columns")
            {
                txR.Read();
            }
            txR.Read();

            while(txR.Name != "Columns")
            {
                if(txR.Name == "Column")
                {
                    Column tmpCol = new Column();
                    tmpCol.Load(txR);
                    Columns.Add(tmpCol);
                }
                txR.Read();
            }
            txR.Read();
            txR.Read();*/
        }
    }
}
