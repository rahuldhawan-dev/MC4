using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.WebPages;
using JetBrains.Annotations;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.DictionaryExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MMSINC.Helpers
{
    /// <summary>
    /// Class for constructing html to use with jQuery UI's Tabs. 
    /// </summary>
    public class TabBuilder : IHtmlString
    {
        #region Constants

        // The css classes needed for the jquery tabs to look correct before the script actually creates the tabs.
        // Should the day come that this needs to be configurable, it should be easy enough to swap these consts
        // out with a TabBuilderProperties object or something.
        private const string TAB_LINK_CSS = "ui-state-default ui-corner-top",
                             // tabs-container is needed for ajaxtabs.js to work.
                             DIV_WRAPPER_CSS = "tabs-container ui-tabs ui-widget ui-widget-content ui-corner-all",

                             // tab-content is required for ajaxtabs.js to work.
                             TAB_BODY_CSS = "tab-content ui-tabs-panel ui-widget-content ui-corner-bottom",
                             UL_CSS = "ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all",
                             AJAX_TAB_ATTRIBUTE = "data-ajax-tab",
                             AJAX_UPDATE_TARGET_ID = "data-ajax-update-target-id";

        #endregion

        #region Fields

        /// <summary>
        /// Dictionary of Tabs by their Id.
        /// </summary>
        /// <remarks>
        /// 
        /// Tabs must have unique ids. This is enforced by the TabBuilder instance, but
        /// could still potentially blow up if more than one TabBuilder on a page is used.
        /// 
        /// </remarks>
        private readonly Dictionary<string, Tab> _tabsById = new Dictionary<string, Tab>();

        #endregion

        #region Properties

        public object HtmlAttributes { get; set; }

        // TODO: Why is this a property, Ross? Why does it have a public setter, Ross? -Ross 10/9/2015
        /// <summary>
        /// Gets/sets the HtmlHelper used for rendering partial views.
        /// </summary>
        public HtmlHelper HtmlHelper { get; set; }

        #endregion

        #region Constructor

        public TabBuilder(HtmlHelper helper)
        {
            HtmlHelper = helper;
        }

        #endregion

        #region Private Methods

        #region For unit testing only

#if DEBUG

        /// <summary>
        /// Returns the Tab object for a given id. THIS IS FOR UNIT TESTING ONLY!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tab GetTabById(string id)
        {
            return _tabsById[id];
        }

#endif

        #endregion

        #region Rendering

        /// <summary>
        /// Adds additional css classes without overwriting existing merged attributes.
        /// NOTE: TagBuilder renders css class names in alphabetical order. 
        /// </summary>
        /// <param name="tagBuilder"></param>
        /// <param name="cssClasses">One of the CSS const strings.</param>
        private static void ApplyCssClasses(TagBuilder tagBuilder, string cssClasses)
        {
            tagBuilder.AddCssClass(cssClasses);
        }

        private string CreateTabHtml(Tab tab)
        {
            if (!tab.IsVisible)
            {
                return null;
            }

            var anchor = new TagBuilder("a");
            anchor.MergeAttribute("href", "#" + tab.TabId);

            var tabText = tab.TabText;
            if (tab.TabItemCount.HasValue)
            {
                tabText = string.Format("{0} ({1})", tab.TabText, tab.TabItemCount);
            }

            anchor.SetInnerText(tabText);

            var li = CreateTag("li", tab.TabHtmlAttributes, TAB_LINK_CSS);
            li.InnerHtml = anchor.ToString();
            // This needs to be the text without the TabItemCount attached to it.
            // This is only used for regression testing at the moment.
            li.MergeAttribute("data-tab-text", tab.TabText);

            return li.ToString();
        }

        private string CreateTabList()
        {
            var ul = CreateTag("ul", null, UL_CSS);

            var sb = new StringBuilder();
            foreach (var tab in _tabsById)
            {
                sb.AppendLine(CreateTabHtml(tab.Value));
            }

            ul.InnerHtml = sb.ToString();
            return ul.ToString();
        }

        private string CreateTabBody(Tab tab, HtmlHelper helper, bool isSelected)
        {
            if (!tab.IsVisible)
            {
                return null;
            }

            var div = CreateTag("div", tab.BodyHtmlAttributes, TAB_BODY_CSS);
            div.MergeAttribute("id", tab.TabId);

            if (tab.IsAjaxTab)
            {
                div.MergeAttribute(AJAX_TAB_ATTRIBUTE, "true");
                if (!string.IsNullOrWhiteSpace(tab.AjaxUpdateTargetId))
                {
                    div.MergeAttribute(AJAX_UPDATE_TARGET_ID, tab.AjaxUpdateTargetId);
                }
            }

            if (!isSelected)
            {
                div.MergeAttribute("style", "display:none;");
            }

            if (tab.HelperResult != null)
            {
                var helperResult = HtmlHelperExtensions.WrapHelperResult(helper, tab.HelperResult);
                div.InnerHtml = helperResult(null).ToString();
            }
            else
            {
                object model;
                ViewDataDictionary vdd;
                if (tab.UseParentViewModel)
                {
                    // Need to use the Partial overload that takes both a model and ViewDataDictionary.
                    // The helper method deals with creating a new ViewDataDictionary that includes
                    // the existing VDD's keys/templateinfo/viewstate and what not so we don't have 
                    // to worry about it.
                    model = helper.ViewData.Model;
                    vdd = helper.ViewData;
                }
                else
                {
                    // NOTE: This does not work when passing a NULL model to a STRONGLY TYPED
                    // view. It'll complain about model type mismatch in the ViewDataDictionary. This 
                    // happens because MVC, by default, decides to pass the parent view's model to the
                    // partial if a null model is passed to the partial helper. 
                    // To get around this, this is creating a new ViewDataDictionary(and copying all the 
                    // values) and then setting the Model to null. 

                    model = tab.PartialModel;
                    vdd = helper.ViewData;
                    if (model == null)
                    {
                        vdd = new ViewDataDictionary(vdd) {Model = null};
                    }
                }

                if (tab.ExtraData != null)
                {
                    vdd.MergeAndReplace(tab.ExtraData);
                }

                div.InnerHtml = helper.Partial(tab.PartialName, model, vdd).ToString();
            }

            return div.ToString();
        }

        private Tab GetSelectedTab()
        {
            // Basically, if no tab has IsSelected = true then the first tab will be considered selected.
            return _tabsById.Values.FirstOrDefault(x => x.IsSelected) ?? _tabsById.Values.FirstOrDefault();
        }

        private string Render(HtmlHelper helper)
        {
            helper.ThrowIfNull("Unable to render TabBuilder when HtmlHelper object is null");
            var wrapper = CreateTag("div", HtmlAttributes, DIV_WRAPPER_CSS);
            var sb = new StringBuilder();
            sb.Append(CreateTabList());

            var selectedTab = GetSelectedTab();

            foreach (var tab in _tabsById.Values)
            {
                var isSelected = (tab == selectedTab);
                sb.Append(CreateTabBody(tab, helper, isSelected));
            }

            wrapper.InnerHtml = sb.ToString();
            return wrapper.ToString();
        }

        private static TagBuilder CreateTag(string tagName, object anonymousHtmlAttributes = null,
            string cssClasses = null)
        {
            var tb = new TagBuilder(tagName);
            if (anonymousHtmlAttributes != null)
            {
                // Use the HtmlHelper method because it properly converts props_like_this to props-like-this.
                tb.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(anonymousHtmlAttributes));
            }

            // Css applied after merging as the class attribute could get messed up.
            if (!string.IsNullOrWhiteSpace(cssClasses))
            {
                ApplyCssClasses(tb, cssClasses);
            }

            return tb;
        }

        #endregion

        private static string ChangeNameToId(string name)
        {
            // Adding "Tab" to the id is to prevent accidentally having different elements with the same id.
            // ex: The allocation permits page has an "Equipment" tab with an "Equipment" dropdown inside of it.
            return name.Replace(" ", string.Empty) + "Tab";
        }

        private Tab CreateAndAddTab(string name, string id, bool isVisible, bool isSelected, object bodyHtmlAttributes,
            object tabHtmlAttributes, bool isAjaxTab, int? itemCount, string updateTargetId)
        {
            var tab = new Tab {
                BodyHtmlAttributes = bodyHtmlAttributes,
                IsSelected = isSelected,
                IsVisible = isVisible,
                TabText = name,
                TabId = id,
                TabHtmlAttributes = tabHtmlAttributes,
                IsAjaxTab = isAjaxTab,
                TabItemCount = itemCount,
                AjaxUpdateTargetId = updateTargetId
            };

            if (_tabsById.ContainsKey(id))
            {
                throw new InvalidOperationException($"A tab has already been added with the id \"{id}\".");
            }

            _tabsById.Add(id, tab);
            return tab;
        }

        private TabBuilder WithPartialViewTab(string name, string id, string partialName, object partialModel,
            bool isVisible, bool isSelected, bool useParentViewModel,
            object bodyHtmlAttributes, object tabHtmlAttributes, bool isAjaxTab,
            ViewDataDictionary extraData, int? itemCount, string updateTargetId)
        {
            var tab = CreateAndAddTab(name, id, isVisible, isSelected, bodyHtmlAttributes, tabHtmlAttributes, isAjaxTab,
                itemCount, updateTargetId);
            tab.UseParentViewModel = useParentViewModel;
            tab.PartialName = partialName;
            tab.PartialModel = partialModel;
            tab.IsAjaxTab = isAjaxTab;
            tab.ExtraData = extraData;
            return this;
        }

        private TabBuilder WithHelperResultTab(string name, string id, Func<object, HelperResult> helperResult,
            bool isVisible, bool isSelected, object bodyHtmlAttributes,
            object tabHtmlAttributes, bool isAjaxTab, int? itemCount,
            string updateTargetId)
        {
            var tab = CreateAndAddTab(name, id, isVisible, isSelected, bodyHtmlAttributes, tabHtmlAttributes, isAjaxTab,
                itemCount, updateTargetId);
            tab.UseParentViewModel = true; // Because there's not gonna be a partial view with separate model.
            tab.HelperResult = helperResult;
            return this;
        }

        #endregion

        #region Exposed Methods

        #region WithTab with HelperResult function.

        /// <summary>
        /// Creates a new Tab and uses the helperResult to render the body html.
        /// 
        /// Because of the number of optional parameters, it's highly recommended you 
        /// use the named parameters when calling the method instead of relying on
        /// parameter order.
        /// </summary>
        /// <param name="name">Text used for the tab title. An id value will be generated from this name.</param>
        /// <param name="helperResult">Function that returns the html needed for this tab's body.</param>
        /// <param name="isVisible">If false, the helper result will not be executed and the tab will not be visible during rendering.</param>
        /// <param name="isSelected">If true, this tab will be displayed by default.</param>
        /// <param name="bodyHtmlAttributes">Additional html attributes used for the div that wraps the body of this specific tab.</param>
        /// <param name="tabHtmlAttributes">Additional html attributes used for the tab itself.</param>
        /// <param name="itemCount">Optional number appended to the tab text but not the #hash fragment.</param>
        /// <returns></returns>
        public TabBuilder WithTab(string name, Func<object, HelperResult> helperResult, bool isVisible = true,
            bool isSelected = false, object bodyHtmlAttributes = null, object tabHtmlAttributes = null,
            int? itemCount = null)
        {
            // ReSharper disable RedundantArgumentName
            return WithTab(name,
                ChangeNameToId(name),
                helperResult,
                isVisible: isVisible,
                isSelected: isSelected,
                bodyHtmlAttributes: bodyHtmlAttributes,
                tabHtmlAttributes: tabHtmlAttributes,
                itemCount: itemCount);
            // ReSharper restore RedundantArgumentName
        }

        /// <summary>
        /// Creates a new Tab and uses the helperResult to render the body html. 
        /// 
        /// Because of the number of optional parameters, it's highly recommended you 
        /// use the named parameters when calling the method instead of relying on
        /// parameter order.
        /// </summary>
        /// <param name="name">Text used for the tab title.</param>
        /// <param name="id">The id to be used for the tab and tab anchor.</param>
        /// <param name="helperResult">Function that returns the html needed for this tab's body.</param>
        /// <param name="isVisible">If false, the helper result will not be executed and the tab will not be visible during rendering.</param>
        /// <param name="isSelected">If true, this tab will be displayed by default.</param>
        /// <param name="bodyHtmlAttributes">Additional html attributes used for the div that wraps the body of this specific tab.</param>
        /// <param name="tabHtmlAttributes">Additional html attributes used for the tab itself.</param>
        /// <param name="itemCount">Optional number appended to the tab text but not the #hash fragment.</param>
        /// <returns></returns>
        public TabBuilder WithTab(string name, string id, Func<object, HelperResult> helperResult,
            bool isVisible = true, bool isSelected = false, object bodyHtmlAttributes = null,
            object tabHtmlAttributes = null, int? itemCount = null)
        {
            return WithHelperResultTab(name, id, helperResult, isVisible, isSelected, bodyHtmlAttributes,
                tabHtmlAttributes, isAjaxTab: false, itemCount: itemCount, updateTargetId: null);
        }

        #endregion

        #region WithTab with Partial Views

        /// <summary>
        /// Creates a new Tab and uses a partial view to render the body html. Model is inherited from the parent view.
        /// 
        /// Because of the number of optional parameters, it's highly recommended you 
        /// use the named parameters when calling the method instead of relying on
        /// parameter order.
        /// </summary>
        /// <param name="name">Text used for the tab title. An id value will be generated from this name.</param>
        /// <param name="partialName">Name of the partial view that should be used to render the body of this tab.</param>
        /// <param name="isVisible">If false, the partial view will not be executed and the tab will not be visible during rendering.</param>
        /// <param name="isSelected">If true, this tab will be displayed by default.</param>
        /// <param name="bodyHtmlAttributes">Additional html attributes used for the div that wraps the body of this specific tab.</param>
        /// <param name="tabHtmlAttributes">Additional html attributes used for the tab itself.</param>
        /// <param name="alterModelFn">Callback that will be passed the viewModel before it's sent off to the partial for rendering.</param>
        /// <param name="itemCount">Optional number appended to the tab text but not the #hash fragment.</param>
        /// <returns></returns>
        public TabBuilder WithTab(string name, [AspMvcView] string partialName, bool isVisible = true,
            bool isSelected = false, object bodyHtmlAttributes = null, object tabHtmlAttributes = null,
            ViewDataDictionary extraData = null, int? itemCount = null)
        {
            // ReSharper disable RedundantArgumentName
            return WithPartialViewTab(name,
                ChangeNameToId(name),
                partialName,
                null,
                isVisible: isVisible,
                isSelected: isSelected,
                useParentViewModel: true,
                bodyHtmlAttributes: bodyHtmlAttributes,
                tabHtmlAttributes: tabHtmlAttributes,
                isAjaxTab: false,
                extraData: extraData,
                itemCount: itemCount,
                updateTargetId: null);
            // ReSharper restore RedundantArgumentName
        }

        /// <summary>
        /// Creates a new Tab and uses a partial view to render the body html.
        /// 
        /// Because of the number of optional parameters, it's highly recommended you 
        /// use the named parameters when calling the method instead of relying on
        /// parameter order.
        /// </summary>
        /// <param name="name">Text used for the tab title. An id value will be generated from this name.</param>
        /// <param name="partialName">Name of the partial view that should be used to render the body of this tab.</param>
        /// <param name="partialModel">The model passed to the partial view. If no model is set, the parent model will be passed in automatically.</param>
        /// <param name="isVisible">If false, the partial view will not be executed and the tab will not be visible during rendering.</param>
        /// <param name="isSelected">If true, this tab will be displayed by default.</param>
        /// <param name="bodyHtmlAttributes">Additional html attributes used for the div that wraps the body of this specific tab.</param>
        /// <param name="tabHtmlAttributes">Additional html attributes used for the tab itself.</param>
        /// <param name="itemCount">Optional number appended to the tab text but not the #hash fragment.</param>
        /// <returns></returns>
        public TabBuilder WithTab(string name, [AspMvcView] string partialName, object partialModel,
            bool isVisible = true, bool isSelected = false, object bodyHtmlAttributes = null,
            object tabHtmlAttributes = null, int? itemCount = null)
        {
            // ReSharper disable RedundantArgumentName
            return WithPartialViewTab(name,
                ChangeNameToId(name),
                partialName,
                partialModel,
                isVisible: isVisible,
                isSelected: isSelected,
                useParentViewModel: false,
                bodyHtmlAttributes: bodyHtmlAttributes,
                tabHtmlAttributes: tabHtmlAttributes,
                isAjaxTab: false,
                extraData: null,
                itemCount: itemCount,
                updateTargetId: null);
            // ReSharper restore RedundantArgumentName
        }

        /// <summary>
        /// Creates a new Tab and uses a partial view to render the body html. Model inherited from parent view.
        /// 
        /// Because of the number of optional parameters, it's highly recommended you 
        /// use the named parameters when calling the method instead of relying on
        /// parameter order.
        /// </summary>
        /// <param name="name">Text used for the tab title.</param>
        /// <param name="id">The id to be used for the tab and tab anchor.</param>
        /// <param name="partialName">Name of the partial view that should be used to render the body of this tab.</param>
        /// <param name="isVisible">If false, the partial view will not be executed and the tab will not be visible during rendering.</param>
        /// <param name="isSelected">If true, this tab will be displayed by default.</param>
        /// <param name="bodyHtmlAttributes">Additional html attributes used for the div that wraps the body of this specific tab.</param>
        /// <param name="tabHtmlAttributes">Additional html attributes used for the tab itself.</param>
        /// <param name="itemCount">Optional number appended to the tab text but not the #hash fragment.</param>
        /// <returns></returns>
        public TabBuilder WithTab(string name, string id, [AspMvcView] string partialName, bool isVisible = true,
            bool isSelected = false, object bodyHtmlAttributes = null, object tabHtmlAttributes = null,
            int? itemCount = null)
        {
            // ReSharper disable RedundantArgumentName
            return WithPartialViewTab(name,
                id,
                partialName,
                null,
                isVisible: isVisible,
                isSelected: isSelected,
                useParentViewModel: true,
                bodyHtmlAttributes: bodyHtmlAttributes,
                tabHtmlAttributes: tabHtmlAttributes,
                isAjaxTab: false,
                extraData: null,
                itemCount: itemCount,
                updateTargetId: null);
            // ReSharper restore RedundantArgumentName
        }

        /// <summary>
        /// Creates a new Tab and uses a partial view to render the body html.
        /// 
        /// Because of the number of optional parameters, it's highly recommended you 
        /// use the named parameters when calling the method instead of relying on
        /// parameter order.
        /// </summary>
        /// <param name="name">Text used for the tab title.</param>
        /// <param name="id">The id to be used for the tab and tab anchor.</param>
        /// <param name="partialName">Name of the partial view that should be used to render the body of this tab.</param>
        /// <param name="partialModel">The model passed to the partial view. If no model is set, the parent model will be passed in automatically.</param>
        /// <param name="isVisible">If false, the partial view will not be executed and the tab will not be visible during rendering.</param>
        /// <param name="isSelected">If true, this tab will be displayed by default.</param>
        /// <param name="bodyHtmlAttributes">Additional html attributes used for the div that wraps the body of this specific tab.</param>
        /// <param name="tabHtmlAttributes">Additional html attributes used for the tab itself.</param>
        /// <param name="itemCount">Optional number appended to the tab text but not the #hash fragment.</param>
        /// <returns></returns>
        public TabBuilder WithTab(string name, string id, [AspMvcView] string partialName, object partialModel,
            bool isVisible = true, bool isSelected = false, object bodyHtmlAttributes = null,
            object tabHtmlAttributes = null, int? itemCount = null)
        {
            // ReSharper disable RedundantArgumentName
            return WithPartialViewTab(name,
                id,
                partialName,
                partialModel,
                isVisible: isVisible,
                isSelected: isSelected,
                useParentViewModel: false,
                bodyHtmlAttributes: bodyHtmlAttributes,
                tabHtmlAttributes: tabHtmlAttributes,
                isAjaxTab: false,
                extraData: null,
                itemCount: itemCount,
                updateTargetId: null);
            // ReSharper restore RedundantArgumentName
        }

        #endregion

        #region WithAjaxTab with HelperResult function.

        /// <summary>
        /// Creates a new Tab and uses the helperResult to render the body html.
        /// 
        /// Because of the number of optional parameters, it's highly recommended you 
        /// use the named parameters when calling the method instead of relying on
        /// parameter order.
        /// </summary>
        /// <param name="name">Text used for the tab title. An id value will be generated from this name.</param>
        /// <param name="helperResult">Function that returns the html needed for this tab's body.</param>
        /// <param name="isVisible">If false, the helper result will not be executed and the tab will not be visible during rendering.</param>
        /// <param name="isSelected">If true, this tab will be displayed by default.</param>
        /// <param name="bodyHtmlAttributes">Additional html attributes used for the div that wraps the body of this specific tab.</param>
        /// <param name="tabHtmlAttributes">Additional html attributes used for the tab itself.</param>
        /// <param name="itemCount">Optional number appended to the tab text but not the #hash fragment.</param>
        /// <param name="updateTargetId">Optional id for the target element that will be updated when the ajax tab updates.</param>
        /// <returns></returns>
        public TabBuilder WithAjaxTab(string name, Func<object, HelperResult> helperResult, bool isVisible = true,
            bool isSelected = false, object bodyHtmlAttributes = null, object tabHtmlAttributes = null,
            int? itemCount = null, string updateTargetId = null)
        {
            // ReSharper disable RedundantArgumentName
            return WithAjaxTab(name,
                ChangeNameToId(name),
                helperResult,
                isVisible: isVisible,
                isSelected: isSelected,
                bodyHtmlAttributes: bodyHtmlAttributes,
                tabHtmlAttributes: tabHtmlAttributes,
                itemCount: itemCount,
                updateTargetId: updateTargetId);
            // ReSharper restore RedundantArgumentName
        }

        /// <summary>
        /// Creates a new Tab and uses the helperResult to render the body html. 
        /// 
        /// Because of the number of optional parameters, it's highly recommended you 
        /// use the named parameters when calling the method instead of relying on
        /// parameter order.
        /// </summary>
        /// <param name="name">Text used for the tab title.</param>
        /// <param name="id">The id to be used for the tab and tab anchor.</param>
        /// <param name="helperResult">Function that returns the html needed for this tab's body.</param>
        /// <param name="isVisible">If false, the helper result will not be executed and the tab will not be visible during rendering.</param>
        /// <param name="isSelected">If true, this tab will be displayed by default.</param>
        /// <param name="bodyHtmlAttributes">Additional html attributes used for the div that wraps the body of this specific tab.</param>
        /// <param name="tabHtmlAttributes">Additional html attributes used for the tab itself.</param>
        /// <param name="itemCount">Optional number appended to the tab text but not the #hash fragment.</param>
        /// <param name="updateTargetId">Optional id for the target element that will be updated when the ajax tab updates.</param>
        /// <returns></returns>
        public TabBuilder WithAjaxTab(string name, string id, Func<object, HelperResult> helperResult,
            bool isVisible = true, bool isSelected = false, object bodyHtmlAttributes = null,
            object tabHtmlAttributes = null, int? itemCount = null, string updateTargetId = null)
        {
            return WithHelperResultTab(name, id, helperResult, isVisible, isSelected, bodyHtmlAttributes,
                tabHtmlAttributes, isAjaxTab: true, itemCount: itemCount, updateTargetId: updateTargetId);
        }

        #endregion

        #region WithAjaxTab with Partial Views

        /// <summary>
        /// Creates a new Tab and uses a partial view to render the body html. Model is inherited from the parent view.
        /// 
        /// Because of the number of optional parameters, it's highly recommended you 
        /// use the named parameters when calling the method instead of relying on
        /// parameter order.
        /// </summary>
        /// <param name="name">Text used for the tab title. An id value will be generated from this name.</param>
        /// <param name="partialName">Name of the partial view that should be used to render the body of this tab.</param>
        /// <param name="isVisible">If false, the partial view will not be executed and the tab will not be visible during rendering.</param>
        /// <param name="isSelected">If true, this tab will be displayed by default.</param>
        /// <param name="bodyHtmlAttributes">Additional html attributes used for the div that wraps the body of this specific tab.</param>
        /// <param name="tabHtmlAttributes">Additional html attributes used for the tab itself.</param>
        /// <param name="itemCount">Optional number appended to the tab text but not the #hash fragment.</param>
        /// <param name="updateTargetId">Optional id for the target element that will be updated when the ajax tab updates.</param>
        /// <returns></returns>
        public TabBuilder WithAjaxTab(string name, [AspMvcView] string partialName, bool isVisible = true,
            bool isSelected = false, object bodyHtmlAttributes = null, object tabHtmlAttributes = null,
            int? itemCount = null, string updateTargetId = null)
        {
            // ReSharper disable RedundantArgumentName
            return WithPartialViewTab(name,
                ChangeNameToId(name),
                partialName,
                null,
                isVisible: isVisible,
                isSelected: isSelected,
                useParentViewModel: true,
                bodyHtmlAttributes: bodyHtmlAttributes,
                tabHtmlAttributes: tabHtmlAttributes,
                isAjaxTab: true,
                extraData: null,
                itemCount: itemCount,
                updateTargetId: updateTargetId);
            // ReSharper restore RedundantArgumentName
        }

        /// <summary>
        /// Creates a new Tab and uses a partial view to render the body html.
        /// 
        /// Because of the number of optional parameters, it's highly recommended you 
        /// use the named parameters when calling the method instead of relying on
        /// parameter order.
        /// </summary>
        /// <param name="name">Text used for the tab title. An id value will be generated from this name.</param>
        /// <param name="partialName">Name of the partial view that should be used to render the body of this tab.</param>
        /// <param name="partialModel">The model passed to the partial view. If no model is set, the parent model will be passed in automatically.</param>
        /// <param name="isVisible">If false, the partial view will not be executed and the tab will not be visible during rendering.</param>
        /// <param name="isSelected">If true, this tab will be displayed by default.</param>
        /// <param name="bodyHtmlAttributes">Additional html attributes used for the div that wraps the body of this specific tab.</param>
        /// <param name="tabHtmlAttributes">Additional html attributes used for the tab itself.</param>
        /// <param name="itemCount">Optional number appended to the tab text but not the #hash fragment.</param>
        /// <param name="updateTargetId">Optional id for the target element that will be updated when the ajax tab updates.</param>
        /// <returns></returns>
        public TabBuilder WithAjaxTab(string name, [AspMvcView] string partialName, object partialModel,
            bool isVisible = true, bool isSelected = false, object bodyHtmlAttributes = null,
            object tabHtmlAttributes = null, int? itemCount = null, string updateTargetId = null)
        {
            // ReSharper disable RedundantArgumentName
            return WithPartialViewTab(name,
                ChangeNameToId(name),
                partialName,
                partialModel,
                isVisible: isVisible,
                isSelected: isSelected,
                useParentViewModel: false,
                bodyHtmlAttributes: bodyHtmlAttributes,
                tabHtmlAttributes: tabHtmlAttributes,
                isAjaxTab: true,
                extraData: null,
                itemCount: itemCount,
                updateTargetId: updateTargetId);
            // ReSharper restore RedundantArgumentName
        }

        /// <summary>
        /// Creates a new Tab and uses a partial view to render the body html. Model inherited from parent view.
        /// 
        /// Because of the number of optional parameters, it's highly recommended you 
        /// use the named parameters when calling the method instead of relying on
        /// parameter order.
        /// </summary>
        /// <param name="name">Text used for the tab title.</param>
        /// <param name="id">The id to be used for the tab and tab anchor.</param>
        /// <param name="partialName">Name of the partial view that should be used to render the body of this tab.</param>
        /// <param name="isVisible">If false, the partial view will not be executed and the tab will not be visible during rendering.</param>
        /// <param name="isSelected">If true, this tab will be displayed by default.</param>
        /// <param name="bodyHtmlAttributes">Additional html attributes used for the div that wraps the body of this specific tab.</param>
        /// <param name="tabHtmlAttributes">Additional html attributes used for the tab itself.</param>
        /// <param name="itemCount">Optional number appended to the tab text but not the #hash fragment.</param>
        /// <param name="updateTargetId">Optional id for the target element that will be updated when the ajax tab updates.</param>
        /// <returns></returns>
        public TabBuilder WithAjaxTab(string name, string id, [AspMvcView] string partialName, bool isVisible = true,
            bool isSelected = false, object bodyHtmlAttributes = null, object tabHtmlAttributes = null,
            int? itemCount = null, string updateTargetId = null)
        {
            // ReSharper disable RedundantArgumentName
            return WithPartialViewTab(name,
                id,
                partialName,
                null,
                isVisible: isVisible,
                isSelected: isSelected,
                useParentViewModel: true,
                bodyHtmlAttributes: bodyHtmlAttributes,
                tabHtmlAttributes: tabHtmlAttributes,
                isAjaxTab: true,
                extraData: null,
                itemCount: itemCount,
                updateTargetId: updateTargetId);
            // ReSharper restore RedundantArgumentName
        }

        /// <summary>
        /// Creates a new Tab and uses a partial view to render the body html.
        /// 
        /// Because of the number of optional parameters, it's highly recommended you 
        /// use the named parameters when calling the method instead of relying on
        /// parameter order.
        /// </summary>
        /// <param name="name">Text used for the tab title.</param>
        /// <param name="id">The id to be used for the tab and tab anchor.</param>
        /// <param name="partialName">Name of the partial view that should be used to render the body of this tab.</param>
        /// <param name="partialModel">The model passed to the partial view. If no model is set, the parent model will be passed in automatically.</param>
        /// <param name="isVisible">If false, the partial view will not be executed and the tab will not be visible during rendering.</param>
        /// <param name="isSelected">If true, this tab will be displayed by default.</param>
        /// <param name="bodyHtmlAttributes">Additional html attributes used for the div that wraps the body of this specific tab.</param>
        /// <param name="tabHtmlAttributes">Additional html attributes used for the tab itself.</param>
        /// <param name="itemCount">Optional number appended to the tab text but not the #hash fragment.</param>
        /// <param name="updateTargetId">Optional id for the target element that will be updated when the ajax tab updates.</param>
        /// <returns></returns>
        public TabBuilder WithAjaxTab(string name, string id, [AspMvcView] string partialName, object partialModel,
            bool isVisible = true, bool isSelected = false, object bodyHtmlAttributes = null,
            object tabHtmlAttributes = null, int? itemCount = null, string updateTargetId = null)
        {
            // ReSharper disable RedundantArgumentName
            return WithPartialViewTab(name,
                id,
                partialName,
                partialModel,
                isVisible: isVisible,
                isSelected: isSelected,
                useParentViewModel: false,
                bodyHtmlAttributes: bodyHtmlAttributes,
                tabHtmlAttributes: tabHtmlAttributes,
                isAjaxTab: true,
                extraData: null,
                itemCount: itemCount,
                updateTargetId: updateTargetId);
            // ReSharper restore RedundantArgumentName
        }

        #endregion

        public string ToHtmlString()
        {
            return ToString();
        }

        public override string ToString()
        {
            return Render(HtmlHelper);
        }

        #endregion

        #region Implementation Details

        /// <summary>
        /// The internal Tab object used for rendering. This class is public only for unit test purposes.
        /// You should never be modifying or accessing this directly.
        /// </summary>
        /// <remarks>
        /// 
        /// So why is this for unit testing only? Because TabBuilder itself already tests the rendered
        /// output of TabBuilder. TabBuilderExtensions only need to test that the Tab arguments are correct.
        /// 
        /// </remarks>
        public sealed class Tab
        {
            #region Properties

            public object BodyHtmlAttributes { get; set; }
            public object TabHtmlAttributes { get; set; }

            public string TabId { get; set; }
            public string TabText { get; set; }

            /// <summary>
            /// An extra number to append to the tab text but not the url fragment.
            /// </summary>
            public int? TabItemCount { get; set; }

            public string PartialName { get; set; }
            public object PartialModel { get; set; }
            public bool IsAjaxTab { get; set; }
            public bool IsSelected { get; set; }
            public bool IsVisible { get; set; }
            public ViewDataDictionary ExtraData { get; set; }

            public Func<object, HelperResult> HelperResult { get; set; }

            public bool UseParentViewModel { get; set; }

            /// <summary>
            /// Gets/sets the optional id of the target element that will be updated when the ajax tab updates.
            /// This must be an element that exists inside of the tab!
            /// </summary>
            public string AjaxUpdateTargetId { get; set; }

            #endregion
        }

        #endregion
    }
}
