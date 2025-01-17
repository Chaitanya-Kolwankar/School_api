using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;

namespace utkarsha_api.Controllers
{
    public class ResultController : ApiController
    {
        result res = new result();
        Exam_Master em = new Exam_Master();
        DataSet ds = new DataSet();
        Class1 cls = new Class1();
        [HttpPost]
        public HttpResponseMessage Result([FromBody] result res)
        {
            string type = res.type;
            string med_id = res.med_id;
            string std_id = res.std_id;
            string div_id = res.div_id;
            string ayid = res.ayid;
            string exam_id = res.exam_id;

               if (type == "nine_result")
                {
                    string query = " select distinct  a.Student_id,ISNULL(stud_F_name,'')+' '+ ISNULL (stud_m_name,'')+' '+ISNULL(stud_L_name,'') as student_name,a.medium_id,a.class_id,b.gr_no, ";
                    query += " cast (b.Roll_no as int) as roll_no,b.division_id,c.duration  from adm_student_master  as a,adm_studentacademicyear as b, ";
                    query += " m_academic as c,exm_marks_entry as d where a.Student_id=b.student_id and a.Student_id=d.stud_id  and b.student_id=d.stud_id and c.AYID=d.AYID ";
                    query += " and d.medium_id='" + med_id + "' and d.class_id='" + std_id + "' and d.AYID='" + ayid + "'  order by CAST(b.Roll_no as int ); ";

                    query+=  " select distinct b.subject_id,b.subject_name,b.criteria,a.out_of from exm_Exam_master as a ,exm_subject_master as b ";
                    query+= " where a.subject_id=b.subject_id and a.AYID='" + ayid + "' and b.medium_id='" + med_id + "' and b.class_id='" + std_id + "' order by b.subject_id; ";
                    ds = cls.fillds(query);
                }
                else if (type == "result")
                {

                    string query = " select a.medium_id,a.class_id,a.subject_id,a.subject_name,b.exam_type,b.exam_id,b.exam_name,out_of,passing,b.AYID from exm_subject_master as a,exm_Exam_master as b ";
                    query+= "  where a.subject_id=b.subject_id and a.subject_id in (select distinct subject_id from exm_marks_entry where ";
                    query+= " medium_id='" + med_id + "' and class_id='" + std_id + "' and division_id='" + div_id + "' and sem_id='" + exam_id + "' and AYID='" + ayid + "' ";
                    query+= " and del_flag=0) and a.medium_id='" + med_id + "' and a.class_id='" + std_id + "' ";
                    query+= " and b.exam_id='" + exam_id + "' and b.AYID='" + ayid + "'  and a.del_flag=0 and b.del_flag=0 order by a.subject_id,b.exam_type; ";

                    query += "  select distinct  b.gr_no,b.student_id,ISNULL(stud_F_name,'')+' '+ISNULL(stud_m_name,'')+' '+ISNULL(stud_L_name,'') as Student_name,b.division_id,b.Roll_no,b.AYID ";
                    query += " from  adm_student_master  as a,adm_studentacademicyear as b,exm_marks_entry as c ";
                    query += " where a.Student_id=b.student_id and a.Student_id=c.stud_id and b.student_id=c.stud_id and ";
                    query += "  b.medium_id='" + med_id + "' and b.class_id='" + std_id + "' and c.sem_id='" + exam_id + "' and b.division_id='" + div_id + "' and c.AYID='" + ayid + "' order by b.Roll_no; ";

                    ds = cls.fillds(query);
                }
                else if (type == "fill_stud_data_primary")
                {
                    string query = " select distinct   a.stud_id, a.subject_id,b.subject_name,a.ayid,b.criteria, cast((select sum(case when marks='Ab' then '0'else  cast(marks as int) end) from exm_marks_entry where  subject_id=b.subject_id ";
                   query+= " and  stud_id='"+res.stud_id+"'  and sem_id='"+exam_id+"') as varchar) as Marks,(select   sum(cast(out_of as int)) from exm_Exam_master where  medium_id='"+med_id+"' ";
                   query+= " and class_id='"+std_id+"' and AYID='"+ayid+"' and del_flag=0 and exam_id='"+exam_id+"' and exam_type<>3  and subject_id=a.subject_id) as out_of, ";
                   query+= " (select   sum(cast(passing as int)) from exm_Exam_master where  medium_id='"+med_id+"' and class_id='"+std_id+"' and AYID='"+ayid+"' and del_flag=0 and exam_id='"+exam_id+"' ";
                   query+= " and exam_type<>3  and subject_id=a.subject_id) as passing  from exm_marks_entry as a ,exm_subject_master as b ,exm_Exam_master as c ";
                   query+= "  where a.subject_id=b.subject_id and a.subject_id=c.subject_id and a.exam_type=c.exam_type and a.del_flag=0 and b.del_flag=0 and c.del_flag=0 ";
                   query+= " and a.sem_id=c.exam_id and b.subject_id=c.subject_id  and stud_id='"+res.stud_id+"'  and sem_id='"+exam_id+"' and a.subject_id in (select subject_id from exm_subject_master  where ";
                   query+= " criteria='Marks' and medium_id='"+med_id+"'  and class_id='"+std_id+"' and AYID='"+ayid+"' and del_flag=0 ) ";
                   query+= " union ";
                   query+= "  select distinct  a.stud_id,  a.subject_id,b.subject_name,a.ayid,b.criteria,a. Marks ,c.out_of,c.passing from exm_marks_entry as a ,exm_subject_master as b ,exm_Exam_master as c ";
                   query+= "   where a.subject_id=b.subject_id and a.subject_id=c.subject_id and a.exam_type=c.exam_type and a.del_flag=0 and b.del_flag=0 and c.del_flag=0  and a.sem_id=c.exam_id  ";
                   query+= "  and b.subject_id=c.subject_id  and stud_id='"+res.stud_id+"'  and sem_id='"+exam_id+"' and a.subject_id in (select subject_id from exm_subject_master  where  ";
                   query+= " criteria='Grade' and medium_id='"+med_id+"' and class_id='"+std_id+"' and AYID='"+ayid+"' and del_flag=0) order by a.subject_id; ";

                    ds = cls.fillds(query);
                 
                }
                else if (type == "fill_stud_data_secondary")
                {
                    string query = " select distinct   a.stud_id, a.subject_id,b.subject_name,a.ayid,b.criteria, cast((select sum(case when marks='Ab' then '0'else  cast(marks as int) end) from exm_marks_entry where  subject_id=b.subject_id  ";
                   query+= " and stud_id='" + res.stud_id + "' and sem_id in (select distinct  exam_id  from exm_Exam_master where (exam_name like '%term 1%' or exam_name like '%term I%' or exam_name like '%semester 1%' ";
                   query+= " or exam_name like '%semester I%' )  and AYID='"+ayid+"' and medium_id='"+med_id+"' and class_id='"+std_id+"')) as varchar) as Term_I,(select   sum(cast(out_of as int)) from exm_Exam_master where  ";
                   query+= " medium_id='"+med_id+"'  and class_id='" + std_id + "' and AYID='" + ayid + "' and del_flag=0 and exam_id in (select distinct  exam_id  from exm_Exam_master where (exam_name like '%term 1%' or ";
                   query+= "  exam_name like '%term I%' or exam_name like '%semester 1%' or exam_name like '%semester I%' )  and AYID='" + ayid + "' and medium_id='" + med_id + "' and class_id='"+std_id+"') and exam_type<>3 ";
                   query+= " and subject_id=a.subject_id) as Term_I_out_of,  (select   sum(cast(passing as int)) from exm_Exam_master where  medium_id='" + med_id + "' and class_id='" + std_id + "' and AYID='" + ayid + "' and ";
                   query+= "  del_flag=0 and exam_id in (select distinct  exam_id  from exm_Exam_master where (exam_name like '%term 1%' or exam_name like '%term I%' or exam_name like '%semester 1%' or";
                   query+= " exam_name like '%semester I%' ) and AYID='" + ayid + "' and medium_id='" + med_id + "' and class_id='" + std_id + "')  and exam_type<>3  and subject_id=a.subject_id) as Term_I_passing,";
                   query+= " cast((select sum(case when marks='Ab' then '0'else  cast(marks as int) end) from exm_marks_entry where  subject_id=b.subject_id  and stud_id='"+res.stud_id+"' ";
                   query+= " and sem_id in (select distinct  exam_id  from exm_Exam_master where (exam_name like '%term 2%' or exam_name like '%term II%' or exam_name like '%semester 2%' or exam_name like '%semester II%' )";
                   query+= "  and AYID='" + ayid + "' and medium_id='" + med_id + "' and class_id='" + std_id + "')) as varchar) as Term_II,(select   sum(cast(out_of as int)) from exm_Exam_master where  medium_id='" + med_id + "'  and class_id='" + std_id + "' ";
                   query+= " and AYID='" + ayid + "' and del_flag=0 and exam_id in (select distinct  exam_id  from exm_Exam_master where (exam_name like '%term 2%' or exam_name like '%term II%' or exam_name like '%semester 2%' or ";
                   query+= " exam_name like '%semester II%' )  and AYID='" + ayid + "' and medium_id='" + med_id + "' and class_id='" + std_id + "') and exam_type<>3  and subject_id=a.subject_id) as Term_II_out_of,(select   sum(cast(passing as int))";
                   query+= " from exm_Exam_master where  medium_id='" + med_id + "' and class_id='" + std_id + "' and AYID='" + ayid + "' and del_flag=0 and exam_id in (select distinct  exam_id  from exm_Exam_master where (exam_name like '%term 2%' or ";
                   query+= "  exam_name like '%term II%' or exam_name like '%semester 2%' or exam_name like '%semester II%' ) and AYID='" + ayid + "' and medium_id='" + med_id + "' and class_id='" + std_id + "')  and exam_type<>3  and ";
                   query+= "	subject_id=a.subject_id) as Term_II_passing from exm_marks_entry as a ,exm_subject_master as b ,exm_Exam_master as c   where a.subject_id=b.subject_id and a.subject_id=c.subject_id and  ";
                   query+= " a.exam_type=c.exam_type and a.del_flag=0 and b.del_flag=0 and c.del_flag=0  and a.sem_id=c.exam_id and b.subject_id=c.subject_id  and stud_id='" + res.stud_id + "'  and sem_id in (select distinct  exam_id ";
                   query+= " from exm_Exam_master where (exam_name like '%term 1%' or exam_name like '%term I%' or exam_name like '%semester 1%' or exam_name like '%semester I%' )";
                   query+= " and AYID='" + ayid + "' and medium_id='" + med_id + "' and class_id='" + std_id + "') and a.subject_id in (select subject_id from exm_subject_master  where criteria='Marks' and medium_id='" + med_id + "'  and class_id='" + std_id + "'";
                   query+= " and AYID='" + ayid + "' and del_flag=0 )";
                   query+= " union ";
                   query+= "  select distinct   a.stud_id, a.subject_id,b.subject_name,a.ayid,b.criteria, (select marks from exm_marks_entry where  subject_id=b.subject_id  and stud_id='" + res.stud_id + "'  ";
                   query+= " and sem_id in (select distinct  exam_id  from exm_Exam_master where (exam_name like '%term 1%' or exam_name like '%term I%' or exam_name like '%semester 1%' or exam_name like '%semester I%' ) ";
                   query+= " and AYID='" + ayid + "' and medium_id='" + med_id + "' and class_id='" + std_id + "') )as Term_I,(select   out_of from exm_Exam_master where  medium_id='" + med_id + "'  and class_id='" + std_id + "' ";
                   query+= " and AYID='" + ayid + "' and del_flag=0 and exam_id in (select distinct  exam_id  from exm_Exam_master where (exam_name like '%term 1%' or exam_name like '%term I%' or exam_name like '%semester 1%' or ";
                   query+= " exam_name like '%semester I%' )  and AYID='" + ayid + "' and medium_id='" + med_id + "' and class_id='" + std_id + "') and exam_type='3'  and subject_id=a.subject_id) as Term_I_out_of,  (select   passing ";
                   query+= " from exm_Exam_master where  medium_id='" + med_id + "' and class_id='" + std_id + "' and AYID='" + ayid + "' and del_flag=0 and exam_id in (select distinct  exam_id  from exm_Exam_master where (exam_name like '%term 1%'  ";
                   query+= " or exam_name like '%term I%' or exam_name like '%semester 1%' or exam_name like '%semester I%' ) and AYID='" + ayid + "' and medium_id='" + med_id + "' and class_id='" + std_id + "')  and exam_type='3'  and ";
                   query+= " subject_id=a.subject_id) as Term_I_passing, (select marks from exm_marks_entry where  subject_id=b.subject_id  and stud_id='"+res.stud_id+"' ";
                   query+= " and sem_id in (select distinct  exam_id  from exm_Exam_master where (exam_name like '%term 2%' or exam_name like '%term II%' or exam_name like '%semester 2%' or exam_name like '%semester II%' ) ";
                   query+= " and AYID='" + ayid + "' and medium_id='" + med_id + "' and class_id='" + std_id + "'))  as Term_II,(select   out_of from exm_Exam_master where  medium_id='" + med_id + "'  and class_id='" + std_id + "'  ";
                   query+= " and AYID='" + ayid + "' and del_flag=0 and exam_id in (select distinct  exam_id  from exm_Exam_master where (exam_name like '%term 2%' or exam_name like '%term II%' or exam_name like '%semester 2%' or ";
                   query+= " exam_name like '%semester II%' )  and AYID='" + ayid + "' and medium_id='" + med_id + "' and class_id='" + std_id + "') and exam_type='3'  and subject_id=a.subject_id) as Term_II_out_of,(select   passing ";
                   query+= " from exm_Exam_master where  medium_id='" + med_id + "' and class_id='" + std_id + "' and AYID='" + ayid + "' and del_flag=0 and exam_id in (select distinct  exam_id  from exm_Exam_master where (exam_name like '%term 2%' or ";
                   query+= "  exam_name like '%term II%' or exam_name like '%semester 2%' or exam_name like '%semester II%' ) and AYID='" + ayid + "' and medium_id='" + med_id + "' and class_id='" + std_id + "')  and exam_type='3'  and ";
                   query+= " subject_id=a.subject_id) as Term_II_passing from exm_marks_entry as a ,exm_subject_master as b ,exm_Exam_master as c   where a.subject_id=b.subject_id and a.subject_id=c.subject_id";
                   query+= "  and a.exam_type=c.exam_type and a.del_flag=0 and b.del_flag=0 and c.del_flag=0  and a.sem_id=c.exam_id and b.subject_id=c.subject_id  and stud_id='" + res.stud_id + "'  ";
                   query+= " and sem_id in (select distinct  exam_id from exm_Exam_master where (exam_name like '%term 1%' or exam_name like '%term I%' or exam_name like '%semester 1%' or exam_name like '%semester I%' ) ";
                   query+= " and AYID='" + ayid + "' and medium_id='" + med_id + "' and class_id='" + std_id + "') and a.subject_id in (select subject_id from exm_subject_master  where criteria='Grade' and medium_id='" + med_id + "'  and class_id='" + std_id + "' ";
                   query+= " 	and AYID='" + ayid + "' and del_flag=0 )";

                   ds = cls.fillds(query);
                }
               
            return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
        }
    }
}
