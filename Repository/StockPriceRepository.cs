using Common;
using Common.Base;
using DB.Entitys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class StockPriceRepository
    {
        SqlSugarClient _db;
        public StockPriceRepository()
        {
            _db = DataBaseConfig.GetInstance();
        }
        public List<StockPrice> GetStockPrices(int stockId)
        {
            var _query = _db.Queryable<StockPrice>().Where(o => o.FK_Stock == stockId && o.IsValid == BusinessConst.ISVALID_TRUR);
            return _query.ToList();
        }

        public void AddStockPrice(StockPrice model)
        {
            _db.CodeFirst.InitTables(typeof(StockPrice));
            _db.Insertable(model).ExecuteCommand();
        }

    }
}
