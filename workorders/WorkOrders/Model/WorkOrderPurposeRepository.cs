using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    /// <summary>
    /// Repository for retrieving WorkOrderPurpose objects from persistence.
    /// </summary>
    public class WorkOrderPurposeRepository : WorkOrdersRepository<WorkOrderPurpose>
    {
        #region Constants

        public struct Indices
        {
            public const short CUSTOMER = 1,
                               COMPLIANCE = 3,
                               SAFETY = 4,
                               LEAK_DETECTION = 5,
                               ESTIMATES = 10;
        }

        public struct Descriptions
        {
            public const string CUSTOMER = "Customer",
                                COMPLIANCE = "Compliance",
                                SAFETY = "Safety",
                                LEAK_DETECTION = "Leak Detection",
                                ESTIMATES = "Estimates";
        }

        public static readonly int[] REVENUE = new[] {
            6, // 150-500
            7, // 500-1000
            8 // > 1000
        };

        #endregion

        #region Private Static Members

        private static WorkOrderPurpose _customer,
                                        _complaince,
                                        _safety,
                                        _leakDetection;

        #endregion

        #region Static Properties

        public static WorkOrderPurpose Customer
        {
            get
            {
                _customer = RetrieveAndAttach(Indices.CUSTOMER);
                return _customer;
            }
        }

        public static WorkOrderPurpose Complaince
        {
            get
            {
                _complaince = RetrieveAndAttach(Indices.COMPLIANCE);
                return _complaince;
            }
        }

        public static WorkOrderPurpose Safety
        {
            get
            {
                _safety = RetrieveAndAttach(Indices.SAFETY);
                return _safety;
            }
        }

        public static WorkOrderPurpose LeakDetection
        {
            get
            {
                _leakDetection = RetrieveAndAttach(Indices.LEAK_DETECTION);
                return _leakDetection;
            }
        }

        #endregion

        #region Private Static Methods

        private static WorkOrderPurpose RetrieveAndAttach(int index)
        {
            var entity = GetEntity(index);
            if (!DataTable.Contains(entity))
                DataTable.Attach(entity);
            return entity;
        }

        #endregion
    }
}