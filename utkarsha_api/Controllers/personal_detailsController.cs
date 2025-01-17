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

    public class personal_detailsController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();
        DataSet ds = new DataSet();
        string str = "";

        [HttpPost]
        public HttpResponseMessage loaddetails([FromBody] personal_details personal_details)
        {
            if (personal_details.type == "select")
            {
                str = "select emp_id,emp_fname,emp_mname,emp_lname,emp_mother_name,convert (varchar(10),emp_dob,103) as emp_dob,convert (varchar(10),emp_trust,103) as emp_trust,emp_trust,emp_blood_group,emp_gender,emp_maritial_status,emp_category,emp_cast,emp_mobile1,emp_mobile2,emp_email,emp_aadharno,emp_pan,emp_address_curr,emp_address_per,emp_state_curr,emp_state_per,emp_pincode_curr,emp_pincode_per,emp_photo,emp_sign,del_flag from mst_employee_personal where emp_id='" + personal_details.emp_id + "' and del_flag=0;select '0' as category_id,'--Select--' as category_name union all select category_id,category_name from mst_category_tbl where del_flag=0; select '0' as category_id, '0' as cast_id,'--Select--' as cast_name union all select category_id,cast_id,cast_name from mst_cast_tbl where del_flag=0";
                ds = cls.fillds(str);
                ds.Tables[0].TableName = "employee";
                ds.Tables[1].TableName = "category";
                ds.Tables[2].TableName = "cast";
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (personal_details.type == "get")
            {
                con.Open();
                DateTime date = DateTime.ParseExact(personal_details.dob, "dd/MM/yyyy", null);
              
                string strquery = "INSERT_UPDATE_emp_personal_details";

                using (SqlCommand cmd = new SqlCommand(strquery, con))
                {
                    string st = "Update";
                    string moddate="";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@emp_id", personal_details.emp_id);
                    cmd.Parameters.AddWithValue("@emp_fname", personal_details.fname);//5
                    cmd.Parameters.AddWithValue("@emp_mname", personal_details.mname);//6                        
                    cmd.Parameters.AddWithValue("@emp_lname", personal_details.lname);//8
                    cmd.Parameters.AddWithValue("@emp_mother_name", personal_details.moname);//9
                    cmd.Parameters.AddWithValue("@emp_dob", date);//10
                                  
                    cmd.Parameters.AddWithValue("@emp_blood_group", personal_details.bloodgroup);//10                        
                    cmd.Parameters.AddWithValue("@emp_gender", personal_details.gender);//10                   
                    cmd.Parameters.AddWithValue("@emp_maritial_status", personal_details.mstatus);//10
                    cmd.Parameters.AddWithValue("@emp_category", personal_details.category);//10
                    if (personal_details.cast != null)
                    {
                        cmd.Parameters.AddWithValue("@emp_cast", personal_details.cast);//10
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@emp_cast", DBNull.Value);//10
                    }
                    cmd.Parameters.AddWithValue("@emp_mobile1", personal_details.mobile1);//10
                    cmd.Parameters.AddWithValue("@emp_mobile2", personal_details.mobile2);//10
                    cmd.Parameters.AddWithValue("@emp_email", personal_details.email);
                    cmd.Parameters.AddWithValue("@emp_aadharno", personal_details.aadhar);
                    cmd.Parameters.AddWithValue("@emp_pan", personal_details.panno);
                    cmd.Parameters.AddWithValue("@emp_address_curr", personal_details.caddress);
                    cmd.Parameters.AddWithValue("@emp_state_curr", personal_details.cstate);
                    cmd.Parameters.AddWithValue("@emp_pincode_curr", personal_details.cpin);
                    cmd.Parameters.AddWithValue("@emp_address_per", personal_details.paddress);
                    cmd.Parameters.AddWithValue("@emp_state_per", personal_details.pstate);
                    cmd.Parameters.AddWithValue("@emp_pincode_per", personal_details.ppin);
                    cmd.Parameters.AddWithValue("mod_dt", moddate);

                    cmd.Parameters.AddWithValue("@StatementType", st);//34

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        //return true;
                    }
                    else
                    {
                        // objConn.closecon();               
                    }
                    con.Close();

                }
                return this.Request.CreateResponse(HttpStatusCode.OK, "No changes made", "application/json");
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "No changes made", "application/json");
            }
        }
    }
}
