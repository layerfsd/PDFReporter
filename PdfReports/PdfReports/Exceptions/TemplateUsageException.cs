using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomCoders.PdfReports.Exceptions
{
    /// <summary>
    /// Exceptions of this type is thrown when there are some problems with template file. Cannot be accessed, read, there are some problems
    /// in template file it self, etc.
    /// </summary>
    public class TemplateUsageException: Exception
    {
        public TemplateUsageException(string message)
            : base(message)
        {
        }

        public TemplateUsageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
