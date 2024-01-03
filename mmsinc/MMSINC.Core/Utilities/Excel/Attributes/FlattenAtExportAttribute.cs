using System;
using MMSINC.ClassExtensions.StringExtensions;

// ReSharper disable CheckNamespace
namespace MMSINC.Utilities.Excel
    // ReSharper restore CheckNamespace
{
    /// <summary>
    /// Attribute for describing how a property should be flattened by an exporter. Use this
    /// if you have a nested complex object type as a property that has a value that's needed
    /// for a column.
    /// 
    /// If this is attached to a property, the property itself will not be exported, only the
    /// flattened values will.
    /// </summary>
    /// <remarks>
    /// 
    /// Example: You have a model(ParentModel) that has a property(ChildProperty)
    /// and ChildProperty is a type with a property(Name). 
    /// 
    /// To automatically map this, you would go:
    /// 
    /// public class ParentModel
    /// {
    ///     [FlattenAtExport("Name")]
    ///     public ChildModel ChildProperty { get; set; }
    /// }
    /// 
    /// This will tell the exporter to retrieve ChildProperty.Name.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class FlattenAtExportAttribute : ExcelExportAttributeBase
    {
        #region Properties

        /// <summary>
        /// Gets/sets the column name that should be used. If not set, the name of the property
        /// is used instead.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets/sets the path to the property needed to render a value for a column.
        /// </summary>
        public string PropertyPath { get; private set; }

        #endregion

        #region Constructor

        public FlattenAtExportAttribute(string propertyPath)
        {
            propertyPath.ThrowIfNullOrWhiteSpace("propertyPath");
            PropertyPath = propertyPath;
        }

        public FlattenAtExportAttribute(string propertyPath, string columnName) : this(propertyPath)
        {
            columnName.ThrowIfNullOrWhiteSpace("columnName");
            ColumnName = columnName;
        }

        #endregion
    }
}
