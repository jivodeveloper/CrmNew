using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Call_Centre_Management.Models
{
    public class Employees_Modal
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "Enter Full Name")]
        public string fullName { get; set; }

        [Required]
        [Display(Name = "Enter Gender")]
        public string gender { get; set; }

        [Required]
        [Display(Name = "Enter DOB")]
        public string dob { get; set; }

        [Required]
        [Display(Name = "Enter Mobile")]
        public string mobile { get; set; }
        public string email { get; set; }

        [Required]
        [Display(Name = "Enter Full Address")]
        public string address { get; set; }

        [Required]
        [Display(Name = "Enter Area")]
        public string area { get; set; }
        public string Zone { get; set; }

        [Required]
        [Display(Name = "Enter State")]
        public string state { get; set; }
        public string Security { get; set; }
        public string pincode { get; set; }

        [Required]
        [Display(Name = "Enter Username")]
        public string userName { get; set; }

        [Required]
        [Display(Name = "Enter Password")]
        public string password { get; set; }
        public string role { get; set; }

        public bool active { get; set; }


    }
    public class MenuMaster
    {
        public int NodeID { get; set; }
        public string NodeName { get; set; }
        public Nullable<int> ParentID { get; set; }
        public String parentName { get; set; }
        public string insertedDate { get; set; }
        //public string Action { get; set; }
        public bool IsChecked { get; set; }

        public string Status { get; set; }
        public string destination { get; set; }
        public MenuMaster()
        {
            menus = new List<MenuMaster>();
        }
        public List<MenuMaster> menus { get; set; }
    }
    public class Employees
    {
        public string userName { get; set; }
        public string Password { get; set; }
        public int role { get; set; }
        public int id { get; set; }

    }
    public class state
    {
        public int Id { get; set; }
        public string State { get; set; }

    }
    public class Role
    {
        public int Id { get; set; }
        public string role { get; set; }

    }

    public class Item_Model
    {
        public Item_Model()
        {
            subItemList = new List<subItemModel>();
        }
        public int id { get; set; }
        public string Item_id { get; set; }
        public string item_name { get; set; }
        public string category { get; set; }
        public string sub_category { get; set; }
        public string UOM { get; set; }
        public int UOM_value { get; set; }
        public double rate { get; set; }
        public string schemeName { get; set; }
        public double Scheme_values { get; set; }
        public int scheme_qty { get; set; }
        public int gst { set; get; }
        public int Active { set; get; }
        public int item_qty { get; set; }
        public double item_total_amount { get; set; }
        public string DeliveryStatus { get; set; }
        public string fileupload { get; set; }

        public List<subItemModel> subItemList { get; set; }

    }

    public class subItemModel 
    {
    public int id { get; set; }
    public int qty { get; set; }
    public string name { get; set; }
    }

    public class MasterRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string active { get; set; }
        public string InsertedDate { get; set; }
    }
    public class StateMasterModal
    {
        public int Id { get; set; }
        public string StateName { get; set; }
        public string Active { get; set; }
    }
    public class ZoneMastermodal
    {
        public int Id { get; set; }
        public string ZoneName { get; set; }
        public int? StateId { get; set; }
        public string StateName { get; set; }
        public string Active { get; set; }
    }

    public class DropdownLit
    {  
        public int Id { get; set; }
        public string DropName { get; set; }
    }


    public class Uom
    {
        public int UomId { get; set; }
        public string UomName { get; set; }
        public string UomActive { get; set; }
    }


    public class category
    {
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public string categoryActive { get; set; }
    }



    public class subcategory
    {
        public int subcategoryId { get; set; }
        public string subcategoryName { get; set; }
        public string subcategoryActive { get; set; }
    }
    public class permission
    {
        public int Id { get; set; }
        public int nodeId { get; set; }
        public string nodeName { get; set; }
        public string destination { get; set; }
        public bool active { get; set; }
        public bool view { get; set; }
        public bool insert { get; set; }
        public bool edit { get; set; }
        public bool delete { get; set; }

    }

    public class TotalOrder
    {
        public TotalOrder()
        {
            TodayEmpPer = new List<DailyempPerformance>();
            MonthEmpPer = new List<MonthyempPerformance>();

        }
        public int todayTotalOrder { get; set; }
        public int monthlyTotalOrder { get; set; }
        public List<DailyempPerformance> TodayEmpPer { get; set; }
        public List<MonthyempPerformance> MonthEmpPer { get; set; }
    }
    public class DailyempPerformance
    {
        public string empName { get; set; }
        public int totalOrder { get; set; }
    }
    public class MonthyempPerformance
    {
        public string empName { get; set; }
        public int totalOrder { get; set; }
    }
    public class OrderSaleOfMonth
    {
        public string Sale { get; set; }
        public string Zone { get; set; }

        public string name { get; set; }
        public int y { get; set; }
    }

   


}
