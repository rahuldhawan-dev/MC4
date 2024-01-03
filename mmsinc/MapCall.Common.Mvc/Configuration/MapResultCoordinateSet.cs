using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Results;
using StructureMap;

namespace MapCall.Common.Configuration
{
    /// <summary>
    /// Represents a set of coordinates for a MapResult instance. 
    /// </summary>
    public class MapResultCoordinateSet
    {
        #region Consts

        private struct JsonKeys
        {
            public const string COORDINATES = "coordinates",
                                ADDRESSES = "addresses",
                                DRAW_LINES = "drawLinesBetweenPoints",
                                LAYER_ID = "layerId";
        }

        #endregion

        #region Fields

        private readonly IIconSetRepository _iconSetRepository;
        private IEnumerable<IThingWithCoordinate> _coordinates;
        private IEnumerable<IThingWithoutCoordinates> _addresses;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the layer id the coordinates will be added to. If null, the default layer is used.
        /// </summary>
        /// <remarks>
        /// A CoordinateSet is NOT a layer on its own. Multiple coordinate sets can use the same layerId.
        /// </remarks>
        public string LayerId { get; set; }

        /// <summary>
        /// Gets/sets the collection of objects that have coordinates.
        /// </summary>
        public IEnumerable<IThingWithCoordinate> Coordinates
        {
            get { return _coordinates; }
            set
            {
                value.ThrowIfNull("coordinates");
                _coordinates = value;
            }
        }

        public IEnumerable<IThingWithoutCoordinates> Addresses
        {
            get { return _addresses; }
            set
            {
                value.ThrowIfNull("items");
                _addresses = value;
            }
        }

        /// <summary>
        /// Gets/sets the route values needed to link a coordinate marker
        /// back to a page to display in the maps info bubble. If null,
        /// the current request's route values will be used during execution.
        /// </summary>
        public RouteValueDictionary CoordinateRouteValues { get; set; }

        /// <summary>
        /// Gets/sets whether the map should create a layer for drawing lines
        /// between points. False by default.
        /// </summary>
        public bool DrawLinesBetweenPoints { get; set; }

        #endregion

        #region Constructor

        public MapResultCoordinateSet(IIconSetRepository iconSetRepository)
        {
            _iconSetRepository = iconSetRepository;
            Coordinates = Enumerable.Empty<IThingWithCoordinate>();
            Addresses = Enumerable.Empty<IThingWithoutCoordinates>();
        }

        #endregion

        #region Private methods

        private RouteValueDictionary GetCoordinateRouteValues(RouteValueDictionary currentRequestRouteValues)
        {
            if (CoordinateRouteValues != null)
            {
                // A custom RVD is set so we don't need to do any magic here.
                // It's up to the person who set it to know what they're doing.
                return CoordinateRouteValues;
            }

            // This lets us use the current request's route data if custom values
            // aren't set in the controller. We don't want to alter the current
            // request's route data though and have it mess up other things
            // that use it, so make a copy of it before doing any modifications.
            var copiedCurrentRequestRouteValues = new RouteValueDictionary(currentRequestRouteValues);
            copiedCurrentRequestRouteValues["action"] = "Show";
            copiedCurrentRequestRouteValues[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.FRAGMENT;

            return copiedCurrentRequestRouteValues;
        }

        private IEnumerable SerializeCoordinates(UrlHelper urlHelper)
        {
            var defaultIcon = new Lazy<MapIcon>(() => {
                return _iconSetRepository.Find(IconSets.SingleDefaultIcon).DefaultIcon;
            });
            var routeValues = GetCoordinateRouteValues(urlHelper.RequestContext.RouteData.Values);
            var serializedCoords = new List<object>();
            foreach (var c in Coordinates)
            {
                // Bug 2423: Coordinates with zero values need to be filtered out because it messes up the automatic zooming of the map.
                // Store coordinate locally cause the getter could have heavy logic behind it that doesn't need to be 
                // repeated 3 times.
                var actualCoordinate = c.Coordinate;
                if (actualCoordinate == null || actualCoordinate.Latitude == 0 || actualCoordinate.Longitude == 0)
                {
                    continue;
                }

                // There are 200k coordinates that do not have an Icon set. These can't be displayed.
                // Rather than not display anything, use the default icon
                var icon = c.Icon ?? defaultIcon.Value;
                if (icon == null)
                {
                    continue;
                }

                routeValues["id"] = c.Id;
                var url = DrawLinesBetweenPoints ? null : urlHelper.RouteUrl(routeValues);
                serializedCoords.Add(new {
                    dataId = c.Id,
                    lat = c.Coordinate.Latitude,
                    lng = c.Coordinate.Longitude,
                    iconId = icon.Id,
                    url
                });
            }

            return serializedCoords;
        }

        private IEnumerable SerializeAddresses(UrlHelper urlHelper)
        {
            var routeValues = GetCoordinateRouteValues(urlHelper.RequestContext.RouteData.Values);
            var serializedAddresses = new List<object>();
            foreach (var c in Addresses.Where(x => !String.IsNullOrWhiteSpace(x.Address)))
            {
                routeValues["id"] = c.Id;
                var url = urlHelper.RouteUrl(routeValues);
                serializedAddresses.Add(new {
                    dataId = c.Id,
                    address = c.Address,
                    url
                });
            }

            return serializedAddresses;
        }

        #endregion

        #region Public Methods

        public object Serialize(UrlHelper urlHelper)
        {
            var dict = new Dictionary<string, object>();
            dict.Add(JsonKeys.LAYER_ID, LayerId);
            dict.Add(JsonKeys.COORDINATES, SerializeCoordinates(urlHelper));
            dict.Add(JsonKeys.ADDRESSES, SerializeAddresses(urlHelper));
            dict.Add(JsonKeys.DRAW_LINES, DrawLinesBetweenPoints);
            return dict;
        }

        #endregion
    }
}
