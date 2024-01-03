using System;
using MMSINC.Controls;
using WorkOrders.Library.Controls;
using WorkOrders.Model;

namespace LINQTo271.Common
{
    public partial class LatLonPicker : WorkOrdersMvpUserControl, ILatLonPicker
    {
        #region Control Declarations

        public IHiddenField hidLatitude { get; protected set; }
        public IHiddenField hidLongitude { get; protected set; }
        protected IImageButton imgShowPicker;
        protected IHiddenField hidState;

        #endregion

        #region Constants

        public const string DEFAULT_STATE_ABBREV = "NJ",
                            OUTER_DIV_ID_FORMAT = "{0}OuterDiv",
                            IMAGE_PATH_FORMAT = "~/Includes/{0}";
        public struct ImageFileNames
        {
            public const string GOOD = "map-icon-blue.png",
                                BAD = "map-icon-red.png";
        }

        #endregion

        #region Private Members

        protected Asset _asset;

        #endregion

        #region Properties

        public Asset Asset
        {
            get
            {
                if (_asset == null)
                    _asset = GetAsset();
                return _asset;
            }
        }

        public int? AssetID { get; set; }
        public int? AssetTypeID { get; set; }
        
        // TODO: Futher hacked the previous hack here. ARR
        // It looks like this still didn't get fully implmented in the correct fasion. 
        // HiddenLatitude/Lon needs to go away or be tested. Proper design is needed 
        // here and the change to do it may be with Sewer Assets.
        public double? Latitude
        {
            get { return (Asset == null) ? HiddenLatitude : Asset.Latitude; }
            set { hidLatitude.Value = value.ToString(); }
        }

        public double? HiddenLatitude
        {
            get
            {
                double val;
                return (double.TryParse(hidLatitude.Value, out val))
                           ? val : (double?)null;
            }
        }

        public double? Longitude
        {
            get { return (Asset == null) ? HiddenLongitude : Asset.Longitude; }
            set { hidLongitude.Value = value.ToString(); }
        }

        public double? HiddenLongitude
        {
            get
            {
                double val;
                return double.TryParse(hidLongitude.Value, out val)
                           ? val : (double?)null;
            }
        }

        public string State
        {
            get
            {
                if(string.IsNullOrEmpty(hidState.Value))
                    SetState();
                return hidState.Value;
            }
            set { hidState.Value = value; }
        }

        public string ClientClickHandler { get; set; }

        public override string ClientID
        {
            get { return OuterDivID; }
        }

        protected virtual string OuterDivID
        {
            get { return String.Format(OUTER_DIV_ID_FORMAT, ID); }
        }

        protected bool HasCoordinates
        {
            get
            {
                return (Latitude != null && Longitude != null);
            }
        }

        #endregion

        #region Private Methods

        protected virtual void SetCoordinateFields()
        {
            if (!HasCoordinates)
                return;

            hidLatitude.Value = Latitude.ToString();
            hidLongitude.Value = Longitude.ToString();
        }

        protected virtual void SetIconUrl()
        {
            imgShowPicker.ImageUrl = String.Format(IMAGE_PATH_FORMAT,
                HasCoordinates ? ImageFileNames.GOOD : ImageFileNames.BAD);
        }

        protected virtual Asset GetAsset()
        {
            if (AssetTypeID == null || AssetID == null)
                return null;
            return Asset.GetAssetByIDs(AssetTypeID.Value, AssetID.Value);
        }

        protected virtual void SetState()
        {
            //TODO: THIS NEEDS SOME WORK.
            hidState.Value = DEFAULT_STATE_ABBREV;
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            SetState();
            SetCoordinateFields();
            SetIconUrl();
        }

        #endregion
    }

    public interface ILatLonPicker : IControl
    {
        #region Properties

        Asset Asset { get; }
        int? AssetID { get; set; }
        int? AssetTypeID { get; set; }
        IHiddenField hidLatitude { get; }
        IHiddenField hidLongitude { get; }
        double? Latitude { get; }
        double? Longitude { get; }
        string State { get; set; }

        #endregion
    }
}
