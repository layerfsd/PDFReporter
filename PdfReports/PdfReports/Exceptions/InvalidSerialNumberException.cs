using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomCoders.PdfReports.Exceptions
{
    public class InvalidSerialNumberException: Exception
    {
        // Default constructor
        public InvalidSerialNumberException(): base(Strings.InvalidSerialNumber)
        {            
        }
    }
}
