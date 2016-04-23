﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClass.TerminalUltility
{
    abstract class SQLiteManagerBase
    {
        public SQLiteManagerBase()
        {

        }
        /// <summary>
        /// Execute a simple SQL command
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public abstract int ExecuteSql(string SQLString);
        /// <summary>
        /// Exceute a Query Command
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="TableName"></param>
        /// <returns>DataTable</returns>
        public abstract DataSet Query(string SQLString, string TableName);
        /// <summary>
        /// Query with parameters
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public abstract DataSet Query(string SQLString, params SQLiteParameter[] cmdParms);
        /// <summary>
        /// Make Sqlite parameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract SQLiteParameter MakeSQLiteParameter(string name, DbType type, int size, object value);

    }
}
