using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.DataPages;
using MMSINC.Utilities;

namespace MapCall.Controls.SearchFields
{

    public enum FilterBuilderMode
    {
        /// <summary>
        /// Default. Indicates a SearchField will automatically build a FilterBuilderExpression
        /// </summary>
        Automatic = 0,

        /// <summary>
        /// Indicates a SearachField user must manually create the FilterBuilderExpression.
        /// </summary>
        Manual = 1
    }

    public interface ISearchField : IDataField
    {
        string DataFieldName { get; set; }
        string Label { get; set; }

        #region Methods

        void AddControlsToTemplate(Control template);
        void AddExpressionsToFilterBuilder(IFilterBuilder builder);

        #endregion
    }

    /// <summary>
    /// Abstract implementation of ISearchField. 
    /// </summary>
    /// <remarks>
    /// 
    /// This isn't like BoundField in that you can use the same BoundField all over the place.
    /// This can only be used in one control at a time. 
    /// 
    /// </remarks>
    public abstract class BaseSearchField : ISearchField
    {
        #region Fields

        /// <summary>
        /// Regex to remove any weird characters from a field name that might cause an error
        /// when used as a control ID.
        /// </summary>
        private static readonly Regex _controlIdRegex = new Regex(@"[\W]", RegexOptions.Compiled);

        private string _dataFieldName;
        private string _label;
        private RequiredFieldValidator _requiredFieldValidator;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the ClientIDMode the internal controls should use. Default = ClientIDMode.Static
        /// </summary>
        public ClientIDMode ClientIDMode { get; set; }

        /// <summary>
        /// When this property is set, the GetControlID method will return this value
        /// instea of generating one itself. This is useful if there's clashing control IDs
        /// on a page. 
        /// </summary>
        public string ControlID { get; set; }

        public string DataFieldName
        {
            get
            {
                return (!string.IsNullOrWhiteSpace(_dataFieldName) ? _dataFieldName : string.Empty);
            }
            set
            {
                _dataFieldName = value;
            }
        }

        /// <summary>
        /// Defaults to Automatic!
        /// </summary>
        public FilterBuilderMode FilterMode { get; set; }

        /// <summary>
        /// Additional text that should be placed under the rendered label.
        /// </summary>
        public string HelpText { get; set; }

        public string Label
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_label))
                {
                    return _label;
                }
                if (!string.IsNullOrWhiteSpace(HeaderText))
                {
                    return GenerateLabel(HeaderText);
                }
                if (!string.IsNullOrWhiteSpace(DataFieldName))
                {
                    return GenerateLabel(DataFieldName);
                }

                return string.Empty;
            }
            set { _label = value; }
        }

        public virtual bool Required { get; set; }

        /// <summary>
        /// While this isn't a Control ID, if you set the value in markup
        /// it will generate a field in the code behind like a control would.
        /// Hurrah!
        /// </summary>
        public string ID { get; set; }

        public bool Visible { get; set; }

        public string HeaderText { get; set; }

        #endregion

        #region Constructors

        protected BaseSearchField()
        {
            this.ClientIDMode = ClientIDMode.Static;
            this.Visible = true;
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Modifies a string to be usable by Wordify.GetWordsFromCamelCase
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string MakeCamelable(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException("value");
            }

            // If a field is something like "table.Field" then we
            // just want the "Field" part.
            if (value.Contains('.'))
            {
                value = value.Split('.').Last();
            }

            value = value.Replace("_", "").Replace(" ", "");
            return value;
        }

        private static string GenerateLabel(string dataFieldName)
        {
            var words = Wordify.GetWordsFromCamelCase(MakeCamelable(dataFieldName));
            return string.Join(" ", words);
        }

        /// <summary>
        /// Gets the ID a BaseSearchField should use on its controls.
        /// </summary>
        /// <returns></returns>
        protected string GetControlID()
        {
            if (!string.IsNullOrWhiteSpace(ControlID))
            {
                return ControlID;
            }
            var name = MakeCamelable(DataFieldName);

            name = "search" + name;
            name = _controlIdRegex.Replace(name, string.Empty);

            return name;
        }

        protected RequiredFieldValidator GetRequiredFieldValidator()
        {
            if (_requiredFieldValidator == null)
            {
                _requiredFieldValidator = new RequiredFieldValidator();
                _requiredFieldValidator.ClientIDMode = ClientIDMode.AutoID;
                _requiredFieldValidator.ErrorMessage = "Required";
                _requiredFieldValidator.InitialValue = string.Empty;
                _requiredFieldValidator.Display = ValidatorDisplay.Dynamic;
            }
            return _requiredFieldValidator;
        }

        protected abstract void AddExpressions(IFilterBuilder builder);

        #endregion

        #region Public methods

        public abstract void AddControlsToTemplate(Control template);

        public void AddExpressionsToFilterBuilder(IFilterBuilder builder)
        {
            if (FilterMode == FilterBuilderMode.Manual)
            {
                return;
            }
            AddExpressions(builder);
        }
        #endregion

        #region Public ISearchField implementation

        public string FilterExpression()
        {
            throw new NotImplementedException("No thank you!");
        }

        public void FilterExpression(IFilterBuilder builder)
        {
            AddExpressionsToFilterBuilder(builder);
        }

        public abstract void SetValue(object value);

        #endregion

        #region Stupid private helper class

        protected abstract class InternalHelper<T> where T : BaseSearchField
        {
            protected T Owner { get; private set; }

            protected InternalHelper(T parentOwner)
            {
                this.Owner = parentOwner;
            }

            public abstract void AddControlsToTemplate(Control template);
            public abstract void AddExpressionsToFilterBuilder(IFilterBuilder builder);
            public abstract void SetValue(object value);
        }

        /// <summary>
        /// This class is needed so we can call TemplateControl.LoadControl() without
        /// needing an instance of a Page/UserControl. Also incorporates all the hackery
        /// needed to load the crap in an ascx file. 
        /// </summary>
        protected sealed class TemplateControlHelper : TemplateControl
        {
            private static readonly TemplateControlHelper _instance = new TemplateControlHelper();

            /// <summary>
            /// Creates a control from the relative path of its ascx template. 
            /// </summary>
            /// <typeparam name="T">The control type</typeparam>
            /// <param name="relativeControlPath">The path the control's ascx template resides in.</param>
            /// <returns></returns>
            public static T CreateControl<T>(string relativeControlPath) where T : TemplateControl
            {
                return (T)_instance.LoadControl(relativeControlPath);
            }

            // No making instances of this except for our little singleton!
            private TemplateControlHelper() { }
        }

        #endregion
    }
}