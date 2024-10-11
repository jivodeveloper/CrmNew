using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using Call_Centre_Management.Classes;
using Call_Centre_Management.Models;

namespace Call_Centre_Management.Controllers
{
    public class ItemController : Controller
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        Common_Class common_class = new Common_Class();
        public ActionResult Index()
        {
            List<Item_Model> item_list = new List<Item_Model>();
            dict.Clear();
            dict.Add("@mode", "Getall_Items");
            DataTable dt = common_class.return_datatable(dict, "proc_item");
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
                    item.Active= Convert.ToInt32(dt.Rows[i]["active"].ToString());
                    item_list.Add(item);
                }
            }
            ViewBag.get_item = item_list;
            return View();
        }

        [HttpGet]
        public ActionResult Insert_Item()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Insert_Item(Item_Model item)
        {
            DataTable dtRawItem = new DataTable();
            dtRawItem.Columns.Add("RawItemid", typeof(int));
            dtRawItem.Columns.Add("name", typeof(string));
            dtRawItem.Columns.Add("qty", typeof(int));
            dtRawItem.TableName = "tblRawItem";

            foreach (var dr in item.subItemList)
            {
                int GetId = GetItemID(dr.name);
                if (GetId != 0)
                {
                    dtRawItem.Rows.Add(GetId, dr.name, dr.qty);
                }
            }
            string ste = common_class.GetXmlDoc(dtRawItem);
            dict.Clear();
            //   dict.Add("@id",item.id);
            dict.Add("@Item_Name", item.item_name);
            dict.Add("@category", item.category);
            dict.Add("@Sub_category", item.sub_category);
            dict.Add("@Uom", item.UOM);
            dict.Add("@Uom_value", item.UOM_value);
            dict.Add("@rate", item.rate);
            dict.Add("@schemeName", item.schemeName);
            dict.Add("@Scheme_value", item.Scheme_values);
            dict.Add("@Scheme_Quantity", item.scheme_qty);
            dict.Add("@Gst", item.gst);
            dict.Add("@doc", ste);
            dict.Add("@user", Convert.ToString(Session["user_name"]));
            dict.Add("@mode", "insert");
            var Result = common_class.return_nonquery(dict, "proc_item");
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public int GetItemID(string prifix)
        {
            dict.Clear();
            dict.Add("@mode", "SearchRawItemONname");
            dict.Add("@RawItem", prifix);
            DataTable dt = common_class.return_datatable(dict, "proc_item");
            Item_Model item = new Item_Model();
            if (dt.Rows.Count > 0)
            {
                item.id = Convert.ToInt32(dt.Rows[0]["id"].ToString());
                item.item_name = dt.Rows[0]["rawitem"].ToString();
            }
            return item.id;
        }

        [HttpPost]
        public ActionResult Delete_Item(int id)
        {
            dict.Clear();
            dict.Add("@id", id);
            dict.Add("@mode", "Deactive");
            var Result = common_class.return_nonquery(dict, "proc_item");
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Edit_Item(int id)
        {
            List<Uom> UomList = new List<Uom>();
            List<category> categoryList = new List<category>();
            List<subcategory> subcategoryList = new List<subcategory>();
            List<Item_Model> item_list = new List<Item_Model>();
            List<subItemModel> subItemList = new List<subItemModel>();

            Item_Model item = new Item_Model();
            dict.Clear();
            dict.Add("@id", id);
            dict.Add("@mode", "Getall_Items_ById");
            DataSet ds = common_class.return_dataset(dict, "proc_item");
            if (ds.Tables.Count > 0)
            {
                Session["itemId"] = ds.Tables[0].Rows[0]["id"].ToString();
                //item.id = Convert.ToInt32(dt.Rows[0]["id"].ToString());
                item.Item_id = ds.Tables[0].Rows[0]["item_id"].ToString();
                item.item_name = ds.Tables[0].Rows[0]["Item_Name"].ToString();
                item.category = ds.Tables[0].Rows[0]["category"].ToString();
                item.sub_category = ds.Tables[0].Rows[0]["Sub_category"].ToString();
                item.UOM = ds.Tables[0].Rows[0]["Uom"].ToString();
                item.UOM_value = Convert.ToInt32(ds.Tables[0].Rows[0]["Uom_value"].ToString());
                item.rate = Convert.ToDouble(ds.Tables[0].Rows[0]["rate"].ToString());
                item.schemeName = ds.Tables[0].Rows[0]["schemeName"].ToString();
                item.Scheme_values = Convert.ToInt32(ds.Tables[0].Rows[0]["Scheme_value"].ToString());
                item.scheme_qty = Convert.ToInt32(ds.Tables[0].Rows[0]["Scheme_Quantity"].ToString());
                item.gst = Convert.ToInt32(ds.Tables[0].Rows[0]["Gst"].ToString());
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                Uom items = new Uom();
                items.UomId = Convert.ToInt32(dr["id"]);
                items.UomName = dr["Uom"].ToString();
                UomList.Add(items);
            }

            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                category items = new category();
                items.categoryId = Convert.ToInt32(dr["id"]);
                items.categoryName = dr["Category"].ToString();
                categoryList.Add(items);
            }

            foreach (DataRow dr in ds.Tables[3].Rows)
            {
                subcategory items = new subcategory();
                items.subcategoryId = Convert.ToInt32(dr["id"]);
                items.subcategoryName = dr["SubCategory"].ToString();
                subcategoryList.Add(items);
            }
            foreach (DataRow dr in ds.Tables[4].Rows)
            {
                subItemModel subItem = new subItemModel();
                subItem.name = Convert.ToString(dr["rawItem"]);
                subItem.qty = Convert.ToInt32(dr["qty"]);
                subItemList.Add(subItem);
            }

            ViewBag.get_Edit_item = item;
            ViewBag.UomList = UomList;
            ViewBag.categoryList = categoryList;
            ViewBag.subcategoryList = subcategoryList;
            ViewBag.subItemList = subItemList;
            return View();
        }

        [HttpPost]
        public ActionResult Edit_Item(Item_Model item)
        {
            DataTable dtRawItem = new DataTable();
            dtRawItem.Columns.Add("RawItemid", typeof(int));
            dtRawItem.Columns.Add("name", typeof(string));
            dtRawItem.Columns.Add("qty", typeof(int));
            dtRawItem.TableName = "tblRawItem";

            foreach (var dr in item.subItemList)
            {
                int GetId = GetItemID(dr.name);
                if (GetId != 0)
                {
                    dtRawItem.Rows.Add(GetId, dr.name, dr.qty);
                }
            }
            string ste = common_class.GetXmlDoc(dtRawItem);

            dict.Clear();
            dict.Add("@id", Convert.ToInt32(Session["itemId"]));
            dict.Add("@Item_Name", item.item_name);
            dict.Add("@category", item.category);
            dict.Add("@Sub_category", item.sub_category);
            dict.Add("@Uom", item.UOM);
            dict.Add("@Uom_value", item.UOM_value);
            dict.Add("@rate", item.rate);
            dict.Add("@schemeName", item.schemeName);
            dict.Add("@Scheme_value", item.Scheme_values);
            dict.Add("@Scheme_Quantity", item.scheme_qty);
            dict.Add("@Gst", item.gst);
            dict.Add("@doc", ste);
            dict.Add("@user", Convert.ToString(Session["user_name"]));
            dict.Add("@mode", "update");
            var Result = common_class.return_nonquery(dict, "proc_item");
            if (Result > 0)
            {
                Session["itemId"] = null;
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult checkItem(string item)
        {
            dict.Clear();
            dict.Add("@Item_Name", item);
            dict.Add("@mode", "CheckItem");
            DataTable dt = common_class.return_datatable(dict, "proc_item");

            if (dt.Rows.Count > 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json("!! OOPS Site Crashed ", JsonRequestBehavior.AllowGet);
        }

        public ActionResult submitUom(string Uom)
        {
            int i = 0;
            dict.Clear();
            dict.Add("@UOM", Uom);
            dict.Add("@insetedBy", Session["user_name"].ToString());
            dict.Add("@mode", "InsertUOM");
            i = common_class.return_nonquery(dict, "Proc_category");
            return Json(i, JsonRequestBehavior.AllowGet);
        }
        public ActionResult submitCategory(string Category)
        {
            int i = 0;
            dict.Clear();
            dict.Add("@Category", Category);

            dict.Add("@insetedBy", Session["user_name"].ToString());
            dict.Add("@mode", "InsertCategory");
            i = common_class.return_nonquery(dict, "Proc_category");
            return Json(i, JsonRequestBehavior.AllowGet);
        }
        public ActionResult submitSubCategory(string Category, string SubCategory)
        {
            int i = 0;
            dict.Clear();
            dict.Add("@Category", Category);
            dict.Add("@Subcategory", SubCategory);
            dict.Add("@insetedBy", Session["user_name"].ToString());
            dict.Add("@mode", "InsertSubCategory");
            i = common_class.return_nonquery(dict, "Proc_category");
            return Json(i, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCategory()
        {
            List<Item_Model> lstitem = new List<Item_Model>();
            dict.Clear();
            //dict.Add("", "");
            dict.Add("@mode", "CategoryDeatils");
            DataTable dt = common_class.return_datatable(dict, "Proc_category");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Item_Model item = new Item_Model();
                    item.category = dt.Rows[i]["Category"].ToString();
                    lstitem.Add(item);
                }
            }
            return Json(lstitem, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getUOM()
        {
            List<Item_Model> lstitem = new List<Item_Model>();
            dict.Clear();
            //dict.Add("", "");
            dict.Add("@mode", "UOMDetails");
            DataTable dt = common_class.return_datatable(dict, "Proc_category");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Item_Model item = new Item_Model();
                    item.UOM = dt.Rows[i]["UOM"].ToString();
                    lstitem.Add(item);
                }
            }
            return Json(lstitem, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getSubcategory(string Category)
        {
            List<Item_Model> lstitem = new List<Item_Model>();
            dict.Clear();
            dict.Add("@mode", "SubCategoryDetails");
            dict.Add("@category", Category);
            DataTable dt = common_class.return_datatable(dict, "Proc_category");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Item_Model item = new Item_Model();
                    item.sub_category = dt.Rows[i]["SubCategory"].ToString();
                    lstitem.Add(item);
                }
            }
            return Json(lstitem, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult SearchAndEdit(string category, string subcategory, string UOM)
        //{
        //    List<CategoryModel> list = new List<CategoryModel>();

        //    dict.Clear();
        //    dict.Add("@mode", "seachAndEditSubCategory");
        //    dict.Add("@UOM", UOM);
        //    dict.Add("@category", category);
        //    dict.Add("@Subcategory", subcategory);
        //    DataSet ds = common_class.return_dataset(dict, "Proc_category");
        //    if (ds.Tables.Count > 0)
        //    {
        //            foreach(DataRow dr in ds.Tables[0].Rows)
        //            {
        //               CategoryModel items = new CategoryModel();
        //                items.UOM = dr["UOM"].ToString();
        //                items.Uom.Add(items);
        //                list.Add(items);
        //            }
        //        //CategoryModel item = new CategoryModel();
        //        // ds.Tables[0].Rows[i]["UOM"].ToString();

        //        ////list.Uom.Add(item);

        //        foreach (DataRow drs in ds.Tables[1].Rows)
        //        {
        //            CategoryModel items = new CategoryModel();
        //            items.Catergory = drs["Category"].ToString();
        //            items.category.Add(items);
        //            list.Add(items);
        //        }

        //        //for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
        //        //{
        //        //    Item_Model item1 = new Item_Model();
        //        //    item1.category = ds.Tables[1].Rows[j]["Category"].ToString();
        //        //    ////list.Add(item);
        //        //}
        //        //for (int k = 0; k < ds.Tables[2].Rows.Count; k++)
        //        //{
        //        //    Item_Model item2 = new Item_Model();
        //        //    item2.sub_category= ds.Tables[2].Rows[k]["SubCategory"].ToString();
        //        //    ////list.Add(item);
        //        //}


        //        foreach (DataRow drd in ds.Tables[2].Rows)
        //        {
        //            CategoryModel items = new CategoryModel();
        //            items.SuvCatergory = drd["SubCategory"].ToString();
        //            items.Subcategory.Add(items);
        //            list.Add(items);
        //        }

        //    }
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public ActionResult CreateRaw(string RawItem, string RawItemCost)
        {
            dict.Clear();
            dict.Add("@RawItem", RawItem);
            dict.Add("@RawItemCost", RawItemCost);
            dict.Add("@user", Convert.ToString(Session["user_name"]));
            dict.Add("@mode", "insertRawItem");
            int i = common_class.return_nonquery(dict, "proc_item");
            return Json(i, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult fetcRawItem(string RawItem)
        {
            List<item_Model> itemList = new List<item_Model>();
            dict.Clear();
            dict.Add("@RawItem", RawItem);
            //dict.Add("@user", Convert.ToString(Session[""]));
            dict.Add("@mode", "SearchRawItem");

            DataTable dt = common_class.return_datatable(dict, "proc_item");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    item_Model item = new item_Model();
                    item.id = Convert.ToInt32(dr["id"]);
                    item.item_name = dr["rawItem"].ToString();
                    itemList.Add(item);
                }
            }
            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult loadRawItems()
        {
            List<displayorder> listdo = new List<displayorder>();
            dict.Clear();
            dict.Add("@mode", "FetchRawItems");
            DataSet ds = common_class.return_dataset(dict, "proc_item");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        displayorder rawItem = new displayorder();
                        rawItem.ItemId = Convert.ToInt32(dr["id"]);
                        rawItem.item_name = Convert.ToString(dr["rawItem"]);
                        rawItem.item_rate = Convert.ToDouble(dr["cost"]);
                        rawItem.item_Active = Convert.ToString(dr["active"]);

                        listdo.Add(rawItem);
                    }
                }
            }
            return Json(listdo, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ChangeSatusRawItem(int id)
        {
            dict.Clear();
            dict.Add("@mode", "DeactiveRawItems");
            dict.Add("@id", id);
            int i = common_class.return_nonquery(dict, "proc_item");
            return Json(i, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditRawItem(int id)
        {
            dict.Clear();
            dict.Add("@mode", "EditRawItems");
            dict.Add("@id", id);
            DataSet ds = common_class.return_dataset(dict, "proc_item");
            displayorder rawItem = new displayorder();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    rawItem.ItemId = Convert.ToInt32(ds.Tables[0].Rows[0]["id"]);
                    rawItem.item_name = Convert.ToString(ds.Tables[0].Rows[0]["rawItem"]);
                    rawItem.item_rate = Convert.ToDouble(ds.Tables[0].Rows[0]["cost"]);
                    rawItem.item_Active = Convert.ToString(ds.Tables[0].Rows[0]["active"]);
                }
            }
            return Json(rawItem, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UpdateRawItem(int id ,string RawItemName,string RawItemCost)
        {
            dict.Clear();
            dict.Add("@mode", "UpdateRawItems");
            dict.Add("@id", id);
            dict.Add("@RawItem", RawItemName);
            dict.Add("@RawItemCost", RawItemCost);
            dict.Add("@user", Convert.ToString(Session["user_name"]));
            int i = common_class.return_nonquery(dict, "proc_item");
            return Json(i, JsonRequestBehavior.AllowGet);
        }

      
    }
}