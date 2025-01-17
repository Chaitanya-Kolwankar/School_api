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
    public class Employee_SearchController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();
        DataSet ds = new DataSet();
        string str = "";

        [HttpPost]
        public HttpResponseMessage loademp([FromBody] Employee_Search employee)
        {
            if (employee.type == "select")
            {
                str = "select '--Select--' as Department_name,'--Select--' as Dept_id union all select Department_name,Dept_id from m_department where del_flag='0';select '--Select--' as Designation_ID,'--Select--' as Designation_Title union all select Designation_ID,Designation_Title from m_designation where del_flag='0'";
                ds = cls.fillds(str);
                ds.Tables[0].TableName = "Department";
                ds.Tables[1].TableName = "designation";
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (employee.type == "get")
            {                
                str = "select * from [employee_select] where emp_dept_id='" + employee.department + "' and emp_des_id='" + employee.designation + "' ";
                ds = cls.fillds(str);
                ds.Tables[0].TableName = "employee";
           
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (employee.type == "Search")
            {              
                str = "select * from employee_search where employee_name like '%"+employee.fname+"%"+employee.mname+"%"+employee.lname+"%'";
                ds = cls.fillds(str);
                ds.Tables[0].TableName = "data";

                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (employee.type == "Update")
            {
                con.Open();
                string strquery = "Exec INSERT_UPDATE_employee_search '" + employee.eid + "','Update'";
                cls.exeIUD(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (employee.type == "set")
            {
                con.Open();
                string strquery = "Exec INSERT_UPDATE_employee_search '" + employee.eid + "','set'";
                cls.exeIUD(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            
        }
    }
}
