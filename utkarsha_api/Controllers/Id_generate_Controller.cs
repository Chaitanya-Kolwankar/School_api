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
    public class Id_generate_Controller : ApiController
    {
        
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();
        [HttpPost]
        public HttpResponseMessage id_gen([FromBody] id_generate id)
        {
            DataSet ds1 = new DataSet();
            if (id.type == "select")
            {
                //string q1 = "select a.Student_id,c.std_name as class_id,a.student_photo,a.dob,a.aadhar_no,a.address ,isnull (a.stud_L_name,'') + ' ' +isnull (a.stud_F_name,'') + ' ' + isnull (a.stud_m_name,'') + ' ' + isnull (a.stud_mo_name,'') as Name,a.phone_no1,b.gr_no from adm_student_master as a,adm_studentacademicyear as b,mst_standard_tbl as c where a.Student_id=b.student_id";
                string q1 = "select * from [dbo].[id_generate] where med_id='"+id.mediumid+"' and std_id='"+id.classid+"' and ayid='"+id.ayid+"'";
                ds1 = cls.fillds(q1);   
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (id.type == "employee")
            {
                string str = "select * from m_department where del_flag=0";
                ds1 = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (id.type == "employee1")
            {
                string str = "select * from emp_details where dept_id='" + id.classid + "'";
                ds1 = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (id.type == "employee1insert")
            {
                DataTable ds = JsonConvert.DeserializeObject<DataTable>(id.insert);
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    string str = "insert into Id_Card_Excel_Data_emp (Stud_id,Name,Roll_No,Ayid,Emp_id,Curr_dt,Mod_dt) values('" + ds.Rows[i]["Stud_id"] + "','" + ds.Rows[i]["Name"] + "','" + ds.Rows[i]["roll_no"] + "','" + ds.Rows[i]["ayid"] + "','" + ds.Rows[i]["Emp_id"] + "',getdate(),getdate())";
                    ds1 = cls.fillds(str);
                }
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (id.type == "insert") {
                DataTable ds = JsonConvert.DeserializeObject<DataTable>(id.insert);
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    //string q1 = "insert into Id_Card_Excel_Data (Stud_id,Name,Roll_No,Ayid,Emp_id,Curr_dt,Mod_dt) values('" + ds.Rows[i]["Stud_id"] + "','" + ds.Rows[i]["Name"] + "','" + ds.Rows[i]["roll_no"] + "','" + ds.Rows[i]["ayid"] + "','" + ds.Rows[i]["Emp_id"] + "',getdate(),getdate())";
                    string q1 = "student_id";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(q1, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Stud_id", ds.Rows[i]["Stud_id"]);
                        cmd.Parameters.AddWithValue("@Name", ds.Rows[i]["Name"]);
                        cmd.Parameters.AddWithValue("@Roll_No", ds.Rows[i]["roll_no"]);
                        cmd.Parameters.AddWithValue("@Ayid", ds.Rows[i]["ayid"]);
                        cmd.Parameters.AddWithValue("@Emp_id", ds.Rows[i]["Emp_id"]);


                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            //ok = "OK";
                        }
                        else
                        {
                            // objConn.closecon();

                        }


                    }
                    //sds1 = cls.fillds(q1);
                    con.Close();
                    
                }
                return this.Request.CreateResponse(HttpStatusCode.OK, "inserted", "application/json");
            }
            else if (id.type == "duplicate") {
                //string q1 = "SELECT * FROM  emp_details WHERE emp_id IN (SELECT emp_id FROM Id_Card_Excel_Data_emp );";
                string q1 = "SELECT * FROM  emp_details WHERE emp_id IN (SELECT Stud_id FROM Id_Card_Excel_Data_emp where Stud_id in('"+id.classid+"'))";
                ds1 = cls.fillds(q1);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (id.type == "duplicate1")
            {
                //string q1 = "SELECT * FROM  emp_details WHERE emp_id IN (SELECT emp_id FROM Id_Card_Excel_Data_emp );";
                string q1 = "SELECT * FROM  [dbo].[id_generate] WHERE student_id IN (SELECT Stud_id FROM Id_Card_Excel_Data where Stud_id in('" + id.classid + "'))";
                ds1 = cls.fillds(q1);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (id.type == "w") {
                string q1 = "select distinct stud_id,Roll_No,Name from Id_Card_Excel_Data where CONVERT(CHAR(10),Curr_dt,120) between '" + id.classid + "' and '" + id.classid + "'";
                ds1 = cls.fillds(q1);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (id.type == "w1")
            {
                string q1 = "SELECT distinct Stud_id ,Name,Roll_No from Id_Card_Excel_Data_emp where CONVERT(CHAR(10),Curr_dt,120) between '" + id.classid + "' and '" + id.classid + "'";
                ds1 = cls.fillds(q1);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            return this.Request.CreateResponse(HttpStatusCode.OK, " no change made", "application/json");
        }
    }
}
