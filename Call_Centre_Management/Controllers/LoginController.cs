using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Call_Centre_Management.Classes;
using Call_Centre_Management.Models;
using ClosedXML.Excel;

namespace Call_Centre_Management.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        Common_Class common_Class = new Common_Class();
        Dictionary<string, object> dict = new Dictionary<string, object>();
        int empid = 0;
        //MenuMaster menu = new MenuMaster();
        public ActionResult Index()
        {
            return View();
        }
        //OrderSaleOfMonth
        public ActionResult Emp_Login(string Username, string Password)
        {
            StringBuilder SB = new StringBuilder();
            string[] s = Username.Split(' ');
            
            foreach (string n in s)
            {
                string Fi = n.First().ToString().ToUpper() + String.Join("", n.Skip(1)).ToLower() + " ";
                SB.Append(Fi);
            }

            if (Username != null && Password != null)
            {
                Password = common_Class.Encode(Password);
                Session["user_name"] = Username.ToString().Trim();
                //Session["Passsword"] = Password.ToString();
                dict.Clear();
                dict.Add("@UserName", Convert.ToString(SB));
                dict.Add("@Password", Password);
                dict.Add("@mode", "Emp_login");
            }
            DataSet dt = common_Class.return_dataset(dict, "proc_employee");
            List<MenuMaster> menu_list = new List<MenuMaster>();
            menu_list.Clear();
            if (dt.Tables.Count > 0)
            {
                int j = Convert.ToInt32(dt.Tables[0].Rows.Count);
                if (j > 0)
                {
                    for (int i = 0; i <= j - 1; i++)
                    {
                        MenuMaster menu = new MenuMaster();
                        menu.NodeID = Convert.ToInt32(dt.Tables[0].Rows[i]["nodeId"].ToString());
                        menu.ParentID = Convert.ToInt32(dt.Tables[0].Rows[i]["parentId"].ToString());
                        menu.NodeName = dt.Tables[0].Rows[i]["nodeName"].ToString();
                        menu.destination = dt.Tables[0].Rows[i]["destination"].ToString();
                        menu_list.Add(menu);
                    }
                }
                if (dt.Tables[1].Rows.Count > 0)
                {
                    empid = Convert.ToInt32(dt.Tables[1].Rows[0][0].ToString());
                    Session["Empid"] = dt.Tables[1].Rows[0][0].ToString();
                }
                ViewBag.MenuList = menu_list;
                Session["Permission"] = menu_list;
                return View(menu_list);
            }

            else if (dt.Tables.Count == 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Employee_Login", "Login");
            }
        }
        public ActionResult Employee_Login()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult logout()
        {
            if (Session["user_name"] != null && Session["Passsword"] != null)
            {
                Session["user_name"] = null;
                Session["Passsword"] = null;
                Session.Contents.RemoveAll();
                Session.Abandon();
                Response.Cookies.Clear();
                Session.Clear();
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMilliseconds(-1);
            }
            return RedirectToAction("Employee_Login", "Login");
        }
        // lucky ka kaam
        [HttpGet]
        public ActionResult AddRole()
        {
            List<MasterRole> RolesList = new List<MasterRole>();
            List<StateMasterModal> StateList = new List<StateMasterModal>();
            List<ZoneMastermodal> ZoneList = new List<ZoneMastermodal>();
            List<DropdownLit> DropList = new List<DropdownLit>();
            dict.Clear();
            dict.Add("@mode", "GetRoleRecord");
            DataSet dt = common_Class.return_dataset(dict, "Proc_role");
            int r = Convert.ToInt32(dt.Tables.Count);
            if (r > 0)
            {
                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {
                    MasterRole master = new MasterRole();
                    master.Id = Convert.ToInt32(dt.Tables[0].Rows[i]["id"]);
                    master.RoleName = dt.Tables[0].Rows[i]["Role"].ToString();
                    master.active = Convert.ToInt32(dt.Tables[0].Rows[i]["Active"]).ToString() == "1" ? "Active" : "Deactive";
                    master.InsertedDate = dt.Tables[0].Rows[i]["inserted_on"].ToString();
                    RolesList.Add(master);
                }
                for (int j = 0; j < dt.Tables[1].Rows.Count; j++)
                {
                    StateMasterModal state = new StateMasterModal();
                    state.Id = Convert.ToInt32(dt.Tables[1].Rows[j]["id"]);
                    state.StateName = dt.Tables[1].Rows[j]["state"].ToString();
                    state.Active = Convert.ToInt32(dt.Tables[1].Rows[j]["Active"]).ToString() == "1" ? "Active" : "Deactive";
                    StateList.Add(state);
                }
                for (int k = 0; k < dt.Tables[2].Rows.Count; k++)
                {
                    ZoneMastermodal zone = new ZoneMastermodal();
                    zone.Id = Convert.ToInt32(dt.Tables[2].Rows[k]["id"]);
                    zone.StateId = Convert.ToInt32(dt.Tables[2].Rows[k]["State_id"]);
                    zone.ZoneName = dt.Tables[2].Rows[k]["Zone"].ToString();
                    zone.StateName = dt.Tables[2].Rows[k]["state"].ToString();
                    zone.Active = Convert.ToInt32(dt.Tables[2].Rows[k]["active"]).ToString() == "1" ? "Active" : "Deactive";
                    ZoneList.Add(zone);
                }
                for (int l = 0; l < dt.Tables[3].Rows.Count; l++)
                {
                    DropdownLit DDLState = new DropdownLit();
                    DDLState.Id = Convert.ToInt32(dt.Tables[3].Rows[l]["id"]);
                    DDLState.DropName = dt.Tables[3].Rows[l]["state"].ToString();
                    DropList.Add(DDLState);
                }
                ViewBag.DropListState = DropList;
                ViewBag.GetZoneList = ZoneList;
                ViewBag.GetRoleList = RolesList;
                ViewBag.GetStateList = StateList;
            }
            return View();
        }
        [HttpPost]
        public ActionResult InsertRole(MasterRole model)
        {
            dict.Clear();
            dict.Add("@Role", model.RoleName);
            dict.Add("@mode", "InsertRole");
            int i = common_Class.return_nonquery(dict, "Proc_role");
            var Result = i > 0 ? Convert.ToBoolean(true).ToString() : Convert.ToBoolean(false).ToString();
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditRole(MasterRole model)
        {
            dict.Clear();
            dict.Add("@id", model.Id);
            if (model.Id > 0 && model.RoleName != null)
            {
                dict.Add("@Role", model.RoleName);
                dict.Add("@mode", "UpdateRole");
                int i = common_Class.return_nonquery(dict, "Proc_role");
                var Result = i > 0 ? Convert.ToBoolean(true).ToString() : Convert.ToBoolean(false).ToString();
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            else if (model.Id > 0 && model.RoleName == null)
            {
                dict.Add("@mode", "RoleActive/Diactivate");
                DataTable dt = common_Class.return_datatable(dict, "Proc_role");
                int Result2 = Convert.ToInt32(dt.Rows[0][0].ToString());
                return Json(Result2, JsonRequestBehavior.AllowGet);
            }
            return Json(3, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult TotalOrder()
        {
            string OrderCount = "";
            dict.Clear();
            dict.Add("", "");
            dict.Add("", "");
            DataTable dt = common_Class.return_datatable(dict, "");
            if (dt.Rows.Count > 0)
            {
                OrderCount = dt.Rows[0][0].ToString();
            }
            return Json(OrderCount, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult OrderSaleOfMonth(int id)
        {
            empClass empClass = new empClass();
            List<OrderSaleOfMonth> listSale = new List<OrderSaleOfMonth>();
            Session["Role"] = empClass.check_role(Convert.ToString(Session["Empid"]));
            dict.Clear();
            dict.Add("@Caller_id", Convert.ToInt32(Session["Empid"]));
            dict.Add("@State_id", id);
            dict.Add("@mode", "OrderSaleOfMonth");
            DataTable dt = common_Class.return_datatable(dict, "proc_order");

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    OrderSaleOfMonth sale = new OrderSaleOfMonth();
                    sale.Sale = dt.Rows[i]["Sale"].ToString();
                    sale.Zone = dt.Rows[i]["zone"].ToString();
                    sale.y = Convert.ToInt32(dt.Rows[i]["Sale"].ToString());
                    sale.name = dt.Rows[i]["zone"].ToString();
                    listSale.Add(sale);
                }
            }
            return Json(listSale, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult OrderStatusOfMonth(int id)
        {
            empClass empClass = new empClass();
            List<OrderSaleOfMonth> listSale = new List<OrderSaleOfMonth>();
            Session["Role"] = empClass.check_role(Convert.ToString(Session["Empid"]));
            dict.Clear();
            dict.Add("@Caller_id", Convert.ToInt32(Session["Empid"]));
            dict.Add("@State_id", id);
            dict.Add("@mode", "OrderStatusOfMonth");
            DataTable dt = common_Class.return_datatable(dict, "proc_order");

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    OrderSaleOfMonth sale = new OrderSaleOfMonth();
                    sale.Sale = dt.Rows[i]["totalOrder"].ToString();
                    sale.Zone = dt.Rows[i]["Status"].ToString();

                    sale.y = Convert.ToInt32(dt.Rows[i]["totalOrder"].ToString());
                    sale.name = dt.Rows[i]["Status"].ToString();
                    listSale.Add(sale);
                }
            }
            return Json(listSale, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult downloadExelMonth()
        {
            dict.Clear();
            dict.Add("@mode", "downloadExelMonth");
            DataTable dt = common_Class.return_datatable(dict, "proc_order");
            if (dt.Rows.Count > 0)
            {
                TempData["ExcelFileName"] = "Month Orders";
                dt.TableName = "Month Order";
                exportExel(dt);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public void downloadExelToday()
        {
            dict.Clear();
            dict.Add("@mode", "downloadExelToday");
            DataTable dt = common_Class.return_datatable(dict, "proc_order");
            if (dt.Rows.Count > 0)
            {
                TempData["ExcelFileName"] = "Today Orders";
                dt.TableName = "Today Order";
                exportExel(dt);
            }
        }
        public void exportExel(DataTable dt)
        {
            string GetDate = DateTime.Now.ToString("dd MMM yyyy HH:mm:ss tt");
            //("MM/dd/yyyy HH:mm:ss tt");//DataSet ds = Session["Order_details"] as DataSet;//if (ds == null)//{//    return;//}

            try
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(dt);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = false;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + TempData["ExcelFileName"] + '(' + GetDate + ')' + ".xlsx");
                MemoryStream MyMemoryStream = new MemoryStream();
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                TempData["ExcelFileName"] = null;
            }
            catch (Exception ex) { string e = ex.ToString(); }
        }

        [HttpPost]
        public ActionResult loadddStateTotalSale()
        {
            dict.Clear();
            dict.Add("@mode", "bindAllfilltersOnload");
            //dict.Add("", "");
            DataTable dt = common_Class.return_datatable(dict, "order_searching");
            List<StateMasterModal> StateList = new List<StateMasterModal>();
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    StateMasterModal state = new StateMasterModal();
                    state.Id = Convert.ToInt32(dr["id"]);
                    state.StateName = dr["state"].ToString();
                    StateList.Add(state);
                }
            }
            return Json(StateList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FetchDashboard()
        {
            Get_permission(23);
            string str = "";
            if (Convert.ToInt32(Session["Role"]) == 3)
            {
                str = "/callerLeads/CallerLeads";
            }
            if (Convert.ToInt32(Session["Role"]) == 5)
            {
                str = "/Employee/DeliveryDashboard";
            }
            else
            {
                str = "../Login/Login";
            }

            return Json(str, JsonRequestBehavior.AllowGet);
        }
        public void Get_permission(int Nodeid)
        {
            empClass e = new empClass();
            //string str1 = e.check_role(Convert.ToString(Session["Empid"].ToString()));
            Session["Role"] = e.check_role(Convert.ToString(Session["Empid"]));
            string str1 = Convert.ToString(Session["Empid"].ToString());
            int str = Nodeid;

            Session["View"] = null;
            Session["Edit"] = null;
            Session["Insert"] = null;
            Session["Delete"] = null;

            if (Convert.ToInt32(str1) == 1)
            {
                Session["View"] = "true";
                Session["Edit"] = "true";
                Session["Insert"] = "true";
                Session["Delete"] = "true";
            }
            else
            {
                dict.Clear();
                dict.Add("@nodeId", Convert.ToInt32(Nodeid));
                dict.Add("@empId", Convert.ToInt32(Session["Empid"].ToString()));
                dict.Add("@mode", "getMenuPermission");
                DataSet ds = common_Class.return_dataset(dict, "proc_Menu");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //condition? exprIfTrue : exprIfFalse
                        Session["View"] = Convert.ToBoolean(ds.Tables[0].Rows[0]["view"]) == true ? "true" : "false";
                        Session["Insert"] = Convert.ToBoolean(ds.Tables[0].Rows[0]["Insert"]) == true ? "true" : "false";
                        Session["Edit"] = Convert.ToBoolean(ds.Tables[0].Rows[0]["edit"]) == true ? "true" : "false";
                        Session["Delete"] = Convert.ToBoolean(ds.Tables[0].Rows[0]["delete"]) == true ? "true" : "false";
                    }
                }
            }
        }

        [HttpGet]
        public JsonResult CancelOrderDashbord()
        {
            List<displayorder> GetOrderList = new List<displayorder>();
            dict.Clear();
            dict.Add("@callerId", Convert.ToInt32(Session["EmpId"]));
            dict.Add("@mode", "ShowCancelOrderDashbord");
            DataTable dt = common_Class.return_datatable(dict, "order_searching");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    displayorder ord = new displayorder();
                    ord.id = Convert.ToInt32(dr["orderId"]);
                    ord.CallerId = Convert.ToInt32(dr["Caller_id"]);
                    ord.callerName = dr["Caller_Name"].ToString();
                    ord.cust_name = dr["Name"].ToString();
                    ord.cust_mobile = dr["Mobile"].ToString();
                    ord.ItemId = Convert.ToInt32(dr["ItemId"]);
                    ord.item_name = dr["Item_Name"].ToString();
                    ord.deliveryDate = dr["deliverydate"].ToString();
                    GetOrderList.Add(ord);
                }
            }
            return Json(GetOrderList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Approvaldata()
        {
            List<OrderApproval> listPendingOrder = new List<OrderApproval>();
            List<OrderApproval> listApprovedOrder = new List<OrderApproval>();
            List<OrderApproval> listNotApprovedOrder = new List<OrderApproval>();

            dict.Clear();
            dict.Add("@mode", "CallerOrderApproval");
            dict.Add("@Callerid", Convert.ToString(Session["EmpId"]));
            DataSet ds = common_Class.return_dataset(dict, "Proc_CallerLeads");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        OrderApproval pending = new OrderApproval();
                        pending.id = Convert.ToInt32(dr["id"]);
                        pending.orderid = Convert.ToInt32(dr["OrderId"]);
                        pending.custMobile = Convert.ToString(dr["Mobile"]);
                        pending.custName = Convert.ToString(dr["CustomerName"]);
                        pending.totalAmount = Convert.ToString(dr["Total_amount"]);
                        pending.totalQty = Convert.ToString(dr["total_qty"]);
                        pending.Address = Convert.ToString(dr["address"]);
                        pending.Remark = Convert.ToString(dr["Remark"]);
                        //pending.ApprovalRemark = Convert.ToString(dr["ApprovedRemark"]);
                        pending.insertedDate = Convert.ToString(dr["InsertedDate"]);
                        listPendingOrder.Add(pending);
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        OrderApproval Approved = new OrderApproval();
                        Approved.id = Convert.ToInt32(dr["id"]);
                        Approved.orderid = Convert.ToInt32(dr["OrderId"]);
                        Approved.custMobile = Convert.ToString(dr["Mobile"]);
                        Approved.custName = Convert.ToString(dr["CustomerName"]);
                        Approved.totalAmount = Convert.ToString(dr["Total_amount"]);
                        Approved.totalQty = Convert.ToString(dr["total_qty"]);
                        Approved.Address = Convert.ToString(dr["address"]);
                        Approved.Remark = Convert.ToString(dr["Remark"]);
                        Approved.ApprovalRemark = Convert.ToString(dr["ApprovedRemark"]);
                        Approved.insertedDate = Convert.ToString(dr["InsertedDate"]);
                        listApprovedOrder.Add(Approved);
                    }
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        OrderApproval NotApproved = new OrderApproval();
                        NotApproved.id = Convert.ToInt32(dr["id"]);
                        NotApproved.orderid = Convert.ToInt32(dr["OrderId"]);
                        NotApproved.custMobile = Convert.ToString(dr["Mobile"]);
                        NotApproved.custName = Convert.ToString(dr["CustomerName"]);
                        NotApproved.totalAmount = Convert.ToString(dr["Total_amount"]);
                        NotApproved.totalQty = Convert.ToString(dr["total_qty"]);
                        NotApproved.Address = Convert.ToString(dr["address"]);
                        NotApproved.Remark = Convert.ToString(dr["Remark"]);
                        NotApproved.ApprovalRemark = Convert.ToString(dr["ApprovedRemark"]);
                        NotApproved.insertedDate = Convert.ToString(dr["InsertedDate"]);
                        listNotApprovedOrder.Add(NotApproved);
                    }
                }
            }
            return Json(new { listPendingOrder, listApprovedOrder, listNotApprovedOrder }, JsonRequestBehavior.AllowGet);
        }

        //public void Notifications()
        //{
        //    if (Convert.ToInt32(Session["EmpId"]) > 0)
        //    {
        //        Session["Notification"] = null;
        //        var Count = new empClass().Notifications(Convert.ToInt32(Session["EmpId"]));
        //        if (Count > 0)
        //        {
        //            Session["Notification"] = Count;
        //        }
        //        else
        //        {
        //            Session["Notification"] = 0;
        //        }
        //    }
        //}
    }
}
