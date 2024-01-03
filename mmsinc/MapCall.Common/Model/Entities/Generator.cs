using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Utilities.Excel;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Generator : IEntity, IValidatableObject
    {
        #region StringLengths

        public struct StringLengths
        {
            public const int ENGINE_SERIAL_NUMBER = AddGeneratorsForBug1462.StringLengths.ENGINE_SERIAL_NUMBER,
                             GENERATOR_SERIAL_NUMBER = AddGeneratorsForBug1462.StringLengths.GENERATOR_SERIAL_NUMBER,
                             OUTPUT_VOLTAGE = AddGeneratorsForBug1462.StringLengths.OUTPUT_VOLTAGE,
                             TRAILER_VIN = AddGeneratorsForBug1462.StringLengths.TRAILER_VIN,
                             GVWR = AddGeneratorsForBug1462.StringLengths.GVWR,
                             AQ_PERMIT_NUMBER = AddGeneratorsForBug1462.StringLengths.AQ_PERMIT_NUMBER,
                             DESCRIPTION = AddGeneratorsForBug1462.StringLengths.DESCRIPTION;
        }

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }

        public virtual string EngineSerialNumber { get; set; }
        public virtual string GeneratorSerialNumber { get; set; }
        public virtual decimal? OutputVoltage { get; set; }
        public virtual decimal? OutputKW { get; set; }
        public virtual decimal? LoadCapacity { get; set; }
        public virtual bool? HasParallelElectricOperation { get; set; }
        public virtual bool? HasAutomaticStart { get; set; }
        public virtual bool? HasAutomaticPowerTransfer { get; set; }
        public virtual bool? IsPortable { get; set; }
        public virtual bool? SCADA { get; set; }
        public virtual string TrailerVIN { get; set; }
        public virtual string GVWR { get; set; }
        public virtual decimal? FuelGPH { get; set; }
        public virtual int? BTU { get; set; }
        public virtual int? HP { get; set; }
        public virtual string AQPermitNumber { get; set; }

        #endregion

        #region References

        [Required]
        public virtual Equipment Equipment { get; set; }

        public virtual EmergencyPowerType EmergencyPowerType { get; set; }
        public virtual EquipmentManufacturer EngineManufacturer { get; set; }
        public virtual EquipmentModel EngineModel { get; set; }
        public virtual EquipmentManufacturer GeneratorManufacturer { get; set; }
        public virtual EquipmentModel GeneratorModel { get; set; }
        public virtual FuelType FuelType { get; set; }

        #endregion

        #region Logical Properties

        #endregion

        #endregion

        #region Exposed Methods

        public virtual
            IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return
                Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
