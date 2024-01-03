using System;
using MMSINC.Common;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using WorkOrders.Model;
using WorkOrders.Views.Assets;

namespace WorkOrders.Presenters.Assets
{
    public class AssetLatLonPickerPresenter : IDetailPresenter<Asset>
    {
        #region Private Members

        private readonly IAssetLatLonPickerView _view;

        #endregion

        #region Properties

        /// <summary>
        /// Implementation of the typed IRepository object required by the
        /// IDetailPresenter contract.  This is not actually needed here,
        /// so attempting to get or set will throw a NotImplementedException.
        /// </summary>
        public IRepository<Asset> Repository
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        #endregion

        #region Constructors

        public AssetLatLonPickerPresenter(IAssetLatLonPickerView view)
        {
            _view = view;
        }

        #endregion

        #region Event Passthroughs

        public void OnViewInitialized()
        {
            // noop
        }

        public void OnViewLoaded()
        {
            // noop
        }

        #endregion
    }
}
