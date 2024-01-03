using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MMSINC.ClassExtensions;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.Common.Model.ViewModels
{
    public class MapView
    {
        #region Consts

        private const string DEFAULT_ACTION = "Index";

        #endregion

        #region Fields

        private string _action;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the name of the Area the Controller belongs to. 
        /// Set to String.Empty if the Controller does not belong to an Area.
        /// </summary>
        /// <remarks>
        /// NOTE: This can't be named "Area" because the ModelBinder will flat out 
        /// ignore the value since it's used in routing.
        /// </remarks>
        public string AreaName { get; set; }

        /// <summary>
        /// Gets/sets the name of the Controller the map will get its results from.
        /// </summary>
        /// <remarks>
        /// NOTE: This can't be named "Controller" because the ModelBinder will flat out 
        /// ignore the value since it's used in routing.
        /// </remarks>
        [Required]
        public string ControllerName { get; set; }

        /// <summary>
        /// Gets/sets the controller action the map will get its results from.
        /// This defaults to Index if the value is not set.
        /// </summary>
        /// <remarks>
        /// NOTE: This can't be named "Action" because the ModelBinder will flat out 
        /// ignore the value since it's used in routing.
        /// </remarks>
        public string ActionName
        {
            get { return (!string.IsNullOrWhiteSpace(_action) ? _action : DEFAULT_ACTION); }
            set { _action = value; }
        }

        /// <summary>
        /// Gets/sets the serialized form of the search model that needs to be passed
        /// back to the controller action for mapping. 
        /// </summary>
        /// <remarks>
        /// This would be nicer if it were a RouteValueDictionary, but the default
        /// model binder doesn't bind it correctly. It converts all string values
        /// to string arrays instead.
        /// </remarks>
        public Dictionary<string, string> Search { get; set; }

        /// <summary>
        /// The map configuration for the view. This is set by the controller and not by postback.
        /// </summary>
        public JsonMapConfiguration MapConfiguration { get; set; }

        public string[] DefaultLayers { get; set; }

        #endregion

        #region Constructors

        [DefaultConstructor]
        public MapView()
        {
            Search = new Dictionary<string, string>();
        }

        /// <summary>
        /// Creates a new MapView and takes the given ModelStateDictionary
        /// and uses it as SearchCriteria.  
        /// </summary>
        /// <param name="searchModelState"></param>
        public MapView(ModelStateDictionary searchModelState)
        {
            Search = searchModelState.ToRouteValueDictionary().ToDictionary(x => x.Key, x => Convert.ToString(x.Value));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts this instance to a proper RouteValueDictionary
        /// that can be used in url generation.
        /// </summary>
        /// <returns></returns>
        public RouteValueDictionary ToRouteValueDictionary()
        {
            // Refactor-proofing this in case the properties get renamed.
            var areaKey = Expressions.GetMember<MapView, string>(x => x.AreaName).Name;
            var controllerKey = Expressions.GetMember<MapView, string>(x => x.ControllerName).Name;
            var actionKey = Expressions.GetMember<MapView, string>(x => x.ActionName).Name;
            var searchKeyRoot = Expressions.GetMember<MapView, Dictionary<string, string>>(x => x.Search).Name;

            var rvd = new RouteValueDictionary();
            if (!string.IsNullOrWhiteSpace(AreaName))
            {
                rvd.Add(areaKey, AreaName);
            }

            if (!string.IsNullOrWhiteSpace(ControllerName))
            {
                rvd.Add(controllerKey, ControllerName);
            }

            if (ActionName != DEFAULT_ACTION)
            {
                rvd.Add(actionKey, ActionName);
            }

            if (Search != null)
            {
                foreach (var kv in Search)
                {
                    // NOTE: This is being turned into almost-dictionary format. The model binder
                    // does not want quotation marks around the keys. It will end up adding the
                    // quotes as part of the key and make things a mess.
                    const string format = "{0}[{1}]";
                    rvd.Add(string.Format(format, searchKeyRoot, kv.Key), kv.Value);
                }
            }

            return rvd;
        }

        /// <summary>
        /// Copies the current Search value as-is to a new RouteValueDictionary.
        /// If Search is null, this returns an empty RouteValueDictionary.
        /// </summary>
        /// <returns></returns>
        public RouteValueDictionary CopySearchToRouteValueDictionary()
        {
            var rvd = new RouteValueDictionary();
            if (Search != null)
            {
                foreach (var kv in Search)
                {
                    rvd.Add(kv.Key, kv.Value);
                }
            }

            return rvd;
        }

        #endregion
    }

    public class JsonMapConfiguration
    {
        #region Properties

        public string dataUrl { get; set; }
        public IEnumerable<JsonMapIcon> icons { get; set; }
        public IEnumerable<string> validationErrors { get; set; }
        public Dictionary<string, object> additionalData { get; private set; }
        public string[] defaultLayers { get; set; }

        #endregion

        #region Constructor

        public JsonMapConfiguration()
        {
            additionalData = new Dictionary<string, object>();
        }

        #endregion
    }

    public class JsonMapIcon
    {
        public int id { get; set; }
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string offset { get; set; }
    }
}
