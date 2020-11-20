using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AP_StockPromotion_V1.Class
{
    public class DARequisition
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

        public DataTable getDataRequest(Int64 ReqHeaderId, int? setFn)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataRequest", conn);
                    sqlComm.Parameters.AddWithValue("@ReqHeaderId", ReqHeaderId);
                    sqlComm.Parameters.AddWithValue("@Function", setFn);


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

        public DataTable getDataRequest(Entities.RequisitionInfo req, int? setFn)
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
                    sqlComm.Parameters.AddWithValue("@withCRMData", req.WithCRMData);
                    sqlComm.Parameters.AddWithValue("@Function", setFn);
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

        public DataTable getDataRequestGroupByCompany(Entities.RequisitionInfo req)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataRequestGroupByCompany", conn);
                    if (req.ReqHeaderId == 0)
                    {
                        sqlComm.Parameters.AddWithValue("@ReqHeaderId", DBNull.Value);
                    }
                    else
                    {
                        sqlComm.Parameters.AddWithValue("@ReqHeaderId", req.ReqHeaderId);
                    }
                    sqlComm.Parameters.AddWithValue("@ReqNo", req.ReqNo);
                    sqlComm.Parameters.AddWithValue("@ReqDateFrom", req._ReqDateFrom);
                    sqlComm.Parameters.AddWithValue("@ReqDateTo", req._ReqDateTo);
                    sqlComm.Parameters.AddWithValue("@Project_Id", req.Project_Id);
                    sqlComm.Parameters.AddWithValue("@ItemId", req.ItemId);
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

        public bool createRequest(Entities.RequisitionInfo req, List<Entities.RequisitionInfo> reqdetail, ref string reqDocNo)
        {
            bool rst = true;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlTransaction tr = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand sqlComm = new SqlCommand("dbo.spInsertDataRequestHeader", conn, tr);
                        sqlComm.Parameters.AddWithValue("@ReqNo", req.ReqNo);
                        sqlComm.Parameters.AddWithValue("@ReqType", req.ReqType);
                        sqlComm.Parameters.AddWithValue("@ReqDocDate", req._ReqDocDate);
                        sqlComm.Parameters.AddWithValue("@ReqDate", req._ReqDate);
                        sqlComm.Parameters.AddWithValue("@ReqBy", req.ReqBy);
                        sqlComm.Parameters.AddWithValue("@ReqHeaderRemark", req.ReqHeaderRemark);
                        sqlComm.Parameters.AddWithValue("@UpdateBy", req.UpdateBy);

                        sqlComm.Parameters.Add("@ReqHeaderId", SqlDbType.Int);
                        sqlComm.Parameters["@ReqHeaderId"].Direction = ParameterDirection.Output;
                        sqlComm.Parameters.Add("@ReqDocNo", SqlDbType.VarChar, 10);
                        sqlComm.Parameters["@ReqDocNo"].Direction = ParameterDirection.Output;

                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.ExecuteNonQuery();

                        Int32 reqHID = (Int32)sqlComm.Parameters["@ReqHeaderId"].Value;
                        reqDocNo = sqlComm.Parameters["@ReqDocNo"].Value + "";

                        foreach (Entities.RequisitionInfo r in reqdetail)
                        {
                            sqlComm = new SqlCommand("dbo.spInsertDataRequestDetail", conn, tr);
                            sqlComm.Parameters.AddWithValue("@ReqHeaderID", reqHID);
                            sqlComm.Parameters.AddWithValue("@ReqNo", r.ReqNo);
                            // sqlComm.Parameters.AddWithValue("@DocNo", r.DocNo);
                            sqlComm.Parameters.AddWithValue("@Project_Id", r.Project_Id);
                            sqlComm.Parameters.AddWithValue("@ProjectID", r.ProjectID);
                            sqlComm.Parameters.AddWithValue("@ItemId", r.ItemId);
                            sqlComm.Parameters.AddWithValue("@ItemNo", r.ItemNo);
                            sqlComm.Parameters.AddWithValue("@ReqAmount", r.ReqAmount);
                            // sqlComm.Parameters.AddWithValue("@UnitNo", r.UnitNo);
                            sqlComm.Parameters.AddWithValue("@ProStartDate", r._ProStartDate);
                            sqlComm.Parameters.AddWithValue("@ProEndDate", r._ProEndDate);
                            sqlComm.Parameters.AddWithValue("@ProAlertDate", r.ProAlertDate);
                            // sqlComm.Parameters.AddWithValue("@ReqStatus", r.ReqStatus);
                            // sqlComm.Parameters.AddWithValue("@UpdateBy", r.UpdateBy);
                            // sqlComm.Parameters.AddWithValue("@UpdateDate", req.UpdateDate);
                            sqlComm.CommandType = CommandType.StoredProcedure;
                            sqlComm.ExecuteNonQuery();
                        }
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

        public bool editRequest(Entities.RequisitionInfo req, List<Entities.RequisitionInfo> reqdetail)
        {
            bool rst = true;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlTransaction tr = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand sqlComm = new SqlCommand("dbo.spUpdateDataRequestHeader", conn, tr);
                        sqlComm.Parameters.AddWithValue("@ReqHeaderId", req.ReqHeaderId);
                        sqlComm.Parameters.AddWithValue("@ReqNo", req.ReqNo);
                        sqlComm.Parameters.AddWithValue("@ReqType", req.ReqType);
                        sqlComm.Parameters.AddWithValue("@ReqDate", req._ReqDate);
                        sqlComm.Parameters.AddWithValue("@ReqBy", req.ReqBy);
                        // sqlComm.Parameters.AddWithValue("@ReqHeaderStatus", req.ReqHeaderStatus);
                        sqlComm.Parameters.AddWithValue("@ReqHeaderRemark", req.ReqHeaderRemark);
                        // sqlComm.Parameters.AddWithValue("@UpdateBy", req.UpdateBy);

                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.ExecuteNonQuery();

                        foreach (Entities.RequisitionInfo r in reqdetail)
                        {
                            sqlComm = new SqlCommand("dbo.spUpdateDataRequestDetail", conn, tr);
                            sqlComm.Parameters.AddWithValue("@ReqId", r.ReqId);
                            sqlComm.Parameters.AddWithValue("@ReqHeaderID", r.ReqHeaderId);
                            sqlComm.Parameters.AddWithValue("@ReqNo", r.ReqNo);
                            // sqlComm.Parameters.AddWithValue("@DocNo", r.DocNo);
                            sqlComm.Parameters.AddWithValue("@Project_Id", r.Project_Id);
                            sqlComm.Parameters.AddWithValue("@ProjectID", r.ProjectID);
                            sqlComm.Parameters.AddWithValue("@ItemId", r.ItemId);
                            sqlComm.Parameters.AddWithValue("@ItemNo", r.ItemNo);
                            sqlComm.Parameters.AddWithValue("@ReqAmount", r.ReqAmount);
                            // sqlComm.Parameters.AddWithValue("@UnitNo", r.UnitNo);
                            sqlComm.Parameters.AddWithValue("@ProStartDate", r._ProStartDate);
                            sqlComm.Parameters.AddWithValue("@ProEndDate", r._ProEndDate);
                            sqlComm.Parameters.AddWithValue("@ProAlertDate", r.ProAlertDate);
                            sqlComm.Parameters.AddWithValue("@ReqStatus", r.ReqStatus);
                            // sqlComm.Parameters.AddWithValue("@UpdateBy", r.UpdateBy);
                            // sqlComm.Parameters.AddWithValue("@UpdateDate", req.UpdateDate);
                            sqlComm.CommandType = CommandType.StoredProcedure;
                            sqlComm.ExecuteNonQuery();
                        }
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

        public bool deleteRequest(Int64 reqId, string delRemark, string userId)
        {
            bool rst = true;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spDeleteDataRequestHeader", conn);
                    sqlComm.Parameters.AddWithValue("@ReqHeaderId", reqId);
                    sqlComm.Parameters.AddWithValue("@ReqDelRemark", delRemark);
                    sqlComm.Parameters.AddWithValue("@UserId", userId);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    rst = true;
                }
                catch (Exception)
                {
                    rst = false;
                }
                finally
                {
                    conn.Close();
                }
            }
            return rst;
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

        public bool InsertSAP_Approver_PR(DataTable dt, ref string msg)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlTransaction trans = conn.BeginTransaction("SampleTransaction");

                try
                {
                    SqlCommand sqlComm = new SqlCommand();
                    sqlComm.Connection = conn;
                    sqlComm.Transaction = trans;

                    sqlComm.CommandText = "Delete From SAP_Approver_PR";
                    sqlComm.ExecuteNonQuery();

                    var sqlText = "INSERT INTO [dbo].[SAP_Approver_PR]"
                        + "(  [ROUTE], [BSART], [KNTTP], [WERKS], [GFWRT]"
                        + " , [KDATB], [APPR_EMAIL1], [APPR_NAME1], [APPR_POSITION1], [KOSTL]"
                        + " , [ImportDate], [IsUpdated])"
                        + " VALUES"
                        + "(  @ROUTE, @BSART, @KNTTP, @WERKS, @GFWRT"
                        + " , @KDATB, @APPR_EMAIL1, @APPR_NAME1, @APPR_POSITION1, @KOSTL"
                        + " , GETDATE(), 0)";
                    sqlComm.CommandText = sqlText;
                    sqlComm.Parameters.Add("@ROUTE", SqlDbType.VarChar, 20);
                    sqlComm.Parameters.Add("@BSART", SqlDbType.VarChar, 4);
                    sqlComm.Parameters.Add("@KNTTP", SqlDbType.VarChar, 1);
                    sqlComm.Parameters.Add("@WERKS", SqlDbType.VarChar, 4);
                    sqlComm.Parameters.Add("@GFWRT", SqlDbType.Decimal, 18);
                    sqlComm.Parameters["@GFWRT"].Precision = 18;
                    sqlComm.Parameters["@GFWRT"].Scale = 2;
                    sqlComm.Parameters.Add("@KDATB", SqlDbType.DateTime);
                    sqlComm.Parameters.Add("@APPR_EMAIL1", SqlDbType.VarChar, 250);
                    sqlComm.Parameters.Add("@APPR_NAME1", SqlDbType.VarChar, 150);
                    sqlComm.Parameters.Add("@APPR_POSITION1", SqlDbType.VarChar, 150);
                    sqlComm.Parameters.Add("@KOSTL", SqlDbType.VarChar, 10);

                    foreach (DataRow dr in dt.Rows)
                    {
                        sqlComm.Parameters["@ROUTE"].Value = dr["ROUTE"];
                        sqlComm.Parameters["@BSART"].Value = dr["BSART"];
                        sqlComm.Parameters["@KNTTP"].Value = dr["KNTTP"];
                        sqlComm.Parameters["@WERKS"].Value = dr["WERKS"];
                        sqlComm.Parameters["@GFWRT"].Value = dr["GFWRT"];
                        sqlComm.Parameters["@KDATB"].Value = dr["KDATB"];
                        sqlComm.Parameters["@APPR_EMAIL1"].Value = dr["APPR_EMAIL1"];
                        sqlComm.Parameters["@APPR_NAME1"].Value = dr["APPR_NAME1"];
                        sqlComm.Parameters["@APPR_POSITION1"].Value = dr["APPR_POSITION1"];
                        sqlComm.Parameters["@KOSTL"].Value = dr["KOSTL"];
                        sqlComm.ExecuteNonQuery();
                    }

                    sqlComm.CommandText = "EXEC spInsertPRExpenseFromSAP";
                    sqlComm.ExecuteNonQuery();

                    trans.Commit();
                    msg = "";
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    msg = ex.ToString();
                    return false;
                }
            }
        }

        public DataTable GetPRExpenseMemoData(string CostCenter, int EXPID, string Approver)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    SqlCommand sqlComm = new SqlCommand("dbo.spGetPRExpenseMemoData", conn);
                    sqlComm.Parameters.AddWithValue("@CostCenter", CostCenter);
                    sqlComm.Parameters.AddWithValue("@EXPID", EXPID);
                    sqlComm.Parameters.AddWithValue("@Approver", Approver);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);
                }
                return ds.Tables[0] ?? new DataTable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetAllMasterExpense()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetAllMasterExpense", conn);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);
                }

                return ds.Tables[0] ?? new DataTable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetInternalOrder(string InternalOrder, string Description, bool? IsActive)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    SqlCommand sqlComm = new SqlCommand("dbo.spGetInternalOrder", conn);
                    sqlComm.Parameters.AddWithValue("@InternalOrder", InternalOrder);
                    sqlComm.Parameters.AddWithValue("@Description", Description);
                    if (IsActive == null){
                        sqlComm.Parameters.AddWithValue("@IsActive", DBNull.Value);
                    }
                    else{
                        sqlComm.Parameters.AddWithValue("@IsActive", IsActive);
                    }

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);
                }

                return ds.Tables[0] ?? new DataTable();
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }

        public bool DeleteInternalOrderByID(int ID, ref string _msg)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string sql = "DELETE FROM dbo.InternalOrder WHERE ID = @ID";

                    SqlCommand sqlComm = new SqlCommand(sql, conn);
                    sqlComm.Parameters.AddWithValue("@ID", ID);

                    sqlComm.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                _msg = ex.ToString();
                return false;
            }
        }

        public bool InsertInternalOrderByID(string InternalOrder, string Description, int IsActive, ref string _msg)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string sql = "INSERT dbo.InternalOrder (InternalOrder, Description, IsActive) VALUES (@InternalOrder, @Description, @IsActive)";

                    SqlCommand sqlComm = new SqlCommand(sql, conn);
                    sqlComm.Parameters.AddWithValue("@InternalOrder", InternalOrder);
                    sqlComm.Parameters.AddWithValue("@Description", Description);
                    sqlComm.Parameters.AddWithValue("@IsActive", IsActive);

                    sqlComm.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                _msg = ex.Message;
                return false;
            }
        }

        public DataTable GetCostCenterByMemoDoc(string MemoDoc)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(
                        "SELECT c.CostCenter, c.CostCenterName FROM dbo.MemoRequest m INNER JOIN dbo.vw_CostCenter c ON c.CostCenter = m.CCCODE WHERE DOCNO = @MemoDoc"
                        , sqlConn))
                    {
                        sqlCmd.Parameters.Add(new SqlParameter("@MemoDoc", MemoDoc));

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }

                ds = ds ?? new DataSet();

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                {
                    return new DataTable();
                }

            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }

    }
}