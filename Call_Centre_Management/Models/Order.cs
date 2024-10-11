using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Call_Centre_Management.Models
{
    public class item_Model
    {
        public int id { get; set; }
        public int item_id { get; set; }
        public string item_name { get; set; }
        public int item_qty { get; set; }

        public double totalAmt { get; set; }
        public int Uom_value { get; set; }
        public double item_rate { get; set; }
        public double item_total_amount { get; set; }
        public string coupon { get; set; }
        public string deliveryRemark { get; set; }
        public string Uom { get; set; }
        public string RawItemName { get; set; }
        public int InQty { get; set; }
        public int OutQty { get; set; }
        public string Active { get; set; }
        public string PayCharge { get; set; }
        public double ReciableAmt { get; set; }
    }

    public class Customer
    {
        public Customer()
        {
            CustOldItems = new List<CustOldItems>();
        }
        public int id { get; set; }
        public int mobile { get; set; }
        public string name { get; set; }
        public int Zone_id { get; set; }
        public int Area_id { get; set; }
        public int State_id { get; set; }
        public string landmark { get; set; }
        public string Address { get; set; }
        public int pincode { get; set; }
        public string CustAPimobile { get; set; }
        public string CustAPiEmail { get; set; }

        public List<CustOldItems> CustOldItems { get; set; }
    }

    public class CustOldItems
    {
        public string ItemName { get; set; }
        public string quantity { get; set; }
        public string date { get; set; }
        public string Status { get; set; }
    }

    public class order
    {
        public order()
        {
            ItemDetails = new List<item_Model>();
        }

        public int id { get; set; }
        public string cust_mobile { get; set; }
        public string cust_name { get; set; }
        public int Zone_id { get; set; }
        public string zoneName { get; set; }
        public int Area_id { get; set; }
        public string AreaName { get; set; }
        public int State_id { get; set; }
        public string stateName { get; set; }
        public string landmark { get; set; }
        public string Address { get; set; }
        public int pincode { get; set; }
        public double Total_Price { get; set; }
        public int total_qty { get; set; }
        public string Payment_mode { get; set; }
        public string payment_remark { get; set; }
        public string paymentNumber { get; set; }
        public string Remark { get; set; }
        public int CallerId { get; set; }
        public int DeliveryAssignId { get; set; }
        public string DeliveryAssignName { get; set; }
        public string DeliveryAssignDate { get; set; }
        public string callerName { get; set; }
        public string Source { get; set; }
        public string insertedDate { get; set; }
        public string ItemCoupon { get; set; }

        public List<item_Model> ItemDetails { get; set; }
    }

    public class orderDetails_model

    {
        public string state { get; set; }
        public string stateId { get; set; }
        public SelectList states { get; set; }
        public string zone { get; set; }
        public string zoneId { get; set; }
        public string Area { get; set; }
        public string AreaId { get; set; }

        public string id { get; set; }

        public string orderItemId { get; set; }
        public string OrderId { get; set; }

        public string customerName { get; set; }
        public string customerMobile { get; set; }
        public string itemName { get; set; }
        public double itemRate { get; set; }
        public int itemQty { get; set; }
        public double totalPrice { get; set; }
        public string deliveryAddres { get; set; }
        public string bookingDate { get; set; }
        public string deliveryStatus { get; set; }
        public int CallerId { get; set; }
        public string callerName { get; set; }

    }

    public class orderExportModel
    {
        public string Caller { get; set; }
        public string Date { get; set; }
        public string OrderFrom { get; set; }
        public string CustomerName { get; set; }
        public string Number { get; set; }
        public string HouseNumber { get; set; }
        public string Area { get; set; }
        public string Zone { get; set; }
        public string Pincode { get; set; }
        public string OrderQty { get; set; }
        public string Amount { get; set; }
        public string Coupon { get; set; }
        public string PaymentMode { get; set; }
        public string Remarks { get; set; }

    }

    public class todayOrder
    {
        public string Category { get; set; }
        public int order { get; set; }
        public string Uom { get; set; }
        public int Value { get; set; }

    }

    public class FillterOrder
    {
        public List<int?> StateId { get; set; }
        public List<int?> ZoneId { get; set; }
        public List<int?> AreaId { get; set; }
        public List<string> InsertedDate { get; set; }
        public List<int?> ItemId { get; set; }

    }


    public class displayorder
    {
        public int id { get; set; }
        public int orderItemId { get; set; }
        public string cust_mobile { get; set; }
        public string cust_name { get; set; }
        public int Zone_id { get; set; }
        public string zoneName { get; set; }
        public int Area_id { get; set; }
        public string AreaName { get; set; }
        public int State_id { get; set; }
        public string stateName { get; set; }
        public string landmark { get; set; }
        public string Address { get; set; }
        public int pincode { get; set; }
        public double Total_Price { get; set; }
        public int total_qty { get; set; }
        public string Payment_mode { get; set; }
        public string payment_remark { get; set; }
        public string Remark { get; set; }
        public string ItemStatus { get; set; }
        public int CallerId { get; set; }
        public string callerName { get; set; }
        public string Source { get; set; }
        public string InsertedDate { get; set; }
        public string Coupon { get; set; }
        public string DeliveryAssignBy { get; set; }
        public string DeliveryAssignDate { get; set; }
        public int DeliveryAssignId { get; set; }

        public int ItemId { get; set; }
        public string item_name { get; set; }
        public int item_qty { get; set; }
        public double item_rate { get; set; }
        public double item_total_amount { get; set; }
        public string item_Active { get; set; }
        public string CategoryName { get; set; }
        public string deliveryDate { get; set; }

    }

    public class OrderStatusRecord
    {
        public string Pendingdays { get; set; }
        public int pandingSumofqty { get; set; }
        public int AssignOrder { get; set; }
        public string Deliverydays { get; set; }
        public int DeliverySumofqty { get; set; }
    }


    public class callerdashModel
    {
        public int id { get; set; }
        public string CallerName { get; set; }
        public int TotalCall { get; set; }
        public int IntrestedCall { get; set; }
        public int totalorder { get; set; }
        public int TotalAmount { get; set; }
        public string Break { get; set; }
        public string workingHour { get; set; }
        public string breakHour { get; set; }
        public string disable { get; set; }
    }

    public class OrderApproval
    {
        public int id { get; set; }
        public string Mobile { get; set; }
        public int orderid { get; set; }
        public string custName { get; set; }
        public string caller { get; set; }
        public string Assigncaller { get; set; }
        public string insertedDate { get; set; }
        public string insertedby { get; set; }
        public string custMobile { get; set; }
        public string totalAmount { get; set; }
        public string totalQty { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
        public string Comment { get; set; }
        public string StatusName { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string ApprovalRemark { get; set; }
    }


    public class Banner {
    public int Id { get; set; }
        public string banner { get; set; }
        public string bannerpath { get; set; }
    }


    public class Areas
    {
        public int Id { get; set; }
        public string area { get; set; }
        public string zone_id { get; set; }
    }

    public class State
    {
        public int Id { get; set; }
        public string state { get; set; }

    }

    public class Zone
    {
        public int id { get; set; }
        public string zone { get; set; }
        public string state_id { get; set; }
    }

}

