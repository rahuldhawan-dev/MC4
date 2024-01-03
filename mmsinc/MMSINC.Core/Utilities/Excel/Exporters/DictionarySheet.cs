using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace MMSINC.Utilities.Excel
{
    /// <summary>
    /// Class for creating excel sheets from a Dictionary.
    /// </summary>
    internal sealed class DictionarySheet : SheetBase<IDictionary<string, object>>
    {
        #region Constructors

        /// <param name="name">The worksheet name for this sheet.</param>
        /// <param name="items">The data items that will be in the worksheet.</param>
        /// <param name="propertiesToExport">The columns to be exported. If null, then all columns will be exported. This does not override the DoesNotExportAttribute.</param>
        /// <param name="header">Optional text placed at the top row of the exported worksheet.</param>
        public DictionarySheet(string name, IEnumerable<IDictionary<string, object>> items,
            IEnumerable<string> propertiesToExport, string header) : base(name)
        {
            Header = header;
            Items = items;
            var columns = new List<ColumnInfo>();
            if (items.Any())
            {
                var keys = items.SelectMany(x => x.Keys).Distinct();

                // This is gonna smell. ColumnInfo is necessary for determining the value type. Since we
                // don't have a main type or a PropertyInfo object to work from that means we'll need to 
                // cycle through every object until we find a non-null value to determine the value type.
                foreach (var key in keys)
                {
                    if (propertiesToExport == null || propertiesToExport.Contains(key))
                    {
                        columns.Add(new ColumnInfo(key));
                    }
                }
            }

            Columns = columns;
        }

        #endregion

        #region Private Methods

        protected override IEnumerable<object> GetValues(IDictionary<string, object> row)
        {
            var valueDict = new Dictionary<string, object>();

            // GetValues assumes the IEnumerable that is returned has the same
            // number of values as there are columns. This doesn't work when 
            // dealing with dictionaries that may have different sets of keys/values.

            // First the columns need to be added to the Dictionary so that the
            // keys end up being in the same order. Gross.
            foreach (var c in Columns)
            {
                valueDict[c.ColumnName] = null;
            }

            // Then copy the existing values in.
            foreach (var kv in row)
            {
                valueDict[kv.Key] = kv.Value;
            }

            return valueDict.Values;
        }

        #endregion
    }
}
