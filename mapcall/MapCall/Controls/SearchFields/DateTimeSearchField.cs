using System;
using System.Web.UI;
using MMSINC.DataPages;

namespace MapCall.Controls.SearchFields
{
    public class DateTimeSearchField : BaseSearchField
    {
        #region Fields

        /// <summary>
        /// Don't call this field directly. use the DateControl property getter. 
        /// This field is lazy-loaded.
        /// </summary>
        private DateTimeRange _dateControl;

        #endregion

        #region Properties

        private DateTimeRange DateControl
        {
            get
            {
                if(_dateControl == null)
                {
                    _dateControl = TemplateControlHelper.CreateControl<DateTimeRange>("~/Controls/DateTimeRange.ascx");
                }
                return _dateControl;
            }
        }

        public bool ShowTime
        {
            get { return DateControl.ShowTimePicker; }
            set
            {
                DateControl.ShowTimePicker = value;
            }
        }

        public DateTime StartDate
        {
            get { return DateControl.StartDate.Value; }
            set { DateControl.StartDate = value; }
        }

        public DateTime EndDate
        {
            get { return DateControl.EndDate.Value; }
            set { DateControl.EndDate = value; }
        }
        #endregion



        public override void AddControlsToTemplate(Control template)
        {
            DateControl.ClientIDMode = this.ClientIDMode;
            DateControl.ID = GetControlID();
            DateControl.UseBetterControlNamingScheme = true;
            DateControl.SelectedOperatorType = OperatorTypes.Between;
            template.Controls.Add(DateControl);
        }

        public override void SetValue(object value)
        {
            throw new NotImplementedException();
        }

        protected override void AddExpressions(IFilterBuilder builder)
        {
            _dateControl.FilterExpression(builder, DataFieldName);
        }
    }
}