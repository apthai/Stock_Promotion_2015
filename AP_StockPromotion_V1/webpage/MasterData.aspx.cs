using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using AP_StockPromotion_V1.ws_authorize;
using Newtonsoft.Json;
using static AP_StockPromotion_V1.Class.DAStockPromotion;

namespace AP_StockPromotion_V1.webpage
{
    public partial class MasterData : Page
    {

        public static Page _page { get; set; }

        public class UserDetail
        {
            public static string EmployeeID { get; set; }
            public static string FirstName { get; set; }
            public static string LastName { get; set; }
            public static string Email { get; set; }
            public static string Source { get; set; }

        }



        protected void Page_Load(object sender, EventArgs e)
        {
            string[] source = ((Page)sender).ClientQueryString.Split('=');

            AutorizeData auth = new AutorizeData();
            auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
            if (auth == null) return;
            UserDetail.FirstName = auth.FirstName;
            UserDetail.LastName = auth.LastName;
            UserDetail.Email = auth.Email;
            UserDetail.EmployeeID = auth.EmployeeID;
            UserDetail.Source = (source[0] != "" ? source[1].Split('%')[0].ToLower().ToString() : UserDetail.Source);

            initPage(UserDetail.Source);

            _page = this;
        }

        private void initPage(string source)
        {
            //switch (source.ToLower().ToString())
            //{
            //    case "source":
            switch (source)
            {
                case "obj":
                    execScript("initFormObjective('" + source + "');");
                    break;
                case "gl":
                    execScript("initFormGL('" + source + "');");
                    break;
                case "ino":
                    execScript("initFormINO('" + source + "');");
                    break;
                case "right":
                    execScript("initFormAccessRight('" + source + "');");
                    break;
            }
            //        break;

            //}
        }

        private void execScript(string script)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "js", script, true);
            return;
        }

        public static List<ReturnAccessRight> OnCommandRefresh(string source, string startDate = "", string endDate = "")
        {
            try
            {
                Class.DAStockPromotion dasp = new Class.DAStockPromotion();

                if (source != "right")
                {
                    var ret = (from rw in dasp.getDataMasterData(source, startDate, endDate).AsEnumerable()
                               select new ReturnAccessRight()
                               {
                                   USID = rw["USID"].ToString(),
                                   USDES = rw["USDES"].ToString(),
                                   LNGRES = rw["LNGRES"].ToString(),

                                   GLACC = rw["GLACC"].ToString(),
                                   GLDES = rw["GLDES"].ToString(),

                                   InternalOrder = rw["InternalOrder"].ToString(),
                                   Description = rw["Description"].ToString(),

                                   VALIDSTD = (rw["VALIDSTD"].ToString() == "" ? "" : Convert.ToDateTime(rw["VALIDSTD"]).ToShortDateString()),
                                   VALIDEND = (rw["VALIDEND"].ToString() == "" ? "" : Convert.ToDateTime(rw["VALIDEND"]).ToShortDateString()),
                                   CreatedBy = rw["CreatedBy"].ToString(),
                                   CreatedDate = (rw["CreatedDate"].ToString() == "" ? "" : Convert.ToDateTime(rw["CreatedDate"]).ToShortDateString()),
                                   UpdatedBy = rw["UpdatedBy"].ToString(),
                                   UpdatedDate = (rw["UpdatedDate"].ToString() == "" ? "" : Convert.ToDateTime(rw["UpdatedDate"]).ToShortDateString()),


                               }).ToList();
                    return ret;
                }
                else
                {
                    var ret = (from rw in dasp.getDataMasterData(source, startDate, endDate).AsEnumerable()
                               select new ReturnAccessRight()
                               {
                                   ID = rw[0].ToString(),
                                   Status = (rw[1].ToString() == "True" ? "ACTIVE" : "INACTIVE"),
                                   UserName = rw[2].ToString(),
                                   GroupID = rw[3].ToString(),
                                   Group = rw[4].ToString(),
                               }).ToList();

                    return ret;
                }

            }
            catch (Exception)
            {
                return null;
            }
            finally
            {

            }
        }

        public static DataController GetDataInitControl()
        {
            try
            {
                DataController _userCon = new DataController();
                Class.DAStockPromotion dasp = new Class.DAStockPromotion();
                var ret1 = (from t1 in dasp.GetUser().AsEnumerable()
                            select new UserController()
                            {
                                ID = t1[0].ToString(),
                                FName = t1[1].ToString(),
                                LName = t1[2].ToString(),
                                CostCenter = t1[3].ToString(),
                            }).ToList();

                var ret2 = (from t2 in dasp.GetGroup().AsEnumerable()
                            select new GroupController()
                            {
                                ID = t2[0].ToString(),
                                RIGHT = t2[1].ToString(),
                                DESC = t2[2].ToString(),
                            }).ToList();


                _userCon.User = ret1;
                _userCon.Group = ret2;

                return _userCon;

            }
            catch (Exception)
            {
                return null;
            }
            finally
            {

            }
        }

        public class DataController
        {
            private List<UserController> user;
            private List<GroupController> group;
            public List<UserController> User
            {
                get { return user; }
                set { user = value; }
            }
            public List<GroupController> Group
            {
                get { return group; }
                set { group = value; }
            }

        }
        public class UserController
        {
            private string id;
            private string fname;
            private string lname;
            private string costcenter;
            public string ID
            {
                get { return id; }
                set { id = value; }
            }
            public string FName
            {
                get { return fname; }
                set { fname = value; }
            }
            public string LName
            {
                get { return lname; }
                set { lname = value; }
            }
            public string CostCenter
            {
                get { return costcenter; }
                set { costcenter = value; }
            }
        }
        public class GroupController
        {
            private string id;
            private string right;
            private string desc;

            public string ID
            {
                get { return id; }
                set { id = value; }
            }
            public string RIGHT
            {
                get { return right; }
                set { right = value; }
            }
            public string DESC
            {
                get { return desc; }
                set { desc = value; }
            }
        }

        public class ReturnAccessRight : ReturnData
        {
            public virtual string ID { get; set; }
            public virtual string Status { get; set; }
            public virtual string UserName { get; set; }
            public virtual string GroupID { get; set; }
            public virtual string Group { get; set; }
        }
        public class ReturnData
        {
            /* ------- Objective ------- */
            #region "#############################"
            public string USID { get; set; }
            public string USDES { get; set; }
            public string LNGRES { get; set; }
            #endregion

            /* ------- GL ------- */
            #region "######################"
            public string GLACC { get; set; }
            public string GLDES { get; set; }
            #endregion

            /* ------- INO ------- */
            #region "######################"
            public string InternalOrder { get; set; }
            public string Description { get; set; }
            #endregion

            /* ------- Use in 3 tables ------- */
            #region "###################################"
            public string VALIDSTD { get; set; }
            public string VALIDEND { get; set; }
            public string CreatedBy { get; set; }
            public string CreatedDate { get; set; }
            public string UpdatedBy { get; set; }
            public string UpdatedDate { get; set; }
            #endregion
        }

        [WebMethod]
        public static DataController OnCommandInitialControl()
        {
            return GetDataInitControl();
        }

        [WebMethod]
        public static List<ReturnAccessRight> OnCommandLoad(int mode, string startDate, string endDate)
        {

            return OnCommandRefresh(UserDetail.Source, startDate, endDate);
        }

        [WebMethod]
        public static Object OnCommandAdd(string title, string description, string startDate, string endDate, int mode)
        {
            string msgerr = "";
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            if (dasp.insertMasterData(title, description, startDate, endDate, UserDetail.FirstName, UserDetail.Source, out msgerr))
            {
                var Data = OnCommandRefresh(UserDetail.Source);
                return new
                {
                    Success = true,
                    Data = Data
                };
            }

            if (msgerr != "")
            {
                return new
                {
                    Success = false,
                    Message = msgerr
                };
            }
            return new List<ReturnAccessRight>();
        }

        private static void ReturnError(Page page, string msgerr)
        {
            //page.ClientScript.RegisterStartupScript(page.GetType(), 
            //                                        "Alert",
            //                                        "callDoSome",
            //                                        "PageMethods.DoSome(Callback_Function, null)",
            //                                        true);
            page.ClientScript.RegisterStartupScript(page.GetType(),
                                                    "callDoSome",
                                                    "PageMethods.DoSome(Callback_Function, null)",
                                                    true);
            //"AlertError('" + msgerr + "');");
            //ScriptManager.RegisterStartupScript((Page)(HttpContext.Current.Handler), typeof(Page), "js", "AlertError('" + msgerr + "');", true);
        }

        [WebMethod]
        public static Object OnCommandEdit(int id, string title, string description, string startDate, string endDate, int mode)
        {
            string Message = "Success";
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            if (dasp.editMasterData(id, title, description, startDate, endDate, UserDetail.FirstName, UserDetail.Source, ref Message))
            {
                return new
                {
                    Success = true,
                    Message = Message,
                    Data = OnCommandRefresh(UserDetail.Source)
                };
            }
            return new
            {
                Success = false,
                Message = Message,
                Data = new List<ReturnAccessRight>()
            };
        }

        [WebMethod]
        public static List<ReturnAccessRight> OnCommandDelete(int id, string title)
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            if (dasp.deleteMasterData(id, title, UserDetail.Source))
            {
                return OnCommandRefresh(UserDetail.Source);
            }
            return new List<ReturnAccessRight>();
        }

        [WebMethod]
        public static List<ReturnAccessRight> OnCommandSaveMapping(List<SaveGroupMapping> saveDataMapping)
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            if (dasp.saveMapping(saveDataMapping, UserDetail.FirstName))
            {
                return OnCommandRefresh(UserDetail.Source);
            }
            return new List<ReturnAccessRight>();
        }

        [WebMethod]
        public static bool OnCommandEditGroupMapping(int ID, int RIGHT)
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            return dasp.editMapping(ID, RIGHT, UserDetail.FirstName);
        }

        [WebMethod]
        public static List<Activated> OnCommandEditingActive(int ID)
        {

            Class.DAStockPromotion dasp = new Class.DAStockPromotion();

            var ret = (from rw in dasp.activated(ID, UserDetail.FirstName).AsEnumerable()
                       select new Activated()
                       {
                           Status = (rw["Active"].ToString() == "True" ? "ACTIVE" : "INACTIVE"),
                       }).ToList();


            return ret; //dasp.activated(ID, UserDetail.FirstName);
        }
    }
}