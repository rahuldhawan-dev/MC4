using System;

namespace MMSINC.DataPages
{
    public interface IDataField
    {
        #region Methods

        [Obsolete("Use other overload. This is only here for backwards compatibility. ")]
        string FilterExpression();

        void FilterExpression(IFilterBuilder builder);

        #endregion
    }
}
