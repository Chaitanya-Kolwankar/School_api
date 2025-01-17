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
using utkarsha_api.App_Start;

namespace utkarsha_api.Controllers
{
    public class Student_transferController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataTable divdt = new DataTable();
        DataTable groupiddt = new DataTable();


        string type;
        

        string strquery;

        [HttpPost]
        public HttpResponseMessage studtransfer([FromBody] stud_transfer st)
        {
            // and asm.AYID <> '" + st.ayid + "' 
            if (st.type == "fgvfill")
            {


                //string strquery = "Select  asm.Student_id,isnull (asm.stud_L_name,'') + ' ' + isnull (asm.stud_F_name,'') + ' ' + isnull (asm.stud_m_name,'') + ' ' + isnull (asm.stud_mo_name,'')as Student_Name,isnull(div.division_name,'')as Division ,div.division_id,(Select distinct group_name from exm_group_master where medium_id=medium_id and class_id= say.class_id   and ayid = say.AYID and group_id = say.group_id) as GroupName ,(Select distinct group_id from exm_group_master where medium_id=medium_id and class_id= say.class_id   and ayid = say.AYID and group_id = say.group_id) as Groupid  ,case when say.AYID='" + st.nextayid + "' then '1' else '0' end as type  from dbo.adm_student_master asm , adm_studentacademicyear say,mst_division_tbl div where say.medium_id='" + st.medium + "' and say.class_id='" + st.standard + "' and say.AYID='" + st.ayid + "'  and asm.student_id not in (select student_id from adm_studentacademicyear  where AYID  in(select AYID from (select AYID,duration,SUBSTRING(AYID,4,4) as rank from m_academic  ) a where a.rank >= SUBSTRING('" + st.ayid + "',4,4))   AND del_flag=0)  and say.Student_id IS NOT null  and say.del_flag=0 and asm.Student_id=say.student_id and say.class_id=div.class_id and say.AYID=div.AYID and say.medium_id=div.medium_id and say.division_id=div.division_id; select COUNT(*) as assign from adm_studentacademicyear  where student_id in (select student_id from adm_studentacademicyear where medium_id='" + st.medium + "' and class_id='" + st.standard + "' and AYID='" + st.ayid + "' and del_flag=0 ) and AYID in(select AYID from (select AYID,duration,SUBSTRING(AYID,4,4) as rank from m_academic  ) a where a.rank >= SUBSTRING('" + st.ayid + "',4,4))   AND del_flag=0 ";

                string strquery = "select asm.Student_id,isnull (asm.stud_L_name,'') + ' ' + isnull (asm.stud_F_name,'') + ' ' + isnull (asm.stud_m_name,'') + ' ' + isnull (asm.stud_mo_name,'')as Student_Name ,(select division_name from mst_division_tbl where medium_id=acd.medium_id and class_id=acd.class_id  and AYID=acd.ayid and  division_id=acd.division_id and del_flag=0) as Division ,acd.division_id division_id,(Select distinct group_name from exm_group_master where medium_id=medium_id and class_id= acd.class_id   and ayid = acd.AYID and group_id = acd.group_id) as GroupName,(Select distinct group_id from exm_group_master where medium_id=medium_id and class_id= acd.class_id   and ayid = acd.AYID and group_id = acd.group_id) as Groupid, case when acd.AYID='' then '1' else '0' end as type from adm_student_master asm,adm_studentacademicyear acd where asm.Student_id=acd.student_id and asm.del_flag=0 and asm.del_flag=acd.del_flag and  asm.Student_id is not null and asm.Student_id not in (select student_id from adm_studentacademicyear  where AYID  in(select AYID from (select AYID,duration,SUBSTRING(AYID,4,4) as rank from m_academic  ) a where a.rank > SUBSTRING('AYD0023',4,4))   AND del_flag=0 and class_id='"+st.nextclass+"') and acd.class_id='" + st.standard + "' and acd.medium_id='" + st.medium + "' and acd.AYID='" + st.ayid + "'; select COUNT(*) as assign from adm_studentacademicyear  where student_id in (select student_id from adm_studentacademicyear where medium_id='" + st.medium + "' and class_id='" + st.standard + "' and AYID='" + st.ayid + "' and del_flag=0 ) and AYID in(select AYID from (select AYID,duration,SUBSTRING(AYID,4,4) as rank from m_academic  ) a where a.rank >= SUBSTRING('" + st.ayid + "',4,4))   AND del_flag=0;";

                ds = cls.fillds(strquery);
                ds.Tables[0].TableName = "fgv";
                ds.Tables[1].TableName = "count";





            }
            else if (st.type == "selectcancel")
            {
                DataTable dt = new DataTable("Table");
                DataTable dt1 = new DataTable("Table1");
                dt.Columns.Add(new DataColumn("Student_id", typeof(string)));
                dt.Columns.Add(new DataColumn("Student_Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Type", typeof(string)));
                dt.Columns.Add(new DataColumn("Division", typeof(string)));
                dt.Columns.Add(new DataColumn("division_id", typeof(string)));
                dt1.Columns.Add(new DataColumn("Student_id", typeof(string)));
                dt1.Columns.Add(new DataColumn("Student_Name", typeof(string)));
                dt1.Columns.Add(new DataColumn("Type", typeof(string)));

                dt1.Columns.Add(new DataColumn("Division", typeof(string)));
                dt1.Columns.Add(new DataColumn("division_id", typeof(string)));

                DataRow dr = dt.NewRow();
                DataRow dr1 = dt1.NewRow();
                DataTable dt2 = JsonConvert.DeserializeObject<DataTable>(st.json);
                con.Open();

                //string student_id = dt2.Rows[i]["STUDENT ID"].ToString();
                //string check_flag = dt2.Rows[i]["CHECK"].ToString();

                string strquery = "Select  asm.Student_id,isnull (asm.stud_L_name,'') + ' ' + isnull (asm.stud_F_name,'') + ' ' + isnull (asm.stud_m_name,'') + ' ' + isnull (asm.stud_mo_name,'')as Student_Name ,case when say.AYID='" + st.nextayid + "' then '1' else '0' end as type, ''[Division],'' [division_id]  from dbo.adm_student_master asm , adm_studentacademicyear say where say.medium_id='" + st.medium + "' and say.class_id='" + st.standard + "' and say.AYID='" + st.ayid + "'  and asm.student_id not in (select student_id from adm_studentacademicyear  where AYID  in(select AYID from (select AYID,duration,SUBSTRING(AYID,4,4) as rank from m_academic  ) a where a.rank > SUBSTRING('" + st.ayid + "',4,4))   AND del_flag=0 union all select student_id from adm_student_master  where AYID='" + st.ayid + "' and del_flag=0 and student_id in (select student_id from  adm_studentacademicyear where AYID='" + st.ayid + "' and del_flag=0) )  and asm.Student_id IS NOT null  and say.del_flag=0 and asm.Student_id=say.student_id";
                ds1 = cls.fillds(strquery);

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    //if (ds1.Tables[0].Rows[0]["Type"].ToString() == "1")
                    //{
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        dr = dt.NewRow();
                        dr["Student_id"] = ds1.Tables[0].Rows[i]["Student_id"].ToString();
                        dr["Student_Name"] = ds1.Tables[0].Rows[i]["Student_Name"].ToString();
                        dr["Type"] = ds1.Tables[0].Rows[i]["Type"].ToString();
                        dr["Division"] = ds1.Tables[0].Rows[i]["Division"].ToString();
                        dr["division_id"] = ds1.Tables[0].Rows[i]["division_id"].ToString();

                        dt.Rows.Add(dr);
                        //}
                        //else
                        //{
                        //dr1 = dt1.NewRow();
                        //dr1["Student_id"] = ds1.Tables[0].Rows[0]["Student_id"].ToString();
                        //dr1["Student_Name"] = ds1.Tables[0].Rows[0]["Student_Name"].ToString();
                        //dr1["Type"] = ds1.Tables[0].Rows[0]["Type"].ToString();
                        //dt1.Rows.Add(dr1);
                        //}
                    }
                }
                con.Close();
                ds.Tables.Add(dt);
                ds.Tables.Add(dt1);
            }
            else if (st.type == "toyear")
            {
                string strayid = "select * from (select AYID,duration,SUBSTRING(AYID,4,4) as rank from m_academic  ) a where a.rank > SUBSTRING('" + st.ayid + "',4,4)  order by AYID ";
                ds = cls.fillds(strayid);
                ds.Tables[0].TableName = "toayid";
            }
            else if (st.type == "insert")
            {


                DataTable dt2 = JsonConvert.DeserializeObject<DataTable>(st.json);



                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    string student_id = dt2.Rows[i]["STUDENT ID"].ToString();
                    string check_flag = dt2.Rows[i]["CHECK"].ToString();
                    string division = dt2.Rows[i]["Division Id"].ToString();
                    string divisioname = dt2.Rows[i]["Division"].ToString();
                    //string grpid = dt2.Rows[i]["Group Id"].ToString();
                    string grpname = dt2.Rows[i]["Group Name"].ToString();

                    string grpid = "";


                    string DIVID = "";
                    DIVID = "Select division_id from mst_division_tbl where AYID='" + st.promotedivyear + "' AND  class_id in (select std_id from mst_standard_tbl where std_id='"
                       + st.standard + "' AND del_flag=0) AND  division_name=N'" + divisioname + "' ";


                    string groupid = "";

                    groupid= "Select distinct group_id from exm_group_master where ayid='" + st.promotedivyear + "' and medium_id='"+st.medium+"' and class_id='"+st.standard+"' and group_name =N'"+ grpname + "' and del_flag=0";
                 


                    groupiddt = cls.filldt(groupid);

                    divdt = cls.filldt(DIVID);
                    if (divdt.Rows.Count == 0)

                    {

                    }



                    else
                    {


                        if ( st.standard == "STD014" || st.standard == "STD015"  || st.standard == "STD033" || st.standard == "STD034")

                        {
                            if (groupiddt.Rows.Count == 0)
                            {


                            }

                            else
                            {

                                con.Open();
                                strquery = "STUDENT_TRANSFER";

                                using (SqlCommand cmd = new SqlCommand(strquery, con))
                                {
                                    string str = "transfer";

                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@Student_id", student_id);
                                    cmd.Parameters.AddWithValue("@medium_id", st.medium);
                                    cmd.Parameters.AddWithValue("@class_id", st.standard);
                                    division = divdt.Rows[0]["division_id"].ToString();
                                    cmd.Parameters.AddWithValue("@division_id", division);
                                    grpid = groupiddt.Rows[0]["group_id"].ToString();

                                    cmd.Parameters.AddWithValue("@Group_id", grpid);
                                    cmd.Parameters.AddWithValue("@AYID", st.ayid);
                                    cmd.Parameters.AddWithValue("@StatementType", str);
                                    cmd.Parameters.AddWithValue("@del_flag", DBNull.Value);

                                    if (cmd.ExecuteNonQuery() > 0)
                                    {

                                    }
                                    else
                                    {
                                        type = "not transfered";
                                    }
                                    con.Close();

                                }


                            }


                        }

                        else

                        {
                            con.Open();
                            strquery = "STUDENT_TRANSFER";

                            using (SqlCommand cmd = new SqlCommand(strquery, con))
                            {
                                string str = "transfer";

                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@Student_id", student_id);
                                cmd.Parameters.AddWithValue("@medium_id", st.medium);
                                cmd.Parameters.AddWithValue("@class_id", st.standard);
                                division = divdt.Rows[0]["division_id"].ToString();
                                cmd.Parameters.AddWithValue("@division_id", division);
                                

                                cmd.Parameters.AddWithValue("@Group_id","null" );
                                cmd.Parameters.AddWithValue("@AYID", st.ayid);
                                cmd.Parameters.AddWithValue("@StatementType", str);
                                cmd.Parameters.AddWithValue("@del_flag", DBNull.Value);

                                if (cmd.ExecuteNonQuery() > 0)
                                {

                                }
                                else
                                {
                                    type = "not transfered";
                                }
                                con.Close();

                            }


                        }

                      
                    }
                }
                DataTable dt = new DataTable("MyTable");
                dt.Columns.Add(new DataColumn("msg", typeof(string)));
                dt.Columns.Add(new DataColumn("0", typeof(string)));
                dt.Columns.Add(new DataColumn("1", typeof(string)));
                DataRow dr = dt.NewRow();

                if (type != "")
                {

                    if (divdt.Rows.Count == 0)

                    {

                        dr["msg"] = "Division Not Transfered For Current Year";
                        dt.Rows.Add(dr);

                        dr["1"] = "Red";



                        ds.Tables.Add(dt);
                    }



                    else if (st.standard == "9" || st.standard == "10" || st.standard == "९ वी" || st.standard == "१० वी")

                        if (groupiddt.Rows.Count == 0)
                        {
                            dr["msg"] = "Group Not Transfered For Current Year";
                            dt.Rows.Add(dr);

                            dr["1"] = "Red";

                            ds.Tables.Add(dt);
                        }

                        
                       else {
                            dr["msg"] = "Students are Transfered to ";
                            dt.Rows.Add(dr);
                            dr["0"] = "Green";
                            ds.Tables.Add(dt);
                        }
                    
                    else
                    {
                        dr["msg"] = "Students are Transfered to ";
                        dt.Rows.Add(dr);
                        dr["0"] = "Green";
                        ds.Tables.Add(dt);
                    }

                }
                else
                {
                    dr["msg"] = "No FormId";
                    dt.Rows.Add(dr);
                    ds.Tables.Add(dt);
                }
            }
            else if (st.type == "update")
            {

                DataTable dt2 = JsonConvert.DeserializeObject<DataTable>(st.json);

                for (int i = 0; i < dt2.Rows.Count; i++)
                {

                    string student_id = dt2.Rows[i]["STUDENT ID"].ToString();
                    string check_flag = dt2.Rows[i]["CHECK"].ToString();
                    //string division = dt2.Rows[i]["DIVISON"].ToString();
                    string division_ID = dt2.Rows[i]["Division Id"].ToString();
                    //string strselect = "select * from adm_studentacademicyear where del_flag=0 and student_id='"+student_id+"' and AYID in (select AYID from (select AYID,duration,SUBSTRING(AYID,4,4) as rank from m_academic  ) a where a.rank < SUBSTRING('"+st.ayid+"',4,4)  )";
                    //DataTable ds = cls.filldt(strselect);
                    con.Open();
                    strquery = "STUDENT_TRANSFER";

                    using (SqlCommand cmd = new SqlCommand(strquery, con))
                    {
                        string str = "change";


                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Student_id", student_id);//38
                        cmd.Parameters.AddWithValue("@medium_id", DBNull.Value);//2

                        cmd.Parameters.AddWithValue("@del_flag", check_flag);//3
                        //cmd.Parameters.AddWithValue("@division", division);//3
                        cmd.Parameters.AddWithValue("@division_id", division_ID);//3
                        cmd.Parameters.AddWithValue("@AYID", st.ayid);//26
                        cmd.Parameters.AddWithValue("@StatementType", str);//34
                        cmd.Parameters.AddWithValue("@class_id", DBNull.Value);
                        if (cmd.ExecuteNonQuery() > 0)
                        {

                        }
                        else
                        {
                            type = "Not Updated";
                        }

                    }

                    con.Close();
                }
                DataTable dt = new DataTable("MyTable");
                dt.Columns.Add(new DataColumn("msg", typeof(string)));
                DataRow dr = dt.NewRow();

                if (type != "")
                {
                    dr["msg"] = "Students Promotion for Current year is Cancelled";
                    dt.Rows.Add(dr);
                    ds.Tables.Add(dt);
                }
                else
                {
                    dr["msg"] = "No Students are Updated";
                    dt.Rows.Add(dr);
                    ds.Tables.Add(dt);
                }
            }

            else if (st.type == "divcheck")
            {
                string divcheck = "";
                divcheck = "Select d.division_id from mst_standard_tbl s, mst_division_tbl d where s.del_flag = 0 and d.del_flag = 0  and s.std_id = d.class_id and s.med_id = d.medium_id  and d.AYID = '" + st.promotedivyear + "' and s.std_name = N'" + st.standard + "'";

                ds = cls.fillds(divcheck);

                ds.Tables[0].TableName = "divcheck";
            }

            else if (st.type == "grpcheck")
            {
                string grpcheck = "";
                if (st.standard == "9" || st.standard == "10" || st.standard == "९ वी" || st.standard == "१० वी")
                {

                    grpcheck = "Select distinct group_id,group_name from exm_group_master where ayid='" + st.promotedivyear + "' and class_id in (Select std_id from mst_standard_tbl where std_name= N'" + st.standard + "' and del_flag=0) ";
                    ds = cls.fillds(grpcheck);
                    ds.Tables[0].TableName = "grpid";

                }


                else

                { 
                
                
                }
            }



            else
            {

            }

           return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
         }
      }
 }
    


