using Common.Base;
using SqlSugar;
using System;

namespace DB.Entitys
{
    public class Stock : EntityBase
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(IsNullable = true, Length = 200)]
        public string Name { get; set; }
    }
}