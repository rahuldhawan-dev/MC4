using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCallMVC.Models.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class AssetConditionReasonViewModel : EntityLookupViewModel<AssetConditionReason>
    {
        #region Properties

        [Required, StringLength(AssetConditionReason.StringLengths.CODE)]
        public string Code { get; set; }
        [Required, DoesNotAutoMap, DropDown, EntityMustExist(typeof(ConditionType))]
        public int? ConditionType { get; set; }
        [Required]
        [EntityMap, EntityMustExist(typeof(ConditionDescription))]
        [DropDown("Production", "ConditionDescription", "GetByConditionTypeId", DependsOn = "ConditionType", PromptText = "Select a condition type above")]
        public int? ConditionDescription { get; set; }

        #endregion

        #region Constructors

        public AssetConditionReasonViewModel(IContainer container) : base(container) { }

        #endregion
    }
}