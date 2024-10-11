using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Call_Centre_Management.Classes;
using Call_Centre_Management.Models;
using ClosedXML.Excel;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
//using System.Net;

namespace Call_Centre_Management.Controllers
{
    public class OrderViewDetailsController : Controller
    {
        Common_Class commonClass = new Common_Class();
        Dictionary<string, object> dict = new Dictionary<string, object>();

        List<Areas> area_list = new List<Areas>();
        List<StateMasterModal> StateList = new List<StateMasterModal>();
        List<ZoneMastermodal> ZoneList = new List<ZoneMastermodal>();
        public Area_CommonClass areaClass = new Area_CommonClass();
        //int orderStatus, state1,zone, area,assign;
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>(); //creating a list to hold the rows of datatable  
        Dictionary<string, object> rowelement; //Initialise a dictionary because it will contain columnName and Column Value and the key is column Name  

        [HttpGet]
        public ActionResult Index()
        {
            return View();

        }
        [HttpGet]
        public ActionResult fetch_data()
        {
            List<displayorder> OrderList = new List<displayorder>();
            List<displayorder> OrderDetailsList = new List<displayorder>();
            List<item_Model> ItemList = new List<item_Model>();
            List<Area_Model> AreaModalList = new List<Area_Model>();
            List<Employees_Modal> DeliveryBoyList = new List<Employees_Modal>();
            dict.Clear();
            dict.Add("@mode", "bindAllfilltersOnload");
            //dict.Add("@ItemActive", 1);
            DataSet ds = commonClass.return_dataset(dict, "order_searching");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    StateMasterModal state = new StateMasterModal();
                    state.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"]);
                    state.StateName = ds.Tables[0].Rows[i]["state"].ToString();
                    StateList.Add(state);
                }
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                {
                    ZoneMastermodal Zone = new ZoneMastermodal();
                    Zone.Id = Convert.ToInt32(ds.Tables[1].Rows[j]["id"]);
                    Zone.ZoneName = ds.Tables[1].Rows[j]["Zone"].ToString();
                    ZoneList.Add(Zone);
                }
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                for (int k = 0; k < ds.Tables[2].Rows.Count; k++)
                {
                    Area_Model AreaModel = new Area_Model();
                    AreaModel.id = Convert.ToInt32(ds.Tables[2].Rows[k]["id"]);
                    AreaModel.AreaName = ds.Tables[2].Rows[k]["Area_name"].ToString();
                    AreaModalList.Add(AreaModel);
                }

            }
            if (ds.Tables[3].Rows.Count > 0)
            {
                for (int l = 0; l < ds.Tables[3].Rows.Count; l++)
                {
                    displayorder Orders = new displayorder();
                    Orders.InsertedDate = ds.Tables[3].Rows[l]["OrderDate"].ToString();
                    OrderList.Add(Orders);
                }
            }
            if (ds.Tables[4].Rows.Count > 0)
            {
                for (int m = 0; m < ds.Tables[4].Rows.Count; m++)
                {
                    item_Model ItemSKU = new item_Model();
                    ItemSKU.item_name = ds.Tables[4].Rows[m]["ItemName"].ToString();
                    ItemSKU.id = Convert.ToInt32(ds.Tables[4].Rows[m]["id"]);
                    ItemList.Add(ItemSKU);
                }
            }
            if (ds.Tables[5].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[5].Rows)
                {
                    Employees_Modal DeliveryBoys = new Employees_Modal();
                    DeliveryBoys.id = Convert.ToInt32(dr["DeliveryBoyId"]);
                    DeliveryBoys.fullName = dr["Name"].ToString();
                    DeliveryBoyList.Add(DeliveryBoys);
                }
            }
            if (ds.Tables[6].Rows.Count > 0)
            {
                for (int n = 0; n < ds.Tables[6].Rows.Count; n++)
                {
                    displayorder Orders = new displayorder();
                    Orders.id = Convert.ToInt32(ds.Tables[6].Rows[n]["orderId"]);
                    Orders.orderItemId = Convert.ToInt32(ds.Tables[6].Rows[n]["orderItemId"]);
                    Orders.cust_name = ds.Tables[6].Rows[n]["Name"].ToString();
                    Orders.cust_mobile = ds.Tables[6].Rows[n]["customerMobile"].ToString();
                    Orders.Address = ds.Tables[6].Rows[n]["Address"].ToString();
                    Orders.AreaName = ds.Tables[6].Rows[n]["AreaName"].ToString();
                    Orders.zoneName = ds.Tables[6].Rows[n]["ZoneName"].ToString();
                    Orders.item_name = ds.Tables[6].Rows[n]["Item_Name"].ToString();
                    Orders.total_qty = Convert.ToInt32(ds.Tables[6].Rows[n]["TotalQty"]);
                    Orders.Total_Price = Convert.ToDouble(ds.Tables[6].Rows[n]["TotalAmount"]);
                    Orders.Payment_mode = ds.Tables[6].Rows[n]["Payment_mode"].ToString();
                    Orders.Remark = ds.Tables[6].Rows[n]["Remark"].ToString();
                    //Orders.ItemId = Convert.ToInt32(ds.Tables[6].Rows[n]["ItemId"]);
                    //Orders.stateName = ds.Tables[6].Rows[n]["StateName"].ToString();
                    //Orders.callerName = ds.Tables[6].Rows[n]["Caller_Name"].ToString();
                    //Orders.Source = ds.Tables[6].Rows[n]["Source"].ToString();
                    //Orders.item_Active = ds.Tables[6].Rows[n]["active"].ToString();
                    Orders.InsertedDate = ds.Tables[6].Rows[n]["CreatedDate"].ToString();
                    OrderDetailsList.Add(Orders);
                }

            }

            ViewBag.AllStateList = StateList;
            ViewBag.AllZoneList = ZoneList;
            ViewBag.AllAreaList = AreaModalList;
            ViewBag.AllOrderDateList = OrderList;
            ViewBag.AllSkUList = ItemList;
            ViewBag.GetOrderItemList = OrderDetailsList;
            ViewBag.DeliveryBoyDetails = DeliveryBoyList;
            return View();
        }
        //[HttpPost]
        //public ActionResult fetch_data(int orderStatus, int[] state, int zone, int area, int assign, DateTime Todate, DateTime FromDate)
        //{
        //    //bind dd state
        //    //dict.Clear();
        //    //dict.Add("@mode", "AllState");
        //    //ViewBag.states = new SelectList(areaClass.BindDropDown("state", "id", "proc_common", dict, "STATE"), "value", "text");

        //    //fetch_data 
        //    if (state[0] == 0)
        //    {
        //        zone = 0;
        //        area = 0;
        //    }
        //    if (state[0] == 0 && zone == 0)
        //    {
        //        area = 0;
        //    }
        //    List<orderDetails_model> list_item = new List<orderDetails_model>();
        //    dict.Clear();
        //    dict.Add("@mode", "orders_details");
        //    dict.Add("@Order_status", orderStatus);
        //    dict.Add("@state", state[0]);
        //    dict.Add("@zone", zone);
        //    dict.Add("@area", area);
        //    dict.Add("@asign", assign);

        //    //dict.Add("@toDate", Todate);
        //    //dict.Add("@fromDate", FromDate);

        //    DataSet ds = commonClass.return_dataset(dict, "order_searching");
        //    ds.Tables[0].TableName = "Order Details";
        //    Session["Order_details"] = null;
        //    Session["Order_details"] = ds;
        //    if (ds.Tables.Count > 0)
        //    {
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                orderDetails_model o = new orderDetails_model();

        //                o.id = ds.Tables[0].Rows[i]["id"].ToString();
        //                o.OrderId = ds.Tables[0].Rows[i]["orderid"].ToString();
        //                o.customerName = ds.Tables[0].Rows[i]["Name"].ToString();
        //                o.customerMobile = ds.Tables[0].Rows[i]["Mobile"].ToString();
        //                o.itemName = ds.Tables[0].Rows[i]["Item_Name"].ToString();
        //                o.itemQty = Convert.ToInt32(ds.Tables[0].Rows[i]["item_qty"].ToString());
        //                o.itemRate = Convert.ToDouble(ds.Tables[0].Rows[i]["rate"].ToString());
        //                o.totalPrice = Convert.ToDouble(ds.Tables[0].Rows[i]["TotalAmount"].ToString());
        //                o.deliveryAddres = ds.Tables[0].Rows[i]["DeliveryAddress"].ToString();
        //                o.deliveryStatus = Convert.ToInt32(ds.Tables[0].Rows[i]["Deliverystatus"]).ToString();
        //                if (o.deliveryStatus == "1") { o.deliveryStatus = "<font color='Blue'>Pending</font>"; }
        //                else if (o.deliveryStatus == "2") { o.deliveryStatus = "<font color='Green'>Delivery</font>"; }
        //                else if (o.deliveryStatus == "3") { o.deliveryStatus = "<font color='pink'>Return</font>"; }
        //                else { o.deliveryStatus = "<font color='Red'>Cancel</font>"; }
        //                o.bookingDate = ds.Tables[0].Rows[i]["BookingDate"].ToString();
        //                o.CallerId = Convert.ToInt32(ds.Tables[0].Rows[i]["Caller_id"].ToString());
        //                o.callerName = ds.Tables[0].Rows[i]["Caller_Name"].ToString();
        //                list_item.Add(o);
        //            }
        //        }
        //    }
        //    ViewBag.itemdetail = list_item;
        //    var data = list_item;
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //    //View(list_item);
        //}
        [HttpGet]
        public ActionResult ChangeOrderStatus(int id)
        {
            dict.Clear();
            dict.Add("@mode", "DeliverStatusChange");
            dict.Add("@Order_status", -1);
            dict.Add("@id", id);
            int Result = commonClass.return_nonquery(dict, "proc_order");
            if (Result > 0)
            {
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
        }
        //public void exportExel()
        //{
        //    commonClass.createExcelFile(Session["Order_details"] as DataSet, "Order Details","D://Exel");
        //    RedirectToAction("OrderViewDetails/ fetch_data");

        //}
        public void exportExel()
        {
            string GetDate = DateTime.Now.ToString("dd MMM yyyy HH:mm:ss tt");//("MM/dd/yyyy HH:mm:ss tt");
            DataSet ds = Session["Order_details"] as DataSet;
            if (ds == null)
            {
                return;
            }

            //wb.Worksheets.Add(Session["Order_details"] as DataSet);
            try
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(ds);
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
        [HttpGet]
        public ActionResult orderExport()
        {
            return View();
        }
        [HttpPost]
        public ActionResult orderExport(DateTime StartDate, DateTime EndDate)
        {
            StartDate = Convert.ToDateTime(StartDate.ToString("yyyy-MM-dd"));
            EndDate = Convert.ToDateTime(EndDate.ToString("yyyy-MM-dd"));
            string s = StartDate.ToString("yyyy-MM-dd");
            string e = EndDate.ToString("yyyy-MM-dd");
            string dsf = null;
            if (StartDate < EndDate)
            {

                dsf = "I am boss";
            }
            List<orderExportModel> orderList = new List<orderExportModel>();
            dict.Clear();
            dict.Add("@StartDate", s);
            dict.Add("@Enddate", e);
            dict.Add("@mode", "ExportExcelWithDate");
            DataSet ds = commonClass.return_dataset(dict, "order_searching");
            if (ds.Tables.Count > 0)
            {
                for (int n = 0; n < ds.Tables[0].Rows.Count; n++)
                {
                    Session["Order_details"] = null;
                    TempData["ExcelFileName"] = "Order Report";
                    ds.Tables[0].TableName = "Order Details";
                    Session["Order_details"] = ds;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        orderExportModel odr = new orderExportModel();
                        odr.Caller = ds.Tables[0].Rows[n]["Caller"].ToString();
                        odr.Date = ds.Tables[0].Rows[n]["Date"].ToString();
                        odr.OrderFrom = ds.Tables[0].Rows[n]["OrderFrom"].ToString();
                        odr.CustomerName = ds.Tables[0].Rows[n]["CustomerName"].ToString();
                        odr.Number = ds.Tables[0].Rows[n]["Number"].ToString();
                        odr.HouseNumber = ds.Tables[0].Rows[n]["HomeAddress"].ToString();
                        odr.Area = ds.Tables[0].Rows[n]["Area"].ToString();
                        odr.Zone = ds.Tables[0].Rows[n]["Zone"].ToString();
                        odr.Pincode = ds.Tables[0].Rows[n]["Pincode"].ToString();
                        odr.OrderQty = ds.Tables[0].Rows[n]["OrderQty"].ToString();
                        odr.Amount = ds.Tables[0].Rows[n]["Amount"].ToString();
                        odr.Coupon = ds.Tables[0].Rows[n]["Coupon"].ToString();
                        odr.PaymentMode = ds.Tables[0].Rows[n]["PaymentMode"].ToString();
                        odr.Remarks = ds.Tables[0].Rows[n]["Remarks"].ToString();
                        orderList.Add(odr);
                    }
                }
            }
            return Json(orderList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult BindStateWithZone(int[] StateId)
        {
            // DataSet ds = new DataSet();
            DataTable ds = new DataTable();

            if (StateId != null)
            {
                foreach (var State in StateId)
                {
                    dict.Clear();
                    dict.Add("@state", State);
                    dict.Add("@mode", "BindStateWithZones");
                    ds = commonClass.return_datatable(dict, "order_searching");
                    if (ds.Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Rows)
                        {
                            ZoneMastermodal Zone = new ZoneMastermodal();
                            Zone.Id = Convert.ToInt32(dr["ZoneId"]);
                            Zone.ZoneName = dr["Zone"].ToString();
                            ZoneList.Add(Zone);
                        }
                    }
                }

            }
            return Json(ZoneList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult BindZoneWithArea(int[] ZoneId)
        {
            List<Area_Model> AreaModalList = new List<Area_Model>();
            List<displayorder> orderDateList = new List<displayorder>();
            var GetOrderdatelist = new List<string>();
            // DataSet ds = new DataSet();
            DataSet ds = new DataSet();
            if (ZoneId != null)
            {
                foreach (var zone in ZoneId)
                {
                    dict.Clear();
                    dict.Add("@zone", zone);
                    dict.Add("@mode", "BindZoneWithAreas");
                    ds = commonClass.return_dataset(dict, "order_searching");
                    //ds = commonClass.return_dataset(dict, "test");

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drs in ds.Tables[0].Rows)
                        {
                            Area_Model Area = new Area_Model();
                            Area.AreaId = Convert.ToInt32(drs["AreaId"]);
                            Area.AreaName = drs["Area_name"].ToString();
                            AreaModalList.Add(Area);
                        }
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow drs in ds.Tables[1].Rows)
                        {
                            displayorder orders = new displayorder();
                            orders.InsertedDate = drs["CreatedDate"].ToString();
                            orderDateList.Add(orders);
                        }
                        GetOrderdatelist = (from num in orderDateList
                                            select num.InsertedDate).Distinct().ToList();
                    }

                }
            }
            var GetBothlist = new { AreaModalList = AreaModalList, orderDateList = GetOrderdatelist };
            return Json(GetBothlist, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult BindAreawithDates(int[] GetAreaId)
        {
            List<displayorder> orderDateList = new List<displayorder>();
            var GetOrderdatelist = new List<string>();
            // DataSet ds = new DataSet();
            DataTable ds = new DataTable();
            if (GetAreaId != null)
            {
                foreach (var Area in GetAreaId)
                {
                    dict.Clear();
                    dict.Add("@area", Area);
                    dict.Add("@mode", "BindAreawithDate");
                    ds = commonClass.return_datatable(dict, "order_searching");
                    //ds = commonClass.return_dataset(dict, "test");
                    if (ds.Rows.Count > 0)
                    {
                        foreach (DataRow drs in ds.Rows)
                        {
                            displayorder orders = new displayorder();
                            orders.InsertedDate = drs["CreatedDate"].ToString();
                            orderDateList.Add(orders);
                        }
                        GetOrderdatelist = (from num in orderDateList
                                            select num.InsertedDate).Distinct().ToList();
                    }
                }
            }
            return Json(GetOrderdatelist, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult BindDatewithItem(string[] GetDates)
        {
            List<item_Model> ItemList = new List<item_Model>();
            // DataSet ds = new DataSet();
            DataTable ds = new DataTable();
            if (GetDates != null)
            {
                foreach (var Dates in GetDates)
                {
                    // var s = Convert.ToDateTime(Dates).ToString("yyyy-MM-dd");
                    dict.Clear();
                    dict.Add("@Getdate", Convert.ToDateTime(Dates).ToString("yyyy-MM-dd"));
                    dict.Add("@mode", "BindDatewithItem");
                    ds = commonClass.return_datatable(dict, "order_searching");
                    if (ds.Rows.Count > 0)
                    {
                        foreach (DataRow drs in ds.Rows)
                        {
                            item_Model items = new item_Model();
                            items.id = Convert.ToInt32(drs["ItemId"]);
                            items.item_name = drs["itemname"].ToString();
                            ItemList.Add(items);
                        }
                    }
                }
            }
            return Json(ItemList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult FinalFillters(FillterOrder fillter)
        {
            List<displayorder> Orders = new List<displayorder>();
            List<displayorder> OrdersList = new List<displayorder>();

            dict.Clear();
            dict.Add("@mode", "FinalFillter");
            DataTable dt = commonClass.return_datatable(dict, "order_searching");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        displayorder odr = new displayorder();
                        odr.id = Convert.ToInt32(dt.Rows[i]["orderId"]);
                        odr.orderItemId = Convert.ToInt32(dt.Rows[i]["orderItemId"]);
                        odr.ItemId = Convert.ToInt32(dt.Rows[i]["ItemId"]);
                        odr.cust_name = dt.Rows[i]["Name"].ToString();
                        odr.cust_mobile = dt.Rows[i]["customerMobile"].ToString();
                        odr.Total_Price = Convert.ToDouble(dt.Rows[i]["TotalAmount"]);
                        odr.total_qty = Convert.ToInt32(dt.Rows[i]["TotalQty"]);
                        odr.Payment_mode = dt.Rows[i]["Payment_mode"].ToString();
                        odr.Remark = dt.Rows[i]["Remark"].ToString();
                        odr.Address = dt.Rows[i]["Address"].ToString();
                        odr.stateName = dt.Rows[i]["StateName"].ToString();
                        odr.zoneName = dt.Rows[i]["ZoneName"].ToString();
                        odr.AreaName = dt.Rows[i]["AreaName"].ToString();
                        odr.callerName = dt.Rows[i]["Caller_Name"].ToString();
                        odr.Source = dt.Rows[i]["Source"].ToString();
                        odr.InsertedDate = dt.Rows[i]["CreatedDate"].ToString();
                        odr.item_name = dt.Rows[i]["Item_Name"].ToString();
                        odr.State_id = Convert.ToInt32(dt.Rows[i]["State_id"]);
                        odr.Zone_id = Convert.ToInt32(dt.Rows[i]["Zone_id"]);
                        odr.Area_id = Convert.ToInt32(dt.Rows[i]["Area_id"]);
                        odr.item_Active = dt.Rows[i]["active"].ToString();
                        Orders.Add(odr);
                    }
                    catch (Exception ex) { string es = ex.ToString(); }

                }
            }

            if (Orders.Count != 0)
            {
                if (fillter.StateId != null)
                {
                    foreach (var State in fillter.StateId)
                    {
                        foreach (var GetState in Orders.Where(s => s.State_id == State).ToList())
                        {
                            if (GetState.State_id == State)
                            {
                                OrdersList.Add(GetState);
                            }
                        }
                    }
                    if (OrdersList.Count > 0 || OrdersList.Count == 0)
                    {
                        Orders.Clear();
                        Orders.AddRange(OrdersList);
                        OrdersList.Clear();
                    }

                }
                if (fillter.ZoneId != null)
                {
                    foreach (var Zone in fillter.ZoneId)
                    {
                        foreach (var GetZone in Orders.Where(z => z.Zone_id == Zone).ToList())
                        {
                            if (GetZone.Zone_id == Zone)
                            {
                                OrdersList.Add(GetZone);
                            }
                        }
                    }

                    if (OrdersList.Count > 0 || OrdersList.Count == 0)
                    {
                        Orders.Clear();
                        Orders.AddRange(OrdersList);
                        OrdersList.Clear();
                    }
                }

                if (fillter.AreaId != null)
                {
                    foreach (var Area in fillter.AreaId)
                    {
                        foreach (var GetArea in Orders.Where(A => A.Area_id == Area).ToList())
                        {
                            if (GetArea.Area_id == Area)
                            {
                                OrdersList.Add(GetArea);
                            }
                        }
                    }

                    if (OrdersList.Count > 0 || OrdersList.Count == 0)
                    {
                        Orders.Clear();
                        Orders.AddRange(OrdersList);
                        OrdersList.Clear();
                    }
                }


                if (fillter.InsertedDate != null)
                {
                    foreach (var OrderDate in fillter.InsertedDate)
                    {
                        foreach (var GetOrderDate in Orders.Where(D => Convert.ToDateTime(D.InsertedDate).ToString("yyyy-MM-dd") == Convert.ToDateTime(OrderDate).ToString("yyyy-MM-dd")).ToList())
                        {
                            if (Convert.ToDateTime(GetOrderDate.InsertedDate).ToString("yyyy-MM-dd") == Convert.ToDateTime(OrderDate).ToString("yyyy-MM-dd"))
                            {
                                OrdersList.Add(GetOrderDate);
                            }
                        }
                    }

                    if (OrdersList.Count > 0 || OrdersList.Count == 0)
                    {
                        Orders.Clear();
                        Orders.AddRange(OrdersList);
                        OrdersList.Clear();
                    }
                }

                if (fillter.ItemId != null)
                {
                    foreach (var ItemId in fillter.ItemId)
                    {
                        foreach (var GetItemDetail in Orders.Where(i => i.ItemId == ItemId).ToList())
                        {
                            if (GetItemDetail.ItemId == ItemId)
                            {
                                OrdersList.Add(GetItemDetail);

                            }
                        }


                    }

                    if (OrdersList.Count > 0 || OrdersList.Count == 0)
                    {
                        Orders.Clear();
                        Orders.AddRange(OrdersList);
                        OrdersList.Clear();
                    }
                }
            }
            var GetOrderList = Orders.Distinct().ToList();
            return Json(GetOrderList, JsonRequestBehavior.AllowGet);
            //return Json(Orders, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AssignOrder(List<int> OrderId, int DeliveryBoyId)
        {
            int Result = 0;
            foreach (var OrdId in OrderId)
            {
                dict.Clear();
                dict.Add("@mode", "InsertBulkOrderDetails");
                dict.Add("@order_id", OrdId);
                dict.Add("@AssignDeliveryBoy", DeliveryBoyId);
                Result = commonClass.return_nonquery(dict, "proc_order");
            }
            if (Result != 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }



        }
        [HttpGet]
        public ActionResult DisplayAssignOrders()
        {
            List<displayorder> Orders = new List<displayorder>();

            try
            {
                dict.Clear();
                dict.Add("@mode", "DisplayAssignOrder");
                DataSet dt = commonClass.return_dataset(dict, "order_searching");
                if (dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        displayorder odr = new displayorder();
                        odr.id = Convert.ToInt32(dr["orderId"]);
                        odr.ItemId = Convert.ToInt32(dr["ItemId"]);
                        odr.cust_name = dr["Name"].ToString();
                        odr.cust_mobile = dr["customerMobile"].ToString();
                        odr.Address = dr["Address"].ToString();
                        odr.AreaName = dr["AreaName"].ToString();
                        odr.zoneName = dr["ZoneName"].ToString();
                        odr.item_name = dr["Item_Name"].ToString();
                        odr.total_qty = Convert.ToInt32(dr["TotalQty"]);
                        odr.Total_Price = Convert.ToDouble(dr["TotalAmount"]);
                        odr.Coupon = dr["Coupon"].ToString();
                        odr.Payment_mode = dr["Payment_mode"].ToString();
                        odr.Remark = dr["Remark"].ToString() == "Urgent" ? "<font color='Red'>" + dr["Remark"].ToString() + "</font>" : dr["Remark"].ToString() == "N/A" ? "<font class='text-primary'>" + dr["Remark"].ToString() + "</font>" : "<font class='text-info'>" + dr["Remark"].ToString() + "</font>";
                        odr.ItemStatus = Convert.ToInt32(dr["Status"]) == 1 ? "Pending" : Convert.ToInt32(dr["Status"]) == 2 ? "Delivered" : Convert.ToInt32(dr["Status"]) == 3 ? "AlReady Delivered" : Convert.ToInt32(dr["Status"]) == -1 ? "Cancel" : "Undefined";
                        odr.DeliveryAssignBy = dr["Deliveryboyname"].ToString();
                        odr.InsertedDate = dr["CreatedDate"].ToString();

                        //odr.callerName = dr["Caller_Name"].ToString();
                        //odr.Source = dr["Source"].ToString();

                        //odr.item_Active = dr["active"].ToString() == "Pending" ? "<font color='blue'>" + dr["active"].ToString() +"</font>": dr["active"].ToString() == "Delivery"? "<font color='Green'>" + dr["active"].ToString() + "</font>": "<font color='Red'>" + dr["active"].ToString() + "</font>";
                        //odr.InsertedDate = dr["CreatedDate"].ToString();
                        Orders.Add(odr);
                    }
                }
                var emplist = GetEmloyee();
                var GetAssigndateList = GetAssigndateLists();

                //if (dt.Tables[1].Rows.Count > 0)
                //{
                //    NotAssignTotalPendingOrder = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalNotAssignPendingItem"]);
                //}
                //if (dt.Tables[2].Rows.Count > 0)
                //{
                //    AssignTotalPendingOrder = Convert.ToInt32(dt.Tables[2].Rows[0]["TotalAssignPendingItem"]);
                //}
                //if (dt.Tables[3].Rows.Count > 0)
                //{
                //    AssignTotalDeliverygOrder = Convert.ToInt32(dt.Tables[3].Rows[0]["TotalAssignDeliveryItem"]);
                //}
                //if (dt.Tables[4].Rows.Count > 0)
                //{
                //    AssignTotalCancelOrder = Convert.ToInt32(dt.Tables[4].Rows[0]["TotalAssignCancelItem"]);
                //}



                ViewBag.deliveryBoy = emplist;
                ViewBag.GetAssignOrder = Orders;
                ViewBag.GetAssignDateList = GetAssigndateList;
            }
            catch (Exception ex) { }
            return View();
        }

        [HttpGet]
        public List<Employees_Modal> GetEmloyee()
        {
            List<Employees_Modal> emplist = new List<Employees_Modal>();
            try
            {
                dict.Clear();
                dict.Add("@mode", "GetAssignemp");
                DataSet dt = commonClass.return_dataset(dict, "order_searching");
                if (dt.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                    {
                        Employees_Modal emp = new Employees_Modal();
                        emp.id = Convert.ToInt32(dt.Tables[0].Rows[i]["id"].ToString());
                        emp.fullName = Convert.ToString(dt.Tables[0].Rows[i]["fullName"]);
                        emplist.Add(emp);
                    }
                }
            }
            catch (Exception ex) { }


            return emplist;
        }
        [HttpGet]
        public List<displayorder> GetAssigndateLists()
        {
            List<displayorder> GetAssigndateList = new List<displayorder>();
            try
            {
                dict.Clear();
                dict.Add("@mode", "GetAssigndateDetails");
                DataSet dt = commonClass.return_dataset(dict, "order_searching");
                if (dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        displayorder GetDate = new displayorder();
                        GetDate.DeliveryAssignDate = dr["AssignDate"].ToString();
                        GetAssigndateList.Add(GetDate);
                    }
                }
            }
            catch (Exception ex) { }
            return GetAssigndateList;
        }

        [HttpGet]
        public ActionResult deliveryBoyExelImport()
        {
            List<Employees_Modal> emp_list = new List<Employees_Modal>();
            dict.Clear();
            //dict.Add("","");
            dict.Add("@mode", "BindDeliveryBoy");
            DataTable dt = commonClass.return_datatable(dict, "proc_employee");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Employees_Modal emp = new Employees_Modal();
                    emp.id = Convert.ToInt32(dt.Rows[i]["id"].ToString());
                    emp.fullName = dt.Rows[i]["fullName"].ToString();
                    emp_list.Add(emp);
                }
            }
            ViewBag.deliveryBoy = emp_list;
            return View();
        }
        [HttpPost]
        public ActionResult deliveryBoyExelImport(string FromDate, string ToDate, int[] deliveryBoy)
        {
            bool status = false;
            //List<order> AddOrderList = new List<order>();
            List<order> ShowList = new List<order>();
            DataSet ds = new DataSet();
            dict.Clear();
            dict.Add("@mode", "ExportDelivertTest");
            dict.Add("@StartDate", FromDate);
            dict.Add("@Enddate", ToDate);
            DataTable dt = commonClass.return_datatable(dict, "order_searching");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    order orders = new order();
                    item_Model items = new item_Model();
                    //orders.id = Convert.ToInt32(dr["orderId"]);  
                    orders.cust_name = dr["Name"].ToString();
                    orders.cust_mobile = dr["customerMobile"].ToString();
                    orders.Address = dr["Address"].ToString();
                    orders.AreaName = dr["AreaName"].ToString();
                    orders.zoneName = dr["ZoneName"].ToString();
                    //items.id = Convert.ToInt32(dr["ItemOrderId"]);
                    items.item_id = Convert.ToInt32(dr["ItemId"]);
                    items.item_name = dr["Item_Name"].ToString();
                    items.item_qty = Convert.ToInt32(dr["TotalQty"]);
                    items.item_total_amount = Convert.ToInt32(dr["TotalAmount"]);
                    items.coupon = dr["Coupon"].ToString();
                    items.deliveryRemark = dr["deliveryStatus"].ToString();
                    orders.ItemDetails.Add(items);
                    orders.Payment_mode = dr["Payment_mode"].ToString();
                    orders.payment_remark = dr["Remark"].ToString();
                    orders.DeliveryAssignId = Convert.ToInt32(dr["AssignDeliveryBoy"]);
                    orders.DeliveryAssignName = dr["DeliveryBoyName"].ToString();
                    orders.DeliveryAssignDate = dr["assignDeliveryDate"].ToString();

                    ShowList.Add(orders);
                }
                if (ShowList.Count > 0)
                {
                    if (deliveryBoy != null)
                    {
                        foreach (var k in deliveryBoy)
                        {
                            var GetName = (from s in ShowList where s.DeliveryAssignId == k select s.DeliveryAssignName).FirstOrDefault();
                            if (GetName != null)
                            {
                                DataTable dtt = new DataTable();
                                dtt.TableName = GetName;
                                //dtt.Columns.Add("OrderId", typeof(int));
                                dtt.Columns.Add("ItemId", typeof(string));
                                dtt.Columns.Add("CustomerName", typeof(string));
                                dtt.Columns.Add("cust_mobile", typeof(string));
                                dtt.Columns.Add("Address", typeof(string));
                                dtt.Columns.Add("AreaName", typeof(string));
                                dtt.Columns.Add("zoneName", typeof(string));
                                dtt.Columns.Add("item_name", typeof(string));
                                dtt.Columns.Add("item_qty", typeof(int));
                                dtt.Columns.Add("item_total_amount", typeof(int));
                                dtt.Columns.Add("Coupon", typeof(string));
                                dtt.Columns.Add("Delevery/CancelRemark", typeof(string));
                                dtt.Columns.Add("Payment_mode", typeof(string));
                                dtt.Columns.Add("Remark", typeof(string));
                                //dtt.Columns.Add("DeliveryAssignId", typeof(int));
                                //dtt.Columns.Add("DeliveryAssignName", typeof(string));

                                foreach (var getId in ShowList.Where(d => d.DeliveryAssignId == k).ToList())
                                {
                                    try
                                    {
                                        if (getId.DeliveryAssignId == k)
                                        {
                                            DataRow row = dtt.NewRow();
                                            // row["OrderId"] = getId.id;
                                            row["CustomerName"] = getId.cust_name;
                                            row["cust_mobile"] = getId.cust_mobile;
                                            row["Address"] = getId.Address;
                                            row["AreaName"] = getId.AreaName;
                                            row["zoneName"] = getId.zoneName;
                                            row["Payment_mode"] = getId.Payment_mode;
                                            row["Remark"] = getId.payment_remark;
                                            //row["DeliveryAssignId"] = getId.DeliveryAssignId;
                                            //row["DeliveryAssignName"] = getId.DeliveryAssignName;

                                            foreach (var items in getId.ItemDetails)
                                            {
                                                // DataRow row2 = dtt.NewRow();
                                                row["ItemId"] = items.item_id;
                                                row["item_name"] = items.item_name;
                                                row["item_qty"] = items.item_qty;
                                                row["item_total_amount"] = items.item_total_amount;
                                                row["Coupon"] = items.coupon;
                                                row["Delevery/CancelRemark"] = items.deliveryRemark;
                                                // dtt.Rows.Add(row);
                                            }
                                            dtt.Rows.Add(row);
                                        }
                                    }
                                    catch (Exception ex) { string s = ex.ToString(); }
                                }

                                ds.Tables.Add(dtt);
                            }
                        }
                    }
                }
            }

            // if (dt.Rows.Count>0)
            if (ds.Tables.Count > 0)
            {
                Session["Order_details"] = null;

                TempData["ExcelFileName"] = "Assign Order Report";
                Session["Order_details"] = ds;
                status = true;
            }


            return Json(status, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult deliveryBoyExelImport2(string FromDate, string ToDate)
        {
            bool status = false;
            //List<order> AddOrderList = new List<order>();0
            
            List<order> ShowList = new List<order>();
            DataSet ds = new DataSet();
            dict.Clear();
            dict.Add("@mode", "AssignorderStatus");
            dict.Add("@StartDate",FromDate) ;
            dict.Add("@Enddate",ToDate);
            DataSet dt = commonClass.return_dataset(dict, "order_searching");

            if (dt.Tables[0].Rows.Count > 0)
            {
                Session["Order_details"] = null;
                dt.Tables[0].TableName = "Assign Order Status" +
                    "";
                TempData["ExcelFileName"] = "Assign Order Status Report";
                Session["Order_details"] = dt;
                status = true;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //public ActionResult BindOrder(List<string> GetDate, int deliveryboy)
        public ActionResult BindOrder(string GetDate, int deliveryboy )
        {
            List<displayorder> Orders = new List<displayorder>();
            //foreach (var Getdate in GetDate)
            //{
            dict.Clear();
            dict.Add("@deliveryBoyID", deliveryboy);
            if (GetDate == null || GetDate == "")
            {
                dict.Add("@mode", "searchOrderOfDeliveryboy");
            }
            else
            {
                dict.Add("@mode", "searchOrderOfDeliveryboyDATE");
                //var f = Convert.ToDateTime(GetDate).ToString("dd-MM-yyyy");
                var sd = Convert.ToDateTime(GetDate).ToString("yyyy-MM-dd");
                dict.Add("@date", Convert.ToDateTime(GetDate).ToString("yyyy-MM-dd"));
            }
                DataTable dt = commonClass.return_datatable(dict, "order_searching");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        displayorder odr = new displayorder();
                        odr.id = Convert.ToInt32(dr["orderId"]);
                        odr.ItemId = Convert.ToInt32(dr["ItemId"]);
                        odr.cust_name = dr["Name"].ToString();
                        odr.cust_mobile = dr["customerMobile"].ToString();
                        odr.Address = dr["Address"].ToString();
                        odr.AreaName = dr["AreaName"].ToString();
                        odr.zoneName = dr["ZoneName"].ToString();
                        odr.item_name = dr["Item_Name"].ToString();
                        odr.total_qty = Convert.ToInt32(dr["TotalQty"]);
                        odr.Total_Price = Convert.ToDouble(dr["TotalAmount"]);
                        odr.Coupon = dr["Coupon"].ToString();
                        odr.Payment_mode = dr["Payment_mode"].ToString();
                        odr.Remark = dr["Remark"].ToString() == "Urgent" ? "<font color='Red'>" + dr["Remark"].ToString() + "</font>" : dr["Remark"].ToString() == "N/A" ? "<font class='text-primary'>" + dr["Remark"].ToString() + "</font>" : "<font class='text-info'>" + dr["Remark"].ToString() + "</font>";
                        odr.ItemStatus = Convert.ToInt32(dr["Status"]) == 1 ? "Pending" : Convert.ToInt32(dr["Status"]) == 2 ? "Delivered" : Convert.ToInt32(dr["Status"]) == 3 ? "AlReady Delivered" : Convert.ToInt32(dr["Status"]) == -1 ? "Cancel" : "Undefined";
                        odr.DeliveryAssignBy = dr["Deliveryboyname"].ToString();
                        odr.InsertedDate = dr["CreatedDate"].ToString();
                        odr.Coupon = dr["Coupon"].ToString();
                        Orders.Add(odr);
                    }
                }
            //}
            return Json(Orders, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BindOrders(string GetDate, int deliveryboy, string txtcalendar1)
        {
            List<displayorder> Orders = new List<displayorder>();
            //foreach (var Getdate in GetDate)
            //{
            dict.Clear();
            dict.Add("@deliveryBoyID", deliveryboy);
            dict.Add("@txtcalendar1", txtcalendar1);
            if (GetDate == null || GetDate == "")
            {
                dict.Add("@mode", "searchOrderOfDeliveryboy1");
            }
            else
            {
                dict.Add("@mode", "searchOrderOfDeliveryboyDATE");
                //var f = Convert.ToDateTime(GetDate).ToString("dd-MM-yyyy");
                var sd = Convert.ToDateTime(GetDate).ToString("yyyy-MM-dd");
                dict.Add("@date", Convert.ToDateTime(GetDate).ToString("yyyy-MM-dd"));
            }
            DataTable dt = commonClass.return_datatable(dict, "order_searching");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    displayorder odr = new displayorder();
                    odr.id = Convert.ToInt32(dr["orderId"]);
                    odr.ItemId = Convert.ToInt32(dr["ItemId"]);
                    odr.cust_name = dr["Name"].ToString();
                    odr.cust_mobile = dr["customerMobile"].ToString();
                    odr.Address = dr["Address"].ToString();
                    odr.AreaName = dr["AreaName"].ToString();
                    odr.zoneName = dr["ZoneName"].ToString();
                    odr.item_name = dr["Item_Name"].ToString();
                    odr.total_qty = Convert.ToInt32(dr["TotalQty"]);
                    odr.Total_Price = Convert.ToDouble(dr["TotalAmount"]);
                    odr.Coupon = dr["Coupon"].ToString();
                    odr.Payment_mode = dr["Payment_mode"].ToString();
                    odr.Remark = dr["Remark"].ToString() == "Urgent" ? "<font color='Red'>" + dr["Remark"].ToString() + "</font>" : dr["Remark"].ToString() == "N/A" ? "<font class='text-primary'>" + dr["Remark"].ToString() + "</font>" : "<font class='text-info'>" + dr["Remark"].ToString() + "</font>";
                    odr.ItemStatus = Convert.ToInt32(dr["Status"]) == 1 ? "Pending" : Convert.ToInt32(dr["Status"]) == 2 ? "Delivered" : Convert.ToInt32(dr["Status"]) == 3 ? "AlReady Delivered" : Convert.ToInt32(dr["Status"]) == -1 ? "Cancel" : "Undefined";
                    odr.DeliveryAssignBy = dr["Deliveryboyname"].ToString();
                    odr.InsertedDate = dr["CreatedDate"].ToString();
                    odr.Coupon = dr["Coupon"].ToString();
                    Orders.Add(odr);
                }
            }
            //}
            return Json(Orders, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public ActionResult OrdersUpdates(string cal, string deliveryboy, string orderStatus, string[] Orders, string deliverycalendar)
        {
            int m = 0;
            int dispatchId = Convert.ToInt32(commonClass.CreateRandomNumber());
            foreach (string i in Orders)
            {
                string[] s = i.ToString().Split(',');   
                dict.Clear();
                dict.Add("@AssignDeliveryBoy", deliveryboy);
                dict.Add("@orderStatus", orderStatus);
                dict.Add("@deliverycalender", deliverycalendar);
                dict.Add("@itemid", s[0].ToString());
                dict.Add("@deliveryStatus", s[1]);
                dict.Add("@userName", Session["user_name"].ToString());
                //dict.Add("@dispatchId", dispatchId);
                dict.Add("@mode", "UpdateOrdersAssign");
                m = m + commonClass.return_nonquery(dict, "proc_order");
                //m = 1;
            }
            if (m > 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult SearchOrderItemNew(Int64? orderItemId, string GetFillterName)
        {

            List<displayorder> Orders = new List<displayorder>();
            dict.Clear();
            dict.Add("@id", orderItemId);
            dict.Add("@SelectSearchColoumn", GetFillterName);
            dict.Add("@mode", "searchOrderItemNew");
            DataTable dt = commonClass.return_datatable(dict, "proc_order");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    displayorder odr = new displayorder();
                    odr.id = Convert.ToInt32(dr["orderId"]);
                    odr.ItemId = Convert.ToInt32(dr["ItemId"]);
                    odr.cust_name = dr["Name"].ToString();
                    odr.cust_mobile = dr["customerMobile"].ToString();
                    odr.Address = dr["Address"].ToString();
                    odr.AreaName = dr["AreaName"].ToString();
                    odr.zoneName = dr["ZoneName"].ToString();
                    odr.item_name = dr["Item_Name"].ToString();
                    odr.total_qty = Convert.ToInt32(dr["TotalQty"]);
                    odr.Total_Price = Convert.ToDouble(dr["TotalAmount"]);
                    odr.Coupon = dr["Coupon"].ToString();
                    odr.Payment_mode = dr["Payment_mode"].ToString();
                    odr.Remark = dr["Remark"].ToString() == "Urgent" ? "<font color='Red'>" + dr["Remark"].ToString() + "</font>" : dr["Remark"].ToString() == "N/A" ? "<font class='text-primary'>" + dr["Remark"].ToString() + "</font>" : "<font class='text-info'>" + dr["Remark"].ToString() + "</font>";
                    odr.ItemStatus = Convert.ToInt32(dr["Status"]) == 1 ? "Pending" : Convert.ToInt32(dr["Status"]) == 2 ? "Delivered" : Convert.ToInt32(dr["Status"]) == 3 ? "AlReady Delivered" : Convert.ToInt32(dr["Status"]) == -1 ? "Cancel" : "Undefined";
                    odr.DeliveryAssignBy = dr["Deliveryboyname"].ToString();
                    odr.InsertedDate = dr["CreatedDate"].ToString();
                    Orders.Add(odr);
                }
            }

            return Json(Orders, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SearchOrderItem(Int64? orderItemId)
        {
            List<displayorder> Orders = new List<displayorder>();
            dict.Clear();
            dict.Add("@id", orderItemId);
            dict.Add("@mode", "searchOrderItem");
            DataTable dt = commonClass.return_datatable(dict, "proc_order");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    displayorder odr = new displayorder();
                    odr.id = Convert.ToInt32(dr["orderId"]);
                    odr.ItemId = Convert.ToInt32(dr["ItemId"]);
                    odr.cust_name = dr["Name"].ToString();
                    odr.cust_mobile = dr["customerMobile"].ToString();
                    odr.Address = dr["Address"].ToString();
                    odr.AreaName = dr["AreaName"].ToString();
                    odr.zoneName = dr["ZoneName"].ToString();
                    odr.item_name = dr["Item_Name"].ToString();
                    odr.total_qty = Convert.ToInt32(dr["TotalQty"]);
                    odr.Total_Price = Convert.ToDouble(dr["TotalAmount"]);
                    odr.Coupon = dr["Coupon"].ToString();
                    odr.Payment_mode = dr["Payment_mode"].ToString();
                    odr.Remark = dr["Remark"].ToString() == "Urgent" ? "<font color='Red'>" + dr["Remark"].ToString() + "</font>" : dr["Remark"].ToString() == "N/A" ? "<font class='text-primary'>" + dr["Remark"].ToString() + "</font>" : "<font class='text-info'>" + dr["Remark"].ToString() + "</font>";
                    odr.ItemStatus = Convert.ToInt32(dr["Status"]) == 1 ? "Pending" : Convert.ToInt32(dr["Status"]) == 2 ? "Delivered" : Convert.ToInt32(dr["Status"]) == 3 ? "AlReady Delivered" : Convert.ToInt32(dr["Status"]) == -1 ? "Cancel" : "Undefined";
                    odr.DeliveryAssignBy = dr["Deliveryboyname"].ToString();
                    odr.InsertedDate = dr["CreatedDate"].ToString();


                    Orders.Add(odr);
                }
            }
            return Json(Orders, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DispalypivotTable(int StateId)
        {
            List<displayorder> listodr = new List<displayorder>();
            dict.Clear();
            dict.Add("@stateid", 1);
            //  dict.Add("@mode", "searchOrderItem");
            DataSet ds = commonClass.return_dataset(dict, "procPivot");
            try
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        displayorder odr = new displayorder();
                        odr.zoneName = dr["Headers"].ToString();
                        listodr.Add(odr);
                    }
                }
            }
            catch (Exception ex) { string s = ex.ToString(); }
            return Json(listodr, JsonRequestBehavior.AllowGet);

        }
        public DataTable DispalypivotTableData2(int StateId)
        {
            List<displayorder> listodr = new List<displayorder>();

            dict.Clear();
            dict.Add("@stateid", 1);
            //  dict.Add("@mode", "searchOrderItem");
            DataSet ds = commonClass.return_dataset(dict, "procPivot");
            try
            {
                var m = ds.Tables[0].Columns.Count;


                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataColumn dr in ds.Tables[0].Columns)
                    {
                        displayorder odr = new displayorder();
                        odr.zoneName = dr.ToString();
                        listodr.Add(odr);
                    }
                }
            }
            catch (Exception ex) { string s = ex.ToString(); }
            DataTable dt = ds.Tables[0];
            // return Json(listodr, JsonRequestBehavior.AllowGet);
            return dt;

        }

        [HttpGet]
        public ActionResult DispalypivotTableData(int? StateId)
        {
            dict.Clear();
            dict.Add("@stateid", StateId);
            //  dict.Add("@mode", "searchOrderItem");
            DataSet ds = commonClass.return_dataset(dict, "procPivot");
            DataTable dt = ds.Tables[0];
                    
            string empjson = DataTableToJSONWithStringBuilder(dt);
            string path = Server.MapPath("../TEST/");
            System.IO.File.WriteAllText(path + "json1.json", empjson);
            
            return Json(DataTableToJSONWithStringBuilder(dt), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Show(int? StateId)
        {
            dict.Clear();
            dict.Add("@stateid", StateId);
            //  dict.Add("@mode", "searchOrderItem");
            DataSet ds = commonClass.return_dataset(dict, "procPivot");
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0) //if data is there in dt(dataTable)  
            {
                foreach (DataRow dr in dt.Rows)
                {
                    rowelement = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        rowelement.Add(col.ColumnName, dr[col].ToString()); //adding columnn
                    }
                    rows.Add(rowelement);
                }
            }
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        
        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }

        [HttpGet]
        public JsonResult GetPaymentDetails(int Itemid) 
        {
            order order = new order();
            if (Itemid > 0)
            {
                dict.Clear();
                dict.Add("@itemId",Itemid);
                dict.Add("@mode", "GetPaymentDetails");
                DataTable dt = commonClass.return_datatable(dict, "proc_order");
                if (dt.Rows.Count > 0)
                {
                    order.id = Convert.ToInt32(dt.Rows[0]["orderId"]);
                    order.Payment_mode = dt.Rows[0]["Payment_mode"].ToString();
                    order.payment_remark = dt.Rows[0]["Payment_Remark"].ToString();
                    order.paymentNumber = dt.Rows[0]["PaymodeNumber"].ToString();
                }
            }
            return Json(order, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditPaymodeDetail(order ord)
        {
            dict.Clear();
            dict.Add("@id",ord.id);
            dict.Add("@Payment_mode",ord.Payment_mode);
            dict.Add("@paymentNumber",ord.paymentNumber);
            dict.Add("@Payment_Remark",ord.payment_remark);
            dict.Add("@mode", "EditPaymode");
            int i = commonClass.return_nonquery(dict, "proc_order");
            return Json(i, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetItemDetails(int Id)
        {
            List<item_Model> itemlist = new List<item_Model>();
            dict.Clear();
            dict.Add("@id", Id);
            dict.Add("@mode", "GetOrderItem");
            DataTable dt = commonClass.return_datatable(dict, "proc_item");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    item_Model item = new item_Model();
                    item.item_id = Convert.ToInt32(dr["id"]);
                    item.item_name = dr["Item_Name"].ToString();
                    item.item_qty = Convert.ToInt32(dr["item_qty"]);
                    item.item_rate = Convert.ToInt32(dr["item_rate"]);
                    item.item_total_amount = Convert.ToInt32(dr["Total_item_price"]);
                    item.id = Convert.ToInt32(dr["orderId"]);
                    itemlist.Add(item);
                }
            }
            return Json(itemlist, JsonRequestBehavior.AllowGet);
        }

        public int GetItemID(string prifix)
        {
            dict.Clear();
            dict.Add("@mode", "getIDfromItemName");
            dict.Add("@Item_Name", prifix);
            DataTable dt = commonClass.return_datatable(dict, "proc_item");
            Item_Model item = new Item_Model();
            if (dt.Rows.Count > 0)
            {
                item.id = Convert.ToInt32(dt.Rows[0]["id"].ToString());
                item.item_name = dt.Rows[0]["Item_Name"].ToString();
            }
            return item.id;
        }
        public JsonResult UpdateItem(List<item_Model> ItemModal,int OrderId) 
        {
            int i = 0;
            if (ItemModal != null)
            {
                int dispatchId = Convert.ToInt32(commonClass.CreateRandomNumber());
                foreach (item_Model item in ItemModal)
                {
                    if (!string.IsNullOrEmpty(item.item_name) && item.item_qty > 0)
                    {
                        int GetId = GetItemID(item.item_name);
                        dict.Clear();
                        dict.Add("@itemId", GetId);//item Name
                        dict.Add("@total_quantity", item.item_qty);
                        dict.Add("@total_rate", item.item_rate);
                        dict.Add("@total_price", item.item_total_amount);
                        dict.Add("@id", item.item_id);
                        dict.Add("@order_id", OrderId);
                        dict.Add("@user_name", Session["user_name"]);
                        dict.Add("@DispatchId", dispatchId);
                        dict.Add("@mode", "UpdateOnlyItems");
                        i = commonClass.return_nonquery(dict, "proc_order");
                    }
                }
            }
            return Json(i,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult deliveryExportTest(string FromDate, string ToDate, int[] deliveryBoy)
        {
            bool status = false;
            List<order> AddOrderList = new List<order>();
            List<order> ShowList = new List<order>();
            DataSet ds = new DataSet();
            dict.Clear();
            dict.Add("@mode", "ExportTEsting");
            dict.Add("@StartDate", FromDate);
            dict.Add("@Enddate", ToDate);
            DataTable dt = commonClass.return_datatable(dict, "proc_common");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    order orders = new order();
                    item_Model items = new item_Model();
                    orders.id = Convert.ToInt32(dr["orderId"]);
                    orders.cust_name = dr["Name"].ToString();
                    orders.cust_mobile = dr["customerMobile"].ToString();
                    orders.Address = dr["Address"].ToString();
                    orders.AreaName = dr["AreaName"].ToString();
                    orders.zoneName = dr["ZoneName"].ToString();
                    items.id = Convert.ToInt32(dr["ItemId"]);
                    items.item_name = dr["Item_Name"].ToString();
                    items.item_qty = Convert.ToInt32(dr["TotalQty"]);
                    items.item_total_amount = Convert.ToInt32(dr["TotalAmount"]);
                    items.coupon = dr["Coupon"].ToString();
                    items.deliveryRemark = dr["deliveryStatus"].ToString();
                    orders.ItemDetails.Add(items);
                    orders.Payment_mode = dr["Payment_mode"].ToString();
                    orders.payment_remark = dr["Remark"].ToString();
                    orders.DeliveryAssignId = Convert.ToInt32(dr["AssignDeliveryBoy"]);
                    orders.DeliveryAssignName = dr["DeliveryBoyName"].ToString();
                    orders.DeliveryAssignDate = dr["assignDeliveryDate"].ToString();
                    ShowList.Add(orders);
                }
                if (ShowList.Count > 0)
                {
                    if (deliveryBoy != null)
                    {
                        foreach (var k in deliveryBoy)
                        {
                            var GetName = (from s in ShowList where s.DeliveryAssignId == k select s.DeliveryAssignName).FirstOrDefault();
                            if (GetName != null)
                            {
                                DataTable dtOrder = new DataTable();
                                dtOrder.TableName = GetName;
                                dtOrder.Columns.Add("OrderId", typeof(int));
                                dtOrder.Columns.Add("CustomerName", typeof(string));
                                dtOrder.Columns.Add("cust_mobile", typeof(string));
                                dtOrder.Columns.Add("Address", typeof(string));
                                dtOrder.Columns.Add("AreaName", typeof(string));
                                dtOrder.Columns.Add("zoneName", typeof(string));
                                dtOrder.Columns.Add("Payment_mode", typeof(string));
                                dtOrder.Columns.Add("Remark", typeof(string));
                              
                                dtOrder.Columns.Add("ItemId", typeof(string));
                                dtOrder.Columns.Add("item_name", typeof(string));
                                dtOrder.Columns.Add("item_qty", typeof(int));
                                dtOrder.Columns.Add("item_total_amount", typeof(int));
                                dtOrder.Columns.Add("Coupon", typeof(string));
                                dtOrder.Columns.Add("Delevery/CancelRemark", typeof(string));

                                foreach (var getId in ShowList.Where(d => d.DeliveryAssignId == k).ToList())
                                {
                                    try
                                    {
                                        if (getId.DeliveryAssignId == k)
                                        {
                                            DataRow row = dtOrder.NewRow();
                                            row["OrderId"] = getId.id;
                                            row["CustomerName"] = getId.cust_name;
                                            row["cust_mobile"] = getId.cust_mobile;
                                            row["Address"] = getId.Address;
                                            row["AreaName"] = getId.AreaName;
                                            row["zoneName"] = getId.zoneName;
                                            row["Payment_mode"] = getId.Payment_mode;
                                            row["Remark"] = getId.payment_remark;
                                            row["DeliveryAssignId"] = getId.DeliveryAssignId;
                                            row["DeliveryAssignName"] = getId.DeliveryAssignName;
                                         
                                            foreach (var items in getId.ItemDetails)
                                            {
                                                DataRow row2 = dtOrder.NewRow();
                                                row2["ItemId"] = items.id;
                                                row2["item_name"] = items.item_name;
                                                row2["item_qty"] = items.item_qty;
                                                row2["item_total_amount"] = items.item_total_amount;
                                                row2["Coupon"] = items.coupon;
                                                row2["Delevery/CancelRemark"] = items.deliveryRemark;
                                                dtOrder.Rows.Add(row2);
                                            }

                                        }
                                    }
                                    catch (Exception ex) { string s = ex.ToString(); }
                                }
                                GridView sd = new GridView();
                                sd.DataSource = dtOrder;
                                sd.DataBind();                                                                    
                                ds.Tables.Add(dtOrder);

                            }
                        }
                    }
                }
            }

            if (dt.Rows.Count > 0)
                if (ds.Tables.Count > 0)
                {
                    Session["Order_details"] = null;

                    TempData["ExcelFileName"] = "Assign Order Report";
                    Session["Order_details"] = ds;
                    status = true;
                }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
    }
}