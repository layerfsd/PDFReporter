using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
	/// <summary>
	/// Main singleton used for saving/loading everything in editor
	/// </summary>
	public class SaveLoadManager
	{
		/// <summary>
		/// Singleton variable
		/// </summary>
		private static SaveLoadManager instance;
		
		/// <summary>
		/// Singleton 
		/// </summary>
		public static SaveLoadManager Instance
		{
		  get 
		  {
		     if (instance == null)
			 {
				instance = new SaveLoadManager();
			 }
			 return instance;			 
		  }
		}
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public SaveLoadManager()
		{

		}

		/// <summary>
		/// This will save entity to file. You can add project here or whatever you want to save
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="fileName"></param>
		public void Save(SaveLoadMechanism entity, string fileName)
		{

		}

		/// <summary>
		/// This will initialize loading of project
		/// </summary>
		/// <param name="project"></param>
		/// <param name="fileName"></param>
		public void LoadProject(EditorProject project, string fileName)
		{

		}

		/// <summary>
		/// Loads options 
		/// </summary>
		/// <param name="options"></param>
		/// <param name="fileName"></param>
		public void LoadOptions(EditorOptions options, string fileName)
		{

		}
	}
}
