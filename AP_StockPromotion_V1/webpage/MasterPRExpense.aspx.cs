using AP_StockPromotion_V1.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class MasterPRExpense : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static List<HeaderType> GetTierData()
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                var res = (from t1 in dasp.GetDataTierExpense().AsEnumerable()
                           group t1 by new
                           {
                               PRID = t1.Field<int>("PRID"),
                               EXPTEXT = t1.Field<string>("EXPTEXT"),
                               EXPMINVALUE = t1.Field<int>("EXPMINVALUE"),
                               EXPMAXVALUE = t1.Field<int>("EXPMAXVALUE"),
                               CCID = t1.Field<string>("CCID")
                           } into t
                           select new HeaderType
                           {
                               PRID = t.Key.PRID,
                               EXPTEXT = t.Key.EXPTEXT,
                               EXPMINVALUE = t.Key.EXPMINVALUE,
                               EXPMAXVALUE = t.Key.EXPMAXVALUE,
                               CCID = t.Key.CCID.Replace('|', '-'),
                               CCVALUE = t.Key.CCID,
                               DetailsType = (from t2 in t
                                              select new DetailsType
                                              {
                                                  APVTYPENAME = t2.ItemArray[4].ToString(),
                                                  AUTRTTEXT = t2.ItemArray[5].ToString()
                                              }).ToList()

                           }).ToList();
                return res;
            }
            catch (Exception)
            {
                return new List<HeaderType>();
            }

        }

        [WebMethod]
        public static ListOfData GetListOfData()
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();

                ListOfData lstData = new ListOfData();

                lstData.ExpDesc = (from a1 in dasp.GetListOfData().AsEnumerable()
                                   where Convert.ToInt32(a1[0]) == 1
                                   select new ExpenseDesc
                                   {
                                       ID = Convert.ToInt32(a1[1]),
                                       EXPTEXT = a1[2].ToString(),
                                       EXPMINVALUE = Convert.ToInt32(a1[3]),
                                       EXPMAXVALUE = (a1[4].ToString() == "" ? 0 : Convert.ToInt32(a1[4]))
                                   }).ToList();
                lstData.Auth = (from a2 in dasp.GetListOfData().AsEnumerable()
                                where Convert.ToInt32(a2[0]) == 2
                                select new Authority
                                {
                                    ID = Convert.ToInt32(a2[1]),
                                    AUTRTTEXT = a2[2].ToString()
                                }).ToList();
                lstData.ApvType = (from a3 in dasp.GetListOfData().AsEnumerable()
                                   where Convert.ToInt32(a3[0]) == 3
                                   select new ApproveType
                                   {
                                       ID = Convert.ToInt32(a3[1]),
                                       APVTYPENAME = a3[2].ToString()
                                   }).ToList();

                return lstData;
            }
            catch (Exception)
            {
                return new ListOfData();
            }

        }
        [WebMethod]
        public static Object GetListOfCostCenter()
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();

                ListOfCostCenter lstData = new ListOfCostCenter();

                string sql = "SELECT [CostCenter],[CostCenterName] FROM [dbo].[vw_CostCenter]";

                var item = (from cc in dasp.execDataTable(sql).AsEnumerable()
                                select new CostCenter
                                {
                                    CostCenterID = cc[0].ToString(),
                                    CostCenterName = cc[1].ToString()
                                }
                            );

                lstData.ListCostCenter = item.ToList();

                return new
                {
                    Success = true,
                    Data = lstData
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                    Data = new ListOfCostCenter(),
                    Message = ex.Message
                };
            }
        }

        [WebMethod]
        public static Object SaveData(List<SaveType> data)
        {
            try
            {
                bool resBool = true;
                DAStockPromotion dasp = new DAStockPromotion();
                string Message = "Success";
                int PRID = dasp.GetPRID();

                foreach (var item in data[0].ListType)
                {
                    if (!dasp.SaveTypeMemo(0, PRID, data[0].ExpID, item.ApvTypeID, item.AuthTypeID, data[0].CCID, ref Message)) resBool = false;
                }

                //var res = (from t1 in dasp.GetDataTierExpense().AsEnumerable()
                //           group t1 by new
                //           {
                //               PRID = t1.Field<int>("PRID"),
                //               EXPTEXT = t1.Field<string>("EXPTEXT"),
                //               EXPMINVALUE = t1.Field<int>("EXPMINVALUE"),
                //               EXPMAXVALUE = t1.Field<int>("EXPMAXVALUE"),
                //           } into t
                //           select new HeaderType
                //           {
                //               PRID = t.Key.PRID,
                //               EXPTEXT = t.Key.EXPTEXT,
                //               EXPMINVALUE = t.Key.EXPMINVALUE,
                //               EXPMAXVALUE = t.Key.EXPMAXVALUE,
                //               DetailsType = (from t2 in dasp.GetDataTierExpense().AsEnumerable()
                //                              group t2 by new
                //                              {
                //                                  APVTYPENAME = t2.Field<string>("APVTYPENAME"),
                //                                  AUTRTTEXT = t2.Field<string>("AUTRTTEXT"),
                //                              } into tt
                //                              select new DetailsType
                //                              {
                //                                  APVTYPENAME = tt.Key.APVTYPENAME,
                //                                  AUTRTTEXT = tt.Key.AUTRTTEXT
                //                              }).ToList()

                //           }).ToList();



                return new
                {
                    Success = resBool,
                    Message = Message
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                    Message = ex.Message

                };
            }
        }

        [WebMethod]
        public static Object DeleteData(int id, string CCID)
        {
            try
            {
                string Message = "Success";
                var valid = true;
                DAStockPromotion dasp = new DAStockPromotion();
                valid = dasp.DeleteTypeMemo(id, CCID, ref Message);
                return new
                {
                    Success = valid,
                    Message = Message
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public class ListOfData
        {
            public List<ExpenseDesc> ExpDesc { get; set; }
            public List<Authority> Auth { get; set; }
            public List<ApproveType> ApvType { get; set; }
        }
        public class ExpenseDesc
        {
            private int id;
            private string exptext;
            private int expminvalue;
            private int? expmaxvalue;
            public int ID
            {
                get { return id; }
                set { id = value; }
            }
            public string EXPTEXT
            {
                get { return exptext; }
                set { exptext = value; }
            }
            public int EXPMINVALUE
            {
                get { return expminvalue; }
                set { expminvalue = value; }
            }
            public int? EXPMAXVALUE
            {
                get { return expmaxvalue; }
                set { expmaxvalue = value; }
            }
        }
        public class Authority
        {
            private int id;
            private string autrttext;
            public int ID
            {
                get { return id; }
                set { id = value; }
            }
            public string AUTRTTEXT
            {
                get { return autrttext; }
                set { autrttext = value; }
            }
        }
        public class ApproveType
        {
            private int id;
            private string apvtypename;
            public int ID
            {
                get { return id; }
                set { id = value; }
            }
            public string APVTYPENAME
            {
                get { return apvtypename; }
                set { apvtypename = value; }
            }
        }

        public class SaveType
        {
            public int ExpID { get; set; }
            public int ExpMin { get; set; }
            public int ExpMax { get; set; }
            public string CCID { get; set; }
            public List<ListType> ListType { get; set; }
        }
        public class ListType
        {
            public int ApvTypeID { get; set; }
            public string ApvType { get; set; }
            public int AuthTypeID { get; set; }
            public string AuthType { get; set; }
        }


        public class HeaderType
        {
            public int PRID { get; set; }
            public string EXPTEXT { get; set; }
            public int EXPMINVALUE { get; set; }
            public int EXPMAXVALUE { get; set; }
            public List<DetailsType> DetailsType { get; set; }
            public string CCID { get; set; }
            public string CCVALUE { get; set; }
        }
        public class DetailsType
        {
            public string APVTYPENAME { get; set; }
            public string AUTRTTEXT { get; set; }
        }
    }
}