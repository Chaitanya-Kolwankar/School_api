using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace utkarsha_api.Controllers
{
    public class Emp_qualificationController : ApiController
    {
        emp_qualification q = new emp_qualification();
        Class1 cls = new Class1();
        DataSet ds = new DataSet();

        [HttpPost]

        public HttpResponseMessage EmpQualification([FromBody] emp_qualification q)
        {
            string del_str = "";
            if (q.type == "insert")
            {
                string qry = "insert into emp_education_details (emp_id,emp_coll_name,emp_unversity_board_name,emp_degree_name,emp_degree_type,emp_specialization_subj,";
                qry = qry + "emp_month_of_passing,emp_year_of_passing,emp_marks_obtained,emp_total_marks,emp_class_secured,emp_pursuing,exam_order,emp_netset_remark,emp_col_state,emp_col_place,emp_seatno)";
                qry = qry + "values ('" + q.emp_id + "','" + q.emp_coll_name + "','" + q.emp_uni_board_name + "','" + q.emp_deg_name + "','" + q.emp_deg_type + "','" + q.emp_spec_subject + "','" + q.emp_month_of_passing + "',";
                qry = qry + " '" + q.emp_year_of_passing + "','" + q.emp_mrk_obt + "','" + q.emp_tot_mrk + "','" + q.emp_class_sec + "',";
                qry = qry + " '" + q.emp_pursuing + "','" + q.emp_exm_order + "','" + q.emp_exm_netset_remrk + "','" + q.emp_coll_state + "','" + q.emp_coll_place + "','" + q.emp_seat_no + "')";


                cls.exeIUD(qry);
                return this.Request.CreateResponse(HttpStatusCode.OK, "Insert", "application/json");
            }
            else if (q.type == "delete")
            {
                del_str = "delete from emp_education_details where emp_id='" + q.emp_id + "' and emp_degree_name='" + q.emp_deg_name + "'";
                cls.exeIUD(del_str);

                return this.Request.CreateResponse(HttpStatusCode.OK, "Delete", "application/json");
            }

            else if (q.type == "delete_with_sub")
            {
                del_str = "delete from emp_education_details where emp_id='" + q.emp_id + "' and emp_degree_name='" + q.emp_deg_name + "' and emp_specialization_subj='" + q.emp_spec_subject + "'";
                cls.exeIUD(del_str);

                return this.Request.CreateResponse(HttpStatusCode.OK, "Delete", "application/json");
            }
     
            else if (q.type == "Others_delete")
            {
                del_str = "delete from emp_education_details where emp_id='" + q.emp_id + "' and emp_degree_name='" + q.emp_deg_name + "' and emp_specialization_subj=''";
                cls.exeIUD(del_str);

                return this.Request.CreateResponse(HttpStatusCode.OK, "Delete", "application/json");
            }
            else if (q.type == "bindgrid_ssc")
            {
                string str = "select case when emp_coll_name='' then '--'  else  coalesce (emp_coll_name,'NA') end emp_coll_name,case when emp_unversity_board_name ='' then '' else coalesce(emp_unversity_board_name,'NA') end emp_unversity_board_name ,    ";
                str = str + "  case  when emp_degree_name='' then '--' else coalesce  (emp_degree_name,'NA') end emp_degree_name  ,case when  emp_degree_type='' then 'NA' else coalesce (emp_degree_type,'NA') end emp_degree_type";
                str = str + " ,case when emp_specialization_subj='' then '--' else coalesce(emp_specialization_subj,'NA') end emp_specialization_subj, case when emp_month_of_passing='' then '--' else coalesce (emp_month_of_passing,'NA') end";
                str = str + " emp_month_of_passing,case when emp_year_of_passing='' then '--' else coalesce  (emp_year_of_passing,'NA') end emp_year_of_passing,  coalesce (emp_marks_obtained,0)  emp_marks_obtained,";
                str = str + " coalesce (emp_total_marks,0) emp_total_marks,case when  emp_class_secured='' then '--' else coalesce (emp_class_secured,'NA') end emp_class_secured,case when emp_pursuing='' then 'False' else coalesce (emp_pursuing,'False') end";
                str = str + "   emp_pursuing ,case when exam_order='' then '--' else coalesce(exam_order,'NA') end exam_order, case when emp_netset_remark='' then '--' else coalesce(emp_netset_remark,'NA') end emp_netset_remark,";
                str = str + "  case when emp_col_state='' then '--' else coalesce(emp_col_state,'NA')end  emp_col_state,case when emp_col_place='' then '--' else coalesce(emp_col_place,'NA') end";
                str = str + "  emp_col_place,case when emp_seatno='' then '--' else coalesce(emp_seatno,'NA') end emp_seatno from emp_education_details where emp_id='" + q.emp_id + "'";
                
                ds = cls.fillds(str);
                ds.Tables[0].TableName = "gridload";
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            else if (q.type == "bindgrid_hsc")
            {
                string str = " select emp_coll_name,emp_unversity_board_name,emp_degree_name,emp_degree_type,emp_specialization_subj,emp_month_of_passing,emp_year_of_passing,emp_marks_obtained,     ";
                str = str + " emp_total_marks,emp_class_secured,emp_pursuing,exam_order,emp_netset_remark,emp_col_state,emp_col_place,emp_seatno from employee_education_details   ";
                str = str + " where emp_id='" + q.emp_id + "' and emp_degree_name='H.S.C' ";
      

                ds = cls.fillds(str);
                ds.Tables[0].TableName = "gridload";
                return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
            }
            
            else
            {


                return this.Request.CreateResponse(HttpStatusCode.OK, "Delete", "application/json");
            }
           

        }
    }
}
