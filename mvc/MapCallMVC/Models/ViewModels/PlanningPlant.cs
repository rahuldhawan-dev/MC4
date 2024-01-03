using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class PlanningPlantViewModel : ViewModel<PlanningPlant>
    {
        #region Properties

        [Required, StringLength(PlanningPlant.StringLengths.DESCRIPTION)]
        public string Description { get; set; }

        [Required, StringLength(PlanningPlant.CODE_LENGTH)]
        public string Code { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        #endregion

        #region Constructors

        public PlanningPlantViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchPlanningPlant : SearchSet<PlanningPlant>
    {
        #region Properties

        public string Code { get; set; }
        public string Description { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        #endregion
    }
}