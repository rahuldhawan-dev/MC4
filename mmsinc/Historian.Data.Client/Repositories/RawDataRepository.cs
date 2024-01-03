using System;
using System.Collections.Generic;
using System.Text;
using Historian.Data.Client.Entities;
using Historian.Data.Client.Mappers;

namespace Historian.Data.Client.Repositories
{
    public class RawDataRepository : RepositoryBase<RawData, IRawDataMapper>, IRawDataRepository
    {
        #region Constructors

        public RawDataRepository(IRawDataMapper mapper) : base(mapper) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<RawData> FindByTagName(string tagName, bool useRaw, DateTime? startDate, DateTime? endDate)
        {
            var query = new QueryBuilder(tagName)
                       .UseRaw(useRaw)
                       .WithStartDate(startDate)
                       .WithEndDate(endDate);
            return Find(query.Build());
        }

        #endregion

        #region Nested Type: QueryBuilder

        private class QueryBuilder
        {
            #region Constants

            private const string OUTER = "SELECT * FROM OPENQUERY(FACILITY_CONNEX, '{0}')",
                                 BASE =
                                     "SELECT Tagname, TimeStamp, Value FROM ihRawData WHERE Tagname = ''{0}'' AND TimeZone=''Server''";

            #endregion

            #region Private Members

            private readonly string _tagName;
            private bool _useRaw;
            private DateTime? _startDate, _endDate;

            #endregion

            #region Constructors

            public QueryBuilder(string tagName)
            {
                _tagName = tagName;
            }

            #endregion

            #region Exposed Methods

            public QueryBuilder UseRaw(bool useRaw)
            {
                _useRaw = useRaw;
                return this;
            }

            public QueryBuilder WithStartDate(DateTime? startDate)
            {
                _startDate = startDate;
                return this;
            }

            public QueryBuilder WithEndDate(DateTime? endDate)
            {
                _endDate = endDate;
                return this;
            }

            public string Build()
            {
                var samplingMode = _useRaw ? "RawByTime" : "Interpolated";
                var sb = new StringBuilder($"set samplingmode={samplingMode};");

                sb.AppendFormat(BASE, _tagName);

                if (_startDate.HasValue)
                {
                    sb.AppendFormat(" AND timestamp >= ''{0}''", _startDate);
                }

                if (_endDate.HasValue)
                {
                    sb.AppendFormat(" AND timestamp <= ''{0}''", _endDate);
                }

                return string.Format(OUTER, sb);
            }

            public override string ToString()
            {
                return Build();
            }

            public static explicit operator string(QueryBuilder builder)
            {
                return builder.ToString();
            }

            #endregion
        }

        #endregion
    }

    public interface IRawDataRepository
    {
        #region Abstract Methods

        IEnumerable<RawData> FindByTagName(string tagName, bool useRaw, DateTime? startDate, DateTime? endDate);

        #endregion
    }
}
