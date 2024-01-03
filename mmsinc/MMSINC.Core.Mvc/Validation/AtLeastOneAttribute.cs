using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using MMSINC.ClassExtensions.IEnumerableExtensions;

namespace MMSINC.Validation
{
    [Obsolete("Use the RequiredCollection attribute instead.")]
    public class AtLeastOneAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                throw new ArgumentException("The value argument must not be null.");

            var typed = value as IEnumerable;

            if (typed == null)
            {
                typed = value as object[];
                if (typed == null)
                {
                    throw new ArgumentException(
                        "The AtLeastOne attribute can only be applied to IEnumerable properties, or properties which can be cast to an array of objects.");
                }
            }

            return typed.Any();
        }
    }
}
