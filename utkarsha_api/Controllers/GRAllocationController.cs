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
    public class GRAllocationController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        string strquery;
        string ok;
        
        

        [HttpPost]
        public HttpResponseMessage updt([FromBody] GRALLO grn)
        {
            if (grn.type_of_query == "previ")
            {
                DataTable dt = new DataTable("New");
               // dt.Columns.Add(new DataColumn("msg", typeof(string)));
                DataRow dr = dt.NewRow();
                string strquery = "";
                string[] a = grn.student_id.Split(',');
                dt.Columns.Add("msg");

                for (int i = 0; i < a.Length; i++)
                {
                    string student_id = a[i] ;
                    //strquery =  "select gr_no from adm_studentacademicyear where ayid ='" + grn.ayid + "' and student_id = '" + student_id + "'";
                    strquery = "select a.gr_no from adm_studentacademicyear as a,adm_student_master as b where  a.student_id = '" + student_id + "' and gr_no is not null and a.AYID ='" + grn.ayid + "' and a.AYID=b.AYID and a.student_id=b.Student_id  ";
                    ds1 = cls.fillds(strquery);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        string str = ds1.Tables[0].Rows[0]["gr_no"].ToString();
                        dt.Rows.Add(str);
                    }
                    else
                    {
                        string str =" ";
                        dt.Rows.Add(str);
                    }
                   
                    //dt.Rows[i]["msg"] = str;                    
                }
                ds.Tables.Add(dt);
                //DataSet ds = cls.fillds(strquery);

                ds.Tables[0].TableName = "drop3";
                
            }
            else if (grn.type_of_query == "rdcheck")
            {
                string strquery = "Select asm.Student_id as [Student id],isnull (asm.stud_L_name,'') + ' ' + isnull (asm.stud_F_name,'') + ' ' + isnull (asm.stud_m_name,'') + ' ' + isnull (asm.stud_mo_name,'')";
                strquery = strquery + " as [Student Name] ,COALESCE(say.gr_no,'') as [GR No],asm.category as Category,format(asm.dob ,'dd-MM-yyyy') as [Date of Birth],asm.form_id  as [Form id]  from dbo.adm_student_master asm , ";
                strquery = strquery + "   dbo.adm_studentacademicyear as say where  asm.student_id = say.student_id and asm.del_flag= 0 and say.del_flag=0  and say.AYID = '" + grn.ayid + "'   ";
                strquery = strquery + " and say.medium_id='" + grn.medium + "' and say.class_id='" + grn.class_id + "' "+grn.sort+"";
                ds = cls.fillds(strquery);
                ds.Tables[0].TableName = "radio1";
            }
            else if (grn.type_of_query == "rdcng")
            {
                string strquery = "Select asm.Student_id as [Student id],isnull (asm.stud_L_name,'') + ' ' + isnull (asm.stud_F_name,'') + ' ' + isnull (asm.stud_m_name,'') + ' ' + isnull (asm.stud_mo_name,'')";
                strquery = strquery + " as [Student Name] ,COALESCE(say.gr_no,'') as [GR No],asm.category as Category,format(asm.dob ,'dd-MM-yyyy') as [Date of Birth],asm.form_id  as [Form id] from dbo.adm_student_master asm , ";
                strquery = strquery + "   dbo.adm_studentacademicyear as say where  asm.student_id = say.student_id and asm.del_flag= 0 and say.del_flag=0  and say.AYID = '" + grn.ayid + "'   ";
                strquery = strquery + " and say.medium_id='" + grn.medium + "' and say.class_id='" + grn.class_id + "' " + grn.sort + "";
                ds = cls.fillds(strquery);
                ds.Tables[0].TableName = "radio"; 
            }
            else if (grn.type_of_query == "filldata")
            {
                string strquery = "select * from admission_student_master where form_id = '" + grn.form_id + "'";
                DataSet ds = cls.fillds(strquery);
                ds.Tables[0].TableName = "";
               
            }
            else if (grn.type_of_query == "dropsel")
            {
                string strquery = "select gr_no from student_academic_year where ayid ='" + grn.ayid + "' and student_id = '" + grn.student_id + "'";
                DataSet ds = cls.fillds(strquery);
                ds.Tables[0].TableName = "drop3";
               
            }
            else if (grn.type_of_query == "str")
            {
                String strquery = "Select distinct asm.Student_id as [Student id] ,isnull (asm.stud_L_name,'') + ' ' + isnull (asm.stud_F_name,'') + ' ' + isnull (asm.stud_m_name,'') + ' ' + isnull (asm.stud_mo_name,'')";
                strquery = strquery + " as [Student Name] ,COALESCE(say.gr_no,'') as [GR No],cat.category_name as Category,format(asm.dob ,'dd-MM-yyyy') as [Date of Birth],asm.form_id as [Form id] from dbo.adm_student_master asm , ";
                strquery = strquery + "   dbo.adm_studentacademicyear as say,mst_category_tbl as cat where  asm.student_id = say.student_id and asm.del_flag= 0 and say.del_flag=0 and cat.category_id=asm.category  and say.AYID = '" + grn.ayid + "'   ";
                strquery = strquery + " and say.medium_id='"+grn.medium+"' and say.class_id='"+grn.class_id+"' "+grn.sort+" " ;
                ds = cls.fillds(strquery);
                ds.Tables[0].TableName = "grid";
            }
            else if (grn.type_of_query == "fill")
            {
                string pre = "select duration,ayid from m_academic where AYID < '"+grn.ayid+"' ";
                ds = cls.fillds(pre);
                ds.Tables[0].TableName = "checkcng";
            }
            else if (grn.type_of_query == "setgr")
            {
                string[] a = grn.gr_no.Split(',');
                string[] b = grn.student_id.Split(',');
                for (int i = 0; i < a.Length; i++)
                {
                    string gr_no = "" + a[i] + "";
                    string student_id = "" + b[i] + "";
                    con.Open();
                    //strquery = "EXEC UPDATE_GRNO @Student_id= " + student_id + ",@medium_id=" + grn.medium + ",@AYID=" + grn.ayid + ", @gr_no= " + gr_no + ", ";

                    //cls.IUDRecord(strquery);

                    strquery = "UPDATE_GRNO";

                    using (SqlCommand cmd = new SqlCommand(strquery, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@gr_no", gr_no.ToUpper().Trim());//38
                        cmd.Parameters.AddWithValue("@Student_id", student_id);//38
                        cmd.Parameters.AddWithValue("@medium_id", grn.medium);//2
                        cmd.Parameters.AddWithValue("@AYID", grn.ayid);//26
                       

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            ok = "OK";
                        }
                        else
                        {
                            // objConn.closecon();
 
                        }
                    }
                    con.Close();
                }
                if(ok=="OK")
                {
                DataTable dt = new DataTable("New");
                dt.Columns.Add(new DataColumn("msg", typeof(string)));
                DataRow dr = dt.NewRow();
                dr["msg"] = "GR alloted to student";
                dt.Rows.Add(dr);
                ds.Tables.Add(dt);
                }
                else
                {
                    DataTable dt = new DataTable("New");
                dt.Columns.Add(new DataColumn("msg", typeof(string)));
                DataRow dr = dt.NewRow();
                dr["msg"] = "Not saved  ";
                dt.Rows.Add(dr);
                ds.Tables.Add(dt);
                }
                   
            }
            else
            {
                DataTable dt = new DataTable("New");
                dt.Columns.Add(new DataColumn("msg", typeof(string)));
                DataRow dr = dt.NewRow();
                    dr["msg"] = "No";
                    dt.Rows.Add(dr);
                    ds.Tables.Add(dt);
              
               
            }

                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
        } 
    }

