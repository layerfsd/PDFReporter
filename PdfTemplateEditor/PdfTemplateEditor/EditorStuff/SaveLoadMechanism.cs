using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
	/// <summary>
	/// interface items need to implement in order to save/load 
	/// </summary>
	public interface SaveLoadMechanism
	{
		/// <summary>
		/// Make saving to this node
		/// </summary>
		/// <param name="node"></param>
		void Save(XmlDocument doc, XmlElement element);

		/// <summary>
		/// Load data from this node
		/// </summary>
		/// <param name="node"></param>
		void Load(XmlNode element);
	}
}
