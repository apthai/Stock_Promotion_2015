using System;
using System.Configuration;
using System.Globalization;

namespace Utility
{
    public static class StringExtention
    {

        readonly static string _customFormat = ConfigurationManager.AppSettings["Application.Settings.UI.DateFormat"];
        readonly static string _customCulture = ConfigurationManager.AppSettings["Application.Settings.UI.Culture"];

        public static string DateFormat(this object value)
        {
            return _customFormat;
        }

        public static string JsDateFormat(this object value)
        {
            return _customFormat.ToLower();
        }

        public static string ToDateString(this DateTime value)
        {
            if (value == null || value == DateTime.MinValue)
            {
                return "";
            }

            return value.ToString(_customFormat, new CultureInfo(_customCulture));

        }

        public static string ToTimeString(this DateTime value)
        {

            return value.ToString("HH:mm:ss", new CultureInfo(_customCulture));

        }

        public static string ToTimeString2(this DateTime value)
        {

            return value.ToString("HH:mm", new CultureInfo(_customCulture));

        }

        public static string ToYearString(this DateTime value)
        {

            return value.ToString("yy", new CultureInfo(_customCulture));
        }

        public static string ToDateTimeString(this DateTime value)
        {

            return value.ToString(_customFormat + " HH:mm:ss", new CultureInfo(_customCulture));

        }

        public static string ToDateTimeString(this DateTime? value)
        {
            if (value.HasValue)
                return (value ?? DateTime.Now).ToString(_customFormat + " HH:mm:ss", new CultureInfo(_customCulture));
            else
                return string.Empty;
        }

        public static string ToDateTimeString2(this DateTime value)
        {

            return value.ToString(_customFormat + " HH:mm", new CultureInfo(_customCulture));

        }

        public static string ToDateTimeString2(this DateTime? value)
        {
            if (value.HasValue)
                return (value ?? DateTime.Now).ToString(_customFormat + " HH:mm", new CultureInfo(_customCulture));
            else
                return string.Empty;
        }

        public static string ToCustomDateTime(this DateTime? value, string CustomFormat)
        {
            if (value.HasValue)
                return (value ?? DateTime.Now).ToString(CustomFormat, new CultureInfo(_customCulture));
            else
                return string.Empty;
        }

        public static string ToCustomDateTime(this DateTime value, string CustomFormat)
        {
            return value.ToString(CustomFormat, new CultureInfo(_customCulture));
        }

        public static string ToDateString(this DateTime? value)
        {
            if (value.HasValue)
                return value.Value.ToString(_customFormat, new CultureInfo(_customCulture));
            else
                return string.Empty;
        }

        public static string ToDateString(this DateTime? value, string _customFormat)
        {
            if (value.HasValue)
                return value.Value.ToString(_customFormat, new CultureInfo(_customCulture));
            else
                return string.Empty;

        }

        public static string ToTimeString(this DateTime? value)
        {
            if (value.HasValue)
                return value.Value.ToString("HH:mm:ss", new CultureInfo(_customCulture));
            else
                return string.Empty;

        }

        public static string ToTimeString2(this DateTime? value)
        {
            if (value.HasValue)
                return value.Value.ToString("HH:mm", new CultureInfo(_customCulture));
            else
                return string.Empty;

        }

        public static DateTime ToDate(this string value, bool throwIfInvalidCast = false)
        {
            DateTime returnValue = DateTime.MinValue;
            bool success = false;
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Contains(":"))
                {
                    success = DateTime.TryParseExact(value, _customFormat + " HH:mm:ss", new CultureInfo(_customCulture), DateTimeStyles.AllowWhiteSpaces, out returnValue);//CultureInfo.InvariantCulture
                }
                else
                {
                    success = DateTime.TryParseExact(value, _customFormat, new CultureInfo(_customCulture), DateTimeStyles.AllowWhiteSpaces, out returnValue);//CultureInfo.InvariantCulture
                }
            }

            if (!success && throwIfInvalidCast)
                throw new FormatException("invalid date format");

            return returnValue;
        }

        public static DateTime? ToDateNullable(this string value)
        {

            if (string.IsNullOrEmpty(value))
                return null;
            else
                return value.ToDate();
        }

        public static DateTime ToDate(this string value)
        {
            return Convert.ToDateTime(value);
        }

        public static string ToInt0DigitComma(this int? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("#,##0");

        }

        public static string ToDecimal0DigitComma(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("#,##0");

        }

        public static string ToDecimal0DigitComma(this decimal value)
        {

            return value.ToString("#,##0");

        }

        public static string ToDecimal1DigitComma(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("#,##0.0");

        }

        public static string ToDecimal2DigitComma(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("#,##0.00");

        }

        public static string ToDecimal2DigitComma0(this decimal? value)
        {
            if (!value.HasValue)
                return "0.00";
            else
                return value.Value.ToString("#,##0.00");
        }

        public static string ToDecimal2DigitComma(this decimal value)
        {

            return value.ToString("#,##0.00");

        }

        public static string ToDecimal3DigitComma(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("#,##0.000");

        }

        public static string ToDecimal4DigitComma(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("#,##0.0000");

        }

        public static string ToInt0Digit(this int? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("###0");

        }

        public static string ToDecimal0Digit(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("###0");

        }

        public static string ToDecimal1Digit(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("###0.0");

        }

        public static string ToDecimal2Digit(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("###0.00");

        }

        public static string ToDecimal3Digit(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("###0.000");

        }

        public static string ToDecimal4Digit(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("###0.0000");

        }

        public static decimal? GetStringToDecimal(this string v)
        {
            decimal result = 0;
            decimal.TryParse(v.Replace(",", ""), out result);

            return result;
        }

        public static int? GetStringToInt(this string v)
        {
            double tmp = 0;
            double.TryParse(v.Replace(",", ""), out tmp);

            int result = 0;
            result = Convert.ToInt32(tmp);
            //int.TryParse(tmp, out result);

            return result;
        }

        public static string ToDouble2DigitComma(this double? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("#,##0.00");

        }

        public static string ToDouble2DigitComma(this double value)
        {

            return value.ToString("#,##0.00");

        }

        public static string ToDouble0DigitComma(this double? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("#,##0");

        }

        public static string ToDouble0DigitComma(this double value)
        {

            return value.ToString("#,##0");

        }

        public static string LeadingZero(this int value, int iZero)
        {
            string strValue = value.ToString();

            if (!string.IsNullOrEmpty(strValue))
            {
                string sZero = "";
                for (int i = 0; i + strValue.Length < iZero; i++)
                {
                    sZero += "0";
                }

                strValue = sZero + strValue;
            }

            return strValue;
        }

        public static string LeadingZero(this string value, int iZero)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string sZero = "";
                for (int i = 0; i + value.Length < iZero; i++)
                {
                    sZero += "0";
                }

                value = sZero + value;
            }
            return value;
        }

        public static string LeadingZeroCheckNumeric(this string value, int iZero)
        {
            if (!string.IsNullOrEmpty(value))
            {
                int intValue;
                int.TryParse(value, out intValue);

                if (intValue != 0)
                {
                    string sZero = "";
                    for (int i = 0; i + value.Length < iZero; i++)
                    {
                        sZero += "0";
                    }

                    value = sZero + value;
                }
            }

            return value;
        }

        public static string RemoveLeadingZero(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                long tmpNumber = 0;
                long.TryParse(value, out tmpNumber);

                if (tmpNumber != 0)
                {
                    value = tmpNumber.ToString();
                }
            }

            return value;
        }

        public static string strLeft(this string value, int length)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > length)
                {
                    value = value.Substring(0, length);
                }
            }

            return value;
        }

        public static string strRight(this string value, int length)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > length)
                {
                    value = value.Substring(value.Length - length, length);
                }
            }

            return value;
        }

        public static string DateToStringFormat(this DateTime? value, string format)
        {
            if (!value.HasValue)
                return "";
            else
            {
                var datetime = value ?? DateTime.Now;
                var strYear = datetime.Year.ToString();

                format = format.Replace("yyyy", strYear);
                format = format.Replace("yy", strYear.Substring(strYear.Length - 2, 2));
                var strDate = datetime.ToString(format);

                return strDate;
            }
        }

        public static string DateToStringFormat(this string value, string format)
        {
            if (value == null)
                return "";
            else
            {
                var date = DateTime.ParseExact(value, "yyyy-MM-dd", new CultureInfo(_customCulture));

                var strYear = date.Year.ToString();

                format = format.Replace("yyyy", strYear);
                format = format.Replace("yy", strYear.Substring(strYear.Length - 2, 2));
                var strDate = date.ToString(format);

                return strDate;
            }
        }
    }
}
