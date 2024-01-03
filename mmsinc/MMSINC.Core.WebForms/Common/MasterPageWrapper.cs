using System.Web.UI;
using MMSINC.Interface;

namespace MMSINC.Common
{
    public class MasterPageWrapper : IMasterPage
    {
        #region Private Members

        private readonly MasterPage _masterPage;

        #endregion

        #region Constructors

        public MasterPageWrapper(MasterPage masterPage)
        {
            _masterPage = masterPage;
        }

        #endregion

        #region Exposed Methods

        public Control FindControl(string ctrl)
        {
            return _masterPage.FindControl(ctrl);
        }

        #endregion
    }
}
