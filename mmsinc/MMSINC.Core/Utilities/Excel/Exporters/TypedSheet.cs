using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MMSINC.Metadata;

// ReSharper disable CheckNamespace
namespace MMSINC.Utilities.Excel
    // ReSharper restore CheckNamespace
{
    public class TypedSheet<TRow> : SheetBase<TRow>
    {
        #region Fields

        // ReSharper disable StaticFieldInGenericType
        private static readonly Type _rowType = typeof(TRow);
        // ReSharper restore StaticFieldInGenericType

        #endregion

        #region Constructors

        /// <param name="name">The worksheet name for this sheet.</param>
        /// <param name="items">The data items that will be in the worksheet.</param>
        /// <param name="propertiesToExport">The columns to be exported. If null, then all columns will be exported. This does not override the DoesNotExportAttribute.</param>
        /// <param name="header">Optional text placed at the top row of the exported worksheet.</param>
        public TypedSheet(string name, IEnumerable<TRow> items, IEnumerable<string> propertiesToExport,
            string header = null) : base(name)
        {
            // use ToList, not ToArray, so that we aren't fragmenting memory by creating a ton of 
            // large object heap objects. This matters because users like to export excel files with
            // thousands of rows.
            Items = items.ToList();
            Columns = GetAllExportablePropertiesForType(propertiesToExport);
            Header = header;
        }

        #endregion

        #region Private Methods

        private static IEnumerable<PropertyInfo> GetPropertiesForType()
        {
            //If T is anonymous, get the declaration order from the ctor params.
            //http://msmvps.com/blogs/jon_skeet/archive/2009/12/09/quot-magic-quot-null-argument-testing.aspx
            var isAnonType = _rowType.Namespace == null
                             && !_rowType.IsPublic
                             && _rowType.IsGenericType
                             && _rowType.Name.Contains("AnonymousType");

            return (isAnonType
                ? _rowType.GetConstructors()[0].GetParameters().Select(p => _rowType.GetProperty(p.Name))
                : _rowType.GetProperties());
        }

        /// <summary>
        /// Returns all properties that could be exportable on a type regardless of any 
        /// ExcelExportAttributeBase derived attributes attached. 
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<PropertyInfo> GetPropertiesWithExportableTypes()
        {
            var ienumerableType = typeof(IEnumerable);
            var stringType = typeof(string);
            // We need to ignore IEnumerable properties since there's no logical way
            // to actually render them in a single cell. 
            return GetPropertiesForType().Where(prop =>
                prop.PropertyType == stringType || !ienumerableType.IsAssignableFrom(prop.PropertyType));
        }

        private IEnumerable<TypedColumnInfo> GetAllExportablePropertiesForType(IEnumerable<string> propertiesToExport)
        {
            var exportableColumns = new List<TypedColumnInfo>();

            Func<string, bool> canExportForThisInstance = (propertyName) => {
                // At this time, this param is optional. If it's null, then 
                // we assume that there is no additional customized exporting
                // going on and that all properties should be exported.
                if (propertiesToExport == null)
                {
                    return true;
                }

                return propertiesToExport.Contains(propertyName);
            };

            foreach (var p in GetPropertiesWithExportableTypes())
            {
                var exportAttributes = p.GetCustomAttributes<ExcelExportAttributeBase>(true).ToArray();
                var doesNotExport = exportAttributes.OfType<DoesNotExportAttribute>().Any();

                if (doesNotExport || !p.CanRead || !canExportForThisInstance(p.Name))
                {
                    continue;
                }

                var flatteners = exportAttributes.OfType<FlattenAtExportAttribute>().ToArray();
                if (!flatteners.Any())
                {
                    exportableColumns.Add(new TypedColumnInfo(p));
                }
                else
                {
                    foreach (var fl in flatteners)
                    {
                        exportableColumns.Add(new TypedColumnInfo(p, fl));
                    }
                }
            }

            // I don't know that this check is useful anymore.
            if (!exportableColumns.Any())
            {
                throw new InvalidOperationException($"Type {_rowType} has no properties to export.");
            }

            var potentialDuplicateColumnNames = exportableColumns.Select(x => x.ColumnName)
                                                                 .GroupBy(x => x)
                                                                 .Where(x => x.Count() > 1);

            foreach (var dupe in potentialDuplicateColumnNames)
            {
                throw new InvalidOperationException(
                    $"Can not export the column '{dupe.Key}' because another column already has that name.");
            }

            return exportableColumns;
        }

        protected override IEnumerable<object> GetValues(TRow row)
        {
            var typedColumns = (IEnumerable<TypedColumnInfo>)Columns;
            return typedColumns.Select(p => p.GetValue(row));
        }

        #endregion

        #region Helper class

        protected class TypedColumnInfo : ColumnInfo
        {
            #region Fields

            private readonly PropertyInfo[] _orderedPropertyAccessors;

            #endregion

            #region Properties

            public Func<TRow, object> GetValue { get; private set; }

            ///<summary>Gets the type of the column.</summary>
            public Type DataType { get; protected set; }

            #endregion

            #region Constructors

            // TODO: Refactor the hell out of these two constructors so they can be one.

            public TypedColumnInfo(PropertyInfo rootProperty, FlattenAtExportAttribute flattener)
            {
                _orderedPropertyAccessors = GetOrderedPropertyAccessors(rootProperty, flattener.PropertyPath).ToArray();
                DataType = _orderedPropertyAccessors.Last().PropertyType;
                ColumnName = GetExpectedColumnName(_orderedPropertyAccessors.Last(), flattener);

                if (!DataType.IsValueType)
                {
                    DataType = typeof(string);
                }

                // TODO: This thing can probably replace what's in the other constructor too.

                GetValue = (row) => {
                    var rawValue = GetFlattenedValue(row);
                    if (DataType == typeof(string))
                    {
                        return Convert.ToString(rawValue);
                    }

                    return rawValue;
                };
            }

            public TypedColumnInfo(PropertyInfo prop)
            {
                ColumnName = GetExpectedColumnName(prop, null);
                DataType = prop.PropertyType;

                if (!prop.PropertyType.IsValueType)
                {
                    DataType = typeof(string);

                    var rawGetter =
                        (Func<TRow, object>)Delegate.CreateDelegate(typeof(Func<TRow, object>), prop.GetGetMethod());
                    Func<TRow, object> getValue = (row) => {
                        var rawValue = rawGetter(row);
                        return Convert.ToString(rawValue);
                    };

                    GetValue = getValue;
                }
                else
                {
                    //If the property returns a value type, we need to compile an expression that boxes it.
                    // ROSS NOTE: I dunno if any of this even matters?  prop.GetGetMethod() should be fine here too?
                    var param = Expression.Parameter(_rowType, "row");
                    GetValue = Expression.Lambda<Func<TRow, object>>(
                        Expression.Convert(
                            Expression.Property(param, prop),
                            typeof(object)
                        ),
                        param
                    ).Compile();
                }
            }

            #endregion

            #region Private Methods

            private static string GetExpectedColumnName(PropertyInfo property, FlattenAtExportAttribute flattener)
            {
                var colName = string.Empty;

                if (flattener != null && !string.IsNullOrWhiteSpace(flattener.ColumnName))
                {
                    colName = flattener.ColumnName;
                }
                else
                {
                    var dna = property.GetCustomAttributes<DisplayNameAttribute>(true).SingleOrDefault();
                    var va = property.GetCustomAttributes<ViewAttribute>(true).SingleOrDefault();
                    var columnOptions = property.GetCustomAttributes<ExcelExportColumnAttribute>(true)
                                                .SingleOrDefault();

                    colName = property.Name;

                    if (columnOptions == null || !columnOptions.UsePropertyName)
                    {
                        if (va != null && !string.IsNullOrWhiteSpace(va.DisplayName))
                        {
                            colName = va.DisplayName;
                        }
                        else if (dna != null)
                        {
                            colName = dna.DisplayName;
                        }
                    }
                }

                // Excel columns can only have a maximum length of 64 characters. Otherwise an exception is thrown.

                //if (colName.Length > 64)
                //{
                //    colName = colName.Substring(0, 64);
                //}
                return colName;
            }

            private object GetFlattenedValue(TRow row)
            {
                // We wanna return null if the child property is null, because
                // that's sort of expected. However we don't want to eat exceptions
                // if the grand child property's getter throws a null reference exception.
                // Or really any exception.
                //
                // I also think this could be refactored more for performance. GetValue
                // is the slowest way of getting the property value. If a delegate could
                // be created somehow, that'd own bones.
                object rawValue = row;
                foreach (var prop in _orderedPropertyAccessors)
                {
                    rawValue = prop.GetValue(rawValue, new object[] { });
                    if (rawValue == null)
                    {
                        break;
                    }
                }

                return rawValue;
            }

            private static List<PropertyInfo> GetOrderedPropertyAccessors(PropertyInfo rootProperty,
                string propertyPathFromRoot)
            {
                // TODO: Probably move this to ObjectExtensions or PropertyInfoExtensions or something.

                var orderedProps = new List<PropertyInfo> {rootProperty};
                var curPropInfo = rootProperty;
                foreach (var pName in propertyPathFromRoot.Split('.'))
                {
                    var nextProp = curPropInfo.PropertyType.GetProperty(pName);
                    if (nextProp != null)
                    {
                        orderedProps.Add(nextProp);
                        curPropInfo = nextProp;
                    }
                    else
                    {
                        throw ExceptionHelper.Format<InvalidOperationException>(
                            "Unable to find property '{0}' on type '{1}'. Full property path: {2}", pName,
                            curPropInfo.PropertyType.FullName, propertyPathFromRoot);
                    }
                }

                return orderedProps;
            }

            #endregion
        }

        #endregion
    }
}
