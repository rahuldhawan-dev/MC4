using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using StructureMap.TypeRules;

namespace MMSINC.Testing.Utilities
{
    /// <summary>
    /// Tester for conversions types, useful for testing that an instance of
    /// <typeparamref name="TCastFrom"/> when converted (cast, mapped, wrapped, etc.) to
    /// <typeparamref name="TCastTo"/> will propagate any settable values.
    /// </summary>
    public class CastPropertyTester<TCastFrom, TCastTo>
        where TCastFrom : new()
        where TCastTo : class
    {
        #region Private Members

        private readonly IEnumerable<PropertyInfo> _destinationProperties;
        private readonly IEnumerable<PropertyInfo> _sourceProperties;
        private readonly Func<TCastFrom, TCastTo> _convertFn;

        private readonly ImmutableDictionary<Type, object> _sampleValueDictionary =
            new Dictionary<Type, object> {
                { typeof(int), 1 },
                { typeof(int?), 2 },
                { typeof(bool), true },
                { typeof(bool?), false },
                { typeof(string), "Foo" },
                { typeof(SearchString), new SearchString { Value = "Bar" } },
                { typeof(DateRange), new DateRange { Start = DateTime.Today } },
                { typeof(IntRange), new IntRange { Start = 3 } },
                { typeof(int[]), new[] { 4, 5 } }
            }.ToImmutableDictionary();

        #endregion

        #region Constructors

        /// <param name="convertFn">
        /// <see cref="Func{TCastFrom,TCastTo}"/> which will be used to cast an instance from
        /// <typeparamref name="TCastFrom"/> to <typeparamref name="TCastTo"/>.
        /// </param>
        public CastPropertyTester(Func<TCastFrom, TCastTo> convertFn)
        {
            _convertFn = convertFn;
            _sourceProperties = typeof(TCastFrom).GetSettableProperties();
            _destinationProperties = typeof(TCastTo)
               .GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        #endregion

        #region Private Methods

        private object SetValueOnSource(TCastFrom source, PropertyInfo property)
        {
            if (!_sampleValueDictionary.TryGetValue(property.PropertyType, out var value))
            {
                throw new NotImplementedException(
                    $"No sample value available for type '{property.PropertyType}'");
            }

            property.SetValue(source, value);
            return value;
        }

        private object GetValueFromDestination(TCastTo result, PropertyInfo sourceProperty)
        {
            var property = _destinationProperties.SingleOrDefault(p => p.Name == sourceProperty.Name);

            if (property == null)
            {
                throw new InvalidOperationException(
                    $"Type '{typeof(TCastTo)}' does not implement property named " +
                    $"'{sourceProperty.Name}'");
            }

            return property.GetValue(result);
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Assert that all settable properties on <typeparamref name="TCastFrom"/> will propagate to
        /// <typeparamref name="TCastTo"/> after conversion.
        /// </summary>
        /// <param name="skip">Names of properties to skip, if any.</param>
        public void AssertAllSettablePropertiesCanMap(params string[] skip)
        {
            foreach (var property in _sourceProperties.Where(p => !skip.Contains(p.Name)))
            {
                var source = new TCastFrom();
                var expected = SetValueOnSource(source, property);

                var result = _convertFn(source);

                Assert.IsNotNull(
                    result,
                    "Attempted cast is not valid, perhaps you're missing an operator?");

                var actual = GetValueFromDestination(result, property);

                Assert.AreEqual(
                    expected,
                    actual,
                    $"Set value on property '{property.Name}' did not propagate");
            }
        }

        #endregion
    }
}
