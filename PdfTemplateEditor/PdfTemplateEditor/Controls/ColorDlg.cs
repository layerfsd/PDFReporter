using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using AxiomCoders.PdfTemplateEditor.Controls;

namespace AxiomCoders.PdfTemplateEditor.Controls
{
    public struct CMYKColor
    {
        public float CVal;
        public float MVal;
        public float YVal;
        public float KVal;
    };

    public partial class ColorDlg : Form
    {
        public CMYKColor cmykColor;

        public ColorDlg()
        {
            InitializeComponent();

            colorTable.SelectedIndexChanged += new EventHandler(this.colorTable_SelectedIndexChanged);
            colorWheel.SelectedColorChanged += new EventHandler(this.colorWheel_SelectedColorChanged);
            colorSlider2.SelectedValueChanged += new EventHandler(this.colorSlider2_SelectedValueChanged);
            colorList.SelectedIndexChanged += new EventHandler(this.colorList_SelectedIndexChanged);
        }

        private bool firstSet;
        private HSLColor selHSLColor;

        public Color SelectedColor
        {
            get{ return selHSLColor.Color; }
            set { selHSLColor = new HSLColor(value); }
        }

        private int DefaultColorHue;

        
        private void tabControl1_Selected(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex == 0)
            {
                if(colorTable.ColorExist(SelectedColor))
                {
                    colorTable.SelectedItem = SelectedColor;
                }else{
                    colorTable.SetCustomColor(SelectedColor);
                }
            }

            if(tabControl1.SelectedIndex == 1)
            {
                colorWheel.SelectedHSLColor = selHSLColor;
                colorSlider2.SelectedHSLColor = selHSLColor;
                colorSlider2.Percent = (float)selHSLColor.Lightness;
            }

            if(tabControl1.SelectedIndex == 3)
            {
                DefaultColorHue = (int)selHSLColor.Hue;
                UpdateFineTune();
            }
        }



        private void colorTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            selHSLColor.Color = colorTable.SelectedItem;
            selHSLColor.Hue = colorTable.SelectedItem.GetHue();
            selHSLColor.Saturation = colorTable.SelectedItem.GetSaturation();
            selHSLColor.Lightness = colorTable.SelectedItem.GetBrightness();            

            UpdateCMYKValues();
            lblFinalColorValue.BackColor = colorTable.SelectedItem;
        }



        private void colorWheel_SelectedColorChanged(object sender, EventArgs e)
        {
            selHSLColor = colorWheel.SelectedHSLColor;
            //selHSLColor.Lightness = colorSlider2.Percent;
            colorSlider2.SelectedHSLColor = selHSLColor;

            UpdateCMYKValues();
            lblFinalColorValue.BackColor = SelectedColor;
        }



        private void colorSlider2_SelectedValueChanged(object sender, EventArgs e)
        {
            selHSLColor = colorSlider2.SelectedHSLColor;
            colorWheel.SelectedHSLColor = selHSLColor;

            UpdateCMYKValues();
            lblFinalColorValue.BackColor = SelectedColor;
        }



        private void colorList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Color tmpColor = (Color)colorList.SelectedItem;
            selHSLColor.Color = tmpColor;
            selHSLColor.Hue = tmpColor.GetHue();
            selHSLColor.Saturation = tmpColor.GetSaturation();
            selHSLColor.Lightness = tmpColor.GetBrightness();

            UpdateCMYKValues();
            lblFinalColorValue.BackColor = selHSLColor.Color;
        }



        private void UpdateFineTune()
        {
            UpdateCMYKValues();            

            lblFinalColorValue.BackColor = selHSLColor.Color;
        }



        private void UpdateCMYKValues()
        {
            float i = 1.0f;
            float Cyan, Yellow, Magenta, Black;
            Cyan = 1.0f - (float)(selHSLColor.Color.R / 255f);
            Magenta = 1.0f - (float)(selHSLColor.Color.G / 255f);
            Yellow = 1.0f - (float)(selHSLColor.Color.B / 255f);
            Black = Math.Min(Math.Min(Cyan, Magenta),Yellow);

            Cyan = Math.Min(1, Math.Max(0, Cyan - Black));
            Magenta = Math.Min(1, Math.Max(0, Magenta - Black));
            Yellow = Math.Min(1, Math.Max(0, Yellow - Black));
            Black = Math.Min(1, Math.Max(0, Black));
            

            int CVal = (int)(Cyan * 100.0f);
            int MVal = (int)(Magenta * 100.0f);
            int YVal = (int)(Yellow * 100.0f);
            int KVal = (int)(Black * 100.0f);

            txtCValue.Text = CVal.ToString();
            txtMValue.Text = MVal.ToString();
            txtYValue.Text = YVal.ToString();
            txtKValue.Text = KVal.ToString();
        }




        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }



        private void btnOk_Click(object sender, EventArgs e)
        {
            if(txtCValue.Text == "")
                UpdateCMYKValues();
            cmykColor.CVal = (float)(Convert.ToDouble(txtCValue.Text));
            cmykColor.MVal = (float)(Convert.ToDouble(txtMValue.Text));
            cmykColor.YVal = (float)(Convert.ToDouble(txtYValue.Text));
            cmykColor.KVal = (float)(Convert.ToDouble(txtCValue.Text));
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        public DialogResult ShowDlg(Color baseColor)
        {
            if(baseColor != null)
            {
                selHSLColor.Hue = baseColor.GetHue();
                selHSLColor.Saturation = baseColor.GetSaturation();
                selHSLColor.Lightness = baseColor.GetBrightness();
                selHSLColor.Color = baseColor;

                lblFinalColorValue.BackColor = baseColor;
            }

            return ShowDialog();
        }
    }
}