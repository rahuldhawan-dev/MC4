using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Utilities;
using MMSINC.Validation;

namespace MMSINC.Metadata
{
    public class DateTimeModelValidatorProvider : AssociatedValidatorProvider
    {
        #region Fields

        private static readonly Type _dateTimeType = typeof(DateTime);
        private static readonly Type _nullableDateTimeType = typeof(DateTime?);

        #endregion

        #region Methods

        protected override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context,
            IEnumerable<Attribute> attributes)
        {
            // We need to ignore anything that's not a date, obviously.
            if (metadata.ModelType != _dateTimeType && metadata.ModelType != _nullableDateTimeType)
            {
                return Enumerable.Empty<ModelValidator>();
            }

            // If there's already a RangeAttribute, then another
            // validator will have to handle it. We don't want
            // redundant validators since it throws an exception.
            if (attributes.OfType<RangeAttribute>().Any())
            {
                return Enumerable.Empty<ModelValidator>();
            }
            
            var minimum = "1/1/1753 12:00:00 AM";
            // if we have a CurrentOrFutureDateAttribute we want to set the minimum to today
            // this could be a potential issue for scheduling late in the evening in diff time zones
            if (attributes.OfType<CurrentOrFutureDateAttribute>().Any())
            {
                var attribute = attributes.OfType<CurrentOrFutureDateAttribute>().FirstOrDefault();
                var dateTimeProvider = DependencyResolver.Current.GetService<IDateTimeProvider>();
                minimum = dateTimeProvider.GetCurrentDate().AddDays(attribute.AddDays).BeginningOfDay().ToString();
            }

            var range = new RangeAttribute(_dateTimeType, minimum, "12/31/9999 11:59:59 PM");
            return new[] {new DateTimeRangeAdapter(metadata, context, range)};
        }

        #endregion
    }
}
