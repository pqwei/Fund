using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Config
{
    public class ConfigurationExt
    {
        public static T GetAppSettingValue<T>(string appKey, Converter<string, T> convert, T defaultValue)
        {
            string appValue = System.Configuration.ConfigurationManager.AppSettings[appKey];
            return null == appValue ? defaultValue : convert(appValue);
        }

        public static string GetAppSettingValue(string appKey, string defaultValue)
        {
            string appValue = System.Configuration.ConfigurationManager.AppSettings[appKey];
            return null == appValue ? defaultValue : appValue;
        }
    }
}
