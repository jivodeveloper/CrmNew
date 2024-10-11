using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Call_Centre_Management.Classes;
using System.Web.Mvc;

namespace Call_Centre_Management.Classes
{
    public class CommonController : Controller
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        public Area_CommonClass areaClass = new Area_CommonClass();
        // GET: Common
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetStates()
        {
            Area_CommonClass areaClass = new Area_CommonClass();
           
            dict.Clear();
            dict.Add("@mode","State");
            var x = new SelectList(areaClass.BindDropDown("state", "stateId", "GetAllStatesByLoginUser", dict, "STATE"), "value", "text");
            return Json(x, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetZones(string selectedValue)
        {
            dict.Clear();
            dict.Add("@mode", "bind_zoneOnStateId");
            dict.Add("@stateid", selectedValue);
            var x = new SelectList(areaClass.BindDropDown("zone", "id", "proc_common", dict, "ZONE"), "value", "text");
            return Json(x, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetArea(string selectedValue)
        {
            dict.Clear();
            dict.Add("@mode", "bind_areaOnZoneId");
            dict.Add("@zoneid", selectedValue);
            var x = new SelectList(areaClass.BindDropDown("Area_name", "id", "proc_common", dict, "AREA"), "value", "text");
            return Json(x, JsonRequestBehavior.AllowGet);
        }
    }
}