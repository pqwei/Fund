using Common.Services;
using DB.Entitys;
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
        public List<Stock> GetStock()
        {
            return stockRepository.GetStock();
        }

        public void AddStock()
        {
            stockRepository.AddStock();
        }
    }
}
