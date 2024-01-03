using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using System;
using System.ComponentModel.DataAnnotations;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class CreateProductionWorkOrder : ProductionWorkOrderViewModel
    {
        #region Properties

        [StringLength(ProductionWorkOrder.StringLengths.SAP_WORK_ORDER)]
        public string SAPWorkOrder { get; set; }

        [Required, DateTimePicker]
        public DateTime? DateReceived { get; set; }

        [Display(Description = "Create a crew assignment for yourself for this order?")]
        [AutoMap(MapDirections.None)]
        public bool AssignToSelf { get; set; }

        [AutoMap(MapDirections.None)]
        public decimal? Latitude { get; set; }

        [AutoMap(MapDirections.None)]
        public decimal? Longitude { get; set; }

        [EntityMap, EntityMustExist(typeof(ProductionWorkOrder))]
        public int? CapitalizedFrom { get; set; }

        [DropDown(Area = "", Controller = "Facility", Action = "UnarchivedByOperatingCenterAndSometimesPlanningPlantDisplayNameAndId", DependsOn = "OperatingCenter,PlanningPlant", PromptText = "Please select an operating center above", DependentsRequired = DependentRequirement.One)]
        public override int? Facility { get; set; }

        [DropDown(Area = "", Controller = "FacilityFacilityArea", Action = "ByFacilityId", DependsOn = "Facility", PromptText = "Please select Facility above")]
        [View("Facility Area")]
        public override int? FacilityFacilityArea { get; set; }

        [DropDown(Area = "", Controller = "Equipment", Action = "GetActiveInServiceSAPEquipmentByFacilityIdOrFacilityFacilityAreaIdOrEquipmentTypeId", DependsOn = "Facility,FacilityFacilityArea,EquipmentType", PromptText = "Select a facility above.", DependentsRequired = DependentRequirement.One)]
        
        // Equipment is logical property and not a table property in production work order entity.
        // All equipments related to pwo are stored in ProductionWorkOrderEquipment table.
        // This is mapped in MapToEntity method.
        [DoesNotAutoMap, EntityMustExist(typeof(Equipment))]
        [RequiredWhen("ProductionWorkDescription", ComparisonType.NotEqualToAny, "GetPMWorkDescriptionIds", typeof(ProductionWorkOrderViewModel))]
        public int? Equipment { get; set; }
        
        [CheckBoxList]
        [EntityMap(MapDirections.None), EntityMustExist(typeof(ProductionPrerequisite))]
        public virtual int[] Prerequisites { get; set; }

        #endregion

        #region Constructors

        [DefaultConstructor]
        public CreateProductionWorkOrder(IContainer container) : base(container) { }

        public CreateProductionWorkOrder(IContainer container, Equipment equipment) : this(container)
        {
            OperatingCenter = equipment.OperatingCenter?.Id;
            Facility = equipment.Facility.Id;
            FacilityFacilityArea = equipment.FacilityFacilityArea?.Id;
            Equipment = equipment.Id;
            EquipmentType = equipment.EquipmentType?.Id;
            PlanningPlant = equipment.Facility.PlanningPlant?.Id;
            FunctionalLocation = equipment.FunctionalLocation;
            if (equipment.Coordinate != null)
            {
                Coordinate = equipment.Coordinate.Id;
            }
            else if (equipment.Facility.Coordinate != null)
            {
                Coordinate = equipment.Facility.Coordinate.Id;
            }
        }

        #endregion

        #region Private Methods

        private void TrySetPrerequisites(ProductionWorkOrder entity)
        {
            if (Prerequisites == null)
            {
                return;
            }
            
            var prerequisitesRepo = _container.GetInstance<IRepository<ProductionPrerequisite>>();
            foreach (var prerequisiteId in Prerequisites)
            {
                // MC-3123 - Red Tag Permits are only supported when Equipment is not null.
                if (prerequisiteId == ProductionPrerequisite.Indices.RED_TAG_PERMIT && 
                    Equipment == null)
                {
                    continue;
                }

                entity.ProductionWorkOrderProductionPrerequisites.Add(
                    new ProductionWorkOrderProductionPrerequisite {
                        ProductionWorkOrder = entity,
                        ProductionPrerequisite = prerequisitesRepo.Find(prerequisiteId)
                    });
            }
        }

        #endregion

        #region Exposed Methods

        public override void SetDefaults()
        {
            base.SetDefaults();
            DateReceived = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            RequestedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser?.Employee?.Id ??
                          RequestedBy;
        }

        public override ProductionWorkOrder MapToEntity(ProductionWorkOrder entity)
        {
            entity = base.MapToEntity(entity);

            entity.DateReceived = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();

            TrySetPrerequisites(entity);

            //todo: move to a private method?
            if (AssignToSelf)
            {
                var employee = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.Employee;
                if (employee != null)
                {
                    entity.EmployeeAssignments.Add(new EmployeeAssignment {
                        AssignedFor = _container.GetInstance<IDateTimeProvider>().GetCurrentDate(),
                        AssignedOn = _container.GetInstance<IDateTimeProvider>().GetCurrentDate(),
                        AssignedBy = employee,
                        AssignedTo = employee,
                        ProductionWorkOrder = entity
                    });
                }
            }

            if (Equipment != null)
            {
                var equipment = _container.GetInstance<RepositoryBase<Equipment>>().Find(Equipment.Value);
                entity.Equipments.Add(new ProductionWorkOrderEquipment {
                    ProductionWorkOrder = entity,
                    Equipment = equipment,
                    IsParent = true
                });
            }

            //Set default Functional location
            entity.FunctionalLocation = entity.Facility?.FunctionalLocation;
            
            return entity;
        }

        #endregion
    }

    public class SearchProductionWorkOrderPerformance : SearchSet<ProductionWorkOrderPerformanceResultViewModel>, ISearchProductionWorkOrderPerformance
    {
        #region Private Members

        private int[] _state;
        private int[] _operatingCenter;
        private int[] _planningPlant;
        private int[] _facility;
        private int[] _orderType;

        #endregion

        #region Properties

        [Search(CanMap = false)] // This is used by the controller action to determine pdf rendering.
        public bool? NoPdf { get; set; }

        [MultiSelect, EntityMustExist(typeof(State))]
        public int[] State
        {
            get => _state;
            set => _state = value ?? new int[0];
        }

        [MultiSelect("", "OperatingCenter", "ByStateIds", DependsOn = "State")]
        [EntityMustExist(typeof(OperatingCenter))]
        public int[] OperatingCenter
        {
            get => _operatingCenter;
            set => _operatingCenter = value ?? new int[0];
        }

        [MultiSelect("", "PlanningPlant", "ByOperatingCenters", DependsOn = "OperatingCenter")]
        [EntityMustExist(typeof(PlanningPlant))]
        public int[] PlanningPlant
        {
            get => _planningPlant;
            set => _planningPlant = value ?? new int[0];
        }

        [MultiSelect("", "Facility", "ByPlanningPlants", DependsOn = "PlanningPlant"),
         EntityMustExist(typeof(Facility))]
        public int[] Facility
        {
            get => _facility;
            set => _facility = value ?? new int[0];
        }

        [Required]
        public RequiredDateRange DateReceived { get; set; }

        [MultiSelect, EntityMustExist(typeof(OrderType))]
        public int[] OrderType
        {
            get => _orderType;
            set => _orderType = value ?? new int[0];
        }

        [UIHint("StringArray")]
        public string[] SelectedFacilities { get; set; }
        [UIHint("StringArray")]
        public string[] SelectedOrderTypes { get; set; }

        [UIHint("StringArray")]
        public string[] SelectedStates { get; set; }

        [UIHint("StringArray")]
        public string[] SelectedOperatingCenters { get; set; }

        [UIHint("StringArray")]
        public string[] SelectedPlanningPlants { get; set; }

        #endregion

        #region Constructors

        public SearchProductionWorkOrderPerformance()
        {
            State = State;
            OperatingCenter = OperatingCenter;
            PlanningPlant = PlanningPlant;
            Facility = Facility;
            OrderType = OrderType;
        }

        #endregion
    }
}
