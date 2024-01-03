using MapCall.Common.Model.Migrations;
using MMSINC.ClassExtensions.StringExtensions;
using NHibernate.Dialect;
using NHibernate.Mapping;
using NHibernate.Engine;

namespace MapCall.Common.Model.Mappings
{
    // THIS IS MAPPED TO A VIEW/CTE
    public class MostRecentArcFlashStudyViewMap : AbstractAuxiliaryDatabaseObject
    {
        public struct Sql
        {
            public const string CREATE_VIEW_FORMAT = MC1522SeparateOutArcFlashDataFromFacilities.CREATE_VIEW_SQLITE;
            public const string DROP_VIEW = MC1522SeparateOutArcFlashDataFromFacilities.DROP_VIEW;
        }

        public override string SqlCreateString(Dialect dialect, IMapping p, string defaultCatalog, string defaultSchema)
        {
            return Sql.CREATE_VIEW_FORMAT;
        }

        public override string SqlDropString(Dialect dialect, string defaultCatalog, string defaultSchema)
        {
            return Sql.DROP_VIEW;
        }
    }
}
