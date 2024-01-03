using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class SpoilRepository : WorkOrdersRepository<Spoil>
    {
        #region Exposed Static Methods

        public static void InsertSpoil(int workOrderID, decimal quantity, int spoilStorageLocationID)
        {
            Insert(new Spoil {
                WorkOrderID = workOrderID,
                Quantity = quantity,
                SpoilStorageLocationID = spoilStorageLocationID
            });
        }

        public static void UpdateSpoil(decimal quantity, int spoilStorageLocationID, int spoilID)
        {
            Update(new Spoil {
                SpoilID = spoilID,
                Quantity = quantity,
                SpoilStorageLocationID = spoilStorageLocationID
            });
        }

        public static void DeleteSpoil(int spoilID)
        {
            Delete(GetEntity(spoilID));
        }

        public static IEnumerable<Spoil> GetSpoilsByWorkOrder(int workOrderID)
        {
            return (from s in DataTable
                    where s.WorkOrderID == workOrderID
                    orderby s.SpoilID
                    select s);
        }

        public static IEnumerable<Spoil> GetSpoilsByTownAndOpCenterForCompleteWorkOrders(int? operatingCenterID, int? townID)
        {
            return (from s in DataTable
                    where
                        s.WorkOrder.DateCompleted != null &&
                        (operatingCenterID == null ||
                         s.WorkOrder.OperatingCenterID == operatingCenterID) &&
                        (townID == null || s.WorkOrder.TownID == townID)
                    select s);
        }

        #region Reporting

        public static IEnumerable<SpoilTotal> GetSpoilTotalsByOperatingCenter()
        {
            var storageLocations =
                SpoilStorageLocationRepository
                    .SelectByOperatingCenters(
                    SecurityService.AdminOperatingCenters);

            foreach (var storageLocation in storageLocations)
            {
                var total = GetTotalByStorageLocation(storageLocation);
                var removed = SpoilRemovalRepository
                    .GetTotalByStorageLocation(storageLocation);
                var left = total - removed;
                yield return new SpoilTotal(storageLocation, left,
                    left *
                    (storageLocation.OperatingCenter.
                        OperatingCenterSpoilRemovalCost?.Cost ?? 0));
            }
        }

        #endregion

        #endregion

        #region Private Static Methods

        private static decimal GetTotalByStorageLocation(SpoilStorageLocation storageLocation)
        {
            return
                GetTotalByStorageLocation(
                    storageLocation.SpoilStorageLocationID);
        }

        private static decimal GetTotalByStorageLocation(int spoilStorageLocationID)
        {
            var ret = (from s in DataTable
                    where
                        s.SpoilStorageLocationID == spoilStorageLocationID
                    select (decimal?)s.Quantity).Sum();
            return ret ?? 0;
        }

        #endregion
    }

    public class SpoilTotal
    {
        #region Private Members

        private readonly string _opCode;
        private readonly SpoilStorageLocation _spoilStorageLocation;
        private readonly decimal _total, _unitCost, _accrualValue;

        #endregion

        #region Properties
        
        public string OpCode
        {
            get { return _opCode; }
        }

        public SpoilStorageLocation SpoilStorageLocation
        {
            get { return _spoilStorageLocation; }
        }

        public decimal Total
        {
            get { return _total; }
        }

        public decimal UnitCost
        {
            get { return _unitCost; }
        }

        public decimal AccrualValue
        {
            get { return _accrualValue; }
        }

        #endregion

        #region Constructors

        public SpoilTotal(SpoilStorageLocation spoilStorageLocation, decimal total, decimal accrualValue)
            : this(spoilStorageLocation.OperatingCenter.FullDescription, spoilStorageLocation, total, spoilStorageLocation.OperatingCenter.OperatingCenterSpoilRemovalCost?.Cost ?? 0, accrualValue)
        {
        }

        public SpoilTotal(string opCode, SpoilStorageLocation spoilStorageLocation, decimal total, decimal unitCost, decimal accrualValue)
        {
            _opCode = opCode;
            _spoilStorageLocation = spoilStorageLocation;
            _total = total;
            _unitCost = unitCost;
            _accrualValue = accrualValue;
        }

        #endregion
    }
}