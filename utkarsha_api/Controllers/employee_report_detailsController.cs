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
    public class employee_report_detailsController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();
        DataSet ds = new DataSet();

        [HttpPost]
        public HttpResponseMessage updt([FromBody] emp_rpt_details edr)
        {
             if (edr.type == "selectempdt")
             {
                   string strquery = "select * from [dbo].[employee_multiselect]";
                   ds = cls.fillds(strquery);
              
                     }
             else if (edr.type == "ddlemptype")
                 {
                     string strquery = "select role_id,role_name from web_tp_roletype";
                        ds = cls.fillds(strquery);
                     
                  }
             else if (edr.type == "getdata")
             {
                string strquery = "";
                if (edr.role_type == null)
                {
                    strquery = " select  Employee_id as [Employee ID],emp_fname +' '+emp_mname + ' ' +emp_lname as Name,Designation_Title as Designation," + edr.arr + "  from [dbo].[emp_dtldata] where del_flag=0  and  Employee_id=Employee_id and role_id=role_id";
                }
                else
                {
                    strquery = " select  Employee_id as [Employee ID],emp_fname +' '+emp_mname + ' ' +emp_lname as Name,Designation_Title as Designation," + edr.arr + "  from [dbo].[emp_dtldata] where del_flag=0   and role_id='" + edr.role_type + "' and  Employee_id=Employee_id and role_id=role_id";
                }
                ds = cls.fillds(strquery);
             }
             else
             {

             }
            return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
        }
    }
}
