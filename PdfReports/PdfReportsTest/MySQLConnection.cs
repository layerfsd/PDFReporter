using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace axiomPdfTest
{
    public class MySQLConnection
    {
        private SqlConnection myConnection = null;

        public SqlConnection MyConnection
        {
            get { return myConnection; }
            set { myConnection = value; }
        }

        private SqlDataAdapter myCompanyAdapter = null;

        public SqlDataAdapter MyCompanyAdapter
        {
            get { return myCompanyAdapter; }
            set { myCompanyAdapter = value; }
        }

        private SqlDataAdapter myManufacturersAdapter = null;

        public SqlDataAdapter MyManufacturersAdapter
        {
            get { return myManufacturersAdapter; }
            set { myManufacturersAdapter = value; }
        }

        private SqlDataAdapter myProductsAdapter = null;

        public SqlDataAdapter MyProductsAdapter
        {
            get { return myProductsAdapter; }
            set { myProductsAdapter = value; }
        }

        private SqlDataAdapter myBanksAdapter = null;

        public SqlDataAdapter MyBanksAdapter
        {
            get { return myBanksAdapter; }
            set { myBanksAdapter = value; }
        }


        private SqlDataReader myReader = null;

        public SqlDataReader MyReader
        {
            get { return myReader; }
            set { myReader = value; }
        }


        private SqlCommand myCommand = null;

        public SqlCommand MyCommand
        {
            get { return myCommand; }
            set { myCommand = value; }
        }

        private DataTable myTable = new DataTable();

        public DataTable MyTable
        {
            get { return myTable; }
            set { myTable = value; }
        }
    }
}
