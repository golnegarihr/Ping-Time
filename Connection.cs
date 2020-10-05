
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb ;
namespace ping
{
   class connection
    {
        public void qurey(string str_val)
        {
            OleDbCommand   command = new OleDbCommand  ();
            OleDbConnection connection = new OleDbConnection (@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|db.mdb");
            command.Connection = connection;
            command.CommandText = str_val;
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void DatasetQuery(string q, string tbl, DataSet d)
        {
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
            OleDbConnection  connection = new OleDbConnection (@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|db.mdb");
            dataAdapter.SelectCommand = new OleDbCommand ();
            dataAdapter.SelectCommand.CommandText = q;
            dataAdapter.SelectCommand.Connection = connection;
            dataAdapter.SelectCommand.CommandType = CommandType.Text;
            connection.Open();
            dataAdapter.Fill(d, tbl);
            connection.Close();
            connection = null;
            dataAdapter = null;
        }
    }
}
