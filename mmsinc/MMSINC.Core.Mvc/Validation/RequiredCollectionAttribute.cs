using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MMSINC.Validation
{
    // TODO someday as this is missing all functionality on the frontend.
    //   1. The unobtrusive validation attributes aren't being added to the rendered html.
    //      Which is weird, because this inherits from RequiredAttribute. I guess whatever
    //      thing MVC is using to do that doesn't see attributes that inherit from RequiredAttribute.
    //   2. We never implemented any client-side validation for the Min property.

    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredCollectionAttribute : RequiredAttribute
    {
        #region Fields

        private int _min = 1;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the minimum amount of objects that need to be in the collection to be considered valid.
        /// </summary>
        /// <remarks>
        /// 
        /// You can set this to 0, but why you'd want to is beyond me.
        /// 
        /// </remarks>
        public int Min
        {
            get { return _min; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Min value can not be less than zero");
                }

                _min = value;
            }
        }

        #endregion

        #region Public methods

        public override bool IsValid(object value)
        {
            // Let the base crap of RequiredAttribute deal with
            // checking for nulls.
            if (!base.IsValid(value))
            {
                return false;
            }

            // String is an enumerable char, we don't handle those.
            if (!(value is IEnumerable) || value is string)
            {
                throw new InvalidCastException("RequiredCollectionAttribute only works on IEnumerable properties.");
            }

            return ((IEnumerable)value).Cast<object>().Count() >= Min;
        }

        #endregion
    }
}
