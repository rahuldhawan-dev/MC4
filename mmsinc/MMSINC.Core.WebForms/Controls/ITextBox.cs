using System;

namespace MMSINC.Controls
{
    public interface ITextBox : IControl
    {
        #region Properties

        string Text { get; set; }

        #endregion

        #region Methods

        int GetIntValue();
        int? TryGetIntValue();
        double GetDoubleValue();
        double? TryGetDoubleValue();
        DateTime GetDateTimeValue();
        DateTime? TryGetDateTimeValue();

        #endregion
    }
}
