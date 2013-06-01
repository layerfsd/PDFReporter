using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using AxiomCoders.PdfTemplateEditor.EditorStuff;
using AxiomCoders.PdfTemplateEditor.Common;
using AxiomCoders.SharedNet20.Forms;
using AxiomCoders.SharedNet20;

namespace AxiomCoders.PdfTemplateEditor.Forms
{
    public partial class OptionsForm : formBase
    {
		public enum OptionsTab
		{
			General,
			Rulers,
			Grid
		}

        protected override void OnHelpButtonClicked(CancelEventArgs e)
        {
            
        }

        public OptionsForm()
        {
            InitializeComponent();

        }
		        
        public void OpenDialog(OptionsTab defaultTab)
        {
            if(defaultTab == OptionsTab.General)
            {
                tabControl.SelectTab("tabPage_General");
            }
            if(defaultTab == OptionsTab.Rulers)
            {
                tabControl.SelectTab("tabPage_Rulers");
            }
            if(defaultTab == OptionsTab.Grid)
            {
                tabControl.SelectTab("tabPage_Grid");
            }

			GetOptionsToGUI();
            this.ShowDialog();
        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            this.applyButton.Enabled = true;
        }



        private void OkApplyButton_Click(object sender, EventArgs e)
        {
            if(!SetOptions())
            {
                return;
            }
			// save options
			AppOptions.Instance.Save();
			if (EditorController.Instance.EditorViewer != null)
			{
				EditorController.Instance.EditorViewer.RefreshView();
			}			
            if(EditorController.Instance.EditorProject != null)
            {
                switch (AppOptions.Instance.Unit)
                {
                    case 0:
                        EditorController.Instance.EditorProject.ReportPage.MeasureUnit = MeasureUnits.inch;
                	    break;
                    case 1:
                        EditorController.Instance.EditorProject.ReportPage.MeasureUnit = MeasureUnits.mm;
                        break;
                    case 2:
                        EditorController.Instance.EditorProject.ReportPage.MeasureUnit = MeasureUnits.cm;
                        break;
                    case 3:
                        EditorController.Instance.EditorProject.ReportPage.MeasureUnit = MeasureUnits.pixel;
                        break;
                }
                EditorController.Instance.ProjectSaved = false;
            }
            Close();
        }



        private void ShowGr_CheckedChanged(object sender, EventArgs e)
        {
            //EditorController.Instance.Grid.ShowGrid = showGridCheckBox.Checked;
            //EditorController.Instance.EditorViewer.RefreshView();
            this.applyButton.Enabled = true;
        }



        private void ShowMiLines_CheckedChanged(object sender, EventArgs e)
        {
            //EditorController.Instance.Grid.ShowMinorLines = showMinorLinesCheckBox.Checked;
            //EditorController.Instance.EditorViewer.RefreshView();
            this.applyButton.Enabled = true;
        }



        private void MjColor_Click(object sender, EventArgs e)
        {
            ColorDialog colDlg = new ColorDialog();
            colDlg.Color = gridColorPicker.BackColor;
            colDlg.AllowFullOpen = true;
            colDlg.FullOpen = true;
            if(DialogResult.OK == colDlg.ShowDialog())
            {
                gridColorPicker.BackColor = colDlg.Color;
                this.applyButton.Enabled = true;
            }
        }


		/// <summary>
		/// Get options to GUI
		/// </summary>
		private void GetOptionsToGUI()
		{
			//Tab 1
			autoSaveIntervalTextBox.Text = AppOptions.Instance.AutoSaveInterval.ToString();
			autoSaveEnableCheckBox.Checked = AppOptions.Instance.AutoSave;
			showBalloonBordersCheckBox.Checked = AppOptions.Instance.ShowBalloonBorders;
            txtNumOffPreviewItems.Text = AppOptions.Instance.NumOffPreviewItems.ToString();

			//Tab 2
			unitComboBox.SelectedIndex = AppOptions.Instance.Unit;

			//Tab 3
			styleComboBox.SelectedIndex = AppOptions.Instance.GridLineStyle;
			gridUnitComboBox.SelectedIndex = AppOptions.Instance.GridUnit;
			gridValueTextBox.Text = AppOptions.Instance.GridInterval.ToString();
			showGridCheckBox.Checked = AppOptions.Instance.ShowGrid;
			showMinorLinesCheckBox.Checked = AppOptions.Instance.ShowGridMinorLines;
			snapToGridCheckBox.Checked = AppOptions.Instance.GridSnap;
			subsValueTextBox.Text = AppOptions.Instance.GridSubdivisions.ToString();
			gridColorPicker.BackColor = AppOptions.Instance.GridColor;

            applyButton.Enabled = false;
		}


        private bool CheckValues()
        {
            if(autoSaveIntervalTextBox.Text == "")
            {
                MessageBox.Show("Please enter auto save interval...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(gridValueTextBox.Text == "")
            {
                MessageBox.Show("Please enter grid interval...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(subsValueTextBox.Text == "")
            {
                MessageBox.Show("Please enter grid subdivisions...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }


		/// <summary>
		/// This will save options from gui to appoptions
		/// </summary>
		private bool SetOptions()
		{
            if(!CheckValues())
            {
                return false;
            }
			//Set general properties
			AppOptions.Instance.AutoSave = autoSaveEnableCheckBox.Checked;
			AppOptions.Instance.AutoSaveInterval = System.Convert.ToInt16(autoSaveIntervalTextBox.Text);
			AppOptions.Instance.ShowBalloonBorders = showBalloonBordersCheckBox.Checked;
            AppOptions.Instance.NumOffPreviewItems = Convert.ToInt32(txtNumOffPreviewItems.Text);

			//Set unit properties
			AppOptions.Instance.Unit = unitComboBox.SelectedIndex;

			//Set grid properties
			AppOptions.Instance.GridLineStyle = styleComboBox.SelectedIndex;
			AppOptions.Instance.GridUnit = gridUnitComboBox.SelectedIndex;
			AppOptions.Instance.GridInterval = (float)System.Convert.ToDouble(gridValueTextBox.Text);
			AppOptions.Instance.GridSubdivisions = (float)System.Convert.ToDouble(subsValueTextBox.Text);
			AppOptions.Instance.ShowGrid = showGridCheckBox.Checked;
			AppOptions.Instance.ShowGridMinorLines = showMinorLinesCheckBox.Checked;
			AppOptions.Instance.GridSnap = snapToGridCheckBox.Checked;
			AppOptions.Instance.GridColor = gridColorPicker.BackColor;

            return true;
		}

        private void applyButton_Click(object sender, EventArgs e)
        {
            if(!SetOptions())
            {
                return;
            }
			// save options
			AppOptions.Instance.Save();

			if (EditorController.Instance.EditorViewer != null)
			{
				EditorController.Instance.EditorViewer.RefreshView();
			}
            this.applyButton.Enabled = false;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void styleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.applyButton.Enabled = true;
        }

        private void gridUnitComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.applyButton.Enabled = true;
        }

        private void gridValueTextBox_TextChanged(object sender, EventArgs e)
        {
            this.applyButton.Enabled = true;
        }

        private void subsValueTextBox_TextChanged(object sender, EventArgs e)
        {
            this.applyButton.Enabled = true;
        }

        private void snapToGridCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.applyButton.Enabled = true;
        }

        private void autoSaveIntervalTextBox_TextChanged(object sender, EventArgs e)
        {
            this.applyButton.Enabled = true;
        }

        private void autoSaveEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.applyButton.Enabled = true;
        }

        private void showBalloonBordersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.applyButton.Enabled = true;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            switch (tabControl.SelectedIndex)
            {
                case 0:                    
                    ProductHelp.OpenHelpTopic(this, "PDF%20Reports%20Template%20Editor_Preferences_General.html");
            	    break;
                case 1:
                    ProductHelp.OpenHelpTopic(this, "PDF%20Reports%20Template%20Editor_Preferences_RulersAndUnits.html");
                    break;
                case 2:
                    ProductHelp.OpenHelpTopic(this, "PDF%20Reports%20Template%20Editor_Preferences_GiudsAndGrid.html");
                    break;
            }
        }

        private void txtNumOffPreviewItems_TextChanged(object sender, EventArgs e)
        {
            if(txtNumOffPreviewItems.Text == "")
            {
                txtNumOffPreviewItems.Text = "1";
            }

            int tmpVal = Convert.ToInt32(txtNumOffPreviewItems.Text);
            if(tmpVal < 1)
                txtNumOffPreviewItems.Text = "1";
            else if(tmpVal > 100)
                txtNumOffPreviewItems.Text = "100";

            applyButton.Enabled = true;
        }
    }
}