using System;

namespace MMSINC.Common
{
    public class VisibilityChangeEventArgs : EventArgs
    {
        #region Private Members

        private readonly bool _newVisibility, _oldVisibility;

        #endregion

        #region Constructors

        public VisibilityChangeEventArgs(bool visibility)
        {
            _newVisibility = visibility;
            _oldVisibility = !visibility;
        }

        #endregion

        #region Properties

        public bool NewVisibility
        {
            get { return _newVisibility; }
        }

        public bool OldVisibility
        {
            get { return _oldVisibility; }
        }

        #endregion
    }
}
