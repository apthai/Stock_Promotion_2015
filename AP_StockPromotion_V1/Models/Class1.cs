using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AP_StockPromotion_V1.Models
{

    public class UserData
    {
        public User User { get; set; }
    }

    public class User
    {
        public int UserID { get; set; }
        public string UserGUID { get; set; }
        public string UserName { get; set; }
        public string EmpCode { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PositionName { get; set; }
        public string FullCodeName { get; set; }
        public string UserNameLogin { get; set; }
    }

}