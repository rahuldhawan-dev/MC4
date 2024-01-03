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
using StructureMap;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class RoadwayImprovementNotificationViewModel : ViewModel<RoadwayImprovementNotification>
    {
        #region Properties

        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [Required, EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(RoadwayImprovementNotificationEntity))]
        public int? RoadwayImprovementNotificationEntity { get; set; }
        [Required, Multiline]
        public string Description { get; set; }
        [Required]
        public DateTime? ExpectedProjectStartDate { get; set; }
        [Required]
        public DateTime? DateReceived { get; set; }
        [Required, Coordinate(AddressCallback = "RoadwayImprovementNotification.getAddress", IconSet = IconSets.SingleDefaultIcon), EntityMap]
        [EntityMustExist(typeof(Coordinate))]
        public int? Coordinate { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(RoadwayImprovementNotificationStatus))]
        public int? RoadwayImprovementNotificationStatus { get; set; }
        public DateTime? PreconMeetingDate { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(RoadwayImprovementNotificationPreconAction))]
        public int? RoadwayImprovementNotificationPreconAction { get; set; }

        #endregion

        #region Constructors

        public RoadwayImprovementNotificationViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreateRoadwayImprovementNotification : RoadwayImprovementNotificationViewModel
    {
        #region Constructors

		public CreateRoadwayImprovementNotification(IContainer container) : base(container) {}

        #endregion
	}

    public class EditRoadwayImprovementNotification : RoadwayImprovementNotificationViewModel
    {
        #region Constructors

		public EditRoadwayImprovementNotification(IContainer container) : base(container) {}

        #endregion
	}

    public class SearchRoadwayImprovementNotification : SearchSet<RoadwayImprovementNotification>
    {
        #region Properties

        [DropDown]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? Town { get; set; }

        public DateRange ExpectedProjectStartDate { get; set; }
        public DateRange DateReceived { get; set; }
        
        public string Description { get; set; }
        
        [DropDown, DisplayName("Status")]
        public int? RoadwayImprovementNotificationStatus { get; set; }

        public DateTime? PreconMeetingDate { get; set; }

        [DropDown, DisplayName("Precon Action Taken")]
        public int? RoadwayImprovementNotificationPreconAction { get; set; }

        #endregion
	}

    public class AddRoadwayImprovementNotificationStreet : ViewModel<RoadwayImprovementNotification>
    {
        #region Properties

        [DropDown("Street", "GetActiveByTownId", DependsOn = "TownId", PromptText = "Please select a town above.", Area = "")]
        [Required, AutoMap(MapDirections.None), EntityMustExist(typeof(Street))]
        public virtual int? Street { get; set; }

        [Coordinate(AddressCallback = "Streets.getAddress", IconSet = IconSets.SingleDefaultIcon), AutoMap(MapDirections.None), EntityMustExist(typeof(Coordinate))]
        public virtual int? Coordinate { get; set; }

        [DoesNotAutoMap("Manually mapped")]
        [StringLength(AddRoadwayNotificationStreetsForBug2596.StringLengths.START_POINT)]
        public virtual string StartPoint { get; set; }

        [DoesNotAutoMap("Manually mapped")]
        [StringLength(AddRoadwayNotificationStreetsForBug2596.StringLengths.TERMINUS)]
        public virtual string Terminus { get; set; }
        
        [DropDown, DoesNotAutoMap, EntityMustExist(typeof(MainSize))]
        public virtual int? MainSize { get; set; }
        //maintype
        [DropDown, DoesNotAutoMap, EntityMustExist(typeof(MainType))]
        public virtual int? MainType { get; set; }
        //status
        [Required, DropDown, DoesNotAutoMap, EntityMustExist(typeof(RoadwayImprovementNotificationStreetStatus))]
        public virtual int? RoadwayImprovementNotificationStreetStatus { get; set; }
        //mainbreakactvity
        [AutoMap(MapDirections.None), Range(typeof(int), "0", "99")]
        public virtual int? MainBreakActivity { get; set; }
        //numberofservices
        [AutoMap(MapDirections.None), Range(typeof(int), "0", "99")]
        public virtual int? NumberOfServices { get; set; }

        [AutoMap(MapDirections.None)]
        public virtual int TownId { get; set; }

        [AutoMap(MapDirections.None)]
        public virtual string TownText { get; set; }

        [DoesNotAutoMap("Manually mapped")]
        public virtual DateTime? MoratoriumEndDate { get; set; }

        [Multiline]
        public virtual string Notes { get; set; }

        #endregion

        #region Exposed Methods

        public override RoadwayImprovementNotification MapToEntity(RoadwayImprovementNotification entity)
        {
            entity = base.MapToEntity(entity);

            var street = _container.GetInstance<IStreetRepository>().Find(Street.Value);
            var roadwayImprovementNotificationStreet = new RoadwayImprovementNotificationStreet {
                RoadwayImprovementNotification = entity,
                Street = street,
                MainBreakActivity = MainBreakActivity,
                NumberOfServicesToBeReplaced = NumberOfServices,
                Notes = Notes 
            };
            if (Coordinate.HasValue)
                roadwayImprovementNotificationStreet.Coordinate =
                    _container.GetInstance<IRepository<Coordinate>>().Find(Coordinate.Value);
            roadwayImprovementNotificationStreet.StartPoint = StartPoint;
            roadwayImprovementNotificationStreet.Terminus = Terminus;
            if (MainSize.HasValue)
                roadwayImprovementNotificationStreet.MainSize =
                    _container.GetInstance<IRepository<MainSize>>().Find(MainSize.Value);
            if (MainType.HasValue)
                roadwayImprovementNotificationStreet.MainType =
                    _container.GetInstance<IRepository<MainType>>().Find(MainType.Value);
            if (RoadwayImprovementNotificationStreetStatus.HasValue)
                roadwayImprovementNotificationStreet.RoadwayImprovementNotificationStreetStatus =
                    _container.GetInstance<IRepository<RoadwayImprovementNotificationStreetStatus>>()
                        .Find(RoadwayImprovementNotificationStreetStatus.Value);
            if (MoratoriumEndDate.HasValue)
                roadwayImprovementNotificationStreet.MoratoriumEndDate = MoratoriumEndDate.Value;
            entity.RoadwayImprovementNotificationStreets.Add(roadwayImprovementNotificationStreet);
            return entity;
        }

        #endregion

        #region Constructor

        public AddRoadwayImprovementNotificationStreet(IContainer container) : base(container) { }

        #endregion

    }

    public class RemoveRoadwayImprovementNotificationStreet : ViewModel<RoadwayImprovementNotification>
    {
        #region Properties

        [Required, DoesNotAutoMap("Mapped manually")]
        public virtual int RoadwayImprovementNotificationStreetId { get; set; }

        #endregion

        #region Constructors

        public RemoveRoadwayImprovementNotificationStreet(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override RoadwayImprovementNotification MapToEntity(RoadwayImprovementNotification entity)
        {
            // Do not call base.MapToEntity because there's nothing for it to do.
            entity.RoadwayImprovementNotificationStreets.Remove(
                _container.GetInstance<IRepository<RoadwayImprovementNotificationStreet>>().Find(RoadwayImprovementNotificationStreetId));
            return entity;
        }

        #endregion
    }
}