using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace utkarsha_api
{
    public class Exam_Master
    {
        public string type { get; set; }
        public string exam_id { get; set; }
        public string exam_name { get; set; }
        public string subject_name { get; set; }
        public string medium_id { get; set; }
        public string class_id { get; set; }
        public string exam_type { get; set; }
        public string examtype_name { get; set; }
        public string out_of { get; set; }
        public string passing { get; set; }
        public string criteria { get; set; }
        public string ayid { get; set; }
        public string subject_id { get; set; }
        public string type2 { get; set; }
        public string std { get; set; }
        public string ref_id { get; set; }
        public string username { get; set; }

    }
}