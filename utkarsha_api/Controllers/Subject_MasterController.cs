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

namespace utkarsha_api.Controllers
{
    public class Subject_MasterController : ApiController
    {
        Subject_Master sm = new Subject_Master();
        DataSet ds = new DataSet();
        Class1 cls = new Class1();

        [HttpPost]
        public HttpResponseMessage Subject_Master([FromBody] Subject_Master sm)
        {
            string strquery = "";
            if (sm.type == "gvfill")
            {
                strquery = "select rank,subject_id,subject_name,criteria,(select COUNT(*) from exm_Exam_master where medium_id='" + sm.medium_id + "' and class_id='" + sm.class_id + "' and del_flag=0 and subject_id=s.subject_id ) as count,(select COUNT(*) from exm_group_master where  medium_id='" + sm.medium_id + "' and class_id='" + sm.class_id + "' and del_flag=0 and subject_id=s.subject_id ) as count_group,'0' as [update] from exm_subject_master s where medium_id='" + sm.medium_id + "' and class_id = '" + sm.class_id + "'  and  del_flag = 0 order by cast(rank as int),subject_id";
                ds = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (sm.type == "delete")
            {
                string chkquery = "select COUNT(*) as countexam from exm_Exam_master where medium_id='" + sm.medium_id + "' and class_id='" + sm.class_id + "' and subject_id='" + sm.subject_id + "' and del_flag=0;select COUNT(*)  as countgroup from exm_group_master where medium_id='" + sm.medium_id + "' and class_id='" + sm.class_id + "' and subject_id='" + sm.subject_id + "' and del_flag=0";
                DataSet chkds=cls.fillds(chkquery);
                if (Convert.ToInt32(chkds.Tables[0].Rows[0]["countexam"].ToString()) > 0 && Convert.ToInt32(chkds.Tables[1].Rows[0]["countgroup"].ToString()) > 0)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "Subject Exist", "application/json");
                }
                else if (Convert.ToInt32(chkds.Tables[0].Rows[0]["countexam"].ToString()) > 0 && Convert.ToInt32(chkds.Tables[1].Rows[0]["countgroup"].ToString()) == 0)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "Exam Exist", "application/json");
                }
                else if (Convert.ToInt32(chkds.Tables[0].Rows[0]["countexam"].ToString()) == 0 && Convert.ToInt32(chkds.Tables[1].Rows[0]["countgroup"].ToString()) > 0)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "Group Exist", "application/json");
                }
                else
                {
                strquery = strquery + "Exec Insert_Update_subject_master '" + sm.medium_id + "','" + sm.class_id + "','" + sm.subject_id + "','" + sm.subject_name + "','" + sm.criteria + "','" + sm.AYID + "','0','" + sm.username + "','Delete'";
                bool msg = cls.exeIUD(strquery);
                if (msg == true)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "Saved", "application/json");
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "Error", "application/json");
                }
                }
            }
            else if (sm.type == "save")
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(sm.table);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strquery = strquery + "Exec Insert_Update_subject_master '" + sm.medium_id + "','" + sm.class_id + "','" + dt.Rows[i]["subject_id"].ToString() + "',N'" + dt.Rows[i]["subject_name"].ToString() + "','" + dt.Rows[i]["criteria"].ToString() + "','" + sm.AYID + "','" + dt.Rows[i]["rank"].ToString() + "','" + sm.username + "',";
                    if (dt.Rows[i]["subject_id"].ToString() != "")
                    {
                        strquery = strquery + "'Update'; ";
                    }
                    else
                    {
                        strquery = strquery + "'Insert'; ";
                    }
                }
                bool msg = cls.exeIUD(strquery);
                if (msg == true)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "Saved", "application/json");
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "Error", "application/json");
                }
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "No Changes Made", "application/json");
            }            
        }
    }
}
