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
    public class change_passwordController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();
        DataSet ds = new DataSet();
        string str = "";

        [HttpPost]
        public HttpResponseMessage loadpass([FromBody] change_password change_password)
        {          
            if (change_password.type == "get")
            {
                con.Open();
                string strquery = "Exec INSERT_UPDATE_change_password '" + change_password.emp_id + "','"+change_password.password+"','Update'";
                cls.exeIUD(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, "password changed", "application/json");
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "No changes made", "application/json");
            }
        }
    }
}
