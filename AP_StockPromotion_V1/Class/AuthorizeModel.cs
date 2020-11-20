

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AP_StockPromotion_V1.Class
{
    public class AuthenRole
    {
        public int RoleID { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
    }

    public partial class Role
    {
        public virtual int UserID { get; set; }
        public virtual string UserGUID { get; set; }
        public virtual string UserName { get; set; }
        public virtual string EmpCode { get; set; }
        public virtual string TitleName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string FullName { get; set; }
        public virtual string Email { get; set; }
        public virtual string PositionName { get; set; }
        public virtual int RoleID { get; set; }
        public virtual string RoleCode { get; set; }
        public virtual string RoleName { get; set; }
    }

    public class AutorizeRoles
    {
        public string UserId { get; set; }
        public List<Role> Roles { get; set; }
    }

    public class WSAuthorizeModel
    {

        public WSAuthorizeModel()
        {
            this.AutorizeData = new ws_authorize.AutorizeData();
        }

        public WSAuthorizeModel(ws_authorize.AutorizeData wsLoginData)
        {
            this.AutorizeData = wsLoginData;
        }
        //public WSAuthorizeModel()
        //{
        //    this.AutorizeData = new WS.AutorizeData();
        //}

        //public WSAuthorizeModel(WS.AutorizeData wsLoginData)
        //{
        //    this.AutorizeData = wsLoginData;
        //}

        public int UserID { get { int value = 0; int.TryParse(AutorizeData.SysUserId, out value); return value; } }
        public string FirstName { get { return AutorizeData.FirstName; } }
        public string LastName { get { return AutorizeData.LastName; } }
        public string DisplayName { get { return AutorizeData.DisplayName; } }
        public string EmployeeID { get { return AutorizeData.EmployeeID; } }
        public string Email { get { return AutorizeData.Email; } }


        public virtual List<string> DivisionList
        {
            get
            {

                List<string> divisions = (this.AutorizeData.Division ?? ",,,,").Split(',').ToList();
                return divisions;

            }
        }
        public virtual string Position
        {
            get
            {
                return DivisionList.FirstOrDefault();
            }
        }
        public virtual string Department
        {
            get
            {
                return DivisionList.Skip(1).FirstOrDefault();
            }
        }

        public virtual string Division
        {
            get
            {
                return DivisionList.Skip(2).FirstOrDefault();
            }
        }

        public virtual string Company
        {
            get
            {
                return DivisionList.Skip(3).FirstOrDefault();
            }
        }

        public virtual List<AuthenRole> Roles
        {

            get
            {

                var sysUserRoles = JObject.Parse(this.AutorizeData.SysUserRoles);
                var RolesJObject = sysUserRoles.SelectToken("Roles");
                var Roles = JsonConvert.DeserializeObject<List<AuthenRole>>(RolesJObject.ToString());
                return Roles;

            }
        }

        public virtual Newtonsoft.Json.Linq.JObject UserData
        {
            get
            {
                return Newtonsoft.Json.Linq.JObject.Parse(AutorizeData.SysUserData);
            }
        }
        
        private ws_authorize.AutorizeData AutorizeData { get; set; }
        //private WS.AutorizeData AutorizeData { get; set; }


    }


}