using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med_a
{
    public class SQLHandler
    {
        public static SqlConnection con = new SqlConnection(@"Data Source=ALEXWORKSTATION;Initial Catalog=Med!a DB;Integrated Security=True");
        public static SqlCommand cmd = new SqlCommand();
        public static SqlDataReader read;

        public static void executeQuery(string query)
        {
            con.Close();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
        }

        public static void selectQuery(string query)
        {
            con.Close();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = query;
            read = cmd.ExecuteReader();
        }
    }
}
