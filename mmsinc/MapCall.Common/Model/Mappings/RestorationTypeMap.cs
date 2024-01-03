using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class RestorationTypeMap : EntityLookupMap<RestorationType>
    {
        #region Properties

        protected override string IdName => "RestorationTypeID";

        #endregion

        #region Constructors

        public RestorationTypeMap()
        {
            Map(x => x.PartialRestorationDaysToComplete).Not.Nullable();
            Map(x => x.FinalRestorationDaysToComplete).Not.Nullable();

            HasMany(x => x.RestorationTypeCosts).KeyColumn("RestorationTypeID");
        }

        #endregion
    }
}
