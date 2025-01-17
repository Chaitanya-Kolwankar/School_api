using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace utkarsha_api
{
    public class ModifyData
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        public string medium_id { get; set; }
        public string class_id { get; set; }
        public string div { get; set; }
        public string data { get; set; }
        public string[] col2 { get; set; }
        public string type { get; set; }
        public string ayid { get; set; }
        

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
    }
}