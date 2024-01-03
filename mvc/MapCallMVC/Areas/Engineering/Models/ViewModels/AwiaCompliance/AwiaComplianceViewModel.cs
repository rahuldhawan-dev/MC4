using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;
using AwiaComplianceEntity = MapCall.Common.Model.Entities.AwiaCompliance;

namespace MapCallMVC.Areas.Engineering.Models.ViewModels.AwiaCompliance
{
    public class AwiaComplianceViewModel : ViewModel<AwiaComplianceEntity>
    {
        #region Constructors

        public AwiaComplianceViewModel(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required, DropDown, EntityMap, EntityMustExist(typeof(State))]
        public virtual int? State { get; set; }

        [Required, DropDown("", "OperatingCenter", "ActiveByStateIdOrAll", DependsOn = nameof(State), PromptText = "Select a state above"),
         EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(PublicWaterSupply))]
        [MultiSelect("", "PublicWaterSupply", "ByOperatingCenterId", DependsOn = nameof(OperatingCenter), PromptText = "Please select an operating center above.")]
        public virtual int[] PublicWaterSupplies { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(AwiaComplianceCertificationType))]
        public virtual int? CertificationType { get; set; }

        [DropDown("", "User", "AwiaUsersByOperatingCenterIdAndRole", DependsOn = nameof(OperatingCenter), PromptText = "Select an operating center above"), 
         EntityMap, EntityMustExist(typeof(User)), Required]
        public virtual int? CertifiedBy { get; set; }

        [Required]
        public virtual DateTime? DateSubmitted { get; set; }

        [Required]
        public virtual DateTime? DateAccepted { get; set; }

        [Required]
        public virtual DateTime? RecertificationDue { get; set; }

        #endregion
    }
}