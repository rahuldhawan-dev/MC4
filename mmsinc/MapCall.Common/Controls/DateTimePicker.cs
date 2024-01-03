using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Utilities;

namespace MapCall.Common.Controls
{
    /// <summary>
    /// jQuery DateTimePicker
    /// </summary>
    /// <remarks>
    /// 
    /// Requires: jQuery UI 1.8.7(latest version tested with as of 9/15/2011)
    ///           jQuery UI DatePicker 
    ///           jQuery TimePicker http://trentrichardson.com/examples/timepicker/
    /// 
    /// NOTE: The jQuery DateTimePicker is slightly modified so that the buttonText defaults to "&nbsp;" 
    ///       for css reasons. If jQuery UI gets updated, that needs to be changed again.
    /// 
    /// NOTE: This does not wrap every option available to the jQuery DatePicker or TimePicker.
    /// 
    /// NOTE: BOOO VIEWSTATE(Backwards compatibility with DataField in MapCall).
    /// 
    /// </remarks>
    [ValidationProperty("SelectedDate")]
    public class DateTimePicker : WebControl, IPostBackDataHandler
    {
        #region Consts

        internal const string TEXTBOX_ID_SUFFIX = "_dateTimePicker";
        internal const string TEXTBOX_CSS_CLASS = "dateTimePicker";
        internal const string CALENDAR_IMAGE_URL = "~/resources/bender/images/date-time-picker-calendar.png";
        internal const string SELECTED_DATE_VIEWSTATE_KEY = "SelectedDate";

        #endregion

        #region Structs

        /// <summary>
        /// Holds properties common to both jQuery UI's DatePicker and the DateTimePicker
        /// </summary>
        private struct JqueryDatePickerKeys
        {
            public const string BUTTON_IMAGE_URL = "buttonImage";
            public const string CHANGE_MONTH = "changeMonth";
            public const string CHANGE_YEAR = "changeYear";
            public const string SHOW_ON = "showOn";
        }

        /// <summary>
        /// Holds properties unique to jQuery DateTimePicker.
        /// </summary>
        private struct JqueryTimePickerKeys
        {
            public const string AMPM = "ampm";
        }

        /// <summary>
        /// These are the default values used by the jQuery plugins as mentioned in their documentation.
        /// </summary>
        internal struct DefaultValues
        {
            public const bool AMPM = false;
            public const bool CHANGE_MONTH = false;
            public const bool CHANGE_YEAR = false;
            public const string SHOW_ON = "focus";

            // These two formats are EQUAL
            public const string CLIENT_DATE_FORMAT = "mm/dd/yy"; // NOTE: This is equal to MM/dd/yyyy in .NET.

            public const string
                NET_DATE_FORMAT = "MM/dd/yyyy"; // NOTE: this is equal to mm/dd/yy in jQuery UI's formatDate function.

            // These two formats are also EQUAL
            public const string CLIENT_TIME_FORMAT = "hh:mm TT";
            public const string NET_TIME_FORMAT = "hh:mm tt";
        }

        #endregion

        #region Events

        public event EventHandler SelectedDateChanged
        {
            add { Events.AddHandler(_onSelectedDateChangedEventKey, value); }
            remove { Events.RemoveHandler(_onSelectedDateChangedEventKey, value); }
        }

        #endregion

        #region Fields

        private static readonly object _onSelectedDateChangedEventKey = new Object();

        private readonly Dictionary<string, object> _fields = new Dictionary<string, object>();

        /// <summary>
        /// Use the ShowCalendarButton property. Do not set this field manually.
        /// </summary>
        private bool _showCalendarButton;

        #endregion

        #region Properties

        public override string ClientID
        {
            get
            {
                // We require an ID since we're using script.
                EnsureID();
                var baseCID = base.ClientID;
                // This is null because ClientIDMode == Static.
                // it's stupid. EnsureID will set the ID but not the ClientID.
                if (!string.IsNullOrWhiteSpace(baseCID))
                {
                    return baseCID;
                }

                // If ClientID is null, try and return the regular ID.
                var id = ID;
                if (!string.IsNullOrWhiteSpace(id))
                {
                    return id;
                }

                throw new InvalidOperationException("Where's the ClientID for this DateTimePicker?");
            }
        }

        internal string CurrentDateFormat
        {
            get
            {
                var format = DefaultValues.NET_DATE_FORMAT;
                if (ShowTimePicker)
                {
                    format += " " + DefaultValues.NET_TIME_FORMAT;
                }

                return format;
            }
        }

        public DateTime? SelectedDate
        {
            get { return (DateTime?)ViewState[SELECTED_DATE_VIEWSTATE_KEY]; }
            set
            {
                ViewState[SELECTED_DATE_VIEWSTATE_KEY] = value;
                OnSelectedDateChanged();
            }
        }

        internal string SelectedDateAsString
        {
            get
            {
                if (!SelectedDate.HasValue)
                {
                    return null;
                }

                return SelectedDate.Value.ToString(CurrentDateFormat);
            }
        }

        /// <summary>
        /// Gets/sets whether the time picker is shown along with the date picker. Default: false.
        /// </summary>
        public bool ShowTimePicker { get; set; }

        #region DatePicker properties

        /// <summary>
        /// Gets/sets whether a calendar button displays next to the textbox.
        /// </summary>
        public bool ShowCalendarButton
        {
            get { return _showCalendarButton; }
            set
            {
                _showCalendarButton = value;
                if (value)
                {
                    SetValue(JqueryDatePickerKeys.SHOW_ON, "both", DefaultValues.SHOW_ON);
                }
                else
                {
                    ClearValue(JqueryDatePickerKeys.SHOW_ON);
                }
            }
        }

        /// <summary>
        /// Gets/sets whether a dropdown list should appear in order to change the selected month. Default: false.
        /// </summary>
        public bool ShowMonthChangeDropDown
        {
            get { return GetValueOrDefault(JqueryDatePickerKeys.CHANGE_MONTH, DefaultValues.CHANGE_MONTH); }
            set { SetValue(JqueryDatePickerKeys.CHANGE_MONTH, value, DefaultValues.CHANGE_MONTH); }
        }

        /// <summary>
        /// Gets/sets whether a dropdown list should appear in order to change the selected year. Default: false.
        /// </summary>
        public bool ShowYearChangeDropDown
        {
            get { return GetValueOrDefault(JqueryDatePickerKeys.CHANGE_YEAR, DefaultValues.CHANGE_YEAR); }
            set { SetValue(JqueryDatePickerKeys.CHANGE_YEAR, value, DefaultValues.CHANGE_YEAR); }
        }

        #endregion

        #endregion

        #region Constructor

        public DateTimePicker()
        {
            ShowCalendarButton = true;
            //   CalendarButtonImageUrl = CALENDAR_IMAGE_URL;
        }

        #endregion

        #region Private Methods

        protected virtual void OnSelectedDateChanged()
        {
            EventHandler handler = (EventHandler)Events[_onSelectedDateChangedEventKey];
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #region _fields Handling Methods

        /// <summary>
        /// Returns the value in the _fields dictionary if a matching key exists, otherwise it returns the default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        internal T GetValueOrDefault<T>(string key, T defaultValue)
        {
            if (!_fields.ContainsKey(key))
            {
                return defaultValue;
            }

            return (T)_fields[key];
        }

        /// <summary>
        /// Sets a value in the _fields dictionary if the value is not equal to the defaultValue. If it's equal to the defaultValue, the
        /// value is cleared from the _fields dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        internal void SetValue<T>(string key, T value, T defaultValue)
        {
            if (Equals(value, defaultValue))
            {
                // We wanna remove the entry so it's not serialized if the value
                // set is the default one the plugin uses.
                ClearValue(key);
            }
            else
            {
                _fields[key] = value;
            }
        }

        /// <summary>
        /// Removes a key value pair from the _fields dictionary so that it isn't serialized to the client.
        /// </summary>
        /// <param name="key"></param>
        internal void ClearValue(string key)
        {
            _fields.Remove(key);
        }

        #endregion

        #region Lifecycle

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // We need a unique key so we don't overwrite other instances of this control used on the same page.
            ScriptManager.RegisterStartupScript(this, GetType(), "DateTimePickerInit-" + UniqueID, BuildInitScript(),
                true);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute("id", ClientID);
            writer.AddAttribute("name", UniqueID);
            writer.AddAttribute("class", TEXTBOX_CSS_CLASS);
            writer.AddAttribute("type", "text");

            if (SelectedDate.HasValue)
            {
                writer.AddAttribute("value", SelectedDate.Value.ToString(CurrentDateFormat));
            }

            writer.RenderBeginTag("input");
            // DO NOT CALL BASE. It'll just render an empty span tag with our control id.
            writer.RenderEndTag();
        }

        #endregion

        #region Scripting

        protected string BuildInitScript()
        {
            // Use jQuery instead of $ to prevent conflicts.
            const string DATE_PICKER_INIT = "jQuery(\"#{0}\").datepicker({1});";
            const string TIME_PICKER_INIT = "jQuery(\"#{0}\").datetimepicker({1});";

            var format = (ShowTimePicker ? TIME_PICKER_INIT : DATE_PICKER_INIT);

            var json = SerializeJsonProperties();

            return string.Format(format, ClientID, json);
        }

        internal string SerializeJsonProperties()
        {
            PrepareSerializedValues();
            var js = JavaScriptSerializerFactory.Build();
            return js.Serialize(_fields);
        }

        private void PrepareSerializedValues()
        {
            // This is for setting any serialized values at rendering time
            // when it's possible that a null reference could be thrown
            // by doing it in a property setter(ex: When the value depends on the Page)

            if (ShowTimePicker)
            {
                // Need to enable AMPM since our time format depends on it.
                SetValue(JqueryTimePickerKeys.AMPM, true, DefaultValues.AMPM);
            }

            // TODO: We'll wanna remove any TimePicker keys if we're not showing it.
        }

        #endregion

        #endregion

        #region IPostBackDataHandler implementation

        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            // If no post data is being loaded then we MUST RETURN FALSE. 
            // That's what WebForms expects. I think returning the wrong value
            // can cause Page.IsPostBack to be incorrect.

            if (!Enabled)
            {
                // Not enabled? Don't want postback data!
                return false;
            }

            var currentValue = SelectedDate;
            var postedValue = ParseDateFromString(postCollection[postDataKey]);

            if (currentValue != postedValue)
            {
                SelectedDate = postedValue;
                return true;
            }

            return false;
        }

        internal DateTime? ParseDateFromString(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                // Can't use ParseExact, which would be nice. We can't assume users will put a leading 0 on single digit months/days.
                var d = DateTime.Parse(value, CultureInfo.InvariantCulture.DateTimeFormat);

                if (ShowTimePicker)
                {
                    return d;
                }

                return
                    d.Date; // On the off chance a time gets posted back when we're not expecting it, we want to ignore the time but keep the date.
            }

            // If we do have a postback value of an empty/whitespace string, we wanna clear out any previous SelectedDate.
            return null;
        }

        public void RaisePostDataChangedEvent()
        {
            OnSelectedDateChanged();
            // Do nothing, I don't care yo! But I might if it matters with binding, so who knows.
        }

        #endregion
    }
}
