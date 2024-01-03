using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCall.Common.Configuration
{
    /// <summary>
    /// ActionResult for returning Coordinates for use on maps.
    /// </summary>
    public class MapResult : JsonResult
    {
        #region Constants

        private struct Keys
        {
            public const string COORDINATE_SETS = "coordinateSets",
                                CENTER = "center",
                                MODELSTATE_IS_VALID = "modelStateIsValid",
                                MODELSTATE_ERRORS = "modelStateErrors";
        }

        #endregion

        #region Fields

        // I opted to use a dictionary rather than a second model in order to make creating a new
        // MapResult instance involve a little less typing(so it can use the object initializer).
        protected readonly Dictionary<string, object> _serializableValues;
        private readonly IAuthenticationService<User> _authenticationService;
        private readonly IIconSetRepository _iconSetRepository;

        #endregion

        #region Properties

        public new object Data
        {
            get => throw new NotSupportedException("Don't touch this. Use the other properties available.");
            set => throw new NotSupportedException("Don't touch this. Use the other properties available.");
        }

        public decimal CenterLatitude { get; set; }
        public decimal CenterLongitude { get; set; }

        /// <summary>
        /// Gets the list of MapResultCoordinateSet instances that will be serialized.
        /// </summary>
        public List<MapResultCoordinateSet> CoordinateSets { get; }

        /// <summary>
        /// Gets/sets whether the controller that returned this instance
        /// had a valid model state. Set this to false so the maps page
        /// can display the proper error message.
        /// </summary>
        [Obsolete("This is going to be removed because it can be done automatically.")]
        public bool ModelStateIsValid
        {
            get => (bool)_serializableValues[Keys.MODELSTATE_IS_VALID];
            private set => _serializableValues[Keys.MODELSTATE_IS_VALID] = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MapResult instance.
        /// </summary>
        public MapResult(
            IAuthenticationService<User> authenticationService,
            IIconSetRepository iconSetRepository)
        {
            _authenticationService = authenticationService.ThrowIfNull(nameof(authenticationService));
            _iconSetRepository = iconSetRepository.ThrowIfNull(nameof(iconSetRepository));
            CoordinateSets = new List<MapResultCoordinateSet>();
            _serializableValues = new Dictionary<string, object>();
            // This is almost always gonna be required because 
            // this will be returned from Index/HttpGet actions.
            JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            ModelStateIsValid = true;

            // Default value is 2097152 which ends up being 4MB. Some
            // results just have a ton of coordinates so let's go all 
            // the way up to 40MB and hope it never hits that point.
            MaxJsonLength = 20971520;
            // ReSharper disable RedundantBaseQualifier
            base.Data = _serializableValues;
            // ReSharper restore RedundantBaseQualifier
        }

        #endregion

        #region Private Methods

        private object GetCenterCoordinate()
        {
            // moved default coordinate to the operating center, want to populate based on the current
            // user's default operating center
            var currentUser = _authenticationService?.CurrentUser;
            if (currentUser?.DefaultOperatingCenter?.Coordinate != null)
            {
                return new {
                    currentUser.DefaultOperatingCenter.Coordinate.Latitude,
                    currentUser.DefaultOperatingCenter.Coordinate.Longitude
                };
            }

            var lat = (CenterLatitude != 0
                ? CenterLatitude
                : Convert.ToDecimal(ConfigurationManager.AppSettings["DefaultMapCenterLatitude"]));
            var lng = (CenterLongitude != 0
                ? CenterLongitude
                : Convert.ToDecimal(ConfigurationManager.AppSettings["DefaultMapCenterLongitude"]));
            return new {
                lat,
                lng
            };
        }

        private static Dictionary<string, string> GetModelStateErrors(ModelStateDictionary modelState)
        {
            return modelState
                  .Where(x => x.Value.Errors.Any())
                  .ToDictionary(
                       x => x.Key,
                       x => string.Join(" ", x.Value.Errors.Select(e => e.ErrorMessage)));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new coordinate set and adds it to the CoordinateSets collection.
        /// </summary>
        public MapResultCoordinateSet CreateCoordinateSet(
            IEnumerable<IThingWithCoordinate> coords,
            string layerId = null)
        {
            var set = new MapResultCoordinateSetWithCoordinates(_iconSetRepository, coords) {
                LayerId = layerId
            };
            CoordinateSets.Add(set);
            return set;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var controller = (Controller)context.Controller;
            var urlHelper = new UrlHelper(context.RequestContext);
            var sets = CoordinateSets.Select(x => x.Serialize(urlHelper));

            _serializableValues[Keys.CENTER] = GetCenterCoordinate();
            _serializableValues[Keys.COORDINATE_SETS] = sets;

            _serializableValues[Keys.MODELSTATE_IS_VALID] = controller.ModelState.IsValid;
            _serializableValues[Keys.MODELSTATE_ERRORS] = GetModelStateErrors(controller.ModelState);

            base.ExecuteResult(context);
        }

        #endregion
    }
}
