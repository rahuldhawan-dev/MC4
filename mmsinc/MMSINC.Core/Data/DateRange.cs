using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions.EnumExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Utilities;
using NHibernate.Criterion;

namespace MMSINC.Data
{
    public class DateRange : Range<DateTime>
    {
        #region Properties

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public override DateTime? Start
        {
            get { return base.Start; }
            set { base.Start = value; }
        }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public override DateTime? End
        {
            get { return base.End; }
            set { base.End = value; }
        }

        protected override string StartValueRequiredErrorMessage
        {
            get { return "Start date is required."; }
        }

        protected override string StartMustBeLessThanEndErrorMessage
        {
            get { return "Start date must be prior to end date."; }
        }

        #endregion

        #region Exposed Methods

        public override ICriterion GetCriterion(ICriterion criterion, string propertyName)
        {
            // Don't use Restrictions.Between in this override. When date math and grouping are involved,
            // the time gets stripped from the parameters for some reason. When trying to search for all records 
            // for a single day, this results in a query like "where Column between '1/1/2018 00:00:00 AM' and '1/1/2018 00:00:00 AM'"), 
            // effectively only returning results that are equal to midnight. 
            // See UserViewedRepository.SearchDailyReportItems for an example of this breaking if you use Restrictions.Between.

            if (End == null)
            {
                return criterion;
            }

            if (Operator == RangeOperator.Equal)
            {
                var start = End.Value.BeginningOfDay();
                var end = start.AddDays(1); // midnight of the next day
                return Restrictions.Conjunction()
                                   .Add(Restrictions.Ge(propertyName, start))
                                   .Add(Restrictions.Lt(propertyName, end));
            }

            if (Operator == RangeOperator.Between)
            {
                var start = Start.Value.BeginningOfDay();
                var end = End.Value.BeginningOfDay().AddDays(1); // midnight of the next day.
                return Restrictions.Conjunction()
                                   .Add(Restrictions.Ge(propertyName, start))
                                   .Add(Restrictions.Lt(propertyName, end));
            }

            return base.GetCriterion(criterion, propertyName);
        }

        public override string ToString()
        {
            switch (Operator)
            {
                case RangeOperator.Between:
                    return $"{Start.Value:d} - {End.Value:d}";
                default:
                    return $"{Enum.GetName(typeof(RangeOperator), Operator).ToTitleCase()} {End.Value:d}";
            }
        }

        #endregion
    }
}
