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
    public class DivisonMasterController : ApiController
    {
        DataSet ds1 = new DataSet();
        string strquery = "";
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();

        [HttpPost]
        public HttpResponseMessage loadgrid([FromBody] DivisonMasterClass div)
        {
            if (div.type == "LoadGrid")
            {
                strquery = "Exec LoadDivision'" + div.medium + "','" + div.classid + "','" + div.prevayid + "'";
                ds1 = cls.fillds(strquery);
                ds1.Tables[0].Columns.Add("flag");
                ds1.Tables[0].TableName = "gridload";
                for(int i=0;i<ds1.Tables[0].Rows.Count;i++)
                {
                    strquery = "select * from adm_studentacademicyear where division_id='" + ds1.Tables[0].Rows[i]["division_id"].ToString() + "' and AYID='" + div.ayid + "'";
                    DataSet ds = cls.fillds(strquery);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds1.Tables[0].Rows[i]["flag"] = "1";
                    }
                    else
                    {
                        ds1.Tables[0].Rows[i]["flag"] = "0";
                    }
                }
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (div.type == "Save")
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(div.division_name);
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    strquery = strquery + "Exec insert_update_division '" + dt.Rows[i]["div"].ToString() + "',N'" + dt.Rows[i]["name"].ToString() + "','" + div.medium + "','" + div.classid + "','" + div.ayid + "','" + div.user_id + "',null,null,'" + dt.Rows[i]["action"].ToString() + "';";
                }
                cls.exeIUD(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, "Insert", "application/json");
            }
            else if (div.type == "Remove")
            {
                string a = "No";
                strquery = "select * from adm_studentacademicyear where division_id='" + div.division_id + "' and AYID='"+div.ayid+"'";
                ds1=cls.fillds(strquery);
                if (ds1.Tables[0].Rows.Count == 0)
                {
                    string str = "update mst_division_tbl set del_flag=1,modified_date=getdate() where division_id = '" + div.division_id + "' and del_flag=0";
                    cls.exeIUD(str);
                    a = "Yes";
                }
                return this.Request.CreateResponse(HttpStatusCode.OK, a.ToString(), "application/json");
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "No Changes Made", "application/json");
            }
            

        }

    }
}
