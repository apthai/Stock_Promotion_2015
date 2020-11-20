using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AP_StockPromotion_V1.Class
{
    public class DATransferToProject
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

        public DataTable getDataRequestList(Int64 ReqId, int? setFn)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataRequest", conn);
                    sqlComm.Parameters.AddWithValue("@ReqId", ReqId);
                    sqlComm.Parameters.AddWithValue("@Function", setFn);


                    // sqlComm.Parameters.AddWithValue("@withCRMData", "Y");
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

        public DataTable getDataRequestListByRequestList(string ReqIdList, string ReqFn)//, int ReqFn
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataRequestByReqList", conn);
                    sqlComm.Parameters.AddWithValue("@ReqIdList", ReqIdList);
                    sqlComm.Parameters.AddWithValue("@ReqFn", ReqFn);
                    //sqlComm.Parameters.AddWithValue("@withCRMData", "Y");
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

        public DataTable getDataRequestList(Int64 reqHeaderId, Int64 reqProject, Int32? reqFunction)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataRequest", conn);
                    sqlComm.Parameters.AddWithValue("@ReqHeaderId", reqHeaderId);
                    sqlComm.Parameters.AddWithValue("@Project_Id", reqProject);
                    sqlComm.Parameters.AddWithValue("@Function", reqFunction);
                    // sqlComm.Parameters.AddWithValue("@withCRMData", "Y");
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


        public DataTable getDataTransferItemToProjectHistoryByReqIdList(string ReqIdList, string refId)
        { 
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDateTransferItemToProjectHistoryByReqIdList", conn);
                    sqlComm.Parameters.AddWithValue("@ReqIdList", ReqIdList);
                    sqlComm.Parameters.AddWithValue("@ReqFn", refId);
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

        public DataTable getDataRequestDetail(Entities.RequisitionInfo req, int? setFn)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataRequest", conn);

                    if (req.ReqHeaderId == 0)
                    {
                        sqlComm.Parameters.AddWithValue("@ReqHeaderId", DBNull.Value);
                    }
                    else
                    {
                        sqlComm.Parameters.AddWithValue("@ReqHeaderId", req.ReqHeaderId);
                    }
                    sqlComm.Parameters.AddWithValue("@ReqNo", req.ReqNo);
                    sqlComm.Parameters.AddWithValue("@ReqDocNo", req.ReqDocNo);
                    sqlComm.Parameters.AddWithValue("@ReqDateFrom", req._ReqDateFrom);
                    sqlComm.Parameters.AddWithValue("@ReqDateTo", req._ReqDateTo);
                    sqlComm.Parameters.AddWithValue("@Project_Id", req.Project_Id);
                    sqlComm.Parameters.AddWithValue("@ItemId", req.ItemId);
                    sqlComm.Parameters.AddWithValue("@Function", setFn);
                    // sqlComm.Parameters.AddWithValue("@withCRMData", "Y");
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.CommandTimeout = 300;
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

        public DataTable getDataRequestDetailByReqId(Entities.RequisitionInfo req, int? setFn)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataRequest", conn);
                    sqlComm.Parameters.AddWithValue("@ReqId", req.ReqId);
                    sqlComm.Parameters.AddWithValue("@Function", setFn);
                    // sqlComm.Parameters.AddWithValue("@withCRMData", "Y");
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

        public DataTable getItemForTransferToProjectBySerial(int masterItemId, string serial, string currItemLst)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetItemForTransferToProject", conn);

                    sqlComm.Parameters.AddWithValue("@MasterItemId", masterItemId);
                    sqlComm.Parameters.AddWithValue("@Serial", serial);
                    sqlComm.Parameters.AddWithValue("@currentItemList", currItemLst);
                    // @currentItemList

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

        public DataTable getItemForTransferToProjectBySerial(string serial, string currItemLst)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetItemForTransferToProject", conn);

                    sqlComm.Parameters.AddWithValue("@Serial", serial);
                    sqlComm.Parameters.AddWithValue("@currentItemList", currItemLst);
                    // @currentItemList

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

        public DataTable getItemForTransferToProjectByAmount(int itemId, string currItemLst)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetItemForTransferToProject", conn);
                    sqlComm.Parameters.AddWithValue("@ItemId", itemId);
                    sqlComm.Parameters.AddWithValue("@currentItemList", currItemLst);
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

        public DataTable getItemForTransferToProjectByMasterItemId(int masterItemId)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataStockItemForTransferToProject", conn);
                    sqlComm.Parameters.AddWithValue("@MasterItemId", masterItemId);
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

        public DataTable getItemForTransferToProjectByAmount(int masterItemId, int transferAmount, string currItemLst)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetItemForTransferToProject", conn);

                    sqlComm.Parameters.AddWithValue("@MasterItemId", masterItemId);
                    sqlComm.Parameters.AddWithValue("@transferAmount", transferAmount);
                    sqlComm.Parameters.AddWithValue("@currentItemList", currItemLst);
                    // @currentItemList

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

        /*
         *  บ้าน สร้างไม่สวย คนไม่อยากซื้อ
         *  บ้าน สร้างไม่เสร็จ ก็เช่นกัน...(ข้าพเจ้ารีบ!!)
         *                  ชิตพล กล่าว!
         */

        public bool saveDataTransferItemToProject_old(DataTable dtRequestList, DataTable dtItemTransferToProject, ref string msgErr)
        {
            bool bRst = true;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlTransaction tr = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (DataRow drReq in dtRequestList.Rows)
                        {
                            Int64 ReqId = (Int64)drReq["ReqId"];
                            int Amount = (int)drReq["Transfer"];
                            string ItemStatus = "2";
                            string ItemList = "";
                            DataRow[] drItemList = dtItemTransferToProject.Select("MasterItemId=" + drReq["ItemId"]);
                            if (Amount != 0 && drItemList.Length != 0)
                            {
                                foreach (DataRow dr in drItemList)
                                {
                                    ItemList += "," + dr["ItemId"];
                                }
                                if (ItemList != "") { ItemList = ItemList.Remove(0, 1); }

                                string ReqStatus = "2"; // กำลังดำเนินการ ( ของยังไม่ครบ )
                                if ((int)drReq["Transfer"] == (int)drReq["Balance"])
                                {
                                    ReqStatus = "3"; // จบงาน ส่งของครบแล้ว
                                }

                                SqlCommand sqlComm = new SqlCommand("dbo.spInsertTransferToProject", conn, tr);
                                sqlComm.Parameters.AddWithValue("@ReqId", ReqId);
                                sqlComm.Parameters.AddWithValue("@Amount", Amount);
                                // sqlComm.Parameters.AddWithValue("@CreateBy", CreateBy);
                                sqlComm.Parameters.AddWithValue("@ReqStatus", ReqStatus);
                                sqlComm.Parameters.AddWithValue("@ItemList", ItemList);
                                sqlComm.Parameters.AddWithValue("@ItemStatus", ItemStatus);

                                sqlComm.Parameters.Add("@TransferId", SqlDbType.BigInt);
                                sqlComm.Parameters["@TransferId"].Direction = ParameterDirection.Output;

                                sqlComm.CommandType = CommandType.StoredProcedure;
                                sqlComm.ExecuteNonQuery();

                                Int64 TransferId = (Int64)sqlComm.Parameters["@TransferId"].Value;

                                if (TransferId == 0)
                                {
                                    // ทำช้า !! .. สินค้าโปรโมชั่นบางตัว โดนคนอื่นแย่งไปแล้ว
                                    msgErr = "ทำช้า !! .. สินค้าโปรโมชั่นบางตัว โดนคนอื่นแย่งไปแล้ว.!!";
                                    bRst = false;
                                    break;
                                }

                            }
                        }
                        if (bRst)
                        {
                            tr.Commit();
                        }
                        else
                        {
                            tr.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        msgErr = ex.Message;
                        tr.Rollback();
                        bRst = false;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return bRst;
        }

        public bool saveDataTransferItemToProject(Dictionary<string, Entities.ReqItemList> ReqItemLst,
                                                    string ItemList, long ReqHeaderId, int reqFn, int Project_Id,
                                                    string UserId, ref long TransferId, ref string msgErr)
        {
            bool bRst = true;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlTransaction tr = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand sqlComm = new SqlCommand("dbo.spInsertTransferHeader", conn, tr);
                        sqlComm.CommandTimeout = 0;
                        sqlComm.Parameters.AddWithValue("@ItemList", ItemList);
                        sqlComm.Parameters.AddWithValue("@UserId", UserId);

                        sqlComm.Parameters.Add("@TrListId", SqlDbType.BigInt);
                        sqlComm.Parameters["@TrListId"].Direction = ParameterDirection.Output;

                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.ExecuteNonQuery();

                        TransferId = (long)sqlComm.Parameters["@TrListId"].Value;

                        if (TransferId < 1)
                        {
                            // ทำช้า !! .. สินค้าโปรโมชั่นบางตัว โดนคนอื่นแย่งไปแล้ว
                            msgErr = "ทำช้า !! .. สินค้าโปรโมชั่นบางตัว โดนคนอื่นแย่งไปแล้ว.!!";
                            bRst = false;
                        }
                        else
                        {
                            foreach (var reqItem in ReqItemLst)
                            {
                                sqlComm = new SqlCommand("dbo.spInsertTransferDetail", conn, tr);
                                sqlComm.Parameters.Add("@TrListId", SqlDbType.BigInt).Value = Convert.ToInt32(TransferId);
                                sqlComm.Parameters.Add("@ReqId", SqlDbType.BigInt).Value = Convert.ToInt64(reqItem.Key);
                                sqlComm.Parameters.Add("@ItemList", SqlDbType.VarChar).Value = reqItem.Value.ItemList;
                                sqlComm.Parameters.Add("@UserId", SqlDbType.VarChar ,10).Value = UserId;
                                sqlComm.Parameters.Add("@RefMatId", SqlDbType.BigInt).Value = Convert.ToInt32(reqItem.Value.refMatId);
                                sqlComm.Parameters.Add("@ReqFn", SqlDbType.Int).Value = reqFn;

                                sqlComm.CommandType = CommandType.StoredProcedure;
                                sqlComm.ExecuteNonQuery();
                            }

                            tr.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        msgErr = ex.Message;
                        tr.Rollback();
                        bRst = false;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return bRst;
        }

        public bool cancelTransferItemToProject(long TransferId, long ReqId, string UserId, string selFn, ref string msgErr)
        {
            bool bRst = true;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spCancelTransferItemToProject", conn);
                    sqlComm.Parameters.AddWithValue("@TrListId", TransferId);
                    sqlComm.Parameters.AddWithValue("@ReqId", ReqId);
                    sqlComm.Parameters.AddWithValue("@userID", UserId);
                    sqlComm.Parameters.AddWithValue("@SelFn", selFn);
                    sqlComm.Parameters.Add("@msgErr", SqlDbType.VarChar,1000);
                    sqlComm.Parameters["@msgErr"].Direction = ParameterDirection.Output;

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();

                    msgErr = sqlComm.Parameters["@msgErr"].Value + "";
                    if (msgErr != "")
                    {
                        bRst = false;
                    }
                }
                catch (Exception ex)
                {
                    msgErr = ex.Message.Replace("'", "");
                    bRst = false;
                }
                finally
                {
                    conn.Close();
                }

            }
            return bRst;
        }

        public string lookTable(DataSet ds)
        {
            string rst = "";
            foreach (DataTable dt in ds.Tables)
            {
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
                rst += "" + "\n";
            }
            return rst;
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
    }
}