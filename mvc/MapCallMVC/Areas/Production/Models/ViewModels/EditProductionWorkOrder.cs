using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class EditProductionWorkOrder : ProductionWorkOrderViewModel
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(ProductionWorkOrderCancellationReason))]
        [RequiredWhen("DateCancelled", ComparisonType.NotEqualTo, null)]
        public int? CancellationReason { get; set; }

        public DateTime? ApprovedOn { get; set; }
        public DateTime? MaterialsApprovedOn { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DateTime? DateCancelled { get; set; }

        [DoesNotAutoMap("Set in MapToEntity, used by controller")]
        public bool ProgressWorkOrder { get; set; }

        [DoesNotAutoMap("Set in MapToEntity, used by controller")]
        public bool FinalizeWorkOrder { get; set; }

        [DoesNotAutoMap("Set in MapToEntity, used by controller")]
        public bool CreateWorkOrder { get; set; }

        [DropDown(Area = "", Controller = "Facility", Action = "ByOperatingCenterAndSometimesPlanningPlantDisplayNameAndId", DependsOn = "OperatingCenter,PlanningPlant", PromptText = "Please select an operating center above", DependentsRequired = DependentRequirement.One)]
        public override int? Facility { get; set; }

        [DropDown(Area = "", Controller = "Equipment", Action = "ByFacilityIdAndOrFacilityFacilityAreaIdAndOrEquipmentTypeId", DependsOn = "Facility,FacilityFacilityArea,EquipmentType", PromptText = "Select a facility above.", DependentsRequired = DependentRequirement.One)]
        [RequiredWhen("ProductionWorkDescription", ComparisonType.NotEqualToAny, "GetPMWorkDescriptionIds", typeof(ProductionWorkOrderViewModel))]
        public int? Equipment { get; set; }

        [DoesNotExport, AutoMap(MapDirections.None)]
        public ProductionWorkOrder DisplayWorkOrder { get; set; }

        [CheckBoxList]
        [EntityMap(MapDirections.None), EntityMustExist(typeof(ProductionPrerequisite))]
        public virtual int[] Prerequisites { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }

        #endregion

        #region Private Methods

        private void TrySetPrerequisites(ProductionWorkOrder entity)
        {
            if (Prerequisites == null)
            {
                return;
            }

            var currentlyAssignedPrerequisites = entity.ProductionWorkOrderProductionPrerequisites
                                                       .Select(x => x.ProductionPrerequisite.Id)
                                                       .ToArray();

            var prerequisitesRepo = _container.GetInstance<IRepository<ProductionPrerequisite>>();
            foreach (var prerequisiteId in Prerequisites)
            {
                var prerequisiteFromViewModel = prerequisitesRepo.Find(prerequisiteId);
                if (currentlyAssignedPrerequisites.Contains(prerequisiteFromViewModel.Id))
                {
                    continue;
                }

                // MC-3123 - Red Tag Permits are only supported when Equipment is not null.
                if (prerequisiteId == ProductionPrerequisite.Indices.RED_TAG_PERMIT && Equipment == null)
                {
                    continue;
                }

                entity.ProductionWorkOrderProductionPrerequisites.Add(
                    new ProductionWorkOrderProductionPrerequisite {
                        ProductionWorkOrder = entity,
                        ProductionPrerequisite = prerequisiteFromViewModel
                    });
            }
        }

        #endregion

        #region Constructors

        public EditProductionWorkOrder(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override ProductionWorkOrder MapToEntity(ProductionWorkOrder entity)
        {
            TrySetPrerequisites(entity);

            if (entity.CanBeCancelled && CancellationReason != null && DateCancelled != null)
            {
                entity.CancelledBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            }

            if (entity.ApprovedOn.HasValue || entity.DateCancelled.HasValue || !entity.OperatingCenter.CanSyncWithSAP)
                return base.MapToEntity(entity);

            if (string.IsNullOrWhiteSpace(entity.SAPWorkOrder) && !entity.SAPNotificationNumber.HasValue)
            {
                CreateWorkOrder = true;
                return base.MapToEntity(entity);
            }

            // FINALIZE WORK ORDER
            if (!entity.ApprovedOn.HasValue && ApprovedOn.HasValue)
            {
                FinalizeWorkOrder = true;
                return base.MapToEntity(entity);
            }

            // IF WE'VE COME THIS FAR, WE CALL PROGRESS
            ProgressWorkOrder = true;

            if (Equipment != null)
            {
                var equipment = _container.GetInstance<RepositoryBase<Equipment>>().Find(Equipment.Value);
                if (entity.Equipment != null)
                {
                    var obj = entity.Equipments.Where(x => x.Equipment == entity.Equipment).FirstOrDefault();
                    if (obj != null) obj.Equipment = equipment;
                }
            }

            //Set default Functional location
            entity.FunctionalLocation = entity.Facility?.FunctionalLocation;

            return base.MapToEntity(entity);
        }

        public override void Map(ProductionWorkOrder entity)
        {
            base.Map(entity);
            DisplayWorkOrder = entity;
            RequestedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser?.Employee?.Id ??
                          RequestedBy;
        }

        #endregion
    }
}