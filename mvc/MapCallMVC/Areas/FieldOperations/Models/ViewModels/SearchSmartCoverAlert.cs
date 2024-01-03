using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SearchSmartCoverAlert : SearchSet<SmartCoverAlert>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("SewerOpening.OperatingCenter", "State.Id")]
        public int? State { get; set; }

        [DropDown("", "OperatingCenter", "ByStateIdForFieldServicesAssets", DependsOn = "State", PromptText = "Select a state above"), EntityMap, EntityMustExist(typeof(OperatingCenter))]
        [SearchAlias("SewerOpening", "OperatingCenter.Id", Required = true)]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above"), EntityMap, EntityMustExist(typeof(Town))]
        [SearchAlias("SewerOpening", "Town.Id")]
        public int? Town { get; set; }

        [DropDown("", "TownSection", "ByTownId", DependsOn = "Town", PromptText = "Select a town above"), EntityMap, EntityMustExist(typeof(TownSection))]
        [SearchAlias("SewerOpening", "TownSection.Id")]
        public int? TownSection { get; set; }

        public SearchString SewerOpeningNumber { get; set; }
        
        [MultiSelect, EntityMap, EntityMustExist(typeof(SmartCoverAlertApplicationDescriptionType))]
        public int[] ApplicationDescription { get; set; }

        [View("Date Received?")]
        public DateRange DateReceived { get; set; }

        public bool? Acknowledged { get; set; }
        
        [View("Work Order Created"), Search(ChecksExistenceOfChildCollection = true)]
        public bool? WorkOrders { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SewerOpening))]
        public int? SewerOpening { get; set; }

        [MultiSelect, SearchAlias("SmartCoverAlertAlarms", "criteriaSmartCoverAlertAlarms", "AlarmType.Id")]
        public int[] HighLevelAlarmType { get; set; }

        [MultiSelect, SearchAlias("SmartCoverAlertSmartCoverAlertTypes", "criteriaSmartCoverAlertSmartCoverAlertTypes", "SmartCoverAlertType.Id")]
        public int[] AlertType { get; set; }

        #endregion
    }
}