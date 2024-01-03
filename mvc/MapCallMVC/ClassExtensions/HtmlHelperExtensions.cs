using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using JetBrains.Annotations;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.DictionaryExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Helpers;

namespace MapCallMVC.ClassExtensions
{
    public static class HtmlHelperExtensions
    {
        #region Exposed Methods

        #region DataTable

        // TODO: Due to time constraints, the DataTable stuff is being done via view + HtmlHelper extensions
        // instead of a proper ControlBuilder.

        /// <summary>
        /// Renders an mc-datatable element around the given razor table.
        /// </summary>
        /// <param name="html">The HtmlHelper instance</param>
        /// <param name="model">The model used to render this data table.</param>
        /// <returns></returns>
        public static IHtmlString DataTable(this HtmlHelper html, DataTableLayoutHtmlHelperViewModel model)
        {
            model.ModelType.ThrowIfNull("ModelType");
            model.RazorTable.ThrowIfNull("RazorTable");
            model.Controller = html.ViewContext.RequestContext.RouteData.GetRequiredString("controller");
            model.Area = html.ViewContext.RequestContext.RouteData.GetRequiredString("area");
          
            // TODO: As a ControlHelper method this would have proper access to a container instance.
            var layoutRepo = DependencyResolver.Current.GetService<IRepository<DataTableLayout>>();
            model.DataTableLayouts = layoutRepo.Where(x => x.Area == model.Area && x.Controller == model.Controller).ToList();

            return html.Partial("~/Views/Shared/DataTable/DataTable.cshtml", model);
        }
        
        #endregion

        #region ParentLink

        public static IHtmlString ParentLink(this HtmlHelper html, string path, string text = null, object htmlAttributes = null, bool wrapLinkTextInSpan = false)
        {
            path = String.Format("{0}../..{1}", html.UrlFor("~/"), path.StartsWith("/") ? path : "/" + path);

            return html.Link(path, text, htmlAttributes, wrapLinkTextInSpan, url => url);
        }

        #endregion

        #region EditorForEquipmentCharacteristicField

        public static IHtmlString EditorForEquipmentCharacteristicField(this HtmlHelper html, EquipmentCharacteristicField field, IList<EquipmentCharacteristic> existing)
        {
            var characteristic = existing.SingleOrDefault(c => c.Field.Id == field.Id);
            var value = characteristic == null ? String.Empty : characteristic.Value;
            IDictionary<string, object> htmlAttributes = new Dictionary<string, object> {
                {"data-val", "true"}
            };
            
            if (field.Required)
            {
                htmlAttributes["data-val-required"] = String.Format("The field {0} is required.", field.DisplayField);
            }

            switch (field.FieldType.Description)
            {
                case "Date":
                    htmlAttributes["class"] = "date";
                    htmlAttributes["data-val-date"] = String.Format("The field {0} must be a date.", field.DisplayField);
                    break;
                case "Currency":
                case "Number":
                    htmlAttributes["class"] = "numeric";
                    htmlAttributes["data-val-number"] = String.Format("The field {0} must be a number.", field.DisplayField);
                    break;
            }

            if (field.FieldType.Description != "DropDown")
            {
                return html.TextBox(field.FieldName, value, htmlAttributes);
            }

            if (field.Required)
            {
                htmlAttributes["data-val-number"] = String.Format("The field {0} is required.", field.DisplayField);
            }

            var options = field.Options.ToList();

            if (characteristic != null)
            {
                options.Each(o => o.Selected = false);
                options.Single(o => o.Value == characteristic.Value).Selected = true;
            }

            return html.DropDownListWithPrompt(field.FieldName, options, htmlAttributes);
        }

        #endregion

        #region Coordinate stuff

        public static IHtmlString CoordinatePicker(this HtmlHelper html, string expression,
            IconSets? iconSet = null)
        {
            return new CoordinatePickerBuilder(html, expression, iconSet);
        }

        public static IHtmlString CoordinatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IconSets? iconSet = null)
        {
            var expressionText = ExpressionHelper.GetExpressionText(expression);
            return CoordinatePicker(html, expressionText, iconSet);
        }

        public static IHtmlString CoordinateDisplay(this HtmlHelper html, string expression)
        {
            return new CoordinateDisplayBuilder(html, expression);
        }

        public static IHtmlString CoordinateDisplayFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var expressionText = ExpressionHelper.GetExpressionText(expression);
            return CoordinateDisplay(html, expressionText);
        }

        #endregion

        #region CurrentUserCanDoWithOperatingCenter

        // This is not a good solution to this problem. It's not really an "html helper".
        public static bool CurrentUserCanDoWithOperatingCenter(this HtmlHelper helper, object model, [AspMvcAction] string actionName, [AspMvcController] string controllerName, [AspMvcArea]string areaName = null)
        {
            model.ThrowIfNull("Can't check access without model.");
            var result = helper.CurrentUserCanDo(actionName, controllerName, areaName);

            // We only need to do any of this extra role checking if the user didn't fail
            // authorization already and if the model is IThingWithOperatingCenter. 
            if (result && model is IThingWithOperatingCenter)
            {
                var opc = ((IThingWithOperatingCenter)model).OperatingCenter;
                var roleServ = DependencyResolver.Current.GetService<IRoleService>();

                var requiredRoles = roleServ.GetRequiredRolesForRoute(helper.ViewContext.RequestContext, actionName, controllerName, areaName);

                foreach (var requiredRole in requiredRoles)
                {
                    if (!roleServ.CanAccessRole(requiredRole.Module, requiredRole.Action, opc))
                    {
                        return false;
                    }
                }
            }

            return result;
        }

        public static bool CurrentUserCanEditWithOperatingCenter(this HtmlHelper helper, object model)
        {
            return helper.CurrentUserCanDoWithOperatingCenter(model, "Edit", helper.ViewContext.RequestContext.RouteData.Values["controller"].ToString());
        }

        public static bool CurrentUserIsEmployee(this HtmlHelper helper, Employee employee)
        {
            return employee.User == DependencyResolver.Current.GetService<IAuthenticationService<User>>().CurrentUser;
        }

        public static Employee CurrentUserEmployee(this HtmlHelper helper)
        {
            return DependencyResolver.Current.GetService<IAuthenticationService<User>>().CurrentUser?.Employee;
        }

        #endregion

        #region UrlForAction

        public static string UrlForAction(this HtmlHelper html, [AspMvcAction] string actionName, [AspMvcController] string controllerName, RouteValueDictionary routeValues = null)
        {
            var urlHelper = new SecureUrlHelper(HttpContext.Current.Request.RequestContext);
            return routeValues == null ? urlHelper.Action(actionName, controllerName) : urlHelper.Action(actionName, controllerName, routeValues);
        }

        public static string UrlForAction(this HtmlHelper html, [AspMvcAction] string actionName, [AspMvcController] string controllerName, object routeValues)
        {
            return html.UrlForAction(actionName, controllerName, MMSINC.ClassExtensions.HtmlHelperExtensions.ConvertToRouteValueDictionary(routeValues));
        }

        #endregion

        #region AssetLink

        private static string GetAssetArea(string typeName)
        {
            switch (typeName)
            {
                case "MainCrossing":
                    return "Facilities";
                case "Valve":
                case "Hydrant":
                case "SewerOpening":
                case "Service":
                    return "FieldOperations";
                default:
                    return string.Empty;
            }
        }

        public static MvcHtmlString AssetLink(this HtmlHelper that, IAsset asset)
        {
            if (asset == null)
            {
                return MvcHtmlString.Empty;
            }
            var typeName = NHibernate.Proxy.NHibernateProxyHelper.GuessClass(asset).Name;
            return that.ActionLink(asset.Identifier, "Show",
                new {controller = typeName, area = GetAssetArea(typeName), id = ((IThingWithCoordinate)asset).Id});
        }

        #endregion

        #endregion
    }

    // TODO: Convert CoordinatePicker to a ControlBbuilder. Since it's MVC specific, ControlHelper will need extension methods.

    public abstract class CoordinateHelperBuilderBase : IHtmlString
    {
        #region Constants

        protected struct IconUrls
        {
            #region Constants

            public const string RED_GLOBE = "~/Content/images/map-icon-red.png",
                                BLUE_GLOBE = "~/Content/images/map-icon-blue.png";

            #endregion
        }

        #endregion

        #region Private Members

        protected readonly HtmlHelper _html;
        protected readonly string _expression;
        protected string _id, _name;
        protected ModelMetadata _modelMetadata;
        protected UrlHelper _urlHelper;
        protected readonly CoordinateAttribute _coordAttr;
        private readonly Dictionary<string, object> _extraIconAttributes = new Dictionary<string,object>();

        #endregion

        #region Properties

        protected ModelMetadata ModelMetadata
        {
            get
            {
                return _modelMetadata;
            }
        }

        public string Name
        {
            get { return _name ?? (_name = _html.Name(_expression).ToString()); }
        }

        public string Id
        {
            get { return _id ?? (_id = _html.Id(_expression).ToString()); }
        }

        protected int? CoordinateId
        {
            get
            {
                // Defaults to the int Id(or null)
                var model = ModelMetadata.Model;
                if (ModelMetadata.Model is Coordinate)
                {
                    return ((Coordinate)ModelMetadata.Model).Id;
                }

                return model as int?;
            }
        }

        protected HttpContextBase HttpContext
        {
            get { return _html.ViewContext.HttpContext; }
        }

        protected UrlHelper UrlHelper
        {
            get { return _urlHelper ?? (_urlHelper = new UrlHelper(_html.ViewContext.RequestContext)); }
        }

        public string IconUrl
        {
            get { return !CoordinateId.HasValue ? IconUrls.RED_GLOBE : IconUrls.BLUE_GLOBE; }
           // get { return string.IsNullOrWhiteSpace(CoordinateId) ? IconUrls.RED_GLOBE : IconUrls.BLUE_GLOBE; }
        }

        protected virtual IDictionary<string, object> ExtraIconAttributes
        {
            get { return _extraIconAttributes; }
        }

        #endregion

        #region Abstract Properties

        protected abstract string CoordinateUrl { get; }
        protected abstract string IconCssClass { get; }

        #endregion

        #region Constructors

        protected CoordinateHelperBuilderBase(HtmlHelper html, string expression)
        {
            _html = html;
            _expression = expression;
            _modelMetadata = ModelMetadata.FromStringExpression(expression, html.ViewData);
            _coordAttr = CoordinateAttribute.TryGetAttributeFromModelMetadata(_modelMetadata) ?? new CoordinateAttribute();
        }

        #endregion

        #region Private Methods

        protected string BuildIcon()
        {
            var icon = new TagBuilder("img");
            icon.MergeAttributes(new Dictionary<string, object> {
                {"src", UrlHelper.GenerateContentUrl(IconUrl, HttpContext)},
                {"coordinateUrl", CoordinateUrl}
            }.MergeAndReplace(ExtraIconAttributes));
            icon.AddCssClass(IconCssClass);

            if (_coordAttr.AddressField != null)
            {
                icon.MergeAttribute("data-address-field", _coordAttr.AddressField);
            }

            if (_coordAttr.AddressCallback != null)
            {
                icon.MergeAttribute("data-address-callback", _coordAttr.AddressCallback);
            }

            var middleStuff = BuildExtraMiddleStuff();

            var coordVals = new TagBuilder("span");
            coordVals.AddCssClass("cp-coordinate-values");

            if (CoordinateId.HasValue)
            {
                var creep = DependencyResolver.Current.GetService<IRepository<Coordinate>>().Find(CoordinateId.Value);
                if (creep != null)
                {
                    const string format = "{0}, {1}";
                    coordVals.SetInnerText(string.Format(format, creep.Latitude, creep.Longitude));
                }
            }

            return $"{icon.ToString(TagRenderMode.SelfClosing)}{middleStuff}{coordVals}";
        }

        protected virtual string BuildExtraMiddleStuff()
        {
            return string.Empty;
        }

        #endregion

        #region Abstract Methods

        public abstract string ToHtmlString();

        #endregion

        #region Exposed Methods

        #region Public Methods

        public override string ToString()
        {
            return ToHtmlString();
        }

        #endregion

        #endregion
    }

    public class CoordinatePickerBuilder : CoordinateHelperBuilderBase
    {
        #region Constants

        public const string ICON_CSS_CLASS = "coordinate-picker-icon";

        #endregion

        #region Private Members

        protected readonly IconSets _iconSet;

        #endregion

        #region Properties

        protected override string CoordinateUrl
        {
            get
            {
                var args = !CoordinateId.HasValue ? (object) new { controller = "Coordinate", action = "New", valueFor = Id, area = "", iconSet = (int)_iconSet } : new { controller = "Coordinate", action = "New", id = CoordinateId, valueFor = Id, area = "", iconSet = (int)_iconSet }; 
                return UrlHelper.RouteUrl(args);
            }
        }

        protected override string IconCssClass => ICON_CSS_CLASS;

        #endregion

        #region Constructors

        public CoordinatePickerBuilder(HtmlHelper html, string expression, IconSets? iconSet)
            : base(html, expression)
        {
            _iconSet = (iconSet != null ? iconSet.Value : _coordAttr.IconSet);
        }

        #endregion

        #region Private Methods

        protected override string BuildExtraMiddleStuff()
        {
            var button = new TagBuilder("button");
            button.MergeAttributes(new Dictionary<string, object> {
                {"id", "coordinateManualEntryButton"},
                {"coordinateUrl", $"{CoordinateUrl}&manual=true"},
                {"type", "button"}
            });
            button.InnerHtml = "Advanced";
            return button.ToString();
        }

        #endregion

        #region Exposed Methods

        public override string ToHtmlString()
        {
            // For some reason, _html.Hidden("") does not return the model value.
            return BuildIcon() + _html.Hidden(_expression, _html.Value(_expression));
        }

        #endregion
    }
    
    public class CoordinateDisplayBuilder : CoordinateHelperBuilderBase
    {
        #region Constants

        public const string ICON_CSS_CLASS = "coordinate-display-icon";

        #endregion

        #region Properties

        protected override IDictionary<string, object> ExtraIconAttributes
        {
            get
            {
                return !CoordinateId.HasValue ? new Dictionary<string, object> { { "title", "This record has no coordinates set." } } : base.ExtraIconAttributes;
            }
        }

        protected override string CoordinateUrl
        {
            get
            {
                return !CoordinateId.HasValue ? null : UrlHelper.RouteUrl(new { controller = "Coordinate", action = "Show", id = CoordinateId, area = "" });
            }
        }

        protected override string IconCssClass
        {
            get { return ICON_CSS_CLASS; }
        }

        #endregion

        #region Constructors

        public CoordinateDisplayBuilder(HtmlHelper html, string expression) : base(html, expression) { }

        #endregion

        #region Exposed Methods

        public override string ToHtmlString()
        {
            return BuildIcon();
        }

        #endregion
    }
}