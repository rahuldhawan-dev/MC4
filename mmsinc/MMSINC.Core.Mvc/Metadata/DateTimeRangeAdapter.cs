using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MMSINC.Metadata
{
    public class DateTimeRangeAdapter : RangeAttributeAdapter
    {
        #region Constructors

        public DateTimeRangeAdapter(ModelMetadata metadata, ControllerContext context, RangeAttribute attribute)
            : base(metadata, context, attribute) { }

        #endregion

        #region Methods

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            // All we're doing in this method is changing "range"
            // to "daterange" to match the custom jquery validator rule.
            foreach (var rule in base.GetClientValidationRules())
            {
                if (rule.ValidationType == "range")
                {
                    rule.ValidationType = "daterange";
                }

                yield return rule;
            }
        }

        #endregion
    }
}
