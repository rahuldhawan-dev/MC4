using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class EditRoadwayImprovementNotificationStreet : ViewModel<RoadwayImprovementNotificationStreet>
    {
        #region Properties

        [Required, EntityMustExist(typeof(RoadwayImprovementNotification)), EntityMap]
        public virtual int RoadwayImprovementNotification { get; set; }

        [DropDown("Street", "ByTownId", DependsOn = "Town", PromptText = "Please select a town above.", Area = "")]
        [Required, EntityMustExist(typeof(Street)), EntityMap]
        public virtual int? Street { get; set; }

        [Coordinate(AddressCallback = "Streets.getAddress", IconSet = IconSets.SingleDefaultIcon), EntityMustExist(typeof(Coordinate)), EntityMap]
        public virtual int? Coordinate { get; set; }

        [StringLength(AddRoadwayNotificationStreetsForBug2596.StringLengths.START_POINT)]
        public virtual string StartPoint { get; set; }
        [StringLength(AddRoadwayNotificationStreetsForBug2596.StringLengths.TERMINUS)]
        public virtual string Terminus { get; set; }

        [DropDown, EntityMustExist(typeof(MainSize)), EntityMap]
        public virtual int? MainSize { get; set; }
        //maintype
        [DropDown, EntityMustExist(typeof(MainType)), EntityMap]
        public virtual int? MainType { get; set; }
        //status
        [Required, DropDown, EntityMustExist(typeof(RoadwayImprovementNotificationStreetStatus)), EntityMap]
        public virtual int? RoadwayImprovementNotificationStreetStatus { get; set; }
        //mainbreakactvity
        [Range(typeof(int), "0", "99")]
        public virtual int? MainBreakActivity { get; set; }
        //numberofservices
        [Range(typeof(int), "0", "99")]
        public virtual int? NumberOfServicesToBeReplaced { get; set; }
        public virtual int? OpenWorkOrders { get; set; }

        [DoesNotAutoMap("Used for cascade only")]
        public virtual int Town { get; set; }

        [DoesNotAutoMap("Don't know where this is even used")]
        public virtual string TownText { get; set; }

        public virtual DateTime? MoratoriumEndDate { get; set; }

        [Multiline]
        public virtual string Notes { get; set; }

        #endregion

        #region Constructors

        public EditRoadwayImprovementNotificationStreet(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(RoadwayImprovementNotificationStreet entity)
        {
            base.Map(entity);
            Town = entity.Street.Town.Id;
        }

        #endregion
    }

    public class SearchRoadwayImprovementNotificationStreet : SearchSet<RoadwayImprovementNotificationStreet>
    {
        #region Properties

        [Display(Name = "Id")]
        public int? EntityId { get; set; }
        [DropDown, SearchAlias("RoadwayImprovementNotification", "OperatingCenter.Id")]
        public int? OperatingCenter { get; set; }
        [SearchAlias("RoadwayImprovementNotification", "Town.Id")]
        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? Town { get; set; }

        [SearchAlias("RoadwayImprovementNotification", "DateReceived")]
        public DateRange DateReceived { get; set; }

        [DropDown, DisplayName("Street Status")]
        public int? RoadwayImprovementNotificationStreetStatus { get; set; }
        
        #endregion
    }
}