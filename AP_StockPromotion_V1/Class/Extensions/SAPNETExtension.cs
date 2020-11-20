using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.apthai.QisWeb.Data.Extensions
{
    public static class SAPNETExtension
    {

        public static string ToSAPDateString(this DateTime dt)
        {

            if (dt > DateTime.MinValue)
                return dt.ToString("yyyyMMdd");
                //return dt.ToUniversalTime().ToString("yyyyMMdd", new System.Globalization.CultureInfo("en-US"));
                //return dt.ToString("yyyyMMdd");
            return string.Empty;

        }

        public static string ToSAPDateString(this DateTime? dt)
        {

            if (dt.HasValue)
                return dt.Value.ToSAPDateString();

            return string.Empty;

        }




    }
}