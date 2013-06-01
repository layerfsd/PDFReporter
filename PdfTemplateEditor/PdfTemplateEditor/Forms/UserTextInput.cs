using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AxiomCoders.SharedNet20.Forms;

namespace AxiomCoders.PdfTemplateEditor.Forms
{
    public partial class UserTextInput : formBase
    {
        public UserTextInput()
        {
            InitializeComponent();
            //Canceld = false;
        }


        //public bool Canceld = false;


        public string ReturnText
        {
            get { return ResultText.Text; }
            set { ResultText.Text = value; }
        }


        public string TextBoxTitle
        {
            set { textBoxLabel.Text = value; }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if(ResultText.Text == "")
            {
                MessageBox.Show("Can't return empty string!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                //Canceld = false;
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            //Canceld = true;
            this.Close();
        }
    }
}