using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace utkarsha_api
{
    public class result
    {
        String cs = ConfigurationManager.ConnectionStrings["connect"].ConnectionString;
        public string type { get; set; }
        public string med_id { get; set; }
        public string std_id { get; set; }
        public string div_id { get; set; }
        public string ayid { get; set; }
        public string exam_id { get; set; }
        public string stud_id { get; set; }
        public string std { get; set; }
       

    }
}