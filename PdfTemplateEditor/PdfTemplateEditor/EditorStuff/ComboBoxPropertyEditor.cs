using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.ComponentModel;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
    [Serializable]
    public class ComboBoxPropertyEditor : UITypeEditor
    {
        private ListBox Box1 = new ListBox();
        IWindowsFormsEditorService srvc;

        public static string[] ItemList;

        public ComboBoxPropertyEditor()
        {
            Box1.BorderStyle = BorderStyle.None;
            Box1.Click += new EventHandler(this.Box1_Click);            
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            try
            {
                Box1.Items.Clear();
                if(ItemList != null)
                {
                    Box1.Items.AddRange(ItemList);
                }else
                {
                    Box1.Items.Clear();
                }
                Box1.Height = Box1.PreferredHeight;
                srvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if(srvc != null)
                {
                    srvc.DropDownControl(Box1);
                    ItemList = null;
                    return Box1.SelectedItem.ToString();
                }
            }
            catch 
            {
            }
            
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        private void Box1_Click(object sender, EventArgs e)
        {
            srvc.CloseDropDown();
        }
    }
}
