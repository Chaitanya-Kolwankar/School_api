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
    public class GroupMasterController : ApiController
    {
        group_master gm = new group_master();
        DataSet ds = new DataSet();
        Class1 cls = new Class1();

        [HttpPost]
        public HttpResponseMessage Group_Master([FromBody] group_master gm)
        {
            try
            {
                string strquery = ""; string status = "";
                if (gm.type == "gvfill")
                {
                    if (gm.prevAyid != null)
                    {
                        status = ",'P' as status";
                        gm.AYID = gm.prevAyid;
                    }
                    else
                    {
                        status = ",'A' as status";

                    }
                    strquery = "SELECT group_id,Group_name, subject_id=STUFF  (      (        SELECT DISTINCT ',' + CAST(subject_id AS VARCHAR(MAX))         FROM exm_group_master t2        WHERE t2.group_id = t1.group_id  and t2.medium_id=t1.medium_id and t2.class_id=t1.class_id and t2.ayid=t1.ayid and t2.del_flag=0     FOR XML PATH('')      ),1,1,''  ),Subject_name=STUFF  (     (        SELECT DISTINCT ','+ CAST(g.subject_name AS NVARCHAR(MAX)) FROM exm_subject_master g,exm_group_master e        WHERE g.subject_id=e.subject_id and e.group_id=t1.group_id  and g.medium_id=e.medium_id and g.class_id=e.class_id and e.ayid=t1.ayid and e.del_flag=0     FOR XMl PATH('')      ),1,1,''  ) " + status + " FROM exm_group_master t1  where t1.ayid='" + gm.AYID + "' and t1.medium_id='" + gm.medium_id + "' and t1.class_id='" + gm.class_id + "' and t1.del_flag=0 GROUP BY group_id,group_name,medium_id,class_id,ayid,del_flag";
                    ds = cls.fillds(strquery);
                    return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
                }
                else if (gm.type == "fillsub")
                {
                    strquery = "select subject_id,Subject_name from exm_subject_master s where medium_id='" + gm.medium_id + "' and class_id = '" + gm.class_id + "'  and  del_flag = 0 order by cast(rank as int),subject_id";
                    ds = cls.fillds(strquery);
                    return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
                }
                else if (gm.type == "Insert")
                {
                    string strgrpname = " select * from exm_group_master where  group_name=N'" + gm.group_name + "' and medium_id='" + gm.medium_id + "' and class_id='" + gm.class_id + "' and ayid='" + gm.AYID + "' and del_flag=0";
                    DataTable dtgrpname = cls.filldt(strgrpname);
                    if (dtgrpname.Rows.Count == 0)
                    {
                        string strgrp = "	EXEC Insert_Update_group_master  '' ,'','' ,'' ,'' ,'' ,'' ,'Groupid'  ";
                        DataTable dtgrp = cls.filldt(strgrp);
                        if (dtgrp.Rows.Count > 0)
                        {
                            string group_id = dtgrp.Rows[0][0].ToString();
                            string[] arr = gm.subject_id.Split(',');
                            for (int i = 0; i < arr.Length; i++)
                            {
                                strquery = strquery + "Exec Insert_Update_group_master '" + gm.medium_id + "','" + gm.class_id + "','" + group_id + "',N'" + gm.group_name + "','" + arr[i].ToString() + "','" + gm.AYID + "','" + gm.username + "','Insert'";
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
                            return this.Request.CreateResponse(HttpStatusCode.OK, "GroupIssue", "application/json");
                        }
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, "GroupName", "application/json");
                    }
                }
                else if (gm.type == "Update")
                {

                    string strgrpname = " select * from exm_group_master where  group_name=N'" + gm.group_name + "' and medium_id='" + gm.medium_id + "' and class_id='" + gm.class_id + "' and ayid='" + gm.AYID + "' and group_id not in  ('" + gm.group_id + "')  and del_flag=0";
                    DataTable dtgrpname = cls.filldt(strgrpname);
                    if (dtgrpname.Rows.Count == 0)
                    {
                        string[] arr = gm.subject_id.Split(',');
                        for (int i = 0; i < arr.Length; i++)
                        {
                            string strgrp = "	select * from exm_group_master where group_id='" + gm.group_id + "' and subject_id='" + arr[i] + "' and ayid='" + gm.AYID + "' and del_flag=0";
                            DataTable dtgrp = cls.filldt(strgrp);
                            if (dtgrp.Rows.Count > 0)
                            {
                                strquery = strquery + "Exec Insert_Update_group_master '" + gm.medium_id + "','" + gm.class_id + "','" + gm.group_id + "',N'" + gm.group_name + "','" + arr[i].ToString() + "','" + gm.AYID + "','" + gm.username + "','Update'";
                            }
                            else
                            {
                                strquery = strquery + "Exec Insert_Update_group_master '" + gm.medium_id + "','" + gm.class_id + "','" + gm.group_id + "',N'" + gm.group_name + "','" + arr[i].ToString() + "','" + gm.AYID + "','" + gm.username + "','Insert'";
                            }
                        }

                        //strquery = strquery + "Exec Insert_Update_group_master '" + gm.medium_id + "','" + gm.class_id + "','" + gm.group_id + "','" + gm.group_name + "','" + gm.subject_id + "','" + gm.AYID + "','" + gm.username + "','delexsub'";

                        strquery = strquery + "update exm_group_master set del_flag=1,user_del_dt = GETDATE() where group_id='" + gm.group_id + "' and subject_id not in ('" + gm.subject_id.Replace(",", "','") + "' ) and ayid='" + gm.AYID + "' and medium_id='" + gm.medium_id + "' and class_id='" + gm.class_id + "' and del_flag=0";


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
                        return this.Request.CreateResponse(HttpStatusCode.OK, "Error", "application/json");
                    }



                }
                else if (gm.type == "delete")
                {
                    string strgrp = "select * from adm_studentacademicyear where AYID='" + gm.AYID + "' and Group_id='" + gm.group_id + "' and medium_id='" + gm.medium_id + "' and class_id='" + gm.class_id + "' and del_flag=0";
                    DataTable dtgrp = cls.filldt(strgrp);
                    if (dtgrp.Rows.Count == 0)
                    {
                        strquery = strquery + "Exec Insert_Update_group_master '" + gm.medium_id + "','" + gm.class_id + "','" + gm.group_id + "',N'" + gm.group_name + "','" + gm.subject_id + "','" + gm.AYID + "','" + gm.username + "','Delgroup'";

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
                        return this.Request.CreateResponse(HttpStatusCode.OK, "Students Exist", "application/json");
                    }

                }
                else if (gm.type == "Insertprev")
                {

                    string strgrp = "	select * from exm_group_master where medium_id='" + gm.medium_id + "' and class_id='" + gm.class_id + "' and ayid='" + gm.prevAyid + "' and del_flag=0";
                    DataTable dtgrp = cls.filldt(strgrp);
                    if (dtgrp.Rows.Count > 0)
                    {

                        for (int i = 0; i < dtgrp.Rows.Count; i++)
                        {
                            strquery = strquery + "Exec Insert_Update_group_master '" + gm.medium_id + "','" + gm.class_id + "','" + dtgrp.Rows[i]["group_id"].ToString() + "',N'" + dtgrp.Rows[i]["group_name"].ToString() + "','" + dtgrp.Rows[i]["subject_id"].ToString() + "','" + gm.AYID + "','" + gm.username + "','Insert'";
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
                        return this.Request.CreateResponse(HttpStatusCode.OK, "NoGroup", "application/json");
                    }

                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "No Changes Made", "application/json");
                }
            }
            catch(Exception ex)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, ex.Message, "application/json");
            }
        }
        
    }
}
