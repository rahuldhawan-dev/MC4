using System;
using System.Data.Common;
using System.Globalization;
using Historian.Data.Client.Entities;

namespace Historian.Data.Client.Mappers
{
    public class RawDataMapper : MapperBase<RawData>, IRawDataMapper
    {
        #region Private Methods

        protected override RawData MapItem(DbDataReader reader)
        {
            return new RawData {
                TagName = reader["Tagname"].ToString(),
                TimeStamp = DateTime.Parse(reader["TimeStamp"].ToString()),
                Value =
                    decimal.Parse(reader["Value"].ToString(),
                        NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint)
            };
        }

        #endregion
    }

    public interface IRawDataMapper : IMapper<RawData> { }
}
