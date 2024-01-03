using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceUtilityTypeMap : EntityLookupMap<ServiceUtilityType>
    {
        #region Constructors

        public ServiceUtilityTypeMap()
        {
            Id(x => x.Id, "ServiceUtilityTypeId");

            Map(x => x.Type).Not.Nullable();
            Map(x => x.Division).Nullable().Length(2);
        }

        #endregion
    }
}
