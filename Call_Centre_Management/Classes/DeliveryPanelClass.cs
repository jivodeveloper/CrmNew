using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Call_Centre_Management.Models;

namespace Call_Centre_Management.Classes
{       
    public class DeliveryPanelClass 
    {           
        Dictionary<string, object> dict = new Dictionary<string, object>();
        Common_Class common_class = new Common_Class();
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
                
        public UserResponseModel LoginSalesPerson(DeliveryPanelModel param)
        {   
            Common_Class commonClass = new Common_Class();

            List<SqlParameter> sqlParams = new List<SqlParameter>();
            UserResponseModel userResponse = new UserResponseModel();
            sqlParams.Add(new SqlParameter("@userName", param.userName));
            sqlParams.Add(new SqlParameter("@password", commonClass.Encode(param.password)));
            DataSet ds = DataAccess.RunProc("LoginDeliveryPanel", sqlParams);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                userResponse.id = Convert.ToInt32(row["id"]);
                userResponse.fullName = row["fullName"].ToString();
            }

            return userResponse;
        }
                
        public static T ConvertValue<T>(object value)
        {
            if (value == DBNull.Value || value == null)
            {
                return default(T);
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
                
        public List<item_Model> InventoryCreate(int[] GetItemId, int DeliveryBoyId)
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
            return itemlist;
        }
                
        public List<CustomerDetailModel> Customerdetails(CustomerDetailModel param)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            List<CustomerDetailModel> customerList = new List<CustomerDetailModel>();

            sqlParams.Add(new SqlParameter("@userId", param.orderId));
            DataSet ds = DataAccess.RunProc("Customerdetail", sqlParams);

            Console.WriteLine($"Executing stored procedure 'Customerdetail' with UserId: {param.orderId}");
            Console.WriteLine($"DataSet contains {ds?.Tables.Count} table(s)");

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    CustomerDetailModel customer = new CustomerDetailModel();

                    customer.id = ConvertValue<int>(row["id"]);
                    customer.orderId = ConvertValue<int>(row["orderId"]);
                    customer.custName = ConvertValue<string>(row["custName"]);
                    customer.custMobile = ConvertValue<string>(row["custMobile"]);
                    customer.zoneId = ConvertValue<int>(row["zoneId"]);
                    customer.zoneName = ConvertValue<string>(row["zoneName"]);
                    customer.areaId = ConvertValue<int>(row["areaId"]);
                    customer.areaName = ConvertValue<string>(row["areaName"]);
                    customer.stateId = ConvertValue<int>(row["stateId"]);
                    customer.stateName = ConvertValue<string>(row["stateName"]);
                    //customer.landmark = ConvertValue<string>(row["landmark"]);
                    customer.address = ConvertValue<string>(row["address"]);
                    //customer.pincode = ConvertValue<string>(row["pincode"]);
                    //customer.totalPrice = ConvertValue<int>(row["totalPrice"]);
                    //customer.totalQty = ConvertValue<int>(row["totalQty"]);
                    //customer.remark = ConvertValue<string>(row["remark"]);
                    //customer.callerId = ConvertValue<int>(row["callerId"]);
                    //customer.deliveryAssignId = ConvertValue<int>(row["deliveryAssignId"]);
                    //customer.deliveryAssignName = ConvertValue<string>(row["deliveryAssignName"]);
                    //customer.deliveryAssignDate = ConvertValue<string>(row["deliveryAssignDate"]);
                    //customer.callerName = ConvertValue<string>(row["callerName"]);
                    //customer.source = ConvertValue<string>(row["source"]);
                    customer.insertedDate = ConvertValue<string>(row["insertedDate"]);
                    //customer.itemCoupon = ConvertValue<string>(row["itemCoupon"]);
                    customer.itemList.odrItemId = ConvertValue<int>(row["odrItemId"]);
                    customer.itemList.itemId = ConvertValue<int>(row["itemId"]);
                    customer.itemList.itemName = ConvertValue<string>(row["itemName"]);
                    customer.itemList.itemQty = ConvertValue<int>(row["itemQty"]);
                    customer.itemList.uomValue = ConvertValue<int>(row["uomValue"]);
                    customer.itemList.itemRate = ConvertValue<float>(row["itemRate"]);
                    customer.itemList.itemTotalAmount = ConvertValue<int>(row["itemTotalAmount"]);
                    customer.itemList.orderId = ConvertValue<int>(row["orderId"]);
                    //customer.itemList.coupon = ConvertValue<string>(row["coupon"]);
                    //customer.itemList.deliveryRemark = ConvertValue<string>(row["deliveryRemark"]);
                    customer.itemList.uom = ConvertValue<string>(row["uom"]);
                    //customer.itemList.paymentMode = ConvertValue<string>(row["paymentMode"]);
                    //customer.itemList.paymentAmount = ConvertValue<string>(row["paymentAmount"]);
                    //customer.itemList.p_reference_Number = ConvertValue<string>(row["p_reference_Number"]);
                    //customer.itemList.rawItemName = ConvertValue<string>(row["rawItemName"]);
                    //customer.itemList.inQty = ConvertValue<int>(row["inQty"]);
                    //customer.itemList.outQty = ConvertValue<int>(row["outQty"]);
                    customer.itemList.active = ConvertValue<string>(row["active"]);
                    //customer.itemList.payCharge = ConvertValue<string>(row["payCharge"]);
                    //customer.itemList.reciableAmt = ConvertValue<int>(row["reciableAmt"]);
                    //customer.itemList.IsSelect = ConvertValue<bool>(row["IsSelect"]);
                    //customer.itemList.c_id = ConvertValue<int>(row["c_id"]);
                    //customer.itemList.status = ConvertValue<string>(row["status"]);
                    customerList.Add(customer);
                }
            }
            return customerList;
        }
                
        /* public void UpdateDeliveryStatus(DeliveryStatusRequest request)
         {
             List<SqlParameter> sqlParams = new List<SqlParameter>();

             sqlParams.Add(new SqlParameter("@OID", request.OID));
             sqlParams.Add(new SqlParameter("@NewStatus", request.NewStatus));

             DataAccess.RunProc("deliveryStatus", sqlParams);
        }*/
            
        public void UpdateDeliveryStatus(DeliveryStatusRequest request)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();

            sqlParams.Add(new SqlParameter("@itemidJSON", request.itemidJSON));
            sqlParams.Add(new SqlParameter("@NewStatus", request.NewStatus));
            sqlParams.Add(new SqlParameter("@OID", request.OID));
            sqlParams.Add(new SqlParameter("@Remark", request.Remark));
            sqlParams.Add(new SqlParameter("@userName", request.userName));


            DataAccess.RunProc("deliveryStatus", sqlParams);
        }
                
        public void Payment(PaymentStatusRequest request)
        {       
            List<SqlParameter> sqlParams = new List<SqlParameter>();

            sqlParams.Add(new SqlParameter("@reference_id", request.reference_id));
            sqlParams.Add(new SqlParameter("@t_amt", request.t_amt));
            sqlParams.Add(new SqlParameter("@payment_option", request.payment_option));
            sqlParams.Add(new SqlParameter("@orderId", request.orderId));
            //sqlParams.Add(new SqlParameter("@itemId", request.itemId));

            DataAccess.RunProc("PaymentStatus", sqlParams);
        }

        public void AssignOrder(AssignOrderRequest request)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();

            sqlParams.Add(new SqlParameter("@userId", request.userId));
            sqlParams.Add(new SqlParameter("@orderidJSON", request.orderidJSON));
            //sqlParams.Add(new SqlParameter("@itemId", request.itemId));

            DataAccess.RunProc("orderAssign", sqlParams);
        }

        public List<AllOrderDetailRequest> GetAllOrderDetail()
        {
            List<AllOrderDetailRequest> orderList = new List<AllOrderDetailRequest>();
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            DataSet ds = DataAccess.RunProc("getAllOrderDetail", sqlParams);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    AllOrderDetailRequest order = new AllOrderDetailRequest();

                    order.orderId = ConvertValue<int>(row["orderId"]);
                    order.custName = ConvertValue<string>(row["custName"]);
                    order.custMobile = ConvertValue<string>(row["custMobile"]);
                    order.zoneId = ConvertValue<int>(row["zoneId"]);
                    order.zoneName = ConvertValue<string>(row["zoneName"]);
                    order.areaId = ConvertValue<int>(row["areaId"]);
                    order.areaName = ConvertValue<string>(row["areaName"]);
                    order.stateId = ConvertValue<int>(row["stateId"]);
                    order.address = ConvertValue<string>(row["address"]);
                    order.itemName = ConvertValue<string>(row["itemName"]);
                    order.stateName = ConvertValue<string>(row["stateName"]);

                    orderList.Add(order);
                }
            }
            return orderList;
        }
        
    }
}

    