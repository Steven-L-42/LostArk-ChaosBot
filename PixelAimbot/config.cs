using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
//add this for MessageBox
using System.Windows.Forms;
// add data function classes
using System.Data;
namespace PixelAimbot
{
    public class Config
    {
        string ConectionString = "";  // save connection string
        public MySqlConnection connection = null;
        public string server = "sql11.freemysqlhosting.net";// MySQL host / ip of the computer
        public string user = "sql11480313";// MySQL user
        public string password = "6d9bdeU7UK";// MySQL password 
        DataSet ds;
        DataTable dt;
        public string Table = "sql11480313"; // initialize db table
        public string ConnectionType = "";
        string RecordSource = "";
        DataGridView tempdata;
        public Config()
        {
        }
        // function to connect to the database
        public void Connect(string database_name)
        {
            try
            {
                ConectionString = "SERVER=" + server + ";" + "DATABASE=" + database_name + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";";
                connection = new MySqlConnection(ConectionString);
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }
        // Function to execute select statements
        public void ExecuteSql(string Sql_command)
        {
            nowquiee(Sql_command);
        }
        // creates connection to MySQL before execution
        public void nowquiee(string sql_comm)
        {
            try
            {
                MySqlConnection cs = new MySqlConnection(ConectionString);
                cs.Open();
                MySqlCommand myc = new MySqlCommand(sql_comm, cs);
                myc.ExecuteNonQuery();
                cs.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        // function to execute delete , insert and update
        public void Execute(string Sql_command)
        {
            RecordSource = Sql_command;
            ConnectionType = Table;
            dt = new DataTable(ConnectionType);
            try
            {
                string command = RecordSource.ToUpper();
                //======================if sql contains select==========================================
                MySqlDataAdapter da2 = new MySqlDataAdapter(RecordSource, connection);
                DataSet tempds = new DataSet();
                da2.Fill(tempds, ConnectionType);
                da2.Fill(tempds);
                //======================================================================================
            }
            catch (Exception err) { MessageBox.Show(err.Message); }
        }
        // function to bring selected results based on column name and row index
        public string Results(int ROW, string COLUMN_NAME)
        {
            try
            {
                return dt.Rows[ROW][COLUMN_NAME].ToString();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                return "";
            }
        }
        // function to bring selected results based on column index and row index
        public string Results(int ROW, int COLUMN_NAME)
        {
            try
            {
                return dt.Rows[ROW][COLUMN_NAME].ToString();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                return dt.Rows[ROW][COLUMN_NAME].ToString();
            }
        }
        // Execute select statement
        public void ExecuteSelect(string Sql_command)
        {
            RecordSource = Sql_command;
            ConnectionType = Table;
            dt = new DataTable(ConnectionType);
            try
            {
                string command = RecordSource.ToUpper();
                MySqlDataAdapter da = new MySqlDataAdapter(RecordSource, connection);
                ds = new DataSet();
                da.Fill(ds, ConnectionType);
                da.Fill(dt);
                tempdata = new DataGridView();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        // count Number of rows after select
        public int Count()
        {
            return dt.Rows.Count;
        }
    }
}