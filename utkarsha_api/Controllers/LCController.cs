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
    public class LCController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();
        DataSet ds = new DataSet();
        string str = "";

        [HttpPost]
        public HttpResponseMessage load([FromBody] LC lc)
        {
            if (lc.type == "standard")
            {
                if (lc.sid.Contains('E'))
                {

                    str = "select std_id, std_name from mst_standard_tbl  where med_id = 2 and del_flag = 0";
                    ds = cls.fillds(str);

                    return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");

                }
                else if (lc.sid.Contains('M'))
                {

                    str = "select std_id, std_name from mst_standard_tbl  where med_id = 1 and del_flag = 0";
                    ds = cls.fillds(str);

                    return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
                }

                else
                {

                    str = "select std_id, std_name from mst_standard_tbl  where del_flag = 0";
                    ds = cls.fillds(str);

                    return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
                }
            }
            //-------------------------
            if (lc.type == "Reason")
            {
                str = "Select distinct Reason from LC_Reason where Reason is not null and Reason<>''";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");

            }
            if (lc.type == "Remark")
            {
                str = "Select distinct Remark from LC_Remark where Remark is not null and Remark<>''";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            if (lc.type == "addRemark")
            {
                string msg = "";
                str = " insert into LC_Remark (Remark ) values (N'" + lc.remark_text + "');";
                if (cls.exeIUD(str))
                {
                    msg = "true";

                }
                else
                {
                    msg = "false";
                }
                return this.Request.CreateResponse(HttpStatusCode.OK, msg, "application/json");
            }


            if (lc.type == "deletereason")
            {
               
                string Deletereasonquery = "";
                string[] arr = lc.Reason.Split('/');
                for (int i = 0; i < arr.Length; i++)

                {
                    string deletecheckdata= "Select * from Leaving_tbl where Reason =N'" + (arr[i]).Replace("'", "''") + "' and del_flag=0";

                    DataTable dt = new DataTable(deletecheckdata);

                    if (dt.Rows.Count == 0)
                    {

                        if (Deletereasonquery == "")
                        {
                            Deletereasonquery = "Delete from LC_Reason where reason =N'" + (arr[i]).Replace("'", "''") + "';";

                        }
                        else
                        {
                            Deletereasonquery += "Delete from LC_Reason where reason =N'" + (arr[i]).Replace("'", "''") + "';";

                        }
                    }

                    else

                    { 
                    
                    
                    }

               
                }
                         bool  msg = cls.exeIUD(Deletereasonquery);
                if (msg == true)
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, "Saved", "application/json");
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, "Error", "application/json");
                    }

                

            }

            if (lc.type == "deleteremark")
            {

                string Deleteremarkquery = "";
                string[] arr = lc.Reason.Split('/');
                for (int i = 0; i < arr.Length; i++)
                {
                    if (Deleteremarkquery == "")
                    {
                        Deleteremarkquery = "Delete from LC_Remark where Remark =N'" + (arr[i]).Replace("'", "''") + "';";

                    }
                    else
                    {
                        Deleteremarkquery += "Delete from LC_Remark where Remark =N'" + (arr[i]).Replace("'", "''") + "';";

                    }
                }
                bool msg = cls.exeIUD(Deleteremarkquery);
                if (msg == true)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "Saved", "application/json");
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "Error", "application/json");
                }
            }
            if (lc.type == "addReason")
            {
                string msg = "";
                str = " insert into LC_Reason (reason ) values (N'" + lc.reason_text + "');";
                if (cls.exeIUD(str))
                {
                    msg = "true";

                }
                else
                {
                    msg = "false";
                }
                return this.Request.CreateResponse(HttpStatusCode.OK, msg, "application/json");
            }

            //-------------------------------
            if (lc.type == "select")
            {
                //str = "select * from [select] where  AYID='" + lc.Ayid + "' and  (gr_no = N'" + lc.sid + "' or Student_id = '" + lc.sid + "');select Student_id,gr_no,(stud_F_name+' '+stud_m_name+' '+stud_L_name) as Student_name from [select] where  AYID='" + lc.Ayid + "' and  (gr_no = N'" + lc.sid + "' or Student_id = '" + lc.sid + "')";

                str = "select b.std_id as stdid ,* from [select] a,mst_standard_tbl b where a.doa_std=b.std_name and b.del_flag=0 and  a.AYID='" + lc.Ayid + "' and  (a.gr_no = N'" + lc.sid + "' or a.Student_id = '" + lc.sid + "');select Student_id,gr_no,(stud_F_name+' '+stud_m_name+' '+stud_L_name) as Student_name from [select] where  AYID='" + lc.Ayid + "' and  (gr_no = N'" + lc.sid + "' or Student_id = '" + lc.sid + "')";

                ds = cls.fillds(str);
                ds.Tables[0].TableName = "leaving";
                ds.Tables[1].TableName = "Multiple";
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (lc.type == "get")
            {
                str = "select distinct Stud_Id from Leaving_tbl  where  Stud_Id='" + lc.sid + "'";
                ds = cls.fillds(str);
                ds.Tables[0].TableName = "dt";
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (lc.type == "modify")
            {

                //str = "select * from [modify] where  AYID='" + lc.Ayid + "' and  (gr_no = N'" + lc.sid + "' or Student_id = '" + lc.sid + "')";
                str = "select b.std_id as stdid , * from [modify] a,mst_standard_tbl b where a.doa_std=b.std_name and b.del_flag=0 and   a.AYID='" + lc.Ayid + "' and  (a.gr_no = N'" + lc.sid + "' or a.Student_id = '" + lc.sid + "')";
                ds = cls.fillds(str);
                ds.Tables[0].TableName = "abc";
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
           
            else if (lc.type == "insert")
            {
                con.Open();
                string msg = "";

                str = "select   [dbo].[Generate_LCNO] ('" + lc.Ayid + "') as lcno";
                ds = cls.fillds(str);
                ds.Tables[0].TableName = "master";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lc.lcno = ds.Tables[0].Rows[0]["lcno"].ToString();
                    string strquery = "INSERT_UPDATE_LC";
                    DateTime date = DateTime.ParseExact(lc.dol, "dd/MM/yyyy", null);
                    using (SqlCommand cmd = new SqlCommand(strquery, con))
                    {
                        string st = "leaving";

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Lc_No", lc.lcno);//4
                        cmd.Parameters.AddWithValue("@Stud_Id", lc.sid);//5
                        cmd.Parameters.AddWithValue("@Date_of_leaving", date);//6
                        cmd.Parameters.AddWithValue("@Conduct", lc.conduct);//7
                        cmd.Parameters.AddWithValue("@progress", lc.progress);//8
                        cmd.Parameters.AddWithValue("@Reason", lc.Reason);//9
                        cmd.Parameters.AddWithValue("@Remark", lc.Remark);//10
                        cmd.Parameters.AddWithValue("@issue", "");//11
                        cmd.Parameters.AddWithValue("@standard_in_which", lc.siw);//12
                        cmd.Parameters.AddWithValue("@standard_in_which_in_numbers", lc.siw2);//12
                        cmd.Parameters.AddWithValue("@seat_no", lc.seat_no);//13
                        cmd.Parameters.AddWithValue("@LC_remark", lc.lcremark);//10
                        cmd.Parameters.AddWithValue("@user_id", lc.userid);//10
                        cmd.Parameters.AddWithValue("@tablename", lc.tablename);//10
                        cmd.Parameters.AddWithValue("@lastschool", lc.lastschool);//10
                        cmd.Parameters.AddWithValue("@StatementType", st);//34
                        cmd.Parameters.AddWithValue("@Doa_std", lc.standard);

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
            else if (lc.type == "Update")
            {
                string msg = "";
                con.Open();
                string strquery = "INSERT_UPDATE_LC";
                DateTime date = DateTime.ParseExact(lc.dol, "dd/MM/yyyy", null);
                using (SqlCommand cmd = new SqlCommand(strquery, con))
                {
                    string st = "Update";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Lc_No", lc.lcno);//4
                    cmd.Parameters.AddWithValue("@Stud_Id", lc.sid);//5
                    cmd.Parameters.AddWithValue("@Date_of_leaving", date);//6
                    cmd.Parameters.AddWithValue("@Conduct", lc.conduct);//7
                    cmd.Parameters.AddWithValue("@progress", lc.progress);//8
                    cmd.Parameters.AddWithValue("@Reason", lc.Reason);//9
                    cmd.Parameters.AddWithValue("@Remark", lc.Remark);//10
                    cmd.Parameters.AddWithValue("@issue", "");//11
                    cmd.Parameters.AddWithValue("@standard_in_which", lc.siw);//12
                    cmd.Parameters.AddWithValue("@standard_in_which_in_numbers", lc.siw2);//12
                    cmd.Parameters.AddWithValue("@seat_no", lc.seat_no);//13
                    cmd.Parameters.AddWithValue("@LC_remark", "");
                    cmd.Parameters.AddWithValue("@user_id", "");//10
                    cmd.Parameters.AddWithValue("@tablename", "");//10
                    cmd.Parameters.AddWithValue("@lastschool", lc.lastschool);//10
                    cmd.Parameters.AddWithValue("@Doa_std", lc.standard);



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
            else if (lc.type == "gettype")
            {
                str = "select * from [gettype] where Lc_No='"+lc.lcno+"' and ayid='"+lc.Ayid+"' ";              
                ds = cls.fillds(str);
                ds.Tables[0].TableName = "issue";
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (lc.type == "set")
            {
                con.Open();
                var msg = "";
                string strquery = "INSERT_UPDATE_LC";
                DateTime date = DateTime.ParseExact(lc.dol, "dd/MM/yyyy", null);
                using (SqlCommand cmd = new SqlCommand(strquery, con))
                {
                    string st = "insert";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Lc_No", lc.lcno);//4
                    cmd.Parameters.AddWithValue("@Stud_Id", lc.sid);//5
                    cmd.Parameters.AddWithValue("@Date_of_leaving", date);//6
                    cmd.Parameters.AddWithValue("@Conduct", lc.conduct);//7
                    cmd.Parameters.AddWithValue("@progress", lc.progress);//8
                    cmd.Parameters.AddWithValue("@Reason", lc.Reason);//9
                    cmd.Parameters.AddWithValue("@Remark", lc.Remark);//10
                    cmd.Parameters.AddWithValue("@issue", lc.issue);//11
                    cmd.Parameters.AddWithValue("@standard_in_which", lc.siw);//12
                    cmd.Parameters.AddWithValue("@standard_in_which_in_numbers", lc.siw2);//12
                    cmd.Parameters.AddWithValue("@seat_no", lc.seat_no);//13
                    cmd.Parameters.AddWithValue("@LC_remark", "");
                    cmd.Parameters.AddWithValue("@user_id", "");//10
                    cmd.Parameters.AddWithValue("@tablename", "");//10
                    cmd.Parameters.AddWithValue("@lastschool", lc.lastschool);//10
                    cmd.Parameters.AddWithValue("@Doa_std", lc.standard);

                    cmd.Parameters.AddWithValue("@StatementType", st);//34

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        msg = "true";
                    }
                    else
                    {
                        msg = "false";              
                    }
                    con.Close();


                }
                return this.Request.CreateResponse(HttpStatusCode.OK, msg, "application/json");
            }
            else if (lc.type == "lcreason")
            {
                str = "select '--Select--' as Reason union all select Reason from lc_reason;select '--Select--' as Remark union all select Remark from LC_Remark";             
                ds = cls.fillds(str);
                ds.Tables[0].TableName = "reason";
                ds.Tables[1].TableName = "remark"; 
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (lc.type == "master")
            {
                str = "select   [dbo].[Generate_LCNO] ('" + lc.Ayid + "') ";
                ds = cls.fillds(str);
                ds.Tables[0].TableName = "master";             
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
  
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "No changes made", "application/json");
            }
        }
    }
}
