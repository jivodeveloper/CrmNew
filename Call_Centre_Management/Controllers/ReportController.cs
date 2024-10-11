using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using Call_Centre_Management.Classes;
using Call_Centre_Management.Models;
namespace Call_Centre_Management.Controllers
{
    public class ReportController : Controller
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        Common_Class common_class = new Common_Class();
        // GET: Reports
        public ActionResult Index()
        {
            List<Item_Model> item_list = new List<Item_Model>();
            dict.Clear();
            dict.Add("@mode", "Getall_Items");
            DataTable dt = common_class.return_datatable(dict, "proc_item");
            int j = dt.Rows.Count;
            if (j > 0)
            {
                for (int i = 0; i < j; i++)
                {
                    Item_Model item = new Item_Model();
                    item.id = Convert.ToInt32(dt.Rows[i]["id"].ToString());
                    item.Item_id = dt.Rows[i]["item_id"].ToString();
                    item.item_name = dt.Rows[i]["Item_Name"].ToString();
                    item.rate = Convert.ToDouble(dt.Rows[i]["rate"].ToString());
                    item.Active = Convert.ToInt32(dt.Rows[i]["active"].ToString());
                    item.UOM_value = Convert.ToInt32(dt.Rows[i]["Uom_value"].ToString());
                    item_list.Add(item);
                }
            }
            ViewBag.get_item = item_list;
            return View();
        }
    }
}