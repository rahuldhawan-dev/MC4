using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class FacilityProcessStepViewModel : ViewModel<FacilityProcessStep>
    {
        #region Properties

        [DropDown, DoesNotAutoMap("Used for cascading. Set in Map.")]
        public int? OperatingCenter { get; set; }

        [DoesNotAutoMap("Used for cascading. Set in Map.")]
        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter")]
        public int? Facility { get; set; }

        [DropDown("Facilities", "FacilityProcess", "ByFacilityId", DependsOn = "Facility")]
        [Required, EntityMap, EntityMustExist(typeof(FacilityProcess))]
        public int? FacilityProcess { get; set; }

        [DropDown("", "Equipment", "ByFacilityId", DependsOn = "Facility")]
        [EntityMap, EntityMustExist(typeof(Equipment))]
        public int? Equipment { get; set; }

        [DropDown]
        [Required, EntityMap, EntityMustExist(typeof(FacilityProcessStepSubProcess))]
        public int? FacilityProcessStepSubProcess { get; set; }

        [DropDown]
        [Required, EntityMap, EntityMustExist(typeof(UnitOfMeasure))]
        public int? UnitOfMeasure { get; set; }

        [Required]
        public int? ElevationInFeet { get; set; }

        [Required]
        public decimal? ProcessTarget { get; set; }

        [Required]
        public decimal? NormalRangeMin { get; set; }

        [Required]
        public decimal? NormalRangeMax { get; set; }

        [Required, StringLength(FacilityProcessStep.MAX_DESCRIPTION_LENGTH)]
        public virtual string Description { get; set; }

        [Required]
        public virtual decimal? StepNumber { get; set; }

        public string ContingencyOperation { get; set; }
        public string LossOfCommunicationPowerImpact { get; set; }

        #endregion

        #region Constructors

        public FacilityProcessStepViewModel(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void Map(FacilityProcessStep entity)
        {
            base.Map(entity);
            var facility = entity.FacilityProcess.Facility;
            Facility = facility.Id;
            OperatingCenter = facility.OperatingCenter.Id;
        }

        #endregion

    }

    public class SearchFacilityProcessStep : SearchSet<FacilityProcessStep>
    {
        #region Properties

        [DropDown, SearchAlias("e.Facility", "f", "OperatingCenter.Id")]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter"), SearchAlias("Equipment", "e", "Facility.Id", Required = true)]
        public int? Facility { get; set; }

        [DropDown("", "Equipment", "ByFacilityId", DependsOn = "Facility")]
        public int? Equipment { get; set; }

        [DropDown]
        public int? FacilityProcessStepSubProcess { get; set; }

        [DropDown]
        public int? UnitOfMeasure { get; set; }
        public int? ElevationInFeet { get; set; }
        public decimal? NormalRangeMin { get; set; }
        public decimal? NormalRangeMax { get; set; }
        public string Description { get; set; }
        public decimal? StepNumber { get; set; }
        public decimal? ProcessTarget { get; set; }

        #endregion
    }

    public class SearchFacilityProcessStepScadaReadings
    {
        public int EquipmentId { get; set; }
    }
}