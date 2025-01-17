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
    public class loginController : ApiController
    {
        login cm = new login();
        Class1 cls = new Class1();

        [HttpPost]
        public HttpResponseMessage login([FromBody] login lg)
        {
            DataSet ds = new DataSet();
            DataSet ds_new = new DataSet();
            string str = "";
            if (lg.type == "Web")
            {
                str = "select password,emp_id,l.role_id,r.form_name,  (SELECT STUFF((SELECT ',' + group_ids  FROM web_tp_login FOR XML PATH('')),1,1,'' ))  AS group_ids,l.col1,l.col2 from dbo.web_tp_login l,dbo.web_tp_roletype r where r.role_id=l.role_id and username='" + lg.username + "' and l.del_flag=0";
                ds_new = cls.fillds(str);

                string qry = "Select role_name from web_tp_roletype where role_id in (select role_id from web_tp_login where emp_id='" + lg.username + "');";
                DataSet dss = cls.fillds(qry);



                if (ds_new.Tables[0].Rows.Count > 0)
                {
                    if (ds_new.Tables[0].Rows[0]["password"].ToString() == lg.passwd)
                    {

                        str = "";
                        str = "select emp_id,NAME,FATHER,SURNAME,MOTHER,DOB,DOJ_gov,BLOOD_GROUP,GENDER,MARITIAL_STATUS,CASTE,MOBILE1,MOBILE2,EMAIL_ADDRESS,CURRENT_ADDRESS,CURRENT_STATE,CURRENT_PIN,CURRENT_DEPARTMENT_NAME,CURRENT_DESIGNATION,CATEGORY,EMAIL_ADDRESS,[PAN.NO]AS PAN_NO,PHOTO,emp_sign ";
                        str += " from EmployeePersonal where emp_id='" + ds_new.Tables[0].Rows[0][1].ToString() + "'";
                        ds = cls.fillds(str);


                        ds.Tables[0].Columns.Add("form_id");
                        ds.Tables[0].Columns.Add("form_name");
                        ds.Tables[0].Columns.Add("Group_name");
                        ds.Tables[0].Columns.Add("col2");
                        ds.Tables[0].Columns.Add("emp_role");
                        if (ds_new.Tables[0].Rows[0]["role_id"].ToString() != "")
                        {
                            ds.Tables[0].Rows[0]["form_id"] = ds_new.Tables[0].Rows[0]["role_id"].ToString();
                        }
                        if (ds_new.Tables[0].Rows[0]["group_ids"].ToString() != "")
                        {
                            ds.Tables[0].Rows[0]["Group_name"] = ds_new.Tables[0].Rows[0]["group_ids"].ToString();
                        }
                        if (ds_new.Tables[0].Rows[0]["form_name"].ToString() != "")
                        {
                            ds.Tables[0].Rows[0]["form_name"] = ds_new.Tables[0].Rows[0]["form_name"].ToString();
                        }
                        if (ds_new.Tables[0].Rows[0]["col2"].ToString() != "")
                        {
                            ds.Tables[0].Rows[0]["col2"] = ds_new.Tables[0].Rows[0]["col2"].ToString();
                        }

                        if (dss.Tables.Count > 0)
                        {
                            if (dss.Tables[0].Rows.Count > 0)
                            {
                                ds.Tables[0].Rows[0]["emp_role"] = dss.Tables[0].Rows[0][0].ToString();
                            }
                        }
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, "password", "application/json");
                    }
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "username", "application/json");
                }


                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (lg.type == "Android")
            {
                str = "select username,password,'Staff' as Role from web_tp_login where username='" + lg.username + "' union all select stud_id as [username],password,'Student' as Role from www_stud_login where stud_id='" + lg.username + "'";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "No Data", "application/json");
            }
        }
    }
}
