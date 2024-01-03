using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using Newtonsoft.Json;

namespace MapCallScheduler.JobHelpers.GIS.Models
{
    public class SewerOpening
    {
        #region Constants

        public static readonly int[] RELEVANT_WORK_DESCRIPTIONS = {
            (int)WorkDescription.Indices.SEWER_OPENING_INSTALLATION,
            (int)WorkDescription.Indices.SEWER_OPENING_REPLACE
        };

        #endregion

        #region Properties

        public int Id { get; set; }
        public decimal? DepthToInvert { get; set; }
        public DateTime? DateInstalled { get; set; }
        public AssetStatus Status { get; set; }
        public SewerOpeningMaterial SewerOpeningMaterial { get; set; }
        public decimal? RimElevation { get; set; }
        public int? Route { get; set; }
        public int? SAPEquipmentId { get; set; }
        public int? Stop { get; set; }
        public string OpeningNumber { get; set; }
        public SewerOpeningType SewerOpeningType { get; set; }
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

        public static SewerOpening FromDbRecord(MapCall.Common.Model.Entities.SewerOpening opening)
        {
            return new SewerOpening {
                Id = opening.Id,
                DepthToInvert = opening.DepthToInvert,
                DateInstalled = opening.DateInstalled.FromDbRecord(),
                Status = AssetStatus.FromDbRecord(opening.Status),
                SewerOpeningMaterial = SewerOpeningMaterial.FromDbRecord(opening.SewerOpeningMaterial),
                RimElevation = opening.RimElevation,
                Route = opening.Route,
                SAPEquipmentId = opening.SAPEquipmentId,
                Stop = opening.Stop,
                OpeningNumber = opening.OpeningNumber,
                SewerOpeningType = SewerOpeningType.FromDbRecord(opening.SewerOpeningType),
                LastUpdated = opening.UpdatedAt.ToUniversalTime(),
                LastUpdatedBy = User.FromDbRecord(opening.UpdatedBy),
                WorkOrder = opening.WorkOrders.Where(wo => RELEVANT_WORK_DESCRIPTIONS.Contains(wo.WorkDescription.Id))
                                   .OrderByDescending(wo => wo.CreatedAt).FirstOrDefault(),
                State = State.FromDbRecord(opening),
                Town = Town.FromDbRecord(opening.Town),
                OperatingCenter = OperatingCenter.FromDbRecord(opening.OperatingCenter),
                FunctionalLocation = opening.FunctionalLocation?.Description
            };
        }

        #endregion
    }
}
