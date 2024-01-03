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
    public class SearchSpoilFinalProcessingLocation : SearchSet<SpoilFinalProcessingLocation>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("oc.State", "st", "Id", Required = true)]
        public int? State { get; set; }

        [DropDown("", "OperatingCenter", "WorkOrdersEnabledByStateId", DependsOn = "State", PromptText = "Please select a State above."), EntityMap]
        [SearchAlias("OperatingCenter", "oc", "Id", Required = true)]
        public int? OperatingCenter { get; set; }

        [EntityMap, EntityMustExist(typeof(Town))]
        [DropDown("Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center", Area = "")]
        public int? Town { get; set; }

        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? Street { get; set; }

        public virtual SearchString Name { get; set; }

        #endregion
    }
}