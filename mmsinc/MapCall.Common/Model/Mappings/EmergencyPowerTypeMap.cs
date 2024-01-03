using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EmergencyPowerTypeMap : ClassMap<EmergencyPowerType>
    {
        #region Constructors

        public EmergencyPowerTypeMap()
        {
            Id(x => x.Id, "EmergencyPowerTypeID");

            Map(x => x.Description).Not.Nullable();
        }

        #endregion
    }
}
