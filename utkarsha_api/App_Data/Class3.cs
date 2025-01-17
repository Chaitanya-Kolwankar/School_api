using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MvcApplication1
{
    public class Class3
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);

        public DataSet GetEmpData(string query)
        {

            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            con.Close();
            DataSet dt = new DataSet();
            sda.Fill(dt);
            return dt;

        }

        public DataTable Filldt(string query)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;

        }

        public DataSet fillds(string query)
        {
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.CommandTimeout = 100000;
            DataSet dt = new DataSet();
            da.Fill(dt);
            return dt;
        }
        public SqlDataAdapter filldr(string query)
        {
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            return da;
        }


        public bool fillup(string query)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            if (Convert.ToBoolean(cmd.ExecuteNonQuery()) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}