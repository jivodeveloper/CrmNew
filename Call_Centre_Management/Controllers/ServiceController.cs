using Call_Centre_Management.Classes;
using Call_Centre_Management.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Call_Centre_Management.Controllers
{
    public class ServiceController : Controller
    {
        Common_Class commonClass = new Common_Class();
        Dictionary<string, object> dict = new Dictionary<string, object>();
        int empid = 0;
        string empNm = "";
        DataTable global_dt = new DataTable();

        [HttpGet]
        public JsonResult Login(string Username)
        {
            int GetId = 0;
            int flag = 0;
            dict.Clear();
            dict.Add("@customer_number", Username);
            dict.Add("@mode", "LoginInsertCustomer");
            global_dt = commonClass.return_datatable(dict, "procApi");
            int j = global_dt.Rows.Count;
            if (j > 0)
            {
                GetId = Convert.ToInt32(global_dt.Rows[0]["id"].ToString());
                flag = Convert.ToInt32(global_dt.Rows[0]["flag"].ToString());
            }
            return Json(new { GetId, flag }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Banner()
        {
            List<Banner> banners = new List<Banner>();
            dict.Clear();
            dict.Add("@mode", "Banner");
            global_dt = commonClass.return_datatable(dict, "procApi");
            int j = global_dt.Rows.Count;
            if (j > 0)
            {
                for (int i = 0; i < j; i++)
                {
                    Banner banner = new Banner();
                    banner.banner = Convert.ToString(global_dt.Rows[i]["BannerName"]);
                    banner.bannerpath = Convert.ToString(global_dt.Rows[i]["BannerPath"]);
                    banners.Add(banner);
                }
            }
            return Json(new { banners }, JsonRequestBehavior.AllowGet);
        }

         // POST api/<controller>
         /* [HttpPost]
          public JsonResult EmployeeProfile(Customer customer)
          {
              int Result = 0;
              dict.Clear();
              dict.Add("@Id", customer.id);
              dict.Add("@customer_name", customer.name);
              dict.Add("@Address", customer.Address);
              dict.Add("@state", customer.State_id);
              dict.Add("@zone", customer.Zone_id);
              dict.Add("@area", customer.Area_id);
              dict.Add("@Email", customer.CustAPiEmail);
              dict.Add("@mode", "updateCustomer");
              Result = commonClass.return_nonquery(dict, "procApi");
              return Json(new { Result }, JsonRequestBehavior.AllowGet);
          }*/
        

        [HttpPost]
        public JsonResult EmployeeProfile(Customer customer)
        {
            int Result = 0;
            dict.Clear();
            dict.Add("@customer_number", customer.CustAPimobile);
            dict.Add("@customer_name", customer.name);
            dict.Add("@Address", customer.Address);
            dict.Add("@state", customer.State_id);
            dict.Add("@zone", customer.Zone_id);
            dict.Add("@area", customer.Area_id);
            dict.Add("@Email", customer.CustAPiEmail);
            dict.Add("@mode", "updateCustomer");
            Result = commonClass.return_nonquery(dict, "procApi");
            return Json(new { Result }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetState()
        {
            List<State> states = new List<State>();
            dict.Clear();
            dict.Add("@mode", "State");
            global_dt = commonClass.return_datatable(dict, "procApi");
            int j = global_dt.Rows.Count;
            if (j > 0)
            {
                for (int i = 0; i < j; i++)
                {
                    State state = new State();
                    state.Id = Convert.ToInt32(global_dt.Rows[i]["Id"]);
                    state.state = Convert.ToString(global_dt.Rows[i]["state"]);
                    states.Add(state);
                }
            }
            return Json(new { states }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Zone()
        {
            List<Zone> zones = new List<Zone>();
            dict.Clear();
            dict.Add("@mode", "Zone");
            global_dt = commonClass.return_datatable(dict, "procApi");
            int j = global_dt.Rows.Count;
            if (j > 0)
            {
                for (int i = 0; i < j; i++)
                {
                    Zone zone = new Zone();
                    zone.id = Convert.ToInt32(global_dt.Rows[i]["Id"]);
                    zone.zone = Convert.ToString(global_dt.Rows[i]["zone"]);
                    zone.state_id = Convert.ToString(global_dt.Rows[i]["State_id"]);
                    zones.Add(zone);
                }
            }
            return Json(new { zones }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Area()
        {
            List<Areas> areas = new List<Areas>();
            dict.Clear();
            dict.Add("@mode", "Area");
            global_dt = commonClass.return_datatable(dict, "procApi");
            int j = global_dt.Rows.Count;
            if (j > 0)
            {
                for (int i = 0; i < j; i++)
                {
                    Areas area = new Areas();
                    area.Id = Convert.ToInt32(global_dt.Rows[i]["Id"]);
                    area.area = Convert.ToString(global_dt.Rows[i]["Area_name"]);
                    area.zone_id = Convert.ToString(global_dt.Rows[i]["Zone_id"]);
                    areas.Add(area);
                }
            }
            return Json(new { areas }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CategoryItem()
        {
            List<Banner> banners = new List<Banner>();
            dict.Clear();
            dict.Add("@mode", "CategoyItem");
            global_dt = commonClass.return_datatable(dict, "procApi");
            int j = global_dt.Rows.Count;
            if (j > 0)
            {
                for (int i = 0; i < j; i++)
                {
                    Banner banner = new Banner();
                    banner.Id = Convert.ToInt32(global_dt.Rows[i]["Id"]);
                    banner.banner = Convert.ToString(global_dt.Rows[i]["CategoryName"]);
                    banner.bannerpath = Convert.ToString(global_dt.Rows[i]["CategoryPath"]);
                    banners.Add(banner);
                }
            }
            return Json(new { banners }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult InsertMultipleAddress(string model)
        //{
        //    var arr = JArray.Parse(model);
        //    foreach (JObject jSale in arr)
        //    {
        //        if (jSale.Count > 0)
        //        {
        //            XDocument xdoc1 = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
        //            XElement productsRoot = new XElement("products");
        //            xdoc1.Add(productsRoot);

        //            JArray d = JArray.Parse(jSale.GetValue("items").ToString());
        //            foreach (JObject item in d)
        //            {
        //                XElement xRoot2 = new XElement("product");
        //                productsRoot.Add(xRoot2);
        //                xRoot2.Add(new XElement("product", Convert.ToInt32(item.GetValue("itemId"))));
        //                xRoot2.Add(new XElement("pieces", Convert.ToInt32(item.GetValue("order"))));
        //                xRoot2.Add(new XElement("cost", Convert.ToInt32(item.GetValue("cost"))));
        //            }

        //        }
        //    }
        //    sqlparam.Add(new SqlParameter("products", xdoc1.ToString()));
        //    dict.Clear();
        //}

    }
}