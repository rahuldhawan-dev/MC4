using System;
using System.Collections.Generic;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MMSINC.Utilities
{
    /// <summary>
    /// Generic IEqualityComparer that uses a lambda. Useful for LINQ extensions that only have IEqualityComparer overloads.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// This is restricted to T as class because there'd probably never be a reason to use this with a value type. 
    /// Also this is setup for performance with references.
    /// </remarks>
    public class LambdaComparer<T> : IComparer<T>, IEqualityComparer<T> where T : class
    {
        #region Fields

        private readonly Func<T, T, bool> _equalityComparer;
        private readonly Func<T, IComparable> _comparer;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new LambdaComparer instance that uses the given Func to compare two 
        /// objects together. Only use this if you're using the comparer for equality.
        /// </summary>
        /// <param name="lambdaComparer"></param>
        public LambdaComparer(Func<T, T, bool> lambdaComparer)
        {
            lambdaComparer.ThrowIfNull("lambaComparer");
            _equalityComparer = lambdaComparer;
        }

        /// <summary>
        /// Creates a new LambdaComparer instance that uses the given to Func to get
        /// a comparable value from two objects and then compare them.
        /// </summary>
        /// <param name="lambdaComparer"></param>
        public LambdaComparer(Func<T, IComparable> lambdaComparer)
        {
            lambdaComparer.ThrowIfNull("lambdaComparer");
            _comparer = lambdaComparer;
            _equalityComparer = (left, right) => { return Equals(lambdaComparer(left), lambdaComparer(right)); };
        }

        #endregion

        #region IEqualityComparer implementation

        public bool Equals(T x, T y)
        {
            return _equalityComparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            // Most things that use an equality comparer first check
            // the GetHashCode call before calling Equals. This has
            // some performance benefit(especially if there's items
            // that are the same reference). 
            // 
            // We can bypass this by just returning 0 and force 
            // a call to Equals, but that'd be silly.
            // -Ross 5/2/2012

            // Nulls aren't allowed, but since there's nothing else going on
            // in this method, there's not much of a reason to do a null check
            // when calling anything on obj is gonna throw one for us anyway.
            return obj.GetHashCode();
        }

        #endregion;

        #region IComparer<T> implementation

        public int Compare(T x, T y)
        {
            var left = _comparer(x);
            var right = _comparer(y);
            return left.CompareTo(right);
        }

        #endregion
    }
}
