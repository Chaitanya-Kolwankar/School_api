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
    public class Statistical_ReportController : ApiController
    {
         SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();
        DataSet ds = new DataSet();
        string type;
        

        string strquery;

        [HttpPost]
        public HttpResponseMessage statisticalreport([FromBody] Statistical_report sr)
        {
            if (sr.type == "datewise")
            {
                string q1 = "select count(*) as Count from adm_student_master as s, mst_standard_tbl as m where s.class_id=m.std_id and  s.ayid='" + sr.ayid + "' and s.medium_id='"+sr.medium+"'; ";
                q1 += " select count(*) as Count from adm_student_master as s, mst_standard_tbl as m where s.class_id=m.std_id and  s.ayid='" + sr.ayid + "' and s.medium_id='" + sr.medium + "' and Student_id is not null; ";
                q1+= " select (select std_name from mst_standard_tbl where std_id=Class) as Class ,convert(varchar,Date ,105) as [Date],[No. of Admissions] from(  select class_id as Class ,convert(date,s.curr_dt ,105) as [Date],count(*) as [No. of Admissions]  from adm_student_master as s join dbo.mst_standard_tbl as m on m.std_id = s.class_id   where s.ayid = '" + sr.ayid + "'  and medium_id='"+sr.medium+"' and s.del_flag=0  group by class_id,convert(date,s.curr_dt ,105))a order by convert(date,[Date],105) desc; ";

                ds = cls.fillds(q1);
            }
            if (sr.type == "sgvfill")
            {
                  string q1 = "select count(*) as Count from adm_student_master as s, mst_standard_tbl as m where s.class_id=m.std_id and  s.ayid='" + sr.ayid + "' and s.medium_id='"+sr.medium+"'; ";
                q1+= " select count(*) as Count from adm_student_master as s, mst_standard_tbl as m where s.class_id=m.std_id and  s.ayid='" + sr.ayid + "' and s.medium_id='" + sr.medium + "' and Student_id is not null; ";
                q1+= " select a.std_name as Class,count(*) as [No. of Admissions] from mst_standard_tbl a,adm_student_master b where a.std_id=b.class_id and b.AYID='" + sr.ayid + "' and b.medium_id='" + sr.medium + "' group by a.std_name,a.rank order by a.rank asc; ";

                ds = cls.fillds(q1);
            }
            if (sr.type == "cgvfill")
            {
                string q1 = "select count(*) as Count from adm_student_master as s, mst_standard_tbl as m where s.class_id=m.std_id and  s.ayid='" + sr.ayid + "' and s.medium_id ='" + sr.medium + "' and s.del_flag='1' ";
                q1+= "select (select std_name from mst_standard_tbl where std_id=Class) class,convert(varchar,Date ,105) as [Date],[No. of Admissions Cancelled] from(  select class_id as Class ,convert(date,s.admcancel_date ,106) as [Date],count(*) as [No. of Admissions Cancelled] from adm_student_master as s join dbo.mst_standard_tbl as m on m.std_id = s.class_id where s.ayid = '" + sr.ayid + "' and s.medium_id='" + sr.medium + "'and s.del_flag=1 group by class_id,convert(date,s.admcancel_date ,106) ) a order by convert(date,[Date],105) desc";
                
                ds = cls.fillds(q1);
            }
            if (sr.type == "graph")
            {
                string strquery = "select (select std_name from mst_standard_tbl where std_id=a.std_id) as Class  ,(select count (class_id) from adm_student_master as b where a.std_id=b.class_id and b.AYID='" + sr.ayid + "  ' and b.medium_id=a.med_id) as Admission from mst_standard_tbl as a where a.med_id='" + sr.medium + "'";

                ds = cls.fillds(strquery);  
            }
            return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
        }
    }
}
