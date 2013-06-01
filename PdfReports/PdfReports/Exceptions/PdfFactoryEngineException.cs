using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomCoders.PdfReports.Exceptions
{
    /// <summary>
    /// Exception of this type is thrown when ther is some issue in PdfFactory engine. Either it cannot be initialized, geneation fails, etc
    /// </summary>
    public class PdfFactoryEngineException: Exception
    {
        public PdfFactoryEngineException(string message)
            : base(message)
        {
        }

        public PdfFactoryEngineException(string message, Exception innerException): base(message, innerException)
        {
        }
    }
}
