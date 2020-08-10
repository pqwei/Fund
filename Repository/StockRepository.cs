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
            _db.CodeFirst.InitTables(typeof(Stock));
            return _db.Queryable<Stock>().ToList();
        }

        public void AddStock()
        {
            //_db.CodeFirst.InitTables(typeof(Stock));
            _db.Insertable(new Stock
            {
                Id = 1,
                Name = "贵州茅台",
                CreateTime=DateTime.Now
            })
            .ExecuteCommand();
        }
    }
}
