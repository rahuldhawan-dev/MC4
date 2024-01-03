using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using JetBrains.Annotations;
using MMSINC.ClassExtensions;
using RouteCollectionExtensions = MMSINC.ClassExtensions.RouteCollectionExtensions;

namespace MMSINC.Helpers
{
    /// <summary>
    /// Encapsulates all the fun ways to create forms from a view without
    /// making 900 HtmlHelper extension methods that have slightly different
    /// names from the built in methods.
    /// </summary>
    public class FormHelper<T>
    {
        #region Fields

        private readonly HtmlHelper<T> _htmlHelper;

        #endregion

        #region Properties

        #endregion

        #region Constructor

        public FormHelper(HtmlHelper<T> htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        #endregion

        #region Private Methods

        private FormBuilder<T> BuildAFormBuilder(string action, string controller, string area, object routeData)
        {
            var fb = new FormBuilder<T>(_htmlHelper);
            fb.Action = action;
            fb.Controller = controller;
            fb.Area = area;

            if (routeData != null)
            {
                var routeVals = HtmlHelperExtensions.ConvertToRouteValueDictionary(routeData);
                fb.MergeRouteValues(routeVals);
            }

            return fb;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a FormBuilder with the action, controller, and routeData set.
        /// </summary>
        public FormBuilder<T> BeginForm([AspMvcAction] string action, [AspMvcController] string controller,
            object routeData = null)
        {
            return BeginForm(action, controller, null, routeData);
        }

        /// <summary>
        /// Returns a FormBuilder with the action, controller, area, and routeData set.
        /// </summary>
        public FormBuilder<T> BeginForm([AspMvcAction] string action, [AspMvcController] string controller,
            [AspMvcArea] string area, object routeData = null)
        {
            return BuildAFormBuilder(action, controller, area, routeData);
        }

        /// <summary>
        /// Returns a FormBuilder with the action, controller, and routeData set. Also it's an ajax form.
        /// </summary>
        public FormBuilder<T> BeginAjaxForm([AspMvcAction] string action, [AspMvcController] string controller,
            object routeData = null)
        {
            return BeginAjaxForm(action, controller, null, routeData);
        }

        /// <summary>
        /// Returns a FormBuilder with the action, controller, area, and routeData set. Also it's an ajax form.
        /// </summary>
        public FormBuilder<T> BeginAjaxForm([AspMvcAction] string action, [AspMvcController] string controller,
            [AspMvcArea] string area, object routeData = null)
        {
            var form = BeginForm(action, controller, area, routeData);
            form.Ajax = new AjaxOptions();
            return form;
        }

        /// <summary>
        /// Returns a FormBuilder for a specific route name.
        /// </summary>
        /// <param name="routeName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public FormBuilder<T> BeginRouteForm(string routeName, object routeValues)
        {
            var form = BeginForm(null, null, routeValues);
            form.RouteName = routeName;
            return form;
        }

        /// <summary>
        /// Returns a FormBuilder for a many to many route.
        /// </summary>
        /// <param name="parentModel"></param>
        /// <param name="childModel"></param>
        /// <param name="routeValues">include both ids. </param>
        /// <param name="action">true for Add/Create, false for Remove/Destroy</param>
        /// <returns></returns>
        public FormBuilder<T> BeginRouteForm(string parentModel, string childModel, object routeValues,
            ManyToManyRouteAction action)
        {
            var routeName = RouteCollectionExtensions.GetManyToManyRouteName(action, parentModel, childModel);
            return BeginRouteForm(routeName, routeValues);
        }

        /// <summary>
        /// Returns a FormBuilder for a specific route name. Also it's an ajax form.
        /// </summary>
        /// <param name="routeName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public FormBuilder<T> BeginAjaxRouteForm(string routeName, object routeValues)
        {
            var form = BeginRouteForm(routeName, routeValues);
            form.Ajax = new AjaxOptions();
            return form;
        }

        /// <summary>
        /// Returns a FormBuilder for a many to many route. Also it's an ajax form.
        /// </summary>
        /// <param name="parentModel"></param>
        /// <param name="childModel"></param>
        /// <param name="routeValues">include both ids. </param>
        /// <param name="action">true for Add/Create, false for Remove/Destroy</param>
        /// <returns></returns>
        public FormBuilder<T> BeginAjaxRouteForm(string parentModel, string childModel, object routeValues,
            ManyToManyRouteAction action)
        {
            var routeName = RouteCollectionExtensions.GetManyToManyRouteName(action, parentModel, childModel);
            return BeginAjaxRouteForm(routeName, routeValues);
        }

        #endregion
    }
}
