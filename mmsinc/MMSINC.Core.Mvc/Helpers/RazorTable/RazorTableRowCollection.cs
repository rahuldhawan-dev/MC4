using System.Collections.Generic;
using System.IO;

// ReSharper disable once CheckNamespace
namespace MMSINC.Helpers
{
    public class RazorTableRowCollection<T> : RazorTable<T>
    {
        #region Constructors

        public RazorTableRowCollection(T model, object htmlAttributes) : base(new[] {model}, htmlAttributes)
        {
            IsSortable = true;
        }

        public RazorTableRowCollection(IEnumerable<T> model, object htmlAttributes) : base(model, htmlAttributes)
        {
            IsSortable = true;
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            using (var writer = new StringWriter())
            {
                foreach (var item in GetModelItems())
                {
                    writer.Write(RenderRow(item));
                }

                return writer.ToString();
            }
        }

        #endregion
    }
}
