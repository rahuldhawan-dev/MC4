using MapCall.Common.Model.Migrations._2022;
using NHibernate.Dialect;
using NHibernate.Mapping;

namespace MapCall.Common.Model.Mappings
{
    public class CountEquipmentMaintenancePlansByMaintenancePlanViewMap : AbstractAuxiliaryDatabaseObject
    {
        #region Exposed Methods
        
        public override string SqlCreateString(Dialect dialect, NHibernate.Engine.IMapping p, string defaultCatalog, string defaultSchema)
        {
            return MC5199CreateViewForCount.CREATE_VIEW;
        }

        public override string SqlDropString(Dialect dialect, string defaultCatalog, string defaultSchema)
        {
            return MC5199CreateViewForCount.DROP_VIEW;
        }

        #endregion
    }
}
