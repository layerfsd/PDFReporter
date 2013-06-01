using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AxiomCoders.SharedNet20.Forms;
using AxiomCoders.SharedNet20;

using System.Globalization;
using System.Threading;

namespace AxiomCoders.PdfTemplateEditor
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
			Application.SetCompatibleTextRenderingDefault(false);

            CultureInfo set_culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            set_culture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = set_culture;

			Application.Run(new MainForm(args));
		}

		static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{			
			formExceptionHandler.ShowException(e.Exception);
		}
	}
}