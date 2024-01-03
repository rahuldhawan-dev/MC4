using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ValveSizeMap : ClassMap<ValveSize>
    {
        public const string SIZE_RANGE_SQL =
            "(CASE WHEN (isNull(Size,0) = 0) THEN 'N/A' WHEN (Size >= 12.0) THEN '>= 12' ELSE '< 12' END)";

        public ValveSizeMap()
        {
            Id(x => x.Id);

            Map(x => x.Size).Not.Nullable();
            Map(x => x.SizeRange).DbSpecificFormula(SIZE_RANGE_SQL, SIZE_RANGE_SQL.ToSqlite());
        }
    }
}
