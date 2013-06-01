using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace AxiomCoders.PdfReports
{
	internal class AxiomLicenser: LicenseProvider
	{

		/// <summary>
		/// Get license from registry. If not expired and is valid then use it
		/// </summary>
		/// <param name="context"></param>
		/// <param name="type"></param>
		/// <param name="instance"></param>
		/// <param name="allowExceptions"></param>
		/// <returns></returns>
		public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
		{
			// Get license only in design mode
			if (context.UsageMode == LicenseUsageMode.Designtime)
			{
				// check and create license 
				AxiomLicense lic = new AxiomLicense();

				// Trial license
				System.Windows.Forms.MessageBox.Show("Please register ME!");
				return lic;
			}
			else 
			{
				return new AxiomLicense();
			}
		}
	}
}
