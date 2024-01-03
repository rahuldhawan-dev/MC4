using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using JetBrains.Annotations;
using MapCall.Common.ClassExtensions.MapExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallIntranet.Models.ViewModels;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.DictionaryExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Helpers;
using StructureMap;

namespace MapCallIntranet.ClassExtensions
{
    public static class HtmlHelperExtensions
    {
        #region Exposed Methods

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
        private readonly Dictionary<string, object> _extraIconAttributes = new Dictionary<string, object>();

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
                var args = !CoordinateId.HasValue
                               ? (object)new { controller = "Coordinate", action = "New", valueFor = Id, area = "", iconSet = (int)_iconSet }
                               : new { controller = "Coordinate", action = "New", id = CoordinateId, valueFor = Id, area = "", iconSet = (int)_iconSet };
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
                return !CoordinateId.HasValue
                           ? new Dictionary<string, object> { { "title", "This record has no coordinates set." } }
                           : base.ExtraIconAttributes;
            }
        }

        protected override string CoordinateUrl
        {
            get
            {
                return !CoordinateId.HasValue
                           ? null
                           : UrlHelper.RouteUrl(new { controller = "Coordinate", action = "Show", id = CoordinateId, area = "" });
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