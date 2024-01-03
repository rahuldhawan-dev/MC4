using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels
{
    public class GasMonitorViewModel : ViewModel<GasMonitor>
    {
        #region Properties

        [EntityMap(MapDirections.ToViewModel)] // Only needed for cascading. 
        [Required, EntityMustExist(typeof(OperatingCenter))]
        [DropDown]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Equipment", "GasDetectorsByOperatingCenter", DependsOn = nameof(OperatingCenter))]
        [Required, EntityMap, EntityMustExist(typeof(Equipment))]
        public int? Equipment { get; set; }

        [Required, Min(1)]
        public int? CalibrationFrequencyDays { get; set; }

        [DropDown("", "Employee", "ActiveLockoutFormEmployeesByOperatingCenterId", DependsOn = nameof(OperatingCenter))]
        [EntityMap, EntityMustExist(typeof(Employee))]
        public virtual int? AssignedEmployee { get; set; }

        #endregion

        #region Constructor

        public GasMonitorViewModel(IContainer container) : base(container)
        {
        }

        #endregion

        #region Public Methods

        private IEnumerable<ValidationResult> ValidateEquipmentIsGasDetector()
        {
            if (!Equipment.HasValue) {
                yield break; // cut out early, this is handled by Required validation.
            }

            var eq = _container.GetInstance<IRepository<Equipment>>().Find(Equipment.Value);
            if (eq != null && (eq.EquipmentType.Id != EquipmentType.Indices.SAFGASDT || eq.EquipmentPurpose.Abbreviation != EquipmentPurpose.PERSONAL_GAS_DETECTOR_ABBREVIATION))
            {
                yield return new ValidationResult("Equipment must be a Gas Detector.", new[] { nameof(Equipment) });
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateEquipmentIsGasDetector());
        }

        #endregion
    }

    public class CreateGasMonitor : GasMonitorViewModel
    {
        #region Constructor

        public CreateGasMonitor(IContainer container) : base(container)
        {
        }

        #endregion
    }

    public class EditGasMonitor : GasMonitorViewModel
    {
        #region Properties

        [DropDown("", "Employee", "LockoutFormEmployeesByOperatingCenterId", DependsOn = nameof(OperatingCenter))]
        public override int? AssignedEmployee { get => base.AssignedEmployee; set => base.AssignedEmployee = value; }
        
        #endregion

        #region Constructor

        public EditGasMonitor(IContainer container) : base(container)
        {
        }

        #endregion
    }

    public class SearchGasMonitor : SearchSet<GasMonitor>
    {
        #region Properties

        [SearchAlias("criteriaFacility.OperatingCenter", "criteriaOperatingCenter", "Id")]
        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [SearchAlias("Equipment", "criteriaEquipment", "Id")]
        [DropDown("", "Equipment", "GasDetectorsByOperatingCenter", DependsOn = nameof(OperatingCenter))]
        [EntityMap, EntityMustExist(typeof(Equipment))]
        public int? Equipment { get; set; }

        [SearchAlias("criteriaEquipment.Facility", "criteriaFacility", "Id")] // Required for OperatingCenter filtering
        [DropDown("", "Facility", "GetByOperatingCenterId", DependsOn = nameof(OperatingCenter))]
        [EntityMap, EntityMustExist(typeof(Facility))]
        public int? Facility { get; set; }

        [DropDown("", "Employee", "LockoutFormEmployeesByOperatingCenterId", DependsOn = nameof(OperatingCenter))]
        [EntityMap, EntityMustExist(typeof(Employee))]
        public int? AssignedEmployee { get; set; }

        [SearchAlias("MostRecentPassingGasMonitorCalibration", "mrpgmc", "DueCalibration", Required = true)]
        public bool? DueCalibration { get; set; }
        
        [SearchAlias("criteriaEquipment.EquipmentStatus", "Id")]
        [DropDown, EntityMap, EntityMustExist(typeof(EquipmentStatus))]
        public int? EquipmentStatus { get; set; }

        [SearchAlias("Equipment", "criteriaEquipment", "SerialNumber")]
        public string SerialNumber { get; set; }

        #endregion
    }
}