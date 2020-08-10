using System;
using System.Web.Mvc;

namespace Common
{
    public static class ControllerEx
    {
        /// <summary>
        /// Action的TryCatch辅助方法
        /// </summary>
        /// <param name="controller">controller</param>
        /// <param name="func">执行内容</param>
        /// <returns>ActionResult</returns>
        public static JsonResult TryCatch(this Controller controller, Func<object> func)
        {
            try
            {
                dynamic result = func();
                if (result != null && result.GetType() == typeof(ResultEntity))
                {
                    return new JsonResult
                    {
                        Data = new { code = result.Code, msg = result.Message, value = result.Value },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    return new JsonResult
                    {
                        Data = new { code = 200, msg = "Success", value = result },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }
            catch (ApplicationException ex)
            {
                //Logging.Logger.Error(ex);
                //Logging.Logger.Information(ex.StackTrace.ToString());
                return new JsonResult
                {
                    Data = new { code = 500, msg = ex.Message, value = string.Empty },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch (Exception ex)
            {
                //Logging.Logger.Error(ex);
                //Logging.Logger.Information(ex.StackTrace.ToString());
                return new JsonResult
                {
                    Data = new { code = 500, msg = "程序内部错误", value = ex.StackTrace },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
    }
    public class ResultEntity
    {
        public int Code { get; set; } = 200;
        public object Value { get; set; } = string.Empty;
        public string Message { get; set; } = "Success";
    }
}