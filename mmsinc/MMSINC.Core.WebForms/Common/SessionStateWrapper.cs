using System.Web.SessionState;
using MMSINC.Interface;

namespace MMSINC.Common
{
    public class SessionStateWrapper : ISessionState
    {
        #region Private Members

        private readonly HttpSessionState _innerSession;

        #endregion

        #region Properties

        public object this[int val]
        {
            get { return _innerSession[val]; }
            set { _innerSession[val] = value; }
        }

        public object this[string val]
        {
            get { return _innerSession[val]; }
            set { _innerSession[val] = value; }
        }

        #endregion

        #region Exposed Methods

        public void Remove(string s)
        {
            _innerSession.Remove(s);
        }

        #endregion

        #region Constructors

        public SessionStateWrapper(HttpSessionState session)
        {
            _innerSession = session;
        }

        #endregion
    }
}
