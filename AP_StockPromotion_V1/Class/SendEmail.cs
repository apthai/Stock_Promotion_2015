using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AP_StockPromotion_V1.Class
{
    public class SendEmail
    {
        private static string connStr = ConfigurationManager.ConnectionStrings["db_APStockPromotion"].ConnectionString;
        private string Username = ConfigurationManager.AppSettings["SMTPUsername"];
        private string Password = ConfigurationManager.AppSettings["SMTPPassword"];
        private string ProfileName = ConfigurationManager.AppSettings["ProfileName"];

        public bool Send(string recipients, string subject, string body, string fileAttachment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("msdb.dbo.sp_send_dbmail", conn);
                    sqlComm.Parameters.Add("profile_name", SqlDbType.VarChar).Value = ProfileName;
                    sqlComm.Parameters.AddWithValue("@recipients", SqlDbType.VarChar).Value = recipients;
                    sqlComm.Parameters.AddWithValue("@subject", SqlDbType.VarChar).Value = subject;
                    sqlComm.Parameters.AddWithValue("@body", SqlDbType.VarChar).Value = body;
                    sqlComm.Parameters.AddWithValue("@body_format", SqlDbType.VarChar).Value = "HTML";
                    sqlComm.Parameters.AddWithValue("@file_attachments", SqlDbType.VarChar).Value = fileAttachment;
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}