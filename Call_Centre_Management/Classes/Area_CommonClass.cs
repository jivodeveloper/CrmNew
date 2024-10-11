using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Call_Centre_Management.Models;
using Call_Centre_Management.Classes;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace Call_Centre_Management.Classes
{
    public class Area_CommonClass
    {


        public List<SelectListItem> BindDropDown(string text, string value, string procName, Dictionary<string,object> sqlParams, string selectionText)
        {
            Common_Class commonClass = new Common_Class();
            return ConvertDtToList(commonClass.return_datatable( sqlParams,procName), text, value, selectionText);
        }

        public List<SelectListItem> ConvertDtToList(DataTable dt, string textfield, string valuefield, string selectionText)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {

                items.Add(new SelectListItem { Value = @dr[valuefield].ToString(), Text = @dr[textfield].ToString() + "  " + "(" + (@dr[valuefield].ToString()) + ")" });
            }
            //from value="-1" remove -1 for dropdown validation 
            //items.Insert(0, new SelectListItem { Value = "-1", Text = "--" + selectionText + "--" });
            items.Insert(0, new SelectListItem { Value = "0", Text = "--" + selectionText + "--" });
            return items;
        }




        public List<SelectListItem> BindDropDown1(string text, string value, string procName, Dictionary<string, object> sqlParams, string selectionText)
        {
            Common_Class commonClass = new Common_Class();
            return ConvertDtToList1(commonClass.return_datatable(sqlParams, procName), text, value, selectionText);
        }
        public List<SelectListItem> ConvertDtToList1(DataTable dt, string textfield, string valuefield, string selectionText)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {

                items.Add(new SelectListItem { Value = @dr[valuefield].ToString(), Text = @dr[textfield].ToString() + "  " + "(" + (@dr[valuefield].ToString()) + ")" });
            }
            //from value="-1" remove -1 for dropdown validation 
            //items.Insert(0, new SelectListItem { Value = "-1", Text = "--" + selectionText + "--" });
          //  items.Insert(0, new SelectListItem { Value = "0", Text = "--" + selectionText + "--" });
            return items;
        }


    }
}