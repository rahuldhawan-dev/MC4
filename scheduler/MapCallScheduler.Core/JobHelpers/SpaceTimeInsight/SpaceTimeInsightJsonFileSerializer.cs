using System;
using System.Collections.Generic;
using System.Linq;
using Historian.Data.Client.Entities;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using Newtonsoft.Json;

namespace MapCallScheduler.JobHelpers.SpaceTimeInsight
{
    public class SpaceTimeInsightJsonFileSerializer : ISpaceTimeInsightJsonFileSerializer
    {
        #region Private Methods

        private string SerializeScadaData(IEnumerable<RawData> rawDatas, string type, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(new {
                DataType = type,
                SourceSystem = "MAPCALL",
                ScadaReadings = rawDatas.Map<RawData, object>(rd => new {
                    Tagname = rd.TagName,
                    Timestamp = rd.TimeStamp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    rd.Value
                })
            }, formatting);
        }

        #endregion

        #region Exposed Methods

        public string SerializeWorkOrders(IEnumerable<WorkOrder> collection, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(new {
                DataType = "WORKORDER_DATA",
                SourceSystem = "MAPCALL",
                WorkOrders = collection.Map<WorkOrder, object>(wo => new {
                    ID = wo.Id.ToString(), wo.WorkDescription.Description,
                    DateCompleted = wo.DateCompleted.Value.ToString("yyyy-MM-dd"),
                    SAPWorkorderNum = wo.SAPWorkOrderNumber,
                    SAPNotifNum = wo.SAPNotificationNumber,
                    wo.LostWater,
                    LostWaterUnitOfMeasure = "CGL",
                    Geometry = $"POINT({wo.Longitude:N13} {wo.Latitude:N13})"
                }).ToArray()
            }, formatting);
        }

        public string SerializeMainBreaks(IEnumerable<MainBreak> collection, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(new {
                DataType = "MAINBREAK_DATA",
                SourceSystem = "MAPCALL",
                MainBreaks = collection.Map<MainBreak, object>(m => new {
                    ID = m.Id.ToString().PadLeft(4, '0'),
                    MainBreakType = m.MainFailureType.ToString(),
                    CreatedDate = m.WorkOrder.CreatedAt.ToString("yyyy-MM-dd"),
                    RepairDate = m.WorkOrder.DateCompleted.Value.ToString("yyyy-MM-dd"),
                    AcquiredYear = string.Empty,
                    LeakDate = m.WorkOrder.CreatedAt.ToString("yyyy-MM-dd"),
                    CustomerImpacted = m.WorkOrder.EstimatedCustomerImpact?.Id,
                    Geometry = $"POINT({m.WorkOrder.Longitude:N13} {m.WorkOrder.Latitude:N13})"
                })
            }, formatting);
        }

        public string SerializeHydrantInspections(IEnumerable<HydrantInspection> coll, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(new {
                DataType = "INSPECTION_DATA",
                SourceSystem = "MAPCALL",
                InspectionData = coll.Map<HydrantInspection, object>(hi => new {
                    hi.Hydrant?.HydrantNumber,
                    InspectionType = hi.HydrantInspectionType?.Description ?? string.Empty,
                    DateInspected = hi.DateInspected.ToString("yyyy-MM-dd"),
                    hi.GallonsFlowed,
                    hi.MinutesFlowed,
                    hi.GPM,
                })
            }, formatting);
        }

        public string SerializeTankLevelData(IEnumerable<RawData> rawDatas, Formatting formatting = Formatting.None)
        {
            return SerializeScadaData(rawDatas, "TANK_LEVEL_DATA", formatting);
        }

        public string SerializeInterconnectData(IEnumerable<RawData> rawDatas, Formatting formatting = Formatting.None)
        {
            return SerializeScadaData(rawDatas, "INTERCONNECT_DATA", formatting);
        }

        #endregion
    }

    public interface ISpaceTimeInsightJsonFileSerializer
    {
        #region Abstract Methods

        string SerializeWorkOrders(IEnumerable<WorkOrder> collection, Formatting formatting = Formatting.None);
        string SerializeMainBreaks(IEnumerable<MainBreak> collection, Formatting formatting = Formatting.None);
        string SerializeHydrantInspections(IEnumerable<HydrantInspection> coll, Formatting formatting = Formatting.None);
        string SerializeTankLevelData(IEnumerable<RawData> rawDatas, Formatting formatting = Formatting.None);
        string SerializeInterconnectData(IEnumerable<RawData> rawDatas, Formatting formatting = Formatting.None);

        #endregion
    }
}