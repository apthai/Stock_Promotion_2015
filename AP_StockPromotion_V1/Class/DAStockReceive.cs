using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AP_StockPromotion_V1.Class
{
    public class DAStockReceive
    {

        /* - = Stock Receive  = - */
        private static string connStr = ConfigurationManager.ConnectionStrings["db_APStockPromotion"].ConnectionString;
        private static Entities.FormatDate formatDate = new Entities.FormatDate();

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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
        }

        public DataTable getDataPODraft(string PO_No)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetPODraft", conn);
                    sqlComm.Parameters.AddWithValue("@PO_No", PO_No);
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


        public DataTable getDataItemFromSerial(string Serial)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetDataItemFromSerial", conn);
                    sqlComm.Parameters.AddWithValue("@Serial", Serial);
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


        public DataTable getDataMasterItem(string ItemNo)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetMasterItem", conn);
                    sqlComm.Parameters.AddWithValue("@ItemNo", ItemNo);

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

        public DataTable getDataReceiveHistory(string PO_No)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetReceiveHistory", conn);
                    sqlComm.Parameters.AddWithValue("@PO_No", PO_No);
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


        public bool InsertDataReceive(List<Entities.StockReceiveInfo> receive, ref string msgErr)
        {
            bool rst = true;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                Int64 receiveHID = 0;
                SqlCommand sqlComm = new SqlCommand("dbo.spInsertStockReceiveHeader", conn);
                sqlComm.Parameters.AddWithValue("@PO_No", receive[0].PO_No);
                sqlComm.Parameters.AddWithValue("@GR_No", receive[0].GR_No);
                sqlComm.Parameters.AddWithValue("@GR_Year", receive[0].GR_Year);
                sqlComm.Parameters.AddWithValue("@Vendor", receive[0].Vendor); // ไม่พบในข้อมูล PO
                sqlComm.Parameters.AddWithValue("@ReceiveHeaderStatus", receive[0].ReceiveHeaderStatus);

                sqlComm.Parameters.AddWithValue("@PostingDate", receive[0]._PostingDate);
                sqlComm.Parameters.AddWithValue("@DocDate", receive[0]._DocDate);
                sqlComm.Parameters.AddWithValue("@DocRefNo", receive[0].DocRefNo);

                sqlComm.Parameters.AddWithValue("@CreateBy", receive[0].CreateBy);
                sqlComm.Parameters.AddWithValue("@CreateDate", receive[0]._CreateDate);
                sqlComm.Parameters.AddWithValue("@UpdateBy", receive[0].UpdateBy);
                sqlComm.Parameters.AddWithValue("@UpdateDate", receive[0]._UpdateDate);

                sqlComm.Parameters.AddWithValue("@SAP_EBELN", receive[0].SAP_EBELN);

                sqlComm.Parameters.Add("@ReceiveHeaderId", SqlDbType.BigInt);
                sqlComm.Parameters["@ReceiveHeaderId"].Direction = ParameterDirection.Output;

                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.ExecuteNonQuery();

                receiveHID = (Int64)sqlComm.Parameters["@ReceiveHeaderId"].Value;

                /* - = Receive Detail = - */
                if (receiveHID != 0)
                {
                    using (SqlTransaction tr = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (Entities.StockReceiveInfo r in receive)
                            {
                                sqlComm = new SqlCommand("dbo.spInsertStockReceiveDetail", conn, tr);
                                sqlComm.Parameters.AddWithValue("@ReceiveHeaderId", receiveHID);
                                sqlComm.Parameters.AddWithValue("@MasterItemId", r.MasterItemId);
                                sqlComm.Parameters.AddWithValue("@ItemNo", r.ItemNo);
                                sqlComm.Parameters.AddWithValue("@PricePerUnit", r.PricePerUnit);
                                sqlComm.Parameters.AddWithValue("@ReceiveAmount", r.ReceiveAmount);
                                sqlComm.Parameters.AddWithValue("@Status", r.Status);
                                sqlComm.Parameters.AddWithValue("@UpdateBy", r.UpdateBy);
                                sqlComm.Parameters.AddWithValue("@UpdateDate", r._UpdateDate);

                                sqlComm.Parameters.AddWithValue("@SAP_EBELP", r.SAP_EBELP);
                                sqlComm.Parameters.AddWithValue("@SAP_BSART", r.SAP_BSART);
                                sqlComm.Parameters.AddWithValue("@SAP_BUKRS", r.SAP_BUKRS);
                                sqlComm.Parameters.AddWithValue("@SAP_WERKS", r.SAP_WERKS);
                                sqlComm.Parameters.AddWithValue("@SAP_MATNR", r.SAP_MATNR);
                                sqlComm.Parameters.AddWithValue("@SAP_TXZ01", r.SAP_TXZ01);
                                sqlComm.Parameters.AddWithValue("@SAP_MENGE", r.SAP_MENGE);
                                sqlComm.Parameters.AddWithValue("@SAP_MENGE_A", r.SAP_MENGE_A);
                                sqlComm.Parameters.AddWithValue("@SAP_MEINS", r.SAP_MEINS);
                                sqlComm.Parameters.AddWithValue("@SAP_NETPR", r.SAP_NETPR);
                                sqlComm.Parameters.AddWithValue("@SAP_NETWR", r.SAP_NETWR);
                                sqlComm.Parameters.AddWithValue("@SAP_NAVNW", r.SAP_NAVNW);
                                sqlComm.Parameters.AddWithValue("@SAP_EFFWR", r.SAP_EFFWR);
                                sqlComm.Parameters.AddWithValue("@SAP_WAERS", r.SAP_WAERS);
                                sqlComm.Parameters.AddWithValue("@SAP_BANFN", r.SAP_BANFN);
                                sqlComm.Parameters.AddWithValue("@SAP_BNFPO", r.SAP_BNFPO);
                                sqlComm.Parameters.AddWithValue("@SAP_KOSTL", r.SAP_KOSTL);
                                sqlComm.Parameters.AddWithValue("@SAP_NPLNR", r.SAP_NPLNR);
                                sqlComm.Parameters.AddWithValue("@SAP_PS_PSP_PNR", r.SAP_PS_PSP_PNR);
                                sqlComm.Parameters.AddWithValue("@SAP_WBS_SHOW", r.SAP_WBS_SHOW);

                                sqlComm.Parameters.AddWithValue("@SAP_LIFNR", r.SAP_LIFNR);
                                sqlComm.Parameters.AddWithValue("@SAP_VENDOR_NAME", r.SAP_VENDOR_NAME);
                                sqlComm.Parameters.AddWithValue("@SAP_ZTERM", r.SAP_ZTERM);

                                sqlComm.CommandType = CommandType.StoredProcedure;
                                sqlComm.ExecuteNonQuery();
                            }
                            tr.Commit();
                        }
                        catch (Exception ex)
                        {
                            tr.Rollback();
                            deleteStockReceiveHeader(receiveHID);
                            msgErr = ex.Message;
                            rst = false;
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
                else
                {
                    if (conn.State == ConnectionState.Open) { conn.Open(); }
                    rst = false;
                }
            }
            return rst;
        }


        public bool cancelGoodsReceipt(Entities.StockReceiveInfo receive)
        {
            bool rst = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("dbo.spCancelStockReceiveHeader", conn);
                    sqlComm.Parameters.AddWithValue("@ReceiveHeaderId", receive.ReceiveHeaderId);
                    sqlComm.Parameters.AddWithValue("@PO_No", receive.PO_No);
                    sqlComm.Parameters.AddWithValue("@GR_No", receive.GR_No);
                    sqlComm.Parameters.AddWithValue("@GR_Year", receive.GR_Year);
                    sqlComm.Parameters.AddWithValue("@GRCancel_No", receive.GRCancel_No);
                    sqlComm.Parameters.AddWithValue("@GRCancel_Year", receive.GRCancel_Year);
                    sqlComm.Parameters.AddWithValue("@UpdateBy", receive.UpdateBy);


                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                rst = false;
            }
            return rst;
        }


        public bool deleteStockReceiveHeader(Int64 receiveHeaderId)
        {
            bool rst = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand sqlComm = new SqlCommand("dbo.spDeleteStockReceiveHeader", conn);
                    sqlComm.Parameters.AddWithValue("@ReceiveHeaderId", receiveHeaderId);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                rst = false;
            }
            return rst;
        }

        public DataTable getDataReceiveHistory(Entities.StockReceiveInfo rc)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    DateTime bDate = new DateTime();
                    formatDate.getDateFromString(rc.CreateDateFrom, ref bDate);
                    DateTime eDate = new DateTime();
                    formatDate.getDateFromString(rc.CreateDateTo, ref eDate);

                    SqlCommand sqlComm = new SqlCommand("dbo.spGetReceiveHistory", conn);
                    sqlComm.Parameters.AddWithValue("@PO_No", rc.PO_No);
                    sqlComm.Parameters.AddWithValue("@GR_No", rc.GR_No);
                    sqlComm.Parameters.AddWithValue("@CreateBy", rc.CreateBy);
                    //Entities.FormatDate convertDate = new Entities.FormatDate();
                    //DateTime begDate = new DateTime();
                    //if (convertDate.getDateFromString(rc.CreateDateFrom, ref begDate))
                    //{
                    //    sqlComm.Parameters.AddWithValue("@CreateDateFrom", begDate);
                    //}
                    //DateTime endDate = new DateTime();
                    //if (convertDate.getDateFromString(rc.CreateDateFrom, ref endDate))
                    //{
                    //    sqlComm.Parameters.AddWithValue("@CreateDateTo", endDate);
                    //}
                    sqlComm.Parameters.AddWithValue("@CreateDateFrom", rc._CreateDateFrom);
                    sqlComm.Parameters.AddWithValue("@CreateDateTo", new DateTime(rc._CreateDateTo.Year, rc._CreateDateTo.Month, rc._CreateDateTo.Day, 23, 59, 59));
                    sqlComm.Parameters.AddWithValue("@MasterItemId", rc.MasterItemId);
                    sqlComm.Parameters.AddWithValue("@MasterItemGroupId", rc.MasterItemGroupId);

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









        public DataTable getStockReceiveDetailItem(Int64 ReceiveDetailId)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetStockReceiveDetailItem", conn);
                    sqlComm.Parameters.AddWithValue("@ReceiveDetailId", ReceiveDetailId);

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

        public bool FullFillDataStockReceiveItemDetail(DataTable dt, string updateBy)
        {
            bool bRst = true;

            string receiveDetailStatus = "3"; // ใส่รายละเอียดเสร็จแล้วทั้งหมด
            DataRow[] drwait = dt.Select("ItemStatus = '1'");
            if (drwait.Length > 0)
            {
                receiveDetailStatus = "2"; // ใส่รายละเอียดเสร็จแล้วบางส่วน
            }

            int ReceiveDetailId = (int)dt.Rows[0]["ReceiveDetailId"];

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlTransaction tr = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand sqlComm = new SqlCommand("dbo.spUpdateStatusStockReceiveDetail", conn, tr);
                        sqlComm.Parameters.AddWithValue("@ReceiveDetailId", ReceiveDetailId);
                        sqlComm.Parameters.AddWithValue("@Status", receiveDetailStatus);
                        sqlComm.Parameters.AddWithValue("@UpdateBy", updateBy);
                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.ExecuteNonQuery();

                        DataRow[] drEdit = dt.Select("ItemStatus = '1' or ItemStatus = '1C' or ItemStatus = '2'");
                        foreach (DataRow dr in drEdit)
                        //foreach (DataRow dr in dt.Rows)
                        {
                            sqlComm = new SqlCommand("dbo.spUpdateFullFillDataStockItem", conn, tr);
                            sqlComm.Parameters.AddWithValue("@ItemId", (Int64)dr["ItemId"]);
                            sqlComm.Parameters.AddWithValue("@UpdateBy", updateBy);
#pragma warning disable CS0252 // Possible unintended reference comparison; to get a value comparison, cast the left hand side to type 'string'
                            if (dr["ItemStatus"] == "1C")
#pragma warning restore CS0252 // Possible unintended reference comparison; to get a value comparison, cast the left hand side to type 'string'
                            {
                                sqlComm.Parameters.AddWithValue("@ItemStatus", "2");
                            }
                            else
                            {
                                sqlComm.Parameters.AddWithValue("@ItemStatus", "" + dr["ItemStatus"]);
                            }
                            //sqlComm.Parameters.AddWithValue("@UpdateBy", "" + dr["UpdateBy"]);//	varchar(10) = 'AP00XXXX',
                            //@UpdateDate	datetime
                            sqlComm.Parameters.AddWithValue("@Serial", "" + dr["Serial"]);//	varchar(30),
                            sqlComm.Parameters.AddWithValue("@Barcode", "" + dr["Barcode"]);//	varchar(30) = null,
                            sqlComm.Parameters.AddWithValue("@ItemName", "" + dr["ItemName"]);//	varchar(1000) = null,
                            sqlComm.Parameters.AddWithValue("@Model", "" + dr["Model"]);//	varchar(1000) = null,
                            sqlComm.Parameters.AddWithValue("@Color", "" + dr["Color"]);//	varchar(100) = null,
                            sqlComm.Parameters.AddWithValue("@DimensionWidth", "" + dr["DimensionWidth"]);//	varchar(10) = null,
                            sqlComm.Parameters.AddWithValue("@DimensionLong", "" + dr["DimensionLong"]);//	varchar(10) = null,
                            sqlComm.Parameters.AddWithValue("@DimensionHeight", "" + dr["DimensionHeight"]);//	varchar(10) = null,
                            sqlComm.Parameters.AddWithValue("@DimensionUnit", "" + dr["DimensionUnit"]);//	varchar(20) = null,
                            sqlComm.Parameters.AddWithValue("@Weight", "" + dr["Weight"]);//	varchar(10) = null,
                            sqlComm.Parameters.AddWithValue("@WeightUnit", "" + dr["WeightUnit"]);//	varchar(20) = null,
                            decimal price = 0;
                            if (decimal.TryParse(dr["Price"] + "", out price))
                            {
                                sqlComm.Parameters.AddWithValue("@Price", (decimal)dr["Price"]);//	decimal(15, 4) = null,
                            }
                            DateTime sDate;
                            if (DateTime.TryParse(dr["ProduceDate"] + "", out sDate))
                            {
                                sqlComm.Parameters.AddWithValue("@ProduceDate", (DateTime)dr["ProduceDate"]);//	datetime = null,
                            }
                            DateTime eDate;
                            if (DateTime.TryParse(dr["ExpireDate"] + "", out eDate))
                            {
                                sqlComm.Parameters.AddWithValue("@ExpireDate", (DateTime)dr["ExpireDate"]);//	datetime = null,
                            }
                            sqlComm.Parameters.AddWithValue("@Detail", "" + dr["Detail"]);//	varchar(1500) = null,
                            sqlComm.Parameters.AddWithValue("@Remark", "" + dr["Remark"]);//	varchar(1000) = null

                            sqlComm.CommandType = CommandType.StoredProcedure;
                            sqlComm.ExecuteNonQuery();
                        }
                        tr.Commit();
                    }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                    catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                    {
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














        //public bool InsertDataReceiveX(List<Entities.StockReceiveInfo> receive)
        //{
        //    bool rst = true;
        //    Int64 receiveHeaderId = InsertDataReceiveHeader(receive[0]);
        //    if (receiveHeaderId != 0)
        //    {
        //        foreach (Entities.StockReceiveInfo r in receive)
        //        {
        //            rst = rst & InsertDataReceiveDetail(receiveHeaderId, r);
        //        }
        //    }
        //    else
        //    {
        //        rst = false;
        //    }
        //    return rst;
        //}

        //private Int64 InsertDataReceiveHeader(Entities.StockReceiveInfo receive_H)
        //{
        //    Int64 reqHID = 0;
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connStr))
        //        {
        //            conn.Open();
        //            SqlCommand sqlComm = new SqlCommand("dbo.spInsertStockReceiveHeader", conn);
        //            sqlComm.Parameters.AddWithValue("@PO_No", receive_H.PO_No);
        //            sqlComm.Parameters.AddWithValue("@Vendor", receive_H.Vendor);
        //            sqlComm.Parameters.AddWithValue("@ReceiveHeaderStatus", receive_H.ReceiveHeaderStatus);
        //            sqlComm.Parameters.AddWithValue("@CreateBy", receive_H.CreateBy);
        //            sqlComm.Parameters.AddWithValue("@CreateDate", receive_H.CreateDate);
        //            sqlComm.Parameters.AddWithValue("@UpdateBy", receive_H.UpdateBy);
        //            sqlComm.Parameters.AddWithValue("@UpdateDate", receive_H.UpdateDate);

        //            sqlComm.Parameters.Add("@ReceiveHeaderId", SqlDbType.BigInt);
        //            sqlComm.Parameters["@ReceiveHeaderId"].Direction = ParameterDirection.Output;

        //            sqlComm.CommandType = CommandType.StoredProcedure;
        //            sqlComm.ExecuteNonQuery();

        //            reqHID = (Int64)sqlComm.Parameters["@ReceiveHeaderId"].Value;
        //            conn.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return reqHID;
        //}


        //private bool InsertDataReceiveDetail(Int64 receiveHeaderId,Entities.StockReceiveInfo receive_D)
        //{
        //    bool rst = true;
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connStr))
        //        {
        //            conn.Open();
        //            SqlCommand sqlComm = new SqlCommand("dbo.spInsertStockReceiveDetail", conn);
        //            sqlComm.Parameters.AddWithValue("@ReceiveHeaderId", receiveHeaderId);
        //            sqlComm.Parameters.AddWithValue("@MasterItemId", receive_D.MasterItemId);
        //            sqlComm.Parameters.AddWithValue("@ItemNo", receive_D.ItemNo);
        //            sqlComm.Parameters.AddWithValue("@PricePerUnit", receive_D.PricePerUnit);
        //            sqlComm.Parameters.AddWithValue("@ReceiveAmount", receive_D.ReceiveAmount);
        //            sqlComm.Parameters.AddWithValue("@Status", receive_D.Status);
        //            sqlComm.Parameters.AddWithValue("@UpdateBy", receive_D.UpdateBy);
        //            sqlComm.Parameters.AddWithValue("@UpdateDate", receive_D.UpdateDate);

        //            sqlComm.CommandType = CommandType.StoredProcedure;
        //            sqlComm.ExecuteNonQuery();

        //            conn.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        rst = false;
        //    }
        //    return rst;
        //}


        public string lookTable(DataSet ds)
        {
            string rst = "";
            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    rst += dc.DataType + "\t";
                }
                rst += "\n";
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
    }
}