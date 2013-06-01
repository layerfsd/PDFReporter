using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomCoders.PdfTemplateEditor.Common
{
	/// <summary>
	/// Log class used for loggin purposes	
	/// </summary>
	internal static class Loger
	{
		/// <summary>
		/// This will delete log file is already exists
		/// </summary>
		public static void ReInitialize()
		{
#if DEBUG
			System.IO.File.Delete("pdflog.txt");
#endif
		}
		
		public static void LogMessage(string message)
		{
#if DEBUG
			System.IO.File.AppendAllText("pdflog.txt", string.Format("{0}: {1}\n", System.DateTime.Now.ToString(), message));
#endif
		}
	}
}
