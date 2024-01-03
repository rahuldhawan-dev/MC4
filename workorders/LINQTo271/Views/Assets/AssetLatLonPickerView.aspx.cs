using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Common;
using MapCall.Common.Utility;
using WorkOrders.Model;

namespace LINQTo271.Views.Assets
{
    public partial class AssetLatLonPickerView : AssetLatLonReadOnlyView
    {
        #region Private Members

        private string _location;

        #endregion

        #region Properties

        public string Location
        {
            get
            {
                if (_location == null)
                    _location = HttpContext.Current.Request.QueryString[QueryStringKeys.LOCATION];
                return _location;
            }
        }

        #endregion

        #region Private Methods

        private EntityEventArgs<Asset> GetCurrentAssetAsEventArgs()
        {
            return new AssetEntityEventArgs(Asset, Latitude, Longitude);
        }

        #endregion

        #region Events

        public override event EventHandler<EntityEventArgs<Asset>> Updating;

        #endregion

        #region Event Handlers

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (IsMvpPostBack)
            {
                ClientScriptManager.TryRegisterClientScriptInclude("foo", "~/Views/Assets/AssetLatLongPickerViewClose.js");
            }
        }


        protected void btnFormSave_Click(object sender, EventArgs e)
        {
            OnSaveClicked(GetCurrentAssetAsEventArgs());
        }

        #endregion

        #region Event Passthroughs

        private void OnSaveClicked(EntityEventArgs<Asset> e)
        {
            if (Updating != null)
                Updating(this, e);
        }

        #endregion
    }
}
