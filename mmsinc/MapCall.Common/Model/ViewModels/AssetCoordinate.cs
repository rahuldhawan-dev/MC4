using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using StructureMap;
using StructureMap.Attributes;

namespace MapCall.Common.Model.ViewModels
{
    public enum AssetIconType
    {
        Default,
        OutOfService, // Hydrant only
        OutOfServiceWithWorkOrder, // Hydrant only
        RequiresInspection,
        RequiresInspectionWithWorkOrder,
        WorkOrder,
        Inactive,
        NonPublic,
        NormallyOpenButClosed, // Valve Only
        NormallyOpenButClosedWithWorkOrder, // Valve Only
        NormallyClosedButOpen, // Valve Only
        NormallyClosedButOpenWithWorkOrder, // Valve Only
    }

    public enum AssetCoordinateType
    {
        Hydrant,
        Valve,
        BlowOff,
        MainCrossing,
        BelowGroundHazard,
        SewerOpening
    }

    public interface IAssetCoordinateSearch : ISearchSet<AssetCoordinate>
    {
        #region Abstract Properties

        int[] OperatingCenter { get; set; }

        [Search(CanMap = false)]
        decimal? LatitudeMin { get; set; }

        [Search(CanMap = false)]
        decimal? LatitudeMax { get; set; }

        [Search(CanMap = false)]
        decimal? LongitudeMin { get; set; }

        [Search(CanMap = false)]
        decimal? LongitudeMax { get; set; }

        #endregion
    }

    public abstract class AssetCoordinate : IThingWithCoordinate
    {
        #region Fields

        private bool _coordinateInitialized;
        private Coordinate _coord;
        private AssetIconType? _assetIconType;
        private MapIcon _icon;
        [NonSerialized] private IIconSetRepository _iconSetRepository;

        #endregion

        #region Properties

        public virtual Coordinate Coordinate
        {
            get
            {
                // ToAssetCoordinate on Hydrant/Valve will not set Latitude/Longitude if there
                // isn't a Coordinate record. We want to return null here as well so MapResult can
                // properly filter them out.
                if (!_coordinateInitialized)
                {
                    if (Latitude.HasValue && Longitude.HasValue)
                    {
                        _coord = new Coordinate {
                            Latitude = Latitude.Value,
                            Longitude = Longitude.Value
                        };
                    }

                    _coordinateInitialized = true;
                }

                return _coord;
            }
            set { }
        }

        public int Id { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool RequiresInspection { get; set; }
        public bool RequiresPainting { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublic { get; set; }
        public bool OutOfService { get; set; } // Hydrant only.
        public DateTime? LastInspection { get; set; }
        public bool HasOpenWorkOrder { get; set; }

        public bool? InNormalPosition { get; set; } // Valve only.
        public ValveNormalPosition NormalPosition { get; set; } // Valve only.

        public AssetIconType IconType
        {
            get
            {
                if (!_assetIconType.HasValue)
                {
                    if (!IsActive)
                    {
                        _assetIconType = AssetIconType.Inactive;
                    }

                    else if (OutOfService) // Hydrant only.
                    {
                        if (RequiresWorkOrder)
                        {
                            _assetIconType = AssetIconType.OutOfServiceWithWorkOrder;
                        }
                        else
                        {
                            _assetIconType = AssetIconType.OutOfService;
                        }
                    }

                    // NOTE: This must be false. Null should not be considered false.
                    else if (InNormalPosition.HasValue && InNormalPosition.Value == false) // Valves only 
                    {
                        if (NormalPosition?.Id == ValveNormalPosition.Indices.OPEN)
                        {
                            if (RequiresWorkOrder)
                            {
                                _assetIconType = AssetIconType.NormallyOpenButClosedWithWorkOrder;
                            }
                            else
                            {
                                _assetIconType = AssetIconType.NormallyOpenButClosed;
                            }
                        }
                        else if (NormalPosition?.Id == ValveNormalPosition.Indices.CLOSED)
                        {
                            if (RequiresWorkOrder)
                            {
                                _assetIconType = AssetIconType.NormallyClosedButOpenWithWorkOrder;
                            }
                            else
                            {
                                _assetIconType = AssetIconType.NormallyClosedButOpen;
                            }
                        }
                        else
                        {
                            throw new NotSupportedException($"Unknown ValveNormalPosition: '{NormalPosition}'");
                        }
                    }
                    else if (RequiresInspection)
                    {
                        _assetIconType = RequiresWorkOrder
                            ? AssetIconType.RequiresInspectionWithWorkOrder
                            : AssetIconType.RequiresInspection;
                    }

                    else if (RequiresWorkOrder)
                    {
                        _assetIconType = AssetIconType.WorkOrder;
                    }

                    else if (!IsPublic)
                    {
                        _assetIconType = AssetIconType.NonPublic;
                    }
                    else
                    {
                        _assetIconType = AssetIconType.Default;
                    }
                }

                return _assetIconType.Value;
            }
        }

        /// <summary>
        /// This property should really be LastInspectionRecordHadWorkOrder
        /// </summary>
        public bool RequiresWorkOrder =>
            // LastNonInspection is an inspection with a WorkOrderRequest
            HasOpenWorkOrder;

        public MapIcon Icon
        {
            get
            {
                if (_icon == null)
                {
                    // This kinda smells, but the way it's using Fine caches it so the iconSet is only queried for
                    // one time, so no real performance hit.
                    var iconSet = _iconSetRepository.Find(IconSets.Assets);
                    Func<string, MapIcon> getIcon = (fileName) => {
                        var path = "MapIcons/" + AssetType + "-" + fileName + ".png";
                        // Ignore case because AssetType will return a capitalized word and all the file names are lowercase.
                        return iconSet.Icons.Single(x =>
                            string.Equals(x.FileName, path, StringComparison.InvariantCultureIgnoreCase));
                    };

                    switch (IconType)
                    {
                        case AssetIconType.Inactive:
                            _icon = getIcon("gray");
                            break;

                        case AssetIconType.OutOfService:
                            _icon = getIcon("yellow");
                            break;

                        case AssetIconType.OutOfServiceWithWorkOrder:
                            _icon = getIcon("yellowblack");
                            break;

                        case AssetIconType.RequiresInspectionWithWorkOrder:
                            _icon = getIcon("redblack");
                            break;

                        case AssetIconType.NormallyClosedButOpen:
                            _icon = getIcon("orangewhite");
                            break;

                        case AssetIconType.NormallyClosedButOpenWithWorkOrder:
                            _icon = getIcon("orangeblack");
                            break;

                        case AssetIconType.NormallyOpenButClosed:
                            _icon = getIcon("purplewhite");
                            break;

                        case AssetIconType.NormallyOpenButClosedWithWorkOrder:
                            _icon = getIcon("purpleblack");
                            break;

                        case AssetIconType.WorkOrder:
                            _icon = getIcon("greenblack");
                            break;

                        case AssetIconType.RequiresInspection:
                            _icon = getIcon("red");
                            break;

                        case AssetIconType.NonPublic:
                            _icon = getIcon("blue");
                            break;

                        case AssetIconType.Default:
                            _icon = getIcon("green");
                            break;

                        default:
                            throw new NotSupportedException(IconType.ToString());
                    }
                }

                return _icon;
            }
        }

        [SetterProperty]
        public virtual IIconSetRepository IconSetRepository
        {
            set => _iconSetRepository = value;
        }

        #endregion

        #region Abstract Properties

        public abstract AssetCoordinateType AssetType { get; }

        #endregion

        #region Constructors

        public AssetCoordinate(IIconSetRepository iconSetRepository)
        {
            _iconSetRepository = iconSetRepository;
        }

        #endregion
    }

    public class HydrantAssetCoordinate : AssetCoordinate
    {
        #region Properties

        public override AssetCoordinateType AssetType => AssetCoordinateType.Hydrant;

        public decimal? Stop { get; set; }

        #endregion

        public HydrantAssetCoordinate(IIconSetRepository iconSetRepository) : base(iconSetRepository) { }

        // needed so this can be passed through a weird NHibernate thing
        public HydrantAssetCoordinate() : this(null) { }
    }

    public class ValveAssetCoordinate : AssetCoordinate
    {
        #region Properties

        public override AssetCoordinateType AssetType => AssetCoordinateType.Valve;
        
        public decimal? Stop { get; set; }

        #endregion

        [DefaultConstructor]
        public ValveAssetCoordinate(IIconSetRepository iconSetRepository) : base(iconSetRepository) { }

        // needed so this can be passed through a weird NHibernate thing
        public ValveAssetCoordinate() : this(null) { }
    }

    public class BlowOffAssetCoordinate : AssetCoordinate
    {
        #region Properties

        public override AssetCoordinateType AssetType => AssetCoordinateType.BlowOff;

        #endregion

        [DefaultConstructor]
        public BlowOffAssetCoordinate(IIconSetRepository iconSetRepository) : base(iconSetRepository) { }

        // needed so this can be passed through a weird NHibernate thing
        public BlowOffAssetCoordinate() : this(null) { }
    }

    public class MainCrossingAssetCoordinate : AssetCoordinate
    {
        #region Properties

        public override AssetCoordinateType AssetType => AssetCoordinateType.MainCrossing;

        #endregion

        [DefaultConstructor]
        public MainCrossingAssetCoordinate(IIconSetRepository iconSetRepository) : base(iconSetRepository) { }

        // needed so this can be passed through a weird NHibernate thing
        public MainCrossingAssetCoordinate() : this(null) { }
    }

    public class BelowGroundHazardAssetCoordinate : AssetCoordinate
    {
        #region Properties

        public override AssetCoordinateType AssetType => AssetCoordinateType.BelowGroundHazard;

        #endregion

        [DefaultConstructor]
        public BelowGroundHazardAssetCoordinate(IIconSetRepository iconSetRepository) : base(iconSetRepository) { }

        // needed so this can be passed through a weird NHibernate thing
        public BelowGroundHazardAssetCoordinate() : this(null) { }
    }

    public class SewerOpeningAssetCoordinate : AssetCoordinate
    {
        #region Properties

        public override AssetCoordinateType AssetType => AssetCoordinateType.SewerOpening;

        #endregion

        [DefaultConstructor]
        public SewerOpeningAssetCoordinate(IIconSetRepository iconSetRepository) : base(iconSetRepository) { }

        // needed so this can be passed through a weird NHibernate thing
        public SewerOpeningAssetCoordinate() : this(null) { }
    }
}
