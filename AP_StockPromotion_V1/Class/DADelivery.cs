using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Hosting;

namespace AP_StockPromotion_V1.Class
{
    public class DADelivery
    {
        private Entities.FormatDate convertDate = new Entities.FormatDate();
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

        public DataTable getDataDelivery(Int64 DeliveryHeaderId)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataDelivery", conn);
                    sqlComm.Parameters.AddWithValue("@DeliveryHeaderId", DeliveryHeaderId);
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

        public DataTable getDataDelivery(string DeliveryNo, string DocNo, string ItemNo, string ProjectId, string dateBeg, string dateEnd)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataDelivery", conn);
                    sqlComm.Parameters.AddWithValue("@DeliveryNo", DeliveryNo);
                    sqlComm.Parameters.AddWithValue("@DocNo", DocNo);
                    sqlComm.Parameters.AddWithValue("@ItemNo", ItemNo);
                    sqlComm.Parameters.AddWithValue("@ProjectId", ProjectId);
                    DateTime DateBeg = new DateTime();
                    if (convertDate.getDateFromString(dateBeg, ref DateBeg))
                    {
                        sqlComm.Parameters.AddWithValue("@DateBeg", DateBeg);
                    }
                    DateTime DateEnd = new DateTime();
                    if (convertDate.getDateFromString(dateEnd, ref DateEnd))
                    {
                        sqlComm.Parameters.AddWithValue("@DateEnd", DateEnd);
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

        public DataTable getDataDeliveryList(string DeliveryNo, string DocNo, string ItemNo, string ProjectId, string dateBeg, string dateEnd)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataDeliveryList", conn);
                    sqlComm.Parameters.AddWithValue("@DeliveryNo", DeliveryNo);
                    sqlComm.Parameters.AddWithValue("@DocNo", DocNo);
                    sqlComm.Parameters.AddWithValue("@ItemNo", ItemNo);
                    sqlComm.Parameters.AddWithValue("@ProjectId", ProjectId);
                    DateTime DateBeg = new DateTime();
                    if (convertDate.getDateFromString(dateBeg, ref DateBeg))
                    {
                        sqlComm.Parameters.AddWithValue("@DateBeg", DateBeg);
                    }
                    DateTime DateEnd = new DateTime();
                    if (convertDate.getDateFromString(dateEnd, ref DateEnd))
                    {
                        sqlComm.Parameters.AddWithValue("@DateEnd", DateEnd);
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

        public DataTable getDataDeliveryLowPriceList(string DeliveryNo, string DocNo, string ItemNo, string ProjectId, string dateBeg, string dateEnd)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataDeliveryLowPriceList", conn);
                    sqlComm.Parameters.AddWithValue("@DeliveryNo", DeliveryNo);
                    sqlComm.Parameters.AddWithValue("@DocNo", DocNo);
                    sqlComm.Parameters.AddWithValue("@ItemNo", ItemNo);
                    sqlComm.Parameters.AddWithValue("@ProjectId", ProjectId);
                    DateTime DateBeg = new DateTime();
                    if (convertDate.getDateFromString(dateBeg, ref DateBeg))
                    {
                        sqlComm.Parameters.AddWithValue("@DateBeg", DateBeg);
                    }
                    DateTime DateEnd = new DateTime();
                    if (convertDate.getDateFromString(dateEnd, ref DateEnd))
                    {
                        sqlComm.Parameters.AddWithValue("@DateEnd", DateEnd);
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

        public DataTable getStockItemProject(string ProjectId, string ItemNo)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetStockItemProject", conn);
                    sqlComm.Parameters.AddWithValue("@ItemNo", ItemNo);
                    sqlComm.Parameters.AddWithValue("@ProjectId", ProjectId);
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
 
        public bool deliveryApproveSendItem(string DeliveryNo, string ItemNo, string TrnIdLst, ref string msgErr)
        {
            bool rst = true;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spDeliveryApprove", conn);
                    sqlComm.Parameters.AddWithValue("@DeliveryNo", DeliveryNo);
                    sqlComm.Parameters.AddWithValue("@ItemNo", ItemNo);
                    sqlComm.Parameters.AddWithValue("@TrnIdLst", TrnIdLst);
                    sqlComm.Parameters.Add("@MsgErr", SqlDbType.BigInt);
                    sqlComm.Parameters["@MsgErr"].Direction = ParameterDirection.Output;

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    msgErr = sqlComm.Parameters["@MsgErr"].Value + "";
                    if (msgErr != "")
                    {
                        rst = false;
                    }
                }
                catch (Exception ex)
                {
                    rst = false;
                    msgErr = ex.Message;
                }
                finally
                {
                    conn.Close();
                }
            }
            return rst;
        }

        public bool deliveryItemLowPrice(ref string DelvPromotionId, string ProjectID, string UserId, string DocDate, string CostCenter, string TrLst, ref string msgErr)
        {
            bool rst = true;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spLowPriceDeliveryItem", conn);
                    //sqlComm.Parameters.AddWithValue("@DelvPromotionId", DelvPromotionId);
                    sqlComm.Parameters.AddWithValue("@UserId", UserId);
                    DateTime docDate = new DateTime();
                    DateTime dateNow = DateTime.Now;
                    if (convertDate.getDateFromString(DocDate, ref docDate))
                    {
                        sqlComm.Parameters.AddWithValue("@DocDate", new DateTime(docDate.Year, docDate.Month, docDate.Day, dateNow.Hour, dateNow.Minute, dateNow.Second));
                    }
                    else
                    {
                        sqlComm.Parameters.AddWithValue("@DocDate", DateTime.Now);
                    }
                    sqlComm.Parameters.AddWithValue("@ProjectID", ProjectID);
                    sqlComm.Parameters.AddWithValue("@CostCenter", CostCenter);
                    sqlComm.Parameters.AddWithValue("@TrIdLst", TrLst);
                    sqlComm.Parameters.Add("@DelvPromotionId", SqlDbType.VarChar, 20);
                    sqlComm.Parameters.Add("@MsgErr", SqlDbType.BigInt);
                    sqlComm.Parameters["@DelvPromotionId"].Direction = ParameterDirection.Output;
                    sqlComm.Parameters["@MsgErr"].Direction = ParameterDirection.Output;

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    msgErr = sqlComm.Parameters["@MsgErr"].Value + "";
                    DelvPromotionId = sqlComm.Parameters["@DelvPromotionId"].Value + "";
                    if (msgErr != "")
                    {
                        rst = false;
                    }
                }
                catch (Exception ex)
                {
                    rst = false;
                    msgErr = ex.Message;
                }
                finally
                {
                    conn.Close();
                }
            }
            return rst;
        }

        public DataTable GetDataDeliveryItem(string DelvPromotionId, string ProjectId, string itemNo, DateTime DateBeg, DateTime DateEnd, string wbs, string isLowPrice, string isConfirm, string isPostAcc)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataDeliveryItem", conn);
                    sqlComm.Parameters.AddWithValue("@DelvPromotionId", DelvPromotionId);
                    sqlComm.Parameters.AddWithValue("@ProjectId", ProjectId);
                    sqlComm.Parameters.AddWithValue("@ItemNo", itemNo);
                    sqlComm.Parameters.AddWithValue("@DateBeg", DateBeg);
                    sqlComm.Parameters.AddWithValue("@DateEnd", DateEnd);
                    sqlComm.Parameters.AddWithValue("@WBS", wbs);
                    sqlComm.Parameters.AddWithValue("@isLowPrice", isLowPrice);
                    sqlComm.Parameters.AddWithValue("@isConfirm", isConfirm);
                    sqlComm.Parameters.AddWithValue("@isPostAcc", isPostAcc);
                    /*
                     	@DelvPromotionId as varchar(20) = '',
	                    @DateBeg as datetime = null,
	                    @DateEnd as datetime = null,
	                    @isLowPrice as varchar(2) = '',
	                    @isConfirm as varchar(2) = '',
	                    @isPostAcc as varchar(2) = ''
                     */
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.CommandTimeout = 300;
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

        public DataTable GetDataDeliveryItemFromDelvLst(string DelvLst)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    DelvLst = DelvLst.Replace(",", ";");
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataDeliveryItemFromDelvLst", conn);
                    sqlComm.Parameters.AddWithValue("@DelvLst", DelvLst);
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

        public DataTable GetDataDeliveryItemFromDelvLst_Check(string DelvLst)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    DelvLst = DelvLst.Replace(",", ";");
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataDeliveryItemFromDelvLst_Check", conn); 
                    sqlComm.Parameters.AddWithValue("@DelvLst", DelvLst);
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

        public DataTable getRequestDetailById(int ReqId)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT * FROM [RequestDetail] WHERE ReqId = @1", sqlConn))
                    {
                        sqlCmd.Parameters.Add(new SqlParameter("1", ReqId));

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }
                return ds.Tables[0];
            }

            catch (Exception)
            {
                return new DataTable();
            }
        }

        public DataTable GetEqualDataDeliveryItemFromDelvLst(string DelvLst)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    DelvLst = DelvLst.Replace(",", ";");
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spGetEqulDataDeliveryItemFromDelvLst]", conn);
                    sqlComm.Parameters.AddWithValue("@DelvLst", DelvLst);
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
        public bool CheckEqualtation(string DelvLst)
        {
            try
            {
                bool isTrue = false;
                DataSet ds = new DataSet();
                using (SqlConnection sqlCon = new SqlConnection(connStr))
                {
                    if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spCheckEqualtation]", sqlCon))
                    {
                        DelvLst = DelvLst.Replace(",", ";");
                        sqlCmd.Parameters.AddWithValue("@DelvLst", DelvLst);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                    if (ds.Tables[0].Rows.Count != 0) isTrue = true;
                }
                return isTrue;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public DataTable GetDataDeliveryItemFromSAPDocNo(string SAPDOC)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataDeliveryItemFromSAPDOCNo", conn);
                    sqlComm.Parameters.AddWithValue("@SAPDOC", SAPDOC);
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

        public DataTable GetDataDeliveryItem(Int64 DelvLstId)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataDeliveryItem", conn);
                    sqlComm.Parameters.AddWithValue("@DelvLstId", DelvLstId);
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


        public bool cancelDeliveryItem(Int64 delvLstId, string userId)
        {
            bool rst = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("dbo.spCancelDeliveryItem", conn);
                    sqlComm.Parameters.AddWithValue("@DelvLstId", delvLstId);
                    sqlComm.Parameters.AddWithValue("@UserId", userId);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                }
            }

            catch (Exception)

            {
                rst = false;
            }
            return rst;
        }

        public bool UpdatePRItem(string PRNo, string itemNo, string prItem, ref string msgErr)
        {
            bool rst = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("dbo.spUpdatePRItem", conn);
                    sqlComm.Parameters.AddWithValue("@PRNo", PRNo);
                    sqlComm.Parameters.AddWithValue("@itemNo", itemNo);
                    sqlComm.Parameters.AddWithValue("@prItem", prItem);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                }
            }

            catch (Exception)

            {
                rst = false;
            }
            return rst;
        }

        public bool verifyDeliveryItem(Int64 delvLstId, string userId, string isConfirm)
        {
            bool rst = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("dbo.spVerifyDeliveryItem", conn);
                    sqlComm.Parameters.AddWithValue("@DelvLstId", delvLstId);
                    sqlComm.Parameters.AddWithValue("@isConfirm", isConfirm);
                    sqlComm.Parameters.AddWithValue("@UserId", userId);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception)
            {
                rst = false;
            }
            return rst;
        }


        public bool cancelVerifyDeliveryItem(Int64 delvLstId, string userId)
        {
            bool rst = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("dbo.spCancelVerifyDeliveryItem", conn);
                    sqlComm.Parameters.AddWithValue("@DelvLstId", delvLstId);
                    sqlComm.Parameters.AddWithValue("@UserId", userId);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                }
            }

            catch (Exception)

            {
                rst = false;
            }
            return rst;
        }

        #region // คิม -- 2018-09-17 เพิ่ม UpdateBy ไว้บันทึกผู้ทำรายการ -- เดิมเป็น User ที่ใช้ส่งไป SAP

        public bool UpdateStatusPostAccount(string DelvLst, string OBJ_TYPE, string OBJ_KEY, string OBJ_SYS, string SAP_User, DateTime postAccDate, string reftxt, string ref_key_3, string UpdateBy)
        {
            bool rst = true;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlTransaction tr = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand sqlComm = new SqlCommand("dbo.spDeliveryUpdateStatusPostAccount", conn, tr);
                        sqlComm.Parameters.AddWithValue("@DelvLst", DelvLst);
                        sqlComm.Parameters.AddWithValue("@PostRet_TYPE", OBJ_TYPE);
                        sqlComm.Parameters.AddWithValue("@PostRet_KEY", OBJ_KEY);
                        sqlComm.Parameters.AddWithValue("@PostRet_SYS", OBJ_SYS);
                        sqlComm.Parameters.AddWithValue("@PostAccDate", postAccDate);
                        sqlComm.Parameters.AddWithValue("@Reference", reftxt);
                        sqlComm.Parameters.AddWithValue("@Ref_Key_3", ref_key_3);
                        sqlComm.Parameters.AddWithValue("@UserId", UpdateBy);
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

        public bool UpdateStatusPostAccountCrossCmp(string DelvLst, string OBJ_TYPE, string OBJ_KEY, string OBJ_SYS, string OBJ_TYPE2, string OBJ_KEY2, string OBJ_SYS2, string OBJ_TYPE3, string OBJ_KEY3, string OBJ_SYS3, string SAP_User, DateTime postAccDate, string REFTXT, string UpdateBy, ref string msgErr)
        {
            bool rst = true;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlTransaction tr = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand sqlComm = new SqlCommand("dbo.spDeliveryUpdateStatusPostAccountCrossCmp", conn, tr);
                        sqlComm.Parameters.AddWithValue("@DelvLst", DelvLst);
                        sqlComm.Parameters.AddWithValue("@PostRet_TYPE", OBJ_TYPE);
                        sqlComm.Parameters.AddWithValue("@PostRet_KEY", OBJ_KEY);
                        sqlComm.Parameters.AddWithValue("@PostRet_SYS", OBJ_SYS);
                        sqlComm.Parameters.AddWithValue("@PostRet_TYPE2", OBJ_TYPE2);
                        sqlComm.Parameters.AddWithValue("@PostRet_KEY2", OBJ_KEY2);
                        sqlComm.Parameters.AddWithValue("@PostRet_SYS2", OBJ_SYS2);
                        sqlComm.Parameters.AddWithValue("@PostRet_TYPE3", OBJ_TYPE3);
                        sqlComm.Parameters.AddWithValue("@PostRet_KEY3", OBJ_KEY3);
                        sqlComm.Parameters.AddWithValue("@PostRet_SYS3", OBJ_SYS3);
                        sqlComm.Parameters.AddWithValue("@PostAccDate", postAccDate);
                        sqlComm.Parameters.AddWithValue("@REFTXT", REFTXT);
                        sqlComm.Parameters.AddWithValue("@UserId", UpdateBy);
                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.ExecuteNonQuery();
                        tr.Commit();
                        rst = true;
                    }
                    catch (Exception ex)
                    {
                        msgErr = msgErr + " \\n" + "[UpdateStatusPostAccountCrossCmp_Error]" + "\\n" + ex.Message;
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

        #endregion

        public object[] GetObjectPRNo(string PostRet_KEY)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetObjectPR]", sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add("@PostRet_KEY", SqlDbType.VarChar, 250).Value = PostRet_KEY;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }


                return new object[]
                {
                    "",
                    ds.Tables[0]
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString(),
                    null
                };
            }
        }

        public object[] GetAccountingRecorded()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetIOAccountingRecorded]", sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }
                return new object[]
                {
                    ds.Tables[0]
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString()
                };
            }
        }
        public object[] GetAccountingRecordedWBS()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetWBSAccountingRecorded]", sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }
                return new object[]
                {
                    ds.Tables[0]
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString()
                };
            }
        }
        public object[] GetAccountingRecordedIO()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetIOAccountingRecorded]", sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }
                return new object[]
                {
                    ds.Tables[0]
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString()
                };
            }
        }
        public object[] GetDataForReverseDoc(string docid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    using (SqlCommand sqlComm = new SqlCommand("[dbo].[spGetDataForReverseDocWBS]", conn))
                    {
                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.Parameters.Add("@SAPID", SqlDbType.VarChar, 150).Value = docid;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlComm;
                        da.Fill(ds);
                    }
                }
                return new object[]
                {
                    ds.Tables[0]
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString()
                };
            }
        }
        public object[] GetDataForReverseDocIO(string docid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    using (SqlCommand sqlComm = new SqlCommand("[dbo].[spGetDataForReverseDocIO]", conn))
                    {
                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.Parameters.Add("@SAPID", SqlDbType.VarChar, 150).Value = docid;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlComm;
                        da.Fill(ds);
                    }
                }
                return new object[]
                {
                    ds.Tables[0]
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString()
                };
            }
        }
        public object[] GetDataForReverseDocWBS(string docid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    using (SqlCommand sqlComm = new SqlCommand("[dbo].[spGetDataForReverseDocWBS]", conn))
                    {
                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.Parameters.Add("@SAPID", SqlDbType.VarChar, 150).Value = docid;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlComm;
                        da.Fill(ds);
                    }
                }
                return new object[]
                {
                    ds.Tables[0]
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString()
                };
            }
        }
        public string InsertReverseAccount(string sapid, string remark, string postingDate, string reasonId, string reasonName, string revSapDocNo, string empid)
        {
            try
            {
                using (SqlConnection SqlCon = new SqlConnection(connStr))
                {
                    if (SqlCon.State == ConnectionState.Closed) SqlCon.Open();
                    using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spInsertReverseAccountWBS]", SqlCon))
                    {
                        SqlCmd.CommandType = CommandType.StoredProcedure;
                        SqlCmd.Parameters.Add("@sapid", SqlDbType.VarChar, 150).Value = sapid;
                        SqlCmd.Parameters.Add("@remark", SqlDbType.VarChar, 250).Value = remark;
                        SqlCmd.Parameters.Add("@postingDate", SqlDbType.VarChar, 250).Value = postingDate;
                        SqlCmd.Parameters.Add("@reasonId", SqlDbType.VarChar, 250).Value = reasonId;
                        SqlCmd.Parameters.Add("@reasonName", SqlDbType.VarChar, 250).Value = reasonName;
                        SqlCmd.Parameters.Add("@revSapDocNo", SqlDbType.VarChar, 250).Value = revSapDocNo;
                        SqlCmd.Parameters.Add("@empid", SqlDbType.VarChar, 25).Value = empid;
                        SqlCmd.ExecuteNonQuery();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public string InsertReverseAccountCrossCenter(string sapid, string remark, string postingDate, string reasonId, string reasonName,
                                                      string revSapDocNo_1, string revSapDocNo_2, string revSapDocNo_3, string empid)
        {
            try
            {
                using (SqlConnection SqlCon = new SqlConnection(connStr))
                {
                    if (SqlCon.State == ConnectionState.Closed) SqlCon.Open();
                    using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spInsertReverseAccountWBSCrossCostCenter]", SqlCon))
                    {
                        SqlCmd.CommandType = CommandType.StoredProcedure;
                        SqlCmd.Parameters.Add("@sapid", SqlDbType.VarChar, 150).Value = sapid;
                        SqlCmd.Parameters.Add("@remark", SqlDbType.VarChar, 250).Value = remark;
                        SqlCmd.Parameters.Add("@postingDate", SqlDbType.VarChar, 250).Value = postingDate;
                        SqlCmd.Parameters.Add("@reasonId", SqlDbType.VarChar, 250).Value = reasonId;
                        SqlCmd.Parameters.Add("@reasonName", SqlDbType.VarChar, 250).Value = reasonName;
                        SqlCmd.Parameters.Add("@revSapDocNo_1", SqlDbType.VarChar, 250).Value = revSapDocNo_1;
                        SqlCmd.Parameters.Add("@revSapDocNo_2", SqlDbType.VarChar, 250).Value = revSapDocNo_2;
                        SqlCmd.Parameters.Add("@revSapDocNo_3", SqlDbType.VarChar, 250).Value = revSapDocNo_3;
                        SqlCmd.Parameters.Add("@empid", SqlDbType.VarChar, 25).Value = empid;
                        SqlCmd.ExecuteNonQuery();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public string InsertReverseAccountWBS(string sapid, string remark, string postingDate, string reasonId, string reasonName, string revSapDocNo, string empid)
        {
            try
            {
                using (SqlConnection SqlCon = new SqlConnection(connStr))
                {
                    if (SqlCon.State == ConnectionState.Closed) SqlCon.Open();
                    using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spInsertReverseAccountWBS]", SqlCon))
                    {
                        SqlCmd.CommandType = CommandType.StoredProcedure;
                        SqlCmd.Parameters.Add("@sapid", SqlDbType.VarChar, 150).Value = sapid;
                        SqlCmd.Parameters.Add("@remark", SqlDbType.VarChar, 250).Value = remark;
                        SqlCmd.Parameters.Add("@postingDate", SqlDbType.VarChar, 250).Value = postingDate;
                        SqlCmd.Parameters.Add("@reasonId", SqlDbType.VarChar, 250).Value = reasonId;
                        SqlCmd.Parameters.Add("@reasonName", SqlDbType.VarChar, 250).Value = reasonName;
                        SqlCmd.Parameters.Add("@revSapDocNo", SqlDbType.VarChar, 250).Value = revSapDocNo;
                        SqlCmd.Parameters.Add("@empid", SqlDbType.VarChar, 25).Value = empid;
                        SqlCmd.ExecuteNonQuery();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public string InsertReverseAccountWBSCrossCenter(string sapid, string remark, string postingDate, string reasonId, string reasonName,
                                                      string revSapDocNo_1, string revSapDocNo_2, string revSapDocNo_3, string empid)
        {
            try
            {
                using (SqlConnection SqlCon = new SqlConnection(connStr))
                {
                    if (SqlCon.State == ConnectionState.Closed) SqlCon.Open();
                    using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spInsertReverseAccountWBSCrossCostCenter]", SqlCon))
                    {
                        SqlCmd.CommandType = CommandType.StoredProcedure;
                        SqlCmd.Parameters.Add("@sapid", SqlDbType.VarChar, 150).Value = sapid;
                        SqlCmd.Parameters.Add("@remark", SqlDbType.VarChar, 250).Value = remark;
                        SqlCmd.Parameters.Add("@postingDate", SqlDbType.VarChar, 250).Value = postingDate;
                        SqlCmd.Parameters.Add("@reasonId", SqlDbType.VarChar, 250).Value = reasonId;
                        SqlCmd.Parameters.Add("@reasonName", SqlDbType.VarChar, 250).Value = reasonName;
                        SqlCmd.Parameters.Add("@revSapDocNo_1", SqlDbType.VarChar, 250).Value = revSapDocNo_1;
                        SqlCmd.Parameters.Add("@revSapDocNo_2", SqlDbType.VarChar, 250).Value = revSapDocNo_2;
                        SqlCmd.Parameters.Add("@revSapDocNo_3", SqlDbType.VarChar, 250).Value = revSapDocNo_3;
                        SqlCmd.Parameters.Add("@empid", SqlDbType.VarChar, 25).Value = empid;
                        SqlCmd.ExecuteNonQuery();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public string InsertReverseAccountIO(string sapid, string remark, string postingDate, string reasonId, string reasonName, string revSapDocNo, string empid)
        {
            try
            {
                using (SqlConnection SqlCon = new SqlConnection(connStr))
                {
                    if (SqlCon.State == ConnectionState.Closed) SqlCon.Open();
                    using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spInsertReverseAccountIO]", SqlCon))
                    {
                        SqlCmd.CommandType = CommandType.StoredProcedure;
                        SqlCmd.Parameters.Add("@sapid", SqlDbType.VarChar, 150).Value = sapid;
                        SqlCmd.Parameters.Add("@remark", SqlDbType.VarChar, 250).Value = remark;
                        SqlCmd.Parameters.Add("@postingDate", SqlDbType.VarChar, 250).Value = postingDate;
                        SqlCmd.Parameters.Add("@reasonId", SqlDbType.VarChar, 250).Value = reasonId;
                        SqlCmd.Parameters.Add("@reasonName", SqlDbType.VarChar, 250).Value = reasonName;
                        SqlCmd.Parameters.Add("@revSapDocNo", SqlDbType.VarChar, 250).Value = revSapDocNo;
                        SqlCmd.Parameters.Add("@empid", SqlDbType.VarChar, 25).Value = empid;
                        SqlCmd.ExecuteNonQuery();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public string InsertReverseAccountIOCrossCenter(string sapid, string remark, string postingDate, string reasonId, string reasonName,
                                                      string revSapDocNo_1, string revSapDocNo_2, string revSapDocNo_3, string empid)
        {
            try
            {
                using (SqlConnection SqlCon = new SqlConnection(connStr))
                {
                    if (SqlCon.State == ConnectionState.Closed) SqlCon.Open();
                    using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spInsertReverseAccountIOCrossCostCenter]", SqlCon))
                    {
                        SqlCmd.CommandType = CommandType.StoredProcedure;
                        SqlCmd.Parameters.Add("@sapid", SqlDbType.VarChar, 150).Value = sapid;
                        SqlCmd.Parameters.Add("@remark", SqlDbType.VarChar, 250).Value = remark;
                        SqlCmd.Parameters.Add("@postingDate", SqlDbType.VarChar, 250).Value = postingDate;
                        SqlCmd.Parameters.Add("@reasonId", SqlDbType.VarChar, 250).Value = reasonId;
                        SqlCmd.Parameters.Add("@reasonName", SqlDbType.VarChar, 250).Value = reasonName;
                        SqlCmd.Parameters.Add("@revSapDocNo_1", SqlDbType.VarChar, 250).Value = revSapDocNo_1;
                        SqlCmd.Parameters.Add("@revSapDocNo_2", SqlDbType.VarChar, 250).Value = revSapDocNo_2;
                        SqlCmd.Parameters.Add("@revSapDocNo_3", SqlDbType.VarChar, 250).Value = revSapDocNo_3;
                        SqlCmd.Parameters.Add("@empid", SqlDbType.VarChar, 25).Value = empid;
                        SqlCmd.ExecuteNonQuery();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public void CreateDirectoryTextFile(string FilePath)
        {
            try
            {
                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }
            }
            catch (Exception)
            {
                // handle them here
            }

        }

        public void WriteLogFile(string sText)
        {
            try
            {
                CreateDirectoryTextFile(HostingEnvironment.MapPath("~/temp/log"));
                using (StreamWriter stream = File.AppendText(HostingEnvironment.MapPath("~/temp/log/StockPromotionLog.txt")))
                {
                    stream.WriteLine(sText);
                }
            }
            catch (Exception)
            {
                // handle them here
            }
        }

        public string UpdatePRItemFromSap(string DelvLst, string PRNo, string Material, string PRItem)
        {
            try
            {
                ////return "xx";
                using (SqlConnection SqlCon = new SqlConnection(connStr))
                {
                    if (SqlCon.State == ConnectionState.Closed) SqlCon.Open();
                    using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spUpdatePRItemFromSap]", SqlCon))
                    {
                        SqlCmd.CommandType = CommandType.StoredProcedure;
                        SqlCmd.Parameters.AddWithValue("@DelvLst", DelvLst);
                        SqlCmd.Parameters.AddWithValue("@PRNo", PRNo);
                        SqlCmd.Parameters.AddWithValue("@Material", Material);
                        SqlCmd.Parameters.AddWithValue("@PRItem", PRItem);
                        SqlCmd.ExecuteNonQuery();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return "UpdatePRItemFromSap " + ex.Message.ToString();
            }
        }



    }
}