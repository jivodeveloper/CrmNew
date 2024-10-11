using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Call_Centre_Management.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Quartz;
using Quartz.Impl;

namespace Call_Centre_Management.Classes
{
    public class Common_Class
    {
        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connection_string"].ConnectionString);
        SqlCommand cmd;
        SqlDataAdapter adp;

        public int return_nonquery(Dictionary<string, object> dict, string proc_name)
        {
            int i = 0;
            try
            {
                con.Open();
                cmd = new SqlCommand(proc_name, con);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (KeyValuePair<string, object> kvp in dict)
                {
                    cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                }
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string strex = ex.ToString();
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
            return i;
        }
        public DataSet return_dataset(Dictionary<string, object> dict, string proc)
        {
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                cmd = new SqlCommand(proc, con);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (KeyValuePair<string, object> kvp in dict)
                {
                    cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                }

                // cmd.ExecuteReader();
                adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);
                adp.Dispose();
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
            finally
            {

                cmd.Dispose();
                con.Close();
            }
            return ds;
        }
        public DataTable return_datatable(Dictionary<string, object> dict, string proc)
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                cmd = new SqlCommand(proc, con);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (KeyValuePair<string, object> kvp in dict)
                {
                    cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                }
                adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                adp.Dispose();
            }
            catch (Exception ex)
            {
                string x = ex.ToString();
            }
            finally
            {

                cmd.Dispose();
                con.Close();
            }
            return dt;
        }
        public DataSet return_dataset_query(string query)
        {
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                cmd = new SqlCommand(query, con);
                cmd.ExecuteReader();
                adp = new SqlDataAdapter(cmd);
                adp.Dispose();
            }
            catch (Exception)
            { }
            finally
            {

                cmd.Dispose();
                con.Close();
            }
            return ds;
        }
        public DataSet RunProc(string procName, List<SqlParameter> sqlParams)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connection_string"].ConnectionString);
            //try
            //{
                SqlCommand cmd = new SqlCommand(procName, conn);
                cmd.CommandTimeout = 600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(sqlParams.ToArray());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                conn.Close();
                return ds;
            //}
            //catch (Exception)
            //{
            //    //if (!System.Diagnostics.Debugger.IsAttached)
            //    //{
            //    //    new SlackClient().sendMessage(procName, e.Message, SlackColor.danger);
            //    //    //new TeamsClient().sendMessage(procName, e.Message, TeamsColor.danger);
            //    //}
            //    //JivoEventSource.Log.WriteError(e.ToString());
            //    //JivoEventSource.Log.WriteError(logMessage);
            //    //throw;
            //}
            //finally
            //{
            //    conn.Close();
            //}
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
        public string GetXmlDoc(DataTable Dt)
        {
            string XmlDoc = "<NewDataSet>";
            int j = 0;
            int k = 0;
            for (j = 0; j <= Dt.Rows.Count - 1; j++)
            {
                XmlDoc = XmlDoc + "<" + Dt.TableName + ">";
                for (k = 0; k <= Dt.Columns.Count - 1; k++)
                {
                    XmlDoc = XmlDoc + "<" + Dt.Columns[k].ColumnName + ">";
                    XmlDoc = XmlDoc + Dt.Rows[j][k].ToString().Replace("&", "&amp;").Replace("<", "&lt;");
                    XmlDoc = XmlDoc + "</" + Dt.Columns[k].ColumnName + ">";
                }
                XmlDoc = XmlDoc + "</" + Dt.TableName + ">";
            }
            XmlDoc = XmlDoc + "</NewDataSet>";
            StringBuilder strPattern = new StringBuilder();
            return XmlDoc;
        }
       
        //public string GetXmlDoc(List<Employees> list )
        //{
        //    list = new List<Employees>();
        //    string XmlDoc = "<NewDataSet>";
        //    int j = 0;
        //    int k = 0;
        //    string str = list.Array[0][1].tostring();
        //    for (j = 0; j <= list.Count - 1; j++)
        //    {
        //        XmlDoc = XmlDoc + "<" + list.TableName + ">";
        //        for (k = 0; k <= list.Count - 1; k++)
        //        {
        //            XmlDoc = XmlDoc + "<" + list.Columns[k].ColumnName + ">";

        //            XmlDoc = XmlDoc + list.Rows[j][k].ToString().Replace("&", "&amp;").Replace("<", "&lt;");

        //            XmlDoc = XmlDoc + "</" + list.Columns[k].ColumnName + ">";
        //        }
        //        XmlDoc = XmlDoc + "</" + list.TableName + ">";
        //    }
        //    XmlDoc = XmlDoc + "</NewDataSet>";
        //    StringBuilder strPattern = new StringBuilder();
        //    return XmlDoc;
        //}

        public void createExcelFile(DataSet ds, string fileName, string folderPath)
        {
            //var home = System.IO.getProperty("user.home");
            //File file = new File(home + folderPath + fileName + ".txt");
            string date = DateTime.Now.ToString("dd-MMM-yyyy.hh.mm.s");
            var path = Path.Combine(folderPath, fileName + ".xlsx");
            string fileNameOnly = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            string directory = Path.GetDirectoryName(path);
            string newFullPath = path;

            int count = 1;
            while (File.Exists(newFullPath))
            {
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(directory, tempFileName + extension);
            }

            if (path != newFullPath)
                File.Move(path, newFullPath);

            using (var workbook = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook))
            {
                //ds = changeTablesName(ds);

                var workbookPart = workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new Sheets();

                foreach (DataTable table in ds.Tables)
                {
                    var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new SheetData();
                    sheetPart.Worksheet = new Worksheet(sheetData);

                    Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                    string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                    uint sheetId = 1;
                    if (sheets.Elements<Sheet>().Count() > 0)
                    {
                        sheetId =
                            sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }

                    Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                    sheets.Append(sheet);

                    Row headerRow = new Row();

                    List<String> columns = new List<string>();
                    foreach (System.Data.DataColumn column in table.Columns)
                    {
                        columns.Add(column.ColumnName);

                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(column.ColumnName);
                        headerRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(headerRow);

                    foreach (DataRow dsrow in table.Rows)
                    {
                        Row newRow = new Row();
                        foreach (String col in columns)
                        {
                            Cell cell = new Cell();
                            float f;
                            if (dsrow[col].ToString().All(char.IsDigit) || float.TryParse(dsrow[col].ToString(), out f))
                            {
                                if (col == "bankAccNo")
                                {
                                    cell.DataType = CellValues.String;
                                }
                                else
                                {
                                    cell.DataType = CellValues.Number;
                                }
                            }
                            else
                            {
                                cell.DataType = CellValues.String;
                            }
                            cell.CellValue = new CellValue(dsrow[col].ToString());
                            newRow.AppendChild(cell);
                        }
                        sheetData.AppendChild(newRow);
                    }
                }
            }
        }
        public string downloadExcel(DataSet ds, string fileName, string locationPath)
        {
            string date = DateTime.Now.ToString("dd-MMM-yyyy.hh.mm.s");
            string file = fileName;
            var path = Path.Combine(locationPath, file + ".xlsx");
            var serverPath = Path.Combine(locationPath, file + ".xlsx");
            using (var workbook = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new Sheets();

                foreach (System.Data.DataTable table in ds.Tables)
                {
                    var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new SheetData();
                    sheetPart.Worksheet = new Worksheet(sheetData);

                    Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                    string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                    uint sheetId = 1;
                    if (sheets.Elements<Sheet>().Count() > 0)
                    {
                        sheetId =
                            sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }

                    Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                    sheets.Append(sheet);

                    Row headerRow = new Row();

                    List<String> columns = new List<string>();
                    foreach (System.Data.DataColumn column in table.Columns)
                    {
                        columns.Add(column.ColumnName);

                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(column.ColumnName);
                        headerRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(headerRow);

                    foreach (System.Data.DataRow dsrow in table.Rows)
                    {
                        Row newRow = new Row();
                        foreach (String col in columns)
                        {
                            Cell cell = new Cell();
                            float f;
                            if (dsrow[col].ToString().All(char.IsDigit) || float.TryParse(dsrow[col].ToString(), out f))
                            {
                                if (col == "bankAccNo")
                                {
                                    cell.DataType = CellValues.String;
                                }
                                else
                                {
                                    cell.DataType = CellValues.Number;
                                }
                            }
                            else
                            {
                                cell.DataType = CellValues.String;
                            }
                            cell.CellValue = new CellValue(dsrow[col].ToString());
                            newRow.AppendChild(cell);
                        }
                        sheetData.AppendChild(newRow);
                    }

                }
            }
            return serverPath;
        }
        public string Encode(string encodeMe)
        {
            // three tier encoding
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            var pwd = Convert.ToBase64String(encoded);

            byte[] encoded1 = System.Text.Encoding.UTF8.GetBytes(pwd);
            var pwd1 = Convert.ToBase64String(encoded1);

            byte[] encoded2 = System.Text.Encoding.UTF8.GetBytes(pwd1);
            var pwd2 = Convert.ToBase64String(encoded2);

            return pwd2;
        }
        public string Decode(string decodeMe)
        {
            // three tier decoding
            byte[] encoded = Convert.FromBase64String(decodeMe);
            var pwd = System.Text.Encoding.UTF8.GetString(encoded);

            byte[] encoded1 = Convert.FromBase64String(pwd);
            var pwd1 = System.Text.Encoding.UTF8.GetString(encoded1);

            byte[] encoded2 = Convert.FromBase64String(pwd1);
            var pwd2 = System.Text.Encoding.UTF8.GetString(encoded2);

            return pwd2;
        }


        public int CreateRandomNumber()
        {
            string allowedChars = "";
            allowedChars += "1,2,3,4,5,6,7,8,9,0";

            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string GenrateNumber = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                GenrateNumber += temp;
            }
            return Convert.ToInt32(GenrateNumber);
        }



    }

    public class SchedulerTask
    {
        public async void Start(int Hour, int Minute)
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            await scheduler.Start();
            IJobDetail job = JobBuilder.Create<ExectueSendEmail>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("SendEmail", "SendEmail")
               .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Hour, Minute))
               .WithPriority(1)
               .Build();
            await scheduler.ScheduleJob(job, trigger);
        }

    }
    public class ExectueSendEmail : IJob
    {
        Task IJob.Execute(IJobExecutionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            EmailRecords email = new EmailRecords();
            Task task1 = new Task(email.SendEmail);
            task1.Start();
            return task1;
        }
    }
    public class EmailRecords
    {
        Common_Class commonClass = new Common_Class();
        Dictionary<string, object> dict = new Dictionary<string, object>();
        public void SendEmail()
        {
            dict.Clear();
            dict.Add("@mode", "SendEmailByScheduler");
            DataSet ds = commonClass.return_dataset(dict, "Proc_role");
            if (ds.Tables.Count > 0)
            {
                MailMessage mm = new MailMessage("developer@jivo.in", "gurvinder@jivo.in");//gurvinder@jivo.in
                //MailMessage mm = new MailMessage("developer@jivo.in", "Nitinlokesh21@gmail.com");//gurvinder@jivo.in
                mm.To.Add("delhibpo@jivo.in");
                //MailMessage mm = new MailMessage("developer@jivo.in", "gurvinder@jivo.in");//gurvinder@jivo.in
                mm.Subject = "Duplicate Pending Orders of Today";
                string TextBody = "<table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + ">" +
                     "<tr bgcolor='#4da6ff'>" +
                    "<td><b>Item ID</b></td>" +
                    "<td><b>Customer Name</b></td>" +
                    "<td><b>Customer Ph NO</b></td>" +
                    "<td><b>Caller Name</b></td>" +
                    "<td><b>Item Name</b></td>" +
                    "<td><b>Item Rate</b></td>" +
                    "<td><b>Item Quentity</b></td>" +
                    "<td><b>Item Total Price</b></td>" +
                    "<td><b>Insert Date</b></td>" +
                    "<td><b>Inserted By</b></td>" +
                    "</tr>";

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    TextBody += "<tr>" +
                       "<td> " + Convert.ToString(dr["itemID"]) + "</td>" +
                        "<td>" + "(" + Convert.ToInt32(dr["Customer_id"]) + ")" + " " + dr["Name"].ToString() + "</td>" +
                        "<td> " + Convert.ToString(dr["Mobile"]) + "</td>" +
                        "<td> " + "(" + Convert.ToInt32(dr["Caller_id"]) + ")" + " " + dr["Caller_Name"].ToString() + "</td>" +
                        "<td> " + "(" + Convert.ToInt32(dr["item_id"]) + ")" + " " + dr["Item_Name"].ToString() + "</td>" +
                        "<td> " + Convert.ToDouble(dr["item_rate"]) + "</td>" +
                        "<td> " + Convert.ToInt32(dr["item_qty"]) + "</td>" +
                        "<td> " + Convert.ToDouble(dr["Total_item_price"]) + "</td>" +
                        "<td> " + dr["Inserted_date"].ToString() + "</td>" +
                        "<td> " + dr["Inserted_by"].ToString() + "</td>" +
                        " </tr>";
                }
                TextBody += "</table>";
                mm.Body = TextBody;
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
            }
        }
    }


  

}