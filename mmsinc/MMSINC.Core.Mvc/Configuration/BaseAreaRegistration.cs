﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MMSINC.Configuration
{
    public abstract class BaseAreaRegistration : AreaRegistration
    {
        #region Fields

        private readonly string _autoGeneratedAreaName;
        private readonly string _defaultNameSpace;

        #endregion

        #region Constructor

        protected BaseAreaRegistration()
        {
            var type = GetType();
            _autoGeneratedAreaName = type.Name.Replace("AreaRegistration", string.Empty);
            _defaultNameSpace = type.Namespace + ".Controllers";
        }

        #endregion

        #region Private Methods

        protected string[] GetControllerNamespace()
        {
            return new[] {_defaultNameSpace};
        }

        /// <summary>
        /// Gets the route name prefixed with the area name.
        /// </summary>
        /// <param name="routeNameSuffix"></param>
        /// <returns></returns>
        protected string GetRouteName(string routeNameSuffix)
        {
            return AreaName + "_" + routeNameSuffix;
        }

        private string GetDefaultControllerNameForArea()
        {
            return AreaName + "Home";
        }

        #endregion

        #region Public Methods

        public override string AreaName
        {
            get { return _autoGeneratedAreaName; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: GetRouteName("DefaultRouteForExtensions"),
                url: AreaName + "/{controller}/{action}/{id}.{ext}",
                defaults: new
                    {area = AreaName} // Can not have id param optional, conflicts with IndexRouteForExtensions
            );
            context.MapRoute(
                name: GetRouteName("IndexExportRoute"),
                url: AreaName + "/{controller}/{action}.{ext}",
                defaults: new {area = AreaName, action = "Index"});
            context.MapRoute(
                name: GetRouteName("default"),
                url: AreaName + "/{controller}/{action}/{id}",
                defaults: new {
                    area = AreaName, controller = GetDefaultControllerNameForArea(), action = "Index",
                    id = UrlParameter.Optional
                }
            );
        }

        #endregion
    }
}
