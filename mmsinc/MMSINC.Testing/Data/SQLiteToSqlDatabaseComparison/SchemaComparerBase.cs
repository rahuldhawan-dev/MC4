using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison.Issues;

namespace MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison
{
    public abstract class SchemaComparerBase
    {
        #region Private Members

        protected readonly SQLiteConnection _fakeDbConn;
        protected readonly SqlConnection _realDbConn;

        #endregion

        #region Constructors

        public SchemaComparerBase(SQLiteConnection fakeDbConn, SqlConnection realDbConn)
        {
            _fakeDbConn = fakeDbConn;
            _realDbConn = realDbConn;
        }

        #endregion

        #region Private Methods

        protected TRet Query<TCommand, TRet>(string query, DbConnection connection, Func<TCommand, TRet> fn)
            where TCommand : DbCommand, new()
        {
            using (var cmd = new TCommand())
            {
                cmd.CommandText = query;
                cmd.Connection = connection;

                return fn(cmd);
            }
        }

        protected DataTable QueryDataTable<TAdapter, TCommand>(string query, DbConnection connection)
            where TCommand : DbCommand, new()
            where TAdapter : DbDataAdapter, new()
        {
            return Query<TCommand, DataTable>(query, connection, cmd => {
                var ret = new DataTable();
                var adp = new TAdapter {SelectCommand = cmd};
                adp.Fill(ret);
                return ret;
            });
        }

        protected IList<T> QueryList<T, TCommand>(string query, DbConnection connection)
            where TCommand : DbCommand, new()
        {
            return Query<TCommand, IList<T>>(query, connection, cmd => {
                var ret = new List<T>();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ret.Add((T)reader.GetValue(0));
                }

                return ret;
            });
        }

        protected IList<T> SQLiteQueryList<T>(string query)
        {
            return QueryList<T, SQLiteCommand>(query, _fakeDbConn);
        }

        protected DataTable SQLiteQueryDataTable(string query)
        {
            return QueryDataTable<SQLiteDataAdapter, SQLiteCommand>(query, _fakeDbConn);
        }

        protected DataTable SQLQueryDataTable(string query)
        {
            return QueryDataTable<SqlDataAdapter, SqlCommand>(query, _realDbConn);
        }

        protected TValue SQLQueryScalar<TValue>(string query)
        {
            return Query<SqlCommand, TValue>(query, _realDbConn, cmd => (TValue)cmd.ExecuteScalar());
        }

        protected TValue SQLiteQueryScalar<TValue>(string query)
        {
            return Query<SQLiteCommand, TValue>(query, _fakeDbConn, cmd => (TValue)cmd.ExecuteScalar());
        }

        protected void MaybeAddResult(IList<IssueBase> failures, IssueBase result)
        {
            if (result != null)
            {
                failures.Add(result);
            }
        }

        #endregion
    }
}
