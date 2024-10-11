using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Call_Centre_Management.Models;
using Call_Centre_Management.Classes;
using System.Data;

namespace Call_Centre_Management.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        Common_Class commonclass = new Common_Class();
        Dictionary<string, object> dict = new Dictionary<string, object>();
        empClass empClass = new empClass();

        [HttpGet]
        public ActionResult Index()
        {
            //Session["Role"] = empClass.check_role(Convert.ToString(Session["Empid"]));
            #region Load State Zone Area
            dict.Clear();
            dict.Add("@mode", "get_all_states/Zone/Area/orderDetails");
            dict.Add("@Callerid", Convert.ToString(Session["Empid"]));
            DataSet ds = commonclass.return_dataset(dict, "proc_employee");

            List<state> list_state = new List<state>();
            List<ZoneMastermodal> list_zone = new List<ZoneMastermodal>();
            List<Area> list_Area = new List<Area>();
            List<order> list_order = new List<order>();
            int Count_State = ds.Tables[0].Rows.Count;
            int Count_Zone = ds.Tables[1].Rows.Count;
            int Count_Area = ds.Tables[2].Rows.Count;
            int count_Order = ds.Tables[3].Rows.Count;


            if (Count_State > 0)
            {
                for (int i = 0; i < Count_State; i++)
                {
                    state s = new state();
                    s.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString());
                    s.State = ds.Tables[0].Rows[i]["state"].ToString();
                    list_state.Add(s);
                }
                //if (Count_Zone >0)
                if (Count_Zone == -1)
                {
                    for (int i = 0; i < Count_Zone; i++)
                    {
                        ZoneMastermodal z = new ZoneMastermodal();
                        z.Id = Convert.ToInt32(ds.Tables[1].Rows[i]["id"].ToString());
                        z.ZoneName = ds.Tables[1].Rows[i]["Zone"].ToString();
                        list_zone.Add(z);
                    }
                    //if (Count_Area > 0)
                    if (Count_Area == -1)
                    {
                        for (int i = 0; i < Count_Area; i++)
                        {
                            Area a = new Area();
                            a.id = Convert.ToInt32(ds.Tables[2].Rows[i]["id"].ToString());
                            a.Area_Name = ds.Tables[2].Rows[i]["Area_name"].ToString();
                            list_Area.Add(a);
                        }
                    }
                }

                #endregion
            }

            if (count_Order > 0)
            {
                for (int y = 0; y < count_Order; y++)
                {
                    //custumerName,custumerMobile,Total_amount,total_qty,Payment_mode,
                    //Payment_Remark,Remark,landmark,Area_name,Zone,state,Pincode,Address
                    order odr = new order();
                    odr.id = Convert.ToInt32(ds.Tables[3].Rows[y]["id"].ToString());
                    odr.cust_name = ds.Tables[3].Rows[y]["custumerName"].ToString();
                    odr.cust_mobile = ds.Tables[3].Rows[y]["custumerMobile"].ToString();
                    odr.Total_Price = Convert.ToDouble(ds.Tables[3].Rows[y]["Total_amount"].ToString());
                    odr.total_qty = Convert.ToInt32(ds.Tables[3].Rows[y]["total_qty"].ToString());
                    odr.Payment_mode = ds.Tables[3].Rows[y]["Payment_mode"].ToString();
                    odr.payment_remark = ds.Tables[3].Rows[y]["Payment_Remark"].ToString();
                    odr.Remark = ds.Tables[3].Rows[y]["Remark"].ToString();
                    odr.landmark = ds.Tables[3].Rows[y]["landmark"].ToString();
                    odr.AreaName = ds.Tables[3].Rows[y]["Area_name"].ToString();
                    odr.zoneName = ds.Tables[3].Rows[y]["Zone"].ToString();
                    odr.stateName = ds.Tables[3].Rows[y]["state"].ToString();
                    odr.pincode = Convert.ToInt32(ds.Tables[3].Rows[y]["Pincode"].ToString());
                    odr.Address = ds.Tables[3].Rows[y]["Address"].ToString();
                    odr.CallerId = Convert.ToInt32(ds.Tables[3].Rows[y]["Caller_id"].ToString());
                    odr.callerName = ds.Tables[3].Rows[y]["Caller_Name"].ToString();
                    odr.Source = ds.Tables[3].Rows[y]["Source"].ToString();
                    odr.insertedDate = ds.Tables[3].Rows[y]["Inserted_date"].ToString();
                    list_order.Add(odr);
                }
            }
            ViewBag.getstate = list_state;
            ViewBag.getZone = list_zone;
            ViewBag.getArea = list_Area;
            ViewBag.getOrder = list_order;

            return View();
        }

        public ActionResult Index1()
        {
            //Session["Role"] = empClass.check_role(Convert.ToString(Session["Empid"]));
            #region Load State Zone Area
            dict.Clear();
            dict.Add("@mode", "get_all_states/Zone/Area/orderDetails");
            dict.Add("@Callerid", Convert.ToString(Session["Empid"]));
            DataSet ds = commonclass.return_dataset(dict, "proc_employee");

            List<state> list_state = new List<state>();
            List<ZoneMastermodal> list_zone = new List<ZoneMastermodal>();
            List<Area> list_Area = new List<Area>();
            List<order> list_order = new List<order>();
            int Count_State = ds.Tables[0].Rows.Count;
            int Count_Zone = ds.Tables[1].Rows.Count;
            int Count_Area = ds.Tables[2].Rows.Count;
            int count_Order = ds.Tables[3].Rows.Count;


            if (Count_State > 0)
            {
                for (int i = 0; i < Count_State; i++)
                {
                    state s = new state();
                    s.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString());
                    s.State = ds.Tables[0].Rows[i]["state"].ToString();
                    list_state.Add(s);
                }
                //if (Count_Zone >0)
                if (Count_Zone == -1)
                {
                    for (int i = 0; i < Count_Zone; i++)
                    {
                        ZoneMastermodal z = new ZoneMastermodal();
                        z.Id = Convert.ToInt32(ds.Tables[1].Rows[i]["id"].ToString());
                        z.ZoneName = ds.Tables[1].Rows[i]["Zone"].ToString();
                        list_zone.Add(z);
                    }
                    //if (Count_Area > 0)
                    if (Count_Area == -1)
                    {
                        for (int i = 0; i < Count_Area; i++)
                        {
                            Area a = new Area();
                            a.id = Convert.ToInt32(ds.Tables[2].Rows[i]["id"].ToString());
                            a.Area_Name = ds.Tables[2].Rows[i]["Area_name"].ToString();
                            list_Area.Add(a);
                        }
                    }
                }

                #endregion
            }

            if (count_Order > 0)
            {
                for (int y = 0; y < count_Order; y++)
                {
                    //custumerName,custumerMobile,Total_amount,total_qty,Payment_mode,
                    //Payment_Remark,Remark,landmark,Area_name,Zone,state,Pincode,Address
                    order odr = new order();
                    odr.id = Convert.ToInt32(ds.Tables[3].Rows[y]["id"].ToString());
                    odr.cust_name = ds.Tables[3].Rows[y]["custumerName"].ToString();
                    odr.cust_mobile = ds.Tables[3].Rows[y]["custumerMobile"].ToString();
                    odr.Total_Price = Convert.ToDouble(ds.Tables[3].Rows[y]["Total_amount"].ToString());
                    odr.total_qty = Convert.ToInt32(ds.Tables[3].Rows[y]["total_qty"].ToString());
                    odr.Payment_mode = ds.Tables[3].Rows[y]["Payment_mode"].ToString();
                    odr.payment_remark = ds.Tables[3].Rows[y]["Payment_Remark"].ToString();
                    odr.Remark = ds.Tables[3].Rows[y]["Remark"].ToString();
                    odr.landmark = ds.Tables[3].Rows[y]["landmark"].ToString();
                    odr.AreaName = ds.Tables[3].Rows[y]["Area_name"].ToString();
                    odr.zoneName = ds.Tables[3].Rows[y]["Zone"].ToString();
                    odr.stateName = ds.Tables[3].Rows[y]["state"].ToString();
                    odr.pincode = Convert.ToInt32(ds.Tables[3].Rows[y]["Pincode"].ToString());
                    odr.Address = ds.Tables[3].Rows[y]["Address"].ToString();
                    odr.CallerId = Convert.ToInt32(ds.Tables[3].Rows[y]["Caller_id"].ToString());
                    odr.callerName = ds.Tables[3].Rows[y]["Caller_Name"].ToString();
                    odr.Source = ds.Tables[3].Rows[y]["Source"].ToString();
                    odr.insertedDate = ds.Tables[3].Rows[y]["Inserted_date"].ToString();
                    list_order.Add(odr);
                }
            }
            ViewBag.getstate = list_state;
            ViewBag.getZone = list_zone;
            ViewBag.getArea = list_Area;
            ViewBag.getOrder = list_order;

            return View();
        }

        [HttpPost]
        public ActionResult LoadOrder()
        {
            //Session["Role"] = empClass.check_role(Convert.ToString(Session["Empid"]));
            #region Load State Zone Area
            dict.Clear();
            dict.Add("@mode", "get_all_states/Zone/Area/orderDetails");
            dict.Add("@Callerid", Convert.ToString(Session["Empid"]));
            DataSet ds = commonclass.return_dataset(dict, "proc_employee");

            List<state> list_state = new List<state>();
            List<ZoneMastermodal> list_zone = new List<ZoneMastermodal>();
            List<Area> list_Area = new List<Area>();
            List<order> list_order = new List<order>();
            int Count_State = ds.Tables[0].Rows.Count;
            int Count_Zone = ds.Tables[1].Rows.Count;
            int Count_Area = ds.Tables[2].Rows.Count;
            int count_Order = ds.Tables[3].Rows.Count;


            if (Count_State > 0)
            {
                for (int i = 0; i < Count_State; i++)
                {
                    state s = new state();
                    s.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString());
                    s.State = ds.Tables[0].Rows[i]["state"].ToString();
                    list_state.Add(s);
                }
                //if (Count_Zone >0)
                if (Count_Zone == -1)
                {
                    for (int i = 0; i < Count_Zone; i++)
                    {
                        ZoneMastermodal z = new ZoneMastermodal();
                        z.Id = Convert.ToInt32(ds.Tables[1].Rows[i]["id"].ToString());
                        z.ZoneName = ds.Tables[1].Rows[i]["Zone"].ToString();
                        list_zone.Add(z);
                    }
                    //if (Count_Area > 0)
                    if (Count_Area == -1)
                    {
                        for (int i = 0; i < Count_Area; i++)
                        {
                            Area a = new Area();
                            a.id = Convert.ToInt32(ds.Tables[2].Rows[i]["id"].ToString());
                            a.Area_Name = ds.Tables[2].Rows[i]["Area_name"].ToString();
                            list_Area.Add(a);
                        }
                    }
                }

                #endregion
            }

            if (count_Order > 0)
            {
                for (int y = 0; y < count_Order; y++)
                {
                    //custumerName,custumerMobile,Total_amount,total_qty,Payment_mode,
                    //Payment_Remark,Remark,landmark,Area_name,Zone,state,Pincode,Address
                    order odr = new order();
                    odr.id = Convert.ToInt32(ds.Tables[3].Rows[y]["id"].ToString());
                    odr.cust_name = ds.Tables[3].Rows[y]["custumerName"].ToString();
                    odr.cust_mobile = ds.Tables[3].Rows[y]["custumerMobile"].ToString();
                    odr.Total_Price = Convert.ToDouble(ds.Tables[3].Rows[y]["Total_amount"].ToString());
                    odr.total_qty = Convert.ToInt32(ds.Tables[3].Rows[y]["total_qty"].ToString());
                    odr.Payment_mode = ds.Tables[3].Rows[y]["Payment_mode"].ToString();
                    odr.payment_remark = ds.Tables[3].Rows[y]["Payment_Remark"].ToString();
                    odr.Remark = ds.Tables[3].Rows[y]["Remark"].ToString();
                    odr.landmark = ds.Tables[3].Rows[y]["landmark"].ToString();
                    odr.AreaName = ds.Tables[3].Rows[y]["Area_name"].ToString();
                    odr.zoneName = ds.Tables[3].Rows[y]["Zone"].ToString();
                    odr.stateName = ds.Tables[3].Rows[y]["state"].ToString();
                    odr.pincode = Convert.ToInt32(ds.Tables[3].Rows[y]["Pincode"].ToString());
                    odr.Address = ds.Tables[3].Rows[y]["Address"].ToString();
                    odr.CallerId = Convert.ToInt32(ds.Tables[3].Rows[y]["Caller_id"].ToString());
                    odr.callerName = ds.Tables[3].Rows[y]["Caller_Name"].ToString();
                    odr.Source = ds.Tables[3].Rows[y]["Source"].ToString();
                    odr.insertedDate = ds.Tables[3].Rows[y]["Inserted_date"].ToString();
                    list_order.Add(odr);
                }
            }
            ViewBag.getstate = list_state;
            ViewBag.getZone = list_zone;
            ViewBag.getArea = list_Area;
            ViewBag.getOrder = list_order;

            return Json(list_order, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult check_user(string mobilenumber)
        {
            dict.Clear();
            dict.Add("@customer_number", mobilenumber);
            dict.Add("@mode", "customer_details");
            DataSet dt = commonclass.return_dataset(dict, "proc_order");
            Customer cust = new Customer();
            string response = string.Empty;
            if (dt.Tables.Count > 0)
            {
                if (dt.Tables[0].Rows.Count > 0)
                {
                    cust.id = Convert.ToInt32(dt.Tables[0].Rows[0]["id"].ToString());
                    cust.name = dt.Tables[0].Rows[0]["Name"].ToString();
                    cust.Zone_id = Convert.ToInt32(dt.Tables[0].Rows[0]["Zone_id"].ToString());
                    cust.Area_id = Convert.ToInt32(dt.Tables[0].Rows[0]["Area_id"].ToString());
                    cust.State_id = Convert.ToInt32(dt.Tables[0].Rows[0]["State_id"].ToString());
                    cust.landmark = dt.Tables[0].Rows[0]["Landmark"].ToString();
                    cust.Address = dt.Tables[0].Rows[0]["Address"].ToString();
                    cust.pincode = Convert.ToInt32(dt.Tables[0].Rows[0]["Pincode"].ToString());
                }
                if (dt.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Tables[1].Rows.Count; i++)
                    {
                        CustOldItems CustOldItems = new CustOldItems();
                        CustOldItems.ItemName = dt.Tables[1].Rows[i]["Item_Name"].ToString();
                        CustOldItems.quantity = dt.Tables[1].Rows[i]["item_qty"].ToString();
                        CustOldItems.date = dt.Tables[1].Rows[i]["OrderDate"].ToString();
                        CustOldItems.Status = dt.Tables[1].Rows[i]["Status"].ToString();
                        cust.CustOldItems.Add(CustOldItems);
                    }
                }
            }
            if (dt.Tables[2].Rows.Count > 0)
            {
                response = Convert.ToString(dt.Tables[2].Rows[0][0]);
            }
            var Result = cust;
            ViewBag.cust = cust;
            return Json(new { Result, response }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult check_item(string mobilenumber)
        {
            dict.Clear();
            dict.Add("@customer_number", mobilenumber);
            dict.Add("@mode", "customer_details");
            DataTable dt = commonclass.return_datatable(dict, "proc_order");
            Customer cust = new Customer();
            if (dt.Rows.Count > 0)
            {
                cust.id = Convert.ToInt32(dt.Rows[0]["id"].ToString());
                cust.name = dt.Rows[0]["Name"].ToString();
                cust.Zone_id = Convert.ToInt32(dt.Rows[0]["Zone_id"].ToString());
                cust.Area_id = Convert.ToInt32(dt.Rows[0]["Area_id"].ToString());
                cust.State_id = Convert.ToInt32(dt.Rows[0]["State_id"].ToString());
                cust.landmark = dt.Rows[0]["Landmark"].ToString();
                cust.Address = dt.Rows[0]["Address"].ToString();
                cust.pincode = Convert.ToInt32(dt.Rows[0]["id"].ToString());
            }
            var Result = cust;
            ViewBag.cust = cust;
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult load_item(string Prefix)
        {
            dict.Clear();
            dict.Add("@item_name", Prefix);
            dict.Add("@mode", "Search_item_details");
            DataTable dt = commonclass.return_datatable(dict, "proc_order");
            var txtItems = (from DataRow row in dt.Rows
                            select row["Item_Name"].ToString()
                                into dbValues
                            select dbValues.ToLower()).ToList();
            return Json(txtItems, JsonRequestBehavior.AllowGet);
        }
        public JsonResult load_item2(string item_name)
        {
            item_Model item = new item_Model();
            dict.Clear();
            dict.Add("@item_name", item_name);
            dict.Add("@mode", "Search_item_details");
            DataTable dt = commonclass.return_datatable(dict, "proc_order");
            if (dt.Rows.Count > 0)
            {
                item.id = Convert.ToInt32(dt.Rows[0]["id"].ToString());
                item.item_name = dt.Rows[0]["Item_Name"].ToString();
                item.item_rate = Convert.ToDouble(dt.Rows[0]["rate"].ToString());
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public ActionResult insert_Order(order order)
        {
            if (Session["user_name"] == null && Convert.ToString(Session["user_name"]) == "")
            {
                return RedirectToAction("Employee_Login", "Login");
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("item_id");
            dt.Columns.Add("item_name");
            dt.Columns.Add("item_qty");
            dt.Columns.Add("item_rate");
            dt.Columns.Add("total_item_price");
            dt.TableName = "Item";
            foreach (var n in order.ItemDetails)
            {
                if (n.item_name != null && n.item_name != "")
                {
                    int GetId = GetItemID(n.item_name);
                    if (GetId != 0)
                    {
                        dt.Rows.Add(GetId, n.item_name, n.item_qty, n.item_rate, n.item_rate * n.item_qty);
                    }
                }
            }
            dict.Clear();
            //customer details
            dict.Add("@customer_name", order.cust_name);
            dict.Add("@customer_number", order.cust_mobile);
            dict.Add("@Zone_id", order.Zone_id);
            dict.Add("@Area_id", order.Area_id);
            dict.Add("@State_id", order.State_id);
            dict.Add("@Landmark", order.landmark);
            dict.Add("@Address", order.Address);
            dict.Add("@Payment_mode", order.Payment_mode);
            dict.Add("@Payment_Remark", order.payment_remark);
            dict.Add("@overall_total_price ", order.Total_Price);
            dict.Add("@total_quantity", order.total_qty);
            dict.Add("@Remark", order.Remark);
            dict.Add("@user_name", Convert.ToString(Session["user_name"]));
            dict.Add("@Caller_id", order.CallerId);
            dict.Add("@Caller_Name", order.callerName);
            dict.Add("@Source", order.Source);
            dict.Add("@doc", commonclass.GetXmlDoc(dt));
            dict.Add("@mode", "Insert_order");
            var Result = commonclass.return_nonquery(dict, "proc_order");
            //var Result = 1;
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Edit_Order(int id)
        {
            order odr = new order();
            dict.Clear();
            dict.Add("@id", id);
            dict.Add("@mode", "fetchOrderDetails");
            DataSet ds = commonclass.return_dataset(dict, "proc_order");
            if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0)
            {
                odr.id = Convert.ToInt32(ds.Tables[0].Rows[0]["id"].ToString());
                odr.cust_name = ds.Tables[0].Rows[0]["custumerName"].ToString();
                odr.cust_mobile = ds.Tables[0].Rows[0]["custumerMobile"].ToString();
                odr.Total_Price = Convert.ToDouble(ds.Tables[0].Rows[0]["Total_amount"].ToString());
                odr.total_qty = Convert.ToInt32(ds.Tables[0].Rows[0]["total_qty"].ToString());
                odr.Payment_mode = ds.Tables[0].Rows[0]["Payment_mode"].ToString();
                odr.payment_remark = ds.Tables[0].Rows[0]["Payment_Remark"].ToString();
                odr.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                odr.landmark = ds.Tables[0].Rows[0]["landmark"].ToString();

                odr.Area_id = Convert.ToInt32(ds.Tables[0].Rows[0]["AreaId"].ToString());
                odr.Zone_id = Convert.ToInt32(ds.Tables[0].Rows[0]["zoneId"].ToString());
                odr.State_id = Convert.ToInt32(ds.Tables[0].Rows[0]["stateId"].ToString());

                odr.pincode = Convert.ToInt32(ds.Tables[0].Rows[0]["Pincode"].ToString());
                odr.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                odr.Source = ds.Tables[0].Rows[0]["Source"].ToString();
                odr.CallerId = Convert.ToInt32(ds.Tables[0].Rows[0]["Caller_id"].ToString());

                for (int y = 0; ds.Tables[1].Rows.Count > y; y++)
                {
                    item_Model item = new item_Model();
                    item.item_id = Convert.ToInt32(ds.Tables[1].Rows[y]["id"].ToString());
                    item.id = Convert.ToInt32(ds.Tables[1].Rows[y]["item_id"].ToString());
                    item.item_name = ds.Tables[1].Rows[y]["Item_Name"].ToString();
                    item.item_qty = Convert.ToInt32(ds.Tables[1].Rows[y]["item_qty"].ToString());
                    item.item_rate = Convert.ToDouble(ds.Tables[1].Rows[y]["item_rate"].ToString());
                    item.item_total_amount = Convert.ToDouble(Convert.ToDouble(ds.Tables[1].Rows[y]["item_rate"].ToString()) * Convert.ToDouble(ds.Tables[1].Rows[y]["item_qty"].ToString()));
                    odr.ItemDetails.Add(item);
                }
            }
            return Json(odr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult updateOrder(order order)
        {
            if (Session["user_name"] == null && Convert.ToString(Session["user_name"]) == "")
            {
                return RedirectToAction("Employee_Login", "Login");
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("item_id");
            dt.Columns.Add("item_name");
            dt.Columns.Add("item_qty");
            dt.Columns.Add("item_rate");
            dt.Columns.Add("total_item_price");
            dt.TableName = "Item";

            foreach (var n in order.ItemDetails)
            {
                int GetId = GetItemID(n.item_name);
                if (GetId != 0)
                {
                    dict.Clear();
                    dict.Add("@id", n.item_id);
                    dict.Add("@Orderid", order.id);
                    dict.Add("@itemid", GetId);
                    dict.Add("@itemName", n.item_name);
                    dict.Add("@item_qty", n.item_qty);
                    dict.Add("@item_rate", n.item_rate);
                    dict.Add("@total_price", n.item_rate * n.item_qty);
                    dict.Add("@mode", "UpdateOrderItem");
                    commonclass.return_nonquery(dict, "procUpdatOrderItem");
                }


                // dt.Rows.Add(n.item_id, GetId, n.item_name, n.item_qty, n.item_rate, n.item_rate * n.item_qty);
            }
            dict.Clear();
            //customer details
            dict.Add("@id", order.id);
            dict.Add("@customer_name", order.cust_name);
            dict.Add("@customer_number", order.cust_mobile);
            dict.Add("@Zone_id", order.Zone_id);
            dict.Add("@Area_id", order.Area_id);
            dict.Add("@State_id", order.State_id);
            dict.Add("@Landmark", order.landmark);
            dict.Add("@Address", order.Address);
            dict.Add("@Payment_mode", order.Payment_mode);
            dict.Add("@Payment_Remark", order.payment_remark);
            dict.Add("@overall_total_price ", order.Total_Price);
            dict.Add("@total_quantity", order.total_qty);
            dict.Add("@Remark", order.Remark);
            dict.Add("@user_name", Convert.ToString(Session["user_name"]));
            dict.Add("@Caller_id", order.CallerId);
            dict.Add("@Caller_Name", order.callerName);
            dict.Add("@Source", order.Source);
            // dict.Add("@doc", commonclass.GetXmlDoc(dt));
            dict.Add("@mode", "Update_order");
            var Result = commonclass.return_nonquery(dict, "proc_order");

            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public int GetItemID(string prifix)
        {
            dict.Clear();
            dict.Add("@mode", "getIDfromItemName");
            dict.Add("@Item_Name", prifix);
            DataTable dt = commonclass.return_datatable(dict, "proc_item");
            Item_Model item = new Item_Model();
            if (dt.Rows.Count > 0)
            {
                item.id = Convert.ToInt32(dt.Rows[0]["id"].ToString());
                item.item_name = dt.Rows[0]["Item_Name"].ToString();
            }
            return item.id;
        }

        [HttpPost]
        public ActionResult AutocompleteItem(string prifix)
        {
            List<Item_Model> item_list = new List<Item_Model>();
            dict.Clear();
            dict.Add("@mode", "AutocompleteGetall_Items");
            dict.Add("@Item_Name", prifix);
            DataTable dt = commonclass.return_datatable(dict, "proc_item");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Item_Model item = new Item_Model();
                item.id = Convert.ToInt32(dt.Rows[i]["id"].ToString());
                item.item_name = dt.Rows[i]["Item_Name"].ToString();
                item.rate = Convert.ToDouble(dt.Rows[i]["rate"].ToString());
                item_list.Add(item);
            }

            return Json(item_list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AutocompleteItemRate(int GetItemId)
        {
            dict.Clear();
            dict.Add("@mode", "GetRate_Items");
            dict.Add("@id", GetItemId);
            DataTable dt = commonclass.return_datatable(dict, "proc_item");
            Item_Model item = new Item_Model();
            if (dt.Rows.Count > 0)
            {
                item.rate = Convert.ToDouble(dt.Rows[0]["rate"].ToString());
            }

            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetItems(int OrderId)
        {
            dict.Clear();
            dict.Add("@mode", "GetItemRecord");
            dict.Add("@id", OrderId);
            DataTable dt = commonclass.return_datatable(dict, "proc_employee");
            List<Item_Model> Listitem = new List<Item_Model>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Item_Model item = new Item_Model();
                    item.Item_id = dt.Rows[i]["id"].ToString();
                    item.id = Convert.ToInt32(dt.Rows[i]["order_id"].ToString());
                    item.item_name = dt.Rows[i]["Item_Name"].ToString();
                    item.item_qty = Convert.ToInt32(dt.Rows[i]["item_qty"].ToString());
                    item.rate = Convert.ToDouble(dt.Rows[i]["item_rate"]);
                    item.item_total_amount = Convert.ToDouble(dt.Rows[i]["Total_item_price"]);
                    item.DeliveryStatus = dt.Rows[i]["DeliveryStatus"].ToString();
                    Listitem.Add(item);
                }
            }
            return Json(Listitem, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult getLocationOnPin(int Pincode)
        {
            dict.Clear();
            dict.Add("@mode", "fetchAreaOnPincode");
            dict.Add("@Pincode", Pincode);
            DataTable dt = commonclass.return_datatable(dict, "proc_Area");
            List<Area_Model> listArea = new List<Area_Model>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Area_Model area = new Area_Model();
                    //area.StateId = Convert.ToInt32(dt.Rows[0]["stateid"]);
                    area.ZoneId = Convert.ToInt32(dt.Rows[i]["zone_id"]);
                    area.AreaId = Convert.ToInt32(dt.Rows[i]["id"]);
                    //area.StateName = dt.Rows[0]["state"].ToString();
                    //area.ZoneName = dt.Rows[0]["Zone"].ToString();
                    area.AreaName = dt.Rows[i]["Area_name"].ToString();
                    area.pincode = Convert.ToInt32(dt.Rows[i]["Pincode"]);
                    listArea.Add(area);
                }
            }
            return Json(listArea, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLocationOnArea(int areaId)
        {
            dict.Clear();
            dict.Add("@mode", "fetchZoneOnArea");
            dict.Add("@id", areaId);
            DataTable dt = commonclass.return_datatable(dict, "proc_Area");
            List<Area_Model> listArea = new List<Area_Model>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Area_Model area = new Area_Model();
                    area.StateId = Convert.ToInt32(dt.Rows[0]["stateid"]);
                    area.ZoneId = Convert.ToInt32(dt.Rows[i]["zoneid"]);
                    //area.AreaId = Convert.ToInt32(dt.Rows[i]["id"]);
                    area.StateName = dt.Rows[0]["state"].ToString();
                    area.ZoneName = dt.Rows[0]["zone"].ToString();
                    //area.AreaName = dt.Rows[i]["area_name"].ToString();
                    area.pincode = Convert.ToInt32(dt.Rows[i]["pincode"]);
                    listArea.Add(area);
                }
            }
            return Json(listArea, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Bind_Emp(int empId)
        {
            List<Employees> empList = new List<Employees>();
            dict.Add("@id", empId);
            dict.Add("@mode", "BindEmp");
            DataTable dt = commonclass.return_datatable(dict, "proc_employee");
            if (dt.Rows.Count > 0)
            {
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    Employees emp = new Employees();
                    emp.id = Convert.ToInt32(dt.Rows[k]["id"].ToString());
                    emp.userName = dt.Rows[k]["fullName"].ToString();
                    emp.role = Convert.ToInt32(dt.Rows[k]["Role"].ToString());
                    emp.Password = Convert.ToString(Session["Empid"].ToString());
                    empList.Add(emp);
                }
            }
            return Json(empList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult TotalOrder(int id)
        {
            Session["Role"] = empClass.check_role(Convert.ToString(Session["Empid"]));
            TotalOrder total = new TotalOrder();
            dict.Clear();
            dict.Add("@Caller_id", Convert.ToInt32(Session["Empid"]));
            dict.Add("@mode", "CheckOrder");
            //dict.Add("@role", Session["Role"]);
            dict.Add("@State_id", id);
            DataSet ds = commonclass.return_dataset(dict, "proc_order");
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                total.todayTotalOrder = Convert.ToInt32(dt.Rows[0][0].ToString());
                total.monthlyTotalOrder = Convert.ToInt32(dt.Rows[1][0].ToString());
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    //TotalOrder todayorder = new TotalOrder();
                    DailyempPerformance todayOrder = new DailyempPerformance();
                    todayOrder.empName = ds.Tables[1].Rows[i]["Caller_Name"].ToString();
                    todayOrder.totalOrder = Convert.ToInt32(ds.Tables[1].Rows[i]["todayTotalOrder"].ToString());
                    total.TodayEmpPer.Add(todayOrder);
                }
            }

            if (ds.Tables[2].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    //TotalOrder todayorder = new TotalOrder();
                    MonthyempPerformance monthOrder = new MonthyempPerformance();
                    monthOrder.empName = ds.Tables[2].Rows[i]["Caller_Name"].ToString();
                    monthOrder.totalOrder = Convert.ToInt32(ds.Tables[2].Rows[i]["monthTotalOrder"].ToString());
                    total.MonthEmpPer.Add(monthOrder);
                }
            }
            return Json(total, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult TotalDailyOrder(int id)
        {
            List<todayOrder> lsttodayOrder = new List<todayOrder>();
            dict.Clear();
            dict.Add("@mode", "OrderDailywithcategory");
            dict.Add("@Caller_id", Convert.ToInt32(Session["Empid"]));
            dict.Add("@State_id", id);
            DataTable dt = commonclass.return_datatable(dict, "proc_order");

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    todayOrder odr = new todayOrder();
                    odr.Category = dt.Rows[i]["Category"].ToString();
                    odr.order = Convert.ToInt32(dt.Rows[i]["Quantity"]);
                    odr.Uom = dt.Rows[i]["Uom"].ToString();
                    odr.Value = Convert.ToInt32(dt.Rows[i]["Value"]);
                    lsttodayOrder.Add(odr);
                }
            }
            return Json(lsttodayOrder, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult OrderMonthlywithcategory(int id)
        {
            List<todayOrder> lsttodayOrder = new List<todayOrder>();
            dict.Clear();
            dict.Add("@mode", "OrderMonthlywithcategory");
            dict.Add("@Caller_id", Convert.ToInt32(Session["Empid"]));
            dict.Add("@State_id", id);
            DataTable dt = commonclass.return_datatable(dict, "proc_order");

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    todayOrder odr = new todayOrder();
                    odr.Category = dt.Rows[i]["Category"].ToString();
                    odr.order = Convert.ToInt32(dt.Rows[i]["Quantity"]);
                    odr.Uom = dt.Rows[i]["Uom"].ToString();
                    odr.Value = Convert.ToInt32(dt.Rows[i]["Value"]);
                    lsttodayOrder.Add(odr);
                }
            }
            return Json(lsttodayOrder, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult OrderMonthlywithcategoryList(string GetCategoryName)
        {
            List<displayorder> ListItemOrder = new List<displayorder>();
            dict.Clear();
            dict.Add("@mode", "DisplayMonthlyItemWithCategory");
            dict.Add("@Caller_id", Convert.ToInt32(Session["Empid"]));
            dict.Add("@categoryName", GetCategoryName);
            DataTable dt = commonclass.return_datatable(dict, "proc_order");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    displayorder odr = new displayorder();
                    odr.CategoryName = dr["Category"].ToString();
                    odr.item_name = dr["Item_Name"].ToString();
                    odr.item_qty = Convert.ToInt32(dr["item_qty"]);
                    odr.item_rate = Convert.ToDouble(dr["item_rate"]);
                    odr.item_total_amount = Convert.ToDouble(dr["Total_item_price"]);
                    ListItemOrder.Add(odr);
                }
            }
            return Json(ListItemOrder, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SearchOrder(Int64 id)
        {
            List<order> list_order = new List<order>();
            dict.Clear();
            dict.Add("@mode", "SearchOrder");
            dict.Add("@id", id);
            DataSet ds = commonclass.return_dataset(dict, "proc_employee");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int y = 0; y < ds.Tables[0].Rows.Count; y++)
                    {
                        order odr = new order();
                        odr.id = Convert.ToInt32(ds.Tables[0].Rows[y]["id"].ToString());
                        odr.cust_name = ds.Tables[0].Rows[y]["custumerName"].ToString();
                        odr.cust_mobile = ds.Tables[0].Rows[y]["custumerMobile"].ToString();
                        odr.Total_Price = Convert.ToDouble(ds.Tables[0].Rows[y]["Total_amount"].ToString());
                        odr.total_qty = Convert.ToInt32(ds.Tables[0].Rows[y]["total_qty"].ToString());
                        odr.Payment_mode = ds.Tables[0].Rows[y]["Payment_mode"].ToString();
                        odr.payment_remark = ds.Tables[0].Rows[y]["Payment_Remark"].ToString();
                        odr.Remark = ds.Tables[0].Rows[y]["Remark"].ToString();
                        odr.landmark = ds.Tables[0].Rows[y]["landmark"].ToString();
                        odr.AreaName = ds.Tables[0].Rows[y]["Area_name"].ToString();
                        odr.zoneName = ds.Tables[0].Rows[y]["Zone"].ToString();
                        odr.stateName = ds.Tables[0].Rows[y]["state"].ToString();
                        odr.pincode = Convert.ToInt32(ds.Tables[0].Rows[y]["Pincode"].ToString());
                        odr.Address = ds.Tables[0].Rows[y]["Address"].ToString();
                        odr.CallerId = Convert.ToInt32(ds.Tables[0].Rows[y]["Caller_id"].ToString());
                        odr.callerName = ds.Tables[0].Rows[y]["Caller_Name"].ToString();
                        odr.Source = ds.Tables[0].Rows[y]["Source"].ToString();
                        odr.insertedDate = ds.Tables[0].Rows[y]["Inserted_date"].ToString();
                        list_order.Add(odr);
                    }

                }
            }
            return Json(list_order, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOrderStatusCountList(int id)
        {
            List<OrderStatusRecord> GetStatus = new List<OrderStatusRecord>(); 
            List<OrderStatusRecord> GetStatuss = new List<OrderStatusRecord>();
            dict.Clear();
            dict.Add("@mode", "displayPendingAndDeliveryCount");
            dict.Add("@State_id", id);
            DataSet ds = commonclass.return_dataset(dict, "proc_order");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        OrderStatusRecord status = new OrderStatusRecord();
                        status.Pendingdays = ds.Tables[0].Rows[i]["days"].ToString();
           
                        
                        status.AssignOrder = Convert.ToInt32(ds.Tables[0].Rows[i]["assignOrders"]);
                        GetStatus.Add(status);
                    }
                }

            
                {
                    for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                    {
                        OrderStatusRecord status = new OrderStatusRecord();
                        status.Deliverydays = ds.Tables[1].Rows[j]["days"].ToString();
                        status.DeliverySumofqty = Convert.ToInt32(ds.Tables[1].Rows[j]["delivery"]);
                        GetStatuss.Add(status);
                    }
                }
            }
            return Json(new { GetStatus, GetStatuss }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult delte_OrderItem(int id)
        {
            dict.Clear();
            dict.Add("@id", id);
            dict.Add("@mode ", "deleteOrderItem");
            int i = commonclass.return_nonquery(dict, "procUpdatOrderItem");
            return Json(i, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult OrdrApporvalDetails()
        {
            return View();
        }

        [HttpGet]
        public ActionResult fetchORderApprovalDetails()
        {
            List<OrderApproval> ordAppList = new List<OrderApproval>();
            List<OrderApproval> GetAfterStatusOrdAppList = new List<OrderApproval>();
            dict.Clear();
            dict.Add("@mode", "ApprovalData");
            DataSet ds = commonclass.return_dataset(dict, "Proc_CallerLeads");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        OrderApproval ordap = new OrderApproval();
                        ordap.id = Convert.ToInt32(dr["id"]);
                        ordap.orderid = Convert.ToInt32(dr["OrderId"]);
                        ordap.caller = Convert.ToString(dr["callerName"] + "(" + dr["CallerId"] + ")");
                        ordap.Assigncaller = Convert.ToString(dr["AssigncallerName"] + "(" + dr["AssignCallerId"] + ")");
                        ordap.insertedDate = Convert.ToString(dr["InsertedDate"]);
                        ordap.insertedby = Convert.ToString(dr["insertedBy"]);

                        ordap.custName = Convert.ToString(dr["customerName"]);
                        ordap.totalAmount = Convert.ToString(Convert.ToInt32(dr["Total_amount"]));
                        ordap.totalQty = Convert.ToString(dr["total_qty"]);
                        ordap.Address = Convert.ToString(dr["Address"]);

                        ordap.Remark = Convert.ToString(dr["Remark"]);
                        ordap.custMobile = Convert.ToString(dr["Mobile"]);
                        //ordap.custName= Convert.ToString(dr["insertedBy"]);


                        ordAppList.Add(ordap);
                    }
                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        OrderApproval ordaps = new OrderApproval();
                        ordaps.id = Convert.ToInt32(dr["id"]);
                        ordaps.orderid = Convert.ToInt32(dr["OrderId"]);
                        ordaps.caller = Convert.ToString(dr["callerName"] + "(" + dr["CallerId"] + ")");
                        ordaps.Assigncaller = Convert.ToString(dr["AssigncallerName"] + "(" + dr["AssignCallerId"] + ")");
                        ordaps.insertedDate = Convert.ToString(dr["InsertedDate"]);
                        ordaps.insertedby = Convert.ToString(dr["insertedBy"]);
                        ordaps.custName = Convert.ToString(dr["customerName"]);
                        ordaps.totalAmount = Convert.ToString(Convert.ToInt32(dr["Total_amount"]));
                        ordaps.totalQty = Convert.ToString(dr["total_qty"]);
                        ordaps.Address = Convert.ToString(dr["Address"]);
                        ordaps.Remark = Convert.ToString(dr["Remark"]);
                        ordaps.Comment = Convert.ToString(dr["Comment"]);
                        ordaps.custMobile = Convert.ToString(dr["Mobile"]);
                        ordaps.StatusName = Convert.ToString(dr["ActionName"]);
                        ordaps.UpdatedBy = Convert.ToString(dr["UpdatedBy"]);
                        ordaps.UpdatedDate = Convert.ToString(dr["UpdatedDate"]);
                        GetAfterStatusOrdAppList.Add(ordaps);
                    }
                }
            }
            return Json(new { ordAppList, GetAfterStatusOrdAppList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OrderApprovalAdd(string Comment, int GetOrderId, string GetName)
        {
            dict.Clear();
            dict.Add("@remark", Comment);
            dict.Add("@OrderId", GetOrderId);
            dict.Add("@GetApprovalName", GetName);
            dict.Add("@insertedby", Session["user_name"]);
            dict.Add("@mode", "InsertApprovalData");
            int GetResult = commonclass.return_nonquery(dict, "Proc_CallerLeads");
            return Json(GetResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OrederInventory(int[] GetItemId, int DeliveryBoyId)
        {
            int i = 0;
            List<item_Model> itemlist = new List<item_Model>();
            List<item_Model> itemlist2 = new List<item_Model>();
            item_Model GetQuentyitem3 = new item_Model();
            if (GetItemId != null)
            {
                foreach (int ItemId in GetItemId)
                {
                    dict.Clear();
                    dict.Add("@mode", "InsertTempItemId");
                    dict.Add("@itemId", ItemId);
                    i=commonclass.return_nonquery(dict, "proc_order");
                }
                if (i > 0)
                {
                    dict.Clear();
                    dict.Add("@AssignDeliveryBoy", DeliveryBoyId);
                    dict.Add("@mode", "GetInventoryBalance");
                    commonclass.return_nonquery(dict, "proc_order");

                    dict.Clear();
                    dict.Add("@AssignDeliveryBoy", DeliveryBoyId);
                    dict.Add("@mode", "dd");
                    DataSet dt = commonclass.return_dataset(dict, "proc_order");
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Tables[0].Rows)
                        {
                            item_Model item = new item_Model();
                            item.InQty = Convert.ToInt32(dr["InQty"]);
                            item.OutQty = Convert.ToInt32(dr["Solds"]);
                            item.RawItemName = Convert.ToString(dr["rawItem"]);
                            item.item_rate = Convert.ToInt32(dr["InQty"])- Convert.ToInt32(dr["Solds"]);
                            itemlist.Add(item);
                        }
                    }

                    if (dt.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow drs in dt.Tables[1].Rows)
                        {
                            item_Model item = new item_Model();
                            item.PayCharge =Convert.ToString(drs["Payment_mode"]);
                            item.item_total_amount = Convert.ToDouble(drs["paycharge"]);
                            itemlist2.Add(item);
                        }
                    }

                    if (dt.Tables[2].Rows.Count > 0)
                    {
                       GetQuentyitem3.item_qty = Convert.ToInt32(dt.Tables[2].Rows[0]["Quentity"]);
                       GetQuentyitem3.ReciableAmt = Convert.ToDouble(dt.Tables[2].Rows[0]["NetReciableAmt"]);
                    }
                }
            }
            return Json(new { itemlist, itemlist2,GetQuentyitem3 }, JsonRequestBehavior.AllowGet);

        }
    }
}