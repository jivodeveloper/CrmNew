using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System;

namespace Call_Centre_Management.Classes
{
    [Serializable]
    public class DataAccess
    {
        static string[] proceduresToLog = new string[] { "UpdateNewRetailerTemp", "UpdateRetailer", "DeleteSalesreport", "DeleteRetailer",
            "CreateSalesPerson", "UpdateSalesPerson", "DeleteSalesPerson", "CreateRetailer","PromoterDailyReport"};
        public static void sendSms(string message, string number)
        {
           
                string url = "http://103.247.98.91/API/SendMsg.aspx?uname=20152216&pass=123456&send=xJIVOx&dest=" + number + "&msg=" + message;
                WebRequest wrGETURL;
                wrGETURL = WebRequest.Create(url);

                Stream objStream;
                objStream = wrGETURL.GetResponse().GetResponseStream();
            
           
        }

        static string logMessage = Environment.NewLine;
        //static SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        public static DataSet RunProc(string procName, List<SqlParameter> sqlParams)
        {

            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connection_string"].ConnectionString);
            try
            {
                string logMessage = Environment.NewLine;
                if (proceduresToLog.Contains(procName))
                {
                    if (HttpContext.Current != null)
                    {
                        if (Convert.ToString(HttpContext.Current.Session["userName"]) != null)
                        {
                            logMessage += "UserId: " + Convert.ToString(HttpContext.Current.Session["userId"]) + Environment.NewLine;
                            logMessage += "UserName: " + Convert.ToString(HttpContext.Current.Session["userName"]) + Environment.NewLine;
                        }
                    }
                    logMessage += "ProcName: " + procName + Environment.NewLine;

                    foreach (var param in sqlParams)
                    {
                        logMessage += param.ParameterName + ": " + param.Value + Environment.NewLine;
                    }

                }


                SqlCommand cmd = new SqlCommand(procName, conn);
                cmd.CommandTimeout = 600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(sqlParams.ToArray());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);


                //conn.Close();
                return ds;
            }
            catch (Exception e)
            {
                if (!System.Diagnostics.Debugger.IsAttached)
                {
                    //new SlackClient().sendMessage(procName, e.Message, SlackColor.danger);
                    //new TeamsClient().sendMessage(procName, e.Message, TeamsColor.danger);
                }
                //JivoEventSource.Log.WriteError(e.ToString());
                //JivoEventSource.Log.WriteError(logMessage);
                throw;
            }
            finally
            {
                conn.Close();
            }

        }

        public static DataSet RunProc(string procName)
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
            try
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand(procName, conn);
                cmd.CommandTimeout = 600;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                //conn.Close();
                return ds;
            }
            catch (Exception e)
            {
                if (!System.Diagnostics.Debugger.IsAttached)
                {
                    //new SlackClient().sendMessage(procName, e.Message, SlackColor.danger);
                    //new TeamsClient().sendMessage(procName, e.Message, TeamsColor.danger);
                }
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public static DataSet RunProc(string procName, SqlParameter sqlParam)
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
            try
            {
                string logMessage = Environment.NewLine;
                if (proceduresToLog.Contains(procName))
                {
                    if (HttpContext.Current != null)
                    {
                        if (Convert.ToString(HttpContext.Current.Session["userName"]) != null)
                        {
                            logMessage += "UserId: " + Convert.ToString(HttpContext.Current.Session["userId"]) + Environment.NewLine;
                            logMessage += "UserName: " + Convert.ToString(HttpContext.Current.Session["userName"]) + Environment.NewLine;
                        }
                    }
                    logMessage += "ProcName: " + procName + Environment.NewLine;
                    logMessage += sqlParam.ParameterName + ": " + sqlParam.Value + Environment.NewLine;

                    //JivoEventSource.Log.WriteInfo(logMessage);
                }

                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand(procName, conn);
                cmd.CommandTimeout = 600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(sqlParam);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);


                //conn.Close();
                return ds;
            }
            catch (Exception e)
            {
                if (!System.Diagnostics.Debugger.IsAttached)
                {
                   // new SlackClient().sendMessage(procName, e.Message, SlackColor.danger);
                    //new TeamsClient().sendMessage(procName, e.Message, TeamsColor.danger);
                }
                //JivoEventSource.Log.WriteError(e.ToString());
                //JivoEventSource.Log.WriteError(logMessage);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public static DataSet RunQuery(string query)
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
            try
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandTimeout = 600;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                if (!System.Diagnostics.Debugger.IsAttached)
                {
                   // new SlackClient().sendMessage(query, e.Message, SlackColor.danger);
                    //new TeamsClient().sendMessage(query, e.Message, TeamsColor.danger);
                }
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public static DataSet RunProcApi(string procName, List<SqlParameter> sqlParams, bool flag = false)
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                if (!flag)
                {
                    if (Convert.ToString(HttpContext.Current.Session["userName"]) != "")
                    {
                        logMessage += "UserId: " + Convert.ToString(HttpContext.Current.Session["userId"]) + Environment.NewLine;
                        logMessage += "UserName: " + Convert.ToString(HttpContext.Current.Session["userName"]) + Environment.NewLine;
                    }
                    logMessage += "ProcName: " + procName + Environment.NewLine;

                }
                else
                {
                    //logMessage += "DistId: " + JwtClass.readToken() + Environment.NewLine;
                    //logMessage += "ProcName: " + procName + Environment.NewLine;
                }
                foreach (var param in sqlParams)
                {
                    logMessage += param.ParameterName + ": " + param.Value + Environment.NewLine;
                }

                //JivoEventSource.Log.WriteInfo(logMessage);

                SqlCommand cmd = new SqlCommand(procName, conn);
                cmd.CommandTimeout = 600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(sqlParams.ToArray());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                //JivoEventSource.Log.WriteError(e.ToString());
                //JivoEventSource.Log.WriteError(logMessage);
                //throw; 
            }
            finally
            {
                conn.Close();
            }
            return ds;
        }

    }
}