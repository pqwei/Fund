using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fund.Controllers
{
    public class AnalystController : Controller
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

        
    }
}