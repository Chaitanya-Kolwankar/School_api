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

namespace utkarsha_api.Controllers
{
    public class gazetteController : ApiController
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
        public HttpResponseMessage loadgazette([FromBody] gazettedata gzt)
        {
            if (gzt.type == "loadmedium")
            {
                string q1 = "select distinct med_id, medium from mst_medium_tbl where del_flag = '0';  ";
                ds = cls.fillds(q1);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (gzt.type == "loadclass")
            {
                str = "select s.std_id, s.std_name from mst_medium_tbl as m inner join mst_standard_tbl as s on m.med_id = s.med_id where s.del_flag = 0 and m.del_flag = 0 and m.med_id='" + gzt.med_id + "' order by m.med_id,s.rank ;  ";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (gzt.type == "loaddivision")
            {
                str = "select division_id,division_name from mst_division_tbl where medium_id='" + gzt.med_id + "' and class_id='" + gzt.class_id + "' and AYID='" + gzt.ayid + "' and del_flag=0";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (gzt.type == "loadexam")
            {
                str = "select distinct exam_name,exam_id,ref_id from exm_exam_master where ayid='" + gzt.ayid + "' and medium_id='" + gzt.med_id + "' and class_id='" + gzt.class_id + "' and del_flag=0 order by exam_id";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (gzt.type == "loaddatawithoutformula")
            {
                //str = "  select isnull(b.[GR No],'') as [stud_gr],(isnull(b.[First Name],'')+' '+isnull(b.[Middle Name],'')+' '+isnull(b.Surname,'')) as stud_name,a.stud_id,case when [Roll No] is null then '' else [Roll No] end as roll_no,case when [Roll No] is not null and [Roll No]!='' then LEFT([Roll No] ,PATINDEX('%[0-9]%',[Roll No] )-1) else '' end as [prefix],case when [Roll No] is not null and [Roll No]!='' then CONVERT(INT,SUBSTRING([Roll No] ,PATINDEX('%[0-9]%',[Roll No] ) ,LEN([Roll No] ))) else '' end as [number],a.subject_id,a.marks,(select criteria from exm_subject_master where subject_id=a.subject_id) as [sub_type],passing_marks as compare, out_of_marks as outof,c.exam_type,case when c.exam_type = 'Theory' then '1' when c.exam_type = 'Practical'  then '"+gzt.med_id+"'   when c.exam_type = 'Internal' then '3' when c.exam_type = 'Grade' then '4' end as abc from exm_marks_entry as a, student_details as b,exm_exam_master as c,adm_studentacademicyear as d where a.class_id='" + gzt.class_id+"' and a.medium_id='"+gzt.med_id+"' and a.AYID='"+gzt.ayid+"' and a.exam_id='"+gzt.examid+"' ";
                //if (gzt.groupid != "0")
                //{
                //    str = str + " and b.Group_id='" + gzt.groupid + "'";
                //}
                //if (gzt.division != "0")
                //{
                //    str = str + " and a.division_id='" + gzt.division + "' ";
                //}
                //str = str + " and a.stud_id=b.[Student Id] and a.class_id=b.class_id and a.medium_id=b.medium_id and a.del_flag=0 and c.subject_id=a.subject_id and a.class_id=c.class_id and a.medium_id=c.medium_id and a.AYID=c.ayid and a.exam_id=c.exam_id and b.class_id=c.class_id and b.medium_id=c.medium_id and a.exam_type=c.exam_type and c.del_flag=0 order by prefix,number,stud_id,subject_id,abc  ;select sub.subject_id,sub.subject_name,exm.passing_marks as passing,exm.out_of_marks as outof,exam_type from exm_subject_master as sub inner join exm_exam_master as exm on exm.subject_id=sub.subject_id and exm.class_id=sub.class_id and exm.medium_id=sub.medium_id  where exm.subject_id in (select distinct subject_id from exm_marks_entry where class_id='" + gzt.class_id+"' and medium_id='"+gzt.med_id+"' and ayid='"+gzt.ayid+"' and exam_id='"+gzt.examid+ "'";
                //if (gzt.division != "0")
                //{
                //    str = str + " and division_id='" + gzt.division + "'";
                //}
                //str = str + " and del_flag=0) ";
                //if (gzt.groupid != "0")
                //{
                //    str = str + " and exm.subject_id in (select distinct subject_id from exm_group_master where  ayid = '"+gzt.ayid+"'  and class_id = '"+gzt.class_id+"' and medium_id = '"+gzt.med_id+"' and del_flag = 0 and group_id='"+gzt.groupid+"' )";
                //}
                //str = str +" and exm.ayid='" + gzt.ayid + "' and exm.class_id='" + gzt.class_id + "' and exm.medium_id='" + gzt.med_id + "' and exm.exam_id='" + gzt.examid + "' and exm.del_flag=0  order by subject_id ;select distinct subject_id,exam_type from exm_exam_master where ayid='" + gzt.ayid + "' and exam_id='" + gzt.examid + "' and class_id='" + gzt.class_id + "' and medium_id='" + gzt.med_id + "' and del_flag=0";
                //if (gzt.groupid != "0")
                //{
                //    str = str + " and exm.subject_id in (select distinct subject_id from exm_group_master where  ayid = '" + gzt.ayid + "'  and class_id = '" + gzt.class_id + "' and medium_id = '" + gzt.med_id + "' and del_flag = 0 and group_id='" + gzt.groupid + "' )";
                //}
                //str = str + ";  select distinct stud_id from exm_marks_entry as a,adm_studentacademicyear as b where  a.ayid = '"+gzt.ayid+"' and a.exam_id = '"+gzt.examid+"' and a.class_id = '"+gzt.class_id+"' and a.medium_id = '"+gzt.med_id+"' and a.del_flag = 0 and a.division_id='"+gzt.division+"' ";
                //if (gzt.groupid != "0")
                //{
                //    str = str + "and b.Group_id='"+gzt.groupid+"' ";
                //}
                //str=str+" and a.stud_id=b.student_id and a.del_flag=b.del_flag order by a.stud_id ";
                //if (gzt.division != "0")
                //{
                //    str = str + " and division_id='" + gzt.division + "'";
                //}

                str = "  select isnull(b.[GR No],'') as [stud_gr],(isnull(b.[First Name],'')+' '+isnull(b.[Middle Name],'')+' '+isnull(b.Surname,'')) as stud_name,a.stud_id,case when [Roll No] is null then '' else [Roll No] end as roll_no,case when [Roll No] is not null and [Roll No]!='' then LEFT([Roll No] ,PATINDEX('%[0-9]%',[Roll No] )-1) else '' end as [prefix],case when [Roll No] is not null and [Roll No]!='' then CONVERT(INT,SUBSTRING([Roll No] ,PATINDEX('%[0-9]%',[Roll No] ) ,LEN([Roll No] ))) else '' end as [number],a.subject_id,a.marks,(select criteria from exm_subject_master where subject_id=a.subject_id) as [sub_type],passing_marks as compare, out_of_marks as outof,c.exam_type,case when c.exam_type = 'Theory' then '1' when c.exam_type = 'Practical'  then '" + gzt.med_id + "'   when c.exam_type = 'Internal' then '3' when c.exam_type = 'Grade' then '4' end as abc,(select division_name from mst_division_tbl where division_id=a.division_id) as division from exm_marks_entry as a, student_details as b,exm_exam_master as c ,adm_studentacademicyear as d where a.class_id='" + gzt.class_id + "' and a.medium_id='" + gzt.med_id + "' and a.AYID='" + gzt.ayid + "' and a.exam_id='" + gzt.examid + "'";
                if (gzt.division != "0")
                {
                    str = str + "and a.division_id='" + gzt.division + "'";
                }
                if (gzt.groupid != "0" && gzt.groupid != "null")
                {
                    str = str + "and d.Group_id='" + gzt.groupid + "' ";
                }
                str = str + "and a.stud_id=b.[Student Id] and a.class_id=b.class_id and a.division_id=d.division_id and a.class_id=d.class_id and a.medium_id=b.medium_id and a.del_flag=0 and c.subject_id=a.subject_id and a.class_id=c.class_id and a.medium_id=c.medium_id and a.AYID=c.ayid and a.exam_id=c.exam_id and b.class_id=c.class_id and b.medium_id=c.medium_id and a.exam_type=c.exam_type and c.del_flag=0 and a.stud_id=d.student_id and d.del_flag=a.del_flag order by prefix,number,stud_id,subject_id,abc  ;";


                str = str + "select sub.subject_id,sub.subject_name,exm.passing_marks as passing,exm.out_of_marks as outof,exam_type,case when exam_type = 'Theory' then '1' when exam_type = 'Practical'  then '2'   when exam_type = 'Internal' then '3' when exam_type = 'Grade' then '4' end as abc from exm_subject_master as sub inner join exm_exam_master as exm on exm.subject_id = sub.subject_id and exm.class_id = sub.class_id and exm.medium_id = sub.medium_id  where exm.subject_id in (select distinct subject_id from exm_marks_entry where class_id = '" + gzt.class_id + "' and medium_id = '" + gzt.med_id + "' and ayid = '" + gzt.ayid + "' and exam_id = '" + gzt.examid + "'";
                if (gzt.division != "0")
                {
                    str = str + "and division_id = '" + gzt.division + "'";
                }
                str = str + "and del_flag = 0) ";
                if (gzt.groupid != "0" && gzt.groupid != "null")
                {
                    str = str + "and exm.subject_id in (select distinct subject_id from exm_group_master where ayid = '" + gzt.ayid + "'  and class_id = '" + gzt.class_id + "' and medium_id = '" + gzt.med_id + "' and del_flag = 0";
                    str = str + "and group_id = '" + gzt.groupid + "' )";
                }
                str = str + "and exm.ayid = '" + gzt.ayid + "' and exm.class_id = '" + gzt.class_id + "' and exm.medium_id = '" + gzt.med_id + "' and exm.exam_id = '" + gzt.examid + "' and exm.del_flag = 0   order by subject_id,abc;";

                str = str + "select distinct subject_id,exam_type from exm_exam_master where ayid = '" + gzt.ayid + "' and exam_id = '" + gzt.examid + "' and class_id = '" + gzt.class_id + "' and medium_id = '" + gzt.med_id + "' and del_flag = 0 ";
                if (gzt.groupid != "0" && gzt.groupid != "null")
                {
                    str = str + "and subject_id in (select distinct subject_id from exm_group_master where ayid = '" + gzt.ayid + "'  and class_id = '" + gzt.class_id + "' and medium_id = '" + gzt.med_id + "' and del_flag = 0 and group_id = '" + gzt.groupid + "' );";
                }
                str = str + "select distinct stud_id from exm_marks_entry as a,adm_studentacademicyear as b where a.ayid = '" + gzt.ayid + "' and a.exam_id = '" + gzt.examid + "' and a.class_id = '" + gzt.class_id + "' and a.medium_id = '" + gzt.med_id + "' and a.del_flag = 0 ";
                if (gzt.division != "0")
                {
                    str = str + "and a.division_id = '" + gzt.division + "'";
                }
                if (gzt.groupid != "0" && gzt.groupid != "null")
                {
                    str = str + "and b.Group_id = '" + gzt.groupid + "' ";
                }
                str = str + "and a.stud_id = b.student_id  and a.division_id=b.division_id and a.del_flag = b.del_flag order by a.stud_id;select substring(duration,7,5)+'-'+substring(duration,20,5) as duration from m_academic where ayid='" + gzt.ayid + "'";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (gzt.type == "loaddata")
            {

                //str = "select [GR No] as stud_gr,stud_name,stud_id,case when [Roll No] is null then '' else [Roll No] end as roll_no,case when [Roll No] is not null and [Roll No]!='' then LEFT([Roll No] ,PATINDEX('%[0-9]%',[Roll No] )-1) else '' end as [prefix],case when [Roll No] is not null and [Roll No]!='' then CONVERT(INT,SUBSTRING([Roll No] ,PATINDEX('%[0-9]%',[Roll No] ) ,LEN([Roll No] ))) else '' end as [number],subject_id,marks,exam_type, replace(substring(formula,0,charindex('=',formula)),' ','') as formula,case when  SUBSTRING (formula,CHARINDEX('=',formula)+1,len(formula)) is null then passing_marks else replace(SUBSTRING (formula,CHARINDEX('=',formula)+1,len(formula)),' ','') end  as compare,case when exam_type = 'Theory' then '1' when exam_type = 'Practical'  then '" + gzt.med_id + "'   when exam_type = 'Internal' then '3' when exam_type = 'Grade' then '4' end as abc  from (select distinct stud.[GR No], (stud.[First Name] + ' ' + stud.[Middle Name] + ' ' + stud.Surname) as stud_name, stud_id,[Roll No], subject_id, marks, exam_type, passing_marks, STUFF((Select ' ' + item from(select stud_id, marks, exam_type, subject_id,case when Item not like 'Exm%' then Item else (select marks from exm_marks_entry where class_id = '" + gzt.class_id + "' and medium_id = '" + gzt.med_id + "' ";
                //if (gzt.division != "0")
                //{
                //    str = str + "and division_id = '" + gzt.division + "'";
                //}
                //str = str + "and ayid = '" + gzt.ayid + "'  and exam_id = item and stud_id = a.stud_id and subject_id = a.subject_id and exam_type = a.exam_type ) end as item  from(select m.stud_id, m.marks, m.exam_type, exm.subject_id, exm.formula from exm_marks_entry as m inner join exm_exam_master as exm on m.exam_id = exm.exam_id and m.subject_id = exm.subject_id and m.exam_type = exm.exam_type and m.class_id = exm.class_id and m.medium_id = exm.medium_id and m.AYID = exm.ayid where m.class_id = '" + gzt.class_id + "' and m.medium_id = '" + gzt.med_id + "' and m.ayid = '" + gzt.ayid + "' and m.exam_id = '" + gzt.examid + "'";
                //if (gzt.division != "0")
                //{
                //    str = str + "and division_id = '" + gzt.division + "'";
                //}
                //str = str + "and m.del_flag = 0)a cross apply Split1(a.formula,' ') )   t1 where T1.subject_id = T2.subject_id and t1.stud_id = t2.stud_id and t1.exam_type = t2.exam_type FOR XML PATH('')),1,1,'') as formula from(select passing_marks, stud_id, marks, exam_type, subject_id,case when Item not like 'Exm%' then Item else (select marks from exm_marks_entry where class_id = '" + gzt.class_id + "' and medium_id = '" + gzt.med_id + "' and ayid = '" + gzt.ayid + "' ";
                //if (gzt.division != "0")
                //{
                //    str = str + "and division_id = '" + gzt.division + "'";
                //}
                //str = str + "and exam_id = item and stud_id = a.stud_id and subject_id = a.subject_id and exam_type = a.exam_type ) end as item  from(select exm.passing_marks, m.stud_id, m.marks, m.exam_type, exm.subject_id, exm.formula from exm_marks_entry as m inner join exm_exam_master as exm on m.exam_id = exm.exam_id and m.subject_id = exm.subject_id and m.exam_type = exm.exam_type and m.class_id = exm.class_id and m.medium_id = exm.medium_id and m.AYID = exm.ayid where m.class_id = '" + gzt.class_id + "' and m.medium_id = '" + gzt.med_id + "' and m.ayid = '" + gzt.ayid + "' and m.exam_id = '" + gzt.examid + "'";
                //if (gzt.division != "0")
                //{
                //    str = str + "and division_id = '" + gzt.division + "' ";
                //}
                //str = str + "and m.del_flag = 0)a cross apply Split1(a.formula,' ') )t2 inner join student_details as stud on t2.stud_id = stud.[Student Id] and stud.AYID = '" + gzt.ayid + "' ) dt order by prefix,number,stud_id, subject_id, abc;select sub.subject_id,sub.subject_name,passing,outof,exam_type from(select case when formula is not null then SUBSTRING(formula, CHARINDEX('=',formula)+1,len(formula)) else sum(cast(pass as int)) end as passing,sum(cast(outof as int)) as outof,subject_id,exam_type,case when exam_type = 'Theory' then '1' when exam_type = 'Practical'  then '" + gzt.med_id + "'   when exam_type = 'Internal' then '3' when exam_type = 'Grade' then '4' end as abc from(select case when Item like 'exm%' then(select SUBSTRING(formula, CHARINDEX('=', formula) + 1, len(formula)) from exm_exam_master where class_id = '" + gzt.class_id + "' and medium_id = '" + gzt.med_id + "' and ayid = '" + gzt.ayid + "'  and exam_id = item  and subject_id = a.subject_id and exam_type = a.exam_type) else item end as pass,case when Item like 'exm%' then(select out_of_marks from exm_exam_master where class_id = '" + gzt.class_id + "' and medium_id = '" + gzt.med_id + "' and ayid = '" + gzt.ayid + "'  and exam_id = item  and subject_id = a.subject_id and exam_type = a.exam_type) else item end as outof,subject_id,exam_type,formula from(select formula, subject_id, exam_type from exm_exam_master where class_id= '" + gzt.class_id + "' and medium_id = '" + gzt.med_id + "' and ayid = '" + gzt.ayid + "' and exam_id = '" + gzt.examid + "'  and del_flag = 0)a cross apply Split1(a.formula,' ') where Item like 'EXM%') b group by subject_id, exam_type, formula union all select passing_marks, out_of_marks, subject_id, exam_type,case when exam_type = 'Theory' then '1' when exam_type = 'Practical'  then '" + gzt.med_id + "'   when exam_type = 'Internal' then '3' when exam_type = 'Grade' then '4' end as abc from exm_exam_master where class_id = '" + gzt.class_id + "' and medium_id = '" + gzt.med_id + "' and ayid = '" + gzt.ayid + "' and exam_id = '" + gzt.examid + "' and formula is null and del_flag=0)dt inner join exm_subject_master as sub on dt.subject_id = sub.subject_id and sub.class_id = '" + gzt.class_id + "' and sub.medium_id = '" + gzt.med_id + "' order by subject_id,abc;select distinct subject_id,exam_type from exm_exam_master where ayid='" + gzt.ayid + "' and exam_id='" + gzt.examid + "' and class_id='" + gzt.class_id + "' and medium_id='" + gzt.med_id + "' and del_flag=0;select distinct stud_id from exm_marks_entry where  ayid = '" + gzt.ayid + "' and exam_id = '" + gzt.examid + "' and class_id = '" + gzt.class_id + "' and medium_id = '" + gzt.med_id + "' and del_flag = 0";
                //if (gzt.division != "0")
                //{
                //    str = str + "and division_id='" + gzt.division + "'";
                //}

                //commented this 28_march_2023
                //str = "select [GR No] as stud_gr,stud_name,stud_id,case when [Roll No] is null then '' else [Roll No] end as roll_no,case when [Roll No] is not null and [Roll No]!='' then LEFT([Roll No] ,PATINDEX('%[0-9]%',[Roll No] )-1) else '' end as [prefix],case when [Roll No] is not null and [Roll No]!='' then CONVERT(INT,SUBSTRING([Roll No] ,PATINDEX('%[0-9]%',[Roll No] ) ,LEN([Roll No] ))) else '' end as [number],subject_id,marks,exam_type, replace(substring(formula,0,charindex('=',formula)),' ','') as formula,case when  SUBSTRING (formula,CHARINDEX('=',formula)+1,len(formula)) is null then passing_marks else replace(SUBSTRING (formula,CHARINDEX('=',formula)+1,len(formula)),' ','') end  as compare,[outof],case when exam_type = 'Theory' then '1' when exam_type = 'Practical'  then '2'   when exam_type = 'Internal' then '3' when exam_type = 'Grade' then '4' end as abc ,dt.division from (select distinct stud.[GR No], (stud.[First Name] + ' ' + stud.[Middle Name] + ' ' + stud.Surname) as stud_name, stud_id,[Roll No], subject_id, marks, exam_type, passing_marks,out_of_marks [outof],Division, STUFF((Select ' ' + item from(select stud_id, marks, exam_type, subject_id,case when Item not like 'Exm%' then Item else (select marks from exm_marks_entry where class_id = '" + gzt.class_id + "' and medium_id = '" + gzt.med_id + "' ";
                //if (gzt.division != "0")
                //{
                //    str = str + " and division_id = '" + gzt.division + "'";
                //}
                //str = str + " and ayid = '" + gzt.ayid + "'  and exam_id = item and stud_id = a.stud_id and subject_id = a.subject_id and exam_type = a.exam_type ) end as item  from(select m.stud_id, m.marks, m.exam_type, exm.subject_id, exm.formula,(select division_name from mst_division_tbl where division_id=m.division_id) as division  from exm_marks_entry as m inner join exm_exam_master as exm on m.exam_id = exm.exam_id and m.subject_id = exm.subject_id and m.exam_type = exm.exam_type and m.class_id = exm.class_id and m.medium_id = exm.medium_id and m.AYID = exm.ayid where m.class_id = '" + gzt.class_id + "' and m.medium_id = '" + gzt.med_id + "' and m.ayid = '" + gzt.ayid + "' and m.exam_id = '" + gzt.examid + "'";
                //if (gzt.division != "0")
                //{
                //    str = str + " and division_id = '" + gzt.division + "'";
                //}
                //str = str + " and m.del_flag = 0)a cross apply Split1(a.formula,' ') )   t1 where T1.subject_id = T2.subject_id and t1.stud_id = t2.stud_id and t1.exam_type = t2.exam_type FOR XML PATH('')),1,1,'') as formula from(select passing_marks,out_of_marks, stud_id, marks, exam_type, subject_id,case when Item not like 'Exm%' then Item else (select marks from exm_marks_entry where class_id = '" + gzt.class_id + "' and medium_id = '" + gzt.med_id + "' and ayid = '" + gzt.ayid + "' ";
                //if (gzt.division != "0")
                //{
                //    str = str + " and division_id = '" + gzt.division + "'";
                //}
                //str = str + " and exam_id = item and stud_id = a.stud_id and subject_id = a.subject_id and exam_type = a.exam_type ) end as item  from(select exm.passing_marks,exm.out_of_marks, m.stud_id, m.marks, m.exam_type, exm.subject_id, exm.formula from exm_marks_entry as m inner join exm_exam_master as exm on m.exam_id = exm.exam_id and m.subject_id = exm.subject_id and m.exam_type = exm.exam_type and m.class_id = exm.class_id and m.medium_id = exm.medium_id and m.AYID = exm.ayid where m.class_id = '" + gzt.class_id + "' and m.medium_id = '" + gzt.med_id + "' and m.ayid = '" + gzt.ayid + "' and m.exam_id = '" + gzt.examid + "'";
                //if (gzt.division != "0")
                //{
                //    str = str + " and division_id = '" + gzt.division + "'";
                //}
                //str = str + " and m.del_flag = 0)a cross apply Split1(a.formula,' ') )t2 inner join student_details as stud on t2.stud_id = stud.[Student Id] and stud.AYID = '" + gzt.ayid + "' ) dt,adm_studentacademicyear as acd where ";
                //if (gzt.groupid != "0" && gzt.groupid != "null")
                //{
                //    str = str + " acd.Group_id='" + gzt.groupid + "' and";
                //}
                //str = str + " dt.stud_id=acd.student_id and acd.del_flag=0 order by prefix,number,stud_id, subject_id, abc;";

                string str = "select * from (select isnull(acdyr.gr_no,'') [stud_gr], (Isnull(studmst.stud_F_name,'') + ' ' + Isnull(studmst.stud_m_name,'') + ' ' + Isnull(studmst.stud_L_name,'')) [stud_name], mrks.stud_id, acdyr.Roll_no  [roll_no], case when acdyr.Roll_no is not null and acdyr.Roll_no <> '' then LEFT(acdyr.Roll_no , PATINDEX('%[0-9]%',acdyr.Roll_no )-1) else '' end as [prefix], case when acdyr.Roll_no is not null and acdyr.Roll_no <> '' then CONVERT(INT,SUBSTRING(acdyr.Roll_no ,PATINDEX('%[0-9]%',acdyr.Roll_no) ,LEN(acdyr.Roll_no))) else '' end as [number], mrks.subject_id, mrks.marks, mrks.exam_type, '' [formula], exm.passing_marks [compare], exm.out_of_marks [outof], case when mrks.exam_type = 'Theory' then '1' when mrks.exam_type = 'Practical'  then '2' when mrks.exam_type = 'Internal' then '3' when mrks.exam_type = 'Grade' then '4' end as [abc], div.division_name [division] from exm_marks_entry mrks, adm_studentacademicyear acdyr, adm_student_master studmst, exm_exam_master exm, mst_division_tbl div where mrks.ayid='" + gzt.ayid + "' and mrks.class_id='" + gzt.class_id + "' and mrks.exam_id='" + gzt.examid + "' and mrks.division_id = '" + gzt.division + "' and  ";


                if (gzt.groupid != "0" && gzt.groupid != "null")
                {
                    str = str + "   acdyr.Group_id='" + gzt.groupid + "' and ";
                }

                str += "mrks.medium_id = '" + gzt.med_id + "' and mrks.del_flag = 0 and mrks.stud_id = acdyr.student_id and mrks.ayid = acdyr.AYID and mrks.class_id = acdyr.class_id and mrks.division_id = acdyr.division_id and mrks.medium_id = acdyr.medium_id and mrks.del_flag = acdyr.del_flag and mrks.stud_id = studmst.Student_id  and mrks.medium_id = studmst.medium_id and mrks.del_flag = studmst.del_flag and mrks.exam_id = exm.exam_id and mrks.AYID = exm.ayid and mrks.subject_id = exm.subject_id and mrks.medium_id = exm.medium_id and mrks.class_id = exm.class_id and mrks.del_flag = exm.del_flag and mrks.division_id = div.division_id and mrks.medium_id = div.medium_id and mrks.class_id = div.class_id and mrks.del_flag = div.del_flag) dt order by prefix, number , stud_id, subject_id, abc; ";

                str = str + "select distinct fdt.subject_id,fdt.subject_name,passing,outof/2 outof,exam_type,abc from(select sub.subject_id, sub.subject_name, passing, outof, exam_type, abc from(select case when formula is not null then SUBSTRING(formula, CHARINDEX('=',formula)+1,len(formula)) else sum(cast(pass as int)) end as passing,sum(cast(outof as int)) as outof,subject_id,exam_type,case when exam_type = 'Theory' then '1' when exam_type = 'Practical'  then '2'   when exam_type = 'Internal' then '3' when exam_type = 'Grade' then '4' end as abc from(select case when Item like 'exm%' then(select SUBSTRING(formula, CHARINDEX('=', formula) + 1, len(formula)) from exm_exam_master where class_id = '" + gzt.class_id + "' and del_flag=0   and medium_id = '" + gzt.med_id + "' and ayid = '" + gzt.ayid + "'  and exam_id = item  and subject_id = a.subject_id and exam_type = a.exam_type) else item end as pass,case when Item like 'exm%' then(select out_of_marks from exm_exam_master where class_id = '" + gzt.class_id + "' and del_flag=0   and medium_id = '" + gzt.med_id + "' and ayid = '" + gzt.ayid + "'  and exam_id = item  and subject_id = a.subject_id and exam_type = a.exam_type) else item end as outof,subject_id,exam_type,formula from(select formula, subject_id, exam_type from exm_exam_master where class_id= '" + gzt.class_id + "' and medium_id = '" + gzt.med_id + "' and ayid = '" + gzt.ayid + "' and exam_id = '" + gzt.examid + "'  and del_flag = 0)a cross apply Split1(a.formula,' ') where Item like 'EXM%') b group by subject_id, exam_type, formula union all select passing_marks, out_of_marks, subject_id, exam_type,case when exam_type = 'Theory' then '1' when exam_type = 'Practical'  then '2'   when exam_type = 'Internal' then '3' when exam_type = 'Grade' then '4' end as abc from exm_exam_master where class_id = '" + gzt.class_id + "' and medium_id = '" + gzt.med_id + "' and ayid = '" + gzt.ayid + "' and exam_id = '" + gzt.examid + "' and formula is null and del_flag = 0)dt inner join exm_subject_master as sub on dt.subject_id = sub.subject_id and sub.class_id = '" + gzt.class_id + "' and sub.medium_id = '" + gzt.med_id + "' ) fdt,exm_group_master as grp  ";
                if (gzt.groupid != "0" && gzt.groupid != "null")
                {
                    str = str + " where grp.group_id = '" + gzt.groupid + "' and fdt.subject_id = grp.subject_id and grp.del_flag = 0";
                }
                str = str + "order by fdt.subject_id,fdt.abc;";

                str = str + "select distinct subject_id,exam_type from exm_exam_master where ayid = '" + gzt.ayid + "' and exam_id = '" + gzt.examid + "' and class_id = '" + gzt.class_id + "' and medium_id = '" + gzt.med_id + "' and del_flag = 0 ";
                if (gzt.groupid != "0" && gzt.groupid != "null")
                {
                    str = str + " and subject_id in (select subject_id from exm_group_master where ayid = '" + gzt.ayid + "' and exam_id = '" + gzt.examid + "' and class_id = '" + gzt.class_id + "' and medium_id = '" + gzt.med_id + "' and del_flag = 0 and group_id = '" + gzt.groupid + "');";
                }


                str = str + "select distinct stud_id from exm_marks_entry as a, adm_studentacademicyear as b where a.ayid = '" + gzt.ayid + "' and exam_id = '" + gzt.examid + "' and a.class_id = '" + gzt.class_id + "' and a.medium_id = '" + gzt.med_id + "' and a.del_flag = 0";
                if (gzt.division != "0")
                {
                    str = str + " and a.division_id = '" + gzt.division + "'";
                }
                if (gzt.groupid != "0" && gzt.groupid != "null")
                {
                    str = str + " and b.Group_id = '" + gzt.groupid + "'";
                }
                str = str + " and a.stud_id = b.student_id and b.del_flag = 0;select substring(duration,7,5)+'-'+substring(duration,20,5) as duration from m_academic where ayid='" + gzt.ayid + "'";
                str += " select distinct formula from exm_exam_master where exam_id = '" + gzt.examid + "' and exam_type = 'Theory' and class_id='" + gzt.class_id + "' and ayid='" + gzt.ayid + "';";
                ds = cls.fillds(str);

                if (ds.Tables[5].Rows.Count > 0)
                {
                    string[] parts = ds.Tables[5].Rows[0]["formula"].ToString().Substring(0, (ds.Tables[5].Rows[0]["formula"].ToString().IndexOf('/'))).Split('+');

                    if (parts.Length > 0)
                    {
                        string stud_prev = "";
                        for (int i = 0; i < parts.Length - 1; i++)
                        {
                            stud_prev += "select mrks.stud_id,studmst.Roll_no,mrks.marks,mrks.exam_type from exm_marks_entry mrks, adm_studentacademicyear studmst ,exm_group_master grp where mrks.class_id='" + gzt.class_id + "' and mrks.ayid='" + gzt.ayid + "' and mrks.exam_id = '" + parts[i].Trim() + "' and mrks.division_id = '" + gzt.division + "' and grp.group_id='" + gzt.groupid + "' and mrks.class_id=studmst.class_id and mrks.class_id=grp.class_id and studmst.class_id=grp.class_id and mrks.AYID=studmst.AYID and mrks.AYID=grp.ayid and studmst.AYID=grp.ayid and studmst.Group_id=grp.group_id and mrks.subject_id=grp.subject_id and mrks.division_id=studmst.division_id and mrks.del_flag=0 and mrks.del_flag=studmst.del_flag and mrks.del_flag=grp.del_flag and mrks.stud_id=studmst.student_id order by cast(studmst.Roll_no as int), studmst.student_id,mrks.subject_id";
                        }

                        DataSet stud_prev_exm = cls.fillds(stud_prev);

                        if (stud_prev_exm.Tables.Count > 0)
                        {
                            //string test = "";
                            //for (int z = 0; z < stud_prev_exm.Tables.Count; z++)
                            //{
                            //    test += stud_prev_exm.Tables[0].Rows[j]["marks"].ToString();
                            //}

                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                if (ds.Tables[0].Rows[j]["stud_id"].ToString() == stud_prev_exm.Tables[0].Rows[j]["stud_id"].ToString())
                                {
                                    if (stud_prev_exm.Tables[0].Rows[j]["exam_type"].ToString() == "Theory")
                                    {
                                        ds.Tables[0].Rows[j]["formula"] = stud_prev_exm.Tables[0].Rows[j]["marks"].ToString() + "+" + ds.Tables[0].Rows[j]["marks"].ToString() + "/" + parts.Length;
                                    }
                                }
                            }
                        }
                    }
                }

                ds.Tables.Remove(ds.Tables[5]);

                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");


                //ds = cls.fillds(str1);
                //return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "No changes made", "application/json");
            }
        }
    }
}
