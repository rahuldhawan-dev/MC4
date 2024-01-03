using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels.PreJobSafetyBriefs
{
    public class CreateProductionPreJobSafetyBriefNoWorkOrder : CreateProductionPreJobSafetyBriefBase
    {
        #region Properties

        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown(
            "",
            "Facility",
            "ByOperatingCenterId",
            DependsOn = nameof(OperatingCenter),
            PromptText = "Please select an operating center above")]
        [EntityMap, EntityMustExist(typeof(Facility)), Required]
        public virtual int? Facility { get; set; }

        [CheckBoxList(
            "",
            "Employee",
            "ActiveEmployeesByOperatingCenterIdForPreJobSafetyBriefs",
            DependsOn = nameof(OperatingCenter))]
        public override int[] Employees { get; set; }

        [Required]
        [StringLength(ProductionPreJobSafetyBrief.StringLengths.DESCRIPTION_OF_WORK)]
        public string DescriptionOfWork { get; set; }

        public override int? ProductionWorkOrder
        {
            get => null;
            set { }
        }

        #endregion

        #region Constructors

        public CreateProductionPreJobSafetyBriefNoWorkOrder(IContainer container) : base(container) { }

        #endregion
    }
}