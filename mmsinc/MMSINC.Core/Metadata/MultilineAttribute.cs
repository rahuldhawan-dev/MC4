using System.ComponentModel.DataAnnotations;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Put this attribute on a model property that should be displayed or edited
    /// as a multiline text box.
    /// </summary>
    public class MultilineAttribute : DataTypeAttribute
    {
        public MultilineAttribute() : base(DataType.MultilineText) { }
    }
}
