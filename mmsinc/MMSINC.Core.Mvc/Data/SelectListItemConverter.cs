using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.TypeExtensions;
using MMSINC.Exceptions;

namespace MMSINC.Data
{
    /// <summary>
    /// Converts data objects to SelectListItems.
    /// </summary>
    public static class SelectListItemConverter
    {
        #region Private Methods
        
        private static object GetFieldOrPropertyValueByName(object obj, string fieldOrPropertyName)
        {
            var type = obj.GetType();

            // The dynamic query selector stuff is what gives us an anonymous object that only has fields rather than properties
            if (type.HasFieldNamed(fieldOrPropertyName))
            {
                return obj.GetFieldValueByName(fieldOrPropertyName);
            }

            if (type.HasPropertyNamed(fieldOrPropertyName))
            {
                return obj.GetPropertyValueByName(fieldOrPropertyName);
            }

            throw new PropertyNotFoundException(type, fieldOrPropertyName);
        }

        #endregion

        /// <summary>
        /// Converts an unknown IEnumerable to SelectListItems by using string property names.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="valueProperty">The name of the property that will be used for the SelectListItem.Value value.</param>
        /// <param name="textProperty">The name of the property taht will be used for the SelectListItem.Text value.</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> Convert(IEnumerable collection, string valueProperty, string textProperty)
        {
            var values = new List<SelectListItem>();

            foreach (var item in collection)
            {
                values.Add(new SelectListItem {
                    Value = GetFieldOrPropertyValueByName(item, valueProperty)?.ToString() ?? string.Empty,
                    Text = GetFieldOrPropertyValueByName(item, textProperty)?.ToString() ?? string.Empty
                });
            }

            return values;
        }

        /// <summary>
        /// Converts an IEnumerable to SelectListItems using the given functions for generating the Text/Value values.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="collection"></param>
        /// <param name="valueGetter">A function that will supply the value for the SelectListItem.Value value.</param>
        /// <param name="textGetter">A function that will supply the value for the SelectListItem.Text value.</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> Convert<TEntity>(IEnumerable<TEntity> collection, Func<TEntity, object> valueGetter,
            Func<TEntity, object> textGetter)
        {
            var values = new List<SelectListItem>();

            foreach (var item in collection)
            {
                values.Add(new SelectListItem {
                    Value = valueGetter(item)?.ToString() ?? string.Empty,
                    Text = textGetter(item)?.ToString() ?? string.Empty
                });
            }

            return values;
        }

        /// <summary>
        /// Converts an enum to a collection of SelectListItems.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<SelectListItem> ConvertFromEnumType(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException($"Type '{enumType.Name}' is not an enum.");
            }

            return (from object x in Enum.GetValues(enumType)
                    select new SelectListItem {
                        Value = ((int)x).ToString(),
                        Text = Enum.GetName(enumType, x)
                    }).ToList();
        }
    }
}
