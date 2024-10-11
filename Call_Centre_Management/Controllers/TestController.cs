using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Call_Centre_Management.Classes;
using Call_Centre_Management.Models;

namespace Call_Centre_Management.Controllers
{
    public class TestController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public DataTable item_table()
        {
            DataTable dt = new DataTable();
            dt.Rows.Add("item_id",typeof(int));
            dt.Rows.Add("item_qty", typeof(int));
            dt.Rows.Add("item_rate", typeof(double));
            dt.Rows.Add("total_item_price", typeof(double));
            return dt;
        }

        public void convert_list_into_dt()
        { 
        
        }
       




    }
}