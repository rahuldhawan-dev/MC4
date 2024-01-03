using MapCall.Common.Model.Migrations._2023;
using MMSINC.ClassExtensions.StringExtensions;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Mapping;

namespace MapCall.Common.Model.Mappings
{
    public class CurrentAssignmentViewMap : AbstractAuxiliaryDatabaseObject
    {
        public override string SqlCreateString(Dialect dialect, IMapping p, string defaultCatalog, string defaultSchema)
        {
            return
                $"CREATE VIEW [{MC5471_FixCurrentAssignmentView.VIEW_NAME}] " +
                $"AS {MC5471_FixCurrentAssignmentView.NEW_VIEW_SQL.ToSqlite()}";
        }

        public override string SqlDropString(Dialect dialect, string defaultCatalog, string defaultSchema)
        {
            return MC5434_CreateCurrentAssignmentView.DROP_SQL;
        }
    }
}
