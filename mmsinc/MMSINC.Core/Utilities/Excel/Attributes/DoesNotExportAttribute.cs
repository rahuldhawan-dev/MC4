using System;

// ReSharper disable CheckNamespace
namespace MMSINC.Utilities.Excel
    // ReSharper restore CheckNamespace
{
    /// <summary>
    /// Attribute that indicates a property can not be exported by an exporter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class DoesNotExportAttribute : ExcelExportAttributeBase { }
}
