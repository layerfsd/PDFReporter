using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Xml;

using AxiomCoders.PdfTemplateEditor.EditorStuff;
using AxiomCoders.PdfTemplateEditor.Common;

namespace AxiomCoders.PdfTemplateEditor.EditorItems
{
	[System.Reflection.Obfuscation(Exclude = true)]
	public class DummyItem: BaseTextItems
	{
		public override void SaveItem(XmlDocument doc, XmlElement element)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override void Load(System.Xml.XmlNode txR)
		{
			throw new Exception("The method or operation is not implemented.");
		}
				

		private string text = "DummyText";
		[Browsable(true), Description("Text to display in control"), Category("Standard")]
		public string Text
		{
			get { return text; }
			set
			{ 
				text = value;
				// Update size of item when text is changed				
				needToUpdateSize = true;
			}
		}



		/// <summary>
		/// Overridden just to hide this in property page
		/// </summary>
		[Browsable(false)]
		public override float WidthInUnits
		{
			get
			{
				return base.WidthInUnits;
			}
			set
			{
				base.WidthInUnits = value;
			}
		}

		/// <summary>
		/// Overridden just to hide this in property page
		/// </summary>
		[Browsable(false)]
		public override float HeightInUnits
		{
			get
			{
				return base.HeightInUnits;
			}
			set
			{
				base.HeightInUnits = value;
			}
		}

		


		public override string VisibleText
		{
			get
			{
				return this.Text;
			}
		}		
	}
}
