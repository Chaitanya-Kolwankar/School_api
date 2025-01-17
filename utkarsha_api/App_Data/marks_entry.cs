using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace utkarsha_api
{
    public class marks_entry
    {
        public string type { get; set; }
        public string ayid { get; set; }
        public string medium { get; set; }
        public string standard { get; set; }
        public string exam_id { get; set; }
        public string subject_id { get; set; }
        public string division_id { get; set; }
        public string stud_id { get; set; }
        public string[] student_id { get; set; }
        public string[] theory_marks { get; set; }
        public string[] internal_marks { get; set; }
        public string[] practical_marks { get; set; }
        public string[] grade { get; set; }
        public string extra1 { get; set; }
        public string extra2 { get; set; }
        public string th_type { get; set; }
        public string int_type { get; set; }
        public string pr_type { get; set; }
        public string grd_type { get; set; }
    }
}