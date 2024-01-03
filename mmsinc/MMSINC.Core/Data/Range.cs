using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.ClassExtensions.DateTimeExtensions;
using NHibernate.Criterion;

namespace MMSINC.Data
{
    public abstract class Range
    {
        #region Consts

        public struct Errors
        {
            public const string DEFAULT_START_VALUE_REQUIRED = "Please enter a starting value.",
                                DEFAULT_START_LESS_THAN_END = "Starting value must be less than the ending value.";
        }

        #endregion
    }

    public class Range<T> : Range, IValidatableObject, ISearchCriterion where T : struct, IComparable<T>
    {
        #region Properties

        /// <summary>
        /// Beginning/minimum <typeparamref name="T"/> value of the value range to be searched, when
        /// <see cref="Operator"/> value is <see cref="RangeOperator.Between"/>.  Ignored otherwise.
        /// </summary>
        public virtual T? Start { get; set; }
        /// <summary>
        /// Ending/maximum <typeparamref name="T"/> value of the value range to be searched, when
        /// <see cref="Operator"/> value is <see cref="RangeOperator.Between"/>.  Otherwise, this will be
        /// the value checked for equality, greater than, or less than (again depending on the value of
        /// <see cref="Operator"/>).
        /// </summary>
        public virtual T? End { get; set; }
        /// <summary>
        /// Operator to use when performing comparisons.  <see cref="RangeOperator.Between"/> indicates a
        /// ternary search where the actual value is expected to fall between <see cref="Start"/> and
        /// <see cref="End"/>; all others are binary where the actual value will be compared, in terms of
        /// the specific <see cref="RangeOperator"/> chosen, against <see cref="End"/>.
        /// </summary>
        public RangeOperator Operator { get; set; }

        protected virtual string StartValueRequiredErrorMessage
        {
            get { return Errors.DEFAULT_START_VALUE_REQUIRED; }
        }

        protected virtual string StartMustBeLessThanEndErrorMessage
        {
            get { return Errors.DEFAULT_START_LESS_THAN_END; }
        }

        #endregion

        #region Public Methods

        public virtual ICriterion GetCriterion(ICriterion criterion, string propertyName)
        {
            if (End == null)
            {
                return criterion;
            }

            switch (Operator)
            {
                case RangeOperator.Between:
                    return Restrictions.Between(propertyName, Start.Value, End.Value);
                case RangeOperator.Equal:
                    return Restrictions.Eq(propertyName, End.Value);
                case RangeOperator.GreaterThan:
                    return Restrictions.Gt(propertyName, End.Value);
                case RangeOperator.GreaterThanOrEqualTo:
                    return Restrictions.Ge(propertyName, End.Value);
                case RangeOperator.LessThan:
                    return Restrictions.Lt(propertyName, End.Value);
                case RangeOperator.LessThanOrEqualTo:
                    return Restrictions.Le(propertyName, End.Value);
                default:
                    return criterion;
            }
        }

        public virtual ICriterion GetRestriction(string propertyName)
        {
            switch (Operator)
            {
                case RangeOperator.Between:
                    return Restrictions.Between(propertyName, Start.Value, End.Value);
                case RangeOperator.Equal:
                    return Restrictions.Eq(propertyName, End.Value);
                case RangeOperator.GreaterThan:
                    return Restrictions.Gt(propertyName, End.Value);
                case RangeOperator.GreaterThanOrEqualTo:
                    return Restrictions.Ge(propertyName, End.Value);
                case RangeOperator.LessThan:
                    return Restrictions.Lt(propertyName, End.Value);
                case RangeOperator.LessThanOrEqualTo:
                    return Restrictions.Le(propertyName, End.Value);
                default:
                    throw new InvalidOperationException();
            }
        }

        public virtual ICriterion GetRestriction(IProjection property)
        {
            switch (Operator)
            {
                case RangeOperator.Between:
                    return Restrictions.Between(property, Start.Value, End.Value);
                case RangeOperator.Equal:
                    return Restrictions.Eq(property, End.Value);
                case RangeOperator.GreaterThan:
                    return Restrictions.Gt(property, End.Value);
                case RangeOperator.GreaterThanOrEqualTo:
                    return Restrictions.Ge(property, End.Value);
                case RangeOperator.LessThan:
                    return Restrictions.Lt(property, End.Value);
                case RangeOperator.LessThanOrEqualTo:
                    return Restrictions.Le(property, End.Value);
                default:
                    throw new InvalidOperationException();
            }
        }

        public virtual bool IsValid
        {
            get
            {
                if (Operator == RangeOperator.Between)
                {
                    // A Between range is valid when no values are entered. It's supposed to be ignored.
                    if ((Start == null && End != null) || (Start != null && End == null))
                    {
                        return false;
                    }

                    if (Start != null && End != null)
                    {
                        // Return true if start value is less than or equal to end value.
                        return (Start.Value.CompareTo(End.Value) <= 0);
                    }
                }

                return true;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Operator == RangeOperator.Between && End.HasValue)
            {
                if (!Start.HasValue)
                {
                    yield return new ValidationResult(StartValueRequiredErrorMessage);
                }
                else if (Start.Value.CompareTo(End.Value) == 1)
                {
                    yield return new ValidationResult(StartMustBeLessThanEndErrorMessage);
                }
            }
        }

        #endregion
    }
}
