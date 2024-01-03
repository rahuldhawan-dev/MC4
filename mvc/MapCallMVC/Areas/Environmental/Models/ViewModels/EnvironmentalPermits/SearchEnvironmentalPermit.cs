using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits
{
    public class SearchEnvironmentalPermit : SearchSet<EnvironmentalPermit>
    {
        #region Properties

        [View("PermitID")]
        public int? Id { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        //TODO: Create a MultiSelect that uses the many to many table
        [MultiSelect("", "OperatingCenter", "ByStateIdForEnvironmentalGeneral", DependsOn = "State", DependentsRequired = DependentRequirement.None)]
        [SearchAlias("OperatingCenters", "Id")]
        [EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int[] OperatingCenter { get; set; }

        [View("Permit Type"), DropDown, EntityMap, EntityMustExist(typeof(EnvironmentalPermitType))]
        public int? EnvironmentalPermitType { get; set; }
        
        [View("Permit Status"), MultiSelect, EntityMap, EntityMustExist(typeof(EnvironmentalPermitStatus))]
        public int[] EnvironmentalPermitStatus { get; set; }

        [View("PWSID"), 
         EntityMap,
         EntityMustExist(typeof(PublicWaterSupply)),
         DropDown("",
             "PublicWaterSupply",
             "ActiveByOperatingCenterId",
             DependsOn = nameof(OperatingCenter),
             PromptText = "Please select an operating center above.")]
        public int? PublicWaterSupply { get; set; }

        [View("WWSID")]
        [DropDown("Environmental", "WasteWaterSystem", "ActiveByOperatingCenter", DependsOn = nameof(OperatingCenter), DependentsRequired = DependentRequirement.None)]
        [EntityMustExist(typeof(WasteWaterSystem)), EntityMap]
        public int? WasteWaterSystem { get; set; }

        public string PermitNumber { get; set; }

        [View("Program Interest #")]
        public string ProgramInterestNumber { get; set; }

        [View("Cross Reference #")]
        public string PermitCrossReferenceNumber { get; set; }

        [View("Effective Date")]
        public DateRange PermitEffectiveDate { get; set; }

        [View("Expiration Date")]
        public DateRange PermitExpirationDate { get; set; }

        public bool? IsLinkedToFacilityOrEquipment { get; set; }

        [View("Has Fees")]
        [Search(ChecksExistenceOfChildCollection = true)]
        public bool? Fees { get; set; }

        public string Description { get; set; }

        public DateRange PermitRenewalDate { get; set; }

        public string PermitName { get; set; }

        #endregion
    }
}