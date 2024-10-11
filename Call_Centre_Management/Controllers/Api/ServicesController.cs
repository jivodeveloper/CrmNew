using Call_Centre_Management.Classes;
using Call_Centre_Management.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;


namespace Call_Centre_Management.Controllers.Api
{
    [RoutePrefix("api")]
    public class ServicesController : ApiController
    {
        Common_Class commonClass = new Common_Class();
        Dictionary<string, object> dict = new Dictionary<string, object>();
        int empid = 0;
        string empNm = "";
        DataTable global_dt = new DataTable();

        [HttpGet]
        [Route("Login")]
        public ApiModel Login(string Username)
        {
            ApiModel api = new ApiModel();
            dict.Clear();
            dict.Add("@customer_number", Username);
            dict.Add("@mode", "LoginInsertCustomer");
            global_dt = commonClass.return_datatable(dict, "procApi");
            int j = global_dt.Rows.Count;
            if (j > 0)
            {
                api.Id = Convert.ToInt32(global_dt.Rows[0]["id"].ToString());
                api.flag = Convert.ToInt32(global_dt.Rows[0]["flag"].ToString());
            }
            return api;
        }

        [HttpGet]
        [Route("Banner")]
        public List<Banner> Banner()
        {
            List<Banner> banners = new List<Banner>();
            dict.Clear();
            dict.Add("@mode", "Banner");
            global_dt = commonClass.return_datatable(dict, "procApi");
            int j = global_dt.Rows.Count;
            if (j > 0)
            {
                for (int i = 0; i < j; i++)
                {
                    Banner banner = new Banner();
                    banner.banner = Convert.ToString(global_dt.Rows[i]["BannerName"]);
                    banner.bannerpath = Convert.ToString(global_dt.Rows[i]["BannerPath"]);
                    banners.Add(banner);
                }
            }
            return  banners;
        }

        // POST api/<controller>
        [HttpPost]
        [Route("EmployeeProfile")]
        public int EmployeeProfile(Customer customer)
        {
            int Result = 0;
            dict.Clear();
            dict.Add("@Id", customer.id);
            dict.Add("@customer_name", customer.name);
            dict.Add("@Address", customer.Address);
            dict.Add("@state", customer.State_id);
            dict.Add("@zone", customer.Zone_id);
            dict.Add("@area", customer.Area_id);
            dict.Add("@Email", customer.CustAPiEmail);
            dict.Add("@mode", "updateCustomer");
            Result = commonClass.return_nonquery(dict, "procApi");
            return  Result;
        }

    }
}
