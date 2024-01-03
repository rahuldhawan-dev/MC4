using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MotorVehicleCodeMap : ClassMap<MotorVehicleCode>
    {
        #region Constructors

        public MotorVehicleCodeMap()
        {
            Id(x => x.Id);

            Map(x => x.Description)
               .Column("Description")
                // ReSharper disable once AccessToStaticMemberViaDerivedType
               .Length(MotorVehicleCode.StringLengths.DESCRIPTION)
               .Not.Nullable();
        }

        #endregion
    }
}
