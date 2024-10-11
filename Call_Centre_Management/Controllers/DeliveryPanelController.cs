using Call_Centre_Management.Classes;
using Call_Centre_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;


namespace Call_Centre_Management.Controllers
{
    public class DeliveryPanelController : Controller
    {   
        public ActionResult Index()
        {
            return View();
        }
            
        [HttpPost]
        public JsonResult LoginSalesPerson(string user, string password)
        {
            UserResponseModel model= new UserResponseModel();
            string Errormsg = string.Empty;
            DeliveryPanelClass obj = new DeliveryPanelClass();
            //password = Encryption.Encrypt(password);
            DeliveryPanelModel param = new DeliveryPanelModel()
            {
                userName = user,
                password = password
            };

            model = obj.LoginSalesPerson(param);
            if (model.fullName != null)
            {

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
            
        [HttpPost]
        public JsonResult Customerdetails(List<int> userIds)
        {

            string Errormsg = string.Empty;
            DeliveryPanelClass obj = new DeliveryPanelClass();

            Dictionary<int, List<CustomerDetailModel>> customerItems = new Dictionary<int, List<CustomerDetailModel>>();

            foreach (int userId in userIds)
            {
                CustomerDetailModel param = new CustomerDetailModel()
                {
                    orderId = userId,
                };

                List<CustomerDetailModel> customers = obj.Customerdetails(param);

                if (customers != null && customers.Count > 0)
                {

                    foreach (var customer in customers)
                    {
                        if (!customerItems.ContainsKey(customer.orderId))
                        {
                            customerItems[customer.orderId] = new List<CustomerDetailModel>();
                        }

                        customerItems[customer.orderId].Add(customer);
                    }
                }
                else
                {
                    Console.WriteLine($"No customer details found for User ID: {userId}");
                }
            }

            List<object> dataArray = new List<object>();

            foreach (var kvp in customerItems)
            {
                var customerData = kvp.Value.FirstOrDefault();
                var itemList = kvp.Value.Select(c => c.itemList).ToList();

                var customerGroup = new 
                {
                        //userId = customerData.userId,
                        id = customerData.id,
                        orderId = customerData.orderId,
                        custName = customerData.custName,
                        custMobile = customerData.custMobile,
                        zoneId = customerData.zoneId,
                        zoneName = customerData.zoneName,
                        areaId = customerData.areaId,
                        areaName = customerData.areaName,
                        stateId = customerData.stateId,
                        stateName = customerData.stateName,
                        //landmark = customerData.landmark,
                        address = customerData.address,
                        //pincode = customerData.pincode,
                        //totalPrice = customerData.totalPrice,
                        //totalQty = customerData.totalQty,
                        //remark = customerData.remark,
                        //callerId = customerData.callerId,
                        //deliveryAssignId = customerData.deliveryAssignId,
                        //deliveryAssignName = customerData.deliveryAssignName,
                        //deliveryAssignDate = customerData.deliveryAssignDate,
                        //callerName = customerData.callerName,
                        //source = customerData.source,
                        insertedDate = customerData.insertedDate,
                        //itemCoupon = customerData.itemCoupon,
                        ItemList = itemList,
                };

                dataArray.Add(customerGroup);
            }

            if (dataArray.Count > 0)
            {
                return Json(dataArray, JsonRequestBehavior.AllowGet);
            }

            return Json(new { error = "No valid customer details found." }, JsonRequestBehavior.AllowGet);
        }

        /*  [HttpPost]
          public JsonResult UpdateDeliveryStatus(int OID, string newStatus)
          {
              DeliveryPanelClass obj = new DeliveryPanelClass();

              DeliveryStatusRequest request = new DeliveryStatusRequest
              {
                  OID = OID,
                  NewStatus = newStatus
              };

              try
              {
                  obj.UpdateDeliveryStatus(request);
                  return Json(new {  Message = "Delivery status updated successfully." });
              }
              catch (Exception ex)
              {
                  return Json(new {  Message = $"Error updating delivery status: {ex.Message}" });
              }
          }*/
            
        [HttpPost]
        public JsonResult UpdateDeliveryStatus(string[] itemidJSON, string newStatus, int OID)
        {
            DeliveryPanelClass obj = new DeliveryPanelClass();
            //string item = itemidJSON != null ? string.Join(",", itemidJSON) : null;
            string item = itemidJSON != null ? string.Join(",", itemidJSON) : null;
            //string ids = string.Join(",", itemidJSON.Where(x => int.TryParse(x, out _)));

            DeliveryStatusRequest request = new DeliveryStatusRequest
            {
                itemidJSON = item , // Adjust property name
                NewStatus = newStatus,
                OID = OID,
            };

            try
            {
                obj.UpdateDeliveryStatus(request);
                return Json(new { Message = "Delivery status updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { Message = $"Error updating delivery status: {ex.Message}" });
            }
        }
        [HttpPost]
        public JsonResult UpdateDeliveryStatus2(string[] itemidJSON, string newStatus, int OID, string userName,string Remark)
        {
            DeliveryPanelClass obj = new DeliveryPanelClass();
            //string item = itemidJSON != null ? string.Join(",", itemidJSON) : null;
            string item = itemidJSON != null ? string.Join(",", itemidJSON) : null;
            //string ids = string.Join(",", itemidJSON.Where(x => int.TryParse(x, out _)));

            DeliveryStatusRequest request = new DeliveryStatusRequest
            {
                itemidJSON = item, // Adjust property name
                NewStatus = newStatus,
                OID = OID,
                Remark = Remark,
                userName = userName
            };

            try
            {
                obj.UpdateDeliveryStatus(request);
                return Json(new { Message = "Delivery status updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { Message = $"Error updating delivery status: {ex.Message}" });
            }
        }
        [HttpPost]
        public JsonResult GetAllOrderDetail()
        {
            try
            {
                DeliveryPanelClass obj = new DeliveryPanelClass();

                // Call the method in your DeliveryPanelClass
                var orderDetails = obj.GetAllOrderDetail();

                return Json(orderDetails);
            }
            catch (Exception ex)
            {
                return Json(new { error = $"Error: {ex.Message}" });
            }
        }

        
        public JsonResult AssignOrder(int userId, string[] orderidJSON)
        {
            DeliveryPanelClass obj = new DeliveryPanelClass();
            string orderId = orderidJSON != null ? string.Join(",", orderidJSON) : null;
            AssignOrderRequest request = new AssignOrderRequest
            {
                userId = userId,
                orderidJSON = orderId
            };
            try
            {
                obj.AssignOrder(request);
                return Json(new { Message = "Order status updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { Message = $"Error updating delivery status: {ex.Message}" });
            }

        }


        [HttpPost]
        public JsonResult Payment(string reference_id,int t_amt,string payment_option,int orderId)
        {   
            DeliveryPanelClass obj = new DeliveryPanelClass();

            PaymentStatusRequest request = new PaymentStatusRequest
            {
                reference_id = reference_id,
                t_amt = t_amt,
                payment_option = payment_option,
                orderId = orderId
            };
            try
            {   
                obj.Payment(request);
                return Json(new { Message = "Order Assigned successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { Message = $"Error Assigning Order: {ex.Message}" });
            }

        }
            
    }
}


