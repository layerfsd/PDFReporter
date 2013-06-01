using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using AxiomCoders.PdfTemplateEditor.EditorItems;
using AxiomCoders.SharedNet20;

namespace AxiomCoders.PdfTemplateEditor.Controls
{
    public partial class GradientColorPickerForm : Form
    {
        public GradientColorPickerForm()
        {
            InitializeComponent();
        }


        private void btnColor1_Click(object sender, EventArgs e)
        {
            using (ColorDlg dlg = new ColorDlg())
            {
                //dlg.AllowFullOpen = true;
                //dlg.FullOpen = true;
                
                if(dlg.ShowDlg(pnlColor1.BackColor) == DialogResult.OK)
                {
                    pnlColor1.BackColor = dlg.SelectedColor;
                    pnlPreview.GradientColors = new GradientDefinition(dlg.SelectedColor, pnlPreview.GradientColors.Color2, GradientColors.Point1, GradientColors.Point2, GradientColors.GradientType);
                }
            }
        }

        private void btnColor2_Click(object sender, EventArgs e)
        {
            using(ColorDlg dlg = new ColorDlg())
            {
                //dlg.AllowFullOpen = true;
                //dlg.FullOpen = true;

                if(dlg.ShowDlg(pnlColor2.BackColor) == DialogResult.OK)
                {
                    pnlColor2.BackColor = dlg.SelectedColor;
                    pnlPreview.GradientColors = new GradientDefinition(pnlPreview.GradientColors.Color1, dlg.SelectedColor, GradientColors.Point1, GradientColors.Point2, GradientColors.GradientType);
                }
            }
        }

        public GradientDefinition GradientColors
        {
            set
            {
                pnlPreview.GradientColors = value;
                pnlColor1.BackColor = GradientColors.Color1;
                pnlColor2.BackColor = GradientColors.Color2;
            }
            get
            {
                return pnlPreview.GradientColors;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            pnlPreview.GradientColors = new GradientDefinition(pnlColor1.BackColor, pnlColor2.BackColor, GradientColors.Point1, GradientColors.Point2, GradientColors.GradientType);
            base.OnLoad(e);
        }

		private void btnOK_Click(object sender, EventArgs e)
		{

		}

        private void btnHelp_Click(object sender, EventArgs e)
        {
            ProductHelp.OpenHelpTopic(this, "PDF%20Reports%20Template%20Editor_GradientDefinition.htm");
        }

    }
}