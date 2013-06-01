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
    public partial class projectInformation : Form
    {
        public projectInformation()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            EditorController.Instance.EditorProject.ReportPage.Name = projectName.Text;
            EditorController.Instance.EditorProject.Title = projectTitle.Text;
            EditorController.Instance.EditorProject.Author = projectAuthor.Text;
            EditorController.Instance.EditorProject.Subject = projectSubject.Text;

            EditorController.Instance.ProjectSaved = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public void ShowInformationDialog()
        {
            projectName.Text = EditorController.Instance.EditorProject.ReportPage.Name;
            projectTitle.Text = EditorController.Instance.EditorProject.Title;
            projectAuthor.Text = EditorController.Instance.EditorProject.Author;
            projectSubject.Text = EditorController.Instance.EditorProject.Subject;
            this.ShowDialog();
        }
    }
}