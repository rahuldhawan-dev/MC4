using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class WayToRemoveLocksMap : EntityLookupMap<WayToRemoveLocks>
    {
        #region Constructors

        public WayToRemoveLocksMap() : base()
        {
            Table("WaysToRemoveLocks");
        }

        #endregion
    }
}
