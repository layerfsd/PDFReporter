using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;


namespace axiomPdfTest
{
    public partial class MainTestForm : Form
    {
        public MainTestForm()
        {
            InitializeComponent();
        }

        private MySQLConnection mySQLConnection = new MySQLConnection();

        private delegate void AddResultTextDelegate(string txt);
        private delegate void AddResultDataDelegate(SqlDataAdapter adapter);

        private void AddResultText(string txt)
        {
            lbxResults.Items.Add(txt);
        }

        private void AddResultData(SqlDataAdapter adapter)
        {
            lbxResults.Items.Add(adapter);
        }

        /// <summary>
        /// This will make sure reports are generated from database
        /// </summary>
        private void GenerateFromDatabase()
        {
            try
            {
                tabDataDesc.SelectedTab = tabResults;
                lbxResults.Items.Clear();
                lbxResults.Items.Add("Generating...");
                lblStatus.Text = "Preparing Data...";
                this.Cursor = Cursors.WaitCursor;
                DataSet mainDataSet = new DataSet();

                if(rbtDemoData1.Checked)
                {

                    ThreadStart ts = new ThreadStart(
                    delegate()
                    {
                        mySQLConnection.MyConnection.Open();
                        string query = "SELECT TOP 10 * FROM Company\nGO\n SELECT TOP 10 * FROM Manufacturer \n GO \nSELECT TOP 10 * FROM Product";
                        mySQLConnection.MyCompanyAdapter = new SqlDataAdapter(query, mySQLConnection.MyConnection);
                        mySQLConnection.MyCompanyAdapter.Fill(mainDataSet);
                        mySQLConnection.MyConnection.Close();
                    }
                    );
                    Thread thread = new Thread(ts);
                    thread.IsBackground = true;
                    thread.Start();

                    while(thread.ThreadState == System.Threading.ThreadState.Background)
                    {
                        Application.DoEvents();
                    }
                }
                if(rbtDemoData2.Checked)
                {
                }
                if(rbtDemoData3.Checked)
                {
                }


                if(rbtDemoData1.Checked)
                {
                    AxiomCoders.PdfReports.PdfReports pdfReporter = new AxiomCoders.PdfReports.PdfReports(txtCompanyName.Text, txtSerial.Text);
                    mainDataSet.Tables[0].TableName = "Company";
                    mainDataSet.Tables[1].TableName = "Manufacturer";
                    mainDataSet.Tables[2].TableName = "Product";                    
                    //mainDataSet.Relations.Add("Rel1", mainDataSet.Tables["Manufacturer"].Columns["Id"], mainDataSet.Tables["Product"].Columns["ManufacturerId"]);

                    
                    pdfReporter.DataSources.Add(mainDataSet);
                    pdfReporter.TemplateFileName = txtTemplateFile.Text;
                    pdfReporter.Logging = true;
                    pdfReporter.ProgressChanged += pdfReporter_ProgressChanged;
                    pdfReporter.GeneratePdf(txtPDF.Text);
                }

                lblStatus.Text = "Generated.";
                this.Cursor = Cursors.Default;
            }
            catch(Exception ex)
            {
                lblStatus.Text = "Generating Failed...";
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// This will generate reports with template data from our local info
        /// </summary>
        private void GenerateFromTemplateData()
        {
            try
            {
                tabDataDesc.SelectedTab = tabResults;
                lbxResults.Items.Clear();
                lbxResults.Items.Add("Generating...");
                lblStatus.Text = "Preparing Data...";
                this.Cursor = Cursors.WaitCursor;
                Generator.Manufacturers.Clear();
                Generator.Products.Clear();
                Generator.Banks.Clear();

                if(rbtDemoData1.Checked)
                {
                    if(rbtDataSourceApplication.Checked)
                    {
                        ThreadStart ts = new ThreadStart(
                         delegate()
                         {
                             Generator.GenerateDemo1(int.Parse(edtNumberOfItems.Text));
                             this.Invoke(new AddResultTextDelegate(AddResultText), new object[] { Generator.GeneratedCompany.ToString() });
                             foreach(Manufacturer man in Generator.Manufacturers)
                             {
                                 this.Invoke(new AddResultTextDelegate(AddResultText), new object[] { man.ToString() });
                                 foreach(Product pro in man.Products)
                                 {
                                     this.Invoke(new AddResultTextDelegate(AddResultText), new object[] { "\t" + pro.ToString() });
                                 }
                             }
                         }
                         );
                        Thread thread = new Thread(ts);
                        thread.IsBackground = true;
                        thread.Start();

                        while(thread.ThreadState == System.Threading.ThreadState.Background)
                        {
                            Application.DoEvents();
                        }
                    }
                    else
                    {

                    }
                }
                if(rbtDemoData2.Checked)
                {
                    if(rbtDataSourceApplication.Checked)
                    {
                        ThreadStart ts = new ThreadStart(
                       delegate()
                       {
                           Generator.GenerateDemo2(int.Parse(edtNumberOfItems.Text));
                           this.Invoke(new AddResultTextDelegate(AddResultText), new object[] { Generator.GeneratedCompany.ToString() });
                           foreach(Product product in Generator.Products)
                           {
                               this.Invoke(new AddResultTextDelegate(AddResultText), new object[] { product.ToString() });

                               foreach(Price price in product.Prices)
                               {
                                   this.Invoke(new AddResultTextDelegate(AddResultText), new object[] { "\t" + price.ToString() });
                               }
                           }
                       }
                       );
                        Thread thread = new Thread(ts);
                        thread.IsBackground = true;
                        thread.Start();

                        while(thread.ThreadState == System.Threading.ThreadState.Background)
                        {
                            Application.DoEvents();
                        }
                    }
                    else
                    {
                        // TODO: make db tables and etc
                    }
                }
                if(rbtDemoData3.Checked)
                {
                    if(rbtDataSourceApplication.Checked)
                    {
                        ThreadStart ts = new ThreadStart(
                        delegate()
                        {
                            Generator.GenerateDemo3(int.Parse(edtNumberOfItems.Text));
                            foreach(Bank bank in Generator.Banks)
                            {
                                this.Invoke(new AddResultTextDelegate(AddResultText), new object[] { bank.ToString() });
                                foreach(Client client in bank.Clients)
                                {
                                    this.Invoke(new AddResultTextDelegate(AddResultText), new object[] { "\t" + client.ToString() });
                                    foreach(Account account in client.Accounts)
                                    {
                                        this.Invoke(new AddResultTextDelegate(AddResultText), new object[] { "\t\t" + account.ToString() });
                                    }
                                }
                            }
                        }
                        );
                        Thread thread = new Thread(ts);
                        thread.IsBackground = true;
                        thread.Start();

                        while(thread.ThreadState == System.Threading.ThreadState.Background)
                        {
                            Application.DoEvents();
                        }
                    }
                    else
                    {
                        // TODO: make db tables and etc
                    }
                }

                lblStatus.Text = "Generating PDF...";

                // Generate pdf
                if(rbtDemoData1.Checked)
                {
                    AxiomCoders.PdfReports.PdfReports pdfReporter = new AxiomCoders.PdfReports.PdfReports(txtCompanyName.Text, txtSerial.Text);

                    if(rbtDataSourceApplication.Checked)
                    {
                        pdfReporter.DataSources.Add(Generator.GeneratedCompany);
                        pdfReporter.DataSources.Add(Generator.Manufacturers);
                    }
                    else
                    {
                        // TODO:
                    }
                    pdfReporter.TemplateFileName = txtTemplateFile.Text;
                    pdfReporter.Logging = true;
                    pdfReporter.ProgressChanged += new AxiomCoders.PdfReports.PdfReports.OnProgreeCallback(pdfReporter_ProgressChanged);
                    pdfReporter.GeneratePdf(txtPDF.Text);
                }
                if(rbtDemoData2.Checked)
                {
                    AxiomCoders.PdfReports.PdfReports pdfReporter = new AxiomCoders.PdfReports.PdfReports(txtCompanyName.Text, txtSerial.Text);
                    if(rbtDataSourceApplication.Checked)
                    {
                        pdfReporter.DataSources.Add(Generator.GeneratedCompany);
                        pdfReporter.DataSources.Add(Generator.Products);
                    }
                    else
                    {
                        // TODO:
                    }
                    pdfReporter.TemplateFileName = txtTemplateFile.Text;
                    pdfReporter.Logging = true;
                    pdfReporter.ProgressChanged += new AxiomCoders.PdfReports.PdfReports.OnProgreeCallback(pdfReporter_ProgressChanged);
                    pdfReporter.GeneratePdf(txtPDF.Text);
                }
                if(rbtDemoData3.Checked)
                {
                    AxiomCoders.PdfReports.PdfReports pdfReporter = new AxiomCoders.PdfReports.PdfReports(txtCompanyName.Text, txtSerial.Text);
                    if(rbtDataSourceApplication.Checked)
                    {
                        pdfReporter.DataSources.Add(Generator.Banks);
                    }
                    else
                    {
                        // TODO:
                    }
                    pdfReporter.TemplateFileName = txtTemplateFile.Text;
                    pdfReporter.Logging = true;
                    pdfReporter.ProgressChanged += new AxiomCoders.PdfReports.PdfReports.OnProgreeCallback(pdfReporter_ProgressChanged);
                    pdfReporter.GeneratePdf(txtPDF.Text);
                }

                lblStatus.Text = "Generated.";
                this.Cursor = Cursors.Default;
            }
            catch(Exception ex)
            {
                lblStatus.Text = "Generating Failed...";
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        int progress = 0;

        void pdfReporter_ProgressChanged(int itemProcessed, int totalCount, ref bool cancel)
        {
            if(totalCount > 0)
            {
                lblStatus.Text = string.Format("Progress.. {0}", progress++);
            }
            Invalidate();
            Application.DoEvents();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if(rbtDataSourceDatabase.Checked)
            {
                GenerateFromDatabase();
            }
            else
            {
                GenerateFromTemplateData();

            }
        }


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OD.InitialDirectory = Application.StartupPath;
            if(OD.ShowDialog(this) == DialogResult.OK)
            {
                txtTemplateFile.Text = OD.FileName;
                txtPDF.Text = System.IO.Path.GetFileNameWithoutExtension(OD.FileName) + ".pdf";
            }
        }

        private void btnBrowsePdfOutput_Click(object sender, EventArgs e)
        {
            if(SD.ShowDialog(this) == DialogResult.OK)
            {
                txtPDF.Text = SD.FileName;
            }

        }

        private void chkDemoData1_MouseEnter(object sender, EventArgs e)
        {
            tabDataDesc.SelectedTab = tabData1;
        }

        private void checkBox2_MouseEnter(object sender, EventArgs e)
        {
            tabDataDesc.SelectedTab = tabData2;
        }

        private void checkBox3_MouseEnter(object sender, EventArgs e)
        {
            tabDataDesc.SelectedTab = tabData3;
        }

        private void rbtDemoData1_Click(object sender, EventArgs e)
        {
            tabDataDesc.SelectedTab = tabData1;
        }

        private void rbtDemoData2_Click(object sender, EventArgs e)
        {
            tabDataDesc.SelectedTab = tabData2;
        }

        private void rbtDemoData3_Click(object sender, EventArgs e)
        {
            tabDataDesc.SelectedTab = tabData3;
        }

        private void btnDatabaseConnection_Click(object sender, EventArgs e)
        {
            SQL_Connection connectionForm = new SQL_Connection();
            connectionForm.ShowConnectionWindow(mySQLConnection);
        }

        private void Form1_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if(mySQLConnection.MyConnection != null)
            {
                try
                {
                    mySQLConnection.MyConnection.Close();
                }
                catch(System.Exception exc)
                {
                    MessageBox.Show(exc.Message, "Closing failed!");
                }
            }
        }


        int itemCounter = 0;
        byte[] tmpImage;
        int numOfItems = 60;

        /// <summary>
        /// Callback that is used for initializing your data.
        /// </summary>
        /// <param name="parentDataStream"></param>
        /// <param name="dataStreamName"></param>
        /// <param name="itemsCount"></param>
        /// <returns></returns>
        bool InitDataCallback(string parentDataStream, string dataStreamName, ref System.Int32 itemsCount)
        {
            //This callback is used for initializing your data stack, what ever it is. In our case
            //it's simple file reading, and file is JPEG image on hard drive.
            //Remember that this callback is called only once for each DataStrem.
            tmpImage = System.IO.File.ReadAllBytes("../../Depending/Image1.jpg");
            return true;
        }




        /// <summary>
        /// In this callback we are going through our data
        /// </summary>
        /// <param name="dataStreamName"></param>
        /// <returns></returns>
        bool ReadDataCall(string dataStreamName)
        {
            //In here you move your pointer through data base or move in your array
            //or anything else you have for data storage.
            //Return false when you done reading.
            if(dataStreamName == "Data1")
            {
                if(itemCounter == numOfItems)
                {
                    return false;
                }
                itemCounter++;
                return true;
            }
            return false;
        }



        /// <summary>
        /// This callback returning actual data if data is byte array
        /// </summary>
        /// <param name="dataStreamName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        byte[] ReqDataCall(string dataStreamName, string columnName)
        {
            //In here we check to see what kind of data is requested by generator and
            //if we have that kind of data return it. In this case, 
            //that is byte array that contains Image data from specific folder.
            if(dataStreamName == "Data1" && columnName == "Column1")
            {
                return tmpImage;
            }
            return null;
        }



        /// <summary>
        /// This callback returning actual data if data is represented as string
        /// </summary>
        /// <param name="dataStreamName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        string ReqStringCall(string dataStreamName, string columnName)
        {
            //In here we check to see what kind of data is requested by generator and
            //if we have that kind of data return it. In this example that is simple text line.
            //You can give it what ever you want, if you going through data base, then it is current data.
            if(dataStreamName == "Data1" && columnName == "Column2")
            {
                return "String from Callback";
            }
            return null;
        }



        /// <summary>
        /// This is our main function where we create generator, initialize all callbacks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCallbackGeneration_Click(object sender, EventArgs e)
        {
            //First we need to create our Generator...  (Remember to use your serial number as param in constructor)
            AxiomCoders.PdfReports.PdfReports pdfReporter = new AxiomCoders.PdfReports.PdfReports(txtCompanyName.Text, txtSerial.Text);

            //Creation of all needed callbacks
            pdfReporter.InitializeDataStream += new AxiomCoders.PdfReports.InitializeDataStreamCallback(InitDataCallback);
            pdfReporter.ReadData += new AxiomCoders.PdfReports.ReadDataCallback(ReadDataCall);
            pdfReporter.RequestBinaryData += new AxiomCoders.PdfReports.RequestBinaryDataCallback(ReqDataCall);
            pdfReporter.RequestStringData += new AxiomCoders.PdfReports.RequestStringDataCallback(ReqStringCall);

            //Set template file name, and call actual generating of you PDF file.
            pdfReporter.TemplateFileName = txtTemplateFile.Text;
            pdfReporter.Logging = true;
            pdfReporter.GeneratePdf(txtPDF.Text);
        }
    }
}