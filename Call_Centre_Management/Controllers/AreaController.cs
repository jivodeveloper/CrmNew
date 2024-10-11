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
    public class AreaController : Controller
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        Common_Class common_class = new Common_Class();

        // Code By Nitin 
        public ActionResult Index()
        {
            List<Area> area_list = new List<Area>();
            //Add by lucky
            List<MasterRole> RolesList = new List<MasterRole>();
            List<StateMasterModal> StateList = new List<StateMasterModal>();
            List<ZoneMastermodal> ZoneList = new List<ZoneMastermodal>();
            List<DropdownLit> DropList = new List<DropdownLit>();
            //End
            dict.Clear();
            //dict.Add("","");
            dict.Add("@mode", "getAll_Area");
            //DataTable dt = common_class.return_datatable(dict, "proc_Area");
            DataSet dt = common_class.return_dataset(dict, "proc_Area");
            int j = dt.Tables.Count;
            if (j > 0)
            {
                for (int i = 0; i < dt.Tables[5].Rows.Count; i++)
                {
                    Area area = new Area();
                    area.id = Convert.ToInt32(dt.Tables[5].Rows[i]["id"].ToString());
                    area.Area_Name = dt.Tables[5].Rows[i]["Area_name"].ToString();
                    area.Zone_id = dt.Tables[5].Rows[i]["Zone"].ToString();
                    area.State_id = dt.Tables[5].Rows[i]["State"].ToString();
                    area.pincode = Convert.ToInt32(dt.Tables[5].Rows[i]["Pincode"].ToString());
                    area.active = Convert.ToBoolean(dt.Tables[5].Rows[i]["active"].ToString());
                    area_list.Add(area);
                }
                //Add by lucky
                for (int k = 0; k < dt.Tables[1].Rows.Count; k++)
                {
                    MasterRole master = new MasterRole();
                    master.Id = Convert.ToInt32(dt.Tables[1].Rows[k]["id"]);
                    master.RoleName = dt.Tables[1].Rows[k]["Role"].ToString();
                    master.active = Convert.ToInt32(dt.Tables[1].Rows[k]["Active"]).ToString() == "1" ? "Active" : "Deactive";
                    master.InsertedDate = dt.Tables[1].Rows[k]["inserted_on"].ToString();
                    RolesList.Add(master);
                }
                for (int l = 0; l < dt.Tables[2].Rows.Count; l++)
                {
                    StateMasterModal state = new StateMasterModal();
                    state.Id = Convert.ToInt32(dt.Tables[2].Rows[l]["id"]);
                    state.StateName = dt.Tables[2].Rows[l]["state"].ToString();
                    //state.Active = Convert.ToInt32(dt.Tables[2].Rows[l]["Active"]).ToString() == "1" ? "Active" : "Deactive";
                    state.Active = dt.Tables[2].Rows[l]["Active"].ToString();
                    StateList.Add(state);
                }
                for (int m = 0; m < dt.Tables[3].Rows.Count; m++)
                {
                    ZoneMastermodal zone = new ZoneMastermodal();
                    zone.Id = Convert.ToInt32(dt.Tables[3].Rows[m]["id"]);
                    zone.StateId = Convert.ToInt32(dt.Tables[3].Rows[m]["State_id"]);
                    zone.ZoneName = dt.Tables[3].Rows[m]["Zone"].ToString();
                    zone.StateName = dt.Tables[3].Rows[m]["state"].ToString();
                    //zone.Active = Convert.ToInt32(dt.Tables[3].Rows[m]["active"]).ToString() == "1" ? "Active" : "Deactive";
                    zone.Active = dt.Tables[3].Rows[m]["Active"].ToString();
                    ZoneList.Add(zone);
                }
                for (int n = 0; n < dt.Tables[4].Rows.Count; n++)
                {
                    DropdownLit DDLState = new DropdownLit();
                    DDLState.Id = Convert.ToInt32(dt.Tables[4].Rows[n]["id"]);
                    DDLState.DropName = dt.Tables[4].Rows[n]["state"].ToString();
                    DropList.Add(DDLState);
                }
                ViewBag.DropListState = DropList;
                ViewBag.GetZoneList = ZoneList;
                ViewBag.ZoneTotalCount = ZoneList.Count;
                ViewBag.GetRoleList = RolesList;
                ViewBag.RoleTotalCount = RolesList.Count;
                ViewBag.GetStateList = StateList;
                ViewBag.StateTotalCount = StateList.Count;
                //End

                ViewBag.list_Area = area_list;
                ViewBag.AreaTotalCount = area_list.Count;
            }
            return View();
        }

        [HttpPost]
        public ActionResult delete_area(int id)
        {
            dict.Clear();
            dict.Add("@id", id);
            dict.Add("@mode", "DeActivate_Area");
            var result = common_class.return_nonquery(dict, "proc_Area");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Insert_area()
        {
            #region return State and Role

            dict.Clear();
            dict.Add("@mode", "get_all_zone");
            DataSet ds = common_class.return_dataset(dict, "proc_Area");
            List<state> list_state = new List<state>();
            List<Role> list_role = new List<Role>();
            int count_zone = ds.Tables[0].Rows.Count;
            int count_state = ds.Tables[1].Rows.Count;



            if (count_state > 0)
            {
                for (int i = 0; i < count_state; i++)
                {
                    state s = new state();
                    s.Id = Convert.ToInt32(ds.Tables[1].Rows[i]["id"].ToString());
                    s.State = ds.Tables[1].Rows[i]["state"].ToString();
                    list_state.Add(s);
                }
                if (count_zone > 0)
                {
                    for (int i = 0; i < count_zone; i++)
                    {
                        Role r = new Role();
                        r.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString());
                        r.role = ds.Tables[0].Rows[i]["Zone"].ToString();
                        list_role.Add(r);
                    }

                }
                ViewBag.getzone = list_role;
                ViewBag.getstate = list_state;
            }
            #endregion
            return View();
        }

        [HttpPost]
        public ActionResult Insert_area(Area area)
        {
            //area = new Area();
            dict.Clear();
            dict.Add("@Area_name", area.Area_Name);
            dict.Add("@zone_id", Convert.ToInt32(area.Zone_id));
           // dict.Add("@State_id", Convert.ToInt32(area.State_id));
            dict.Add("@Pincode", area.pincode);
            dict.Add("@User", Convert.ToString(Session["user_name"]));
            dict.Add("@mode", "Insert_Area");
            var Result = common_class.return_nonquery(dict, "proc_Area");
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        //   [HttpGet]
        public ActionResult Edit(int id)
        {
            #region return State and Role

            dict.Clear();
            dict.Add("@mode", "get_all_zone");
            DataSet ds = common_class.return_dataset(dict, "proc_Area");
            List<state> list_state = new List<state>();
            List<Role> list_role = new List<Role>();
            int count_zone = ds.Tables[0].Rows.Count;
            int count_state = ds.Tables[1].Rows.Count;



            if (count_state > 0)
            {
                for (int i = 0; i < count_state; i++)
                {
                    state s = new state();
                    s.Id = Convert.ToInt32(ds.Tables[1].Rows[i]["id"].ToString());
                    s.State = ds.Tables[1].Rows[i]["state"].ToString();
                    list_state.Add(s);
                }
                if (count_zone > 0)
                {
                    for (int i = 0; i < count_zone; i++)
                    {
                        Role r = new Role();
                        r.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString());
                        r.role = ds.Tables[0].Rows[i]["Zone"].ToString();
                        list_role.Add(r);
                    }

                }
                ViewBag.getzone = list_role;
                ViewBag.getstate = list_state;
            }
            #endregion

            //id = 1; //
            dict.Clear();
            dict.Add("@id", id);
            dict.Add("@mode", "getAll_Area_ByID");
            DataTable dt = common_class.return_datatable(dict, "proc_Area");
            Area area = new Area();
            if (dt.Rows.Count > 0)
            {
                area.id = Convert.ToInt32(dt.Rows[0]["id"].ToString());
                area.Area_Name = dt.Rows[0]["Area_name"].ToString();
                area.Zone_id = dt.Rows[0]["ZoneId"].ToString();
                area.State_id = dt.Rows[0]["stateId"].ToString();
                area.pincode = Convert.ToInt32(dt.Rows[0]["Pincode"].ToString());
            }
            ViewBag.Edit_Area = area;
            return View(area);
        }

        [HttpPost]
        public ActionResult Edit(Area area)
        {
            //area = new Area();
            dict.Clear();
            dict.Add("@id", area.id);
            dict.Add("@Area_name", area.Area_Name);
            dict.Add("@zone_id", area.Zone_id);
          // dict.Add("@State_id", area.State_id);
            dict.Add("@Pincode", area.pincode);
            dict.Add("@User", Convert.ToString(Session["user_name"]));
            dict.Add("@mode", "Update_Area");
            var result = common_class.return_nonquery(dict, "proc_Area");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertState(StateMasterModal model)
        {
            dict.Clear();
            dict.Add("@State_Name", model.StateName);
            dict.Add("@mode", "InsertState");
            int i = common_class.return_nonquery(dict, "proc_Area");
            var Result = i > 0 ? Convert.ToBoolean(true).ToString() : Convert.ToBoolean(false).ToString();
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateState(StateMasterModal model)
        {
            dict.Clear();
            dict.Add("@State_id", model.Id);
            if (model.Id > 0 && model.StateName != null)
            {
                dict.Add("@State_Name", model.StateName);
                dict.Add("@mode", "UpdateState");
                int i = common_class.return_nonquery(dict, "proc_Area");
                bool Result1 = i > 0 ? true : false;
                return Json(Result1, JsonRequestBehavior.AllowGet);
            }
            else if (model.Id > 0 && model.StateName == null)
            {
                dict.Add("@mode", "StateRoleActive/Diactivate");
                DataTable dt = common_class.return_datatable(dict, "proc_Area");
                int Result2 = Convert.ToInt32(dt.Rows[0][0].ToString());
                return Json(Result2, JsonRequestBehavior.AllowGet);
            }
            return Json(3, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertZoneDetails(ZoneMastermodal modal)
        {
            dict.Clear();
            dict.Add("@State_id", modal.StateId);
            dict.Add("@zone_Name", modal.ZoneName);
            dict.Add("@mode", "InsertZone");
            int i = common_class.return_nonquery(dict, "proc_Area");
            bool Result = i > 0 ? true : false;
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateZone(ZoneMastermodal model)
        {
            dict.Clear();
            dict.Add("@id", model.Id);
            if (model.Id > 0 && model.StateId != null && model.ZoneName != null)
            {
                dict.Add("@State_id", model.StateId);
                dict.Add("@zone_Name", model.ZoneName);
                dict.Add("@mode", "UpadateZone");
                int i = common_class.return_nonquery(dict, "proc_Area");
                bool Result1 = i > 0 ? true : false;
                return Json(Result1, JsonRequestBehavior.AllowGet);
            }
            else if (model.Id > 0 && model.StateId == null && model.ZoneName == null)
            {
                dict.Add("@mode", "ZoneRoleActive/Diactivate");
                DataTable dt = common_class.return_datatable(dict, "proc_Area");
                int Result2 = Convert.ToInt32(dt.Rows[0][0].ToString());
                return Json(Result2, JsonRequestBehavior.AllowGet);
            }
            return Json(3, JsonRequestBehavior.AllowGet);
        }
    }
}