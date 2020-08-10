using Common.Base;
using Common.Repository;
using DB.Entitys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class StockRepository
    {
        SqlSugarClient _db;
        public StockRepository()
        {
            _db = DataBaseConfig.GetInstance();
        }

        public List<Stock> GetStock()
        {
            return _db.Queryable<Stock>().ToList();
        }

        public void AddStock(Stock stock)
        {
            _db.CodeFirst.InitTables(typeof(Stock));
            _db.Insertable(stock).ExecuteCommand();
        }
    }
}
