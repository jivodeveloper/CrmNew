using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Call_Centre_Management.Classes;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Configuration;
using Call_Centre_Management.Models;

namespace Call_Centre_Management.Controllers
{
    public class CommonController : Controller
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        public Area_CommonClass areaClass = new Area_CommonClass();
        Common_Class commonClass = new Common_Class();
        // GET: Common
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetStates()
        {
            Area_CommonClass areaClass = new Area_CommonClass();

            dict.Clear();
            dict.Add("@mode", "State");
            var x = new SelectList(areaClass.BindDropDown("state", "stateId", "GetAllStatesByLoginUser", dict, "STATE"), "value", "text");
            return Json(x, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetZones(string selectedValue)
        {
            dict.Clear();
            dict.Add("@mode", "bind_zoneOnStateId");
            dict.Add("@stateid", selectedValue);
            var x = new SelectList(areaClass.BindDropDown("zone", "id", "proc_common", dict, "ZONE"), "value", "text");
            return Json(x, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetArea(string selectedValue)
        {
            dict.Clear();
            dict.Add("@mode", "bind_areaOnZoneId");
            dict.Add("@zoneid", selectedValue);
            var x = new SelectList(areaClass.BindDropDown("Area_name", "id", "proc_common", dict, "AREA"), "value", "text");
            return Json(x, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStates1()
        {
            Area_CommonClass areaClass = new Area_CommonClass();

            dict.Clear();
            dict.Add("@mode", "State1");
            var x = new SelectList(areaClass.BindDropDown1("state", "stateId", "GetAllStatesByLoginUser", dict, "STATE"), "value", "text");
            return Json(x, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetZonesWithId(string stateId, string zoneId)
        {
            dict.Clear();
            dict.Add("@mode", "bind_zoneOnStateId1");
            dict.Add("@zoneid", zoneId);
            dict.Add("@stateid", stateId);
            var zone = new SelectList(areaClass.BindDropDown1("zone", "id", "proc_common", dict, "ZONE"), "value", "text");
            return Json(zone, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAreaWithId(string AreaId, string zoneId)
        {
            dict.Clear();
            dict.Add("@mode", "bind_areaOnZoneId1");
            dict.Add("@zoneid", zoneId);
            dict.Add("@areaid", AreaId);
            var area = new SelectList(areaClass.BindDropDown1("Area_name", "id", "proc_common", dict, "AREA"), "value", "text");
            return Json(area, JsonRequestBehavior.AllowGet);
        }
        public void Get_permission(int Nodeid)
        {
            empClass e = new empClass();
            //string str1 = e.check_role(Convert.ToString(Session["Empid"].ToString()));
            Session["Role"] = e.check_role(Convert.ToString(Session["Empid"]));
            string str1 = Convert.ToString(Session["Empid"].ToString());
            int str = Nodeid;

            Session["View"] = null;
            Session["Edit"] = null;
            Session["Insert"] = null;
            Session["Delete"] = null;

            if (Convert.ToInt32(str1) == 1)
            {
                Session["View"] = "true";
                Session["Edit"] = "true";
                Session["Insert"] = "true";
                Session["Delete"] = "true";
            }
            else
            {
                dict.Clear();
                dict.Add("@nodeId", Convert.ToInt32(Nodeid));
                dict.Add("@empId", Convert.ToInt32(Session["Empid"].ToString()));
                dict.Add("@mode", "getMenuPermission");
                DataSet ds = commonClass.return_dataset(dict, "proc_Menu");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //condition? exprIfTrue : exprIfFalse
                        Session["View"] = Convert.ToBoolean(ds.Tables[0].Rows[0]["view"]) == true ? "true" : "false";
                        Session["Insert"] = Convert.ToBoolean(ds.Tables[0].Rows[0]["Insert"]) == true ? "true" : "false";
                        Session["Edit"] = Convert.ToBoolean(ds.Tables[0].Rows[0]["edit"]) == true ? "true" : "false";
                        Session["Delete"] = Convert.ToBoolean(ds.Tables[0].Rows[0]["delete"]) == true ? "true" : "false";
                    }
                }

            }
        }
        public void checkSession()
        {
            if (Session["user_name"] == null || Session["user_name"].ToString() == "")
            {
                RedirectToAction("Login", "Employee_Login");
            }
        }
        public JsonResult sendmail(string username)
        {
            var GetEncrptpwd = "";
            var GetRandomPassword = "";

            if (username != "")
            {
                GetRandomPassword = GenrateRandomPassword();
                GetEncrptpwd = commonClass.Encode(GetRandomPassword);
            }
            dict.Clear();
            dict.Add("@mode", "ForgetPassword");
            dict.Add("@UserName", username);
            dict.Add("@Password", GetEncrptpwd);
            DataTable dt = commonClass.return_datatable(dict, "proc_employee");
            if (dt.Rows.Count > 0)
            {
                // bool GetvalidEmail= Convert.ToBoolean(dt.Rows[0]["validemail"]);
                var GetEmail = dt.Rows[0]["email"].ToString();

                if (GetEmail != "0" && GetEmail != "")
                {
                    bool Getvalue = isValidEmail(GetEmail);
                    if (Getvalue == true)
                    {
                        MailMessage mm = new MailMessage("developer@jivo.in", GetEmail);
                        mm.Subject = "Password Recovery";
                        mm.Body = string.Format("Hello &nbsp;{0},<br /><br />Your Email Address : &nbsp; {1} <br />Your password is : &nbsp; {2}<br /><br />Thank you : &nbsp;{0} <br /><br />A Simple Solution to healthy India. To provide robust health to everyone at the right price To inspire righteousness in the marketplace, happiness and cheer in the society by contributing and sharing our profits for the wellbeing of the society at large." + "<a href='https://jivo.in/' target='_blank'>Jivo Wellness Pvt.Ltd.</a>", username, GetEmail, GetRandomPassword);
                        mm.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        NetworkCredential NetworkCred = new NetworkCredential();
                        NetworkCred.UserName = ConfigurationManager.AppSettings.Get("userid");
                        NetworkCred.Password = ConfigurationManager.AppSettings.Get("password");
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        smtp.Send(mm);

                        return Json(Getvalue, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(Getvalue, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(2, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(3, JsonRequestBehavior.AllowGet);
        }
        public static string GenrateRandomPassword()
        {
            string allowedChars = "";
            allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars += "1,2,3,4,5,6,7,8,9,0,!,@,#,$,%,&,?";
            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string passwordString = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i < 4; i++)

            {
                temp = arr[rand.Next(0, arr.Length)];
                passwordString += temp;
            }

            return passwordString;
        }
        public static bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex Check = new Regex(strRegex);
            if (Check.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
        //private string Encryptdata(string password)
        //{
        //    string strmsg = string.Empty;
        //    byte[] encode = new byte[password.Length];
        //    encode = Encoding.UTF8.GetBytes(password);
        //    strmsg = Convert.ToBase64String(encode);
        //    return strmsg;
        //}

        //private string Decryptdata(string encryptpwd)
        //{
        //    string decryptpwd = string.Empty;
        //    UTF8Encoding encodepwd = new UTF8Encoding();
        //    Decoder Decode = encodepwd.GetDecoder();
        //    byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
        //    int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
        //    char[] decoded_char = new char[charCount];
        //    Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
        //    decryptpwd = new String(decoded_char);
        //    return decryptpwd;
        //}

        [HttpPost]
        public ActionResult searchUser(string prefix)
        {
            List<Employees> emp_list = new List<Employees>();
            dict.Add("@UserName", prefix);
            dict.Add("@mode", "prefix_user");
            DataTable dt = commonClass.return_datatable(dict, "proc_employee");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Employees emp = new Employees();
                    emp.userName = dt.Rows[i]["userName"].ToString();
                    emp_list.Add(emp);
                }
            }
            return Json(emp_list, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult CheckPagePermission(int i)
        {
            Get_permission(i);
            return Json(Convert.ToBoolean(Session["View"]), JsonRequestBehavior.AllowGet);
        }
    }
}