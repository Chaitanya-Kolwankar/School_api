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
    public class StandardController : ApiController
    {
        DataSet ds1 = new DataSet();
        string strquery = "";
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();

        [HttpPost]
        public HttpResponseMessage std([FromBody] StandardMaster std)
        {
            if (std.type == "loadstd")
            {
                strquery = "select std_id,std_name from mst_standard_tbl where med_id='"+std.med+"' and del_flag=0 order by rank";
                ds1 = cls.fillds(strquery);
                ds1.Tables[0].Columns.Add("flag");
                ds1.Tables[0].Columns.Add("action");
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    strquery = "select class_id from adm_studentacademicyear where class_id='" + ds1.Tables[0].Rows[i]["std_id"].ToString() + "' union all select class_id from adm_student_master where class_id='" + ds1.Tables[0].Rows[i]["std_id"].ToString() + "'";
                    DataSet ds = cls.fillds(strquery);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds1.Tables[0].Rows[i]["flag"] = "1";
                    }
                    else
                    {
                        ds1.Tables[0].Rows[i]["flag"] = "0";
                    }
                    ds1.Tables[0].Rows[i]["action"] = "exist";
                }
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (std.type == "delete")
            {
                strquery = "update mst_standard_tbl set del_flag=1,mod_date=getdate() where std_id='"+std.stdid+"' and med_id='"+std.med+"' and del_flag=0";
                cls.exeIUD(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, "Delete", "application/json");
            }
            else if (std.type == "save")
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(std.rank);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strquery = strquery + "Exec insert_update_standard '" + dt.Rows[i]["std_id"].ToString() + "',N'" + dt.Rows[i]["std_name"].ToString() + "','" + std.med + "','" + dt.Rows[i]["rank"].ToString() + "','" + std.user + "',null,null,'" + dt.Rows[i]["action"].ToString() + "'; ";
                }
                cls.exeIUD(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, "Saved", "application/json");
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "No Changes Made", "application/json");
            }
        }

    }
}
