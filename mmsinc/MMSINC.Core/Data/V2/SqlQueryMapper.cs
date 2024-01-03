using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate;

namespace MMSINC.Data.V2
{
    public class SqlQueryWrapper : ISqlQuery
    {
        #region Private Members

        protected readonly ISQLQuery _innerQuery;

        #endregion

        #region Constructors

        public SqlQueryWrapper(ISQLQuery innerQuery)
        {
            _innerQuery = innerQuery;
        }

        #endregion

        #region Exposed Methods

        public int ExecuteUpdate()
        {
            return _innerQuery.ExecuteUpdate();
        }

        public T UniqueResult<T>()
        {
            return _innerQuery.UniqueResult<T>();
        }

        /// <summary>
        /// Where a single integer value is expected, either returns null or an integer value.
        /// If the value cannot be cast to int or long, a NotImplementedException is thrown.
        /// </summary>
        public int? SafeUniqueIntResult()
        {
            var result = _innerQuery.UniqueResult<object>();

            switch (result)
            {
                case null:
                    return null;
                case int i:
                    return i;
                case long l:
                    return (int)l;
            }

            throw new NotImplementedException($"Not sure how to handle result type {result.GetType().Name}.");
        }

        public IEnumerable<T> Enumerable<T>()
        {
            return _innerQuery.Enumerable<T>();
        }

        public ISqlQuery SetString(string name, string value)
        {
            _innerQuery.SetString(name, value);
            return this;
        }

        public ISqlQuery SetInt32(string name, int value)
        {
            _innerQuery.SetInt32(name, value);
            return this;
        }

        public ISqlQuery SetParameterList(string name, IEnumerable vals)
        {
            _innerQuery.SetParameterList(name, vals);
            return this;
        }

        public ISqlQuery AddScalar(string columnAlias, Type type)
        {
            _innerQuery.AddScalar(columnAlias, global::NHibernate.Type.TypeFactory.GetSerializableType(type));
            return this;
        }

        #endregion
    }
}
