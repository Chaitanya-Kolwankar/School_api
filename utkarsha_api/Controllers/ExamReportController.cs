using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;

namespace utkarsha_api.Controllers
{
    public class ExamReportController : ApiController
    {

        DataSet ds = new DataSet();
        Class1 cls = new Class1();

        [HttpPost]
        public HttpResponseMessage ExamReport([FromBody]exam_reports er)
        {
            if (er.type == "ddlfill")
            {
                string q1 = "select distinct med_id, medium from mst_medium_tbl where del_flag = '0'";
                q1 += "select s.std_id, s.std_name, m.med_id, m.medium from mst_medium_tbl as m inner join mst_standard_tbl as s on m.med_id = s.med_id where s.del_flag = 0 and m.del_flag = 0 order by m.med_id,s.rank";
                q1 += "select * from m_academic";
                ds = cls.fillds(q1);
                DataTable dt1 = ds.Tables[0];
                DataTable dt2 = ds.Tables[1];

                List<string> med_name_arr = new List<string>();

                foreach (DataRow row in dt1.Rows)
                {
                    med_name_arr.Add(row[0].ToString());
                }

                string[] med_name_array = med_name_arr.ToArray();


                for (int i = 0; i < med_name_array.Length; i++)
                {
                    string temp_dt_name = med_name_array[i].ToString();
                    DataTable temp_dt = new DataTable(temp_dt_name);
                    temp_dt.Columns.Add("std_id");
                    temp_dt.Columns.Add("std_name");

                    foreach (DataRow row in dt2.Rows)
                    {
                        if (row[2].ToString() == med_name_array[i].ToString())
                        {
                            DataRow _newrow = temp_dt.NewRow();
                            _newrow["std_id"] = row[0].ToString();
                            _newrow["std_name"] = row[1].ToString();
                            temp_dt.Rows.Add(_newrow);
                        }
                    }

                    ds.Tables.Add(temp_dt);
                }

                ds.Tables.Remove("Table2");
            }
            else if (er.type == "fillexam")
            {
                string query = "";
                query = "select distinct exam_id, exam_name from exm_Exam_master where medium_id = '" + Convert.ToInt32(er.medium_id) + "' and class_id = '" + er.class_id + "' and del_flag = 0 and AYID = '" + er.ayid + "' ";
                ds = cls.fillds(query);
            }
            else if (er.type == "filldata")
            {
                string term = "";
                if (er.exam_name == "Term I")
                {
                    term = "(exam_name like '%Term I%' or exam_name like '%Term 1%')";
                }
                else if (er.exam_name == "Term II")
                {
                    term = "(exam_name like '%Term II%' or exam_name like '%Term 2%')";
                }
                string query = "select distinct  c.stud_id ,case when a.gender='Male' then  ISNULL(stud_F_name,'')+' '+ISNULL(stud_m_name,'')+' '+ISNULL(stud_L_name,'') else  '/'+ ISNULL(stud_F_name,'')";
                query += " +' '+ISNULL(stud_m_name,'')+' '+ISNULL(stud_L_name,'')  end as Student_name ,a.stud_mo_name as Mother_name ,a.gender,c.medium_id,c.class_id,c.AYID ";
                query += "  from adm_student_master as a ,adm_studentacademicyear as b,exm_marks_entry as c ,exm_Exam_master as d  where a.Student_id=b.student_id";
                query += "  and a.Student_id=c.stud_id and b.student_id=c.stud_id and c.sem_id=d.exam_id and c.exam_type=d.exam_type";
                query += "  and a.del_flag=0 and b.del_flag=0 and c.del_flag=0 and d.del_flag=0 ";
                query += "  and c.medium_id='" + er.medium_id + "' and c.class_id='" + er.class_id + "' and c.AYID='" + er.ayid + "' and " + term + "; ";

                query += " select distinct  a.medium_id,a.class_id,a.subject_id,a.subject_name,a.criteria,b.exam_id,b.exam_name,(select   sum(cast(out_of as int)) from exm_Exam_master where  medium_id='" + er.medium_id + "' ";
                query += "  and class_id='" + er.class_id + "' and AYID='" + er.ayid + "' and del_flag=0 and " + term + " ";
                query += "   and exam_type<>3  and subject_id=a.subject_id) as out_of,(select   sum(cast(passing as int)) from exm_Exam_master where  medium_id='" + er.medium_id + "'  ";
                query += "  and class_id='" + er.class_id + "' and AYID='" + er.ayid + "' and del_flag=0 and " + term + " ";
                query += "  and exam_type<>3  and subject_id=a.subject_id) as Passing,b.AYID from exm_subject_master as a,exm_Exam_master as b,exm_marks_entry as c ";
                query += "  where a.subject_id=b.subject_id and a.subject_id=c.subject_id and b.subject_id=c.subject_id and  b.exam_type<>3 and b.exam_type=c.exam_type and b.exam_id=c.sem_id ";
                query += " and a.medium_id='" + er.medium_id + "' and a.class_id='" + er.class_id + "' and " + term + " ";
                query += " and b.AYID='" + er.ayid + "'  and a.del_flag=0 and b.del_flag=0 order by a.subject_id; ";

                ds = cls.fillds(query);
            }
            else if (er.type == "fill_marks")
            {
                string term = "";
                if (er.exam_name == "Term I")
                {
                    term = "(exam_name like '%term 1%' or exam_name like '%term I%' or exam_name like '%semester 1%'  or exam_name like '%semester I%')";
                }
                else if (er.exam_name == "Term II")
                {
                    term = "(exam_name like '%term 2%' or exam_name like '%term II%' or exam_name like '%semester 2%'  or exam_name like '%semester II%')";
                }

                string query = "  select (select count(stud_id) from grade_marks_by_student where  AYID='"+er.ayid+"' and medium_id='"+er.medium_id+"'  and class_id='"+er.class_id+"' and subject_id='"+er.subject_id+"' and ("+term+")";
                query += "  and gender='Male' and grade='A1') as boys  ,(select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' ";
                query += " and medium_id='" + er.medium_id + "'  and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ") and gender='Female' and grade='A1') as girls,";
                query += " (select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' and medium_id='" + er.medium_id + "' and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ")";
                query += " and grade='A1') as tot union all ";//-----------------A1

                query += " select (select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' and medium_id='" + er.medium_id + "' and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ")";
                query += "  and gender='Male' and grade='A2') as boys  ,(select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' ";
                query += " and medium_id='" + er.medium_id + "'  and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ") and gender='Female' and grade='A2') as girls,";
                query += " (select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' and medium_id='" + er.medium_id + "' and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ")";
                query += " and grade='A2') as tot union all ";//------------------A2

                query += " select (select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' and medium_id='" + er.medium_id + "' and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ")";
                query += "  and gender='Male' and grade='B1') as boys  ,(select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' ";
                query += " and medium_id='" + er.medium_id + "'  and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ") and gender='Female' and grade='B1') as girls,";
                query += " (select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' and medium_id='" + er.medium_id + "' and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ")";
                query += " and grade='B1') as tot union all ";//-----------------------B1

                query += " select (select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' and medium_id='" + er.medium_id + "' and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ")";
                query += "  and gender='Male' and grade='B2') as boys  ,(select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' ";
                query += " and medium_id='" + er.medium_id + "'  and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ") and gender='Female' and grade='B2') as girls,";
                query += " (select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' and medium_id='" + er.medium_id + "' and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ")";
                query += " and grade='B2') as tot union all ";//----------------------B2

                query += " select (select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' and medium_id='" + er.medium_id + "' and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ")";
                query += "  and gender='Male' and grade='C1') as boys  ,(select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' ";
                query += " and medium_id='" + er.medium_id + "'  and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ") and gender='Female' and grade='C1') as girls,";
                query += " (select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' and medium_id='" + er.medium_id + "' and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ")";
                query += " and grade='C1') as tot union all ";//---------------------C1

                query += " select (select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' and medium_id='" + er.medium_id + "' and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ")";
                query += "  and gender='Male' and grade='C2') as boys  ,(select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' ";
                query += " and medium_id='" + er.medium_id + "'  and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ") and gender='Female' and grade='C2') as girls,";
                query += " (select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' and medium_id='" + er.medium_id + "' and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ")";
                query += " and grade='C2') as tot union all ";//---------------------C2

                query += " select (select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' and medium_id='" + er.medium_id + "' and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ")";
                query += "  and gender='Male' and grade='D') as boys  ,(select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' ";
                query += " and medium_id='" + er.medium_id + "'  and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ") and gender='Female' and grade='D') as girls,";
                query += " (select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' and medium_id='" + er.medium_id + "' and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ")";
                query += " and grade='D') as tot union all ";//-----------------------D

                query += " select (select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' and medium_id='" + er.medium_id + "' and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ")";
                query += "  and gender='Male' and grade='E1') as boys  ,(select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' ";
                query += " and medium_id='" + er.medium_id + "'  and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ") and gender='Female' and grade='E1') as girls,";
                query += " (select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' and medium_id='" + er.medium_id + "' and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ")";
                query += " and grade='E1') as tot union all ";//---------------------------E1

                query += " select (select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' and medium_id='" + er.medium_id + "' and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ")";
                query += "  and gender='Male' and grade='E2') as boys  ,(select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' ";
                query += " and medium_id='" + er.medium_id + "'  and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ") and gender='Female' and grade='E2') as girls,";
                query += " (select count(stud_id) from grade_marks_by_student where  AYID='" + er.ayid + "' and medium_id='" + er.medium_id + "' and class_id='" + er.class_id + "' and subject_id='" + er.subject_id + "' and (" + term + ")";
                query += " and grade='E2') as tot";//---------------------E2

                ds = cls.fillds(query);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
        }
    }
}
