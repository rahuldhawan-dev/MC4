using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MapCall.SAP.MaintenancePlanLookupWS;
using MMSINC.Data;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Metadata;
using System;
using System.Globalization;

namespace MapCall.SAP.Model.Entities
{
    public class SAPMaintenancePlanLookup : SAPEntity
    {
        #region Properties

        #region Request Properties

        public virtual string OperatingCenter { get; set; }
        public virtual string[] PlanningPlant { get; set; }

        public virtual string FunctionalLocation { get; set; }
        public virtual string EquipmentType { get; set; }
        public virtual string SAPEquipmentID { get; set; }
        public virtual string MaintenancePlan { get; set; }

        #endregion

        #region Response Properties

        public virtual string Createdon { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string MaintenancePlanText { get; set; }
        public virtual string MaintenancePlanSort { get; set; }
        public virtual string MaintenancePlanCategory { get; set; }
        public IList<SAPMaintenanceItemList> SapMaintenanceItemList { get; set; }
        public IEnumerable<SAPSchedulingList> SapSchedulingList { get; set; }
        public IEnumerable<SAPRecordCycleList> SapRecordCycleList { get; set; }
        public string SAPErrorCode { get; set; }

        #endregion

        #endregion

        #region Exposed Methods

        public SAPMaintenancePlanLookup(MaintenancePlan_InfoRecord response)
        {
            MaintenancePlan = response.MaintenancePlan;
            CreatedBy = response.CreatedBy;
            Createdon = response.Createdon;
            MaintenancePlanText = response.MaintenancePlanText;
            MaintenancePlanSort = response.MaintenancePlanSortField;
            MaintenancePlanCategory = response.MaintenancePlancategory;

            if (response.MaintenanceItemList != null)
            {
                SapMaintenanceItemList = new SAPMaintenanceItemList[response.MaintenanceItemList.Count()];

                for (int i = 0; i < response.MaintenanceItemList.Count(); i++)
                {
                    SapMaintenanceItemList[i] = new SAPMaintenanceItemList();
                    SapMaintenanceItemList[i].MaintenanceItem = response.MaintenanceItemList[i].MaintenanceItem;
                    SapMaintenanceItemList[i].MaintenanceItemDescription =
                        response.MaintenanceItemList[i].MaintenanceItemDescription;
                    SapMaintenanceItemList[i].FunctionalLocation = response.MaintenanceItemList[i].FunctionalLocation;
                    SapMaintenanceItemList[i].FunctionalLocationdescription =
                        response.MaintenanceItemList[i].FunctionalLocationdescription;
                    SapMaintenanceItemList[i].Equipment = response.MaintenanceItemList[i].Equipment;
                    SapMaintenanceItemList[i].PlanningPlant = response.MaintenanceItemList[i].PlanningPlant;
                    SapMaintenanceItemList[i].MaintenanceActivityType =
                        response.MaintenanceItemList[i].MaintenanceActivityType;
                    SapMaintenanceItemList[i].MainWorkCenter = response.MaintenanceItemList[i].MainWorkCenter;
                    SapMaintenanceItemList[i].Priority = response.MaintenanceItemList[i].Priority;
                    SapMaintenanceItemList[i].PriorityText = response.MaintenanceItemList[i].PriorityText;
                    if (response.MaintenanceItemList[i].ObjectListItem != null &&
                        response.MaintenanceItemList[i].ObjectListItem.Any())
                    {
                        SapMaintenanceItemList[i].SapObjectListItem =
                            new SAPObjectListItem[response.MaintenanceItemList[i].ObjectListItem.Count()];
                        for (int j = 0; j < response.MaintenanceItemList[i].ObjectListItem.Count(); j++)
                        {
                            SapMaintenanceItemList[i].SapObjectListItem[j] = new SAPObjectListItem {
                                Equipment = response.MaintenanceItemList[i].ObjectListItem[j].Equipment,
                                EquipmentDescription =
                                    response.MaintenanceItemList[i].ObjectListItem[j].EquipmentDescription,
                                FunctionalLocation =
                                    response.MaintenanceItemList[i].ObjectListItem[j].FunctionalLocation,
                                FunctionalLocationDescription = response
                                                               .MaintenanceItemList[i].ObjectListItem[j]
                                                               .FunctionalLocationDescription
                            };
                        }
                    }

                    SapMaintenanceItemList[i].GroupCounter = response.MaintenanceItemList[i].TaskList?.GroupCounter;
                    SapMaintenanceItemList[i].TaskList = response.MaintenanceItemList[i].TaskList?.TaskList;
                    SapMaintenanceItemList[i].TaskListDescrption =
                        response.MaintenanceItemList[i].TaskList?.TaskListDescrption;
                    SapMaintenanceItemList[i].TaskListType = response.MaintenanceItemList[i].TaskList?.TaskListType;
                    SapMaintenanceItemList[i].Usage = response.MaintenanceItemList[i].TaskList?.Usage;

                    if (response.MaintenanceItemList[i].TaskList != null &&
                        response.MaintenanceItemList[i].TaskList.Operations != null &&
                        response.MaintenanceItemList[i].TaskList.Operations.Any())
                    {
                        SapMaintenanceItemList[i].SapOperations =
                            new SAPOperations[response.MaintenanceItemList[i].TaskList.Operations.Count()];
                        for (int k = 0; k < response.MaintenanceItemList[i].TaskList.Operations.Count(); k++)
                        {
                            SapMaintenanceItemList[i].SapOperations[k] = new SAPOperations();
                            SapMaintenanceItemList[i].SapOperations[k].ActivityType =
                                response.MaintenanceItemList[i].TaskList.Operations[k].ActivityType;
                            SapMaintenanceItemList[i].SapOperations[k].ControlKey =
                                response.MaintenanceItemList[i].TaskList.Operations[k].ControlKey;
                            SapMaintenanceItemList[i].SapOperations[k].Duration =
                                response.MaintenanceItemList[i].TaskList.Operations[k].Duration;
                            SapMaintenanceItemList[i].SapOperations[k].Frequency =
                                response.MaintenanceItemList[i].TaskList.Operations[k].Frequency;
                            SapMaintenanceItemList[i].SapOperations[k].NormalDurationOfActivity = response
                               .MaintenanceItemList[i]
                               .TaskList.Operations[k]
                               .NormalDurationOfActivity;
                            SapMaintenanceItemList[i].SapOperations[k].NumberOfCapacityrequired = response
                               .MaintenanceItemList[i]
                               .TaskList.Operations[k]
                               .NumberOfCapacityrequired;
                            SapMaintenanceItemList[i].SapOperations[k].OperationDescription = response
                               .MaintenanceItemList[i]
                               .TaskList.Operations[k]
                               .OperationDescription;
                            SapMaintenanceItemList[i].SapOperations[k].OperationOrActivityNumber = response
                               .MaintenanceItemList[
                                    i].TaskList
                               .Operations[k]
                               .OperationOrActivityNumber;
                            SapMaintenanceItemList[i].SapOperations[k].OperationsActivity =
                                response.MaintenanceItemList[i].TaskList.Operations[k].OperationsActivity;
                            SapMaintenanceItemList[i].SapOperations[k].Plant =
                                response.MaintenanceItemList[i].TaskList.Operations[k].Plant;
                            SapMaintenanceItemList[i].SapOperations[k].StandardTextKey =
                                response.MaintenanceItemList[i].TaskList.Operations[k].StandardTextKey;
                            SapMaintenanceItemList[i].SapOperations[k].Unit1 =
                                response.MaintenanceItemList[i].TaskList.Operations[k].Unit1;
                            SapMaintenanceItemList[i].SapOperations[k].Unit2 =
                                response.MaintenanceItemList[i].TaskList.Operations[k].Unit2;
                            SapMaintenanceItemList[i].SapOperations[k].WorkCenter =
                                response.MaintenanceItemList[i].TaskList.Operations[k].WorkCenter;
                            SapMaintenanceItemList[i].SapOperations[k].WorkInvolved =
                                response.MaintenanceItemList[i].TaskList.Operations[k].WorkInvolved;
                        }
                    }
                }
            }

            if (response.CycleList != null)
            {
                SapRecordCycleList = from c in response.CycleList
                                     select new SAPRecordCycleList {
                                         Cycle = c?.Cycle,
                                         CycleText = c?.CycleText,
                                         CycleUnit = c?.CycleUnit
                                     };
            }

            if (response.SchedulingList != null)
            {
                SapSchedulingList = from s in response.SchedulingList
                                    select new SAPSchedulingList {
                                        CallNumber = s?.CallNumber,
                                        PlanDate = s?.PlanDate,
                                        CallDate = s?.CallDate,
                                        CompletionDate = s?.CompletionDate,
                                        SchedulingType = s?.SchedulingType,
                                        Status = s?.Status
                                    };
            }

            SAPErrorCode = "Successfully";
        }

        public SAPMaintenancePlanLookup() { }

        public MaintenancePlan_Query Request()
        {
            MaintenancePlan_Query request = new MaintenancePlan_Query();
            request.OperatingCentre = PlanningPlant;
            request.FunctionalLocation = FunctionalLocation;
            request.SAPEquipmentID = SAPEquipmentID;
            request.EquipmentType = EquipmentType;
            request.MaintenancePlan = MaintenancePlan;
            return request;
        }

        #endregion
    }

    public class SAPMaintenanceItemList
    {
        #region Properties

        public virtual string MaintenanceItem { get; set; }
        public virtual string MaintenanceItemDescription { get; set; }
        public virtual string FunctionalLocation { get; set; }

        [View(DisplayName = "Functional Location Description")]
        public virtual string FunctionalLocationdescription { get; set; }

        public virtual string Equipment { get; set; }
        public virtual string PlanningPlant { get; set; }
        public virtual string MaintenanceActivityType { get; set; }
        public virtual string MainWorkCenter { get; set; }
        public virtual string Priority { get; set; }
        public virtual string PriorityText { get; set; }
        public IList<SAPObjectListItem> SapObjectListItem { get; set; }
        public virtual string TaskListType { get; set; }
        public virtual string TaskList { get; set; }
        public virtual string GroupCounter { get; set; }
        public virtual string TaskListDescrption { get; set; }
        public virtual string Usage { get; set; }
        public IList<SAPOperations> SapOperations { get; set; }

        #region Logical Properties

        public virtual string MaintenanceActivityTypeDescription
        {
            get
            {
                switch (MaintenanceActivityType)
                {
                    case "P01":
                        return "Predictive Inspection";
                    case "P02":
                        return "Predictive Re-Inspection";
                    case "P03":
                        return "Preventive Inspection";
                    case "P04":
                        return "Preventive Re-Inspection";
                    case "P05":
                        return "Preventive Valve Turn";
                    case "P06":
                        return "Preventive Hydrant Inspect";
                    case "P07":
                        return "Preventive Maint Training";
                    default:
                        return string.Empty;
                }
            }
        }

        #endregion

        #endregion
    }

    public class SAPObjectListItem
    {
        #region Properties

        public virtual string Equipment { get; set; }
        public virtual string EquipmentDescription { get; set; }
        public virtual string FunctionalLocation { get; set; }
        public virtual string FunctionalLocationDescription { get; set; }

        #endregion
    }

    public class SAPOperations
    {
        #region Properties

        public virtual string OperationsActivity { get; set; }
        public virtual string Frequency { get; set; }
        public virtual string WorkCenter { get; set; }
        public virtual string Duration { get; set; }
        public virtual string OperationOrActivityNumber { get; set; }
        public virtual string Plant { get; set; }
        public virtual string ControlKey { get; set; }
        public virtual string OperationDescription { get; set; }
        public virtual string WorkInvolved { get; set; }
        public virtual string Unit1 { get; set; }
        public virtual string NumberOfCapacityrequired { get; set; }
        public virtual string NormalDurationOfActivity { get; set; }
        public virtual string Unit2 { get; set; }
        public virtual string ActivityType { get; set; }
        public virtual string StandardTextKey { get; set; }

        #endregion
    }

    public class SAPSchedulingList
    {
        #region Properties

        public virtual string CallNumber { get; set; }
        public virtual string PlanDate { get; set; }
        public virtual string CallDate { get; set; }
        public virtual string CompletionDate { get; set; }
        public virtual string SchedulingType { get; set; }
        public virtual string Status { get; set; }

        #region Logical Properties

        public virtual string SchedulingTypeDescription
        {
            get
            {
                switch (SchedulingType)
                {
                    case "M":
                        return "Manual";
                    case "N":
                        return "New Start";
                    case "T":
                        return "Scheduled";
                    case "Z":
                        return "Cycle Start";
                    default:
                        return string.Empty;
                }
            }
        }

        public virtual string StatusDescription
        {
            get
            {
                switch (Status)
                {
                    case "A":
                        return "Complete";
                    case "B":
                        return "Called";
                    case "C":
                        return "Skipped";
                    case "D":
                        return "Hold";
                    case "E":
                        return "Locked";
                    case "F":
                        return "Fixed";
                    default:
                        return string.Empty;
                }
            }
        }

        #endregion

        #endregion
    }

    public class SAPRecordCycleList
    {
        #region Properties

        public virtual string Cycle { get; set; }
        public virtual string CycleUnit { get; set; }
        public virtual string CycleText { get; set; }

        #endregion
    }
}
