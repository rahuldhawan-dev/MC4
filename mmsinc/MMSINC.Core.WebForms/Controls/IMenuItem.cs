using System;

namespace MMSINC.Controls
{
    /// <summary>
    /// Microsoft doesn't even have this one.
    /// </summary>
    public interface IMenuItem
    {
        #region Properties

        string Value { get; set; }
        string TextFormat { get; set; }

        #endregion

        #region Events

        //        EventHandler OnClick { get; set; }
        event EventHandler Click;
        EventHandler OnClickHandler { get; }

        #endregion
    }
}
