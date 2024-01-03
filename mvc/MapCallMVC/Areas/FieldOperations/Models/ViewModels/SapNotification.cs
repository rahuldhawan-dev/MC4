using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MapCall.SAP.Model.Entities;
using StructureMap;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    //TODO: make sure sapnotification number goes along for the ride and gets added to the work order. this is what closes it in sap
    
    public class CreateSapNotificationWorkOrder : ViewModel<SAPNotification>
    {
        #region Private Members

        private Dictionary<string, string> _purposes;

        #endregion

        #region Properties

        [DoesNotAutoMap("SAPNotification doesn't have an Id property.")]
        public override int Id { get => base.Id; set => base.Id = value; }

        public virtual string PlanningPlant { get; set; }
        public virtual string NotificationType { get; set; }
        [View(DisplayName = "NotificationType")]
        public virtual string NotificationTypeText { get; set; }
        [View(DisplayName = "Notification")]
        public virtual string SAPNotificationNumber { get; set; }
        [View(DisplayName = "Description")]
        public virtual string NotificationShortText { get; set; }
        public virtual string NotificationLongText { get; set; }
        public virtual string SpecialInstructions { get; set; }
        public virtual string ReportedBy { get; set; }
        public virtual string CustomerName { get; set; }
        public virtual string AccountNumberOfCustomer { get; set; }
        public virtual string Telephone { get; set; }
        public virtual string FunctionalLocation { get; set; }
        public virtual string Installation { get; set; }
        public virtual string Locality { get; set; }
        public virtual string LocalityDescription { get; set; }
        public virtual string Equipment { get; set; }
        public virtual string Premise { get; set; }
        public virtual string CodingGroupCodeDescription { get; set; }
        public virtual string DateCreated { get; set; }
        public virtual string TimeCreated { get; set; }
        public virtual string Priority { get; set; }
        public virtual string AddressNumber { get; set; }
        public virtual string House { get; set; }
        public virtual string Street1 { get; set; }
        public virtual string Street2 { get; set; }
        public virtual string Street5 { get; set; }
        public virtual string City { get; set; }
        public virtual string OtherCity { get; set; }
        public virtual string State { get; set; }
        public virtual string Country { get; set; }
        public virtual string CityPostalCode { get; set; }
        public virtual string Latitude { get; set; }
        public virtual string Longitude { get; set; }
        public virtual string SAPErrorCode { get; set; }
        public virtual string UserStatus { get; set; }
        public virtual string SystemStatus { get; set; }
        public virtual string AssetType { get; set; }
        //logical properties
        [DisplayName("Priority")]
        public virtual string MapCallPriority { get; set; }
        public virtual string Purpose => GetPurpose();
        public virtual string PurposeCodingCode { get; set; }

        [DoesNotAutoMap]
        public Hydrant Hydrant { get; set; }
        [DoesNotAutoMap]
        public Valve Valve { get; set; }
        [DoesNotAutoMap]
        public SewerOpening SewerOpening { get; set; }

        [DoesNotAutoMap]
        public int OpenWorkOrderCount { get; set; }

        [AutoMap(MapDirections.None)]
        public virtual SearchSapNotification IndexSearch { get; set; }

        [DoesNotAutoMap("Property is unreferenced?")]
        [Multiline, View(DisplayName = "Remarks"), DisplayName("Remarks")]
        public string NotificationRemarks { get; set; }

        #endregion

        #region Constructors

        public CreateSapNotificationWorkOrder(IContainer container) : base(container)
        {
            _purposes = new Dictionary<string, string> {
                {"I01", "Customer"},
                {"I02", "Equip Reliability"},
                {"I03", "Safety"},
                {"I04", "Compliance"},
                {"I05", "Regulatory"},
                {"I06", "Seasonal"},
                {"I07", "Leak Detection"},
                {"I08", "Revenue 150-500"},
                {"I09", "Revenue 500-1000"},
                {"I10", "Revenue >1000"},
                {"I11", "Damaged/Billable"},
                {"I12", "Estimates"},
                {"I13", "Water Quality"},
                {"I14", "Asset Record Control"},
                {"I15", "Demolition"},
                {"I16", "Locate"},
                {"I17", "Clean Out"},
                {"DV02", "Construction Project"}
            };
        }

        #endregion

        #region Exposed Methods
        
        //H - Hydrant; 
        //V - Valve; 
        //D - Main(either FL category or Equipment category with class DISTSYS)
        //C - Sewer Main(FL category)
        //4 - Sewer Main(Equipment with class GMAIN)
        //4 - Sewer Opening(Equipment with class MH)
        //G - Service(FL category For Device Location)
        //I - Service(Equipment cateogry for meter or IS-U Equipment)
        protected CreateWorkOrder TrySetAsset()
        {
            if (!String.IsNullOrWhiteSpace(Equipment))
            {
                var equipmentId = int.Parse(Equipment.TrimStart('0'));
                if (AssetType == "HYDRANT")
                {
                    Hydrant =
                        _container.GetInstance<IRepository<Hydrant>>()
                            .Where(x => x.SAPEquipmentId == equipmentId).FirstOrDefault();
                    if (Hydrant != null)
                    {
                        if (Hydrant.WorkOrders != null)
                            OpenWorkOrderCount = Hydrant.WorkOrders.Count(x => x.DateCompleted == null && x.CancelledAt == null);
                        return new CreateWorkOrder(_container, Hydrant);
                    }
                }
                else if (AssetType == "VALVE")
                {
                    Valve =
                        _container.GetInstance<IRepository<Valve>>()
                            .Where(x => x.SAPEquipmentId == equipmentId).FirstOrDefault();
                    if (Valve != null)
                    {
                        if (Valve.WorkOrders != null)
                            OpenWorkOrderCount = Valve.WorkOrders.Count(x => x.DateCompleted == null && x.CancelledAt == null);
                        return new CreateWorkOrder(_container, Valve);
                    }
                }
                else if(AssetType == "SEWER OPENING")
                {
                    SewerOpening =
                        _container.GetInstance<IRepository<SewerOpening>>()
                            .Where(x => x.SAPEquipmentId == equipmentId).FirstOrDefault();
                    if (SewerOpening != null)
                    {
                        if (SewerOpening.WorkOrders != null)
                            OpenWorkOrderCount = SewerOpening.WorkOrders.Count(x => x.DateCompleted == null && x.CancelledAt == null);
                        return new CreateWorkOrder(_container, SewerOpening);
                    }
                }
            }
            return null;
        }

        //TODO: test and clean up duplication
        /// <summary>
        /// We are taking an SAPNotification and turning it into a CreateWorkOrder, this is so
        /// that we can pre-populate the New work order screen for the user.
        /// </summary>
        /// <returns></returns>
        public CreateWorkOrder ToCreateWorkOrder()
        {
            var priority = _container.GetInstance<IRepository<WorkOrderPriority>>().Where(x => x.Description == MapCallPriority).FirstOrDefault();
            var purpose = _container.GetInstance<IRepository<WorkOrderPurpose>>().Where(x => x.Description == Purpose).FirstOrDefault();

            var createWorkOrder = TrySetAsset();
            if (createWorkOrder != null)
            {
                if (SAPNotificationNumber != null)
                    createWorkOrder.SAPNotificationNumber = long.Parse(SAPNotificationNumber);
                createWorkOrder.Priority = priority?.Id;
                createWorkOrder.Purpose = purpose?.Id;
                createWorkOrder.RequestedBy = (purpose?.Id == 1) ? 1 : (int?)null;
                createWorkOrder.CustomerName = CustomerName;
                createWorkOrder.PhoneNumber = Telephone;
                //createWorkOrder.Notes = NotificationLongText;
                return createWorkOrder;
            }
            
            var planningPlant =_container.GetInstance<IRepository<PlanningPlant>>().Where(x => PlanningPlant == x.Code).FirstOrDefault();
            var assetType = _container.GetInstance<IRepository<AssetType>>().Where(x => x.Description.ToUpper() == AssetType.ToUpper()).FirstOrDefault();
            createWorkOrder = new CreateWorkOrder(_container) {
                OperatingCenter = planningPlant?.OperatingCenter?.Id,
                AssetType = assetType?.Id,
                Purpose = purpose?.Id,
                Priority = priority?.Id
            };
            if ((assetType.Id == MapCall.Common.Model.Entities.AssetType.Indices.SERVICE ||
                 assetType.Id == MapCall.Common.Model.Entities.AssetType.Indices.SEWER_LATERAL) &&
                !string.IsNullOrWhiteSpace(FunctionalLocation) &&
                long.TryParse(FunctionalLocation, out var location))
            {
                createWorkOrder.DeviceLocation = location;
            }

            if (planningPlant != null)
            {
                var operatingcenter = planningPlant.OperatingCenter;
                var town = operatingcenter.Towns.FirstOrDefault(x => x.ShortName.ToUpper() == City?.ToUpper());
                if (town == null)
                {
                    if (AssetType.ToUpper() == "MAIN" && !string.IsNullOrWhiteSpace(Equipment))
                        town = operatingcenter.OperatingCenterTowns?.FirstOrDefault(x => x.MainSAPEquipmentId == int.Parse(Equipment.TrimStart('0')))?.Town;
                    if (AssetType.ToUpper() == "SEWER MAIN" && !string.IsNullOrWhiteSpace(Equipment))
                        town = operatingcenter.OperatingCenterTowns?.FirstOrDefault(x => x.SewerMainSAPEquipmentId == int.Parse(Equipment.TrimStart('0')))?.Town;
                }
                var street = town?.Streets.FirstOrDefault(x => x.FullStName.ToUpper() == Street1?.ToUpper());
                var crossStreet = town?.Streets.FirstOrDefault(x => x.FullStName.ToUpper() == Street2?.ToUpper());
                // really? Street5???
                var townSection =
                    town?.TownSections.FirstOrDefault(x => x.Description.ToUpper() == Street5?.ToUpper());

                var order = new CreateWorkOrder(_container) {
                    OperatingCenter = planningPlant.OperatingCenter.Id,
                    Town = town?.Id,
                    TownSection = townSection?.Id,
                    StreetNumber = House,
                    Street = street?.Id,
                    NearestCrossStreet = crossStreet?.Id,
                    ZipCode = CityPostalCode,
                    AssetType = assetType?.Id,
                    RequestedBy = (purpose?.Id == 1) ? 1 : (int?)null,
                    CustomerName = CustomerName,
                    PhoneNumber = Telephone,
                    // Latitude = Latitude // Go create a coordinate?
                    // Longitude = Longitude
                    PremiseNumber = Premise,
                    SAPNotificationNumber = long.Parse(SAPNotificationNumber),
                    // SAPEquipmentId = Equipment, // this isn't a field on work order.
                    // FunctionalLocation = functionalLocation?.Id,
                    // Purpose?
                    Priority = priority?.Id,
                    Purpose = purpose?.Id,
                    //Notes = NotificationLongText,
                };
                if (!string.IsNullOrWhiteSpace(Installation) &&
                    long.TryParse(Installation, out var installation))
                {
                    order.Installation = installation;
                }

                if ((assetType.Id == MapCall.Common.Model.Entities.AssetType.Indices.SERVICE ||
                     assetType.Id == MapCall.Common.Model.Entities.AssetType.Indices.SEWER_LATERAL) &&
                    !string.IsNullOrWhiteSpace(FunctionalLocation) &&
                    long.TryParse(FunctionalLocation, out var l))
                {
                    order.DeviceLocation = l;
                }
                if (!string.IsNullOrWhiteSpace(Latitude))
                    order.Latitude = decimal.Parse(Latitude);
                if (!string.IsNullOrWhiteSpace(Longitude))
                    order.Longitude = decimal.Parse(Longitude);

                return order;
            }
            return createWorkOrder;
        }

        protected string GetPurpose()
        {
            return (PurposeCodingCode != null && _purposes.TryGetValue(PurposeCodingCode, out var purpose)) ? purpose : string.Empty;
        }

        public override string ToString()
        {
            return SAPNotificationNumber;
        }

        #endregion
    }

    public class SearchSapNotification
    {
        // This isn't used in the search page, just to mark it in the results.
        public string SAPNotificationNumber { get; set; }

        // THIS IS SAP, So we send string values along to their web service
        [Required, MultiSelect]
        public string[] PlanningPlant { get; set; }
        [Required]
        public DateTime? DateCreatedFrom { get; set; }
        [Required]
        public DateTime? DateCreatedTo { get; set; }
        // THIS IS SAP, So we send string values along to their web service
        [Required, MultiSelect]
        public string[] NotificationType { get; set; }

        // THIS IS SAP, So we send string values along to their web service
        [DropDown]
        public virtual string Priority { get; set; }
        public virtual string CodeGroup => (string.IsNullOrWhiteSpace(Code) ? null : "N-D-PUR1");
        // THIS IS SAP, So we send string values along to their web service
        [DropDown, View(DisplayName = "Purpose")]
        public virtual string Code { get; set; }

        //SAP want's all strings so lets provide them with that.
        public MapCall.Common.Model.ViewModels.SearchSapNotification ToSearchSapNotification()
        {
            return new MapCall.Common.Model.ViewModels.SearchSapNotification {
                PlanningPlant = string.Join(",", PlanningPlant),
                DateCreatedTo = $"{DateCreatedTo:yyyyMMdd}",
                DateCreatedFrom = $"{DateCreatedFrom:yyyyMMdd}",
                NotificationType = string.Join(",", NotificationType),
                Priority = Priority,
                Code = Code,
                SAPNotificationNo = SAPNotificationNumber
            };
        }
    }

    /// <summary>
    /// NOTE: This ViewModel isn't used as a proper ViewModel by SAPNotificationController. -Ross 12/27/2017
    /// </summary>
    public class EditSapNotification : ViewModel<SAPNotification>
    {
        #region Properties

        public EditSapNotification(IContainer container) : base(container) { }

        [DoesNotAutoMap("SAP entities don't have Id properties")]
        public override int Id { get => base.Id; set => base.Id = value; }

        public virtual string SAPNotificationNumber { get; set; }

        [DoesNotAutoMap]
        public virtual string Complete { get; set; }
        [DoesNotAutoMap]
        public virtual string Cancel { get; set; }

        [AutoMap(MapDirections.ToViewModel)]
        public virtual string Remarks { get; set; }

        [AutoMap(MapDirections.None)]
        public virtual string IndexSearch { get; set; }

        /// <summary>
        /// Used for redirecting back to WaterQualityComplaints/Show page.
        /// </summary>
        [DoesNotAutoMap]
        public string RedirectUrl { get; set; }

        /// <summary>
        /// WaterQualityComplaints needs to append its Notes to the Remarks field when updating an SAP record.
        /// </summary>
        [DoesNotAutoMap]
        public string ReadOnlyNotes { get; set; }

        #endregion
    }
}