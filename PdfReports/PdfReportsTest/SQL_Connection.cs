using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace axiomPdfTest
{
    public partial class SQL_Connection : Form
    {
        //private SqlConnection myConnection = null;
        private MySQLConnection mySQLConnection = null;

        public SQL_Connection()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string username = "user id=" + txtBoxUserName.Text + ";";
            string password = "password=" + txtBoxPassword.Text + ";";
            string server = "server=" + txtBoxServerName.Text + ";";
            string trusted = "Trusted_Connection=no;";
            string database = "database=" + txtBoxDatabaseName.Text + ";";
            string timeout = "connection timeout=30;";

            mySQLConnection.MyConnection = new SqlConnection(username + password + server + trusted + database + timeout);
            bool connection_sucsess = true;

            try
            {
                mySQLConnection.MyConnection.Open();
            }
            catch (System.Exception exc)
            {
                connection_sucsess = false;
                MessageBox.Show(exc.Message);
            }

            if (connection_sucsess)
            {
                try
                {
                    mySQLConnection.MyConnection.Close();
                }
                catch (System.Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }else{
                MessageBox.Show("Connection failed!");
                mySQLConnection.MyConnection = null;
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            try
            {
                mySQLConnection.MyConnection.Close();
            }
            catch(System.Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            this.Close();
        }

        public void ShowConnectionWindow(MySQLConnection connection)
        {
            if(connection == null)
            {
                MessageBox.Show("Error loading connection window!", "Error", MessageBoxButtons.OK);
                return;
            }
            mySQLConnection = connection;
            this.ShowDialog();
        }
    }
}