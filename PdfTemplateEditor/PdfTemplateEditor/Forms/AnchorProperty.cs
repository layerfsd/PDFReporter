using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using AxiomCoders.PdfTemplateEditor.EditorStuff;


namespace AxiomCoders.PdfTemplateEditor.Forms
{
    public partial class AnchorProperty : Form
    {
        private bool tempTop = false;
        private bool tempLeft = false;
        private bool tempBottom = false;
        private bool tempRight = false;
        private Anchor tmpAnchor = null;

        public AnchorProperty()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            tmpAnchor.TopAnchor = tempTop;
            tmpAnchor.LeftAnchor = tempLeft;
            tmpAnchor.Bottomanchor = tempBottom;
            tmpAnchor.RightAnchor = tempRight;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public DialogResult Show(Anchor anchor)
        {
            if(anchor == null) btnCancel_Click(this, EventArgs.Empty);

            tmpAnchor = anchor;
            tempTop = tmpAnchor.TopAnchor;
            chkBoxTop.Checked = tempTop;
            tempLeft = tmpAnchor.LeftAnchor;
            chkBoxLeft.Checked = tempLeft;
            tempRight = tmpAnchor.RightAnchor;
            chkBoxRight.Checked = tempRight;
            tempBottom = tmpAnchor.Bottomanchor;
            chkBoxBottom.Checked = tempBottom;

            return this.ShowDialog();
        }

        private void chkBoxTop_CheckedChanged(object sender, EventArgs e)
        {
            tempTop = chkBoxTop.Checked;
        }

        private void chkBoxBottom_CheckedChanged(object sender, EventArgs e)
        {
            tempBottom = chkBoxBottom.Checked;
        }

        private void chkBoxLeft_CheckedChanged(object sender, EventArgs e)
        {
            tempLeft = chkBoxLeft.Checked;
        }

        private void chkBoxRight_CheckedChanged(object sender, EventArgs e)
        {
            tempRight = chkBoxRight.Checked;
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            chkBoxTop.Checked = tempTop = true;
            chkBoxLeft.Checked = tempLeft = true;
            chkBoxBottom.Checked = tempBottom = true;
            chkBoxRight.Checked = tempRight = true;
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            chkBoxTop.Checked = tempTop = false;
            chkBoxLeft.Checked = tempLeft = false;
            chkBoxBottom.Checked = tempBottom = false;
            chkBoxRight.Checked = tempRight = false;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {

        }
    }
}