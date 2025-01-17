using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using utkarsha_api.App_Start;

namespace utkarsha_api.Controllers
{
    public class socialcategoryController : ApiController
    {
        DataSet ds1 = new DataSet();
        string strquery = "";
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();

        [HttpPost]
        public HttpResponseMessage Agereport([FromBody]social_ctg socat)
        {
            if (socat.type == "religion")
            {
                strquery = "select count(a.religion_id) countf, a.religion_id from mst_religion_tbl a,adm_student_master b,adm_studentacademicyear c,mst_standard_tbl d where a.religion_id=b.religion and b.Student_id=c.student_id and c.class_id=d.std_id and c.AYID='" + socat.Ayid + "' and c.medium_id='" + socat.med_id + "' and c.class_id='" + socat.class_id + "' and b.gender='Female' and a.religion_id='" + socat.religion_id + "' and a.del_flag=0 and b.del_flag=0 and c.del_flag=0 and d.del_flag=0 group by a.religion_id ; select count(a.religion_id) as countm, a.religion_id from mst_religion_tbl a,adm_student_master b,adm_studentacademicyear c,mst_standard_tbl d where a.religion_id=b.religion and b.Student_id=c.student_id and c.class_id=d.std_id and c.AYID='" + socat.Ayid + "' and c.medium_id='" + socat.med_id + "' and c.class_id='" + socat.class_id + "' and b.gender='Male' and a.religion_id='" + socat.religion_id + "' and a.del_flag=0 and b.del_flag=0 and c.del_flag=0 and d.del_flag=0 group by a.religion_id";

                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if(socat.type=="category")
            {
                strquery = "select count(a.category_id) countf, a.category_id from mst_category_tbl a,adm_student_master b,adm_studentacademicyear c,mst_standard_tbl d where a.category_id=b.category and b.Student_id=c.student_id and c.class_id=d.std_id and c.AYID='" + socat.Ayid + "' and c.medium_id='" + socat.med_id + "' and c.class_id='" + socat.class_id + "' and b.gender='Female' and a.category_id='" + socat.category_id + "' and a.del_flag=0 and b.del_flag=0 and c.del_flag=0 and d.del_flag=0 group by a.category_id ; select count(a.category_id) as countm, a.category_id from mst_category_tbl a,adm_student_master b,adm_studentacademicyear c,mst_standard_tbl d where a.category_id=b.category and b.Student_id=c.student_id and c.class_id=d.std_id and c.AYID='" + socat.Ayid + "' and c.medium_id='" + socat.med_id + "' and c.class_id='" + socat.class_id + "' and a.category_id='" + socat.category_id + "' and b.gender='Male'  and a.del_flag=0 and b.del_flag=0 and c.del_flag=0 and d.del_flag=0 group by a.category_id ; ";
                strquery = strquery + "select count(a.category_id) countftotal, a.category_id from mst_category_tbl a,adm_student_master b,adm_studentacademicyear c,mst_standard_tbl d where a.category_id=b.category and b.Student_id=c.student_id and c.class_id=d.std_id and c.AYID='" + socat.Ayid + "' and c.medium_id='" + socat.med_id + "' and c.class_id='" + socat.class_id + "' and b.gender='Female'  and a.del_flag=0 and b.del_flag=0 and c.del_flag=0 and d.del_flag=0 group by a.category_id ; select count(a.category_id) as countmtotal, a.category_id from mst_category_tbl a,adm_student_master b,adm_studentacademicyear c,mst_standard_tbl d where a.category_id=b.category and b.Student_id=c.student_id and c.class_id=d.std_id and c.AYID='" + socat.Ayid + "' and c.medium_id='" + socat.med_id + "' and c.class_id='" + socat.class_id + "' and b.gender='Male'  and a.del_flag=0 and b.del_flag=0 and c.del_flag=0 and d.del_flag=0 group by a.category_id";
      ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (socat.type == "class")
            {
                strquery = "select std_id,std_name from mst_standard_tbl where med_id='" + socat.med_id + "' and del_flag=0";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (socat.type == "religioncategory")
            {
                strquery = "select category_id,category_name from mst_category_tbl where del_flag=0 ; select religion_id,religion from mst_religion_tbl where del_flag=0";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (socat.type == "aadharbpl")
            {
                strquery = "select count(a.aadhar_no) as countf from adm_student_master as a, adm_studentacademicyear as b, mst_standard_tbl as c where a.Student_id=b.student_id and b.class_id=c.std_id and b.AYID='" + socat.Ayid + "'   and b.medium_id='" + socat.med_id + "' and b.class_id='" + socat.class_id + "' and a.gender='Female'  and a.aadhar_no!='' and   a.aadhar_no is not null  and a.del_flag=0 and b.del_flag=0 and c.del_flag=0 ;select count(a.aadhar_no) as countm from adm_student_master as a, adm_studentacademicyear as b, mst_standard_tbl as c where a.Student_id=b.student_id and b.class_id=c.std_id and b.AYID='" + socat.Ayid + "'   and b.medium_id='" + socat.med_id + "' and b.class_id='" + socat.class_id + "' and a.gender='Male'  and a.aadhar_no!='' and   a.aadhar_no is not null  and a.del_flag=0 and b.del_flag=0 and c.del_flag=0 ";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (socat.type == "transgender")
            {
                strquery = "select count(b.gender) as countt from adm_student_master b,adm_studentacademicyear c,mst_standard_tbl d where b.Student_id=c.student_id and c.class_id=d.std_id and c.AYID='" + socat.Ayid + "' and c.medium_id='" + socat.med_id + "' and c.class_id='" + socat.class_id + "' and b.gender='Transgender'  and b.del_flag=0 and c.del_flag=0 and d.del_flag=0 ";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }

            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "No changes made", "application/json");
            }
        }
    }
}
