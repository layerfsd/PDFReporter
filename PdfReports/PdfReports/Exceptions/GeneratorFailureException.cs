using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomCoders.PdfReports.Exceptions
{
    /// <summary>
    /// Thrown when generating process failed
    /// </summary>
    public class GeneratorFailureException: Exception
    {
        public GeneratorFailureException(): base(Strings.FailedGeneratingPdf)
        {        
        }
    }
}
