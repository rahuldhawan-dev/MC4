using MapCall.Common.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.SAP.service;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPCompleteWorkOrder : SAPEntity, ISAPServiceEntity
    {
        #region Properties

        public string SAPErrorCode { get; set; }

        #region WebService Request Properties

        public virtual string AssetType { get; set; }
        public virtual string Notes { get; set; }
        public virtual string SAPOrderNo { get; set; }
        public virtual string DateCompleted { get; set; }
        public virtual string Employee { get; set; }
        public virtual string Finalize { get; set; }
        public virtual string StartDate { get; set; }
        public virtual string StartTime { get; set; }
        public virtual string EndDate { get; set; }
        public virtual string EndTime { get; set; }
        public virtual string LostWater { get; set; }
        public virtual string WorkOrderDescription { get; set; }

        public virtual string PMActType { get; set; }

        //New field User Id included as per change request
        public virtual string UserID { get; set; }
        public virtual IEnumerable<SAPMainBreaks> sapMainBreaks { get; set; }
        public IEnumerable<SAPCrewAssignment> sapCrewAssignments { get; set; }

        #endregion

        #region WebService Response Properties

        public virtual string NotificationNumber { get; set; }
        public virtual string OrderNumber { get; set; }
        public virtual string Status { get; set; }
        public virtual string WBSElement { get; set; }
        public virtual string CostCenter { get; set; }

        #endregion

        #endregion

        #region Constructors

        public SAPCompleteWorkOrder() { }

        public SAPCompleteWorkOrder(WorkOrder workOrder)
        {
            AssetType = workOrder.AssetType?.Description.ToUpper();
            Notes = workOrder.Notes;
            SAPOrderNo = workOrder.SAPWorkOrderNumber.ToString();
            //As per prod defect on 14th mar datecompleted changed to current date time
            //DateCompleted = workOrder.DateCompleted?.Date.ToString(SAP_DATE_FORMAT) ;
            DateCompleted = DateTime.Now.Date.ToString(SAP_DATE_FORMAT);
            // Employee = workOrder.RequestingEmployee?.UserName    ;
            Finalize = workOrder.DateCompleted != null ? "X" : null;
            WorkOrderDescription = workOrder.WorkDescription?.Description;
            Notes = workOrder.Notes;
            //New field User Id included as per change request
            UserID = workOrder.UserId;

            PMActType = workOrder.PlantMaintenanceActivityType?.Code;
            if (workOrder.PlantMaintenanceActivityTypeOverride != null)
            {
                WBSElement = workOrder.AccountCharged;
            }

            if (workOrder.ApprovedOn != null && AssetType == "MAIN")
            {
                LostWater = workOrder.LostWater.ToString();
                if (workOrder.MainBreaks != null && workOrder.MainBreaks.Any())
                {
                    var MainBreaks = from b in workOrder.MainBreaks
                                     select new SAPMainBreaks {
                                         FailureType = b.MainFailureType?.Description,
                                         SoilCondition = b.MainBreakSoilCondition?.Description,
                                         CustomersAffected = b.CustomersAffected.ToString(),
                                         ShutDownTime = b.ShutdownTime.ToString(),
                                         ChlorineResidual = b.ChlorineResidual.ToString(),
                                         Size = b.ServiceSize?.Description,
                                         //replacedwith should be changed to MainBreakMaterial - Bug-3875
                                         ReplacedWith = b.MainBreakMaterial?.Description,
                                         Depth = Math.Round(b.Depth / 12, 1, MidpointRounding.AwayFromZero).ToString(),
                                         FootageReplaced = b.FootageReplaced.ToString(),
                                         //new fields added as part of Bug-3875
                                         MainInvestigation = "CONFIRMED",
                                         DisinfectionMethod =
                                             b.MainBreakDisinfectionMethod?.Description ==
                                             @"Disinfection of Fittings\Pipe"
                                                 ? "Disinfection Fitting/Pipe"
                                                 : b.MainBreakDisinfectionMethod?.Description,
                                         FlushMethod = b.MainBreakFlushMethod?.Description == "Blowoff"
                                             ? "Blow off"
                                             : b.MainBreakFlushMethod?.Description,
                                     };

                    sapMainBreaks = MainBreaks.ToList();
                }
            }

            if (workOrder.CrewAssignments != null && workOrder.CrewAssignments.Any())
            {
                //bug-3689 - data sorting based on datestarted instead of assignedon
                var CrewAssignments = from ca in workOrder.CrewAssignments
                                      where (ca.DateStarted != null && ca.DateEnded != null)
                                      orderby ca.DateStarted descending
                                      select new SAPCrewAssignment {
                                          DateStart = ca.DateStarted?.Date.ToString(SAP_DATE_FORMAT),
                                          DateEnd = ca.DateEnded?.Date.ToString(SAP_DATE_FORMAT),
                                          StartTime = ca.DateStarted?.ToString(SAP_TIME_FORMAT),
                                          EndTime = ca.DateEnded?.ToString(SAP_TIME_FORMAT),
                                          TotalManHours = ca.DateStarted != null
                                              ? (ca.DateEnded - ca.DateStarted).Value.TotalHours * ca.EmployeesOnJob
                                              : 0, // (ca.DateEnded?.Date.Subtract(ca.DateStarted?.Date)) * ca.EmployeesOnJob,
                                          //TotalManHours = ca.TotalManHours
                                      };

                sapCrewAssignments = CrewAssignments.ToList();
            }

            //DateCompleted = workOrder.DateCompleted?.Date.ToString(SAP_DATE_FORMAT);// as per demo, need work order completed date here.
        }

        #endregion

        #region Exposed Methods

        public CompleteWorkOrder CompleteWorkOrderRequest()
        {
            CompleteWorkOrderWorkOrder[] completeWorkOrderWorkOrder = new CompleteWorkOrderWorkOrder[1];

            CompleteWorkOrderWorkOrderChangeOrder[] SAPChangeOrder = new CompleteWorkOrderWorkOrderChangeOrder[1];

            SAPChangeOrder[0] = new CompleteWorkOrderWorkOrderChangeOrder();
            SAPChangeOrder[0].Notes = Notes;
            SAPChangeOrder[0].PMActType = PMActType;
            SAPChangeOrder[0].WorkOrderDescription = WorkOrderDescription;

            CompleteWorkOrderWorkOrderTimeConfirmation[] SAPTimeConfirmation =
                new CompleteWorkOrderWorkOrderTimeConfirmation[1];
            CompleteWorkOrderWorkOrderChangeNotification[] SAPChangeNotification =
                new CompleteWorkOrderWorkOrderChangeNotification[1];

            SAPTimeConfirmation[0] = new CompleteWorkOrderWorkOrderTimeConfirmation();
            SAPTimeConfirmation[0].AssetType = AssetType;
            SAPTimeConfirmation[0].DateCompleted = DateCompleted;
            SAPTimeConfirmation[0].Employee = Employee;

            if (sapCrewAssignments != null && sapCrewAssignments.Any())
            {
                SAPTimeConfirmation[0].EndDate = sapCrewAssignments.ToList()[0].DateEnd;
                SAPTimeConfirmation[0].EndTime = sapCrewAssignments.ToList()[0].EndTime;
                SAPTimeConfirmation[0].StartDate =
                    sapCrewAssignments.ToList()[sapCrewAssignments.ToList().Count - 1].DateStart;
                SAPTimeConfirmation[0].StartTime =
                    sapCrewAssignments.ToList()[sapCrewAssignments.ToList().Count - 1].StartTime;
                double? ActualWork = sapCrewAssignments.Sum(i => i.TotalManHours);
                SAPTimeConfirmation[0].ActualWork = ActualWork.ToString();
            }

            SAPTimeConfirmation[0].Finalize = Finalize;
            SAPTimeConfirmation[0].LostWater = LostWater;
            SAPTimeConfirmation[0].Notes = Notes;
            SAPTimeConfirmation[0].SAPOrderNo = SAPOrderNo;

            if (sapMainBreaks != null && sapMainBreaks.Any())
            {
                SAPTimeConfirmation[0].ChlorineResidual = sapMainBreaks.ToList()[0].ChlorineResidual;
                SAPTimeConfirmation[0].CustomersAffected = sapMainBreaks.ToList()[0].CustomersAffected;
                SAPTimeConfirmation[0].Depth = sapMainBreaks.ToList()[0].Depth;
                SAPTimeConfirmation[0].FootageReplaced = sapMainBreaks.ToList()[0].FootageReplaced;
                SAPTimeConfirmation[0].ShutDownTime = sapMainBreaks.ToList()[0].ShutDownTime;
                SAPTimeConfirmation[0].TotalChlorine = sapMainBreaks.ToList()[0].TotalChlorine;

                SAPChangeNotification[0] = new CompleteWorkOrderWorkOrderChangeNotification();
                SAPChangeNotification[0].FailureType = sapMainBreaks.ToList()[0].FailureType;
                SAPChangeNotification[0].ReplacedWith = sapMainBreaks.ToList()[0].ReplacedWith;
                SAPChangeNotification[0].Size = sapMainBreaks.ToList()[0].Size;
                SAPChangeNotification[0].SoilCondition = sapMainBreaks.ToList()[0].SoilCondition;
                //added new fields as part of Bug-3875
                SAPChangeNotification[0].MainInvestigation = sapMainBreaks.ToList()[0].MainInvestigation;
                SAPChangeNotification[0].DisinfectionMethod = sapMainBreaks.ToList()[0].DisinfectionMethod;
                SAPChangeNotification[0].FlushMethod = sapMainBreaks.ToList()[0].FlushMethod;
            }

            //New field UserId included as per change request
            CompleteWorkOrder completeWorkOrder = new CompleteWorkOrder {
                WorkOrder = new CompleteWorkOrderWorkOrder {
                    UserID = UserID, ChangeNotification = SAPChangeNotification, ChangeOrder = SAPChangeOrder,
                    TimeConfirmation = SAPTimeConfirmation
                }
            };

            return completeWorkOrder;
        }

        public void MapToWorkOrder(ISapWorkOrder workOrder)
        {
            workOrder.SAPErrorCode = Status;
            if (!string.IsNullOrEmpty(CostCenter))
            {
                workOrder.BusinessUnit = CostCenter;
            }
        }

        #endregion
    }

    [Serializable]
    public class SAPMainBreaks
    {
        #region properties

        public virtual string FailureType { get; set; }
        public virtual string SoilCondition { get; set; }
        public virtual string CustomersAffected { get; set; }
        public virtual string ShutDownTime { get; set; }
        public virtual string ChlorineResidual { get; set; }
        public virtual string Size { get; set; }
        public virtual string ReplacedWith { get; set; }
        public virtual string Depth { get; set; }
        public virtual string TotalChlorine { get; set; }
        public virtual string FootageReplaced { get; set; }
        public virtual string MainInvestigation { get; set; }
        public virtual string DisinfectionMethod { get; set; }
        public virtual string FlushMethod { get; set; }

        #endregion
    }
}
