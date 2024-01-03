using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using DataType = System.ComponentModel.DataAnnotations.DataType;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels
{
    public class AllocationPermitViewModel : ViewModel<AllocationPermit>
    {
        #region Properties

        [AutoMap(MapDirections.ToViewModel)]
        public override int Id { get; set; }

        [DisplayName("PWSID"), DropDown]
        [EntityMustExist(typeof(PublicWaterSupply))]
        [EntityMap]
        public virtual int? PublicWaterSupply { get; set; }

        [DisplayName("Environmental Permit ID")]
        [EntityMustExist(typeof(EnvironmentalPermit))]
        [EntityMap]
        [DropDown]
        public virtual int? EnvironmentalPermit { get; set; }

        [DisplayName("Operating Center")]
        [EntityMustExist(typeof(OperatingCenter))]
        [EntityMap]
        [DropDown]
        public virtual int? OperatingCenter { get; set; }

        public virtual DateTime? CreatedAt { get; set; }

        [StringLength(UpdateEnvironmentalPermitTablesForBug1627.StringLengths.SYSTEM)]
        public virtual string System { get; set; }

        public virtual bool? SurfaceSupply { get; set; }
        public virtual bool? GroundSupply { get; set; }

        [StringLength(UpdateEnvironmentalPermitTablesForBug1627.StringLengths.GEOLOGICAL_FORMATION)]
        public virtual string GeologicalFormation { get; set; }

        public virtual bool? ActivePermit { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime? EffectiveDateOfPermit { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime? RenewalApplicationDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime? ExpirationDate { get; set; }

        [StringLength(UpdateEnvironmentalPermitTablesForBug1627.StringLengths.SUB_ALLOCATION_NUMBER)]
        public virtual string SubAllocationNumber { get; set; }

        [DisplayName("GPD"), DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES)]
        public virtual decimal? Gpd { get; set; }
        [DisplayName("MGM"), DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES)]
        public virtual decimal? Mgm { get; set; }
        [DisplayName("MGY"), DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES)]
        public virtual decimal? Mgy { get; set; }

        [StringLength(UpdateEnvironmentalPermitTablesForBug1627.StringLengths.PERMIT_TYPE)]
        public virtual string PermitType { get; set; }

        [RegularExpression(@"[0-9]+", ErrorMessage = "Please enter an integer.")]
        public virtual int? PermitFee { get; set; }

        [StringLength(UpdateEnvironmentalPermitTablesForBug1627.StringLengths.SOURCE_DESCRIPTION)]
        public virtual string SourceDescription { get; set; }

        [StringLength(UpdateEnvironmentalPermitTablesForBug1627.StringLengths.SOURCE_RESTRICTIONS)]
        public virtual string SourceRestrictions { get; set; }

        [StringLength(UpdateEnvironmentalPermitTablesForBug1627.StringLengths.NOTES)]
        [DataType(DataType.MultilineText)]
        public virtual string PermitNotes { get; set; }

        [DisplayName("GPM"), DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES)]
        public virtual decimal? Gpm { get; set; }

        #endregion
        
        #region Constructors

        public AllocationPermitViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateAllocationPermit : AllocationPermitViewModel
    {
        public CreateAllocationPermit(IContainer container) : base(container) { }
    }

    public class EditAllocationPermit : AllocationPermitViewModel
    {
        public EditAllocationPermit(IContainer container) : base(container) { }
    }

    public class SearchAllocationPermit : SearchSet<AllocationPermit>
    {
        #region Properties

        [View("AllocationGroupingID")]
        public int? EntityId { get; set; }

        [View("PWSID"), DropDown]
        public virtual int? PublicWaterSupply { get; set; }

        [View("Environmental Permit ID"), DropDown]
        public virtual int? EnvironmentalPermit { get; set; }

        [View("Operating Center"), DropDown]
        public virtual int? OperatingCenter { get; set; }

        #endregion
    }
}