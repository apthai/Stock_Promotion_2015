using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AP_StockPromotion_V1.Class
{
    public class DAChangeResponsible
    {
        private static string connStr = ConfigurationManager.ConnectionStrings["db_APStockPromotion"].ConnectionString;
        

        public DataTable getResponsibleByProject(int Project_Id)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {   //  [dbo].[spGetResponsibleByProject]
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetResponsibleByProject", conn);
                    sqlComm.Parameters.AddWithValue("@Project_Id", Project_Id);
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

        public DataTable getItemsResponsibilityByProject(string ResponseUser, int Project_Id)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {   //  [dbo].[spGetResponsibleByProject]
                    SqlCommand sqlComm = new SqlCommand("dbo.getItemsResponsibility", conn);
                    sqlComm.Parameters.AddWithValue("@ResponseUser", ResponseUser);
                    sqlComm.Parameters.AddWithValue("@project_Id", Project_Id);
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

        public DataTable getItemsResponsibilityByProject(int Project_Id, int MasterItemId)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {   //  [dbo].[spGetResponsibleByProject]
                    SqlCommand sqlComm = new SqlCommand("dbo.getProjectResponsibility", conn);
                    sqlComm.Parameters.AddWithValue("@project_Id", Project_Id);
                    sqlComm.Parameters.AddWithValue("@MasterItemId", MasterItemId);
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


        public DataTable getChangeResponsibleListHistory(string Project_Id,string CRFrom,string CRTo,string DateBeg,string DateEnd)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetChangeResponsibleListHistory", conn);
                    int project_id = 0;
                    int.TryParse(Project_Id, out project_id);
                    sqlComm.Parameters.AddWithValue("@Project_Id", project_id);
                    sqlComm.Parameters.AddWithValue("@CRFrom", CRFrom);
                    sqlComm.Parameters.AddWithValue("@CRTo", CRTo);
                    Entities.FormatDate convertDate = new Entities.FormatDate();
                    DateTime begDate = new DateTime();
                    if (convertDate.getDateFromString(DateBeg, ref begDate))
                    {
                        sqlComm.Parameters.AddWithValue("@DateBeg", begDate);
                    }
                    DateTime endDate = new DateTime();
                    if (convertDate.getDateFromString(DateEnd, ref endDate))
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
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getChangeResponsibleDetailHistory(string CRListId)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetChangeResponsibleListHistory", conn);
                    int crListId = 0;
                    if (!int.TryParse(CRListId, out crListId))
                    {
                        crListId = -1;
                    }                    
                    sqlComm.Parameters.AddWithValue("@CRListId", crListId);
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

        public DataTable getChangeResponsibleDetailItem(string CRListId)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand("dbo.spGetChangeResponsibleDetailItem", conn);
                    int crListId = 0;
                    if (!int.TryParse(CRListId, out crListId))
                    {
                        crListId = -1;
                    }
                    sqlComm.Parameters.AddWithValue("@CRListId", crListId);
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

        public bool changeItemResponsibility(string itemList, int Project_Id, string CRFrom, string CRTo, string docRefNo, DateTime docDate, string fileAttch, string fileAttchName, string userId)
        {
            bool rst = true;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlTransaction tr = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand sqlComm = new SqlCommand("dbo.spChangeItemResponsibility", conn, tr);
                        sqlComm.Parameters.AddWithValue("@ItemList", itemList);
                        sqlComm.Parameters.AddWithValue("@Project_Id", Project_Id);
                        sqlComm.Parameters.AddWithValue("@CRFrom ", CRFrom);
                        sqlComm.Parameters.AddWithValue("@CRTo", CRTo);
                        sqlComm.Parameters.AddWithValue("@DocRefNo", docRefNo);
                        sqlComm.Parameters.AddWithValue("@DocDate", docDate);
                        sqlComm.Parameters.AddWithValue("@FileAttch", fileAttch);
                        sqlComm.Parameters.AddWithValue("@FileAttchName", fileAttchName);
                        sqlComm.Parameters.AddWithValue("@User", userId);

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
    }
}