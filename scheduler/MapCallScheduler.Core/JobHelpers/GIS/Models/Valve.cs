using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using Newtonsoft.Json;

namespace MapCallScheduler.JobHelpers.GIS.Models
{
    public class Valve
    {
        #region Constants

        public static readonly int[] RELEVANT_WORK_DESCRIPTIONS = {
            (int)WorkDescription.Indices.VALVE_INSTALLATION,
            (int)WorkDescription.Indices.VALVE_REPLACEMENT,
            (int)WorkDescription.Indices.VALVE_RETIREMENT
        };

        #endregion

        #region Properties

        public int Id { get; set; }
        public ValveControl ValveControls { get; set; }
        public ValveSize ValveSize { get; set; }
        public DateTime? DateInstalled { get; set; }
        public AssetStatus Status { get; set; }
        public ValveMake ValveMake { get; set; }
        public ValveNormalPosition NormalPosition { get; set; }
        public ValveOpenDirection OpenDirection { get; set; }
        public int? Route { get; set; }
        public int? SAPEquipmentId { get; set; }
        public decimal? Stop { get; set; }
        public decimal? Turns { get; set; }
        public string ValveNumber { get; set; }
        public ValveType ValveType { get; set; }
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

        public static Valve FromDbRecord(MapCall.Common.Model.Entities.Valve valve)
        {
            return new Valve {
                Id = valve.Id,
                ValveControls = ValveControl.FromDbRecord(valve.ValveControls),
                ValveSize = ValveSize.FromDbRecord(valve.ValveSize),
                DateInstalled = valve.DateInstalled.FromDbRecord(),
                Status = AssetStatus.FromDbRecord(valve.Status),
                ValveMake = ValveMake.FromDbRecord(valve.ValveMake),
                NormalPosition = ValveNormalPosition.FromDbRecord(valve.NormalPosition),
                OpenDirection = ValveOpenDirection.FromDbRecord(valve.OpenDirection),
                Route = valve.Route,
                SAPEquipmentId = valve.SAPEquipmentId,
                Stop = valve.Stop,
                Turns = valve.Turns,
                ValveNumber = valve.ValveNumber,
                ValveType = ValveType.FromDbRecord(valve.ValveType),
                LastUpdated = valve.UpdatedAt.ToUniversalTime(),
                LastUpdatedBy = User.FromDbRecord(valve.UpdatedBy),
                WorkOrder = valve.WorkOrders.Where(wo => RELEVANT_WORK_DESCRIPTIONS.Contains(wo.WorkDescription.Id))
                                 .OrderByDescending(wo => wo.CreatedAt).FirstOrDefault(),
                State = State.FromDbRecord(valve),
                Town = Town.FromDbRecord(valve.Town),
                OperatingCenter = OperatingCenter.FromDbRecord(valve.OperatingCenter),
                FunctionalLocation = valve.FunctionalLocation?.Description
            };
        }

        #endregion
    }
}
