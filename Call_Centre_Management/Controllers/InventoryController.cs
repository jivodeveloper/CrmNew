using Call_Centre_Management.Classes;
using Call_Centre_Management.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace Call_Centre_Management.Controllers
{
    public class InventoryController : Controller
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        Common_Class common_class = new Common_Class();
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();

        [HttpGet]
        public ActionResult InventoryDetails()
        {
           
            List<displayorder> Orders = new List<displayorder>();
            try
            {
                dict.Clear();
                dict.Add("@mode", "DeliveryInventory");
                ds = common_class.return_dataset(dict, "proc_order");
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
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
                var DeliveryBoyList = GetDeliveryBoys();
                ViewBag.GetShowOrder = Orders;
                ViewBag.DeliveryBoyDetails = DeliveryBoyList;
            }
            catch(Exception ex) { }
            return View();
        }

        public ActionResult DailyReport()
        {

            List<displayorder> Orders = new List<displayorder>();
            try
            {
                dict.Clear();
                dict.Add("@mode", "DeliveryInventory");
                ds = common_class.return_dataset(dict, "proc_order");
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
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
                var DeliveryBoyList = GetDeliveryBoys();
                ViewBag.GetShowOrder = Orders;
                ViewBag.DeliveryBoyDetails = DeliveryBoyList;
            }
            catch (Exception ex) { }
            return View();
        }

        [HttpGet]
        public List<Employees_Modal> GetDeliveryBoys()
        {
            List<Employees_Modal> DeliveryBoyList = new List<Employees_Modal>();
            try
            {
                dict.Clear();
                dict.Add("@mode", "GetDeliveryboyDetails");
                ds = common_class.return_dataset(dict, "proc_order");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Employees_Modal DeliveryBoys = new Employees_Modal();
                        DeliveryBoys.id = Convert.ToInt32(dr["DeliveryBoyId"]);
                        DeliveryBoys.fullName = dr["Name"].ToString();
                        DeliveryBoyList.Add(DeliveryBoys);
                    }
                }
            }
            catch (Exception ex) { }


            return DeliveryBoyList;
        }

        [HttpPost]
        public ActionResult InventoryCreate(int[] GetItemId, int DeliveryBoyId)
        {
            List<item_Model> itemlist = new List<item_Model>();
            try
            {
                if (GetItemId != null)
                {
                    foreach (int ItemId in GetItemId)
                    {
                        dict.Clear();
                        dict.Add("@mode", "InsertTempItemId");
                        dict.Add("@itemId", ItemId);
                        common_class.return_nonquery(dict, "proc_order");
                    }

                    dict.Clear();
                    dict.Add("@AssignDeliveryBoy", DeliveryBoyId);
                    dict.Add("@mode", "GenreateInventroy");
                    dt = common_class.return_datatable(dict, "proc_order");
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            item_Model item = new item_Model();
                            item.item_qty = Convert.ToInt32(dr["totalqty"]);
                            item.RawItemName = Convert.ToString(dr["rawItem"]);
                            itemlist.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return Json(itemlist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InventoryCreate1(int[] GetItemId, int DeliveryBoyId)
        {
            List<item_Model> itemlist = new List<item_Model>();
            try
            {
                if (GetItemId != null)
                {
                    foreach (int ItemId in GetItemId)
                    {
                        dict.Clear();
                        dict.Add("@mode", "InsertTempItemId");
                        dict.Add("@itemId", ItemId);
                        common_class.return_nonquery(dict, "proc_order");
                    }

                    dict.Clear();
                    dict.Add("@AssignDeliveryBoy", DeliveryBoyId);
                    dict.Add("@mode", "GenreateInventroy1");
                    dt = common_class.return_datatable(dict, "proc_order");
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            item_Model item = new item_Model();
                            item.item_qty = Convert.ToInt32(dr["totalqty"]);
                            item.RawItemName = Convert.ToString(dr["rawItem"]);
                            item.totalAmt = Convert.ToDouble(dr["totalAmt"]);
                            itemlist.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return Json(itemlist, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateVerificationStatus(int[] selectedItems)
        {
            try
            {
                if (selectedItems != null && selectedItems.Length > 0)
                {
                    // Iterate through the selected items and update verification status to 'Yes'
                    foreach (int ItemId in selectedItems)
                    {
                        dict.Clear();
                        dict.Add("@mode", "UpdateVerificationStatus");
                        dict.Add("@ItemId", ItemId);
                        dict.Add("@VerificationStatus","yes");
                        common_class.return_nonquery(dict, "proc_order");
                    }

                    return Json(new { success = true, message = "Verification status updated successfully." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "No item IDs provided." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions if needed
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult DispatchOrder(int?[] GetItemId, int DeliveryBoyId,int?[] SelectedOreder)
        {
            int i = 0;
            List<displayorder> Orders = new List<displayorder>();
            if (GetItemId != null)
            {
                foreach (int ItemId in GetItemId)
                {
                    dict.Clear();
                    dict.Add("@AssignDeliveryBoy", DeliveryBoyId);
                    dict.Add("@itemId", ItemId);
                    dict.Add("@mode", "CreateInventroy");
                    i = common_class.return_nonquery(dict, "proc_order");
                }   
                if (i > 0)
                {
                    dict.Clear();
                    dict.Add("@AssignDeliveryBoy", DeliveryBoyId);
                    dict.Add("@mode","DispatchOrder");
                    ds = common_class.return_dataset(dict, "proc_order");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
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
                }
            }

            if (SelectedOreder != null)
            {
                int dispatchId=Convert.ToInt32(common_class.CreateRandomNumber());
                foreach (int ItemsId in SelectedOreder)
                {
                    dict.Clear();
                    dict.Add("@AssignDeliveryBoy", DeliveryBoyId);
                    dict.Add("@itemId", ItemsId);
                    dict.Add("@DispatchId", dispatchId);
                    dict.Add("@mode", "CreateDispatchInventory");
                    i = common_class.return_nonquery(dict, "proc_order");
                }
            }

            return Json(Orders, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CancelItem(int[] GetItemId, int DeliveryBoyId)
        {
            int i = 0;
            List<displayorder> Orders = new List<displayorder>();
            try
            {
                if (GetItemId != null)
                {
                    foreach (int ItemId in GetItemId)
                    {
                        dict.Clear();
                        dict.Add("@AssignDeliveryBoy", DeliveryBoyId);
                        dict.Add("@itemId", ItemId);
                        dict.Add("@mode", "CancelItem");
                        i = common_class.return_nonquery(dict, "proc_order");
                    }
                    if (i > 0)
                    {
                        dict.Clear();
                        dict.Add("@AssignDeliveryBoy", DeliveryBoyId);
                        dict.Add("@mode", "DispatchOrder");
                        ds = common_class.return_dataset(dict, "proc_order");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
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
                    }
                }
            }
            catch(Exception ex) { }
            return Json(Orders, JsonRequestBehavior.AllowGet);
        }
    }
}