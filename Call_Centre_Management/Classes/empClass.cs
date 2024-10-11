using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Call_Centre_Management.Models;
using Call_Centre_Management.Classes;

namespace Call_Centre_Management.Classes
{
    public class empClass
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        public Area_CommonClass areaClass = new Area_CommonClass();
        Common_Class commonClass = new Common_Class();
        public string check_role(string empId)
        {
            dict.Clear();
            dict.Add("@mode", "EmpDetails_ID");
            dict.Add("@id", Convert.ToInt32(empId));
            DataTable dt = commonClass.return_datatable(dict, "proc_employee");
            return Convert.ToString(dt.Rows[0]["role"]);
        }

        //public int Notifications(int EmpId)
        //{
        //    int Count = 0;
        //    dict.Clear();
        //    dict.Add("@UserId", Convert.ToInt32(EmpId));
        //    dict.Add("@mode", "CountNotification");
        //    DataTable dt = commonClass.return_datatable(dict, "ProcDashboard");
        //    if (dt.Rows.Count > 0)
        //    {
        //        Count = Convert.ToInt32(dt.Rows[0]["total"]);
        //    }
            
        //    return Count;
        //}

    }


}
