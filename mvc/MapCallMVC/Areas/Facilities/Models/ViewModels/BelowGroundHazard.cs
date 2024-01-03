using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public abstract class BelowGroundHazardViewModel : ViewModel<BelowGroundHazard>
    {
        #region Properties

        [EntityMustExist(typeof(WorkOrder))]
        [EntityMap("WorkOrder")]
        public virtual int? WorkOrder { get; set; }

        [Required, Coordinate(AddressCallback = "BelowGroundHazard.getAddress", IconSet = IconSets.SingleDefaultIcon), EntityMap]
        [EntityMustExist(typeof(Coordinate))]
        public virtual int? Coordinate { get; set; }

        [Required, DisplayName(BelowGroundHazard.Strings.TOWN)]
        [DropDown("Town", "ByOperatingCenterId", DependsOn = "OperatingCenter",
            PromptText = "Please select an operating center", Area = "")]
        [EntityMap, EntityMustExist(typeof(Town))]
        public virtual int? Town { get; set; }

        [DropDown("TownSection", "ActiveByTownId", DependsOn = "Town",
            PromptText = "Please select a town", Area = "")]
        [EntityMap, EntityMustExist(typeof(TownSection))]
        public virtual int? TownSection { get; set; }
        public virtual int? StreetNumber { get; set; }

        [Required, DisplayName(BelowGroundHazard.Strings.STREET)]
        [DropDown("Street", "GetActiveByTownId", DependsOn = "Town",
            PromptText = "Please select a town", Area = "")]
        [EntityMap, EntityMustExist(typeof(Street))]
        public virtual int? Street { get; set; }

        [DropDown("Street", "GetActiveByTownId", DependsOn = "Town",
            PromptText = "Please select a town", Area = "")]
        [EntityMustExist(typeof(Street))]
        [EntityMap]
        public virtual int? CrossStreet { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }
        [Required, Range(BelowGroundHazard.Ranges.AREA_LOWER, BelowGroundHazard.Ranges.AREA_UPPER)]
        public virtual int? HazardArea { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(HazardType))]
        public virtual int? HazardType { get; set; }
        [Range(BelowGroundHazard.Ranges.DEPTH_LOWER, BelowGroundHazard.Ranges.DEPTH_UPPER)]
        public virtual int? DepthOfHazard { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(AssetStatus))]
        public virtual int? AssetStatus { get; set; }
        [Required, StringLength(maximumLength: BelowGroundHazard.StringLengths.HAZARD_DESCRIPTION_MAX, MinimumLength = BelowGroundHazard.StringLengths.HAZARD_DESCRIPTION_MIN), Multiline]
        public virtual string HazardDescription { get; set; }
        public virtual int? ProximityToAmWaterAsset { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(HazardApproachRecommendedType))]
        public virtual int? HazardApproachRecommendedType { get; set; }

        #endregion

        #region Constructor

        public BelowGroundHazardViewModel(IContainer container) : base(container) { }

        #endregion
    }
}