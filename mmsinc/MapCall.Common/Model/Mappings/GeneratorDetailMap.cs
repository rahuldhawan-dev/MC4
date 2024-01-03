using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class GeneratorMap : ClassMap<Generator>
    {
        #region Constructors

        public GeneratorMap()
        {
            Id(x => x.Id, "GeneratorID");

            References(x => x.Equipment);
            References(x => x.EmergencyPowerType);
            References(x => x.EngineManufacturer);
            References(x => x.EngineModel);
            References(x => x.GeneratorManufacturer);
            References(x => x.GeneratorModel);
            References(x => x.FuelType);

            Map(x => x.EngineSerialNumber).Nullable();
            Map(x => x.GeneratorSerialNumber).Nullable();
            Map(x => x.OutputVoltage);
            Map(x => x.OutputKW);
            Map(x => x.LoadCapacity);
            Map(x => x.HasParallelElectricOperation);
            Map(x => x.HasAutomaticStart);
            Map(x => x.HasAutomaticPowerTransfer);
            Map(x => x.IsPortable);
            Map(x => x.SCADA);
            Map(x => x.TrailerVIN).Column("TrailerVin").Nullable();
            Map(x => x.GVWR).Nullable();
            Map(x => x.FuelGPH);
            Map(x => x.BTU);
            Map(x => x.HP);
            Map(x => x.AQPermitNumber).Nullable();
        }

        #endregion
    }
}
