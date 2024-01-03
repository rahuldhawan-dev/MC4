using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class CreateSpoilFinalProcessingLocation : SpoilFinalProcessingLocationViewModel
    {
        #region Properties

        [DropDown, Required, EntityMap(MapDirections.None), EntityMustExist(typeof(State))]
        public override int? State { get; set; }

        [DropDown("", "OperatingCenter", "WorkOrdersEnabledByStateId", DependsOn = "State", PromptText = "Please select a State above."), Required, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public override int? OperatingCenter { get; set; }
  
        #endregion

        #region Constructors

        public CreateSpoilFinalProcessingLocation(IContainer container) : base(container) { }

        #endregion
    }
}