using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Data.Linq;
using WorkOrderImportHelper.Model;
using WorkOrders.Model;

namespace WorkOrderImportHelper
{
    public class WorkOrderImporter : WorkOrderProcessor
    {
        #region Constants

        public const int SERVICE_OFF_AT_CURB_STOP_STORM_RESTORATION = 170, 
                         OPERATING_CENTER_ID = 14;

        #endregion
        
        #region Private Members

        private WorkOrdersImportDataContext _dataContext;
        private IQueryable<BarrierWorkOrder> _workOrders;
        private WorkOrderRepository _workOrderRepository;
        private StreetRepository _streetRepository;
        private IList<Street> _streets;
        private IEnumerable<WorkOrder> _premiseOrders; 

        #endregion

        #region Properties

        public WorkOrdersImportDataContext DataContext
        {
            get
            {
                if (_dataContext == null)
                    _dataContext = new WorkOrdersImportDataContext();
                return _dataContext;
            }
        }
        
        public IQueryable<BarrierWorkOrder> WorkOrders
        {
            get
            {
                if (_workOrders == null)
                    _workOrders = GetBarrierWorkOrders();
                return _workOrders;
            }
        }

        #region 271 Repositories
        
        public WorkOrderRepository Repository
        {
            get
            {
                if (_workOrderRepository == null)
                    _workOrderRepository = new WorkOrderRepository();
                return _workOrderRepository;
            }
        }
        
        public StreetRepository StreetRepository
        {
            get
            {
                if (_streetRepository == null)
                    _streetRepository = new StreetRepository();
                return _streetRepository;
            }
        }
        
        #endregion

        #region Lookups

        public IList<Street> Streets
        {
            get
            {
                if (_streets == null)
                    _streets = StreetRepository.SelectAllAsList();
                return _streets;
            }
        }
        
        public IEnumerable<WorkOrder> PremiseOrders
        {
            get
            {
                if (_premiseOrders == null)
                    _premiseOrders = GetPremiseOrders();
                return _premiseOrders;
            }
        }
        
        #endregion

        #endregion

        #region Constructors

        public WorkOrderImporter(Action<int> displayCurrentWorkOrderID,
                                 Action<string> outputFn)
            : base(displayCurrentWorkOrderID, outputFn)
        {
        }

        #endregion

        #region Private Methods
        
        private IEnumerable<WorkOrder> GetPremiseOrders()
        {
            return
                (from wo in WorkOrderRepository.SelectAllAsList()
                 where wo.WorkDescriptionID == SERVICE_OFF_AT_CURB_STOP_STORM_RESTORATION
                    && wo.OperatingCenterID == OPERATING_CENTER_ID
                    && wo.CreatedOn > new DateTime(2011, 11, 1)
                 select wo);
        }

        private IQueryable<BarrierWorkOrder> GetBarrierWorkOrders()
        {
            return (from wo in DataContext.BarrierWorkOrders
                    select wo);
        }

        private IEnumerable<BarrierCloseOutWorkOrder> GetBarrierCloseOutWorkOrders()
        {
            return from wo in DataContext.BarrierCloseOutWorkOrders
                   select wo;
        }

        public override int LoadWorkOrders()
        {
            return WorkOrders.Count();
        }

        private void CreateWorkOrder(BarrierWorkOrder order)
        {
            // make sure we don't already have an order for that operating center, workdescription, and premise #
            if (PremiseOrders.Any(x => x.PremiseNumber == order.Premise_ID))
            {
                _outputFn(String.Format(" Already Existed: \t{0}",order.Premise_ID));
                return;
            }
            // no street, no order
            if (order.StreetID == null)
            {
                _outputFn(String.Format(" No Street: \t{0}", order.Premise_ID));
                return;
            }

            var street = Streets.FirstOrDefault(x => x.StreetID == order.StreetID);

            // street doesn't exist
            if (street == null)
            {
                _outputFn(String.Format(" Invalid Street: \t{0}", order.Premise_ID));
                return;
            }

            var workOrder = new WorkOrder();
            //Operating Center
            workOrder.OperatingCenterID = OPERATING_CENTER_ID;                   // NJ4
            //Town
            workOrder.TownID = GetTown(order.Town);// PROVIDED
            //TOWN SECTION
            if (!String.IsNullOrEmpty(order.Town_Section))
            workOrder.TownSectionID = GetTownSection(order.Town_Section);
            //Street Number
            workOrder.StreetNumber = order.House_Number.ToString();        //PROVIDED
            //Street
            workOrder.StreetID = ((int?)order.StreetID).Value;  //PROVIDED
            //Nearest Cross Street
            workOrder.NearestCrossStreetID = GetHwy35S(workOrder.TownID);//DEFAULT : S State Hwy 35 for the town. TODO: Look this up
            //Asset Type
            workOrder.AssetTypeID = 4;                          //DEFAULT : Service
            //Requested By
            workOrder.RequesterID = 2;                          //DEFAULT : Employee
            workOrder.RequestingEmployeeID = 134;               //DEFAULT : Crissy Forrester
            //Premise #
            workOrder.PremiseNumber = order.Premise_ID;         //PROVIDED
            //Lat/Lng
            workOrder.Latitude = order.Y;                       //PROVIDED
            workOrder.Longitude = order.X;                      //PROVIDED
            //Purpose
            workOrder.PurposeID = 18;                           //DEFAULT: Hurricane Sandy
            //Priority
            workOrder.PriorityID = 4;                           //DEFAULT: Routine
            //Markout Requirement
            workOrder.MarkoutRequirementID = 1;                 //DEFAULT: None
            workOrder.CreatorID = 134;                          //DEFAULT: Crissy Forrester
            //workOrder.DrivenBy = new WorkOrderPurpose { WorkOrderPurposeID = workOrder.PurposeID };
            //Work Description
            workOrder.WorkDescriptionID = SERVICE_OFF_AT_CURB_STOP_STORM_RESTORATION;                  //DEFAULT: //SERVICE OFF AT CURB STOP-STORM RESTORATION
            //workOrder.WorkDescription = new WorkDescription { WorkDescriptionID = 170 };

            if (street.TownID != workOrder.TownID)
            {
                var newStreet = Streets.FirstOrDefault(x => x.FullStName == order.Street_Name && x.TownID == workOrder.TownID);
                if (newStreet == null || newStreet.TownID != workOrder.TownID)
                {
                    _outputFn(String.Format(" Could not find Street: \t{0}", order.Premise_ID));
                    return;
                }
                workOrder.StreetID = newStreet.StreetID;
            }

            Repository.InsertNewEntity(workOrder);
            _outputFn(String.Format(" Created: \t{0}", order.Premise_ID));
        }

        private int? GetHwy35S(int townId)
        {
            switch (townId)
            {
                case 193:
                    return 36989;
                case 194:
                    return 36990;
                case 196:
                    return 36991;
                case 197:
                    return 36992;
                case 19:
                    return 56950;
            }
            return 0;
        }

        private int? GetTownSection(string townSection)
        {
            switch(townSection)
            {
                case "ORTLEY BEACH":
                    return 90;
                case "NORMANDY BEACH":
                    return 168;
                case "TOMS RIVER":
                    return 175;
                case "CHADWICK BEACH":
                    return 189;
            }
            return null;
        }

        private int GetTown(string town)
        {
            switch (town)
            {
                case "BRICK TWP":
                    return 193;
                case "MANTOLOKING":
                    return 197;
                case "LAVALLETTE":
                    return 196;
                case "TOMS RIVER":
                    return 194;
            }
            return 0;
        }

        #endregion

        #region Exposed Methods

        public void Import()
        {
            _outputFn(" ... \r\n");
            var counter = 1;
            foreach(var order in WorkOrders)
            {
                _displayCurrentWorkOrderID(counter);
                CreateWorkOrder(order);
                counter++;
            }
        }

        public void CloseOutOrders()
        {
            const string closed = " -- Closed in batch by Christine Forrester.";
            var employee = EmployeeRepository.SelectByName("Crissy Forrester");
            var allOrders = WorkOrderRepository.SelectAllAsList()
                                   .Select(x => x.WorkOrderID).ToArray();
            var count = allOrders.Count();
            var orders = GetBarrierCloseOutWorkOrders();
            WorkOrder wo;
            foreach (var order in orders)
            {
                if (allOrders.Contains(order.WorkOrderID))
                {
                    wo = WorkOrderRepository.GetEntity(order.WorkOrderID);
                    if (wo.Notes == null)
                        wo.Notes = closed;
                    else if (!wo.Notes.Contains(closed))
                        wo.Notes = wo.Notes + closed;
                    wo.DateCompleted = wo.ApprovedOn = DateTime.Now;
                    wo.CompletedByID = wo.ApprovedByID = employee.EmployeeID;
                    
                    Repository.UpdateCurrentEntity(wo);
                    _outputFn(String.Format("Updated: {0} \n", order.WorkOrderID));
                }
                else
                {
                    _outputFn(String.Format("Skipped, No Order Matched: {0} \n", order.WorkOrderID));
                }
            }
        }

        #endregion
    }
}
