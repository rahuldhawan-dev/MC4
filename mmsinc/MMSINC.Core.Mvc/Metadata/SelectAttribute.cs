using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using JetBrains.Annotations;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Helpers;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Represents a field that should be a dropdown. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SelectAttribute : Attribute, ICustomModelMetadataAttribute
    {
        #region Consts

        /// <summary>
        /// They key used when adding a SelectAttribute instance to a ModelMetadata's
        /// AdditionalValues dictionary.
        /// </summary>
        public const string ADDITIONAL_VALUES_KEY = "SelectAttribute",
                            DROPDOWN_TEMPLATE_HINT = "Select",
                            DEFAULT_ITEM_LABEL = ControlHelper.DEFAULT_EMPTY_TEXT_FOR_DROPDOWNS;

        private const string DATA_VALUE_FIELD = "Value",
                             DATA_TEXT_FIELD = "Text";

        #endregion

        #region Private Members

        private string _defaultItemLabel;
        private bool? _async;

        #endregion

        #region Properties

        private bool AllowMultiple
        {
            get { return Type == SelectType.MultiSelect || Type == SelectType.CheckBoxList; }
        }

        /// <summary>
        /// Gets/sets the key to the item collection in the ViewData used for this dropdown. If not set,
        /// this defaults to the name of the model property beind bound.
        ///
        /// Don't use this. This is an old hack to make the AddDropDownData method work without
        /// having to specify the model's property name if the property name doesn't match the
        /// name of the entity type. It was also used to share data when multiple dropdowns use
        /// the same data. This feature is barely used and the things using it can be rewritten
        /// to use autopopulated dropdown stuff or with a slightly different cache in SetLookupData.
        /// </summary>
        [Obsolete("Don't use this. See comment.")]
        public string ControllerViewDataKey { get; set; }

        public string DefaultItemLabel
        {
            get { return _defaultItemLabel ?? DEFAULT_ITEM_LABEL; }
            set { _defaultItemLabel = value; }
        }

        public SelectType Type { get; set; }

        /// <summary>
        /// Set to true if the ValueField shouldn't be generated from the bound property's name. 
        /// Basically, use this because you have an IEnumerable of strings for items.
        /// </summary>
        public bool UseNullForValueField { get; set; }

        #region Cascading related

        /// <summary>
        /// Gets/sets whether all SelectAttributes can or can't use async requests when cascade is enabled. Default is true.
        /// </summary>
        public static bool EnableAsync { get; set; }

        /// <summary>
        /// Gets/sets the controller action that's called when the parent's value is changed.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets/sets the area the controller action belongs to.
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Gets/sets the controller called when the parent's value is changed.
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Gets/sets this individual CascadingAttribute's async. If EnableAsync is false,
        /// then this will also return false.
        /// </summary>
        public bool? Async
        {
            get
            {
                if (!EnableAsync)
                {
                    return false;
                }

                return _async.GetValueOrDefault(EnableAsync);
            }
            set { _async = value; }
        }

        /// <summary>
        /// The name of the parent property this cascading drop down depends on. Multiple properties are allowed and must
        /// be separated by a comma. Property names MUST BE IN THE SAME ORDER AS THE PARAMETERS OF THE CONTROLLER ACTION.
        /// Also see the DependentsRequired documentation.
        /// </summary>
        public string DependsOn { get; set; }

        /// <summary>
        /// Gets or sets how the child dropdown will with values.
        /// All = All parents must have non-null values.
        /// One = At least one parent must have a non-null value.
        /// None = All parents can still have a null value.
        /// 
        /// Default is All.
        ///
        /// When using One or None, the action MUST support null values for every parameter.
        /// </summary>
        /// <remarks>
        /// 
        /// NOTE: If you set this to None, then the action will always be called. It is up to the action
        /// to deal with null values! Similarly, if you have multiple dependents and only one value is required
        /// then it is up to the action to deal with null values there.
        /// 
        /// No, you can not configure the SelectAttribute to have multiple dependents with one of them
        /// being required. You have to deal with this in the action.
        /// 
        /// </remarks>
        public DependentRequirement DependentsRequired { get; set; }

        /// <summary>
        /// Gets the error text that should be displayed. If no value is set, defaults to the script default.
        /// </summary>
        public string ErrorText { get; set; }

        /// <summary>
        /// Gets the http method the ajax request should make. If no value is set, defaults to the script default.
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// Gets the loading text that should be displayed. If no value is set, defaults to the script default.
        /// </summary>
        public string LoadingText { get; set; }

        /// <summary>
        /// Gets the prompt text that should be displayed. If no value is set, defaults to the script default.
        /// </summary>
        public string PromptText { get; set; }

        /// <summary>
        /// Gets the name of the property that this attribute is applied to. This value is set when
        /// the Process method is called.
        /// </summary>
        public string OwningProperty { get; private set; }

        /// <summary>
        /// Gets whether or not this attribute should be used for cascading data.
        /// </summary>
        public bool IsCascading
        {
            get { return (!string.IsNullOrWhiteSpace(Action) && !string.IsNullOrWhiteSpace(Controller)); }
        }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for if you just wanna use named parameters.
        /// </summary>
        public SelectAttribute()
        {
            DependentsRequired = DependentRequirement.All;
        }

        public SelectAttribute(SelectType type) : this(type, null) { }

        public SelectAttribute(SelectType type, string controllerViewDataKey)
            : this()
        {
            Type = type;
            ControllerViewDataKey = controllerViewDataKey;
        }

        /// <summary>
        /// Creates a new cascading SelectAttribute instance with the given controller and action names. 
        /// If an Area is not set, the default area will be used.
        /// </summary>
        public SelectAttribute(SelectType type, [AspMvcController] string controllerName,
            [AspMvcAction] string actionName)
            : this(type)
        {
            Controller = controllerName;
            Action = actionName;
        }

        /// <summary>
        /// Creates a new cascading SelectAttribute instance with the given area, controller, and action names.
        /// </summary>
        public SelectAttribute(SelectType type, [AspMvcArea] string areaName, [AspMvcController] string controllerName,
            [AspMvcAction] string actionName)
            : this(type, controllerName, actionName)
        {
            Area = areaName;
        }

        #endregion

        #region Private methods

        protected IEnumerable<SelectListItem> GetSelectListForDefault(ViewDataDictionary viewData, object selectedValue)
        {
            // TODO: This just needs to return SelectListItems because SelectListBuilder handles all this now.
            var items = GetItems(viewData);

            if (AllowMultiple && selectedValue != null)
            {
                if (!(selectedValue is IEnumerable) || (selectedValue is string))
                {
                    throw new InvalidOperationException(
                        "A select list that allows multiple selected values requires that the selected value be IEnumerable.");
                }

                return new MultiSelectList(items, DATA_VALUE_FIELD, DATA_TEXT_FIELD, (IEnumerable)selectedValue);
            }

            // Mvc's DropDownList doesn't understand how to match an enum value to a selected id 
            // if the id is not explicitly an integer. So this blows up if you have an enum dropdown
            // and then try to set the model's property value explicitly(versus it being a posted value).
            if (selectedValue != null && selectedValue.GetType().IsEnum)
            {
                // Use convert instead of casting, enums may not necessarily be
                // integers.
                selectedValue = Convert.ToInt32(selectedValue);
            }

            return new SelectList(items, DATA_VALUE_FIELD, DATA_TEXT_FIELD, selectedValue);
        }

        protected IEnumerable GetItems(ViewDataDictionary viewData)
        {
            var key = GetViewDataKey(viewData.ModelMetadata);
            var items = (IEnumerable)viewData[key];
            if (items == null)
            {
                throw new NullReferenceException("No items found in ViewData for key: " + key);
            }

            // Because HtmlHelper.DropDownListFor is poorly implemented, we need to remove the
            // items from viewdata entirely. The DropDownListFor method, for whatever reason, overrides
            // the selected value of a SelectList if ViewData has a key that matches the name of the DataValueField
            // in the SelectList. Since we're using that value to store items in the ViewData, this prevents
            // use from actually setting the selected value. Removing the key stops this from happening. 
            viewData.Remove(key);

            return items;
        }

        private string GetViewDataKey(ModelMetadata metaData)
        {
            return !string.IsNullOrWhiteSpace(ControllerViewDataKey)
                ? ControllerViewDataKey
                : metaData.PropertyName;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the SelectAttribute instance found in ModelMetadata. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public static SelectAttribute GetFromModelMetadata(ModelMetadata metadata)
        {
            object result;
            if (metadata.AdditionalValues.TryGetValue(ADDITIONAL_VALUES_KEY, out result))
            {
                return (SelectAttribute)result;
            }

            return null;
        }

        public virtual IEnumerable<SelectListItem> GetSelectList(ViewDataDictionary viewData, object selectedValue)
        {
            return GetSelectListForDefault(viewData, selectedValue);
        }

        public void Process(ModelMetadata modelMetaData)
        {
            modelMetaData.ThrowIfNull("modelMetaData");

            // ControllerViewDataKey.ThrowIfNullOrWhiteSpace("ControllerViewDataKey");
            // Using Add so we don't accidently overwrite an existing instance,
            // or if someone's using the same key for some stupid reason. 
            modelMetaData.AdditionalValues.Add(ADDITIONAL_VALUES_KEY, this);
            modelMetaData.TemplateHint = DROPDOWN_TEMPLATE_HINT;
            OwningProperty = modelMetaData.PropertyName;

            if (IsCascading)
            {
                if (Type == SelectType.ComboBox)
                {
                    throw new NotSupportedException(Type + " is not supported for cascades.");
                }

                DependsOn.ThrowIfNullOrWhiteSpace("DependsOn property is not set on " + modelMetaData.ModelType.Name +
                                                  "." + modelMetaData.PropertyName);
            }
        }

        #endregion
    }

    public enum SelectType
    {
        DropDown = 0, // Force to be default
        MultiSelect,
        CheckBoxList,
        ComboBox
    }

    public enum DependentRequirement
    {
        All = 0, // Force to be default
        /// <summary>
        /// Remember that the action used with this *must* support null values for all parameters.
        /// </summary>
        One,
        /// <summary>
        /// Remember that the action used with this *must* support null values for all parameters.
        /// </summary>
        None
    }
}
