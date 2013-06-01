using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using AxiomCoders.PdfTemplateEditor.EditorItems;
using AxiomCoders.PdfTemplateEditor.Controls;
using AxiomCoders.SharedNet20;

namespace AxiomCoders.PdfTemplateEditor.Forms
{
    public partial class BorderPropertyForm : Form
    {
        private Balloon sourceBalloon;

        public BorderPropertyForm()
        {
            InitializeComponent();

            // There is no other styles right now so set it to solid lines
            cmbBoxBottomStyle.SelectedIndex = 0;
            cmbBoxLeftStyle.SelectedIndex = 0;
            cmbBoxRightStyle.SelectedIndex = 0;
            cmbBoxTopStyle.SelectedIndex = 0;
        }

        public Balloon SourceBalloon
        {
            set
            {
                sourceBalloon = value;
            }
        }

        private void SetColor(Button outButton)
        {
            ColorDlg colDlg = new ColorDlg();
            
            if(DialogResult.OK == colDlg.ShowDlg(outButton.BackColor))
            {
                outButton.BackColor = colDlg.SelectedColor;
            }
        }

        private void buttonTopColor_Click(object sender, EventArgs e)
        {
            SetColor(buttonTopColor);
        }

        private void buttonLeftColor_Click(object sender, EventArgs e)
        {
            SetColor(buttonLeftColor);
        }

        private void buttonBottomColor_Click(object sender, EventArgs e)
        {
            SetColor(buttonBottomColor);
        }

        private void buttonRightColor_Click(object sender, EventArgs e)
        {
            SetColor(buttonRightColor);
        }

        private void UpdateCheckState()
        {
            if(chkBoxEnableTop.Checked && chkBoxEnableBottom.Checked && chkBoxEnableLeft.Checked && chkBoxEnableRight.Checked)
                chkAll.Checked = true;
            else
                chkAll.Checked = false;
        }


        private void BorderPropertyForm_Shown(object sender, EventArgs e)
        {
            //Get top border properties
            numTexBoxTopWidth.Text = sourceBalloon.Borders.top.LineWidth.ToString();
            buttonTopColor.BackColor = sourceBalloon.Borders.top.LineColor;
            chkBoxEnableTop.Checked = sourceBalloon.Borders.top.Enabled;

            //Get Left border properties
            numTexBoxLeftWidth.Text = sourceBalloon.Borders.left.LineWidth.ToString();
            buttonLeftColor.BackColor = sourceBalloon.Borders.left.LineColor;
            chkBoxEnableLeft.Checked = sourceBalloon.Borders.left.Enabled;

            //Get bottom border properties
            numTexBoxBottomWidth.Text = sourceBalloon.Borders.bottom.LineWidth.ToString();
            buttonBottomColor.BackColor = sourceBalloon.Borders.bottom.LineColor;
            chkBoxEnableBottom.Checked = sourceBalloon.Borders.bottom.Enabled;

            //Get right border properties
            numTexBoxRightWidth.Text = sourceBalloon.Borders.right.LineWidth.ToString();
            buttonRightColor.BackColor = sourceBalloon.Borders.right.LineColor;
            chkBoxEnableRight.Checked = sourceBalloon.Borders.right.Enabled;

            lablBalloonName.Text = sourceBalloon.Name;
            UpdateCheckState();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if(numTexBoxTopWidth.Text == "" || Convert.ToDouble(numTexBoxTopWidth.Text) <= 0)
            {
                MessageBox.Show("Value of Top-Border isn't correct.\nValue is empty or less equal to 0!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(numTexBoxRightWidth.Text == "" || Convert.ToDouble(numTexBoxRightWidth.Text) <= 0)
            {
                MessageBox.Show("Value of Right-Border isn't correct.\nValue is empty or less equal to 0!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(numTexBoxLeftWidth.Text == "" || Convert.ToDouble(numTexBoxLeftWidth.Text) <= 0)
            {
                MessageBox.Show("Value of Left-Border isn't correct.\nValue is empty or less equal to 0!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(numTexBoxBottomWidth.Text == "" || Convert.ToDouble(numTexBoxBottomWidth.Text) <= 0)
            {
                MessageBox.Show("Value of Bottom-Border isn't correct.\nValue is empty or less equal to 0!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Set top border props
            sourceBalloon.Borders.top.LineWidth = (float)Convert.ToDouble(numTexBoxTopWidth.Text);
            sourceBalloon.Borders.top.LineColor = buttonTopColor.BackColor;
            sourceBalloon.Borders.top.Enabled = chkBoxEnableTop.Checked;

            //Set left border props
            sourceBalloon.Borders.left.LineWidth = (float)Convert.ToDouble(numTexBoxLeftWidth.Text);
            sourceBalloon.Borders.left.LineColor = buttonLeftColor.BackColor;
            sourceBalloon.Borders.left.Enabled = chkBoxEnableLeft.Checked;

            //Set bottom border props
            sourceBalloon.Borders.bottom.LineWidth = (float)Convert.ToDouble(numTexBoxBottomWidth.Text);
            sourceBalloon.Borders.bottom.LineColor = buttonBottomColor.BackColor;
            sourceBalloon.Borders.bottom.Enabled = chkBoxEnableBottom.Checked;

            //Set right border props
            sourceBalloon.Borders.right.LineWidth = (float)Convert.ToDouble(numTexBoxRightWidth.Text);
            sourceBalloon.Borders.right.LineColor = buttonRightColor.BackColor;
            sourceBalloon.Borders.right.Enabled = chkBoxEnableRight.Checked;

			DialogResult = DialogResult.OK;
            this.Close();
        }

        private void chkBoxEnableTop_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxEnableTop.Checked)
            {
                numTexBoxTopWidth.Enabled = true;
                buttonTopColor.Enabled = true;
            }
            else
            {
                numTexBoxTopWidth.Enabled = false;
                buttonTopColor.Enabled = false;
            }
        }

        private void chkBoxEnableLeft_CheckedChanged(object sender, EventArgs e)
        {
            if(chkBoxEnableLeft.Checked)
            {
                numTexBoxLeftWidth.Enabled = true;
                buttonLeftColor.Enabled = true;
            }
            else
            {
                numTexBoxLeftWidth.Enabled = false;
                buttonLeftColor.Enabled = false;
            }
        }

        private void chkBoxEnableBottom_CheckedChanged(object sender, EventArgs e)
        {
            if(chkBoxEnableBottom.Checked)
            {
                numTexBoxBottomWidth.Enabled = true;
                buttonBottomColor.Enabled = true;
            }
            else
            {
                numTexBoxBottomWidth.Enabled = false;
                buttonBottomColor.Enabled = false;
            }
        }

        private void chkBoxEnableRight_CheckedChanged(object sender, EventArgs e)
        {
            if(chkBoxEnableRight.Checked)
            {
                numTexBoxRightWidth.Enabled = true;
                buttonRightColor.Enabled = true;
            }
            else
            {
                numTexBoxRightWidth.Enabled = false;
                buttonRightColor.Enabled = false;
            }
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            ProductHelp.OpenHelpTopic(this, "PDF%20Reports%20Template%20Editor_Border%20Properties.html");
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox tmp = (CheckBox)sender;
            if(tmp.CheckState == CheckState.Checked)
            {
                chkBoxEnableBottom.Checked = true;
                chkBoxEnableLeft.Checked = true;
                chkBoxEnableRight.Checked = true;
                chkBoxEnableTop.Checked = true;
            }else{
                chkBoxEnableBottom.Checked = false;
                chkBoxEnableLeft.Checked = false;
                chkBoxEnableRight.Checked = false;
                chkBoxEnableTop.Checked = false;
            }
        }
    }
}