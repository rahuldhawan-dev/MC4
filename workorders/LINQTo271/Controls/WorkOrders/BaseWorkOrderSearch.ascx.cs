using System;
using System.Collections.Generic;
using MMSINC.Controls;
using WorkOrders.Library.Controls;
using WorkOrders.Library.Permissions;
using SecurityServiceClass = WorkOrders.Library.Permissions.SecurityService;

namespace LINQTo271.Controls.WorkOrders
{
    /// <summary>
    /// How low can you go?  Death row. What a borther knows.
    /// </summary>
    public partial class BaseWorkOrderSearch : WorkOrdersMvpUserControl, IBaseWorkOrderSearch
    {
        #region Constants

        public struct EntityKeys
        {
            public const string OPERATING_CENTER_ID = "OperatingCenterID";
        }

        #endregion

        #region Control Declarations

        protected ITableRow trWorkOrderNumber,
                            trOldWorkOrderNumber,
                            trTown,
                            trTownSection,
                            trStreetNumber,
                            trApartmentAddtl,
                            trStreet,
                            trNearestCrossStreet,
                            trAssetType,
                            trDescriptionOfWork;

        protected ILabel lblError;

        protected IDropDownList ddlTown,
                                ddlTownSection,
                                ddlStreet,
                                ddlNearestCrossStreet,
                                ddlAssetType,
                                ddlOperatingCenter;

        protected ITextBox txtWorkOrderNumber,
                           txtStreetNumber,
                           txtApartmentAddtl;

        protected IListBox lstDescriptionOfWork;

        #endregion

        #region Private Members

        private bool _visibilityConfigured = false;

        private bool? _showAssetType,
                      _showDescriptionOfWork,
                      _showNearestCrossStreet,
                      _showOldWorkOrderNumber,
                      _showStreet,
                      _showStreetNumber,
                      _showApartmentAddtl,
                      _showTown,
                      _showTownSection,
                      _showWorkOrderNumber;

        protected ISecurityService _securityService;

        #endregion

        #region Properties

        public ISecurityService SecurityService
        {
            get
            {
                if (_securityService == null)
                    _securityService = SecurityServiceClass.Instance;
                return _securityService;
            }
        }

        #region Configuration Properties

        public bool ShowAssetType
        {
            set
            {
                _visibilityConfigured = true;
                _showAssetType = value;
            }
        }

        public bool ShowDescriptionOfWork
        {
            set
            {
                _visibilityConfigured = true;
                _showDescriptionOfWork = value;
            }
        }

        public bool ShowNearestCrossStreet
        {
            set
            {
                _visibilityConfigured = true;
                _showNearestCrossStreet = value;
            }
        }

        public bool ShowOldWorkOrderNumber
        {
            set
            {
                _visibilityConfigured = true;
                _showOldWorkOrderNumber = value;
            }
        }

        public bool ShowStreet
        {
            set
            {
                _visibilityConfigured = true;
                _showStreet = value;
            }
        }

        public bool ShowStreetNumber
        {
            set
            {
                _visibilityConfigured = true;
                _showStreetNumber = value;
            }
        }

        public bool ShowApartmentAddtl
        {
            set
            {
                _visibilityConfigured = true;
                _showApartmentAddtl = value;
            }
        }

        public bool ShowTown
        {
            set
            {
                _visibilityConfigured = true;
                _showTown = value;
            }
        }

        public bool ShowTownSection
        {
            set
            {
                _visibilityConfigured = true;
                _showTownSection = value;
            }
        }

        public bool ShowWorkOrderNumber
        {
            set
            {
                _visibilityConfigured = true;
                _showWorkOrderNumber = value;
            }
        }

        #endregion

        #region Value Properties

        public virtual int? OperatingCenterID
        {
            get { return ddlOperatingCenter.GetSelectedValue(); }
        }

        public virtual int? TownID
        {
            get { return ddlTown.GetSelectedValue(); }
        }

        public virtual int? TownSectionID
        {
            get { return ddlTownSection.GetSelectedValue(); }
        }

        public virtual int? StreetID
        {
            get { return ddlStreet.GetSelectedValue(); }
        }

        public virtual int? NearestCrossStreetID
        {
            get { return ddlNearestCrossStreet.GetSelectedValue(); }
        }

        public virtual int? AssetTypeID
        {
            get { return ddlAssetType.GetSelectedValue(); }
        }

        public virtual int? WorkOrderNumber
        {
            get { return txtWorkOrderNumber.TryGetIntValue(); }
        }
        
        public virtual List<int> DescriptionOfWorkIDs
        {
            get { return lstDescriptionOfWork.GetSelectedValues(); }
        }

        public virtual string StreetNumber
        {
            get { return txtStreetNumber.Text; }
        }

        public virtual string ApartmentAddtl
        {
            get { return txtApartmentAddtl.Text; }
        }

        #endregion

        #endregion

        #region Private Methods

        private void ToggleSearchControls()
        {
            trAssetType.Visible = _showAssetType ?? true;
            trDescriptionOfWork.Visible = _showDescriptionOfWork ?? true;
            trNearestCrossStreet.Visible = _showNearestCrossStreet ?? true;
            trWorkOrderNumber.Visible = _showWorkOrderNumber ?? true;
            trStreet.Visible = _showStreet ?? true;
            trApartmentAddtl.Visible = _showApartmentAddtl ?? true;
            trStreetNumber.Visible = _showStreetNumber ?? true;
            trTown.Visible = _showTown ?? true;
            trTownSection.Visible = _showTownSection ?? true;
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = String.Empty;

            if (_visibilityConfigured)
            {
                ToggleSearchControls();
            }
        }

        protected void ddlOperatingCenter_DataBound(object sender, EventArgs e)
        {
            if (SecurityService.UserOperatingCentersCount==1)
                ddlOperatingCenter.SelectedIndex = 1;
        }

        #endregion

        #region Exposed Methods

        public void DisplaySearchError(string message)
        {
            lblError.Text = message;
        }

        #endregion
    }

    public interface IBaseWorkOrderSearch
    {
        #region Properties

        int? OperatingCenterID { get; }
        int? TownID { get; }
        int? TownSectionID { get; }
        int? StreetID { get; }
        int? NearestCrossStreetID { get; }
        int? AssetTypeID { get; }
        int? WorkOrderNumber { get; }
        List<int> DescriptionOfWorkIDs { get; }
        string StreetNumber { get; }
        string ApartmentAddtl { get; }

        #endregion

        #region Methods

        void DisplaySearchError(string message);

        #endregion
    }
}