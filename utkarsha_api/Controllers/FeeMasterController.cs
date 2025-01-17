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
    public class FeeMasterController : ApiController
    {
        DataSet ds1 = new DataSet();
        string strquery = "";
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();

        [HttpPost]
        public HttpResponseMessage loadfee([FromBody] FeeMaster fm)
        {
            if (fm.type == "caste")
            {
                strquery = "select distinct category_id,category_name from mst_category_tbl where del_flag=0";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (fm.type == "load")
            {
                strquery = "select m.admflg,m.struct_id,m.struct_name,m.Amount,cast(m.Rank as varchar) as Rank,case when e.struct_id is null then '0' else '1' end as Flag from mst_fee_master as m left join (select distinct struct_id from m_feeentry where ayid='" + fm.ayid + "' and del_flag=0) e on m.struct_id=e.struct_id where m.AYID='" + fm.ayid + "' and m.med_id='" + fm.medium_id + "' and m.class_id='" + fm.class_id + "' and m.caste='" + fm.caste + "' and m.type_name='" + fm.dutype + "' and type_description='" + fm.duration + "' and m.del_flag=0 order by m.Rank";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (fm.type == "loadtype")
            {
                if (fm.ayid == null && fm.medium_id == null && fm.class_id == null)
                {
                    strquery = "select * from mst_fee_type where del_flag=0";
                }
                else
                {
                    strquery = "select distinct d.type,t.value from mst_fee_duration as d,mst_fee_type as t where d.type=t.type and d.ayid='"+fm.ayid+"' and d.med_id='"+fm.medium_id+"' and d.class_id='"+fm.class_id+"' and d.del_flag=0 and t.del_flag=0";
                }
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (fm.type == "duration")
            {
                strquery = "select distinct type from mst_fee_duration where ayid='" + fm.ayid + "' and med_id='" + fm.medium_id + "' and class_id='" + fm.class_id + "' and del_flag=0";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (fm.type == "defined")
            {
                strquery = "select * from mst_fee_duration where ayid='"+fm.ayid+"' and med_id='"+fm.medium_id+"' and class_id='"+fm.class_id+"' and type='"+fm.dutype+"' and del_flag=0 order by duration_id,duration";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (fm.type == "save")
            {
                strquery = "";
                DataTable dsdata = JsonConvert.DeserializeObject<DataTable>(fm.table);
                if (dsdata.Rows.Count > 0)
                {
                    for (int i = 0; i < dsdata.Rows.Count; i++)
                    {
                        if (dsdata.Rows[i]["struct_id"].ToString() == "")
                        {
                            strquery = strquery + "Exec insert_update_structure '" + fm.ayid + "','" + fm.medium_id + "','" + fm.class_id + "',null,N'" + dsdata.Rows[i]["struct_name"].ToString() + "','" + fm.duration_id + "','" + dsdata.Rows[i]["Amount"].ToString() + "','" + fm.caste + "','" + fm.dutype + "','" + fm.duration + "','" + dsdata.Rows[i]["Rank"].ToString() + "','"+ dsdata.Rows[i]["admflg"].ToString() + "','" + fm.user + "','insert' ;";
                        }
                        else
                        {
                            strquery = strquery + "Exec insert_update_structure '" + fm.ayid + "','" + fm.medium_id + "','" + fm.class_id + "','" + dsdata.Rows[i]["struct_id"].ToString() + "',N'" + dsdata.Rows[i]["struct_name"].ToString() + "','" + fm.duration_id + "','" + dsdata.Rows[i]["Amount"].ToString() + "','" + fm.caste + "','" + fm.dutype + "','" + fm.duration + "','" + dsdata.Rows[i]["Rank"].ToString() + "','"+ dsdata.Rows[i]["admflg"].ToString() + "','" + fm.user + "','update' ;";
                        }

                    }
                    cls.exeIUD(strquery);
                    return this.Request.CreateResponse(HttpStatusCode.OK, "success", "application/json");
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "No Changes Made", "application/json");
                }
            }
            else if (fm.type == "saveprevious")
            {
                strquery = "";
                DataTable dsdata = JsonConvert.DeserializeObject<DataTable>(fm.table);
                DataSet dt = cls.fillds("select * from mst_fee_duration where duration_id in (" + fm.duration_id + ") and ayid='"+fm.ayid+"' and med_id='"+fm.medium_id+"' and class_id='"+fm.class_id+"' and del_flag=0");

                if (dsdata.Rows.Count > 0)
                {
                    for (int j = 0; j < dt.Tables[0].Rows.Count; j++)
                    {
                        for (int i = 0; i < dsdata.Rows.Count; i++)
                        {
                            if (dsdata.Rows[i]["struct_id"].ToString() == "")
                            {
                                strquery = strquery + "Exec insert_update_structure '" + fm.ayid + "','" + fm.medium_id + "','" + fm.class_id + "',null,N'" + dsdata.Rows[i]["struct_name"].ToString() + "','" + dt.Tables[0].Rows[j]["duration_id"].ToString() + "','" + dsdata.Rows[i]["Amount"].ToString() + "','" + fm.caste + "','" + fm.dutype + "','" + dt.Tables[0].Rows[j]["duration"].ToString() + "','" + dsdata.Rows[i]["Rank"].ToString() + "','" + fm.user + "','insert' ;";
                            }
                            else
                            {
                                strquery = strquery + "Exec insert_update_structure '" + fm.ayid + "','" + fm.medium_id + "','" + fm.class_id + "','" + dsdata.Rows[i]["struct_id"].ToString() + "',N'" + dsdata.Rows[i]["struct_name"].ToString() + "','" + dt.Tables[0].Rows[j]["duration_id"].ToString() + "','" + dsdata.Rows[i]["Amount"].ToString() + "','" + fm.caste + "','" + fm.dutype + "','" + dt.Tables[0].Rows[j]["duration"].ToString() + "','" + dsdata.Rows[i]["Rank"].ToString() + "','" + fm.user + "','update' ;";
                            }

                        }
                    }
                    cls.exeIUD(strquery);
                    return this.Request.CreateResponse(HttpStatusCode.OK, "success", "application/json");
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "No Changes Made", "application/json");
                }
            }
            else if (fm.type == "savepreviouscastewise")
            {
                //string[] arr = new string[0];
                
                string caste = fm.caste.ToString();
                string[] arr = caste.ToString().Split(',');
                //if (fm.caste.ToString().Contains(",") == true)
                //{
                //    arr = caste.ToString().Split(',');
                //}
                //else
                //{
                //    arr[0] = caste;
                //}
                strquery = "";
                DataTable dsdata = JsonConvert.DeserializeObject<DataTable>(fm.table);
                DataSet dt = cls.fillds("select * from mst_fee_master where caste in (" + caste.ToString() + ") and ayid='" + fm.ayid + "' and med_id='" + fm.medium_id + "' and class_id='" + fm.class_id + "' and duration_id='"+fm.duration_id+"' and del_flag=0");

                if (dt.Tables[0].Rows.Count == 0)
                {
                    if (dsdata.Rows.Count > 0)
                    {
                        for (int j = 0; j < arr.Length; j++)
                        {
                            for (int i = 0; i < dsdata.Rows.Count; i++)
                            {
                                if (dsdata.Rows[i]["struct_id"].ToString() == "")
                                {
                                    strquery = strquery + "Exec insert_update_structure '" + fm.ayid + "','" + fm.medium_id + "','" + fm.class_id + "',null,N'" + dsdata.Rows[i]["struct_name"].ToString() + "','" + fm.duration_id + "','" + dsdata.Rows[i]["Amount"].ToString() + "'," + arr[j].ToString() + ",'" + fm.dutype + "','" + fm.duration + "','" + dsdata.Rows[i]["Rank"].ToString() + "','" + fm.user + "','insert' ;";
                                }
                                else
                                {
                                    strquery = strquery + "Exec insert_update_structure '" + fm.ayid + "','" + fm.medium_id + "','" + fm.class_id + "','" + dsdata.Rows[i]["struct_id"].ToString() + "',N'" + dsdata.Rows[i]["struct_name"].ToString() + "','" + fm.duration_id + "','" + dsdata.Rows[i]["Amount"].ToString() + "'," + arr[j].ToString() + ",'" + fm.dutype + "','" + fm.duration + "','" + dsdata.Rows[i]["Rank"].ToString() + "','" + fm.user + "','update' ;";
                                }

                            }
                        }
                        cls.exeIUD(strquery);
                        return this.Request.CreateResponse(HttpStatusCode.OK, "success", "application/json");
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, "No Changes Made", "application/json");
                    }
                }

                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "No Changes Made", "application/json");
                }
            }
            else if (fm.type == "saveduration")
            {
                strquery = "";
                DataTable dsdata = JsonConvert.DeserializeObject<DataTable>(fm.table);
                if (dsdata.Rows.Count > 0)
                {
                    if (fm.caste == "1")
                    {
                        for (int i = 0; i < dsdata.Rows.Count; i++)
                        {
                            strquery = strquery + "Exec insert_duration null,'" + fm.dutype + "','" + dsdata.Rows[i]["duration"].ToString() + "','" + fm.medium_id + "','" + fm.class_id + "','" + fm.ayid + "','" + fm.user + "','insert' ;";
                        }
                    }
                    if (fm.caste == "3")
                    {
                        strquery = "Exec insert_duration null,'" + fm.dutype + "','" + dsdata.Rows[0]["duration"].ToString() + "','" + fm.medium_id + "','" + fm.class_id + "','" + fm.ayid + "','" + fm.user + "','insert'";
                    }
                    if (fm.caste == "6")
                    {
                        strquery = "Exec insert_duration null,'" + fm.dutype + "','" + dsdata.Rows[0]["duration"].ToString() + "','" + fm.medium_id + "','" + fm.class_id + "','" + fm.ayid + "','" + fm.user + "','insert'";
                    }
                    if (fm.caste == "12")
                    {
                        strquery = "Exec insert_duration null,'" + fm.dutype + "','" + dsdata.Rows[0]["duration"].ToString() + "','" + fm.medium_id + "','" + fm.class_id + "','" + fm.ayid + "','" + fm.user + "','insert'";
                    }
                    if (cls.exeIUD(strquery) == true)
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, "success", "application/json");
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, "No Changes Made", "application/json");
                    }
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "No Changes Made", "application/json");
                }
            }
            else if (fm.type == "gridload")
            {
                strquery = "select duration,d.duration_id,case when m.duration_id is null then '0' else '1' end as Flag from mst_fee_duration as d left join (select distinct duration_id from mst_fee_master where ayid='" + fm.ayid + "' and del_flag=0 and class_id='" + fm.class_id + "' and med_id='" + fm.medium_id + "' and type_name='" + fm.dutype + "') m on d.duration_id=m.duration_id  where type='" + fm.dutype + "' and d.med_id='" + fm.medium_id + "' and d.class_id='" + fm.class_id + "' and d.ayid='" + fm.ayid + "' and d.del_flag=0 order by d.duration_id,d.duration ";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (fm.type == "checkexist")
            {
                if (fm.caste == "1")
                {
                    strquery = "";
                    int count = 0;
                    DataTable dsdata = JsonConvert.DeserializeObject<DataTable>(fm.table);
                    DataTable dt = new DataTable();
                    dt.Columns.Add("duration");
                    if (dsdata.Rows.Count > 0)
                    {
                        for (int i = 0; i < dsdata.Rows.Count; i++)
                        {
                            strquery = "select * from mst_fee_duration where ayid='" + fm.ayid + "' and med_id='" + fm.medium_id + "' and class_id='" + fm.class_id + "' and type='" + fm.dutype + "' and duration like '%" + dsdata.Rows[i]["duration"] + "%' and del_flag=0";
                            ds1 = cls.fillds(strquery);
                            if (ds1.Tables[0].Rows.Count != 0)
                            {
                                dt.Rows.Add("");
                                dt.Rows[count]["duration"] = dsdata.Rows[i]["duration"].ToString();
                                count++;
                            }
                        }
                    }
                    return this.Request.CreateResponse(HttpStatusCode.OK, dt, "application/json");
                }
                if (fm.caste == "3")
                {
                    string[] str = fm.table.ToString().Split(',');
                    strquery = "select * from mst_fee_duration where (duration like '%" + str[0].ToString() + "%' or duration like '%" + str[1].ToString() + "%' or duration like '%" + str[2].ToString() + "%') and ayid='" + fm.ayid + "' and type='" + fm.dutype + "' and med_id='" + fm.medium_id + "' and class_id='" + fm.class_id + "' and del_flag=0";
                    ds1 = cls.fillds(strquery);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, "found", "application/json");
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, "nodata", "application/json");
                    }
                }
                if (fm.caste == "6")
                {
                    strquery = "select * from mst_fee_duration where ayid='" + fm.ayid + "' and med_id='" + fm.medium_id + "' and class_id='" + fm.class_id + "' and type='" + fm.dutype + "' and del_flag=0 ; select * from mst_fee_duration where ayid='" + fm.ayid + "' and med_id='" + fm.medium_id + "' and class_id='" + fm.class_id + "' and type='" + fm.dutype + "' and duration like '%" + fm.table + "%' and del_flag=0";
                    ds1 = cls.fillds(strquery);
                    if (ds1.Tables[0].Rows.Count < 2)
                    {
                        if (ds1.Tables[1].Rows.Count == 0)
                        {
                            return this.Request.CreateResponse(HttpStatusCode.OK, "ok", "application/json");
                        }
                        else
                        {
                            return this.Request.CreateResponse(HttpStatusCode.OK, "found", "application/json");
                        }
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, "exist", "application/json");
                    }
                }
                if (fm.caste == "12")
                {
                    strquery = "select * from mst_fee_duration where ayid='" + fm.ayid + "' and med_id='" + fm.medium_id + "' and class_id='" + fm.class_id + "' and type='" + fm.dutype + "' and del_flag=0";
                    ds1 = cls.fillds(strquery);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, "found", "application/json");
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, "zero", "application/json");
                    }
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "nodata", "application/json");
                }

            }
            else if (fm.type == "deletedur")
            {
                strquery = "update mst_fee_duration set del_flag=1  where duration_id='" + fm.table + "' ";
                if (cls.exeIUD(strquery) == true)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "success", "application/json");
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "fail", "application/json");
                }
            }
            else if (fm.type == "delete")
            {
                strquery = "update mst_fee_master set del_flag=1  where struct_id='" + fm.caste + "'";
                cls.exeIUD(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, "delete", "application/json");
            }
            else if (fm.type == "previous")
            {
                strquery = "select * from mst_fee_duration where duration_id in (select duration_id from mst_fee_master where del_flag=0 and AYID='" + fm.ayid + "' and med_id='" + fm.medium_id + "' and class_id='" + fm.class_id + "' and caste='" + fm.caste + "' and type_name='" + fm.dutype + "') and ayid='" + fm.ayid + "' and med_id='" + fm.medium_id + "' and class_id='" + fm.class_id + "' and type='" + fm.dutype + "' and del_flag=0 order by duration_id,duration ; select * from mst_fee_duration where duration_id not in (select duration_id from mst_fee_master where del_flag=0 and AYID='" + fm.ayid + "' and med_id='" + fm.medium_id + "' and class_id='" + fm.class_id + "' and caste='" + fm.caste + "' and type_name='" + fm.dutype + "') and ayid='" + fm.ayid + "' and med_id='" + fm.medium_id + "' and class_id='" + fm.class_id + "' and type='" + fm.dutype + "' and del_flag=0 order by duration_id,duration; select * from mst_fee_master where AYID='"+fm.ayid+"' and med_id='"+fm.medium_id+"' and class_id='"+fm.class_id+"' and duration_id='"+fm.duration_id+"' and type_name='"+fm.dutype+"' and del_flag=0";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (fm.type == "previouscaste")
            {
                strquery = "select distinct category_id,category_name from mst_category_tbl where category_id in(select caste from mst_fee_master where ayid='" + fm.ayid + "' and med_id='" + fm.medium_id + "' and class_id='" + fm.class_id + "' and duration_id='" + fm.duration_id + "' and type_name='" + fm.dutype + "' and del_flag=0) and del_flag=0 ; select distinct category_id,category_name from mst_category_tbl where category_id not in (select distinct caste from mst_fee_master where  ayid='" + fm.ayid + "' and med_id='" + fm.medium_id + "' and class_id='" + fm.class_id + "' and duration_id='" + fm.duration_id + "' and  type_name='" + fm.dutype + "' and del_flag=0)  and del_flag=0";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "No Changes Made", "application/json");
            }
        }
    }
}
