using System;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using WorkOrders.Model;
using WorkOrders.Views.SpoilFinalProcessingLocations;

namespace LINQTo271.Views.SpoilFinalProcessingLocations
{
    public partial class SpoilFinalProcessingLocationListView : WorkOrdersListView<SpoilFinalProcessingLocation>, ISpoilFinalProcessingLocationListView
    {
        #region Constants

        public struct ControlIDs
        {
            public const string NAME = "txtName",
                                TOWN_ID = "ddlTown",
                                STREET_ID = "ddlStreet";
        }

        public struct EntityKeys
        {
            public const string OPERATING_CENTER_ID = "OperatingCenterID",
                                NAME = "Name",
                                TOWN_ID = "TownID",
                                STREET_ID = "StreetID";
        }

        #endregion

        #region Control Declarations

        protected IObjectDataSource odsSpoilFinalProcessingLocations, odsTowns;
        protected IGridView gvSpoilFinalProcessingLocations;
        protected ITextBox txtName;
        protected IDropDownList ddlTown, ddlStreet;

        #endregion

        #region Properties

        #region Form Values

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

        public string Name
        {
            get { return NameTextBox.Text; }
        }

        public int? TownID
        {
            get { return TownDropDown.GetSelectedValue(); }
        }

        public int? StreetID
        {
            get { return (StreetDropDown == null) ? null : StreetDropDown.GetSelectedValue(); }
        }

        #endregion

        public override IListControl ListControl
        {
            get { return gvSpoilFinalProcessingLocations; }
        }

        protected ITextBox NameTextBox
        {
            get
            {
                if (txtName == null)
                    txtName =
                        gvSpoilFinalProcessingLocations.IFooterRow.FindIControl
                            <ITextBox>(ControlIDs.NAME);
                return txtName;
            }
        }

        protected IDropDownList TownDropDown
        {
            get
            {
                if (ddlTown == null)
                    ddlTown =
                        gvSpoilFinalProcessingLocations.IFooterRow.FindIControl
                            <IDropDownList>(ControlIDs.TOWN_ID);
                return ddlTown;
            }
        }

        protected IDropDownList StreetDropDown
        {
            get
            {
                if (ddlStreet == null)
                    ddlStreet =
                        gvSpoilFinalProcessingLocations.IFooterRow.FindIControl
                            <IDropDownList>(ControlIDs.STREET_ID);
                return ddlStreet;
            }
        }

        #endregion

        #region Event Handlers

        protected void ListControl_DataBinding(object sender, EventArgs e)
        {
            odsSpoilFinalProcessingLocations.SetDefaultSelectParameterValue(
                EntityKeys.OPERATING_CENTER_ID, OperatingCenterID.ToString());
            odsTowns.SetDefaultSelectParameterValue(
                EntityKeys.OPERATING_CENTER_ID, OperatingCenterID.ToString());
        }

        protected void odsSpoilFinalProcessingLocations_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            e.InputParameters[EntityKeys.OPERATING_CENTER_ID] =
                OperatingCenterID;
            e.InputParameters[EntityKeys.NAME] = Name;
            if (TownID != null)
            {
                e.InputParameters[EntityKeys.TOWN_ID] = TownID;
                if (StreetID != null)
                {
                    e.InputParameters[EntityKeys.STREET_ID] = StreetID;
                }
            }
        }


        protected void odsSpoilFinalProcessingLocations_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var streetID = (e.InputParameters[EntityKeys.STREET_ID] ?? string.Empty).ToString();

            if (!String.IsNullOrEmpty(streetID))
                e.InputParameters[EntityKeys.STREET_ID] =
                    new Regex(":::.*").Replace(streetID, String.Empty);
        }

        // TODO: This should be pushed down to a common base, the functionality is shared in a number of classes
        protected void lbInsert_Click(object sender, EventArgs e)
        {
            if (IPage.IsValid)
            {
                odsSpoilFinalProcessingLocations.Insert();
            }
        }

        protected void lbCancel_Click(object sender, EventArgs e)
        {
            // herp derp.
        }

        #endregion

        #region Exposed Methods

        public override void SetViewControlsVisible(bool visible)
        {
            // noop
        }

        #endregion
    }
}