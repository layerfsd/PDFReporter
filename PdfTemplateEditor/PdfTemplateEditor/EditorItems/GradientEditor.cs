using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using AxiomCoders.PdfTemplateEditor.Controls;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AxiomCoders.PdfTemplateEditor.EditorItems
{
    class GradientEditor : UITypeEditor
    {
        GradientColorPickerForm editorDialog;
        private object editorValue;

        public GradientEditor()
        {
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetFocus();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr SetFocus(HandleRef hWnd);
 


        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            editorValue = value;
            if ((provider != null) && (((IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService))) != null))
            {
                if (this.editorDialog == null)
                {
                    this.editorDialog = new GradientColorPickerForm();
                }

                GradientDefinition colors = editorValue as GradientDefinition;
                if (colors != null)
                {
                    editorDialog.GradientColors = colors;
                }
                IntPtr focus = GetFocus();
                try
                {
                    if (editorDialog.ShowDialog() == DialogResult.OK)
                    {
                        editorValue = editorDialog.GradientColors;
                    }
                }
                finally
                {
                    if (focus != IntPtr.Zero)
                    {
                        SetFocus(new HandleRef(null, focus));
                    }
                }
            }
            value = editorValue;
            editorValue = null;
            return value;

        }

        
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }


    }
}
