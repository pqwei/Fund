using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DB.Entitys
{
    public class EntityBase
    {
        /// <summary>
        /// 实体数据是否有效
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 2)]
        public string IsValid { get; set; }

        /// <summary>
        /// 数据创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 数据创建人
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public Nullable<int> CreateUser { get; set; }

        /// <summary>
        /// 数据修改时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 数据修改人
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public Nullable<int> ModifyUser { get; set; }
    }
}
