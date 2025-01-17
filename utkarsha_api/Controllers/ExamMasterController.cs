using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace utkarsha_api.Controllers
{
    public class ExamMasterController : ApiController
    {
        Exam_Master exm = new Exam_Master();
        DataSet ds = new DataSet();
        Class1 cls = new Class1();

        [HttpPost]
        public HttpResponseMessage ExamMaster([FromBody] Exam_Master exm)
        {
            if (exm.type == "fillsub")
            {
                string query = "";

                if (exm.exam_id != "")
                {
                    if (exm.exam_type == null)
                    {
                        query += "select distinct subject_id, subject_name,exam_id, exam_name, class_id,cast(rank as int) as rank from V_exam_master where medium_id = '" + exm.medium_id + "' and class_id = '" + exm.class_id + "' and exam_id = '" + exm.exam_id + "' and AYID = '" + exm.ayid + "' order by cast(rank as int) ";
                        ds = cls.fillds(query);
                    }

                    else
                    {

                        query += "select distinct subject_id, subject_name,cast(rank as int) as rank from exm_subject_master where  medium_id = '" + exm.medium_id + "' and class_id = '" + exm.class_id + "' and  del_flag = 0 ";
                        if (exm.exam_type == "Grade")
                        {
                            query += " and criteria='Grade' ";
                        }
                        else
                        {
                            query += " and criteria <>'Grade' ";
                        }
                        query += " and subject_id not in (select subject_id from V_exam_master where medium_id = '" + exm.medium_id + "' and class_id = '" + exm.class_id + "' and exam_id = '" + exm.exam_id + "' and exam_type = '" + exm.exam_type + "' and AYID = '" + exm.ayid + "' )  order by cast(rank as int)";

                        ds = cls.fillds(query);
                    }
                }
                else
                {
                    if (exm.exam_type == "Grade")
                    {

                        query = "select distinct subject_id,subject_name,cast(rank as int) as rank from exm_subject_master where  del_flag = 0 and medium_id = '" + exm.medium_id + "' and criteria='Grade' and class_id = '" + exm.class_id + "'  order by cast(rank as int)";
                    }
                    else if (exm.exam_type != "" && exm.exam_type != null)
                    {
                        query = "select distinct subject_id,subject_name,cast(rank as int) as rank from exm_subject_master where  del_flag = 0 and medium_id = '" + exm.medium_id + "' and criteria <>'Grade' and class_id = '" + exm.class_id + "'  order by cast(rank as int)";
                    }
                    else
                    {
                        query = "select distinct subject_id,subject_name,cast(rank as int) as rank from exm_subject_master where  del_flag = 0 and medium_id = '" + exm.medium_id + "' and class_id = '" + exm.class_id + "'  order by cast(rank as int)";
                    }
                    ds = cls.fillds(query);
                }
            }
            else if(exm.type=="fillyear")
            {
                string query = "";
                query = "select * from m_academic where AYID in (select distinct ayid from exm_exam_master where del_flag=0) and AYID<'" + exm.ayid + "' order by ayid desc";
                ds = cls.fillds(query);
            }
            else if (exm.type == "fillexam")
            {
                string query = "";
                query = "select distinct exam_id, exam_name from exm_Exam_master where medium_id = '" + exm.medium_id + "' and class_id = '" + exm.class_id + "' and del_flag = 0 and AYID = '" + exm.ayid + "' ";
                ds = cls.fillds(query);
            }
            else if (exm.type == "fillpreviousexam")
            {
                string query = "";
                query = "select distinct exam_id, exam_name from exm_Exam_master where medium_id = '" + exm.medium_id + "' and class_id = '" + exm.class_id + "' and del_flag = 0 and AYID = '" + exm.ref_id + "' and ref_id not in(select distinct ref_id from exm_Exam_master where medium_id = '" + exm.medium_id + "' and class_id = '" + exm.class_id + "' and del_flag = 0 and AYID = '" + exm.ayid + "')";
                ds = cls.fillds(query);
            }
            else if (exm.type == "findref")
            {
                string query = "select distinct ref_id from exm_exam_master where medium_id='" + exm.medium_id + "' and class_id='" + exm.class_id + "' and ayid='" + exm.ayid + "' and exam_id='" + exm.exam_id + "' and del_flag=0";
                ds = cls.fillds(query);
            }
            else if (exm.type == "fillgv")
            {
                ds = fillgrid(exm.medium_id, exm.class_id, exm.exam_id, exm.ayid, exm.exam_type);
            }
            else if (exm.type == "update")
            {
                string query = "exec Insert_Update_exam_master @medium_id='" + exm.medium_id + "',@class_id='" + exm.class_id + "',@subject_id='" + exm.subject_id + "',@AYID='" + exm.ayid + "',@exam_id='" + exm.exam_id + "',@exam_name=N'" + exm.exam_name + "',@exam_type='" + exm.exam_type + "',@passing='" + exm.passing + "',@out_of='" + exm.out_of + "',@ref_id='" + exm.ref_id + "',@username='" + exm.username + "',@StatementType='Update';";
                bool msg = cls.exeIUD(query);
                if (msg == true)
                {
                    ds = fillgrid(exm.medium_id, exm.class_id, exm.exam_id, exm.ayid, exm.exam_type);
                    return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "Error", "application/json");
                }
            }
            else if (exm.type == "delete")
            {
                string query = "exec Insert_Update_exam_master @medium_id='" + exm.medium_id + "',@class_id='" + exm.class_id + "',@subject_id='" + exm.subject_id + "',@AYID='" + exm.ayid + "',@exam_id='" + exm.exam_id + "',@exam_name=N'" + exm.exam_name + "',@exam_type='" + exm.exam_type + "',@passing='" + exm.passing + "',@out_of='" + exm.out_of + "',@ref_id='" + exm.ref_id + "',@username='" + exm.username + "',@StatementType='Delete';";
                cls.exeIUD(query);
                ds = fillgrid(exm.medium_id, exm.class_id, exm.exam_id, exm.ayid, "");
            }
            else if (exm.type == "insert")
            {
                if (exm.exam_id == "")
                {
                    DataSet ds = cls.fillds("select CONCAT('EXM',REPLICATE('0',(5-LEN(maxno))),maxno) as id from (select case when max(cast(substring(exam_id,4,5) as int)) is null then 1 else max(cast(substring(exam_id,4,5) as int)+1) end as maxno from exm_Exam_master)a;select CONCAT('R',REPLICATE('0',(4-LEN(maxno))),maxno) as id from (select case when max(cast(substring(ref_id,2,4) as int)) is null then 1 else max(cast(substring(ref_id,2,4) as int)+1) end as maxno from exm_Exam_master)a");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        exm.exam_id = ds.Tables[0].Rows[0]["id"].ToString();
                    }
                    else
                    {

                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        exm.ref_id = ds.Tables[1].Rows[0]["id"].ToString();
                    }
                    else
                    {

                    }
                }
                string[] subject_list = exm.subject_id.Split(new char[] { ',' });
                string query = "";
                foreach (string item in subject_list)
                {
                    query += " exec Insert_Update_exam_master @medium_id='" + exm.medium_id + "',@class_id='" + exm.class_id + "',@subject_id='" + item + "',@AYID='" + exm.ayid + "',@exam_id='" + exm.exam_id + "',@exam_name=N'" + exm.exam_name + "',@exam_type='" + exm.exam_type + "',@passing='" + exm.passing + "',@out_of='" + exm.out_of + "',@ref_id='" + exm.ref_id + "',@username='" + exm.username + "',@StatementType='Insert'; ";
                }
                cls.exeIUD(query);
                ds = fillgrid(exm.medium_id, exm.class_id, exm.exam_id, exm.ayid, exm.exam_type);
            }
            else if (exm.type == "saveprevious")
            {
                DataTable dt = cls.filldt("select * from exm_exam_master where exam_id='" + exm.exam_id + "' and medium_id='" + exm.medium_id + "' and class_id='" + exm.class_id + "' and del_flag=0");
                if (dt.Rows.Count > 0)
                {
                    DataSet ds = cls.fillds("select CONCAT('EXM',REPLICATE('0',(5-LEN(maxno))),maxno) as id from (select case when max(cast(substring(exam_id,4,5) as int)) is null then 1 else max(cast(substring(exam_id,4,5) as int)+1) end as maxno from exm_Exam_master)a;");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        exm.exam_id = ds.Tables[0].Rows[0]["id"].ToString();
                    }
                    else
                    {

                    }

                    string query = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        query += " exec Insert_Update_exam_master @medium_id='" + exm.medium_id + "',@class_id='" + exm.class_id + "',@subject_id='" + dt.Rows[i]["subject_id"].ToString() + "',@AYID='" + exm.ayid + "',@exam_id='" + exm.exam_id + "',@exam_name=N'" + exm.exam_name + "',@exam_type='" + dt.Rows[i]["exam_type"].ToString() + "',@passing='" + dt.Rows[i]["passing_marks"].ToString() + "',@out_of='" + dt.Rows[i]["out_of_marks"].ToString() + "',@ref_id='" + exm.ref_id + "',@username='" + exm.username + "',@StatementType='Insert'; ";
                    }
                    cls.exeIUD(query);
                }
                ds = fillgrid(exm.medium_id, exm.class_id, exm.exam_id, exm.ayid, exm.exam_type);
            }
            return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
        }

        public DataSet fillgrid(string medium_id, string class_id, string exam_id, string ayid, string exam_type)
        {
            string query = "select * from V_exam_master where medium_id = '" + medium_id + "' and class_id = '" + class_id + "' and exam_id = '" + exam_id + "' and AYID = '" + ayid + "' and del_flag = '0' ";
            if (exam_type != "" && exam_type != null)
            {
                query += " and exam_type='" + exam_type + "' ";
            }
            query += " order by CAST(rank as int),exam_type desc";
            DataSet ds = cls.fillds(query);
            return ds;
        }
    }
}
