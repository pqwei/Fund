using Common.Config;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Base
{
    /// <summary>
    /// 数据库配置节点
    /// </summary>
    public class DataBaseConfig
    {
        private static SqlSugarClient _db;

        public static SqlSugarClient GetInstance()
        {
            if (_db == null)
            {
                _db = CreateInstance();
            }
            return _db;
        }

        private static SqlSugarClient CreateInstance()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "server=120.79.138.193;Database=Analyst;User ID=sa;Password=WPQSQLDLMM;",
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
            //Print sql
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
            return db;
        }
    }
}
