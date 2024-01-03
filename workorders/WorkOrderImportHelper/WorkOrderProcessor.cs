using System;

namespace WorkOrderImportHelper
{
    public abstract class WorkOrderProcessor
    {
        #region Private Members

        protected Action<int> _displayCurrentWorkOrderID;
        protected Action<string> _outputFn;

        #endregion

        #region Constructors

        public WorkOrderProcessor(Action<int> displayCurrentWorkOrderID, Action<string> outputFn)
        {
            _displayCurrentWorkOrderID = displayCurrentWorkOrderID;
            _outputFn = outputFn;
        }

        #endregion

        #region Exposed Abstract Methods

        public abstract int LoadWorkOrders();

        #endregion
    }
}
