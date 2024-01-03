using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataAnnotationsExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using MapCall.Common.Model.Entities;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;
using StringLengths = MapCall.Common.Model.Migrations.CreateTablesForBug1510.StringLengths.FilterMedia;

namespace MapCallMVC.Models.ViewModels
{
    public class FilterMediaViewModel : ViewModel<FilterMedia>
    {
        #region Properties

        [Required, DropDown]
        [EntityMustExist(typeof(Facility))]
        [EntityMap]
        public virtual int Facility { get; set; }
        [Required, DropDown("Equipment", "ByFacilityId", DependsOn = "Facility")]
        [EntityMustExist(typeof(Equipment))]
        [EntityMap]
        public virtual int Equipment { get; set; }
        [Required, DropDown(ControllerViewDataKey = "FilterMediaType")]
        [EntityMustExist(typeof(FilterMediaType))]
        [EntityMap]
        public virtual int MediaType { get; set; }
        [DropDown(ControllerViewDataKey = "FilterMediaWashType")]
        [EntityMustExist(typeof(FilterMediaWashType))]
        [EntityMap]
        public virtual int? WashType { get; set; }
        [DropDown(ControllerViewDataKey = "FilterMediaLevelControlMethod")]
        [EntityMustExist(typeof(FilterMediaLevelControlMethod))]
        [EntityMap]
        public virtual int? LevelControlMethod { get; set; }
        [Required, DropDown(ControllerViewDataKey = "FilterMediaFilterType")]
        [EntityMustExist(typeof(FilterMediaFilterType))]
        [EntityMap]
        public virtual int FilterType { get; set; }
        [Required, DropDown(ControllerViewDataKey = "FilterMediaLocation")]
        [EntityMustExist(typeof(FilterMediaLocation))]
        [EntityMap]
        public virtual int Location { get; set; }

        [Required]
        public virtual int FilterNumber { get; set; }
        [StringLength(StringLengths.EQUIPMENT_IDENTIFIER)]
        public virtual string EquipmentIdentifier { get; set; }
        public virtual int? YearInService { get; set; }
        public virtual int? EstimatedMediaLifecycle { get; set; }
        public virtual float? CapacityMGD { get; set; }
        [StringLength(StringLengths.COEFFICIENT)]
        public virtual string Coefficient { get; set; }
        [StringLength(StringLengths.FILTER_DIMENSIONS)]
        public virtual string FilterDimensions { get; set; }
        public virtual int? MediaArea { get; set; }
        public virtual int? MediaVolume { get; set; }
        public virtual bool? GravelSupportMedia { get; set; }
        public virtual decimal? MonthlyMediaExpense { get; set; }
        public virtual decimal? AnnualInspectionCosts { get; set; }
        public virtual decimal? AnnualAnalysisCosts { get; set; }
        public virtual decimal? AnnualCompanyLaborCosts { get; set; }
        public virtual int? EquipmentCriticalRating { get; set; }
        public virtual int? YearLastPainted { get; set; }
        public virtual bool? ServedByStandbyPower { get; set; }
        [StringLength(StringLengths.TURBIDIMETER_MODEL)]
        public virtual string TurbidimeterModel { get; set; }
        [StringLength(StringLengths.NOTES)]
        public virtual string Notes { get; set; }
        [StringLength(StringLengths.PRODUCT_CODE)]
        public virtual string ProductCode { get; set; }
        public virtual int? AnthraciteDepth { get; set; }
        public virtual int? GACDepth { get; set; }
        public virtual int? SandDepth { get; set; }
        public virtual int? GravelDepth { get; set; }
        public virtual DateTime? LastTimeChanged { get; set; }
        public virtual DateTime? LastTimeCleaned { get; set; }
        public virtual bool? AirScouring { get; set; }
        public virtual bool? Recycling { get; set; }
        [StringLength(StringLengths.COMMENT)]
        public virtual string Comment { get; set; }

        [DoesNotAutoMap("Display only")]
        public virtual Facility FacilityObj { get; set; }

        [DoesNotAutoMap("Display only")]
        public virtual Equipment EquipmentObj { get; set; }

        #endregion

        #region Constructors

        public FilterMediaViewModel(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        // This is here, rather than in Map, because the New action needs it.
        public virtual void MapSelf()
        {
            if (Facility != 0)
            {
                FacilityObj = _container.GetInstance<MMSINC.Data.NHibernate.IRepository<Facility>>().Find(Facility);
            }

            if (Equipment != 0)
            {
                EquipmentObj = _container.GetInstance<MMSINC.Data.NHibernate.IRepository<Equipment>>().Find(Equipment);
            }
        }

        public override void Map(FilterMedia entity)
        {
            base.Map(entity);
            MapSelf();
        }

        public override FilterMedia MapToEntity(FilterMedia entity)
        {
            if (FilterNumber != entity.FilterNumber)
            {
                AdjustFilterNumber(entity);
            }

            return base.MapToEntity(entity);
        }

        private void AdjustFilterNumber(FilterMedia entity)
        {
            var repository = _container.GetInstance<MMSINC.Data.NHibernate.IRepository<FilterMedia>>();
            var equipment =
                _container.GetInstance<MMSINC.Data.NHibernate.IRepository<Equipment>>().Find(Equipment);
            if (FilterNumber > entity.FilterNumber)
            {
                (from f in equipment.FilterMediae
                 where f.FilterNumber > entity.FilterNumber && f.FilterNumber <= FilterNumber
                 select f).Each(f => {
                     f.FilterNumber--;
                     repository.Save(f);
                 });
            }
            else if (FilterNumber < entity.FilterNumber)
            {
                (from f in equipment.FilterMediae
                 where f.FilterNumber < entity.FilterNumber && f.FilterNumber >= FilterNumber
                 select f).Each(f => {
                     f.FilterNumber++;
                     repository.Save(f);
                 });
            }
        }

        #endregion
    }

    public class CreateFilterMedia : FilterMediaViewModel
    {
        #region Constructors

        public CreateFilterMedia(IContainer container) : base(container) {}

        #endregion
    }

    public class UpdateFilterMedia : FilterMediaViewModel
    {
        #region Constructors

        public UpdateFilterMedia(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchFilterMedia : SearchSet<FilterMedia>
    {
        #region Properties

        [DropDown, SearchAlias("f.Town", "t", "State.Id", Required = true)]
        public virtual int? State { get; set; }
        [Display(Name = "Town"), DropDown("Town", "WithFacilitiesByStateId", DependsOn = "State", PromptText = "Please select a state above"), SearchAlias("e.Facility", "f", "Town.Id", Required = true)]
        public virtual int? Town { get; set; }
        [Display(Name = "Facility"), DropDown("Facility", "ByTownId", DependsOn = "Town", PromptText = "Please select a town above"), SearchAlias("Equipment", "e", "Facility.Id", Required = true)]
        public int? Facility { get; set; }
        [Display(Name = "Filter Type"), DropDown(ControllerViewDataKey = "FilterMediaFilterType")]
        public int? FilterType { get; set; }
        [Display(Name = "Wash Type"), DropDown(ControllerViewDataKey = "FilterMediaWashType")]
        public int? WashType { get; set; }
        [Display(Name = "Media Type"), DropDown(ControllerViewDataKey = "FilterMediaType")]
        public int? MediaType { get; set; }
        [Display(Name = "Location"), DropDown(ControllerViewDataKey = "FilterMediaLocation")]
        public int? Location { get; set; }

        #endregion
    }
}