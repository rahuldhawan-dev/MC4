using System.ComponentModel;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.SewerMainCleanings
{
    public class SearchSewerMainCleaning : SearchSet<SewerMainCleaning>
    {
        #region Properties

        [DisplayName("Id")]
        public int? EntityId { get; set; }

        [DropDown]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? Town { get; set; }

        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? Street { get; set; }

        public DateRange Date { get; set; }
        
        [EntityMap, EntityMustExist(typeof(SewerMainInspectionType)), DropDown]
        public int? InspectionType { get; set; }

        [EntityMap, EntityMustExist(typeof(SewerMainInspectionGrade)), DropDown]
        public int? InspectionGrade { get; set; }

        public bool? BlockageFound { get; set; }

        [DropDown, EntityMustExist(typeof(CauseOfBlockage))]
        public int? CauseOfBlockage { get; set; }

        [Search(CanMap = false)]
        public bool? HasSAPErrorCode { get; set; }
        public string SAPErrorCode { get; set; }
        public SearchString SAPNotificationNumber { get; set; }

        #endregion

        #region Public Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);
            if (HasSAPErrorCode.HasValue)
            {
                if (HasSAPErrorCode.Value)
                {
                    mapper.MappedProperties["SAPErrorCode"].Value = SearchMapperSpecialValues.IsNotNull;
                }
                else
                {
                    mapper.MappedProperties["SAPErrorCode"].Value = SearchMapperSpecialValues.IsNull;
                }
            }
        }

        #endregion
    }
}