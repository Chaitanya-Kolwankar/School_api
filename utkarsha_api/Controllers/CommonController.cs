using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace utkarsha_api.Controllers
{
    public class CommonController : ApiController
    {
        Common cm = new Common();
        Class1 cs = new Class1();
        [HttpPost]

        public HttpResponseMessage Common([FromBody] Common cm)
        {
            string type = cm.type;
            

            DataSet ds = new DataSet();
            DataSet dsnew = new DataSet();
            //col1 is type
            if (type == "ddlfill")
            {
                string q1 = "select distinct med_id, medium from mst_medium_tbl where del_flag = '0'; ";
                q1+= " select s.std_id, s.std_name, m.med_id, m.medium from mst_medium_tbl as m inner join mst_standard_tbl as s on m.med_id = s.med_id where s.del_flag = 0 and m.del_flag = 0 order by m.med_id,s.rank; ";
                q1+= " select * from m_academic ";
                if (cm.year != null && cm.year != "")
                {
                    q1+= " where ayid<'" + cm.year + "' ";
                }
                q1 += " order by AYID desc";

                ds = cs.fillds(q1);
                DataTable dt1 = ds.Tables[0];
                DataTable dt2 = ds.Tables[1];
                //Medium name in array
                List<string> med_name_arr = new List<string>();

                foreach (DataRow row in dt1.Rows)
                {
                    med_name_arr.Add(row[0].ToString());
                }
                string[] med_name_array = med_name_arr.ToArray();
                //array of mediums

                for (int i = 0; i < med_name_array.Length; i++)
                {
                    string temp_dt_name = med_name_array[i].ToString();
                    DataTable temp_dt = new DataTable(temp_dt_name);
                    temp_dt.Columns.Add("std_id");
                    temp_dt.Columns.Add("std_name");
                    //dt.Clear();
                    foreach (DataRow row in dt2.Rows)
                    {
                        if (row[2].ToString() == med_name_array[i].ToString())
                        {                                                            
                            DataRow _newrow = temp_dt.NewRow();
                            _newrow["std_id"] = row[0].ToString();
                            _newrow["std_name"] = row[1].ToString();
                            temp_dt.Rows.Add(_newrow);
                        }
                    }

                    ds.Tables.Add(temp_dt);
                }

                ds.Tables.Remove("Table1");
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, ds, "application/json");
        }

    }

    
}
