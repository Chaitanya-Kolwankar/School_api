using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace utkarsha_api
{
    public class Subject_Master
    {
        public string medium_id { get; set; }
        public string class_id { get; set; }
        public string subject_id { get; set; }
        public string subject_name { get; set; }
        public string criteria { get; set; }
        public string AYID { get; set; }
        public string username { get; set; }
        public string table { get; set; }
        public string type { get; set; }
    }

   



}