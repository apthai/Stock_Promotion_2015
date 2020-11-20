using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AP_StockPromotion_V1.Class
{
    public class DAReport
    {
        private static string connStr = ConfigurationManager.ConnectionStrings["db_APStockPromotion"].ConnectionString;

        private string SAPExProt_APD = ConfigurationManager.AppSettings["SAPExProt_APD"];
        private string SAPExProt_Domain = ConfigurationManager.AppSettings["SAPExProt_Domain"];
        private string SAPExProt_User = ConfigurationManager.AppSettings["SAPExProt_User"];
        private string SAPExProt_Password = ConfigurationManager.AppSettings["SAPExProt_Password"];


        private SqlConnectionStringBuilder objSB1 = new SqlConnectionStringBuilder(connStr);
            // "server=192.168.0.41;database=db_APStockPromotion;uid=sa;password=P@ssw0rd;"

        public string getUserID()
        {
            return objSB1.UserID;
        }
        public string getPassword()
        {
            return objSB1.Password;
        }
        public string getDataSource()
        {
            return objSB1.DataSource;
        }
        public string getInitialCatalog()
        {
            return objSB1.InitialCatalog;
        }
        public bool getIntegratedSecurity()
        {
            return objSB1.IntegratedSecurity;
        }
        public int getMinPoolSize()
        {
            return objSB1.MinPoolSize;
        }
        public int getMaxPoolSize()
        {
            return objSB1.MaxPoolSize;
        }
        public int getLoadBalanceTimeout()
        {
            return objSB1.LoadBalanceTimeout;
        }

        public string getSAPExProt_APD()
        {
            return SAPExProt_APD;
        }
        public string getSAPExProt_Domain()
        {
            return SAPExProt_Domain;
        }
        public string getSAPExProt_User()
        {
            return SAPExProt_User;
        }
        public string getSAPExProt_Password()
        {
            return SAPExProt_Password;
        }


        public DataTable getRequestType()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataRequestType", conn);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);
                }
                return ds.Tables[0];
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return null;
            }
        }





        public void SetSubreportLogOnInfo(ref ReportDocument crReportDocument)
        {

            foreach (Section crSection in crReportDocument.ReportDefinition.Sections)
            {
                foreach (ReportObject crReportObject in crSection.ReportObjects)
                {
                    if (crReportObject.Kind == ReportObjectKind.SubreportObject)
                    {
                        SubreportObject crSubreportObject = (SubreportObject)crReportObject;
                        ReportDocument crSubreportDocument = crSubreportObject.OpenSubreport(crSubreportObject.SubreportName);
                        ApplyTableLogOnInfo(ref crSubreportDocument);

                    }

                }

            }

        }

        public void ApplyTableLogOnInfo(ref ReportDocument crReportDocument)
        {

            ConnectionInfo crConnectionInfo = SetConnectionInfo();
            foreach (Table crTable in crReportDocument.Database.Tables)
            {

                TableLogOnInfo crLogOnInfo = crTable.LogOnInfo;
                crLogOnInfo.ConnectionInfo = crConnectionInfo;
                crTable.ApplyLogOnInfo(crLogOnInfo);
                crTable.Location = crTable.Location.Substring(crTable.Location.LastIndexOf(".") + 1);

            }

        }

        private ConnectionInfo SetConnectionInfo()
        {
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            crConnectionInfo.ServerName = getDataSource(); // ConfigurationManager.AppSettings["Application.CReportConn.ServerName"];
            crConnectionInfo.DatabaseName = getInitialCatalog(); // ConfigurationManager.AppSettings["Application.CReportConn.DatabaseName"];
            crConnectionInfo.UserID = getUserID();// ConfigurationManager.AppSettings["Application.CReportConn.UserID"];
            crConnectionInfo.Password = getPassword(); // ConfigurationManager.AppSettings["Application.CReportConn.Password"];
            return crConnectionInfo;
        }


        public DataTable getBudgetWaitToPostGL(string ProjectLst)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetBudgetWaitToPostGL", conn);

#pragma warning disable CS0618 // 'SqlParameterCollection.Add(string, object)' is obsolete: 'Add(String parameterName, Object value) has been deprecated.  Use AddWithValue(String parameterName, Object value).  http://go.microsoft.com/fwlink/?linkid=14202'
                    sqlComm.Parameters.Add("@ProjectLst", ProjectLst);
#pragma warning restore CS0618 // 'SqlParameterCollection.Add(string, object)' is obsolete: 'Add(String parameterName, Object value) has been deprecated.  Use AddWithValue(String parameterName, Object value).  http://go.microsoft.com/fwlink/?linkid=14202'
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);
                }
                return ds.Tables[0];
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return null;
            }
        }

    }
}