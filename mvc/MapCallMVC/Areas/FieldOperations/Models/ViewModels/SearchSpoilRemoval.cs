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
    public class SearchSpoilRemoval : SearchSet<SpoilRemoval>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("oc.State", "st", "Id", Required = true)]

        public int? State { get; set; }

        [DropDown("", "OperatingCenter", "WorkOrdersEnabledByStateId", DependsOn = "State", PromptText = "Please select a State above.")]
        [SearchAlias("rf.OperatingCenter", "oc", "Id", Required = true)]
        public int? OperatingCenter { get; set; }

        [DropDown("FieldOperations", "SpoilStorageLocation", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select a OperatingCenter above."), EntityMap, EntityMustExist(typeof(SpoilStorageLocation))]
        [SearchAlias("RemovedFrom", "rf", "Id")]
        public int? RemovedFrom { get; set; }

        [DropDown("FieldOperations", "SpoilFinalProcessingLocation", "GetByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select a OperatingCenter above."), EntityMap, EntityMustExist(typeof(SpoilStorageLocation))]
        public int? FinalDestination { get; set; }

        public DateRange DateRemoved { get; set; }

        public NumericRange Quantity { get; set; }

        #endregion
    }
}