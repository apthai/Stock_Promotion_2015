using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AP_StockPromotion_V1.Class
{
    public class DAStockItemDestroy
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

        public DataTable getDataStockItemProjectDestroyed(long Project_Id, long MasterItemId)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataStockItemProjectDestroyed", conn);
                    if (MasterItemId != 0)
                    {
                        sqlComm.Parameters.AddWithValue("@MasterItemId", MasterItemId);
                    }
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);
                }
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getDataStockItemProjectDestroyedByDestroyListId(long DestroyListId)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataStockItemProjectDestroyed", conn);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.Parameters.AddWithValue("@DestroyListId", DestroyListId);
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

        public DataTable getDataStockItemFormProjectDestroy(Int64 Project_Id, Int64 MasterItemId)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataStockItemFormProjectDestroy", conn);
                    sqlComm.Parameters.AddWithValue("@Project_Id", Project_Id);
                    if (MasterItemId != 0)
                    {
                        sqlComm.Parameters.AddWithValue("@MasterItemId", MasterItemId);
                    }
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

        public DataTable getDataStockItemFormCenterStockDestroy(long MasterItemId)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataStockItemFormCenterStockDestroy", conn);
                    sqlComm.Parameters.AddWithValue("@MasterItemId", MasterItemId);
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

        public bool destroyItem(string itemList, string reason, string userId, string OBJ_TYPE, string OBJ_KEY, string OBJ_SYS)
        {
            bool rst = true;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlTransaction tr = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand sqlComm = new SqlCommand("dbo.spDestroyItem", conn, tr);
                        sqlComm.Parameters.AddWithValue("@itemList", itemList);
                        sqlComm.Parameters.AddWithValue("@DestroyReason", reason);
                        sqlComm.Parameters.AddWithValue("@User", userId);
                        sqlComm.Parameters.AddWithValue("@OBJ_TYPE", OBJ_TYPE);
                        sqlComm.Parameters.AddWithValue("@OBJ_KEY", OBJ_KEY);
                        sqlComm.Parameters.AddWithValue("@OBJ_SYS", OBJ_SYS);
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


        public bool cancelDestroyItem(int DestroyListId, string userId, string OBJ_TYPE, string OBJ_KEY, string OBJ_SYS)
        {
            bool rst = true;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlTransaction tr = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand sqlComm = new SqlCommand("dbo.spCancelDestroyItem", conn, tr);
                        sqlComm.Parameters.AddWithValue("@DestroyListId", DestroyListId);
                        sqlComm.Parameters.AddWithValue("@OBJ_TYPE_C", OBJ_TYPE);
                        sqlComm.Parameters.AddWithValue("@OBJ_KEY_C", OBJ_KEY);
                        sqlComm.Parameters.AddWithValue("@OBJ_SYS_C", OBJ_SYS);
                        sqlComm.Parameters.AddWithValue("@UserID", userId);
                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.ExecuteNonQuery();
                        tr.Commit();
                        rst = true;
                    }
                    catch (Exception ex)
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

        public bool destroyReasonEdit(string DestroyListId, string DestroyReason, string EmpployeeID)
        {
            bool rst = true;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlTransaction tr = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand sqlComm = new SqlCommand("dbo.spDestroyReasonEdit", conn, tr);
                        sqlComm.Parameters.AddWithValue("@DestroyListId", DestroyListId);
                        sqlComm.Parameters.AddWithValue("@DestroyReason", DestroyReason);
                        sqlComm.Parameters.AddWithValue("@EmpployeeID", EmpployeeID);
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