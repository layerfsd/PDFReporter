using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using AxiomCoders.SharedNet20.Forms;
using AxiomCoders.PdfTemplateEditor.Common;
using AxiomCoders.PdfTemplateEditor.EditorStuff;

namespace AxiomCoders.PdfTemplateEditor.Forms
{
	public partial class NewProjectForm : formBase
	{
        private MeasureUnits currentMeasureUnit = MeasureUnits.pixel;
        private bool customEnabled = false;

		public NewProjectForm()
		{
			InitializeComponent();
            this.projectResolution.Text = "72";
            this.projectResolutionProperty.SelectedIndex = 0;
            this.ProjectWidth = 800.0F;
            this.projectWidthProperty.SelectedIndex = 3;
            this.ProjectHeight = 600.0F;
            this.projectHeightProperty.SelectedIndex = 3;
            this.projectName.Text = "Unnamed";
            this.projectTitle.Text = "Untitled";
            this.projectAuthor.Text = "Unnamed";
            this.projectSubject.Text = "No Subject";
            this.projectSizeProperty.SelectedIndex = 7;
		}

        public float ProjectWidth
        {
            get { return (float)System.Convert.ToDouble(projectWidth.Text); }
            set { projectWidth.Text = value.ToString(); }
        }

        public float ProjectHeight
        {
            get { return (float)System.Convert.ToDouble(projectHeight.Text); }
            set { projectHeight.Text = value.ToString(); }
        }

        public string ProjectName
        {
            get { return projectName.Text; }
        }

        public string ProjectTitle
        {
            get { return projectTitle.Text; }
        }

        public string ProjectAuthor
        {
            get { return projectAuthor.Text; }
        }

        public string ProjectSubject
        {
            get { return projectSubject.Text; }
        }


        public float ProjectResolution
        {
            get { return (float)Convert.ToDouble(projectResolution.Text); }
        }

        public MeasureUnits MeasureUnit
        {
            get { return currentMeasureUnit; }
        }



        /// <summary>
        /// This should check if the values are valid.
        /// </summary>
        /// <returns></returns>
        private bool CheckValues()
        {
            if(projectName.Text == "")
            {
                MessageBox.Show("Can't create project without name...", "New...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(projectWidth.Text == "" || Convert.ToInt16(projectWidth.Text) == 0)
            {
                MessageBox.Show("Width must be entered, and can't be null.", "New...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(projectHeight.Text == "" || Convert.ToInt16(projectHeight.Text) == 0)
            {
                MessageBox.Show("Height must be entered, and can't be null.", "New...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(projectResolution.Text == "" || Convert.ToInt16(projectResolution.Text) == 0)
            {
                MessageBox.Show("Resolution must be entered, and can't be null.", "New...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }



        private void btnOk_Click(object sender, EventArgs e)
        {
            if(CheckValues())
            {
                UnitsManager.Instance.Resolution = ProjectResolution;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // Letter, Legal, Tabloid, 640x480, 800x600, 1024x768, A4, A3, B5, B4, B3
        private void projectSizeProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            customEnabled = false;
            switch (projectSizeProperty.SelectedIndex)
            {
                case 1:
                    projectWidthProperty.SelectedIndex = 0;
                    projectHeightProperty.SelectedIndex = 0;
                    currentMeasureUnit = MeasureUnits.inch;
                    ProjectWidth = 8.5F;
                    ProjectHeight = 11.0F;
            	    break;
                case 2:
                    projectWidthProperty.SelectedIndex = 0;
                    projectHeightProperty.SelectedIndex = 0;
                    currentMeasureUnit = MeasureUnits.inch;
                    ProjectWidth = 8.5F;
                    ProjectHeight = 14.0F;
                    break;
                case 3:
                    projectWidthProperty.SelectedIndex = 0;
                    projectHeightProperty.SelectedIndex = 0;
                    currentMeasureUnit = MeasureUnits.inch;
                    ProjectWidth = 11.0F;
                    ProjectHeight = 17.0F;
                    break;
                case 4:
                    projectWidthProperty.SelectedIndex = 3;
                    projectHeightProperty.SelectedIndex = 3;
                    currentMeasureUnit = MeasureUnits.pixel;
                    ProjectWidth = 640.0F;
                    ProjectHeight = 480.0F;
                    break;
                case 5:
                    projectWidthProperty.SelectedIndex = 3;
                    projectHeightProperty.SelectedIndex = 3;
                    currentMeasureUnit = MeasureUnits.pixel;
                    ProjectWidth = 800.0F;
                    ProjectHeight = 600.0F;
                    break;
                case 6:
                    projectWidthProperty.SelectedIndex = 3;
                    projectHeightProperty.SelectedIndex = 3;
                    currentMeasureUnit = MeasureUnits.pixel;
                    ProjectWidth = 1024.0F;
                    ProjectHeight = 768.0F;
                    break;
                case 7:
                    projectWidthProperty.SelectedIndex = 2;
                    projectHeightProperty.SelectedIndex = 2;
                    currentMeasureUnit = MeasureUnits.mm;
                    ProjectWidth = 210.0F;
                    ProjectHeight = 297.0F;
                    break;
                case 8:
                    projectWidthProperty.SelectedIndex = 2;
                    projectHeightProperty.SelectedIndex = 2;
                    currentMeasureUnit = MeasureUnits.mm;
                    ProjectWidth = 297.0F;
                    ProjectHeight = 420.0F;
                    break;
                case 9:
                    projectWidthProperty.SelectedIndex = 2;
                    projectHeightProperty.SelectedIndex = 2;
                    currentMeasureUnit = MeasureUnits.mm;
                    ProjectWidth = 176.0F;
                    ProjectHeight = 250.0F;
                    break;
                case 10:
                    projectWidthProperty.SelectedIndex = 2;
                    projectHeightProperty.SelectedIndex = 2;
                    currentMeasureUnit = MeasureUnits.mm;
                    ProjectWidth = 250.0F;
                    ProjectHeight = 353.0F;
                    break;
                case 11:
                    projectWidthProperty.SelectedIndex = 2;
                    projectHeightProperty.SelectedIndex = 2;
                    currentMeasureUnit = MeasureUnits.mm;
                    ProjectWidth = 353.0F;
                    ProjectHeight = 500.0F;
                    break;
            }
        }

        private void projectWidthProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            projectHeightProperty.SelectedIndex = projectWidthProperty.SelectedIndex;
            UpdateSize();
        }

        private void projectHeightProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            projectWidthProperty.SelectedIndex = projectHeightProperty.SelectedIndex;
            UpdateSize();
        }


        private void projectResolutionProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (projectResolutionProperty.SelectedIndex)
            {
                case 0:
                    UnitsManager.Instance.ResolutionMeasure = ResolutionMeasure.PixelsPerInch;
                    UnitsManager.Instance.Resolution = (float)System.Convert.ToDouble(projectResolution.Text);
            	    break;
                case 1:
                    UnitsManager.Instance.ResolutionMeasure = ResolutionMeasure.PixelsPerCm;
                    UnitsManager.Instance.Resolution = (float)System.Convert.ToDouble(projectResolution.Text);
                    break;
            }
        }

        //inches, cm, mm, pixels
        private void UpdateSize()
        {
            switch (projectWidthProperty.SelectedIndex)
            {
                case 0:
                    ProjectWidth = (float)System.Math.Round(UnitsManager.Instance.ConvertUnit(ProjectWidth, currentMeasureUnit, MeasureUnits.inch),2);
                    ProjectHeight = (float)System.Math.Round(UnitsManager.Instance.ConvertUnit(ProjectHeight, currentMeasureUnit, MeasureUnits.inch),2);
                    currentMeasureUnit = MeasureUnits.inch;
            	    break;
                case 1:
                    ProjectWidth = (float)System.Math.Round(UnitsManager.Instance.ConvertUnit(ProjectWidth, currentMeasureUnit, MeasureUnits.cm),2);
                    ProjectHeight = (float)System.Math.Round(UnitsManager.Instance.ConvertUnit(ProjectHeight, currentMeasureUnit, MeasureUnits.cm),2);
                    currentMeasureUnit = MeasureUnits.cm;
                    break;
                case 2:
                    ProjectWidth = (float)System.Math.Round(UnitsManager.Instance.ConvertUnit(ProjectWidth, currentMeasureUnit, MeasureUnits.mm),2);
                    ProjectHeight = (float)System.Math.Round(UnitsManager.Instance.ConvertUnit(ProjectHeight, currentMeasureUnit, MeasureUnits.mm), 2);
                    currentMeasureUnit = MeasureUnits.mm;
                    break;
                case 3:
                    ProjectWidth = (float)System.Math.Round(UnitsManager.Instance.ConvertUnit(ProjectWidth, currentMeasureUnit, MeasureUnits.pixel),0);
                    ProjectHeight = (float)System.Math.Round(UnitsManager.Instance.ConvertUnit(ProjectHeight, currentMeasureUnit, MeasureUnits.pixel),0);
                    currentMeasureUnit = MeasureUnits.pixel;
                    break;
            }
        }

        private void projectWidth_TextChanged(object sender, EventArgs e)
        {
            if(customEnabled)
            {
                projectSizeProperty.SelectedIndex = 0; //Set size to custom
            }
        }

        private void projectHeight_TextChanged(object sender, EventArgs e)
        {
            if(customEnabled)
            {
                projectSizeProperty.SelectedIndex = 0; //Set size to custom
            }
        }

        private void projectWidth_MouseClick(object sender, MouseEventArgs e)
        {
            customEnabled = true;
        }

        private void projectHeight_MouseClick(object sender, MouseEventArgs e)
        {
            customEnabled = true;
        }


        private void projectWidth_GotFocus(object sender, EventArgs e)
        {
            customEnabled = true;
        }

        private void projectHeight_GotFocus(object sender, EventArgs e)
        {
            customEnabled = true;
        }

        private void projectResolution_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if(Convert.ToInt32(projectResolution.Text) < 1)
                {
                    projectResolution.Text = "1";
                }
            }
            catch (System.Exception ex)
            {
            	
            }
        }
	}
}