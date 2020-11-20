using System;
using System.Net.Mail;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AP_StockPromotion_V1.Class
{
    public class classMail
    {
        private string mSmtpServer = ConfigurationManager.AppSettings["SMTPServer"];
        private string mSMTPUsername = ConfigurationManager.AppSettings["SMTPUsername"];
        private string mSMTPPassword = ConfigurationManager.AppSettings["SMTPPassword"];
        private int mSMTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);

        private static string connStr = ConfigurationManager.ConnectionStrings["db_APStockPromotion"].ConnectionString;

        public bool sendMail(string mailForm, string mailTo, string mailCc, string mailBcc, string mailSubject, string mailBody, string fileAttachment, ref string msgErr)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient(mSmtpServer);

                mail.From = new MailAddress(mailForm);
                mail.To.Add(mailTo);
                mail.Subject = mailSubject;
                mail.Body = mailBody;
                mail.IsBodyHtml = true;

                if (fileAttachment != "")
                {
                    Attachment attachment;
                    string[] fileAtt = fileAttachment.Split(';');
                    foreach (string f in fileAtt)
                    {
                        attachment = new Attachment(f);
                        mail.Attachments.Add(attachment);
                    }
                }

                smtpServer.Port = 25;
                //smtpServer.Credentials = new System.Net.NetworkCredential("puwarun_p@apthai.com", "Limpkinpark0");
                //smtpServer.Credentials = new System.Net.NetworkCredential(mSMTPUsername, mSMTPPassword);
                smtpServer.UseDefaultCredentials = true;
                smtpServer.EnableSsl = false;
                smtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                msgErr = ex.Message;
                return false;
            }
        }

        public bool sendMultiEmail(string mailForm, string[] mailTo, string[] mailCc, string[] mailBcc, string mailSubject, string mailBody, string fileAttachment, ref string msgErr)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient(mSmtpServer);

                mail.From = new MailAddress(mailForm);

                for (int i = 0; i < mailTo.Length; i++)
                {
                    if (mailTo[i] != "")
                    {
                        mail.To.Add(mailTo[i]);
                    }
                }

                for (int i = 0; i < mailCc.Length; i++)
                {
                    if (mailCc[i] != "")
                    {
                        mail.CC.Add(mailCc[i]);
                    }
                }

                for (int i = 0; i < mailBcc.Length; i++)
                {
                    if (mailBcc[i] != "")
                    {
                        mail.Bcc.Add(mailBcc[i]);
                    }
                }

                mail.Subject = mailSubject;
                mail.Body = mailBody;
                mail.IsBodyHtml = true;

                if (fileAttachment != "")
                {
                    Attachment attachment;
                    string[] fileAtt = fileAttachment.Split(';');
                    foreach (string f in fileAtt)
                    {
                        attachment = new Attachment(f);
                        mail.Attachments.Add(attachment);
                    }
                }

                smtpServer.Port = 25;
                smtpServer.UseDefaultCredentials = true;
                smtpServer.EnableSsl = false;
                smtpServer.Send(mail);
                msgErr = "";
                return true;
            }
            catch (Exception ex)
            {
                msgErr = ex.Message;
                return false;
            }
        }


        public DataTable getDateWarningReturnItem(string isAlert)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spRptWarningReturnItem", conn);
                    sqlComm.Parameters.AddWithValue("@isAlert", isAlert);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);
                }
                return ds.Tables[0];
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}