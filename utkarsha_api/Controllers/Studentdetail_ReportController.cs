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

namespace utkarsha_api.Controllers
{
    public class Studentdetail_ReportController : ApiController
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();
        DataSet ds = new DataSet();

        [HttpPost]
        public HttpResponseMessage studentdetailrpt([FromBody] Studentdetrpt sd)
        {
            string condition = "";
            if(sd.med_id!=null)
            {
                condition = " and b.medium_id='" + sd.med_id + "' ";
            }
            if (sd.std_id != null)
            {
                condition = " and b.class_id='" + sd.std_id + "' " + condition;
            }
            if (sd.div_id != null)
            {
                condition = " and b.division_id='" + sd.div_id + "' " + condition;
            }

            string strquery = "select Medium,Class,Division,[Form ID],[Student ID],[First Name],[Middle Name],Surname "+sd.fields+ "  from (select distinct (select medium from mst_medium_tbl where med_id=b.medium_id) Medium, (select std_name from mst_standard_tbl where std_id=b.class_id) Class,(select rank from mst_standard_tbl where std_id=b.class_id) Rank,(coalesce((select division_name from mst_division_tbl where division_id=b.division_id),'')) Division,a.form_id [Form ID],a.Student_id [Student ID],coalesce(gr_no,'') as [GR No.],coalesce(Roll_no,'') AS [Roll No.],format(date_of_admission,'dd-MM-yyyy') [Date of Admission],stud_F_name as [First Name],stud_m_name [Middle Name]	,stud_L_name [Surname]	,stud_mo_name [Mother Name]	,address [Address],coalesce(phone_no1,'') as [Mob 1],	coalesce(phone_no2 ,'')  [Mob 2],	gender as Gender,	coalesce(format(dob,'dd-MM-yyyy'),'') as [Date of Birth]	,coalesce(birth_place,'') as [Birth Place]	,coalesce(mother_tongue,'') [Mother Tongue]	,coalesce(nationality,'') as [Nationality],	coalesce(last_school_name,'') as [Last School Name],	coalesce(last_studied_std,'') as [Last studied Name],	coalesce(percentage,'') as [Percentage],	coalesce(grade,'') as Grade, coalesce((select religion from mst_religion_tbl where religion_id=a.religion ),'') as Religion, coalesce((select category_name from mst_category_tbl where category_id=category ),'') as	Category,	 coalesce(( select cast_name from mst_cast_tbl where cast_id=caste ),'') as Caste,	 coalesce(( select subcast_name from mst_subcast_tbl where subcast_id=sub_caste ),'') as	Subcaste, coalesce(aadhar_no,'') [Aadhar Card]	,coalesce(vehicle_type,'') [Vehicle Type],	coalesce(vehicle_no,'') [Vehicle No],	coalesce(driver_no,'') [Driver Number]	,coalesce(saral_id,'') [Saral ID],	coalesce(bank_ac_no,'') [Bank Account No],	coalesce(bank_name,'') [Bank Name],	coalesce(IFSC_code,'') [IFSC Code],coalesce(Branch_name,'') [Branch Name]	,coalesce(pincode,'') [Pincode],	coalesce(dist,'') [District],	coalesce(Taluka,'') [Taluka]	,coalesce(state,'') [State] from adm_student_master a inner join adm_studentacademicyear b on a.Student_id=b.student_id, mst_standard_tbl c where b.AYID='" + sd.AYID+ "' "+condition+ " and b.del_flag=0 and a.del_flag=0 ) a  order by Rank,cast(a.[Roll No.] as int)   ";
            ds = cls.fillds(strquery);

            return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
        }
    }
}
