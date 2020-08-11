using Common.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DB.Entitys
{
    public class StockPrice : EntityBase
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(IsNullable = false)]
        public int FK_Stock { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? Date { get; set; }

        [SugarColumn(IsNullable = true, DecimalDigits = 2)]
        public decimal? Price { get; set; }
    }
}
