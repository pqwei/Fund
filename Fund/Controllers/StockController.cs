using Common;
using Common.Base;
using DTO.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fund.Controllers
{
    public class StockController : Controller
    {
        StockService stockService = new StockService();
        // GET: Analyst
        public ActionResult Index()
        {
            var list = stockService.GetStock();
            return View(list);
        }

        public ActionResult AddStock()
        {
            stockService.AddStock();
            return View();
        }


        /// <summary>
        /// 导入数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportData()
        {
            stockService.ImportData();
            return View();
        }

        /// <summary>
        /// 获取日涨跌幅
        /// </summary>
        /// <param name="stockId"></param>
        /// <returns></returns>
        public JsonResult GetDailyChange(int stockId)
        {
            return this.TryCatch(() =>
            {
                AssertUtils.Is(stockId > 0, "证券不存在!");
                var result = stockService.GetDailyChange(stockId);
                return new DataPoint
                {
                    data = result.Select(o => new XYPair<object, object>
                    {
                        x = o.Date.ToShortDateTimeStr(),
                        y = o.Change
                    }).ToList()
                };
            });
        }

        /// <summary>
        /// 获取累计涨跌幅
        /// </summary>
        /// <param name="stockId"></param>
        /// <returns></returns>
        public JsonResult GetCumulativeChange(int stockId, DateTime startDate, DateTime endDate)
        {
            return this.TryCatch(() =>
            {
                AssertUtils.Is(stockId > 0, "证券不存在!");
                var result = stockService.GetCumulativeChange(stockId, startDate, endDate);
                return new DataPoint
                {
                    data = result.Select(o => new XYPair<object, object>
                    {
                        x = o.Date.ToShortDateTimeStr(),
                        y = o.Change
                    }).ToList()
                };
            });
        }

    }
}