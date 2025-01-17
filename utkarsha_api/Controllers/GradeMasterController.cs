using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using utkarsha_api.App_Start;
using Newtonsoft.Json;

namespace utkarsha_api.Controllers
{


    public class GradeMasterController : ApiController
    {
        grd_master gm = new grd_master();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        string strquery;




        [HttpPost]
        public HttpResponseMessage GradeMaster([FromBody] grd_master gm)
        {
            if (gm.type == "Insert")
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(gm.table);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string min = dt.Rows[i]["min"].ToString();
                    string max = dt.Rows[i]["max"].ToString();
                    string grade_id = dt.Rows[i]["grade_id"].ToString();
                    string grade = dt.Rows[i]["grade"].ToString();
                    string remark = dt.Rows[i]["remark"].ToString();
                    string action = dt.Rows[i]["action"].ToString();
                    if (action == "insert")
                    {

                        strquery = strquery + " Exec Insert_Update_grade_master '" + gm.medium_id + "','" + gm.class_id + "','" + min + "','" + max + "','','" + grade + "','" + remark + "','" + gm.ayid + "','" + gm.username + "','Insert'; ";
                    }
                    else
                    {
                        strquery = strquery + " Exec Insert_Update_grade_master '" + gm.medium_id + "','" + gm.class_id + "','" + min + "','" + max + "','" + grade_id + "','" + grade + "','" + remark + "','" + gm.ayid + "','" + gm.username + "','Update'; ";
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
            else if (gm.type == "copyclsinsert")
            {
                string strchk = "select std_name from mst_standard_tbl where std_id in (select distinct class_id from exm_grade_master where medium_id='" + gm.medium_id + "' and class_id in ('" + gm.copy_class_id.Replace(",", "','") + "') and del_flag=0 and ayid='"+gm.ayid+"')";
                DataTable dtchk = cls.filldt(strchk);
                if (dtchk.Rows.Count == 0)
                {
                    string[] arr = gm.copy_class_id.Split(',');
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(gm.table);
                    for (int j = 0; j < arr.Length; j++)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string min = dt.Rows[i]["min"].ToString();
                            string max = dt.Rows[i]["max"].ToString();
                            //string grade_id = dt.Rows[i]["grade_id"].ToString();
                            string grade = dt.Rows[i]["grade"].ToString();
                            string remark = dt.Rows[i]["remark"].ToString();
                            string action = dt.Rows[i]["action"].ToString();
                            if (action == "insert")
                            {

                                strquery = strquery + " Exec Insert_Update_grade_master '" + gm.medium_id + "','" + arr[j] + "','" + min + "','" + max + "','','" + grade + "','" + remark + "','" + gm.ayid + "','" + gm.username + "','Insert'; ";
                            }

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
                    string classnames = "";
                    for (int i = 0; i < dtchk.Rows.Count; i++)
                    {
                        if (classnames == "")
                        {
                            classnames =  dtchk.Rows[i]["std_name"].ToString();
                        }
                        else
                        {
                            classnames = classnames + "," + dtchk.Rows[i]["std_name"].ToString();
                        }
                    }
                    return this.Request.CreateResponse(HttpStatusCode.OK, "Grade Already Exist in Class " + classnames + " ", "application/json");
                }




            }
            else if (gm.type == "fillgrd")
            {
                strquery = "select min,max,grade_id,grade,remark,'2' as flag from exm_grade_master where ayid='" + gm.ayid + "' and medium_id='" + gm.medium_id + "' and class_id='" + gm.class_id + "' and del_flag=0 order by min desc";
                ds = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (gm.type == "fillfromclass")
            {
                strquery = "select std_id,std_name from mst_standard_tbl where std_id in (select distinct class_id from exm_grade_master where medium_id='" + gm.medium_id + "' and ayid='" + gm.ayid + "' and del_flag=0 ) and del_flag=0 order by rank";
                ds = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (gm.type == "Remove")
            {
                strquery = " Exec Insert_Update_grade_master '" + gm.medium_id + "','" + gm.class_id + "','','','" + gm.grade_id + "','','','" + gm.ayid + "','','Delgrade'; ";
                bool msg = cls.exeIUD(strquery);
                if (msg == true)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "Deleted", "application/json");
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
