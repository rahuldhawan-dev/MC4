using System;
using System.Web.UI;
using AjaxControlToolkit;

namespace MMSINC.Controls
{
    public class MvpAsyncFileUpload : AsyncFileUpload, IAsyncFileUpload
    {
        #region Private Members

        private IPostedFileWrapper _iPostedFile;

        #endregion

        #region Properties

        public IPostedFileWrapper IPostedFile
        {
            get
            {
                if (_iPostedFile == null)
                    _iPostedFile = new PostedFileWrapper(PostedFile);
                return _iPostedFile;
            }
        }

        #endregion

        #region Implementation of IControl

        public TControl FindControl<TControl>(string id) where TControl : Control
        {
            throw new NotImplementedException();
        }

        public TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
