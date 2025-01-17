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
    public class category_masterController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();

        [HttpPost]
        public HttpResponseMessage category([FromBody] category cs)
        {
            if (cs.type1 == "modsubcast")
            {

                DataSet ds1 = new DataSet();
                string q1 = "select category_id ,category_name,(case when category_id in (select category_id from mst_cast_tbl  where del_flag=0)  then 0 else 1 end )as flag from mst_category_tbl where del_flag=0; ";
                q1+= " select  cast_id,cast_name,(case when cast_id in(select cast_id from mst_subcast_tbl where del_flag=0)then 0 else 1 end)as flag from mst_cast_tbl where del_flag=0; ";
                q1+= " select subcast_id,subcast_name,(case when subcast_name in (select sub_caste from adm_student_master where del_flag=0)then 0 else 1 end)as flag from mst_subcast_tbl where del_flag=0; ";
                q1+= " select category,caste,sub_caste from adm_student_master; ";
                ds1 = cls.fillds(q1);                
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (cs.type1 == "inserting")
            {
                string query = "insert into mst_category_tbl (category_name,del_flag,curr_dt,mod_dt)values('"+cs.cat+"',0,GETDATE(),GETDATE())";
                cls.exeIUD(query);
                return this.Request.CreateResponse(HttpStatusCode.OK, "Inserted Succesfully", "application/json");
            }
            else if (cs.type1 == "inserting1")
            {
               string query = "insert into mst_cast_tbl (category_id,cast_name,curr_dt,mod_dt,del_flag)values('"+cs.delet+"','" + cs.cat + "',GETDATE(),GETDATE(),0)";
               cls.exeIUD(query);
               return this.Request.CreateResponse(HttpStatusCode.OK, "Inserted Succesfully", "application/json");
            }
            else if (cs.type1 == "inserting2")
            {
                string query = "insert into mst_subcast_tbl (cast_id,subcast_name,curr_dt,mod_dt,del_flag)values('" + cs.delet + "','" + cs.cat + "',GETDATE(),GETDATE(),0)";
                cls.exeIUD(query);
                return this.Request.CreateResponse(HttpStatusCode.OK, "Inserted Succesfully", "application/json");
            }
            else if (cs.type1 == "deleting")
            {
                string query = "update mst_category_tbl set del_flag=1 where category_name='" + cs.delet + "'";
                cls.exeIUD(query);
                return this.Request.CreateResponse(HttpStatusCode.OK, "Deleted Successfully", "application/json");
            }
            else if (cs.type1 == "updating")
            {
                string query = "update mst_category_tbl set category_name='" + cs.cat + "' where category_id='" + cs.delet + "'";
                cls.exeIUD(query);
                return this.Request.CreateResponse(HttpStatusCode.OK, "Updated Successfully", "application/json");
            }
            else if (cs.type1 == "deleting2")
            {
                string query = "update mst_cast_tbl set del_flag=1 where cast_id='" + cs.delet + "'";
                cls.exeIUD(query);
                return this.Request.CreateResponse(HttpStatusCode.OK, "Deleted Successfully", "application/json");
            }
            else if (cs.type1 == "deleting3")
            {
                string query = "update mst_subcast_tbl set del_flag=1 where subcast_id='" + cs.delet + "'";
                cls.exeIUD(query);
                return this.Request.CreateResponse(HttpStatusCode.OK, "Deleted Successfully", "application/json");
            }
            else if (cs.type1 == "updating1")
            {
                string query = "update mst_cast_tbl set cast_name='" + cs.cat + "' where cast_id='" + cs.delet + "'";
                cls.exeIUD(query);
                return this.Request.CreateResponse(HttpStatusCode.OK, "Updated Successfully", "application/json");
            }
            else if (cs.type1 == "updating2")
            {
                string query = "update mst_subcast_tbl set subcast_name='" + cs.cat + "' where subcast_id='" + cs.delet + "'";
                cls.exeIUD(query);
                return this.Request.CreateResponse(HttpStatusCode.OK, "Updated Successfully", "application/json");
            }
            else if (cs.type1 == "checkdata")
            {
                DataSet ds1 = new DataSet();
                string query = "select category from adm_student_master where del_flag=0; ";
                query += " select category_id from mst_cast_tbl where del_flag=0; ";
                query += " select * from mst_subcast_tbl where del_flag=0; ";
                ds1 = cls.fillds(query);

                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "No Changes Made", "application/json");
            }
        }
        
      
    }
}
