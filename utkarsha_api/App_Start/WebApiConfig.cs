using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace utkarsha_api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "login",
                routeTemplate: "login/",
                defaults: new { controller = "login" }
            );
            //----------------------------

            config.Routes.MapHttpRoute(
             name: "Common",
             routeTemplate: "Common/",
             defaults: new { controller = "Common" }
         );

            //------------------------

            config.Routes.MapHttpRoute(
            name: "AcademicYear",
            routeTemplate: "AcademicYears/",
            defaults: new { controller = "AcademicYear" }
            );

            //----------------------------------------------
            config.Routes.MapHttpRoute(
            name: "Medium",
            routeTemplate: "Medium/",
            defaults: new { controller = "Medium" }
            );

            //---------------------------------------

            config.Routes.MapHttpRoute(
            name: "mediumload",
            routeTemplate: "loadgrid/",
            defaults: new { controller = "DivisonMaster" }
             );

            //--------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "std",
            routeTemplate: "std/",
            defaults: new { controller = "Standard" }
             );
            //---------------------------------------------
            config.Routes.MapHttpRoute(
            name: "abc",
            routeTemplate: "load/",
            defaults: new { controller = "LC" }
             );
            //----------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "Student_mast",
            routeTemplate: "stud_mast/",
            defaults: new { controller = "Student_mast" }
            );
            //-----------------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "GR3",
            routeTemplate: "updt/",
            defaults: new { controller = "GRAllocation" }
            );
            //---------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "stdacy",
            routeTemplate: "loaddata/",
            defaults: new { controller = "stdacy" }
            );
            //--------------------------------------------
            config.Routes.MapHttpRoute(
            name: "Student_transfer",
            routeTemplate: "studtransfer/",
            defaults: new { controller = "Student_transfer" }
            );
            //-----------------------------------------
            config.Routes.MapHttpRoute(
            name: "Modify",
            routeTemplate: "Modify/",
            defaults: new { controller = "Modify" }
            );
            //-----------------------------------------------
            config.Routes.MapHttpRoute(
            name: "cat",
            routeTemplate: "category/",
            defaults: new { controller = "category_master" }
            );
            //-------------------------------------
            config.Routes.MapHttpRoute(
            name: "bonafideload",
            routeTemplate: "loadbonafide/",
            defaults: new { controller = "bonafide" }
            );
            //------------------------------------------
            config.Routes.MapHttpRoute(
            name: "employee",
            routeTemplate: "loademp/",
            defaults: new { controller = "Employee_Search" }
            );
            //---------------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "id",
            routeTemplate: "id_gen/",
            defaults: new { controller = "id_generate_" }
            );
            //--------------------------------
            config.Routes.MapHttpRoute(
            name: "Statistical_Report",
            routeTemplate: "statisticalreport/",
            defaults: new { controller = "Statistical_Report" }
             );
            //----------------------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "employee_report_details",
            routeTemplate: "emprptdtl/",
            defaults: new { controller = "employee_report_details" }
             );
            //----------------------------------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "GR_Report",
            routeTemplate: "grrpt/",
            defaults: new { controller = "GR_Report" }
            );
            //-----------------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "change_password",
            routeTemplate: "loadpass/",
            defaults: new { controller = "change_password" }
            );
            //---------------------------------------------------------------------
            config.Routes.MapHttpRoute(//hrishi post notice application
            name: "post_notice_",
            routeTemplate: "postnotice/",
            defaults: new { controller = "Post_notice_" }
            );
            //------------------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "Emp_qualification",
            routeTemplate: "EmpQualification/",
            defaults: new { controller = "Emp_qualification" }
            );
            //----------------------------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "personal_details",
            routeTemplate: "loaddetails/",
            defaults: new { controller = "personal_details" }
            );
            //--------------------------------------------
            config.Routes.MapHttpRoute(
            name: "form_insert",
            routeTemplate: "Form1/",
            defaults: new { controller = "form_insert" }
            );
            //--------------------------------------------
            config.Routes.MapHttpRoute(
            name: "all/",
            routeTemplate: "all/",
            defaults: new { controller = "Allocation" }
            );
            //----------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "loadfee/",
            routeTemplate: "loadfee/",
            defaults: new { controller = "FeeMaster" }
            );
            //--------------------------------------------------------------
            config.Routes.MapHttpRoute(
             name: "Subject_Master",
             routeTemplate: "Subject_Master/",
             defaults: new { controller = "Subject_Master" }
            );
            //--------------------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "ExamMaster",
            routeTemplate: "ExamMaster/",
            defaults: new { controller = "ExamMaster" }
            );
            //-------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "MarksEntry",
            routeTemplate: "MarksEntry/",
            defaults: new { controller = "MarksEntry" }
            );
            //------------------------------------------------------------------
            config.Routes.MapHttpRoute(
             name: "Result",
             routeTemplate: "Result/",
             defaults: new { controller = "Result" }
             );
            //------------------------------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "ExamReport",
            routeTemplate: "ExamReport/",
            defaults: new { controller = "ExamReport" }
            );
            //-----------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "FeeEntry/",
            routeTemplate: "FeeEntry/",
            defaults: new { controller = "FeeEntry" }
            );
            //------------------------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "agereport",
            routeTemplate: "agereport/",
            defaults: new { controller = "agereport" }
            );
            //------------------------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "socialcategory",
            routeTemplate: "socialcategory/",
            defaults: new { controller = "socialcategory" }
            );
            //------------------------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "feereport",
            routeTemplate: "feereport/",
            defaults: new { controller = "feereport" }
            );
            //------------------------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "GroupMaster",
            routeTemplate: "GroupMaster/",
            defaults: new { controller = "GroupMaster" }
            );
            //------------------------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "GradeMaster",
            routeTemplate: "GradeMaster/",
            defaults: new { controller = "GradeMaster" }
            );
            //------------------------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "GroupAllocation",
            routeTemplate: "GroupAllocation/",
            defaults: new { controller = "GroupAllocation" }
            );
            //------------------------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "Studentdetail_Report",
            routeTemplate: "studentdetailrpt/",
            defaults: new { controller = "Studentdetail_Report" }
            );
            //------------------------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "loadmarks",
            routeTemplate: "loadmarks/",
            defaults: new { controller = "marks_criteria" }
            );
            //------------------------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "loadmarksentry",
            routeTemplate: "loadmarksentry/",
            defaults: new { controller = "MarksEntry" }
            );
            //------------------------------------------------------------------
            config.Routes.MapHttpRoute(
            name: "loadgazette",
            routeTemplate: "loadgazette/",
            defaults: new { controller = "gazette" }
            );
        }
    }
}
