using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SearchRestorationProcessing : SearchWorkOrder
    {
        #region Properties

        [DropDown, RequiredWhen(nameof(Id), ComparisonType.EqualTo, null)]
        public override int? OperatingCenter
        {
            get => base.OperatingCenter; 
            set => base.OperatingCenter = value;
        }

        [View("Last Crew Assigned")]
        [EntityMap, EntityMustExist(typeof(Crew))]
        [SearchAlias("CurrentAssignment", "crewAlias", "Crew.Id", Required = true)]
        [DropDown("FieldOperations", "Crew", "ByOperatingCenterOrAll", DependsOn = nameof(OperatingCenter), PromptText = "Select an Operating Center")]
        public int? CurrentCrew { get; set; }

        //This property is only here so sorting works correctly. 
        [SearchAlias("crewAlias.Crew", "crewAliasSorting", "Id", Required = true)]
        public int? CurrentCrewForSortingOnly { get; set; }

        [SearchAlias("Restorations", "PartialRestorationDate")]
        public DateRange InitialDate { get; set; }

        [SearchAlias("Restorations", "FinalRestorationDate")]
        public DateRange FinalDate { get; set; }

        [SearchAlias("WorkOrderDocuments", "CreatedAt")]
        public DateRange DocumentDate { get; set; }
        
        #endregion
    }
}