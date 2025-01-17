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
    public class GR_ReportController : ApiController
    {
         SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();
        DataSet ds = new DataSet();
        string strquery;
        string ok;
        
        

        [HttpPost]
        public HttpResponseMessage grrpt([FromBody] GR_rept grp)
        {
            if (grp.type == "getexl")
            {
                string strquery = "Select asm.Student_id,isnull (asm.stud_L_name,'') + ' ' + isnull (asm.stud_F_name,'') + ' ' + isnull (asm.stud_m_name,'') + ' ' + isnull (asm.stud_mo_name,'')";
                strquery = strquery + " as Student_Name ,COALESCE(say.gr_no,'') as gr_no,(select category_name from mst_category_tbl where category_id=asm.category) as cat,format(convert(date ,asm.dob ,103) ,'dd-MM-yyyy') as Date,asm.form_id from dbo.adm_student_master asm , ";
                strquery = strquery + "   dbo.adm_studentacademicyear as say where  asm.student_id = say.student_id and asm.del_flag= 0 and say.del_flag=0  and say.AYID = '" + grp.ayid + "'   ";
                strquery = strquery + " and say.medium_id='" + grp.medium + "' and say.class_id='" + grp.standard + "'";
                ds = cls.fillds(strquery);
            }
            else if (grp.type == "getidexl")
            {
                string strquery = " Select isnull (asm.stud_L_name,'') + ' ' + isnull (asm.stud_F_name,'') + ' ' + isnull (asm.stud_m_name,'') + ' ' + isnull (asm.stud_mo_name,'') as [NAME OF THE STUDENT] ,asm.address as ADDRESS,COALESCE(say.gr_no,'') as [GR NO],say.Roll_no AS [ROLL NO],(SELECT std_name  FROM mst_standard_tbl where std_id='" + grp.standard + "') AS STANDARD,(SELECT division_name  FROM mst_division_tbl WHERE division_id=say.division_id) AS DIVISION,asm.phone_no1 as [CONTACT 1],asm.phone_no2 as [CONTACT 2],asm.aadhar_no as [AADHAR NO],asm.saral_id AS[SARAL ID],asm.bank_ac_no as [BANK ACCOUNT NO],asm.Branch_name as [BRANCH NAME],asm.IFSC_code as [IFSC NO],asm.bank_name as [BANK NAME], (select category_name from mst_category_tbl where category_id=asm.category) as CATEGORY,(select cast_name from mst_cast_tbl where cast_id=asm.caste) AS [CASTE],(select subcast_name from mst_subcast_tbl where subcast_id=asm.sub_caste) AS [SUB CASTE] from dbo.adm_student_master asm ,  dbo.adm_studentacademicyear as say where  asm.student_id = say.student_id and asm.del_flag= 0 and say.del_flag=0  and say.AYID = '" + grp.ayid + "'  and say.medium_id='" + grp.medium + "' and say.class_id='" + grp.standard + "'";
               
                ds = cls.fillds(strquery);
            }  
            else
            {
                DataTable dt = new DataTable("New");
                dt.Columns.Add(new DataColumn("msg", typeof(string)));
                DataRow dr = dt.NewRow();
                dr["msg"] = "No";
                dt.Rows.Add(dr);
                ds.Tables.Add(dt);


            }

                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
    }
}
