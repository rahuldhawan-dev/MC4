using MapCall.Common.Model.Migrations._2019;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Mapping;

namespace MapCall.Common.Model.Mappings
{
    public class ProductionWorkOrderRequiresSupervisorApprovalViewMap : AbstractAuxiliaryDatabaseObject
    {
        public struct Sql
        {
            public const string CREATE_VIEW_FORMAT = MC1220AddView.CREATE_VIEW;
            public const string DROP_VIEW = MC1220AddView.DROP_VIEW;
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
