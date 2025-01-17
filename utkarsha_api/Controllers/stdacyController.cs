using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace utkarsha_api.Controllers
{
    public class stdacyController : ApiController
    {

        DataSet ds1 = new DataSet();
        string strquery = "";
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();
        [HttpPost]
        public HttpResponseMessage loaddata([FromBody] std s)
        {
            if (s.type == "load")
            {
                
                if (s.firstname == null)
                {
                    if (s.id == null)
                    {
                        strquery = "select  isnull(stud_L_name,'') + '   '+ isnull(stud_F_name,'')+'   '+isnull(stud_m_name,'')  as student ,student_id from adm_student_master where  stud_L_name like'" + s.name + "%'";
                        ds1 = cls.fillds(strquery);

                        ds1.Tables[0].TableName = "adm_student_master";
                        return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
                    }
                    else {
                        strquery = "select  isnull(stud_L_name,'') + '   '+ isnull(stud_F_name,'')+'   '+isnull(stud_m_name,'')  as student ,student_id from adm_student_master where  Student_id ='" + s.id + "'";
                        ds1 = cls.fillds(strquery);

                        ds1.Tables[0].TableName = "adm_student_master";
                        return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
                    }
                }
                else {
                    strquery = "select  isnull(stud_L_name,'') + '   '+ isnull(stud_F_name,'')+'   '+isnull(stud_m_name,'')  as student ,student_id from adm_student_master  where stud_L_name like'"+s.name+"%' and stud_F_name like '"+s.firstname+"%'";

                    ds1 = cls.fillds(strquery);
                    ds1.Tables[0].TableName = "adm_student_master";
                    return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
                }
            }
            else if (s.type == "load1")
            {
                string strquery = "exec [pro_stud_academic_record] ";
                strquery = strquery + " '" + s.id + "'";
                ds1 = cls.fillds(strquery);
                ds1.Tables[0].TableName = "view_student_academic_records";
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (s.type == "app_stud_search") {

                strquery = "select c.Student_id, a.medium ,b.std_name,d.Roll_no,c.phone_no1,c.dob,c.student_photo,isnull(c.stud_L_name,'') + ' '+ isnull(c.stud_F_name,'')+'   '+isnull(c.stud_m_name,'')  as student,c.address from mst_medium_tbl as a,mst_standard_tbl as b, adm_student_master as c ,adm_studentacademicyear as d where c.Student_id='" + s.id + "' and a.med_id=b.med_id and b.med_id=c.medium_id and a.med_id=c.medium_id and a.med_id=d.medium_id and b.med_id=d.medium_id and c.medium_id=d.medium_id and b.std_id=c.class_id and b.std_id=d.class_id and c.class_id=d.class_id and c.del_flag=0 and d.student_id=c.Student_id  ";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");









            }
                //else if(s.type=="app_stud_search_names"){
                //    if(s.firstname==null){
                //    strquery = "";
                //    DataSet ds = cls.fillds(strquery);
                //    return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");}
                //}
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "No Changes Made", "application/json");
            }
        }

    }
}
