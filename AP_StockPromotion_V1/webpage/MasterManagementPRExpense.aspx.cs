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
    public partial class MasterManagementPRExpense : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static Object GetListOfCostCenter()
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                var res = (from cc in dasp.GetDistinctCostCenter().AsEnumerable()
                           select new CostCenter
                           {
                               CostCenterID = cc[0].ToString(),
                               CostCenterName = cc[1].ToString()
                           }
                           ).ToList();
                return new
                {
                    Success = true,
                    Data = res,
                    Message = "Success"
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
        public static Object GetTierDataByCostCenter(string CostCenter)
        {
            if (string.IsNullOrEmpty(CostCenter))
            {
                return new
                {
                    Success = true,
                    Data = new List<Expense>(),
                    Message = "Success"
                };
            }
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                var res = (from e in dasp.GetDataTierExpenseByCostCenter(CostCenter).AsEnumerable()
                           select new Expense
                           {
                               ID = e.Field<int>("ID"),
                               EXPTEXT = e.Field<string>("EXPTEXT"),
                               EXPMINVALUE = e.Field<int>("EXPMINVALUE"),
                               EXPMAXVALUE = e.Field<int?>("EXPMAXVALUE")
                           }
                           ).ToList();
                return new
                {
                    Success = true,
                    Data = res,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                    Data = new Expense(),
                    Message = ex.Message
                };
            }
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
                           } into t
                           select new HeaderType
                           {
                               PRID = t.Key.PRID,
                               EXPTEXT = t.Key.EXPTEXT,
                               EXPMINVALUE = t.Key.EXPMINVALUE,
                               EXPMAXVALUE = t.Key.EXPMAXVALUE,
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
        public static ListOfData GetListMemoData(int PRID, string CCID)
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();

                ListOfData lstData = new ListOfData();

                var rawData = dasp.GetListMemoData(PRID, CCID);

                List<GrpData> grpData = (from g in rawData.AsEnumerable()
                                         group g by new
                                         {
                                             RoleCode = g.Field<string>("RoleCode"),
                                             RoleName = g.Field<string>("RoleName"),
                                             AUTRTTEXT = g.Field<string>("AUTRTTEXT"),
                                             PRID = g.Field<int>("PRID")
                                         } into dd
                                         select new GrpData
                                         {
                                             RoleCode = dd.Key.RoleCode,
                                             RoleName = dd.Key.RoleName,
                                             AUTRTTEXT = dd.Key.AUTRTTEXT,
                                             PRID = dd.Key.PRID
                                         }).ToList();


                List<ApproveList> lstApv = new List<ApproveList>();
                List<SignList> lstSign = new List<SignList>();
                foreach (var item in grpData)
                {
                    if (item.AUTRTTEXT == "อนุมัติ")
                    {
                        lstApv = (from apv in rawData.AsEnumerable().Where(q => q.ItemArray[4].ToString() == item.AUTRTTEXT)
                                  select new ApproveList
                                  {
                                      UserID = apv.ItemArray[0].ToString(),
                                      FullName = apv.ItemArray[1].ToString(),
                                      RoleCode = apv.ItemArray[2].ToString(),
                                      RoleName = apv.ItemArray[3].ToString()
                                  }).ToList();
                    }
                    else if (item.AUTRTTEXT == "ลงนามร่วม")
                    {
                        lstSign = (from sqn in rawData.AsEnumerable().Where(q => q.ItemArray[4].ToString() == item.AUTRTTEXT)
                                   select new SignList
                                   {
                                       UserID = sqn.ItemArray[0].ToString(),
                                       FullName = sqn.ItemArray[1].ToString(),
                                       RoleCode = sqn.ItemArray[2].ToString(),
                                       RoleName = sqn.ItemArray[3].ToString()
                                   }).ToList();
                    }
                }

                lstData.ListApv = lstApv;
                lstData.ListSign = lstSign;


                return lstData;
            }
            catch (Exception)
            {
                return new ListOfData();
            }

        }

        [WebMethod]
        public static List<ExpenseDesc> GetExpenseDesc()
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();

                var empDesc = (from t1 in dasp.GetExpenseDesc().AsEnumerable()
                               select new ExpenseDesc
                               {
                                   ID = Convert.ToInt32(t1.ItemArray[0]),
                                   EXPTEXT = t1.ItemArray[1].ToString(),
                                   EXPMINVALUE = Convert.ToInt32(t1.ItemArray[2]),
                                   EXPMAXVALUE = Convert.ToInt32((t1.ItemArray[3].ToString() != "" ? t1.ItemArray[3] : 0))
                               }).ToList();


                return empDesc;
            }
            catch (Exception)
            {
                return new List<ExpenseDesc>();
            }
        }

        [WebMethod]
        public static List<PRMemoData> GetPRMemoData()
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();

                var empDesc = (from t1 in dasp.GetPRMemoData().AsEnumerable()
                               select new PRMemoData
                               {
                                   ID = Convert.ToInt32(t1.ItemArray[0]),
                                   PREXPID = Convert.ToInt32(t1.ItemArray[1]),
                                   EXPTEXT = t1.ItemArray[2].ToString(),
                                   EXPMINVALUE = Convert.ToInt32(t1.ItemArray[3]),
                                   EXPMAXVALUE = Convert.ToInt32((t1.ItemArray[4].ToString() != "" ? t1.ItemArray[4] : 0)),
                                   MainSign = t1.ItemArray[5].ToString(),
                                   SubSign = t1.ItemArray[6].ToString(),
                                   CCID = t1.ItemArray[7].ToString(),
                                   CCTEXT = t1.ItemArray[7].ToString().Replace("|", "-")

                               }).ToList();


                return empDesc;
            }
            catch (Exception)
            {
                return new List<PRMemoData>();
            }
        }

        [WebMethod]
        public static Object SaveData(string expense, int apvID, int signID, string CCID)
        {
            try
            {
                bool resBool = true;
                DAStockPromotion dasp = new DAStockPromotion();
                string Message = "";
                int PRID = dasp.GetPRID();

                if (!dasp.SaveMasterPRExpense(0, Convert.ToInt32(expense.Split(':')[0]), apvID, signID, CCID, ref Message)) resBool = false;

                var res = (from t1 in dasp.GetDataTierExpense().AsEnumerable()
                           group t1 by new
                           {
                               PRID = t1.Field<int>("PRID"),
                               EXPTEXT = t1.Field<string>("EXPTEXT"),
                               EXPMINVALUE = t1.Field<int>("EXPMINVALUE"),
                               EXPMAXVALUE = t1.Field<int>("EXPMAXVALUE"),
                           } into t
                           select new HeaderType
                           {
                               PRID = t.Key.PRID,
                               EXPTEXT = t.Key.EXPTEXT,
                               EXPMINVALUE = t.Key.EXPMINVALUE,
                               EXPMAXVALUE = t.Key.EXPMAXVALUE,
                               DetailsType = (from t2 in dasp.GetDataTierExpense().AsEnumerable()
                                              group t2 by new
                                              {
                                                  APVTYPENAME = t2.Field<string>("APVTYPENAME"),
                                                  AUTRTTEXT = t2.Field<string>("AUTRTTEXT"),
                                              } into tt
                                              select new DetailsType
                                              {
                                                  APVTYPENAME = tt.Key.APVTYPENAME,
                                                  AUTRTTEXT = tt.Key.AUTRTTEXT
                                              }).ToList()

                           }).ToList();



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
        public static bool DeleteData(int id, string CCID)
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                return dasp.DeleteTypeMemoMgr(id, CCID);
            }
            catch (Exception)
            {
                return false;
            }
        }



        public class GrpData
        {
            private string rolecode;
            private string rolename;
            private string autrttext;
            private int prid;

            public string AUTRTTEXT
            {
                get { return autrttext; }
                set { autrttext = value; }
            }
            public string RoleCode
            {
                get { return rolecode; }
                set { rolecode = value; }
            }
            public string RoleName
            {
                get { return rolename; }
                set { rolename = value; }
            }

            public int PRID { get { return prid; } set { prid = value; } }
        }
        public class PRMemoData : ExpenseDesc
        {
            private string mainsign;
            private string subsign;
            private string ccid;
            private string cctext;

            public string MainSign
            {
                get { return mainsign; }
                set { mainsign = value; }
            }
            public string SubSign
            {
                get { return subsign; }
                set { subsign = value; }
            }

            public string CCID
            {
                get { return ccid; }
                set { ccid = value; }
            }

            public string CCTEXT
            {
                get { return cctext; }
                set { cctext = value; }
            }
        }
        public class ListOfData
        {
            public List<ApproveList> ListApv { get; set; }
            public List<SignList> ListSign { get; set; }
        }
        public class ExpenseDesc
        {
            private int id;
            private int prexpid;
            private string exptext;
            private int expminvalue;
            private int? expmaxvalue;
            public int ID
            {
                get { return id; }
                set { id = value; }
            }
            public int PREXPID
            {
                get { return prexpid; }
                set { prexpid = value; }
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

        public class Expense
        {
            public int ID { get; set; }
            public string EXPTEXT
            { get; set; }
            public int EXPMINVALUE
            { get; set; }
            public int? EXPMAXVALUE
            { get; set; }
        }
        public class ApproveList
        {
            public string UserID { get; set; }
            public string FullName { get; set; }
            public string RoleCode { get; set; }
            public string RoleName { get; set; }
        }
        public class SignList : ApproveList
        {

        }
        public class SaveType
        {
            public int ExpID { get; set; }
            public int ExpMin { get; set; }
            public int ExpMax { get; set; }
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
        }

        public class DetailsType
        {
            public string APVTYPENAME { get; set; }
            public string AUTRTTEXT { get; set; }
        }

        protected void btnGetPRFromSAP_Click(object sender, EventArgs e)
        {
            DASAPConnector DASap = new DASAPConnector();
            DARequisition DAReq = new DARequisition();

            DataTable dt = new DataTable();

            dt = DASap.SAP_GET_PRApprover();

            string msg = "";
            if (dt.Rows.Count > 0)
            {
                DAReq.InsertSAP_Approver_PR(dt, ref msg);
            }

            msg = (msg == "") ? "บันทึกเรียบร้อย" : "บันทึกข้อมูลไม่สำเร็จ : " + msg;

            //Response.Write("<script>alert('" + msg + "');</script>");
        }
    }
}