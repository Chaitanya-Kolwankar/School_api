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
    public class feereportController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();
        DataSet ds = new DataSet();
        string str = "";

        [HttpPost]
        public HttpResponseMessage feereport([FromBody] fee_rpt fr)
        {

            string qext = "", qtotext = "";
            if(fr.type=="feereport")
            {
                if(fr.med_id!="")
                {
                    qext = " and medium_id='"+fr.med_id+"' ";
                }
                if (fr.class_id != "")
                {
                    qext = qext+ " and  class_id='" + fr.class_id + "' ";
                }
                if (fr.div_id != "")
                {
                    qext = qext + " and division_id='" + fr.div_id + "' ";
                }
                string query = "select * from [student_feerpt_view] where AYID='" + fr.ayid + "' " + qext + " order by ayid,medium_id,std_rank,student_id,division_id,duration_id,feemaster_rank,Roll_no,gr_no,Name,pay_date";
                ds = cls.fillds(query);
                return this.Request.CreateResponse(HttpStatusCode.OK,ds,"application/json");
            }
            else if (fr.type == "summarised")
            {
                if (fr.med_id != "")
                {
                    qext = " and medium_id='" + fr.med_id + "' ";
                    qtotext = " and med_id='" + fr.med_id + "' ";
                }
                if (fr.class_id != "")
                {
                    qext = qext + " and  class_id='" + fr.class_id + "' ";
                    qtotext = qtotext + " and  class_id='" + fr.class_id + "' ";
                }
                if (fr.div_id != "")
                {
                    qext = qext + " and division_id='" + fr.div_id + "' ";
                }
                string query = "select student_id,AYID,Roll_no,gr_no,Name,gender,medium,std_name,division_name,medium_id,class_id,sum( cast (Amount as int)) as Amount from [student_feerpt_view] where AYID='" + fr.ayid + "' " + qext + "  group by student_id,AYID,Roll_no,gr_no,Name,gender,medium,std_name,division_name,medium_id,class_id order by ayid,medium_id,class_id,student_id,Roll_no,gr_no,Name;select AYID,med_id,class_id,sum(cast(Amount as int)) as Totfees from mst_fee_master where del_flag=0 and  AYID='" + fr.ayid + "' " + qtotext + " group by AYID,med_id,class_id ";
                ds = cls.fillds(query);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
             
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "No changes made", "application/json");
            }

            
        }
    }
}
