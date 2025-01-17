using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace utkarsha_api.Controllers
{
    public class Post_notice_Controller : ApiController
    {
        Class1 cls = new Class1();
        [HttpPost]
        public HttpResponseMessage postnotice([FromBody] postnotice pn)
        {
            if (pn.type == "post")
            {
                //String q1 = "insert into Post_Notice (FileName,FileType,FileData,group_id,del_flag,mod_dt,curr_dt,topic,description,ayid,div,user_id) values('" + pn.filename + "','" + pn.filetype + "','" + pn.filedata + "','" + pn.groupid + "',0,getdate(),getdate(),'" + pn.topic + "','" + pn.description + "','" + pn.ayid + "','" + pn.div + "');";
                String q1 = "insert into Post_Notice (group_id,del_flag,mod_dt,curr_dt,topic,description,div,user_id,ext1) values('" + pn.groupid + "',0,getdate(),getdate(),'" + pn.topic + "','" + pn.description + "','" + pn.div + "','" + pn.userid + "','" + pn.ext1 + "');update adm_studentacademicyear set col1='1' where medium_id='" + pn.groupid + "' and class_id in ('" + pn.ext1.Replace(",", "','") + "') and division_id in ('" + pn.div.Replace(",", "','") + "' )";
                DataSet ds1 = cls.fillds(q1);
                return this.Request.CreateResponse(HttpStatusCode.OK, "HI", "application/json");
            }
            else if (pn.type == "standard")
            {//groupid ==standard
                String q1 = "select std_id,std_name from mst_standard_tbl where med_id in(select med_id from mst_medium_tbl where medium='" + pn.groupid + "')";
                DataSet ds1 = cls.fillds(q1);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (pn.type == "Division")
            {
                String hh = pn.groupid;
                String h = hh.Replace(",", "','");
                //String q2 = "select division_id,division_name from mst_division_tbl where class_id in ('" + h + "')";
                //String q2 = "select division_id,division_name,b.std_name from mst_division_tbl as a , mst_standard_tbl as b  where a.class_id=b.std_id and  a.class_id in ('"+h+"')";
                String q2 = "select division_id,  division_name+'(std '+b.std_name+')' as division_name,b.std_name as div_std from mst_division_tbl as a , mst_standard_tbl as b  where a.class_id=b.std_id and  a.class_id in ('" + h + "')";
                DataSet ds1 = cls.fillds(q2);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (pn.type == "getpost")
            {
                //SELECT a.topic,a.description,a.ext1, (b.emp_fname +  '  ' +b.emp_lname ) as name FROM Post_Notice as a, mst_employee_personal as b where ext1='vss00001' order by Id desc 
                String q2 = "SELECT  convert(varchar, a.curr_dt, 0) as curr_dt , a.topic,a.description,a.ext1, ('by '+b.emp_fname +  '  ' +b.emp_lname ) as name FROM Post_Notice as a, mst_employee_personal as b where user_id='" + pn.groupid + "' and a.ext1=b.emp_id and a.del_flag=0 order by Id desc ";
                string q3 = "SELECT  id,convert(varchar, a.curr_dt, 0) as curr_dt , a.topic,a.description,a.user_id, ('by '+b.emp_fname +  '  ' +b.emp_lname+ ' on '+convert (varchar,curr_dt,105)  ) as name FROM Post_Notice as a, mst_employee_personal as b where user_id='" + pn.groupid + "' and a.user_id=b.emp_id and a.del_flag=0 order by Id desc";
                //String q2 = "SELECT topic,description,ext1  FROM Post_Notice where ext1='"+pn.groupid+"' order by Id desc ";
                DataSet ds1 = cls.fillds(q3);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (pn.type == "delet")
            {
                //SELECT a.topic,a.description,a.ext1, (b.emp_fname +  '  ' +b.emp_lname ) as name FROM Post_Notice as a, mst_employee_personal as b where ext1='vss00001' order by Id desc 
                String q2 = "update Post_Notice set del_flag=1 where id='" + pn.userid + "';";
                DataSet ds1 = cls.fillds(q2);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (pn.type == "getstaff")
            {
                String q2 = "select emp_id,NAME,FATHER,SURNAME,MOTHER,DOB,DOJ_gov,BLOOD_GROUP,GENDER,MARITIAL_STATUS,CASTE,MOBILE1,MOBILE2,EMAIL_ADDRESS," +
"CURRENT_ADDRESS,CURRENT_STATE,CURRENT_PIN,CURRENT_DEPARTMENT_NAME,CURRENT_DESIGNATION,CATEGORY,EMAIL_ADDRESS,[PAN.NO]AS PAN_NO,PHOTO,emp_sign " +
                         "from EmployeePersonal where emp_id='" + pn.userid + "';";
                DataSet ds1 = cls.fillds(q2);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (pn.type == "getstuddetail")
            {
                //                String q2 = "select * from studentdetailforapp where Student_id='"+pn.userid+"'";
                string q2 = "select * from studentdetailforapp where Student_id='" + pn.userid + "'";
                DataSet ds1 = cls.fillds(q2);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (pn.type == "chno")
            {
                string q1 = "select col1 from dbo.adm_studentacademicyear where student_id='" + pn.userid + "'";
                DataSet ds1 = cls.fillds(q1);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (pn.type == "updatenotification")
            {
                string q1 = "update adm_studentacademicyear set col1=0 where student_id='" + pn.userid + "'";
                DataSet ds1 = cls.fillds(q1);
                return this.Request.CreateResponse(HttpStatusCode.OK, " notifiread", "application/json");
            }
            else if (pn.type == "getnotice")
            {
                //string q1 = "select * from Post_Notice  where group_id='"+pn.groupid+"' and ext1 like '%"+pn.ext1+"%' and div like '%"+pn.div+"%'";
                string q1 = "select id,a.topic,a.description,a.user_id,('by '+b.emp_fname +  '  ' +b.emp_lname+  ' on '+convert (varchar,curr_dt,105) ) as name from Post_Notice as a ,mst_employee_personal as b where a.group_id='" + pn.groupid + "' and a.ext1 like '%" + pn.ext1 + "%' and a.div like '%" + pn.div + "%' and a.user_id=b.emp_id";
                DataSet ds1 = cls.fillds(q1);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (pn.type == "getstudent")
            {
                string q1 = "SELECT adm_studentacademicyear.student_id, adm_studentacademicyear.medium_id, adm_studentacademicyear.division_id,adm_studentacademicyear.gr_no,adm_studentacademicyear.Roll_no,adm_student_master.stud_F_name+' '+adm_student_master.stud_m_name+' '+adm_student_master.stud_L_name as name FROM adm_studentacademicyear INNER JOIN adm_student_master ON adm_student_master.Student_id=adm_studentacademicyear.student_id and adm_studentacademicyear.division_id='" + pn.userid + "'";
                DataSet ds1 = cls.fillds(q1);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (pn.type == "takeattendance")
            {
                string q1 = "INSERT INTO student_attendance (del_flag, curr_date, mod_date, emo_id,med_id,std_id,div_id,attendie)VALUES (0, GETDATE(), GETDATE(), 'x12','x12','x12','x12','x12,x12');";
                DataSet ds1 = cls.fillds(q1);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, " no data", "application/json");
        }
    }
}
