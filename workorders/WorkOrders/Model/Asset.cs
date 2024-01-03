using System;
using System.Data.Linq;
using System.Web.Mvc;
using MMSINC.Data.Linq;
using StructureMap;

namespace WorkOrders.Model
{
    /// <summary>
    /// Logical class to represent an Asset (Valve, Hydrant, Main, or Service),
    /// aggregate its WorkOrders and coordinates, along with its AssetType.
    ///
    /// This object does not map directly to a table in the schema.
    /// </summary>
    public class Asset
    {
        #region Constants

        private const string WRONG_ASSET_TYPE_MESSAGE_FORMAT =
            "Cannot create an Asset object with AssetType {0} without supplying an argument for the {0} property.";

        #endregion

        #region Private Members

        private readonly AssetType _assetType;
        private readonly Valve _valve;
        private readonly Hydrant _hydrant;
        private readonly SewerOpening _sewerOpening;
        private readonly StormCatch _stormCatch;
        private readonly Equipment _equipment;
        private readonly MainCrossing _mainCrossing;

        private IAsset _innerAsset;

        #endregion

        #region Properties

        /// <summary>
        /// AssetType object representing the type of the InnerAsset managed
        /// by this class.
        /// </summary>
        public AssetType AssetType
        {
            get { return _assetType; }
        }

        /// <summary>
        /// Valve object to hold a Valve when AssetType is Valve.
        /// </summary>
        public Valve Valve
        {
            get { return _valve; }
        }

        /// <summary>
        /// Hydrant object to hold a Hydrant when AssetType is Hydrant.
        /// </summary>
        public Hydrant Hydrant
        {
            get { return _hydrant; }
        }

        /// <summary>
        /// SewerOpening object to hold the SewerOpening when AssetType is SewerOpening
        /// </summary>
        public SewerOpening SewerOpening
        {
            get { return _sewerOpening; }
        }

        /// <summary>
        /// StormCatch object to hold the StormCatch when AssetType is StormCatch
        /// </summary>
        public StormCatch StormCatch
        {
            get { return _stormCatch; }
        }

        public Equipment Equipment
        {
            get { return _equipment; }
        }

        public MainCrossing MainCrossing
        {
            get { return _mainCrossing; }
        }

        /// <summary>
        /// IAsset object used to gather the properties common to all asset
        /// object types.
        /// </summary>
        public IAsset InnerAsset
        {
            get { return _innerAsset; }
        }

        public double? Latitude
        {
            get { return InnerAsset.Latitude; }
            set { InnerAsset.Latitude = value;}
        }

        public double? Longitude
        {
            get { return InnerAsset.Longitude; }
            set { InnerAsset.Longitude = value; }
        }

        /// <summary>
        /// Primary key value of a given Asset.
        /// </summary>
        public object AssetKey
        {
            get { return InnerAsset.AssetKey; }
        }

        /// <summary>
        /// Domain level identifier used to describe an Asset.
        /// </summary>
        public string AssetID
        {
            get { return (InnerAsset == null) ? String.Empty : InnerAsset.AssetID; }
        }

        public int? SAPEquipmentID
        {
            get
            {
                if (Valve != null)
                {
                    return Valve.SAPEquipmentID;
                }
                if (Hydrant != null)
                {
                    return Hydrant.SAPEquipmentID;
                }
                return null;
            }
        }

        /// <summary>
        /// Collection of all the WorkOrder objects linked to a given Asset.
        /// </summary>
        public EntitySet<WorkOrder> WorkOrders
        {
            get { return InnerAsset.WorkOrders; }
        }

        public IRepository<Valve> ValveRepository
        {
            get
            {
                return DependencyResolver.Current.GetService<IRepository<Valve>>();
            }
        }

        public IRepository<Hydrant> HydrantRepository
        {
            get
            {
                return DependencyResolver.Current.GetService<IRepository<Hydrant>>();
            }
        }

        public IRepository<SewerOpening> SewerOpeningRepository
        {
            get
            {
                return DependencyResolver.Current.GetService<IRepository<SewerOpening>>();
            }
        }

        public IRepository<StormCatch> StormCatchRepository
        {
            get
            {
                return DependencyResolver.Current.GetService<IRepository<StormCatch>>();
            }
        }

        public IRepository<Equipment> EquipmentRepository
        {
            get { return DependencyResolver.Current.GetService<IRepository<Equipment>>(); }
        }

        public IRepository<MainCrossing> MainCrossingRepository
        {
            get { return DependencyResolver.Current.GetService<IRepository<MainCrossing>>(); }
        }

        #endregion

        #region Constructors

        private Asset(AssetType assetType)
        {
            _assetType = assetType;
        }

        /// <summary>
        /// Creates a new instance of the Asset class, using a Valve as the
        /// InnerAsset.  This will throw an exception if either object is null,
        /// or if AssetType is not Valve.
        /// </summary>
        /// <param name="assetType">AssetType object representing the type of
        /// asset this object will manage.  Must be AssetType.Valve.</param>
        /// <param name="valve">Valve object this object will manage.  Must
        /// not be null.</param>
        public Asset(AssetType assetType, Valve valve) : this(assetType, valve, null, null, null, null, null)
        {
        }

        /// <summary>
        /// Creates a new instance of the Asset class, using a Hydrant as the
        /// InnerAsset.  This will throw an exception if either object is null,
        /// or if AssetType is not Hydrant.
        /// </summary>
        /// <param name="assetType">AssetType object representing the type of
        /// asset this object will manage.  Must be AssetType.Hydrant.</param>
        /// <param name="hydrant">Hydrant object this object will manage.  Must
        /// not be null.</param>
        public Asset(AssetType assetType, Hydrant hydrant)
            : this(assetType, null, hydrant, null, null, null, null)
        {
        }

        /// <summary>
        /// Behaves the same as Hydrant/Valve
        /// </summary>
        /// <param name="assetType"></param>
        /// <param name="sewerOpening"></param>
        public Asset(AssetType assetType, SewerOpening sewerOpening)
            : this(assetType, null, null, sewerOpening, null, null, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetType"></param>
        /// <param name="stormCatch"></param>
        public Asset(AssetType assetType, StormCatch stormCatch)
            : this(assetType, null, null, null, stormCatch, null, null)
        {

        }

        public Asset(AssetType assetType, Equipment equipment)
            : this(assetType, null, null, null, null, equipment, null)
        {
            
        }

        public Asset(AssetType assetType, MainCrossing mainCrossing) : this(assetType, null, null, null, null, null, mainCrossing) { }

        /// <summary>
        /// Creates a new instance of the Asset class, using the supplied object
        /// which matches the AssetType supplied.  This will throw an exception
        /// if no asset supplied, if assetType is null, or if assetType does not
        /// match the non-null object supplied.
        /// </summary>
        /// <param name="assetType">AssetType object representing the type of
        /// asset this object will manage.  Must match the type of the non-null
        /// asset argument supplied.</param>
        /// <param name="valve">Valve object this object will manage.  Must be
        /// non-null if assetType is Valve.</param>
        /// <param name="hydrant">Hydrant object this object will manage.  Must be
        /// non-null if assetType is Hydrant.</param>
        /// <param name="sewerOpening">SewerOpening object this object will manage.
        /// Must be non-null if assetType is SewerOpening </param>
        /// <param name="stormCatch">"</param>
        /// <param name="equipment"></param>
        public Asset(AssetType assetType, Valve valve, Hydrant hydrant, SewerOpening sewerOpening, StormCatch stormCatch, Equipment equipment, MainCrossing mainCrossing) : this(assetType)
        {
            _valve = valve;
            _hydrant = hydrant;
            _sewerOpening = sewerOpening;
            _stormCatch = stormCatch;
            _equipment = equipment;
            _mainCrossing = mainCrossing;

            VerifyAssetWithType();
        }

        public Asset(AssetType assetType, int? valveID, int? hydrantID, int? sewerOpeningID, int? stormCatchID, int? equipmentID, int? mainCrossingID) : this(assetType)
        {
            if (valveID != null)
                _valve = ValveRepository.Get(valveID);
            else if (hydrantID != null)
                _hydrant = HydrantRepository.Get(hydrantID);
            else if (sewerOpeningID != null)
                _sewerOpening = SewerOpeningRepository.Get(sewerOpeningID);
            else if (stormCatchID != null)
                _stormCatch = StormCatchRepository.Get(stormCatchID);
            else if (equipmentID != null)
                _equipment = EquipmentRepository.Get(equipmentID);
            else if (mainCrossingID != null)
                _mainCrossing = MainCrossingRepository.Get(mainCrossingID);
            VerifyAssetWithType();
        }

        #endregion

        #region Private Members

        private void VerifyAssetWithType()
        {
            switch (AssetType.TypeEnum)
            {
                case AssetTypeEnum.Valve:
                    _innerAsset = Valve;
                    break;
                case AssetTypeEnum.Hydrant:
                    _innerAsset = Hydrant;
                    break;
                case AssetTypeEnum.SewerOpening:
                    _innerAsset = SewerOpening;
                    break;
                case AssetTypeEnum.StormCatch:
                    _innerAsset = StormCatch;
                    break;
                case AssetTypeEnum.Equipment:
                    _innerAsset = Equipment;
                    break;
                case AssetTypeEnum.MainCrossing:
                    _innerAsset = MainCrossing;
                    break;
                // TODO:
                // needed to break this so they can save work orders
                // for mains and services
                case AssetTypeEnum.Service:
                case AssetTypeEnum.Main:
                    return;
                default:
                    _innerAsset = null;
                    break;
            }

            switch (AssetType.TypeEnum)
            {
                case AssetTypeEnum.Valve:
                case AssetTypeEnum.Hydrant:
                case AssetTypeEnum.SewerOpening:
                case AssetTypeEnum.StormCatch:
                case AssetTypeEnum.Equipment:
                case AssetTypeEnum.MainCrossing:
                    if (_innerAsset == null)
                        throw new ArgumentException(
                            String.Format(WRONG_ASSET_TYPE_MESSAGE_FORMAT,
                                AssetType.Description));
                    break;
            }
        }

        #endregion

        #region Exposed Static Methods

        public static Asset GetAssetByIDs(int assetTypeID, object assetKey)
        {
            var assetType = AssetTypeRepository.GetEntity(assetTypeID);
            return GetAssetByTypeAndKey(assetType, assetKey);
        }

        /// <summary>
        /// Creates an Asset object with the given type and key.
        /// </summary>
        /// <param name="assetType">AssetType object representing the type of
        /// asset object the new instance will manage.</param>
        /// <param name="assetKey">Primary key value of the asset being managed.
        /// </param>
        /// <returns>An Asset object containing the correct AssetType and
        /// InnerAsset based on the supplied objects.</returns>
        public static Asset GetAssetByTypeAndKey(AssetType assetType, object assetKey)
        {
            // TODO: Review. I don't think these can be moved to properties
            // because this method is static.
            switch (assetType.TypeEnum)
            {
                case AssetTypeEnum.Valve:
                    return new Asset(assetType, Model.ValveRepository.GetEntity(assetKey));
                case AssetTypeEnum.Hydrant:
                    return new Asset(assetType, Model.HydrantRepository.GetEntity(assetKey));
                case AssetTypeEnum.SewerOpening:
                    return new Asset(assetType, Model.SewerOpeningRepository.GetEntity(assetKey));
                case AssetTypeEnum.StormCatch:
                    return new Asset(assetType, Model.StormCatchRepository.GetEntity(assetKey));
                case AssetTypeEnum.Equipment:
                    return new Asset(assetType, Model.EquipmentRepository.GetEntity(assetKey));
                case AssetTypeEnum.MainCrossing:
                    return new Asset(assetType, Model.MainCrossingRepository.GetEntity(assetKey));
                default:
                    return null;
            }
        }

        #endregion
    }
}
