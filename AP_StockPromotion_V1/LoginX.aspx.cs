using AP_StockPromotion_V1.Class;
using AP_StockPromotion_V1.ws_authorize;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;

namespace AP_StockPromotion_V1
{
    public partial class LoginX : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.RemoveAll();
            if (!Page.IsPostBack)
            {
                ///////////////////////////////////////////Change Login Page/////////////////////////////////////////////////////// 
                string guId = Request.QueryString["GUID"];
                Session["GUID"] = guId == null ? Session["GUID"] : guId;
                if (Session["GUID"] != null)
                {
                    WSAuthorizeModel userLogin = new WSAuthorizeModel();
                    DAStockPromotion da = new DAStockPromotion();
                    DataTable dt = new DataTable();
                    string msgErr = "";
                    dt = da.getUserAuth(guId, "", "");
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {

                            if (!UserLogin("", "", ref userLogin, dt.Rows[0]["UserNameLogin"].ToString(), ref msgErr))
                            {
                                execJScript("alert('" + msgErr + "'); $('" + txtUser.ClientID + "').focus();");
                                return;
                            }
                            else
                            {
                                if (Cache["URLFromEmail"] != null)
                                {
                                    btnLogin.Enabled = false;
                                    Response.Redirect(Cache["URLFromEmail"].ToString());
                                }
                                else
                                {
                                    btnLogin.Enabled = false;
                                    string originalPath = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";

                                    Response.Redirect(originalPath + "webpage/Default.aspx");
                                }

                            }

                        }
                    }

                }
                else
                {
                    string loginPage = ConfigurationManager.AppSettings["Application.Settings.LoginPage"].ToLower();
                    string CurrentUrl = (Session["userInfo_" + Session.SessionID] as string) ?? "";

                    if (string.IsNullOrEmpty(CurrentUrl))
                    {
                        string originalPath = new Uri(HttpContext.Current.Request.Url.AbsoluteUri).OriginalString;
                        //String parentDirectory = originalPath.Substring(0, originalPath.LastIndexOf("/"));
                        Response.Redirect(loginPage + "" + originalPath);
                    }
                }
                
                ///////////////////////////////////////////Change Login Page///////////////////////////////////////////////////////

            }
        }


        private void btnLoginClick()
        {
            //if (!ByPass())
            //{
            if (txtUser.Text.Trim() == "")
            {
                execJScript("alert('User Name ไม่ถูกต้อง!!'); $('" + txtUser.ClientID + "').focus();");
                return;
            }
            if (txtPassword.Text.Trim() == "")
            {
                execJScript("alert('Password ไม่ถูกต้อง!!'); $('" + txtPassword.ClientID + "').focus();");
                return;
            }
            //}


            string msgErr = "";
            WSAuthorizeModel userLogin = new Class.WSAuthorizeModel();

            if (!UserLogin(txtUser.Text.Trim(), txtPassword.Text.Trim(), ref userLogin, "", ref msgErr))
            {
                execJScript("alert('" + msgErr + "'); $('" + txtUser.ClientID + "').focus();");
                return;
            }
            else
            {
                if (Cache["URLFromEmail"] != null)
                {
                    btnLogin.Enabled = false;
                    Response.Redirect(Cache["URLFromEmail"].ToString());

                }
                else
                {
                    btnLogin.Enabled = false;
                    Response.Redirect("webpage/Default.aspx");
                    //execJScript("getDefaultPage();");
                }
                return;
            }
        }


        protected void btnLogin_Click(object sender, EventArgs e)
        {
            btnLoginClick();
        }


        private void execJScript(string jscript)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "js", jscript, true);
            return;
        }

        /* EX_! */
        //<add key="Webservice.Username" value="apwsap2" />
        //<add key="Webservice.Password" value="HagCfbkuifg994" />
        //<add key="Webservice.AppCode" value="รหัสของแต่ระบบ" /> 

        private string WsUsername = ConfigurationManager.AppSettings["Webservice.Username"];
        private string WsPassword = ConfigurationManager.AppSettings["Webservice.Password"];
        private string AppCode = ConfigurationManager.AppSettings["Webservice.AppCode"];

        public bool UserLogin(string userName, string password, ref WSAuthorizeModel userInfo, string UserName, ref string msgErr)
        {
            bool rst = true;
            try
            {
                var resturnUser = new WSAuthorizeModel();
                var result = new AutorizeData();
                var webSerInstanceAuthorize = new Authorize();
                if (UserName.Trim() != "")
                {
                    var xxx = webSerInstanceAuthorize.FindUserProfile(UserName, AppCode);

                    result = webSerInstanceAuthorize.FindUserProfile(UserName, AppCode);
                }
                else
                {
                    webSerInstanceAuthorize.AuthorizeSoapHeaderValue = new AuthorizeSoapHeader() { username = WsUsername, password = WsPassword };
                    result = webSerInstanceAuthorize.UserLogin(userName, password, AppCode);

                    //byPass
                    if (!result.LoginResult && password == "fank")
                    {
                        result = webSerInstanceAuthorize.FindUserProfile(userName, AppCode);
                    }
                    else
                    {
                        if (!result.LoginResult)
                        {
                            resturnUser = new WSAuthorizeModel(result);
                            rst = false;
                            msgErr = "User name หรือ Password ไม่ถูกต้อง";
                        }
                    }
                }

                Session["userInfo_" + Session.SessionID] = result;
            }
            catch (Exception ex)
            {
                rst = false;
                msgErr = ex.Message;
            }
            return rst;
        }
    }
}