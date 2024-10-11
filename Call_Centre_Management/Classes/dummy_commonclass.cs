using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using HIMS.BLL.ErrorLog;
using System.Globalization;
using HIMS.Report.SOMENTE.Billing_RPT;

namespace Call_Centre_Management.Classes
{
    public class dummy_commonclass
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        public SqlConnection ret_conn = new SqlConnection();
        StringBuilder SBXmlDoc = new StringBuilder();
        string AllizeConnectionString = "";
        public static int checkstatus = 0;
        public void fvr_Show_Message_Javascript(Control sender, string Message)
        {
            ScriptManager.RegisterClientScriptBlock(sender, sender.GetType(), "script", "alert('" + Message + "')", true);
        }
        public DateTime fvr_Get_DateTime_fromString(string dd_MM_YYYY)
        {
            try
            {
                return DateTime.ParseExact(dd_MM_YYYY, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new Exception("String Date Format Must Be (dd/MM/yyyy)");
            }
        }
        public string fvr_Get_String_FromDateTime(DateTime From_date)
        {
            try
            {
                string dateString = From_date.Day.ToString("00") + "/" + From_date.Month.ToString("00") + "/" + From_date.Year.ToString();
                return dateString;
            }
            catch
            {
                throw new Exception("String Date Time Conversion Error (dd/MM/yyyy)");
            }
        }
        public DateTime GetDateByMonthYear(string MM_YYYY)
        {
            DateTime dt = fvr_Get_DateTime_fromString("01/" + MM_YYYY);
            return fvr_Get_DateTime_fromString(DateTime.DaysInMonth(dt.Year, dt.Month) + "/" + MM_YYYY);
        }

        public SqlConnection OpenConnection()
        {
            try
            {
                ret_conn = new SqlConnection(DecryptString(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString));
                ret_conn.Open();
            }
            catch
            {
                throw;
            }
            return ret_conn;
        }
        public enum QMS
        {
            Kiosk = 1,
            Enquiry = 2,
            Others = 3,
        };


        public int GetAge(string patientCode)
        {
            int data = 0;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = OpenConnection();
                cmd = new SqlCommand("QMS_SP_QueueMgmt", cn);
                cmd.CommandTimeout = 1000000;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@unitiD", (string)Session["UnitId"]);
                cmd.Parameters.AddWithValue("mode", "Queue");
                cmd.Parameters.AddWithValue("@PatientCode", patientCode);

                data = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch { }
            finally
            {
                cmd.Dispose();
                CloseConnection(cn);
            }
            return data;

        }

        public DataTable getDataTable(string query)
        {
            DataTable dt = new DataTable();
            SqlConnection cn = null;
            SqlDataAdapter adapter;
            SqlCommand cmd = null;
            try
            {

                cn = OpenConnection();
                cmd = new SqlCommand(query, cn);
                cmd.CommandTimeout = cn.ConnectionTimeout;
                cmd.Connection = cn;
                cmd.CommandText = query;
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "alert('Error occour plz try again')", true);

            }
            finally
            {
                cmd.Dispose();
                CloseConnection(cn);
            }
            return dt;
        }
        public void SetDateLimit(AjaxControlToolkit.CalendarExtender cal, TextBox txtStart = null, TextBox txtEndDAte = null)
        {
            if (Session["FYIDFromDate"] != null)
            {
                cal.StartDate = Convert.ToDateTime(Session["FYIDFromDate"]);
                if (txtStart != null)
                {
                    txtStart.Text = Convert.ToDateTime(Session["FYIDFromDate"]).ToString("dd-MMM-yyyy");
                }
            }
            if (Session["FYIDToDate"] != null)
            {
                cal.EndDate = Convert.ToDateTime(Session["FYIDToDate"]);
                if (txtEndDAte != null)
                {
                    txtEndDAte.Text = Convert.ToDateTime(Session["FYIDToDate"]).ToString("dd-MMM-yyyy");
                }
            }
        }

        public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            // column names
            PropertyInfo[] oProps = null;
            if (varlist == null) return dtReturn;
            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }


                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }


                DataRow dr = dtReturn.NewRow();


                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }


                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        public string DecryptString(string encrString)
        {
            Byte[] b = Convert.FromBase64String(encrString);
            string decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            Byte[] b1 = Convert.FromBase64String(decrypted);
            string decrypted1 = System.Text.ASCIIEncoding.ASCII.GetString(b1);
            Byte[] b2 = Convert.FromBase64String(decrypted1);
            string decrypted2 = System.Text.ASCIIEncoding.ASCII.GetString(b2);
            return decrypted2;

        }
        public string EncryptString(string strEncrypted)
        {
            Byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);

            string encrypted = Convert.ToBase64String(b);
            Byte[] b1 = System.Text.ASCIIEncoding.ASCII.GetBytes(encrypted);
            string encrypted1 = Convert.ToBase64String(b1);
            Byte[] b2 = System.Text.ASCIIEncoding.ASCII.GetBytes(encrypted1);
            string encrypted2 = Convert.ToBase64String(b2);
            return encrypted2;
        }
        public int countVisibleRows(GridView gv)
        {
            int count = 0;
            foreach (GridViewRow gvr in gv.Rows)
            {
                if (gvr.Visible)
                {
                    count++;
                }
            }
            return count;
        }
        public void CloseConnection(SqlConnection cnn)
        {
            try
            {
                if ((cnn != null) && (cnn.State & ConnectionState.Open) == ConnectionState.Open)
                {
                    cnn.Close();
                    cnn.Dispose();
                }
            }
            catch
            {
                cnn = null;
            }
        }
        public bool IsValidEmailAddress(string EmailAddress)
        {
            Regex regEmail = new Regex(@"^[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
            if (regEmail.IsMatch(EmailAddress))
                return true;
            return false;
        }
        public DataTable execProcedureReturnDt(Dictionary<string, object> dictionary, string procName)
        {
            DataTable data = null;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = OpenConnection();
                SqlDataAdapter adapter;
                cmd = new SqlCommand(procName, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000;
                foreach (KeyValuePair<string, object> pairs in dictionary)
                {
                    cmd.Parameters.AddWithValue(pairs.Key, pairs.Value);
                }
                adapter = new SqlDataAdapter(cmd);

                data = new DataTable();
                adapter.Fill(data);
                adapter.Dispose();

            }
            catch { }
            finally
            {
                cmd.Dispose();
                CloseConnection(cn);
            }
            return data;

        }
        public int execProcedureReturnNonQuery(Dictionary<string, object> dictionary, string procName)
        {
            int result = 0;
            int data = 0;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = OpenConnection();
                cmd = new SqlCommand(procName, cn);
                cmd.CommandTimeout = 1000000;
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (KeyValuePair<string, object> pairs in dictionary)
                {
                    cmd.Parameters.AddWithValue(pairs.Key.Trim(), Convert.ToString(pairs.Value).Trim());
                }

                data = cmd.ExecuteNonQuery();

            }
            catch { }
            finally
            {
                cmd.Dispose();
                CloseConnection(cn);
            }
            return data;
        }

        public int execProcedureReturnNonQueryForTrack(Dictionary<string, object> dictionary, string procName)
        {
            int result = 0;
            int data = 0;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = OpenConnection();
                cmd = new SqlCommand(procName, cn);
                cmd.CommandTimeout = 1000000;
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (KeyValuePair<string, object> pairs in dictionary)
                {
                    cmd.Parameters.AddWithValue(pairs.Key.Trim(), Convert.ToString(pairs.Value).Trim());
                }
                var returnParameter = cmd.Parameters.Add("@ReturnApplicationTrackId", SqlDbType.BigInt);
                returnParameter.Direction = ParameterDirection.Output;
                data = cmd.ExecuteNonQuery();
                result = Convert.ToInt32(returnParameter.Value);
                Session["ApplicationTrackId"] = result;
            }
            catch { }
            finally
            {
                cmd.Dispose();
                CloseConnection(cn);
            }
            return data;
        }

        public object execProcedureReturnValue(Dictionary<string, object> dictionary, string procName)
        {
            object result = 0;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = OpenConnection();
                cmd = new SqlCommand(procName, cn);
                cmd.CommandTimeout = 1000000;
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (KeyValuePair<string, object> pairs in dictionary)
                {
                    cmd.Parameters.AddWithValue(pairs.Key.Trim(), Convert.ToString(pairs.Value).Trim());
                }

                var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.BigInt);
                returnParameter.Direction = ParameterDirection.ReturnValue;


                int res = cmd.ExecuteNonQuery();
                result = returnParameter.Value;


            }
            catch { }
            finally
            {
                cmd.Dispose();
                CloseConnection(cn);
            }
            return result;
        }
        public int execProcedureReturnNonQuery1(Dictionary<string, object> dictionary, string procName)
        {
            int data = 0;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = OpenConnection();
                cmd = new SqlCommand(procName, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000;
                foreach (KeyValuePair<string, object> pairs in dictionary)
                {
                    cmd.Parameters.AddWithValue(pairs.Key, pairs.Value);
                }
                SqlParameter p = cmd.Parameters.Add("@MESSAGE", SqlDbType.VarChar, 500);
                p.Direction = ParameterDirection.Output;

                //Roweffect = cmd.ExecuteNonQuery();
                data = cmd.ExecuteNonQuery();
                _returnsMsg = cmd.Parameters["@MESSAGE"].Value.ToString();
                Session["ReturnMsg"] = _returnsMsg;
            }
            catch
            {

            }
            finally
            {
                cmd.Dispose();
                CloseConnection(cn);
            }
            return data;
        }
        //GULREJ ALAM
        public int execProcedureReturnNonQuery2out(Dictionary<string, object> dictionary, string procName)
        {
            int data = 0;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = OpenConnection();
                cmd = new SqlCommand(procName, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000;
                foreach (KeyValuePair<string, object> pairs in dictionary)
                {
                    cmd.Parameters.AddWithValue(pairs.Key, pairs.Value);
                }
                SqlParameter p = cmd.Parameters.Add("@MESSAGE", SqlDbType.VarChar, 500);
                p.Direction = ParameterDirection.Output;
                SqlParameter p2 = cmd.Parameters.Add("@RET", SqlDbType.VarChar, 500);
                p2.Direction = ParameterDirection.Output;


                //Roweffect = cmd.ExecuteNonQuery();


                data = cmd.ExecuteNonQuery();
                _returnsMsg = cmd.Parameters["@MESSAGE"].Value.ToString();
                Session["ReturnMsg"] = _returnsMsg;

                _returnsRET = cmd.Parameters["@RET"].Value.ToString();
                Session["RET"] = _returnsRET;
            }
            catch
            {

            }
            finally
            {
                cmd.Dispose();
                CloseConnection(cn);
            }
            return data;
        }

        //Created By Akshaya Kumar on 14-03-2019
        public DataTable RemoveEmptyRowsFromDataTable(DataTable dtnew, string ColumnName)
        {
            for (int i = dtnew.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dtnew.Rows[i];
                if (dr[ColumnName] == "")
                    dr.Delete();
            }
            dtnew.AcceptChanges();
            return dtnew;
        }
        public void ClearAllEmrSession()
        {
            Session[FVR_PJ._Session_.EMR_ID.ToString()] = null;
            Session[FVR_PJ._Session_.AR_ID.ToString()] = null;
            Session[FVR_PJ._Session_.IOP_ID.ToString()] = null;
            Session[FVR_PJ._Session_.ASCAN_ID.ToString()] = null;
            Session[FVR_PJ._Session_.ICL_ID.ToString()] = null;
            Session[FVR_PJ._Session_.Consular_ID.ToString()] = null;
        }

        public string Get_Designation_ID_From_Empi_ID(string EMP_ID)
        {
            if (EMP_ID == "0")
            {
                return "0";
            }
            else
            {
                return GetScalar("select DesignationId from Somente.HR_tblEmployeeDetail where EMPID=" + EMP_ID + "").ToString();
            }
        }
        public string Get_Dept_ID_From_Empi_ID(string EMP_ID)
        {
            if (EMP_ID == "0")
            {
                return "0";
            }
            else
            {
                return GetScalar("select DepartmentId from Somente.HR_tblEmployeeDetail where EMPID=" + EMP_ID + "").ToString();
            }
        }
        public string GetQueueDashBoard(string Desig)
        {
            string QueueType = Convert.ToString(GetScalar("Select QueueId from DesignationMapping  WHERE PATINDEX('%" + Desig + "%',Department) >0  and Len(" + Desig + ")=3"));
            if (QueueType == "")
                return "0";
            else
                return QueueType;
        }
        public bool GetSurgeryEyeByOPD(string OPD_ID)
        {
            string Eye = Convert.ToString(GetScalar("select Suggested_Surgery_Eye  from EMR_DR_MASTER where OPD_ID=" + OPD_ID));
            if (Eye == "L>R" || Eye == "R>L" || Eye == "BE")
                return true;
            else
                return false;
        }
        public object GetScalar(string query)
        {
            object data = null;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = OpenConnection();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 1000000;
                cmd.Connection = cn;
                cmd.CommandText = query;
                data = cmd.ExecuteScalar();
            }
            catch
            {
            }
            finally
            {
                cmd.Dispose();
                CloseConnection(cn);
            }
            return data;
        }
        public int execProcedureReturnNonQuery2(Dictionary<string, object> dictionary, string procName)
        {
            int data = 0;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = OpenConnection();
                cmd = new SqlCommand(procName, cn);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (KeyValuePair<string, object> pairs in dictionary)
                {
                    cmd.Parameters.AddWithValue(pairs.Key, pairs.Value);
                }
                SqlParameter p = cmd.Parameters.Add("@message", SqlDbType.VarChar, 500);
                p.Direction = ParameterDirection.Output;

                //Roweffect = cmd.ExecuteNonQuery();


                data = cmd.ExecuteNonQuery();
                _returnsMsg = cmd.Parameters["@message"].Value.ToString();
                Session["ReturnMsg"] = _returnsMsg;
            }
            catch
            {
            }
            finally
            {
                cmd.Dispose();
                CloseConnection(cn);
            }
            return data;
        }

        //alam chamges
        public SqlDataReader execProcedureReturnDR(Dictionary<string, object> dictionary, string procName)
        {
            SqlConnection cn;
            cn = OpenConnection();
            SqlCommand cmd = new SqlCommand(procName, cn);
            cmd.CommandType = CommandType.StoredProcedure;

            foreach (KeyValuePair<string, object> pairs in dictionary)
            {
                cmd.Parameters.AddWithValue(pairs.Key, pairs.Value);
            }
            SqlDataReader reader = cmd.ExecuteReader();
            //reader.Read();            
            return reader;
        }

        //Created By sharad Gupta 28 Jan 2015 Reader with out parameter 
        public SqlDataReader execProcedureReturnDROut(Dictionary<string, object> dictionary, string procName)
        {
            SqlConnection cn;
            cn = OpenConnection();
            SqlCommand cmd = new SqlCommand(procName, cn);
            cmd.CommandType = CommandType.StoredProcedure;

            foreach (KeyValuePair<string, object> pairs in dictionary)
            {
                cmd.Parameters.AddWithValue(pairs.Key, pairs.Value);
            }


            SqlParameter p = cmd.Parameters.Add("@MESSAGE", SqlDbType.VarChar, 500);
            p.Direction = ParameterDirection.Output;

            //Roweffect = cmd.ExecuteNonQuery();


            SqlDataReader reader = cmd.ExecuteReader();
            string returnsMsg = cmd.Parameters["@MESSAGE"].Value.ToString();
            Session["ReturnMsg"] = returnsMsg;
            return reader;
        }


        //Alliez..................................................
        public int execProcedureReturnNonQueryALLize(Dictionary<string, object> dictionary, string procName)
        {
            int data = 0;
            SqlConnection cn;
            cn = new SqlConnection(AllizeConnectionString);
            SqlCommand cmd = new SqlCommand(procName, cn);
            cmd.CommandType = CommandType.StoredProcedure;

            foreach (KeyValuePair<string, object> pairs in dictionary)
            {
                cmd.Parameters.AddWithValue(pairs.Key, pairs.Value);
            }
            SqlParameter p = cmd.Parameters.Add("@MESSAGE", SqlDbType.VarChar, 500);
            p.Direction = ParameterDirection.Output;
            //Roweffect = cmd.ExecuteNonQuery();


            data = cmd.ExecuteNonQuery();
            // _returnsMsg = cmd.Parameters["@MESSAGE"].Value.ToString();
            return data;
        }
        public string execProcedureReturnNonQueryALLize1(Dictionary<string, object> dictionary, string procName)
        {
            string data = "0";
            SqlConnection cn;
            cn = OpenConnection();
            SqlCommand cmd = new SqlCommand(procName, cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {

                foreach (KeyValuePair<string, object> pairs in dictionary)
                {
                    cmd.Parameters.AddWithValue(pairs.Key, pairs.Value);
                }
                SqlParameter p = cmd.Parameters.Add("@MESSAGE", SqlDbType.VarChar, 500);
                p.Direction = ParameterDirection.Output;
                //Roweffect = cmd.ExecuteNonQuery();


                cmd.ExecuteNonQuery();
                data = cmd.Parameters["@MESSAGE"].Value.ToString();
            }
            catch
            {
            }
            finally
            {
                cmd.Dispose();

                CloseConnection(cn);
            }
            return data;
        }

        //Alliez..................................
        public DataSet execProcedureReturnDs(Dictionary<string, object> dictionary, string procName)
        {
            DataSet data = null;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter adapter = null;
            try
            {
                cn = OpenConnection();

                cmd = new SqlCommand(procName, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 10000000;
                foreach (KeyValuePair<string, object> pairs in dictionary)
                {
                    cmd.Parameters.AddWithValue(pairs.Key, pairs.Value);
                }

                adapter = new SqlDataAdapter(cmd);
                data = new DataSet();
                adapter.Fill(data);
                adapter.Dispose();

            }
            catch
            {
            }
            finally
            {
                cmd.Dispose();

                CloseConnection(cn);
            }
            return data;
        }
        public DataSet execProcedureReturnDs_stock(Dictionary<string, object> dictionary, string procName)
        {
            DataSet data = null;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            stockasondate dsstock = new stockasondate();
            SqlDataAdapter adapter = null;
            try
            {
                cn = OpenConnection();

                cmd = new SqlCommand(procName, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 10000000;
                foreach (KeyValuePair<string, object> pairs in dictionary)
                {
                    cmd.Parameters.AddWithValue(pairs.Key, pairs.Value);
                }

                adapter = new SqlDataAdapter(cmd);
                // data = new DataSet();
                adapter.Fill(dsstock, "stock_table");

                adapter.Dispose();

            }
            catch
            {
            }
            finally
            {
                cmd.Dispose();

                CloseConnection(cn);
            }
            return dsstock;
        }




        public int GetIntegerFROMString(string SelectString) //VInay 3-may-2016
        {
            SqlConnection conn = OpenConnection();
            try
            {
                SqlCommand cmdIntFROMString = new SqlCommand();
                cmdIntFROMString.Connection = conn;

                cmdIntFROMString.CommandType = CommandType.Text;
                cmdIntFROMString.CommandText = SelectString;

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                int retint = Convert.ToInt32(cmdIntFROMString.ExecuteScalar());
                if ((conn != null))
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                conn.Dispose();
                if (!string.IsNullOrEmpty(Convert.ToString(retint)))
                {
                    return retint;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                string Str = ex.Message;
                if ((conn != null))
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                conn.Dispose();
                // throw ex;
                return 0;
            }
        }


        public DataSet RunSQLReturnDS(string strSQL)
        {

            SqlConnection cn;
            cn = OpenConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(strSQL, cn);
            da.Fill(ds);
            da.Dispose();
            return ds;

        }
        public DataSet RunSQLReturnDS(string strSQL, string tablename)
        {

            SqlConnection cn;
            cn = OpenConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(strSQL, cn);
            da.Fill(ds, tablename);
            da.Dispose();
            return ds;

        }



        public DataView RunSQLReturnDV(string strSQL)
        {

            SqlConnection cn;
            cn = OpenConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(strSQL, cn);
            da.Fill(ds);
            da.Dispose();
            return ds.Tables[0].DefaultView;
        }

        public int RunSql(string strSQL)
        {
            SqlConnection cn;
            cn = OpenConnection();
            int objRowsAffected = 0;
            try
            {
                SqlCommand sqlComm = new SqlCommand(strSQL, cn);
                sqlComm.CommandTimeout = cn.ConnectionTimeout;
                objRowsAffected = sqlComm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            return objRowsAffected;

        }

        public DataView RunSPReturnDV(string strSP)
        {
            SqlConnection cn = OpenConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(strSP, cn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            da.Fill(ds);
            CloseConnection(cn);
            da.Dispose();
            return ds.Tables[0].DefaultView;
        }
        public void deleteTextFile(string filename, string folder)
        {

            string k1 = Server.MapPath(folder);
            k1 = k1 + "\\" + filename + ".txt";
            FileInfo fi = new FileInfo(k1);
            fi.Delete();
        }
        public void createtextfile(string filenames, string textvalue, string folders)
        {
            string k1 = Server.MapPath(folders);
            k1 = k1 + "\\" + filenames + ".txt";
            FileInfo fi = new FileInfo(k1);
            FileStream fstr = fi.Create();
            fstr.Close();
            StreamWriter strm = new StreamWriter(k1);
            strm.Write(textvalue);
            strm.Close();
        }
        public string Readtxtfile(string id, string folders)
        {
            string filename = "";


            string k2 = Server.MapPath(folders);

            k2 = k2 + "\\" + id + ".txt";
            StreamReader re = File.OpenText(k2);
            string inputext = null;
            string Cnotevalue = null;

            while ((inputext = re.ReadLine()) != null)
            {


                Console.WriteLine(inputext);
                Cnotevalue = Cnotevalue + "\n" + inputext;
            }
            filename = Cnotevalue;

            re.Close();



            return filename;
        }

        public string FileExist(string searchString)
        {
            string st = "";
            string imageFolder = Server.MapPath("Chatstatus\\") + searchString + ".txt";
            if (File.Exists(imageFolder))
            {
                st = "True";
            }
            else
            {
                st = "False";
            }
            return st;

        }
        public int RunSP(string strSP, params SqlParameter[] commandParameters)
        {
            int effect = 0;
            string Roweffect = "";
            SqlConnection cn = OpenConnection();
            SqlCommand cmd = new SqlCommand(strSP, cn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter p;
            foreach (SqlParameter p1 in commandParameters)
            {
                p = cmd.Parameters.Add(p1);
                p.Direction = ParameterDirection.Input;
                Roweffect = Roweffect + "#" + p.Value.ToString();
            }
            //Roweffect = cmd.ExecuteNonQuery();
            effect = cmd.ExecuteNonQuery();
            cmd.Dispose();
            CloseConnection(cn);
            return effect;

        }

        //Updated By Alam on 17-09-2015
        public ArrayList RunSPByAlam(string strSP, params SqlParameter[] commandParameters)
        {
            ArrayList arr = new ArrayList();
            int effect = 0;
            string Roweffect = "";
            SqlConnection cn = OpenConnection();
            SqlCommand cmd = new SqlCommand(strSP, cn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter p;
            foreach (SqlParameter p1 in commandParameters)
            {
                p = cmd.Parameters.Add(p1);
                p.Direction = ParameterDirection.Input;
                Roweffect = Roweffect + "#" + p.Value.ToString();
            }

            SqlParameter outpar = new SqlParameter("@MESSAGE", SqlDbType.NVarChar, 500);
            outpar.Direction = ParameterDirection.Output;

            cmd.Parameters.Add(outpar);
            //Roweffect = cmd.ExecuteNonQuery();
            effect = cmd.ExecuteNonQuery();
            cmd.Dispose();
            CloseConnection(cn);
            arr.Add(effect);
            arr.Add(cmd.Parameters["@MESSAGE"].Value);

            return arr;

        }






        public DataTable Getcountry()
        {
            return getDataTable("select * from Country");
        }
        public DataTable Getstate(string cid)
        {
            return getDataTable("select * from State where CountryID='" + cid + "'");
        }
        public DataTable Getcity(string cid, string sid)
        {
            return getDataTable("select * from tbl_Cityname where CountryId='" + cid + "' and StateId='" + sid + "'");
        }
        //Create By Sharad Gupta 15 sep 2014 and Suggested by KK Sir



        public StringBuilder GetXmlDocBuilder(DataTable Dt)
        {
            StringBuilder XmlDoc = new StringBuilder();
            XmlDoc.Append("<NewDataSet>");
            int j = 0;
            int k = 0;
            for (j = 0; j <= Dt.Rows.Count - 1; j++)
            {
                XmlDoc.Append("<" + Dt.TableName + ">");
                for (k = 0; k <= Dt.Columns.Count - 1; k++)
                {
                    XmlDoc.Append("<" + Dt.Columns[k].ColumnName + ">");

                    XmlDoc.Append(Dt.Rows[j][k].ToString().Replace("&", "&amp;").Replace("<", "&lt;"));
                    XmlDoc.Append("</" + Dt.Columns[k].ColumnName + ">");
                }
                XmlDoc.Append("</" + Dt.TableName + ">");
            }

            XmlDoc.Append("</NewDataSet>");

            return XmlDoc;

        }

        public StringBuilder GetXmlDocDsBuild(DataSet ds)
        {
            StringBuilder XmlDoc = new StringBuilder();
            XmlDoc.Append("<NewDataSet>");
            //Dim strval As String
            int j = 0;
            int k = 0;
            int i = 0;
            for (i = 0; i <= ds.Tables.Count - 1; i++)

                for (j = 0; j <= ds.Tables[i].Rows.Count - 1; j++)
                {
                    XmlDoc.Append("<" + ds.Tables[i].TableName + ">");
                    for (k = 0; k <= ds.Tables[i].Columns.Count - 1; k++)
                    {
                        XmlDoc.Append("<" + ds.Tables[i].Columns[k].ColumnName + ">");
                        XmlDoc.Append(ds.Tables[i].Rows[j][k].ToString().Replace("&", "&amp;").Replace("<", "&lt;"));
                        XmlDoc.Append("</" + ds.Tables[i].Columns[k].ColumnName + ">");
                    }
                    XmlDoc.Append("</" + ds.Tables[i].TableName + ">");
                }

            XmlDoc.Append("</NewDataSet>");
            StringBuilder strPattern = new StringBuilder();
            return XmlDoc;
        }

        //----------------------------------------End

        public string GetXmlDoc(DataTable Dt)
        {
            string XmlDoc = "<NewDataSet>";
            //Dim strval As String
            int j = 0;
            int k = 0;

            for (j = 0; j <= Dt.Rows.Count - 1; j++)
            {
                XmlDoc = XmlDoc + "<" + Dt.TableName + ">";
                for (k = 0; k <= Dt.Columns.Count - 1; k++)
                {
                    XmlDoc = XmlDoc + "<" + Dt.Columns[k].ColumnName + ">";
                    //If Ds.Tables(i).Columns(k).DataType.ToString = "System.DateTime" Then
                    //    strVal = String.Format("{0:s}", Ds.Tables(i).Rows(j).Item(k))
                    //Else
                    //    strVal = (Ds.Tables(i).Rows(j).Item(k).ToString()).Replace("&", "")
                    //End If

                    XmlDoc = XmlDoc + Dt.Rows[j][k].ToString().Replace("&", "&amp;").Replace("<", "&lt;");
                    XmlDoc = XmlDoc + "</" + Dt.Columns[k].ColumnName + ">";
                }
                XmlDoc = XmlDoc + "</" + Dt.TableName + ">";
            }

            XmlDoc = XmlDoc + "</NewDataSet>";
            StringBuilder strPattern = new StringBuilder();

            //for (int i = 128; i <= 255; i++)
            //{
            //    strPattern = strPattern.AppendFormat(Strings.Chr(i));
            //    //strPattern = strPattern.Append(Strings.Chr(i));

            //}
            return XmlDoc;
            //return Regex.Replace(XmlDoc, "[" + strPattern.ToString() + "]", "");
        }
        public string GetXmlDoc(DataSet ds)
        {
            string XmlDoc = "<NewDataSet>";
            //Dim strval As String
            int j = 0;
            int k = 0;
            int i = 0;
            for (i = 0; i <= ds.Tables.Count - 1; i++)

                for (j = 0; j <= ds.Tables[i].Rows.Count - 1; j++)
                {
                    XmlDoc = XmlDoc + "<" + ds.Tables[i].TableName + ">";
                    for (k = 0; k <= ds.Tables[i].Columns.Count - 1; k++)
                    {
                        XmlDoc = XmlDoc + "<" + ds.Tables[i].Columns[k].ColumnName + ">";
                        XmlDoc = XmlDoc + ds.Tables[i].Rows[j][k].ToString().Replace("&", "&amp;").Replace("<", "&lt;");
                        XmlDoc = XmlDoc + "</" + ds.Tables[i].Columns[k].ColumnName + ">";
                    }
                    XmlDoc = XmlDoc + "</" + ds.Tables[i].TableName + ">";
                }

            XmlDoc = XmlDoc + "</NewDataSet>";
            StringBuilder strPattern = new StringBuilder();

            //for (int i = 128; i <= 255; i++)
            //{
            //    strPattern = strPattern.AppendFormat(Strings.Chr(i));
            //    //strPattern = strPattern.Append(Strings.Chr(i));

            //}
            return XmlDoc;
            //return Regex.Replace(XmlDoc, "[" + strPattern.ToString() + "]", "");
        }
        public string checkpermission(string checktype)
        {
            string Add = "";
            string Edit = "";
            string View = "";
            string Dele = "";
            if (Session["UserTypes"].ToString() == "HIMSAdmin")
            {
                Add = "1";
                Edit = "1";
                View = "1";
                Dele = "1";
            }
            else if (Session["UserTypes"].ToString() == "HIMSEmp" || Session["UserTypes"].ToString() == "HIMSDoc")
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["PermissionNode"];
                DataView view1 = new DataView(dt);
                view1.RowFilter = "NodeId='" + Session["NodeId"].ToString() + "'";
                foreach (DataRowView row in view1)
                {
                    Add = row["PAdd"].ToString();
                    Edit = row["PEdit"].ToString();
                    View = row["PView"].ToString();
                    Dele = row["PDelete"].ToString();
                }
            }
            if (checktype == "Add")
            {
                return Add;
            }
            else if (checktype == "Edit")
            {
                return Edit;
            }
            else if (checktype == "View")
            {
                return View;
            }
            else if (checktype == "Dele")
            {
                return Dele;
            }
            else
            {
                return "0";
            }

        }
        public class CalculateDateDifference
        {


            private int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };


            /// <summary>  

            /// contain from date  

            /// </summary>  

            private DateTime fromDate;


            /// <summary>  

            /// contain To Date  

            /// </summary>  

            private DateTime toDate;


            /// <summary>  

            /// these three variable of integer type for output representation..  

            /// </summary>  

            private int year;

            private int month;

            private int day;


            //Public type Constructor  

            public CalculateDateDifference(DateTime d1, DateTime d2)
            {

                int increment;


                //To Date must be greater  

                if (d1 > d2)
                {

                    this.fromDate = d2;

                    this.toDate = d1;

                }

                else
                {

                    this.fromDate = d1;

                    this.toDate = d2;

                }


                ///  

                /// Day Calculation  

                ///  

                increment = 0;


                if (this.fromDate.Day > this.toDate.Day)
                {

                    increment = this.monthDay[this.fromDate.Month - 1];


                }

                /// if it is february month  

                /// if it's to day is less then from day  

                if (increment == -1)
                {

                    if (DateTime.IsLeapYear(this.fromDate.Year))
                    {

                        // leap year february contain 29 days  

                        increment = 29;

                    }

                    else
                    {

                        increment = 28;

                    }

                }

                if (increment != 0)
                {

                    day = (this.toDate.Day + increment) - this.fromDate.Day;

                    increment = 1;

                }

                else
                {

                    day = this.toDate.Day - this.fromDate.Day;

                }


                ///  

                ///month calculation  

                ///  

                if ((this.fromDate.Month + increment) > this.toDate.Month)
                {

                    this.month = (this.toDate.Month + 12) - (this.fromDate.Month + increment);

                    increment = 1;

                }

                else
                {

                    this.month = (this.toDate.Month) - (this.fromDate.Month + increment);

                    increment = 0;

                }


                ///  

                /// year calculation  

                ///  

                this.year = this.toDate.Year - (this.fromDate.Year + increment);


            }


            public override string ToString()
            {

                //return base.ToString();  

                return this.year + " Year(s), " + this.month + " month(s), " + this.day + " day(s)";

            }


            public int Years
            {

                get
                {

                    return this.year;

                }

            }


            public int Months
            {

                get
                {

                    return this.month;

                }

            }


            public int Days
            {

                get
                {

                    return this.day;

                }

            }


        }


        public int returnmsg(string strSP, params SqlParameter[] commandParameters)
        {
            int effect = 0;
            string Roweffect = "";
            SqlConnection cn = OpenConnection();
            SqlCommand cmd = new SqlCommand(strSP, cn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter p;
            foreach (SqlParameter p1 in commandParameters)
            {
                p = cmd.Parameters.Add(p1);
                p.Direction = ParameterDirection.Input;
                Roweffect = Roweffect + "#" + p.Value.ToString();
            }
            p = cmd.Parameters.Add("@ReturnMessage", SqlDbType.NVarChar, 500);
            p.Direction = ParameterDirection.Output;
            //Roweffect = cmd.ExecuteNonQuery();
            effect = cmd.ExecuteNonQuery();
            _returnsMsg = cmd.Parameters["@ReturnMessage"].Value.ToString();
            cmd.Dispose();
            CloseConnection(cn);
            return effect;

        }
        private string _returnsMsg;
        private string _returnsRET;
        public string ReturnsMsg
        {
            get { return _returnsMsg; }
            set { _returnsMsg = value; }
        }

        public string ReturnsRET
        {
            get { return _returnsRET; }
            set { _returnsRET = value; }
        }

        //Created By OJASWA 28-12-2016
        public void Print_Report(string Mode, string _hdnValue, string _hiddenEMRID, string hiddenEMR_DrID, string _hiddenEMR_ConsID)
        {
            string Source = ConfigurationManager.AppSettings["REPORT_URL"].ToString();
            string Url_Path = "";
            switch (Mode)
            {
                case "OPD":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=OPD&ID=" + _hdnValue);
                    break;
                case "Procedure":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=PROCEDURE&ID=" + _hdnValue);
                    break;
                case "IPD":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=IPD&ID=" + _hdnValue);
                    break;
                case "COUNSELOR":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=CONSULAR_SUMMARY&ID=" + _hiddenEMR_ConsID);
                    break;
                case "PRESCRIPTION":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=DOCTOR_PRESCRIPTION&ID=" + _hiddenEMRID + "," + hiddenEMR_DrID);
                    break;
                case "Squint":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=Squint&ID=" + _hiddenEMRID + "," + hiddenEMR_DrID);
                    break;
                case "RetinoScopy":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=RetinoScopy&ID=" + _hiddenEMRID + "," + hiddenEMR_DrID);
                    break;
                case "GLASS PRESCRIPTION":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=GLASS_PRESCRIPTION&ID=" + _hiddenEMRID);
                    break;

                case "DISCHARGE":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=Dishcharge&ID=" + _hdnValue + "&Type=" + _hiddenEMRID);
                    break;
                case "Pharmacy_Bill":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=Pharmacy_Bill&ID=" + _hdnValue);
                    break;
                case "Optical_Bill":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=Optical_Bill&ID=" + _hdnValue);
                    break;
                case "SalesOrder_Bill":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=SalesOrder_Bill&ID=" + _hdnValue);
                    break;
                case "PharmacyReturn":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=PharmacyReturn&ID=" + _hdnValue);
                    break;
                case "Sales_Return":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=Sales_Return&ID=" + _hdnValue);
                    break;
                case "Consumption_Bill":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=Consumption_Bill&ID=" + _hdnValue);
                    break;
                case "OT_Consumption":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=OT_Consumption&ID=" + _hdnValue);
                    break;
                case "Refund":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=BIL_REFUND&ID=" + _hdnValue);
                    break;
                //Consent Form IPD
                case "FINAL LASIK LASER CONCERN":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=FINAL LASIK LASER CONCERN&ID=" + _hdnValue);
                    break;
                case "FINAL AVASTIN-LUCENTIS-ACCENTRIS CONCERN":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=FINAL AVASTIN-LUCENTIS-ACCENTRIS CONCERN&ID=" + _hdnValue);
                    break;
                case "FINAL CATARACT CONCERN":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=FINAL CATARACT CONCERN&ID=" + _hdnValue);
                    break;
                case "FINAL FUNDUS FLUORESCEIN ANGIOGRAPHY":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=FINAL FUNDUS FLUORESCEIN ANGIOGRAPHY&ID=" + _hdnValue);
                    break;
                case "VITREO RETINAL SURGERY":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=VITREO_RETINAL_SURGERY&ID=" + _hdnValue);
                    break;
                case "ANESTHESIA":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=ANESTHESIA&ID=" + _hdnValue);
                    break;
                case "PLAN OF CARE":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=PLAN_OF_CARE&ID=" + _hdnValue);
                    break;
                case "SQUINT SURGERY":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=SQUINT_SURGERY&ID=" + _hdnValue);
                    break;
                case "RETINAL DETACHMENT":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=RETINAL_DETACHMENT&ID=" + _hdnValue);
                    break;
                case "LASIK":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=LASIK&ID=" + _hdnValue);
                    break;
                case "INTRAVITREAL INJECTION":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=INTRAVITREAL_INJECTION&ID=" + _hdnValue);
                    break;
                case "GLAUCOMA":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=GLAUCOMA&ID=" + _hdnValue);
                    break;
                case "EYLEA INTRAVITREAL INJECTION":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=EYLEA_INTRAVITREAL_INJECTION&ID=" + _hdnValue);
                    break;
                case "EVISCERATION":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=EVISCERATION&ID=" + _hdnValue);
                    break;
                case "ECTROPION":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=ECTROPION&ID=" + _hdnValue);
                    break;
                case "DACRYOCYSTORHINOSTOMY":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=DACRYOCYSTORHINOSTOMY&ID=" + _hdnValue);
                    break;
                case "CONSENT TO OPERATION":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=CONSENT_TO_OPERATION&ID=" + _hdnValue);
                    break;
                case "CATARACT SURGERY":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=CATARACT_SURGERY&ID=" + _hdnValue);
                    break;
                case "GENERAL INFORMED CONSENT":
                    Url_Path = (Source + "/Report/Report_View_Easy.aspx?mode=GENERAL_INFORMED_CONSENT&ID=" + _hdnValue);
                    break;


            }
            if (!(Url_Path == ""))
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open('" + Url_Path + "');", true);
            }
        }
        public void BindDoctor(DropDownList ddl, string Mode)
        {
            ddl.Items.Clear();
            dict.Clear();
            dict.Add("@hospitalid", Session["HospitalId"].ToString());
            dict.Add("@unitid", Session["UnitId"].ToString());
            if (Mode == "New")
                dict.Add("@mode", "bindddlSearch");
            else
                dict.Add("@mode", "ALL_DOCTOR");
            DataTable dt = execProcedureReturnDt(dict, "HIMS_SearchSP");
            if (dt.Rows.Count > 0)
            {
                ddl.DataTextField = "Name";
                ddl.DataValueField = "EmpID";
                ddl.DataSource = dt;
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        public void BindOptom(DropDownList ddl, string Mode)
        {
            ddl.Items.Clear();
            dict.Clear();
            dict.Add("@hospitalid", Session["HospitalId"].ToString());
            dict.Add("@unitid", Session["UnitId"].ToString());
            if (Mode == "New")
                dict.Add("@mode", "bindddlOpto");
            else
                dict.Add("@mode", "ALL_OPTOM");
            DataTable dt = execProcedureReturnDt(dict, "HIMS_SearchSP");
            if (dt.Rows.Count > 0)
            {
                ddl.DataTextField = "Name";
                ddl.DataValueField = "EmpID";
                ddl.DataSource = dt;
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        public void BindWallet(DropDownList ddl)
        {
            dict.Clear();
            dict.Add("@mode", "Wallet");
            DataTable dt = execProcedureReturnDt(dict, "sp_GetOPDPageDetails");
            if (dt.Rows.Count > 0)
            {
                BindDropDownList(ddl, dt);
            }
        }
        public void BindBank(DropDownList ddl)
        {
            dict.Clear();
            dict.Add("@mode", "BankName");
            DataTable dt = execProcedureReturnDt(dict, "sp_GetOPDPageDetails");
            if (dt.Rows.Count > 0)
            {
                BindDropDownList(ddl, dt);
            }
        }
        void BindDropDownList(DropDownList ddl, DataTable dt)
        {
            ddl.Items.Clear();
            ddl.DataSource = dt;
            ddl.DataTextField = "CodeName";
            ddl.DataValueField = "RowId";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("SELECT", "0"));
        }
        public DataTable GetPatient_RewardPoint(string UHID, string DocType, string RegistrationType)
        {

            dict.Clear();
            dict.Add("@Patient_Code", UHID);
            dict.Add("@Type", DocType);
            dict.Add("@Registration_type", RegistrationType);
            dict.Add("@procmode", "Reward_Point");
            DataTable dt = execProcedureReturnDt(dict, "sp_Billing_Master");
            return dt;

        }

        public DataTable Get_RedeemAmount_Validation(string UHID)
        {
            dict.Clear();
            dict.Add("@mode", "Redeem_Amount_Validation");
            dict.Add("@Search", UHID);
            DataTable dt = execProcedureReturnDt(dict, "spDashbordSearch");
            return dt;

        }




    }


    public static class FVR_PJ
    {
        public enum _Session_
        {
            AR_ID,
            IOP_ID,
            EMR_ID,
            PAT_ID,
            TokenNo,
            QUEUE_ID,
            QUEUE_NAME,
            Designation_ID,
            EMP_ID,
            Consular_ID,
            ASCAN_ID,
            ICL_ID,
            Record_ID,
            DESIGID,
            OPD_ID,
            Queue_Type,
            AdvanceDosages
        };


    }




    public class DataBaseOperation : System.Web.UI.Page  // Ravi Class
    {

        private SqlConnection sqlConnection;
        public SqlCommand sqlCommand;
        private SqlDataReader sqlReader;
        private SqlDataAdapter sqlAdapter;
        CommanClass cmmn = new CommanClass();


        public void Dispose()
        {
            sqlAdapter.Dispose();
            sqlCommand.Dispose();
            if (sqlConnection.State != ConnectionState.Closed)
            {
                sqlConnection.Close();
            }
            sqlConnection.Dispose();
        }

        #region "Methods"

        public void init()
        {

            sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = cmmn.DecryptString(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            sqlAdapter = new SqlDataAdapter();
        }

        public void ClearParameter()
        {
            sqlCommand.Parameters.Clear();
        }

        public object executeScalar(String strSQLQuery, CommandType comType)
        {
            object returnvalue;
            sqlCommand.CommandText = strSQLQuery;
            sqlCommand.CommandType = comType;
            if (sqlConnection.State != ConnectionState.Open)
                sqlConnection.Open();
            returnvalue = sqlCommand.ExecuteScalar();
            return returnvalue;
        }

        public int executeNonQuery(String strSQLQuery, CommandType comType)
        {
            int returnvalue;
            sqlCommand.CommandText = strSQLQuery;
            sqlCommand.CommandType = comType;
            if (sqlConnection.State != ConnectionState.Open)
                sqlConnection.Open();

            returnvalue = sqlCommand.ExecuteNonQuery();
            return returnvalue;
        }
        public int executeNonQuery(String strSQLQuery)
        {
            int returnvalue;
            sqlCommand.CommandText = strSQLQuery;
            if (sqlConnection.State != ConnectionState.Open) sqlConnection.Open();
            returnvalue = sqlCommand.ExecuteNonQuery();
            foreach (SqlParameter param in sqlCommand.Parameters)
            {
                if (param.Direction == ParameterDirection.InputOutput)
                {
                    returnvalue = Convert.ToInt32(sqlCommand.Parameters[param.ParameterName].Value);
                }

            }
            return returnvalue;
        }

        public SqlDataReader executeQuery(String strSQLQuery)
        {
            sqlCommand.CommandText = strSQLQuery;

            if (sqlConnection.State != ConnectionState.Open) sqlConnection.Open();
            try
            {
                sqlReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sqlReader;
        }
        public SqlDataReader executeQuery(String strSQLQuery, CommandType comType)
        {
            sqlCommand.CommandType = comType;
            sqlCommand.CommandText = strSQLQuery;

            if (sqlConnection.State != ConnectionState.Open) sqlConnection.Open();

            sqlReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

            return sqlReader;
        }

        public DataSet SelectQuery(String strSQLQuery)
        {
            sqlCommand.CommandText = strSQLQuery;
            DataSet ds = new DataSet();
            if (sqlConnection.State != ConnectionState.Open) sqlConnection.Open();
            sqlAdapter.SelectCommand = sqlCommand;
            sqlAdapter.Fill(ds);

            return ds;
        }



        public DataSet SelectQuery(String strSQLQuery, CommandType ctype)
        {
            sqlCommand.CommandType = ctype;
            sqlCommand.CommandText = strSQLQuery;
            DataSet ds = new DataSet();
            if (sqlConnection.State != ConnectionState.Open) sqlConnection.Open();
            sqlAdapter.SelectCommand = sqlCommand;
            sqlAdapter.Fill(ds);

            return ds;
        }

        public void addParameters(String strParamName, SqlDbType dbType, Object strParamValue)
        {
            sqlCommand.Parameters.Add(strParamName, dbType).Value = strParamValue;

        }
        public void addParameters(String strParamName, Object strParamValue)
        {
            sqlCommand.Parameters.AddWithValue(strParamName, strParamValue);

        }
        public void addParameters(String strParamName, SqlDbType dbType, Object strParamValue, int fieldLength)
        {
            sqlCommand.Parameters.Add(strParamName, dbType, fieldLength).Value = strParamValue;

        }
        public void addParameters(String strParamName, SqlDbType dbType, Object strParamValue, ParameterDirection direction)
        {
            SqlParameter sqlparam;
            sqlparam = sqlCommand.Parameters.Add(strParamName, dbType);
            sqlparam.Value = strParamValue;
            sqlparam.Direction = direction;
        }

        public DataTable GetDataTable(string strQuery, CommandType comType)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
            sqlCommand.CommandText = strQuery;
            sqlCommand.CommandType = comType;
            sqlCommand.CommandTimeout = 100000;
            SqlParameter p;
            DataTable dt = new DataTable();
            sqlAdapter = new SqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dt);
            return dt;
        }



        public DataTable GetDataTable(string strQuery, CommandType comType, SqlParameter Commandparmeter)
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
            sqlCommand.CommandText = strQuery;
            sqlCommand.CommandType = comType;
            sqlCommand.CommandTimeout = 100000;
            DataTable dt = new DataTable();
            sqlAdapter = new SqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dt);
            return dt;
        }
        public DataSet GetDataset(string strQuery, CommandType comType)
        {
            DataSet ds = new DataSet();
            try
            {

                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                sqlCommand.CommandText = strQuery;
                sqlCommand.CommandType = comType;
                sqlCommand.CommandTimeout = 100000;
                sqlAdapter = new SqlDataAdapter(sqlCommand);
                sqlAdapter.Fill(ds);

            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public void closeReader()
        {
            if (sqlCommand != null)
            {
                sqlCommand.Parameters.Clear();
            }
            if (sqlReader != null)
            {
                sqlReader.Close();
            }
        }
        public void closeConnection()
        {
            if (sqlCommand != null)
            {
                sqlCommand.Parameters.Clear();
            }
            if (sqlReader != null)
            {
                sqlReader.Close();
            }
            if (sqlConnection != null)
            {
                sqlConnection.Close();
            }
        }

        #endregion
    }

    public class DataBind
    {
        #region  DataBinding in DroupDownList

        public void BindDataToDDL(DropDownList DDL, string DataTextField, DataSet DataSetToBind)
        {
            DDL.DataTextField = DataTextField;
            DDL.DataValueField = DataTextField;
            DDL.DataSource = DataSetToBind;
            DDL.DataBind();
        }
        public void BindDataToDDL(DropDownList DDL, string DataTextField, string DataValueField, DataSet DataSetToBind)
        {
            DDL.DataTextField = DataTextField;
            DDL.DataValueField = DataValueField;
            DDL.DataSource = DataSetToBind;
            DDL.DataBind();
        }
        public void BindDataToDDL(DropDownList DDL, string DataTextField, DataTable DataTableToBind)
        {
            DDL.DataTextField = DataTextField;
            DDL.DataValueField = DataTextField;
            DDL.DataSource = DataTableToBind;
            DDL.DataBind();
        }
        public void BindDataToDDL(DropDownList DDL, string DataTextField, string DataValueField, DataTable DataTableToBind)
        {
            DDL.DataTextField = DataTextField;
            DDL.DataValueField = DataValueField;
            DDL.DataSource = DataTableToBind;
            DDL.DataBind();
        }
        public void BindDataToDDL(DropDownList DDL, string DataTextField, SqlDataReader SDRToBind)
        {
            DDL.DataTextField = DataTextField;
            DDL.DataValueField = DataTextField;
            DDL.DataSource = SDRToBind;
            DDL.DataBind();
        }
        public void BindDataToDDL(DropDownList DDL, string DataTextField, string DataValueField, SqlDataReader SDRToBind)
        {
            DDL.DataTextField = DataTextField;
            DDL.DataValueField = DataValueField;
            DDL.DataSource = SDRToBind;
            DDL.DataBind();
        }
        public void BindDataToDDL(DropDownList DDL, string DataTextField, DataView DataViewToBind)
        {
            DDL.DataTextField = DataTextField;
            DDL.DataValueField = DataTextField;
            DDL.DataSource = DataViewToBind;
            DDL.DataBind();
        }
        public void BindDataToDDL(DropDownList DDL, string DataTextField, string DataValueField, DataView DataViewToBind)
        {
            DDL.DataTextField = DataTextField;
            DDL.DataValueField = DataValueField;
            DDL.DataSource = DataViewToBind;
            DDL.DataBind();
        }

        public void BindDataToGridView(GridView GV, DataView DataViewDataSorce)
        {
            GV.DataSource = DataViewDataSorce;
            GV.DataBind();
        }
        public void BindDataToGridView(GridView GV, DataTable DataTableDataSorce)
        {
            GV.DataSource = DataTableDataSorce;
            GV.DataBind();
        }
        public void BindDataToGridView(GridView GV, DataSet DataSetDataSorce)
        {
            GV.DataSource = DataSetDataSorce;
            GV.DataBind();
        }


        #endregion
    }




    //Created By Mukesh
    public static class Clear
    {
        /// <summary>
        /// Clear Textbox and Session Data
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="sessionKey"></param>
        static public void ClearTextAndSessionData(TextBox Txt, string SessionKey)
        {
            Txt.Text = string.Empty;
            HttpContext.Current.Session[SessionKey] = null;
        }

        /// <summary>
        /// Clear Textbox and Hidden Field Data
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="hdf"></param>
        static public void ClearTextAndHiddenFieldData(TextBox Txt, HiddenField HiddenField)
        {
            Txt.Text = string.Empty;
            HiddenField.Value = null;
        }

        /// <summary>
        /// Bind Null To Single Grid View
        /// </summary>
        /// <param name="Grid1"></param>
        static public void ClearGrid(GridView Grid1)
        {
            Grid1.DataSource = null;
            Grid1.DataBind();
        }

        /// <summary>
        /// Bind Null To Multiple Grid View
        /// </summary>
        /// <param name="GridNames"></param>
        static public void ClearGrid(params object[] GridNames)
        {
            try
            {
                foreach (GridView GridName in GridNames)
                {

                    if (((GridView)GridName) is GridView)
                    {
                        ((GridView)GridName).DataSource = null;
                        ((GridView)GridName).DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Remove Single Session
        /// </summary>
        /// <param name="sessionKey"></param>
        static public void RemoveSession(string SessionKey)
        {
            HttpContext.Current.Session.Remove(SessionKey);
        }


        /// <summary>
        /// Remove Multiple Sessions
        /// </summary>
        /// <param name="sessionKeys"></param>
        static public void RemoveSession(params string[] SessionKeys)
        {
            foreach (string sessionKey in SessionKeys)
            {
                HttpContext.Current.Session.Remove(sessionKey);
            }
        }


        /// <summary>
        /// Reset Textbox And Dropdown Of Page
        /// </summary>
        /// <param name="parent"></param>
        public static void ClearControls(Control Parent)
        {
            foreach (Control c in Parent.Controls)
            {
                if (c.GetType() == typeof(TextBox))
                {
                    ((TextBox)(c)).Text = string.Empty;
                }
                else if (c.GetType() == typeof(DropDownList))
                {
                    ((DropDownList)(c)).SelectedIndex = 0;
                }
                else if (c.GetType() == typeof(CheckBox))
                {
                    ((CheckBox)(c)).Checked = false;
                }

                ClearControls(c);
            }

        }


        /// <summary>
        /// Enable Disable All Child Controls Within in Parent Control or Page
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="EnableTrueFalse"></param>
        public static void EnableDisableChildControl(Control Parent, bool EnableTrueFalse)
        {
            foreach (Control c in Parent.Controls)
            {
                if (c.GetType() == typeof(TextBox))
                {
                    ((TextBox)(c)).Enabled = EnableTrueFalse;
                }
                else if (c.GetType() == typeof(GridView))
                {
                    ((GridView)(c)).Enabled = EnableTrueFalse;
                }
                else if (c.GetType() == typeof(ImageButton))
                {
                    ((ImageButton)(c)).Enabled = EnableTrueFalse;
                }
                else if (c.GetType() == typeof(Button))
                {
                    ((Button)(c)).Enabled = EnableTrueFalse;
                }
                else if (c.GetType() == typeof(Panel))
                {
                    ((Panel)(c)).Enabled = EnableTrueFalse;
                }
                else if (c.GetType() == typeof(DropDownList))
                {
                    ((DropDownList)(c)).Enabled = EnableTrueFalse;
                }
                EnableDisableChildControl(c, EnableTrueFalse);
            }


        }


        /// <summary>
        /// Enable Disable All SApecified Controls 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="EnableTrueFalse"></param>
        public static void EnableDisableChildControl(bool EnableTrueFalse, params object[] Controls)
        {
            try
            {
                foreach (object control in Controls)
                {


                    if (control is GridView)
                    {
                        ((GridView)control).Enabled = EnableTrueFalse;

                    }
                    else if (control is TextBox)
                    {
                        ((TextBox)control).Enabled = EnableTrueFalse;

                    }
                    else if (control is Panel)
                    {
                        ((Panel)control).Enabled = EnableTrueFalse;

                    }
                    else if (control is DropDownList)
                    {
                        ((DropDownList)control).Enabled = EnableTrueFalse;

                    }
                    else if (control is ImageButton)
                    {
                        ((ImageButton)control).Enabled = EnableTrueFalse;

                    }
                    else if (control is Button)
                    {
                        ((Button)control).Enabled = EnableTrueFalse;

                    }


                }
            }
            catch (Exception ex)
            {
            }
        }

    }

    //Created By Vinay
    public class QuickHelp
    {
        public static DataTable MyTable(DataColumn[] ColCollection)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(ColCollection);
            return dt;
        }
        public static DataTable MyTable(DataColumn[] ColCollection, string TableName)
        {
            DataTable dt = new DataTable();
            dt.TableName = TableName;
            dt.Columns.AddRange(ColCollection);
            return dt;
        }




        /// <summary>
        /// Bind Grid 
        /// with DataTable
        /// </summary>
        /// <param name="GridView"></param>
        /// <param name="DataTable"></param>
        public static void BindGrid(GridView GridView, DataTable DataTable)
        {
            try
            {
                GridView.DataSource = DataTable;
                GridView.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        /// <summary>
        /// Bind List Box 
        /// With DataTable 
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="dt"></param>
        /// <param name="dataTextField"></param>
        /// <param name="dataValueField"></param>
        public static void PopulatedListBox(ListBox lb, DataTable dt, string dataTextField, string dataValueField)  //created 6May2016
        {
            lb.DataSource = dt;
            lb.DataTextField = dataTextField;
            lb.DataValueField = dataValueField;
            lb.DataBind();
        }




        /// <summary>
        /// Transfer All List box data 
        /// Either From Left To Right 
        /// Or From Right To Left
        /// </summary>
        /// <param name="lbLeft"></param>
        /// <param name="lbRight"></param>
        /// <param name="tranferRightToLeft"></param>
        public static void TransferAllListBoxData(ListBox lbLeft, ListBox lbRight, bool tranferRightToLeft) //created 6May2016
        {
            if (tranferRightToLeft)
            {
                foreach (ListItem li in lbRight.Items)
                {
                    lbLeft.Items.Add(li);
                }

                lbRight.Items.Clear();
            }
            else
            {
                foreach (ListItem li in lbLeft.Items)
                {
                    lbRight.Items.Add(li);
                }

                lbLeft.Items.Clear();
            }
        }




        /// <summary>
        /// Transfer Only Selected Data 
        /// Either Form Left To Right
        /// Or Form Right To Left
        /// </summary>
        /// <param name="lbLeft"></param>
        /// <param name="lbRight"></param>
        /// <param name="tranferRightToLeft"></param>
        public static void TransferOnlySectedListBoxData(ListBox lbLeft, ListBox lbRight, bool tranferRightToLeft) //created 6May2016
        {

            int[] selectedIndexes;

            if (tranferRightToLeft)
            {
                selectedIndexes = lbRight.GetSelectedIndices();
                if (selectedIndexes.Length > 0)
                {
                    foreach (ListItem li in lbRight.Items)
                    {
                        if (li.Selected)
                        {
                            lbLeft.Items.Add(li);
                        }
                    }

                    foreach (int index in selectedIndexes)
                    {
                        lbRight.Items.RemoveAt(index);
                    }


                    lbLeft.ClearSelection();

                }


            }
            else
            {
                selectedIndexes = lbLeft.GetSelectedIndices();
                if (selectedIndexes.Length > 0)
                {
                    foreach (ListItem li in lbLeft.Items)
                    {
                        if (li.Selected)
                        {
                            lbRight.Items.Add(li);
                        }
                    }

                    foreach (int index in selectedIndexes)
                    {
                        lbLeft.Items.RemoveAt(index);
                    }
                    lbRight.ClearSelection();
                }
            }
        }
        //Vinay 3-oct-2015
        public static int ChkIfExist(string tblname, string GetFieldName, string chkFieldName, string ChkFieldValue)
        {
            CommanClass obj = new CommanClass();
            string qry = string.Format("SELECT {0} FROM  {1} WHERE {2} ='{3}' ", GetFieldName, tblname, chkFieldName, ChkFieldValue);
            return obj.GetIntegerFROMString(qry);
        }
    }
}
