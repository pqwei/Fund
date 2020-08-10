using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ObjectExtension
    {
        #region DateTime
        public static string ToTimeSpanStr(this TimeSpan? tsp)
        {
            if (tsp == null) return "00:00";
            TimeSpan ts = tsp.Value;
            string hour = ts.Hours < 10 ? "0" + ts.Hours : ts.Hours.ToString();
            string min = ts.Minutes < 10 ? "0" + ts.Minutes : ts.Minutes.ToString();
            return hour + ":" + min;
        }
        public static string ToShortDateTimeStr(this DateTime? dateTime)
        {
            if (dateTime == null) return "";
            return String.Format("{0:yyyy-MM-dd}", dateTime.Value);
        }
        public static string ToLongDateTimeStr(this DateTime? dateTime)
        {
            return String.Format("{0:yyyy-MM-dd HH:mm}", dateTime);
        }
        public static DateTime ToDateTime(this object obj)
        {
            if (obj == null)
                throw new NullReferenceException();

            DateTime d;
            if (DateTime.TryParse(obj.ToString(), out d))
                return d;
            else
                if (DateTime.TryParseExact(obj.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out d))
                return d;
            else
                return default(DateTime);
        }

        public static DateTime? ToDateTimeNullable(this object obj)
        {
            if (obj == null)
                return null;

            DateTime d;
            if (DateTime.TryParse(obj.ToString(), out d))
                return d;
            else
                if (DateTime.TryParseExact(obj.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out d))
                return d;
            else
                return null;
        }

        #endregion

        #region Int
        public static int ToINT(this object item)
        {
            return ToINT(item, 0);
        }
        public static int ToINT(this object item, int defaultValue)
        {
            if (item != null)
            {
                int.TryParse(item.ToString(), out defaultValue);
            }

            return defaultValue;
        }
        public static int? ToNullableINT(this object item)
        {
            int result = 0;
            if (item != null && int.TryParse(item.ToString(), out result))
            {
                return result;
            }
            return null;
        }
        #endregion

        #region decimal
        public static decimal? ToDecimalNullable(this object obj)
        {
            if (obj == null)
                return null;

            decimal d;
            if (decimal.TryParse(obj.ToString(), out d))
            {
                return d;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 枚举

        /// <summary>
        /// 将枚举（其中的值）转化为string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EnumToValueString<T>(this T value) where T : struct
        {
            Type type = value.GetType();
            if (type.IsEnum)
            {
                FieldInfo fieldInfo = type.GetField("value__");
                if (fieldInfo != null)
                {
                    return string.Format("{0}", fieldInfo.GetValue(value));
                }
            }
            return null;
        }

        #endregion
    }
}
