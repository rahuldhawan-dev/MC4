using System;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using WorkOrders.Model;
using WorkOrders.Views.SpoilRemovals;

namespace LINQTo271.Views.SpoilRemovals
{
    public partial class SpoilRemovalListView : WorkOrdersListView<SpoilRemoval>, ISpoilRemovalListView
    {
        #region Constants

        public struct ControlIDs
        {
            public const string REMOVED_FROM = "ddlSpoilStorageLocation";
            public const string FINAL_DESTINATION = "ddlSpoilFinalProcessingLocation";
            public const string DATE_REMOVED = "txtDateRemoved";
            public const string QUANTITY = "txtQuantity";
        }

        public struct EntityKeys
        {
            public const string OPERATING_CENTER_ID = "OperatingCenterID",
                                REMOVED_FROM = "RemovedFrom",
                                FINAL_DESTINATION ="FinalDestination",
                                DATE_REMOVED = "DateRemoved",
                                QUANTITY = "Quantity";
        }

        #endregion

        #region Control Declarations

        protected IObjectDataSource odsSpoilRemovals, odsSpoilStorageLocation, odsSpoilFinalProcessingLocation;
        protected IDropDownList ddlSpoilStorageLocation, ddlSpoilFinalProcessingLocation;
        protected ITextBox txtDateRemoved, txtQuantity;
        protected IGridView gvSpoilRemovals;

        #endregion

        #region Properties

        #region Form Values

        public int? RemovedFrom
        {
            get { return RemovedFromDropDownList.GetSelectedValue(); }
        }
        public int? FinalDestination
        {
            get { return FinalDestinationDropDownList.GetSelectedValue(); }
        }
        public string DateRemoved
        {
            get { return DateRemovedTextBox.Text;}
        }
        public string Quantity
        {
            get { return QuantityTextBox.Text;}
        }

        public int OperatingCenterID
        {
            get
            {
                return (int)IViewState.GetValue(EntityKeys.OPERATING_CENTER_ID);
            }
            set
            {
                IViewState.SetValue(EntityKeys.OPERATING_CENTER_ID, value);
            }
        }

        #endregion

        public IDropDownList RemovedFromDropDownList
        {
            get {
                if (ddlSpoilStorageLocation == null)
                    ddlSpoilStorageLocation =
                        gvSpoilRemovals.IFooterRow.FindIControl<IDropDownList>(
                            ControlIDs.REMOVED_FROM);
                return ddlSpoilStorageLocation;
            }
        }
        public IDropDownList FinalDestinationDropDownList
        {
            get {
                if (ddlSpoilFinalProcessingLocation == null)
                    ddlSpoilFinalProcessingLocation =
                        gvSpoilRemovals.IFooterRow.FindIControl<IDropDownList>(
                            ControlIDs.FINAL_DESTINATION);
                return ddlSpoilFinalProcessingLocation;
            }
        }
        public ITextBox DateRemovedTextBox
        {
            get
            {
                if (txtDateRemoved == null)
                    txtDateRemoved =
                        gvSpoilRemovals.IFooterRow
                            .FindIControl<ITextBox>(ControlIDs.DATE_REMOVED);
                return txtDateRemoved;
            }
        }
        public ITextBox QuantityTextBox
        {
            get
            {
                if (txtQuantity == null)
                    txtQuantity =
                        gvSpoilRemovals.IFooterRow
                            .FindIControl<ITextBox>(ControlIDs.QUANTITY);
                return txtQuantity;
            }
        }

        public override IListControl ListControl
        {
            get { return gvSpoilRemovals; }
        }

        #endregion

        #region Event Handlers

        // TODO: This should be pushed down to a common base, the functionality is shared in a number of classes
        protected void lbInsert_Click(object sender, EventArgs e)
        {
            if (IPage.IsValid)
            {
                odsSpoilRemovals.Insert();
            }
        }

        protected void lbCancel_Click(object sender, EventArgs e)
        {
            //Description = String.Empty;
        }

        protected void ListControl_DataBinding(object sender, EventArgs e)
        {
            odsSpoilRemovals.SetDefaultSelectParameterValue(
                EntityKeys.OPERATING_CENTER_ID, OperatingCenterID.ToString());
            odsSpoilFinalProcessingLocation.SetDefaultSelectParameterValue(
                EntityKeys.OPERATING_CENTER_ID, OperatingCenterID.ToString());
            odsSpoilStorageLocation.SetDefaultSelectParameterValue(
                EntityKeys.OPERATING_CENTER_ID, OperatingCenterID.ToString());
        }

        protected void odsSpoilRemovals_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            e.InputParameters[EntityKeys.REMOVED_FROM] = RemovedFrom;
            e.InputParameters[EntityKeys.FINAL_DESTINATION] = FinalDestination;
            e.InputParameters[EntityKeys.DATE_REMOVED] = DateRemoved;
            e.InputParameters[EntityKeys.QUANTITY] = Quantity;
        }

        #endregion

        #region Exposed Methods

        public override void SetViewControlsVisible(bool visible)
        {
            /* noop */
        }

        #endregion
    }
}