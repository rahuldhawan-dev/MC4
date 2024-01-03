using System;
using System.Web.UI.WebControls;

namespace MMSINC.Controls
{
    /// <summary>
    /// Microsoft sealed this guy. 
    /// </summary>
    public class MenuItem : WebControl, IMenuItem
    {
        #region Properties

        public string Value { get; set; }
        public string TextFormat { get; set; }

        #endregion

        #region Events

        public event EventHandler Click
        {
            add => OnClickHandler = value;
            remove
            {
                if (OnClickHandler != value)
                {
                    throw new InvalidOperationException(
                        "Attempting to remove an event handler which was not attached.");
                }
                OnClickHandler = null;
            }
        }

        public EventHandler OnClickHandler { get; set; }

        #endregion
    }
}
