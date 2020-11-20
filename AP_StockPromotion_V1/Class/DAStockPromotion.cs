using AP_StockPromotion_V1.webpage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Transactions;
using System.Linq;
using static AP_StockPromotion_V1.webpage.CostCenterACRecord;
using Entities;

namespace AP_StockPromotion_V1.Class
{
    public class DAStockPromotion
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
                return new DataTable();
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

        public DataTable getDataUserByEmpCode(string empCode)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetUserStock", conn);
                    sqlComm.Parameters.AddWithValue("@EmpCode", empCode);
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

        public DataTable getUserAuth(string GUID, string UserNameLogin, string Email)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetUserAuth", conn);
                    sqlComm.Parameters.AddWithValue("@GUID", GUID);
                    sqlComm.Parameters.AddWithValue("@UserNameLogin", UserNameLogin);
                    sqlComm.Parameters.AddWithValue("@Email", Email);
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

        public DataTable getMenuList(string RoleCode)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetMenuListByRoleCode", conn);
                    sqlComm.Parameters.AddWithValue("@RoleCode", RoleCode);
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

        public bool CheckRefNoStatus(ref List<string> lstRef, ref string Message)
        {
            try
            {
                var valid = true;
                DataSet ds = new DataSet();
                List<string> invalidRef = new List<string>();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    foreach (var i in lstRef)
                    {
                        using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spCheckApprovedMemo]", conn))
                        {
                            if (conn.State != ConnectionState.Open)
                            {
                                conn.Open();
                            }
                            sqlCmd.Parameters.Add("@DOCNO", SqlDbType.NVarChar, 20).Value = i.Trim();
                            sqlCmd.Parameters.Add("@MESSAGE", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;
                            sqlCmd.Parameters.Add("@VALID", SqlDbType.Bit).Direction = ParameterDirection.Output;
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            sqlCmd.ExecuteNonQuery();
                            valid = Convert.ToBoolean(sqlCmd.Parameters["@VALID"].Value);
                            Message = Convert.ToString(sqlCmd.Parameters["@MESSAGE"].Value);
                        }
                        if (!valid)
                        {
                            invalidRef.Add(i);
                        }
                    }
                }
                if (invalidRef.Any())
                {
                    valid = false;
                    lstRef = invalidRef;
                }
                else
                {
                    lstRef = new List<string>();
                }
                return valid;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public DataTable getMenuList(int RoleId)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetMenuList", conn);
                    sqlComm.Parameters.AddWithValue("@RoleId", RoleId);
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

        public DataTable getDataUser(string firstName, string lastName)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetUserStock", conn);
                    sqlComm.Parameters.AddWithValue("@FirstName", firstName);
                    sqlComm.Parameters.AddWithValue("@LastName", lastName);
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

        public DataTable getDataStatus(string StatusHeaderId)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetStatusDetailList", conn);
                    sqlComm.Parameters.AddWithValue("@StatusHeaderId", StatusHeaderId);
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

        public DataTable getDataMasterItem(MasterItemInfo item)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetMasterItem", conn);
                    sqlComm.Parameters.AddWithValue("@MasterItemId", item.MasterItemId);
                    sqlComm.Parameters.AddWithValue("@MasterItemGroupId", item.MasterItemGroupId);
                    sqlComm.Parameters.AddWithValue("@ItemNo", item.ItemNo);
                    sqlComm.Parameters.AddWithValue("@ItemName", item.ItemName);
                    sqlComm.Parameters.AddWithValue("@ItemCostBegin", item.ItemCostBeg);
                    sqlComm.Parameters.AddWithValue("@ItemCostEnd", item.ItemCostEnd);
                    sqlComm.Parameters.AddWithValue("@ItemCountMethod", item.ItemCountMethod);
                    sqlComm.Parameters.AddWithValue("@ItemStock", item.ItemStock);
                    sqlComm.Parameters.AddWithValue("@ItemStatus", item.ItemStatus);

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

        public DataTable GetDataMasterItemByID(int MasterItemID, string selectTop, string serial, string ItemID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetDataMasterItemByID]", sqlConn))
                    {
                        sqlCmd.Parameters.Add("@MIID", SqlDbType.VarChar).Value = MasterItemID;
                        sqlCmd.Parameters.Add("@TOP", SqlDbType.VarChar).Value = selectTop;
                        sqlCmd.Parameters.Add("@Serial", SqlDbType.VarChar).Value = serial;
                        sqlCmd.Parameters.Add("@ItemID", SqlDbType.VarChar).Value = ItemID;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
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

        public bool InsertDataMasterItem(MasterItemInfo item)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("dbo.spInsertMasterItem", conn);
                    sqlComm.Parameters.AddWithValue("@MasterItemGroupId", item.MasterItemGroupId);
                    sqlComm.Parameters.AddWithValue("@ItemNo", item.ItemNo);
                    sqlComm.Parameters.AddWithValue("@ItemName", item.ItemName);
                    sqlComm.Parameters.AddWithValue("@ItemBasePricePerUnit", item.ItemCost);
                    sqlComm.Parameters.AddWithValue("@ItemPricePerUnit", item.ItemCostIncVat);
                    sqlComm.Parameters.AddWithValue("@ItemCountMethod", item.ItemCountMethod);
                    sqlComm.Parameters.AddWithValue("@ItemStock", item.ItemStock);
                    sqlComm.Parameters.AddWithValue("@ItemStatus", item.ItemStatus);
                    sqlComm.Parameters.AddWithValue("@UpdateBy", item.UpdateBy);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }

            catch (Exception)

            {
                return false;
            }
        }

        public bool CheckStatusBeforeApprove(string docno)
        {
            try
            {
                bool isCheck = false;
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spCheckStatusBeforeApprove]", sqlConn))
                    {
                        sqlCmd.Parameters.Add("@DOCNO", SqlDbType.VarChar, 9).Value = docno;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    isCheck = true;
                }

                return isCheck; //ds.Tables[0].Rows[0].ItemArray[0];
            }

            catch (Exception)

            {
                return false;
            }
        }

        public bool DeleteMemoRequest(string docno)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spDeleteMemoRequest]", sqlConn))
                    {
                        if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();

                        sqlCmd.Parameters.Add("@DOCNO", SqlDbType.VarChar, 9).Value = docno;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.ExecuteNonQuery();
                    }
                }
                return true;
            }

            catch (Exception)

            {
                return false;
            }
        }

        public bool spUpdateApproveMemoRequest(string docno)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spUpdateApproveMemoRequest]", sqlConn))
                    {
                        if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();

                        sqlCmd.Parameters.Add("@DOCNO", SqlDbType.VarChar, 9).Value = docno;

                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool spAutoGenerateRequisition(string memoDocNo, ref bool IsMkt)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spAutoGenerateRequisition]", sqlConn))
                    {
                        if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();

                        sqlCmd.Parameters.Add("@MEMODOCNO", SqlDbType.VarChar, 25).Value = memoDocNo;
                        sqlCmd.Parameters.Add("@ISMKT", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.ExecuteNonQuery();
                        IsMkt = Convert.ToBoolean(sqlCmd.Parameters["@ISMKT"].Value);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckStatusBeforeReject(string docno)
        {
            try
            {
                bool isCheck = false;
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spCheckStatusBeforeReject]", sqlConn))
                    {
                        sqlCmd.Parameters.Add("@DOCNO", SqlDbType.VarChar, 9).Value = docno;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    isCheck = true;
                }

                return isCheck; //ds.Tables[0].Rows[0].ItemArray[0];
            }

            catch (Exception)

            {
                return false;
            }
        }

        public bool UpdateRejectMemoRequest(string docno)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spUpdateRejectMemoRequest]", sqlConn))
                    {
                        if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();

                        sqlCmd.Parameters.Add("@DOCNO", SqlDbType.VarChar, 9).Value = docno;

                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.ExecuteNonQuery();
                    }
                }
                return true;
            }

            catch (Exception)

            {
                return false;
            }
        }

        public bool UpdateRejectReason(string docno, string reason)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spUpdateRejectReason]", sqlConn))
                    {
                        if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();

                        sqlCmd.Parameters.Add("@DOCNO", SqlDbType.VarChar, 9).Value = docno;
                        sqlCmd.Parameters.Add("@REASON", SqlDbType.VarChar, 250).Value = reason;

                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.ExecuteNonQuery();
                    }
                }
                return true;
            }

            catch (Exception)

            {
                return false;
            }
        }

        public void UpdateSendForApproveMemoRequest(string docno, string Approver)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    using (SqlCommand sqlComm = new SqlCommand("[dbo].[spUpdateSendForApproveMemoRequest]", conn))
                    {
                        sqlComm.Parameters.Add("@DOCNO", SqlDbType.VarChar, 100).Value = docno;
                        sqlComm.Parameters.Add("@Approver", SqlDbType.VarChar, 100).Value = Approver;

                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.ExecuteNonQuery();
                    }
                }
            }

            catch (Exception)

            {
            }
        }

        public bool UpdateDataMasterItem(MasterItemInfo item)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("dbo.spUpdateMasterItem", conn);
                    sqlComm.Parameters.AddWithValue("@MasterItemId", item.MasterItemId);
                    sqlComm.Parameters.AddWithValue("@MasterItemGroupId", item.MasterItemGroupId);
                    sqlComm.Parameters.AddWithValue("@ItemNo", item.ItemNo);
                    sqlComm.Parameters.AddWithValue("@ItemName", item.ItemName);
                    sqlComm.Parameters.AddWithValue("@ItemBasePricePerUnit", item.ItemCost);
                    sqlComm.Parameters.AddWithValue("@ItemPricePerUnit", item.ItemCostIncVat);
                    sqlComm.Parameters.AddWithValue("@ItemCountMethod", item.ItemCountMethod);
                    sqlComm.Parameters.AddWithValue("@ItemStock", item.ItemStock);
                    sqlComm.Parameters.AddWithValue("@ItemForceExpire", item.ItemForceExpire);
                    sqlComm.Parameters.AddWithValue("@ItemStatus", item.ItemStatus);
                    sqlComm.Parameters.AddWithValue("@UpdateBy", item.UpdateBy);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }

            catch (Exception)

            {
                return false;
            }
        }

        public DataTable getDataMasterProject(bool includeNoneProject = false)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetMasterProject", conn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);
                }
                if (includeNoneProject)
                {
                    NoneProject(ref ds);
                }
                return ds.Tables[0];
            }

            catch (Exception)

            {
                return null;
            }
        }

        public DataTable getDataMasterProjectAndCostCenter(bool includeNoneProject = false)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetMasterProjectAndCostCenter", conn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);
                }
                if (includeNoneProject)
                {
                    NoneProject(ref ds);
                }
                return ds.Tables[0];
            }

            catch (Exception)

            {
                return null;
            }
        }

        /* - None Project - */
        private void NoneProject(ref DataSet ds)
        {

            DataRow drNoProject = ds.Tables[0].NewRow();
            drNoProject["ProjectID"] = "99998";
            drNoProject["ProjectCode"] = "XXXXX";
            drNoProject["ProjectName"] = "ไม่ระบุโครงการ";
            ds.Tables[0].Rows.Add(drNoProject);
            ds.Tables[0].AcceptChanges();
        }

        public DataTable getDataMasterProjectCostCenter(bool includeNoneProjectCostCenter = false)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetMasterProjectCostCenter", conn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);
                }
                if (includeNoneProjectCostCenter)
                {
                    NoneProjectCostCenter(ref ds);
                }
                return ds.Tables[0];
            }

            catch (Exception)

            {
                return null;
            }
        }

        /* - None Project - */
        private void NoneProjectCostCenter(ref DataSet ds)
        {

            DataRow drNoProject = ds.Tables[0].NewRow();
            drNoProject["CostCenter"] = "99999";
            drNoProject["CostCenterName"] = "ไม่ระบุโครงการ/หน่วยงาน";
            ds.Tables[0].Rows.Add(drNoProject);
            ds.Tables[0].AcceptChanges();
        }

        public DataTable getDataMasterInternalOrder()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetMasterInternalOrder", conn);
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

        public DataTable getDataSAPCompany()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetSAPCompany", conn);
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

        public bool SyncMasterDataItem()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("dbo.spTransferDataMasterItemToStockPromotion", conn);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }

            catch (Exception)

            {
                return false;
            }
        }

        public DataTable getDataMasterCompany()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetMasterCompany", conn);
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

        public DataTable getDataMasterItemGroup(MasterItemGroupInfo itemGrp)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetMasterItemGroup", conn);
                    sqlComm.Parameters.AddWithValue("@MasterItemGroupId", itemGrp.MasterItemGroupId);
                    sqlComm.Parameters.AddWithValue("@ItemGroupName", itemGrp.ItemGroupName + "");

                    //sqlComm.Parameters.AddWithValue("@ItemCountMethod", itemGrp.ItemCountMethod + "");
                    //sqlComm.Parameters.AddWithValue("@ItemStock", itemGrp.ItemStock + "");
                    //sqlComm.Parameters.AddWithValue("@ItemForceExpire", itemGrp.ItemForceExpire);

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

        public bool InsertDataMasterItemGroup(MasterItemGroupInfo itemGrp)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("dbo.spInsertMasterItemGroup", conn);
                    sqlComm.Parameters.AddWithValue("@MasterItemGroupName", itemGrp.ItemGroupName);

                    //sqlComm.Parameters.AddWithValue("@ItemCountMethod", itemGrp.ItemCountMethod + "");
                    //sqlComm.Parameters.AddWithValue("@ItemStock", itemGrp.ItemStock + "");
                    //sqlComm.Parameters.AddWithValue("@ItemForceExpire", itemGrp.ItemForceExpire);

                    sqlComm.Parameters.AddWithValue("@UpdateBy", itemGrp.UpdateBy);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }

            catch (Exception)

            {
                return false;
            }
        }

        public bool UpdateDataMasterItemGroup(MasterItemGroupInfo itemGrp)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("dbo.spUpdateMasterItemGroup", conn);
                    sqlComm.Parameters.AddWithValue("@MasterItemGroupId", itemGrp.MasterItemGroupId);
                    sqlComm.Parameters.AddWithValue("@MasterItemGroupName", itemGrp.ItemGroupName);

                    //sqlComm.Parameters.AddWithValue("@ItemCountMethod", itemGrp.ItemCountMethod);
                    //sqlComm.Parameters.AddWithValue("@ItemStock", itemGrp.ItemStock);
                    //sqlComm.Parameters.AddWithValue("@ItemForceExpire", itemGrp.ItemForceExpire);

                    sqlComm.Parameters.AddWithValue("@UpdateBy", itemGrp.UpdateBy);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }

            catch (Exception)

            {
                return false;
            }
        }

        public object[] GetCbxCostCenter()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlCon = new SqlConnection(connStr))
                {
                    if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
                    string cmd = @"SELECT
	                                   CostCenter AS CC_ID
	                                   ,CostCenterName AS CC_NAME
                                   FROM vw_CostCenter";
                    using (SqlCommand sqlCmd = new SqlCommand(cmd, sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.Text;

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
                    new DataTable()
                };
            }
        }

        public object[] GetCbxItemPromotion()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlCon = new SqlConnection(connStr))
                {
                    if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
                    string cmd = @"select Substring(ItemNo, 11, 10) as ItemNo, ItemName from MasterItem order by ItemNo";
                    using (SqlCommand sqlCmd = new SqlCommand(cmd, sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.Text;

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
                    new DataTable()
                };
            }
        }

        /* --------------------- Phase II --------------------- */
        #region "############# Add by Puwarun.P 19-08-2016 #############"

        public string UpdateFinishMemoRequest(string docno)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    if (sqlConn.State == ConnectionState.Closed)
                    {
                        sqlConn.Open();
                    }

                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spUpdateFinishMemoRequest]", sqlConn))
                    {
                        sqlCmd.Parameters.Add("@DOCNO", SqlDbType.VarChar, 15).Value = docno;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.ExecuteNonQuery();
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public DataTable GetPRMemoData()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetPRMemoData]", sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                        da.Fill(ds);
                    }
                }

                return ds.Tables[0] ?? new DataTable();
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        public DataTable GetExpenseDesc()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetDataPRExpense]", sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
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

        public DataTable GetDataTierExpense()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetDataTierExpense]", conn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
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

        public string GetCompanySAPCode(string SelDelv)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("spGetCompanySAPCode", conn))
                    {
                        sqlCmd.Parameters.Add("@DelvLstId", SqlDbType.NVarChar, 1000).Value = SelDelv;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand = sqlCmd;
                        da.Fill(dt);
                    }
                }

                return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


        public string GetCompanySAPCodeMKT(string SelDelv)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetCompanySAPCodeMKT]", conn))
                    {
                        sqlCmd.Parameters.Add("@DelvLstId", SqlDbType.NVarChar, 250).Value = SelDelv;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }

                return ds.Tables[0].Rows[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public DataTable GetDataTierExpenseByCostCenter(string CostCenter)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetDataTierCostCenter]", conn))
                    {
                        sqlCmd.Parameters.Add("@CCID", SqlDbType.NVarChar, 100).Value = CostCenter ?? "";
                        sqlCmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable GetDistinctCostCenter()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetDistinctCostCenter]", conn))
                    {

                        sqlCmd.CommandType = CommandType.StoredProcedure;
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

        public int GetPRID()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetPRID]", sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }
                return Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0]);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool DeleteTypeMemo(int id, string CCID, ref string Message)
        {
            var valid = true;
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spDeleteDataPRExpense]", sqlConn))
                    {
                        sqlConn.Open();
                        sqlCmd.Parameters.Add("@PRID", SqlDbType.Int).Value = id;
                        sqlCmd.Parameters.Add("@CCID", SqlDbType.NVarChar, 100).Value = CCID ?? "";
                        sqlCmd.Parameters.Add("@MESSAGE", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;
                        sqlCmd.Parameters.Add("@VALID", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.ExecuteNonQuery();
                        valid = Convert.ToBoolean(sqlCmd.Parameters["@VALID"].Value);
                        Message = Convert.ToString(sqlCmd.Parameters["@MESSAGE"].Value);
                    }
                }
                return valid;
            }

            catch (Exception ex)

            {
                Message = ex.Message;
                return false;
            }
        }

        public bool DeleteTypeMemoMgr(int id, string CCID)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spDeleteDataManagementPRExpense]", sqlConn)) //[spDeleteDataPRExpense]
                    {
                        sqlConn.Open();
                        sqlCmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        sqlCmd.Parameters.Add("@CCID", SqlDbType.NVarChar, 250).Value = CCID;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.ExecuteNonQuery();
                    }
                }
                return true;
            }

            catch (Exception)

            {
                return false;
            }
        }

        public bool SaveTypeMemo(int Mode, int PRID, int ExpID, int ApvTypeID, int AuthTypeID, string CCID, ref string Message)
        {
            var valid = true;
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spInsertDataPRExpense]", sqlConn))
                    {
                        sqlConn.Open();
                        sqlCmd.Parameters.Add("@MODE", SqlDbType.Int).Value = Mode;
                        sqlCmd.Parameters.Add("@PRID", SqlDbType.Int).Value = PRID;
                        sqlCmd.Parameters.Add("@EXPID", SqlDbType.Int).Value = ExpID;
                        sqlCmd.Parameters.Add("@APVTYPEID", SqlDbType.Int).Value = ApvTypeID;
                        sqlCmd.Parameters.Add("@AUTHTYPEID", SqlDbType.Int).Value = AuthTypeID;
                        sqlCmd.Parameters.Add("@CCID", SqlDbType.NVarChar, 100).Value = (CCID == null ? "" : CCID);
                        sqlCmd.Parameters.Add("@MESSAGE", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;
                        sqlCmd.Parameters.Add("@VALID", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.ExecuteNonQuery();
                        valid = Convert.ToBoolean(sqlCmd.Parameters["@VALID"].Value);
                        Message = Convert.ToString(sqlCmd.Parameters["@MESSAGE"].Value);
                    }
                }

                return valid;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public bool SaveMasterPRExpense(int Mode, int EXPENSEID, int APVID, int SIGNID, string CCID, ref string Message)
        {
            var valid = true;
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spInsertMasterPRExpense]", sqlConn))
                    {
                        sqlConn.Open();
                        sqlCmd.Parameters.Add("@MODE", SqlDbType.Int).Value = Mode;
                        sqlCmd.Parameters.Add("@EXPENSEID", SqlDbType.Int).Value = EXPENSEID;
                        sqlCmd.Parameters.Add("@APVID", SqlDbType.Int).Value = APVID;
                        sqlCmd.Parameters.Add("@SIGNID", SqlDbType.Int).Value = SIGNID;
                        sqlCmd.Parameters.Add("@CCID", SqlDbType.NVarChar, 100).Value = CCID ?? "";
                        sqlCmd.Parameters.Add("@MESSAGE", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;
                        sqlCmd.Parameters.Add("@VALID", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.ExecuteNonQuery();
                        valid = Convert.ToBoolean(sqlCmd.Parameters["@VALID"].Value);
                        Message = Convert.ToString(sqlCmd.Parameters["@MESSAGE"].Value);
                    }
                }
                return valid;
            }

            catch (Exception ex)

            {
                Message = ex.Message;
                return false;
            }
        }

        public DataTable GetListMemoData(int PRID, string CCID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlComm = new SqlCommand("[dbo].[spGetListMemoData]", conn))
                    {
                        sqlComm.Parameters.Add("@PRID", SqlDbType.Int).Value = PRID;
                        sqlComm.Parameters.Add("@CCID", SqlDbType.NVarChar, 100).Value = CCID ?? "";
                        sqlComm.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlComm;
                        da.Fill(ds);
                    }
                }

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                var Error = ex;
                return new DataTable();
            }
        }

        public DataTable GetListOfData()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlComm = new SqlCommand("[dbo].[spGetMasterListMemo]", conn))
                    {
                        sqlComm.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlComm;
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

        private ListDataID SetListDataID(List<ListOfID> lstID, int mode, string ref_doc_no, string ref_key_3, string posting_date)
        {
            string _id = "";
            for (int i = 0; i < lstID.Count; i++)
            {
                _id += lstID[i].ID.ToString() + ",";
            }

            ListDataID _lstid = new ListDataID();
            _lstid.MODE = mode;
            _lstid.USERNAME = "APCOMMP2";
            _lstid.DOC_DATE = posting_date;
            _lstid.PSTNG_DATE = posting_date;

            string FISC_YEAR = "";
            if (posting_date.Length == 9)
            {
                FISC_YEAR = posting_date.Substring(5, 4);
            }
            else if (posting_date.Length == 8)
            {
                FISC_YEAR = posting_date.Substring(4, 4);
            }
            else
            {
                FISC_YEAR = posting_date.Substring(6, 4);
            }

            _lstid.FISC_YEAR = FISC_YEAR;
            _lstid.DOC_TYPE = "IA";
            _lstid.REF_DOC_NO = ref_doc_no;
            _lstid.REF_KEY_3 = ref_key_3;
            _lstid.CURRENCY = "THB";
            _lstid.ID = _id.Substring(0, _id.Length - 1);

            return _lstid;
        }

        private ListCrossData SetListCrossDataID(List<ListOfID> lstID, int mode, string ref_doc_no, string ref_key_3, string posting_date)
        {
            string _id = "";
            foreach (ListOfID item in lstID)
            {
                _id += item.ID.ToString() + ",";
            }

            //for (int i = 0; i < lstID.Count; i++)
            //{
            //    _id += lstID[i].ID.ToString() + ",";
            //}

            ListCrossData _lstid = new ListCrossData();
            _lstid.MODE = mode;
            _lstid.USERNAME = "APCOMMP2";

            string _pstDate = (posting_date.Split('/')[0].Length == 2 ? posting_date.Split('/')[0] : "0" + posting_date.Split('/')[0].ToString()) + "/" +
                              (posting_date.Split('/')[1].Length == 2 ? posting_date.Split('/')[1] : "0" + posting_date.Split('/')[1].ToString()) + "/" +
                              posting_date.Split('/')[2];

            _lstid.DOC_DATE = _pstDate;
            _lstid.PSTNG_DATE = _pstDate;
            _lstid.FISC_YEAR = (_pstDate != "" ? _pstDate.Substring(6, 4) : _pstDate);

            _lstid.PAYABLE_DOC_TYPE = "KR";
            _lstid.CUTSTOCK_DOC_TYPE = "IA";
            _lstid.RECEIVEABLE_DOC_TYPE = "DR";

            _lstid.BUSINESSPLACE = "0001";
            _lstid.VENDOR_NO = "1000";

            _lstid.REF_DOC_NO = ref_doc_no;
            _lstid.REF_KEY_3 = ref_key_3;
            _lstid.CURRENCY = "THB";
            _lstid.ID = _id.Substring(0, _id.Length - 1);

            //0,
            //'6',
            //'APCOMMP2',
            //'07/10/2016',
            //'07/10/2016',
            //'2016',
            //'KR',
            //'IA',
            //'DR',
            //'1001021',
            //'6666',
            //'THB',
            //'0001',
            //'1000'
            return _lstid;
        }

        private string SetIDCrossCompanyBeforeSave(List<ListOfID> lstID)
        {
            string id = "";
            for (int i = 0; i < lstID.Count; i++)
            {
                id += lstID[i].ID.ToString() + ",";
            }
            return id.Substring(0, id.Length - 1);
        }

        private string GetStrID(List<ListOfID> lstID)
        {
            string ret = "";
            foreach (var item in lstID)
            {
                ret += item.ID.ToString() + ",";
            }
            return ret.Substring(0, ret.Length - 1);
        }

        public DataTable CheckComCodeRetValue(List<ListOfID> lstID)
        {
            string strID = GetStrID(lstID);
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlComm = new SqlCommand("[dbo].[spGetComCodeCountRetValue]", conn))
                    {
                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.Parameters.Add("@ID", SqlDbType.VarChar, 150).Value = strID;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlComm;
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

        public DataTable CheckComCode(List<ListOfID> lstID)
        {
            string strID = GetStrID(lstID);
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlComm = new SqlCommand("[dbo].[spGetComCodeCount]", conn))
                    {
                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.Parameters.Add("@ID", SqlDbType.VarChar, 150).Value = strID;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlComm;
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

        public DataTable GetListDataItemsCrossCompanyByID(List<ListOfID> lstID, int mode, string ref_doc_no, string ref_key_3, string posting_date)
        {
            ListCrossData lstid = SetListCrossDataID(lstID, mode, ref_doc_no, ref_key_3, posting_date);
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlComm = new SqlCommand("[dbo].[spGetListDataItemsCrossCompanyByID]", conn))
                    {
                        sqlComm.CommandTimeout = 300;
                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.Parameters.Add("@MODE", SqlDbType.Int).Value = lstid.MODE;
                        sqlComm.Parameters.Add("@ID", SqlDbType.VarChar, 25).Value = lstid.ID;
                        sqlComm.Parameters.Add("@USERNAME", SqlDbType.VarChar, 12).Value = lstid.USERNAME;
                        sqlComm.Parameters.Add("@DOC_DATE", SqlDbType.VarChar, 10).Value = lstid.DOC_DATE;
                        sqlComm.Parameters.Add("@PSTNG_DATE", SqlDbType.VarChar, 10).Value = lstid.PSTNG_DATE;
                        sqlComm.Parameters.Add("@FISC_YEAR", SqlDbType.VarChar, 4).Value = lstid.FISC_YEAR;
                        sqlComm.Parameters.Add("@PAYABLE_DOC_TYPE", SqlDbType.VarChar, 2).Value = lstid.PAYABLE_DOC_TYPE;
                        sqlComm.Parameters.Add("@CUTSTOCK_DOC_TYPE", SqlDbType.VarChar, 2).Value = lstid.CUTSTOCK_DOC_TYPE;
                        sqlComm.Parameters.Add("@RECEIVEABLE_DOC_TYPE", SqlDbType.VarChar, 2).Value = lstid.RECEIVEABLE_DOC_TYPE;
                        sqlComm.Parameters.Add("@REF_DOC_NO", SqlDbType.VarChar, 16).Value = lstid.REF_DOC_NO;
                        sqlComm.Parameters.Add("@REF_KEY_3", SqlDbType.VarChar, 500).Value = lstid.REF_KEY_3;
                        sqlComm.Parameters.Add("@CURRENCY", SqlDbType.VarChar, 3).Value = lstid.CURRENCY;
                        sqlComm.Parameters.Add("@BUSINESSPLACE", SqlDbType.VarChar, 10).Value = lstid.BUSINESSPLACE;
                        sqlComm.Parameters.Add("@VENDOR_NO", SqlDbType.VarChar, 10).Value = lstid.VENDOR_NO;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlComm;
                        da.Fill(ds);
                    }
                }

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }

        public DataTable GetListDataItemsByID(List<ListOfID> lstID, int mode, string ref_doc_no, string ref_key_3, string posting_date)
        {
            ListDataID lstid = SetListDataID(lstID, mode, ref_doc_no, ref_key_3, posting_date);
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlComm = new SqlCommand("spGetListDataItemsByID", conn))
                    {
                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.Parameters.Add("@MODE", SqlDbType.Int).Value = lstid.MODE;
                        sqlComm.Parameters.Add("@USERNAME", SqlDbType.VarChar, 12).Value = lstid.USERNAME;
                        sqlComm.Parameters.Add("@DOC_DATE", SqlDbType.VarChar, 10).Value = lstid.DOC_DATE;
                        sqlComm.Parameters.Add("@PSTNG_DATE", SqlDbType.VarChar, 10).Value = lstid.PSTNG_DATE;
                        sqlComm.Parameters.Add("@FISC_YEAR", SqlDbType.VarChar, 4).Value = lstid.FISC_YEAR;
                        sqlComm.Parameters.Add("@DOC_TYPE", SqlDbType.VarChar, 2).Value = lstid.DOC_TYPE;
                        sqlComm.Parameters.Add("@REF_DOC_NO", SqlDbType.VarChar, 16).Value = lstid.REF_DOC_NO;
                        sqlComm.Parameters.Add("@REF_KEY_3", SqlDbType.VarChar, 500).Value = lstid.REF_KEY_3;
                        sqlComm.Parameters.Add("@CURRENCY", SqlDbType.VarChar, 3).Value = lstid.CURRENCY;
                        sqlComm.Parameters.Add("@ID", SqlDbType.VarChar, 250).Value = lstid.ID;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlComm;
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

        public DataTable GetListDataMasterItems(string docid, string itemno, string stddate, string enddate, int IsPostAcc)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spGetListDataMasterItems]", conn);

                    sqlComm.Parameters.Add("@DOCID", SqlDbType.VarChar, 20).Value = docid;
                    sqlComm.Parameters.Add("@ITEMNO", SqlDbType.VarChar, 20).Value = itemno ?? "";

                    if (stddate != "" && enddate != "")
                    {
                        sqlComm.Parameters.Add("@STDDATE", SqlDbType.VarChar, 10).Value = stddate.Split('/')[2].ToString() + stddate.Split('/')[1].ToString() + stddate.Split('/')[0].ToString();
                        sqlComm.Parameters.Add("@ENDDATE", SqlDbType.VarChar, 10).Value = enddate.Split('/')[2].ToString() + enddate.Split('/')[1].ToString() + enddate.Split('/')[0].ToString();
                    }
                    else
                    {
                        sqlComm.Parameters.Add("@STDDATE", SqlDbType.VarChar, 10).Value = stddate;
                        sqlComm.Parameters.Add("@ENDDATE", SqlDbType.VarChar, 10).Value = enddate;
                    }
                    sqlComm.Parameters.Add("@IsPostAcc", SqlDbType.Int).Value = IsPostAcc;

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

        public string OnCommandSaveRequisition(List<string> lstStr, string UserID)
        {
            string retStr;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {

                    using (SqlCommand sqlComm = new SqlCommand("[dbo].[spInsertDataRequisition]", conn))
                    {

                        DataSet ds = new DataSet();
                        sqlComm.Parameters.Add("@RQRNO", SqlDbType.VarChar, 10).Value = lstStr[0];
                        sqlComm.Parameters.Add("@DRNO", SqlDbType.VarChar, 25).Value = lstStr[1];

                        DateTime dt;
                        dt = DateTime.ParseExact((lstStr[2].Length == 9 ? "0" + lstStr[2] : lstStr[2]), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        sqlComm.Parameters.Add("@RCVD", SqlDbType.DateTime).Value = dt.ToString("dd/MM/yyyy");
                        dt = DateTime.ParseExact((lstStr[3].Length == 9 ? "0" + lstStr[2] : lstStr[2]), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        sqlComm.Parameters.Add("@CURD", SqlDbType.DateTime).Value = dt.ToString("dd/MM/yyyy");

                        sqlComm.Parameters.Add("@REFREQID", SqlDbType.VarChar, 10).Value = lstStr[4];
                        sqlComm.Parameters.Add("@REQID", SqlDbType.VarChar, 10).Value = lstStr[5];
                        sqlComm.Parameters.Add("@REQN", SqlDbType.VarChar, 150).Value = lstStr[6];
                        sqlComm.Parameters.Add("@PROMOTID", SqlDbType.Int).Value = Convert.ToInt32(lstStr[7]);
                        sqlComm.Parameters.Add("@PROMOTN", SqlDbType.VarChar, 150).Value = lstStr[8];
                        sqlComm.Parameters.Add("@DEPTID", SqlDbType.VarChar, 20).Value = lstStr[9];
                        sqlComm.Parameters.Add("@DEPTN", SqlDbType.VarChar, 150).Value = lstStr[10];

                        sqlComm.Parameters.Add("@GLID", SqlDbType.Int).Value = (lstStr[11] != "" ? Convert.ToInt32(lstStr[11]) : 0);
                        sqlComm.Parameters.Add("@GLN", SqlDbType.VarChar, 150).Value = lstStr[12];
                        sqlComm.Parameters.Add("@OBJID", SqlDbType.Int).Value = Convert.ToInt32(lstStr[13]);
                        sqlComm.Parameters.Add("@OBJN", SqlDbType.VarChar, 150).Value = lstStr[14];
                        sqlComm.Parameters.Add("@REMK", SqlDbType.VarChar, 250).Value = lstStr[15];
                        sqlComm.Parameters.Add("@PROJID", SqlDbType.VarChar, 20).Value = lstStr[16];
                        sqlComm.Parameters.Add("@PROJN", SqlDbType.VarChar, 250).Value = lstStr[17];
                        sqlComm.Parameters.Add("@PROPID", SqlDbType.VarChar, 20).Value = lstStr[18];
                        sqlComm.Parameters.Add("@PROPN", SqlDbType.VarChar, 250).Value = lstStr[19];
                        sqlComm.Parameters.Add("@QUANT", SqlDbType.Int).Value = (lstStr[20].ToString() == "" || lstStr[20].ToString() == null ? 0 : Convert.ToInt32(lstStr[20]));

                        if (lstStr[21].ToString() != "" || lstStr[22].ToString() != "")
                        {
                            dt = DateTime.ParseExact(lstStr[21], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            sqlComm.Parameters.Add("@STDDT", SqlDbType.DateTime).Value = dt.ToString("dd/MM/yyyy");
                            dt = DateTime.ParseExact(lstStr[22], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            sqlComm.Parameters.Add("@ENDDT", SqlDbType.DateTime).Value = dt.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            sqlComm.Parameters.Add("@STDDT", SqlDbType.DateTime).Value = DateTime.Now;
                            sqlComm.Parameters.Add("@ENDDT", SqlDbType.DateTime).Value = DateTime.Now;
                        }

                        sqlComm.Parameters.Add("@RMDB", SqlDbType.Int).Value = lstStr[23].ToString() == "" || lstStr[23].ToString() == null ? 0 : Convert.ToInt32(lstStr[23]);
                        sqlComm.Parameters.Add("@ITEMS", SqlDbType.Int).Value = lstStr[24].ToString() == "" || lstStr[24].ToString() == null ? 0 : Convert.ToInt32(lstStr[24]);
                        sqlComm.Parameters.Add("@APPVID", SqlDbType.VarChar, 20).Value = lstStr[25] == null ? "" : lstStr[25];
                        sqlComm.Parameters.Add("@APPVN", SqlDbType.VarChar, 150).Value = lstStr[26] == null ? "" : lstStr[26];
                        sqlComm.Parameters.Add("@STATUS", SqlDbType.Int).Value = Convert.ToInt32(lstStr[27]);

                        sqlComm.Parameters.Add("@DRNOBYITEM", SqlDbType.VarChar, 150).Value = lstStr[28] == null ? "" : lstStr[28].ToString();

                        sqlComm.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 150).Value = UserID;
                        sqlComm.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 150).Value = UserID;

                        sqlComm.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlComm;
                        da.Fill(ds);

                        retStr = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                    }
                }

                return retStr;
            }
            catch (Exception ex)
            {
                retStr = ex.ToString();
                return "";
            }
        }

        public bool OnCommandDeleteRequisitionByRQRNO(string rqrno)
        {
            try
            {
                SqlCommand sqlComm;
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (sqlComm = new SqlCommand("[dbo].[spDeleteRequisitionByRQRNO]", conn))
                    {
                        sqlComm.Parameters.Add("@RQRNO", SqlDbType.VarChar, 10).Value = rqrno;
                        sqlComm.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlComm;
                        da.Fill(ds);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public void OnCommandUpdateRequisition(List<string> lstStr, string userName)
        {
            //string retStr;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm;
                    DataSet ds = new DataSet();

                    //using (sqlComm = new SqlCommand("[dbo].[spDeleteRequisitionByRQRNO]", conn))
                    //{
                    //    sqlComm.Parameters.Add("@RQRNO", SqlDbType.VarChar, 10).Value = lstStr[0];
                    //    sqlComm.CommandType = CommandType.StoredProcedure;

                    //    SqlDataAdapter da = new SqlDataAdapter();
                    //    da.SelectCommand = sqlComm;
                    //    da.Fill(ds);
                    //}


                    using (sqlComm = new SqlCommand("[dbo].[spUpdateRequisitionByRQRNO]", conn))
                    {

                        sqlComm.Parameters.Add("@RQRNO", SqlDbType.VarChar, 10).Value = lstStr[0];
                        sqlComm.Parameters.Add("@DRNO", SqlDbType.VarChar, 25).Value = lstStr[1];

                        DateTime dt;
                        dt = DateTime.ParseExact(lstStr[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        sqlComm.Parameters.Add("@RCVD", SqlDbType.DateTime).Value = dt.ToString("dd/MM/yyyy");
                        dt = DateTime.ParseExact(lstStr[3], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        sqlComm.Parameters.Add("@CURD", SqlDbType.DateTime).Value = dt.ToString("dd/MM/yyyy");

                        sqlComm.Parameters.Add("@REFREQID", SqlDbType.VarChar, 10).Value = lstStr[4];
                        sqlComm.Parameters.Add("@REQID", SqlDbType.VarChar, 10).Value = lstStr[5];
                        sqlComm.Parameters.Add("@REQN", SqlDbType.VarChar, 150).Value = lstStr[6];
                        sqlComm.Parameters.Add("@PROMOTID", SqlDbType.Int).Value = Convert.ToInt32(lstStr[7]);
                        sqlComm.Parameters.Add("@PROMOTN", SqlDbType.VarChar, 150).Value = lstStr[8];
                        sqlComm.Parameters.Add("@DEPTID", SqlDbType.VarChar, 20).Value = lstStr[9];
                        sqlComm.Parameters.Add("@DEPTN", SqlDbType.VarChar, 150).Value = lstStr[10];

                        sqlComm.Parameters.Add("@GLID", SqlDbType.Int).Value = (lstStr[11] != "" ? Convert.ToInt32(lstStr[11]) : 0);
                        sqlComm.Parameters.Add("@GLN", SqlDbType.VarChar, 150).Value = lstStr[12];
                        sqlComm.Parameters.Add("@OBJID", SqlDbType.Int).Value = Convert.ToInt32(lstStr[13]);
                        sqlComm.Parameters.Add("@OBJN", SqlDbType.VarChar, 150).Value = lstStr[14];
                        sqlComm.Parameters.Add("@REMK", SqlDbType.VarChar, 250).Value = lstStr[15];
                        sqlComm.Parameters.Add("@PROJID", SqlDbType.VarChar, 20).Value = lstStr[16];
                        sqlComm.Parameters.Add("@PROJN", SqlDbType.VarChar, 250).Value = lstStr[17];
                        sqlComm.Parameters.Add("@PROPID", SqlDbType.VarChar, 20).Value = lstStr[18];
                        sqlComm.Parameters.Add("@PROPN", SqlDbType.VarChar, 250).Value = lstStr[19];
                        sqlComm.Parameters.Add("@RMDB", SqlDbType.Int).Value = (lstStr[20].ToString() == "" || lstStr[20].ToString() == null ? 0 : Convert.ToInt32(lstStr[20]));
                        if (lstStr[21].ToString() != "" || lstStr[22].ToString() != "")
                        {
                            dt = DateTime.ParseExact(lstStr[21], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            sqlComm.Parameters.Add("@STDDT", SqlDbType.DateTime).Value = dt.ToString("dd/MM/yyyy");
                            dt = DateTime.ParseExact(lstStr[22], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            sqlComm.Parameters.Add("@ENDDT", SqlDbType.DateTime).Value = dt.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            sqlComm.Parameters.Add("@STDDT", SqlDbType.DateTime).Value = DateTime.Now;
                            sqlComm.Parameters.Add("@ENDDT", SqlDbType.DateTime).Value = DateTime.Now;
                        }


                        sqlComm.Parameters.Add("@QUANT", SqlDbType.Int).Value = Convert.ToInt32(lstStr[23]);
                        sqlComm.Parameters.Add("@ITEMS", SqlDbType.Int).Value = Convert.ToInt32(lstStr[24]);
                        sqlComm.Parameters.Add("@APPVID", SqlDbType.VarChar, 20).Value = (lstStr[25] == null ? "" : lstStr[25]);
                        sqlComm.Parameters.Add("@APPVN", SqlDbType.VarChar, 150).Value = (lstStr[26] == null ? "" : lstStr[26]);
                        sqlComm.Parameters.Add("@STATUS", SqlDbType.Int).Value = Convert.ToInt32(lstStr[27]);

                        sqlComm.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 150).Value = userName;
                        sqlComm.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 150).Value = userName;

                        //sqlComm.Parameters.Add("@ACCROLE", SqlDbType.VarChar, 20).Value = "";

                        sqlComm.CommandType = CommandType.StoredProcedure;

                        //sqlComm.ExecuteNonQuery();


                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlComm;
                        da.Fill(ds);

                        //retStr = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                    }


                }

                //return "";
            }

            catch (Exception)

            {
                //return "";
            }
        }

        public bool OnCommandRejectRequisition(string rqrno, string reason)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[spRejectDataRequisition]", conn))
                    {
                        conn.Open();
                        cmd.Parameters.Add("@RQRNO", SqlDbType.VarChar, 10).Value = rqrno;
                        cmd.Parameters.Add("@REASON", SqlDbType.VarChar, 500).Value = reason;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }

            catch (Exception)

            {
                return false;
            }
        }

        public bool OnCommandApproveRequisition(string rqrno)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[spApproveDataRequisition]", conn))
                    {
                        conn.Open();
                        cmd.Parameters.Add("@RQRNO", SqlDbType.VarChar, 10).Value = rqrno;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }

            catch (Exception)

            {
                return false;
            }
        }

        public bool OnCommandAcceptRequisition(string rqrno)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[spAcceptDataRequisition]", conn))
                    {
                        conn.Open();
                        cmd.Parameters.Add("@RQRNO", SqlDbType.VarChar, 10).Value = rqrno;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }

            catch (Exception)

            {
                return false;
            }
        }

        public bool OnCommandSetSentRequisition(string rqrno)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[spSetSentDataRequisition]", conn))
                    {
                        conn.Open();
                        cmd.Parameters.Add("@RQRNO", SqlDbType.VarChar, 10).Value = rqrno;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }

            catch (Exception)

            {
                return false;
            }
        }

        public bool OnCommandDeleteRequisition(string rqrno)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[spDeleteDataRequisition]", conn))
                    {
                        conn.Open();
                        cmd.Parameters.Add("@RQRNO", SqlDbType.VarChar, 10).Value = rqrno;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }

            catch (Exception)

            {
                return false;
            }
        }

        public bool OnCommandOpenRecallRequisition(string rqrno)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[spSetOpenRecallDataRequisition]", conn))
                    {
                        conn.Open();
                        cmd.Parameters.Add("@RQRNO", SqlDbType.VarChar, 10).Value = rqrno;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }

            catch (Exception)

            {
                return false;
            }
        }

        public bool OnCommandConfirmRecallRequisition(string rqrno)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[spSetConfirmRecallDataRequisition]", conn))
                    {
                        conn.Open();
                        cmd.Parameters.Add("@RQRNO", SqlDbType.VarChar, 10).Value = rqrno;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }

            catch (Exception)

            {
                return false;
            }
        }

        public DataTable OnCommandEditRequisition(string rqrno)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    DataSet ds = new DataSet();
                    using (SqlCommand cmd = new SqlCommand("[dbo].[spGetRequisitionDataDetails]", conn))
                    {
                        cmd.Parameters.Add("@RQRNO", SqlDbType.VarChar, 10).Value = rqrno;
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                    }
                    return ds.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetRequisitionByEmpId(string empId, string mode)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[spGetDataRequisitionByEmpId]", conn))
                    {
                        conn.Open();
                        cmd.Parameters.Add("@EMPID", SqlDbType.VarChar, 10).Value = empId;
                        cmd.Parameters.Add("@MODE", SqlDbType.VarChar, 10).Value = mode;
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                    }
                    return ds.Tables[0];
                }
            }

            catch (Exception)

            {
                return null;
            }
        }

        public DataTable GetDataRequisition(
            string EmpCode, string docSearchID, string tagSearchID, string RecieveDatePicker,
            string BookDatePicker, string cbxUser, string cbxPromotionType, string cbxCostCenter,
            string cbxObjective, string cbxGLNo, out string msgerr)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[spGetDataRequisition]", conn))
                    {
                        conn.Open();
                        cmd.Parameters.Add("@EmpCode", SqlDbType.VarChar, 20).Value = EmpCode;

                        cmd.Parameters.Add("@docSearchID", SqlDbType.VarChar, 20).Value = docSearchID;
                        cmd.Parameters.Add("@tagSearchID", SqlDbType.VarChar, 20).Value = tagSearchID;

                        cmd.Parameters.Add("@RecieveDatePicker", SqlDbType.VarChar, 10).Value = RecieveDatePicker;

                        cmd.Parameters.Add("@BookDatePicker", SqlDbType.VarChar, 10).Value = BookDatePicker;

                        cmd.Parameters.Add("@cbxUser", SqlDbType.VarChar, 15).Value = (cbxUser == "All" ? "" : cbxUser);
                        cmd.Parameters.Add("@cbxPromotionType", SqlDbType.VarChar, 10).Value = (cbxPromotionType == "All" ? "" : cbxPromotionType);
                        cmd.Parameters.Add("@cbxCostCenter", SqlDbType.VarChar, 10).Value = (cbxCostCenter == "All" ? "" : cbxCostCenter);
                        cmd.Parameters.Add("@cbxObjective", SqlDbType.VarChar, 10).Value = (cbxObjective == "All" ? "" : cbxObjective);
                        cmd.Parameters.Add("@cbxGLNo", SqlDbType.VarChar, 10).Value = (cbxGLNo == "All" ? "" : cbxGLNo);
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                    }
                    msgerr = "";
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                msgerr = ex.Message.ToString();
                return null;
            }
        }

        public static DateTime? TryParse(string text)
        {
            DateTime date;
            if (DateTime.TryParseExact(text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date;
            }
            else
            {
                return null;
            }
        }

        public DataTable getDataMasterData(string source, string StartDate = "", string EndDate = "")
        {
            string STORED = (source != "right" ? "[dbo].[spGetDataMasterDatas]" : "[dbo].[spGetDataMasterGroupMapping]");
            try
            {
                var Start = TryParse(StartDate) == null ? new DateTime(1990, 1, 1) : TryParse(StartDate);
                var End = TryParse(EndDate) == null ? new DateTime(2300, 12, 31) : TryParse(EndDate);
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand(STORED, conn);
                    //sqlComm.Parameters.AddWithValue("@Function", func);
                    //sqlComm.Parameters.AddWithValue("@StartDate", startDate);
                    //sqlComm.Parameters.AddWithValue("@EndDate", endDate);
                    sqlComm.Parameters.AddWithValue("@Source", source);
                    sqlComm.Parameters.AddWithValue("@StartDate", Start);
                    sqlComm.Parameters.AddWithValue("@EndDate", End);
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

        public bool InsertAccReferCostCenterReq(string a, string b, string c)
        {
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(connStr))
                {
                    if (SqlConn.State == ConnectionState.Closed) SqlConn.Open();
                    using (SqlCommand SqlCmd = new SqlCommand("", SqlConn))
                    {

                    }
                }


                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool InsertAccCrossRecord(List<CREDITOR> ff, List<CUTSTOCK> hh, List<DEDTOR> jj)
        {
            try
            {

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool insertMasterData(string description, string reason, string startDate,
                                     string endDate, string userName, string source, out string msgerr)
        {
            try
            {
                var valid = true;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spInsertDataMasterDatas]", conn);
                    sqlComm.Parameters.AddWithValue("@Title", description);
                    sqlComm.Parameters.AddWithValue("@Description", reason);
                    if (startDate != "" && endDate != "")
                    {
                        DateTime _stdDate = new DateTime();
                        DateTime _endDate = new DateTime();
                        _stdDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        _endDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);

                        sqlComm.Parameters.AddWithValue("@StartDate", _stdDate);
                        sqlComm.Parameters.AddWithValue("@EndDate", _endDate);
                    }
                    else
                    {
                        sqlComm.Parameters.AddWithValue("@StartDate", startDate);
                        sqlComm.Parameters.AddWithValue("@EndDate", endDate);
                    }


                    sqlComm.Parameters.AddWithValue("@CreatedBy", userName);
                    sqlComm.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    sqlComm.Parameters.AddWithValue("@UpdatedBy", userName);
                    sqlComm.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                    sqlComm.Parameters.AddWithValue("@source", source);
                    sqlComm.Parameters.Add("@VALID", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@MESSAGE", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    Object output = sqlComm.Parameters["@VALID"].Value;
                    string outputMsg = sqlComm.Parameters["@MESSAGE"].Value.ToString();
                    valid = Convert.ToBoolean(sqlComm.Parameters["@VALID"].Value);
                    msgerr = Convert.ToString(sqlComm.Parameters["@MESSAGE"].Value.ToString());
                    conn.Close();
                }
                return valid;
            }
            catch (Exception ex)
            {
                msgerr = ex.Message;
                return false;
            }
        }

        public bool editMasterData(int id, string description, string reason, string startDate, string endDate, string userName, string source, ref string Message)
        {
            try
            {
                var valid = true;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spUpdateDataMasterDatas]", conn);
                    sqlComm.Parameters.AddWithValue("@ID", id);
                    sqlComm.Parameters.AddWithValue("@Title", description);
                    sqlComm.Parameters.AddWithValue("@Description", reason);
                    sqlComm.Parameters.Add("@MESSAGE", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@VALID", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    if (startDate != "" && endDate != "")
                    {
                        DateTime _stdDate = new DateTime();
                        DateTime _endDate = new DateTime();
                        _stdDate = DateTime.ParseExact((startDate.Length == 9 ? "0" + startDate : startDate), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        _endDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);

                        sqlComm.Parameters.AddWithValue("@StartDate", _stdDate);
                        sqlComm.Parameters.AddWithValue("@EndDate", _endDate);
                    }
                    else
                    {
                        sqlComm.Parameters.AddWithValue("@StartDate", startDate);
                        sqlComm.Parameters.AddWithValue("@EndDate", endDate);
                    }

                    //sqlComm.Parameters.AddWithValue("@StartDate", (startDate != "" ? Convert.ToDateTime(startDate) : DateTime.Now));
                    //sqlComm.Parameters.AddWithValue("@EndDate", (endDate != "" ? Convert.ToDateTime(endDate) : DateTime.Now));
                    sqlComm.Parameters.AddWithValue("@UpdatedBy", userName);
                    sqlComm.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                    sqlComm.Parameters.AddWithValue("@Source", source);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    valid = Convert.ToBoolean(sqlComm.Parameters["@VALID"].Value);
                    Message = Convert.ToString(sqlComm.Parameters["@MESSAGE"].Value);
                    conn.Close();
                }
                return valid;
            }

            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public bool deleteMasterData(int id, string title, string source)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spDeleteDataMasterDatas]", conn);
                    sqlComm.Parameters.AddWithValue("@ID", id);
                    sqlComm.Parameters.AddWithValue("@Title", title);
                    sqlComm.Parameters.AddWithValue("@Source", source);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                return false;
            }
        }

        public DataTable GetUser()
        {
            try
            {

                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spGetUserData]", conn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);
                }
                return ds.Tables[0];

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public DataTable GetUserEmail()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spGetUserEmailData]", conn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);
                }
                return ds.Tables[0];

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public DataTable GetGroup()
        {
            try
            {

                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spGetGroupData]", conn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);
                }
                return ds.Tables[0];

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public bool saveMapping(List<SaveGroupMapping> data, string username)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    for (int i = 0; i < data.Count; i++)
                    {
                        SqlCommand sqlComm = new SqlCommand("[dbo].[spInsertDataMasterGroupMapping]", conn);
                        sqlComm.Parameters.AddWithValue("@UserID", data[i].userID);
                        sqlComm.Parameters.AddWithValue("@CostCenter", data[i].costCenter);
                        sqlComm.Parameters.AddWithValue("@MasterGroupID", data[i].masterGroupID);
                        sqlComm.Parameters.AddWithValue("@CreatedBy", username);
                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                return true;
            }

            catch (Exception)

            {

                return false;
            }
        }

        public bool editMapping(int ID, int RIGHT, string USERNAME)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spUpdateDataMasterGroupMapping]", conn);
                    sqlComm.Parameters.AddWithValue("@ID", ID);
                    sqlComm.Parameters.AddWithValue("@RIGHT", RIGHT);
                    sqlComm.Parameters.AddWithValue("@USERNAME", USERNAME);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                }

                return true;
            }

            catch (Exception)

            {
                return false;
            }
        }

        public DataTable activated(int ID, string USERNAME)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spUpdateDataMasterActivation]", conn);
                    sqlComm.Parameters.AddWithValue("@ID", ID);
                    sqlComm.Parameters.AddWithValue("@USERNAME", USERNAME);
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

        public DataTable GetUserDetailByEmpID(string EmpID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spGetUserDetailByEmpID]", conn);
                    sqlComm.Parameters.AddWithValue("@EmpID", EmpID);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.CommandTimeout = 0;

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

        public DataTable GetCostCenterData(out string msgerr)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spGetCostCenter]", conn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);
                }
                msgerr = "";
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                msgerr = ex.Message.ToString();
                return null;
            }
        }

        public DataTable GetGLData(out string msgerr)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spGetGL]", conn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);
                }
                msgerr = "";
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                msgerr = ex.Message.ToString();
                return null;
            }
        }

        public DataTable GetItems()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetItems]", sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

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

        public DataTable GetAllMatItems()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetAllMatItems]", sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

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

        public ListGLandObj CheckMarketingUser(string empcode)
        {
            DataTable _gl = new DataTable();
            DataTable _Obj = new DataTable();
            SqlDataReader _sqlReader;
            ListGLandObj _lstGlandObj = new ListGLandObj();

            try
            {
                using (SqlConnection SqlCon = new SqlConnection(connStr))
                {
                    if (SqlCon.State == ConnectionState.Closed) SqlCon.Open();
                    using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spCheckMarketingUser]", SqlCon))
                    {
                        SqlCmd.Parameters.Add("@empcode", SqlDbType.VarChar, 15).Value = empcode;
                        SqlCmd.CommandType = CommandType.StoredProcedure;

                        _sqlReader = SqlCmd.ExecuteReader();
                        if (_sqlReader.FieldCount > 0)
                        {
                            _lstGlandObj.IsMKT = false;
                        }
                        else
                        {
                            _lstGlandObj.IsMKT = true;
                        }


                        List<GLData> _lstgldata = new List<GLData>();
                        while (_sqlReader.Read())
                        {
                            GLData _gldata = new GLData();
                            object[] values = new object[_sqlReader.FieldCount];
                            _sqlReader.GetValues(values);

                            _gldata.GLNO = values[0].ToString();
                            _gldata.GLName = values[1].ToString();
                            _lstgldata.Add(_gldata);
                        }
                        _lstGlandObj.GL = _lstgldata;

                        _sqlReader.NextResult();
                        List<ObjData> _lstobjdata = new List<ObjData>();
                        while (_sqlReader.Read())
                        {
                            ObjData _objdata = new ObjData();
                            object[] values = new object[_sqlReader.FieldCount];
                            _sqlReader.GetValues(values);
                            _objdata.ObjId = values[0].ToString();
                            _objdata.ObjName = values[1].ToString();
                            _lstobjdata.Add(_objdata);
                        }
                        _lstGlandObj.OBJ = _lstobjdata;

                        _sqlReader.NextResult();
                        List<Porjects> _lstProjects = new List<Porjects>();
                        while (_sqlReader.Read())
                        {
                            Porjects _projects = new Porjects();
                            object[] values = new object[_sqlReader.FieldCount];
                            _sqlReader.GetValues(values);
                            _projects.ProduectId = values[0].ToString();
                            _projects.Project = values[1].ToString();
                            _lstProjects.Add(_projects);
                        }
                        _lstGlandObj.PROJ = _lstProjects;


                        _lstGlandObj.IsCheck = true;
                        _lstGlandObj.Message = "";
                    }
                }
                return _lstGlandObj;
            }
            catch (Exception ex)
            {
                _lstGlandObj.IsCheck = false;
                _lstGlandObj.Message = ex.Message.ToString();
                _lstGlandObj.GL = new List<GLData>();
                _lstGlandObj.OBJ = new List<ObjData>();
                return _lstGlandObj;
            }
        }

        public string GetDocMemoType(string docno)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetDocType]", sqlConn))
                    {
                        sqlCmd.Parameters.Add("@DOCNO", SqlDbType.VarChar, 25).Value = docno;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }
                return ds.Tables[0].Rows[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public DataTable GetCostCenter(string empid)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetCostCenterByEmpId]", sqlConn))
                    {
                        sqlCmd.Parameters.Add("@EMPID", SqlDbType.VarChar, 10).Value = empid;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }

                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }

                return dt;
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        private List<ListMemo> GetListMemo(List<MemoItems> data, string EmpId)
        {
            List<ListMemo> Memo = new List<ListMemo>();

            foreach (var item in data[0].DataItemsValue)
            {
                ListMemo lstMemo = new ListMemo();
                lstMemo.ProjectID = item.ProjectId;
                lstMemo.ProjectName = item.ProjectName;
                lstMemo.ItemNo = item.ItemNo;
                lstMemo.ItemName = item.ItemName;
                lstMemo.PricePerUnit = item.PricePerUnit;
                lstMemo.Quantity = item.Quantity;
                lstMemo.Type = item.Type;
                lstMemo.TotalPrice = item.TotalPrice;
                lstMemo.DocType = data[0].DocType;
                lstMemo.DocNo = data[0].DocNo;
                lstMemo.CreateDate = data[0].CreateDate;
                lstMemo.UsingDate = data[0].UsingDate;
                lstMemo.EndingDate = data[0].EndingDate;
                lstMemo.UserCreateName = data[0].UserCreateName;
                lstMemo.CostCenterCode = data[0].CostCenterCode;
                lstMemo.CostCenterName = data[0].CostCenterName;
                lstMemo.Reason = data[0].Reason;

                lstMemo.CreateBy = EmpId;

                lstMemo.GLNO = data[0].GLNO;
                lstMemo.GLName = data[0].GLName;
                lstMemo.ObjID = (data[0].ObjID == "" ? (int?)null : Convert.ToInt32(data[0].ObjID));
                lstMemo.ObjName = data[0].ObjName;
                lstMemo.PromotionID = (data[0].PromotionID == "" ? (int?)null : Convert.ToInt32(data[0].PromotionID));
                lstMemo.Promotion = data[0].Promotion;

                Memo.Add(lstMemo);
            }
            return Memo;
        }

        private string EditMemoItem(List<ListMemo> DataValue, string EmpId)
        {
            try
            {
                string docNo = "";
                foreach (var item in DataValue)
                {
                    docNo = item.DocNo;
                }

                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (var ts = new TransactionScope())
                    {
                        if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
                        using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spDeleteMemoRequest]", sqlConn))
                        {
                            sqlCmd.Parameters.Add("@DOCNO", SqlDbType.VarChar, 15).Value = docNo;
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            sqlCmd.ExecuteNonQuery();
                        }

                        DataSet ds = new DataSet();
                        foreach (var item in DataValue)
                        {

                            using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spSaveMemoItems]", sqlConn))
                            {
                                sqlCmd.Parameters.Add("@DocType", SqlDbType.VarChar, 10).Value = item.DocType;
                                sqlCmd.Parameters.Add("@DocNo", SqlDbType.VarChar, 10).Value = (docNo == "" ? item.DocNo : docNo);
                                sqlCmd.Parameters.Add("@CreateDate", SqlDbType.Date).Value = Convert.ToDateTime(item.CreateDate);
                                sqlCmd.Parameters.Add("@UsingDate", SqlDbType.Date).Value = Convert.ToDateTime(item.UsingDate);
                                sqlCmd.Parameters.Add("@EndingDate", SqlDbType.Date).Value = Convert.ToDateTime(item.EndingDate);
                                sqlCmd.Parameters.Add("@UserCreateName", SqlDbType.VarChar, 50).Value = item.UserCreateName;
                                sqlCmd.Parameters.Add("@CostCenterCode", SqlDbType.VarChar, 10).Value = item.CostCenterCode;
                                sqlCmd.Parameters.Add("@CostCenterName", SqlDbType.VarChar, 150).Value = item.CostCenterName;
                                sqlCmd.Parameters.Add("@Reason", SqlDbType.VarChar).Value = item.Reason;
                                sqlCmd.Parameters.Add("@ProjectId", SqlDbType.VarChar, 20).Value = item.ProjectID;
                                sqlCmd.Parameters.Add("@ProjectName", SqlDbType.VarChar, 250).Value = item.ProjectName;
                                sqlCmd.Parameters.Add("@ItemNo", SqlDbType.VarChar, 15).Value = item.ItemNo;
                                sqlCmd.Parameters.Add("@ItemName", SqlDbType.VarChar, 150).Value = item.ItemName;
                                sqlCmd.Parameters.Add("@PricePerUnit", SqlDbType.Decimal).Value = item.PricePerUnit;
                                sqlCmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = item.Quantity;
                                sqlCmd.Parameters.Add("@Type", SqlDbType.VarChar, 25).Value = item.Type;
                                sqlCmd.Parameters.Add("@TotalPrice", SqlDbType.Decimal).Value = item.TotalPrice;
                                sqlCmd.Parameters.Add("@CreateBy", SqlDbType.VarChar, 20).Value = item.CreateBy;

                                sqlCmd.Parameters.Add("@GLNO", SqlDbType.VarChar, 10).Value = item.GLNO;
                                sqlCmd.Parameters.Add("@GLName", SqlDbType.VarChar, 250).Value = item.GLName;
                                sqlCmd.Parameters.Add("@ObjID", SqlDbType.Int).Value = item.ObjID;
                                sqlCmd.Parameters.Add("@ObjName", SqlDbType.VarChar, 250).Value = item.ObjName;
                                sqlCmd.Parameters.Add("@PromotionID", SqlDbType.Int).Value = item.PromotionID;
                                sqlCmd.Parameters.Add("@Promotion", SqlDbType.VarChar, 250).Value = item.Promotion;

                                sqlCmd.CommandType = CommandType.StoredProcedure;
                                sqlCmd.ExecuteNonQuery();
                            }
                        }
                        ts.Complete();
                    }

                }

                return docNo;


            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public bool SaveMemoItems(int savemode, List<MemoItems> DataValue, string EmpId, out string docno)
        {
            try
            {
                string docNo = "";
                List<ListMemo> lstMemo = new List<ListMemo>();
                lstMemo = GetListMemo(DataValue, EmpId);
                if (savemode == 1)
                {
                    docno = EditMemoItem(lstMemo, EmpId);
                }
                else
                {
                    DataSet ds = new DataSet();
                    foreach (var item in lstMemo)
                    {
                        using (SqlConnection sqlConn = new SqlConnection(connStr))
                        {
                            using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spSaveMemoItems]", sqlConn))
                            {
                                sqlCmd.Parameters.Add("@DocType", SqlDbType.VarChar, 10).Value = item.DocType;
                                sqlCmd.Parameters.Add("@DocNo", SqlDbType.VarChar, 10).Value = (docNo == "" ? item.DocNo : docNo);
                                sqlCmd.Parameters.Add("@CreateDate", SqlDbType.Date).Value = Convert.ToDateTime(item.CreateDate);
                                sqlCmd.Parameters.Add("@UsingDate", SqlDbType.Date).Value = Convert.ToDateTime(item.UsingDate);
                                sqlCmd.Parameters.Add("@EndingDate", SqlDbType.Date).Value = Convert.ToDateTime(item.EndingDate);
                                sqlCmd.Parameters.Add("@UserCreateName", SqlDbType.VarChar, 50).Value = item.UserCreateName;
                                sqlCmd.Parameters.Add("@CostCenterCode", SqlDbType.VarChar, 10).Value = item.CostCenterCode;
                                sqlCmd.Parameters.Add("@CostCenterName", SqlDbType.VarChar, 150).Value = item.CostCenterName;
                                sqlCmd.Parameters.Add("@Reason", SqlDbType.VarChar).Value = item.Reason;
                                sqlCmd.Parameters.Add("@ProjectId", SqlDbType.VarChar, 20).Value = (item.ProjectID == null ? "" : item.ProjectID);
                                sqlCmd.Parameters.Add("@ProjectName", SqlDbType.VarChar, 250).Value = (item.ProjectName == null ? "" : item.ProjectName);
                                sqlCmd.Parameters.Add("@ItemNo", SqlDbType.VarChar, 15).Value = item.ItemNo;
                                sqlCmd.Parameters.Add("@ItemName", SqlDbType.VarChar, 150).Value = item.ItemName;
                                sqlCmd.Parameters.Add("@PricePerUnit", SqlDbType.Decimal).Value = item.PricePerUnit;
                                sqlCmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = item.Quantity;
                                sqlCmd.Parameters.Add("@Type", SqlDbType.VarChar, 25).Value = item.Type;
                                sqlCmd.Parameters.Add("@TotalPrice", SqlDbType.Decimal).Value = item.TotalPrice;
                                sqlCmd.Parameters.Add("@CreateBy", SqlDbType.VarChar, 20).Value = item.CreateBy;

                                if (item.ObjID != null)
                                {
                                    sqlCmd.Parameters.Add("@GLNO", SqlDbType.VarChar, 10).Value = item.GLNO;
                                    sqlCmd.Parameters.Add("@GLName", SqlDbType.VarChar, 250).Value = item.GLName;

                                    sqlCmd.Parameters.Add("@ObjID", SqlDbType.Int).Value = item.ObjID;
                                    sqlCmd.Parameters.Add("@ObjName", SqlDbType.VarChar, 250).Value = item.ObjName;
                                    sqlCmd.Parameters.Add("@PromotionID", SqlDbType.Int).Value = item.PromotionID;
                                    sqlCmd.Parameters.Add("@Promotion", SqlDbType.VarChar, 250).Value = item.Promotion;
                                }

                                sqlCmd.CommandType = CommandType.StoredProcedure;

                                SqlDataAdapter da = new SqlDataAdapter();
                                da.SelectCommand = sqlCmd;
                                da.Fill(ds);

                                if (ds.Tables[0].Rows[0].ItemArray[0].ToString() != "")
                                {
                                    docNo = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                                }
                            }
                        }
                    }
                    docno = docNo;
                }

                return true;
            }
            catch (Exception)
            {
                docno = "";
                return false;
            }
        }

        public DataTable GetDataMemo(string DocNo, string StdDate, string EndDate, string status, string empcode)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetDataMemoRequest]", sqlConn))
                    {
                        sqlCmd.Parameters.Add("@DOCUNO", SqlDbType.VarChar, 9).Value = DocNo;

                        DateTime stdDttime = new DateTime(), endDttime = new DateTime();

                        if (StdDate != "" || EndDate != "")
                        {
                            string day, month, year;

                            //StdDate
                            day = (StdDate.Split('/')[0].Length == 1 ? '0' + StdDate.Split('/')[0].ToString() : StdDate.Split('/')[0].ToString());
                            month = (StdDate.Split('/')[1].Length == 1 ? '0' + StdDate.Split('/')[1].ToString() : StdDate.Split('/')[1].ToString());
                            year = StdDate.Split('/')[2].ToString();
                            stdDttime = DateTime.ParseExact(day + "/" + month + "/" + year, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);

                            //EndDate
                            day = (EndDate.Split('/')[0].Length == 1 ? '0' + EndDate.Split('/')[0].ToString() : EndDate.Split('/')[0].ToString());
                            month = (EndDate.Split('/')[1].Length == 1 ? '0' + EndDate.Split('/')[1].ToString() : EndDate.Split('/')[1].ToString());
                            year = EndDate.Split('/')[2].ToString();
                            endDttime = DateTime.ParseExact(day + "/" + month + "/" + year, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);

                            sqlCmd.Parameters.Add("@STDDATE", SqlDbType.VarChar, 10).Value = stdDttime.ToString("yyyy-MM-dd").ToString();
                            sqlCmd.Parameters.Add("@ENDDATE", SqlDbType.VarChar, 10).Value = endDttime.ToString("yyyy-MM-dd").ToString();
                        }
                        else
                        {
                            sqlCmd.Parameters.Add("@STDDATE", SqlDbType.VarChar, 10).Value = "";
                            sqlCmd.Parameters.Add("@ENDDATE", SqlDbType.VarChar, 10).Value = "";
                        }

                        sqlCmd.Parameters.Add("@STATUS", SqlDbType.VarChar, 1).Value = status;
                        sqlCmd.Parameters.Add("@EMPCODE", SqlDbType.VarChar, 10).Value = empcode;

                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }

        public DataTable GetDataMemoByDocID(string DocNo)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetDataMemoRequestByID]", sqlConn))
                    {
                        sqlCmd.Parameters.Add("@DOCNO", SqlDbType.VarChar, 9).Value = DocNo;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

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

        public string GetUserPositionByDocNo(string DocNo)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetUserPositionByDocNo]", sqlConn))
                    {
                        sqlCmd.Parameters.Add("@DOCNO", SqlDbType.VarChar, 9).Value = DocNo;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }
                string data = ds.Tables[0].Rows[0].ItemArray[0].ToString() + '|' + ds.Tables[0].Rows[0].ItemArray[0].ToString();
                return ds.Tables[0].Rows[0].ItemArray[0].ToString();
            }

            catch (Exception)

            {
                return "";
            }
        }

        public DataTable GetGLData()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spGetMasterGL]", conn);
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

        public DataTable GetGLDataByRole(string RoleCode)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spGetMasterGLByRole]", conn);
                    sqlComm.Parameters.Add("@RoleCode", SqlDbType.VarChar).Value = RoleCode;
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

        public DataTable GetProdutcDetailsData()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spGetDataPromotionItems]", conn);
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

        public DataTable GetProjectsData()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spGetDataProjects]", conn);
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

        public DataTable GetObjectiveData()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spGetMasterReason]", conn);
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

        public DataTable GetListItemsMemoByDocNo(string docno, string itemno, out string msgerr)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetItemsMemoByDocNo]", conn))
                    {
                        sqlCmd.Parameters.Add("@DOCNO", SqlDbType.VarChar, 12).Value = docno;
                        sqlCmd.Parameters.Add("@ITEMNO", SqlDbType.VarChar, 30).Value = itemno;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }

                msgerr = "";
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                msgerr = ex.Message.ToString();
                return new DataTable();
            }
        }

        public DataTable GetMemoData(string empid, out string msgerr)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("[dbo].[GetMemoData]", conn);
                    sqlComm.Parameters.Add("@EMPCODE", SqlDbType.VarChar, 10).Value = empid;
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);
                }
                msgerr = "";
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                msgerr = ex.Message.ToString();
                return null;
            }
        }

        public DataTable GetUserByType(int type)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spGetRolesByRoleCode]", conn);
                    sqlComm.Parameters.AddWithValue("@Key", type);
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

        public DataTable GetUserRole()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spUserRole]", conn);
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

        public string GetUserEmailByuserGUID(string USERGUID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetUserEmailByuserGUID]", sqlConn))
                    {
                        sqlCmd.Parameters.Add("@USERGUID", SqlDbType.VarChar, 100).Value = USERGUID;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }
                return ds.Tables[0].Rows[0].ItemArray[0].ToString();
            }

            catch (Exception)

            {
                return "";
            }
        }

        public string GetUserEmailByuserID(int UserID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT Email FROM [dbo].[vw_User] WHERE UserID = @1", sqlConn))
                    {
                        sqlCmd.Parameters.Add(new SqlParameter("1", UserID));

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }
                return ds.Tables[0].Rows[0].ItemArray[0].ToString();
            }

            catch (Exception)
            {
                return "";
            }
        }

        public string GetGUIDByEmpCode(string EmpCode)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT UserGUID FROM [dbo].[vw_User] WHERE EmpCode = @1", sqlConn))
                    {
                        sqlCmd.Parameters.Add(new SqlParameter("1", EmpCode));

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }
                return ds.Tables[0].Rows[0].ItemArray[0].ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public DataTable GetUserByGUID(string GUID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT * FROM [dbo].[vw_User] WHERE UserGUID = @1", sqlConn))
                    {
                        sqlCmd.Parameters.Add(new SqlParameter("1", GUID));

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }
                return ds.Tables[0] ?? new DataTable();
            }

            catch (Exception)
            {
                return new DataTable();
            }
        }

        public string GetEmailApproverByDocNo(string DocNo)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[sp_Get_EmailApprover_By_DocNo]", sqlConn))
                    {
                        sqlCmd.Parameters.Add("@DOCNO", SqlDbType.VarChar, 10).Value = DocNo;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }
                return ds.Tables[0].Rows[0].ItemArray[0].ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string GetUserGUIDByUserId(string USERID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetUserGUIDByUserId]", sqlConn))
                    {
                        sqlCmd.Parameters.Add("@USERID", SqlDbType.Int).Value = Convert.ToInt32(USERID);
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }
                return ds.Tables[0].Rows[0].ItemArray[0].ToString();
            }

            catch (Exception)

            {
                return "";
            }
        }

        public DataTable GetUserByMemoDocNo(string docno)

        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetUserByMemoDocNo]", sqlConn))
                    {
                        sqlCmd.Parameters.Add("@DOCNO", SqlDbType.VarChar, 9).Value = docno;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

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

        public int GetMemoStatus(string docno)
        {
            try
            {
                int status = 0;
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spCheckStatusBeforeApproveReturnStatus]", sqlConn))
                    {
                        sqlCmd.Parameters.Add("@DOCNO", SqlDbType.VarChar, 9).Value = docno;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }

                if (ds.Tables[0].Rows.Count != 0)
                {
                    status = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0]);
                }

                return status; //ds.Tables[0].Rows[0].ItemArray[0];
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void SendMailResponseApproved(string DOCNO, string MailTest, ref string _msg)
        {
            try
            {
                int status = 0;
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spSendMailResponseApproved]", sqlConn))
                    {
                        sqlCmd.Parameters.Add("@DOCNO", SqlDbType.VarChar).Value = DOCNO;
                        sqlCmd.Parameters.Add("@MailTest", SqlDbType.VarChar).Value = MailTest;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }

                _msg = "";
            }
            catch (Exception ex)
            {
                _msg = ex.ToString();
            }
        }

        public double GetPRItemPriceByReqNo(string rcvPrmId, string materialId, out string msgerr)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection SqlConn = new SqlConnection(connStr))
                {
                    if (SqlConn.State == ConnectionState.Closed) SqlConn.Open();
                    using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spGetPRItemPriceByReqNo]", SqlConn))
                    {
                        SqlCmd.Parameters.Add("@rcvPrmId", SqlDbType.VarChar, 15).Value = rcvPrmId;
                        SqlCmd.Parameters.Add("@materialId", SqlDbType.VarChar, 20).Value = materialId;
                        SqlCmd.CommandType = CommandType.StoredProcedure;
                        SqlCmd.ExecuteNonQuery();

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = SqlCmd;
                        da.Fill(ds);
                    }
                }
                msgerr = "";
                return Convert.ToDouble(ds.Tables[0].Rows[0].ItemArray[0]);
            }
            catch (Exception ex)
            {
                msgerr = ex.Message.ToString();
                return 0;
            }
        }

        public double GetPriceFromMasterItemByMasterId(int mstItemId, out string msgerr)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection SqlConn = new SqlConnection(connStr))
                {
                    if (SqlConn.State == ConnectionState.Closed) SqlConn.Open();
                    using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spGetPriceFromMasterItemByMasterId]", SqlConn))
                    {
                        SqlCmd.Parameters.Add("@mstItemId", SqlDbType.VarChar, 15).Value = mstItemId;
                        SqlCmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = SqlCmd;
                        da.Fill(ds);
                    }
                }
                msgerr = "";
                return Convert.ToDouble(ds.Tables[0].Rows[0].ItemArray[0]);
            }
            catch (Exception ex)
            {
                msgerr = ex.Message.ToString();
                return 0;
            }
        }

        public DataTable GetStockItemTypeByMasterItemId(int masterItemId, out string msgerr)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection SqlConn = new SqlConnection(connStr))
                {
                    if (SqlConn.State == ConnectionState.Closed) SqlConn.Open();
                    using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spGetStockItemTypeByMasterItemId]", SqlConn))
                    {
                        SqlCmd.Parameters.Add("@MasterItemId", SqlDbType.VarChar, 15).Value = masterItemId;
                        SqlCmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = SqlCmd;
                        da.Fill(ds);
                    }
                }
                msgerr = "";
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                msgerr = ex.Message.ToString();
                return new DataTable();
            }
        }

        public DataTable GetStockItemByMasterItemId(int masterItemId, string serial, out string msgerr)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection SqlConn = new SqlConnection(connStr))
                {
                    if (SqlConn.State == ConnectionState.Closed) SqlConn.Open();
                    using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spGetStockItemByMasterItemId]", SqlConn))
                    {
                        SqlCmd.Parameters.Add("@MasterItemId", SqlDbType.VarChar, 15).Value = masterItemId;
                        SqlCmd.Parameters.Add("@serial", SqlDbType.VarChar, 20).Value = serial;
                        SqlCmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = SqlCmd;
                        da.Fill(ds);
                    }
                }
                msgerr = "";
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                msgerr = ex.Message.ToString();
                return new DataTable();
            }
        }

        public string UpdateRequisReq(SqlConnection SqlCon, string id, string rqrno, string sapid, string ref_doc_no, string ref_key_3, string posting_date, string empid, string itemno, string docType)
        {
            try
            {

                if (SqlCon.State == ConnectionState.Closed) SqlCon.Open();
                {
                    using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spUpdateRequisReq]", SqlCon))
                    {
                        SqlCmd.Parameters.Add("@RQRNO", SqlDbType.VarChar, 25).Value = rqrno;
                        SqlCmd.Parameters.Add("@SAPID", SqlDbType.VarChar, 250).Value = sapid;
                        SqlCmd.Parameters.Add("@SAPREFID", SqlDbType.VarChar, 50).Value = id;
                        SqlCmd.Parameters.Add("@REFDOCNO", SqlDbType.VarChar, 50).Value = ref_doc_no;
                        SqlCmd.Parameters.Add("@REFKEY3", SqlDbType.VarChar, 50).Value = ref_key_3;
                        SqlCmd.Parameters.Add("@PSTINGDATE", SqlDbType.VarChar, 50).Value = posting_date;
                        SqlCmd.Parameters.Add("@TYPE", SqlDbType.VarChar, 20).Value = docType;
                        SqlCmd.Parameters.Add("@EMPID", SqlDbType.VarChar, 20).Value = empid;
                        SqlCmd.Parameters.Add("@ITEMNO", SqlDbType.VarChar, 20).Value = itemno;
                        SqlCmd.CommandType = CommandType.StoredProcedure;
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

        public bool UpdateSapDocIdToRequisitionRequest(List<ListOfID> lstID, string sapdocno, string ref_doc_no, string ref_key_3, string posting_date, List<ACCREC> lstAcc, string empid)
        {
            try
            {
                string msgErr = "";
                var _cr = (from tmpcr in lstAcc.AsEnumerable()
                           group tmpcr by new
                           {
                               id = tmpcr.RQRNO
                           } into c
                           select new
                           {
                               c.Key.id
                           }).ToList();

                var _item = (from tmpItem in lstAcc.AsEnumerable()
                             group tmpItem by new
                             {
                                 rqrno = tmpItem.RQRNO,
                                 itemno = tmpItem.ITEMNO
                             } into c
                             select new
                             {
                                 c.Key.rqrno,
                                 c.Key.itemno
                             }).ToList();

                using (SqlConnection SqlCon = new SqlConnection(connStr))
                {
                    for (int i = 0; i < _cr.Count; i++)
                    {
                        string id = "";
                        if (SqlCon.State == ConnectionState.Closed) SqlCon.Open();
                        using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spInsertAccountRefCostCenter]", SqlCon))
                        {
                            SqlCmd.Parameters.Add("@RQRNO", SqlDbType.VarChar, 25).Value = _cr[i].id.ToString();
                            SqlCmd.Parameters.Add("@CURACC", SqlDbType.VarChar, 35).Value = sapdocno;
                            SqlCmd.Parameters.Add("@CREDITOR", SqlDbType.VarChar, 35).Value = DBNull.Value;
                            SqlCmd.Parameters.Add("@CUTSTOCK", SqlDbType.VarChar, 35).Value = DBNull.Value;
                            SqlCmd.Parameters.Add("@DEDTOR", SqlDbType.VarChar, 35).Value = DBNull.Value;
                            SqlCmd.Parameters.Add("@REFID", SqlDbType.NVarChar, 10).Direction = ParameterDirection.Output;
                            SqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlCmd.ExecuteNonQuery();
                            id = Convert.ToString(SqlCmd.Parameters["@REFID"].Value);
                        }

                        if (SqlCon.State == ConnectionState.Closed) SqlCon.Open();
                        string reqrno = _cr[i].id.ToString();
                        msgErr = InsertCurrAccountingRecords(SqlCon, id, reqrno, lstAcc);
                        if (msgErr == "")
                        {
                            string sap_id = sapdocno.Substring(0, 10);

                            var ddd = _item.Where(q => q.rqrno == reqrno).ToList();

                            for (int ii = 0; ii < ddd.Count; ii++)
                            {
                                msgErr = UpdateRequisReq(SqlCon, id, reqrno, sap_id, ref_doc_no, ref_key_3, posting_date, empid, ddd[ii].itemno, "I");
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public object[] SaveRecordingAccount(List<CREDITOR> cr, List<CUTSTOCK> cs, List<DEDTOR> dd,
                                             string sap_PayableDocNo, string sap_CutstockDocNo, string sap_RecieveableDocNo,
                                             string ref_doc_no, string ref_key_3, string posting_date, string empid)
        {
            try
            {
                string msgErr = "";

                var _cr = (from tmpcr in cs.AsEnumerable()
                           group tmpcr by new { id = tmpcr.RQRNO } into c
                           select new { c.Key.id }).ToList();

                var _item = (from tmpItem in cs.AsEnumerable()
                             group tmpItem by new { rqrno = tmpItem.RQRNO, itemno = tmpItem.ITEMNO } into c
                             select new { c.Key.rqrno, c.Key.itemno }).ToList();

                using (SqlConnection SqlCon = new SqlConnection(connStr))
                {
                    for (int i = 0; i < _cr.Count; i++)
                    {
                        string id = "";

                        if (SqlCon.State == ConnectionState.Closed)
                            SqlCon.Open();

                        using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spInsertAccountRefCostCenter]", SqlCon))
                        {
                            SqlCmd.Parameters.Add("@RQRNO", SqlDbType.VarChar, 25).Value = _item[i].rqrno.ToString();
                            SqlCmd.Parameters.Add("@CURACC", SqlDbType.VarChar, 35).Value = DBNull.Value;
                            SqlCmd.Parameters.Add("@CREDITOR", SqlDbType.VarChar, 35).Value = sap_PayableDocNo;
                            SqlCmd.Parameters.Add("@CUTSTOCK", SqlDbType.VarChar, 35).Value = sap_CutstockDocNo;
                            SqlCmd.Parameters.Add("@DEDTOR", SqlDbType.VarChar, 35).Value = sap_RecieveableDocNo;
                            SqlCmd.Parameters.Add("@REFID", SqlDbType.NVarChar, 10).Direction = ParameterDirection.Output;
                            SqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlCmd.ExecuteNonQuery();
                            id = Convert.ToString(SqlCmd.Parameters["@REFID"].Value);
                        }

                        if (SqlCon.State == ConnectionState.Closed)
                            SqlCon.Open();

                        string reqrno = _item[i].rqrno.ToString();

                        msgErr = InsertAccountingRecords(SqlCon, id, reqrno, cr, cs, dd);
                        if (msgErr == "")
                        {
                            string sap_id = sap_PayableDocNo.Substring(0, 10) + ", " + sap_CutstockDocNo.Substring(0, 10) + ", " + sap_RecieveableDocNo.Substring(0, 10);

                            if (reqrno == _item[i].rqrno.ToString())
                            {
                                msgErr = UpdateRequisReq(SqlCon, id, reqrno, sap_id, ref_doc_no, ref_key_3, posting_date, empid, _item[i].itemno, "C");
                            }
                        }

                        if (msgErr != "")
                        {
                            return new object[]
                            {
                                msgErr
                            };
                        }
                    }
                }
                return new object[]{
                        msgErr
                };
            }
            catch (Exception ex)
            {
                return new object[]{
                        ex.Message.ToString()
                };
            }
        }

        private string InsertCurrAccountingRecords(SqlConnection SqlCon, string id, string rqrno, List<ACCREC> lstAcc)
        {
            try
            {
                int i = 1;
                int iCount = 0;
                for (iCount = 0; iCount < lstAcc.Count; iCount++)
                {
                    if (rqrno == lstAcc[iCount].RQRNO)
                    {
                        using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spInsertAccountingRecords]", SqlCon))
                        {
                            SqlCmd.Parameters.Add("@ITEM", SqlDbType.Int).Value = i;
                            SqlCmd.Parameters.Add("@ACCRECTYPECODE", SqlDbType.Int).Value = lstAcc[iCount].ACCRECTYPECODE;
                            SqlCmd.Parameters.Add("@CCAREFREQID", SqlDbType.Int).Value = id;

                            SqlCmd.Parameters.Add("@GLNO", SqlDbType.VarChar, 20).Value = lstAcc[iCount].GLNO;
                            SqlCmd.Parameters.Add("@GLNAME", SqlDbType.VarChar, 500).Value = lstAcc[iCount].GLNAME;
                            SqlCmd.Parameters.Add("@COSTCENTERID", SqlDbType.VarChar, 20).Value = lstAcc[iCount].COSTCENTERID;
                            SqlCmd.Parameters.Add("@PROFITCENTER", SqlDbType.VarChar, 20).Value = lstAcc[iCount].PROFITCENTER;
                            SqlCmd.Parameters.Add("@DEBIT", SqlDbType.Decimal).Value = (lstAcc[iCount].DEBIT == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(lstAcc[iCount].DEBIT));
                            SqlCmd.Parameters.Add("@CREDIT", SqlDbType.Decimal).Value = (lstAcc[iCount].CREDIT == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(lstAcc[iCount].CREDIT));
                            SqlCmd.Parameters.Add("@ITEMTEXT", SqlDbType.VarChar, 500).Value = lstAcc[iCount].ITEMTEXT;

                            SqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlCmd.ExecuteNonQuery();

                            i++;
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        private string InsertAccountingRecords(SqlConnection SqlCon, string id, string rqrno, List<CREDITOR> cr, List<CUTSTOCK> cs, List<DEDTOR> dd)
        {
            try
            {
                int i = 1;
                int iCount = 0;
                for (iCount = 0; iCount < cr.Count; iCount++)
                {
                    if (rqrno == cr[iCount].RQRNO)
                    {
                        using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spInsertAccountingRecords]", SqlCon))
                        {
                            SqlCmd.Parameters.Add("@ITEM", SqlDbType.Int).Value = i;
                            SqlCmd.Parameters.Add("@ACCRECTYPECODE", SqlDbType.Int).Value = cr[iCount].ACCRECTYPECODE;
                            SqlCmd.Parameters.Add("@CCAREFREQID", SqlDbType.Int).Value = id;

                            SqlCmd.Parameters.Add("@GLNO", SqlDbType.VarChar, 20).Value = cr[iCount].GLNO;
                            SqlCmd.Parameters.Add("@GLNAME", SqlDbType.VarChar, 500).Value = cr[iCount].GLNAME;
                            SqlCmd.Parameters.Add("@COSTCENTERID", SqlDbType.VarChar, 20).Value = cr[iCount].COSTCENTERID;
                            SqlCmd.Parameters.Add("@PROFITCENTER", SqlDbType.VarChar, 20).Value = cr[iCount].PROFITCENTER;
                            SqlCmd.Parameters.Add("@DEBIT", SqlDbType.Decimal).Value = (cr[iCount].DEBIT == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(cr[iCount].DEBIT));
                            SqlCmd.Parameters.Add("@CREDIT", SqlDbType.Decimal).Value = (cr[iCount].CREDIT == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(cr[iCount].CREDIT));
                            SqlCmd.Parameters.Add("@ITEMTEXT", SqlDbType.VarChar, 500).Value = cr[iCount].ITEMTEXT;

                            SqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlCmd.ExecuteNonQuery();

                            i++;
                        }
                    }
                }

                for (iCount = 0; iCount < cs.Count; iCount++)
                {
                    if (rqrno == cs[iCount].RQRNO)
                    {
                        using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spInsertAccountingRecords]", SqlCon))
                        {
                            SqlCmd.Parameters.Add("@ITEM", SqlDbType.Int).Value = i;
                            SqlCmd.Parameters.Add("@ACCRECTYPECODE", SqlDbType.Int).Value = cs[iCount].ACCRECTYPECODE;
                            SqlCmd.Parameters.Add("@CCAREFREQID", SqlDbType.Int).Value = id;

                            SqlCmd.Parameters.Add("@GLNO", SqlDbType.VarChar, 20).Value = cs[iCount].GLNO;
                            SqlCmd.Parameters.Add("@GLNAME", SqlDbType.VarChar, 500).Value = cs[iCount].GLNAME;
                            SqlCmd.Parameters.Add("@COSTCENTERID", SqlDbType.VarChar, 20).Value = cs[iCount].COSTCENTERID;
                            SqlCmd.Parameters.Add("@PROFITCENTER", SqlDbType.VarChar, 20).Value = cs[iCount].PROFITCENTER;
                            SqlCmd.Parameters.Add("@DEBIT", SqlDbType.Decimal).Value = cs[iCount].DEBIT == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(cs[iCount].DEBIT);
                            SqlCmd.Parameters.Add("@CREDIT", SqlDbType.Decimal).Value = cs[iCount].CREDIT == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(cs[iCount].CREDIT);
                            SqlCmd.Parameters.Add("@ITEMTEXT", SqlDbType.VarChar, 500).Value = cs[iCount].ITEMTEXT;

                            SqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlCmd.ExecuteNonQuery();
                            i++;
                        }
                    }
                }

                for (iCount = 0; iCount < dd.Count; iCount++)
                {
                    if (rqrno == dd[iCount].RQRNO)
                    {
                        using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spInsertAccountingRecords]", SqlCon))
                        {
                            SqlCmd.Parameters.Add("@ITEM", SqlDbType.Int).Value = i;
                            SqlCmd.Parameters.Add("@ACCRECTYPECODE", SqlDbType.Int).Value = dd[iCount].ACCRECTYPECODE;
                            SqlCmd.Parameters.Add("@CCAREFREQID", SqlDbType.Int).Value = id;

                            SqlCmd.Parameters.Add("@GLNO", SqlDbType.VarChar, 20).Value = dd[iCount].GLNO;
                            SqlCmd.Parameters.Add("@GLNAME", SqlDbType.VarChar, 500).Value = dd[iCount].GLNAME;
                            SqlCmd.Parameters.Add("@COSTCENTERID", SqlDbType.VarChar, 20).Value = dd[iCount].COSTCENTERID;
                            SqlCmd.Parameters.Add("@PROFITCENTER", SqlDbType.VarChar, 20).Value = dd[iCount].PROFITCENTER;
                            SqlCmd.Parameters.Add("@DEBIT", SqlDbType.Decimal).Value = (dd[iCount].DEBIT == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(dd[iCount].DEBIT));
                            SqlCmd.Parameters.Add("@CREDIT", SqlDbType.Decimal).Value = (dd[iCount].CREDIT == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(dd[iCount].CREDIT));
                            SqlCmd.Parameters.Add("@ITEMTEXT", SqlDbType.VarChar, 500).Value = dd[iCount].ITEMTEXT;

                            SqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlCmd.ExecuteNonQuery();

                            i++;
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
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
                    using (SqlCommand sqlComm = new SqlCommand("[dbo].[spGetDataForReverseDoc]", conn))
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

        public object[] UpdateReversedAccount(string docid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlComm = new SqlCommand("[dbo].[]", conn))
                    {
                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.Parameters.Add("@RQRID", SqlDbType.Int).Value = Convert.ToInt32(docid);

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

        public object[] GetAccountingRecorded()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetAccountingRecorded]", sqlConn))
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

        public string InsertReverseAccount(string sapid, string remark, string postingDate, string reasonId, string reasonName, string revSapDocNo, string empid)
        {
            try
            {
                using (SqlConnection SqlCon = new SqlConnection(connStr))
                {
                    if (SqlCon.State == ConnectionState.Closed) SqlCon.Open();
                    using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spInsertReverseAccount]", SqlCon))
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
                    using (SqlCommand SqlCmd = new SqlCommand("[dbo].[spInsertReverseAccountCrossCostCenter]", SqlCon))
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

        public DataTable GetDataDeliveryCostCenterSearch(string docno, string costcenter, string itemno, string startdate, string enddate, int DelvStatus, out string msgerr)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlCon = new SqlConnection(connStr))
                {
                    if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[GetDataDeliveryCostCenterSearch]", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add("@DOCNO", SqlDbType.VarChar).Value = docno;
                        sqlCmd.Parameters.Add("@COSTCENTER", SqlDbType.VarChar).Value = costcenter;
                        sqlCmd.Parameters.Add("@ITEMNO", SqlDbType.VarChar).Value = itemno;
                        sqlCmd.Parameters.Add("@STARTDATE", SqlDbType.VarChar).Value = startdate;
                        sqlCmd.Parameters.Add("@ENDDATE", SqlDbType.VarChar).Value = enddate;
                        sqlCmd.Parameters.Add("@DelvStatus", SqlDbType.Int).Value = DelvStatus;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }

                msgerr = "";
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                msgerr = ex.Message.ToString();
                return new DataTable();
            }
        }

        public string DeliveryAddDetail(int id, string amount, string pstingdate, string empcode, out string msgerr)
        {
            try
            {
                string NewDocNo = "";
                using (SqlConnection sqlCon = new SqlConnection(connStr))
                {
                    if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spInsertDeliveryCostCenterDetail]", sqlCon))
                    {
                        sqlCmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        sqlCmd.Parameters.Add("@AMOUNT", SqlDbType.VarChar, 50).Value = amount;

                        DateTime dt;
                        dt = DateTime.ParseExact(pstingdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        sqlCmd.Parameters.Add("@PSTINGDATE", SqlDbType.VarChar, 10).Value = dt.ToString("yyyyMMdd");

                        sqlCmd.Parameters.Add("@EMPCODE", SqlDbType.VarChar, 10).Value = empcode;
                        sqlCmd.Parameters.Add("@NEWDOCNO", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCmd.Parameters.Add("@MSGERR", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.ExecuteNonQuery();
                        NewDocNo = sqlCmd.Parameters["@NEWDOCNO"].Value.ToString();
                        msgerr = sqlCmd.Parameters["@MSGERR"].Value.ToString();
                    }
                }
                return NewDocNo;
            }
            catch (Exception ex)
            {
                msgerr = ex.Message.ToString();
                return string.Empty;
            }
        }

        public string DeliveryAddSerialDetail(int id, string amount, string pstingdate, string empcode, out string msgerr, out bool isSuccess, out string Msg)
        {
            try
            {
                string NewDocNo = "";
                using (SqlConnection sqlCon = new SqlConnection(connStr))
                {
                    if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spInsertDeliveryCostCenterSerialDetail]", sqlCon))
                    {
                        sqlCmd.Parameters.Add("@DOCID", SqlDbType.Int).Value = id;
                        sqlCmd.Parameters.Add("@AmountOrSerail", SqlDbType.VarChar, 50).Value = amount;

                        DateTime dt;
                        dt = DateTime.ParseExact(pstingdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        sqlCmd.Parameters.Add("@PSTINGDATE", SqlDbType.VarChar, 10).Value = dt.ToString("yyyyMMdd");

                        sqlCmd.Parameters.Add("@EMPCODE", SqlDbType.VarChar, 10).Value = empcode;
                        sqlCmd.Parameters.Add("@NEWDOC", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCmd.Parameters.Add("@SUCCESS", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        sqlCmd.Parameters.Add("@MSGERR", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                        sqlCmd.Parameters.Add("@MSG", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.ExecuteNonQuery();
                        NewDocNo = sqlCmd.Parameters["@NEWDOC"].Value.ToString();
                        isSuccess = Convert.ToBoolean(sqlCmd.Parameters["@SUCCESS"].Value);
                        msgerr = sqlCmd.Parameters["@MSGERR"].Value.ToString();
                        Msg = sqlCmd.Parameters["@MSG"].Value.ToString();
                    }
                }
                return NewDocNo;
            }
            catch (Exception ex)
            {
                Msg = "";
                isSuccess = false;
                msgerr = ex.Message.ToString();
                return string.Empty;
            }
        }

        public bool DeliveryDelDetail(int id, out string msgerr)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connStr))
                {
                    if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spDeleteDeliveryCostCenterDetail]", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        sqlCmd.ExecuteNonQuery();
                    }
                }
                msgerr = "";
                return true;
            }
            catch (Exception ex)
            {
                msgerr = ex.Message.ToString();
                return false;
            }
        }

        public DataTable GetDeliveryCostCenterDetail(int id, out string msgerr)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlCon = new SqlConnection(connStr))
                {
                    if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetDeliveryCostCenterDetail]", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        sqlCmd.Parameters.Add("@MSGERR", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);

                        msgerr = sqlCmd.Parameters["@MSGERR"].Value.ToString();
                    }
                }
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                msgerr = ex.Message.ToString();
                return null;
            }
        }

        public bool CheckingPR(string costCenterNo, string totalPrice, string docType, string docNo, out string msgErr)
        {
            try
            {
                bool isHasPR = false;
                using (SqlConnection sqlCon = new SqlConnection(connStr))
                {
                    if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spCheckingPR]", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add("@COSTCENTERNO", SqlDbType.VarChar, 50).Value = costCenterNo;
                        sqlCmd.Parameters.Add("@TOTALPRICE", SqlDbType.Decimal).Value = Convert.ToDecimal(totalPrice);
                        sqlCmd.Parameters.Add("@DOCTYPE", SqlDbType.VarChar, 20).Value = docType;
                        sqlCmd.Parameters.Add("@DOCNO", SqlDbType.VarChar, 20).Value = docNo;

                        sqlCmd.Parameters.Add("@MSGERR", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                        sqlCmd.ExecuteNonQuery();
                        msgErr = sqlCmd.Parameters["@MSGERR"].Value.ToString();
                        if (msgErr == "") isHasPR = true;
                    }

                }
                return isHasPR;
            }
            catch (Exception ex)
            {
                msgErr = ex.Message.ToString();
                return false;
            }
        }

        public int GetRecQuantity(int id, out string msgErr)
        {
            try
            {
                int quantity = 0;
                using (SqlConnection sqlCon = new SqlConnection(connStr))
                {
                    if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetRecQuantity]", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add("@ID", SqlDbType.Int).Value = Convert.ToInt32(id);

                        sqlCmd.Parameters.Add("@QUANTITY", SqlDbType.Int).Direction = ParameterDirection.Output;
                        sqlCmd.ExecuteNonQuery();

                        quantity = Convert.ToInt32(sqlCmd.Parameters["@QUANTITY"].Value);
                    }
                }
                msgErr = "";
                return quantity;
            }
            catch (Exception ex)
            {
                msgErr = ex.Message.ToString();
                return 0;
            }
        }

        public void EditCostCenter(int docId, string costCenterId, out string msgErr)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connStr))
                {
                    if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spEditCostCenterRequisition]", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add("@DocId", SqlDbType.Int).Value = docId;
                        sqlCmd.Parameters.Add("@CostCenterId", SqlDbType.VarChar, 50).Value = costCenterId;

                        sqlCmd.Parameters.Add("@MSGERR", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                        sqlCmd.ExecuteNonQuery();

                        msgErr = sqlCmd.Parameters["@MSGERR"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                msgErr = ex.Message.ToString();
            }
        }

        public void EditGL(int docId, string GLId, out string msgErr)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connStr))
                {
                    if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spEditGLRequisition]", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add("@DocId", SqlDbType.Int).Value = docId;
                        sqlCmd.Parameters.Add("@GLId", SqlDbType.VarChar, 50).Value = GLId;

                        sqlCmd.Parameters.Add("@MSGERR", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                        sqlCmd.ExecuteNonQuery();

                        msgErr = sqlCmd.Parameters["@MSGERR"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                msgErr = ex.Message.ToString();
            }
        }

        public bool checkIsMarketing(int DeliveryID, ref string msg)
        {
            try
            {
                bool valid = false;

                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("[dbo].[spCheckIsMarketing]", conn);

                    sqlComm.Parameters.Add("@DeliveryID", SqlDbType.Int).Value = DeliveryID;

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);
                }

                if ((ds.Tables[0] ?? new DataTable()).Rows.Count > 0)
                {
                    string resValid = ds.Tables[0].Rows[0][0].ToString();
                    string resMsg = ds.Tables[0].Rows[0][1].ToString();

                    msg = resMsg;
                    if (resValid == "1")
                    {
                        valid = true;
                    }
                    else
                    {
                        valid = false;
                    }
                }
                else
                {
                    valid = false;
                    msg = "spCheckIsMarketing : ไม่พบข้อมูล";
                }

                return valid;
            }
            catch (Exception ex)
            {
                msg = "spCheckIsMarketing : " + ex.ToString();
                return false;
            }
        }

        public string GetInternalOrderByCostCenter(string CostCenter, ref string msg)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlCon = new SqlConnection(connStr))
                {
                    if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spGetInternalOrderByCostCenter]", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add("@CostCenter", SqlDbType.VarChar, 50).Value = CostCenter;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }

                if ((ds.Tables[0] ?? new DataTable()).Rows.Count > 0)
                {
                    msg = "";
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    msg = "GetInternalOrderByCostCenter : ไม่พบข้อมูล ";
                    return ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                msg = "GetInternalOrderByCostCenter : " + ex.ToString();
                return "";
            }
        }


        public class Activated
        {
            public string Status { get; set; }
        }

        public class SaveGroupMapping
        {
            public string userID { get; set; }
            public string costCenter { get; set; }
            public string masterGroupID { get; set; }
        }

        private class ListDataID
        {
            public int MODE { get; set; }
            public string USERNAME { get; set; }
            public string COMP_CODE { get; set; }
            public string DOC_DATE { get; set; }
            public string PSTNG_DATE { get; set; }
            public string FISC_YEAR { get; set; }
            public string DOC_TYPE { get; set; }
            public string REF_DOC_NO { get; set; }
            public string REF_KEY_3 { get; set; }
            public string CURRENCY { get; set; }
            public double AMT_DOCCUR { get; set; }
            public string ID { get; set; }
        }

        private class ListCrossData
        {
            public int MODE { get; set; }
            public string ID { get; set; }
            public string USERNAME { get; set; }
            public string DOC_DATE { get; set; }
            public string PSTNG_DATE { get; set; }
            public string FISC_YEAR { get; set; }
            public string PAYABLE_DOC_TYPE { get; set; }
            public string CUTSTOCK_DOC_TYPE { get; set; }
            public string RECEIVEABLE_DOC_TYPE { get; set; }
            public string REF_DOC_NO { get; set; }
            public string REF_KEY_3 { get; set; }
            public string CURRENCY { get; set; }
            public string BUSINESSPLACE { get; set; }
            public string VENDOR_NO { get; set; }
        }

        private class ListMemo
        {
            public string DocType { get; set; }
            public string DocNo { get; set; }
            public string CreateDate { get; set; }
            public string UsingDate { get; set; }
            public string EndingDate { get; set; }
            public string UserCreateName { get; set; }
            public string CostCenterCode { get; set; }
            public string CostCenterName { get; set; }
            public string Reason { get; set; }
            public string ProjectID { get; set; }
            public string ProjectName { get; set; }
            public string ItemNo { get; set; }
            public string ItemName { get; set; }
            public string PricePerUnit { get; set; }
            public string Quantity { get; set; }
            public string Type { get; set; }
            public string TotalPrice { get; set; }
            public string CreateBy { get; set; }

            public string GLNO { get; set; }
            public string GLName { get; set; }
            public int? ObjID { get; set; }
            public string ObjName { get; set; }
            public int? PromotionID { get; set; }
            public string Promotion { get; set; }

        }

        public class ListGLandObj
        {
            public bool IsCheck { get; set; }
            public bool IsMKT { get; set; }
            public string Message { get; set; }
            public List<GLData> GL { get; set; }
            public List<ObjData> OBJ { get; set; }
            public List<Porjects> PROJ { get; set; }
        }

        public class GLData
        {
            public string GLNO { get; set; }
            public string GLName { get; set; }
        }
        public class ObjData
        {
            public string ObjId { get; set; }
            public string ObjName { get; set; }
        }
        public class Porjects
        {
            public string ProduectId { get; set; }
            public string Project { get; set; }
        }
        #endregion




        public DataTable Get_spRptPurchesingOrder(string StartOrderDate, string EndOrderDate, string DOCNO, string REFNO, string PROJECTID, string @MasterItemID, bool IsAurora)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlCon = new SqlConnection(connStr))
                {
                    if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand("[dbo].[spRptPurchesingOrder_New]", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add("@StartOrderDate", SqlDbType.VarChar, 10).Value = StartOrderDate;
                        sqlCmd.Parameters.Add("@EndOrderDate", SqlDbType.VarChar, 10).Value = EndOrderDate;
                        sqlCmd.Parameters.Add("@DOCNO", SqlDbType.VarChar, 15).Value = DOCNO;
                        sqlCmd.Parameters.Add("@REFNO", SqlDbType.VarChar, 15).Value = REFNO;
                        sqlCmd.Parameters.Add("@PROJECTID", SqlDbType.VarChar, 25).Value = PROJECTID;
                        sqlCmd.Parameters.Add("@MasterItemID", SqlDbType.Int).Value = @MasterItemID ?? "0";
                        sqlCmd.Parameters.Add("@IsAurora", SqlDbType.Bit).Value = IsAurora;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(ds);
                    }
                }

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                string msg = ex.ToString();
                return new DataTable();
            }
        }


        public string TestX(string sqlQuery)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlCon = new SqlConnection(connStr))
                {
                    if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlCon))
                    {                        
                        sqlCmd.ExecuteNonQuery();                        
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

    }
}
