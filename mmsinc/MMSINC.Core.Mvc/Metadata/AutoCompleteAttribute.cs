using System;
using System.Web.Mvc;
using JetBrains.Annotations;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.StringExtensions;

namespace MMSINC.Metadata
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AutoCompleteAttribute : Attribute, ICustomModelMetadataAttribute
    {
        #region Consts

        private const string METADATA_KEY = "AutoCompleteAttributeData",
                             DEFAULT_HTTP_METHOD = "GET";

        #endregion

        #region Fields

        private string _httpMethod;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the controller action that's called when the parent's value is changed.
        /// </summary>
        public string Action { get; private set; }

        /// <summary>
        /// Gets/sets the area the controller action belongs to.
        /// </summary>
        public string Area { get; private set; }

        /// <summary>
        /// Gets/sets the controller called when the parent's value is changed.
        /// </summary>
        public string Controller { get; private set; }

        /// <summary>
        /// Gets/sets the http method required to access the controller action. Defaults to "GET" if not set.
        /// </summary>
        public string HttpMethod
        {
            get { return !string.IsNullOrWhiteSpace(_httpMethod) ? _httpMethod : DEFAULT_HTTP_METHOD; }
            set { _httpMethod = value; }
        }

        /// <summary>
        /// Gets / Sets the field that the autocomplete results should be filtered on
        /// </summary>
        public string DependsOn { get; set; } = "";

        /// <summary>
        /// Gets / Sets the field that the autocomplete text box uses as a place holder
        /// </summary>
        public string PlaceHolder { get; set; }

        /// <summary>
        /// Gets/sets the optional property name that will be used as the display value when the
        /// autocomplete control first renders. If this is not set, the property value will be
        /// displayed instead. Setting this is only necessary when the value needs to be pre-populated
        /// by the server.
        ///
        /// NOTE: If you are using this then the view model property must also have EntityMustExist.
        /// </summary>
        public string DisplayProperty { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new AutoCompleteAttribute instance with the given controller and action names. 
        /// If an Area is not set, the default area will be used.
        /// </summary>
        public AutoCompleteAttribute([AspMvcController] string controllerName, [AspMvcAction] string actionName) : this(
            string.Empty, controllerName, actionName) { }

        /// <summary>
        /// Creates a new AutoCompleteAttribute instance with the given area, controller, and action names.
        /// </summary>
        public AutoCompleteAttribute([AspMvcArea] string areaName, [AspMvcController] string controllerName,
            [AspMvcAction] string actionName)
        {
            controllerName.ThrowIfNullOrWhiteSpace("controllerName");
            actionName.ThrowIfNullOrWhiteSpace("actionName");
            Controller = controllerName;
            Action = actionName;
            Area = !string.IsNullOrWhiteSpace(areaName) ? areaName : string.Empty;
        }

        #endregion

        #region Public Methods

        public void Process(ModelMetadata modelMetaData)
        {
            modelMetaData.AdditionalValues.Add(METADATA_KEY, this);
            modelMetaData.TemplateHint = "AutoComplete";
        }

        /// <summary>
        /// Returns the AutoCompleteAttribute for the given ModelMetadata. Returns null if an attribute
        /// does not exist.
        ///  </summary>
        public static AutoCompleteAttribute GetAttributeForModel(ModelMetadata meta)
        {
            meta.ThrowIfNull("metadata");

            object attr;
            if (meta.AdditionalValues.TryGetValue(METADATA_KEY, out attr))
            {
                return (AutoCompleteAttribute)attr;
            }

            return null;
        }

        #endregion
    }
}
