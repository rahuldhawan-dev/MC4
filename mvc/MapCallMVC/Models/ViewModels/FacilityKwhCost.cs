using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class FacilityKwhCostViewModel : ViewModel<FacilityKwhCost>
    {
        #region Properties

        [EntityMap(MapDirections.ToPrimary)]
        public virtual int? Facility { get; set; }
        [Required, Display(Name = "Cost per kWh")]
        public virtual decimal? CostPerKwh { get; set; }
        [Required, DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime? StartDate { get; set; }
        [Required, DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime? EndDate { get; set; }

        #endregion

        #region Constructors

        public FacilityKwhCostViewModel(IContainer container) : base(container) {}

        #endregion

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = base.Validate(validationContext);

            if (!Facility.HasValue)
            {
                return result;
            }

            var facility = _container.GetInstance<RepositoryBase<Facility>>().Find(Facility.Value);
            var theStart = StartDate.Value.Date;
            var theEnd = EndDate.Value.Date;

            foreach (var cost in facility.KwhCosts.Where(c => c.Id != Id))
            {
                var curStart = cost.StartDate.Date;
                var curEnd = cost.EndDate.Date;
                if (curStart == theStart || curEnd == theEnd ||
                    (curStart <= theEnd && curEnd >= theStart))
                {
                    return result.Union(new List<ValidationResult> {new ValidationResult("Cannot create a new cost within the date range of another cost for the same facility.")});
                }
            }

            return result;
        }
    }

    public class CreateFacilityKwhCost : FacilityKwhCostViewModel
    {
        #region Properties

        [Required, DropDown, DoesNotAutoMap("For cascades only")]
        public virtual int? OperatingCenter { get; set; }
        [Required, DropDown("Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center above."), EntityMustExist(typeof(Facility)), EntityMap]
        public override int? Facility { get; set; }

        #endregion

        #region Constructors

        public CreateFacilityKwhCost(IContainer container) : base(container) {}

        #endregion
    }

    public class EditFacilityKwhCost : FacilityKwhCostViewModel
    {
        #region Constructors

        public EditFacilityKwhCost(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchFacilityKwhCost : SearchSet<FacilityKwhCost>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter)), SearchAlias("Facility", "f", "OperatingCenter.Id")]
        public virtual int? OperatingCenter { get; set; }

        [DropDown("Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center above."), EntityMustExist(typeof(Facility))]
        public virtual int? Facility { get; set; }

        #endregion
    }
}