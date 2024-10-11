using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Call_Centre_Management.Models
{
    public class UserResponseModel
    {
        public int id { get; set; }
        public string fullName { get; set; }
    }

    public class DeliveryPanelModel
    {
        public string userName { get; set; }
        public string password { get; set; }
    }

    public class CustomerDetailModel
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string custName { get; set; }
        public string custMobile { get; set; }
        public int zoneId { get; set; }
        public string zoneName { get; set; }
        public int areaId { get; set; }
        public string areaName { get; set; }
        public int stateId { get; set; }
        public string stateName { get; set; }
        

        public int  orderId { get; set; }
//public string landmark { get; set; }
        public string address { get; set; }
//        public string pincode { get; set; }
//        public decimal totalPrice { get; set; }
//        public int totalQty { get; set; }
        
 //       public string remark { get; set; }
//        public int callerId { get; set; }
//        public int deliveryAssignId { get; set; }
//        public string deliveryAssignName { get; set; }
//        public string deliveryAssignDate { get; set; }
//        public string callerName { get; set; }
//        public string source { get; set; }
        public string insertedDate { get; set; }
 //       public string itemCoupon { get; set; }

        public ItemList itemList { get; set; } = new ItemList();
    }
    public class ItemList
    {
        public int itemId { get; set; }

        public int orderId { get; set; }
        public string itemName { get; set; }
        public string userName { get; set; }
        public int itemQty { get; set; }
        public int uomValue { get; set; }
        //public string Payment_mode { get; set; }
        //public string paymentAmount { get; set; }
        //public string p_reference_Number { get; set; }
        public float itemRate { get; set; }

        public int odrItemId { get; set; }
        public float itemTotalAmount { get; set; }

        //public string coupon {  get; set; }

        //public string deliveryRemark { get; set; }

        public string uom {  get; set; }

        //public string rawItemName { get; set; }

        //public int inQty { get; set; }

        //public int outQty { get; set; }

        public string active {  get; set; }

        //public string payCharge { get; set; }

        //public float reciableAmt { get; set; }

        //public bool IsSelect { get; set; }

        //public int c_id { get; set; }

        public string status { get; set; }

    }
    public class DeliveryStatusRequest
    {
        public string itemidJSON { get; set; }
        // public int OID { get; set; }
        public string NewStatus { get; set; }

        public int OID { get; set; }

        public string userName { get; set; }

        public string Remark { get; set; }
    }

    public class PaymentStatusRequest
    {
        public string reference_id { get; set; }

        public double t_amt { get; set; }

        public string payment_option { get; set; }

        public int orderId { get; set; }

        //public int itemId { get; set; }
    }
    public class AssignOrderRequest
    {
       public int userId { get; set; }

        public string orderidJSON { get; set; }

     
    }

    public class AllOrderDetailRequest 
    {
        public int orderId { get; set; }
        public string custName { get; set; }
        public string custMobile { get; set; }
        public int zoneId { get; set; }
        public string zoneName { get; set; }
        public int areaId { get; set; }
        public string areaName { get; set; }
        public int stateId { get; set; }
        public string address {  get; set; }

        public string itemName { get; set; }
        public string stateName { get; set; }


    }
}
