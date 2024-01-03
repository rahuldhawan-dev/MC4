using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using Newtonsoft.Json;

namespace MapCallScheduler.JobHelpers.GIS.Models
{
    public class Hydrant
    {
        #region Constants

        public static readonly int[] RELEVANT_WORK_DESCRIPTIONS = {
            (int)WorkDescription.Indices.HYDRANT_INSTALLATION,
            (int)WorkDescription.Indices.HYDRANT_RELOCATION,
            (int)WorkDescription.Indices.HYDRANT_REPLACEMENT,
            (int)WorkDescription.Indices.HYDRANT_RETIREMENT
        };

        #endregion

        #region Properties

        public int Id { get; set; }
        public HydrantBilling HydrantBilling { get; set; }
        public string HydrantNumber { get; set; }
        public DateTime? DateInstalled { get; set; }
        public AssetStatus Status { get; set; }
        public HydrantManufacturer HydrantManufacturer { get; set; }
        public string PremiseNumber { get; set; }
        public int? Route { get; set; }
        public int? SAPEquipmentId { get; set; }
        public decimal? Stop { get; set; }
        public DateTime? LastUpdated { get; set; }
        public User LastUpdatedBy { get; set; }
        public string WBSNumber => WorkOrder?.AccountCharged;
        public int? WorkOrderId => WorkOrder?.Id;
        public State State { get; set; }
        public Town Town { get; set; }
        public OperatingCenter OperatingCenter { get; set; }
        public string FunctionalLocation { get; set; }

        [JsonIgnore]
        public WorkOrder WorkOrder { get; set; }

        #endregion

        #region Exposed Methods

        public static Hydrant FromDbRecord(MapCall.Common.Model.Entities.Hydrant hydrant)
        {
            return new Hydrant {
                Id = hydrant.Id,
                HydrantBilling = HydrantBilling.FromDbRecord(hydrant.HydrantBilling),
                HydrantNumber = hydrant.HydrantNumber,
                DateInstalled = hydrant.DateInstalled.FromDbRecord(),
                Status = AssetStatus.FromDbRecord(hydrant.Status),
                HydrantManufacturer = HydrantManufacturer.FromDbRecord(hydrant.HydrantManufacturer),
                PremiseNumber = hydrant.FireDistrict == null ? null : hydrant.FireDistrict.PremiseNumber,
                Route = hydrant.Route,
                SAPEquipmentId = hydrant.SAPEquipmentId,
                Stop = hydrant.Stop,
                LastUpdated = hydrant.UpdatedAt.ToUniversalTime(),
                LastUpdatedBy = User.FromDbRecord(hydrant.UpdatedBy),
                WorkOrder = hydrant.WorkOrders.Where(wo => RELEVANT_WORK_DESCRIPTIONS.Contains(wo.WorkDescription.Id))
                                   .OrderByDescending(wo => wo.CreatedAt).FirstOrDefault(),
                State = State.FromDbRecord(hydrant),
                Town = Town.FromDbRecord(hydrant.Town),
                OperatingCenter = OperatingCenter.FromDbRecord(hydrant.OperatingCenter),
                FunctionalLocation = hydrant.FunctionalLocation?.Description
            };
        }

        #endregion
    }
}
