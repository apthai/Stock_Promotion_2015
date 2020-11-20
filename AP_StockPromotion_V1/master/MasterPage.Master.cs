using AP_StockPromotion_V1.Class;
using AP_StockPromotion_V1.ws_authorize;
using System;
using System.Data;
using System.Web.UI;
using Newtonsoft.Json;
using System.Web;

namespace AP_StockPromotion_V1.master
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            /////////////////////////////////////////// Change Login Page   /////////////////////////////////////////////////////// 
            //string loginPage = ConfigurationManager.AppSettings["Application.Settings.LoginPage"].ToLower();
            //string CurrentUrl = (Session["CurrentUrl" + Session.SessionID] as string) ?? "";

            //if (string.IsNullOrEmpty(CurrentUrl) )
            //{
            //    return Redirect(loginPage + "" + Url.Action("Index", "CheckPrice", null, Request.Url.Scheme));
            //}
            /////////////////////////////////////////// Change Login Page   ///////////////////////////////////////////////////////



            if (Session["userInfo_" + Session.SessionID] == null)
            {
                if (Session["ByPass"] != null)
                {
                    return;
                }

                if ((Session["firstlogin"] ?? "").ToString() == "Y")
                {
                    Session.Remove("firstlogin");
                    execJScript("firstlogin();");
                    return;
                }

                //IsSessionExpired();

                execJScript("relogin();");

                return;
            }

            AutorizeData auth = new AutorizeData();
            auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
            lbUserId.Text = auth.EmployeeID;
            lbUserName.Text = auth.DisplayName;
            if (Page.IsPostBack == false)
            {
                if (auth.SysUserRoles != null)
                {
                    var results = JsonConvert.DeserializeObject<dynamic>(auth.SysUserRoles);
                    // string RoleCode = "AdminCenter";
                    string RoleCode = "";// results.Roles[0].RoleCode;                 
                    foreach (dynamic roles in results.Roles)
                    {
                        RoleCode += "," + roles.RoleCode;
                    }
                    if (RoleCode.Length >= 1)
                    {
                        RoleCode = RoleCode.Remove(0, 1);
                    }
                    setMenu(RoleCode);
                }
                else
                {
                    if (Session["Bypass"] == null)
                    {
                        Session.RemoveAll();
                    }
                }
            }
        }

        private void setMenu(string RoleCode)
        {
            ltrMenu.Text = "<ul class=\"nav nav-list\">";
            DataTable dt = new DAStockPromotion().getMenuList(RoleCode);
            DataTable dtGrp = dt.DefaultView.ToTable(true, "MenuGroupId", "MenuGroupName", "MenuGroupIcon");

            int id = 0, subId = 0;

            foreach (DataRow drGrp in dtGrp.Rows)
            {
                string tagGrp1 = "";
                string tagGrp2 = "";
                string tagMenu = "";
                if (drGrp["MenuGroupName"] + "" != "")
                {
                    tagGrp1 = "<li class=\"\" id='M" + id + ":" + subId + "'><a href=\"#\" class=\"dropdown-toggle\"><i class=\"" + drGrp["MenuGroupIcon"] + "\"></i><span class=\"menu-text\">" + drGrp["MenuGroupName"] + "</span><b class=\"arrow fa fa-angle-down\"></b></a><b class=\"arrow\"></b><ul class=\"submenu\">";
                    tagGrp2 = "</ul></li>";
                    id++;
                    subId++;
                }
                DataRow[] drMenu = dt.Select("MenuGroupId=" + drGrp["MenuGroupId"]);
                foreach (DataRow dr in drMenu)
                {
                    tagMenu += "<li class=\"\" id='M" + id + "'><a href=\"" + dr["MenuLink"] + "\"><i class=\"" + dr["MenuIcon"] + "\"></i><span class=\"menu-text\">" + dr["MenuName"] + "</span></a></li>";
                    id++;
                }
                ltrMenu.Text += tagGrp1 + tagMenu + tagGrp2;
            }
            ltrMenu.Text += "</ul>";
        }

        private void execJScript(string jscript)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "js", jscript, true);
            return;
        }    
    }
}