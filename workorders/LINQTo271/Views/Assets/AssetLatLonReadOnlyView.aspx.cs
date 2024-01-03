using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using StructureMap;
using StructureMap.Pipeline;
using WorkOrders;
using WorkOrders.Model;
using WorkOrders.Views.Assets;

namespace LINQTo271.Views.Assets
{
    public partial class AssetLatLonReadOnlyView : AssetLatLonPage, IAssetLatLonPickerView
    {
        #region Constants

        public struct QueryStringKeys
        {
            public const string ASSET_ID = "assetID",
                                ASSET_TYPE_ID = "assetTypeID",
                                LOCATION = "location", 
                                LATITUDE = "latitude", 
                                LONGITUDE = "longitude", 
                                OPERATING_CENTER = "operatingCenter";
        }

        public struct DefaultCoordinates
        {
            public const double LATITUDE = 40.32246702,
                                LONGITUDE = -74.14810180;
        }

        #endregion

        #region Control Declarations

        protected IHiddenField hidLatitude, hidLongitude;

        #endregion

        #region Private Members

        protected Asset _asset;
        protected int? _assetID, _assetTypeID;
        protected IDetailPresenter<Asset> _presenter;
        protected IRepository<Valve> _valveRepository;
        protected IRepository<Hydrant> _hydrantRepository;
        protected IRepository<SewerOpening> _sewerOpeningRepository;
        protected IRepository<StormCatch> _stormCatchRepository;
        protected IRepository<Equipment> _equipmentRepository;

        public static int DEFAULT_ASSET_ID = -1;
        /* TODO: Uncomment these when Main and Service classes are created
        private IRepository<Main> _mainRepository;
        private IRepository<Service> _serviceRepository;
        */

        #endregion

        #region Properties

        public Asset Asset
        {
            get
            {
                if (_asset == null)
                    _asset = Asset.GetAssetByIDs(AssetTypeID, AssetID);
                return _asset;
            }
        }

        /// <summary>
        /// Hacked-20090309 - Must fix. 
        /// </summary>
        public int AssetID
        {
            get
            {
                if (_assetID == null)
                {
                    if ((AssetTypeID == (int)AssetTypeEnum.Hydrant + 1 ||
                         AssetTypeID == (int)AssetTypeEnum.Valve + 1 || 
						 AssetTypeID == (int)AssetTypeEnum.SewerOpening + 1 ||
                         AssetTypeID == (int)AssetTypeEnum.StormCatch + 1 ||
                         AssetTypeID == (int)AssetTypeEnum.Equipment + 1))
                    {
                        _assetID =
                            IRequest.IQueryString.GetValue<int?>(
                                QueryStringKeys.ASSET_ID);
                        if (_assetID == null)
                        {
                            throw new NullReferenceException(
                                "Must provide an assetID value via the QueryString.");
                        }
                    }
                }
                if (AssetTypeID == (int)AssetTypeEnum.Hydrant + 1 || 
                    AssetTypeID == (int)AssetTypeEnum.Valve + 1 || 
                    AssetTypeID == (int)AssetTypeEnum.SewerOpening + 1 ||
                    AssetTypeID == (int)AssetTypeEnum.StormCatch + 1 ||
                    AssetTypeID == (int)AssetTypeEnum.Equipment + 1)
                    return _assetID.Value;
                return DEFAULT_ASSET_ID;
            }
        }

        public int AssetTypeID
        {
            get
            {
                if (_assetTypeID == null)
                {
                    _assetTypeID =
                        IRequest.IQueryString.GetValue<int?>(
                            QueryStringKeys.ASSET_TYPE_ID);
                    if (_assetTypeID == null)
                    {
                        throw new NullReferenceException(
                            "Must provide an assetTypeID value via the QueryString.");
                    }
                }
                return _assetTypeID.Value;
            }
        }

        public Double? QueryStringLatitude
        {
            get
            {
                return
                    IRequest.IQueryString.GetValue<Double?>(
                        QueryStringKeys.LATITUDE);
            } 
        }
        
        public double Latitude
        {
            get
            {
                double latitude;
                if (hidLatitude == null ||
                    String.IsNullOrEmpty(hidLatitude.Value) ||
                    !Double.TryParse(hidLatitude.Value, out latitude))
                {
                    latitude = ((Asset != null) ?
                        Asset.Latitude :
                        QueryStringLatitude) ??
                        DefaultCoordinates.LATITUDE;
                }
                return latitude;
            }
            set { hidLatitude.Value = value.ToString(); }
        }

        public Double? QueryStringLongitude
        {
            get
            {
                return
                    IRequest.IQueryString.GetValue<Double?>(
                        QueryStringKeys.LONGITUDE);
            }
        }

        public double Longitude
        {
            get
            {
                double longitude;
                if (hidLongitude == null ||
                    String.IsNullOrEmpty(hidLongitude.Value) ||
                    !Double.TryParse(hidLongitude.Value, out longitude))
                {
                    longitude = ((Asset != null) ?
                        Asset.Longitude :
                        QueryStringLongitude) ??
                        DefaultCoordinates.LONGITUDE;
                }
                return longitude;
            }
            set { hidLongitude.Value = value.ToString(); }
        }

        public int? QueryStringOperatingCenter
        {
            get
            {
                return IRequest.IQueryString.GetValue<int?>(QueryStringKeys.OPERATING_CENTER);
            }
        }

        public override string MapId
        {
            get
            {
                if (QueryStringOperatingCenter.HasValue)
                    OperatingCenter =
                        DependencyResolver.Current.GetService<IOperatingCenterRepository>().GetAll271OperatingCenters().First(x => x.OperatingCenterID == QueryStringOperatingCenter.Value);
                return base.MapId;
            }
        }

        public IDetailPresenter<Asset> Presenter
        {
            get { return _presenter; }
        }

        public DetailViewMode CurrentMode
        {
            get { return DetailViewMode.Edit; }
        }

        public IEnumerable<IChildResourceView> ChildResourceViews
        {
            get { return null; }
        }

        public IRepository<Valve> ValveRepository
        {
            get
            {
                if (_valveRepository == null)
                    _valveRepository =
                        DependencyResolver.Current.GetService<IRepository<Valve>>();
                return _valveRepository;
            }
        }

        public IRepository<Hydrant> HydrantRepository
        {
            get
            {
                if (_hydrantRepository == null)
                    _hydrantRepository =
                        DependencyResolver.Current.GetService<IRepository<Hydrant>>();
                return _hydrantRepository;
            }
        }

        public IRepository<SewerOpening> SewerOpeningRepository
        {
            get
            {
                if (_sewerOpeningRepository == null)
                    _sewerOpeningRepository =
                        DependencyResolver.Current.GetService<IRepository<SewerOpening>>();
                return _sewerOpeningRepository;
            }
        }

        public IRepository<StormCatch> StormCatchRepository
        {
            get
            {
                if (_stormCatchRepository == null)
                    _stormCatchRepository =
                        DependencyResolver.Current.GetService<IRepository<StormCatch>>();
                return _stormCatchRepository;
            }
        }

        public IRepository<Equipment> EquipmentRepository
        {
            get
            {
                if (_equipmentRepository == null)
                    _equipmentRepository =
                        DependencyResolver.Current.GetService<IRepository<Equipment>>();
                return _equipmentRepository;
            }
        }

        /* TODO: Uncomment these when Main and Service classes are created
        public IRepository<Hydrant> MainRepository
        {
            get
            {
                if (_hydrantRepository == null)
                    _hydrantRepository =
                        WorkOrdersContainer.Instance.GetImplementorOf
                            <IRepository<Main>>();
                return _mainRepository;
            }
        }

        public IRepository<Hydrant> ServiceRepository
        {
            get
            {
                if (_hydrantRepository == null)
                    _hydrantRepository =
                        WorkOrdersContainer.Instance.GetImplementorOf
                            <IRepository<Service>>();
                return _serviceRepository;
            }
        }
        */

        public object CurrentDataKey
        {
            get { return null; }
        }

        #if DEBUG

        public override bool IsMvpPostBack
        {
            get
            {
                return (_isMvpPostBack == null) ?
                    IsPostBack : _isMvpPostBack.Value;
            }
        }

        #endif

        #endregion

        #region Events

        public event EventHandler EditClicked,
            DiscardChangesClicked,
            UserControlLoaded,
            EntityLoaded;

        public event EventHandler<EntityEventArgs<Asset>> Inserting,
                                                          DeleteClicked;

        public virtual event EventHandler<EntityEventArgs<Asset>> Updating;

        #endregion

        #region Event Handlers

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            _presenter =
                DependencyResolver.Current.GetService<IContainer>().GetInstance<IDetailPresenter<Asset>>(
                    new ExplicitArguments(new Dictionary<string, object> {
                        {"view", this}
                    }));

            if (!IsMvpPostBack)
            {
                Presenter.OnViewInitialized();
                DataBind();
            }

            Presenter.OnViewLoaded();
        }

        #endregion

        #region Exposed Methods

        public void SetViewControlsVisible(bool visible)
        {
            throw new NotImplementedException();
        }

        public void SetViewMode(DetailViewMode mode)
        {
            throw new NotImplementedException();
        }

        public void ShowEntity(Asset entity)
        {
            throw new NotImplementedException();
        }

        public void SetChildResourceViewFilterExpressions(Asset entity)
        {
            throw new NotImplementedException();
        }

        public TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
