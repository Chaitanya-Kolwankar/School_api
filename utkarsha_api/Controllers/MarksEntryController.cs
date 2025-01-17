using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;

namespace utkarsha_api.Controllers
{
    public class MarksEntryController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();
        DataSet ds = new DataSet();
        string str = "";

        public static T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }

        [HttpPost]
        public HttpResponseMessage loadmarksentry([FromBody] marksentry marks)
        {
            if (marks.type == "loadmedium")
            {
                string q1 = "select distinct med_id, medium from mst_medium_tbl where del_flag = '0';  ";
                ds = cls.fillds(q1);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (marks.type == "loadclass")
            {
                str = "select s.std_id, s.std_name from mst_medium_tbl as m inner join mst_standard_tbl as s on m.med_id = s.med_id where s.del_flag = 0 and m.del_flag = 0 and m.med_id='" + marks.med_id + "' order by m.med_id,s.rank ;  ";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (marks.type == "loadsubject")
            {
                //str = "select subject_id,subject_name from exm_subject_master where medium_id='" + marks.med_id + "' and class_id='" + marks.class_id + "' and del_flag=0 and subject_id in (select subject_id from exm_exam_master where ayid='" + marks.ayid + "' and class_id='" + marks.class_id + "' and medium_id='" + marks.med_id + "' and exam_id='" + marks.examid + "')";


                str = "select subject_id,subject_name from exm_subject_master where medium_id='" + marks.med_id + "' and class_id='" + marks.class_id + "' and del_flag=0 and subject_id in (select subject_id from exm_exam_master where ayid='" + marks.ayid + "' and class_id='" + marks.class_id + "' and medium_id='" + marks.med_id + "' and exam_id='" + marks.examid + "' and del_flag=0)";

                if (marks.groupid != "0" && marks.groupid != "null")
                {
                    str = str + " and subject_id in (select distinct subject_id from exm_group_master where ayid='" + marks.ayid + "' and class_id='" + marks.class_id + "' and medium_id='" + marks.med_id + "' and group_id='" + marks.groupid + "' and del_flag=0)";
                }
                    ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (marks.type == "loaddivision")
            {
                str = "select division_id,division_name from mst_division_tbl where medium_id='" + marks.med_id + "' and class_id='" + marks.class_id + "' and AYID='" + marks.ayid + "' and del_flag=0";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (marks.type == "loadexam")
            {
                str = "select distinct exam_name,exam_id,ref_id from exm_exam_master where ayid='" + marks.ayid + "' and medium_id='" + marks.med_id + "' and class_id='" + marks.class_id + "' and del_flag=0 order by exam_id";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }

            else if (marks.type == "loadgroup")
            {
                str = "select distinct group_id,group_name from exm_group_master where ayid='" + marks.ayid + "' and medium_id='" + marks.med_id + "' and class_id='" + marks.class_id + "' and del_flag=0 order by group_id";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }

            else if (marks.type == "loaddata")
            {
                str = "select distinct a.Student_id,(a.stud_F_name+' '+ a.stud_m_name+' '+a.stud_L_name) as student_name,case when b.Roll_no is null then '' else b.Roll_no end as Roll_no,case when b.Roll_no is not null and b.Roll_no!='' then LEFT(b.Roll_no ,PATINDEX('%[0-9]%',b.Roll_no )-1) else '' end as [prefix],case when b.Roll_no is not null and b.Roll_no!='' then CONVERT(INT,SUBSTRING(b.Roll_no ,PATINDEX('%[0-9]%',b.Roll_no ) ,LEN(b.Roll_no ))) else '' end as [number],m.marks,e.exam_id,e.exam_name,e.exam_type,e.passing_marks,e.out_of_marks,case when e.exam_type='Theory' then '1' when e.exam_type='Practical'  then '2'   when e.exam_type='Internal' then '3' when e.exam_type='Grade' then '4' end as abc from adm_student_master as a, adm_studentacademicyear as b,exm_marks_entry as m,exm_exam_master as e where a.Student_id=b.student_id and m.AYID='" + marks.ayid + "' and m.class_id='" + marks.class_id + "' and b.medium_id='" + marks.med_id + "' and m.exam_id='" + marks.examid + "'";
                if (marks.groupid != "0" && marks.groupid != "null")
                {
                    str = str + "and b.Group_id='"+marks.groupid+"'";
                }

                str = str + "and a.Student_id=m.stud_id and m.subject_id='" + marks.subject_id + "'  and b.division_id='" + marks.division + "' and a.del_flag=0 and b.student_id=m.stud_id and a.del_flag=b.del_flag and m.exam_id=e.exam_id and a.medium_id=b.medium_id and b.medium_id=m.medium_id and a.medium_id=m.medium_id and m.exam_type=e.exam_type and e.del_flag=0 and b.AYID=m.AYID and b.class_id=m.class_id  and b.division_id=m.division_id and m.subject_id=e.subject_id and m.del_flag=0 and a.del_flag=0 union all select a.Student_id,(a.stud_F_name + ' ' + a.stud_m_name + ' ' + a.stud_L_name) as student_name,case when b.Roll_no is null then '' else b.Roll_no end as Roll_no,case when b.Roll_no is not null and b.Roll_no != '' then LEFT(b.Roll_no , PATINDEX('%[0-9]%',b.Roll_no )-1) else '' end as [prefix],case when b.Roll_no is not null and b.Roll_no != '' then CONVERT(INT, SUBSTRING(b.Roll_no, PATINDEX('%[0-9]%',b.Roll_no ) ,LEN(b.Roll_no))) else '' end as [number],'' as marks,e.exam_id,e.exam_name,e.exam_type,e.passing_marks,e.out_of_marks,case when exam_type = 'Theory' then '1' when exam_type = 'Practical'  then '2'   when exam_type = 'Internal' then '3' when exam_type = 'Grade' then '4' end as abc  from adm_student_master as a, adm_studentacademicyear as b,exm_exam_master as e where a.Student_id = b.student_id and b.AYID = '" + marks.ayid + "' and b.class_id = '" + marks.class_id + "' and b.medium_id = '" + marks.med_id + "' and e.exam_id = '" + marks.examid + "' and e.subject_id = '" + marks.subject_id + "' and b.division_id = '" + marks.division + "'";
                if (marks.groupid != "0" && marks.groupid != "null")
                {
                    str = str + "and b.Group_id='" + marks.groupid + "'";
                }
                str =str+"  and a.Student_id not in (select distinct stud_id from exm_marks_entry where exam_id = '"+marks.examid+"' and AYID = '"+marks.ayid+"' and subject_id = '"+marks.subject_id+"' and class_id = '"+marks.class_id+"' and medium_id = '"+marks.med_id+ "') and a.medium_id = b.medium_id and b.AYID = e.ayid and a.del_flag = 0 and a.del_flag = b.del_flag and e.del_flag=0  order by prefix,number,a.Student_id,abc; select exam_id,exam_name,exam_type,passing_marks,out_of_marks,case when exam_type='Theory' then '1' when exam_type='Practical'  then '2'   when exam_type='Internal' then '3' when exam_type='Grade' then '4' end as abc from exm_exam_master where subject_id='" + marks.subject_id+"' and medium_id='"+marks.med_id+"' and class_id='"+marks.class_id+"' and ayid='"+marks.ayid+"' and exam_id='"+marks.examid+ "' and del_flag=0  order by abc";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (marks.type == "checkexcel")
            {
                str = "select distinct a.Student_id from adm_student_master as a, adm_studentacademicyear as b,exm_marks_entry as m,exm_exam_master as e where a.Student_id=b.student_id and m.AYID='" + marks.ayid + "' and m.class_id='" + marks.class_id + "' and b.medium_id='" + marks.med_id + "' and m.exam_id='" + marks.examid + "' and a.Student_id=m.stud_id and m.subject_id='" + marks.subject_id + "'  and b.division_id='" + marks.division + "' and a.del_flag=0 and a.class_id=b.class_id and b.student_id=m.stud_id and a.del_flag=b.del_flag and m.exam_id=e.exam_id and a.medium_id=b.medium_id and b.medium_id=m.medium_id and a.medium_id=m.medium_id and m.exam_type=e.exam_type and e.del_flag=0 and b.AYID=m.AYID and b.class_id=m.class_id and a.class_id=m.class_id and b.division_id=m.division_id  and m.del_flag=0 and a.del_flag=0 union all select distinct a.Student_id from adm_student_master as a, adm_studentacademicyear as b,exm_exam_master as e where a.Student_id = b.student_id and b.AYID = '" + marks.ayid + "' and b.class_id = '" + marks.class_id + "' and b.medium_id = '" + marks.med_id + "' and e.exam_id = '" + marks.examid + "' and e.subject_id = '" + marks.subject_id + "' and b.division_id = '" + marks.division + "' and a.Student_id not in (select distinct stud_id from exm_marks_entry where exam_id = '" + marks.examid + "' and AYID = '" + marks.ayid + "' and subject_id = '" + marks.subject_id + "' and class_id = '" + marks.class_id + "' and medium_id = '" + marks.med_id + "') and a.medium_id = b.medium_id and b.AYID = e.ayid and a.del_flag = 0 and a.del_flag = b.del_flag  order by a.Student_id; select exam_id,exam_name,exam_type,passing_marks,out_of_marks,case when exam_type='Theory' then '1' when exam_type='Practical'  then '2'   when exam_type='Internal' then '3' when exam_type='Grade' then '4' end as abc from exm_exam_master where subject_id='" + marks.subject_id + "' and medium_id='" + marks.med_id + "' and class_id='" + marks.class_id + "' and ayid='" + marks.ayid + "' and exam_id='" + marks.examid + "' order by abc";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }


            else if (marks.type == "insert")
            {
                string qry = "";


                //DataSet arr = JsonConvert.DeserializeObject<DataSet>(marks.mrkdata);
                //JsonObjectAttribute jObj = new JsonObjectAttribute(marks.mrkdata);

                for (int i = 0; i < marks.mrkdata.Length; i++)
                {
                    SqlCommand cmd = new SqlCommand();
                    //string d1 = marks.mrkdata[i]["stud_id"].tostring();
                    qry = qry + "EXECUTE [sp_MarksEntry] '" + marks.mrkdata[i].stud_id + "','" + marks.class_id + "','" + marks.mrkdata[i].exam_type + "','" + marks.examid + "','" + marks.ayid + "','" + marks.subject_id + "','" + marks.med_id + "',N'" + marks.mrkdata[i].marks.ToUpper() + "','" + marks.division + "'; ";
                }
                bool msg = cls.exeIUD(qry);
                if (msg == true)
                {                    
                    return this.Request.CreateResponse(HttpStatusCode.OK, msg, "application/json");
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "Error", "application/json");
                }

            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "No changes made", "application/json");
            }
        }
    }
}
