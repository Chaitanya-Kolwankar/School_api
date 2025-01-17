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

namespace utkarsha_api.Controllers
{
    public class AllocationController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();

        public HttpResponseMessage all([FromBody] Allocation ac)
        {
            DataSet ds = new DataSet();
            string str = "";
            if (ac.type == "divload")
            {
                str = "select * from mst_division_tbl where medium_id='" + ac.medium + "' and AYID='" + ac.ayid + "' and class_id='" + ac.classid + "'";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (ac.type == "studload")
            {
                str = "Exec get_division_roll_details '" + ac.ayid + "','" + ac.medium + "','" + ac.classid +"','"+ac.div+"'";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (ac.type == "save")
            {
                str = "";
                DataTable dsdata = JsonConvert.DeserializeObject<DataTable>(ac.div);
                for (int i = 0; i < dsdata.Rows.Count; i++)
                {
                    if (dsdata.Rows[i]["division_id"].ToString() != "" && dsdata.Rows[i]["Roll_no"].ToString() != "")
                    {
                        str = str + "Exec update_division_roll_details '" + ac.ayid + "','" + ac.medium + "','" + ac.classid + "','" + dsdata.Rows[i]["division_id"].ToString() + "','" + dsdata.Rows[i]["Roll_no"].ToString() + "','" + dsdata.Rows[i]["Student_id"] + "'; ";
                    }
                    else if (dsdata.Rows[i]["division_id"].ToString() != "" && dsdata.Rows[i]["Roll_no"].ToString() == "")
                    {
                        str = str + "Exec update_division_roll_details '" + ac.ayid + "','" + ac.medium + "','" + ac.classid + "','" + dsdata.Rows[i]["division_id"].ToString() + "',null,'" + dsdata.Rows[i]["Student_id"] + "'; ";
                    }
                    else if (dsdata.Rows[i]["division_id"].ToString() == "" && dsdata.Rows[i]["Roll_no"].ToString() == "")
                    {
                        str = str + "Exec update_division_roll_details '" + ac.ayid + "','" + ac.medium + "','" + ac.classid + "',null,null,'" + dsdata.Rows[i]["Student_id"] + "'; ";
                    }
                }
                cls.exeIUD(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, "Update", "application/json");
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "No Changes Made", "application/json");
            }
        }
    }
}
