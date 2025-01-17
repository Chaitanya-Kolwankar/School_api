using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using utkarsha_api.App_Start;

namespace utkarsha_api.Controllers
{
    public class GroupAllocationController : ApiController
    {
        grpallocation gm = new grpallocation();

        Class1 cls = new Class1();
        DataSet ds = new DataSet();
        [HttpPost]
        public HttpResponseMessage GroupAllocation([FromBody] grpallocation gm)
        {
            try
            {
                string strquery = "";
                string strextquery = "";
                string ordext = "";
                string grptype = "";
                if (gm.type == "gvfill")
                {
                    if (gm.division != null)
                    {
                        strextquery = " and ay.division_id='" + gm.division + "'";

                    }
                    else
                    {
                        strextquery = "";

                    }
                    if (gm.orderby == "divroll")
                    {
                        ordext = "c.Roll_no,d.division_name";
                    }
                    else
                    {
                        ordext = "c.stud_L_name,c.stud_f_name,c.stud_m_name";
                    }
                    if (gm.grouptype == "edit")
                    {
                        grptype = " and ay.Group_id ='" + gm.group_id + "' ";
                    }
                    else
                    {
                        grptype = "and (ay.Group_id  = '' or Group_id='null' or ay.Group_id is null) ";
                    }
                    strquery = "select c.student_id,c.Name,c.Roll_no,d.division_name from (select ay.student_id,isnull(am.stud_L_name,'')+' '+isnull(am.stud_F_name,'')+' '+isnull(am.stud_M_name,'') as Name,ay.Roll_no,ay.division_id,am.stud_L_name,am.stud_m_name,am.stud_F_name from adm_studentacademicyear ay ,adm_student_master am where ay.medium_id='" + gm.medium_id + "' and ay.class_id='" + gm.class_id + "' and ay.AYID='" + gm.AYID + "' " + strextquery + " " + grptype + " and ay.del_flag=0 and ay.student_id=am.Student_id )c left join mst_division_tbl d on d.division_id=c.division_id  order by " + ordext + ";select c.student_id,c.Name,c.Roll_no,d.division_name from (select ay.student_id,isnull(am.stud_L_name,'')+' '+isnull(am.stud_F_name,'')+' '+isnull(am.stud_M_name,'') as Name,ay.Roll_no,ay.division_id,am.stud_L_name,am.stud_m_name,am.stud_F_name from adm_studentacademicyear ay ,adm_student_master am where ay.medium_id='" + gm.medium_id + "' and ay.class_id='" + gm.class_id + "' and ay.AYID='" + gm.AYID + "' " + strextquery + "  and ay.Group_id ='" + gm.group_id + "'  and ay.del_flag=0 and ay.student_id=am.Student_id )c left join mst_division_tbl d on d.division_id=c.division_id  order by " + ordext + "";
                    ds = cls.fillds(strquery);
                    return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
                }
                else if (gm.type == "fillgroup")
                {
                    strquery = "select distinct group_id,group_name from exm_group_master where medium_id='" + gm.medium_id + "' and class_id='" + gm.class_id + "' and ayid='" + gm.AYID + "' and del_flag=0 order by group_id";
                    ds = cls.fillds(strquery);
                    return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");

                }
                else if (gm.type == "Insert")
                {
                    string[] arr = gm.stud_id.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        strquery = strquery + "Exec Group_Allocation '" + gm.medium_id + "','" + gm.class_id + "','" + gm.AYID + "','" + gm.group_id + "','" + arr[i].ToString() + "','Insert'; ";

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
                else if (gm.type == "Delete")
                {
                    string[] arr = gm.stud_id.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        strquery = strquery + "Exec Group_Allocation '" + gm.medium_id + "','" + gm.class_id + "','" + gm.AYID + "','" + gm.group_id + "','" + arr[i] + "','Delete'; ";

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
            catch (Exception ex)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, ex.Message, "application/json");
            }
        }
    }
}
