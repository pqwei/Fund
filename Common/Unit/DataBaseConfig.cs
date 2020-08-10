﻿using Common.Config;
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
        private static string _connectionWriteStr;
        private static List<string> _connectionReadStr = new List<string>();
        private SqlSugarClient _db;

        /// <summary>
        /// 链接数据库
        /// </summary>
        /// <param name="IsAutoCloseConnection">是否主动关闭数据库连接：设置为true无需使用using或者Close操作，默认false</param>
        /// <param name="OperationDBType">数据库的读写操作选择</param>
        private SqlSugarClient CreateClient(bool IsAutoCloseConnection)
        {
            _connectionWriteStr = "server=120.79.138.193;Database=Analyst;User ID=sa;Password=WPQSQLDLMM;";
            var connStr = _connectionWriteStr;
            if (!string.IsNullOrEmpty(connStr))
            {
                if (connStr.Contains('|'))
                {
                    _connectionReadStr.AddRange(connStr.Split('|'));
                }
                else
                    _connectionReadStr.Add(connStr);
            }

            if (string.IsNullOrEmpty(_connectionWriteStr))
                throw new ArgumentNullException("数据库连接字符串为空");

            List<SlaveConnectionConfig> connectionConfigs = null;
            if (_connectionReadStr != null && _connectionReadStr.Any())
            {
                connectionConfigs = new List<SlaveConnectionConfig>();
                foreach (var item in _connectionReadStr)
                {
                    connectionConfigs.Add(new SlaveConnectionConfig
                    {
                        ConnectionString = item,
                        HitRate = 10
                    });
                }
            }
            //数据库读写分离（自动将写的操作连接到主库，读的操作连接到次库）
           return _db = new SqlSugarClient(new ConnectionConfig()
            {
                //主连接
                ConnectionString = _connectionWriteStr,
                DbType = DbType.MySql,
                IsAutoCloseConnection = IsAutoCloseConnection,
                IsShardSameThread = false,
                //从连接
                SlaveConnectionConfigs = connectionConfigs,
                //如果是非正常的数据类型ORM不支持可以自已添加扩展，其他功能详见官网设置
                ConfigureExternalServices = new ConfigureExternalServices() { },
                //用于一些全局设置
                MoreSettings = new ConnMoreSettings()
                {
                    IsAutoRemoveDataCache = false  //为true表示可以自动删除二级缓存
                }
            });

        }


        /// <summary>
        /// 获取数据连接对象       
        /// </summary>
        public  SqlSugarClient GetClient()
        {
            if (_db==null)
            {
                _db = CreateClient(false);
            }
            return _db;
        }

        public static SqlSugarClient GetInstance()
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
