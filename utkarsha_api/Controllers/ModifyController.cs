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
    public class ModifyController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        ModifyData md = new ModifyData();
        Class1 cls = new Class1();
        [HttpPost]
        public HttpResponseMessage Modify([FromBody] ModifyData md)
        {

            string type = md.type;
            string[] col2 = md.col2;
            if (md.type == "div")
            {
                string selectquery = "select distinct division_id,division_name from mst_division_tbl where  medium_id='" + md.medium_id + "' and class_id='" + md.class_id + "' and AYID='"+md.ayid +"' ";
                DataTable dt = md.Filldt(selectquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, dt, "application/json");
            }

            else if (md.type == "gdvall")
            {
                //string select1query = "select distinct  a.Student_id,b.std_name,a.stud_F_name,a.stud_m_name,a.stud_L_name,a.stud_mo_name,a.address,a.phone_no1,a.phone_no2,a.dob,a.birth_place,a.mother_tongue,a.nationality,a.category,a.caste,a.sub_caste,a.co_mobile_no,a.adhar_no,a.pincode,a.dist,a.Taluka,a.state,a.residence_no  from adm_student_master as a,mst_standard_tbl as b  where medium_id='" + md.medium_id + "' and class_id='" + md.class_id + "'";
                string select1query = "select * from V_ModifyData where medium_id='" + md.medium_id + "' and  class_id='" + md.class_id + "' and division_id='"+md.div+"' and AYID='"+md.ayid +"'";
                select1query = select1query + "select category_id,category_name from  mst_category_tbl  where del_flag=0";
                select1query = select1query + "select  category_id,cast_id,cast_name from mst_cast_tbl where del_flag=0";
                select1query = select1query + "select cast_id, subcast_id,subcast_name from mst_subcast_tbl where del_flag=0";
                DataSet ds =new DataSet();
                ds = cls.fillds(select1query);
                //DataSet ds = md.Filldt(select1query);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (md.type == "save")
            {
                
              
           
                

                //string str = "";
                DataTable dsdata = JsonConvert.DeserializeObject<DataTable>(md.data);
                for (int i = 0; i < dsdata.Rows.Count; i++)
                {
                    SqlCommand cmd = new SqlCommand("INSERT_UPDATE_STUDENT_MASTER", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@form_id", "");
                    cmd.Parameters.AddWithValue("@medium_id", "");
                    cmd.Parameters.AddWithValue("@Student_id", dsdata.Rows[i]["Student_id"].ToString());
                    cmd.Parameters.AddWithValue("@class_id", "");
                    cmd.Parameters.AddWithValue("@new_admission", "");
                    cmd.Parameters.AddWithValue("@date_of_admission", "");
                    cmd.Parameters.AddWithValue("@stud_F_name", dsdata.Rows[i]["stud_F_name"].ToString());
                    cmd.Parameters.AddWithValue("@stud_m_name", dsdata.Rows[i]["stud_m_name"].ToString());
                    cmd.Parameters.AddWithValue("@stud_L_name", dsdata.Rows[i]["stud_L_name"].ToString());
                    cmd.Parameters.AddWithValue("@stud_mo_name", dsdata.Rows[i]["stud_mo_name"].ToString());
                    cmd.Parameters.AddWithValue("@gender", dsdata.Rows[i]["gender"].ToString());
                    cmd.Parameters.AddWithValue("@address", dsdata.Rows[i]["address"].ToString());
                    cmd.Parameters.AddWithValue("@phone_no1", dsdata.Rows[i]["phone_no1"].ToString());
                    cmd.Parameters.AddWithValue("@phone_no2", dsdata.Rows[i]["phone_no2"].ToString());
                    cmd.Parameters.AddWithValue("@dob", "");
                    cmd.Parameters.AddWithValue("@co_mobile_no", dsdata.Rows[i]["co_mobile_no"].ToString());
                    cmd.Parameters.AddWithValue("@birth_place", dsdata.Rows[i]["birth_place"].ToString());
                    cmd.Parameters.AddWithValue("@mother_tongue", dsdata.Rows[i]["mother_tongue"].ToString());
                    cmd.Parameters.AddWithValue("@nationality", dsdata.Rows[i]["nationality"].ToString());
                    cmd.Parameters.AddWithValue("@last_school_name", "");
                    cmd.Parameters.AddWithValue("@last_studied_std", "");
                    cmd.Parameters.AddWithValue("@percentage", "");
                    cmd.Parameters.AddWithValue("@grade", "");
                    cmd.Parameters.AddWithValue("@category", dsdata.Rows[i]["category"].ToString());
                    cmd.Parameters.AddWithValue("@caste", dsdata.Rows[i]["caste"].ToString());
                    SqlParameter imageParameter = new SqlParameter("@student_photo", SqlDbType.Image);
                    imageParameter.Value = DBNull.Value;
                    cmd.Parameters.Add(imageParameter);


                    SqlParameter imageParameter1 = new SqlParameter("@student_sign", SqlDbType.Image);
                    imageParameter1.Value = DBNull.Value;
                    cmd.Parameters.Add(imageParameter1);
                    cmd.Parameters.AddWithValue("@sub_caste", dsdata.Rows[i]["sub_caste"].ToString());
                    cmd.Parameters.AddWithValue("@AYID", "");
                    cmd.Parameters.AddWithValue("@username", "");
                    cmd.Parameters.AddWithValue("@aadhar_no", dsdata.Rows[i]["aadhar_no"].ToString());
                    cmd.Parameters.AddWithValue("@vehicle_no", dsdata.Rows[i]["vehicle_no"].ToString());
                    cmd.Parameters.AddWithValue("@vehicle_type", dsdata.Rows[i]["vehicle_type"].ToString());
                    cmd.Parameters.AddWithValue("@driver_no", dsdata.Rows[i]["driver_no"].ToString());
                    cmd.Parameters.AddWithValue("@saral_id", dsdata.Rows[i]["saral_id"].ToString());
                    cmd.Parameters.AddWithValue("@bank_ac_no", dsdata.Rows[i]["bank_ac_no"].ToString());
                    cmd.Parameters.AddWithValue("@bank_name", dsdata.Rows[i]["bank_name"].ToString());
                    cmd.Parameters.AddWithValue("@IFSC_code", dsdata.Rows[i]["IFSC_code"].ToString());
                    cmd.Parameters.AddWithValue("@Branch_name", dsdata.Rows[i]["Branch_name"].ToString());
                    cmd.Parameters.AddWithValue("@cancel_remark", "");
                    cmd.Parameters.AddWithValue("@pincode", dsdata.Rows[i]["pincode"].ToString());
                    cmd.Parameters.AddWithValue("@dist", dsdata.Rows[i]["dist"].ToString());
                    cmd.Parameters.AddWithValue("@Taluka", dsdata.Rows[i]["Taluka"].ToString());
                    cmd.Parameters.AddWithValue("@state", dsdata.Rows[i]["state"].ToString());
                  
                    cmd.Parameters.AddWithValue("@StatementType","Update_modify_data");
                    
                   
               
                
                 
                    //cmd.Parameters.AddWithValue("@student_photo", DBNull.Value);
                    //cmd.Parameters.AddWithValue("@student_sign", DBNull.Value);
                   
                  
                   
                   
                   
        



                  




                    //string str = "exec INSERT_UPDATE_STUDENT_MASTER  @StatementType = 'Update_modify_data',@Student_id='" + dsdata.Rows[i]["Student_id"].ToString() + "',@stud_F_name='" + dsdata.Rows[i]["stud_F_name"].ToString() + "',@stud_m_name='" + dsdata.Rows[i]["stud_m_name"].ToString() + "',@stud_L_name='" + dsdata.Rows[i]["stud_L_name"].ToString() + "',@stud_mo_name='" + dsdata.Rows[i]["stud_mo_name"].ToString() + "',@gender='" + dsdata.Rows[i]["gender"].ToString() + "',@address='" + dsdata.Rows[i]["address"].ToString() + "',";
                    //str=str +" @phone_no1='" + dsdata.Rows[i]["phone_no1"].ToString() + "',@phone_no2='" + dsdata.Rows[i]["phone_no2"].ToString() + "',@co_mobile_no='" + dsdata.Rows[i]["co_mobile_no"].ToString() + "',@birth_place='" + dsdata.Rows[i]["birth_place"].ToString() + "',@mother_tongue='" + dsdata.Rows[i]["mother_tongue"].ToString() + "',@nationality='" + dsdata.Rows[i]["nationality"].ToString() + "',@category='" + dsdata.Rows[i]["category"].ToString() + "',";
                    //str = str + "@caste='" + dsdata.Rows[i]["caste"].ToString() + "',@sub_caste='" + dsdata.Rows[i]["sub_caste"].ToString() + "',@aadhar_no='" + dsdata.Rows[i]["aadhar_no"].ToString() + "',@pincode='" + dsdata.Rows[i]["pincode"].ToString() + "',@dist='" + dsdata.Rows[i]["dist"].ToString() + "',@Taluka='" + dsdata.Rows[i]["Taluka"].ToString() + "',@state='" + dsdata.Rows[i]["state"].ToString() + "',@vehicle_no='" + dsdata.Rows[i]["vehicle_no"].ToString() + "',";
                    //str=str + "@vehicle_type='" + dsdata.Rows[i]["vehicle_type"].ToString() + "',@driver_no='" + dsdata.Rows[i]["driver_no"].ToString() + "',@saral_id='" + dsdata.Rows[i]["saral_id"].ToString() + "',@bank_ac_no='" + dsdata.Rows[i]["bank_ac_no"].ToString() + "',@bank_name='" + dsdata.Rows[i]["bank_name"].ToString() + "',@IFSC_code='" + dsdata.Rows[i]["IFSC_code"].ToString() + "',@Branch_name='" + dsdata.Rows[i]["Branch_name"].ToString() +"'";
                    ////str = str + "@form_id= null,@medium_id=null,@new_admission=null,@date_of_admission=null,@dob=null,@last_school_name=null,@last_studied_std=null,@percentage=null,@grade=null,@AYID=null,@username=null,@cancel_remark=null,@class_id=null  ";




                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                }


                
                return this.Request.CreateResponse(HttpStatusCode.OK, "Save", "application/json");
            }
            
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "error", "application/json");
            }
        }
    }
}
