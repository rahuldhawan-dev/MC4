using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder
{
    public class EditWorkOrderAdditional : ViewModel<WorkOrder>, IWorkOrderAdditional
    {
        #region Constructor

        public EditWorkOrderAdditional(IContainer container) : base(container) { }

        #endregion

        #region Fields

        private WorkOrder _original;

        #endregion

        #region Properties

        [DoesNotAutoMap]
        public WorkOrder WorkOrder
        {
            get
            {
                if (_original == null)
                {
                    _original = Original ?? _container.GetInstance<IRepository<WorkOrder>>().Find(Id);
                }
                return _original;
            }
        }

        [AutoMap(MapDirections.None)]
        public int? AssetTypeId => WorkOrder?.AssetType?.Id;

        [AutoMap(MapDirections.None)]
        public bool? IsRevisit => WorkOrder?.WorkDescription?.Revisit;

        [DropDown("FieldOperations", "WorkDescription", "ActiveByAssetTypeIdAndIsRevisit", DependsOn = nameof(AssetTypeId) + "," + nameof(IsRevisit)), 
         EntityMap("WorkDescription"), EntityMustExist(typeof(WorkDescription))]
        public int? FinalWorkDescription { get; set; }

        [RequiredWhen(nameof(FinalWorkDescription), ComparisonType.EqualToAny,
            nameof(MainBreakWorkDescriptions), typeof(EditWorkOrderAdditional))]
        public int? LostWater { get; set; }

        [RequiredWhen(nameof(StreetOpeningPermitRequired), true)]
        [RequiredWhen(nameof(WorkOrderPriority), ComparisonType.EqualTo, (int)MapCall.Common.Model.Entities.WorkOrderPriority.Indices.EMERGENCY)]
        public double? DistanceFromCrossStreet { get; set; }

        [RequiredWhen(nameof(FinalWorkDescription), ComparisonType.EqualToAny,
            nameof(MainBreakWorkDescriptions), typeof(EditWorkOrderAdditional))]
        [DropDown, EntityMap("EstimatedCustomerImpact"), EntityMustExist(typeof(CustomerImpactRange)), 
         View(WorkOrder.DisplayNames.ESTIMATED_CUSTOMER_IMPACT)]
        public int? CustomerImpact { get; set; }

        [RequiredWhen(nameof(FinalWorkDescription), ComparisonType.EqualToAny,
            nameof(MainBreakWorkDescriptions), typeof(EditWorkOrderAdditional))]
        [DropDown, EntityMap("AnticipatedRepairTime"), EntityMustExist(typeof(RepairTimeRange)), 
         View(WorkOrder.DisplayNames.ANTICIPATED_REPAIR_TIME)]
        public int? RepairTime { get; set; }

        [RequiredWhen(nameof(FinalWorkDescription), ComparisonType.EqualToAny,
            nameof(MainBreakWorkDescriptions), typeof(EditWorkOrderAdditional)), 
         View(WorkOrder.DisplayNames.SIGNIFICANT_TRAFFIC_IMPACT), AutoMap("SignificantTrafficImpact")]
        public bool? TrafficImpact { get; set; }

        public bool? AlertIssued { get; set; }

        [AutoMap(MapDirections.None)]
        public bool? StreetOpeningPermitRequired => WorkOrder?.StreetOpeningPermitRequired;

        [AutoMap(MapDirections.None)]
        public int? WorkOrderPriority => WorkOrder?.Priority?.Id;

        [Multiline, DoesNotAutoMap]
        public string AppendNotes { get; set; }

        #endregion

        #region Private Methods

        public static int[] MainBreakWorkDescriptions() => WorkDescription.GetMainBreakWorkDescriptions();

        #endregion

        #region Exposed Methods

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            base.MapToEntity(entity);

            if (!string.IsNullOrWhiteSpace(AppendNotes))
            {
                if (!string.IsNullOrWhiteSpace(entity.Notes))
                {
                    entity.Notes += Environment.NewLine;
                }

                entity.Notes += $"{_container.GetInstance<IAuthenticationService<User>>().CurrentUser.FullName} " +
                                _container.GetInstance<IDateTimeProvider>().GetCurrentDate().ToString(CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE_FOR_WEBFORMS) +
                                $" {AppendNotes}";
            }

            return entity;
        }

        #endregion
    }

    public interface IWorkOrderAdditional
    {
        int? FinalWorkDescription { get; set; }

        int? LostWater { get; set; }

        double? DistanceFromCrossStreet { get; set; }

        int? CustomerImpact { get; set; }

        int? RepairTime { get; set; }

        bool? TrafficImpact { get; set; }

        bool? AlertIssued { get; set; }

        string AppendNotes { get; set; }
    }
}
