using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MvcApplication1.Controllers
{
    public class MediumController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class3 cls = new Class3();
        [HttpPost]
        public HttpResponseMessage Medium([FromBody] Class2 Cls)
        {
            if (Cls.form_code == "insert")
            {
                string select = "select * from mst_medium_tbl where medium='" + Cls.medium + "' and del_flag=0";
                DataTable dtnew = cls.Filldt(select);
                if (dtnew.Rows.Count > 0)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "Exist", "application/json");
                }
                else
                {
                    string insertquery = "insert into mst_medium_tbl(medium,user_id,curr_date,del_flag) values ('" + Cls.medium.ToUpper() + "','" + Cls.user_id + "',getdate(),0)";
                    DataTable dt = cls.Filldt(insertquery);
                    return this.Request.CreateResponse(HttpStatusCode.OK, "inserted", "application/json");
                }
            }

            else if (Cls.form_code == "delete")
            {

                string updatetquery = "update mst_medium_tbl set del_flag=1 where med_id=" + Cls.med_id + " and del_flag=0";
                DataTable dt = cls.Filldt(updatetquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, "deleted", "application/json");

            }
            else if (Cls.form_code == "update")
            {
                string select = "select * from mst_medium_tbl where medium='" + Cls.medium + "' and del_flag=0";
                DataTable dtnew = cls.Filldt(select);
                if (dtnew.Rows.Count > 0)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "Exist", "application/json");
                }
                else
                {
                    string updatetquery = "update mst_medium_tbl set medium='" + Cls.medium.ToUpper() + "' where med_id=" + Cls.med_id + " and del_flag=0";
                    DataTable dt = cls.Filldt(updatetquery);
                    return this.Request.CreateResponse(HttpStatusCode.OK, "updated", "application/json");
                }

            }

            else if (Cls.form_code == "select")
            {
                DataSet gvdata = new DataSet();
                gvdata = cls.GetEmpData("select *,case when (select count(*) from mst_standard_tbl where del_flag=0 and med_id=m.med_id)>0 then 1 else 0 end as newcolumn from mst_medium_tbl m where m.del_flag=0");
                return this.Request.CreateResponse(HttpStatusCode.OK, gvdata, "application/json");
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "error", "application/json");
            }
        }

       

    }
}
