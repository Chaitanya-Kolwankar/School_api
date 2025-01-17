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
    public class marks_criteriaController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();
        DataSet ds = new DataSet();
        string str = "";

        [HttpPost]
        public HttpResponseMessage loadmarks([FromBody] marks mrk)
        {
            if (mrk.type == "loadmedium")
            {
                string q1 = "select distinct med_id, medium from mst_medium_tbl where del_flag = '0';  ";
                ds = cls.fillds(q1);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (mrk.type == "loadclass")
            {
                str = "select s.std_id, s.std_name from mst_medium_tbl as m inner join mst_standard_tbl as s on m.med_id = s.med_id where s.del_flag = 0 and m.del_flag = 0 and m.med_id='" + mrk.med_id + "' order by m.med_id,s.rank ;  ";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (mrk.type == "loadsubject")
            {
                str = "select subject_id,subject_name from exm_subject_master where medium_id='" + mrk.med_id + "' and class_id='" + mrk.class_id + "' and del_flag=0 and subject_id in (select subject_id from exm_exam_master where medium_id='" + mrk.med_id + "' and class_id='" + mrk.class_id + "'  and del_flag=0 and exam_id='" + mrk.exam_id + "' and ayid='" + mrk.ayid + "' and exam_type='" + mrk.exam_type + "')";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (mrk.type == "loadexam")
            {
                str = "select distinct exam_name,exam_id,ref_id from exm_exam_master where ayid='" + mrk.ayid + "' and medium_id='" + mrk.med_id + "' and class_id='" + mrk.class_id + "' and subject_id='" + mrk.sub_id + "' and del_flag=0  order by exam_id;select formula,ref_formula from exm_exam_master where medium_id='" + mrk.med_id+"' and class_id='"+mrk.class_id+"'  and del_flag=0 and exam_id='"+mrk.exam_id+"' and ayid='"+mrk.ayid+"' and exam_type='"+mrk.exam_type+"' and subject_id='"+mrk.sub_id+"' and formula is not null";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (mrk.type == "checkref")
            {
                str = "select distinct exam_name,exam_id,ref_id from exm_exam_master where ayid='" + mrk.ayid + "' and medium_id='" + mrk.med_id + "' and class_id='" + mrk.class_id + "' and subject_id='" + mrk.sub_id + "' and del_flag=0 order by exam_id;select distinct ref_id from exm_exam_master where ayid='" + mrk.ayid + "' and medium_id='"+mrk.med_id+"' and class_id='"+mrk.class_id+"' and subject_id='" + mrk.sub_id + "' and del_flag=0 and ref_id in ('"+mrk.refid+ "') order by ref_id ";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (mrk.type == "checkexam")
            {
                str = "select * from exm_exam_master where class_id='" + mrk.class_id + "' and medium_id='" + mrk.med_id + "' and ayid='" + mrk.ayid + "' and ref_id in (select ref_id from exm_exam_master where class_id='" + mrk.class_id + "' and medium_id='" + mrk.med_id + "' and ayid='" + mrk.prevayid + "' and exam_id='" + mrk.exam_id + "' and del_flag=0 and exam_type='" + mrk.exam_type + "') and exam_type='" + mrk.exam_type + "'";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (mrk.type == "checksub")
            {
                str = "select * from exm_exam_master where class_id='" + mrk.class_id + "' and medium_id='" + mrk.med_id + "' and ayid='" + mrk.ayid + "' and ref_id in (select ref_id from exm_exam_master where class_id='" + mrk.class_id + "' and medium_id='" + mrk.med_id + "' and ayid='" + mrk.prevayid + "' and exam_id='" + mrk.exam_id + "' and del_flag=0 and exam_type='" + mrk.exam_type + "' and subject_id='"+mrk.sub_id+"') and exam_type='" + mrk.exam_type + "' and subject_id='"+mrk.sub_id+"'";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (mrk.type == "loadexmtype")
            {
                str = "select distinct exam_type,case when exam_type='Theory' then '1' when exam_type='Practical'  then '2'   when exam_type='Internal' then '3' when exam_type='Grade' then '4' end as abc from exm_exam_master where class_id='" + mrk.class_id + "'and medium_id ='" + mrk.med_id + "'  and ayid='" + mrk.ayid + "' and exam_id='" + mrk.exam_id + "' order by abc";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (mrk.type == "loadprevayid")
            {
                str = "select AYID,Duration from m_academic where AYID < ('" + mrk.ayid + "')  order by AYID desc";
                ds = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (mrk.type == "insert")
            {
                string qry = "";
                SqlCommand cmd = new SqlCommand();
                qry = qry + "EXECUTE [sp_MarksCriteria] '" + mrk.med_id + "','" + mrk.class_id + "','" + mrk.exam_type + "','" + mrk.exam_id + "','" + mrk.ayid + "','" + mrk.sub_id + "','" + mrk.formula.Trim() + "','" + mrk.refformula.Trim() + "','" + mrk.prevayid + "','" + mrk.sptype + "'; ";

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
