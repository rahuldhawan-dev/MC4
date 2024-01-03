using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.Results;
using StructureMap;

namespace MapCall.Common.Configuration
{
    /// <summary>
    /// ActionResult for use with AssetMapController.
    /// </summary>
    public class AssetMapResult : MapResult
    {
        #region Consts

        private struct Keys
        {
            #region Constants

            public const string RELATED_ASSETS_URL = "relatedAssetsUrl";

            #endregion
        }

        private enum LineLayerOptions
        {
            NoLine,
            ShowLineWithoutSorting,
            ShowLineSortedByLastInspection,
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets a url to retrieve coordinates for additional assets
        /// related to those being displayed here.
        /// </summary>
        public string RelatedAssetsUrl { get; set; }

        #endregion

        #region Constructors

        public AssetMapResult(IAuthenticationService<User> authenticationService, IIconSetRepository iconSetRepository)
            : base(authenticationService, iconSetRepository) { }

        #endregion

        #region Private Methods

        private static RouteValueDictionary GetRouteValueDictionaryForAssetCoordinateType(AssetCoordinateType act)
        {
            var rvd = new RouteValueDictionary {
                {"area", "FieldOperations"},
                {"action", "Show"},
                {ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME, ResponseFormatter.KnownExtensions.FRAGMENT}
            };

            switch (act)
            {
                case AssetCoordinateType.Hydrant:
                    rvd["controller"] = "Hydrant";
                    break;

                case AssetCoordinateType.Valve:
                case AssetCoordinateType.BlowOff:
                    rvd["controller"] = "Valve";
                    break;

                case AssetCoordinateType.MainCrossing:
                    rvd["controller"] = "MainCrossing";
                    rvd["area"] = "Facilities";
                    break;

                case AssetCoordinateType.BelowGroundHazard:
                    rvd["controller"] = "BelowGroundHazard";
                    rvd["area"] = "Facilities";
                    break;
                case AssetCoordinateType.SewerOpening:
                    rvd["controller"] = "SewerOpening";
                    break;

                default:
                    throw new NotSupportedException(act.ToString());
            }

            return rvd;
        }

        /// <summary>
        /// Takes a collection of AssetCoordinates and adds different CoordinateSets based on the AssetType and IconType found.
        /// The AssetCoordinates can be mixed types.
        /// </summary>
        private void Initialize(LineLayerOptions option, params IEnumerable<AssetCoordinate>[] coordCollection)
        {
            // Doing a ToList() to ensure all the weird lazy IEnumerables only get enumerated once.
            // Also didn't Linq this flattened list collection because Resharper kept giving multiple enumeration warnings
            // for reasons I don't understand.
            var coords = new List<AssetCoordinate>();
            foreach (var set in coordCollection)
            {
                coords.AddRange(set);
            }

            Action<IEnumerable<IThingWithCoordinate>> createLineLayer = (coordsForLineLayer) => {
                var lineLayer = CreateCoordinateSet(coordsForLineLayer.ToList(), "lineLayer");
                lineLayer.DrawLinesBetweenPoints = true;
            };

            // line layer must be the first/bottom-most layer so the lines don't get drawn on top of the icons.
            switch (option)
            {
                case LineLayerOptions.ShowLineWithoutSorting:
                    createLineLayer(coords);
                    break;

                case LineLayerOptions.ShowLineSortedByLastInspection:
                    createLineLayer(coords.OrderBy(x => x.LastInspection));
                    break;

                case LineLayerOptions.NoLine:
                    break;

                default:
                    throw new NotSupportedException(option.ToString());
            }

            var coordsByAsset = coords.GroupBy(x => x.AssetType);

            foreach (var cba in coordsByAsset)
            {
                var assetType = cba.Key.ToString();
                var rvd = GetRouteValueDictionaryForAssetCoordinateType(cba.Key);
                var coordsByIconType = cba.GroupBy(x => x.IconType);

                foreach (var set in coordsByIconType)
                {
                    var coordSet = CreateCoordinateSet(set, assetType + set.Key);
                    coordSet.CoordinateRouteValues = rvd;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Takes a collection of AssetCoordinates and adds different CoordinateSets based on the AssetType and IconType found.
        /// The AssetCoordinates can be mixed types.
        /// </summary>
        public void Initialize(params IEnumerable<AssetCoordinate>[] coords)
        {
            Initialize(LineLayerOptions.NoLine, coords);
        }

        /// <summary>
        /// Takes a collection of AssetCoordinates and adds different CoordinateSets based on the AssetType and IconType found.
        /// The AssetCoordinates can be mixed types. Includes a line layer that is displayed in the order the coordinates are given.
        /// </summary>
        public void InitializeWithLineLayer(params IEnumerable<AssetCoordinate>[] coords)
        {
            Initialize(LineLayerOptions.ShowLineWithoutSorting, coords);
        }

        /// <summary>
        /// Takes a collection of AssetCoordinates and adds different CoordinateSets based on the AssetType and IconType found.
        /// The AssetCoordinates can be mixed types. Includes a line layer that is sorted by the Last Inspection date.
        /// </summary>
        public void InitializeWithInspectionLineLayer(params IEnumerable<AssetCoordinate>[] coords)
        {
            Initialize(LineLayerOptions.ShowLineSortedByLastInspection, coords);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (!string.IsNullOrWhiteSpace(RelatedAssetsUrl))
            {
                _serializableValues[Keys.RELATED_ASSETS_URL] = RelatedAssetsUrl;
            }

            base.ExecuteResult(context);
        }

        #endregion
    }
}
