using MMSINC.ClassExtensions.StringExtensions;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Mapping;
using Migration = MapCall.Common.Model.Migrations._2023.MC5763_CreateWorkOrderAssetIdView;

namespace MapCall.Common.Model.Mappings
{
    public class WorkOrderAssetIdViewMap : AbstractAuxiliaryDatabaseObject
    {
        public override string SqlCreateString(
            Dialect dialect,
            IMapping p,
            string defaultCatalog,
            string defaultSchema)
        {
            return Migration.CREATE_VIEW_SQL.ToSqlite();
        }

        public override string SqlDropString(Dialect dialect, string defaultCatalog, string defaultSchema)
        {
            return $"DROP VIEW [{Migration.VIEW_NAME}]";
        }
    }
}
