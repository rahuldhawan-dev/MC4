using MapCall.Common.Model.Migrations._2020;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Mapping;

namespace MapCall.Common.Model.Mappings
{
    public class InitialShortCycleAssignmentsAuxilaryMap : AbstractAuxiliaryDatabaseObject
    {
        public struct SQL
        {
            public const string CREATE_VIEW = ChangeCallIdToLongForMC2428.CREATE_VIEW;
            public const string DROP_VIEW = ChangeCallIdToLongForMC2428.DROP_VIEW;
        }

        public override string SqlCreateString(Dialect dialect, IMapping p, string defaultCatalog, string defaultSchema)
        {
            return SQL.CREATE_VIEW;
        }

        public override string SqlDropString(Dialect dialect, string defaultCatalog, string defaultSchema)
        {
            return SQL.DROP_VIEW;
        }
    }
}
