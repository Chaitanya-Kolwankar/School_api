using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using utkarsha_api.App_Start;
using System.Configuration;
using System.Globalization;

namespace utkarsha_api.Controllers
{
    public class Student_mastController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();
        string strquery;
        string msg;
        DataSet ds1 = new DataSet();

        [HttpPost]
        public HttpResponseMessage stud_mast([FromBody] stud_master sm)
        {
            if (sm.type_of_query == "retrive")
            {
                strquery = "select * from adm_student_master where form_id='" + sm.form_no + "' or student_id='" + sm.student_id + "'";
                ds1 = cls.fillds(strquery);
                ds1.Tables[0].TableName = "load";
            }
            else if (sm.type_of_query == "select")
            {
                strquery = "select Student_id,form_id,del_flag from adm_student_master";
                ds1 = cls.fillds(strquery);
            }
            else if (sm.type_of_query == "catcastsubcast")
            {
                strquery = strquery + " select distinct category_id,category_name from mst_category_tbl where del_flag=0; ";
                strquery = strquery + " select distinct category_id,cast_id,cast_name from mst_cast_tbl where del_flag=0; ";
                strquery = strquery + " select distinct cast_id,subcast_id,subcast_name from mst_subcast_tbl where del_flag=0; ";
                strquery = strquery + " select distinct religion_id,religion from mst_religion_tbl where del_flag=0; ";

                ds1 = cls.fillds(strquery);
            }
            else if (sm.type_of_query == "recover")
            {
                con.Open();
                strquery = "INSERT_UPDATE_STUDENT_MASTER";

                using (SqlCommand cmd = new SqlCommand(strquery, con))
                {
                    string st = "Recover";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@new_admission", DBNull.Value);
                    cmd.Parameters.AddWithValue("@date_of_admission", DBNull.Value);
                    cmd.Parameters.AddWithValue("@stud_F_name", DBNull.Value);
                    cmd.Parameters.AddWithValue("@stud_m_name", DBNull.Value);
                    cmd.Parameters.AddWithValue("@stud_L_name", DBNull.Value);
                    cmd.Parameters.AddWithValue("@stud_mo_name", DBNull.Value);
                    cmd.Parameters.AddWithValue("@address", DBNull.Value);
                    cmd.Parameters.AddWithValue("@phone_no1", DBNull.Value);
                    cmd.Parameters.AddWithValue("@phone_no2", DBNull.Value);
                    cmd.Parameters.AddWithValue("@gender", DBNull.Value);
                    cmd.Parameters.AddWithValue("@dob", DBNull.Value);
                    cmd.Parameters.AddWithValue("@birth_place", DBNull.Value);
                    cmd.Parameters.AddWithValue("@mother_tongue", DBNull.Value);
                    cmd.Parameters.AddWithValue("@nationality", DBNull.Value);
                    cmd.Parameters.AddWithValue("@religion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@last_school_name", DBNull.Value);
                    cmd.Parameters.AddWithValue("@last_studied_std", DBNull.Value);
                    cmd.Parameters.AddWithValue("@percentage", DBNull.Value);
                    cmd.Parameters.AddWithValue("@grade", DBNull.Value);
                    cmd.Parameters.AddWithValue("@category", DBNull.Value);
                    cmd.Parameters.AddWithValue("@caste", DBNull.Value);
                    cmd.Parameters.AddWithValue("@sub_caste", DBNull.Value);
                    cmd.Parameters.AddWithValue("@username", DBNull.Value);
                    cmd.Parameters.AddWithValue("@co_mobile_no", DBNull.Value);
                    cmd.Parameters.AddWithValue("@aadhar_no", DBNull.Value);
                    cmd.Parameters.AddWithValue("@vehicle_type", DBNull.Value);
                    cmd.Parameters.AddWithValue("@vehicle_no", DBNull.Value);
                    cmd.Parameters.AddWithValue("@driver_no", DBNull.Value);
                    cmd.Parameters.AddWithValue("@saral_id", DBNull.Value);
                    cmd.Parameters.AddWithValue("@bank_ac_no", DBNull.Value);
                    cmd.Parameters.AddWithValue("@bank_name", DBNull.Value);
                    cmd.Parameters.AddWithValue("@IFSC_code", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Branch_name", DBNull.Value);
                    cmd.Parameters.AddWithValue("@cancel_remark", DBNull.Value);
                    cmd.Parameters.AddWithValue("@dist", DBNull.Value);
                    cmd.Parameters.AddWithValue("@state", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Taluka", DBNull.Value);
                    cmd.Parameters.AddWithValue("@pincode", DBNull.Value);
                    cmd.Parameters.AddWithValue("@student_photo", DBNull.Value);
                    cmd.Parameters.AddWithValue("@student_sign", DBNull.Value);


                    if (sm.form_no != null)
                    {
                        cmd.Parameters.AddWithValue("@form_id", sm.form_no);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@form_id", DBNull.Value);
                    }

                    if (sm.student_id != null)
                    {
                        cmd.Parameters.AddWithValue("@Student_id", sm.student_id);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Student_id", DBNull.Value);
                    }
                    cmd.Parameters.AddWithValue("@medium_id", DBNull.Value);
                    cmd.Parameters.AddWithValue("@class_id", DBNull.Value);
                    cmd.Parameters.AddWithValue("@AYID", sm.AYID);
                    cmd.Parameters.AddWithValue("@StatementType", st);

                    if (cmd.ExecuteNonQuery() > 0)
                    {

                    }
                    else
                    {

                        sm.get_form_id = "";
                    }
                    con.Close();

                }
                DataTable dt = new DataTable("MyTable");
                dt.Columns.Add(new DataColumn("msg", typeof(string)));
                DataRow dr = dt.NewRow();

                if (sm.get_form_id != "")
                {
                    dr["msg"] = "Student Addmission  Recovered";
                    dt.Rows.Add(dr);
                    ds1.Tables.Add(dt);
                }
                else
                {
                    dr["msg"] = "No FormId";
                    dt.Rows.Add(dr);
                    ds1.Tables.Add(dt);
                }
            }
            else if (sm.type_of_query == "cancel")
            {
                con.Open();
                strquery = "INSERT_UPDATE_STUDENT_MASTER";

                using (SqlCommand cmd = new SqlCommand(strquery, con))
                {
                    string st = "Admission_cancel";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@new_admission", DBNull.Value);
                    cmd.Parameters.AddWithValue("@date_of_admission", DBNull.Value);
                    cmd.Parameters.AddWithValue("@stud_F_name", DBNull.Value);
                    cmd.Parameters.AddWithValue("@stud_m_name", DBNull.Value);
                    cmd.Parameters.AddWithValue("@stud_L_name", DBNull.Value);
                    cmd.Parameters.AddWithValue("@stud_mo_name", DBNull.Value);
                    cmd.Parameters.AddWithValue("@address", DBNull.Value);
                    cmd.Parameters.AddWithValue("@phone_no1", DBNull.Value);
                    cmd.Parameters.AddWithValue("@phone_no2", DBNull.Value);
                    cmd.Parameters.AddWithValue("@gender", DBNull.Value);
                    cmd.Parameters.AddWithValue("@dob", DBNull.Value);
                    cmd.Parameters.AddWithValue("@birth_place", DBNull.Value);
                    cmd.Parameters.AddWithValue("@mother_tongue", DBNull.Value);
                    cmd.Parameters.AddWithValue("@nationality", DBNull.Value);
                    cmd.Parameters.AddWithValue("@religion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@last_school_name", DBNull.Value);
                    cmd.Parameters.AddWithValue("@last_studied_std", DBNull.Value);
                    cmd.Parameters.AddWithValue("@percentage", DBNull.Value);
                    cmd.Parameters.AddWithValue("@grade", DBNull.Value);
                    cmd.Parameters.AddWithValue("@category", DBNull.Value);
                    cmd.Parameters.AddWithValue("@caste", DBNull.Value);
                    cmd.Parameters.AddWithValue("@sub_caste", DBNull.Value);
                    cmd.Parameters.AddWithValue("@username", DBNull.Value);
                    cmd.Parameters.AddWithValue("@co_mobile_no", DBNull.Value);
                    cmd.Parameters.AddWithValue("@aadhar_no", DBNull.Value);
                    cmd.Parameters.AddWithValue("@vehicle_type", DBNull.Value);
                    cmd.Parameters.AddWithValue("@vehicle_no", DBNull.Value);
                    cmd.Parameters.AddWithValue("@driver_no", DBNull.Value);
                    cmd.Parameters.AddWithValue("@saral_id", DBNull.Value);
                    cmd.Parameters.AddWithValue("@bank_ac_no", DBNull.Value);
                    cmd.Parameters.AddWithValue("@bank_name", DBNull.Value);
                    cmd.Parameters.AddWithValue("@IFSC_code", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Branch_name", DBNull.Value);
                    cmd.Parameters.AddWithValue("@dist", DBNull.Value);
                    cmd.Parameters.AddWithValue("@state", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Taluka", DBNull.Value);
                    cmd.Parameters.AddWithValue("@pincode", DBNull.Value);
                    cmd.Parameters.AddWithValue("@student_photo", DBNull.Value);
                    cmd.Parameters.AddWithValue("@student_sign", DBNull.Value);

                    if (sm.cancel_remark != null)
                    {
                        cmd.Parameters.AddWithValue("@cancel_remark", sm.cancel_remark.Trim());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@cancel_remark", DBNull.Value);
                    }

                    if (sm.form_no != null)
                    {
                        cmd.Parameters.AddWithValue("@form_id", sm.form_no);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@form_id", DBNull.Value);
                    }

                    if (sm.student_id != null)
                    {
                        cmd.Parameters.AddWithValue("@Student_id", sm.student_id);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Student_id", DBNull.Value);
                    }

                    cmd.Parameters.AddWithValue("@medium_id", DBNull.Value);
                    cmd.Parameters.AddWithValue("@class_id", DBNull.Value);
                    cmd.Parameters.AddWithValue("@AYID", sm.AYID);
                    cmd.Parameters.AddWithValue("@StatementType", st);
                    if (cmd.ExecuteNonQuery() > 0)
                    {

                    }
                    else
                    {

                        sm.get_form_id = "";
                    }
                    con.Close();

                }
                DataTable dt = new DataTable("MyTable");
                dt.Columns.Add(new DataColumn("msg", typeof(string)));
                DataRow dr = dt.NewRow();

                if (sm.get_form_id != "")
                {
                    dr["msg"] = "Student Admission Cancelled";
                    dt.Rows.Add(dr);
                    ds1.Tables.Add(dt);
                }
                else
                {
                    dr["msg"] = "No FormId";
                    dt.Rows.Add(dr);
                    ds1.Tables.Add(dt);
                }

            }
            else if (sm.type_of_query == "update")
            {
                con.Open();
                strquery = "INSERT_UPDATE_STUDENT_MASTER";

                using (SqlCommand cmd = new SqlCommand(strquery, con))
                {
                    DateTime doa = DateTime.ParseExact(sm.date_of_admission, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime dob = DateTime.ParseExact(sm.DOB, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string st = "Update";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@form_id", sm.form_no);
                    cmd.Parameters.AddWithValue("@Student_id", sm.student_id);
                    cmd.Parameters.AddWithValue("@medium_id", sm.medium);
                    cmd.Parameters.AddWithValue("@class_id", sm.stud_class);
                    cmd.Parameters.AddWithValue("@new_admission", sm.new_admission);
                    cmd.Parameters.AddWithValue("@date_of_admission", doa);
                    cmd.Parameters.AddWithValue("@stud_F_name", sm.name.Trim());
                    cmd.Parameters.AddWithValue("@stud_m_name", sm.father_name.Trim());
                    cmd.Parameters.AddWithValue("@stud_L_name", sm.surname.Trim());
                    cmd.Parameters.AddWithValue("@stud_mo_name", sm.mo_name.Trim());
                    cmd.Parameters.AddWithValue("@address", sm.address.Trim());
                    cmd.Parameters.AddWithValue("@phone_no1", sm.mob_no.Trim());
                    cmd.Parameters.AddWithValue("@gender", sm.gender.Trim());
                    cmd.Parameters.AddWithValue("@dob", dob);
                    cmd.Parameters.AddWithValue("@birth_place", sm.birth_place.Trim());
                    cmd.Parameters.AddWithValue("@mother_tongue", sm.mother_tongue.Trim());
                    cmd.Parameters.AddWithValue("@nationality", sm.nationality.Trim());
                    //cmd.Parameters.AddWithValue("@religion", sm.religion);
                    //cmd.Parameters.AddWithValue("@category", sm.category);
                    //cmd.Parameters.AddWithValue("@caste", sm.caste);
                    cmd.Parameters.AddWithValue("@cancel_remark", DBNull.Value);
                    cmd.Parameters.AddWithValue("@dist", sm.district.Trim());
                    cmd.Parameters.AddWithValue("@state", sm.state.Trim());
                    cmd.Parameters.AddWithValue("@Taluka", sm.taluka.Trim());
                    cmd.Parameters.AddWithValue("@pincode", sm.pincode.Trim());
                    cmd.Parameters.AddWithValue("@AYID", sm.AYID);
                    cmd.Parameters.AddWithValue("@username", sm.username.Trim());
                    cmd.Parameters.AddWithValue("@aadhar_no", sm.aadhar_no.Trim());

                    if (sm.religion == null)
                    {
                        cmd.Parameters.AddWithValue("@religion", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@religion", sm.religion);
                    }

                    if (sm.category == null)
                    {
                        cmd.Parameters.AddWithValue("@category", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@category", sm.category);
                    }

                    if (sm.caste == null)
                    {
                        cmd.Parameters.AddWithValue("@caste", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@caste", sm.caste);
                    }

                    if (sm.student_photo == null)
                    {
                        cmd.Parameters.AddWithValue("@student_photo", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@student_photo", sm.student_photo);
                    }

                    if (sm.student_sign == null)
                    {
                        cmd.Parameters.AddWithValue("@student_sign", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@student_sign", sm.student_sign);
                    }

                    if (sm.subcaste == null)
                    {
                        cmd.Parameters.AddWithValue("@sub_caste", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@sub_caste", sm.subcaste);
                    }

                    if (sm.saral != null)
                    {
                        cmd.Parameters.AddWithValue("@saral_id", sm.saral.Trim());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@saral_id", DBNull.Value);
                    }

                    if (sm.residence_no != null)
                    {
                        cmd.Parameters.AddWithValue("@phone_no2", sm.residence_no.Trim());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@phone_no2", DBNull.Value);
                    }
                    if (sm.co_no != null)
                    {
                        cmd.Parameters.AddWithValue("@co_mobile_no", sm.co_no.Trim());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@co_mobile_no", DBNull.Value);
                    }

                    if (sm.last_schl != null)
                    {
                        cmd.Parameters.AddWithValue("@last_school_name", sm.last_schl.Trim());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@last_school_name", DBNull.Value);
                    }
                    if (sm.last_std != null)
                    {
                        cmd.Parameters.AddWithValue("@last_studied_std", sm.last_std.Trim());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@last_studied_std", DBNull.Value);
                    }

                    if (sm.grade != null)
                    {
                        cmd.Parameters.AddWithValue("@grade", sm.grade.Trim());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@grade", DBNull.Value);
                    }

                    if (sm.per != null)
                    {
                        cmd.Parameters.AddWithValue("@percentage", sm.per.Trim());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@percentage", DBNull.Value);
                    }


                    if (sm.vehical_type != null)
                    {
                        cmd.Parameters.AddWithValue("@vehicle_type", sm.vehical_type.Trim());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@vehicle_type", DBNull.Value);
                    }

                    if (sm.vehical_no != null)
                    {
                        cmd.Parameters.AddWithValue("@vehicle_no", sm.vehical_no.Trim());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@vehicle_no", DBNull.Value);
                    }

                    if (sm.driver_mob != null)
                    {
                        cmd.Parameters.AddWithValue("@driver_no", sm.driver_mob.Trim());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@driver_no", DBNull.Value);
                    }


                    if (sm.b_acc_no != null)
                    {
                        cmd.Parameters.AddWithValue("@bank_ac_no", sm.b_acc_no.Trim());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@bank_ac_no", DBNull.Value);
                    }

                    if (sm.b_name != null)
                    {
                        cmd.Parameters.AddWithValue("@bank_name", sm.b_name.Trim());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@bank_name", DBNull.Value);
                    }

                    if (sm.ifsc_code != null)
                    {
                        cmd.Parameters.AddWithValue("@IFSC_code", sm.ifsc_code.Trim());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@IFSC_code", DBNull.Value);
                    }

                    if (sm.branch_name != null)
                    {
                        cmd.Parameters.AddWithValue("@Branch_name", sm.branch_name.Trim());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Branch_name", DBNull.Value);
                    }

                    cmd.Parameters.AddWithValue("@StatementType", st);

                    if (cmd.ExecuteNonQuery() > 0)
                    {

                    }
                    else
                    {
                        sm.get_form_id = "";
                    }
                    con.Close();

                }
                DataTable dt = new DataTable("MyTable");
                dt.Columns.Add(new DataColumn("msg", typeof(string)));
                DataRow dr = dt.NewRow();

                if (sm.get_form_id != "")
                {
                    dr["msg"] = "Student Data Updated";
                    dt.Rows.Add(dr);
                    ds1.Tables.Add(dt);
                }
                else
                {
                    dr["msg"] = "No FormId";
                    dt.Rows.Add(dr);
                    ds1.Tables.Add(dt);
                }


            }
            else if (sm.type_of_query == "save")
            {
                strquery = "SELECT [dbo].[Generate_Form_id]  ('" + sm.medium_name + "','" + sm.AYID + "')";
                DataSet fid = cls.fillds(strquery);

                if (fid.Tables[0].Rows.Count == 1)
                {

                    sm.get_form_id = fid.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    sm.get_form_id = "No";

                }


                if (sm.get_form_id != "No")
                {

                    strquery = "INSERT_UPDATE_STUDENT_MASTER";

                    using (SqlCommand cmd = new SqlCommand(strquery, con))
                    {
                        DateTime doa = DateTime.ParseExact(sm.date_of_admission, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        DateTime dob = DateTime.ParseExact(sm.DOB, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        string st = "Form_Id";


                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@form_id", sm.get_form_id);
                        cmd.Parameters.AddWithValue("@Student_id", DBNull.Value);
                        cmd.Parameters.AddWithValue("@medium_id", sm.medium);
                        cmd.Parameters.AddWithValue("@class_id", sm.stud_class);
                        cmd.Parameters.AddWithValue("@new_admission", sm.new_admission);
                        cmd.Parameters.AddWithValue("@date_of_admission", doa);
                        cmd.Parameters.AddWithValue("@stud_F_name", sm.name.Trim());
                        cmd.Parameters.AddWithValue("@stud_m_name", sm.father_name.Trim());
                        cmd.Parameters.AddWithValue("@stud_L_name", sm.surname.Trim());
                        cmd.Parameters.AddWithValue("@stud_mo_name", sm.mo_name.Trim());
                        cmd.Parameters.AddWithValue("@address", sm.address.Trim());
                        cmd.Parameters.AddWithValue("@phone_no1", sm.mob_no.Trim());
                        cmd.Parameters.AddWithValue("@gender", sm.gender);
                        cmd.Parameters.AddWithValue("@dob", dob);
                        cmd.Parameters.AddWithValue("@birth_place", sm.birth_place.Trim());
                        cmd.Parameters.AddWithValue("@mother_tongue", sm.mother_tongue.Trim());
                        cmd.Parameters.AddWithValue("@nationality", sm.nationality.Trim());
                        //cmd.Parameters.AddWithValue("@religion", sm.religion);
                        //cmd.Parameters.AddWithValue("@category", sm.category);
                        //cmd.Parameters.AddWithValue("@caste", sm.caste);
                        cmd.Parameters.AddWithValue("@cancel_remark", DBNull.Value);
                        cmd.Parameters.AddWithValue("@dist", sm.district.Trim());
                        cmd.Parameters.AddWithValue("@state", sm.state.Trim());
                        cmd.Parameters.AddWithValue("@Taluka", sm.taluka.Trim());
                        cmd.Parameters.AddWithValue("@pincode", sm.pincode.Trim());
                        cmd.Parameters.AddWithValue("@AYID", sm.AYID);
                        cmd.Parameters.AddWithValue("@username", sm.username.Trim());
                        cmd.Parameters.AddWithValue("@aadhar_no", sm.aadhar_no.Trim());

                        if (sm.religion == null)
                        {
                            cmd.Parameters.AddWithValue("@religion", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@religion", sm.religion);
                        }

                        if (sm.category == null)
                        {
                            cmd.Parameters.AddWithValue("@category", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@category", sm.category);
                        }

                        if (sm.caste == null)
                        {
                            cmd.Parameters.AddWithValue("@caste", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@caste", sm.caste);
                        }


                        if (sm.student_photo == null)
                        {
                            cmd.Parameters.AddWithValue("@student_photo", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@student_photo", sm.student_photo);
                        }

                        if (sm.student_sign == null)
                        {
                            cmd.Parameters.AddWithValue("@student_sign", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@student_sign", sm.student_sign);
                        }

                        if (sm.subcaste == null)
                        {
                            cmd.Parameters.AddWithValue("@sub_caste", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@sub_caste", sm.subcaste);
                        }

                        if (sm.saral != null)
                        {
                            cmd.Parameters.AddWithValue("@saral_id", sm.saral.Trim());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@saral_id", DBNull.Value);
                        }

                        if (sm.residence_no != null)
                        {
                            cmd.Parameters.AddWithValue("@phone_no2", sm.residence_no.Trim());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@phone_no2", DBNull.Value);
                        }
                        if (sm.co_no != null)
                        {
                            cmd.Parameters.AddWithValue("@co_mobile_no", sm.co_no.Trim());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@co_mobile_no", DBNull.Value);
                        }

                        if (sm.last_schl != null)
                        {
                            cmd.Parameters.AddWithValue("@last_school_name", sm.last_schl.Trim());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@last_school_name", DBNull.Value);
                        }
                        if (sm.last_std != null)
                        {
                            cmd.Parameters.AddWithValue("@last_studied_std", sm.last_std.Trim());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@last_studied_std", DBNull.Value);
                        }

                        if (sm.grade != null)
                        {
                            cmd.Parameters.AddWithValue("@grade", sm.grade.Trim());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@grade", DBNull.Value);
                        }

                        if (sm.per != null)
                        {
                            cmd.Parameters.AddWithValue("@percentage", sm.per.Trim());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@percentage", DBNull.Value);
                        }


                        if (sm.vehical_type != null)
                        {
                            cmd.Parameters.AddWithValue("@vehicle_type", sm.vehical_type.Trim());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@vehicle_type", DBNull.Value);
                        }

                        if (sm.vehical_no != null)
                        {
                            cmd.Parameters.AddWithValue("@vehicle_no", sm.vehical_no.Trim());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@vehicle_no", DBNull.Value);
                        }

                        if (sm.driver_mob != null)
                        {
                            cmd.Parameters.AddWithValue("@driver_no", sm.driver_mob.Trim());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@driver_no", DBNull.Value);
                        }


                        if (sm.b_acc_no != null)
                        {
                            cmd.Parameters.AddWithValue("@bank_ac_no", sm.b_acc_no.Trim());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@bank_ac_no", DBNull.Value);
                        }

                        if (sm.b_name != null)
                        {
                            cmd.Parameters.AddWithValue("@bank_name", sm.b_name.Trim());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@bank_name", DBNull.Value);
                        }

                        if (sm.ifsc_code != null)
                        {
                            cmd.Parameters.AddWithValue("@IFSC_code", sm.ifsc_code.Trim());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@IFSC_code", DBNull.Value);
                        }

                        if (sm.branch_name != null)
                        {
                            cmd.Parameters.AddWithValue("@Branch_name", sm.branch_name.Trim());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Branch_name", DBNull.Value);
                        }

                        cmd.Parameters.AddWithValue("@StatementType", st);

                        con.Open();

                        if (cmd.ExecuteNonQuery() > 0)
                        {

                        }
                        else
                        {
                            sm.get_form_id = "";
                        }
                        con.Close();

                    }
                }

                DataTable dt = new DataTable("MyTable");
                dt.Columns.Add(new DataColumn("msg", typeof(string)));
                DataRow dr = dt.NewRow();

                if (sm.get_form_id != "")
                {
                    dr["msg"] = sm.get_form_id;
                    dt.Rows.Add(dr);
                    ds1.Tables.Add(dt);
                }
                else
                {
                    dr["msg"] = "No FormId";
                    dt.Rows.Add(dr);
                    ds1.Tables.Add(dt);
                }
            }
            else if (sm.type_of_query == "stud_id")
            {
                String str = "SELECT [dbo].[Generate_student_id] ('" + sm.form_no + "','" + sm.medium_name + "','" + sm.stud_class + "','" + sm.AYID + "')";
                con.Open();
                SqlDataReader rs;
                rs = cls.RetriveDataBaseQuery(str);
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        sm.student_id = rs[0].ToString().Trim();

                        sm.student_id = rs[0].ToString();
                    }
                }
                else
                {
                    sm.student_id = "No";
                }
                if (sm.student_id != "No")
                {
                    strquery = "INSERT_UPDATE_STUDENT_MASTER";

                    using (SqlCommand cmd = new SqlCommand(strquery, con))
                    {
                        string st = "Student_Id";

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@new_admission", DBNull.Value);
                        cmd.Parameters.AddWithValue("@date_of_admission", DBNull.Value);
                        cmd.Parameters.AddWithValue("@stud_F_name", DBNull.Value);
                        cmd.Parameters.AddWithValue("@stud_m_name", DBNull.Value);
                        cmd.Parameters.AddWithValue("@stud_L_name", DBNull.Value);
                        cmd.Parameters.AddWithValue("@stud_mo_name", DBNull.Value);
                        cmd.Parameters.AddWithValue("@address", DBNull.Value);
                        cmd.Parameters.AddWithValue("@phone_no1", DBNull.Value);
                        cmd.Parameters.AddWithValue("@phone_no2", DBNull.Value);
                        cmd.Parameters.AddWithValue("@gender", DBNull.Value);
                        cmd.Parameters.AddWithValue("@dob", DBNull.Value);
                        cmd.Parameters.AddWithValue("@birth_place", DBNull.Value);
                        cmd.Parameters.AddWithValue("@mother_tongue", DBNull.Value);
                        cmd.Parameters.AddWithValue("@nationality", DBNull.Value);
                        cmd.Parameters.AddWithValue("@religion", DBNull.Value);
                        cmd.Parameters.AddWithValue("@last_school_name", DBNull.Value);
                        cmd.Parameters.AddWithValue("@last_studied_std", DBNull.Value);
                        cmd.Parameters.AddWithValue("@percentage", DBNull.Value);
                        cmd.Parameters.AddWithValue("@grade", DBNull.Value);
                        cmd.Parameters.AddWithValue("@category", DBNull.Value);
                        cmd.Parameters.AddWithValue("@caste", DBNull.Value);
                        cmd.Parameters.AddWithValue("@sub_caste", DBNull.Value);
                        cmd.Parameters.AddWithValue("@username", DBNull.Value);
                        cmd.Parameters.AddWithValue("@co_mobile_no", DBNull.Value);
                        cmd.Parameters.AddWithValue("@aadhar_no", DBNull.Value);
                        cmd.Parameters.AddWithValue("@vehicle_type", DBNull.Value);
                        cmd.Parameters.AddWithValue("@vehicle_no", DBNull.Value);
                        cmd.Parameters.AddWithValue("@driver_no", DBNull.Value);
                        cmd.Parameters.AddWithValue("@saral_id", DBNull.Value);
                        cmd.Parameters.AddWithValue("@bank_ac_no", DBNull.Value);
                        cmd.Parameters.AddWithValue("@bank_name", DBNull.Value);
                        cmd.Parameters.AddWithValue("@IFSC_code", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Branch_name", DBNull.Value);
                        cmd.Parameters.AddWithValue("@cancel_remark", DBNull.Value);
                        cmd.Parameters.AddWithValue("@dist", DBNull.Value);
                        cmd.Parameters.AddWithValue("@state", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Taluka", DBNull.Value);
                        cmd.Parameters.AddWithValue("@pincode", DBNull.Value);
                        cmd.Parameters.AddWithValue("@student_photo", DBNull.Value);
                        cmd.Parameters.AddWithValue("@student_sign", DBNull.Value);
                        cmd.Parameters.AddWithValue("@form_id", sm.form_no);
                        cmd.Parameters.AddWithValue("@Student_id", sm.student_id);
                        cmd.Parameters.AddWithValue("@medium_id", sm.medium);
                        cmd.Parameters.AddWithValue("@class_id", sm.stud_class);
                        cmd.Parameters.AddWithValue("@AYID", sm.AYID);
                        cmd.Parameters.AddWithValue("@StatementType", st);

                        if (cmd.ExecuteNonQuery() > 0)
                        {

                        }
                        else
                        {

                            sm.get_form_id = "";
                        }
                        con.Close();

                    }
                }
                DataTable dt = new DataTable("MyTable");
                dt.Columns.Add(new DataColumn("msg", typeof(string)));
                DataRow dr = dt.NewRow();

                if (sm.student_id != "")
                {
                    dr["msg"] = sm.student_id;
                    dt.Rows.Add(dr);
                    ds1.Tables.Add(dt);
                }
                else
                {
                    dr["msg"] = "No StudentId";
                    dt.Rows.Add(dr);
                    ds1.Tables.Add(dt);
                }
            }
            else if (sm.type_of_query == "updateimagepath")
            {
                string str = "update adm_student_master set student_photo='" + sm.student_photo + "',student_sign='" + sm.student_sign + "' where form_id='" + sm.form_no + "' or student_id='" + sm.student_id + "'";
                ds1 = cls.fillds(str);
                return this.Request.CreateResponse(HttpStatusCode.OK, "Updated Successfully", "application/json");
            }

                //-------------------------------------religion
            else if (sm.type_of_query == "religionmodel")
            {
                strquery = "select religion_id,religion,case when (select count(*) from adm_student_master where religion=r.religion_id and del_flag=0)>0 then '1' else '0' end as flag from mst_religion_tbl r where del_flag=0";
                ds1 = cls.fillds(strquery);
                ds1.Tables[0].TableName = "religion";
            }
            else if (sm.type_of_query == "savereligion")
            {

                DataTable dt = new DataTable("MyTable");
                dt.Columns.Add(new DataColumn("msg", typeof(string)));
                DataRow dr = dt.NewRow();

                string query = "select * from mst_religion_tbl where religion=N'"+sm.category+"' and del_flag=0";
                ds1 = cls.fillds(query);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    dr["msg"] = "Same Religion Found";
                    dt.Rows.Add(dr);
                    ds1.Tables.Add(dt);
                }
                else
                {
                    if (sm.categories == "savereligion")
                    {
                        query = "insert into mst_religion_tbl (religion,curr_dt,mod_dt,del_flag)values(N'" + sm.category + "',GETDATE(),NULL,0)";
                        dr["msg"] = "Saved Successfully";
                    }
                    else
                    {
                        query = "update mst_religion_tbl set religion=N'" + sm.category + "',mod_dt=GETDATE() where religion_id='" + sm.categories + "' and del_flag=0";
                        dr["msg"] = "Updated Successfully";
                    }
                    ds1 = cls.fillds(query);
                    strquery = "select religion_id,religion,case when (select count(*) from adm_student_master where religion=r.religion_id and del_flag=0)>0 then '1' else '0' end as flag from mst_religion_tbl r where del_flag=0";
                    ds1 = cls.fillds(strquery);
                    ds1.Tables[0].TableName = "category";
                    dt.Rows.Add(dr);
                    ds1.Tables.Add(dt);
                }
            }
            else if (sm.type_of_query == "updatedeletereligion")
            {
                DataTable dt = new DataTable("MyTable");
                dt.Columns.Add(new DataColumn("msg", typeof(string)));
                DataRow dr = dt.NewRow();

                string query = "update mst_religion_tbl set del_flag=1,mod_dt=GETDATE() where religion_id='" + sm.category + "' and del_flag=0";
                dr["msg"] = "Deleted Successfully";
                ds1 = cls.fillds(query);
                strquery = "select religion_id,religion,case when (select count(*) from adm_student_master where religion=r.religion_id and del_flag=0)>0 then '1' else '0' end as flag from mst_religion_tbl r where del_flag=0";
                ds1 = cls.fillds(strquery);
                ds1.Tables[0].TableName = "category";
                dt.Rows.Add(dr);
                ds1.Tables.Add(dt);
            }
            //---------------------------------------------------

            //-----------------------------------------category
            else if (sm.type_of_query == "categorymodal")
            {
                strquery = "select category_id,category_name, case when (select count(*) from adm_student_master where category=cat.category_id and del_flag=0)>0 then '1' else '0' end as flag,case when (select count(*) from mst_cast_tbl where category_id=cat.category_id and del_flag=0)>0 then '1' else '0' end as count from mst_category_tbl cat where del_flag=0";
                ds1 = cls.fillds(strquery);
                ds1.Tables[0].TableName = "category";
            }
            else if (sm.type_of_query == "savecategory")
            {

                DataTable dt = new DataTable("MyTable");
                dt.Columns.Add(new DataColumn("msg", typeof(string)));
                DataRow dr = dt.NewRow();

                string query = "select * from mst_category_tbl where category_name=N'" + sm.category + "' and del_flag=0";
                ds1 = cls.fillds(query);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    dr["msg"] = "Same Category Found";
                    dt.Rows.Add(dr);
                    ds1.Tables.Add(dt);
                }
                else
                {
                    if (sm.categories == "savecategory")
                    {
                        query = "insert into mst_category_tbl (category_name,curr_dt,mod_dt,del_flag)values(N'" + sm.category + "',GETDATE(),NULL,0)";
                        dr["msg"] = "Saved Successfully";
                    }
                    else
                    {
                        query = "update mst_category_tbl set category_name=N'" + sm.category + "',mod_dt=GETDATE() where category_id='" + sm.categories + "' and del_flag=0";
                        dr["msg"] = "Updated Successfully";
                    }
                    ds1 = cls.fillds(query);
                    strquery = "select category_id,category_name, case when (select count(*) from adm_student_master where category=cat.category_id and del_flag=0)>0 then '1' else '0' end as flag,case when (select count(*) from mst_cast_tbl where category_id=cat.category_id and del_flag=0)>0 then '1' else '0' end as count from mst_category_tbl cat where del_flag=0";
                    ds1 = cls.fillds(strquery);
                    ds1.Tables[0].TableName = "category";
                    dt.Rows.Add(dr);
                    ds1.Tables.Add(dt);
                }
            }
            else if (sm.type_of_query == "updatedeletecategory")
            {
                DataTable dt = new DataTable("MyTable");
                dt.Columns.Add(new DataColumn("msg", typeof(string)));
                DataRow dr = dt.NewRow();

                string query = "update mst_category_tbl set del_flag=1,mod_dt=GETDATE() where category_id='" + sm.category + "' and del_flag=0";
                dr["msg"] = "Deleted Successfully";
                ds1 = cls.fillds(query);
                strquery = "select category_id,category_name, case when (select count(*) from adm_student_master where category=cat.category_id and del_flag=0)>0 then '1' else '0' end as flag,case when (select count(*) from mst_cast_tbl where category_id=cat.category_id and del_flag=0)>0 then '1' else '0' end as count from mst_category_tbl cat where del_flag=0";
                ds1 = cls.fillds(strquery);
                ds1.Tables[0].TableName = "category";
                dt.Rows.Add(dr);
                ds1.Tables.Add(dt);
            }
            //---------------------------------------------------------

                //------------------------caste-------------------------
            else if (sm.type_of_query == "castemodal")
            {
                if (sm.category == null)
                {
                    strquery = "select category_id,cast_id,cast_name,case when (select count(*) from adm_student_master where caste=ca.cast_id and del_flag=0)>0 then '1' else '0' end as flag ,case when (select count(*) from mst_subcast_tbl where cast_id=ca.cast_id and del_flag=0)>0 then '1' else '0' end as count from mst_cast_tbl ca where del_flag=0; select category_id,category_name from mst_category_tbl where del_flag=0";
                }
                else if (sm.category != null && sm.category != "")
                {
                    strquery = "select category_id,cast_id,cast_name,case when (select count(*) from adm_student_master where caste=ca.cast_id and del_flag=0)>0 then '1' else '0' end as flag ,case when (select count(*) from mst_subcast_tbl where cast_id=ca.cast_id and del_flag=0)>0 then '1' else '0' end as count from mst_cast_tbl ca where del_flag=0 and category_id='" + sm.category + "'; select category_id,category_name from mst_category_tbl where del_flag=0";
                }
                ds1 = cls.fillds(strquery);
                ds1.Tables[0].TableName = "caste";
            }
            else if (sm.type_of_query == "savecaste")
            {
                DataTable dt = new DataTable("MyTable");
                dt.Columns.Add(new DataColumn("msg", typeof(string)));
                DataRow dr = dt.NewRow();

                string query = "select * from mst_cast_tbl where cast_name=N'" + sm.caste + "' and category_id='" + sm.category + "' and del_flag=0; select category_id,category_name from mst_category_tbl where del_flag=0";
                ds1 = cls.fillds(query);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    dr["msg"] = "Same Caste Found";
                    dt.Rows.Add(dr);
                    ds1.Tables.Add(dt);
                }
                else
                {
                    if (sm.categories == "savecaste")
                    {
                        query = "insert into mst_cast_tbl(category_id,cast_name,curr_dt,del_flag) values('" + sm.category + "',N'" + sm.caste + "',GETDATE(),0)";
                        dr["msg"] = "Saved Successfully";
                    }
                    else if (sm.categories == "updatecaste")
                    {
                        query = "update mst_cast_tbl set cast_name=N'" + sm.caste + "',mod_dt=GETDATE() where category_id='" + sm.category + "' and cast_id='" + sm.cast + "' and del_flag=0";
                        dr["msg"] = "Updated Successfully";
                    }
                    ds1 = cls.fillds(query);
                    strquery = "select category_id,cast_id,cast_name,case when (select count(*) from adm_student_master where caste=ca.cast_id and del_flag=0)>0 then '1' else '0' end as flag ,case when (select count(*) from mst_subcast_tbl where cast_id=ca.cast_id and del_flag=0)>0 then '1' else '0' end as count from mst_cast_tbl ca where del_flag=0; select category_id,category_name from mst_category_tbl where del_flag=0";
                    ds1 = cls.fillds(strquery);
                    ds1.Tables[0].TableName = "caste";
                    dt.Rows.Add(dr);
                    ds1.Tables.Add(dt);
                }
            }
            else if (sm.type_of_query == "deletecaste")
            {
                DataTable dt = new DataTable("MyTable");
                dt.Columns.Add(new DataColumn("msg", typeof(string)));
                DataRow dr = dt.NewRow();

                string query = "update mst_cast_tbl set del_flag=1,mod_dt=GETDATE() where category_id='" + sm.category + "' and cast_id='" + sm.caste + "' and del_flag=0";
                dr["msg"] = "Deleted Successfully";
                ds1 = cls.fillds(query);
                strquery = "select category_id,cast_id,cast_name,case when (select count(*) from adm_student_master where caste=ca.cast_id and del_flag=0)>0 then '1' else '0' end as flag ,case when (select count(*) from mst_subcast_tbl where cast_id=ca.cast_id and del_flag=0)>0 then '1' else '0' end as count from mst_cast_tbl ca where del_flag=0; select category_id,category_name from mst_category_tbl where del_flag=0";
                ds1 = cls.fillds(strquery);
                ds1.Tables[0].TableName = "caste";
                dt.Rows.Add(dr);
                ds1.Tables.Add(dt);
            }
            //------------------------------------------------------

                //------------------------------subcaste---------------------
            else if (sm.type_of_query == "subcastemodal")
            {
                if (sm.caste == null)
                {
                    strquery = "select cast_id,subcast_id,subcast_name,case when (select count(*) from adm_student_master where sub_caste=sc.subcast_id and del_flag=0)>0 then '1' else '0' end as flag from mst_subcast_tbl sc where del_flag=0 ; select cast_id,cast_name from mst_cast_tbl where del_flag=0";
                }
                else if (sm.caste != null && sm.caste != "")
                {
                    strquery = "select cast_id,subcast_id,subcast_name,case when (select count(*) from adm_student_master where sub_caste=sc.subcast_id and del_flag=0)>0 then '1' else '0' end as flag from mst_subcast_tbl sc where del_flag=0 and cast_id='" + sm.caste + "' ; select cast_id,cast_name from mst_cast_tbl where del_flag=0";
                }
                ds1 = cls.fillds(strquery);
                ds1.Tables[0].TableName = "subcaste";
            }
            else if (sm.type_of_query == "savesubcaste")
            {
                DataTable dt = new DataTable("MyTable");
                dt.Columns.Add(new DataColumn("msg", typeof(string)));
                DataRow dr = dt.NewRow();

                string query = "select * from mst_subcast_tbl where subcast_name=N'" + sm.subcaste + "' and cast_id='" + sm.caste + "' and del_flag=0; select cast_id,cast_name from mst_cast_tbl where del_flag=0";
                ds1 = cls.fillds(query);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    dr["msg"] = "Same Subcaste Found";
                    dt.Rows.Add(dr);
                    ds1.Tables.Add(dt);
                }
                else
                {
                    if (sm.categories == "savesubcaste")
                    {
                        query = "insert into mst_subcast_tbl(cast_id,subcast_name,curr_dt,del_flag) values('" + sm.caste + "',N'" + sm.subcaste + "',GETDATE(),0)";
                        dr["msg"] = "Saved Successfully";
                    }
                    else if (sm.categories == "updatesubcaste")
                    {
                        query = "update mst_subcast_tbl set subcast_name=N'" + sm.subcaste + "',mod_dt=GETDATE() where cast_id='" + sm.caste + "' and subcast_id='" + sm.subcast + "' and del_flag=0";
                        dr["msg"] = "Updated Successfully";
                    }
                    ds1 = cls.fillds(query);
                    strquery = "select cast_id,subcast_id,subcast_name,case when (select count(*) from adm_student_master where sub_caste=sc.subcast_id and del_flag=0)>0 then '1' else '0' end as flag from mst_subcast_tbl sc where del_flag=0 ; select cast_id,cast_name from mst_cast_tbl where del_flag=0";
                    ds1 = cls.fillds(strquery);
                    ds1.Tables[0].TableName = "caste";
                    dt.Rows.Add(dr);
                    ds1.Tables.Add(dt);
                }
            }
            else if (sm.type_of_query == "deletesubcaste")
            {
                DataTable dt = new DataTable("MyTable");
                dt.Columns.Add(new DataColumn("msg", typeof(string)));
                DataRow dr = dt.NewRow();

                string query = "update mst_subcast_tbl set del_flag=1,mod_dt=GETDATE() where cast_id='" + sm.caste + "' and subcast_id='" + sm.subcast + "' and del_flag=0";
                dr["msg"] = "Deleted Successfully";
                ds1 = cls.fillds(query);
                strquery = "select cast_id,subcast_id,subcast_name,case when (select count(*) from adm_student_master where sub_caste=sc.subcast_id and del_flag=0)>0 then '1' else '0' end as flag from mst_subcast_tbl sc where del_flag=0 ; select cast_id,cast_name from mst_cast_tbl where del_flag=0";
                ds1 = cls.fillds(strquery);
                ds1.Tables[0].TableName = "caste";
                dt.Rows.Add(dr);
                ds1.Tables.Add(dt);
            }
            //--------------------------------------------------------------------
            return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
        }

    }
}
