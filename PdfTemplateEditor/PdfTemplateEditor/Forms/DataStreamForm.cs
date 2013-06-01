using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using AxiomCoders.SharedNet20.Forms;
using AxiomCoders.PdfTemplateEditor.EditorStuff;
using AxiomCoders.SharedNet20;


namespace AxiomCoders.PdfTemplateEditor.Forms
{
    public partial class DataStreamForm : formBase
	{
        private List<DataStream> tempDataStreams = new List<DataStream>();
        private List<renamedItems> tempRenamed = new List<renamedItems>();

        protected override void OnHelpRequested(HelpEventArgs hevent)
        {
            
        }

        public DataStreamForm()
        {
            InitializeComponent();
        }


        private void DataStreamForm_Shown(object sender, EventArgs e)
        {
            UpdateToTempData();
            RefreshTreeItems("");
        }


        private void UpdateToTempData()
        {
            foreach (DataStream tmpData in EditorController.Instance.EditorProject.DataStreams)
            {
                DataStream tmpDataStream = new DataStream();
                tmpDataStream.Name = tmpData.Name;
                foreach(Column tmpColumn in tmpData.Columns)
                {
                    Column tmpColumnData = new Column();
                    tmpColumnData.Name = tmpColumn.Name;
                    tmpDataStream.Columns.Add(tmpColumnData);
                }
                tempDataStreams.Add(tmpDataStream);
            }
        }


        private void UpdateFromTempData()
        {
            EditorController.Instance.EditorProject.DataStreams.Clear();
            foreach(DataStream tmpData in tempDataStreams)
            {
                DataStream tmpDataStream = new DataStream();
                tmpDataStream.Name = tmpData.Name;
                foreach(Column tmpColumn in tmpData.Columns)
                {
                    Column tmpColumnData = new Column();
                    tmpColumnData.Name = tmpColumn.Name;
                    tmpDataStream.Columns.Add(tmpColumnData);
                }
                EditorController.Instance.EditorProject.DataStreams.Add(tmpDataStream);
            }

            foreach(renamedItems renItem in tempRenamed)
            {
                foreach(EditorItem item in EditorController.Instance.EditorProject.Children)
                {
                    RenameDataAndColumn(item, renItem);
                }
            }
             
        }

        private void RenameDataAndColumn(EditorItem newItem, renamedItems renItem)
        {
            string tmpColumnString;

            foreach(EditorItem item in newItem.Children)
            {
                RenameDataAndColumn(item, renItem);
                if(item is DynamicEditorItemInterface)
                {
                    DynamicEditorItemInterface tmp = (DynamicEditorItemInterface)item;
                    if(renItem.type == 0)
                    {
                        if(tmp.SourceDataStream == renItem.befoure)
                        {
                            tmpColumnString = tmp.SourceColumn;
                            tmp.SourceDataStream = renItem.after;
                            tmp.SourceColumn = tmpColumnString;
                        }
                    }else if(renItem.type == 1)
                    {
                        if(tmp.SourceColumn == renItem.befoure)
                        {
                            tmp.SourceColumn = renItem.after;
                        }
                    }
                }
            }
        }



        private void Add_DataStream(string name)
        {
            DataStream tmpDS = new DataStream();
            tmpDS.Name = name;
            //EditorController.Instance.EditorProject.DataStreams.Add(tmpDS);
            tempDataStreams.Add(tmpDS);
            RefreshTreeItems(name);
        }


        private void Add_Column(string dataStreamParent, string name)
        {
            foreach(DataStream tmpParent in tempDataStreams)
            {
                if(tmpParent.Name == dataStreamParent)
                {
                    Column tmpColumn = new Column();
                    tmpColumn.Name = name;
                    tmpParent.Columns.Add(tmpColumn);
                    RefreshTreeItems(tmpParent.Name);
                    return;
                }
            }
        }



        private void Remove_DataStream(string name)
        {
            foreach(DataStream tmpDS in tempDataStreams)
            {
                if (tmpDS.Name == name)
                {
                    tempDataStreams.Remove(tmpDS);
                    RefreshTreeItems("");
                    return;
                }
            }
        }


        private void Remove_Column(string name)
        {
            foreach(DataStream tmpDS in tempDataStreams)
            {
                foreach (Column tmpCol in tmpDS.Columns)
                {
                    if (tmpCol.Name == name)
                    {
                        tmpDS.Columns.Remove(tmpCol);
                        RefreshTreeItems("");
                        return;
                    }
                }
            }
        }



        private void RefreshTreeItems(string nameToSelect)
        {
            DataStreamColections.Nodes.Clear();
            foreach(DataStream tmpData in tempDataStreams)
            {
                TreeNode parentNode = DataStreamColections.Nodes.Add(tmpData.Name);
                if (nameToSelect == tmpData.Name)
                {
                    DataStreamColections.SelectedNode = parentNode;
                    DataStreamColections_AfterSelect(null, null);
                }
                foreach (Column tmpCol in tmpData.Columns)
                {
                    TreeNode tmpNode = parentNode.Nodes.Add(tmpCol.Name);
                }
            }
            DataStreamColections.ExpandAll();
        }

        /// <summary>
        /// Adds data stream to root tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDataStream_Click(object sender, EventArgs e)
        {
            UserTextInput txtInput = new UserTextInput();
            txtInput.Text = "Add data stream";
            txtInput.TextBoxTitle = "Enter name...";
            //txtInput.ShowDialog();

            if(txtInput.ShowDialog() == DialogResult.OK)
            {
                Add_DataStream(txtInput.ReturnText);
            }
        }


        /// <summary>
        /// Adds column to specific selected data stream
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddColumn_Click(object sender, EventArgs e)
        {
            TreeNode parentNode = DataStreamColections.SelectedNode;
            if(parentNode.Level != 0)
            {
                return;
            }
            UserTextInput txtInput = new UserTextInput();
            txtInput.Text = "Add column";
            txtInput.TextBoxTitle = "Enter name...";
            //txtInput.ShowDialog();

            if(txtInput.ShowDialog() == DialogResult.OK)
            {
                Add_Column(parentNode.Text, txtInput.ReturnText);
            }
        }


        /// <summary>
        /// Pure visual contest, enables or disables some buttons...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataStreamColections_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (DataStreamColections.SelectedNode == null)
            {
                RemoveDataStream.Enabled = false;
                AddColumn.Enabled = false;
                RemoveColumn.Enabled = false;
                RenameColumn.Enabled = false;
                RenameDataStream.Enabled = false;
                ColumnTypeDrop.Enabled = false;
                return;
            }
            switch (DataStreamColections.SelectedNode.Level)
            {
                case 0: 
                    AddColumn.Enabled = true;
                    RemoveColumn.Enabled = false;
                    RenameColumn.Enabled = false;
                    ColumnTypeDrop.Enabled = false;
                    RenameDataStream.Enabled = true;
                    RemoveDataStream.Enabled = true;
                    break;
                case 1:
                    AddColumn.Enabled = false;
                    RemoveColumn.Enabled = true;
                    RenameColumn.Enabled = true;
                    ColumnTypeDrop.Enabled = true;
                    RemoveDataStream.Enabled = false;
                    RenameDataStream.Enabled = false;
                    break;
            }
        }


        /// <summary>
        /// Removes selected data stream
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveDataStream_Click(object sender, EventArgs e)
        {
            if(DataStreamColections.SelectedNode == null)
            {
                MessageBox.Show("You must select Data Stream", "Remove Data stream...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(DataStreamColections.SelectedNode.Level == 0)
            {
                if(DialogResult.Yes == MessageBox.Show("You want to delete Data Stream: "+DataStreamColections.SelectedNode.Text,"Remove Data Stream...", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    //DataStreamColections.Nodes.Remove(DataStreamColections.SelectedNode);
                    Remove_DataStream(DataStreamColections.SelectedNode.Text);
                    DataStreamColections_AfterSelect(null, null);
                }
            }
        }


        /// <summary>
        /// Removes selected column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveColumn_Click(object sender, EventArgs e)
        {
            if(DataStreamColections.SelectedNode == null)
            {
                MessageBox.Show("You must select Column", "Remove Column...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(DataStreamColections.SelectedNode.Level == 1)
            {
                if(DialogResult.Yes == MessageBox.Show("You want to delete Column: " + DataStreamColections.SelectedNode.Text, "Remove Column...", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    //DataStreamColections.Nodes.Remove(DataStreamColections.SelectedNode);
                    Remove_Column(DataStreamColections.SelectedNode.Text);
                    DataStreamColections_AfterSelect(null, null);
                }
            }
        }

        private void RenameDataStream_Click(object sender, EventArgs e)
        {
            UserTextInput tmpTx = new UserTextInput();
            renamedItems tmpRenamedItem;
            tmpTx.Text = "Rename";
            tmpTx.TextBoxTitle = "Enter new name...";
            foreach(DataStream tmpDS in tempDataStreams)
            {
                if(tmpDS.Name == DataStreamColections.SelectedNode.Text)
                {
                    tmpTx.ReturnText = tmpDS.Name;
                }
            }
            //tmpTx.ShowDialog();

            if(tmpTx.ShowDialog() == DialogResult.OK)
            {
                foreach(DataStream tmpDS in tempDataStreams)
                {
                    if(tmpDS.Name == DataStreamColections.SelectedNode.Text)
                    {
                        tmpRenamedItem = new renamedItems();
                        tmpRenamedItem.befoure = tmpDS.Name;
                        tmpRenamedItem.after = tmpTx.ReturnText;
                        tmpRenamedItem.type = 0;
                        tempRenamed.Add(tmpRenamedItem);

                        tmpDS.Name = tmpTx.ReturnText;
                        RefreshTreeItems("");
                        return;
                    }
                }
            }
        }

        private void RenameColumn_Click(object sender, EventArgs e)
        {
            renamedItems tmpRenamedItem;
            UserTextInput tmpTx = new UserTextInput();
            tmpTx.ReturnText = DataStreamColections.SelectedNode.Text;
            tmpTx.Text = "Rename";
            tmpTx.TextBoxTitle = "Enter new name...";
            foreach(DataStream tmpDS in tempDataStreams)
            {
                if(tmpDS.Name == DataStreamColections.SelectedNode.Parent.Text)
                {
                    foreach(Column tmpCol in tmpDS.Columns)
                    {
                        if(tmpCol.Name == DataStreamColections.SelectedNode.Text)
                        {
                            tmpTx.ReturnText = tmpCol.Name;
                        }
                    }
                }
            }
            //tmpTx.ShowDialog();

            if(tmpTx.ShowDialog() == DialogResult.OK)
            {
                foreach(DataStream tmpDS in tempDataStreams)
                {
                    if(tmpDS.Name == DataStreamColections.SelectedNode.Parent.Text)
                    {
                        foreach(Column tmpCol in tmpDS.Columns)
                        {
                            if(tmpCol.Name == DataStreamColections.SelectedNode.Text)
                            {
                                tmpRenamedItem = new renamedItems();
                                tmpRenamedItem.befoure = tmpCol.Name;
                                tmpRenamedItem.after = tmpTx.ReturnText;
                                tmpRenamedItem.type = 1;
                                tempRenamed.Add(tmpRenamedItem);
                                
                                tmpCol.Name = tmpTx.ReturnText;
                                RefreshTreeItems("");
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void StringType_Click(object sender, EventArgs e)
        {
            //foreach(DataStream tmpDS in EditorController.Instance.EditorProject.DataStreams)
            //{
            //    if(tmpDS.Name == DataStreamColections.SelectedNode.Parent.Text)
            //    {
            //        foreach(Column tmpCol in tmpDS.Columns)
            //        {
            //            if(tmpCol.Name == DataStreamColections.SelectedNode.Text)
            //            {
            //                tmpCol.Type = "";
            //                return;
            //            }
            //        }
            //    }
            //}
        }

        private void IntType_Click(object sender, EventArgs e)
        {
            //foreach(DataStream tmpDS in EditorController.Instance.EditorProject.DataStreams)
            //{
            //    if(tmpDS.Name == DataStreamColections.SelectedNode.Parent.Text)
            //    {
            //        foreach(Column tmpCol in tmpDS.Columns)
            //        {
            //            if(tmpCol.Name == DataStreamColections.SelectedNode.Text)
            //            {
            //                tmpCol.Type = "int";
            //                return;
            //            }
            //        }
            //    }
            //}
        }

        private void FloatType_Click(object sender, EventArgs e)
        {
            //foreach(DataStream tmpDS in EditorController.Instance.EditorProject.DataStreams)
            //{
            //    if(tmpDS.Name == DataStreamColections.SelectedNode.Parent.Text)
            //    {
            //        foreach(Column tmpCol in tmpDS.Columns)
            //        {
            //            if(tmpCol.Name == DataStreamColections.SelectedNode.Text)
            //            {
            //                tmpCol.Type = "float";
            //                return;
            //            }
            //        }
            //    }
            //}
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            UpdateFromTempData();
            EditorController.Instance.ProjectSaved = false;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            ProductHelp.OpenHelpTopic(this, "PDF%20Reports%20Template%20Editor_Manage%20DataStreams.html");
        }
    }

    public class renamedItems
    {
        public int type;
        public string befoure;
        public string after;
    }
}