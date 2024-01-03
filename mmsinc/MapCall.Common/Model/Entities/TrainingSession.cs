using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MMSINC.Common;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TrainingSession : IEntity, IValidatableObject
    {
        public struct CalendarColors
        {
            public struct Background
            {
                public const string LESS_THAN_FIFTY = "green",
                                    LESS_THAN_ONE_HUNDRED = "yellow",
                                    ONE_HUNDRED = "red",
                                    PAST = "gray",
                                    CANCELED = "black";
            }

            public struct Text
            {
                public const string LESS_THAN_FIFTY = "white",
                                    LESS_THAN_ONE_HUNDRED = "black",
                                    ONE_HUNDRED = "white",
                                    PAST = "black",
                                    CANCELED = "white";
            }
        }

        #region Properties

        public virtual int Id { get; set; }
        public virtual DateTime StartDateTime { get; set; }
        public virtual DateTime EndDateTime { get; set; }
        public virtual TrainingRecord TrainingRecord { get; set; }

        public virtual double Duration => (EndDateTime - StartDateTime).TotalHours;

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public virtual CalendarItem ToCalendarItem(UrlHelper url)
        {
            // ReSharper disable once Mvc.ActionNotResolved, Mvc.ControllerNotResolved
            var calendarUrl = url.Action("Show", "TrainingRecord", new {id = TrainingRecord.Id});
            var item = new CalendarItem {
                title = TrainingRecord.TrainingModule == null
                    ? "RECORD HAS NO MODULE!!!"
                    : TrainingRecord.TrainingModule.Title,
                start = StartDateTime,
                end = EndDateTime,
                url = calendarUrl,
                description = GetCalendarDescription()
            };

            return DetermineCalendarColors(item);
        }

        private CalendarItem DetermineCalendarColors(CalendarItem item)
        {
            if (TrainingRecord.Canceled.GetValueOrDefault())
            {
                item.color = CalendarColors.Background.CANCELED;
                item.textColor = CalendarColors.Text.CANCELED;
                return item;
            }

            if (TrainingRecord.Past)
            {
                item.color = CalendarColors.Background.PAST;
                item.textColor = CalendarColors.Text.PAST;
                return item;
            }

            var count = TrainingRecord.Past
                ? TrainingRecord.EmployeesAttended.Count
                : TrainingRecord.EmployeesScheduled.Count;
            var percentage = (TrainingRecord?.MaximumClassSize == null || TrainingRecord?.MaximumClassSize == 0)
                ? 100
                : (count / (decimal)TrainingRecord.MaximumClassSize) * 100;

            if (percentage < 50)
            {
                item.color = CalendarColors.Background.LESS_THAN_FIFTY;
                item.textColor = CalendarColors.Text.LESS_THAN_FIFTY;
            }
            else
            {
                if (percentage < 100)
                {
                    item.color = CalendarColors.Background.LESS_THAN_ONE_HUNDRED;
                    item.textColor = CalendarColors.Text.LESS_THAN_ONE_HUNDRED;
                }
                else
                {
                    item.color = CalendarColors.Background.ONE_HUNDRED;
                    item.textColor = CalendarColors.Text.ONE_HUNDRED;
                }
            }

            return item;
        }

        private string GetCalendarDescription()
        {
            var tm = TrainingRecord.TrainingModule;
            if (tm == null)
            {
                return "No description available.";
            }

            return string.Format("{0} hours - {1} - {2}", tm.TotalHours.GetValueOrDefault(0.0f),
                TrainingRecord.ClassLocation, tm.Description);
        }

        #endregion
    }
}
