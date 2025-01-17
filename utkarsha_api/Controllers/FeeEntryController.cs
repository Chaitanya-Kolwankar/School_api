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
using System.Globalization;

namespace utkarsha_api.Controllers
{
    public class FeeEntryController : ApiController
    {
        DataSet ds1 = new DataSet();
        string strquery = "";
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        Class1 cls = new Class1();

        [HttpPost]
        public HttpResponseMessage FeeEntry([FromBody] FeeEntry fee)
        {
            if (fee.type == "yearlydata")
            {
                strquery = "select sm.Student_id,UPPER(ISNULL(sm.stud_L_name,'')+' '+ISNULL(sm.stud_F_name,'')+' '+ISNULL(sm.stud_m_name,'')+' '+ISNULL(sm.stud_mo_name,'')) as [Student_Name],a.AYID,(select duration from m_academic where AYID=a.AYID) as [Year],a.class_id,(select std_name from mst_standard_tbl where std_id=a.class_id and del_flag=0) as [standard],a.division_id, case when (select division_name from mst_division_tbl where division_id=a.division_id and del_flag=0) is null then 'NA' else (select division_name from mst_division_tbl where division_id=a.division_id and del_flag=0) end as [Division], case when (a.Roll_no is null or a.Roll_no='')then 'NA' else a.Roll_no end as [roll_no],a.medium_id from adm_student_master sm,adm_studentacademicyear a where sm.Student_id=a.student_id and a.del_flag=0 and sm.del_flag=0 and sm.Student_id='" + fee.stud_id + "' order by AYID desc";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (fee.type == "feesdata")
            {
                strquery = "select sm.Student_id,UPPER(ISNULL(sm.stud_L_name,'')+' '+ISNULL(sm.stud_F_name,'')+' '+ISNULL(sm.stud_m_name,'')+' '+ISNULL(sm.stud_mo_name,'')) as [Student_Name],a.AYID,(select duration from m_academic where AYID=a.AYID) as [Year],a.class_id,(select std_name from mst_standard_tbl where std_id=a.class_id and del_flag=0) as [standard],a.division_id, case when (select division_name from mst_division_tbl where division_id=a.division_id and del_flag=0) is null then 'NA' else (select division_name from mst_division_tbl where division_id=a.division_id and del_flag=0) end as [Division], case when (a.Roll_no is null or a.Roll_no='')then 'NA' else a.Roll_no end as [roll_no],a.medium_id,(select medium from mst_medium_tbl where med_id=a.medium_id and del_flag=0) as medium, (select category_name from mst_category_tbl where category_id=sm.category and del_flag=0) as categoryname,sm.category,case when a.gr_no is not null and a.gr_no!='' then a.gr_no else 'NA' end as gr_no from adm_student_master sm,adm_studentacademicyear a where sm.Student_id=a.student_id and a.del_flag=0 and sm.del_flag=0 and sm.Student_id='" + fee.stud_id + "' and a.AYID='" + fee.ayid + "'; ";
                strquery = strquery + "select type,value from mst_fee_type where del_flag=0 and type in (select distinct type from mst_fee_duration where ayid='" + fee.ayid + "' and med_id='" + fee.medium_id + "' and class_id='" + fee.class_id + "' and del_flag=0 and type in (select distinct type_name from mst_fee_master where ayid='" + fee.ayid + "' and med_id='" + fee.medium_id + "' and class_id='" + fee.class_id + "' and del_flag=0)); ";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (fee.type == "duration")
            {
                strquery = "select duration_id,duration from mst_fee_duration where ayid='" + fee.ayid + "' and med_id='" + fee.medium_id + "' and class_id='" + fee.class_id + "' and type='" + fee.dutype + "' and del_flag=0 order by duration_id, curr_dt desc;  ";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (fee.type == "gridfee")
            {
                strquery = "if((select YEAR(convert(date,date_of_admission,105)) as date from adm_student_master where Student_id='" + fee.stud_id + "') = (select year(convert(date,substring(duration,0,11),105)) from m_academic where is_current=1)) select struct_id,struct_name,Amount,paid,case when cast(balance as int)<0 then 0 else balance end as balance,Rank from(select struct_id,struct_name,Amount,case when (select sum(cast(amount as int)) from m_FeeEntry where Student_id='" + fee.stud_id + "' and AYID='" + fee.ayid + "' and struct_id=fm.struct_id and del_flag=0) is not null then (select sum(cast(amount as int)) from m_FeeEntry where Student_id='" + fee.stud_id + "' and AYID='" + fee.ayid + "' and struct_id=fm.struct_id and del_flag=0) else 0 end as paid,case when (select sum(cast(amount as int)) from m_FeeEntry where Student_id='" + fee.stud_id + "' and AYID='" + fee.ayid + "' and struct_id=fm.struct_id and del_flag=0) is not null then ((select cast(Amount as int) from mst_fee_master where AYID='" + fee.ayid + "' and med_id='" + fee.medium_id + "' and class_id='" + fee.class_id + "' and caste='" + fee.caste + "' and type_name='" + fee.dutype + "' and duration_id='" + fee.duration + "' and del_flag=0 and struct_id= fm.struct_id)-(select sum(cast(amount as int)) from m_FeeEntry where Student_id='" + fee.stud_id + "' and AYID='" + fee.ayid + "' and struct_id=fm.struct_id and del_flag=0)) else Amount end as balance,Rank from mst_fee_master fm where AYID='" + fee.ayid + "' and med_id='" + fee.medium_id + "' and class_id='" + fee.class_id + "' and caste='" + fee.caste + "' and type_name='" + fee.dutype + "' and duration_id='" + fee.duration + "' and del_flag=0  ) a order by cast(a.rank as int) else select struct_id,struct_name,Amount,paid,case when cast(balance as int)<0 then 0 else balance end as balance,Rank from(select struct_id,struct_name,Amount,case when (select sum(cast(amount as int)) from m_FeeEntry where Student_id='" + fee.stud_id + "' and AYID='" + fee.ayid + "' and struct_id=fm.struct_id and del_flag=0) is not null then (select sum(cast(amount as int)) from m_FeeEntry where Student_id='" + fee.stud_id + "' and AYID='" + fee.ayid + "' and struct_id=fm.struct_id and del_flag=0) else 0 end as paid,case when (select sum(cast(amount as int)) from m_FeeEntry where Student_id='" + fee.stud_id + "' and AYID='" + fee.ayid + "' and struct_id=fm.struct_id and del_flag=0) is not null then ((select cast(Amount as int) from mst_fee_master where AYID='" + fee.ayid + "' and med_id='" + fee.medium_id + "' and class_id='" + fee.class_id + "' and caste='" + fee.caste + "' and type_name='" + fee.dutype + "' and duration_id='" + fee.duration + "' and del_flag=0 and struct_id= fm.struct_id)-(select sum(cast(amount as int)) from m_FeeEntry where Student_id='" + fee.stud_id + "' and AYID='" + fee.ayid + "' and struct_id=fm.struct_id and del_flag=0)) else Amount end as balance,Rank from mst_fee_master fm where AYID='" + fee.ayid + "' and med_id='" + fee.medium_id + "' and class_id='" + fee.class_id + "' and caste='" + fee.caste + "' and type_name='" + fee.dutype + "' and duration_id='" + fee.duration + "' and del_flag=0 and fm.admflg=0) a order by cast(a.rank as int)";
                strquery = strquery + "select a.total, case when a.paid is null then 0 else case when a.refunded is not null  then a.paid-a.refunded else a.paid end end as paid,case when a.balance is null then a.total when a.balance<0 then 0 else a.balance end as balance,case when a.balance <0 then case when a.refunded is not null then abs(a.balance)-a.refunded else abs(a.balance) end end as refundable ,case when a.refunded is null then 0 else a.refunded end as refunded from  (select sum(cast(amount as int)) as total,(select sum(cast(amount as int)) from m_FeeEntry where Student_id='" + fee.stud_id + "' and AYID='" + fee.ayid + "' and del_flag=0  and Type='Fees' and struct_id in (select distinct struct_id from mst_fee_master where AYID='" + fee.ayid + "' and med_id='" + fee.medium_id + "' and class_id='" + fee.class_id + "' and caste='" + fee.caste + "' and type_name='" + fee.dutype + "' and duration_id='" + fee.duration + "' and del_flag=0)) as paid, ((select sum(cast(Amount as int)) from mst_fee_master where AYID='" + fee.ayid + "' and med_id='" + fee.medium_id + "' and class_id='" + fee.class_id + "' and caste='" + fee.caste + "' and type_name='" + fee.dutype + "' and duration_id='" + fee.duration + "' and del_flag=0)-(select sum(cast(amount as int)) from m_FeeEntry where Student_id='" + fee.stud_id + "' and AYID='" + fee.ayid + "' and Type='Fees' and struct_id in (select distinct struct_id from mst_fee_master where AYID='" + fee.ayid + "' and med_id='" + fee.medium_id + "' and class_id='" + fee.class_id + "' and caste='" + fee.caste + "' and type_name='" + fee.dutype + "' and duration_id='" + fee.duration + "' and del_flag=0) and del_flag=0)) as balance,(select sum(cast(amount as int)) from m_FeeEntry where Student_id='" + fee.stud_id + "' and AYID='" + fee.ayid + "' and duration_id='" + fee.duration + "' and Type='Refund' and del_flag=0) as refunded from mst_fee_master fm where AYID='" + fee.ayid + "' and med_id='" + fee.medium_id + "' and class_id='" + fee.class_id + "' and caste='" + fee.caste + "' and type_name='" + fee.dutype + "' and duration_id='" + fee.duration + "' and del_flag=0)a";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (fee.type == "refundable")
            {
                strquery = "select a.total, case when a.paid is null then 0 else case when a.refunded is not null  then a.paid-a.refunded else a.paid end end as paid,case when a.balance is null then a.total when a.balance<0 then 0 else a.balance end as balance,case when a.balance <0 then case when a.refunded is not null then abs(a.balance)-a.refunded else abs(a.balance) end end as refundable ,case when a.refunded is null then 0 else a.refunded end as refunded from  (select sum(cast(amount as int)) as total,(select sum(cast(amount as int)) from m_FeeEntry where Student_id='" + fee.stud_id + "' and AYID='" + fee.ayid + "' and del_flag=0  and Type='Fees' and struct_id in (select distinct struct_id from mst_fee_master where AYID='" + fee.ayid + "' and med_id='" + fee.medium_id + "' and class_id='" + fee.class_id + "' and caste='" + fee.caste + "' and type_name='" + fee.dutype + "' and duration_id='" + fee.duration + "' and del_flag=0)) as paid, ((select sum(cast(Amount as int)) from mst_fee_master where AYID='" + fee.ayid + "' and med_id='" + fee.medium_id + "' and class_id='" + fee.class_id + "' and caste='" + fee.caste + "' and type_name='" + fee.dutype + "' and duration_id='" + fee.duration + "' and del_flag=0)-(select sum(cast(amount as int)) from m_FeeEntry where Student_id='" + fee.stud_id + "' and AYID='" + fee.ayid + "' and Type='Fees' and struct_id in (select distinct struct_id from mst_fee_master where AYID='" + fee.ayid + "' and med_id='" + fee.medium_id + "' and class_id='" + fee.class_id + "' and caste='" + fee.caste + "' and type_name='" + fee.dutype + "' and duration_id='" + fee.duration + "' and del_flag=0) and del_flag=0)) as balance,(select sum(cast(amount as int)) from m_FeeEntry where Student_id='" + fee.stud_id + "' and AYID='" + fee.ayid + "' and duration_id='" + fee.duration + "' and Type='Refund' and del_flag=0) as refunded from mst_fee_master fm where AYID='" + fee.ayid + "' and med_id='" + fee.medium_id + "' and class_id='" + fee.class_id + "' and caste='" + fee.caste + "' and type_name='" + fee.dutype + "' and duration_id='" + fee.duration + "' and del_flag=0)a";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (fee.type == "loadgrid")
            {
                strquery = "select distinct Recpt_no,sum(cast(Amount as int)) as amount,Recpt_mode,convert(varchar,Pay_date,103) as date,type,(select duration from mst_fee_duration where duration_id=e.duration_id and del_flag=0) as duration,e.duration_id from m_FeeEntry e where Student_id='" + fee.stud_id + "' and ayid='" + fee.ayid + "' and del_flag=0 group by Recpt_no,Pay_date,Recpt_mode,Type,duration_id";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (fee.type == "editdata")
            {
                strquery = "select * from m_FeeEntry where Recpt_no='" + fee.receipt_no + "' and Student_id='" + fee.stud_id + "' and ayid='" + fee.ayid + "' and del_flag=0; ";
                strquery = strquery + " select * from mst_fee_type where type=(select type from mst_fee_duration where duration_id=(select distinct duration_id from m_FeeEntry where Recpt_no='" + fee.receipt_no + "' and Student_id='" + fee.stud_id + "' and ayid='" + fee.ayid + "' and del_flag=0) and del_flag=0) and del_flag=0;";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (fee.type == "deletefee")
            {
                strquery = "update m_FeeEntry set del_flag=1,del_dt=GETDATE() where Recpt_no='" + fee.receipt_no + "' and Student_id='" + fee.stud_id + "' and ayid='" + fee.ayid + "'";
                bool msg = cls.exeIUD(strquery);
                if (msg == true)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "success", "application/json");
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "fail", "application/json");
                }
            }
            else if (fee.type == "savetran")
            {
                strquery = "";
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(fee.table);
                if (dt.Rows.Count > 0)
                {
                    strquery = "select concat('" + fee.stud_id + "','/',(select (SUBSTRING(duration,9,2)+'-'+SUBSTRING(duration,22,2)) as ayid from m_academic where AYID='" + fee.ayid + "'),'/',(select (cast(count(distinct Recpt_no) as int))+1 from m_FeeEntry where Student_id='" + fee.stud_id + "' and Ayid='" + fee.ayid + "')) as no; ";
                    ds1 = cls.fillds(strquery);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        strquery = "";
                        fee.receipt_no = ds1.Tables[0].Rows[0]["no"].ToString();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            strquery = strquery + "Exec sp_fee_entry '" + fee.stud_id + "','" + dt.Rows[i]["Amount"].ToString() + "','" + fee.ayid + "','" + fee.paydate + "','" + dt.Rows[i]["struct_id"].ToString() + "','" + fee.duration_id + "',NULL,'" + fee.paymode + "','" + fee.receipt_no + "','" + fee.chdate + "','" + fee.chno + "','" + fee.bankname + "','" + fee.branchname + "','" + fee.chstatus + "','" + fee.fees_type + "','" + fee.user + "','" + dt.Rows[i]["Flag"].ToString() + "'";
                        }
                        bool msg = cls.exeIUD(strquery);

                        if (msg == true)
                        {
                            return this.Request.CreateResponse(HttpStatusCode.OK, "success", "application/json");
                        }
                        else
                        {
                            return this.Request.CreateResponse(HttpStatusCode.OK, "fail", "application/json");
                        }
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, "fail", "application/json");
                    }
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "No Changes Made", "application/json");
                }
            }
            else if (fee.type == "savetranrefund")
            {
                strquery = "";

                strquery = "select concat('" + fee.stud_id + "','/',(select (SUBSTRING(duration,9,2)+'-'+SUBSTRING(duration,22,2)) as ayid from m_academic where AYID='" + fee.ayid + "'),'/',(select (cast(count(distinct Recpt_no) as int))+1 from m_FeeEntry where Student_id='" + fee.stud_id + "' and Ayid='" + fee.ayid + "')) as no; ";
                ds1 = cls.fillds(strquery);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    fee.receipt_no = ds1.Tables[0].Rows[0]["no"].ToString();
                    strquery = "Exec sp_fee_entry '" + fee.stud_id + "','" + fee.table + "','" + fee.ayid + "','" + fee.paydate + "',NULL,'" + fee.duration_id + "','" + fee.duration + "','" + fee.paymode + "','" + fee.receipt_no + "','" + fee.chdate + "','" + fee.chno + "','" + fee.bankname + "','" + fee.branchname + "','" + fee.chstatus + "','" + fee.fees_type + "','" + fee.user + "','refund'";

                    bool msg = cls.exeIUD(strquery);

                    if (msg == true)
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, "success", "application/json");
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, "fail", "application/json");
                    }
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "fail", "application/json");
                }

            }
            else if (fee.type == "printdata")
            {
                strquery = "select a.total, case when a.paid is null then 0 else a.paid end as paid,case when a.balance is null then a.total when a.balance like '%-%' then 0 else a.balance end as balance from (select sum(cast(amount as int)) as total,(select sum(cast(amount as int)) from m_FeeEntry where Student_id in (select distinct Student_id from m_FeeEntry where Recpt_no='" + fee.receipt_no + "') and AYID in (select distinct AYID from m_FeeEntry where Recpt_no='" + fee.receipt_no + "') and del_flag=0 and struct_id in (select distinct struct_id from mst_fee_master where AYID in (select distinct AYID from m_FeeEntry where Recpt_no='" + fee.receipt_no + "') and med_id in (select  distinct fm.med_id from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and class_id in (select  distinct class_id from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and caste in (select  distinct caste from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and type_name in (select  distinct fm.type_name from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and duration_id in (select  distinct fe.duration_id from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and del_flag=0 and  Pay_date=(select distinct Pay_date from m_FeeEntry where Recpt_no='" + fee.receipt_no + "'))) as paid,((select sum(cast(Amount as int)) from mst_fee_master where AYID in (select distinct Ayid from m_FeeEntry where Recpt_no='" + fee.receipt_no + "') and med_id in (select  distinct fm.med_id from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and class_id in (select  distinct fm.class_id from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and caste in (select  distinct fm.caste from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and type_name in (select  distinct fm.type_name from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and duration_id in (select  distinct fe.duration_id from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and del_flag=0)-(select sum(cast(amount as int)) from m_FeeEntry where Student_id in (select distinct Student_id from m_FeeEntry where Recpt_no='" + fee.receipt_no + "')  and AYID in (select distinct Ayid from m_FeeEntry where Recpt_no='" + fee.receipt_no + "') and struct_id in (select distinct struct_id from mst_fee_master where AYID in (select distinct Ayid from m_FeeEntry where Recpt_no='" + fee.receipt_no + "') and med_id in (select  distinct fm.med_id from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and class_id in (select  distinct fm.class_id from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and caste in (select  distinct fm.caste from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and type_name in (select  distinct fm.type_name from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and duration_id in (select  distinct fe.duration_id from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and del_flag=0) and del_flag=0  and  Pay_date<=(select distinct Pay_date from m_FeeEntry where Recpt_no='" + fee.receipt_no + "'))) as balance from mst_fee_master fm where AYID in (select distinct Ayid from m_FeeEntry where Recpt_no='" + fee.receipt_no + "') and med_id in (select  distinct fm.med_id from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and class_id in (select  distinct fm.class_id from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and caste='55' and type_name in (select  distinct fm.type_name from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and duration_id in (select  distinct fe.duration_id from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and del_flag=0)a; ";
                strquery = strquery + "select  fm.struct_name,sum(cast (fe.Amount as int)) as rateamount,sum(cast(fm.Amount as int)) as totalamount from m_FeeEntry fe,mst_fee_master fm,mst_fee_duration fd where fe.Student_id in (select distinct Student_id from m_FeeEntry where Recpt_no='" + fee.receipt_no + "')   and fe.duration_id=fd.duration_id and fe.struct_id=fm.struct_id  and fd.med_id=fm.med_id and fd.class_id=fm.class_id and fd.duration_id=fe.duration_id and fm.med_id in (select  distinct fm.med_id from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and fm.class_id in (select  distinct fm.class_id from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and fe.del_flag=0 and fm.del_flag=0 and fe.duration_id in (select  distinct fm.duration_id from m_FeeEntry fe,mst_fee_master fm where fe.Recpt_no ='" + fee.receipt_no + "' and fe.duration_id=fm.duration_id and fe.struct_id=fm.struct_id) and fe.Pay_date <=  (select distinct Pay_date from m_FeeEntry where Recpt_no='" + fee.receipt_no + "') group by fm.struct_name,fm.Amount; ";
                strquery = strquery + "select distinct sm.Student_id,UPPER(ISNULL(sm.stud_L_name,'')+' '+ISNULL(sm.stud_F_name,'')+' '+ISNULL(sm.stud_m_name,'')+' '+ISNULL(sm.stud_mo_name,'')) as [Student_Name],sm.gender,a.AYID,(select duration from m_academic where AYID=a.AYID) as [Year],a.class_id,(select std_name from mst_standard_tbl where std_id=a.class_id and del_flag=0) as [standard],a.division_id, case when (select division_name from mst_division_tbl where division_id=a.division_id and del_flag=0) is null then 'NA' else (select division_name from mst_division_tbl where division_id=a.division_id and del_flag=0) end as [Division], case when (a.Roll_no is null or a.Roll_no='')then 'NA' else a.Roll_no end as [roll_no],a.medium_id,(select medium from mst_medium_tbl where med_id=a.medium_id and del_flag=0) as medium, (select category_name from mst_category_tbl where category_id=sm.category and del_flag=0) as categoryname,sm.category,case when a.gr_no is not null and a.gr_no!='' then a.gr_no else 'NA' end as gr_no,fe.Recpt_mode,fe.Recpt_Chq_No,fe.Recpt_Chq_dt,fe.Recpt_Bnk_Name,fe.Recpt_Bnk_Branch,format( fe.Pay_date,'dd/MM/yyyy') as Pay_date,fe.Type  from adm_student_master sm,adm_studentacademicyear a,m_FeeEntry as fe  where sm.Student_id=a.student_id and a.del_flag=0 and sm.del_flag=0 and sm.Student_id  in (select distinct student_id from m_FeeEntry where Recpt_no='" + fee.receipt_no + "')  and a.AYID  in (select distinct Ayid from m_FeeEntry where Recpt_no='" + fee.receipt_no + "') and fe.Student_id=a.student_id and fe.Recpt_no='" + fee.receipt_no + "' and fe.Ayid=a.AYID and fe.del_flag=0 ; ";
                strquery = strquery + "select Amount,refund_details from m_FeeEntry where Recpt_no='" + fee.receipt_no + "' ;";
                ds1 = cls.fillds(strquery);
                return this.Request.CreateResponse(HttpStatusCode.OK, ds1, "application/json");
            }
            else if (fee.type == "updatetran")
            {
                strquery = "";
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(fee.table);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strquery = strquery + "Exec sp_fee_entry '" + fee.stud_id + "','" + dt.Rows[i]["Amount"].ToString() + "','" + fee.ayid + "','" + fee.paydate + "','" + dt.Rows[i]["struct_id"].ToString() + "','" + fee.duration_id + "',NULL,'" + fee.paymode + "','" + fee.receipt_no + "','" + fee.chdate + "','" + fee.chno + "','" + fee.bankname + "','" + fee.branchname + "','" + fee.chstatus + "','" + fee.fees_type + "','" + fee.user + "','" + dt.Rows[i]["Flag"].ToString() + "'";
                    }
                    bool msg = cls.exeIUD(strquery);

                    if (msg == true)
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, "success", "application/json");
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, "fail", "application/json");
                    }

                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "No Changes Made", "application/json");
                }
            }
            else if (fee.type == "updatetranrefund")
            {
                strquery = "";

                strquery = "Exec sp_fee_entry '" + fee.stud_id + "','" + fee.table + "','" + fee.ayid + "','" + fee.paydate + "',NULL,'" + fee.duration_id + "','" + fee.duration + "','" + fee.paymode + "','" + fee.receipt_no + "','" + fee.chdate + "','" + fee.chno + "','" + fee.bankname + "','" + fee.branchname + "','" + fee.chstatus + "','" + fee.fees_type + "','" + fee.user + "','updaterefund'";

                bool msg = cls.exeIUD(strquery);

                if (msg == true)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "success", "application/json");
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, "fail", "application/json");
                }
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, "No Changes Made", "application/json");
            }
        }
    }
}
