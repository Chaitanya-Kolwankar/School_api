using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;
using utkarsha_api.App_Start;

namespace utkarsha_api.Controllers
{
    public class agereportController : ApiController
    {
       DataSet ds1 = new DataSet();
        string strquery = "";
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();

        [HttpPost]
        public HttpResponseMessage Agereport([FromBody]agereport age)
        {
            if (age.type == "age")
            {
                strquery = "declare @now datetime select @now='" + age.currdate.Replace('/', '-') + "' select count((CONVERT(int,CONVERT(char(8),@now,112))-CONVERT(char(8),a.dob,112))/10000) AS Noofmale,(CONVERT(int,CONVERT(char(8),@now,112))-CONVERT(char(8),a.dob,112))/10000 AS AgeIntYears from adm_student_master as a,adm_studentacademicyear as b,mst_standard_tbl as c where a.Student_id=b.student_id and b.AYID='" + age.Ayid + "' and c.std_id=b.class_id  and b.medium_id='" + age.med_id + "'  and b.class_id='" + age.class_id + "' and b.del_flag=0  and a.del_flag=0  and a.gender='Male'   group by ((CONVERT(int,CONVERT(char(8),@now,112))-CONVERT(char(8),a.dob,112))/10000 ) ; select count((CONVERT(int,CONVERT(char(8),@now,112))-CONVERT(char(8),a.dob,112))/10000) AS Nooffemale,(CONVERT(int,CONVERT(char(8),@now,112))-CONVERT(char(8),a.dob,112))/10000 AS AgeIntYears from adm_student_master as a,adm_studentacademicyear as b,mst_standard_tbl as c where a.Student_id=b.student_id and b.AYID='" + age.Ayid + "' and c.std_id=b.class_id  and b.medium_id='" + age.med_id + "' and b.class_id='" + age.class_id + "' and b.del_flag=0  and a.del_flag=0  and a.gender='Female'   group by ((CONVERT(int,CONVERT(char(8),@now,112))-CONVERT(char(8),a.dob,112))/10000 ) ;select count(*) as TotalFemale from adm_student_master as a,adm_studentacademicyear as b,mst_standard_tbl as c where a.Student_id=b.student_id and b.AYID='" + age.Ayid + "' and c.std_id=b.class_id  and b.medium_id='" + age.med_id + "' and b.class_id='" + age.class_id + "' and b.del_flag=0  and a.del_flag=0 and a.gender='Female'; select count(*) as TotalMale from adm_student_master as a,adm_studentacademicyear as b,mst_standard_tbl as c where a.Student_id=b.student_id and b.AYID='" + age.Ayid + "' and c.std_id=b.class_id  and b.medium_id='" + age.med_id + "' and b.class_id='" + age.class_id + "' and b.del_flag=0  and a.del_flag=0 and a.gender='Male'";
                
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if(age.type=="class")
            {
                strquery =  "select std_id,std_name from mst_standard_tbl where med_id='" + age.med_id + "' and del_flag=0";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "No Changes Made", "application/json");
            }
        }
    }
}
