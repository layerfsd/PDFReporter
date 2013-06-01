using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
    interface DynamicEditorItemInterface
    {
        string SourceDataStream
        {
            get;
            set;
        }

        string SourceColumn
        {
            get;
            set;
        }
    }
}
