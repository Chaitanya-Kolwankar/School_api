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
    public class AcademicYearController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();

        [HttpPost]
        public HttpResponseMessage AcademicYears([FromBody]AcademicYear acd)
        {
            DataSet ds = new DataSet();
            string str = "";
            if (acd.type == "load")
            {
                str = " Select * from m_academic";
                ds = cls.fillds(str);
                ds.Tables[0].TableName = "ACD";
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (acd.type == "add")
            {
                string start = acd.start.Replace("a", "/");
                string strchk = "select * from m_academic where substring(duration,7,6)+''+ substring(duration,20,23)='" + acd.date + "'; select case when CONVERT(Date,'"+start+"',103)> CONVERT(Date,a.enddate,103) then 'Allow' else 'Deny' end as remark from(select top 1 AYID,duration,Convert(Date,SUBSTRING(duration,(CHARINDEX('-',duration)+2),10),103) as enddate from m_academic order by AYID desc)a";
                ds = cls.fillds(strchk);
                ds.Tables[0].TableName = "check";
                ds.Tables[1].TableName = "remark";
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (acd.type == "save")
            {
                str = "";
                DataSet dsdata = JsonConvert.DeserializeObject<DataSet>(acd.date);
                for (int i = 0; i < dsdata.Tables[0].Rows.Count; i++)
                {
                    str = str + "Exec insert_update_Academic_year '" + dsdata.Tables[0].Rows[i]["AYID"].ToString() + "','" + dsdata.Tables[0].Rows[i]["Duration"].ToString() + "','" + dsdata.Tables[0].Rows[i]["flag"].ToString() + "','" + dsdata.Tables[0].Rows[i]["Type"].ToString() + "';";
                }
                cls.exeIUD(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, "Save", "application/json");
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "No changes made", "application/json");
            }
        }

    }
}
