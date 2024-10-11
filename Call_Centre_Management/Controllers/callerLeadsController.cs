using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Call_Centre_Management.Classes;
using System.Data;
using System.Data.SqlClient;
using Call_Centre_Management.Models;
using System.Data.OleDb;
using System.Configuration;
using System.IO;
using ExcelDataReader;

namespace Call_Centre_Management.Controllers
{
    public class callerLeadsController : Controller
    {
        Common_Class commonclass = new Common_Class();
        Dictionary<string, object> dict = new Dictionary<string, object>();
        public ActionResult CallerLeads()
        {
            return View();
        }
        public ActionResult fetchLeadData()
        {
            List<order> cust = new List<order>();
            List<order> cust1 = new List<order>();
            List<order> cust2 = new List<order>();
            List<order> cust3 = new List<order>();
            dict.Clear();
            dict.Add("@mode", "fetchLead");
            dict.Add("@Callerid", Convert.ToInt32(Session["Empid"].ToString()));
            dict.Add("@roleid", Convert.ToInt32(Session["Role"].ToString()));
            DataSet ds = commonclass.return_dataset(dict, "Proc_CallerLeads");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        order odr = new order();
                        odr.cust_mobile = row["Mobile"].ToString();
                        cust.Add(odr);
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        order odr = new order();
                        odr.cust_mobile = row["Mobile"].ToString();
                        odr.Source = row["Response"].ToString();
                        cust1.Add(odr);
                    }
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[2].Rows)
                    {
                        order odr = new order();
                        odr.cust_mobile = row["TotalCall"].ToString();
                        cust2.Add(odr);
                    }
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[3].Rows)
                    {
                        order odr = new order();
                        odr.cust_mobile = row["Daily"].ToString();
                        cust3.Add(odr);
                    }
                }
            }
            //ViewBag.custmobile = cust;
            return Json(new { cust, cust1, cust2, cust3 }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet] 
        public ActionResult getItem()
        {
            List<Item_Model> item_list = new List<Item_Model>();
            dict.Clear();
            dict.Add("@mode", "Getall_Items");
            DataTable dt = commonclass.return_datatable(dict, "proc_item");
            int j = dt.Rows.Count;
            if (j > 0)
            {
                for (int i = 0; i < j; i++)
                {
                    Item_Model item = new Item_Model();
                    item.id = Convert.ToInt32(dt.Rows[i]["id"].ToString());
                    item.Item_id = dt.Rows[i]["item_id"].ToString();
                    item.item_name = dt.Rows[i]["Item_Name"].ToString();
                    item.category = dt.Rows[i]["category"].ToString();
                    item.sub_category = dt.Rows[i]["Sub_category"].ToString();
                    item.UOM = dt.Rows[i]["Uom"].ToString();
                    item.UOM_value = Convert.ToInt32(dt.Rows[i]["Uom_value"].ToString());
                    item.rate = Convert.ToDouble(dt.Rows[i]["rate"].ToString());
                    item.Scheme_values = Convert.ToInt32(dt.Rows[i]["Scheme_value"].ToString());
                    item.scheme_qty = Convert.ToInt32(dt.Rows[i]["Scheme_Quantity"].ToString());
                    item.gst = Convert.ToInt32(dt.Rows[i]["Gst"].ToString());
                    item_list.Add(item);
                }
            }
            return Json(item_list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LeadsUpload()
        {
            List<Employees_Modal> GetEmployeeList = new List<Employees_Modal>();
            dict.Clear();
            dict.Add("@mode", "GetCallerList");
            DataTable dt = commonclass.return_datatable(dict, "Proc_CallerLeads");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Employees_Modal employees = new Employees_Modal();
                    employees.id = Convert.ToInt32(dr["id"]);
                    employees.fullName = dr["fullName"].ToString();
                    GetEmployeeList.Add(employees);
                }
            }
            ViewBag.GetEmployeeList = GetEmployeeList;
            return View();
        }

        [HttpPost]
        public ActionResult ImportExcel(HttpPostedFileBase postedFile, int EmployeeId)
        {
            DataTable dt = new DataTable();
            //bool check = false;
            if (postedFile != null && EmployeeId != 0)
            {
                var fileExtension = System.IO.Path.GetExtension(postedFile.FileName);
                if (fileExtension == ".csv")
                {
                    using (var reader = ExcelReaderFactory.CreateCsvReader(postedFile.InputStream))
                    {
                        var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true // To set First Row As Column Names    
                            }
                        });

                        if (dataSet.Tables.Count > 0)
                        {
                            var dataTable = dataSet.Tables[0];
                            dt.Columns.Add("ContactNumber", typeof(Int64));
                            foreach (DataRow objDataRow in dataTable.Rows)
                            {
                                if (objDataRow.ItemArray.All(x => string.IsNullOrEmpty(x?.ToString()))) continue;
                                {
                                    DataRow row = dt.NewRow();
                                    row["ContactNumber"] = Convert.ToInt64(objDataRow["ContactNumber"]);
                                    dt.Rows.Add(row);
                                }
                            }
                        }
                    }

                    dict.Clear();
                    dt.TableName = "CallerLeads";
                    var dt1 = commonclass.GetXmlDoc(dt);
                    dict.Add("@Callerid", EmployeeId);
                    dict.Add("@insertedby", Convert.ToString(Session["user_name"]));
                    dict.Add("@doc", dt1);
                    dict.Add("@mode", "AssignImportCallerLeads");
                    int i = commonclass.return_nonquery(dict, "Proc_CallerLeads");
                    if (i > 0)
                    {
                        TempData["Msg"] = String.Format("Record Sucessfully Saved...");
                        return RedirectToAction("LeadsUpload", "callerLeads");
                    }
                    else
                    {
                        TempData["Msg"] = String.Format("Excel Sheet Is Empty Please Fill Excel Sheet...");
                        return RedirectToAction("LeadsUpload", "callerLeads");
                    }
                }
                else
                {
                    TempData["Msg"] = String.Format("Please Select CSV File");
                    return RedirectToAction("LeadsUpload", "callerLeads");
                }
            }
            else
            {
                TempData["Msg"] = String.Format("Please Fill Entities..");
                return RedirectToAction("LeadsUpload", "callerLeads");
            }
            //return View();

        }

        [HttpPost]
        public ActionResult Response(string Respons, string mobile, string followUpDate, string remark, int callHour, int callMinutes, int callSeconds)
        {
            dict.Clear();
            dict.Add("@response", Respons);
            dict.Add("@mobile", mobile);
            dict.Add("@followUpDate", followUpDate);
            dict.Add("@remark", remark);
            dict.Add("@callHour", callHour);
            dict.Add("@callMinutes", callMinutes);
            dict.Add("@callSeconds", callSeconds);
            dict.Add("@Callerid", Convert.ToInt32(Session["Empid"].ToString()));
            dict.Add("@insertedby", Convert.ToString(Session["user_name"]));
            dict.Add("@mode", "UpdateResponse");
            int j = commonclass.return_nonquery(dict, "Proc_CallerLeads");
            return Json(j, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertHoldLogs(string hour, string minutes, string seconds, string Response)
        {
            dict.Clear();
            //dict.Add("@mode", "");
            dict.Add("@Callerid", Session["Empid"]);
            dict.Add("@callHour", Convert.ToInt32(hour));
            dict.Add("@callMinutes", Convert.ToInt32(minutes));
            dict.Add("@callSeconds", Convert.ToInt32(seconds));
            dict.Add("@insertedby", Convert.ToString(Session["user_name"]));
            dict.Add("@remark", Response);
            dict.Add("@mode", "InsertCallerBreakTime");
            int i = commonclass.return_nonquery(dict, "Proc_CallerLeads");
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void InsertHoldLogsonEvent(string GetRemark)
        {

            dict.Clear();
            dict.Add("@mode", "InsertCallerBreakTime");
            dict.Add("@Callerid", Session["Empid"]);
            dict.Add("@insertedby", Convert.ToString(Session["user_name"]));
            dict.Add("@remark", GetRemark.Trim());
            int s = commonclass.return_nonquery(dict, "Proc_CallerLeads");
            //return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DailyCallDashboard()
        {
            return View();
        }
        [HttpPost]
        public ActionResult fetchDailyCallDashboard()
        {
            Session["user_name"] = Session["user_name"];
            Session["Empid"] = Session["Empid"];
            List<callerdashModel> listcallerdash = new List<callerdashModel>();
            dict.Clear();
            dict.Add("@mode", "fetchDailyCallerRecords");
            DataSet ds = commonclass.return_dataset(dict, "Proc_CallerLeads");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        callerdashModel Callerdash = new callerdashModel();
                        Callerdash.id = Convert.ToInt32(dr["id"]);
                        Callerdash.CallerName = Convert.ToString(dr["fullname"]);
                        Callerdash.TotalCall = Convert.ToInt32(dr["totalcall"]);
                        Callerdash.IntrestedCall = Convert.ToInt32(dr["attendcall"]);
                        Callerdash.totalorder = Convert.ToInt32(dr["orders"]);
                        Callerdash.workingHour = Convert.ToString(dr["WorkingTime"]);
                        Callerdash.breakHour = Convert.ToString(dr["BreakTime"]);
                        Callerdash.TotalAmount = Convert.ToInt32(dr["totalAmount"]);
                        if (Convert.ToInt32(dr["onBreak"]) == 1)
                        {
                            Callerdash.Break = "lightcoral";
                        }
                        else
                            Callerdash.Break = "lightgreen";

                        if (Convert.ToInt32(dr["orders"]) == 0)
                        {
                            Callerdash.Break = "gray";
                        }
                        listcallerdash.Add(Callerdash);
                    }
                }
            }
            return Json(listcallerdash, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult checkBreak()
        {
            dict.Add("@Callerid", Convert.ToInt32(Session["Empid"]));
            dict.Add("@mode", "CheckBreak");
            DataTable ds = commonclass.return_datatable(dict, "Proc_CallerLeads");
            return Json(Convert.ToBoolean(ds.Rows[0][0]), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DndMobile(string mobile )
        {
            dict.Clear();
            dict.Add("@mode", "DNDMobile");
            dict.Add("@mobile", mobile);
            dict.Add("@insertedby", Convert.ToString(Session["user_name"].ToString()));
            int i = commonclass.return_nonquery(dict, "Proc_CallerLeads");
            return Json(i, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReferMobile(string mobile,string referMobile)
        {
            dict.Clear();
            dict.Add("@mode", "referNumber");
            dict.Add("@mobile", mobile);
            dict.Add("@referenceMobile", referMobile);
            dict.Add("@insertedby", Convert.ToString(Session["user_name"].ToString()));
            int i = commonclass.return_nonquery(dict, "Proc_CallerLeads");
            return Json(i, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReferMobile1()
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CheckAsignCaller(string mobilenumber, string RadioSelected)
        {
            dict.Clear();
            dict.Add("@mode", "CheckAssignCaller");
            dict.Add("@mobile", mobilenumber);
            dict.Add("@RadioLabelSelected", RadioSelected);
            dict.Add("@Callerid", Convert.ToInt32(Session["Empid"]));
            DataTable dt = commonclass.return_datatable(dict, "Proc_CallerLeads");
            return Json(dt.Rows[0][0], JsonRequestBehavior.AllowGet);
        }
    }
}