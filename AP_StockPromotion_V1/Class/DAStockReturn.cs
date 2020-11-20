using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AP_StockPromotion_V1.Class
{
    public class DAStockReturn
    {

        private static string connStr = ConfigurationManager.ConnectionStrings["db_APStockPromotion"].ConnectionString;

        public DataSet execDataSet(string sql)
        {
            try
            {
                DataSet ds = new DataSet();

                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand comm = new SqlCommand(sql, conn))
                using (SqlDataAdapter da = new SqlDataAdapter(comm))
                {
                    da.Fill(ds);
                }

                return ds;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable execDataTable(string sql)
        {
            try
            {
                DataSet ds = new DataSet();

                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand comm = new SqlCommand(sql, conn))
                using (SqlDataAdapter da = new SqlDataAdapter(comm))
                {
                    da.Fill(ds);
                }
                return ds.Tables[0];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool execCommand(string sql)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string lookTable(DataTable dt)
        {
            string rst = "";
            foreach (DataColumn dc in dt.Columns)
            {
                rst += dc.ColumnName + "\t";
            }
            rst += "\n";
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    rst += dr[dc.ColumnName] + "\t";
                }
                rst += "\n";
            }
            return rst;
        }

        public DataTable getDataStockProject(string project_Id)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataStockItemProject", conn);
                    sqlComm.Parameters.AddWithValue("@ProjectId", project_Id);
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

        public DataTable getDataStockLowPriceProject(int project_Id)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataStockLowPriceItemProject", conn);
                    sqlComm.Parameters.AddWithValue("@Project_Id", project_Id);
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

        public DataTable getDataReturned(int ReturnListId)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDateReturned", conn);
                    sqlComm.Parameters.AddWithValue("@ReturnListId", ReturnListId);
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

        public DataTable getDataReturned(int Project_Id, int ItemId, DateTime DateBeg, DateTime DateEnd)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDateReturned", conn);
                    if (Project_Id != 0) { sqlComm.Parameters.AddWithValue("@Project_Id", Project_Id); }
                    if (ItemId != 0) { sqlComm.Parameters.AddWithValue("@MasterItemId", ItemId); }
                    if (DateBeg != null) { sqlComm.Parameters.AddWithValue("@RetDateBegin", DateBeg); }
                    if (DateEnd != null) { sqlComm.Parameters.AddWithValue("@RetDateEnd", DateEnd); }
                    
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

        public bool returnItem(string itemList, int project_Id,string userId,string reason)
        {
            bool rst = true;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlTransaction tr = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand sqlComm = new SqlCommand("dbo.spReturnItemToHO", conn, tr);
                        sqlComm.Parameters.AddWithValue("@ItemList", itemList);
                        sqlComm.Parameters.AddWithValue("@Project_Id", project_Id);
                        sqlComm.Parameters.AddWithValue("@UserId", userId);
                        sqlComm.Parameters.AddWithValue("@Reason", reason);
                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.ExecuteNonQuery();
                        tr.Commit();
                        rst = true;
                    }
                    catch (Exception)
                    {
                        tr.Rollback();
                        rst = false;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return rst;
        }
    }
}