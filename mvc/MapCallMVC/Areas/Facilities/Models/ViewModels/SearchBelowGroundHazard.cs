using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using DataAnnotationsExtensions;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class SearchBelowGroundHazard : SearchSet<BelowGroundHazard>
    {

        [DisplayName("Id")]
        public int? EntityId { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("OperatingCenter", "criteriaOperatingCenter", "State.Id")]
        public int? State { get; set; }
        [DropDown("", "OperatingCenter", "ByStateIdForFieldServicesAssets", DependsOn = "State")]
        [EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
        [DisplayName(BelowGroundHazard.Strings.TOWN)]
        [DropDown("Town", "ByOperatingCenterId", DependsOn = "OperatingCenter",
            PromptText = "Please select an operating center", Area = "")]
        [EntityMustExist(typeof(Town))]
        [EntityMap]
        public virtual int? Town { get; set; }
        [DropDown("TownSection", "ActiveByTownId", DependsOn = "Town", PromptText = "Please select an town", Area = "")]
        [EntityMustExist(typeof(TownSection))]
        [EntityMap]
        public virtual int? TownSection { get; set; }
        public virtual int? StreetNumber { get; set; }

        [DisplayName(BelowGroundHazard.Strings.STREET)]
        [DropDown("Street", "GetActiveByTownId", DependsOn = "Town", PromptText = "Please select an town", Area = "")]
        [EntityMustExist(typeof(Street))]
        [EntityMap]
        public virtual int? Street { get; set; }

        [DropDown("Street", "GetActiveByTownId", DependsOn = "Town", PromptText = "Please select an town", Area = "")]
        [EntityMustExist(typeof(Street))]
        [EntityMap]
        public virtual int? CrossStreet { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(HazardType))]
        public int? HazardType { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(AssetStatus))]
        public int? AssetStatus { get; set; }
        [StringLength(maximumLength: BelowGroundHazard.StringLengths.HAZARD_DESCRIPTION_MAX, MinimumLength = BelowGroundHazard.StringLengths.HAZARD_DESCRIPTION_MIN), Multiline]
        public string HazardDescription { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(HazardApproachRecommendedType))]
        public int? HazardApproachRecommendedType { get; set; }
    }
}