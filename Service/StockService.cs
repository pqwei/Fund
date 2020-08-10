using Common;
using Common.Base;
using Common.Services;
using DB.Entitys;
using DTO.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class StockService
    {
        StockRepository stockRepository = new StockRepository();
        StockPriceRepository stockPriceRepository = new StockPriceRepository();
        public List<Stock> GetStock()
        {
            return stockRepository.GetStock();
        }

        public void AddStock()
        {
            stockRepository.AddStock(new Stock());
        }

        public void ImportData()
        {
            var dataSet = NPOIHelper.ExcelToDataSet("E:\\Fund\\Library\\浙商基金信息技术部开发工程师作业-人选.xlsx", out string errorMessage);
            var table = dataSet.Tables[1];
            var columnHead = table.Rows[0];
            var heads = columnHead.ItemArray;
            for (int i = 1; i <= 6; i++)
            {
                Stock stock = new Stock
                {
                    Name = heads[i].ToString(),
                    CreateTime = DateTime.Now,
                    IsValid = BusinessConst.ISVALID_TRUR
                };
                stockRepository.AddStock(stock);
            }
            for (int i = 1; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                var array = row.ItemArray;
                var dateTime = array[0].ToDateTimeNullable();
                for (int j = 1; j <= 6; j++)
                {
                    var value = array[j].ToDecimalNullable();
                    StockPrice stockPrice = new StockPrice
                    {
                        FK_Stock = j,
                        Date = dateTime,
                        Price = value,
                        CreateTime = DateTime.Now,
                        IsValid = BusinessConst.ISVALID_TRUR
                    };
                    stockPriceRepository.AddStockPrice(stockPrice);
                }
            }
        }

        public List<StockChange> GetDailyChange(int stockId)
        {
            List<StockChange> stockChanges = new List<StockChange>();
            var stockPrices = stockPriceRepository.GetStockPrices(stockId)?.OrderBy(o => o.Date)?.ToList();
            if (stockPrices != null && stockPrices.Count > 0)
            {
                var last = stockPrices.FirstOrDefault().Price.GetValueOrDefault();
                foreach (var item in stockPrices)
                {
                    var change = Math.Round((item.Price.GetValueOrDefault() - last) / item.Price.GetValueOrDefault(), 4);
                    stockChanges.Add(new StockChange { Date = item.Date, Change = change });
                    last = item.Price.GetValueOrDefault();
                }
            }
            return stockChanges;
        }

        public List<StockChange> GetCumulativeChange(int stockId, DateTime startDate, DateTime endDate)
        {
            List<StockChange> stockChanges = new List<StockChange>();
            var szExponent = GetDailyChange(1)?.Where(o => o.Date > startDate && o.Date <= endDate)?.ToList();//上证日涨跌幅
            var stockExponent = GetDailyChange(stockId).Where(o => o.Date > startDate && o.Date <= endDate)?.ToList();//证券日涨跌幅
            if (stockExponent != null && stockExponent.Count > 0)
            {
                var last = 1.00m;
                stockChanges.Add(new StockChange { Date = startDate, Change = last });
                foreach (var item in stockExponent)
                {
                    var curSZExponent = szExponent.Find(o => o.Date == item.Date);
                    var change = Math.Round((item.Change.GetValueOrDefault() - curSZExponent.Change.GetValueOrDefault() + 1) * last, 2);
                    stockChanges.Add(new StockChange { Date = item.Date, Change = change });
                    last = change;
                }
            }

            return stockChanges;
        }
    }
}
