using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ProductionWorkOrderPriorityMap : EntityLookupMap<ProductionWorkOrderPriority>
    {
        #region Constants

        public const string TABLE_NAME = "ProductionWorkOrderPriorities";

        #endregion

        #region Constructors

        public ProductionWorkOrderPriorityMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Assigned();
        }

        #endregion
    }
}
