using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.IO;
using System.Data.Common;
using Microsoft.Win32;
using System.Threading;
using AxiomCoders;


namespace PdfGenerator
{
    /// <summary>
    /// Enumeration - data source type
    /// SqlServer, CSV file, TXTFile, Manual entered data
    /// </summary>
    public enum enumDataSourceType : int
    {
        SqlServer = 1,
        CSVFile,
        TXTFile,
        ManualEntered
    }

    /// <summary>
    /// Enumeration - Sql Server type
    /// Data base engine or SqlServerCe
    /// </summary>
    public enum enumServerType : int
    {
        DataBaseEngine = 0,
        SqlServerCompactEdition
    }

    /// <summary>
    /// Enumeration - Authentication
    /// Windows, Sql Server or SqlServerCe authentication
    /// </summary>
    public enum enumAuthentication : int
    {
        WindowsAuthentication = 0,
        SQLServerAuthentication,
        SqlServerCompactEditionAuthentication
    }
    
    

    public partial class Main : Form
    {

        /// <summary>
        /// Help file name
        /// </summary>
        const string helpFileName = "AxiomCoders Pdf Reports Help.chm";
        /// <summary>
        /// Successfuly established connection with SqlServer
        /// </summary>
        const string msg1 = @"Successful!";
        /// <summary>
        /// Message: 
        /// A column named "column name" alredy belongs to table "table"
        /// </summary>
        const string msg2 = @"A column named ";
        const string msg3 = @" alredy belongs to table ";
        /// <summary>
        /// Existing Data Stream Name
        /// </summary>
        const string msg4 = @"Choose another Data Stream Name.";
        /// <summary>
        /// Eroor that occurs during entering data
        /// </summary>
        const string msg5 = @"Entering data error.";
        /// <summary>
        /// Save changes in DataView
        /// </summary>
        const string msg6 = @"Apply changes?";
        /// <summary>
        /// 
        /// </summary>
        const string msg7 = @"File not found!";
        const string msg8 = @"Successful creating PDF file!";
        /// <summary>
        /// CrLf
        /// </summary>
        const char Cr = (char)13;
        const char Lf = (char)10;
        string CrLf = Cr.ToString() + Lf.ToString(); /* CrLf */
        
        /// <summary>
        /// Data source type is changed
        /// </summary>
        private bool isDataSourceTypeChanged = false;
        /// <summary>
        /// Sql Server type is changed
        /// </summary>
        private bool isSqlServerTypeChanged = false;
        /// <summary>
        /// Template file name
        /// </summary>
        private string templateFileName = string.Empty;
        /// <summary>
        /// Data source type
        /// </summary>
        private enumDataSourceType dataSourceType = enumDataSourceType.SqlServer;
        /// <summary>
        /// Sql server type
        /// </summary>
        static enumServerType serverType = enumServerType.DataBaseEngine;
        /// <summary>
        /// Sql server name
        /// 
        /// </summary>
        static string serverName = string.Empty;
        /// <summary>
        /// Authentication
        /// </summary>
        static enumAuthentication authentication = enumAuthentication.WindowsAuthentication;
        /// <summary>
        /// Data base name (SqlServer)
        /// or path sdf file (Sql Server Ce)
        /// </summary>
        static string dataBaseName=string.Empty;
        /// <summary>
        /// User name
        /// </summary>
        static string userName=string.Empty;
        /// <summary>
        /// Password
        /// </summary>
        static string password=string.Empty;
        /// <summary>
        /// Dataset(data streams)
        /// </summary>
        static DataSet Ds = null;
        /// <summary>
        /// Output file name
        /// </summary>
        private string outputFileName = string.Empty;
        /// <summary>
        /// Dictionary
        /// Save: Data Stream Name-Table Name in SqlServer(or SqlServer Ce)
        /// </summary>
        private Dictionary<string,string> dictTableName=new Dictionary<string,string>();
        /// <summary>
        /// Thread ProgressBar
        /// </summary>
        Thread t2 = null;
        /// <summary>
        /// GeneratingOutputPDFFile
        /// </summary>
        Thread t1 = null;
       
        
        public Main()
        {
            InitializeComponent();
        }
             
        /// <summary>
        /// Exeption
        /// </summary> 
        /// <param name="e">
        /// System.Exeption - e
        /// </param>
       static private void MyException(System.Exception e)
        {
            
            MessageBox.Show(e.Message,"Error");
        }

        private void StartGeneratingOutputPDFFile()
        {
            t2 = new Thread(new ThreadStart(ProgressBar));
            t1 = new Thread(new ThreadStart(GeneratingOutputPDFFile));
            t2.Start();
            t1.Start();
            
         }


        /***********************************************************************************************/
        /// <summary>
        /// Starting progress bar(ProgressBar())
        /// and generating output file 
        /// </summary>
       private void GeneratingOutputPDFFile()
        {
            
            try
            {
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
                lblGeneratingFile.Text = @"Generating file: " + @lblPDFFile.Text;
                prbGenerating.Value = 0;
                prbGenerating.PerformStep();
                Application.DoEvents();
                /// <summary>
                /// Binding data from dataset(Ds) with DataStream in template
                /// </summary>
                AxiomCoders.PdfReports.PdfReports pdfReporter = new AxiomCoders.PdfReports.PdfReports();
                pdfReporter.DataSources.Add(Ds);
                pdfReporter.TemplateFileName = @templateFileName;
                pdfReporter.Logging = true;
                pdfReporter.GeneratePdf(@outputFileName);
                prbGenerating.Value = 100;
                prbGenerating.PerformStep();
                Application.DoEvents();
                btnNext.Enabled = true;
            }
            catch 
            {
                /// <summary>
                /// Settings default values in case of error
                /// </summary>
                btnNext.Enabled = false;
                prbGenerating.Value = 0;
                prbGenerating.PerformStep();
                Application.DoEvents();
                
            }
        }
        /*****************************************************************************************************/
        
        
        /// <summary>
        /// Simulation progress bar
        /// </summary>
       private void ProgressBar()
        {
             
            try
            {
                while (true)
                {
                    if(prbGenerating.Value==100)
                    {
                        return;
                    }
                    if (prbGenerating.Value < 99)
                    {
                        prbGenerating.Value += 1;
                        prbGenerating.PerformStep();
                        Application.DoEvents();
                    }
                    else
                    {
                        prbGenerating.Value = 1;
                    }
                    Thread.Sleep(250);

                }
            }
            catch
            {
                

            }
        }
        

        /// <summary>
        /// Create relatin ( field(parent table)->field(child table))
        /// </summary>
        private void SetRelationship()
        {
            string relationName = string.Empty;
            try
            {
                DataColumn parentColumn = Ds.Tables[cmbStreamLRelationships.Text].Columns[lbxLColumnsRelationships.SelectedIndex];
                DataColumn childColumn = Ds.Tables[cmbStreamRRelationships.Text].Columns[lbxRColumnsRelationships.SelectedIndex];
                /// <summary>
                /// creating relationName
                /// example: parentColumn(Parent TableName)->ChildColumnName(ChildTableName)
                /// one-many
                /// </summary>
                relationName = parentColumn.ColumnName.Trim() + @"(" + cmbStreamLRelationships.Text +
                                      @")->" + childColumn.ColumnName.ToString() + @"(" + cmbStreamRRelationships.Text + @")";
                if (!Ds.Relations.Contains(relationName))
                {
                    /// <summary>
                    /// true-add constraint
                    /// false - do not add constraint
                    /// </summary>
                    Ds.Relations.Add(relationName, parentColumn, childColumn, false);
                    lbxExistingRelationships.Items.Add(relationName);
                }
            }
            catch (System.Exception e)
            {
                if (Ds.Relations.Contains(relationName))
                {

                    /// <summary>
                    /// Removing relation from dataset Ds
                    /// in case of uncomplet adding relation(relationName)
                    /// </summary>
                    Ds.Relations.Remove(relationName);
                }
                MyException(e);
            }
        }





        /// <summary>
        /// Adding existing relations 
        /// </summary>
        /// <param name="dataStreamName">
        /// Data stream name (table name in dataset Ds)
        /// </param>
        /// <param name="tableName">
        /// Table name in SqlServer(or SqlServerCe)
        /// </param>
        private void AddExistingRelations(string dataStreamName, string tableName)
        {
            DbConnection conn = null;
            try
            {
                DbCommand command = null;
                DbDataReader reader;
                /// <summary>
                /// Column name in information_schema
                /// constraint_name-name existing relation 
                /// </summary>
                string constraint_name = string.Empty;
                /// <summary>
                /// column_name - column name (many)
                /// </summary>
                string column_name = string.Empty;
                /// <summary>
                /// ref_table_name - table name (one)
                /// </summary>
                string ref_table_name = string.Empty;
                /// <summary>
                /// ref_column_name-column name(one)
                /// </summary>
                string ref_column_name = string.Empty;
                /// <summary>
                /// Command text (SqlCommand.CommandText)
                /// </summary>
                string commandText = string.Empty;
                /// <summary>
                /// logical indicator
                /// </summary>
                bool ind = false;
                /// <summary>
                /// relation name
                /// ColumnName(TableName)->ColumnName(TableName)
                /// one-many
                /// </summary>
                string relationName = string.Empty;
                string parentTableName = string.Empty;
                /// <summary>
                ///Parent column (one)
                /// </summary>
                DataColumn parentColumn = null;
                /// <summary>
                /// Child column (many)
                /// </summary>
                DataColumn childColumn = null;
                /// <summary>
                /// Child column (many)
                /// </summary>
                //create instance (SqlCommand)
                if (serverType == enumServerType.DataBaseEngine)
                {
                    command = (SqlCommand)Activator.CreateInstance(typeof(SqlCommand));
                }
                /// <summary>
                /// create instance (SqlCeCommand)
                /// </summary>
                if (serverType == enumServerType.SqlServerCompactEdition)
                {
                    command = (SqlCeCommand)Activator.CreateInstance(typeof(SqlCeCommand));
                }
                Cursor.Current = Cursors.WaitCursor;
                if(GetSqlConnection(ref conn))
                {
                    /// <summary>
                    /// Adding (Data Stream Name, Table name) in Dictionary
                    /// </summary>
                    dictTableName.Add(dataStreamName, tableName);
                    /// <summary>
                    /// Passing through collection of existing tables in dataset Ds
                    /// </summary>
                    foreach (KeyValuePair<string, string> kvp in dictTableName)
                    {
                        /// <summary>
                        /// Finding existing relation in database(Sqlserver or SqlCeServer) for table 
                        /// kvp.Value(many)
                        /// </summary>
                        commandText = @"select T2.CONSTRAINT_NAME, T1.COLUMN_NAME, T4.TABLE_NAME as 
                        REF_TABLE_NAME, T4.COLUMN_NAME as REF_COLUMN_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE as T1
                        left join INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS as T2 on T2.CONSTRAINT_NAME = T1.CONSTRAINT_NAME
                        left join INFORMATION_SCHEMA.TABLE_CONSTRAINTS as T3 on T2.UNIQUE_CONSTRAINT_NAME = T3.CONSTRAINT_NAME
                        left join INFORMATION_SCHEMA.KEY_COLUMN_USAGE as T4 on T3.CONSTRAINT_NAME = T4.CONSTRAINT_NAME
                        where T1.TABLE_NAME = '<table name>' and T1.ORDINAL_POSITION = T4.ORDINAL_POSITION";
                        commandText = commandText.Replace(@"<table name>", @kvp.Value);
                        command.CommandText = commandText;
                        command.Connection = conn;
                        reader = command.ExecuteReader();
                        /// <summary>
                        /// Passing through collection of existing relations(SqlServer or SqlserverCe)
                        /// </summary>
                        while (reader.Read())
                        {
                            constraint_name = string.Empty;
                            column_name = string.Empty;
                            ref_table_name = string.Empty;
                            ref_column_name = string.Empty;
                            /// <summary>
                            /// ind=false,  null  value detected
                            /// </summary>
                            ind = true;
                            //ind=false,  null  value detected
                            /// <summary>
                            /// Child column (many)
                            /// </summary>
                            if (reader["CONSTRAINT_NAME"] != null)
                            {
                                constraint_name = reader["CONSTRAINT_NAME"].ToString();
                            }
                            else
                            {
                                ind = false;
                            }
                            if ((reader["COLUMN_NAME"] != null) && ind)
                            {
                                column_name = reader["COLUMN_NAME"].ToString();
                            }
                            else
                            {
                                ind = false;
                            }
                            if ((reader["REF_TABLE_NAME"] != null) && ind)
                            {
                                ref_table_name = reader["REF_TABLE_NAME"].ToString();
                            }
                            else
                            {
                                ind = false;
                            }
                            if ((reader["REF_COLUMN_NAME"] != null) && ind)
                            {
                                ref_column_name = reader["REF_COLUMN_NAME"].ToString();
                            }
                            else
                            {
                                ind = false;
                            }

                            if (ind)
                            // contraint_name!null and column_name!=null and ref_table_name!=null and ref_column_name!=null

                            {
                                if (dictTableName.ContainsValue(ref_table_name))
                                {
                                    /// <summary>
                                    /// Finding table Name (SqlServer or SqlServerCe) in Dictionary
                                    /// kvp1-item in Dictionary dictTableName
                                    /// kvp1.Key-Data Stream Name
                                    /// kvp1.Value - Table Name (SqlServer or sqlServer Ce)
                                    /// </summary>
                                    parentTableName = string.Empty;
                                    foreach (KeyValuePair<string, string> kvp1 in dictTableName)
                                    {
                                        if (kvp1.Value == ref_table_name)
                                        {
                                            parentTableName = kvp1.Key;
                                            break;
                                        }
                                    }
                                    /// <summary>
                                    /// creating relationName
                                    /// example: ColumnName(TableName)->ColumnName(TableName)
                                    /// one-many
                                    /// </summary>
                                    relationName = @ref_column_name + @"(" + @parentTableName + @")->" + @column_name + @"(" + @kvp.Key + @")";
                                    ind = false;
                                    foreach (DataRelation dr in Ds.Relations)
                                    {
                                        if (dr.RelationName == relationName)
                                        {
                                            ind = true; 
                                            /// <summary>
                                            /// Detected relation in dataset Ds
                                            /// </summary>
                                            break;
                                        }
                                    }

                                    /// <summary>
                                    /// Relation doesn't in dataset Ds
                                    /// </summary>
                                    if (!ind)
                                    {
                                        
                                        parentColumn = Ds.Tables[parentTableName].Columns[ref_column_name];
                                        childColumn = Ds.Tables[@kvp.Key].Columns[column_name];
                                        try
                                        {
                                            /// <summary>
                                            /// Adding relation in dataset vDs
                                            /// </summary>
                                            Ds.Relations.Add(relationName, parentColumn, childColumn,false);
                                            /// <summary>
                                            /// true-add constraint
                                            /// false - do not add constraint
                                            /// </summary>
                                            lbxExistingRelationships.Items.Add(relationName);
                                        }
                                        catch 
                                        {
                                            if(Ds.Relations.Contains(relationName))
                                            {
                                                /// <summary>
                                                /// Removing relation from dataset Ds
                                                /// in case of uncomplet adding relation(relationName)
                                                /// </summary>
                                                Ds.Relations.Remove(relationName);
                                            }
                                        }
                                    }
                                }

                            }

                        }
                        reader.Close();
                    } 
                }
                Cursor.Current = Cursors.Default;
                conn.Close();
                conn = null;
              
            }
            catch (System.Exception e)
            {
                Cursor.Current = Cursors.Default;
                MyException(e);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    conn = null;
                }


            }
        }
   
        /// <summary>
        /// Opening output (PDF) file in default browser
        /// </summary>
        private void OpenOutputFile()
        {

            try
            {
                if(File.Exists(outputFileName))
                {
                    System.Diagnostics.Process.Start(outputFileName);
                }
                else
                {
                    MessageBox.Show(msg7 + CrLf + "(" + outputFileName + ")","Error");
                }


            }
            catch (System.Exception e)
            {
                MyException(e);
            }
        }



        /// <summary>
        /// Opening help file
        /// </summary>
        private void OpenHelpFile()
        {
           try
            {
                if (File.Exists(helpFileName))
                {
                    System.Diagnostics.Process.Start(helpFileName);
                }
                else
                {
                    MessageBox.Show(msg7 + CrLf + "(" + helpFileName + ")", "Error");
                }


            }
            catch (System.Exception e)
            {
                MyException(e);
            }
        }

        /// <summary>
        /// Fill textbox in Summary
        /// </summary>
        private void Summary()
        {
             try
            {
                string line = string.Empty;
                string sourceType = string.Empty;
                string dataStreamSelected=string.Empty;
                string relationshipsSet = string.Empty;
                txtSummary.Text = string.Empty;
                line=@"Output PDF file: " +lblPDFFile.Text+CrLf;
                line += @"Template file used: "+lblTemplateFileName.Text+CrLf;
                switch (dataSourceType)
                {
                    case enumDataSourceType.SqlServer:
                    {
                        sourceType = @"Sql server";
                        break;
                    }
                    case enumDataSourceType.CSVFile:
                    {
                        sourceType = @"CSV file";
                        break;
                    }
                    case enumDataSourceType.TXTFile:
                    {
                        sourceType = @"TXT file";
                        break;
                    }
                    case enumDataSourceType.ManualEntered:
                    {
                        sourceType = @"Manual entered data";
                        break;
                    }
                }
                line+=@"Data Source Type: " + sourceType+CrLf;
                line+=@"Data Stream selected: " ;
                foreach(DataTable dt in Ds.Tables)
                {
                    if(dataStreamSelected==string.Empty)
                    {
                        dataStreamSelected=dt.TableName;
                    }
                    else
                    {
                         dataStreamSelected+=@" , " + dt.TableName;
                    }
                }
                line+=dataStreamSelected+CrLf;
                line += "Relation ships set: ";
                foreach(DataRelation dt in Ds.Relations)
                {
                    if (relationshipsSet == string.Empty)
                    {
                        relationshipsSet= @"("+dt.RelationName + @")";
                    }
                    else
                    {
                        relationshipsSet+= @" , " + @"(" + dt.RelationName + @")";
                    }
                }
                line += relationshipsSet + CrLf;
                txtSummary.Text = line;

                btnNext.Focus();
            }
            catch (System.Exception e)
            {
                MyException(e);
            }
        }


        /// <summary>
        /// Delete selected rows in data grid
        /// </summary>
        private void DeleteRow()
        {
            try
            {
                string tableName = cmbDataStreamsManualEnteredData.Text;
                dataGridViewManualEnteredData.AllowUserToDeleteRows = true;
                foreach (DataGridViewRow dr in dataGridViewManualEnteredData.SelectedRows)
                {
                    dataGridViewManualEnteredData.Rows.Remove(dr);
                }
                dataGridViewManualEnteredData.AllowUserToDeleteRows = false;
                ApplyChanges();
                ValidateData();
            }
            catch (System.Exception ex)
            {
                MyException(ex);
            }
        }




        /// <summary>
        /// Decide to save or not changes in dataset(data stream) tables 
        /// </summary>
        private void SaveChangesTables()
        {
            try
            {
                if (btnApplyChanges.Enabled)
                {
                    if (MessageBox.Show(msg6, "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ApplyChanges();
                    }
                    else
                    {
                        RejectChanges();
                    }
                }
            }
            catch (Exception e)
            {
                MyException(e);
            }
        }

        


        /// <summary>
        /// Reject changes in dataset tables
        /// </summary>
        private void RejectChanges()
        {
            try
            {
                foreach (DataTable dt in Ds.Tables)
                {
                    dt.RejectChanges();
                }
                btnApplyChanges.Enabled = false;
                dataGridViewManualEnteredData.ReadOnly = true;
                dataGridViewManualEnteredData.AllowUserToAddRows = false;

                }
            catch (Exception e)
            {
                MyException(e);
            }
        }
        



        /// <summary>
        /// Accept changes in dataset tables
        /// </summary>
        private void ApplyChanges()
        {
            try
            {
                foreach (DataTable dt in Ds.Tables)
                {
                    dt.AcceptChanges();
                }
                btnApplyChanges.Enabled = false;
                dataGridViewManualEnteredData.ReadOnly = true;
                dataGridViewManualEnteredData.AllowUserToAddRows = false;
            }
            catch(Exception e)
            {
                MyException(e);
            }        
         }
        
        /// <summary>
        /// If the changes in dataset tables are made returns true if not returns folse
        /// </summary>
        /// <param name="dt">
        /// Dataset table for witch the changes are made
        /// </param>
        private bool IsChangedDs(string dt)
        {
            try
            {
                DataView dwAdded= new DataView(Ds.Tables[dt]);
                dwAdded.RowStateFilter = DataViewRowState.Added;
                DataView dwDeleted = new DataView(Ds.Tables[dt]);
                dwDeleted.RowStateFilter = DataViewRowState.Deleted;
                DataView dwModified = new DataView(Ds.Tables[dt]);
                dwModified.RowStateFilter = DataViewRowState.ModifiedCurrent;
                if((dwAdded.Count+dwDeleted.Count+dwModified.Count)>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                MyException(e);
                return false;
            }
            
        }

        /// <summary>
        /// Select item in listbox
        /// </summary>
        /// <param name="lbx">
        /// List box
        /// </param>
        /// <param name="item">
        /// Item in listbox
        /// </param>
        private void SelectItem(ListBox lbx, string item)
        {
            try
            {
                for (int i = 0; i < lbx.Items.Count;i++ )
                {
                    if(string.Equals(lbx.Items[i].ToString().Trim(),item.Trim()))
                    {
                        lbx.SetSelected(i,true);
                    }
                }
            }
            catch(System.Exception e)
            {
                MyException(e);
            }
            
        }
        
        /// <summary>
        /// Clear Ds (data stream)
        /// and setings isDataSourceChanged=false, 
        ///             isDataSourceType=false
        /// </summary>
        private void ClearDs()
        {
            try
            {
                
                isDataSourceTypeChanged = false;
                isSqlServerTypeChanged = false;
                if(Ds!=null)
                {
                    Ds.Dispose();
                    Ds = null;
                    
                }
                dictTableName.Clear();
                ClearSqlChooseDataStream();
                ClearCsvTxt();
                ClearManualEnteredDataStreams();

            }
            
            catch(System.Exception e)
            {
                MyException(e);
            }
            
        }

        /// <summary>
        /// Add column name in list boxes (relationships)
        /// </summary>
        /// <param name="tableName">
        /// </param>
        /// <param name="columnName">
        /// New column name
        /// </param>
        private void AddColumnRelationships(string tableName, string columnName)
        {
            try
            {
               if(cmbStreamLRelationships.Text==tableName)
               {
                   lbxLColumnsRelationships.Items.Add(columnName);
               }
               if (cmbStreamRRelationships.Text == tableName)
               {
                   lbxRColumnsRelationships.Items.Add(columnName);
               }
            }
            catch (System.Exception e)
            {
                MyException(e);
            }
        }

        /// <summary>
        /// Deleting column name in list boxes (relationships)
        /// </summary>
        /// <param name="tableName">
        /// </param>
        /// <param name="columnName">
        /// New column name
        /// </param>
        private void DeleteColumnRelationships(string tableName, string columnName)
        {
            try
            {
                if (cmbStreamLRelationships.Text == tableName)
                {
                    lbxLColumnsRelationships.Items.Remove(columnName);
                }
                if (cmbStreamRRelationships.Text == tableName)
                {
                    lbxRColumnsRelationships.Items.Remove(columnName);
                }
            }
            catch (System.Exception e)
            {
                MyException(e);
            }
        }

        /// <summary>
        /// Clear ds
        /// Clear listbox in SqlChooseDataStreams
        /// </summary>
        private void ClearSqlChooseDataStream()
        {
            try
            {
                cmbTable.Items.Clear();
                lbxDataStreamsSqlChooseDataStreams.Items.Clear();
                ClearSqlManageTables();
            }
            catch(System.Exception e)
            {
                MyException(e);
            }
        }



        /// <summary>
        /// Clear combobox, dataGrid in Manual Entered Data 
        /// </summary>
        private void ClearManualEnteredData()
        {
            try
            {
                cmbDataStreamsManualEnteredData.Items.Clear();
                dataGridViewManualEnteredData.DataSource = string.Empty;
                ClearRelationships();
            }
            catch (System.Exception e)
            {
                MyException(e);
            }
        }


        /// <summary>
        /// Clear txtSummary in Summary
        /// </summary>
        private void ClearSummary()
        {
            try
            {
                txtSummary.Text = string.Empty;
            }
            catch (System.Exception e)
            {
                MyException(e);
            }
        }


        /// <summary>
        /// Clear lblPDFFile in Chouse output file
        /// </summary>
        private void ClearChouseOutputFile()
        {
            try
            {
                lblPDFFile.Text = string.Empty;
                ClearSummary();
            }
            catch (System.Exception e)
            {
                MyException(e);
            }
        }


        /// <summary>
        /// Clear combobox, dataGrid in Manual Entered Data Streams 
        /// </summary>
        private void ClearManualEnteredDataStreams()
        {
            try
            {
                txtDataStreamNameManual.Text = string.Empty;
                lbxDataStreamManual.Items.Clear();
                lbxColumns.Items.Clear();
                txtColumnName.Text = string.Empty;
                ClearManualEnteredData();
            }
            catch (System.Exception e)
            {
                MyException(e);
            }
        }

        
        /// <summary>
        /// Clear combobox, dataGrid in SqlManageTables 
        /// </summary>
        private void ClearSqlManageTables()
        {
            try
            {
                cmbDataStreamsSqlManageTables.Items.Clear();
                dataGridViewSQL.DataSource = string.Empty;
                ClearRelationships();
            }
            catch (System.Exception e)
            {
                MyException(e);
            }
        }

        /// <summary>
        /// Clear comboboxes, listboxes in Relationships
        /// </summary>
        private void ClearRelationships()
        {
            try
            {
                cmbStreamLRelationships.Items.Clear();
                cmbStreamRRelationships.Items.Clear();
                lbxLColumnsRelationships.Items.Clear();
                lbxRColumnsRelationships.Items.Clear();
                lbxExistingRelationships.Items.Clear();
                ClearChouseOutputFile();
               
            }
            catch (System.Exception e)
            {
                MyException(e);
            }
        }
        
        
        /// <summary>
        /// Clear comboboxes, listboxes in CSV and TXT
        /// </summary>
        private void ClearCsvTxt()
        {
            try
            {
                lbxDataStreamsCSVTXT.Items.Clear();
            }
            catch (System.Exception e)
            {
                MyException(e);
            }
        }



        /// <summary>
        /// Gives list available SqlServers
        /// i fill cmbServerName with that list
        /// </summary>
                                                             
        private void GetSqlServers()
        {
           try
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server");
                if (rk == null) return;
                String[] instances = (String[])rk.GetValue("InstalledInstances");
                if(instances!=null)
                {
                    if (instances.Length > 0)
                    {
                        foreach (String element in instances)
                        {
                            if (element == "MSSQLSERVER")
                                cmbServerName.Items.Add(System.Environment.MachineName);
                            else
                                cmbServerName.Items.Add(System.Environment.MachineName + @"\" + element);
                        }
                    }
                }
             }
            catch(System.Exception e)
            {
                MyException(e);  
            }
            
        }

     

       /// <summary>
       /// Gives list of tables current database(SqlServer or SqlServerCE)
       /// fill cmtTable that list
       /// </summary>
                                                    
        private void SqlChooseDataStreams()
        {
            DbConnection conn=null;
            try
            {
                DbCommand command = null;
                DbDataReader reader ;
                string commandText=string.Empty;

                if(isSqlServerTypeChanged)
                {
                    ClearDs();
                }
                if (serverType == enumServerType.DataBaseEngine)
                {
                    conn = (SqlConnection)Activator.CreateInstance(typeof(SqlConnection));
                    command = (SqlCommand)Activator.CreateInstance(typeof(SqlCommand));
                }

                if (serverType == enumServerType.SqlServerCompactEdition)
                {
                    conn = (SqlCeConnection)Activator.CreateInstance(typeof(SqlCeConnection));
                    command = (SqlCeCommand)Activator.CreateInstance(typeof(SqlCeCommand));
                }

                if(GetSqlConnection(ref conn))
                {
                    commandText=@"select table_name as Name from INFORMATION_SCHEMA.Tables";
                    if(serverType==enumServerType.DataBaseEngine)
                    {
                        commandText += @" where TABLE_TYPE ='BASE TABLE' ";
                        commandText +=@"AND OBJECTPROPERTY(OBJECT_ID(TABLE_NAME), 'IsMsShipped') = 0";
                        commandText += @" ORDER BY TABLE_SCHEMA, TABLE_NAME ";
                    }
                    commandText += ";";
                    command.Connection = conn;
                    command.CommandText=commandText;
                    Cursor.Current = Cursors.WaitCursor;
                    reader = command.ExecuteReader();
                    cmbTable.Items.Clear();
                    cmbTable.Items.Add(string.Empty);
                    while(reader.Read())
                    {
                        string tableName = reader["Name"].ToString();
                        cmbTable.Items.Add(tableName);
                    }
                    Cursor.Current = Cursors.Default;
                    cmbTable.SelectedItem = string.Empty;
                    reader.Close();
                    reader = null;
                    command.Dispose();
                    command = null;
                    conn.Close();
                    conn = null;
                    }
                                 
            }
            catch (SystemException e)
            {
                Cursor.Current = Cursors.Default;
                MyException(e);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    conn = null;
                }
            }
        }

        /// <summary>
        /// Previous page
        /// 0 Select template file
        /// 1 Select data source type
        /// 2 Sql server
        /// 3 Sql choose data stream
        /// 4 Sql manage tabels
        /// 5 CSV and TXT 1
        /// 6 CSV and TXT2
        /// 7 Relationships
        /// 8 Manual entered data stream
        /// 9 Manual entered data
        /// 10 Choose output file
        /// 11 Sumary
        /// 12 Generating
        /// 13 Final
        /// </summary>
        private void PreviousPage()
        {
            try
            {
                btnNext.Enabled = true;
                btnPrevious.Enabled = true;
                int selectedIndexPage = tabControl.SelectedIndex;
                switch (selectedIndexPage)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        {
                            if ((dataSourceType == enumDataSourceType.SqlServer) || (dataSourceType == enumDataSourceType.ManualEntered))
                            {
                                if (selectedIndexPage <= 13)
                                    tabControl.SelectedIndex = selectedIndexPage - 1;
                                else
                                    tabControl.SelectedIndex = 0;
                            }
                            if (dataSourceType == enumDataSourceType.CSVFile)
                            {
                                tabControl.SelectedIndex = 5;
                            }
                            if (dataSourceType == enumDataSourceType.TXTFile)
                            {
                                tabControl.SelectedIndex = 5;
                            }

                            break;
                        }
                    case 5:
                        {
                            tabControl.SelectedIndex = 1;
                            break;
                        }
                    case 7:
                        {
                            switch (dataSourceType)
                            {
                                case enumDataSourceType.CSVFile:
                                case enumDataSourceType.TXTFile:
                                case enumDataSourceType.SqlServer:
                                    {
                                        tabControl.SelectedIndex = 4;
                                        break;
                                    }
                                case enumDataSourceType.ManualEntered:
                                    {
                                        tabControl.SelectedIndex = 9;
                                        break;
                                    }

                            }
                            break;
                        }
                    case 8:
                        {
                            tabControl.SelectedIndex = 1;
                            break;
                        }
                    case 9:
                        {
                            SaveChangesTables();
                            tabControl.SelectedIndex = 8;
                            break;
                        }
                    case 10:
                        {
                            tabControl.SelectedIndex = 7;
                            break;
                        }
                    case 11:
                        {
                            tabControl.SelectedIndex = 10;
                            break;
                        }
                    case 12:
                        {
                            tabControl.SelectedIndex = 11;
                            break;
                        }
                    case 13:
                        {
                            tabControl.SelectedIndex = 12;
                            break;
                        }


                }
               
            }
            catch (System.Exception e)
            {
                MyException(e);
            }
        }


        /// <summary>
        /// Next page
        /// 0 Select template file
        /// 1 Select data source type
        /// 2 Sql server
        /// 3 Sql choose data stream
        /// 4 Sql manage tabels
        /// 5 CSV and TXT 1
        /// 6 CSV and TXT2
        /// 7 Relationships
        /// 8 Manual entered data stream
        /// 9 Manual entered data
        /// 10 Choose output file
        /// 11 Sumary
        /// 12 Generating
        /// 13 Final
        /// </summary>
 
        private void NextPage()
       {
           try
           {
                int selectedIndexPage = tabControl.SelectedIndex;
                btnNext.Enabled = true;
                btnPrevious.Enabled = true;
                switch(selectedIndexPage)
                {
                    case 0:
                    case 3:
                       {
                            if (selectedIndexPage<= 13)
                                tabControl.SelectedIndex = selectedIndexPage+1;
                            else
                                tabControl.SelectedIndex = 0;
                            break;
                        }
                    case 1: 
                        {
                            if(isDataSourceTypeChanged)
                            {
                               ClearDs();
                            }
                            if(dataSourceType==enumDataSourceType.SqlServer)
                            {
                                tabControl.SelectedIndex = 2; /*tabPage3 - SqlServer */   
                            }
                            if (dataSourceType == enumDataSourceType.CSVFile)
                            {
                                tabControl.SelectedIndex = 5; /*tabPage5 - CSV and TXT1 */
                            }
                            if (dataSourceType == enumDataSourceType.TXTFile)
                            {
                                tabControl.SelectedIndex = 5; /*tabPage5 - CSV  and TXT1 */
                            }
                            if(dataSourceType==enumDataSourceType.ManualEntered)
                            {
                                tabControl.SelectedIndex = 8; /*tabPage8 - Manual entered data streams */
                            }
                            break;
                        } 

                    case 2: 
                        {
                            if (TestConnection())
                            {
                                SqlChooseDataStreams();
                                tabControl.SelectedIndex = 3; /*tabPage4 - Sql Choose Data streems */
                            }
                            break;
                        }
                    case 4: 
                        {
                            tabControl.SelectedIndex = 7;
                            break;
                        }
                    case 5: 
                        {
                            tabControl.SelectedIndex = 4;
                            break;
                        }
                    case 8:
                        {
                            tabControl.SelectedIndex = 9;
                            break;
                        }
                    case 9:
                        {
                            SaveChangesTables();
                            tabControl.SelectedIndex = 7;
                            break;
                        }
                    case 7:
                        {
                            tabControl.SelectedIndex = 10;
                            break;
                        }
                    case 10:
                        {
                            Summary();
                            tabControl.SelectedIndex = 11;
                            break;
                        }
                    case 11:
                        {
                            tabControl.SelectedIndex = 12;
                            StartGeneratingOutputPDFFile();
                            break;
                        }
                    case 12:
                        {
                            tabControl.SelectedIndex = 13;
                            break;
                        }
                    case 13:
                        {
                            if(chkOpenPDFFile.Checked)
                            {
                                OpenOutputFile();
                            }
                            if(chkStartAgain.Checked)
                            {
                                StartAgain();
                                tabControl.SelectedIndex = 0;
                            }
                            else
                            {
                                Application.Exit();
                            }


                            break;
                        }
                    } // END SWITCH
           }
           catch(System.Exception e)
           {
               MyException(e);
           }
       }



        /// <summary>
        /// Settings Font.Regular=true for the labels
        /// (Select template file, ... , Final)
        /// </summary>
        void SetFontRegular()
        {
            try
            {
                using(Font myFont=new Font(lblFindTemplate.Font, FontStyle.Regular))
                {
                    if (lblFindTemplate.Font.Bold)
                    {
                        lblFindTemplate.Font = myFont;
                        return;
                    }
                    if (lblSelectDataSourceType.Font.Bold)
                    {
                        lblSelectDataSourceType.Font = myFont;
                        return;
                    }
                    if (lblManageDataStreams.Font.Bold)
                    {
                        lblManageDataStreams.Font = myFont;
                        return;
                    }
                    if (lblChooseOutputFile.Font.Bold)
                    {
                        lblChooseOutputFile.Font = myFont;
                        return;
                    }
                    if (lblSummary.Font.Bold)
                    {
                        lblSummary.Font = myFont;
                        return;
                    }
                    if (lblGeneratingPdfOutput.Font.Bold)
                    {
                        lblGeneratingPdfOutput.Font = myFont;
                        return;
                    }
                    if (lblFinal.Font.Bold) 
                    {
                        lblFinal.Font = myFont;
                    }
                }

            }
            catch(System.Exception e)
            {
                MyException(e);
            }
        }

        /// <summary>
        /// Setings FontStyle.Bold za current label
        /// 0 Select template file
        /// 1 Select data source type
        /// 2 Sql server
        /// 3 Sql choose data stream
        /// 4 Sql manage tabels
        /// 5 CSV and TXT 1
        /// 6 CSV and TXT2
        /// 7 Relationships
        /// 8 Manual entered data stream
        /// 9 Manual entered data
        /// 10 Choose output file
        /// 11 Sumary
        /// 12 Generating
        /// 13 Final
        /// </summary>
        private void SetFontBold()
        {
            try
            {
                using(Font myFont=new Font(lblFindTemplate.Font, FontStyle.Bold))
                {
                    int selectedIndexTab=this.tabControl.SelectedIndex;
                    switch(selectedIndexTab)
                    {
                        case 0:
                            lblFindTemplate.Font =myFont ;
                            return;
                        case 1:
                            lblSelectDataSourceType.Font = myFont;
                            return;
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            lblManageDataStreams.Font = myFont;
                            return;
                        case 10:
                            lblChooseOutputFile.Font = myFont;
                            return;
                        case 11:
                            lblSummary.Font = myFont;
                            return;
                        case 12:
                            lblGeneratingPdfOutput.Font = myFont;
                            return;
                        case 13:
                            lblFinal.Font = myFont;
                            return;
                    }
                }
            }
            catch(System.Exception e)
            {
                MyException(e);
            }

        }
       
        /// <summary>
        /// Settings properties Visible, Enable...
        /// changes of the pages...
        /// 0 Select template file
        /// 1 Select data source type
        /// 2 Sql server
        /// 3 Sql choose data stream
        /// 4 Sql manage tabels
        /// 5 CSV and TXT 1
        /// 6 CSV and TXT2
        /// 7 Relationships
        /// 8 Manual entered data stream
        /// 9 Manual entered data
        /// 10 Choose output file
        /// 11 Sumary
        /// 12 Generating
        /// 13 Final
        /// </summary>
        private void ValidateData()
        {
            bool ind = true;
            int selectedIndex = tabControl.SelectedIndex;
            try
            {
                SetFontRegular();
                SetFontBold();
                switch (selectedIndex)
                {
                    case 0:
                      {
                            btnPrevious.Enabled = false;
                            ind = (templateFileName.Length != 0);
                            btnNext.Enabled = ind;

                            break;
                        }
                    case 1:
                        {
                            btnPrevious.Enabled = true;
                            ind = (templateFileName != string.Empty);
                            btnNext.Enabled = ind;
                            break;
                        }
                    case 2:
                        {
                            if(serverType==enumServerType.DataBaseEngine)
                            {
                                ind = (serverName != string.Empty);
                            }
                            ind = ind && (dataBaseName != string.Empty);
                            if (authentication == enumAuthentication.SQLServerAuthentication)
                            {
                                ind = ind && (userName != string.Empty);
                            }

                            btnTestConnection.Enabled = ind;
                            btnConnect.Enabled = ind;
                            btnNext.Enabled = ind;
                            
                            break;

                        }
                    case 3:
                        {
                            btnAddSqlChooseDataStreams.Enabled = (txtDataStreamName.Text.Trim() != string.Empty);
                            btnDeleteSqlChooseDataStreams.Enabled = (this.lbxDataStreamsSqlChooseDataStreams.SelectedItems.Count > 0);
                            btnNext.Enabled = (lbxDataStreamsSqlChooseDataStreams.Items.Count > 0);
                            break;
                        }
                    case 5:
                        {
                            btnAddCSVFile.Enabled = (dataSourceType == enumDataSourceType.CSVFile);
                            btnAddTxtFile.Enabled = (dataSourceType == enumDataSourceType.TXTFile);
                            btnDeleteSelectedCSVTXT.Enabled=(lbxDataStreamsCSVTXT.SelectedItems.Count==1);
                            btnNext.Enabled=(lbxDataStreamsCSVTXT.Items.Count>0);
                            break;
                        }
                    case 7:
                        {
                            btnSetRelationship.Enabled = ((lbxLColumnsRelationships.SelectedItems.Count == 1) && (lbxRColumnsRelationships.SelectedItems.Count == 1));
                            btnDeleteRelationship.Enabled=(lbxExistingRelationships.SelectedItems.Count==1);
                            break;
                        }
                    case 8:
                        {
                            btnCreateManual.Enabled=(txtDataStreamNameManual.Text.Trim()!=string.Empty);
                            btnDeleteManual.Enabled=(lbxDataStreamManual.SelectedItems.Count==1);
                            ind = false;
                            if(Ds!=null)
                            {
                                if(Ds.Tables.Count>0)
                                {
                                    {
                                        ind = true;
                                        foreach(DataTable dt in Ds.Tables)
                                        {
                                            if(dt.Columns.Count==0)
                                            {
                                                ind = false;
                                             break;
                                            }
                                        }
                                    }
                                }
                            }
                            btnNext.Enabled = ind;
                            btnAddColumn.Enabled=(lbxDataStreamManual.SelectedItems.Count>0);
                            btnAddColumn.Enabled=btnAddColumn.Enabled && (txtColumnName.Text.Trim()!=string.Empty);
                            btnAddColumn.Enabled = btnAddColumn.Enabled && (cmbTypeColumn.Text != string.Empty);
                            btnDeleteColumn.Enabled=(lbxColumns.SelectedItems.Count>0);
                            break;
                        }
                    case 9:
                        {
                            btnNewRow.Enabled = false;
                            btndeleteRow.Enabled = false;
                            btnNext.Enabled = true;
                            if(dataGridViewManualEnteredData.DataSource!=null)
                            {
                                btnNewRow.Enabled = (dataGridViewManualEnteredData.DataSource.ToString() != string.Empty);
                                btndeleteRow.Enabled = (dataGridViewManualEnteredData.RowCount > 0);
                            }
                            if(cmbDataStreamsManualEnteredData.Text!=string.Empty)
                            {
                                btnApplyChanges.Enabled=IsChangedDs(cmbDataStreamsManualEnteredData.Text);
                                
                            }
                            break;
                        }
                    case 10:
                        {
                            btnNext.Enabled = (outputFileName!=string.Empty);
                           break;
                        }
                    case 12:
                    case 13:
                        {
                           
                            btnPrevious.Enabled = true;
                            break;
                        }
                    

                }
            }
            catch (System.Exception e)
            {
                MyException(e);
            }
         }


         /// <summary>
         /// Restarting wizards
         /// initilation default values
         /// </summary>
         private void StartAgain()
         {
             try
             {
                 ClearDs();
                 isDataSourceTypeChanged = false;
                 isSqlServerTypeChanged = false;
                 lblTemplateFileName.Text = string.Empty;
                 templateFileName = string.Empty;
                 rbtSqlServer.Checked = true;
                 cbServerType.SelectedIndex = 0;
                 cmbServerName.Text = string.Empty;
                 cmbAuthentication.SelectedIndex = 0;
                 txtDataBaseName.Text = string.Empty;
                 txtUserName.Text = string.Empty;
                 txtPassword.Text = string.Empty;
                 lblPDFFile.Text = string.Empty;
                 outputFileName = string.Empty;
                 prbGenerating.Value = 0;
            }
             catch (System.Exception e)
             {
                 MyException(e);
             }
         }


        /// <summary>
        /// Setings default value
        /// executing only during program starting
        /// </summary>
        private void RefreshData()
        {
            try
            {
                lblTemplateFileName.Text = templateFileName;
                switch (dataSourceType)
                {
                    case enumDataSourceType.SqlServer:
                        {
                            rbtSqlServer.Checked = true;
                            break;
                        }

                    case enumDataSourceType.CSVFile:
                        {
                            rbtCSV.Enabled = true;
                            break;
                        }

                    case enumDataSourceType.TXTFile:
                        {
                            rbtTXT.Enabled = true;
                            break;
                        }

                    case enumDataSourceType.ManualEntered:
                        {
                            rbtManual.Enabled = true;
                            break;
                        }
                }
                cbServerType.SelectedIndex = (int)serverType;
                GetSqlServers();
                cmbAuthentication.SelectedIndex = (int)authentication;
                txtDataBaseName.Text = dataBaseName;
                txtUserName.Text = userName;
                txtPassword.Text = password;
                tabControl.Top = -25;

            }
            catch (System.Exception e)
            {
                MyException(e);
            }
            
        }
        
        
        /// <summary>
        /// Set data source type
        /// (Sql Server, CSV file, TXT file or Manual entered data)
        /// </summary>
        void SetDataSourceType()
        {
            try
            {
                enumDataSourceType olddataSourceType = dataSourceType;
                if (rbtSqlServer.Checked)
                {
                
                    dataSourceType = enumDataSourceType.SqlServer;
                    txtBoxDescription.Text =  CrLf + @"Select this when data that you wish to be used in generation process is stored in Microsoft SQL Server.";
                }
                else
                    if (rbtCSV.Checked)
                    {
                        dataSourceType = enumDataSourceType.CSVFile;
                        txtBoxDescription.Text = CrLf +  @"Select this when data that you wish to be used in generation process is stored in CSV files.";
                    }
                    else
                        if (rbtTXT.Checked)
                        {
                            dataSourceType = enumDataSourceType.TXTFile;
                            txtBoxDescription.Text = CrLf +  @"Select this when data that you wish to be used in generation process is stored in TXT files.";
                        }
                        else
                        {
                            dataSourceType = enumDataSourceType.ManualEntered;
                            txtBoxDescription.Text = CrLf +  @"Select this when data that you wish to be used in generation process should be entered manually.";

                        }
                if(olddataSourceType!=dataSourceType)
                {
                    isDataSourceTypeChanged = true;
                }
                }
            catch(System.Exception e)
            {
                MyException(e);
            }
        }

        
        
        /// <summary>
        /// Make connection string for SqlServer or SqlServer CE
        /// </summary>
        /// <returns>
        /// connection string
        /// </returns>
        public static string GetConnectionString()
        {
            try
            {
                string connectionString = string.Empty;
                if (serverType == enumServerType.DataBaseEngine)
                {
                    /* SQL SERVER CONNECTION STRING */
                    connectionString = @"Data Source=";
                    /* SERVER NAME */
                    connectionString = string.Concat(connectionString, serverName, @";");
                    /* DATABASE NAME */
                    connectionString = string.Concat(connectionString, @"Database=", dataBaseName, ";");
                    if (authentication == enumAuthentication.WindowsAuthentication)
                    {
                        /* WINDOWS AUTHENTICATION */
                        connectionString = string.Concat(connectionString, @"Trusted_Connection=True;");
                    }
                    else
                    {
                        /*SQL SERVER AUTHENTICATION */
                        connectionString = string.Concat(connectionString, @"User ID=", userName, @";");
                        connectionString = string.Concat(connectionString, @"Password=", password, @";");
                        connectionString = string.Concat(connectionString, @"Trusted_Connection=False;");
                    }
                    /* CONNECTION TIMEOUT */
                    connectionString = string.Concat(connectionString, @"Connection Timeout=10;");
                }
                if (serverType == enumServerType.SqlServerCompactEdition)
                {
                    /*SQL SERVER COMPACT EDITION */
                    connectionString = @"Data Source=";
                    connectionString = string.Concat(connectionString, dataBaseName, @";");
                    connectionString = string.Concat(connectionString, @"Persist Security Info=False;");
                    if (password != string.Empty)
                    {
                        connectionString = string.Concat(connectionString, @"Password=", password, @";");
                    }
                }
                return connectionString;
            }
            catch(System.Exception e)
            {
                MyException(e);
                return string.Empty;
            }
        }

        /// <summary>
        /// Establishe connection with SqlServer(SqlServer or SqlserverCe)
        /// </summary>
        /// <param name="conn">
        /// Object type DbConnection which later convert in 
        /// SqlConnection ili SqlCeConnection
        /// </param>
        /// <returns>
        /// =true, connection successfuly
        /// =false, connection isn't successfuly
        /// </returns>
        private bool GetSqlConnection(ref DbConnection conn)
        {
            
            try
            {
                if(serverType==enumServerType.DataBaseEngine)
                {
                    conn = (SqlConnection)Activator.CreateInstance(typeof(SqlConnection)); 
                }
                if(serverType==enumServerType.SqlServerCompactEdition)
                {
                    conn = (SqlCeConnection)Activator.CreateInstance(typeof(SqlCeConnection)); 
                }
                conn.ConnectionString = GetConnectionString(); 
                conn.Open();
                return true;
            }
            catch(System.Exception e)
            {
                MyException(e);
                return false;
            }

        }

       
        /// <summary>
        /// Testing connection(Sqlserver or SqlServerCe)
        /// </summary>
        /// <returns>
        /// =true, connection successfuly
        /// =false, connection isn't successfuly
        /// </returns>
        public static bool TestConnection()
        {
            bool _connect = true;
            DbConnection conn = null;
            if (serverType == enumServerType.DataBaseEngine)
            {
                conn = (SqlConnection)Activator.CreateInstance(typeof(SqlConnection));
            }

            if (serverType == enumServerType.SqlServerCompactEdition)
            {
                conn = (SqlCeConnection)Activator.CreateInstance(typeof(SqlCeConnection));
            }
                       
            try
            {
                conn.ConnectionString = GetConnectionString();
                conn.Open();
                
            }
            catch (SystemException e)
            {
                _connect = false;
                MyException(e);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    conn = null;
                }
            }
            return _connect;
        }

       
           
        /// <summary>
        /// Find template file
        /// </summary>
        private void FindTemplateFile()
        {
            FileInfo file = null;
            try
            {
                dlgFileOpen.Filter = @"Prtp files (*.prtp)|*.prtp";
                dlgFileOpen.FilterIndex = 2;
                dlgFileOpen.RestoreDirectory = true;

                if (dlgFileOpen.ShowDialog() == DialogResult.OK)
                {
                    templateFileName = dlgFileOpen.FileName;
                    if(File.Exists(templateFileName))
                    {
                        file = new FileInfo(templateFileName);
                        lblTemplateFileName.Text = file.Name;
                    }
                }
            }
            catch (System.Exception e)
            {
                MyException(e);
            }
            finally
            {
                if (file != null)
                {
                    file = null;
                }
            }
        }





        /// <summary>
        /// Find output file
        /// </summary>
        private void FindPDFFile()
        {
            FileInfo file = null;
            try
            {
                dlgFileSave.Filter = @"Pdf files (*.pdf)|*.pdf";
                dlgFileSave.FilterIndex = 2;
                dlgFileSave.RestoreDirectory = true;
                if (dlgFileSave.ShowDialog() == DialogResult.OK)
                {
                    outputFileName = dlgFileSave.FileName;
                    file = new FileInfo(outputFileName);
                    lblPDFFile.Text = file.Name;
                    
                }
            }
            catch (System.Exception e)
            {
                MyException(e);
            }
            finally
            {
                if (file != null)
                {
                    file = null;
                }
            }
        }



        /// <summary>
        /// Selection Sql Server type(Database engine or SqlServer CE)
        /// and depending of that settings of properties visible, enable, text...
        /// rest of object in Sql Server type (tab control).
        /// </summary>
        private void SelectServerType()
       {
           try
           {
               if(serverType!=(enumServerType)cbServerType.SelectedIndex)
               {
                   isSqlServerTypeChanged = true;
               }
               dataBaseName = string.Empty;
               txtDataBaseName.Text = string.Empty;
               serverType = (enumServerType)cbServerType.SelectedIndex;
               txtDataBaseName.BackColor = Color.White;
               if ((enumServerType)cbServerType.SelectedIndex == enumServerType.SqlServerCompactEdition)
               {
                   cmbAuthentication.Items.Add(@"Sql Server Compact Authentication");
                   cmbAuthentication.SelectedIndex = 2;
                   cmbAuthentication.Enabled = false;
                   btnBrowse.Visible = true;
                   cmbServerName.Enabled = false;
                   lblDataBaseName.Text = @"File name";
                   txtUserName.Enabled = false;
                   txtDataBaseName.ReadOnly = true;
               }
               else
               {
                   cmbAuthentication.Items.Remove(@"Sql Server Compact Authentication");
                   cmbAuthentication.Enabled = true;
                   cmbAuthentication.SelectedIndex = 0;
                   btnBrowse.Visible = false;
                   cmbServerName.Enabled = true;
                   lblDataBaseName.Text = @"Data Base name";
                   txtUserName.Enabled = true;
                   txtDataBaseName.ReadOnly = false;
               }
               ValidateData();
           }
           catch(System.Exception e)
           {
               MyException(e);
           }
            
            
         }


        /// <summary>
        /// Find sdf file (Sqlserver Ce)
        /// </summary>
        private void FindSdfFile()
        {
            FileInfo file = null;
            try
            {
                dlgFileOpen.Filter = @"Sql Server CE files (*.sdf)|*.sdf";
                dlgFileOpen.FilterIndex = 2;
                dlgFileOpen.RestoreDirectory = true;

                if (dlgFileOpen.ShowDialog() == DialogResult.OK)
                {
                    if(dataBaseName.Trim().ToUpper()!=dlgFileOpen.FileName)
                    {
                        //if (dlgFileOpen.ShowDialog() == DialogResult.OK)
                        //{
                            isSqlServerTypeChanged = true;
                            dataBaseName = dlgFileOpen.FileName.Trim();
                            file = new FileInfo(dataBaseName);
                            txtDataBaseName.Text = file.Name;
                        //}
                    }
                }

            }
            catch (System.Exception ex)
            {
                MyException(ex);
            }
            finally
            {
                if (file != null)
                {
                    file = null;
                }
            }
        }

        /// <summary>
        /// Import text file (csv or txt) to table in dataset(Ds)
        /// </summary>
        /// <param name="textFile">
        /// Absolute path TXT or CSV file(for example c:\PdfGenerator\Test\TXT\products.txt)
        /// </param>
        /// <param name="tableName">
        /// Table name in dataset Ds
        /// </param>
        /// <returns></returns>
        private bool ImportTextToDataSet(string textFile,string tableName)
        {
            try
            {
                string strLine;
                string[] strArray;
                char delimiter=',';
                if(dataSourceType==enumDataSourceType.TXTFile)
                {
                    delimiter = '\t';
                }
                char[] charArray = new char[] { delimiter };
                DataTable dt = Ds.Tables.Add(tableName); // Add table in dataset Ds
                FileStream aFile = new FileStream(textFile, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                strLine = sr.ReadLine(); 
                strArray = strLine.Split(charArray);
                for (int i = 0; i < strArray.Length; i++)
                {
                    dt.Columns.Add(@"Field" + (i+1));
                }
                Ds.AcceptChanges();
                Cursor.Current = Cursors.WaitCursor;
                while (strLine != null)
                {
                    strArray = strLine.Split(charArray);
                    DataRow dr = dt.NewRow();
                    if(strArray.Length==Ds.Tables[tableName].Columns.Count)
                    {
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            dr[i] = strArray[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                    strLine = sr.ReadLine();
                }
                Cursor.Current = Cursors.Default;
                sr.Close();
                return true;
            }
            catch(System.Exception e)
            {
                Cursor.Current = Cursors.Default;
                MyException(e);
                return false;
            }
         }

        /// <summary>
        /// Find CSV or TXT file 
        /// 
        /// </summary>
        private void AddTextFile()
        {
            FileInfo file = null;
            try
            {
                
                string textFile = string.Empty; // txt or csv file
                string tableName = string.Empty;
                string textFileName = string.Empty;
                bool ind = false;
                for (int i = 0; i < lbxDataStreamsCSVTXT.Items.Count; i++)
                {
                    if (lbxDataStreamsCSVTXT.Items[i].ToString().Trim().ToUpper() == tableName.Trim().ToUpper())
                    {
                        ind = true;
                        break;
                    }
                }
                if (ind && (lbxDataStreamsCSVTXT.Items.Count > 0))
                {
                    MessageBox.Show(msg4, "Error");
                    return;
                }
                if(dataSourceType==enumDataSourceType.CSVFile)
                {
                    dlgFileOpen.Filter = @"CSV files (*.csv)|*.csv";

                }
                else
                {
                    dlgFileOpen.Filter = @"Txt files (*.txt)|*.txt";

                }
                dlgFileOpen.FilterIndex = 2;
                dlgFileOpen.RestoreDirectory = true;

                if (dlgFileOpen.ShowDialog() == DialogResult.OK)
                {
                   textFile = dlgFileOpen.FileName.Trim();
                   file = new FileInfo(textFile);
                   textFileName = file.Name;
                   file = null;
                   if(textFile.Trim()!=string.Empty)
                   {
                       tableName = textFileName.Substring(0, textFileName.Length - 4);
                       if(InputBox("Data stream name", "Enter data stream name", ref tableName)==DialogResult.OK)
                       {
                            if(tableName.Trim()!=string.Empty)
                            {
                                for (int i = 0; i < lbxDataStreamsCSVTXT.Items.Count; i++)
                                {
                                    if (lbxDataStreamsCSVTXT.Items[i].ToString().Trim().ToUpper() == tableName.Trim().ToUpper())
                                    {
                                        ind = true;
                                        break;
                                    }
                                }
                                if (ind && (lbxDataStreamsCSVTXT.Items.Count > 0))
                                {
                                    MessageBox.Show(msg4, "Error");
                                    return;
                                }
                                if (Ds == null)
                                {
                                    Ds = new DataSet();
                                }
                                
                                if(ImportTextToDataSet(textFile,tableName))
                                {
                                    lbxDataStreamsCSVTXT.Items.Add(tableName);
                                    SelectItem(lbxDataStreamsCSVTXT, tableName);
                                    FillSqlManageTables(tableName);
                                    FillRelationships(tableName);
                                    ValidateData();
                                }
                                else
                                {
                                    foreach(DataTable dt in Ds.Tables)
                                    {
                                        if(dt.TableName==tableName)
                                        {
                                            Ds.Tables.Remove(tableName);
                                            Ds.AcceptChanges();
                                        }
                                    }
                                }

                            }
                       }
                   }
                   
                }

            }
            catch (System.Exception ex)
            {
                MyException(ex);
            }
            finally
            {
                if (file != null)
                {
                    file = null;
                }
            }
         }




         /// <summary>
         /// Add data stream name (table name) in listbox and combobox (Relationships)
         /// </summary>
         /// <param name="dataStreamName">
         /// New table(data stream name) in dataset Ds
         /// </param>
         private void FillRelationships(string dataStreamName)
         {
             try
             {
                 if (cmbStreamLRelationships.Items.Count == 0)
                 {
                     cmbStreamLRelationships.Items.Add(string.Empty);
                 }
                 cmbStreamLRelationships.Items.Add(dataStreamName);
                 if (cmbStreamRRelationships.Items.Count == 0)
                 {
                     cmbStreamRRelationships.Items.Add(string.Empty);
                 }
                 cmbStreamRRelationships.Items.Add(dataStreamName);
             }
             catch (System.Exception e)
             {
                 MyException(e);
             }           
         }

        /// <summary>
        /// Add data stream name (table name) in combobox (Manual entered data)
        /// </summary>
        /// <param name="dataStreamName">
        /// New table (data stream name) in dataset Ds
        /// </param>
        private void FillManualEnteredData(string dataStreamName)
        {
            try
            {
                if (cmbDataStreamsManualEnteredData.Items.Count == 0)
                {
                    cmbDataStreamsManualEnteredData.Items.Add(string.Empty);
                }
                cmbDataStreamsManualEnteredData.Items.Add(dataStreamName);
            }
            catch (System.Exception e)
            {
                MyException(e);
            }
        }


          
        /// <summary>
        /// Add listbox and combobox in Sql Manage Tables
        /// </summary>
        /// <param name="dataStreamName">
        /// New table in dataset Ds
        /// </param>
        private void FillSqlManageTables(string dataStreamName)
        {
            try
            {
                if (cmbDataStreamsSqlManageTables.Items.Count == 0)
                {
                    //cmbDataStreamsSqlManageTables.Items.Add(string.Empty);
                }
                cmbDataStreamsSqlManageTables.Items.Add(dataStreamName);   
            }
            catch(System.Exception e)
            {
                MyException(e);
            }

        }



        /// <summary>
        /// Add the table in dataset Ds
        /// </summary>
        /// <param name="tableName">
        /// the name of the table in SqlServer
        /// </param>
        /// the name of the table in dataset Ds(data stream name in Sql choose data stream)
        /// <param name="dataStreamName"></param>
        private void AddTableToDs(string tableName, string dataStreamName)
        {
            DbConnection conn = null;

            try
            {
                DbDataAdapter da = null;
                DbCommand command = null;

                string constraint_name = string.Empty;
                string column_name = string.Empty;
                string ref_table_name = string.Empty;
                string ref_column_name = string.Empty;
                string commandText = string.Empty;
                bool ind = false;
                string relationName = string.Empty;


                for (int i = 0; i < lbxDataStreamsSqlChooseDataStreams.Items.Count; i++)
                {
                    if (lbxDataStreamsSqlChooseDataStreams.Items[i].ToString().Trim().ToUpper() == dataStreamName.Trim().ToUpper())
                    {
                        ind = true;
                        break;
                    }

                }
                if (ind && (lbxDataStreamsSqlChooseDataStreams.Items.Count > 0))
                {
                    MessageBox.Show(msg4, "Error");
                    return;
                }
                if (Ds == null)
                {
                    Ds = new DataSet();
                }
                if (GetSqlConnection(ref conn))
                {
                    if (serverType == enumServerType.DataBaseEngine)
                    {
                        command = (SqlCommand)Activator.CreateInstance(typeof(SqlCommand));
                        da = (SqlDataAdapter)Activator.CreateInstance(typeof(SqlDataAdapter));
                    }
                    if (serverType == enumServerType.SqlServerCompactEdition)
                    {
                        command = (SqlCeCommand)Activator.CreateInstance(typeof(SqlCeCommand));
                        da = (SqlCeDataAdapter)Activator.CreateInstance(typeof(SqlCeDataAdapter));
                    }
                    command.Connection = conn;
                    commandText = @"SELECT * FROM " + tableName + ";";
                    command.CommandText = commandText;
                    da.SelectCommand = command;
                    da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    Cursor.Current = Cursors.WaitCursor;
                    da.Fill(Ds, dataStreamName);
                    lbxDataStreamsSqlChooseDataStreams.Items.Add(dataStreamName);
                    FillSqlManageTables(dataStreamName);
                    FillRelationships(dataStreamName);
                    SelectItem(lbxDataStreamsSqlChooseDataStreams, dataStreamName);
                    cmbTable.Text = string.Empty;
                    Cursor.Current = Cursors.Default;
                    command.Dispose();
                    command = null;
                    da.Dispose();
                    da = null;
                    conn.Close();
                    conn = null;
                    if (dataSourceType == enumDataSourceType.SqlServer)
                    {
                        AddExistingRelations(dataStreamName, tableName);
                    }



                }
            }
            catch (System.Exception e)
            {
                Cursor.Current = Cursors.Default;
                MyException(e);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    conn = null;
                }


            }
        }




        

        /// <summary>
        /// Deleting the table in dataset Ds(data streams) 
        /// </summary>
        /// <param name="dataStreamName">
        /// Table name in dataset
        /// </param>
        private bool DeleteTableFromDs(string dataStreamName)
        {
           
            try
            {
                if (Ds != null)
                {
                    Ds.Tables.Remove(dataStreamName);
                    Ds.AcceptChanges();
                }
                if(dataSourceType!=enumDataSourceType.ManualEntered)
                {
                    lbxDataStreamsSqlChooseDataStreams.Items.Remove(dataStreamName);
                    if (string.Equals(cmbDataStreamsSqlManageTables.Text.Trim().ToUpper(),dataStreamName.Trim().ToUpper()))
                    {
                        dataGridViewSQL.DataSource = string.Empty;
                    }
                    cmbDataStreamsSqlManageTables.Items.Remove(dataStreamName);
                }
                else
                {
                    lbxDataStreamManual.Items.Remove(dataStreamName);
                    if (string.Equals(cmbDataStreamsManualEnteredData.Text.Trim().ToUpper(), dataStreamName.Trim().ToUpper()))
                    {
                        dataGridViewManualEnteredData.DataSource = string.Empty;
                    }
                    cmbDataStreamsManualEnteredData.Items.Remove(dataStreamName);
                }
                
                if (string.Equals(cmbStreamLRelationships.Text.Trim().ToUpper(), dataStreamName.Trim().ToUpper()))
                {
                     lbxLColumnsRelationships.Items.Clear();
                }
                cmbStreamLRelationships.Items.Remove(dataStreamName);
                if (string.Equals(cmbStreamRRelationships.Text.Trim().ToUpper(), dataStreamName.Trim().ToUpper()))
                {
                    lbxRColumnsRelationships.Items.Clear();
                }
                cmbStreamRRelationships.Items.Remove(dataStreamName);

                if ((dataSourceType == enumDataSourceType.CSVFile) || (dataSourceType == enumDataSourceType.TXTFile))
                {
                    lbxDataStreamsCSVTXT.Items.Remove(dataStreamName);
                }
                if(dataSourceType==enumDataSourceType.SqlServer)
                {
                    dictTableName.Remove(dataStreamName);
                }

                return true;
            }
            catch (System.Exception e)
            {
                MyException(e);
                return false;
            }
        }


        /// <summary>
        /// Function InputBox
        /// </summary>
        /// <param name="title">
        /// 
        /// </param>
        /// <param name="promptText"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }


        


        /// <summary>
        /// Deleting selected relationship
        /// </summary>
        private void DeleteRelationship()
        {
            try
            {
                string relationName = lbxExistingRelationships.SelectedItem.ToString();
                ///Find child table name in relation name ( field(parent table)->field(child table))
                string childTable = relationName.Substring(relationName.LastIndexOf(@"(") + 1,
                                    relationName.LastIndexOf(@")") - relationName.LastIndexOf(@"(") - 1);
                Ds.Relations.Remove(relationName);
                /// <summary>
                /// Delete constraint(relationName) if exist
                /// </summary>
                if(Ds.Tables[childTable].Constraints.Contains(relationName))
                {
                    Ds.Tables[childTable].Constraints.Remove(relationName);
                }
                Ds.AcceptChanges();
                lbxExistingRelationships.Items.Remove(relationName);
            }
            catch (System.Exception e)
            {
                MyException(e);
            }
        }


        /// <summary>
        /// Rename column(field) in table(dataset Ds)
        /// </summary>
        /// <param name="tableName">
        /// Table name in dataset Ds
        /// </param>
        /// <param name="columnIndex">
        /// Column index in table
        /// </param>
        /// <param name="columnName">
        /// New column name
        /// </param>
        private void RenameColumn(string tableName, int columnIndex, string columnName)
        {
            try
            {
                string oldColumnName = Ds.Tables[tableName].Columns[columnIndex].ColumnName;
                bool ind = false;
                string oldRelationName = string.Empty;
                string newRelationName = string.Empty;
                int n=0;
                /// <summary>
                /// Check wether column with name columnName in Dataset Ds exist or not                    
                /// </summary>
                foreach (DataColumn dc in Ds.Tables[tableName].Columns)
                {
                    if (string.Compare(dc.ColumnName.Trim(), columnName.Trim(),true)==0)
                    {
                        ind = true;
                        break;
                    }
                }
                /// <summary>
                /// if the columnName doesn't exists change ColumnName                    
                /// </summary>
                if ((!ind) || (string.Compare(columnName.Trim(), oldColumnName.Trim(),true)==0))
                {
                    Ds.Tables[tableName].Columns[columnIndex].ColumnName = columnName;
                    Ds.AcceptChanges();
                    /// <summary>
                    /// Change ColumnName in lbxLColumnsRelationships (Relationships) if there is a need                    
                    /// </summary>
                    if (string.Equals(tableName, cmbStreamLRelationships.Text))
                    {
                        n=lbxLColumnsRelationships.Items.Count;
                        for(int i=0;i<n;i++)
                        {
                            if (string.Equals(lbxLColumnsRelationships.Items[i].ToString(), oldColumnName))
                            {
                                lbxLColumnsRelationships.Items.RemoveAt(i);
                                lbxLColumnsRelationships.Items.Insert(i,columnName);
                                break;
                            }
                        }
                    }

                     /// <summary>
                     /// Change ColumnName in lbxRColumnsRelationships (Relationships) if there is a need                    
                     /// </summary>
                    if(string.Equals(tableName,cmbStreamRRelationships.Text))
                    {
                        n = lbxRColumnsRelationships.Items.Count;
                        for (int i = 0; i < n; i++)
                        {
                            if (string.Equals(lbxRColumnsRelationships.Items[i].ToString(), oldColumnName))
                            {
                                lbxRColumnsRelationships.Items.RemoveAt(i);
                                lbxRColumnsRelationships.Items.Insert(i, columnName);
                                break;
                            }
                        }
                    }

                    /// <summary>
                    /// Change RelationName in lbxExistingRelations(Relationships) if there is a need                    
                    /// </summary>
                    n = lbxExistingRelationships.Items.Count;
                    for (int i = 0; i < n; i++)
                    {
                        oldRelationName = lbxExistingRelationships.Items[i].ToString();
                        newRelationName = oldRelationName.Replace(oldColumnName + @"(" + tableName + @")", columnName + @"(" + tableName + @")");
                        if(!string.Equals(oldRelationName,newRelationName))
                        {
                            Ds.Relations[oldRelationName].RelationName=newRelationName;
                            lbxExistingRelationships.Items.RemoveAt(i);
                            lbxExistingRelationships.Items.Insert(i, newRelationName);
                         }
                     }
                }
                else
                {
                    MessageBox.Show(msg2 + columnName + msg3 + tableName + ".", "Error");
                }
            }
            catch (System.Exception e)
            {
                MyException(e);
            }
        }



        /// <summary>
        /// Rename column(field) name
        /// event dataGridViewSQL click
        /// </summary>
        private void dataGridViewSQL_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            string tableName = cmbDataStreamsSqlManageTables.Text;
            string columnName = string.Empty;
            string oldColumnName = string.Empty;
            try
            {
                if (columnIndex == -1) return;
                if (rowIndex == -1)
                {
                    oldColumnName = Ds.Tables[tableName].Columns[columnIndex].ColumnName;
                    if (InputBox("Column: " + oldColumnName, "New column name", ref columnName) == DialogResult.OK)
                    {
                        if (columnName.Trim() != string.Empty)
                        {
                            RenameColumn(tableName, columnIndex, columnName);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                MyException(ex);
            }
        }

        /// <summary>
        /// Add data stream(table) in dataset Ds(Manual entered data)
        /// </summary>
        private void AddTableToDsManual(string dataStreamName)
        {
            try
            {
                 bool ind = false;
                if (dataStreamName != string.Empty)
                {
                    if (Ds == null)
                    {
                        Ds = new DataSet();
                    }
                    foreach (DataTable dt in Ds.Tables)
                    {
                        if (dt.TableName.Trim().ToUpper() == dataStreamName.Trim().ToUpper())
                        {
                            ind = true;
                            break;
                        }
                    }
                    if (!ind)
                    {
                        Ds.Tables.Add(dataStreamName);
                        lbxDataStreamManual.Items.Add(dataStreamName);
                        txtDataStreamNameManual.Text = string.Empty;
                        if (lbxDataStreamManual.Items.Count > 0)
                        {
                            lbxDataStreamManual.SetSelected(lbxDataStreamManual.Items.Count - 1, true);
                        }
                        FillRelationships(dataStreamName);
                        FillManualEnteredData(dataStreamName);

                    }
                    else
                    {
                        MessageBox.Show(msg4, "Error");
                        txtDataStreamNameManual.Text = string.Empty;
                    }
                }
            }
            catch (System.Exception ex)
            {
                MyException(ex);
            }
        }



        /// <summary>
        /// Give column name from table
        /// Fill listbox lbxColumns
        /// </summary>
        /// <param name="tableName">
        /// Table name in datastream(dataset) ds
        /// </param>
        private void FillLbxColumns(string tableName)
        {
            try
            {
                lbxColumns.Items.Clear();
                txtColumnName.Text = string.Empty;
                cmbTypeColumn.Text = string.Empty;
                foreach (DataColumn dc in Ds.Tables[tableName].Columns)
                {
                    lbxColumns.Items.Add(dc.ColumnName);
                }
                txtColumnName.Focus();
                if(lbxColumns.Items.Count>0)
                {
                    lbxColumns.SetSelected(0,true);
                }

            }
            catch (System.Exception e)
            {
                MyException(e);
            }
        }

        /// <summary>
        /// Deleting column from table
        /// </summary>
        private void DeleteColumn()
         {
         try
            {
                string tableName = lbxDataStreamManual.SelectedItem.ToString();
                string columnName = lbxColumns.SelectedItem.ToString();
                int oldIndexSelected = lbxColumns.SelectedIndex;
                Ds.Tables[tableName].Columns.Remove(columnName);
                lbxColumns.Items.Remove(columnName);
                DeleteColumnRelationships(tableName, columnName);
                if (lbxColumns.Items.Count != 0)
                {
                    int indexSelected = oldIndexSelected - 1;
                    if (indexSelected < 0)
                    {
                        indexSelected = 0; ;
                    }
                    lbxColumns.SetSelected(indexSelected, true);
                }
            }
            catch(Exception ex)
            {
                MyException(ex);
            }
        }
        /// <summary>
        /// Add column in table
        /// if column already exists in the table procedure do nothing
        /// </summary>
        private void AddColumn()
        {
            string typeColumn = cmbTypeColumn.Text;
            
            try
            {
                string tableName = lbxDataStreamManual.SelectedItem.ToString();
                string columnName = txtColumnName.Text;
                foreach(DataColumn dc in Ds.Tables[tableName].Columns)
                {
                        if(string.Equals(dc.ColumnName.ToUpper(),columnName.ToString().ToUpper()))
                        {
                            MessageBox.Show(msg2 + columnName + msg3+tableName+".","Error");
                            return;
                        }
                }
                switch (typeColumn)
                {
                   case "bool":
                        {
                            typeColumn = @"System.Boolean";
                            break;
                        }
                    case "byte":
                        {
                            typeColumn = @"System.Byte";
                            break;
                        }
                    case "sbyte":
                        {
                            typeColumn = @"System.SByte";
                            break;
                        }
                    case "char":
                        {
                            typeColumn = @"System.Char";
                            break;
                        }
                    case "decimal":
                        {
                            typeColumn = @"System.Decimal";
                            break;
                        }
                    case "double":
                        {
                            typeColumn = @"System.Double";
                            break;
                        }
                    case "float":
                        {
                            typeColumn = @"System.Single";
                            break;
                        }
                    case "int":
                        {
                            typeColumn = @"System.Int32";
                            break;
                        }
                    case "uint":
                        {
                            typeColumn = @"System.UInt32";
                            break;
                        }
                    case "long":
                        {
                            typeColumn = "System.Int64";
                            break;
                        }
                    case "ulong":
                        {
                            typeColumn = @"System.UInt64";
                            break;
                        }
                    case "object":
                        {
                            typeColumn = @"System.Object";
                            break;
                        }
                    case "short":
                        {
                            typeColumn = @"System.Int16";
                            break;
                        }
                    case "ushort":
                        {
                            typeColumn = @"System.UInt16";
                            break;
                        }
                    case "string":
                        {
                            typeColumn = @"System.String";
                            break;
                        }
                }
                DataColumn newColumn = new DataColumn(columnName, Type.GetType(typeColumn));
                Ds.Tables[tableName].Columns.Add(newColumn);
                lbxColumns.Items.Add(columnName);
                txtColumnName.Text = string.Empty;
                cmbTypeColumn.Text = string.Empty;
                AddColumnRelationships(tableName, columnName);
                SelectItem(lbxColumns, columnName);
                ValidateData();
            }
            catch (System.Exception e)
            {
                MyException(e);
            }
        }




       
        private void btnAddSqlChooseDataStreams_Click(object sender, EventArgs e)
        {
            AddTableToDs(cmbTable.Text, txtDataStreamName.Text);
            ValidateData();
        }

        private void cmbDataStreamsSqlManageTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbDataStreamsSqlManageTables.Text==string.Empty)
            {
                dataGridViewSQL.DataSource = string.Empty;
            }
            else
            {
                dataGridViewSQL.DataSource = Ds.Tables[cmbDataStreamsSqlManageTables.Text];
            }
        }
        
       /// <summary>
       /// Deleting table from datastream(dataset) Ds 
       /// Sql Manage tables
       /// </summary>
       private void btnDeleteSqlChooseDataStreams_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbxDataStreamsSqlChooseDataStreams.SelectedItems.Count == 1)
                {
                    int oldIndexSelected = lbxDataStreamsSqlChooseDataStreams.SelectedIndex;
                    if (DeleteTableFromDs(lbxDataStreamsSqlChooseDataStreams.SelectedItem.ToString()))
                    {
                        if (lbxDataStreamsSqlChooseDataStreams.Items.Count != 0)
                        {
                            int indexSelected = oldIndexSelected - 1;
                            if (indexSelected < 0)
                            {
                                indexSelected = 0; ;
                            }
                            lbxDataStreamsSqlChooseDataStreams.SetSelected(indexSelected, true);
                        }
                        ValidateData();
                    }
                }
                 
            }
            catch (System.Exception ex)
            {
                MyException(ex);
            }
        }

       

        private void lbxDataStreamsSqlChooseDataStreams_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateData();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FindSdfFile();
            ValidateData();
        }

        private void rbtSqlServer_CheckedChanged(object sender, EventArgs e)
        {
            SetDataSourceType();
        }

        private void rbtCSV_CheckedChanged(object sender, EventArgs e)
        {
            SetDataSourceType();
        }

        private void rbtTXT_CheckedChanged(object sender, EventArgs e)
        {
            SetDataSourceType();
        }

        private void rbtManual_CheckedChanged(object sender, EventArgs e)
        {
            SetDataSourceType();
        }

        private void cmbServerName_TextChanged(object sender, EventArgs e)
        {
            if (serverName.Trim().ToUpper() != cmbServerName.Text.Trim().ToUpper())
            {
                isSqlServerTypeChanged = true;
                serverName = cmbServerName.Text.Trim();
                ValidateData();
            }
        }

        private void cmbAuthentication_SelectedIndexChanged(object sender, EventArgs e)
        {
            authentication = (enumAuthentication)cmbAuthentication.SelectedIndex;
            ValidateData();
        }

       
        private void cmbTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDataStreamName.Text = cmbTable.Text;
            txtDataStreamName.Select();
            ValidateData();
        }



        private void cbServerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectServerType();
        }



        private void txtDataBaseName_TextChanged(object sender, EventArgs e)
        {
            dataBaseName = txtDataBaseName.Text.Trim();
            ValidateData();
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            userName = txtUserName.Text.Trim();
            ValidateData();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            password = txtPassword.Text.Trim();
            ValidateData();
        }



        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            if (TestConnection())
            {
                MessageBox.Show(msg1,"Alert");
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (TestConnection())
            {
                MessageBox.Show(msg1,"Alert");
            }
        }





        private void btnBrowseTemplateFile_Click(object sender, EventArgs e)
        {
            FindTemplateFile();
        }


        private void Main_Load(object sender, EventArgs e)
        {

            RefreshData();
            ValidateData();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (tabControl.SelectedIndex)
                {
                    case 8:
                        {
                            txtDataStreamNameManual.Text = string.Empty;
                            txtDataStreamNameManual.Focus();
                            break;
                        }

                }
                ValidateData();
            }
            catch(System.Exception ex)
            {
                MyException(ex);
            }
         }

        private void txtTemplateFileName_TextChanged(object sender, EventArgs e)
        {
            ValidateData();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PreviousPage();
            ValidateData();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            NextPage();
            ValidateData();
        }


        private void txtDataBaseName_TextChanged_1(object sender, EventArgs e)
        {
            if (dataBaseName.Trim().ToUpper() != txtDataBaseName.Text.Trim().ToUpper())
            {
                isSqlServerTypeChanged = true;
                if (serverType == enumServerType.DataBaseEngine)
                {
                    dataBaseName = txtDataBaseName.Text.Trim();
                }
                
            }
            ValidateData();
        }
        
        
        
        /// <summary>
        /// Fill left combobox with the list of field selected table(Relationships)
        /// </summary>
        private void cmbStreamLRelationships_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string tableName = cmbStreamLRelationships.Text;
                if (tableName == string.Empty)
                {
                    lbxLColumnsRelationships.Items.Clear();
                }
                else
                {
                    lbxLColumnsRelationships.Items.Clear();
                    foreach (DataColumn dc in Ds.Tables[tableName].Columns)
                    {
                        lbxLColumnsRelationships.Items.Add(dc.ColumnName);
                    }
                }
                ValidateData();
            }
            catch(System.Exception ex)
            {
                MyException(ex);
            }
            
        }

        /// <summary>
        /// Fill right combobox with the list of field selected table(Relationships)
        /// </summary>
        private void cmbStreamRRelationships_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string tableName = cmbStreamRRelationships.Text;
                if (tableName == string.Empty)
                {
                    lbxRColumnsRelationships.Items.Clear();
                }
                else
                {
                    lbxRColumnsRelationships.Items.Clear();
                    foreach (DataColumn dc in Ds.Tables[tableName].Columns)
                    {
                        lbxRColumnsRelationships.Items.Add(dc.ColumnName);
                    }
                }
                ValidateData();
            }
            catch (System.Exception ex)
            {
                MyException(ex);
            }
        }

        private void lbxLColumnsRelationships_Click(object sender, EventArgs e)
        {
            ValidateData();
        }

        private void lbxRColumnsRelationships_Click(object sender, EventArgs e)
        {
            ValidateData();
        }

        private void lbxExistingRelationships_Click(object sender, EventArgs e)
        {
            ValidateData();
        }

        
        private void btnSetRelationship_Click(object sender, EventArgs e)
        {
            SetRelationship();
        }

        private void btnDeleteRelationship_Click(object sender, EventArgs e)
        {
            if(lbxExistingRelationships.SelectedItems.Count>0)
            {
                DeleteRelationship();
            }
        }

        private void btnAddCSVFile_Click(object sender, EventArgs e)
        {
            AddTextFile();
            
        }

        private void btnAddTxtFile_Click(object sender, EventArgs e)
        {
            AddTextFile();
        }

        private void lbxDataStreamsCSVTXT_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateData();
        }

        /// <summary>
        /// Deleting table in data stream(dataset) Ds
        /// event-btnDeleteSelectedCSVTXT_Click
        /// </summary>
        private void btnDeleteSelectedCSVTXT_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbxDataStreamsCSVTXT.SelectedItems.Count == 1)
                {
                    int oldIndexSelected = lbxDataStreamsCSVTXT.SelectedIndex;
                    if (DeleteTableFromDs(lbxDataStreamsCSVTXT.SelectedItem.ToString()))
                    {
                        if (lbxDataStreamsCSVTXT.Items.Count != 0)
                        {
                            int indexSelected = oldIndexSelected - 1;
                            if (indexSelected < 0)
                            {
                                indexSelected = 0; ;
                            }
                            lbxDataStreamsCSVTXT.SetSelected(indexSelected, true);
                        }
                    }
                    ValidateData();
                }
            }
            catch(Exception ex)
            {
                MyException(ex);
            }
        }

        private void txtDataStreamNameManual_TextChanged(object sender, EventArgs e)
        {
            ValidateData();
        }

        private void btnCreateManual_Click(object sender, EventArgs e)
        {
            string dataStreamName = txtDataStreamNameManual.Text.Trim();
            if(dataStreamName!=string.Empty)
            {
                AddTableToDsManual(dataStreamName);
                ValidateData();
            }
         }

        private void lbxDataStreamManual_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lbxDataStreamManual.SelectedItems.Count>0)
            {
                string tableName = lbxDataStreamManual.SelectedItem.ToString();
                FillLbxColumns(tableName);
                ValidateData();
             }
        }
      

        /// <summary>
        /// Deleting table from datastream(dataset) Ds
        /// </summary>
        private void btnDeleteManual_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbxDataStreamManual.SelectedItems.Count == 1)
                {
                    string dataStreamName = lbxDataStreamManual.SelectedItem.ToString();
                    int oldIndexSelected = lbxDataStreamManual.SelectedIndex;
                    if(DeleteTableFromDs(dataStreamName))
                    {
                        if (lbxDataStreamManual.Items.Count == 0)
                        {
                            lbxColumns.Items.Clear();
                        }
                        else
                        {
                            int indexSelected = oldIndexSelected - 1;
                            if (indexSelected < 0)
                            {
                                indexSelected = 0; ;
                            }
                            lbxDataStreamManual.SetSelected(indexSelected, true);
                        }
                    }
                    ValidateData();
                }
            }
            catch (System.Exception ex)
            {
                MyException(ex);
            }
        }

        
        private void btnAddColumn_Click(object sender, EventArgs e)
        {
            AddColumn();
        }

        private void txtColumnName_TextChanged(object sender, EventArgs e)
        {
            ValidateData();
        }

        private void cmbTypeColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateData();
        }

        private void cmbDataStreamsManualEnteredData_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tableName =cmbDataStreamsManualEnteredData.Text;
            try
            {
                SaveChangesTables();
                dataGridViewManualEnteredData.DataSource = Ds.Tables[tableName];
                ValidateData();
            }
            catch(System.Exception ex)
            {
                MyException(ex);
            }
       }

        private void btnDeleteColumn_Click(object sender, EventArgs e)
        {
            DeleteColumn();
        }

        private void lbxColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateData();
        }

        /// <summary>
        /// Error in entering data
        /// </summary>
        private void dataGridViewManualEnteredData_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(msg5,"Error");
            e.Cancel = true;
        }

        private void btnNewRow_Click(object sender, EventArgs e)
        {
            dataGridViewManualEnteredData.ReadOnly = false;
            dataGridViewManualEnteredData.AllowUserToAddRows = true;
            btnNewRow.Enabled = false;

        }

        private void dataGridViewManualEnteredData_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            ValidateData();
            
        }

        private void btnApplyChanges_Click(object sender, EventArgs e)
        {
            ApplyChanges();
        }

        private void dataGridViewManualEnteredData_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            ValidateData();
        }

        

        private void btndeleteRow_Click(object sender, EventArgs e)
        {
            DeleteRow();
        }

        private void btnBrowsePDFFileName_Click(object sender, EventArgs e)
        {
            FindPDFFile();
        }

        private void txtPDFFile_TextChanged(object sender, EventArgs e)
        {
            ValidateData();
        }
      

        private void lblPDFFile_TextChanged(object sender, EventArgs e)
        {
            ValidateData();
        }

        private void lblTemplateFileName_TextChanged(object sender, EventArgs e)
        {
            ValidateData();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            OpenHelpFile();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dg.DataSource = Ds;
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        { 
            /// <summary>
            /// Abort threads
            /// </summary>
            try
            {
                if (t1.IsAlive) t1.Abort();
                if (t2.IsAlive) t2.Abort();
            }
            catch
            {
            }
        }
      
  
    }
}
