using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using DataType = System.ComponentModel.DataAnnotations.DataType;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits
{
    public class EnvironmentalPermitViewModel : ViewModel<EnvironmentalPermit>
    {
        #region Properties

        [AutoMap(MapDirections.ToViewModel)]
        public override int Id { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(State))]
        public virtual int? State { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(WaterType))]
        public virtual int? FacilityType { get; set; }

        [DropDown]
        [EntityMustExist(typeof(EnvironmentalPermitType))]
        [EntityMap, Required]
        public virtual int? EnvironmentalPermitType { get; set; }

        [DropDown]
        [EntityMustExist(typeof(EnvironmentalPermitStatus))]
        [EntityMap]
        public virtual int? EnvironmentalPermitStatus { get; set; }

        [DropDown("", "PublicWaterSupply", "ActiveByOperatingCenterId", DependsOn = nameof(OperatingCenters), ErrorText = "Please select an operating center above.")]
        [EntityMustExist(typeof(PublicWaterSupply))]
        [EntityMap]
        [RequiredWhen("WasteWaterSystem", ComparisonType.EqualTo, null, ErrorMessage = "Please select either a PWSID or a WWSID")]
        public virtual int? PublicWaterSupply { get; set; }
        
        [EntityMustExist(typeof(WasteWaterSystem))]
        [EntityMap]
        [RequiredWhen("PublicWaterSupply", ComparisonType.EqualTo, null, ErrorMessage = "Please select either a PWSID or a WWSID")]
        [DropDown("Environmental", "WasteWaterSystem", "ActiveByOperatingCenter", DependsOn = nameof(OperatingCenters), ErrorText = "Please select an operating center above.")]
        public int? WasteWaterSystem { get; set; }

        [StringLength(EnvironmentalPermit.StringLengths.PERMIT_NUMBER), Required]
        public virtual string PermitNumber { get; set; }

        [StringLength(EnvironmentalPermit.StringLengths.PROGRAM_INTEREST_NUMBER)]
        public virtual string ProgramInterestNumber { get; set; }

        [StringLength(EnvironmentalPermit.StringLengths.PERMIT_CROSS_REFERENCE_NUMBER)]
        public virtual string PermitCrossReferenceNumber { get; set; }

        [Required]
        public virtual DateTime? PermitEffectiveDate { get; set; }

        public virtual DateTime? PermitRenewalDate { get; set; }

        [RequiredWhen("PermitExpires", true)]
        public virtual DateTime? PermitExpirationDate { get; set; }

        [DoesNotAutoMap]
        public virtual bool PermitExpires { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(EnvironmentalPermit.StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }

        public bool RequiresFees { get; set; }
        [Required]
        public virtual bool? ReportingRequired { get; set; }

        public virtual bool? RequiresRequirements { get; set; }

        [CheckBoxList("", "OperatingCenter", "ByStateIdOrAll", DependsOn = nameof(State), DependentsRequired = DependentRequirement.None)]
        public virtual int[] OperatingCenters { get; set; }

        [StringLength(EnvironmentalPermit.StringLengths.PERMIT_NAME)]
        public virtual string PermitName { get; set; }
        
        #endregion
        
        #region Constructors

        public EnvironmentalPermitViewModel(IContainer container) : base(container) { }

        #endregion
    }
}
