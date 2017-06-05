using BBP.DAL.Interface.ADO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBP.DAL.ADO
{
    internal class MicroORM : IMicroORM
    {

        public virtual IEnumerable<T> Query<T>(IDbConnection cnn, string sql, object param, IDbTransaction transaction, bool buffered, int? commandTimeout, CommandType? commandType)
        {
            return Dapper.SqlMapper.Query<T>(cnn,sql,param,transaction,buffered,commandTimeout,commandType);
        }

        public virtual T QueryFirst<T>(IDbConnection cnn, string sql, object param, IDbTransaction transaction, int? commandTimeout, CommandType? commandType)
        {
            return Dapper.SqlMapper.QueryFirst<T>(cnn, sql, param, transaction,  commandTimeout, commandType);
        }
    }
}
