using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 断言工具类
    /// </summary>
    public class AssertUtils
    {
        // Methods
        public AssertUtils()
        {
        }
        /// <summary>
        /// 检查两个值是否相等 如果不相等，抛出一个ApplicationException异常 
        /// </summary>
        /// <param name="left">一个值</param>
        /// <param name="right">另一个值</param>
        /// <param name="errorMsg">错误信息</param>
        /// <param name="args">参数，用于构造错误信息</param>
        public static void Equals(object left, object right, string errorMsg, params object[] args)
        {
            Is((left == null) ? (right == null) : left.Equals(right), errorMsg, args);
        }
        /// <summary>
        /// 检查文件是否存在于文件系统 如果文件不存在，抛出一个ApplicationException异常
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static void ExistFile(string filePath)
        {
            object[] args = new object[] { filePath };
            Is(File.Exists(filePath), "找不到文件[{0}]", args);
        }
        /// <summary>
        /// 检查文件夹是否存在于文件系统 如果文件夹不存在，抛出一个ApplicationException异常 
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        public static void ExistFolder(string folderPath)
        {
            object[] args = new object[] { folderPath };
            Is(Directory.Exists(folderPath), "找不到文件夹[{0}]", args);
        }

        /// <summary>
        /// 格式化错误信息，相当于String.Format 
        /// </summary>
        /// <param name="errorMsg">错误信息</param>
        /// <param name="args">参数，用于构造错误信息</param>
        /// <returns></returns>
        public static string FormatString(string errorMsg, params object[] args)
        {
            if ((args != null) && (args.Length != 0))
            {
                return string.Format(errorMsg, args);
            }
            return errorMsg;
        }
        /// <summary>
        /// 检查一个字符串是否有内容（包含任意非空白字符） 如果没有内容，抛出一个ApplicationException异常
        /// </summary>
        /// <param name="value">待检查的一个字符串</param>
        /// <param name="errorMsg">错误信息</param>
        /// <param name="args">参数，用于构造错误信息</param>
        public static void HasContent(string value, string errorMsg, params object[] args)
        {
            Is(!string.IsNullOrWhiteSpace(value), errorMsg, args);
        }
        /// <summary>
        /// 检查一个值为true 如果是false，抛出一个ApplicationException异常 
        /// </summary>
        /// <param name="value">指定值</param>
        /// <param name="errorMsg">错误信息</param>
        /// <param name="args">参数，用于构造错误信息</param>
        public static void Is(bool value, string errorMsg, params object[] args)
        {
            if (!value)
            {
                Throw(errorMsg, args);
            }
        }
        /// <summary>
        /// 检查一个字符串是否为url（以http://或https://开头，不区分大小写） 如果不是url，抛出一个ApplicationException异常 
        /// </summary>
        /// <param name="url">待检查的字符串</param>
        /// <param name="errorMsg">错误信息</param>
        /// <param name="args">参数，用于构造错误信息</param>
        public static void IsUrl(string url, string errorMsg, params object[] args)
        {
            Is(url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) || url.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase), errorMsg, args);
        }
        /// <summary>
        /// 检查一个值为false 如果是true，抛出一个ApplicationException异常
        /// </summary>
        /// <param name="value">指定值</param>
        /// <param name="errorMsg">错误信息</param>
        /// <param name="args">参数，用于构造错误信息</param>
        public static void Not(bool value, string errorMsg, params object[] args)
        {
            Is(!value, errorMsg, args);
        }
        /// <summary>
        /// 检查一个值不为null 如果是null，抛出一个ApplicationException异常 
        /// </summary>
        /// <param name="value">待检查的值</param>
        /// <param name="errorMsg">错误信息</param>
        /// <param name="args">参数，用于构造错误信息</param>
        public static void NotNull(object value, string errorMsg, params object[] args)
        {
            Is(value != null, errorMsg, args);
        }
        /// <summary>
        /// 直接抛出一个ApplicationException异常
        /// </summary>
        /// <param name="errorMsg">错误信息</param>
        /// <param name="args">参数，用于构造错误信息</param>
        public static void Throw(string errorMsg, params object[] args)
        {
            throw new ApplicationException(FormatString(errorMsg, args));
        }
    }
}
