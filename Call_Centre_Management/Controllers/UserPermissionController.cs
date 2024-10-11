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
    public class UserPermissionController : Controller
    {
        Common_Class commonClass = new Common_Class();
        Dictionary<string, object> dict = new Dictionary<string, object>();
        // GET: UserPermission
        public ActionResult Index()
        {
            //if (Session["user_name"] == null || Session["user_name"].ToString() == "")
            //{
            //    return RedirectToAction("Employee_Login", "Login");
            //}
            return View();
        }
        [HttpPost]
        public ActionResult getMenu(int id)
        {
            List<permission> permission_list = new List<permission>();
            dict.Clear();
            dict.Add("@mode", "Get_All_Menu");
            dict.Add("@empId", id);
            DataSet ds = commonClass.return_dataset(dict, "proc_Menu");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i< ds.Tables[0].Rows.Count; i++)
                    {
                        permission permission = new permission();
                        permission.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"].ToString());
                        permission.nodeId = Convert.ToInt32(ds.Tables[0].Rows[i]["nodeId"].ToString());                        
                        permission.nodeName = ds.Tables[0].Rows[i]["nodeName"].ToString();
                        permission.destination = ds.Tables[0].Rows[i]["destination"].ToString();
                        permission.active = Convert.ToBoolean(ds.Tables[0].Rows[i]["pagePermission"]);
                        permission.view = Convert.ToBoolean(ds.Tables[0].Rows[i]["view"]);
                        permission.insert = Convert.ToBoolean(ds.Tables[0].Rows[i]["insert"]);
                        permission.edit = Convert.ToBoolean(ds.Tables[0].Rows[i]["edit"]);
                        permission.delete = Convert.ToBoolean(ds.Tables[0].Rows[i]["delete"]);
                        permission_list.Add(permission);
                    }
                }
            }
           return Json(permission_list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult LoadDD()
        {
            Area_CommonClass areaClass = new Area_CommonClass();
            dict.Clear();
            dict.Add("@mode", "Get_all_Emp");
            var Emp = new SelectList(areaClass.BindDropDown("fullName", "Id", "proc_employee", dict, "Select Employee"), "value", "text");
            return Json(Emp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertPermission(List<permission> permissions,int empid)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("empid");
            dt.Columns.Add("nodeid");            
            dt.Columns.Add("insert");
            dt.Columns.Add("view");
            dt.Columns.Add("edit");
            dt.Columns.Add("delete");
            dt.Columns.Add("active");
            dt.TableName = "tblPermission";
            foreach(var p in permissions)
            {
                dt.Rows.Add(empid,p.nodeId, p.insert,p.view, p.edit,p.delete,p.active );
            }
            string strXML = commonClass.GetXmlDoc(dt);

            dict.Add("@doc", strXML);
            dict.Add("@empid", empid);
            dict.Add("@mode", "InsertEmpPermission");          
            var Result = commonClass.return_nonquery(dict, "proc_Menu");                     
            
            return Json(Result,JsonRequestBehavior.AllowGet);     
        }


        [HttpGet]
        public ActionResult Menu()
        {
            List<MenuMaster> menuListForDD = new List<MenuMaster>();
            List<MenuMaster> menuListForTable = new List<MenuMaster>();
            dict.Clear();
            dict.Add("@mode", "getAllMenu");
            DataSet dt = commonClass.return_dataset(dict, "proc_Menu");
            if (dt.Tables.Count > 0)
            {
                if (dt.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                    {
                        MenuMaster m = new MenuMaster();
                        m.NodeID = Convert.ToInt32(dt.Tables[0].Rows[i]["nodeId"].ToString());
                        m.NodeName = dt.Tables[0].Rows[i]["nodeName"].ToString();
                        m.destination = dt.Tables[0].Rows[i]["destination"].ToString();
                        menuListForDD.Add(m);
                    }
                }
                if (dt.Tables[1].Rows.Count > 1)
                {
                    for (int i = 0; i < dt.Tables[1].Rows.Count; i++)
                    {
                        MenuMaster m = new MenuMaster();
                        m.NodeID = Convert.ToInt32(dt.Tables[1].Rows[i]["nodeId"].ToString());
                        m.NodeName = dt.Tables[1].Rows[i]["nodeName"].ToString();
                        m.parentName = dt.Tables[1].Rows[i]["ParentName"].ToString();
                        m.destination = dt.Tables[1].Rows[i]["destination"].ToString();
                        //m.Status = dt.Tables[1].Rows[i]["active"].ToString(); == "true" ? ""'<span style="color:red">DeActive</span>'"" : "<span style='color: green'>Active</span>";

                        m.Status = dt.Tables[1].Rows[i]["active"].ToString();
                        menuListForTable.Add(m);
                        //nodeId nodeName    ParentName destination active(No column name)
                    }
                }
            }
            ViewBag.ddMenuForDD = menuListForDD;
            ViewBag.menuTable = menuListForTable;
            //return Json(menulist, JsonRequestBehavior.AllowGet);
            return View();
        }

        [HttpPost]
        public ActionResult Submit(MenuMaster menu)
        {
            dict.Clear();
            dict.Add("@parentid", menu.ParentID);
            dict.Add("@nodeName", menu.NodeName);
            dict.Add("@destination", menu.destination);
            dict.Add("@mode", "insertNode");
            var i = commonClass.return_nonquery(dict, "proc_Menu");
            return Json(i, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int i)
        {
            dict.Clear();
            dict.Add("@nodeId", i);
            dict.Add("@mode", "Active/Deactivate");
            var Result = commonClass.return_nonquery(dict, "proc_Menu");
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Edit(int i)
        {
            dict.Clear();
            dict.Add("@nodeId", i);
            dict.Add("@mode", "Edit");
            DataSet dt = commonClass.return_dataset(dict, "proc_Menu");
            //  List<MenuMaster> listmenu = new List<MenuMaster>();
            MenuMaster menu = new MenuMaster();

            if (dt.Tables.Count > 0)
            {
                if (dt.Tables[0].Rows.Count > 0)
                {
                    Session["NodeId"] = dt.Tables[0].Rows[0]["nodeId"].ToString();
                    menu.NodeName = dt.Tables[0].Rows[0]["nodeName"].ToString();
                    menu.destination = dt.Tables[0].Rows[0]["destination"].ToString();
                    menu.ParentID = Convert.ToInt32(dt.Tables[0].Rows[0]["parentId"].ToString());
                    //  listmenu.Add(menu);
                }
                if (dt.Tables[1].Rows.Count > 0)
                {
                    for (int j = 0; j < dt.Tables[1].Rows.Count; j++)
                    {
                        MenuMaster m = new MenuMaster();
                        m.NodeID = Convert.ToInt32(dt.Tables[1].Rows[j]["nodeId"].ToString());
                        m.NodeName = dt.Tables[1].Rows[j]["nodeName"].ToString();
                        m.destination = dt.Tables[1].Rows[j]["destination"].ToString();
                        menu.menus.Add(m);
                    }
                }
            }
            return Json(menu, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(MenuMaster menu)
        {
            dict.Clear();
            dict.Add("@nodeId", Convert.ToInt32(Session["NodeId"]));
            dict.Add("@parentid", menu.ParentID);
            dict.Add("@nodeName", menu.NodeName);
            dict.Add("@destination", menu.destination);
            dict.Add("@mode", "UpdateNode");
            var i = commonClass.return_nonquery(dict, "proc_Menu");
            if (i > 0)
            {
                Session["NodeId"] = null;
            }
            return Json(i, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult checkNodeName(string nodeName)
        {
            dict.Clear();
            dict.Add("@nodeName", nodeName);
            dict.Add("@mode", "CheckNodename");
            DataTable dt = commonClass.return_datatable(dict, "proc_Menu");
            if (dt.Rows.Count > 0)
            {
                nodeName = "true";
            }
            return Json(nodeName, JsonRequestBehavior.AllowGet);
        }

    }
}