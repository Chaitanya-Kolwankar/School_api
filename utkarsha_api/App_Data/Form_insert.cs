using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace utkarsha_api
{
    public class Form_insert
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        public string type { get; set; }
        public string data { get; set; }
        public string module_name { get; set; }
        public string Form_Name { get; set; }
        public string sr_no { get; set; }
        

    }
}