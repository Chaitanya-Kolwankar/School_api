using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace utkarsha_api
{
    public class marksentry
    {
        public string type { get; set; }
        public string med_id { get; set; }
        public string class_id { get; set; }
        public string ayid { get; set; }
        public string division { get; set; }
        public string examid { get; set; }
        public string mrk { get; set; }
        public string subject_id { get; set; }

        public string groupid { get; set; }

        public fillgridmarks[] mrkdata { get; set; }
    }

    public class fillgridmarks
    {
        public string stud_id { get; set; }
        public string marks { get; set; }
        public string exam_type { get; set; }
    }
}