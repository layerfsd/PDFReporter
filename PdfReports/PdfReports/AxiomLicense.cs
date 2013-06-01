using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace AxiomCoders.PdfReports
{
	internal class AxiomLicense: License
	{

		public override void Dispose()
		{
			
		}

		private string licenseKey = null;
		public override string LicenseKey
		{
			get { return licenseKey; }
		}
	}
}
