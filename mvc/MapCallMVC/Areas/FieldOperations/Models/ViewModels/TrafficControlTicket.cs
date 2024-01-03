using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class TrafficControlTicketViewModel : ViewModel<TrafficControlTicket>
    {
        #region Properties

        [Required, DateTimePicker]
        public DateTime? WorkStartDate { get; set; }
        [DateTimePicker]
        public DateTime? WorkEndDate { get; set; }

        [Required]
        public string StreetNumber { get; set; }

        [Required]
        public int? SAPWorkOrderNumber { get; set; }

        [Required, DropDown, EntityMap]
        public virtual int? OperatingCenter { get; set; }

        [EntityMap, EntityMustExist(typeof(WorkOrder)), DisplayName("Work Order ID")]
        public virtual int? WorkOrder { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter"), Required, EntityMap]
        [EntityMustExist(typeof(Town))]
        public virtual int? Town { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(BillingParty)), ComboBox]
        public virtual int? BillingParty { get; set; }

        [Required]
        public virtual decimal? TotalHours { get; set; }

        [Required]
        public virtual int? NumberOfOfficers { get; set; }

        [StringLength(CreateTrafficControlTicketsForBug2341.StringLengths.TrafficControlTickets.ACCOUNTING_CODE)]
        public virtual string AccountingCode { get; set; }

        [DisplayName("Operating Center"), EntityMap(MapDirections.ToPrimary, SecondaryPropertyName = "WorkOrder")]
        public virtual OperatingCenter OperatingCenterDisplay { get; set; }

        [DisplayName("Work Order"), EntityMap(MapDirections.ToPrimary, SecondaryPropertyName = "WorkOrder")]
        public virtual WorkOrder WorkOrderDisplay { get; set; }

        [DisplayName("Town"), EntityMap(MapDirections.ToPrimary, SecondaryPropertyName = "Town")]
        public virtual Town TownDisplay { get; set; }

        [DisplayName("Street"), EntityMap(MapDirections.ToPrimary, SecondaryPropertyName = "Street")]
        public virtual Street StreetDisplay { get; set; }

        [DisplayName("Cross Street"), EntityMap(MapDirections.ToPrimary, SecondaryPropertyName = "CrossStreet")]
        public virtual Street CrossStreetDisplay { get; set; }

        [DisplayName("Coordinate"), EntityMap(MapDirections.ToPrimary, SecondaryPropertyName = "Coordinate")]
        public virtual Coordinate CoordinateDisplay { get; set; }

        [Coordinate(IconSet = IconSets.SingleDefaultIcon, AddressCallback =  "TrafficControlTicket.getAddress"), EntityMap, EntityMustExist(typeof(Coordinate))]
        public virtual int? Coordinate{ get; set; }

        public virtual string TrafficControlTicketNotes { get; set; }

        public virtual bool? PaidByNJAW { get; set; }

        #endregion

        #region Constructors

        public TrafficControlTicketViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateTrafficControlTicket : TrafficControlTicketViewModel
    {
        #region Properties


        [DropDown("", "Street", "GetActiveByTownId", DependsOn = "Town"), Required, EntityMap, EntityMustExist(typeof(Street))]
        public virtual int? Street { get; set; }

        [DropDown("", "Street", "GetActiveByTownId", DependsOn = "Town"), EntityMap, EntityMustExist(typeof(Street))]
        public virtual int? CrossStreet { get; set; }

        #endregion

        #region Constructors

        [DefaultConstructor]
        public CreateTrafficControlTicket(IContainer container) : base(container)
        {
            WorkStartDate = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
        }

        public CreateTrafficControlTicket(IContainer container, int? workOrderId) : this(container)
        {
            if (workOrderId != null)
            {
                PopulateFields(workOrderId.Value);
            }
        }

        #endregion

        #region Private Methods

        private void PopulateFields(int id)
        {
            var workOrder = _container.GetInstance<IRepository<WorkOrder>>().Find(id);

            WorkOrder = workOrder.Id;
            WorkOrderDisplay = workOrder;

            OperatingCenter = workOrder.OperatingCenter.Id;
            OperatingCenterDisplay = workOrder.OperatingCenter;

            Town = workOrder.Town.Id;
            TownDisplay = workOrder.Town;

            Street = workOrder.Street.Id;
            StreetDisplay = workOrder.Street;

            CrossStreet = workOrder.NearestCrossStreet.Id;
            CrossStreetDisplay = workOrder.NearestCrossStreet;

            AccountingCode = workOrder.AccountCharged;
            StreetNumber = workOrder.StreetNumber;

            var icon = _container.GetInstance<IRepository<MapIcon>>().Find(5);
            var coordinate =
                _container.GetInstance<IRepository<Coordinate>>()
                    .Save(new Coordinate {
                                    Latitude = (decimal)workOrder.Latitude, 
                                    Longitude = (decimal)workOrder.Longitude,
                                    Icon = icon});

            Coordinate = coordinate.Id;

            if (workOrder.SAPWorkOrderNumber.HasValue)
            {
                SAPWorkOrderNumber = (int)workOrder.SAPWorkOrderNumber.Value;
            }
        }

        #endregion

        #region ExposedMethods

        public override TrafficControlTicket MapToEntity(TrafficControlTicket entity)
        {
            base.MapToEntity(entity);
            entity.MerchantTotalFee = _container.GetInstance<IRepository<MerchantTotalFee>>().Where(x => x.IsCurrent).Single();
            return entity;
        }

        #endregion
    }

    public class EditTrafficControlTicket : TrafficControlTicketViewModel
    {
        #region Properties
        
        [DropDown("", "Street", "ByTownId", DependsOn = "Town"), Required, EntityMap, EntityMustExist(typeof(Street))]
        public virtual int? Street { get; set; }

        [DropDown("", "Street", "ByTownId", DependsOn = "Town"), EntityMap, EntityMustExist(typeof(Street))]
        public virtual int? CrossStreet { get; set; }

        [RequiredWhen("InvoiceAmount", ComparisonType.NotEqualTo, null)]
        [RequiredWhen("InvoiceDate", ComparisonType.NotEqualTo, null)]
        [StringLength(CreateTrafficControlTicketsForBug2341.StringLengths.TrafficControlTickets.INVOICE_NUMBER)]
        public virtual string InvoiceNumber { get; set; }
        [RequiredWhen("InvoiceNumber", ComparisonType.NotEqualTo, null)]
        [RequiredWhen("InvoiceDate", ComparisonType.NotEqualTo, null)]
        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY, ApplyFormatInEditMode = false)]
        public virtual decimal? InvoiceAmount { get; set; }
        [RequiredWhen("InvoiceNumber", ComparisonType.NotEqualTo, null)]
        [RequiredWhen("InvoiceAmount", ComparisonType.NotEqualTo, null)]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = false)]
        public virtual DateTime? InvoiceDate { get; set; }
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? DateApproved { get; set; }
        public virtual decimal? InvoiceTotalHours { get; set; }
        public virtual string TrackingNumber { get; set; }

        [AutoMap(MapDirections.None)]
        public virtual bool InvoiceAlreadyValid { get; set; }
        [AutoMap(MapDirections.None)]
        public virtual bool PaymentAlreadyReceived { get; set; }

        #endregion

        #region Constructors

		public EditTrafficControlTicket(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override TrafficControlTicket MapToEntity(TrafficControlTicket entity)
        {
            if (entity.DateApproved == null && DateApproved.HasValue)
            {
                entity.ApprovedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            }
            
            //if the entity.invoice isValid at this point, then it's not being updated, it's already been set.
            InvoiceAlreadyValid = entity.InvoiceValid;
            PaymentAlreadyReceived = entity.PaymentReceivedAt.HasValue;
            
            return base.MapToEntity(entity);
        }

        public override void Map(TrafficControlTicket entity)
        {
            base.Map(entity);

            if (entity.DateApproved == null)
            {
                DateApproved = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            }
        }

        #endregion
    }

    public class SearchTrafficControlTicket : SearchSet<TrafficControlTicket>
    {
        #region Properties

        [DisplayName("Id")]
        public virtual int? EntityId { get; set; }
        [DropDown, EntityMap]
        public virtual int? OperatingCenter { get; set; }
        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter")]
        public virtual int? Town { get; set; }
        [EntityMustExist(typeof(WorkOrder)), DisplayName("Work Order ID")]
        [SearchAlias("WorkOrder", "Id")]
        public virtual int? WorkOrder { get; set; }
        public int? SAPWorkOrderNumber { get; set; }
        public DateRange DateApproved { get; set; }
        public bool? HasInvoice { get; set; }
        [MultiSelect]
        public int[] Status { get; set; }

        [ComboBox]
        public int? BillingParty { get; set; }

        #endregion
    }
}