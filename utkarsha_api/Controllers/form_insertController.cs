using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.IO;
using Newtonsoft.Json;

namespace utkarsha_api.Controllers
{
    public class form_insertController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Form_insert fi = new Form_insert();
        Class1 cls = new Class1();
        
      

        [HttpPost]
        public HttpResponseMessage Form1([FromBody]Form_insert fi)
        {

            string type = fi.type;
            if (fi.type == "save")
            {
                
                    SqlCommand cmd = new SqlCommand("insert_update_Module", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Module_name", fi.module_name);
                    cmd.Parameters.AddWithValue("@col1", "");
                    cmd.Parameters.AddWithValue("@col2", "");
                    cmd.Parameters.AddWithValue("@col3", "");
                    cmd.Parameters.AddWithValue("@StatementType", "insert");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
               
      
                return this.Request.CreateResponse(HttpStatusCode.OK, "Save", "application/json");
            }
            else if (fi.type == "gdvall")
            {

                string select1query = "select * from Module_form where del_flag=0";
                DataSet ds = new DataSet();
                ds = cls.fillds(select1query);
               
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (fi.type == "delete")
            {
                string a = "No";
                string strquery = "select * from Register_Form where Portal='"+fi.module_name+"' ";
                 DataSet ds1 = cls.fillds(strquery);
                if (ds1.Tables[0].Rows.Count == 0)
                {
                    string select1query = "Update Module_form set del_flag=1 where sr_no='" + fi.sr_no + "' and del_flag=0";
                    cls.exeIUD(select1query);
                    a = "Yes";
                }
                return this.Request.CreateResponse(HttpStatusCode.OK, a.ToString(), "application/json");

            }
            else if (fi.type == "update")
            {

                
                string updatequery = "update Module_form set Module_name='"+fi.module_name+"' where sr_no='" + fi.sr_no + "' and del_flag=0";
                DataSet ds = new DataSet();
                ds = cls.fillds(updatequery);

                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
          
            else if (fi.type == "gdform")
            {

                string select1query = "select * from Register_Form where [Del Flag]=0";
                DataSet ds = new DataSet();
                ds = cls.fillds(select1query);

                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (fi.type == "save_form")
            {
               
                SqlCommand cmd1 = new SqlCommand("insert_Register_Form", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@Form_Name", fi.Form_Name);
                cmd1.Parameters.AddWithValue("@Portal", fi.module_name);
                cmd1.Parameters.AddWithValue("@col1", "");
                cmd1.Parameters.AddWithValue("@col2", "");
                cmd1.Parameters.AddWithValue("@col3", "");
                cmd1.Parameters.AddWithValue("@StatementType", "insert");

                SqlDataAdapter adapter1 = new SqlDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                adapter1.Fill(dt1);
                return this.Request.CreateResponse(HttpStatusCode.OK, "Save", "application/json");
            }
            else if (fi.type == "Delete_Form")
            {
                string a = "No";
                string str = "select * from  Register_Form where Sr_no='" + fi.sr_no + "' and [Del Flag]=0 ";
                DataSet ds_new = cls.fillds(str);
                if (ds_new.Tables[0].Rows.Count== 0)
                {
              
                string select1query = "Update Register_Form set [Del Flag]=1 where Sr_no='" + fi.sr_no + "' and [Del Flag]=0";
                DataSet ds = new DataSet();
                ds = cls.fillds(select1query);
                 a = "yes";
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, a.ToString(), "application/json");
            }
            else if (fi.type == "select_form")
            {

               
                string select1query = "select * from Module_form where Module_name='"+ fi.module_name+"' and  del_flag=0 ";
                DataSet ds = new DataSet();
                ds = cls.fillds(select1query);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    return this.Request.CreateResponse(HttpStatusCode.OK, "Data Found", "application/json");
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "No data found", "application/json");
                }
            }
            else if (fi.type == "update_form")
            {

                
                string updatequery = "update Register_Form set Form_Name='" + fi.Form_Name + "' where Sr_no='" + fi.sr_no + "' and [Del Flag]=0";
                DataSet ds = new DataSet();
                ds = cls.fillds(updatequery);

                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "error", "application/json");
            }
            
           
        }
        
    }
}
