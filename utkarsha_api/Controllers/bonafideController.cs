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
    public class bonafideController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();
        DataSet ds = new DataSet();
        string str = "";

        [HttpPost]
        public HttpResponseMessage loadbonafide([FromBody] Bonafide bonafide)
        {
            if (bonafide.type == "select")
            {
                str = "select * from [bonafide_select] where (student_id='" + bonafide.sid + "' or gr_no=N'" + bonafide.sid + "') and ayid='" + bonafide.Ayid + "';select student_id,gr_no,Stud_Name,(select std_name from mst_standard_tbl where std_id=class_id) as class,class_id from [bonafide_select] where (student_id='" + bonafide.sid + "' or gr_no=N'" + bonafide.sid + "') and ayid='" + bonafide.Ayid + "'";
                ds = cls.fillds(str);
                ds.Tables[0].TableName = "Bonafide";
                ds.Tables[1].TableName = "Multiple";
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (bonafide.type == "get")
            {
                str = "select * from [bonafide_get] where Stud_id='" + bonafide.sid + "' and class_id='" + bonafide.standard + "';select * from m_academic where AYID='" + bonafide.Ayid + "'";
                ds = cls.fillds(str);
                ds.Tables[0].TableName = "issue_bonafide";
                ds.Tables[1].TableName = "ayid";
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (bonafide.type == "printdata")
            {
                str = "select * from [bonafide_data] where Stud_id='" + bonafide.sid + "' and class_id='" + bonafide.standard + "';select * from m_academic where AYID='" + bonafide.Ayid + "'";
                ds = cls.fillds(str);
                ds.Tables[0].TableName = "issue_bonafide";
                ds.Tables[1].TableName = "ayid";
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (bonafide.type == "otherstd")
            {
                str = "select '--Select--' as std_name,'0' as std_id,'0' as rank union all select distinct std_name,std_id,rank from mst_standard_tbl where med_id in (select distinct medium_id from adm_studentacademicyear where student_id='" + bonafide.sid + "' or gr_no=N'" + bonafide.sid + "' ) and del_flag=0 order by rank";
                ds = cls.fillds(str);
                ds.Tables[0].TableName = "std";
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (bonafide.type == "Year")
            {
                str = "select '--Select--' as duration union all select distinct top(1) convert(varchar, right(duration, 4) - 1) as duration from m_academic union all select distinct convert(varchar, right(duration, 4)) as duration from m_academic";
                ds = cls.fillds(str);
                ds.Tables[0].TableName = "duration";
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (bonafide.type == "data")
            {
                str = "select b.Stud_id,b.class_id,b.issue from adm_student_master as a,Bonafide_certificate as b where Stud_id='" + bonafide.sid + "' and a.Student_id=b.Stud_id and b.class_id='" + bonafide.standard + "'";
                ds = cls.fillds(str);
                ds.Tables[0].TableName = "issue";
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (bonafide.type == "insert")
            {
                con.Open();
                string msg = "";
                str = "select [dbo].[Generate_bonafideNO] ('" + bonafide.Ayid + "') as bonafide_no";
                ds = cls.fillds(str);
                ds.Tables[0].TableName = "master";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    bonafide.bonafideno = ds.Tables[0].Rows[0]["bonafide_no"].ToString();
                    string strquery = "INSERT_UPDATE_Bonafide";

                    using (SqlCommand cmd = new SqlCommand(strquery, con))
                    {
                        string st = "Bonafide";

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Stud_id", bonafide.sid);//5
                        cmd.Parameters.AddWithValue("@ayid", bonafide.Ayid);//6                      
                        cmd.Parameters.AddWithValue("@year", bonafide.examyear);//8
                        cmd.Parameters.AddWithValue("@month", bonafide.exammonth);//9
                        cmd.Parameters.AddWithValue("@class_id", bonafide.standard);//10
                        cmd.Parameters.AddWithValue("@issue_dt", DateTime.Now);//11                                       
                        cmd.Parameters.AddWithValue("@issue", "");//10                        
                        cmd.Parameters.AddWithValue("@other_std", bonafide.otherstandard);//10            
                        cmd.Parameters.AddWithValue("@userid", bonafide.userid);//10
                        cmd.Parameters.AddWithValue("@bonafide_remark", bonafide.bonafide_remark);//10
                        cmd.Parameters.AddWithValue("@bonafide_number", bonafide.bonafideno);//10
                        cmd.Parameters.AddWithValue("@curr_date", DateTime.Now);//10
                        cmd.Parameters.AddWithValue("@mod_date", "");//10
                        cmd.Parameters.AddWithValue("@Remark", bonafide.remark);//10
                        cmd.Parameters.AddWithValue("@StatementType", st);//34

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            msg = "true";
                            //return true;
                        }
                        else
                        {
                            msg = "false";
                            // objConn.closecon();               
                        }
                        con.Close();

                    }
                }
                return this.Request.CreateResponse(HttpStatusCode.OK, msg, "application/json");
            }
            else if (bonafide.type == "Update")
            {
                con.Open();
                string strquery = "INSERT_UPDATE_Bonafide";
                string msg = "";
                using (SqlCommand cmd = new SqlCommand(strquery, con))
                {
                    string st = "Update";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Stud_id", bonafide.sid);//5
                    cmd.Parameters.AddWithValue("@ayid", bonafide.Ayid);//6                        
                    cmd.Parameters.AddWithValue("@year", bonafide.examyear);//8
                    cmd.Parameters.AddWithValue("@month", bonafide.exammonth);//9
                    cmd.Parameters.AddWithValue("@class_id", bonafide.standard);//10
                    cmd.Parameters.AddWithValue("@issue_dt", DateTime.Now);//11                  
                    cmd.Parameters.AddWithValue("@issue", "");//10                        
                    cmd.Parameters.AddWithValue("@other_std", bonafide.otherstandard);//10                   
                    cmd.Parameters.AddWithValue("@userid", bonafide.userid);//10
                    cmd.Parameters.AddWithValue("@bonafide_remark", bonafide.bonafide_remark);//10
                    cmd.Parameters.AddWithValue("@bonafide_number", bonafide.bonafideno);//10
                    cmd.Parameters.AddWithValue("@curr_date", "");//10
                    cmd.Parameters.AddWithValue("@mod_date", DateTime.Now);//10
                    cmd.Parameters.AddWithValue("@Remark", bonafide.remark);//10
                    cmd.Parameters.AddWithValue("@StatementType", st);//34

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        msg = "true";
                        //return true;
                    }
                    else
                    {
                        msg = "false";
                        // objConn.closecon();               
                    }
                    con.Close();

                }
                return this.Request.CreateResponse(HttpStatusCode.OK, msg, "application/json");
            }
            else if (bonafide.type == "set")
            {
                con.Open();
                //string strquery = "Exec INSERT_UPDATE_Bonafide '" + bonafide.sid + "',null,'" + bonafide.Ayid + "','" + bonafide.exammonth + "','" + bonafide.examyear + "','" + bonafide.standard + "','" + bonafide.issue + "','" + bonafide.otherstandard + "','" + bonafide.userid + "','" + bonafide.bonafide_remark + "','" + bonafide.bonafideno + "',null,null,N'"+bonafide.remark+"','insert'";

                string strquery = "Exec INSERT_UPDATE_Bonafide '" + bonafide.sid + "',null,'" + bonafide.Ayid + "','" + bonafide.exammonth + "','" + bonafide.examyear + "','" + bonafide.standard + "','" + bonafide.issue + "','" + bonafide.otherstandard + "','" + bonafide.userid + "','" + bonafide.bonafide_remark + "','" + bonafide.bonafideno + "',null,null,N'" + bonafide.remark + "','insert'";

                cls.exeIUD(strquery);

                return this.Request.CreateResponse(HttpStatusCode.OK, "No changes made", "application/json");
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "No changes made", "application/json");
            }
        }
    }
}
